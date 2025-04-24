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
using DevExpress.XtraTab;
using DevExpress.XtraVerticalGrid;
using ERPNext_PowerPlay.Helpers;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
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
            dtFrom.DateTime = DateTime.Now.AddDays(-1);
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
                    string query = string.Format("?fields={0}", rpt.FieldList);
                    if (!string.IsNullOrEmpty(rpt.FilterList))
                    {
                        string filter = rpt.FilterList
                            .Replace("{from_date}", dtFrom.DateTime.Date.ToString("yyyy/MM/dd"))
                            .Replace("{to_date}", dtTo.DateTime.Date.ToString("yyyy/MM/dd"));
                        query = string.Format("?fields={0}&filters={1}", rpt.FieldList, filter);
                    }
                    string json = await fapi.GetAsString(rpt.EndPoint, query);

                    JsonElement doc = JsonSerializer.Deserialize<JsonElement>(json);
                    JsonElement docdata = JsonHelper.GetJsonElement(doc, "data");

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
                            SetupGrid();
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
                                field.Caption = name;

                                pv.Fields.Add(field);
                            }
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

        private GridControl CreateGridControl(object dataSource, string viewType, string RptName)
        {
            XtraTabPage tabPage = new XtraTabPage();
            tabPage.Tag = RptName;
            tabPage.Text = string.Format("{0}", RptName);
            tabPage.Tooltip = RptName;
            tabPage.PageVisible = true;
            tabPage.Image = ImageResourceCache.Default.GetImage("office2013/grid/grid_16x16.png");

            GridControl gridControl = new GridControl();
            
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
            gridControl.OptionsLayout.Columns.RemoveOldColumns = true;

            tabPage.AddControl(gridControl);
            xtraTabControl1.TabPages.Add(tabPage);
            xtraTabControl1.SelectedTabPage = tabPage;
            return gridControl;
        }


        string[] headers_numbers = new string[] { "TOTAL", "AMOUNT", "GROSS", "TAX", "VAT", "PAID", "VALUE" };
        string[] headers_dates = new string[] { "DATE", "TIME" };

        public async void SetupGrid()
        {
            GridView gv = (GridView)currentGrid.MainView;
            gv.PopulateColumns();
            try
            {
                
                gv.BeginUpdate();
                gv.PopupMenuShowing += gv_Data_PopupMenuShowing;
                XtraGrid_LoadLayout();
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
                                if (gridColumns.Summary.Count == 0)
                                {
                                    gridColumns.Summary.Add(DevExpress.Data.SummaryItemType.Sum);
                                    gridColumns.Summary[gridColumns.Summary.Count - 1].DisplayFormat = "{0:n2}";
                                }
                            }
                        }
                        foreach (string str in headers_dates)
                        {
                            if (gridColumns.FieldName.ToUpper().Contains(str.ToUpper()))
                            {
                                gridColumns.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                                gridColumns.DisplayFormat.FormatString = "g";
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

        private void XtraGrid_SaveLayout(object? sender, EventArgs e)
        {
            try
            {
                DXMenuItem item = sender as DXMenuItem;
                GridView gv = item.Tag as GridView;

                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ERPNext_PowerPlay",
                    MakeValidFileName(currentGrid.MainView.ViewCaption) + ".xml");
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                gv.SaveLayoutToXml(path);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void XtraGrid_LoadLayout()
        {
            try
            {
                if (currentGrid == null) return;
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ERPNext_PowerPlay",
                    MakeValidFileName(currentView.ViewCaption) + ".xml");
                if (File.Exists(path))
                    currentView.RestoreLayoutFromXml(path);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
                DXSubMenuItem sItem = new DXSubMenuItem("Export Options");

                sItem.Items.Add(new DXMenuItem("Export to xlsx", new EventHandler(ExportGrid), image: imageCollection1.Images[3]));
                sItem.Items.Add(new DXMenuItem("Export to csv", new EventHandler(ExportGrid), image: imageCollection1.Images[4]));
                sItem.Items.Add(new DXMenuItem("Export to pdf", new EventHandler(ExportGrid), image: imageCollection1.Images[5]));
                menu2.Items.Add(sItem);
                DXMenuItem saver = new DXMenuItem("Save layout", new EventHandler(XtraGrid_SaveLayout), image: imageCollection1.Images[3]);
                saver.Tag = gv;
                menu2.Items.Add(saver);
                menu2.ShowPopup(grid, e.HitInfo.HitPoint);
                
            }
        }
        GridControl currentGrid;
        GridView currentView;
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
              image,new EventHandler(OnFixedClick));
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
