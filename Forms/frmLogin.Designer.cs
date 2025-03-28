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
            txtPASS = new DevExpress.XtraEditors.TextEdit();
            btnLogin = new DevExpress.XtraEditors.SimpleButton();
            txtUSER = new DevExpress.XtraEditors.TextEdit();
            txtURL = new DevExpress.XtraEditors.ButtonEdit();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)layoutControl1).BeginInit();
            layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtPASS.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtUSER.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtURL.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).BeginInit();
            SuspendLayout();
            // 
            // layoutControl1
            // 
            layoutControl1.Controls.Add(txtPASS);
            layoutControl1.Controls.Add(btnLogin);
            layoutControl1.Controls.Add(txtUSER);
            layoutControl1.Controls.Add(txtURL);
            layoutControl1.Dock = DockStyle.Fill;
            layoutControl1.Location = new Point(0, 0);
            layoutControl1.Name = "layoutControl1";
            layoutControl1.Root = Root;
            layoutControl1.Size = new Size(434, 216);
            layoutControl1.TabIndex = 0;
            layoutControl1.Text = "layoutControl1";
            // 
            // txtPASS
            // 
            txtPASS.Location = new Point(284, 74);
            txtPASS.Name = "txtPASS";
            txtPASS.Properties.UseSystemPasswordChar = true;
            txtPASS.Size = new Size(136, 22);
            txtPASS.StyleController = layoutControl1;
            txtPASS.TabIndex = 2;
            // 
            // btnLogin
            // 
            btnLogin.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnLogin.ImageOptions.SvgImage");
            btnLogin.Location = new Point(218, 102);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(204, 36);
            btnLogin.StyleController = layoutControl1;
            btnLogin.TabIndex = 3;
            btnLogin.Text = "&Login";
            btnLogin.Click += btnLogin_Click;
            // 
            // txtUSER
            // 
            txtUSER.Location = new Point(77, 74);
            txtUSER.Name = "txtUSER";
            txtUSER.Size = new Size(136, 22);
            txtUSER.StyleController = layoutControl1;
            txtUSER.TabIndex = 0;
            // 
            // txtURL
            // 
            txtURL.Location = new Point(77, 44);
            txtURL.Name = "txtURL";
            txtURL.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton() });
            txtURL.Size = new Size(343, 22);
            txtURL.StyleController = layoutControl1;
            txtURL.TabIndex = 0;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem1, emptySpaceItem1, layoutControlItem2, layoutControlItem3, emptySpaceItem2, emptySpaceItem3, layoutControlItem4 });
            Root.Name = "Root";
            Root.Size = new Size(434, 216);
            Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = txtURL;
            layoutControlItem1.Location = new Point(0, 30);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new Size(414, 30);
            layoutControlItem1.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            layoutControlItem1.Text = "URL";
            layoutControlItem1.TextSize = new Size(51, 13);
            // 
            // emptySpaceItem1
            // 
            emptySpaceItem1.Location = new Point(0, 90);
            emptySpaceItem1.Name = "emptySpaceItem1";
            emptySpaceItem1.Size = new Size(206, 40);
            emptySpaceItem1.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = txtUSER;
            layoutControlItem2.Location = new Point(0, 60);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new Size(207, 30);
            layoutControlItem2.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            layoutControlItem2.Text = "Username";
            layoutControlItem2.TextSize = new Size(51, 13);
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = btnLogin;
            layoutControlItem3.Location = new Point(206, 90);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new Size(208, 40);
            layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            emptySpaceItem2.Location = new Point(0, 0);
            emptySpaceItem2.Name = "emptySpaceItem2";
            emptySpaceItem2.Size = new Size(414, 30);
            emptySpaceItem2.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            // 
            // emptySpaceItem3
            // 
            emptySpaceItem3.Location = new Point(0, 130);
            emptySpaceItem3.Name = "emptySpaceItem3";
            emptySpaceItem3.Size = new Size(414, 66);
            emptySpaceItem3.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = txtPASS;
            layoutControlItem4.Location = new Point(207, 60);
            layoutControlItem4.Name = "layoutControlItem4";
            layoutControlItem4.Size = new Size(207, 30);
            layoutControlItem4.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            layoutControlItem4.Text = "Password";
            layoutControlItem4.TextSize = new Size(51, 13);
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 216);
            Controls.Add(layoutControl1);
            IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("frmLogin.IconOptions.SvgImage");
            Name = "frmLogin";
            Text = "Login";
            ((System.ComponentModel.ISupportInitialize)layoutControl1).EndInit();
            layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)txtPASS.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtUSER.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtURL.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit txtPASS;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.TextEdit txtUSER;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.ButtonEdit txtURL;
    }
}