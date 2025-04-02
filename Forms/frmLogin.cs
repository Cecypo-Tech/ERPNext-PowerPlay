using DevExpress.DataAccess.Json;
using DevExpress.Dialogs.Core.Filtering;
using DevExpress.Mvvm.Native;
using DevExpress.XtraEditors;
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

                txtURL.Text = "https://staging-cecypo-3.frappe.cloud";
                txtUSER.Text = "kushal@cecypo.com";
                txtPASS.Text = "Kushal@2024";
                if (txtURL.Text.Length == 0 || txtUSER.Text.Length == 0 || txtPASS.Text.Length == 0) return;
                txtURL.Text = txtURL.Text.Trim();
                txtUSER.Text = txtUSER.Text.Trim();
                btnLogin.Enabled = false;

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
                    var uri = new Uri(txtURL.Text + "/api/method/login");
                    var request = new HttpRequestMessage(HttpMethod.Post, uri);
                    request.Headers.Add("Accept", "application/json");
                    dynamic login = new System.Dynamic.ExpandoObject();
                    login.usr = txtUSER.Text;
                    login.pwd = txtPASS.Text;
                    var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    //response.EnsureSuccessStatusCode();
                    JsonNode data = JsonSerializer.Deserialize<JsonNode>(response.Content.ReadAsStringAsync().Result);
                    if (data != null)
                    {
                        Log.Information(data.ToString());
                        if (response.IsSuccessStatusCode)
                        {
                            var loggedin = data["message"].ToString();
                            if (loggedin.Contains("Logged In"))
                            {
                                Program.FrappeUser = txtUSER.Text;
                                Program.FrappeURL = txtURL.Text;
                                Program.Cookies.Add(cookieContainer.GetCookies(uri));
                                GetWarehouses();    //List warehouses
                                DialogResult = DialogResult.OK;
                                return;
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
        private async void GetWarehouses()
        {
            try
            {
                AppDbContext db = new AppDbContext();
                //Get Warehouse List & save to DB
                FrappeAPI fapi = new FrappeAPI();
                //Get warehouses once and save to DB. In login?
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/{1}?{2}", Program.FrappeURL, "/api/resource/Warehouse", "&filters=[[\"Warehouse\",\"is_group\",\"=\",\"0\"]]"));
                using (var handler = new HttpClientHandler() { CookieContainer = Program.Cookies })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(Program.FrappeURL) })
                {
                    HttpResponseMessage response_qr = await client.SendAsync(request);
                    response_qr.EnsureSuccessStatusCode();

                    string result = await response_qr.Content.ReadAsStringAsync();
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
    }
}