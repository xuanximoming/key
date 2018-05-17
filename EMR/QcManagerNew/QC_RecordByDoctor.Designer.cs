namespace YindanSoft.Emr.QcManagerNew
{
    partial class QC_RecordByDoctor
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
            this.gridControlRecord = new DevExpress.XtraGrid.GridControl();
            this.gridViewRecord = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lookUpEditorDepartment = new YidanSoft.Common.Library.LookUpEditor();
            this.lookUpWindowDepartment = new YidanSoft.Common.Library.LookUpWindow();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditorUser = new YidanSoft.Common.Library.LookUpEditor();
            this.lookUpWindowUser = new YidanSoft.Common.Library.LookUpWindow();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.dateEdit_begin = new DevExpress.XtraEditors.DateEdit();
            this.dateEdit_end = new DevExpress.XtraEditors.DateEdit();
            this.simpleButtonReset = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Export = new DevExpress.XtraEditors.SimpleButton();
            this.btn_print = new DevExpress.XtraEditors.SimpleButton();
            this.btn_query = new DevExpress.XtraEditors.SimpleButton();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.labPatCount = new DevExpress.XtraEditors.LabelControl();
            this.labelControlTotalPats = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_begin.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_begin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_end.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_end.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlRecord
            // 
            this.gridControlRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlRecord.Location = new System.Drawing.Point(0, 68);
            this.gridControlRecord.MainView = this.gridViewRecord;
            this.gridControlRecord.Name = "gridControlRecord";
            this.gridControlRecord.Size = new System.Drawing.Size(1054, 486);
            this.gridControlRecord.TabIndex = 1;
            this.gridControlRecord.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewRecord});
            // 
            // gridViewRecord
            // 
            this.gridViewRecord.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Green;
            this.gridViewRecord.Appearance.ViewCaption.Options.UseForeColor = true;
            this.gridViewRecord.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.gridViewRecord.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridViewRecord.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn17,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn12,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn16,
            this.gridColumn13,
            this.gridColumn14,
            this.gridColumn15});
            this.gridViewRecord.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewRecord.GridControl = this.gridControlRecord;
            this.gridViewRecord.IndicatorWidth = 40;
            this.gridViewRecord.Name = "gridViewRecord";
            this.gridViewRecord.OptionsBehavior.Editable = false;
            this.gridViewRecord.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewRecord.OptionsCustomization.AllowFilter = false;
            this.gridViewRecord.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewRecord.OptionsFilter.AllowFilterEditor = false;
            this.gridViewRecord.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewRecord.OptionsMenu.EnableColumnMenu = false;
            this.gridViewRecord.OptionsMenu.EnableFooterMenu = false;
            this.gridViewRecord.OptionsView.ShowFooter = true;
            this.gridViewRecord.OptionsView.ShowGroupPanel = false;
            this.gridViewRecord.OptionsView.ShowViewCaption = true;
            this.gridViewRecord.ViewCaption = "  医师书写病历统计";
            this.gridViewRecord.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridViewRecord_CustomDrawRowIndicator);
            this.gridViewRecord.DoubleClick += new System.EventHandler(this.gridViewRecord_DoubleClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "病案首页号";
            this.gridColumn1.FieldName = "NOOFINPAT";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 78;
            // 
            // gridColumn17
            // 
            this.gridColumn17.Caption = "病案创建时间";
            this.gridColumn17.FieldName = "CREATE_TIME";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 1;
            this.gridColumn17.Width = 80;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "病案创建医生";
            this.gridColumn2.FieldName = "CREATUSER";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "患者ID";
            this.gridColumn3.FieldName = "NOOFINPAT";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Width = 52;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "住院号";
            this.gridColumn4.FieldName = "PATID";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 66;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "姓名";
            this.gridColumn5.FieldName = "NAME";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn5.OptionsFilter.AllowFilter = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 47;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "性别";
            this.gridColumn6.FieldName = "SEX";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn6.OptionsFilter.AllowFilter = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            this.gridColumn6.Width = 31;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "住院次数";
            this.gridColumn7.FieldName = "INCOUNT";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn7.OptionsFilter.AllowFilter = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            this.gridColumn7.Width = 60;
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.Caption = "所在科室";
            this.gridColumn8.FieldName = "DEPT";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn8.OptionsFilter.AllowFilter = false;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            this.gridColumn8.Width = 59;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn9.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.Caption = "入院诊断";
            this.gridColumn9.FieldName = "ADMITDIAG";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn9.OptionsFilter.AllowFilter = false;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 8;
            this.gridColumn9.Width = 102;
            // 
            // gridColumn12
            // 
            this.gridColumn12.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.Caption = "出院诊断";
            this.gridColumn12.FieldName = "OUTDIAG";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 9;
            this.gridColumn12.Width = 102;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn10.Caption = "入院时间";
            this.gridColumn10.FieldName = "ADMITDATE";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn10.OptionsFilter.AllowFilter = false;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 10;
            this.gridColumn10.Width = 65;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn11.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn11.Caption = "出院时间";
            this.gridColumn11.FieldName = "OUTHOSDATE";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn11.OptionsFilter.AllowFilter = false;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 11;
            this.gridColumn11.Width = 65;
            // 
            // gridColumn16
            // 
            this.gridColumn16.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn16.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn16.Caption = "手术名称";
            this.gridColumn16.FieldName = "OPERATION";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 12;
            this.gridColumn16.Width = 94;
            // 
            // gridColumn13
            // 
            this.gridColumn13.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn13.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.Caption = "经治医生";
            this.gridColumn13.FieldName = "CHIEF";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn13.OptionsFilter.AllowFilter = false;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 13;
            this.gridColumn13.Width = 54;
            // 
            // gridColumn14
            // 
            this.gridColumn14.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn14.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn14.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn14.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn14.Caption = "年龄";
            this.gridColumn14.FieldName = "AGE";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn14.OptionsFilter.AllowFilter = false;
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 14;
            this.gridColumn14.Width = 33;
            // 
            // gridColumn15
            // 
            this.gridColumn15.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn15.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn15.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn15.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn15.Caption = "住院天数";
            this.gridColumn15.FieldName = "TOTALDAYS";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn15.OptionsFilter.AllowFilter = false;
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 15;
            this.gridColumn15.Width = 76;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lookUpEditorDepartment);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.lookUpEditorUser);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.dateEdit_begin);
            this.panelControl1.Controls.Add(this.dateEdit_end);
            this.panelControl1.Controls.Add(this.simpleButtonReset);
            this.panelControl1.Controls.Add(this.btn_Export);
            this.panelControl1.Controls.Add(this.btn_print);
            this.panelControl1.Controls.Add(this.btn_query);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1054, 68);
            this.panelControl1.TabIndex = 0;
            // 
            // lookUpEditorDepartment
            // 
            this.lookUpEditorDepartment.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDepartment.ListWindow = this.lookUpWindowDepartment;
            this.lookUpEditorDepartment.Location = new System.Drawing.Point(98, 10);
            this.lookUpEditorDepartment.Name = "lookUpEditorDepartment";
            this.lookUpEditorDepartment.ShowSButton = true;
            this.lookUpEditorDepartment.Size = new System.Drawing.Size(150, 20);
            this.lookUpEditorDepartment.TabIndex = 0;
            this.lookUpEditorDepartment.ToolTip = "支持科室编码、科室名称(汉字/拼音/五笔)查询";
            this.lookUpEditorDepartment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // lookUpWindowDepartment
            // 
            this.lookUpWindowDepartment.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDepartment.GenShortCode = null;
            this.lookUpWindowDepartment.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDepartment.Owner = null;
            this.lookUpWindowDepartment.SqlHelper = null;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(60, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 14);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "科室：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(257, 12);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(84, 14);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "病案创建医生：";
            // 
            // lookUpEditorUser
            // 
            this.lookUpEditorUser.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorUser.ListWindow = this.lookUpWindowUser;
            this.lookUpEditorUser.Location = new System.Drawing.Point(343, 9);
            this.lookUpEditorUser.Name = "lookUpEditorUser";
            this.lookUpEditorUser.ShowSButton = true;
            this.lookUpEditorUser.Size = new System.Drawing.Size(150, 20);
            this.lookUpEditorUser.TabIndex = 1;
            this.lookUpEditorUser.ToolTip = "支持员工工号、员工姓名(汉字/拼音/五笔)查询";
            this.lookUpEditorUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // lookUpWindowUser
            // 
            this.lookUpWindowUser.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowUser.GenShortCode = null;
            this.lookUpWindowUser.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowUser.Owner = null;
            this.lookUpWindowUser.SqlHelper = null;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(289, 41);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(12, 14);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "至";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(12, 41);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(84, 14);
            this.labelControl5.TabIndex = 10;
            this.labelControl5.Text = "病案创建时间：";
            // 
            // dateEdit_begin
            // 
            this.dateEdit_begin.EditValue = null;
            this.dateEdit_begin.Location = new System.Drawing.Point(98, 38);
            this.dateEdit_begin.Name = "dateEdit_begin";
            this.dateEdit_begin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_begin.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit_begin.Size = new System.Drawing.Size(150, 21);
            this.dateEdit_begin.TabIndex = 2;
            this.dateEdit_begin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // dateEdit_end
            // 
            this.dateEdit_end.EditValue = null;
            this.dateEdit_end.Location = new System.Drawing.Point(343, 38);
            this.dateEdit_end.Name = "dateEdit_end";
            this.dateEdit_end.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_end.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit_end.Size = new System.Drawing.Size(150, 21);
            this.dateEdit_end.TabIndex = 3;
            this.dateEdit_end.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // simpleButtonReset
            // 
            this.simpleButtonReset.Image = global::YindanSoft.Emr.QcManagerNew.Properties.Resources.重置;
            this.simpleButtonReset.Location = new System.Drawing.Point(621, 22);
            this.simpleButtonReset.Name = "simpleButtonReset";
            this.simpleButtonReset.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonReset.TabIndex = 5;
            this.simpleButtonReset.Text = "重置(&B)";
            this.simpleButtonReset.Click += new System.EventHandler(this.simpleButtonReset_Click);
            // 
            // btn_Export
            // 
            this.btn_Export.Image = global::YindanSoft.Emr.QcManagerNew.Properties.Resources.导出;
            this.btn_Export.Location = new System.Drawing.Point(807, 22);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(80, 27);
            this.btn_Export.TabIndex = 7;
            this.btn_Export.Text = "导出(&I)";
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // btn_print
            // 
            this.btn_print.Image = global::YindanSoft.Emr.QcManagerNew.Properties.Resources.打印;
            this.btn_print.Location = new System.Drawing.Point(714, 22);
            this.btn_print.Name = "btn_print";
            this.btn_print.Size = new System.Drawing.Size(80, 27);
            this.btn_print.TabIndex = 6;
            this.btn_print.Text = "打印(&P)";
            this.btn_print.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // btn_query
            // 
            this.btn_query.Image = global::YindanSoft.Emr.QcManagerNew.Properties.Resources.查询;
            this.btn_query.Location = new System.Drawing.Point(528, 22);
            this.btn_query.Name = "btn_query";
            this.btn_query.Size = new System.Drawing.Size(80, 27);
            this.btn_query.TabIndex = 4;
            this.btn_query.Text = "查询(&Q)";
            this.btn_query.Click += new System.EventHandler(this.btn_query_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // labPatCount
            // 
            this.labPatCount.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labPatCount.Location = new System.Drawing.Point(71, 532);
            this.labPatCount.Name = "labPatCount";
            this.labPatCount.Size = new System.Drawing.Size(24, 14);
            this.labPatCount.TabIndex = 31;
            this.labPatCount.Text = "人数";
            // 
            // labelControlTotalPats
            // 
            this.labelControlTotalPats.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelControlTotalPats.Location = new System.Drawing.Point(20, 532);
            this.labelControlTotalPats.Name = "labelControlTotalPats";
            this.labelControlTotalPats.Size = new System.Drawing.Size(40, 14);
            this.labelControlTotalPats.TabIndex = 30;
            this.labelControlTotalPats.Text = "总人数:";
            // 
            // QC_RecordByDoctor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labPatCount);
            this.Controls.Add(this.labelControlTotalPats);
            this.Controls.Add(this.gridControlRecord);
            this.Controls.Add(this.panelControl1);
            this.Name = "QC_RecordByDoctor";
            this.Size = new System.Drawing.Size(1054, 554);
            this.SizeChanged += new System.EventHandler(this.QC_RecordByDoctor_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_begin.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_begin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_end.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_end.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlRecord;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewRecord;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonReset;
        private DevExpress.XtraEditors.SimpleButton btn_Export;
        private DevExpress.XtraEditors.SimpleButton btn_print;
        private DevExpress.XtraEditors.SimpleButton btn_query;
        private DevExpress.XtraEditors.DateEdit dateEdit_begin;
        private DevExpress.XtraEditors.DateEdit dateEdit_end;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private YidanSoft.Common.Library.LookUpEditor lookUpEditorDepartment;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private YidanSoft.Common.Library.LookUpEditor lookUpEditorUser;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowDepartment;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowUser;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private DevExpress.XtraEditors.LabelControl labPatCount;
        private DevExpress.XtraEditors.LabelControl labelControlTotalPats;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    }
}
