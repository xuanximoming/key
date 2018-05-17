namespace MedicalRecordManage.UI
{
    partial class MedicalRecordManageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MedicalRecordManageForm));
            this.tabControl = new DevExpress.XtraTab.XtraTabControl();
            this.tabPageApprove = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageApprovedList = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageWriteUpApprove = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageWriteUpApproveList = new DevExpress.XtraTab.XtraTabPage();
            this.tablePageRecord = new DevExpress.XtraTab.XtraTabPage();
            this.tablePageUnRec = new DevExpress.XtraTab.XtraTabPage();
            this.tablePageRecChecked = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageIndexCard = new DevExpress.XtraTab.XtraTabPage();
            this.tabTabIemAcrive = new DevExpress.XtraTab.XtraTabPage();
            this.panelBody = new DevExpress.XtraEditors.PanelControl();
            this.tabTabOrders = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelBody)).BeginInit();
            this.panelBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabPage = this.tabPageApprove;
            this.tabControl.Size = new System.Drawing.Size(1016, 734);
            this.tabControl.TabIndex = 0;
            this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPageApprove,
            this.tabPageApprovedList,
            this.tabPageWriteUpApprove,
            this.tabPageWriteUpApproveList,
            this.tablePageRecord,
            this.tablePageUnRec,
            this.tablePageRecChecked,
            this.tabPageIndexCard,
            this.tabTabIemAcrive,
            this.tabTabOrders});
            this.tabControl.SelectedPageChanging += new DevExpress.XtraTab.TabPageChangingEventHandler(this.tabControl_SelectedPageChanging);
            // 
            // tabPageApprove
            // 
            this.tabPageApprove.Name = "tabPageApprove";
            this.tabPageApprove.Size = new System.Drawing.Size(1010, 705);
            this.tabPageApprove.Text = "病历借阅审核";
            // 
            // tabPageApprovedList
            // 
            this.tabPageApprovedList.Name = "tabPageApprovedList";
            this.tabPageApprovedList.Size = new System.Drawing.Size(1010, 705);
            this.tabPageApprovedList.Text = "病历借阅记录查询";
            // 
            // tabPageWriteUpApprove
            // 
            this.tabPageWriteUpApprove.Name = "tabPageWriteUpApprove";
            this.tabPageWriteUpApprove.Size = new System.Drawing.Size(1010, 705);
            this.tabPageWriteUpApprove.Text = "病历补写审核";
            // 
            // tabPageWriteUpApproveList
            // 
            this.tabPageWriteUpApproveList.Name = "tabPageWriteUpApproveList";
            this.tabPageWriteUpApproveList.Size = new System.Drawing.Size(1010, 705);
            this.tabPageWriteUpApproveList.Text = "病历补写记录查询";
            // 
            // tablePageRecord
            // 
            this.tablePageRecord.Name = "tablePageRecord";
            this.tablePageRecord.Size = new System.Drawing.Size(1010, 705);
            this.tablePageRecord.Text = "病历查询";
            // 
            // tablePageUnRec
            // 
            this.tablePageUnRec.Name = "tablePageUnRec";
            this.tablePageUnRec.Size = new System.Drawing.Size(1010, 705);
            this.tablePageUnRec.Text = "未归档的出院病人列表";
            // 
            // tablePageRecChecked
            // 
            this.tablePageRecChecked.Name = "tablePageRecChecked";
            this.tablePageRecChecked.Size = new System.Drawing.Size(1010, 705);
            this.tablePageRecChecked.Text = "已归档的病案查询";
            // 
            // tabPageIndexCard
            // 
            this.tabPageIndexCard.Name = "tabPageIndexCard";
            this.tabPageIndexCard.Size = new System.Drawing.Size(1010, 705);
            this.tabPageIndexCard.Text = "索引卡";
            // 
            // tabTabIemAcrive
            // 
            this.tabTabIemAcrive.Name = "tabTabIemAcrive";
            this.tabTabIemAcrive.Size = new System.Drawing.Size(1010, 705);
            this.tabTabIemAcrive.Text = "病案首页编辑归档";
            // 
            // panelBody
            // 
            this.panelBody.Appearance.BackColor = System.Drawing.Color.White;
            this.panelBody.Appearance.Options.UseBackColor = true;
            this.panelBody.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelBody.Controls.Add(this.tabControl);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(1016, 734);
            this.panelBody.TabIndex = 2;
            // 
            // tabTabOrders
            // 
            this.tabTabOrders.Name = "tabTabOrders";
            this.tabTabOrders.PageVisible = false;
            this.tabTabOrders.Size = new System.Drawing.Size(1010, 705);
            this.tabTabOrders.Text = "医嘱查看";
            // 
            // MedicalRecordManageForm
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 734);
            this.Controls.Add(this.panelBody);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MedicalRecordManageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "病案管理";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MedicalRecordManageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelBody)).EndInit();
            this.panelBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabControl;
        private DevExpress.XtraTab.XtraTabPage tabPageApprove;
        private DevExpress.XtraTab.XtraTabPage tabPageApprovedList;
        private DevExpress.XtraEditors.PanelControl panelBody;
        private DevExpress.XtraTab.XtraTabPage tablePageRecord;
        private DevExpress.XtraTab.XtraTabPage tablePageUnRec;
        private DevExpress.XtraTab.XtraTabPage tablePageRecChecked;
        private DevExpress.XtraTab.XtraTabPage tabPageIndexCard;
        private DevExpress.XtraTab.XtraTabPage tabTabIemAcrive;
        private DevExpress.XtraTab.XtraTabPage tabPageWriteUpApprove;
        private DevExpress.XtraTab.XtraTabPage tabPageWriteUpApproveList;
        private DevExpress.XtraTab.XtraTabPage tabTabOrders;

    }
}