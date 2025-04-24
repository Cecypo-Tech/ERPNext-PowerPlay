using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERPNext_PowerPlay.Forms
{
    public partial class frmReportList : DevExpress.XtraEditors.XtraForm
    {
        AppDbContext db = new AppDbContext();
        public frmReportList()
        {
            InitializeComponent();

            db.ReportList.Load();
            gc_ReportList.DataSource = db.ReportList.Local.ToBindingList();
            gv_ReportList.PopulateColumns();
            gv_ReportList.OptionsEditForm.EditFormColumnCount = 2;

            RepositoryItemMemoEdit memoFilter1 = new RepositoryItemMemoEdit();
            gv_ReportList.Columns["FieldList"].ColumnEdit = memoFilter1;
            gv_ReportList.Columns["FieldList"].OptionsEditForm.ColumnSpan = 1;
            gv_ReportList.Columns["FieldList"].OptionsEditForm.RowSpan = 1;
            gv_ReportList.Columns["FieldList"].OptionsEditForm.UseEditorColRowSpan = true;
            RepositoryItemMemoEdit memoFilter2 = new RepositoryItemMemoEdit();
            gv_ReportList.Columns["FilterList"].ColumnEdit = memoFilter2;
            gv_ReportList.Columns["FilterList"].OptionsEditForm.ColumnSpan = 1;
            gv_ReportList.Columns["FilterList"].OptionsEditForm.RowSpan = 1;
            gv_ReportList.Columns["FilterList"].OptionsEditForm.UseEditorColRowSpan = true;

            gv_ReportList.Columns[colDateCreated.FieldName].OptionsColumn.AllowEdit = false;
            gv_ReportList.Columns[colDateCreated.FieldName].OptionsColumn.ReadOnly = true;
            gv_ReportList.Columns[colDateModified.FieldName].OptionsColumn.AllowEdit = false;
            gv_ReportList.Columns[colDateModified.FieldName].OptionsColumn.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                gv_ReportList.UpdateCurrentRow();
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message + System.Environment.NewLine + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gv_ReportList_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            //Set default values
            this.gv_ReportList.SetRowCellValue(e.RowHandle, "Enabled", true);
        }
    }
}