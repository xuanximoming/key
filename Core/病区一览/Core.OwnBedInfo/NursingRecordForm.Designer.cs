namespace DrectSoft.Core.OwnBedInfo
{
	partial class NursingRecordForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NursingRecordForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblPatName = new DevExpress.XtraEditors.LabelControl();
            this.txtName = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.gcBaby = new DevExpress.XtraEditors.GroupControl();
            this.lblInfo = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.lblPre = new DevExpress.XtraEditors.LabelControl();
            this.gcPreList = new DevExpress.XtraGrid.GridControl();
            this.gvPreList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.lblCurrent = new DevExpress.XtraEditors.LabelControl();
            this.gcCurrentList = new DevExpress.XtraGrid.GridControl();
            this.gvCurrentList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.TIMESLOT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcBaby)).BeginInit();
            this.gcBaby.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcPreList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPreList)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCurrentList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCurrentList)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lblPatName);
            this.panelControl1.Controls.Add(this.txtName);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(527, 35);
            this.panelControl1.TabIndex = 0;
            // 
            // lblPatName
            // 
            this.lblPatName.Location = new System.Drawing.Point(12, 10);
            this.lblPatName.Name = "lblPatName";
            this.lblPatName.Size = new System.Drawing.Size(36, 14);
            this.lblPatName.TabIndex = 0;
            this.lblPatName.Text = "姓名：";
            // 
            // txtName
            // 
            this.txtName.EnterMoveNextControl = true;
            this.txtName.IsEnterChangeBgColor = false;
            this.txtName.IsEnterKeyToNextControl = false;
            this.txtName.IsNumber = false;
            this.txtName.Location = new System.Drawing.Point(49, 8);
            this.txtName.Name = "txtName";
            this.txtName.Properties.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 0;
            // 
            // gcBaby
            // 
            this.gcBaby.AppearanceCaption.ForeColor = System.Drawing.Color.Blue;
            this.gcBaby.AppearanceCaption.Options.UseForeColor = true;
            this.gcBaby.Controls.Add(this.lblInfo);
            this.gcBaby.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gcBaby.Location = new System.Drawing.Point(0, 221);
            this.gcBaby.Name = "gcBaby";
            this.gcBaby.Size = new System.Drawing.Size(527, 54);
            this.gcBaby.TabIndex = 2;
            this.gcBaby.Text = "婴儿体征数据";
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(15, 28);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(0, 14);
            this.lblInfo.TabIndex = 0;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 35);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(527, 186);
            this.xtraTabControl1.TabIndex = 3;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.lblPre);
            this.xtraTabPage1.Controls.Add(this.gcPreList);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(521, 157);
            this.xtraTabPage1.Text = "昨天数据";
            // 
            // lblPre
            // 
            this.lblPre.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lblPre.Location = new System.Drawing.Point(213, 80);
            this.lblPre.Name = "lblPre";
            this.lblPre.Size = new System.Drawing.Size(36, 14);
            this.lblPre.TabIndex = 6;
            this.lblPre.Text = "无数据";
            // 
            // gcPreList
            // 
            this.gcPreList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPreList.Location = new System.Drawing.Point(0, 0);
            this.gcPreList.MainView = this.gvPreList;
            this.gcPreList.Name = "gcPreList";
            this.gcPreList.Size = new System.Drawing.Size(521, 157);
            this.gcPreList.TabIndex = 4;
            this.gcPreList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPreList});
            // 
            // gvPreList
            // 
            this.gvPreList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn7,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13,
            this.gridColumn14,
            this.gridColumn15});
            this.gvPreList.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gvPreList.GridControl = this.gcPreList;
            this.gvPreList.IndicatorWidth = 42;
            this.gvPreList.Name = "gvPreList";
            this.gvPreList.OptionsCustomization.AllowColumnMoving = false;
            this.gvPreList.OptionsCustomization.AllowColumnResizing = false;
            this.gvPreList.OptionsCustomization.AllowFilter = false;
            this.gvPreList.OptionsCustomization.AllowGroup = false;
            this.gvPreList.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvPreList.OptionsCustomization.AllowRowSizing = true;
            this.gvPreList.OptionsCustomization.AllowSort = false;
            this.gvPreList.OptionsMenu.EnableColumnMenu = false;
            this.gvPreList.OptionsMenu.EnableFooterMenu = false;
            this.gvPreList.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvPreList.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvPreList.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvPreList.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvPreList.OptionsMenu.ShowGroupSummaryEditorItem = true;
            this.gvPreList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvPreList.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvPreList.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gvPreList.OptionsSelection.UseIndicatorForSelection = false;
            this.gvPreList.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gvPreList.OptionsView.ShowGroupPanel = false;
            this.gvPreList.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvPreList_CustomDrawRowIndicator);
            this.gvPreList.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvList_CustomDrawCell);
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "时段";
            this.gridColumn7.FieldName = "TIMESLOT";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsColumn.AllowFocus = false;
            this.gridColumn7.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 0;
            this.gridColumn7.Width = 40;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "体温(℃)";
            this.gridColumn9.FieldName = "TEMPERATURE";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.OptionsColumn.AllowFocus = false;
            this.gridColumn9.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 1;
            this.gridColumn9.Width = 60;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "脉搏(次/分)";
            this.gridColumn10.FieldName = "PULSE";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.OptionsColumn.AllowFocus = false;
            this.gridColumn10.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 2;
            this.gridColumn10.Width = 70;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "心率(次/分)";
            this.gridColumn11.FieldName = "HEARTRATE";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.OptionsColumn.AllowFocus = false;
            this.gridColumn11.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 4;
            this.gridColumn11.Width = 70;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "序号";
            this.gridColumn12.CustomizationCaption = "首页序号";
            this.gridColumn12.FieldName = "NOOFINPAT";
            this.gridColumn12.Name = "gridColumn12";
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "呼吸(次/分)";
            this.gridColumn13.FieldName = "BREATHE";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.AllowEdit = false;
            this.gridColumn13.OptionsColumn.AllowFocus = false;
            this.gridColumn13.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 3;
            this.gridColumn13.Width = 70;
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "物理降温(℃)";
            this.gridColumn14.FieldName = "PHYSICALCOOLING";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.OptionsColumn.AllowEdit = false;
            this.gridColumn14.OptionsColumn.AllowFocus = false;
            this.gridColumn14.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 5;
            this.gridColumn14.Width = 80;
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "物理升温(℃)";
            this.gridColumn15.FieldName = "PHYSICALHOTTING";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.OptionsColumn.AllowEdit = false;
            this.gridColumn15.OptionsColumn.AllowFocus = false;
            this.gridColumn15.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 6;
            this.gridColumn15.Width = 80;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.lblCurrent);
            this.xtraTabPage2.Controls.Add(this.gcCurrentList);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(521, 157);
            this.xtraTabPage2.Text = "今天数据";
            // 
            // lblCurrent
            // 
            this.lblCurrent.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lblCurrent.Location = new System.Drawing.Point(213, 80);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(36, 14);
            this.lblCurrent.TabIndex = 5;
            this.lblCurrent.Text = "无数据";
            // 
            // gcCurrentList
            // 
            this.gcCurrentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCurrentList.Location = new System.Drawing.Point(0, 0);
            this.gcCurrentList.MainView = this.gvCurrentList;
            this.gcCurrentList.Name = "gcCurrentList";
            this.gcCurrentList.Size = new System.Drawing.Size(521, 157);
            this.gcCurrentList.TabIndex = 4;
            this.gcCurrentList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCurrentList});
            // 
            // gvCurrentList
            // 
            this.gvCurrentList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.TIMESLOT,
            this.gridColumn2,
            this.gridColumn8,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn1,
            this.gridColumn5,
            this.gridColumn6});
            this.gvCurrentList.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gvCurrentList.GridControl = this.gcCurrentList;
            this.gvCurrentList.IndicatorWidth = 42;
            this.gvCurrentList.Name = "gvCurrentList";
            this.gvCurrentList.OptionsCustomization.AllowColumnMoving = false;
            this.gvCurrentList.OptionsCustomization.AllowColumnResizing = false;
            this.gvCurrentList.OptionsCustomization.AllowFilter = false;
            this.gvCurrentList.OptionsCustomization.AllowGroup = false;
            this.gvCurrentList.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvCurrentList.OptionsCustomization.AllowRowSizing = true;
            this.gvCurrentList.OptionsCustomization.AllowSort = false;
            this.gvCurrentList.OptionsMenu.EnableColumnMenu = false;
            this.gvCurrentList.OptionsMenu.EnableFooterMenu = false;
            this.gvCurrentList.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvCurrentList.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvCurrentList.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvCurrentList.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvCurrentList.OptionsMenu.ShowGroupSummaryEditorItem = true;
            this.gvCurrentList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvCurrentList.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvCurrentList.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gvCurrentList.OptionsSelection.UseIndicatorForSelection = false;
            this.gvCurrentList.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gvCurrentList.OptionsView.ShowGroupPanel = false;
            this.gvCurrentList.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvCurrentList_CustomDrawRowIndicator);
            this.gvCurrentList.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvList_CustomDrawCell);
            // 
            // TIMESLOT
            // 
            this.TIMESLOT.Caption = "时段";
            this.TIMESLOT.FieldName = "TIMESLOT";
            this.TIMESLOT.Name = "TIMESLOT";
            this.TIMESLOT.OptionsColumn.AllowEdit = false;
            this.TIMESLOT.OptionsColumn.AllowFocus = false;
            this.TIMESLOT.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.TIMESLOT.Visible = true;
            this.TIMESLOT.VisibleIndex = 0;
            this.TIMESLOT.Width = 40;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "体温(℃)";
            this.gridColumn2.FieldName = "TEMPERATURE";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 60;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "脉搏(次/分)";
            this.gridColumn8.FieldName = "PULSE";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsColumn.AllowFocus = false;
            this.gridColumn8.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 2;
            this.gridColumn8.Width = 70;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "心率(次/分)";
            this.gridColumn3.FieldName = "HEARTRATE";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 4;
            this.gridColumn3.Width = 70;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "序号";
            this.gridColumn4.CustomizationCaption = "首页序号";
            this.gridColumn4.FieldName = "NOOFINPAT";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "呼吸(次/分)";
            this.gridColumn1.FieldName = "BREATHE";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            this.gridColumn1.Width = 70;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "物理降温(℃)";
            this.gridColumn5.FieldName = "PHYSICALCOOLING";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 5;
            this.gridColumn5.Width = 80;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "物理升温(℃)";
            this.gridColumn6.FieldName = "PHYSICALHOTTING";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            this.gridColumn6.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 6;
            this.gridColumn6.Width = 80;
            // 
            // NursingRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 275);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.gcBaby);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NursingRecordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "病人体征数据";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.NursingRecordForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcBaby)).EndInit();
            this.gcBaby.ResumeLayout(false);
            this.gcBaby.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcPreList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPreList)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCurrentList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCurrentList)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtName;
        private DevExpress.XtraEditors.LabelControl lblPatName;
        private DevExpress.XtraEditors.GroupControl gcBaby;
        private DevExpress.XtraEditors.LabelControl lblInfo;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl gcPreList;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPreList;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.GridControl gcCurrentList;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCurrentList;
        private DevExpress.XtraGrid.Columns.GridColumn TIMESLOT;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.LabelControl lblCurrent;
        private DevExpress.XtraEditors.LabelControl lblPre;

    }
}