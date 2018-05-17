namespace DrectSoft.Emr.QCTimeLimit.QCControlUse
{
    partial class UCTimeQcInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCTimeQcInfo));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.btnZhanKai = new DevExpress.XtraEditors.SimpleButton();
            this.btnShouSuo = new DevExpress.XtraEditors.SimpleButton();
            this.gridControlTimeLimitInfo = new DevExpress.XtraGrid.GridControl();
            this.gridViewTimeLimit = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colmessage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colnoofinpat = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTimeLimitInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTimeLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnRefresh);
            this.panelControl1.Controls.Add(this.btnZhanKai);
            this.panelControl1.Controls.Add(this.btnShouSuo);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(252, 31);
            this.panelControl1.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(169, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新(&R)";
            // 
            // btnZhanKai
            // 
            this.btnZhanKai.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnZhanKai.Image = ((System.Drawing.Image)(resources.GetObject("btnZhanKai.Image")));
            this.btnZhanKai.Location = new System.Drawing.Point(86, 3);
            this.btnZhanKai.Name = "btnZhanKai";
            this.btnZhanKai.Size = new System.Drawing.Size(80, 23);
            this.btnZhanKai.TabIndex = 1;
            this.btnZhanKai.Text = "展开(&U)";
            // 
            // btnShouSuo
            // 
            this.btnShouSuo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnShouSuo.Image = ((System.Drawing.Image)(resources.GetObject("btnShouSuo.Image")));
            this.btnShouSuo.Location = new System.Drawing.Point(3, 3);
            this.btnShouSuo.Name = "btnShouSuo";
            this.btnShouSuo.Size = new System.Drawing.Size(80, 23);
            this.btnShouSuo.TabIndex = 0;
            this.btnShouSuo.Text = "收缩(&V)";
            // 
            // gridControlTimeLimitInfo
            // 
            this.gridControlTimeLimitInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlTimeLimitInfo.Location = new System.Drawing.Point(0, 31);
            this.gridControlTimeLimitInfo.MainView = this.gridViewTimeLimit;
            this.gridControlTimeLimitInfo.Name = "gridControlTimeLimitInfo";
            this.gridControlTimeLimitInfo.Size = new System.Drawing.Size(252, 371);
            this.gridControlTimeLimitInfo.TabIndex = 1;
            this.gridControlTimeLimitInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTimeLimit});
            // 
            // gridViewTimeLimit
            // 
            this.gridViewTimeLimit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gridViewTimeLimit.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colmessage,
            this.coltime,
            this.colname,
            this.colnoofinpat});
            this.gridViewTimeLimit.GridControl = this.gridControlTimeLimitInfo;
            this.gridViewTimeLimit.Name = "gridViewTimeLimit";
            this.gridViewTimeLimit.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridViewTimeLimit.OptionsBehavior.Editable = false;
            this.gridViewTimeLimit.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewTimeLimit.OptionsCustomization.AllowFilter = false;
            this.gridViewTimeLimit.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewTimeLimit.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewTimeLimit.OptionsMenu.EnableColumnMenu = false;
            this.gridViewTimeLimit.OptionsMenu.EnableFooterMenu = false;
            this.gridViewTimeLimit.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewTimeLimit.OptionsView.ShowGroupPanel = false;
            this.gridViewTimeLimit.OptionsView.ShowIndicator = false;
            // 
            // colmessage
            // 
            this.colmessage.Caption = "消息";
            this.colmessage.FieldName = "message";
            this.colmessage.Name = "colmessage";
            this.colmessage.Visible = true;
            this.colmessage.VisibleIndex = 0;
            // 
            // coltime
            // 
            this.coltime.Caption = "提醒时间";
            this.coltime.FieldName = "time";
            this.coltime.Name = "coltime";
            this.coltime.Visible = true;
            this.coltime.VisibleIndex = 1;
            // 
            // colname
            // 
            this.colname.Caption = "病人姓名";
            this.colname.FieldName = "name";
            this.colname.Name = "colname";
            this.colname.Visible = true;
            this.colname.VisibleIndex = 2;
            // 
            // colnoofinpat
            // 
            this.colnoofinpat.Caption = "病案号";
            this.colnoofinpat.FieldName = "noofinpat";
            this.colnoofinpat.Name = "colnoofinpat";
            this.colnoofinpat.Visible = true;
            this.colnoofinpat.VisibleIndex = 3;
            // 
            // UCTimeQcInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlTimeLimitInfo);
            this.Controls.Add(this.panelControl1);
            this.Name = "UCTimeQcInfo";
            this.Size = new System.Drawing.Size(252, 402);
            this.Load += new System.EventHandler(this.UCTimeQcInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTimeLimitInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTimeLimit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnShouSuo;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraEditors.SimpleButton btnZhanKai;
        private DevExpress.XtraGrid.GridControl gridControlTimeLimitInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTimeLimit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn colmessage;
        private DevExpress.XtraGrid.Columns.GridColumn coltime;
        private DevExpress.XtraGrid.Columns.GridColumn colname;
        private DevExpress.XtraGrid.Columns.GridColumn colnoofinpat;
    }
}
