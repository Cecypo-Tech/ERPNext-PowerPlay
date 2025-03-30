using DevExpress.DataAccess.Wizard.Model;
using DevExpress.Office.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraMap;
using DevExpress.XtraReports.Native.Templates;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSpreadsheet.Model;
using ERPNext_PowerPlay.Helpers;
using ERPNext_PowerPlay.Models;
using ERPNext_PowerPlay.Properties;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ERPNext_PowerPlay
{
    public partial class frmPrintSettings : DevExpress.XtraEditors.XtraForm
    {
        AppDbContext db = new AppDbContext();

        public frmPrintSettings()
        {
            InitializeComponent();

            db.PrinterSetting.Load();

            gc_PrintSettings.DataSource = db.PrinterSetting.Local.ToBindingList();
            gv_PrintSettings.PopulateColumns();
            gv_PrintSettings.Columns["FrappeTemplateName"].OptionsEditForm.StartNewRow = true;
            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gv_PrintSettings.Columns.Where(x => x.FieldName.StartsWith("Date") || x.FieldName == "ID"))
            {
                col.OptionsColumn.AllowEdit = false;
                col.VisibleIndex = -1;
            }

            //Warehouse Filter List
            db.Warehouse.Load();
            RepositoryItemCheckedComboBoxEdit repository_list = new RepositoryItemCheckedComboBoxEdit();
            List<Warehouse> warelist = db.Warehouse.Local.ToList();
            repository_list.DataSource = warelist;
            repository_list.ValueMember = "name";
            repository_list.DisplayMember = "name";

            gv_PrintSettings.Columns["WarehouseFilter"].ColumnEdit = repository_list;
            repository_list.EditValueChanged += repository_list_EditValueChanged;

            //List installed Printers
            AddPrinters();

            //REPX Search + Editor Buttons
            RepositoryItemButtonEdit repository_repx = new RepositoryItemButtonEdit();
            repository_repx.Buttons[0].Caption = "Select .REPX File";

            EditorButton editButton = new EditorButton(ButtonPredefines.Glyph);
            editButton.ImageOptions.Image = svgImageCollection1.GetImage("DrawSolid.svg");
            editButton.Caption = "Repx Layout Designer";

            repository_repx.Buttons.Add(editButton);
            repository_repx.ButtonClick += new ButtonPressedEventHandler(Repxbuttonclick);

            gv_PrintSettings.Columns["REPX_Template"].ColumnEdit = repository_repx;

            RepositoryItemMemoEdit memoFilter = new RepositoryItemMemoEdit();
            gv_PrintSettings.Columns["DocFilter"].ColumnEdit = memoFilter;
            gv_PrintSettings.Columns["DocFilter"].OptionsEditForm.ColumnSpan = 2;
            gv_PrintSettings.Columns["DocFilter"].OptionsEditForm.RowSpan  = 2;
            gv_PrintSettings.Columns["DocFilter"].OptionsEditForm.UseEditorColRowSpan = true;
        }

        private void Repxbuttonclick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit buttonedit = (ButtonEdit)sender;
            switch (e.Button.Index)
            {
                case 0:
                    OpenFileDialog fdlg = new OpenFileDialog();
                    fdlg.Title = "Select Report File";
                    fdlg.InitialDirectory = Application.StartupPath;
                    fdlg.Filter = "Report Files (*.repx)|*.repx";
                    fdlg.FilterIndex = 2;
                    fdlg.RestoreDirectory = true;
                    if (fdlg.ShowDialog() == DialogResult.OK)
                    { buttonedit.Text = fdlg.FileName; }
                    break;
                case 1:
                    DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.ReportLoadingRestrictionLevel = DevExpress.XtraReports.UI.RestrictionLevel.Enable;
                    XtraReport report = new XtraReport();
                    if (System.IO.File.Exists(buttonedit.Text))
                    {
                        // With Trusted deserialization
                        DevExpress.Utils.DeserializationSettings.InvokeTrusted(() => report.LoadLayout(buttonedit.Text));
                    }
                    //report.DataSource = new PdfProcessor.Document();
                    ReportDesignTool designTool = new ReportDesignTool(report);

                    // Invoke the Ribbon End-User Report Designer form  
                    // and load the report into it.
                    designTool.ShowRibbonDesigner();

                    // Invoke the Ribbon End-User Report Designer form modally 
                    // with the specified look and feel settings.
                    designTool.ShowRibbonDesigner();
                    break;
            }
            ;
        }

        private void AddPrinters()
        {
            DevExpress.XtraEditors.Repository.RepositoryItemComboBox cmbPrinters = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            gv_PrintSettings.Columns["Printer"].ColumnEdit = cmbPrinters;
            cmbPrinters.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                //template2s.Columns["ForwardPrinter"] cboPrinters.Properties.Items.Add(printer);
                cmbPrinters.Items.Add(printer);
            }

        }

        private void repository_list_EditValueChanged(object sender, EventArgs e)
        {
            var editor = sender as CheckedComboBoxEdit;
            var checkedItems = editor.Properties.GetCheckedItems() as List<Warehouse>;

            foreach (Warehouse item in editor.Properties.DataSource as List<Warehouse>)
            {
                if (checkedItems != null) item.selected = checkedItems.Contains(item);
            }

            gv_PrintSettings.RefreshData();

            gv_PrintSettings.UpdateCurrentRow();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                gv_PrintSettings.UpdateCurrentRow();
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message + System.Environment.NewLine + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void gc_PrintSettings_Click(object sender, EventArgs e)
        {

        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
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
                    }
                    db.SaveChanges();

                    var p = new PrintActions();

                    Stopwatch clock = Stopwatch.StartNew();
                    
                   
                    foreach (ERPNext_PowerPlay.Models.PrinterSetting ps in db.PrinterSetting.ToList())
                    {
                        Frappe_DocList.FrappeDocList DocList = await fapi.GetDocs2Print(ps);
                        Log.Information("Collected Documents in: {0}s", clock.Elapsed.TotalSeconds.ToString());

                        //await p.Frappe_GetDoc("INV-00632", ps); //Tests a single doc.
                        foreach (Frappe_DocList.data fd in DocList.data)
                        {
                            bool processed = await p.PrintDoc(fd);//.Frappe_GetDoc(fd.name, ps);
                            if (processed)
                            {
                                await fapi.UpdateCount("/api/resource/Sales Invoice", fd);
                            }
                        }
                        Log.Information("Princed {0} Documents in: {1}s", DocList.data.Count(), clock.Elapsed.TotalSeconds.ToString());

                    }
                }

                //request.Headers.Add("Accept", "application/json");
                //request.Headers.co
                ////request.Headers.Add("Authorization", "token " + string.Join(":", Program.AppUser));
                //HttpResponseMessage response_qr = await client.SendAsync(request);
                //response_qr.EnsureSuccessStatusCode();
                //Log.Information("[API] Fetch Warehouse List successful!");
                //string result = await response_qr.Content.ReadAsStringAsync();
                //if (result != null)
                //{
                //    List<Warehouse> waredata = JsonSerializer.Deserialize<List<Warehouse>>(result.ToString());
                //    AppDbContext db = new AppDbContext();
                //    db.Warehouse.AddRange(waredata);
                //}


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message + System.Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}