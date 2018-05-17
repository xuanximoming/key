namespace Consultation.NEW
{
    partial class FormApplyForMultiply
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApplyForMultiply));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraForReview = new DevExpress.XtraTab.XtraTabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupControlReview = new DevExpress.XtraEditors.GroupControl();
            this.memoEditSuggestion = new DevExpress.XtraEditors.MemoEdit();
            this.ucConsultationApplyForMultiplyNew1 = new Consultation.NEW.UCConsultationApplyForMultiplyNew();
            this.xtraEmrInfo = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraForReview.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlReview)).BeginInit();
            this.groupControlReview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraForReview;
            this.xtraTabControl1.Size = new System.Drawing.Size(1061, 614);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraForReview,
            this.xtraEmrInfo});
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // xtraForReview
            // 
            this.xtraForReview.Appearance.PageClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.xtraForReview.Appearance.PageClient.Options.UseBackColor = true;
            this.xtraForReview.AutoScroll = true;
            this.xtraForReview.Controls.Add(this.panel1);
            this.xtraForReview.Name = "xtraForReview";
            this.xtraForReview.Size = new System.Drawing.Size(1055, 585);
            this.xtraForReview.Text = "申请信息";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.panel1.Controls.Add(this.groupControlReview);
            this.panel1.Controls.Add(this.ucConsultationApplyForMultiplyNew1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1055, 585);
            this.panel1.TabIndex = 2;
            // 
            // groupControlReview
            // 
            this.groupControlReview.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControlReview.AppearanceCaption.Options.UseFont = true;
            this.groupControlReview.Controls.Add(this.memoEditSuggestion);
            this.groupControlReview.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControlReview.Location = new System.Drawing.Point(0, 578);
            this.groupControlReview.Name = "groupControlReview";
            this.groupControlReview.Size = new System.Drawing.Size(1038, 100);
            this.groupControlReview.TabIndex = 3;
            this.groupControlReview.Text = "审核意见";
            // 
            // memoEditSuggestion
            // 
            this.memoEditSuggestion.Location = new System.Drawing.Point(16, 35);
            this.memoEditSuggestion.Name = "memoEditSuggestion";
            this.memoEditSuggestion.Properties.ReadOnly = true;
            this.memoEditSuggestion.Size = new System.Drawing.Size(997, 50);
            this.memoEditSuggestion.TabIndex = 1;
            this.memoEditSuggestion.UseOptimizedRendering = true;
            // 
            // ucConsultationApplyForMultiplyNew1
            // 
            this.ucConsultationApplyForMultiplyNew1.Location = new System.Drawing.Point(19, 3);
            this.ucConsultationApplyForMultiplyNew1.Name = "ucConsultationApplyForMultiplyNew1";
            this.ucConsultationApplyForMultiplyNew1.Size = new System.Drawing.Size(1015, 575);
            this.ucConsultationApplyForMultiplyNew1.TabIndex = 0;
            // 
            // xtraEmrInfo
            // 
            this.xtraEmrInfo.Name = "xtraEmrInfo";
            this.xtraEmrInfo.Size = new System.Drawing.Size(1055, 585);
            this.xtraEmrInfo.Text = "病历信息";
            // 
            // FormApplyForMultiply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 614);
            this.Controls.Add(this.xtraTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormApplyForMultiply";
            this.Text = "会诊申请";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraForReview.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlReview)).EndInit();
            this.groupControlReview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraForReview;
        private DevExpress.XtraTab.XtraTabPage xtraEmrInfo;
        private UCConsultationApplyForMultiplyNew ucConsultationApplyForMultiplyNew1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.GroupControl groupControlReview;
        private DevExpress.XtraEditors.MemoEdit memoEditSuggestion;
    }
}