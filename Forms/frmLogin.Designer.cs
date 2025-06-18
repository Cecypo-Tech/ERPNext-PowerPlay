namespace ERPNext_PowerPlay
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            spin_TimerValue = new DevExpress.XtraEditors.SpinEdit();
            chkLock = new CheckBox();
            chkAutoStartPrinting = new CheckBox();
            chkAutoLogin = new CheckBox();
            txtPASS = new DevExpress.XtraEditors.TextEdit();
            btnLogin = new DevExpress.XtraEditors.SimpleButton();
            txtUSER = new DevExpress.XtraEditors.TextEdit();
            txtURL = new DevExpress.XtraEditors.ButtonEdit();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            simpleSeparator1 = new DevExpress.XtraLayout.SimpleSeparator();
            simpleSeparator2 = new DevExpress.XtraLayout.SimpleSeparator();
            tabbedControlGroup1 = new DevExpress.XtraLayout.TabbedControlGroup();
            layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            simpleSeparator3 = new DevExpress.XtraLayout.SimpleSeparator();
            ((System.ComponentModel.ISupportInitialize)layoutControl1).BeginInit();
            layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spin_TimerValue.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtPASS.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtUSER.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtURL.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)simpleSeparator1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)simpleSeparator2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tabbedControlGroup1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)simpleSeparator3).BeginInit();
            SuspendLayout();
            // 
            // layoutControl1
            // 
            layoutControl1.Controls.Add(spin_TimerValue);
            layoutControl1.Controls.Add(chkLock);
            layoutControl1.Controls.Add(chkAutoStartPrinting);
            layoutControl1.Controls.Add(chkAutoLogin);
            layoutControl1.Controls.Add(txtPASS);
            layoutControl1.Controls.Add(btnLogin);
            layoutControl1.Controls.Add(txtUSER);
            layoutControl1.Controls.Add(txtURL);
            layoutControl1.Dock = DockStyle.Fill;
            layoutControl1.Location = new Point(0, 0);
            layoutControl1.Name = "layoutControl1";
            layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new Rectangle(-1438, 85, 650, 400);
            layoutControl1.Root = Root;
            layoutControl1.Size = new Size(510, 235);
            layoutControl1.TabIndex = 0;
            layoutControl1.Text = "layoutControl1";
            // 
            // spin_TimerValue
            // 
            spin_TimerValue.EditValue = new decimal(new int[] { 30, 0, 0, 0 });
            spin_TimerValue.Location = new Point(157, 122);
            spin_TimerValue.Name = "spin_TimerValue";
            spin_TimerValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            spin_TimerValue.Properties.MaxValue = new decimal(new int[] { 10800, 0, 0, 0 });
            spin_TimerValue.Properties.MinValue = new decimal(new int[] { 30, 0, 0, 0 });
            spin_TimerValue.Size = new Size(329, 22);
            spin_TimerValue.StyleController = layoutControl1;
            spin_TimerValue.TabIndex = 8;
            // 
            // chkLock
            // 
            chkLock.Location = new Point(24, 98);
            chkLock.Name = "chkLock";
            chkLock.Size = new Size(462, 20);
            chkLock.TabIndex = 7;
            chkLock.Text = "Lock Print and Report Configurations";
            chkLock.UseVisualStyleBackColor = true;
            // 
            // chkAutoStartPrinting
            // 
            chkAutoStartPrinting.Location = new Point(24, 74);
            chkAutoStartPrinting.Name = "chkAutoStartPrinting";
            chkAutoStartPrinting.Size = new Size(462, 20);
            chkAutoStartPrinting.TabIndex = 6;
            chkAutoStartPrinting.Text = "Auto Start Printing";
            chkAutoStartPrinting.UseVisualStyleBackColor = true;
            // 
            // chkAutoLogin
            // 
            chkAutoLogin.Location = new Point(24, 50);
            chkAutoLogin.Name = "chkAutoLogin";
            chkAutoLogin.Size = new Size(462, 20);
            chkAutoLogin.TabIndex = 5;
            chkAutoLogin.Text = "Auto Login";
            chkAutoLogin.UseVisualStyleBackColor = true;
            // 
            // txtPASS
            // 
            txtPASS.Location = new Point(157, 102);
            txtPASS.Name = "txtPASS";
            txtPASS.Properties.UseSystemPasswordChar = true;
            txtPASS.Size = new Size(329, 22);
            txtPASS.StyleController = layoutControl1;
            txtPASS.TabIndex = 3;
            // 
            // btnLogin
            // 
            btnLogin.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnLogin.ImageOptions.SvgImage");
            btnLogin.Location = new Point(24, 129);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(462, 36);
            btnLogin.StyleController = layoutControl1;
            btnLogin.TabIndex = 4;
            btnLogin.Text = "&Login";
            btnLogin.ToolTip = "Login & Save Settings";
            btnLogin.Click += btnLogin_Click;
            // 
            // txtUSER
            // 
            txtUSER.Location = new Point(157, 76);
            txtUSER.Name = "txtUSER";
            txtUSER.Size = new Size(329, 22);
            txtUSER.StyleController = layoutControl1;
            txtUSER.TabIndex = 2;
            // 
            // txtURL
            // 
            txtURL.Location = new Point(157, 50);
            txtURL.Name = "txtURL";
            txtURL.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton() });
            txtURL.Size = new Size(329, 22);
            txtURL.StyleController = layoutControl1;
            txtURL.TabIndex = 0;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { simpleSeparator1, simpleSeparator2, tabbedControlGroup1 });
            Root.Name = "Root";
            Root.Size = new Size(510, 235);
            Root.TextVisible = false;
            // 
            // simpleSeparator1
            // 
            simpleSeparator1.Location = new Point(0, 1);
            simpleSeparator1.Name = "simpleSeparator1";
            simpleSeparator1.Size = new Size(490, 1);
            // 
            // simpleSeparator2
            // 
            simpleSeparator2.Location = new Point(0, 0);
            simpleSeparator2.Name = "simpleSeparator2";
            simpleSeparator2.Size = new Size(490, 1);
            // 
            // tabbedControlGroup1
            // 
            tabbedControlGroup1.Location = new Point(0, 2);
            tabbedControlGroup1.Name = "tabbedControlGroup1";
            tabbedControlGroup1.SelectedTabPage = layoutControlGroup2;
            tabbedControlGroup1.Size = new Size(490, 213);
            tabbedControlGroup1.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlGroup2, layoutControlGroup1 });
            // 
            // layoutControlGroup1
            // 
            layoutControlGroup1.CaptionImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("layoutControlGroup1.CaptionImageOptions.SvgImage");
            layoutControlGroup1.CaptionImageOptions.SvgImageSize = new Size(16, 16);
            layoutControlGroup1.CustomizationFormText = "Settings";
            layoutControlGroup1.ExpandButtonVisible = true;
            layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem5, layoutControlItem6, layoutControlItem7, layoutControlItem8 });
            layoutControlGroup1.Location = new Point(0, 0);
            layoutControlGroup1.Name = "layoutControlGroup1";
            layoutControlGroup1.Size = new Size(466, 165);
            layoutControlGroup1.Text = "Settings";
            // 
            // layoutControlItem5
            // 
            layoutControlItem5.Control = chkAutoLogin;
            layoutControlItem5.Location = new Point(0, 0);
            layoutControlItem5.Name = "layoutControlItem5";
            layoutControlItem5.Size = new Size(466, 24);
            layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            layoutControlItem6.Control = chkAutoStartPrinting;
            layoutControlItem6.Location = new Point(0, 24);
            layoutControlItem6.Name = "layoutControlItem6";
            layoutControlItem6.Size = new Size(466, 24);
            layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            layoutControlItem7.Control = chkLock;
            layoutControlItem7.Location = new Point(0, 48);
            layoutControlItem7.Name = "layoutControlItem7";
            layoutControlItem7.Size = new Size(466, 24);
            layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            layoutControlItem8.Control = spin_TimerValue;
            layoutControlItem8.Location = new Point(0, 72);
            layoutControlItem8.Name = "layoutControlItem8";
            layoutControlItem8.Size = new Size(466, 93);
            layoutControlItem8.Text = "Timer Internal (seconds)";
            layoutControlItem8.TextSize = new Size(121, 13);
            // 
            // layoutControlGroup2
            // 
            layoutControlGroup2.CaptionImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("layoutControlGroup2.CaptionImageOptions.SvgImage");
            layoutControlGroup2.CaptionImageOptions.SvgImageSize = new Size(16, 16);
            layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem1, layoutControlItem2, layoutControlItem4, layoutControlItem3, simpleSeparator3 });
            layoutControlGroup2.Location = new Point(0, 0);
            layoutControlGroup2.Name = "layoutControlGroup2";
            layoutControlGroup2.Size = new Size(466, 165);
            layoutControlGroup2.Text = "Login";
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = txtURL;
            layoutControlItem1.Location = new Point(0, 0);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new Size(466, 26);
            layoutControlItem1.Text = "URL";
            layoutControlItem1.TextSize = new Size(121, 13);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = txtUSER;
            layoutControlItem2.Location = new Point(0, 26);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new Size(466, 26);
            layoutControlItem2.Text = "Username";
            layoutControlItem2.TextSize = new Size(121, 13);
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = txtPASS;
            layoutControlItem4.Location = new Point(0, 52);
            layoutControlItem4.Name = "layoutControlItem4";
            layoutControlItem4.Size = new Size(466, 26);
            layoutControlItem4.Text = "Password";
            layoutControlItem4.TextSize = new Size(121, 13);
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = btnLogin;
            layoutControlItem3.Location = new Point(0, 79);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new Size(466, 86);
            layoutControlItem3.TextVisible = false;
            // 
            // simpleSeparator3
            // 
            simpleSeparator3.Location = new Point(0, 78);
            simpleSeparator3.Name = "simpleSeparator3";
            simpleSeparator3.Size = new Size(466, 1);
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(510, 235);
            Controls.Add(layoutControl1);
            IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("frmLogin.IconOptions.SvgImage");
            Name = "frmLogin";
            Text = "Login";
            ((System.ComponentModel.ISupportInitialize)layoutControl1).EndInit();
            layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)spin_TimerValue.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtPASS.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtUSER.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtURL.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)simpleSeparator1).EndInit();
            ((System.ComponentModel.ISupportInitialize)simpleSeparator2).EndInit();
            ((System.ComponentModel.ISupportInitialize)tabbedControlGroup1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem8).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)simpleSeparator3).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit txtPASS;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.TextEdit txtUSER;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.ButtonEdit txtURL;
        private CheckBox chkAutoLogin;
        private CheckBox chkAutoStartPrinting;
        private CheckBox chkLock;
        private DevExpress.XtraLayout.SimpleSeparator simpleSeparator1;
        private DevExpress.XtraLayout.SimpleSeparator simpleSeparator2;
        private DevExpress.XtraEditors.SpinEdit spin_TimerValue;
        private DevExpress.XtraLayout.SimpleSeparator simpleSeparator3;
        private DevExpress.XtraLayout.TabbedControlGroup tabbedControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
    }
}