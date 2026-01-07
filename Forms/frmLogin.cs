using DevExpress.DataAccess.Json;
using DevExpress.Dialogs.Core.Filtering;
using DevExpress.Mvvm.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraPivotGrid.Data;
using ERPNext_PowerPlay.Helpers;
using ERPNext_PowerPlay.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using JsonNode = System.Text.Json.Nodes.JsonNode;

namespace ERPNext_PowerPlay
{
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        public frmLogin()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                using (AppDbContext db = new AppDbContext())
                {
                    db.Creds.Load();
                    Cred cred = db.Creds.Local.ToBindingList().FirstOrDefault();
                    if (cred != null)
                    {   //Load saved credentials
                        txtURL.Text = cred.URL;
                        txtAPIKey.Text = cred.APIKey;
                        txtAPISecret.Text = cred.Secret;

                        db.Settings.Load();
                        List<Settings> s = new List<Settings>();
                        s = db.Settings.Local.ToBindingList().ToList();
                        chkAutoLogin.Checked = s.Where(x => x.Name == "AutoLogin").FirstOrDefault()?.Enabled ?? false;
                        chkAutoStartPrinting.Checked = s.Where(x => x.Name == "AutoStartPrinting").FirstOrDefault()?.Enabled ?? false;
                        chkLock.Checked = s.Where(x => x.Name == "Lock").FirstOrDefault()?.Enabled ?? false;
                        chkUseSocketIO.Checked = s.Where(x => x.Name == "UseSocketIO").FirstOrDefault()?.Enabled ?? true;
                        spin_TimerValue.Value = s.Where(x => x.Name == "Timer").FirstOrDefault()?.Value ?? 0;
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while loading settings");
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Program.FrappeUser = "";
                Program.FrappeURL = "";
                Program.ApiToken = "";

                if (txtURL.Text.Length == 0 || txtAPIKey.Text.Length == 0 || txtAPISecret.Text.Length == 0) return;
                txtURL.Text = txtURL.Text.Trim();
                txtAPIKey.Text = txtAPIKey.Text.Trim();
                txtAPISecret.Text = txtAPISecret.Text.Trim();
                btnLogin.Enabled = false;

                bool LoginAttempt = await AttemptLogin(txtURL.Text, txtAPIKey.Text, txtAPISecret.Text);
                if (LoginAttempt)
                {
                    //Only do this when manually logging in
                    SaveSettings();
                    GetWarehouses();    //List warehouses
                    GetUsers();    //List users
                }

                btnLogin.Enabled = true;
            }
            catch (Exception ex)
            {
                Log.Error("btnLogin error.");
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = true;
            }
        }

