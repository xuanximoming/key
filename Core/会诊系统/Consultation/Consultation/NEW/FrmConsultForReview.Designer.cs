using Consultation.NEW;
namespace Consultation.NEW
{
    partial class FrmConsultForReview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConsultForReview));
            this.xtraControlReview = new DevExpress.XtraTab.XtraTabControl();
            this.xtraReview = new DevExpress.XtraTab.XtraTabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ucConsultationApplyForMultiplyNew1 = new Consultation.NEW.UCConsultationApplyForMultiplyNew();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnReject = new DevExpress.XtraEditors.SimpleButton();
            this.memoEditSuggestion = new DevExpress.XtraEditors.MemoEdit();
            this.xtraEmrPat = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.xtraControlReview)).BeginInit();
            this.xtraControlReview.SuspendLayout();
            this.xtraReview.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraControlReview
            // 
            this.xtraControlReview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraControlReview.Location = new System.Drawing.Point(0, 0);
            this.xtraControlReview.Name = "xtraControlReview";
            this.xtraControlReview.SelectedTabPage = this.xtraReview;
            this.xtraControlReview.Size = new System.Drawing.Size(1061, 657);
            this.xtraControlReview.TabIndex = 0;
            this.xtraControlReview.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraReview,
            this.xtraEmrPat});
            this.xtraControlReview.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraControlReview_SelectedPageChanged);
            // 
            // xtraReview
            // 
            this.xtraReview.Controls.Add(this.panel1);
            this.xtraReview.Name = "xtraReview";
            this.xtraReview.Size = new System.Drawing.Size(1055, 628);
            this.xtraReview.Text = "审核信息";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.panel1.Controls.Add(this.ucConsultationApplyForMultiplyNew1);
            this.panel1.Controls.Add(this.groupControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1055, 628);
            this.panel1.TabIndex = 0;
            // 
            // ucConsultationApplyForMultiplyNew1
            // 
            this.ucConsultationApplyForMultiplyNew1.Location = new System.Drawing.Point(12, 119);
            this.ucConsultationApplyForMultiplyNew1.Name = "ucConsultationApplyForMultiplyNew1";
            this.ucConsultationApplyForMultiplyNew1.Size = new System.Drawing.Size(1014, 581);
            this.ucConsultationApplyForMultiplyNew1.TabIndex = 1;
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.btnOk);
            this.groupControl1.Controls.Add(this.btnReject);
            this.groupControl1.Controls.Add(this.memoEditSuggestion);
            this.groupControl1.Location = new System.Drawing.Point(12, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1014, 116);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "审核意见（限1500字）";
            // 
            // btnOk
            // 
            this.btnOk.Image = global::DrectSoft.Core.Consultation.Properties.Resources.确定;
            this.btnOk.Location = new System.Drawing.Point(804, 81);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 27);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "通   过";
            // 
            // btnReject
            // 
            this.btnReject.Image = global::DrectSoft.Core.Consultation.Properties.Resources.否决;
            this.btnReject.Location = new System.Drawing.Point(899, 81);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(80, 27);
            this.btnReject.TabIndex = 3;
            this.btnReject.Text = "否   决";
            // 
            // memoEditSuggestion
            // 
            this.memoEditSuggestion.Location = new System.Drawing.Point(15, 30);
            this.memoEditSuggestion.Name = "memoEditSuggestion";
            this.memoEditSuggestion.Properties.MaxLength = 1500;
            this.memoEditSuggestion.Size = new System.Drawing.Size(964, 45);
            this.memoEditSuggestion.TabIndex = 1;
            this.memoEditSuggestion.ToolTip = "审核意见 (限3000字)";
            this.memoEditSuggestion.UseOptimizedRendering = true;
            // 
            // xtraEmrPat
            // 
            this.xtraEmrPat.Name = "xtraEmrPat";
            this.xtraEmrPat.Size = new System.Drawing.Size(1055, 628);
            this.xtraEmrPat.Text = "病历信息";
            // 
            // FrmConsultForReview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 657);
            this.Controls.Add(this.xtraControlReview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConsultForReview";
            this.Text = "会诊审核";
            this.Activated += new System.EventHandler(this.FrmConsultForReview_Activated);
            this.Load += new System.EventHandler(this.FrmConsultForReview_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraControlReview)).EndInit();
            this.xtraControlReview.ResumeLayout(false);
            this.xtraReview.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraControlReview;
        private DevExpress.XtraTab.XtraTabPage xtraReview;
        private DevExpress.XtraTab.XtraTabPage xtraEmrPat;
        private System.Windows.Forms.Panel panel1;
        private UCConsultationApplyForMultiplyNew ucConsultationApplyForMultiplyNew1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnReject;
        private DevExpress.XtraEditors.MemoEdit memoEditSuggestion;
    }
}