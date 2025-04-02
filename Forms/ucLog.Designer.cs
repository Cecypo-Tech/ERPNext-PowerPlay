namespace ERPNext_PowerPlay.Forms
{
    partial class ucLog
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            gridLog1 = new Serilog.Sinks.WinForms.Core.GridLog();
            SuspendLayout();
            // 
            // gridLog1
            // 
            gridLog1.Dock = DockStyle.Fill;
            gridLog1.Font = new Font("Arial Narrow", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            gridLog1.Location = new Point(0, 0);
            gridLog1.Margin = new Padding(3, 4, 3, 4);
            gridLog1.Name = "gridLog1";
            gridLog1.Size = new Size(441, 192);
            gridLog1.TabIndex = 0;
            // 
            // ucLog
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridLog1);
            Name = "ucLog";
            Size = new Size(441, 192);
            ResumeLayout(false);
        }

        #endregion

        private Serilog.Sinks.WinForms.Core.GridLog gridLog1;
    }
}
