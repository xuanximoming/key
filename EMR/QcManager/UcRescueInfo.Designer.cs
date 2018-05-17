namespace DrectSoft.Emr.QcManager
{
    partial class UcRescueInfo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcRescueInfo));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonReset = new DevExpress.XtraEditors.SimpleButton();
            this.DevButtonImportExcel1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonImportExcel(this.components);
            this.DevButtonPrint1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonPrint(this.components);
            this.DevButtonQurey1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey(this.components);
            this.lookUpEditorDepartment = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowDepartment = new DrectSoft.Common.Library.LookUpWindow(this.components);
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dateEdit_begin = new DevExpress.XtraEditors.DateEdit();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dateEdit_end = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.labPatCount = new DevExpress.XtraEditors.LabelControl();
            this.labelControlTotalPats = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_begin.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_begin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_end.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_end.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 39);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1483, 589);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.TabStop = false;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Green;
            this.gridView1.Appearance.ViewCaption.Options.UseForeColor = true;
            this.gridView1.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.gridView1.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn3,
            this.gridColumn2,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn15,
            this.gridColumn12,
            this.gridColumn16,
            this.gridColumn13,
            this.gridColumn14});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.IndicatorWidth = 40;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowViewCaption = true;
            this.gridView1.ViewCaption = "  抢救信息统计";
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "序号";
            this.gridColumn1.FieldName = "XH";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Width = 60;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "住院号";
            this.gridColumn3.FieldName = "PATID";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 100;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "姓名";
            this.gridColumn2.FieldName = "NAME";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 80;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "性别";
            this.gridColumn4.FieldName = "PATSEX";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 45;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "年龄";
            this.gridColumn5.FieldName = "AGESTR";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            this.gridColumn5.Width = 60;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "诊断";
            this.gridColumn6.FieldName = "DIAG_NAME";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            this.gridColumn6.Width = 120;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "入院日期";
            this.gridColumn7.FieldName = "ADMITDATE";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 5;
            this.gridColumn7.Width = 150;
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.Caption = "出院日期";
            this.gridColumn8.FieldName = "OUTHOSDATE";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 6;
            this.gridColumn8.Width = 150;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.Caption = "住址";
            this.gridColumn9.FieldName = "ADDRESS";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 7;
            this.gridColumn9.Width = 100;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn10.Caption = "天数";
            this.gridColumn10.FieldName = "INCOUNT";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 8;
            this.gridColumn10.Width = 87;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn11.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn11.Caption = "抢救次数";
            this.gridColumn11.FieldName = "QJCS";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Width = 87;
            // 
            // gridColumn15
            // 
            this.gridColumn15.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn15.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn15.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn15.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn15.Caption = "成功次数";
            this.gridColumn15.FieldName = "CGCS";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.Width = 87;
            // 
            // gridColumn12
            // 
            this.gridColumn12.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.Caption = "疗效";
            this.gridColumn12.FieldName = "LX";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Width = 87;
            // 
            // gridColumn16
            // 
            this.gridColumn16.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn16.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn16.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn16.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn16.Caption = "费用";
            this.gridColumn16.FieldName = "FEE";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.Width = 66;
            // 
            // gridColumn13
            // 
            this.gridColumn13.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.Caption = "科室";
            this.gridColumn13.FieldName = "DEPT_NAME";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 10;
            this.gridColumn13.Width = 100;
            // 
            // gridColumn14
            // 
            this.gridColumn14.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn14.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn14.Caption = "经治医生";
            this.gridColumn14.FieldName = "DOC_NAME";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 11;
            this.gridColumn14.Width = 102;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.simpleButtonReset);
            this.panelControl2.Controls.Add(this.DevButtonImportExcel1);
            this.panelControl2.Controls.Add(this.DevButtonPrint1);
            this.panelControl2.Controls.Add(this.DevButtonQurey1);
            this.panelControl2.Controls.Add(this.lookUpEditorDepartment);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.dateEdit_begin);
            this.panelControl2.Controls.Add(this.dateEdit_end);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.labelControl22);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1483, 39);
            this.panelControl2.TabIndex = 0;
            // 
            // simpleButtonReset
            // 
            this.simpleButtonReset.Image = global::DrectSoft.Emr.QcManager.Properties.Resources.重置;
            this.simpleButtonReset.Location = new System.Drawing.Point(619, 8);
            this.simpleButtonReset.Name = "simpleButtonReset";
            this.simpleButtonReset.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonReset.TabIndex = 4;
            this.simpleButtonReset.Text = "重置(&B)";
            this.simpleButtonReset.Click += new System.EventHandler(this.simpleButtonReset_Click);
            // 
            // DevButtonImportExcel1
            // 
            this.DevButtonImportExcel1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonImportExcel1.Image")));
            this.DevButtonImportExcel1.ImageSelect = DrectSoft.Common.Ctrs.OTHER.DevButtonImportExcel.ImageTypes.ImportExcel;
            this.DevButtonImportExcel1.Location = new System.Drawing.Point(817, 8);
            this.DevButtonImportExcel1.Name = "DevButtonImportExcel1";
            this.DevButtonImportExcel1.Size = new System.Drawing.Size(80, 27);
            this.DevButtonImportExcel1.TabIndex = 6;
            this.DevButtonImportExcel1.Text = "导出(&I)";
            this.DevButtonImportExcel1.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // DevButtonPrint1
            // 
            this.DevButtonPrint1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonPrint1.Image")));
            this.DevButtonPrint1.Location = new System.Drawing.Point(718, 8);
            this.DevButtonPrint1.Name = "DevButtonPrint1";
            this.DevButtonPrint1.Size = new System.Drawing.Size(80, 27);
            this.DevButtonPrint1.TabIndex = 5;
            this.DevButtonPrint1.Text = "打印(&P)";
            this.DevButtonPrint1.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // DevButtonQurey1
            // 
            this.DevButtonQurey1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonQurey1.Image")));
            this.DevButtonQurey1.Location = new System.Drawing.Point(520, 8);
            this.DevButtonQurey1.Name = "DevButtonQurey1";
            this.DevButtonQurey1.Size = new System.Drawing.Size(80, 27);
            this.DevButtonQurey1.TabIndex = 3;
            this.DevButtonQurey1.Text = "查询(&Q)";
            this.DevButtonQurey1.Click += new System.EventHandler(this.btn_query_Click);
            // 
            // lookUpEditorDepartment
            // 
            this.lookUpEditorDepartment.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDepartment.ListWindow = this.lookUpWindowDepartment;
            this.lookUpEditorDepartment.Location = new System.Drawing.Point(55, 11);
            this.lookUpEditorDepartment.Name = "lookUpEditorDepartment";
            this.lookUpEditorDepartment.ShowSButton = true;
            this.lookUpEditorDepartment.Size = new System.Drawing.Size(150, 20);
            this.lookUpEditorDepartment.TabIndex = 0;
            this.lookUpEditorDepartment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // lookUpWindowDepartment
            // 
            this.lookUpWindowDepartment.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDepartment.GenShortCode = null;
            this.lookUpWindowDepartment.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDepartment.Owner = null;
            this.lookUpWindowDepartment.SqlHelper = null;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 14);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "科室：";
            // 
            // dateEdit_begin
            // 
            this.dateEdit_begin.EditValue = null;
            this.dateEdit_begin.Location = new System.Drawing.Point(281, 11);
            this.dateEdit_begin.Name = "dateEdit_begin";
            this.dateEdit_begin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_begin.Properties.ContextMenuStrip = this.contextMenuStrip;
            this.dateEdit_begin.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit_begin.Size = new System.Drawing.Size(103, 21);
            this.dateEdit_begin.TabIndex = 1;
            this.dateEdit_begin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // dateEdit_end
            // 
            this.dateEdit_end.EditValue = null;
            this.dateEdit_end.Location = new System.Drawing.Point(406, 11);
            this.dateEdit_end.Name = "dateEdit_end";
            this.dateEdit_end.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_end.Properties.ContextMenuStrip = this.contextMenuStrip;
            this.dateEdit_end.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit_end.Size = new System.Drawing.Size(103, 21);
            this.dateEdit_end.TabIndex = 2;
            this.dateEdit_end.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(389, 14);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(12, 14);
            this.labelControl2.TabIndex = 9;
            this.labelControl2.Text = "至";
            // 
            // labelControl22
            // 
            this.labelControl22.Location = new System.Drawing.Point(215, 14);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(60, 14);
            this.labelControl22.TabIndex = 8;
            this.labelControl22.Text = "入院时间：";
            // 
            // labPatCount
            // 
            this.labPatCount.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labPatCount.Location = new System.Drawing.Point(68, 605);
            this.labPatCount.Name = "labPatCount";
            this.labPatCount.Size = new System.Drawing.Size(24, 14);
            this.labPatCount.TabIndex = 23;
            this.labPatCount.Text = "人数";
            // 
            // labelControlTotalPats
            // 
            this.labelControlTotalPats.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelControlTotalPats.Location = new System.Drawing.Point(17, 605);
            this.labelControlTotalPats.Name = "labelControlTotalPats";
            this.labelControlTotalPats.Size = new System.Drawing.Size(40, 14);
            this.labelControlTotalPats.TabIndex = 22;
            this.labelControlTotalPats.Text = "总人数:";
            // 
            // UcRescueInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labPatCount);
            this.Controls.Add(this.labelControlTotalPats);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControl2);
            this.Name = "UcRescueInfo";
            this.Size = new System.Drawing.Size(1483, 628);
            this.Load += new System.EventHandler(this.UcRescueInfo_Load);
            this.SizeChanged += new System.EventHandler(this.UcRescueInfo_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_begin.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_begin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_end.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_end.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DrectSoft.Common.Library.LookUpEditor lookUpEditorDepartment;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dateEdit_begin;
        private DevExpress.XtraEditors.DateEdit dateEdit_end;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DrectSoft.Common.Library.LookUpWindow lookUpWindowDepartment;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonImportExcel DevButtonImportExcel1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonPrint DevButtonPrint1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey DevButtonQurey1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonReset;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private DevExpress.XtraEditors.LabelControl labPatCount;
        private DevExpress.XtraEditors.LabelControl labelControlTotalPats;
    }
}
