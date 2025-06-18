using DevExpress.CodeParser;
using DevExpress.CodeParser.VB;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraPrinting.Native;
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
        private System.Timers.Timer _timer = new System.Timers.Timer();
        bool _LoggedIn = false;
        public frmMain()
        {
            InitializeComponent();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = string.Format("{0} - v{1}.{2}.{3}", Application.ProductName, version.Major, version.Minor, version.Build);
            //Preview Doctypes
            repositoryItemLookUp_PreviewDocType.DataSource = Enum.GetValues(typeof(DocType));
            CheckSettings();
        }

        private async void CheckSettings()
        {
            try
            {
                using (AppDbContext db = new AppDbContext())
                {
                    db.Settings.Load();
                    List<Settings> s = new List<Settings>();
                    s = db.Settings.Local.ToBindingList().ToList();

                    var item_login = s.Where(x => x.Name == "AutoLogin").First();
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
                            }
                        }

                        if (_LoggedIn)
                            foreach (var item in s)
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
                                            _timer.Interval = 1000 * tmr;
                                            InitTimer();
                                            barToggleSwitchItem1.Checked = true;
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

        private void btnLogin_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmLogin frm = new frmLogin();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                btnLogin.Enabled = false;
                btnLogout.Enabled = true;
                _LoggedIn = true;
            }
        }

        private void btnLogout_ItemClick(object sender, ItemClickEventArgs e)
        {
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
                                string jsonDoc = await new FrappeAPI().GetAsString(string.Format("api/resource/{0}/", doctype), docName); //Full JSON for this document
                                //await
                                pa.PrintREPX(docName, jsonDoc, ps, true);
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
            _timer.Elapsed += OnTimerElapsed;
            //timer1.Interval = 1000; // in miliseconds
            _timer.Start();
        }
        private volatile bool _requestStop = false;
        AppDbContext dbC = new AppDbContext();

        [STAThread]
        private async void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Debug.Print(DateTime.Now.ToString("HH:mm:ss"));
            Log.Information("Timer at {Time}", DateTime.Now.ToString("HH:mm:ss"));

            Thread t = new Thread((ThreadStart)(() =>
            {
                Stop();

                PrintJobHelper printJobHelper = new PrintJobHelper(dbC);
                printJobHelper.RunPrintJobsAsync();

            }));
            // Run from a thread that joins the STA Thread
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            if (barToggleSwitchItem1.Checked) Start();
        }
        private void Stop()
        {
            _requestStop = true;
            _timer.Stop();
        }

        private void Start()
        {
            _requestStop = false;
            _timer.Start();
        }

        private void barToggleSwitchItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (barToggleSwitchItem1.Checked)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }
        }
    }
}