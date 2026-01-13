namespace ERPNext_PowerPlay
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            btnLogin = new DevExpress.XtraBars.BarButtonItem();
            btnLogout = new DevExpress.XtraBars.BarButtonItem();
            btnPrintSettings = new DevExpress.XtraBars.BarButtonItem();
            barEditItem_DocNamePreview = new DevExpress.XtraBars.BarEditItem();
            repositoryItemTextEdit_PreviewDocName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            barEditItem_DoctypePreview = new DevExpress.XtraBars.BarEditItem();
            repositoryItemLookUp_PreviewDocType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            barButtonItem_JobHistory = new DevExpress.XtraBars.BarButtonItem();
            skinPaletteRibbonGalleryBarItem1 = new DevExpress.XtraBars.SkinPaletteRibbonGalleryBarItem();
            btnReportList = new DevExpress.XtraBars.BarButtonItem();
            skinRibbonGalleryBarItem1 = new DevExpress.XtraBars.SkinRibbonGalleryBarItem();
            barToggleSwitchItem1 = new DevExpress.XtraBars.BarToggleSwitchItem();
            ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            repositoryItemComboBox_PreviewDocTypes = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(components);
            tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(components);
            notifyIcon1 = new NotifyIcon(components);
            barButtonItem_getJson = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)ribbon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemTextEdit_PreviewDocName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemLookUp_PreviewDocType).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemComboBox_PreviewDocTypes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)documentManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tabbedView1).BeginInit();
            SuspendLayout();
            // 
            // ribbon
            // 
            ribbon.ExpandCollapseItem.Id = 0;
            ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] { ribbon.ExpandCollapseItem, btnLogin, btnLogout, btnPrintSettings, barEditItem_DocNamePreview, barButtonItem1, barEditItem_DoctypePreview, barButtonItem_JobHistory, skinPaletteRibbonGalleryBarItem1, btnReportList, skinRibbonGalleryBarItem1, barToggleSwitchItem1, barButtonItem_getJson });
            ribbon.Location = new Point(0, 0);
            ribbon.MaxItemId = 16;
            ribbon.Name = "ribbon";
            ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { ribbonPage1, ribbonPage2 });
            ribbon.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemComboBox_PreviewDocTypes, repositoryItemTextEdit_PreviewDocName, repositoryItemLookUp_PreviewDocType });
            ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            ribbon.Size = new Size(889, 170);
            ribbon.StatusBar = ribbonStatusBar;
            // 
            // btnLogin
            // 
            btnLogin.Caption = "Login";
            btnLogin.Id = 1;
            btnLogin.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnLogin.ImageOptions.SvgImage");
            btnLogin.Name = "btnLogin";
            btnLogin.ItemClick += btnLogin_ItemClick;
            // 
            // btnLogout
            // 
            btnLogout.Caption = "Logout";
            btnLogout.Id = 2;
            btnLogout.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnLogout.ImageOptions.SvgImage");
            btnLogout.Name = "btnLogout";
            btnLogout.ItemClick += btnLogout_ItemClick;
            // 
            // btnPrintSettings
            // 
            btnPrintSettings.Caption = "Print Jobs";
            btnPrintSettings.Id = 3;
            btnPrintSettings.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnPrintSettings.ImageOptions.SvgImage");
            btnPrintSettings.Name = "btnPrintSettings";
            btnPrintSettings.ItemClick += btnPrintSettings_ItemClick;
            // 
            // barEditItem_DocNamePreview
            // 
            barEditItem_DocNamePreview.Caption = "DocName";
            barEditItem_DocNamePreview.Edit = repositoryItemTextEdit_PreviewDocName;
            barEditItem_DocNamePreview.EditWidth = 150;
            barEditItem_DocNamePreview.Id = 7;
            barEditItem_DocNamePreview.Name = "barEditItem_DocNamePreview";
            // 
            // repositoryItemTextEdit_PreviewDocName
            // 
            repositoryItemTextEdit_PreviewDocName.AutoHeight = false;
            repositoryItemTextEdit_PreviewDocName.Name = "repositoryItemTextEdit_PreviewDocName";
            // 
            // barButtonItem1
            // 
            barButtonItem1.Caption = "Show Preview";
            barButtonItem1.Id = 8;
            barButtonItem1.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barButtonItem1.ImageOptions.SvgImage");
            barButtonItem1.Name = "barButtonItem1";
            barButtonItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem1.ItemClick += barButtonItem1_ItemClick;
            // 
            // barEditItem_DoctypePreview
            // 
            barEditItem_DoctypePreview.Caption = "DocType";
            barEditItem_DoctypePreview.Edit = repositoryItemLookUp_PreviewDocType;
            barEditItem_DoctypePreview.EditWidth = 150;
            barEditItem_DoctypePreview.Id = 9;
            barEditItem_DoctypePreview.Name = "barEditItem_DoctypePreview";
            // 
            // repositoryItemLookUp_PreviewDocType
            // 
            repositoryItemLookUp_PreviewDocType.AutoHeight = false;
            repositoryItemLookUp_PreviewDocType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemLookUp_PreviewDocType.Name = "repositoryItemLookUp_PreviewDocType";
            repositoryItemLookUp_PreviewDocType.NullText = "[None]";
            // 
            // barButtonItem_JobHistory
            // 
            barButtonItem_JobHistory.Caption = "Job History";
            barButtonItem_JobHistory.Id = 10;
            barButtonItem_JobHistory.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barButtonItem_JobHistory.ImageOptions.SvgImage");
            barButtonItem_JobHistory.Name = "barButtonItem_JobHistory";
            barButtonItem_JobHistory.ItemClick += barButtonItem_JobHistory_ItemClick;
            // 
            // skinPaletteRibbonGalleryBarItem1
            // 
            skinPaletteRibbonGalleryBarItem1.Caption = "skinPaletteRibbonGalleryBarItem1";
            skinPaletteRibbonGalleryBarItem1.Id = 11;
            skinPaletteRibbonGalleryBarItem1.Name = "skinPaletteRibbonGalleryBarItem1";
            // 
            // btnReportList
            // 
            btnReportList.Caption = "Report List";
            btnReportList.Id = 12;
            btnReportList.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnReportList.ImageOptions.SvgImage");
            btnReportList.Name = "btnReportList";
            btnReportList.ItemClick += btnReportList_ItemClick;
            // 
            // skinRibbonGalleryBarItem1
            // 
            skinRibbonGalleryBarItem1.Caption = "skinRibbonGalleryBarItem1";
            skinRibbonGalleryBarItem1.Id = 13;
            skinRibbonGalleryBarItem1.Name = "skinRibbonGalleryBarItem1";
            // 
            // barToggleSwitchItem1
            // 
            barToggleSwitchItem1.BindableChecked = true;
            barToggleSwitchItem1.Caption = "Timer";
            barToggleSwitchItem1.Checked = true;
            barToggleSwitchItem1.Id = 14;
            barToggleSwitchItem1.Name = "barToggleSwitchItem1";
            barToggleSwitchItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barToggleSwitchItem1.CheckedChanged += barToggleSwitchItem1_CheckedChanged;
            // 
            // ribbonPage1
            // 
            ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroup1, ribbonPageGroup2, ribbonPageGroup3, ribbonPageGroup6 });
            ribbonPage1.Name = "ribbonPage1";
            ribbonPage1.Text = "Main";
            // 
            // ribbonPageGroup1
            // 
            ribbonPageGroup1.ItemLinks.Add(btnLogin);
            ribbonPageGroup1.ItemLinks.Add(btnLogout);
            ribbonPageGroup1.Name = "ribbonPageGroup1";
            ribbonPageGroup1.Text = "Login";
            // 
            // ribbonPageGroup2
            // 
            ribbonPageGroup2.ItemLinks.Add(btnPrintSettings);
            ribbonPageGroup2.ItemLinks.Add(btnReportList);
            ribbonPageGroup2.Name = "ribbonPageGroup2";
            ribbonPageGroup2.Text = "Configurations";
            // 
            // ribbonPageGroup3
            // 
            ribbonPageGroup3.ItemLinks.Add(barButtonItem_JobHistory);
            ribbonPageGroup3.Name = "ribbonPageGroup3";
            ribbonPageGroup3.Text = "Data";
            // 
            // ribbonPageGroup6
            // 
            ribbonPageGroup6.ItemLinks.Add(barToggleSwitchItem1);
            ribbonPageGroup6.Name = "ribbonPageGroup6";
            ribbonPageGroup6.Text = "Actions";
            // 
            // ribbonPage2
            // 
            ribbonPage2.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroup4, ribbonPageGroup5 });
            ribbonPage2.Name = "ribbonPage2";
            ribbonPage2.Text = "Options";
            // 
            // ribbonPageGroup4
            // 
            ribbonPageGroup4.ItemLinks.Add(barEditItem_DoctypePreview);
            ribbonPageGroup4.ItemLinks.Add(barEditItem_DocNamePreview);
            ribbonPageGroup4.ItemLinks.Add(barButtonItem1);
            ribbonPageGroup4.ItemLinks.Add(barButtonItem_getJson, true);
            ribbonPageGroup4.Name = "ribbonPageGroup4";
            ribbonPageGroup4.Text = "Preview Test";
            // 
            // ribbonPageGroup5
            // 
            ribbonPageGroup5.ItemLinks.Add(skinRibbonGalleryBarItem1);
            ribbonPageGroup5.ItemLinks.Add(skinPaletteRibbonGalleryBarItem1);
            ribbonPageGroup5.Name = "ribbonPageGroup5";
            ribbonPageGroup5.Text = "Skins";
            // 
            // repositoryItemComboBox_PreviewDocTypes
            // 
            repositoryItemComboBox_PreviewDocTypes.AutoHeight = false;
            repositoryItemComboBox_PreviewDocTypes.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemComboBox_PreviewDocTypes.Name = "repositoryItemComboBox_PreviewDocTypes";
            repositoryItemComboBox_PreviewDocTypes.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // ribbonStatusBar
            // 
            ribbonStatusBar.Location = new Point(0, 416);
            ribbonStatusBar.Name = "ribbonStatusBar";
            ribbonStatusBar.Ribbon = ribbon;
            ribbonStatusBar.Size = new Size(889, 31);
            // 
            // documentManager1
            // 
            documentManager1.MdiParent = this;
            documentManager1.MenuManager = ribbon;
            documentManager1.View = tabbedView1;
            documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] { tabbedView1 });
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // barButtonItem_getJson
            // 
            barButtonItem_getJson.Caption = "Get Json";
            barButtonItem_getJson.Id = 15;
            barButtonItem_getJson.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            barButtonItem_getJson.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barButtonItem_getJson.ImageOptions.SvgImage");
            barButtonItem_getJson.ItemAppearance.Normal.ForeColor = Color.Gray;
            barButtonItem_getJson.ItemAppearance.Normal.Options.UseForeColor = true;
            barButtonItem_getJson.Name = "barButtonItem_getJson";
            barButtonItem_getJson.ItemClick += barButtonItem_getJson_ItemClick;
            // 
            // frmMain
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(889, 447);
            Controls.Add(ribbonStatusBar);
            Controls.Add(ribbon);
            IconOptions.Image = (Image)resources.GetObject("frmMain.IconOptions.Image");
            IsMdiContainer = true;
            Name = "frmMain";
            Ribbon = ribbon;
            StatusBar = ribbonStatusBar;
            Text = "ERPNext PowerPlay";
            WindowState = FormWindowState.Maximized;
            FormClosing += frmMain_FormClosing;
            Resize += frmMain_Resize;
            ((System.ComponentModel.ISupportInitialize)ribbon).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemTextEdit_PreviewDocName).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemLookUp_PreviewDocType).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemComboBox_PreviewDocTypes).EndInit();
            ((System.ComponentModel.ISupportInitialize)documentManager1).EndInit();
            ((System.ComponentModel.ISupportInitialize)tabbedView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarButtonItem btnLogin;
        private DevExpress.XtraBars.BarButtonItem btnLogout;
        private DevExpress.XtraBars.BarButtonItem btnPrintSettings;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox_PreviewDocTypes;
        private DevExpress.XtraBars.BarEditItem barEditItem_DocNamePreview;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit_PreviewDocName;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarEditItem barEditItem_DoctypePreview;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUp_PreviewDocType;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_JobHistory;
        private DevExpress.XtraBars.SkinPaletteRibbonGalleryBarItem skinPaletteRibbonGalleryBarItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
        private DevExpress.XtraBars.BarButtonItem btnReportList;
        private NotifyIcon notifyIcon1;
        private DevExpress.XtraBars.SkinRibbonGalleryBarItem skinRibbonGalleryBarItem1;
        private DevExpress.XtraBars.BarToggleSwitchItem barToggleSwitchItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_getJson;
    }
}