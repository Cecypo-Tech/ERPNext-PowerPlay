using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.Native.Web;
using DevExpress.Entity.Model.Metadata;
using DevExpress.Images;
using DevExpress.Utils.Extensions;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraTab;
using DevExpress.XtraVerticalGrid;
using ERPNext_PowerPlay.Helpers;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using System.Xml.Linq;
using GridView = DevExpress.XtraGrid.Views.Grid.GridView;
using Image = System.Drawing.Image;

namespace ERPNext_PowerPlay.Forms
{
    public partial class frmData : DevExpress.XtraEditors.XtraForm
    {
        AppDbContext db = new AppDbContext();
        public frmData()
        {
            InitializeComponent();
            xtraTabControl1.TabPages.Clear();
            dtFrom.DateTime = DateTime.Now;
            dtTo.DateTime = DateTime.Now;
            LoadData();

            db.ReportList.Load();
            comboBoxEdit1.Properties.Items.Add("~Print History");
            foreach (var item in db.ReportList.Local.ToBindingList())
            {
                comboBoxEdit1.Properties.Items.Add(item.ReportName);
            }
            comboBoxEdit1.SelectedIndex = 0;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        public async void LoadData()
        {
            try
            {
                if (comboBoxEdit1.SelectedIndex < 0) return;
                if (comboBoxEdit1.Text == "~Print History")
                {
                    LoadJobHistory(comboBoxEdit1.Text);
                }
                else
                {
                    var rpt = db.ReportList.Local.ToBindingList().FirstOrDefault(x => x.ReportName == comboBoxEdit1.Text);
                    FrappeAPI fapi = new FrappeAPI();
                    string json;
                    JsonElement docdata;
                    List<QueryReportColumn> columnMeta = null;

                    // Check if this is a query_report endpoint
                    bool isQueryReport = rpt.EndPoint.Contains("api/method/frappe.desk.query_report.run", StringComparison.OrdinalIgnoreCase);

                    if (isQueryReport)
                    {
                        // For query_report endpoints, parse the FieldList JSON to extract report_name and filters
                        string fieldListJson = rpt.FieldList
                            .Replace("{from_date}", dtFrom.DateTime.Date.ToString("yyyy-MM-dd"))
                            .Replace("{to_date}", dtTo.DateTime.Date.ToString("yyyy-MM-dd"));

                        // Parse the JSON to get report_name and filters
                        JsonElement fieldListDoc = JsonSerializer.Deserialize<JsonElement>(fieldListJson);
                        string reportName = fieldListDoc.GetProperty("report_name").GetString() ?? rpt.ReportName;
                        string filtersJson = "{}";
                        if (fieldListDoc.TryGetProperty("filters", out JsonElement filtersElement))
                        {
                            filtersJson = filtersElement.GetRawText();
                        }

                        json = await fapi.GetQueryReport(rpt.EndPoint, reportName, filtersJson);

                        JsonElement doc = JsonSerializer.Deserialize<JsonElement>(json);
                        JsonElement message = JsonHelper.GetJsonElement(doc, "message");
                        docdata = JsonHelper.GetJsonElement(message, "result");

                        // Extract column metadata
                        if (message.TryGetProperty("columns", out JsonElement columnsElement))
                        {
                            columnMeta = JsonSerializer.Deserialize<List<QueryReportColumn>>(columnsElement.GetRawText());
                        }
                    }
                    else
                    {
                        // Standard resource API
                        string query = string.Format("?fields={0}", rpt.FieldList);
                        if (!string.IsNullOrEmpty(rpt.FilterList))
                        {
                            string filter = rpt.FilterList
                                .Replace("{from_date}", dtFrom.DateTime.Date.ToString("yyyy/MM/dd"))
                                .Replace("{to_date}", dtTo.DateTime.Date.ToString("yyyy/MM/dd"));
                            query = string.Format("?fields={0}&filters=[{1}]", rpt.FieldList, filter);
                        }
                        query += "&limit_page_length=9999";
                        json = await fapi.GetAsString(rpt.EndPoint, query);

                        JsonElement doc = JsonSerializer.Deserialize<JsonElement>(json);
                        docdata = JsonHelper.GetJsonElement(doc, "data");
                    }

                    if (!toggleSwitch1.IsOn)    //Grid
                    {
                        var dataSource = CreateDataSourceFromText(docdata.ToString());
                        if (dataSource != null)
                        {
                            GridControl gc = CreateGridControl(dataSource, "GridView", rpt.ReportName);
                            gc.DataSource = dataSource;

                            GridHelper gridHelper = new GridHelper(this.Name);
                            gridHelper.Grid_AddExportOptions(gc, popupMenu1, barManager1);
                            gc.MainView.PopulateColumns();
                            currentGrid = gc;
                            currentView = (GridView)gc.MainView;

                            // Apply column metadata if available (query reports)
                            if (columnMeta != null)
                            {
                                ApplyColumnMetadata(currentView, columnMeta);
                            }

                            SetupGrid();
                            FormatNumericFields_Grid(currentView);
                            currentView.ViewCaption = string.Format("{0} - {1} to {2}", rpt.ReportName, dtFrom.DateTime.Date.ToString("yy/MM/dd"), dtTo.DateTime.Date.ToString("yy/MM/dd"));
                            // Load layout AFTER all setup is complete
                            XtraGrid_LoadLayout();
                        }
                    }
                    else                        //Pivot
                    {
                        var dataSource = CreateDataSourceFromText(docdata.ToString());
                        if (dataSource != null)
                        {
                            PivotGridControl pv = CreatePivotControl(dataSource, "", rpt.ReportName);
                            pv.DataSource = dataSource;

                            JsonDataSource js = dataSource;
                            for (int i = 0; i < js.Schema.Nodes.Count; i++)
                            {
                                string name = js.Schema.Nodes[i].Value.Name;
                                PivotGridField field = new PivotGridField(name, PivotArea.FilterArea);

                                // Apply caption from column metadata if available
                                if (columnMeta != null)
                                {
                                    var colMeta = columnMeta.FirstOrDefault(c => c.fieldname == name);
                                    if (colMeta != null)
                                    {
                                        field.Caption = colMeta.label ?? name;
                                        // Apply formatting based on fieldtype
                                        if (colMeta.fieldtype == "Currency" || colMeta.fieldtype == "Float")
                                        {
                                            field.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                            field.CellFormat.FormatString = "#,##0.00";
                                        }
                                        else if (colMeta.fieldtype == "Date")
                                        {
                                            field.CellFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                                            field.CellFormat.FormatString = "d";
                                        }
                                    }
                                    else
                                    {
                                        field.Caption = name;
                                    }
                                }
                                else
                                {
                                    field.Caption = name;
                                }

                                pv.Fields.Add(field);
                            }
                            pv.Tag = string.Format("{0} - {1} to {2}", rpt.ReportName, dtFrom.DateTime.Date.ToString("yy/MM/dd"), dtTo.DateTime.Date.ToString("yy/MM/dd"));
                            FormatNumericFields_Pivot(pv);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Column metadata from query_report response
        /// </summary>
        private class QueryReportColumn
        {
            public string label { get; set; }
            public string fieldname { get; set; }
            public string fieldtype { get; set; }
            public string options { get; set; }
            public string width { get; set; }
            public bool hidden { get; set; }
        }

        /// <summary>
        /// Apply column metadata from query_report to grid columns
        /// </summary>
        private void ApplyColumnMetadata(GridView gv, List<QueryReportColumn> columnMeta)
        {
            if (gv == null || columnMeta == null) return;

            foreach (GridColumn col in gv.Columns)
            {
                var meta = columnMeta.FirstOrDefault(c => c.fieldname == col.FieldName);
                if (meta != null)
                {
                    // Set caption/label
                    col.Caption = meta.label ?? col.FieldName;

                    // Hide column if specified
                    if (meta.hidden)
                    {
                        col.Visible = false;
                    }

                    // Apply formatting based on fieldtype
                    switch (meta.fieldtype?.ToLower())
                    {
                        case "currency":
                        case "float":
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            col.DisplayFormat.FormatString = "#,##0.00";
                            col.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            col.GroupFormat.FormatString = "#,##0.00";
                            // Add summary
                            if (col.Summary.Count == 0)
                            {
                                col.Summary.Add(DevExpress.Data.SummaryItemType.Sum);
                                col.Summary[col.Summary.Count - 1].DisplayFormat = "{0:#,##0.00}";
                            }
                            // Add group summary
                            var groupSummary = new GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, col.FieldName, col, "{0:#,##0.00}");
                            if (!gv.GroupSummary.Cast<GridGroupSummaryItem>().Any(gs => gs.FieldName == col.FieldName))
                            {
                                gv.GroupSummary.Add(groupSummary);
                            }
                            break;

                        case "int":
                        case "integer":
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            col.DisplayFormat.FormatString = "#,##0";
                            break;

                        case "date":
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            col.DisplayFormat.FormatString = "d"; // Date only
                            break;

                        case "datetime":
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            col.DisplayFormat.FormatString = "g"; // General date/time
                            break;

                        case "percent":
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            col.DisplayFormat.FormatString = "0.00%";
                            break;
                    }

                    // Set column width if specified
                    if (!string.IsNullOrEmpty(meta.width) && int.TryParse(meta.width, out int width))
                    {
                        col.Width = width;
                    }
                }
            }
        }

        private GridControl CreateGridControl(object dataSource, string viewType, string RptName)
        {
            XtraTabPage tabPage = new XtraTabPage();
            tabPage.Tag = RptName;
            tabPage.Text = string.Format("{0}", RptName);
            tabPage.Tooltip = RptName;
            tabPage.PageVisible = true;
            tabPage.Image = ImageResourceCache.Default.GetImage("office2013/grid/grid_16x16.png");

            GridControl gridControl = new GridControl();
            gridControl.DataSource = dataSource;
            gridControl.Dock = DockStyle.Fill;
            gridControl.Tag = RptName;
            gridControl.MainView = gridControl.CreateView("Grid");
            GridView view = new GridView(gridControl);
            gridControl.ViewCollection.Add(view);
            gridControl.MainView = view;

            view.Tag = RptName;
            view.ViewCaption = RptName;

            //SetupGrid();

            tabPage.AddControl(gridControl);
            xtraTabControl1.TabPages.Add(tabPage);
            xtraTabControl1.SelectedTabPage = tabPage;
            return gridControl;
        }

        private PivotGridControl CreatePivotControl(object dataSource, string viewType, string RptName)
        {
            XtraTabPage tabPage = new XtraTabPage();
            tabPage.Tag = RptName;
            tabPage.Text = string.Format("{0}", RptName);
            tabPage.Tooltip = RptName;
            tabPage.PageVisible = true;
            tabPage.Image = ImageResourceCache.Default.GetImage("office2013/grid/grid_16x16.png");

            var gridControl = new PivotGridControl();
            gridControl.DataSource = dataSource;
            gridControl.Dock = DockStyle.Fill;
            gridControl.Tag = RptName;

            gridControl.OptionsLayout.StoreAllOptions = true;
            gridControl.OptionsLayout.StoreAppearance = true;
            gridControl.OptionsLayout.StoreDataSettings = true;
            gridControl.OptionsLayout.StoreFormatRules = true;
            gridControl.OptionsLayout.StoreVisualOptions = true;
            gridControl.OptionsLayout.Columns.StoreAllOptions = true;
            gridControl.OptionsLayout.Columns.StoreAppearance = true;
            gridControl.OptionsLayout.Columns.StoreLayout = true;
            gridControl.OptionsLayout.Columns.AddNewColumns = true;
            gridControl.OptionsLayout.Columns.RemoveOldColumns = false;

            // Add popup menu for layout options
            gridControl.PopupMenuShowing += PivotGrid_PopupMenuShowing;

            tabPage.AddControl(gridControl);
            xtraTabControl1.TabPages.Add(tabPage);
            xtraTabControl1.SelectedTabPage = tabPage;

            // Auto-load layout if exists
            currentPivot = gridControl;
            LoadLayout_Pivot(gridControl, RptName);

            return gridControl;
        }

        private void PivotGrid_PopupMenuShowing(object sender, DevExpress.XtraPivotGrid.PopupMenuShowingEventArgs e)
        {
            PivotGridControl pv = (PivotGridControl)sender;
            if (pv == null) return;

            currentPivot = pv;

            // Add print menu items
            DXMenuItem printPreview = new DXMenuItem("Print Preview", new EventHandler(PrintPreview_Pivot));
            printPreview.Tag = pv;
            e.Menu.Items.Add(printPreview);

            DXMenuItem printDirect = new DXMenuItem("Print", new EventHandler(PrintDirect_Pivot));
            printDirect.Tag = pv;
            e.Menu.Items.Add(printDirect);

            // Add layout menu items with separator
            DXMenuItem saveLayout = new DXMenuItem("Save Layout", new EventHandler(SaveLayout_Pivot));
            saveLayout.BeginGroup = true;
            saveLayout.Tag = pv;
            e.Menu.Items.Add(saveLayout);

            DXMenuItem resetLayout = new DXMenuItem("Reset Layout", new EventHandler(ResetLayout_Pivot));
            resetLayout.Tag = pv;
            e.Menu.Items.Add(resetLayout);
        }


        string[] headers_numbers = new string[] { "TOTAL", "AMOUNT", "GROSS", "TAX", "VAT", "PAID", "VALUE" };
        string[] headers_dates = new string[] { "DATE", "TIME" };

        public async void SetupGrid()
        {
            GridView gv = (GridView)currentGrid.MainView;
            try
            {
                gv.BeginUpdate();
                gv.PopupMenuShowing += gv_Data_PopupMenuShowing;
                gv.OptionsSelection.MultiSelect = true;
                gv.OptionsMenu.ShowConditionalFormattingItem = true;
                gv.OptionsMenu.EnableColumnMenu = true;
                gv.OptionsMenu.EnableFooterMenu = true;
                gv.OptionsMenu.EnableGroupPanelMenu = true;
                gv.OptionsMenu.ShowSplitItem = true;
                gv.OptionsBehavior.Editable = false;
                gv.OptionsBehavior.AutoExpandAllGroups = true;
                gv.OptionsBehavior.AutoPopulateColumns = true;
                gv.OptionsBehavior.AutoUpdateTotalSummary = true;
                gv.OptionsView.AnimationType = DevExpress.XtraGrid.Views.Base.GridAnimationType.AnimateAllContent;
                gv.OptionsView.ShowFooter = true;
                gv.OptionsView.ShowViewCaption = true;
                gv.AppearancePrint.FooterPanel.BackColor = Color.Transparent;
                gv.AppearancePrint.HeaderPanel.BackColor = Color.Transparent;
                gv.AppearancePrint.HeaderPanel.FontStyleDelta = FontStyle.Bold;
                gv.OptionsPrint.PrintHorzLines = true;
                gv.OptionsPrint.PrintVertLines = false;
                gv.OptionsPrint.ExpandAllDetails = true;
                gv.OptionsPrint.ExpandAllGroups = true;
                gv.OptionsPrint.AutoWidth = false;
                gv.OptionsPrint.UsePrintStyles = true;
                // Reduce print row height to fit more rows per page
                gv.AppearancePrint.Row.Font = new Font(gv.AppearancePrint.Row.Font.FontFamily, 8f);
                gv.AppearancePrint.Row.Options.UseFont = true;
                gv.OptionsLayout.StoreAppearance = true;
                gv.OptionsLayout.StoreAllOptions = true;
                gv.OptionsLayout.StoreVisualOptions = true;
                gv.OptionsLayout.StoreDataSettings = true;
                gv.OptionsView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;

                if (gv.Columns.Count > 0)
                {
                    foreach (GridColumn gridColumns in gv.Columns)
                    {
                        //Format columns
                        foreach (string str in headers_numbers)
                        {
                            if (gridColumns.FieldName.ToUpper().Contains(str.ToUpper()))
                            {
                                gridColumns.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                gridColumns.DisplayFormat.FormatString = "n2";
                                gridColumns.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                gridColumns.GroupFormat.FormatString = "n2";
                                // Add footer summary
                                if (gridColumns.Summary.Count == 0)
                                {
                                    gridColumns.Summary.Add(DevExpress.Data.SummaryItemType.Sum);
                                    gridColumns.Summary[gridColumns.Summary.Count - 1].DisplayFormat = "{0:n2}";
                                }
                                // Add group footer summary
                                var groupSummary = new GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, gridColumns.FieldName, gridColumns, "{0:n2}");
                                if (!gv.GroupSummary.Cast<GridGroupSummaryItem>().Any(gs => gs.FieldName == gridColumns.FieldName))
                                {
                                    gv.GroupSummary.Add(groupSummary);
                                }
                            }
                        }
                        foreach (string str in headers_dates)
                        {
                            if (gridColumns.FieldName.ToUpper().Contains(str.ToUpper()))
                            {
                                gridColumns.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                                gridColumns.DisplayFormat.FormatString = "d"; // Date only, no time
                            }
                        }
                    }
                    //Global Summary
                    if (gv.Columns.Count > 0)
                    {
                        if (gv.Columns[0].Summary.Count == 0)
                        {
                            gv.Columns[0].Summary.Add(DevExpress.Data.SummaryItemType.Count);
                            gv.Columns[0].Summary[0].DisplayFormat = "{0:n} rows";
                        }
                    }
                }
                gv.EndUpdate();

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gv.EndUpdate();
            }
        }

        #region Layout Management

        private string LayoutsFolder => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ERPNext_PowerPlay", "layouts");

        private string GetLayoutPath(string name, bool isPivot)
        {
            string prefix = isPivot ? "pivot_" : "grid_";
            return Path.Combine(LayoutsFolder, prefix + MakeValidFileName(name) + ".xml");
        }

        private void EnsureLayoutsFolderExists()
        {
            if (!Directory.Exists(LayoutsFolder))
                Directory.CreateDirectory(LayoutsFolder);
        }

        private void SaveLayout_Grid(object? sender, EventArgs e)
        {
            try
            {
                DXMenuItem item = sender as DXMenuItem;
                GridView gv = item.Tag as GridView;
                if (gv == null) return;
                if (gv.Tag == null)
                {
                    Log.Warning("Grid tag missing!");
                    return;
                }
                EnsureLayoutsFolderExists();
                string path = GetLayoutPath(gv.Tag.ToString(), false);
                gv.SaveLayoutToXml(path);
                Log.Information("Grid layout saved: {0}", path);
                XtraMessageBox.Show("Layout saved successfully", "Layout", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveLayout_Pivot(object? sender, EventArgs e)
        {
            try
            {
                DXMenuItem item = sender as DXMenuItem;
                PivotGridControl pv = item.Tag as PivotGridControl;
                if (pv == null) return;

                EnsureLayoutsFolderExists();
                string name = pv.Tag?.ToString() ?? "Pivot";
                string path = GetLayoutPath(name, true);
                pv.SaveLayoutToXml(path);
                Log.Information("Pivot layout saved: {0}", path);
                XtraMessageBox.Show("Layout saved successfully", "Layout", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLayout_Grid(GridView gv, string layoutName)
        {
            try
            {
                if (gv == null) return;
                string path = GetLayoutPath(layoutName, false);
                if (File.Exists(path))
                {
                    // Handle layout upgrade by setting options before restore
                    gv.BeginUpdate();
                    gv.OptionsLayout.Columns.AddNewColumns = true;
                    gv.OptionsLayout.Columns.RemoveOldColumns = false;
                    // Use default restore (not FullLayout) to preserve data bindings
                    gv.RestoreLayoutFromXml(path);
                    gv.EndUpdate();
                    // Refresh data to ensure cells display correctly
                    gv.RefreshData();
                    Log.Information("Grid layout loaded: {0}", path);
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Could not load grid layout, attempting upgrade: {0}", ex.Message);
                TryUpgradeLayout_Grid(gv, layoutName);
            }
        }

        private void LoadLayout_Pivot(PivotGridControl pv, string layoutName)
        {
            try
            {
                if (pv == null) return;
                string path = GetLayoutPath(layoutName, true);
                if (File.Exists(path))
                {
                    // Handle layout upgrade by setting options before restore
                    pv.OptionsLayout.Columns.AddNewColumns = true;
                    pv.OptionsLayout.Columns.RemoveOldColumns = false;
                    // Use default restore (not FullLayout) to preserve data bindings
                    pv.RestoreLayoutFromXml(path);
                    pv.RefreshData();
                    Log.Information("Pivot layout loaded: {0}", path);
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Could not load pivot layout, attempting upgrade: {0}", ex.Message);
                TryUpgradeLayout_Pivot(pv, layoutName);
            }
        }

        private void TryUpgradeLayout_Grid(GridView gv, string layoutName)
        {
            try
            {
                string path = GetLayoutPath(layoutName, false);
                if (!File.Exists(path)) return;

                // Backup old layout
                string backupPath = path + ".bak";
                File.Copy(path, backupPath, true);

                // Try loading with relaxed options
                gv.OptionsLayout.Columns.AddNewColumns = true;
                gv.OptionsLayout.Columns.RemoveOldColumns = true;
                gv.OptionsLayout.StoreAllOptions = false;

                try
                {
                    gv.RestoreLayoutFromXml(path);
                    // Save upgraded layout
                    gv.SaveLayoutToXml(path);
                    Log.Information("Grid layout upgraded successfully: {0}", path);
                }
                catch
                {
                    // Layout is too incompatible, delete it
                    File.Delete(path);
                    Log.Warning("Grid layout was incompatible and has been reset: {0}", path);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to upgrade grid layout");
            }
        }

        private void TryUpgradeLayout_Pivot(PivotGridControl pv, string layoutName)
        {
            try
            {
                string path = GetLayoutPath(layoutName, true);
                if (!File.Exists(path)) return;

                // Backup old layout
                string backupPath = path + ".bak";
                File.Copy(path, backupPath, true);

                // Try loading with relaxed options
                pv.OptionsLayout.Columns.AddNewColumns = true;
                pv.OptionsLayout.Columns.RemoveOldColumns = true;
                pv.OptionsLayout.StoreAllOptions = false;

                try
                {
                    pv.RestoreLayoutFromXml(path);
                    // Save upgraded layout
                    pv.SaveLayoutToXml(path);
                    Log.Information("Pivot layout upgraded successfully: {0}", path);
                }
                catch
                {
                    // Layout is too incompatible, delete it
                    File.Delete(path);
                    Log.Warning("Pivot layout was incompatible and has been reset: {0}", path);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to upgrade pivot layout");
            }
        }

        private void ResetLayout_Grid(object? sender, EventArgs e)
        {
            try
            {
                DXMenuItem item = sender as DXMenuItem;
                GridView gv = item.Tag as GridView;
                if (gv == null) return;
                if (gv.Tag == null) return;

                string path = GetLayoutPath(gv.Tag.ToString(), false);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    Log.Information("Grid layout deleted: {0}", path);
                }

                // Reset to default
                gv.PopulateColumns();
                XtraMessageBox.Show("Layout reset to default", "Layout", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetLayout_Pivot(object? sender, EventArgs e)
        {
            try
            {
                DXMenuItem item = sender as DXMenuItem;
                PivotGridControl pv = item.Tag as PivotGridControl;
                if (pv == null) return;

                string name = pv.Tag?.ToString() ?? "Pivot";
                string path = GetLayoutPath(name, true);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    Log.Information("Pivot layout deleted: {0}", path);
                }

                // Reset fields to filter area
                foreach (PivotGridField field in pv.Fields)
                    field.Area = PivotArea.FilterArea;

                XtraMessageBox.Show("Layout reset to default", "Layout", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Legacy method for compatibility
        private void XtraGrid_LoadLayout()
        {
            if (currentView != null && currentView.Tag != null)
                LoadLayout_Grid(currentView, currentView.Tag.ToString());
        }

        #endregion

        #region Printing

        private void PrintPreview_Grid(object? sender, EventArgs e)
        {
            try
            {
                DXMenuItem item = sender as DXMenuItem;
                GridView gv = item?.Tag as GridView;
                if (gv == null) gv = currentView;
                if (gv == null) return;

                // Enable auto-width to fit columns within page width
                gv.OptionsPrint.AutoWidth = true;

                PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
                link.Component = gv.GridControl;

                // Set margins (left, right, top, bottom) - top increased by 5
                link.Margins = new System.Drawing.Printing.Margins(30, 25, 30, 30);

                // Header and footer
                PageHeaderFooter phf = link.PageHeaderFooter as PageHeaderFooter;
                phf.Header.Content.Clear();
                phf.Header.Content.AddRange(new string[] { "", gv.ViewCaption ?? "Grid Report", "" });
                phf.Header.LineAlignment = BrickAlignment.Center;
                phf.Footer.Content.Clear();
                phf.Footer.Content.AddRange(new string[] { "ERPNext PowerPlay", "", "Page [Page #]/[Pages #]" });
                phf.Footer.LineAlignment = BrickAlignment.Near;

                link.CreateDocument();
                link.ShowPreviewDialog();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDirect_Grid(object? sender, EventArgs e)
        {
            try
            {
                DXMenuItem item = sender as DXMenuItem;
                GridView gv = item?.Tag as GridView;
                if (gv == null) gv = currentView;
                if (gv == null) return;

                // Enable auto-width to fit columns within page width
                gv.OptionsPrint.AutoWidth = true;

                PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
                link.Component = gv.GridControl;

                // Set narrow margins
                link.Margins = new System.Drawing.Printing.Margins(25, 25, 25, 25);

                link.CreateDocument();
                link.Print();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintPreview_Pivot(object? sender, EventArgs e)
        {
            try
            {
                DXMenuItem item = sender as DXMenuItem;
                PivotGridControl pv = item?.Tag as PivotGridControl;
                if (pv == null) pv = currentPivot;
                if (pv == null) return;

                // Enable auto-width to fit columns within page width
                pv.OptionsPrint.PrintColumnHeaders = DevExpress.Utils.DefaultBoolean.True;
                pv.OptionsPrint.PrintRowHeaders = DevExpress.Utils.DefaultBoolean.True;

                PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
                link.Component = pv;

                // Set margins (left, right, top, bottom) - top increased by 5
                link.Margins = new System.Drawing.Printing.Margins(25, 25, 30, 25);

                // Header and footer
                PageHeaderFooter phf = link.PageHeaderFooter as PageHeaderFooter;
                phf.Header.Content.Clear();
                phf.Header.Content.AddRange(new string[] { "", pv.Tag?.ToString() ?? "Pivot Report", "" });
                phf.Header.LineAlignment = BrickAlignment.Center;
                phf.Footer.Content.Clear();
                phf.Footer.Content.AddRange(new string[] { "ERPNext PowerPlay", "", "Page [Page #]/[Pages #]" });
                phf.Footer.LineAlignment = BrickAlignment.Near;

                link.CreateDocument();
                link.ShowPreviewDialog();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDirect_Pivot(object? sender, EventArgs e)
        {
            try
            {
                DXMenuItem item = sender as DXMenuItem;
                PivotGridControl pv = item?.Tag as PivotGridControl;
                if (pv == null) pv = currentPivot;
                if (pv == null) return;

                PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
                link.Component = pv;

                // Set narrow margins
                link.Margins = new System.Drawing.Printing.Margins(25, 25, 25, 25);

                link.CreateDocument();
                link.Print();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Toolbar Actions

        private void barButtonPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!toggleSwitch1.IsOn && currentView != null)
                PrintPreview_Grid(null, EventArgs.Empty);
            else if (toggleSwitch1.IsOn && currentPivot != null)
                PrintPreview_Pivot(null, EventArgs.Empty);
        }

        private void barButtonPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!toggleSwitch1.IsOn && currentView != null)
                PrintDirect_Grid(null, EventArgs.Empty);
            else if (toggleSwitch1.IsOn && currentPivot != null)
                PrintDirect_Pivot(null, EventArgs.Empty);
        }

        private void barButtonExportXlsx_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExportCurrentGrid("Export to xlsx");
        }

        private void barButtonExportCsv_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExportCurrentGrid("Export to csv");
        }

        private void barButtonExportPdf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExportCurrentGrid("Export to pdf");
        }

        private void barButtonSaveLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!toggleSwitch1.IsOn && currentView != null)
            {
                var menuItem = new DXMenuItem { Tag = currentView };
                SaveLayout_Grid(menuItem, EventArgs.Empty);
            }
            else if (toggleSwitch1.IsOn && currentPivot != null)
            {
                var menuItem = new DXMenuItem { Tag = currentPivot };
                SaveLayout_Pivot(menuItem, EventArgs.Empty);
            }
        }

        private void barButtonResetLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!toggleSwitch1.IsOn && currentView != null)
            {
                var menuItem = new DXMenuItem { Tag = currentView };
                ResetLayout_Grid(menuItem, EventArgs.Empty);
            }
            else if (toggleSwitch1.IsOn && currentPivot != null)
            {
                var menuItem = new DXMenuItem { Tag = currentPivot };
                ResetLayout_Pivot(menuItem, EventArgs.Empty);
            }
        }

        private void ExportCurrentGrid(string exportType)
        {
            if (currentGrid == null) return;
            var menuItem = new DXMenuItem { Caption = exportType };
            ExportGrid(menuItem, EventArgs.Empty);
        }

        #endregion

        #region Numeric Formatting

        /// <summary>
        /// Formats all numeric columns in a GridView with #,##0.00 format
        /// </summary>
        private void FormatNumericFields_Grid(GridView gv)
        {
            if (gv == null) return;

            try
            {
                foreach (GridColumn col in gv.Columns)
                {
                    // Check if column type is numeric
                    if (col.ColumnType == typeof(double) ||
                        col.ColumnType == typeof(decimal) ||
                        col.ColumnType == typeof(float) ||
                        col.ColumnType == typeof(int) ||
                        col.ColumnType == typeof(long) ||
                        IsNumericFieldName(col.FieldName))
                    {
                        col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        col.DisplayFormat.FormatString = "#,##0.00";
                        col.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        col.GroupFormat.FormatString = "#,##0.00";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error formatting numeric fields in grid");
            }
        }

        /// <summary>
        /// Formats all numeric fields in a PivotGridControl with #,##0.00 format
        /// </summary>
        private void FormatNumericFields_Pivot(PivotGridControl pv)
        {
            if (pv == null) return;

            try
            {
                foreach (PivotGridField field in pv.Fields)
                {
                    // Check if field is likely numeric based on data type or name
                    if (field.DataType == typeof(double) ||
                        field.DataType == typeof(decimal) ||
                        field.DataType == typeof(float) ||
                        field.DataType == typeof(int) ||
                        field.DataType == typeof(long) ||
                        IsNumericFieldName(field.FieldName))
                    {
                        field.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        field.CellFormat.FormatString = "#,##0.00";
                        field.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        field.ValueFormat.FormatString = "#,##0.00";
                        field.GrandTotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        field.GrandTotalCellFormat.FormatString = "#,##0.00";
                        field.TotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        field.TotalCellFormat.FormatString = "#,##0.00";
                        field.TotalValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        field.TotalValueFormat.FormatString = "#,##0.00";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error formatting numeric fields in pivot");
            }
        }

        /// <summary>
        /// Checks if a field name suggests it contains numeric data
        /// </summary>
        private bool IsNumericFieldName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName)) return false;

            string upper = fieldName.ToUpper();
            string[] numericKeywords = {
                "TOTAL", "AMOUNT", "GROSS", "TAX", "VAT", "PAID", "VALUE",
                "PRICE", "COST", "RATE", "QTY", "QUANTITY", "SUM", "COUNT",
                "BALANCE", "CREDIT", "DEBIT", "DISCOUNT", "PERCENT", "PERCENTAGE",
                "WEIGHT", "VOLUME", "SIZE", "NUMBER", "NUM", "CHARGE", "FEE"
            };

            foreach (string keyword in numericKeywords)
            {
                if (upper.Contains(keyword))
                    return true;
            }

            return false;
        }

        #endregion

        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        public static JsonDataSource CreateDataSourceFromText(string json_string)
        {
            var jsonDataSource = new JsonDataSource();
            // Specify the object that retrieves JSON data.
            jsonDataSource.JsonSource = new CustomJsonSource(json_string);
            // Populate the data source with data.

            if (json_string.Length > 3)
            {
                jsonDataSource.Fill();
                return jsonDataSource;
            }
            return null;
        }

        private void LoadJobHistory(string RptName)
        {
            try
            {
                GridControl gc;
                GridView gv;
                using (var db = new AppDbContext())
                {
                    gc = CreateGridControl(db.JobHistory.Where(x => x.Date >= dtFrom.DateTime.Date && x.Date <= dtTo.DateTime.Date).ToList(), "GridView", RptName);
                    gv = (GridView)gc.MainView;
                    gv.PopulateColumns();
                }
                //Format columns
                if (gv.Columns.Count > 1)
                {
                    //Hide
                    gv.Columns["custom_print_count"].VisibleIndex = -1;
                    //Naming
                    gv.Columns["Grand_Total"].Caption = "Grand Total";
                    //Format
                    gv.Columns["Grand_Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gv.Columns["Grand_Total"].DisplayFormat.FormatString = "n2";
                    gv.Columns["Grand_Total"].GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gv.Columns["Grand_Total"].GroupFormat.FormatString = "n2";
                    gv.Columns["JobDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    gv.Columns["JobDate"].DisplayFormat.FormatString = "g";
                    //Summary
                    gv.Columns["Grand_Total"].Summary.Add(DevExpress.Data.SummaryItemType.Sum);
                    gv.Columns["Grand_Total"].Summary[0].DisplayFormat = "{0:n2}";
                    //Fit
                    gv.BestFitMaxRowCount = 100;
                    gv.BestFitColumns();
                    gv.OptionsBehavior.Editable = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gv_Data_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            GridView gv = (GridView)sender;
            GridControl grid = (sender as GridView).GridControl;
            if (gv == null) return;
            if (grid == null) return;

            currentGrid = grid;
            currentView = gv;

            if (e.MenuType == GridMenuType.Column)
            {
                DevExpress.XtraGrid.Menu.GridViewColumnMenu menu = e.Menu as GridViewColumnMenu;
                if (menu.Column != null)
                {
                    //Adding new items
                    menu.BeginGroup = true;
                    DXSubMenuItem fItem = new DXSubMenuItem("Fixed Columns");
                    fItem.Items.Add(CreateCheckItem("Fixed None", menu.Column, FixedStyle.None, imageCollection1.Images[0]));
                    fItem.Items.Add(CreateCheckItem("Fixed Left", menu.Column, FixedStyle.Left, imageCollection1.Images[1]));
                    fItem.Items.Add(CreateCheckItem("Fixed Right", menu.Column, FixedStyle.Right, imageCollection1.Images[2]));
                    menu.Items.Add(fItem);
                }
            }
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
            {
                DXPopupMenu menu2 = new DXPopupMenu();

                // Print submenu
                DXSubMenuItem printItem = new DXSubMenuItem("Print");
                DXMenuItem printPreview = new DXMenuItem("Print Preview", new EventHandler(PrintPreview_Grid));
                printPreview.Tag = gv;
                printItem.Items.Add(printPreview);
                DXMenuItem printDirect = new DXMenuItem("Print", new EventHandler(PrintDirect_Grid));
                printDirect.Tag = gv;
                printItem.Items.Add(printDirect);
                menu2.Items.Add(printItem);

                // Export submenu
                DXSubMenuItem sItem = new DXSubMenuItem("Export");
                sItem.Items.Add(new DXMenuItem("Export to xlsx", new EventHandler(ExportGrid), image: imageCollection1.Images[3]));
                sItem.Items.Add(new DXMenuItem("Export to csv", new EventHandler(ExportGrid), image: imageCollection1.Images[4]));
                sItem.Items.Add(new DXMenuItem("Export to pdf", new EventHandler(ExportGrid), image: imageCollection1.Images[5]));
                menu2.Items.Add(sItem);

                // Layout submenu
                DXSubMenuItem layoutItem = new DXSubMenuItem("Layout");
                DXMenuItem saveLayout = new DXMenuItem("Save Layout", new EventHandler(SaveLayout_Grid));
                saveLayout.Tag = gv;
                layoutItem.Items.Add(saveLayout);
                DXMenuItem resetLayout = new DXMenuItem("Reset Layout", new EventHandler(ResetLayout_Grid));
                resetLayout.Tag = gv;
                layoutItem.Items.Add(resetLayout);
                menu2.Items.Add(layoutItem);

                menu2.ShowPopup(grid, e.HitInfo.HitPoint);
            }
        }
        GridControl currentGrid;
        GridView currentView;
        PivotGridControl currentPivot;
        private void ExportGrid(object? sender, EventArgs e)
        {
            try
            {
                var exportItem = sender as DXMenuItem;
                if (currentGrid == null) return;
                if (currentGrid.DataSource == null) return;
                if (currentGrid.MainView.RowCount < 1) return;

                string path = GetTemp("xlsx");
                switch (exportItem.Caption)
                {
                    case "Print Preview":
                        path = "";

                        // Enable auto-width to fit columns within page width
                        currentView.OptionsPrint.AutoWidth = true;

                        PrintingSystem printingSystem1 = new PrintingSystem();
                        PrintableComponentLink link = new PrintableComponentLink();
                        printingSystem1.Links.AddRange(new object[] { link });
                        link.Component = currentGrid;

                        string hleftColumn = Application.ProductName;
                        string hmiddleColumn = "";
                        string hrightColumn = currentView.ViewCaption;

                        string fleftColumn = "Pages: [Page # of Pages #]";
                        string fmiddleColumn = "User: [User Name]";
                        string frightColumn = "Printed Date: [Date Printed]";

                        // Create a PageHeaderFooter
                        PageHeaderFooter phf = link.PageHeaderFooter as PageHeaderFooter;

                        // Clear the PageHeaderFooter's contents.
                        phf.Header.Content.Clear();

                        // Add custom information to the link's header.
                        phf.Header.Content.AddRange(new string[] { hleftColumn, hmiddleColumn, hrightColumn });
                        phf.Header.LineAlignment = BrickAlignment.Far;
                        phf.Footer.Content.AddRange(new string[] { fleftColumn, fmiddleColumn, frightColumn });
                        phf.Footer.LineAlignment = BrickAlignment.Near;
                        //End Header/FootPageHeaderFooter

                        //Set thinner margins
                        link.Margins.Top = 60; // Set top margin in points
                        link.Margins.Bottom = 60;
                        link.Margins.Left = 40;
                        link.Margins.Right = 40;

                        link.CreateDocument();
                        link.ShowRibbonPreview(LookAndFeel.ActiveLookAndFeel);

                        break;
                    case "Export to xlsx":
                        path = GetTemp("xlsx");
                        currentGrid.ExportToXlsx(path);
                        break;
                    case "Export to csv":
                        path = GetTemp("csv");
                        currentGrid.ExportToCsv(path);
                        break;
                    case "Export to pdf":
                        path = GetTemp("pdf");
                        currentGrid.ExportToPdf(path);
                        break;
                    default:
                        break;
                }

                if (path.Length > 0)
                    if (File.Exists(path))
                    {
                        //Open the file
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path) { UseShellExecute = true });
                    }
                    else
                    {
                        XtraMessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string GetTemp(string type)
        {
            switch (type)
            {
                case "xlsx":
                    return Path.Combine(System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".xlsx");
                case "csv":
                    return Path.Combine(System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".csv");
                case "pdf":
                    return Path.Combine(System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf");
                default:
                    return "";
            }
        }

        //Create a menu item
        DXMenuCheckItem CreateCheckItem(string caption, GridColumn column, FixedStyle style, Image image)
        {
            DXMenuCheckItem item = new DXMenuCheckItem(caption, column.Fixed == style,
              image, new EventHandler(OnFixedClick));
            item.Tag = new MenuInfo(column, style);
            return item;
        }

        //Menu item click handler
        void OnFixedClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            MenuInfo info = item.Tag as MenuInfo;
            if (info == null) return;
            info.Column.Fixed = info.Style;
        }

        //The class that stores menu specific information
        class MenuInfo
        {
            public MenuInfo(GridColumn column, FixedStyle style)
            {
                this.Column = column;
                this.Style = style;
            }
            public FixedStyle Style;
            public GridColumn Column;
        }

    }
}
