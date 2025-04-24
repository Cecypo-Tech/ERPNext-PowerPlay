using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraBars;
using DevExpress.Images;
using System.IO;
using DevExpress.XtraEditors;


namespace ERPNext_PowerPlay.Helpers
{
    public class GridHelper
    {

        public GridControl TheGrid;
        public GridView View;
        public PopupMenu myPopupMenu;
        public BarManager myBarManager;

        private string formName;

        public GridHelper(string name_of_form)
        {
            formName = name_of_form;
        }

        private void Grid_ViewRegistered(object sender, ViewOperationEventArgs e) // Handles grid.ViewRegistered
        {
            (e.View as GridView).PopupMenuShowing += myPopupMenuShowing;
        }

        private void Grid_ViewRemoved(object sender, ViewOperationEventArgs e) // Handles GridControl1.ViewRemoved
        {
            (e.View as GridView).PopupMenuShowing -= myPopupMenuShowing;
        }
        private void myPopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            GridView view = TheGrid.FocusedView as GridView;
            if (e.HitInfo.InRow)
            {
                view.FocusedRowHandle = e.HitInfo.RowHandle;
                if (view.IsDetailView)
                {
                    myPopupMenu.ShowPopup(myBarManager, view.GridControl.PointToScreen(e.Point));
                }
            }
        }

        public void Grid_AddExportOptions(GridControl grid, PopupMenu addToMenu, BarManager barManager)
        {
            try
            {
                TheGrid = grid;
                View = (GridView)grid.MainView;
                if (View == null) return;
                myPopupMenu = addToMenu;
                myBarManager = barManager;

                for (int i = addToMenu.ItemLinks.Count - 1; i >= 0; i--)
                {
                    addToMenu.ItemLinks.RemoveAt(i);
                }

                addToMenu.MenuCaption = View.ViewCaption;
                addToMenu.ShowCaption = true;

                TheGrid.ViewRegistered += Grid_ViewRegistered;
                TheGrid.ViewRemoved += Grid_ViewRemoved;

                var itemCreateNewFrom = new BarButtonItem(barManager, "Create new Document from this");
                itemCreateNewFrom.Tag = MakeValidFileName(grid.MainView.GetViewCaption()).Replace("/", ".");
                itemCreateNewFrom.Glyph = ImageResourceCache.Default.GetImage("devav/actions/newdoc_16x16.png");
                addToMenu.AddItem(itemCreateNewFrom).BeginGroup = true; // ADD WITH SEPARATOR
                itemCreateNewFrom.ItemClick += BarButtonExportOptions_ItemClick;

                var itemHeader = new BarSubItem
                {
                    Caption = "Export Options",
                    Name = "Export Options",
                    Glyph = ImageResourceCache.Default.GetImage("office2013/export/export_16x16.png")
                };
                addToMenu.AddItem(itemHeader);
                
                // ADD EXPORT TO EXCEL WITH SEPARATOR
                var itemXLSX = new BarButtonItem(barManager, "Export to Excel")
                {
                    Tag = MakeValidFileName(grid.MainView.GetViewCaption()).Replace("/", "."),
                    Glyph = ImageResourceCache.Default.GetImage("office2013/export/exporttoxlsx_16x16.png"),
                    Enabled = true
                };
                itemHeader.AddItem(itemXLSX).BeginGroup = true; // ADD WITH SEPARATOR
                itemXLSX.ItemClick += BarButtonExportOptions_ItemClick;

                // ADD PRINT WITH SEPARATOR
                var itemPrint = new BarButtonItem(barManager, "Print Preview")
                {
                    Tag = MakeValidFileName(grid.MainView.GetViewCaption()).Replace("/", "."),
                    Glyph = ImageResourceCache.Default.GetImage("office2013/printpreview_16x16.png"),
                    Enabled = true
                };
                itemHeader.AddItem(itemPrint);
                itemPrint.ItemClick += BarButtonExportOptions_ItemClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Error while automatically adding Export Options (clsGridExport){0}{1}", Environment.NewLine, ex.Message),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        private void BarButtonExportOptions_ItemClick(Object sender, ItemClickEventArgs e)
        {
            string path = GetTemp("xlsx");
            View.ExportToXlsx(path);
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
    }
}