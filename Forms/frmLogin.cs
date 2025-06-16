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
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Program.FrappeUser = "";
                Program.FrappeURL = "";

                if (txtURL.Text.Length == 0 || txtUSER.Text.Length == 0 || txtPASS.Text.Length == 0) return;
                txtURL.Text = txtURL.Text.Trim();
                txtUSER.Text = txtUSER.Text.Trim();
                btnLogin.Enabled = false;

                bool LoginAttempt = await AttemptLogin(txtURL.Text, txtUSER.Text, txtPASS.Text);
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
                Log.Error(ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = true;
            }
        }

        public async Task<bool> AttemptLogin(string url,  string user, string pass)
        {
            // Create a CookieContainer instance
            var cookieContainer = new CookieContainer();

            // Create an HttpClientHandler and assign the CookieContainer to it
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true, // Ensure that the handler uses the CookieContainer
            };

            // Create an HttpClient instance with the HttpClientHandler
            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url + "/api/method/login");
                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Add("Accept", "application/json");
                dynamic login = new System.Dynamic.ExpandoObject();
                login.usr = user;
                login.pwd = pass;
                var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                JsonNode data = JsonSerializer.Deserialize<JsonNode>(response.Content.ReadAsStringAsync().Result);
                if (data != null)
                {
                    Log.Information(data.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        var loggedin = data["message"].ToString();
                        if (loggedin.Contains("Logged In"))
                        {
                            Program.FrappeUser = login.usr;
                            Program.FrappeURL = url;
                            Program.Cookies.Add(cookieContainer.GetCookies(uri));

                            Log.Information("Logged into " + url);

                            EnsureDBExists();
                            DialogResult = DialogResult.OK;
                            return true;
                        }
                        else
                        {
                            XtraMessageBox.Show(loggedin, "Login Failed - " + url, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(data["exception"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Log.Warning(response.ToString());
                    }
                }
                else
                {
                    Log.Warning("Login Failed. Please try again.");
                    XtraMessageBox.Show("Login Failed. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
        }

        private void EnsureDBExists()
        {
            using (AppDbContext db = new AppDbContext())
            {
                bool created = db.Database.EnsureCreated();
                if (created)
                {
                    Log.Information("Database Created!");
                }
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
                        spin_TimerValue.Value > 0 ? new Settings() { Name = "Timer", Enabled = true, Value = Convert.ToInt32(spin_TimerValue.Text) } : new Settings() { Name = "Timer", Enabled = false, Value = 0 },
                        //txtExportConnString.Text.Length > 0 ? new Settings() { Name = "MSSQLConnStr", Enabled = true, StringValue = txtExportConnString.Text.Trim() } : new Settings() { Name = "MSSQLConnStr", Enabled = false, Value = 0 },
                    ];

                    Settings _autologin = new Settings();
                    if (chkAutoLogin.Checked)
                    {
                        _autologin = new Settings() { Name = "AutoLogin", Enabled = true };
                        //Save credentials
                        db.Creds.ExecuteDelete();
                        db.Creds.Add(new Cred() { User = txtUSER.Text, Pass = txtPASS.Text, URL = txtURL.Text });
                        db.SaveChanges();
                    }
                    else
                    { 
                        _autologin = new Settings() { Name = "AutoLogin", Enabled = false };
                    }
                    s.Add(_autologin);

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
                    HttpResponseMessage response = await new FrappeAPI().GetAsReponse("/api/resource/User?fields=[\"name\", \"full_name\", \"location\"]", "&filters[[\"User\", \"Enabled\", \"=\", \"1\"]]&limit_page_length=100");
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