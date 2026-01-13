using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace ERPNext_PowerPlay.Forms
{
    public partial class frmLog : DevExpress.XtraEditors.XtraForm
    {
        public frmLog()
        {
            InitializeComponent();
        }

        // Prevent closing via X button or Alt+F4
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Only allow closing when the application is shutting down
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }
    }
}
