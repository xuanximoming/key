namespace DrectSoft.Core.OwnBedInfo
{
    partial class UCConsultationForDocCenter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCConsultationForDocCenter));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.panelControl6 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonState = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonConsultationRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.gridControlConsultation = new DevExpress.XtraGrid.GridControl();
            this.gridViewConsultation = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnInpatientName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnConsultDateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnStateID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnNoofFristPat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnConsultapplysn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItemOpen = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDeleteApplySave = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemCallBack = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenuConsultation = new DevExpress.XtraBars.PopupMenu();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).BeginInit();
            this.panelControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlConsultation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewConsultation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuConsultation)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl6
            // 
            this.panelControl6.Controls.Add(this.simpleButtonState);
            this.panelControl6.Controls.Add(this.simpleButtonConsultationRefresh);
            this.panelControl6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl6.Location = new System.Drawing.Point(0, 0);
            this.panelControl6.Name = "panelControl6";
            this.panelControl6.Size = new System.Drawing.Size(281, 31);
            this.panelControl6.TabIndex = 21;
            // 
            // simpleButtonState
            // 
            this.simpleButtonState.Location = new System.Drawing.Point(141, 4);
            this.simpleButtonState.Name = "simpleButtonState";
            this.simpleButtonState.Size = new System.Drawing.Size(80, 23);
            this.simpleButtonState.TabIndex = 9;
            this.simpleButtonState.Text = "说明";
            this.simpleButtonState.Click += new System.EventHandler(this.simpleButtonState_Click);
            // 
            // simpleButtonConsultationRefresh
            // 
            this.simpleButtonConsultationRefresh.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonConsultationRefresh.Image")));
            this.simpleButtonConsultationRefresh.Location = new System.Drawing.Point(47, 4);
            this.simpleButtonConsultationRefresh.Name = "simpleButtonConsultationRefresh";
            this.simpleButtonConsultationRefresh.Size = new System.Drawing.Size(80, 23);
            this.simpleButtonConsultationRefresh.TabIndex = 8;
            this.simpleButtonConsultationRefresh.Text = "刷新 (&R)";
            this.simpleButtonConsultationRefresh.Click += new System.EventHandler(this.simpleButtonConsultationRefresh_Click);
            // 
            // gridControlConsultation
            // 
            this.gridControlConsultation.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.gridControlConsultation.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControlConsultation.Location = new System.Drawing.Point(0, 31);
            this.gridControlConsultation.MainView = this.gridViewConsultation;
            this.gridControlConsultation.Name = "gridControlConsultation";
            this.gridControlConsultation.Size = new System.Drawing.Size(281, 343);
            this.gridControlConsultation.TabIndex = 22;
            this.gridControlConsultation.TabStop = false;
            this.gridControlConsultation.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewConsultation});
            // 
            // gridViewConsultation
            // 
            this.gridViewConsultation.Appearance.GroupRow.BackColor = System.Drawing.Color.White;
            this.gridViewConsultation.Appearance.GroupRow.BackColor2 = System.Drawing.Color.White;
            this.gridViewConsultation.Appearance.GroupRow.ForeColor = System.Drawing.Color.Green;
            this.gridViewConsultation.Appearance.GroupRow.Options.UseBackColor = true;
            this.gridViewConsultation.Appearance.GroupRow.Options.UseForeColor = true;
            this.gridViewConsultation.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Transparent;
            this.gridViewConsultation.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gridViewConsultation.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnType,
            this.gridColumnInpatientName,
            this.gridColumn7,
            this.gridColumnConsultDateTime,
            this.gridColumn8,
            this.gridColumnStateID,
            this.gridColumnNoofFristPat,
            this.gridColumnTypeID,
            this.gridColumnConsultapplysn,
            this.gridColumn1});
            this.gridViewConsultation.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewConsultation.GridControl = this.gridControlConsultation;
            this.gridViewConsultation.GroupCount = 1;
            this.gridViewConsultation.Name = "gridViewConsultation";
            this.gridViewConsultation.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridViewConsultation.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewConsultation.OptionsCustomization.AllowFilter = false;
            this.gridViewConsultation.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridViewConsultation.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewConsultation.OptionsFilter.AllowFilterEditor = false;
            this.gridViewConsultation.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewConsultation.OptionsFind.ShowCloseButton = false;
            this.gridViewConsultation.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewConsultation.OptionsView.ShowGroupPanel = false;
            this.gridViewConsultation.OptionsView.ShowIndicator = false;
            this.gridViewConsultation.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumnType, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gridViewConsultation.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridViewConsultation_CustomDrawCell);
            this.gridViewConsultation.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridViewConsultation_RowStyle);
            this.gridViewConsultation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridViewConsultation_MouseDown);
            this.gridViewConsultation.DoubleClick += new System.EventHandler(this.gridViewConsultation_DoubleClick);
            // 
            // gridColumnType
            // 
            this.gridColumnType.AppearanceCell.BackColor = System.Drawing.Color.Green;
            this.gridColumnType.AppearanceCell.Options.UseBackColor = true;
            this.gridColumnType.Caption = "会诊状态";
            this.gridColumnType.FieldName = "CONSULTSTATUS";
            this.gridColumnType.FieldNameSortGroup = "CONSULTSTATUS";
            this.gridColumnType.Name = "gridColumnType";
            this.gridColumnType.OptionsColumn.AllowEdit = false;
            this.gridColumnType.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnType.OptionsFilter.AllowFilter = false;
            // 
            // gridColumnInpatientName
            // 
            this.gridColumnInpatientName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnInpatientName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnInpatientName.Caption = "患者姓名";
            this.gridColumnInpatientName.FieldName = "INPATIENTNAME";
            this.gridColumnInpatientName.Name = "gridColumnInpatientName";
            this.gridColumnInpatientName.OptionsColumn.AllowEdit = false;
            this.gridColumnInpatientName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnInpatientName.Visible = true;
            this.gridColumnInpatientName.VisibleIndex = 0;
            this.gridColumnInpatientName.Width = 81;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "紧急度";
            this.gridColumn7.FieldName = "URGENCYTYPE";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 1;
            this.gridColumn7.Width = 50;
            // 
            // gridColumnConsultDateTime
            // 
            this.gridColumnConsultDateTime.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnConsultDateTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnConsultDateTime.Caption = "会诊时间";
            this.gridColumnConsultDateTime.FieldName = "CONSULTTIME";
            this.gridColumnConsultDateTime.Name = "gridColumnConsultDateTime";
            this.gridColumnConsultDateTime.OptionsColumn.AllowEdit = false;
            this.gridColumnConsultDateTime.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnConsultDateTime.Visible = true;
            this.gridColumnConsultDateTime.VisibleIndex = 2;
            this.gridColumnConsultDateTime.Width = 148;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "患者";
            this.gridColumn8.FieldName = "INPATIENTINFO";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            // 
            // gridColumnStateID
            // 
            this.gridColumnStateID.Caption = "状态";
            this.gridColumnStateID.FieldName = "STATEID";
            this.gridColumnStateID.Name = "gridColumnStateID";
            this.gridColumnStateID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            // 
            // gridColumnNoofFristPat
            // 
            this.gridColumnNoofFristPat.Caption = "首页序号";
            this.gridColumnNoofFristPat.FieldName = "NOOFINPAT";
            this.gridColumnNoofFristPat.Name = "gridColumnNoofFristPat";
            this.gridColumnNoofFristPat.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            // 
            // gridColumnTypeID
            // 
            this.gridColumnTypeID.Caption = "会诊类型";
            this.gridColumnTypeID.FieldName = "CONSULTTYPEID";
            this.gridColumnTypeID.Name = "gridColumnTypeID";
            this.gridColumnTypeID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            // 
            // gridColumnConsultapplysn
            // 
            this.gridColumnConsultapplysn.Caption = "会诊序号";
            this.gridColumnConsultapplysn.FieldName = "CONSULTAPPLYSN";
            this.gridColumnConsultapplysn.Name = "gridColumnConsultapplysn";
            this.gridColumnConsultapplysn.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "是否审核过";
            this.gridColumn1.FieldName = "ISPASSED";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemOpen,
            this.barButtonItemDeleteApplySave,
            this.barButtonItemCallBack});
            this.barManager1.MaxItemId = 17;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(281, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 374);
            this.barDockControlBottom.Size = new System.Drawing.Size(281, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 374);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(281, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 374);
            // 
            // barButtonItemOpen
            // 
            this.barButtonItemOpen.Caption = "打开会诊信息";
            this.barButtonItemOpen.Id = 16;
            this.barButtonItemOpen.Name = "barButtonItemOpen";
            this.barButtonItemOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemOpen_ItemClick);
            // 
            // barButtonItemDeleteApplySave
            // 
            this.barButtonItemDeleteApplySave.Caption = "删除会诊记录";
            this.barButtonItemDeleteApplySave.Id = 14;
            this.barButtonItemDeleteApplySave.Name = "barButtonItemDeleteApplySave";
            this.barButtonItemDeleteApplySave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDeleteApplySave_ItemClick);
            // 
            // barButtonItemCallBack
            // 
            this.barButtonItemCallBack.Caption = "撤销申请单";
            this.barButtonItemCallBack.Id = 15;
            this.barButtonItemCallBack.Name = "barButtonItemCallBack";
            this.barButtonItemCallBack.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemCallBack_ItemClick);
            // 
            // popupMenuConsultation
            // 
            this.popupMenuConsultation.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemOpen),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemDeleteApplySave),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemCallBack)});
            this.popupMenuConsultation.Manager = this.barManager1;
            this.popupMenuConsultation.Name = "popupMenuConsultation";
            // 
            // UCConsultationForDocCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlConsultation);
            this.Controls.Add(this.panelControl6);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UCConsultationForDocCenter";
            this.Size = new System.Drawing.Size(281, 374);
            this.Load += new System.EventHandler(this.UCConsultationForDocCenter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).EndInit();
            this.panelControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlConsultation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewConsultation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuConsultation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl6;
        private DevExpress.XtraEditors.SimpleButton simpleButtonState;
        private DevExpress.XtraEditors.SimpleButton simpleButtonConsultationRefresh;
        private DevExpress.XtraGrid.GridControl gridControlConsultation;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewConsultation;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnInpatientName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnConsultDateTime;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnStateID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnNoofFristPat;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTypeID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnConsultapplysn;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.PopupMenu popupMenuConsultation;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDeleteApplySave;
        private DevExpress.XtraBars.BarButtonItem barButtonItemCallBack;
        private DevExpress.XtraBars.BarButtonItem barButtonItemOpen;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}