        public async Task<bool> AttemptLogin(string url, string apiKey, string apiSecret)
        {
            try
            {
                // For API Key/Secret authentication, we don't need to call a login endpoint
                // We simply construct the token and test it with a simple API call
                string token = $"{apiKey}:{apiSecret}";
                Program.ApiToken = token;
                Program.FrappeURL = url;
                Program.FrappeUser = apiKey; // Store API Key as user identifier

                // Test the API credentials by making a simple API call
                var testRequest = new HttpRequestMessage(HttpMethod.Get, $"{url}/api/method/frappe.auth.get_logged_user");
                testRequest.Headers.Add("Authorization", $"token {token}");

                using (var client = new HttpClient())
                {
                    var response = await client.SendAsync(testRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        Log.Information("Successfully authenticated to " + url);
                        EnsureDBExists();
                        DialogResult = DialogResult.OK;
                        return true;
                    }
                    else
                    {
                        string errorMsg = $"Authentication failed. Status: {response.StatusCode}";
                        string responseContent = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            try
                            {
                                JsonNode errorData = JsonSerializer.Deserialize<JsonNode>(responseContent);
                                if (errorData != null && errorData["exception"] != null)
                                {
                                    errorMsg = errorData["exception"].ToString();
                                }
                            }
                            catch
                            {
                                errorMsg += $"\n{responseContent}";
                            }
                        }

                        XtraMessageBox.Show(errorMsg, "Authentication Failed - " + url, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Log.Warning("Authentication Failed: " + errorMsg);

                        // Clear the credentials on failure
                        Program.ApiToken = "";
                        Program.FrappeURL = "";
                        Program.FrappeUser = "";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during authentication");
                XtraMessageBox.Show(ex.Message, "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Clear the credentials on error
                Program.ApiToken = "";
                Program.FrappeURL = "";
                Program.FrappeUser = "";
                return false;
            }
        }

        private void EnsureDBExists()
        {
            try
            {
                using (AppDbContext db = new AppDbContext())
                {
                    // Check if database exists but was created without migrations (EnsureCreated)
                    bool dbExists = File.Exists(db.DbPath);

                    if (dbExists)
                    {
                        // Create migrations history table and baseline existing migrations
                        db.Database.ExecuteSqlRaw(@"
                            CREATE TABLE IF NOT EXISTS ""__EFMigrationsHistory"" (
                                ""MigrationId"" TEXT NOT NULL PRIMARY KEY,
                                ""ProductVersion"" TEXT NOT NULL
                            )");

                        db.Database.ExecuteSqlRaw(@"
                            INSERT OR IGNORE INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
                            VALUES ('20250424124228_InitialCreate', '8.0.0')");

                        // Add any missing columns that may have been added after InitialCreate
                        // This handles databases created with EnsureCreated that have the columns already
                        AddColumnIfNotExists(db, "Settings", "StringValue", "TEXT NOT NULL DEFAULT ''");
                        AddColumnIfNotExists(db, "Settings", "Value", "INTEGER NOT NULL DEFAULT 0");
                        AddColumnIfNotExists(db, "JobHistory", "Set_Warehouse", "TEXT NOT NULL DEFAULT ''");

                        // Mark Set_Warehouse migration as applied since we handled columns manually
                        db.Database.ExecuteSqlRaw(@"
                            INSERT OR IGNORE INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
                            VALUES ('20260107124935_Set_Warehouse', '8.0.0')");

                        Log.Information("Database schema updated successfully");
                    }
                    else
                    {
                        // New database - run all migrations normally
                        db.Database.Migrate();
                        Log.Information("Database created with migrations");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while applying database migrations");
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddColumnIfNotExists(AppDbContext db, string table, string column, string type)
        {
            try
            {
                // Check if column exists
                var conn = db.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT COUNT(*) FROM pragma_table_info('{table}') WHERE name='{column}'";
                    var result = Convert.ToInt32(cmd.ExecuteScalar());

                    if (result == 0)
                    {
                        cmd.CommandText = $@"ALTER TABLE ""{table}"" ADD COLUMN ""{column}"" {type}";
                        cmd.ExecuteNonQuery();
                        Log.Information("Added column {0} to table {1}", column, table);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Could not add column {0} to {1} - may already exist", column, table);
            }
        }

        private async void SaveSettings()
        {
            try
            {
                using (AppDbContext db = new AppDbContext())
                {
                    List<Settings> s =
                    [
                        chkAutoStartPrinting.Checked ? new Settings() { Name = "AutoStartPrinting", Enabled = true } : new Settings() { Name = "AutoStartPrinting", Enabled = false },
                        chkLock.Checked ? new Settings() { Name = "Lock", Enabled = true } : new Settings() { Name = "Lock", Enabled = false },
                        chkUseSocketIO.Checked ? new Settings() { Name = "UseSocketIO", Enabled = true } : new Settings() { Name = "UseSocketIO", Enabled = false },
                        spin_TimerValue.Value > 0 ? new Settings() { Name = "Timer", Enabled = true, Value = Convert.ToInt32(spin_TimerValue.Text) } : new Settings() { Name = "Timer", Enabled = false, Value = 0 },
                        //txtExportConnString.Text.Length > 0 ? new Settings() { Name = "MSSQLConnStr", Enabled = true, StringValue = txtExportConnString.Text.Trim() } : new Settings() { Name = "MSSQLConnStr", Enabled = false, Value = 0 },
                    ];

                    //Save credentials
                    db.Creds.ExecuteDelete();
                    db.Creds.Add(new Cred() { APIKey = txtAPIKey.Text, Secret = txtAPISecret.Text, URL = txtURL.Text });
                    
                    Settings _autologin = new Settings();
                    if (chkAutoLogin.Checked)
                        _autologin = new Settings() { Name = "AutoLogin", Enabled = true };
                    else
                        _autologin = new Settings() { Name = "AutoLogin", Enabled = false };

                    s.Add(_autologin);

                    //Clear settings and save new.
                    db.Settings.ExecuteDelete();
                    foreach (var item in s)
                    {
                        db.Add<Settings>(item);
                    }

                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while saving settings");
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void GetWarehouses()
        {
            try
            {
                using (AppDbContext db = new AppDbContext())
                {
                    //Get Warehouse List & save to DB
                    HttpResponseMessage response = await new FrappeAPI().GetAsReponse("/api/resource/Warehouse", "?filters=[[\"Warehouse\",\"is_group\",\"=\",\"0\"]]");
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    if (result != null)
                    {
                        db.Warehouse.ExecuteDelete();
                        WarehouseRoot waredata = JsonSerializer.Deserialize<WarehouseRoot>(result.ToString());
                        //AppDbContext db = new AppDbContext();
                        foreach (var ware in waredata.data)
                            db.Add<Warehouse>(new Warehouse() { name = ware.name });

                        Log.Information("Updated Warehouse Count: {0}", waredata.data.Count());
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void GetUsers()
        {
            try
            {
                using (AppDbContext db = new AppDbContext())
                {
                    //Get Warehouse List & save to DB
                    HttpResponseMessage response = await new FrappeAPI().GetAsReponse("/api/resource/User?fields=[\"name\", \"full_name\", \"location\"]", "&filters[[\"User\", \"Enabled\", \"=\", \"1\"]]&limit_page_length=200");
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    if (result != null)
                    {
                        db.User.ExecuteDelete();
                        UserData.Root userdata = JsonSerializer.Deserialize<UserData.Root>(result.ToString());

                        foreach (var usr in userdata.data)
                            db.Add<User>(new User() { FullName = usr.full_name, Email = usr.name });

                        Log.Information("Updated User Count: {0}", userdata.data.Count());
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class UserData
    {
        public class Datum
        {
            public string name { get; set; }
            public string full_name { get; set; }
        }

        public class Root
        {
            public List<Datum> data { get; set; }
        }
    }
}