namespace ERPNext_PowerPlay.Forms
{
    partial class frmLog
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
            gridLog1 = new Serilog.Sinks.WinForms.Core.GridLog();
            SuspendLayout();
            //
            // gridLog1
            //
            gridLog1.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLog1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            gridLog1.Location = new System.Drawing.Point(0, 0);
            gridLog1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            gridLog1.Name = "gridLog1";
            gridLog1.Size = new System.Drawing.Size(800, 250);
            gridLog1.TabIndex = 0;
            //
            // frmLog
            //
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 250);
            Controls.Add(gridLog1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "frmLog";
            Text = "Log";
            ResumeLayout(false);
        }

        #endregion

        private Serilog.Sinks.WinForms.Core.GridLog gridLog1;
    }
}
