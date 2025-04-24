namespace ERPNext_PowerPlay.Forms
{
    partial class frmReportList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReportList));
            svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(components);
            emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            btnSave = new DevExpress.XtraEditors.SimpleButton();
            layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            gc_ReportList = new DevExpress.XtraGrid.GridControl();
            reportListBindingSource = new BindingSource(components);
            gv_ReportList = new DevExpress.XtraGrid.Views.Grid.GridView();
            colID = new DevExpress.XtraGrid.Columns.GridColumn();
            colEnabled = new DevExpress.XtraGrid.Columns.GridColumn();
            colReportName = new DevExpress.XtraGrid.Columns.GridColumn();
            colDocType = new DevExpress.XtraGrid.Columns.GridColumn();
            colEndPoint = new DevExpress.XtraGrid.Columns.GridColumn();
            colFieldList = new DevExpress.XtraGrid.Columns.GridColumn();
            colFilterList = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateCreated = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateModified = new DevExpress.XtraGrid.Columns.GridColumn();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)svgImageCollection1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControl1).BeginInit();
            layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gc_ReportList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)reportListBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gv_ReportList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem2).BeginInit();
            SuspendLayout();
            // 
            // svgImageCollection1
            // 
            svgImageCollection1.Add("EditMirrored.svg", (DevExpress.Utils.Svg.SvgImage)resources.GetObject("svgImageCollection1.EditMirrored.svg"));
            svgImageCollection1.Add("DrawSolid.svg", (DevExpress.Utils.Svg.SvgImage)resources.GetObject("svgImageCollection1.DrawSolid.svg"));
            // 
            // emptySpaceItem1
            // 
            emptySpaceItem1.Location = new Point(0, 434);
            emptySpaceItem1.Name = "emptySpaceItem1";
            emptySpaceItem1.Size = new Size(152, 10);
            emptySpaceItem1.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = btnSave;
            layoutControlItem2.Location = new Point(804, 387);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new Size(199, 57);
            layoutControlItem2.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            layoutControlItem2.TextVisible = false;
            // 
            // btnSave
            // 
            btnSave.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnSave.ImageOptions.SvgImage");
            btnSave.Location = new Point(818, 401);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(191, 36);
            btnSave.StyleController = layoutControl1;
            btnSave.TabIndex = 2;
            btnSave.Text = "&Save";
            btnSave.Click += btnSave_Click;
            // 
            // layoutControl1
            // 
            layoutControl1.Controls.Add(labelControl1);
            layoutControl1.Controls.Add(btnSave);
            layoutControl1.Controls.Add(gc_ReportList);
            layoutControl1.Dock = DockStyle.Fill;
            layoutControl1.Location = new Point(0, 0);
            layoutControl1.Name = "layoutControl1";
            layoutControl1.Padding = new Padding(2);
            layoutControl1.Root = Root;
            layoutControl1.Size = new Size(1023, 464);
            layoutControl1.TabIndex = 1;
            layoutControl1.Text = "layoutControl1";
            // 
            // gc_ReportList
            // 
            gc_ReportList.DataSource = reportListBindingSource;
            gc_ReportList.Location = new Point(14, 14);
            gc_ReportList.MainView = gv_ReportList;
            gc_ReportList.Name = "gc_ReportList";
            gc_ReportList.Size = new Size(995, 379);
            gc_ReportList.TabIndex = 0;
            gc_ReportList.UseEmbeddedNavigator = true;
            gc_ReportList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gv_ReportList });
            // 
            // reportListBindingSource
            // 
            reportListBindingSource.DataSource = typeof(Models.ReportList);
            // 
            // gv_ReportList
            // 
            gv_ReportList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colID, colEnabled, colReportName, colDocType, colEndPoint, colFieldList, colFilterList, colDateCreated, colDateModified });
            gv_ReportList.GridControl = gc_ReportList;
            gv_ReportList.Name = "gv_ReportList";
            gv_ReportList.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            gv_ReportList.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            gv_ReportList.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            gv_ReportList.InitNewRow += gv_ReportList_InitNewRow;
            // 
            // colID
            // 
            colID.FieldName = "ID";
            colID.Name = "colID";
            colID.Visible = true;
            colID.VisibleIndex = 0;
            // 
            // colEnabled
            // 
            colEnabled.FieldName = "Enabled";
            colEnabled.Name = "colEnabled";
            colEnabled.Visible = true;
            colEnabled.VisibleIndex = 1;
            // 
            // colReportName
            // 
            colReportName.FieldName = "ReportName";
            colReportName.Name = "colReportName";
            colReportName.Visible = true;
            colReportName.VisibleIndex = 2;
            // 
            // colDocType
            // 
            colDocType.FieldName = "DocType";
            colDocType.Name = "colDocType";
            colDocType.Visible = true;
            colDocType.VisibleIndex = 3;
            // 
            // colEndPoint
            // 
            colEndPoint.FieldName = "EndPoint";
            colEndPoint.Name = "colEndPoint";
            colEndPoint.Visible = true;
            colEndPoint.VisibleIndex = 4;
            // 
            // colFieldList
            // 
            colFieldList.FieldName = "FieldList";
            colFieldList.Name = "colFieldList";
            colFieldList.Visible = true;
            colFieldList.VisibleIndex = 5;
            // 
            // colFilterList
            // 
            colFilterList.FieldName = "FilterList";
            colFilterList.Name = "colFilterList";
            colFilterList.Visible = true;
            colFilterList.VisibleIndex = 6;
            // 
            // colDateCreated
            // 
            colDateCreated.FieldName = "DateCreated";
            colDateCreated.Name = "colDateCreated";
            colDateCreated.Visible = true;
            colDateCreated.VisibleIndex = 7;
            // 
            // colDateModified
            // 
            colDateModified.FieldName = "DateModified";
            colDateModified.Name = "colDateModified";
            colDateModified.Visible = true;
            colDateModified.VisibleIndex = 8;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem1, layoutControlItem2, emptySpaceItem1, layoutControlItem3, emptySpaceItem2 });
            Root.Name = "Root";
            Root.Size = new Size(1023, 464);
            Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = gc_ReportList;
            layoutControlItem1.Location = new Point(0, 0);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new Size(1003, 387);
            layoutControlItem1.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            layoutControlItem1.TextVisible = false;
            // 
            // labelControl1
            // 
            labelControl1.Location = new Point(14, 401);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new Size(144, 39);
            labelControl1.StyleController = layoutControl1;
            labelControl1.TabIndex = 4;
            labelControl1.Text = "Usable variables for reports; \r\n{from_date}\r\n{to_date}";
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = labelControl1;
            layoutControlItem3.Location = new Point(0, 387);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new Size(152, 47);
            layoutControlItem3.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            emptySpaceItem2.Location = new Point(152, 387);
            emptySpaceItem2.Name = "emptySpaceItem2";
            emptySpaceItem2.Size = new Size(652, 57);
            // 
            // frmReportList
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1023, 464);
            Controls.Add(layoutControl1);
            IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("frmReportList.IconOptions.SvgImage");
            Name = "frmReportList";
            Text = "Report List";
            ((System.ComponentModel.ISupportInitialize)svgImageCollection1).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControl1).EndInit();
            layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gc_ReportList).EndInit();
            ((System.ComponentModel.ISupportInitialize)reportListBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gv_ReportList).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.Utils.SvgImageCollection svgImageCollection1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraGrid.GridControl gc_ReportList;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_ReportList;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private BindingSource reportListBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colEnabled;
        private DevExpress.XtraGrid.Columns.GridColumn colReportName;
        private DevExpress.XtraGrid.Columns.GridColumn colDocType;
        private DevExpress.XtraGrid.Columns.GridColumn colEndPoint;
        private DevExpress.XtraGrid.Columns.GridColumn colFieldList;
        private DevExpress.XtraGrid.Columns.GridColumn colFilterList;
        private DevExpress.XtraGrid.Columns.GridColumn colDateCreated;
        private DevExpress.XtraGrid.Columns.GridColumn colDateModified;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
    }
}