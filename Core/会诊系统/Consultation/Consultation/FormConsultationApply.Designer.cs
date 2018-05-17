namespace DrectSoft.Core.Consultation
{
    partial class FormConsultationApply
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConsultationApply));
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            this.ApplyForMultiply = new DrectSoft.Core.Consultation.UCConsultationApplyForMultiply();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageApply = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.xtraTabPageEmrContent = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPageApply.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ApplyForMultiply
            // 
            this.ApplyForMultiply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ApplyForMultiply.Location = new System.Drawing.Point(2, 2);
            this.ApplyForMultiply.Name = "ApplyForMultiply";
            this.ApplyForMultiply.Size = new System.Drawing.Size(1007, 689);
            this.ApplyForMultiply.TabIndex = 0;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPageApply;
            this.xtraTabControl1.Size = new System.Drawing.Size(1017, 722);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageApply,
            this.xtraTabPageEmrContent});
            this.xtraTabControl1.TabStop = false;
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // xtraTabPageApply
            // 
            this.xtraTabPageApply.Controls.Add(this.panelControl1);
            this.xtraTabPageApply.Name = "xtraTabPageApply";
            this.xtraTabPageApply.Size = new System.Drawing.Size(1011, 693);
            this.xtraTabPageApply.Text = "会诊申请";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.ApplyForMultiply);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1011, 693);
            this.panelControl1.TabIndex = 0;
            // 
            // xtraTabPageEmrContent
            // 
            this.xtraTabPageEmrContent.Name = "xtraTabPageEmrContent";
            this.xtraTabPageEmrContent.Size = new System.Drawing.Size(1011, 693);
            this.xtraTabPageEmrContent.Text = "病历信息";
            // 
            // FormConsultationApply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 722);
            this.Controls.Add(this.xtraTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConsultationApply";
            this.Text = "会诊申请";
            this.Load += new System.EventHandler(this.FormConsultationApply_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPageApply.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1; 
        private UCConsultationApplyForMultiply ApplyForMultiply;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageApply;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageEmrContent;
        private DevExpress.XtraEditors.PanelControl panelControl1;

    }
}