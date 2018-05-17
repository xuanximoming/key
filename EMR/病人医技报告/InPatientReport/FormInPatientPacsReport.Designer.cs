namespace DrectSoft.Core.InPatientReport
{
    partial class FormInPatientPacsReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInPatientPacsReport));
            this.panelControlReport = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlReport)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlReport
            // 
            this.panelControlReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlReport.Location = new System.Drawing.Point(0, 0);
            this.panelControlReport.Name = "panelControlReport";
            this.panelControlReport.Size = new System.Drawing.Size(1170, 534);
            this.panelControlReport.TabIndex = 1;
            // 
            // FormInPatientPacsReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 534);
            this.Controls.Add(this.panelControlReport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormInPatientPacsReport";
            this.Text = "FormInPatientPacsReport";
            ((System.ComponentModel.ISupportInitialize)(this.panelControlReport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlReport;
    }
}