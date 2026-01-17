namespace ERPNext_PowerPlay.Forms
{
    partial class frmData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmData));
            layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            barManager1 = new DevExpress.XtraBars.BarManager(components);
            popupMenu1 = new DevExpress.XtraBars.PopupMenu(components);
            popupMenuActions = new DevExpress.XtraBars.PopupMenu(components);
            barButtonPrintPreview = new DevExpress.XtraBars.BarButtonItem();
            barButtonPrint = new DevExpress.XtraBars.BarButtonItem();
            barButtonExportXlsx = new DevExpress.XtraBars.BarButtonItem();
            barButtonExportCsv = new DevExpress.XtraBars.BarButtonItem();
            barButtonExportPdf = new DevExpress.XtraBars.BarButtonItem();
            barButtonSaveLayout = new DevExpress.XtraBars.BarButtonItem();
            barButtonResetLayout = new DevExpress.XtraBars.BarButtonItem();
            bar3 = new DevExpress.XtraBars.Bar();
            dropDownActions = new DevExpress.XtraEditors.DropDownButton();
            barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            toggleSwitch1 = new DevExpress.XtraEditors.ToggleSwitch();
            comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            btnLoad = new DevExpress.XtraEditors.SimpleButton();
            dtTo = new DevExpress.XtraEditors.DateEdit();
            dtFrom = new DevExpress.XtraEditors.DateEdit();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItemActions = new DevExpress.XtraLayout.LayoutControlItem();
            imageCollection1 = new DevExpress.Utils.ImageCollection(components);
            ((System.ComponentModel.ISupportInitialize)layoutControl1).BeginInit();
            layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)barManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)popupMenu1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)popupMenuActions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).BeginInit();
            xtraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)toggleSwitch1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)comboBoxEdit1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtTo.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtTo.Properties.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtFrom.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtFrom.Properties.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItemActions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imageCollection1).BeginInit();
            SuspendLayout();
            //
            // layoutControl1
            //
            layoutControl1.Controls.Add(xtraTabControl1);
            layoutControl1.Controls.Add(toggleSwitch1);
            layoutControl1.Controls.Add(comboBoxEdit1);
            layoutControl1.Controls.Add(btnLoad);
            layoutControl1.Controls.Add(dtTo);
            layoutControl1.Controls.Add(dtFrom);
            layoutControl1.Controls.Add(dropDownActions);
            layoutControl1.Dock = DockStyle.Fill;
            layoutControl1.Location = new Point(0, 0);
            layoutControl1.Name = "layoutControl1";
            layoutControl1.Padding = new Padding(2);
            layoutControl1.Root = Root;
            layoutControl1.Size = new Size(1018, 376);
            layoutControl1.TabIndex = 1;
            layoutControl1.Text = "layoutControl1";
            //
            // barManager1
            //
            barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] { bar3 });
            barManager1.DockControls.Add(barDockControlTop);
            barManager1.DockControls.Add(barDockControlBottom);
            barManager1.DockControls.Add(barDockControlLeft);
            barManager1.DockControls.Add(barDockControlRight);
            barManager1.Form = this;
            barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { barButtonPrintPreview, barButtonPrint, barButtonExportXlsx, barButtonExportCsv, barButtonExportPdf, barButtonSaveLayout, barButtonResetLayout });
            barManager1.StatusBar = bar3;
            //
            // popupMenu1
            //
            popupMenu1.Manager = barManager1;
            popupMenu1.Name = "popupMenu1";
            //
            // popupMenuActions
            //
            popupMenuActions.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
                new DevExpress.XtraBars.LinkPersistInfo(barButtonPrintPreview),
                new DevExpress.XtraBars.LinkPersistInfo(barButtonPrint),
                new DevExpress.XtraBars.LinkPersistInfo(barButtonExportXlsx, true),
                new DevExpress.XtraBars.LinkPersistInfo(barButtonExportCsv),
                new DevExpress.XtraBars.LinkPersistInfo(barButtonExportPdf),
                new DevExpress.XtraBars.LinkPersistInfo(barButtonSaveLayout, true),
                new DevExpress.XtraBars.LinkPersistInfo(barButtonResetLayout)
            });
            popupMenuActions.Manager = barManager1;
            popupMenuActions.Name = "popupMenuActions";
            //
            // barButtonPrintPreview
            //
            barButtonPrintPreview.Caption = "Print Preview";
            barButtonPrintPreview.Id = 1;
            barButtonPrintPreview.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromResources("DevExpress.Utils.Svg.SvgImages.Outlook Inspired.PrintPreview.svg", typeof(DevExpress.Utils.Svg.SvgImage).Assembly);
            barButtonPrintPreview.Name = "barButtonPrintPreview";
            barButtonPrintPreview.ItemClick += barButtonPrintPreview_ItemClick;
            //
            // barButtonPrint
            //
            barButtonPrint.Caption = "Print";
            barButtonPrint.Id = 2;
            barButtonPrint.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromResources("DevExpress.Utils.Svg.SvgImages.Outlook Inspired.Print.svg", typeof(DevExpress.Utils.Svg.SvgImage).Assembly);
            barButtonPrint.Name = "barButtonPrint";
            barButtonPrint.ItemClick += barButtonPrint_ItemClick;
            //
            // barButtonExportXlsx
            //
            barButtonExportXlsx.Caption = "Export to xlsx";
            barButtonExportXlsx.Id = 3;
            barButtonExportXlsx.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromResources("DevExpress.Utils.Svg.SvgImages.Export.ExportToXLSX.svg", typeof(DevExpress.Utils.Svg.SvgImage).Assembly);
            barButtonExportXlsx.Name = "barButtonExportXlsx";
            barButtonExportXlsx.ItemClick += barButtonExportXlsx_ItemClick;
            //
            // barButtonExportCsv
            //
            barButtonExportCsv.Caption = "Export to csv";
            barButtonExportCsv.Id = 4;
            barButtonExportCsv.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromResources("DevExpress.Utils.Svg.SvgImages.Export.ExportToCSV.svg", typeof(DevExpress.Utils.Svg.SvgImage).Assembly);
            barButtonExportCsv.Name = "barButtonExportCsv";
            barButtonExportCsv.ItemClick += barButtonExportCsv_ItemClick;
            //
            // barButtonExportPdf
            //
            barButtonExportPdf.Caption = "Export to pdf";
            barButtonExportPdf.Id = 5;
            barButtonExportPdf.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromResources("DevExpress.Utils.Svg.SvgImages.Export.ExportToPDF.svg", typeof(DevExpress.Utils.Svg.SvgImage).Assembly);
            barButtonExportPdf.Name = "barButtonExportPdf";
            barButtonExportPdf.ItemClick += barButtonExportPdf_ItemClick;
            //
            // barButtonSaveLayout
            //
            barButtonSaveLayout.Caption = "Save Layout";
            barButtonSaveLayout.Id = 6;
            barButtonSaveLayout.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromResources("DevExpress.Utils.Svg.SvgImages.Save.Save.svg", typeof(DevExpress.Utils.Svg.SvgImage).Assembly);
            barButtonSaveLayout.Name = "barButtonSaveLayout";
            barButtonSaveLayout.ItemClick += barButtonSaveLayout_ItemClick;
            //
            // barButtonResetLayout
            //
            barButtonResetLayout.Caption = "Reset Layout";
            barButtonResetLayout.Id = 7;
            barButtonResetLayout.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromResources("DevExpress.Utils.Svg.SvgImages.Icon Builder.Actions_Clear.svg", typeof(DevExpress.Utils.Svg.SvgImage).Assembly);
            barButtonResetLayout.Name = "barButtonResetLayout";
            barButtonResetLayout.ItemClick += barButtonResetLayout_ItemClick;
            //
            // dropDownActions
            //
            dropDownActions.DropDownControl = popupMenuActions;
            dropDownActions.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromResources("DevExpress.Utils.Svg.SvgImages.Dashboards.ShowItemCaption.svg", typeof(DevExpress.Utils.Svg.SvgImage).Assembly);
            dropDownActions.Location = new Point(929, 12);
            dropDownActions.Name = "dropDownActions";
            dropDownActions.Size = new Size(77, 22);
            dropDownActions.StyleController = layoutControl1;
            dropDownActions.TabIndex = 7;
            dropDownActions.Text = "Actions";
            //
            // bar3
            // 
            bar3.BarName = "Status bar";
            bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            bar3.DockCol = 0;
            bar3.DockRow = 0;
            bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            bar3.OptionsBar.AllowQuickCustomization = false;
            bar3.OptionsBar.DrawDragBorder = false;
            bar3.OptionsBar.UseWholeRow = true;
            bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            barDockControlTop.CausesValidation = false;
            barDockControlTop.Dock = DockStyle.Top;
            barDockControlTop.Location = new Point(0, 0);
            barDockControlTop.Manager = barManager1;
            barDockControlTop.Size = new Size(1018, 0);
            // 
            // barDockControlBottom
            // 
            barDockControlBottom.CausesValidation = false;
            barDockControlBottom.Dock = DockStyle.Bottom;
            barDockControlBottom.Location = new Point(0, 376);
            barDockControlBottom.Manager = barManager1;
            barDockControlBottom.Size = new Size(1018, 25);
            // 
            // barDockControlLeft
            // 
            barDockControlLeft.CausesValidation = false;
            barDockControlLeft.Dock = DockStyle.Left;
            barDockControlLeft.Location = new Point(0, 0);
            barDockControlLeft.Manager = barManager1;
            barDockControlLeft.Size = new Size(0, 376);
            // 
            // barDockControlRight
            // 
            barDockControlRight.CausesValidation = false;
            barDockControlRight.Dock = DockStyle.Right;
            barDockControlRight.Location = new Point(1018, 0);
            barDockControlRight.Manager = barManager1;
            barDockControlRight.Size = new Size(0, 376);
            // 
            // xtraTabControl1
            // 
            xtraTabControl1.Location = new Point(12, 46);
            xtraTabControl1.Name = "xtraTabControl1";
            xtraTabControl1.SelectedTabPage = xtraTabPage1;
            xtraTabControl1.Size = new Size(994, 318);
            xtraTabControl1.TabIndex = 6;
            xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] { xtraTabPage1 });
            // 
            // xtraTabPage1
            // 
            xtraTabPage1.Name = "xtraTabPage1";
            xtraTabPage1.Size = new Size(992, 294);
            xtraTabPage1.Text = "xtraTabPage1";
            // 
            // toggleSwitch1
            // 
            toggleSwitch1.Location = new Point(12, 12);
            toggleSwitch1.Name = "toggleSwitch1";
            toggleSwitch1.Properties.OffText = "GRID";
            toggleSwitch1.Properties.OnText = "PIVOT";
            toggleSwitch1.Size = new Size(107, 24);
            toggleSwitch1.StyleController = layoutControl1;
            toggleSwitch1.TabIndex = 0;
            // 
            // comboBoxEdit1
            // 
            comboBoxEdit1.Location = new Point(170, 12);
            comboBoxEdit1.Name = "comboBoxEdit1";
            comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            comboBoxEdit1.Size = new Size(200, 22);
            comboBoxEdit1.StyleController = layoutControl1;
            comboBoxEdit1.TabIndex = 2;
            // 
            // btnLoad
            // 
            btnLoad.Appearance.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLoad.Appearance.Options.UseFont = true;
            btnLoad.AppearanceHovered.FontSizeDelta = 2;
            btnLoad.AppearanceHovered.FontStyleDelta = FontStyle.Bold;
            btnLoad.AppearanceHovered.Options.UseFont = true;
            btnLoad.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            btnLoad.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            btnLoad.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnLoad.ImageOptions.SvgImage");
            btnLoad.ImageOptions.SvgImageSize = new Size(16, 16);
            btnLoad.Location = new Point(776, 14);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(149, 22);
            btnLoad.StyleController = layoutControl1;
            btnLoad.TabIndex = 5;
            btnLoad.Text = "Load";
            btnLoad.Click += btnLoad_Click;
            // 
            // dtTo
            // 
            dtTo.EditValue = null;
            dtTo.Location = new Point(621, 12);
            dtTo.Name = "dtTo";
            dtTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dtTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dtTo.Size = new Size(149, 22);
            dtTo.StyleController = layoutControl1;
            dtTo.TabIndex = 4;
            // 
            // dtFrom
            // 
            dtFrom.EditValue = null;
            dtFrom.Location = new Point(421, 12);
            dtFrom.Name = "dtFrom";
            dtFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dtFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dtFrom.Size = new Size(149, 22);
            dtFrom.StyleController = layoutControl1;
            dtFrom.TabIndex = 3;
            //
            // Root
            //
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem1, layoutControlItem4, layoutControlItem5, layoutControlItem3, layoutControlItem7, layoutControlItem8, layoutControlItemActions });
            Root.Name = "Root";
            Root.Size = new Size(1018, 376);
            Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = dtFrom;
            layoutControlItem1.CustomizationFormText = "From";
            layoutControlItem1.Location = new Point(362, 0);
            layoutControlItem1.MaxSize = new Size(200, 34);
            layoutControlItem1.MinSize = new Size(200, 34);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new Size(200, 34);
            layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            layoutControlItem1.Text = "From";
            layoutControlItem1.TextSize = new Size(35, 13);
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = btnLoad;
            layoutControlItem4.Location = new Point(762, 0);
            layoutControlItem4.MaxSize = new Size(157, 30);
            layoutControlItem4.MinSize = new Size(157, 30);
            layoutControlItem4.Name = "layoutControlItem4";
            layoutControlItem4.Size = new Size(157, 34);
            layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            layoutControlItem4.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            layoutControlItem5.Control = comboBoxEdit1;
            layoutControlItem5.Location = new Point(111, 0);
            layoutControlItem5.MaxSize = new Size(251, 34);
            layoutControlItem5.MinSize = new Size(251, 34);
            layoutControlItem5.Name = "layoutControlItem5";
            layoutControlItem5.Size = new Size(251, 34);
            layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            layoutControlItem5.Text = "Report";
            layoutControlItem5.TextSize = new Size(35, 13);
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = dtTo;
            layoutControlItem3.CustomizationFormText = "To";
            layoutControlItem3.Location = new Point(562, 0);
            layoutControlItem3.MaxSize = new Size(200, 34);
            layoutControlItem3.MinSize = new Size(200, 34);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new Size(200, 34);
            layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            layoutControlItem3.Text = "To";
            layoutControlItem3.TextSize = new Size(35, 13);
            // 
            // layoutControlItem7
            // 
            layoutControlItem7.Control = toggleSwitch1;
            layoutControlItem7.Location = new Point(0, 0);
            layoutControlItem7.Name = "layoutControlItem7";
            layoutControlItem7.Size = new Size(111, 34);
            layoutControlItem7.Text = "Mode";
            layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            layoutControlItem8.Control = xtraTabControl1;
            layoutControlItem8.Location = new Point(0, 34);
            layoutControlItem8.Name = "layoutControlItem8";
            layoutControlItem8.Size = new Size(998, 322);
            layoutControlItem8.TextVisible = false;
            //
            // layoutControlItemActions
            //
            layoutControlItemActions.Control = dropDownActions;
            layoutControlItemActions.Location = new Point(919, 0);
            layoutControlItemActions.MaxSize = new Size(79, 34);
            layoutControlItemActions.MinSize = new Size(79, 34);
            layoutControlItemActions.Name = "layoutControlItemActions";
            layoutControlItemActions.Size = new Size(79, 34);
            layoutControlItemActions.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            layoutControlItemActions.TextVisible = false;
            // 
            // imageCollection1
            // 
            imageCollection1.ImageStream = (DevExpress.Utils.ImageCollectionStreamer)resources.GetObject("imageCollection1.ImageStream");
            imageCollection1.Images.SetKeyName(0, "clear_16x16.png");
            imageCollection1.Images.SetKeyName(1, "alignverticalleft_16x16.png");
            imageCollection1.Images.SetKeyName(2, "alignverticalright_16x16.png");
            imageCollection1.Images.SetKeyName(3, "exporttoxls_16x16.png");
            imageCollection1.InsertGalleryImage("exporttocsv_16x16.png", "images/export/exporttocsv_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/export/exporttocsv_16x16.png"), 4);
            imageCollection1.Images.SetKeyName(4, "exporttocsv_16x16.png");
            imageCollection1.InsertGalleryImage("exporttopdf_16x16.png", "images/export/exporttopdf_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/export/exporttopdf_16x16.png"), 5);
            imageCollection1.Images.SetKeyName(5, "exporttopdf_16x16.png");
            imageCollection1.InsertGalleryImage("print_16x16.png", "office2013/print/print_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("office2013/print/print_16x16.png"), 6);
            imageCollection1.Images.SetKeyName(6, "print_16x16.png");
            // 
            // frmData
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1018, 401);
            Controls.Add(layoutControl1);
            Controls.Add(barDockControlLeft);
            Controls.Add(barDockControlRight);
            Controls.Add(barDockControlBottom);
            Controls.Add(barDockControlTop);
            IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("frmData.IconOptions.SvgImage");
            Name = "frmData";
            Text = "Data";
            ((System.ComponentModel.ISupportInitialize)layoutControl1).EndInit();
            layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)barManager1).EndInit();
            ((System.ComponentModel.ISupportInitialize)popupMenu1).EndInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).EndInit();
            xtraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)toggleSwitch1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)comboBoxEdit1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtTo.Properties.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtTo.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtFrom.Properties.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtFrom.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem8).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItemActions).EndInit();
            ((System.ComponentModel.ISupportInitialize)popupMenuActions).EndInit();
            ((System.ComponentModel.ISupportInitialize)imageCollection1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.SimpleButton btnLoad;
        private DevExpress.XtraEditors.DateEdit dtTo;
        private DevExpress.XtraEditors.DateEdit dtFrom;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.ToggleSwitch toggleSwitch1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemActions;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.PopupMenu popupMenuActions;
        private DevExpress.XtraEditors.DropDownButton dropDownActions;
        private DevExpress.XtraBars.BarButtonItem barButtonPrintPreview;
        private DevExpress.XtraBars.BarButtonItem barButtonPrint;
        private DevExpress.XtraBars.BarButtonItem barButtonExportXlsx;
        private DevExpress.XtraBars.BarButtonItem barButtonExportCsv;
        private DevExpress.XtraBars.BarButtonItem barButtonExportPdf;
        private DevExpress.XtraBars.BarButtonItem barButtonSaveLayout;
        private DevExpress.XtraBars.BarButtonItem barButtonResetLayout;
    }
}