namespace ERPNext_PowerPlay.Forms
{
    partial class frmLogs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogs));
            gridLog1 = new Serilog.Sinks.WinForms.Core.GridLog();
            SuspendLayout();
            // 
            // gridLog1
            // 
            gridLog1.Dock = DockStyle.Fill;
            gridLog1.Location = new Point(0, 0);
            gridLog1.Margin = new Padding(4, 3, 4, 3);
            gridLog1.Name = "gridLog1";
            gridLog1.Size = new Size(570, 262);
            gridLog1.TabIndex = 0;
            // 
            // frmLogs
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(570, 262);
            Controls.Add(gridLog1);
            IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("frmLogs.IconOptions.SvgImage");
            Name = "frmLogs";
            Text = "Logs";
            ResumeLayout(false);
        }

        #endregion

        private Serilog.Sinks.WinForms.Core.GridLog gridLog1;
    }
}