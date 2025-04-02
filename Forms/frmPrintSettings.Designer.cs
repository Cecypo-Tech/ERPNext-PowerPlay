namespace ERPNext_PowerPlay
{
    partial class frmPrintSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrintSettings));
            layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            btnSave = new DevExpress.XtraEditors.SimpleButton();
            gc_PrintSettings = new DevExpress.XtraGrid.GridControl();
            printerSettingBindingSource = new BindingSource(components);
            gv_PrintSettings = new DevExpress.XtraGrid.Views.Grid.GridView();
            colID = new DevExpress.XtraGrid.Columns.GridColumn();
            colPrintEngine = new DevExpress.XtraGrid.Columns.GridColumn();
            colDocType = new DevExpress.XtraGrid.Columns.GridColumn();
            colCopyName = new DevExpress.XtraGrid.Columns.GridColumn();
            colFontSize = new DevExpress.XtraGrid.Columns.GridColumn();
            colOrientation = new DevExpress.XtraGrid.Columns.GridColumn();
            colScaling = new DevExpress.XtraGrid.Columns.GridColumn();
            colPrinter = new DevExpress.XtraGrid.Columns.GridColumn();
            colCopies = new DevExpress.XtraGrid.Columns.GridColumn();
            colPageRange = new DevExpress.XtraGrid.Columns.GridColumn();
            colFrappeTemplateName = new DevExpress.XtraGrid.Columns.GridColumn();
            colLetterHead = new DevExpress.XtraGrid.Columns.GridColumn();
            colCompact = new DevExpress.XtraGrid.Columns.GridColumn();
            colUOM = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateCreated = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateModified = new DevExpress.XtraGrid.Columns.GridColumn();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(components);
            ((System.ComponentModel.ISupportInitialize)layoutControl1).BeginInit();
            layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gc_PrintSettings).BeginInit();
            ((System.ComponentModel.ISupportInitialize)printerSettingBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gv_PrintSettings).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)svgImageCollection1).BeginInit();
            SuspendLayout();
            // 
            // layoutControl1
            // 
            layoutControl1.Controls.Add(simpleButton1);
            layoutControl1.Controls.Add(btnSave);
            layoutControl1.Controls.Add(gc_PrintSettings);
            layoutControl1.Dock = DockStyle.Fill;
            layoutControl1.Location = new Point(0, 0);
            layoutControl1.Name = "layoutControl1";
            layoutControl1.Padding = new Padding(2);
            layoutControl1.Root = Root;
            layoutControl1.Size = new Size(775, 456);
            layoutControl1.TabIndex = 0;
            layoutControl1.Text = "layoutControl1";
            // 
            // simpleButton1
            // 
            simpleButton1.Location = new Point(12, 404);
            simpleButton1.Name = "simpleButton1";
            simpleButton1.Size = new Size(110, 22);
            simpleButton1.StyleController = layoutControl1;
            simpleButton1.TabIndex = 4;
            simpleButton1.Text = "&Print";
            simpleButton1.Click += simpleButton1_Click;
            // 
            // btnSave
            // 
            btnSave.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnSave.ImageOptions.SvgImage");
            btnSave.Location = new Point(616, 406);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(145, 36);
            btnSave.StyleController = layoutControl1;
            btnSave.TabIndex = 2;
            btnSave.Text = "&Save";
            btnSave.Click += btnSave_Click;
            // 
            // gc_PrintSettings
            // 
            gc_PrintSettings.DataSource = printerSettingBindingSource;
            gc_PrintSettings.Location = new Point(14, 14);
            gc_PrintSettings.MainView = gv_PrintSettings;
            gc_PrintSettings.Name = "gc_PrintSettings";
            gc_PrintSettings.Size = new Size(747, 384);
            gc_PrintSettings.TabIndex = 0;
            gc_PrintSettings.UseEmbeddedNavigator = true;
            gc_PrintSettings.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gv_PrintSettings });
            gc_PrintSettings.Click += gc_PrintSettings_Click;
            // 
            // printerSettingBindingSource
            // 
            printerSettingBindingSource.DataSource = typeof(Models.PrinterSetting);
            // 
            // gv_PrintSettings
            // 
            gv_PrintSettings.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colID, colPrintEngine, colDocType, colCopyName, colFontSize, colOrientation, colScaling, colPrinter, colCopies, colPageRange, colFrappeTemplateName, colLetterHead, colCompact, colUOM, colDateCreated, colDateModified });
            gv_PrintSettings.GridControl = gc_PrintSettings;
            gv_PrintSettings.Name = "gv_PrintSettings";
            gv_PrintSettings.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            gv_PrintSettings.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            gv_PrintSettings.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            gv_PrintSettings.InitNewRow += gv_PrintSettings_InitNewRow;
            // 
            // colID
            // 
            colID.FieldName = "ID";
            colID.Name = "colID";
            colID.Visible = true;
            colID.VisibleIndex = 0;
            // 
            // colPrintEngine
            // 
            colPrintEngine.FieldName = "PrintEngine";
            colPrintEngine.Name = "colPrintEngine";
            colPrintEngine.Visible = true;
            colPrintEngine.VisibleIndex = 1;
            // 
            // colDocType
            // 
            colDocType.FieldName = "DocType";
            colDocType.Name = "colDocType";
            colDocType.Visible = true;
            colDocType.VisibleIndex = 2;
            // 
            // colCopyName
            // 
            colCopyName.FieldName = "CopyName";
            colCopyName.Name = "colCopyName";
            colCopyName.Visible = true;
            colCopyName.VisibleIndex = 3;
            // 
            // colFontSize
            // 
            colFontSize.FieldName = "FontSize";
            colFontSize.Name = "colFontSize";
            colFontSize.Visible = true;
            colFontSize.VisibleIndex = 4;
            // 
            // colOrientation
            // 
            colOrientation.FieldName = "Orientation";
            colOrientation.Name = "colOrientation";
            colOrientation.Visible = true;
            colOrientation.VisibleIndex = 5;
            // 
            // colScaling
            // 
            colScaling.FieldName = "Scaling";
            colScaling.Name = "colScaling";
            colScaling.Visible = true;
            colScaling.VisibleIndex = 6;
            // 
            // colPrinter
            // 
            colPrinter.FieldName = "Printer";
            colPrinter.Name = "colPrinter";
            colPrinter.Visible = true;
            colPrinter.VisibleIndex = 7;
            // 
            // colCopies
            // 
            colCopies.FieldName = "Copies";
            colCopies.Name = "colCopies";
            colCopies.Visible = true;
            colCopies.VisibleIndex = 8;
            // 
            // colPageRange
            // 
            colPageRange.FieldName = "PageRange";
            colPageRange.Name = "colPageRange";
            colPageRange.Visible = true;
            colPageRange.VisibleIndex = 9;
            // 
            // colFrappeTemplateName
            // 
            colFrappeTemplateName.Caption = "Template";
            colFrappeTemplateName.FieldName = "FrappeTemplateName";
            colFrappeTemplateName.Name = "colFrappeTemplateName";
            colFrappeTemplateName.OptionsEditForm.ColumnSpan = 3;
            colFrappeTemplateName.OptionsEditForm.StartNewRow = true;
            colFrappeTemplateName.Visible = true;
            colFrappeTemplateName.VisibleIndex = 10;
            // 
            // colLetterHead
            // 
            colLetterHead.FieldName = "LetterHead";
            colLetterHead.Name = "colLetterHead";
            colLetterHead.Visible = true;
            colLetterHead.VisibleIndex = 11;
            // 
            // colCompact
            // 
            colCompact.FieldName = "Compact";
            colCompact.Name = "colCompact";
            colCompact.Visible = true;
            colCompact.VisibleIndex = 12;
            // 
            // colUOM
            // 
            colUOM.FieldName = "UOM";
            colUOM.Name = "colUOM";
            colUOM.Visible = true;
            colUOM.VisibleIndex = 13;
            // 
            // colDateCreated
            // 
            colDateCreated.FieldName = "DateCreated";
            colDateCreated.Name = "colDateCreated";
            colDateCreated.Visible = true;
            colDateCreated.VisibleIndex = 14;
            // 
            // colDateModified
            // 
            colDateModified.FieldName = "DateModified";
            colDateModified.Name = "colDateModified";
            colDateModified.Visible = true;
            colDateModified.VisibleIndex = 15;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem1, layoutControlItem2, emptySpaceItem1, layoutControlItem3 });
            Root.Name = "Root";
            Root.Size = new Size(775, 456);
            Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = gc_PrintSettings;
            layoutControlItem1.Location = new Point(0, 0);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new Size(755, 392);
            layoutControlItem1.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = btnSave;
            layoutControlItem2.Location = new Point(602, 392);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new Size(153, 44);
            layoutControlItem2.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            layoutControlItem2.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            emptySpaceItem1.Location = new Point(114, 392);
            emptySpaceItem1.Name = "emptySpaceItem1";
            emptySpaceItem1.Size = new Size(488, 44);
            emptySpaceItem1.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = simpleButton1;
            layoutControlItem3.Location = new Point(0, 392);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new Size(114, 44);
            layoutControlItem3.TextVisible = false;
            // 
            // svgImageCollection1
            // 
            svgImageCollection1.Add("EditMirrored.svg", (DevExpress.Utils.Svg.SvgImage)resources.GetObject("svgImageCollection1.EditMirrored.svg"));
            svgImageCollection1.Add("DrawSolid.svg", (DevExpress.Utils.Svg.SvgImage)resources.GetObject("svgImageCollection1.DrawSolid.svg"));
            // 
            // frmPrintSettings
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(775, 456);
            Controls.Add(layoutControl1);
            IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("frmPrintSettings.IconOptions.SvgImage");
            Name = "frmPrintSettings";
            Text = "Print Settings";
            ((System.ComponentModel.ISupportInitialize)layoutControl1).EndInit();
            layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gc_PrintSettings).EndInit();
            ((System.ComponentModel.ISupportInitialize)printerSettingBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gv_PrintSettings).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)svgImageCollection1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraGrid.GridControl gc_PrintSettings;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_PrintSettings;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private BindingSource printerSettingBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colPrintEngine;
        private DevExpress.XtraGrid.Columns.GridColumn colDocType;
        private DevExpress.XtraGrid.Columns.GridColumn colCopyName;
        private DevExpress.XtraGrid.Columns.GridColumn colFontSize;
        private DevExpress.XtraGrid.Columns.GridColumn colOrientation;
        private DevExpress.XtraGrid.Columns.GridColumn colScaling;
        private DevExpress.XtraGrid.Columns.GridColumn colPrinter;
        private DevExpress.XtraGrid.Columns.GridColumn colCopies;
        private DevExpress.XtraGrid.Columns.GridColumn colPageRange;
        private DevExpress.XtraGrid.Columns.GridColumn colFrappeTemplateName;
        private DevExpress.XtraGrid.Columns.GridColumn colLetterHead;
        private DevExpress.XtraGrid.Columns.GridColumn colCompact;
        private DevExpress.XtraGrid.Columns.GridColumn colUOM;
        private DevExpress.XtraGrid.Columns.GridColumn colDateCreated;
        private DevExpress.XtraGrid.Columns.GridColumn colDateModified;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.Utils.SvgImageCollection svgImageCollection1;
    }
}