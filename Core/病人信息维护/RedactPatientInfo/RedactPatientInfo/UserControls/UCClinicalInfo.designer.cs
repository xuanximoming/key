namespace DrectSoft.Core.RedactPatientInfo.UserControls
{
    partial class UCClinicalInfo
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.xtraTabControlInfo = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageBasInfo = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageDiagInfo = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPagePhysical = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageLISRIS = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageOper = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlInfo)).BeginInit();
            this.xtraTabControlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControlInfo
            // 
            this.xtraTabControlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControlInfo.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControlInfo.Name = "xtraTabControlInfo";
            this.xtraTabControlInfo.SelectedTabPage = this.xtraTabPagePhysical;
            this.xtraTabControlInfo.Size = new System.Drawing.Size(846, 547);
            this.xtraTabControlInfo.TabIndex = 0;
            this.xtraTabControlInfo.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPagePhysical,
            this.xtraTabPageBasInfo,
            this.xtraTabPageDiagInfo,
            this.xtraTabPageLISRIS,
            this.xtraTabPageOper});
            // 
            // xtraTabPageBasInfo
            // 
            this.xtraTabPageBasInfo.Name = "xtraTabPageBasInfo";
            this.xtraTabPageBasInfo.Size = new System.Drawing.Size(837, 515);
            this.xtraTabPageBasInfo.Text = "基本信息";
            // 
            // xtraTabPageDiagInfo
            // 
            this.xtraTabPageDiagInfo.Name = "xtraTabPageDiagInfo";
            this.xtraTabPageDiagInfo.Size = new System.Drawing.Size(837, 515);
            this.xtraTabPageDiagInfo.Text = "病史信息";
            // 
            // xtraTabPagePhysical
            // 
            this.xtraTabPagePhysical.Name = "xtraTabPagePhysical";
            this.xtraTabPagePhysical.Size = new System.Drawing.Size(837, 515);
            this.xtraTabPagePhysical.Text = "体征信息";
            // 
            // xtraTabPageLISRIS
            // 
            this.xtraTabPageLISRIS.Name = "xtraTabPageLISRIS";
            this.xtraTabPageLISRIS.Size = new System.Drawing.Size(837, 515);
            this.xtraTabPageLISRIS.Text = "医技信息";
            // 
            // xtraTabPageOper
            // 
            this.xtraTabPageOper.Name = "xtraTabPageOper";
            this.xtraTabPageOper.Size = new System.Drawing.Size(837, 515);
            this.xtraTabPageOper.Text = "手术信息";
            // 
            // UCClinicalInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControlInfo);
            this.Name = "UCClinicalInfo";
            this.Size = new System.Drawing.Size(846, 547);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlInfo)).EndInit();
            this.xtraTabControlInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControlInfo;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageBasInfo;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageDiagInfo;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageLISRIS;
        private DevExpress.XtraTab.XtraTabPage xtraTabPagePhysical;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageOper;
    }
}
