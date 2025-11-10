using DevExpress.CodeParser;
using DevExpress.CodeParser.VB;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.Design;
using DevExpress.XtraTabbedMdi;
using ERPNext_PowerPlay.Forms;
using ERPNext_PowerPlay.Helpers;
using ERPNext_PowerPlay.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERPNext_PowerPlay
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public List<Settings> _settings;
        private System.Timers.Timer _timer = new System.Timers.Timer();
        private SocketIOHelper _socketIOHelper;
        bool _LoggedIn = false;
        public frmMain()
        {
            InitializeComponent();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = string.Format("{0} - v{1}.{2}.{3}", Application.ProductName, version.Major, version.Minor, version.Build);
            //Preview Doctypes
            repositoryItemLookUp_PreviewDocType.DataSource = Enum.GetValues(typeof(DocType));
            CheckSettings();
            _timer.Elapsed += OnTimerElapsed;
        }

        private async void CheckSettings()
        {
            try
            {
                using (AppDbContext db = new AppDbContext())
                {
                    db.Settings.Load();
                    _settings = new List<Settings>();
                    _settings = db.Settings.Local.ToBindingList().ToList();

                    var item_login = _settings.Where(x => x.Name == "AutoLogin").First();
                    if (item_login.Enabled == true)
                    {
                        frmLogin frm = new frmLogin();
                        db.Creds.Load();
                        if (db.Creds.Local.ToBindingList().Count() == 0)
                        {
                            frm.ShowDialog();
                        }
                        else
                        {
                            Cred cred = db.Creds.Local.ToBindingList().FirstOrDefault();
                            bool LoginAttempt = await frm.AttemptLogin(cred.URL, cred.User, cred.Pass);
                            if (LoginAttempt)
                            {
                                btnLogin.Enabled = false;
                                btnLogout.Enabled = true;
                                _LoggedIn = true;
                                await InitializeSocketIO();
                            }
                        }

                        foreach (var item in _settings)
                        {
                            switch (item.Name)
                            {
                                case "AutoStartPrinting":

                                    break;
                                case "Lock":
                                    btnPrintSettings.Enabled = !item.Enabled;
                                    btnReportList.Enabled = !item.Enabled;
                                    break;
                                case "Timer":
                                    int tmr = item.Value;
                                    if (tmr > 0 && _LoggedIn)
                                    {
                                        if (tmr < 30) tmr = 30;
                                        _timer.Interval = 1000 * tmr;
                                        Log.Information("Timer: {0} seconds", tmr);
                                        if (_LoggedIn)
                                        {
                                            InitTimer();
                                            barToggleSwitchItem1.Checked = true;
                                            StartTimer();
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in CheckSettings");
            }
        }

        private async void btnLogin_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmLogin frm = new frmLogin();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                btnLogin.Enabled = false;
                btnLogout.Enabled = true;
                _LoggedIn = true;
                await InitializeSocketIO();
            }
        }

        private async void btnLogout_ItemClick(object sender, ItemClickEventArgs e)
        {
            StopTimer();
            await DisconnectSocketIO();
            _LoggedIn = false;
            btnLogin.Enabled = true;
            btnLogout.Enabled = false;
        }

        private void btnPrintSettings_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmPrintSettings frm = new frmPrintSettings();
            frm.MdiParent = this;
            frm.Show();
        }


        private async void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                byte[] byteData;
                PrintActions pa = new PrintActions();
                DocType d = (DocType)barEditItem_DoctypePreview.EditValue;
                string docName = (string)barEditItem_DocNamePreview.EditValue;
                using (var db = new AppDbContext())
                {
                    foreach (PrinterSetting ps in db.PrinterSetting.Where(x => x.Enabled && x.DocType == d))
                    {
                        string doctype = ps.DocType.GetAttributeOfType<DescriptionAttribute>().Description;
                        //Show FrappePDF (opens in Default PDF Viewer)
                        switch (ps.PrintEngine)
                        {
                            case PrintEngine.FrappePDF:
                                byteData = await pa.getFrappeDoc_AsBytes(docName, ps);
                                await Task.Run(() => pa.PrintDX(docName, byteData, ps, true));
                                break;
                            case PrintEngine.Ghostscript:
                                Serilog.Log.Information("Ghostscript does not have a preview method. Select FrappePDF or CustomTemplate.");
                                break;
                            case PrintEngine.SumatraPDF:
                                Serilog.Log.Information("SumatraPDF does not have a preview method. Select FrappePDF or CustomTemplate.");
                                break;
                            case PrintEngine.CustomTemplate:
                                try
                                {
                                    string jsonDoc = await new FrappeAPI().GetAsString(string.Format("api/resource/{0}/", doctype), docName); //Full JSON for this document
                                    //Log.Information("jsondoc: {0}", jsonDoc);
                                    pa.PrintREPX(docName, jsonDoc, ps, true);
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex, "Error in Custom Template Print Preview");
                                    MessageBox.Show("Error in Custom Template Print Preview: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                    break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Error in Print Preview");
            }
        }

        private void barButtonItem_JobHistory_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmData frm = new frmData();
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnReportList_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmReportList frm = new frmReportList();
            frm.MdiParent = this;
            frm.Show();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case FormWindowState.Maximized:
                    this.ShowInTaskbar = true;
                    break;
                case FormWindowState.Minimized:
                    this.ShowInTaskbar = false;
                    notifyIcon1.Visible = true;
                    notifyIcon1.BalloonTipText = "ERPNext PowerPlay Minimized";
                    notifyIcon1.ShowBalloonTip(500);
                    this.Hide();
                    break;
                case FormWindowState.Normal:
                    this.ShowInTaskbar = true;
                    break;
                default:
                    break;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            notifyIcon1.Visible = false;
            InitTimer();
        }

        public void InitTimer()
        {
            StopTimer();
            StartTimer();
            
        }
        private volatile bool _requestStop = false;
        AppDbContext dbC = new AppDbContext();

        [STAThread]
        private async void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Log.Information("Job check at {Time}", DateTime.Now.ToString("HH:mm:ss"));

            Thread t = new Thread((ThreadStart)(() =>
            {
              
                PrintJobHelper printJobHelper = new PrintJobHelper(dbC);
                printJobHelper.RunPrintJobsAsync();

            }));
            // Run from a thread that joins the STA Thread
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
        private void StopTimer()
        {
            _requestStop = true;
            _timer.Stop();
        }

        private void StartTimer()
        {
            _timer.Interval = _settings.Where(x => x.Name == "Timer").FirstOrDefault().Value * 1000; // Convert seconds to milliseconds
            _requestStop = false;
            _timer.Start();
        }

        private void barToggleSwitchItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (barToggleSwitchItem1.Checked)
            {
                Log.Information("Starting Timer");
                StartTimer();
            }
            else
            {
                Log.Information("Stopping Timer");
                StopTimer();
            }
        }

        /// <summary>
        /// Initialize and connect to Frappe's socket.io server for real-time Sales Invoice events
        /// </summary>
        private async Task InitializeSocketIO()
        {
            try
            {
                if (_socketIOHelper != null && _socketIOHelper.IsConnected)
                {
                    Log.Information("Socket.IO already connected");
                    return;
                }

                Log.Information("Initializing Socket.IO connection for real-time events");
                _socketIOHelper = new SocketIOHelper(dbC);
                bool connected = await _socketIOHelper.ConnectToSocketIO();

                if (connected)
                {
                    Log.Information("Socket.IO connection established successfully");
                }
                else
                {
                    Log.Warning("Failed to establish Socket.IO connection");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error initializing Socket.IO: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Disconnect from socket.io server
        /// </summary>
        private async Task DisconnectSocketIO()
        {
            try
            {
                if (_socketIOHelper != null)
                {
                    Log.Information("Disconnecting from Socket.IO");
                    await _socketIOHelper.Disconnect();
                    _socketIOHelper.Dispose();
                    _socketIOHelper = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error disconnecting from Socket.IO: {0}", ex.Message);
            }
        }

        private async void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopTimer();
            await DisconnectSocketIO();
        }
    }
}