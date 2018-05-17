namespace DrectSoft.Core.Consultation
{
    partial class FormMainConsultation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMainConsultation));
            this.xtraTabControlRecordList = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageConsultationList = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageApplyList = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl4 = new DevExpress.XtraGrid.GridControl();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn43 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn46 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn48 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn47 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn44 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn45 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn41 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn40 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn39 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn42 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.comboBoxEdit9 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.textEdit15 = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.comboBoxEdit8 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl26 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit7 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl25 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl24 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit14 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl23 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabPageApproveList = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.XtraConfigAudio = new DevExpress.XtraTab.XtraTabPage();
            this.lookUpWindow2 = new DrectSoft.Common.Library.LookUpWindow();
            this.lookUpWindowDepartment = new DrectSoft.Common.Library.LookUpWindow();
            this.lookUpWindowEmployee = new DrectSoft.Common.Library.LookUpWindow();
            this.lookUpWindow1 = new DrectSoft.Common.Library.LookUpWindow();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlRecordList)).BeginInit();
            this.xtraTabControlRecordList.SuspendLayout();
            this.xtraTabPageApplyList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit9.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit15.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit8.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit7.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit14.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindow2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowEmployee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindow1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControlRecordList
            // 
            this.xtraTabControlRecordList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControlRecordList.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControlRecordList.Name = "xtraTabControlRecordList";
            this.xtraTabControlRecordList.SelectedTabPage = this.xtraTabPageConsultationList;
            this.xtraTabControlRecordList.Size = new System.Drawing.Size(1084, 562);
            this.xtraTabControlRecordList.TabIndex = 0;
            this.xtraTabControlRecordList.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageConsultationList,
            this.xtraTabPageApplyList,
            this.xtraTabPageApproveList,
            this.xtraTabPage3,
            this.XtraConfigAudio});
            this.xtraTabControlRecordList.TabStop = false;
            this.xtraTabControlRecordList.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControlRecordList_SelectedPageChanged);
            this.xtraTabControlRecordList.Click += new System.EventHandler(this.xtraTabControlRecordList_Click);
            // 
            // xtraTabPageConsultationList
            // 
            this.xtraTabPageConsultationList.Name = "xtraTabPageConsultationList";
            this.xtraTabPageConsultationList.Size = new System.Drawing.Size(1078, 533);
            this.xtraTabPageConsultationList.Text = "会诊清单";
            // 
            // xtraTabPageApplyList
            // 
            this.xtraTabPageApplyList.Controls.Add(this.panelControl1);
            this.xtraTabPageApplyList.Controls.Add(this.panel1);
            this.xtraTabPageApplyList.Name = "xtraTabPageApplyList";
            this.xtraTabPageApplyList.PageVisible = false;
            this.xtraTabPageApplyList.Size = new System.Drawing.Size(1078, 533);
            this.xtraTabPageApplyList.Text = "会诊申请";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridControl4);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 55);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1078, 478);
            this.panelControl1.TabIndex = 7;
            // 
            // gridControl4
            // 
            this.gridControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl4.Location = new System.Drawing.Point(2, 2);
            this.gridControl4.MainView = this.gridView4;
            this.gridControl4.Name = "gridControl4";
            this.gridControl4.Size = new System.Drawing.Size(1074, 474);
            this.gridControl4.TabIndex = 0;
            this.gridControl4.TabStop = false;
            this.gridControl4.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView4});
            // 
            // gridView4
            // 
            this.gridView4.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn43,
            this.gridColumn46,
            this.gridColumn48,
            this.gridColumn47,
            this.gridColumn44,
            this.gridColumn45,
            this.gridColumn41,
            this.gridColumn40,
            this.gridColumn39,
            this.gridColumn42});
            this.gridView4.GridControl = this.gridControl4;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsBehavior.Editable = false;
            this.gridView4.OptionsCustomization.AllowColumnMoving = false;
            this.gridView4.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView4.OptionsFilter.AllowMRUFilterList = false;
            this.gridView4.OptionsMenu.EnableColumnMenu = false;
            this.gridView4.OptionsMenu.EnableFooterMenu = false;
            this.gridView4.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView4.OptionsView.ShowGroupPanel = false;
            this.gridView4.OptionsView.ShowIndicator = false;
            // 
            // gridColumn43
            // 
            this.gridColumn43.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn43.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn43.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn43.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn43.Caption = "住院号";
            this.gridColumn43.Name = "gridColumn43";
            this.gridColumn43.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn43.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn43.OptionsFilter.AllowFilter = false;
            this.gridColumn43.Visible = true;
            this.gridColumn43.VisibleIndex = 0;
            this.gridColumn43.Width = 90;
            // 
            // gridColumn46
            // 
            this.gridColumn46.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn46.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn46.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn46.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn46.Caption = "科室";
            this.gridColumn46.Name = "gridColumn46";
            this.gridColumn46.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn46.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn46.OptionsFilter.AllowFilter = false;
            this.gridColumn46.Visible = true;
            this.gridColumn46.VisibleIndex = 1;
            this.gridColumn46.Width = 100;
            // 
            // gridColumn48
            // 
            this.gridColumn48.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn48.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn48.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn48.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn48.Caption = "病区";
            this.gridColumn48.Name = "gridColumn48";
            this.gridColumn48.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn48.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn48.OptionsFilter.AllowFilter = false;
            this.gridColumn48.Visible = true;
            this.gridColumn48.VisibleIndex = 2;
            this.gridColumn48.Width = 100;
            // 
            // gridColumn47
            // 
            this.gridColumn47.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn47.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn47.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn47.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn47.Caption = "床位号";
            this.gridColumn47.Name = "gridColumn47";
            this.gridColumn47.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn47.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn47.OptionsFilter.AllowFilter = false;
            this.gridColumn47.Visible = true;
            this.gridColumn47.VisibleIndex = 3;
            this.gridColumn47.Width = 60;
            // 
            // gridColumn44
            // 
            this.gridColumn44.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn44.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn44.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn44.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn44.Caption = "姓名";
            this.gridColumn44.Name = "gridColumn44";
            this.gridColumn44.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn44.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn44.OptionsFilter.AllowFilter = false;
            this.gridColumn44.Visible = true;
            this.gridColumn44.VisibleIndex = 4;
            this.gridColumn44.Width = 80;
            // 
            // gridColumn45
            // 
            this.gridColumn45.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn45.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn45.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn45.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn45.Caption = "病历号";
            this.gridColumn45.Name = "gridColumn45";
            this.gridColumn45.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn45.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn45.OptionsFilter.AllowFilter = false;
            this.gridColumn45.Visible = true;
            this.gridColumn45.VisibleIndex = 5;
            this.gridColumn45.Width = 90;
            // 
            // gridColumn41
            // 
            this.gridColumn41.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn41.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn41.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn41.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn41.Caption = "性别";
            this.gridColumn41.Name = "gridColumn41";
            this.gridColumn41.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn41.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn41.OptionsFilter.AllowFilter = false;
            this.gridColumn41.Visible = true;
            this.gridColumn41.VisibleIndex = 6;
            this.gridColumn41.Width = 60;
            // 
            // gridColumn40
            // 
            this.gridColumn40.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn40.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn40.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn40.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn40.Caption = "年龄";
            this.gridColumn40.Name = "gridColumn40";
            this.gridColumn40.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn40.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn40.OptionsFilter.AllowFilter = false;
            this.gridColumn40.Visible = true;
            this.gridColumn40.VisibleIndex = 7;
            this.gridColumn40.Width = 60;
            // 
            // gridColumn39
            // 
            this.gridColumn39.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn39.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn39.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn39.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn39.Caption = "婚姻";
            this.gridColumn39.Name = "gridColumn39";
            this.gridColumn39.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn39.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn39.OptionsFilter.AllowFilter = false;
            this.gridColumn39.Visible = true;
            this.gridColumn39.VisibleIndex = 8;
            this.gridColumn39.Width = 60;
            // 
            // gridColumn42
            // 
            this.gridColumn42.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn42.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn42.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn42.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn42.Caption = "职业";
            this.gridColumn42.Name = "gridColumn42";
            this.gridColumn42.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn42.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn42.OptionsFilter.AllowFilter = false;
            this.gridColumn42.Visible = true;
            this.gridColumn42.VisibleIndex = 9;
            this.gridColumn42.Width = 372;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1078, 55);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.simpleButton4);
            this.groupBox1.Controls.Add(this.comboBoxEdit9);
            this.groupBox1.Controls.Add(this.textEdit15);
            this.groupBox1.Controls.Add(this.comboBoxEdit8);
            this.groupBox1.Controls.Add(this.labelControl26);
            this.groupBox1.Controls.Add(this.comboBoxEdit7);
            this.groupBox1.Controls.Add(this.labelControl25);
            this.groupBox1.Controls.Add(this.labelControl24);
            this.groupBox1.Controls.Add(this.textEdit14);
            this.groupBox1.Controls.Add(this.labelControl22);
            this.groupBox1.Controls.Add(this.labelControl23);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1078, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // simpleButton4
            // 
            this.simpleButton4.Image = global::DrectSoft.Core.Consultation.Properties.Resources.查询;
            this.simpleButton4.Location = new System.Drawing.Point(943, 17);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(80, 27);
            this.simpleButton4.TabIndex = 5;
            this.simpleButton4.Text = "查询 (&Q)";
            // 
            // comboBoxEdit9
            // 
            this.comboBoxEdit9.EnterMoveNextControl = true;
            this.comboBoxEdit9.Location = new System.Drawing.Point(794, 21);
            this.comboBoxEdit9.Name = "comboBoxEdit9";
            this.comboBoxEdit9.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit9.Size = new System.Drawing.Size(120, 20);
            this.comboBoxEdit9.TabIndex = 4;
            this.comboBoxEdit9.ToolTip = "床位号";
            // 
            // textEdit15
            // 
            this.textEdit15.EnterMoveNextControl = true;
            this.textEdit15.Location = new System.Drawing.Point(59, 22);
            this.textEdit15.Name = "textEdit15";
            this.textEdit15.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEdit15.Size = new System.Drawing.Size(120, 20);
            this.textEdit15.TabIndex = 0;
            this.textEdit15.ToolTip = "病历号";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // comboBoxEdit8
            // 
            this.comboBoxEdit8.EnterMoveNextControl = true;
            this.comboBoxEdit8.Location = new System.Drawing.Point(599, 21);
            this.comboBoxEdit8.Name = "comboBoxEdit8";
            this.comboBoxEdit8.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit8.Size = new System.Drawing.Size(120, 20);
            this.comboBoxEdit8.TabIndex = 3;
            this.comboBoxEdit8.ToolTip = "病区";
            // 
            // labelControl26
            // 
            this.labelControl26.Location = new System.Drawing.Point(11, 24);
            this.labelControl26.Name = "labelControl26";
            this.labelControl26.Size = new System.Drawing.Size(48, 14);
            this.labelControl26.TabIndex = 6;
            this.labelControl26.Text = "病历号：";
            // 
            // comboBoxEdit7
            // 
            this.comboBoxEdit7.EnterMoveNextControl = true;
            this.comboBoxEdit7.Location = new System.Drawing.Point(420, 21);
            this.comboBoxEdit7.Name = "comboBoxEdit7";
            this.comboBoxEdit7.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit7.Size = new System.Drawing.Size(120, 20);
            this.comboBoxEdit7.TabIndex = 2;
            this.comboBoxEdit7.ToolTip = "科室";
            // 
            // labelControl25
            // 
            this.labelControl25.Location = new System.Drawing.Point(384, 23);
            this.labelControl25.Name = "labelControl25";
            this.labelControl25.Size = new System.Drawing.Size(36, 14);
            this.labelControl25.TabIndex = 8;
            this.labelControl25.Text = "科室：";
            // 
            // labelControl24
            // 
            this.labelControl24.Location = new System.Drawing.Point(563, 23);
            this.labelControl24.Name = "labelControl24";
            this.labelControl24.Size = new System.Drawing.Size(36, 14);
            this.labelControl24.TabIndex = 9;
            this.labelControl24.Text = "病区：";
            // 
            // textEdit14
            // 
            this.textEdit14.EnterMoveNextControl = true;
            this.textEdit14.Location = new System.Drawing.Point(239, 21);
            this.textEdit14.Name = "textEdit14";
            this.textEdit14.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEdit14.Size = new System.Drawing.Size(120, 20);
            this.textEdit14.TabIndex = 1;
            this.textEdit14.ToolTip = "姓名";
            // 
            // labelControl22
            // 
            this.labelControl22.Location = new System.Drawing.Point(203, 24);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(36, 14);
            this.labelControl22.TabIndex = 7;
            this.labelControl22.Text = "姓名：";
            // 
            // labelControl23
            // 
            this.labelControl23.Location = new System.Drawing.Point(746, 23);
            this.labelControl23.Name = "labelControl23";
            this.labelControl23.Size = new System.Drawing.Size(48, 14);
            this.labelControl23.TabIndex = 10;
            this.labelControl23.Text = "床位号：";
            // 
            // xtraTabPageApproveList
            // 
            this.xtraTabPageApproveList.Name = "xtraTabPageApproveList";
            this.xtraTabPageApproveList.Size = new System.Drawing.Size(1078, 533);
            this.xtraTabPageApproveList.Text = "待审核清单";
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(1078, 533);
            this.xtraTabPage3.Text = "会诊记录";
            // 
            // XtraConfigAudio
            // 
            this.XtraConfigAudio.Name = "XtraConfigAudio";
            this.XtraConfigAudio.Size = new System.Drawing.Size(1078, 533);
            this.XtraConfigAudio.Text = "审核人配置";
            // 
            // lookUpWindow2
            // 
            this.lookUpWindow2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindow2.GenShortCode = null;
            this.lookUpWindow2.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindow2.Owner = null;
            this.lookUpWindow2.SqlHelper = null;
            // 
            // lookUpWindowDepartment
            // 
            this.lookUpWindowDepartment.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDepartment.GenShortCode = null;
            this.lookUpWindowDepartment.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDepartment.Owner = null;
            this.lookUpWindowDepartment.SqlHelper = null;
            // 
            // lookUpWindowEmployee
            // 
            this.lookUpWindowEmployee.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowEmployee.GenShortCode = null;
            this.lookUpWindowEmployee.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowEmployee.Owner = null;
            this.lookUpWindowEmployee.SqlHelper = null;
            // 
            // lookUpWindow1
            // 
            this.lookUpWindow1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindow1.GenShortCode = null;
            this.lookUpWindow1.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindow1.Owner = null;
            this.lookUpWindow1.SqlHelper = null;
            // 
            // FormMainConsultation
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1084, 562);
            this.Controls.Add(this.xtraTabControlRecordList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMainConsultation";
            this.Text = "院内会诊系统";
            this.Load += new System.EventHandler(this.FormMainConsultation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlRecordList)).EndInit();
            this.xtraTabControlRecordList.ResumeLayout(false);
            this.xtraTabPageApplyList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit9.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit15.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit8.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit7.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit14.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindow2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowEmployee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControlRecordList;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageConsultationList;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageApproveList;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageApplyList;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.LabelControl labelControl23;
        private DevExpress.XtraEditors.TextEdit textEdit14;
        private DevExpress.XtraEditors.TextEdit textEdit15;
        private DevExpress.XtraEditors.LabelControl labelControl26;
        private DevExpress.XtraGrid.GridControl gridControl4;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn43;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn44;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn45;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn46;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn47;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn41;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn40;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn39;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn42;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit9;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit8;
        private DevExpress.XtraEditors.LabelControl labelControl24;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit7;
        private DevExpress.XtraEditors.LabelControl labelControl25;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn48;
        private DevExpress.XtraTab.XtraTabPage XtraConfigAudio;
        private Common.Library.LookUpWindow lookUpWindowDepartment;
        private Common.Library.LookUpWindow lookUpWindow1;
        private Common.Library.LookUpWindow lookUpWindowEmployee;
        private Common.Library.LookUpWindow lookUpWindow2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}