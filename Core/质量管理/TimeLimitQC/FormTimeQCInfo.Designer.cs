namespace DrectSoft.Core.TimeLimitQC
{
    partial class FormTimeQCInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimeQCInfo));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.ToolStripMenuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.ucTimeQcInfo1 = new DrectSoft.Emr.QCTimeLimit.QCControlUse.UCTimeQcInfo();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridTimeLimit = new DevExpress.XtraGrid.GridControl();
            this.gridViewTimeLimit = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemImageXB = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_reset = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl19 = new DevExpress.XtraEditors.LabelControl();
            this.date_inpEnd = new DevExpress.XtraEditors.DateEdit();
            this.lookUpEditorDept = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowDept = new DrectSoft.Common.Library.LookUpWindow();
            this.lookUpEditorDoc = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowDoc = new DrectSoft.Common.Library.LookUpWindow();
            this.btn_query = new DevExpress.XtraEditors.SimpleButton();
            this.btn_expansion = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.btn_contract = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.text_patientSN = new DevExpress.XtraEditors.TextEdit();
            this.date_inpStart = new DevExpress.XtraEditors.DateEdit();
            this.imageListXB = new System.Windows.Forms.ImageList();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTimeLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTimeLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageXB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.date_inpEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_inpEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.text_patientSN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_inpStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_inpStart.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemRefresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 26);
            // 
            // ToolStripMenuItemRefresh
            // 
            this.ToolStripMenuItemRefresh.Name = "ToolStripMenuItemRefresh";
            this.ToolStripMenuItemRefresh.Size = new System.Drawing.Size(112, 22);
            this.ToolStripMenuItemRefresh.Text = "刷新 &R";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1184, 537);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.xtraTabControl1.TabStop = false;
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.ucTimeQcInfo1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1178, 508);
            this.xtraTabPage1.Text = "医生时限信息";
            // 
            // ucTimeQcInfo1
            // 
            this.ucTimeQcInfo1.App = null;
            this.ucTimeQcInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTimeQcInfo1.Location = new System.Drawing.Point(0, 0);
            this.ucTimeQcInfo1.Name = "ucTimeQcInfo1";
            this.ucTimeQcInfo1.Size = new System.Drawing.Size(1178, 508);
            this.ucTimeQcInfo1.TabIndex = 0;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.panelControl2);
            this.xtraTabPage2.Controls.Add(this.panelControl1);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.PageVisible = false;
            this.xtraTabPage2.Size = new System.Drawing.Size(1178, 508);
            this.xtraTabPage2.Text = "质控时限信息";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gridTimeLimit);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 80);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1178, 428);
            this.panelControl2.TabIndex = 1;
            // 
            // gridTimeLimit
            // 
            this.gridTimeLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTimeLimit.Location = new System.Drawing.Point(2, 2);
            this.gridTimeLimit.MainView = this.gridViewTimeLimit;
            this.gridTimeLimit.Name = "gridTimeLimit";
            this.gridTimeLimit.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemImageXB});
            this.gridTimeLimit.Size = new System.Drawing.Size(1174, 424);
            this.gridTimeLimit.TabIndex = 0;
            this.gridTimeLimit.TabStop = false;
            this.gridTimeLimit.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTimeLimit});
            // 
            // gridViewTimeLimit
            // 
            this.gridViewTimeLimit.Appearance.GroupPanel.BackColor = System.Drawing.Color.White;
            this.gridViewTimeLimit.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gridViewTimeLimit.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9});
            this.gridViewTimeLimit.CustomizationFormBounds = new System.Drawing.Rectangle(951, 454, 216, 187);
            this.gridViewTimeLimit.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewTimeLimit.GridControl = this.gridTimeLimit;
            this.gridViewTimeLimit.GroupCount = 2;
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
            this.gridViewTimeLimit.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewTimeLimit.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewTimeLimit.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewTimeLimit.OptionsMenu.ShowGroupSummaryEditorItem = true;
            this.gridViewTimeLimit.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewTimeLimit.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gridViewTimeLimit.OptionsView.ShowIndicator = false;
            this.gridViewTimeLimit.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn1, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn2, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "床位医生";
            this.gridColumn1.FieldName = "UserName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn1.Width = 80;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "患者姓名";
            this.gridColumn2.FieldName = "PatName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn2.Width = 120;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "科室";
            this.gridColumn3.FieldName = "DeptName";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 120;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "年龄";
            this.gridColumn4.FieldName = "Age";
            this.gridColumn4.MaxWidth = 80;
            this.gridColumn4.MinWidth = 50;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 79;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "性别";
            this.gridColumn5.ColumnEdit = this.repositoryItemImageXB;
            this.gridColumn5.FieldName = "SexID";
            this.gridColumn5.MaxWidth = 60;
            this.gridColumn5.MinWidth = 40;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 1;
            this.gridColumn5.Width = 60;
            // 
            // repositoryItemImageXB
            // 
            this.repositoryItemImageXB.AutoHeight = false;
            this.repositoryItemImageXB.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageXB.Name = "repositoryItemImageXB";
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "床位号";
            this.gridColumn6.FieldName = "Outbed";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            this.gridColumn6.Width = 70;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "入院日期";
            this.gridColumn7.DisplayFormat.FormatString = "DateTime \"yyyy-MM-dd HH:mm\"";
            this.gridColumn7.FieldName = "InTime";
            this.gridColumn7.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn7.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 4;
            this.gridColumn7.Width = 150;
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.Caption = "时限";
            this.gridColumn8.FieldName = "TimeLimit";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 5;
            this.gridColumn8.Width = 150;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.ForeColor = System.Drawing.Color.Red;
            this.gridColumn9.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn9.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.Caption = "提示或警告信息";
            this.gridColumn9.FieldName = "TipWarnInfo";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 6;
            this.gridColumn9.Width = 543;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btn_reset);
            this.panelControl1.Controls.Add(this.labelControl19);
            this.panelControl1.Controls.Add(this.date_inpEnd);
            this.panelControl1.Controls.Add(this.lookUpEditorDept);
            this.panelControl1.Controls.Add(this.lookUpEditorDoc);
            this.panelControl1.Controls.Add(this.btn_query);
            this.panelControl1.Controls.Add(this.btn_expansion);
            this.panelControl1.Controls.Add(this.labelControl13);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.labelControl12);
            this.panelControl1.Controls.Add(this.btn_contract);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.text_patientSN);
            this.panelControl1.Controls.Add(this.date_inpStart);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1178, 80);
            this.panelControl1.TabIndex = 0;
            // 
            // btn_reset
            // 
            this.btn_reset.Image = global::DrectSoft.Core.TimeLimitQC.QcResource.重置;
            this.btn_reset.Location = new System.Drawing.Point(1061, 11);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(80, 27);
            this.btn_reset.TabIndex = 6;
            this.btn_reset.Text = "重置 (&B)";
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // labelControl19
            // 
            this.labelControl19.Location = new System.Drawing.Point(12, 15);
            this.labelControl19.Name = "labelControl19";
            this.labelControl19.Size = new System.Drawing.Size(60, 14);
            this.labelControl19.TabIndex = 9;
            this.labelControl19.Text = "医生科室：";
            // 
            // date_inpEnd
            // 
            this.date_inpEnd.EditValue = null;
            this.date_inpEnd.EnterMoveNextControl = true;
            this.date_inpEnd.Location = new System.Drawing.Point(848, 14);
            this.date_inpEnd.Name = "date_inpEnd";
            this.date_inpEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.date_inpEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.date_inpEnd.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.date_inpEnd.Size = new System.Drawing.Size(103, 20);
            this.date_inpEnd.TabIndex = 4;
            this.date_inpEnd.ToolTip = "结束日期";
            // 
            // lookUpEditorDept
            // 
            this.lookUpEditorDept.EnterMoveNextControl = true;
            this.lookUpEditorDept.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDept.ListWindow = this.lookUpWindowDept;
            this.lookUpEditorDept.Location = new System.Drawing.Point(74, 14);
            this.lookUpEditorDept.Name = "lookUpEditorDept";
            this.lookUpEditorDept.ShowFormImmediately = true;
            this.lookUpEditorDept.ShowSButton = true;
            this.lookUpEditorDept.Size = new System.Drawing.Size(150, 18);
            this.lookUpEditorDept.TabIndex = 0;
            this.lookUpEditorDept.ToolTip = "支持科室编码、科室名称(汉字/拼音/五笔)查询";
            this.lookUpEditorDept.CodeValueChanged += new System.EventHandler(this.lookUpEditorDept_CodeValueChanged);
            // 
            // lookUpWindowDept
            // 
            this.lookUpWindowDept.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDept.GenShortCode = null;
            this.lookUpWindowDept.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDept.Owner = null;
            this.lookUpWindowDept.SqlHelper = null;
            // 
            // lookUpEditorDoc
            // 
            this.lookUpEditorDoc.EnterMoveNextControl = true;
            this.lookUpEditorDoc.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDoc.ListWindow = this.lookUpWindowDoc;
            this.lookUpEditorDoc.Location = new System.Drawing.Point(302, 14);
            this.lookUpEditorDoc.Name = "lookUpEditorDoc";
            this.lookUpEditorDoc.ShowFormImmediately = true;
            this.lookUpEditorDoc.ShowSButton = true;
            this.lookUpEditorDoc.Size = new System.Drawing.Size(150, 18);
            this.lookUpEditorDoc.TabIndex = 1;
            this.lookUpEditorDoc.ToolTip = "支持医生工号、医生名称(汉字/拼音/五笔)查询";
            // 
            // lookUpWindowDoc
            // 
            this.lookUpWindowDoc.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDoc.GenShortCode = null;
            this.lookUpWindowDoc.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDoc.Owner = null;
            this.lookUpWindowDoc.SqlHelper = null;
            // 
            // btn_query
            // 
            this.btn_query.Image = global::DrectSoft.Core.TimeLimitQC.QcResource.查询;
            this.btn_query.Location = new System.Drawing.Point(975, 11);
            this.btn_query.Name = "btn_query";
            this.btn_query.Size = new System.Drawing.Size(80, 27);
            this.btn_query.TabIndex = 5;
            this.btn_query.Text = "查询 (&Q)";
            this.btn_query.Click += new System.EventHandler(this.btn_query_Click);
            // 
            // btn_expansion
            // 
            this.btn_expansion.Image = global::DrectSoft.Core.TimeLimitQC.QcResource.展开;
            this.btn_expansion.Location = new System.Drawing.Point(98, 43);
            this.btn_expansion.Name = "btn_expansion";
            this.btn_expansion.Size = new System.Drawing.Size(80, 27);
            this.btn_expansion.TabIndex = 8;
            this.btn_expansion.Text = "展开 (&U)";
            this.btn_expansion.Click += new System.EventHandler(this.btn_expansion_Click);
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(828, 15);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(12, 14);
            this.labelControl13.TabIndex = 13;
            this.labelControl13.Text = "至";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(471, 15);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 11;
            this.labelControl5.Text = "病历号：";
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(659, 15);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(60, 14);
            this.labelControl12.TabIndex = 12;
            this.labelControl12.Text = "入院日期：";
            // 
            // btn_contract
            // 
            this.btn_contract.Image = global::DrectSoft.Core.TimeLimitQC.QcResource.收缩;
            this.btn_contract.Location = new System.Drawing.Point(12, 43);
            this.btn_contract.Name = "btn_contract";
            this.btn_contract.Size = new System.Drawing.Size(80, 27);
            this.btn_contract.TabIndex = 7;
            this.btn_contract.Text = "收缩 (&V)";
            this.btn_contract.Click += new System.EventHandler(this.btn_contract_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(240, 15);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 14);
            this.labelControl6.TabIndex = 10;
            this.labelControl6.Text = "医生姓名：";
            // 
            // text_patientSN
            // 
            this.text_patientSN.EnterMoveNextControl = true;
            this.text_patientSN.Location = new System.Drawing.Point(521, 14);
            this.text_patientSN.Name = "text_patientSN";
            this.text_patientSN.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.text_patientSN.Size = new System.Drawing.Size(120, 20);
            this.text_patientSN.TabIndex = 2;
            this.text_patientSN.ToolTip = "病历号";
            // 
            // date_inpStart
            // 
            this.date_inpStart.EditValue = null;
            this.date_inpStart.EnterMoveNextControl = true;
            this.date_inpStart.Location = new System.Drawing.Point(721, 14);
            this.date_inpStart.Name = "date_inpStart";
            this.date_inpStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.date_inpStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.date_inpStart.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.date_inpStart.Size = new System.Drawing.Size(103, 20);
            this.date_inpStart.TabIndex = 3;
            this.date_inpStart.ToolTip = "开始日期";
            // 
            // imageListXB
            // 
            this.imageListXB.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListXB.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListXB.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FormTimeQCInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 537);
            this.Controls.Add(this.xtraTabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTimeQCInfo";
            this.Text = "病历时限信息";
            this.Load += new System.EventHandler(this.FormTimeQCInfo_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTimeLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTimeLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageXB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.date_inpEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_inpEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.text_patientSN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_inpStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_inpStart.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRefresh;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private Common.Library.LookUpWindow lookUpWindowDept;
        private DevExpress.XtraGrid.GridControl gridTimeLimit;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTimeLimit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.TextEdit text_patientSN;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton btn_query;
        private DevExpress.XtraEditors.DateEdit date_inpEnd;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.DateEdit date_inpStart;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private Common.Library.LookUpEditor lookUpEditorDoc;
        private Common.Library.LookUpEditor lookUpEditorDept;
        private DevExpress.XtraEditors.LabelControl labelControl19;
        private Common.Library.LookUpWindow lookUpWindowDoc;
        private DevExpress.XtraEditors.SimpleButton btn_expansion;
        private DevExpress.XtraEditors.SimpleButton btn_contract;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_reset;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageXB;
        private System.Windows.Forms.ImageList imageListXB;
        private Emr.QCTimeLimit.QCControlUse.UCTimeQcInfo ucTimeQcInfo1;
    }
}