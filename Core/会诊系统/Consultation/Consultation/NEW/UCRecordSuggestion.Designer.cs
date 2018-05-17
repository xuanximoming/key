namespace Consultation.NEW
{
	partial class UCRecordSuggestion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCRecordSuggestion));
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.gridControlDepartment = new DevExpress.XtraGrid.GridControl();
            this.gridViewDept = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Department = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Level = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnEmployee = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DeleteButton = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.DepartmentCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LevelID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EmployeeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEditEmployee = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.btnDelete = new DrectSoft.Common.Ctrs.OTHER.DevButtonDelete();
            this.btnEdit = new DrectSoft.Common.Ctrs.OTHER.DevButtonEdit();
            this.btnAdd = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.timeEditConsultationTime = new DevExpress.XtraEditors.TimeEdit();
            this.dateEditConsultationDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.memoEditSuggestion = new DevExpress.XtraEditors.MemoEdit();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnSaveConsultApply = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave();
            this.btnCompleteConsult = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditEmployee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeEditConsultationTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).BeginInit();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl3
            // 
            this.groupControl3.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl3.AppearanceCaption.Options.UseFont = true;
            this.groupControl3.Controls.Add(this.gridControlDepartment);
            this.groupControl3.Controls.Add(this.btnDelete);
            this.groupControl3.Controls.Add(this.btnEdit);
            this.groupControl3.Controls.Add(this.btnAdd);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl3.Location = new System.Drawing.Point(0, 273);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(976, 251);
            this.groupControl3.TabIndex = 45;
            this.groupControl3.Text = "受邀医师列表";
            // 
            // gridControlDepartment
            // 
            this.gridControlDepartment.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridControlDepartment.Location = new System.Drawing.Point(2, 71);
            this.gridControlDepartment.MainView = this.gridViewDept;
            this.gridControlDepartment.Name = "gridControlDepartment";
            this.gridControlDepartment.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHyperLinkEdit1,
            this.repositoryItemLookUpEditEmployee});
            this.gridControlDepartment.Size = new System.Drawing.Size(972, 178);
            this.gridControlDepartment.TabIndex = 54;
            this.gridControlDepartment.TabStop = false;
            this.gridControlDepartment.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDept});
            // 
            // gridViewDept
            // 
            this.gridViewDept.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Green;
            this.gridViewDept.Appearance.ViewCaption.Options.UseForeColor = true;
            this.gridViewDept.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Department,
            this.Level,
            this.gridColumnEmployee,
            this.DeleteButton,
            this.DepartmentCode,
            this.LevelID,
            this.gridColumn1,
            this.gridColumn2,
            this.EmployeeName});
            this.gridViewDept.GridControl = this.gridControlDepartment;
            this.gridViewDept.IndicatorWidth = 40;
            this.gridViewDept.Name = "gridViewDept";
            this.gridViewDept.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewDept.OptionsBehavior.Editable = false;
            this.gridViewDept.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewDept.OptionsCustomization.AllowColumnResizing = false;
            this.gridViewDept.OptionsCustomization.AllowFilter = false;
            this.gridViewDept.OptionsCustomization.AllowSort = false;
            this.gridViewDept.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewDept.OptionsFilter.AllowFilterEditor = false;
            this.gridViewDept.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewDept.OptionsMenu.EnableColumnMenu = false;
            this.gridViewDept.OptionsMenu.EnableFooterMenu = false;
            this.gridViewDept.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewDept.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewDept.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewDept.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewDept.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewDept.OptionsView.ShowGroupPanel = false;
            this.gridViewDept.OptionsView.ShowViewCaption = true;
            this.gridViewDept.ViewCaption = "受邀医师列表";
            // 
            // Department
            // 
            this.Department.AppearanceHeader.Options.UseTextOptions = true;
            this.Department.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Department.Caption = "会诊科室";
            this.Department.FieldName = "DepartmentName";
            this.Department.Name = "Department";
            this.Department.OptionsColumn.AllowEdit = false;
            this.Department.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.Department.OptionsColumn.AllowIncrementalSearch = false;
            this.Department.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.Department.OptionsFilter.AllowAutoFilter = false;
            this.Department.OptionsFilter.AllowFilter = false;
            this.Department.Visible = true;
            this.Department.VisibleIndex = 0;
            this.Department.Width = 120;
            // 
            // Level
            // 
            this.Level.AppearanceHeader.Options.UseTextOptions = true;
            this.Level.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Level.Caption = "医师资质";
            this.Level.FieldName = "EmployeeLevelName";
            this.Level.Name = "Level";
            this.Level.OptionsColumn.AllowEdit = false;
            this.Level.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.Level.OptionsColumn.AllowIncrementalSearch = false;
            this.Level.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.Level.OptionsFilter.AllowAutoFilter = false;
            this.Level.OptionsFilter.AllowFilter = false;
            this.Level.Visible = true;
            this.Level.VisibleIndex = 1;
            this.Level.Width = 100;
            // 
            // gridColumnEmployee
            // 
            this.gridColumnEmployee.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnEmployee.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnEmployee.Caption = "会诊医师";
            this.gridColumnEmployee.FieldName = "EMPLOYEENAMESTR";
            this.gridColumnEmployee.Name = "gridColumnEmployee";
            this.gridColumnEmployee.OptionsColumn.AllowEdit = false;
            this.gridColumnEmployee.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnEmployee.Visible = true;
            this.gridColumnEmployee.VisibleIndex = 2;
            this.gridColumnEmployee.Width = 100;
            // 
            // DeleteButton
            // 
            this.DeleteButton.AppearanceCell.Options.UseTextOptions = true;
            this.DeleteButton.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DeleteButton.AppearanceHeader.Options.UseTextOptions = true;
            this.DeleteButton.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DeleteButton.Caption = "操作";
            this.DeleteButton.ColumnEdit = this.repositoryItemHyperLinkEdit1;
            this.DeleteButton.FieldName = "DeleteButton";
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.OptionsColumn.AllowEdit = false;
            this.DeleteButton.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.DeleteButton.OptionsColumn.AllowIncrementalSearch = false;
            this.DeleteButton.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.DeleteButton.OptionsFilter.AllowAutoFilter = false;
            this.DeleteButton.OptionsFilter.AllowFilter = false;
            this.DeleteButton.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.DeleteButton.Visible = true;
            this.DeleteButton.VisibleIndex = 3;
            this.DeleteButton.Width = 80;
            // 
            // repositoryItemHyperLinkEdit1
            // 
            this.repositoryItemHyperLinkEdit1.AutoHeight = false;
            this.repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            // 
            // DepartmentCode
            // 
            this.DepartmentCode.AppearanceHeader.Options.UseTextOptions = true;
            this.DepartmentCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DepartmentCode.Caption = "科室代码";
            this.DepartmentCode.FieldName = "DepartmentCode";
            this.DepartmentCode.Name = "DepartmentCode";
            this.DepartmentCode.OptionsColumn.AllowEdit = false;
            this.DepartmentCode.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.DepartmentCode.OptionsColumn.AllowIncrementalSearch = false;
            this.DepartmentCode.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.DepartmentCode.OptionsFilter.AllowAutoFilter = false;
            this.DepartmentCode.OptionsFilter.AllowFilter = false;
            // 
            // LevelID
            // 
            this.LevelID.AppearanceHeader.Options.UseTextOptions = true;
            this.LevelID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LevelID.Caption = "资质代码";
            this.LevelID.FieldName = "EmployeeLevelID";
            this.LevelID.Name = "LevelID";
            this.LevelID.OptionsColumn.AllowEdit = false;
            this.LevelID.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.LevelID.OptionsColumn.AllowIncrementalSearch = false;
            this.LevelID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.LevelID.OptionsFilter.AllowAutoFilter = false;
            this.LevelID.OptionsFilter.AllowFilter = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "会诊医师代码";
            this.gridColumn1.FieldName = "EMPLOYEECODE";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "签到";
            this.gridColumn2.FieldName = "ISSIGNIN";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            // 
            // EmployeeName
            // 
            this.EmployeeName.Caption = "医生姓名";
            this.EmployeeName.Name = "EmployeeName";
            // 
            // repositoryItemLookUpEditEmployee
            // 
            this.repositoryItemLookUpEditEmployee.AutoHeight = false;
            this.repositoryItemLookUpEditEmployee.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEditEmployee.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EMPLOYEECODE", 60, "医师代码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EMPLOYEENAMESTR", 60, "医师名称"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DEPTNAME", 90, "科室"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DEPTCODE", "科室代码", 60, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.repositoryItemLookUpEditEmployee.DisplayMember = "EMPLOYEENAMESTR";
            this.repositoryItemLookUpEditEmployee.Name = "repositoryItemLookUpEditEmployee";
            this.repositoryItemLookUpEditEmployee.NullText = "";
            this.repositoryItemLookUpEditEmployee.ReadOnly = true;
            this.repositoryItemLookUpEditEmployee.ValueMember = "EMPLOYEECODE";
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Location = new System.Drawing.Point(166, 29);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(73, 23);
            this.btnDelete.TabIndex = 57;
            this.btnDelete.Text = "删除(&D)";
            // 
            // btnEdit
            // 
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.Location = new System.Drawing.Point(90, 29);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(73, 23);
            this.btnEdit.TabIndex = 56;
            this.btnEdit.Text = "编辑(&E)";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(14, 29);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(73, 23);
            this.btnAdd.TabIndex = 55;
            this.btnAdd.Text = "新增(&A)";
            // 
            // groupControl2
            // 
            this.groupControl2.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl2.AppearanceCaption.Options.UseFont = true;
            this.groupControl2.Controls.Add(this.timeEditConsultationTime);
            this.groupControl2.Controls.Add(this.dateEditConsultationDate);
            this.groupControl2.Controls.Add(this.labelControl4);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Controls.Add(this.memoEditSuggestion);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(976, 273);
            this.groupControl2.TabIndex = 44;
            this.groupControl2.Text = "会诊记录填写";
            // 
            // timeEditConsultationTime
            // 
            this.timeEditConsultationTime.EditValue = new System.DateTime(((long)(0)));
            this.timeEditConsultationTime.EnterMoveNextControl = true;
            this.timeEditConsultationTime.Location = new System.Drawing.Point(274, 237);
            this.timeEditConsultationTime.Name = "timeEditConsultationTime";
            this.timeEditConsultationTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeEditConsultationTime.Size = new System.Drawing.Size(93, 21);
            this.timeEditConsultationTime.TabIndex = 47;
            this.timeEditConsultationTime.ToolTip = "会诊时间";
            // 
            // dateEditConsultationDate
            // 
            this.dateEditConsultationDate.EditValue = null;
            this.dateEditConsultationDate.EnterMoveNextControl = true;
            this.dateEditConsultationDate.Location = new System.Drawing.Point(98, 237);
            this.dateEditConsultationDate.Name = "dateEditConsultationDate";
            this.dateEditConsultationDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditConsultationDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateEditConsultationDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditConsultationDate.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateEditConsultationDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditConsultationDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditConsultationDate.Size = new System.Drawing.Size(175, 21);
            this.dateEditConsultationDate.TabIndex = 46;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(14, 240);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(84, 14);
            this.labelControl4.TabIndex = 48;
            this.labelControl4.Text = "会诊完成时间：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 32);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(84, 14);
            this.labelControl1.TabIndex = 45;
            this.labelControl1.Text = "会诊医师意见：";
            // 
            // memoEditSuggestion
            // 
            this.memoEditSuggestion.Location = new System.Drawing.Point(6, 54);
            this.memoEditSuggestion.Name = "memoEditSuggestion";
            this.memoEditSuggestion.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoEditSuggestion.Properties.Appearance.Options.UseFont = true;
            this.memoEditSuggestion.Size = new System.Drawing.Size(958, 172);
            this.memoEditSuggestion.TabIndex = 44;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnSaveConsultApply);
            this.panel7.Controls.Add(this.btnCompleteConsult);
            this.panel7.Location = new System.Drawing.Point(576, 528);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(395, 42);
            this.panel7.TabIndex = 43;
            // 
            // btnSaveConsultApply
            // 
            this.btnSaveConsultApply.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveConsultApply.Image")));
            this.btnSaveConsultApply.Location = new System.Drawing.Point(199, 8);
            this.btnSaveConsultApply.Name = "btnSaveConsultApply";
            this.btnSaveConsultApply.Size = new System.Drawing.Size(80, 23);
            this.btnSaveConsultApply.TabIndex = 2;
            this.btnSaveConsultApply.Text = "保存(&S)";
            // 
            // btnCompleteConsult
            // 
            this.btnCompleteConsult.Image = global::DrectSoft.Core.Consultation.Properties.Resources.确定;
            this.btnCompleteConsult.Location = new System.Drawing.Point(291, 8);
            this.btnCompleteConsult.Name = "btnCompleteConsult";
            this.btnCompleteConsult.Size = new System.Drawing.Size(80, 23);
            this.btnCompleteConsult.TabIndex = 1;
            this.btnCompleteConsult.Text = "会诊完成";
            // 
            // UCRecordSuggestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.panel7);
            this.Name = "UCRecordSuggestion";
            this.Size = new System.Drawing.Size(976, 574);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditEmployee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeEditConsultationTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).EndInit();
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraGrid.GridControl gridControlDepartment;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDept;
        private DevExpress.XtraGrid.Columns.GridColumn Department;
        private DevExpress.XtraGrid.Columns.GridColumn Level;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEmployee;
        private DevExpress.XtraGrid.Columns.GridColumn DeleteButton;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn DepartmentCode;
        private DevExpress.XtraGrid.Columns.GridColumn LevelID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEditEmployee;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonDelete btnDelete;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonEdit btnEdit;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd btnAdd;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.TimeEdit timeEditConsultationTime;
        private DevExpress.XtraEditors.DateEdit dateEditConsultationDate;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit memoEditSuggestion;
        private System.Windows.Forms.Panel panel7;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnSaveConsultApply;
        private DevExpress.XtraEditors.SimpleButton btnCompleteConsult;
        private DevExpress.XtraGrid.Columns.GridColumn EmployeeName;
	}
}
