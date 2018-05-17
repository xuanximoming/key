namespace DrectSoft.Core.RecordManage
{
    partial class RecordManageCenter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordManageCenter));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.tabPageRecordNoOnFile = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageRecordOnFile = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageApplyRecord = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageSignInRecord = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageOperRecordLog = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.tabPageRecordNoOnFile;
            this.xtraTabControl1.Size = new System.Drawing.Size(884, 562);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPageRecordNoOnFile,
            this.tabPageRecordOnFile,
            this.tabPageApplyRecord,
            this.tabPageSignInRecord,
            this.tabPageOperRecordLog});
            this.xtraTabControl1.TabStop = false;
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // tabPageRecordNoOnFile
            // 
            this.tabPageRecordNoOnFile.Name = "tabPageRecordNoOnFile";
            this.tabPageRecordNoOnFile.Size = new System.Drawing.Size(878, 533);
            this.tabPageRecordNoOnFile.Text = "未归档的出院病人列表";
            // 
            // tabPageRecordOnFile
            // 
            this.tabPageRecordOnFile.Name = "tabPageRecordOnFile";
            this.tabPageRecordOnFile.Size = new System.Drawing.Size(878, 533);
            this.tabPageRecordOnFile.Text = "已归档病案查询";
            // 
            // tabPageApplyRecord
            // 
            this.tabPageApplyRecord.Name = "tabPageApplyRecord";
            this.tabPageApplyRecord.Size = new System.Drawing.Size(878, 533);
            this.tabPageApplyRecord.Text = "病历借阅审批";
            // 
            // tabPageSignInRecord
            // 
            this.tabPageSignInRecord.Name = "tabPageSignInRecord";
            this.tabPageSignInRecord.Size = new System.Drawing.Size(878, 533);
            this.tabPageSignInRecord.Text = "病历归还签收";
            // 
            // tabPageOperRecordLog
            // 
            this.tabPageOperRecordLog.Name = "tabPageOperRecordLog";
            this.tabPageOperRecordLog.PageVisible = false;
            this.tabPageOperRecordLog.Size = new System.Drawing.Size(878, 533);
            this.tabPageOperRecordLog.Text = "用户病历操作记录";
            // 
            // RecordManageCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.xtraTabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RecordManageCenter";
            this.Text = "病案管理";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RecordManageCenter_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage tabPageRecordNoOnFile;
        private DevExpress.XtraTab.XtraTabPage tabPageRecordOnFile;
        private DevExpress.XtraTab.XtraTabPage tabPageApplyRecord;
        private DevExpress.XtraTab.XtraTabPage tabPageSignInRecord;
        private DevExpress.XtraTab.XtraTabPage tabPageOperRecordLog;
    }
}