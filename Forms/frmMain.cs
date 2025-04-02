using DevExpress.XtraBars;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraTabbedMdi;
using ERPNext_PowerPlay.Forms;
using ERPNext_PowerPlay.Helpers;
using ERPNext_PowerPlay.Models;
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
        AppDbContext db = new AppDbContext();

        private static readonly ILogger _logger = Log.ForContext<frmMain>();
        bool _LoggedIn = false;
        public frmMain()
        {
            InitializeComponent();

            //Preview Doctypes
            repositoryItemLookUp_PreviewDocType.DataSource = Enum.GetValues(typeof(DocType));
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

        private async void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                byte[] byteData;
                PrintActions pa = new PrintActions();
                DocType d = (DocType)barEditItem_DoctypePreview.EditValue;
                string docName = (string)barEditItem_DocNamePreview.EditValue;
                foreach (PrinterSetting ps in db.PrinterSetting.Where(x => x.DocType == d))
                {
                    byteData = await pa.getFrappeDoc_AsBytes(docName, ps);
                    await Task.Run(() => pa.PrintDX(docName, byteData, ps, true));
                    await Task.Run(() => pa.PrintDX(docName, byteData, ps, true));
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error in Print Preview");
            }
        }
    }
}