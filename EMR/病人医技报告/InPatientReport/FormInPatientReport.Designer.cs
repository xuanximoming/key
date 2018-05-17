namespace DrectSoft.Core.InPatientReport
{
    partial class FormInPatientReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInPatientReport));
            this.panelControlReport = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlReport)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlReport
            // 
            this.panelControlReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlReport.Location = new System.Drawing.Point(0, 0);
            this.panelControlReport.Name = "panelControlReport";
            this.panelControlReport.Size = new System.Drawing.Size(1080, 717);
            this.panelControlReport.TabIndex = 0;
            // 
            // FormInPatientReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 717);
            this.Controls.Add(this.panelControlReport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormInPatientReport";
            this.ShowIcon = false;
            this.Text = "医技报告浏览";
            this.Load += new System.EventHandler(this.InPatientReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlReport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlReport;

    }
}