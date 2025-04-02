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
            btnShowLogs = new DevExpress.XtraBars.BarButtonItem();
            barEditItem_DocNamePreview = new DevExpress.XtraBars.BarEditItem();
            repositoryItemTextEdit_PreviewDocName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            barEditItem_DoctypePreview = new DevExpress.XtraBars.BarEditItem();
            repositoryItemLookUp_PreviewDocType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            repositoryItemComboBox_PreviewDocTypes = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(components);
            tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(components);
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
            ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] { ribbon.ExpandCollapseItem, btnLogin, btnLogout, btnPrintSettings, btnShowLogs, barEditItem_DocNamePreview, barButtonItem1, barEditItem_DoctypePreview });
            ribbon.Location = new Point(0, 0);
            ribbon.MaxItemId = 10;
            ribbon.Name = "ribbon";
            ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { ribbonPage1, ribbonPage2 });
            ribbon.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemComboBox_PreviewDocTypes, repositoryItemTextEdit_PreviewDocName, repositoryItemLookUp_PreviewDocType });
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
            btnPrintSettings.Caption = "Print Settings";
            btnPrintSettings.Id = 3;
            btnPrintSettings.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnPrintSettings.ImageOptions.SvgImage");
            btnPrintSettings.Name = "btnPrintSettings";
            btnPrintSettings.ItemClick += btnPrintSettings_ItemClick;
            // 
            // btnShowLogs
            // 
            btnShowLogs.Caption = "Show Logs";
            btnShowLogs.Id = 4;
            btnShowLogs.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnShowLogs.ImageOptions.SvgImage");
            btnShowLogs.Name = "btnShowLogs";
            btnShowLogs.ItemClick += btnShowLogs_ItemClick;
            // 
            // barEditItem_DocNamePreview
            // 
            barEditItem_DocNamePreview.Caption = "DocName";
            barEditItem_DocNamePreview.Edit = repositoryItemTextEdit_PreviewDocName;
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
            // ribbonPage1
            // 
            ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroup1, ribbonPageGroup2, ribbonPageGroup3 });
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
            ribbonPageGroup2.Name = "ribbonPageGroup2";
            ribbonPageGroup2.Text = "Printing";
            // 
            // ribbonPageGroup3
            // 
            ribbonPageGroup3.ItemLinks.Add(btnShowLogs);
            ribbonPageGroup3.Name = "ribbonPageGroup3";
            ribbonPageGroup3.Text = "Other";
            // 
            // ribbonPage2
            // 
            ribbonPage2.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroup4 });
            ribbonPage2.Name = "ribbonPage2";
            ribbonPage2.Text = "Options";
            // 
            // ribbonPageGroup4
            // 
            ribbonPageGroup4.ItemLinks.Add(barEditItem_DoctypePreview);
            ribbonPageGroup4.ItemLinks.Add(barEditItem_DocNamePreview);
            ribbonPageGroup4.ItemLinks.Add(barButtonItem1);
            ribbonPageGroup4.Name = "ribbonPageGroup4";
            ribbonPageGroup4.Text = "Preview";
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
            // frmMain
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(889, 447);
            Controls.Add(ribbonStatusBar);
            Controls.Add(ribbon);
            IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("frmMain.IconOptions.SvgImage");
            IsMdiContainer = true;
            Name = "frmMain";
            Ribbon = ribbon;
            StatusBar = ribbonStatusBar;
            Text = "frmMain";
            WindowState = FormWindowState.Maximized;
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
        private DevExpress.XtraBars.BarButtonItem btnShowLogs;
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
    }
}