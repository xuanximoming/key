namespace DrectSoft.Core.ZymosisReport.Print
{
    partial class PrintFormExt
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
            this.xPrint = new XDesigner.Report.XPrintControlExt();
            this.SuspendLayout();
            // 
            // xPrint
            // 
            this.xPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xPrint.LabelEditHotKeysString = "Enter, F2";
            this.xPrint.Location = new System.Drawing.Point(0, 0);
            this.xPrint.Name = "xPrint";
            this.xPrint.RefreshButtonVisible = true;
            this.xPrint.ShowVersionInfo = true;
            this.xPrint.Size = new System.Drawing.Size(816, 507);
            this.xPrint.TabIndex = 0;
            this.xPrint.ToolbarVisible = true;
            this.xPrint.Load += new System.EventHandler(this.xPrint_Load);
            // 
            // PrintFormExt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 507);
            this.Controls.Add(this.xPrint);
            this.Name = "PrintFormExt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印预览";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private XDesigner.Report.XPrintControlExt xPrint;
    }
}