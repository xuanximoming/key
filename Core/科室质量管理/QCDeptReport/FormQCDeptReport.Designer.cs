namespace DrectSoft.Core.QCDeptReport
{
    partial class FormQCDeptReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQCDeptReport));
            this.xtraTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.TabPageFail = new DevExpress.XtraTab.XtraTabPage();
            this.panelControlFail = new DevExpress.XtraEditors.PanelControl();
            this.TabPageFiling = new DevExpress.XtraTab.XtraTabPage();
            this.panelControlFiling = new DevExpress.XtraEditors.PanelControl();
            this.TabPageDeductPoint = new DevExpress.XtraTab.XtraTabPage();
            this.panelControlDeductPoint = new DevExpress.XtraEditors.PanelControl();
            this.TabPageDeductPointInfo = new DevExpress.XtraTab.XtraTabPage();
            this.panelControlDeductPointInfo = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).BeginInit();
            this.xtraTabControl.SuspendLayout();
            this.TabPageFail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlFail)).BeginInit();
            this.TabPageFiling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlFiling)).BeginInit();
            this.TabPageDeductPoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlDeductPoint)).BeginInit();
            this.TabPageDeductPointInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlDeductPointInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl
            // 
            this.xtraTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl.Name = "xtraTabControl";
            this.xtraTabControl.SelectedTabPage = this.TabPageFail;
            this.xtraTabControl.Size = new System.Drawing.Size(955, 628);
            this.xtraTabControl.TabIndex = 0;
            this.xtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.TabPageFail,
            this.TabPageFiling,
            this.TabPageDeductPoint,
            this.TabPageDeductPointInfo});
            this.xtraTabControl.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl_SelectedPageChanged);
            // 
            // TabPageFail
            // 
            this.TabPageFail.Controls.Add(this.panelControlFail);
            this.TabPageFail.Name = "TabPageFail";
            this.TabPageFail.Size = new System.Drawing.Size(949, 599);
            this.TabPageFail.Text = "出院未归档患者信息";
            // 
            // panelControlFail
            // 
            this.panelControlFail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlFail.Location = new System.Drawing.Point(0, 0);
            this.panelControlFail.Name = "panelControlFail";
            this.panelControlFail.Size = new System.Drawing.Size(949, 599);
            this.panelControlFail.TabIndex = 0;
            // 
            // TabPageFiling
            // 
            this.TabPageFiling.Controls.Add(this.panelControlFiling);
            this.TabPageFiling.Name = "TabPageFiling";
            this.TabPageFiling.Size = new System.Drawing.Size(949, 599);
            this.TabPageFiling.Text = "出院已归档病人信息";
            // 
            // panelControlFiling
            // 
            this.panelControlFiling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlFiling.Location = new System.Drawing.Point(0, 0);
            this.panelControlFiling.Name = "panelControlFiling";
            this.panelControlFiling.Size = new System.Drawing.Size(949, 599);
            this.panelControlFiling.TabIndex = 0;
            // 
            // TabPageDeductPoint
            // 
            this.TabPageDeductPoint.Controls.Add(this.panelControlDeductPoint);
            this.TabPageDeductPoint.Name = "TabPageDeductPoint";
            this.TabPageDeductPoint.Size = new System.Drawing.Size(949, 599);
            this.TabPageDeductPoint.Text = "科室病历失分点统计";
            // 
            // panelControlDeductPoint
            // 
            this.panelControlDeductPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlDeductPoint.Location = new System.Drawing.Point(0, 0);
            this.panelControlDeductPoint.Name = "panelControlDeductPoint";
            this.panelControlDeductPoint.Size = new System.Drawing.Size(949, 599);
            this.panelControlDeductPoint.TabIndex = 0;
            // 
            // TabPageDeductPointInfo
            // 
            this.TabPageDeductPointInfo.Controls.Add(this.panelControlDeductPointInfo);
            this.TabPageDeductPointInfo.Name = "TabPageDeductPointInfo";
            this.TabPageDeductPointInfo.Size = new System.Drawing.Size(949, 599);
            this.TabPageDeductPointInfo.Text = "科室病历失分点明细";
            // 
            // panelControlDeductPointInfo
            // 
            this.panelControlDeductPointInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlDeductPointInfo.Location = new System.Drawing.Point(0, 0);
            this.panelControlDeductPointInfo.Name = "panelControlDeductPointInfo";
            this.panelControlDeductPointInfo.Size = new System.Drawing.Size(949, 599);
            this.panelControlDeductPointInfo.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(955, 628);
            this.panelControl1.TabIndex = 1;
            // 
            // FormQCDeptReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 628);
            this.Controls.Add(this.xtraTabControl);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormQCDeptReport";
            this.ShowInTaskbar = false;
            this.Text = "科室质控管理";
            this.Load += new System.EventHandler(this.FormQCDeptReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).EndInit();
            this.xtraTabControl.ResumeLayout(false);
            this.TabPageFail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlFail)).EndInit();
            this.TabPageFiling.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlFiling)).EndInit();
            this.TabPageDeductPoint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlDeductPoint)).EndInit();
            this.TabPageDeductPointInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlDeductPointInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl;
        private DevExpress.XtraTab.XtraTabPage TabPageFail;
        private DevExpress.XtraTab.XtraTabPage TabPageFiling;
        private DevExpress.XtraTab.XtraTabPage TabPageDeductPoint;
        private DevExpress.XtraTab.XtraTabPage TabPageDeductPointInfo;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControlFail;
        private DevExpress.XtraEditors.PanelControl panelControlFiling;
        private DevExpress.XtraEditors.PanelControl panelControlDeductPoint;
        private DevExpress.XtraEditors.PanelControl panelControlDeductPointInfo;
    }
}