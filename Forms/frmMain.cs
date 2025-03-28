using DevExpress.XtraBars;
using DevExpress.XtraTabbedMdi;
using ERPNext_PowerPlay.Forms;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERPNext_PowerPlay
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private static readonly ILogger _logger = Log.ForContext<frmMain>();
        bool _LoggedIn = false;
        public frmMain()
        {
            InitializeComponent();

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

        private void btnShowLogs_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmLogs frm = new frmLogs();
            frm.MdiParent = this;
            frm.Show();
        }
    }
}