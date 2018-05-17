namespace DrectSoft.Core.QCDeptReport
{
    partial class FormDeductPointInfo
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
            this.panelControlDeductPointInfo = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlDeductPointInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlDeductPointInfo
            // 
            this.panelControlDeductPointInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlDeductPointInfo.Location = new System.Drawing.Point(0, 0);
            this.panelControlDeductPointInfo.Name = "panelControlDeductPointInfo";
            this.panelControlDeductPointInfo.Size = new System.Drawing.Size(1041, 618);
            this.panelControlDeductPointInfo.TabIndex = 1;
            // 
            // _FormDeductPointInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 618);
            this.Controls.Add(this.panelControlDeductPointInfo);
            this.Name = "_FormDeductPointInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "科室病历失分明细";
            this.Load += new System.EventHandler(this._FormDeductPointInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlDeductPointInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlDeductPointInfo;
    }
}