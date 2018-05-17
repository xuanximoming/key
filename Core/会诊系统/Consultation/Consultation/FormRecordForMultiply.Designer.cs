namespace DrectSoft.Core.Consultation
{
    partial class FormRecordForMultiply
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecordForMultiply));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageRecord = new DevExpress.XtraTab.XtraTabPage();
            this.panelRecord = new DevExpress.XtraEditors.PanelControl();
            this.xtraTabPageEmrContent = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPageRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelRecord)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPageRecord;
            this.xtraTabControl1.Size = new System.Drawing.Size(1024, 702);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageRecord,
            this.xtraTabPageEmrContent});
            this.xtraTabControl1.TabStop = false;
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // xtraTabPageRecord
            // 
            this.xtraTabPageRecord.AutoScroll = true;
            this.xtraTabPageRecord.Controls.Add(this.panelRecord);
            this.xtraTabPageRecord.Name = "xtraTabPageRecord";
            this.xtraTabPageRecord.Size = new System.Drawing.Size(1018, 673);
            this.xtraTabPageRecord.Text = "会诊信息";
            // 
            // panelRecord
            // 
            this.panelRecord.Location = new System.Drawing.Point(398, 28);
            this.panelRecord.Name = "panelRecord";
            this.panelRecord.Size = new System.Drawing.Size(10, 10);
            this.panelRecord.TabIndex = 0;
            this.panelRecord.Visible = false;
            // 
            // xtraTabPageEmrContent
            // 
            this.xtraTabPageEmrContent.Name = "xtraTabPageEmrContent";
            this.xtraTabPageEmrContent.Size = new System.Drawing.Size(1018, 673);
            this.xtraTabPageEmrContent.Text = "病历内容";
            // 
            // FormRecordForMultiply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1024, 702);
            this.Controls.Add(this.xtraTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRecordForMultiply";
            this.Text = "会诊记录单";
            this.Load += new System.EventHandler(this.FormRecordForMultiply_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPageRecord.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelRecord)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageRecord;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageEmrContent;
        private DevExpress.XtraEditors.PanelControl panelRecord;


    }
}
