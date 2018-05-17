namespace DrectSoft.Core.Consultation
{
    partial class FormApproveForMultiply
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApproveForMultiply));
            this.groupControlApprove = new DevExpress.XtraEditors.GroupControl();
            this.simpleButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonReject = new DevExpress.XtraEditors.SimpleButton();
            this.memoEditSuggestion = new DevExpress.XtraEditors.MemoEdit();
            this.ApplyForMultiply = new DrectSoft.Core.Consultation.UCConsultationApplyForMultiply();
            this.xtraTabControlApprove = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.xtraTabPageEmrContent = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlApprove)).BeginInit();
            this.groupControlApprove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlApprove)).BeginInit();
            this.xtraTabControlApprove.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControlApprove
            // 
            this.groupControlApprove.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControlApprove.AppearanceCaption.Options.UseFont = true;
            this.groupControlApprove.Controls.Add(this.simpleButtonOK);
            this.groupControlApprove.Controls.Add(this.simpleButtonReject);
            this.groupControlApprove.Controls.Add(this.memoEditSuggestion);
            this.groupControlApprove.Location = new System.Drawing.Point(0, 0);
            this.groupControlApprove.Name = "groupControlApprove";
            this.groupControlApprove.Size = new System.Drawing.Size(1000, 113);
            this.groupControlApprove.TabIndex = 0;
            this.groupControlApprove.TabStop = true;
            this.groupControlApprove.Text = "审核意见 (限3000字)";
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Image = global::DrectSoft.Core.Consultation.Properties.Resources.确定;
            this.simpleButtonOK.Location = new System.Drawing.Point(799, 79);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonOK.TabIndex = 1;
            this.simpleButtonOK.Text = "通   过";
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButtonOK_Click);
            // 
            // simpleButtonReject
            // 
            this.simpleButtonReject.Image = global::DrectSoft.Core.Consultation.Properties.Resources.否决;
            this.simpleButtonReject.Location = new System.Drawing.Point(885, 79);
            this.simpleButtonReject.Name = "simpleButtonReject";
            this.simpleButtonReject.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonReject.TabIndex = 2;
            this.simpleButtonReject.Text = "否   决";
            this.simpleButtonReject.Click += new System.EventHandler(this.simpleButtonReject_Click);
            // 
            // memoEditSuggestion
            // 
            this.memoEditSuggestion.Location = new System.Drawing.Point(19, 28);
            this.memoEditSuggestion.Name = "memoEditSuggestion";
            this.memoEditSuggestion.Size = new System.Drawing.Size(946, 45);
            this.memoEditSuggestion.TabIndex = 0;
            this.memoEditSuggestion.ToolTip = "审核意见 (限3000字)";
            this.memoEditSuggestion.UseOptimizedRendering = true;
            // 
            // ApplyForMultiply
            // 
            this.ApplyForMultiply.Location = new System.Drawing.Point(0, 112);
            this.ApplyForMultiply.Name = "ApplyForMultiply";
            this.ApplyForMultiply.Size = new System.Drawing.Size(1000, 690);
            this.ApplyForMultiply.TabIndex = 1;
            // 
            // xtraTabControlApprove
            // 
            this.xtraTabControlApprove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControlApprove.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControlApprove.Name = "xtraTabControlApprove";
            this.xtraTabControlApprove.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControlApprove.Size = new System.Drawing.Size(1024, 702);
            this.xtraTabControlApprove.TabIndex = 0;
            this.xtraTabControlApprove.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPageEmrContent});
            this.xtraTabControlApprove.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControlApprove_SelectedPageChanged);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.panel1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1018, 673);
            this.xtraTabPage1.Text = "审核信息";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.panel1.Controls.Add(this.ApplyForMultiply);
            this.panel1.Controls.Add(this.groupControlApprove);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1018, 673);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // xtraTabPageEmrContent
            // 
            this.xtraTabPageEmrContent.Name = "xtraTabPageEmrContent";
            this.xtraTabPageEmrContent.Size = new System.Drawing.Size(1018, 673);
            this.xtraTabPageEmrContent.Text = "病历信息";
            // 
            // FormApproveForMultiply
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 702);
            this.Controls.Add(this.xtraTabControlApprove);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormApproveForMultiply";
            this.Text = "会诊审核";
            this.Load += new System.EventHandler(this.FormApproveForMultiply_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlApprove)).EndInit();
            this.groupControlApprove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlApprove)).EndInit();
            this.xtraTabControlApprove.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UCConsultationApplyForMultiply ApplyForMultiply;
        private DevExpress.XtraEditors.GroupControl groupControlApprove;
        private DevExpress.XtraEditors.SimpleButton simpleButtonOK;
        private DevExpress.XtraEditors.SimpleButton simpleButtonReject;
        private DevExpress.XtraEditors.MemoEdit memoEditSuggestion;
        private DevExpress.XtraTab.XtraTabControl xtraTabControlApprove;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageEmrContent;
        private System.Windows.Forms.Panel panel1;
    }
}