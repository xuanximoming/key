namespace Consultation.NEW
{
	partial class FrmRecordConsult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRecordConsult));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtrConsultRecord = new DevExpress.XtraTab.XtraTabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ucRecordSuggestion1 = new Consultation.NEW.UCRecordSuggestion();
            this.xtraConsultApply = new DevExpress.XtraTab.XtraTabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ucConsultationApplyForMultiplyNew1 = new Consultation.NEW.UCConsultationApplyForMultiplyNew();
            this.xtraEmrInpat = new DevExpress.XtraTab.XtraTabPage();
            this.xtraRecord = new DevExpress.XtraTab.XtraTabPage();
            this.panelConsultRecord = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtrConsultRecord.SuspendLayout();
            this.panel1.SuspendLayout();
            this.xtraConsultApply.SuspendLayout();
            this.panel2.SuspendLayout();
            this.xtraRecord.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtrConsultRecord;
            this.xtraTabControl1.Size = new System.Drawing.Size(1045, 603);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtrConsultRecord,
            this.xtraConsultApply,
            this.xtraEmrInpat,
            this.xtraRecord});
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // xtrConsultRecord
            // 
            this.xtrConsultRecord.Controls.Add(this.panel1);
            this.xtrConsultRecord.Name = "xtrConsultRecord";
            this.xtrConsultRecord.Size = new System.Drawing.Size(1039, 574);
            this.xtrConsultRecord.Text = "会诊记录单";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.panel1.Controls.Add(this.ucRecordSuggestion1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1039, 574);
            this.panel1.TabIndex = 0;
            // 
            // ucRecordSuggestion1
            // 
            this.ucRecordSuggestion1.Location = new System.Drawing.Point(22, 9);
            this.ucRecordSuggestion1.Name = "ucRecordSuggestion1";
            this.ucRecordSuggestion1.Size = new System.Drawing.Size(983, 566);
            this.ucRecordSuggestion1.TabIndex = 0;
            // 
            // xtraConsultApply
            // 
            this.xtraConsultApply.Controls.Add(this.panel2);
            this.xtraConsultApply.Name = "xtraConsultApply";
            this.xtraConsultApply.Size = new System.Drawing.Size(1039, 574);
            this.xtraConsultApply.Text = "申请信息";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.panel2.Controls.Add(this.ucConsultationApplyForMultiplyNew1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1039, 574);
            this.panel2.TabIndex = 1;
            // 
            // ucConsultationApplyForMultiplyNew1
            // 
            this.ucConsultationApplyForMultiplyNew1.Location = new System.Drawing.Point(13, 1);
            this.ucConsultationApplyForMultiplyNew1.Name = "ucConsultationApplyForMultiplyNew1";
            this.ucConsultationApplyForMultiplyNew1.Size = new System.Drawing.Size(1015, 588);
            this.ucConsultationApplyForMultiplyNew1.TabIndex = 0;
            // 
            // xtraEmrInpat
            // 
            this.xtraEmrInpat.Name = "xtraEmrInpat";
            this.xtraEmrInpat.Size = new System.Drawing.Size(1039, 574);
            this.xtraEmrInpat.Text = "病历信息";
            // 
            // xtraRecord
            // 
            this.xtraRecord.Controls.Add(this.panelConsultRecord);
            this.xtraRecord.Name = "xtraRecord";
            this.xtraRecord.Size = new System.Drawing.Size(1039, 574);
            this.xtraRecord.Text = "受邀医师意见";
            // 
            // panelConsultRecord
            // 
            this.panelConsultRecord.AutoScroll = true;
            this.panelConsultRecord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.panelConsultRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConsultRecord.Location = new System.Drawing.Point(0, 0);
            this.panelConsultRecord.Name = "panelConsultRecord";
            this.panelConsultRecord.Size = new System.Drawing.Size(1039, 574);
            this.panelConsultRecord.TabIndex = 0;
            // 
            // FrmRecordConsult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 603);
            this.Controls.Add(this.xtraTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRecordConsult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "会诊记录单";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtrConsultRecord.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.xtraConsultApply.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.xtraRecord.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtrConsultRecord;
        private DevExpress.XtraTab.XtraTabPage xtraConsultApply;
        private DevExpress.XtraTab.XtraTabPage xtraEmrInpat;
        private DevExpress.XtraTab.XtraTabPage xtraRecord;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelConsultRecord;
        private global::Consultation.NEW.UCConsultationApplyForMultiplyNew ucConsultationApplyForMultiplyNew1;
        private UCRecordSuggestion ucRecordSuggestion1;
	}
}