namespace DrectSoft.Core.RedactPatientInfo
{
    partial class BasePatientInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BasePatientInfo));
            this.tabPatientInfo = new DevExpress.XtraTab.XtraTabControl();
            this.tabPageBaseInfo = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageLinkman = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageDiacrisis = new DevExpress.XtraTab.XtraTabPage();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.defaultToolTipController1 = new DevExpress.Utils.DefaultToolTipController();
            ((System.ComponentModel.ISupportInitialize)(this.tabPatientInfo)).BeginInit();
            this.tabPatientInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPatientInfo
            // 
            this.tabPatientInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPatientInfo.Location = new System.Drawing.Point(8, 8);
            this.tabPatientInfo.Name = "tabPatientInfo";
            this.tabPatientInfo.SelectedTabPage = this.tabPageBaseInfo;
            this.tabPatientInfo.Size = new System.Drawing.Size(976, 497);
            this.tabPatientInfo.TabIndex = 0;
            this.tabPatientInfo.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPageBaseInfo,
            this.tabPageLinkman,
            this.tabPageDiacrisis});
            // 
            // tabPageBaseInfo
            // 
            this.tabPageBaseInfo.Name = "tabPageBaseInfo";
            this.tabPageBaseInfo.Size = new System.Drawing.Size(970, 468);
            this.tabPageBaseInfo.Text = "基本信息";
            // 
            // tabPageLinkman
            // 
            this.tabPageLinkman.Name = "tabPageLinkman";
            this.tabPageLinkman.Size = new System.Drawing.Size(970, 468);
            this.tabPageLinkman.Text = "联系人信息";
            // 
            // tabPageDiacrisis
            // 
            this.tabPageDiacrisis.Name = "tabPageDiacrisis";
            this.tabPageDiacrisis.Size = new System.Drawing.Size(970, 468);
            this.tabPageDiacrisis.Text = "就诊信息";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(769, 513);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 27);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(888, 513);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // BasePatientInfo
            // 
            this.defaultToolTipController1.SetAllowHtmlText(this, DevExpress.Utils.DefaultBoolean.Default);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 551);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabPatientInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BasePatientInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "病人信息管理维护";
            this.Load += new System.EventHandler(this.BasePatientInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabPatientInfo)).EndInit();
            this.tabPatientInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabPatientInfo;
        private DevExpress.XtraTab.XtraTabPage tabPageBaseInfo;
        private DevExpress.XtraTab.XtraTabPage tabPageLinkman;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraTab.XtraTabPage tabPageDiacrisis;
        private DevExpress.Utils.DefaultToolTipController defaultToolTipController1;
    }
}