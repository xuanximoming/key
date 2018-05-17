namespace DrectSoft.Emr.QcManager
{
    partial class OutMedicalScore
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnRYZD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnKouFenRE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.lookUpEditorStatus = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowStatus = new DrectSoft.Common.Library.LookUpWindow(this.components);
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.textEditName = new DevExpress.XtraEditors.TextEdit();
            this.textEditPatID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditEndInTime = new DevExpress.XtraEditors.DateEdit();
            this.dateEditBeginInTime = new DevExpress.XtraEditors.DateEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.lookUpEditorDepartment = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowDepartment = new DrectSoft.Common.Library.LookUpWindow(this.components);
            this.labelControlInfo = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPatID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEndInTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEndInTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBeginInTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBeginInTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 46);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
            this.gridControl1.Size = new System.Drawing.Size(1195, 576);
            this.gridControl1.TabIndex = 8;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.Click += new System.EventHandler(this.gridControl1_Click);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn5,
            this.gridColumn8,
            this.gridColumn4,
            this.gridColumn1,
            this.gridColumn6,
            this.gridColumn14,
            this.gridColumn2,
            this.gridColumn7,
            this.gridColumnRYZD,
            this.gridColumnKouFenRE});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.RowAutoHeight = true;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "首页序号";
            this.gridColumn3.FieldName = "NOOFINPAT";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Width = 52;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "患者姓名";
            this.gridColumn5.FieldName = "PATNAME";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 134;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "科室";
            this.gridColumn8.FieldName = "DEPTNAME";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 1;
            this.gridColumn8.Width = 71;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "床位号";
            this.gridColumn4.FieldName = "BEDID";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 50;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "住院号";
            this.gridColumn1.FieldName = "PATID";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            this.gridColumn1.Width = 100;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "性别";
            this.gridColumn6.FieldName = "SEXNAME";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            this.gridColumn6.Width = 56;
            // 
            // gridColumn14
            // 
            this.gridColumn14.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn14.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn14.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn14.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn14.Caption = "年龄";
            this.gridColumn14.FieldName = "AGE";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 5;
            this.gridColumn14.Width = 56;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "总分数";
            this.gridColumn2.FieldName = "REDPOINT";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 6;
            this.gridColumn2.Width = 56;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "住院次数";
            this.gridColumn7.FieldName = "INCOUNT";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 7;
            this.gridColumn7.Width = 68;
            // 
            // gridColumnRYZD
            // 
            this.gridColumnRYZD.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnRYZD.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnRYZD.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnRYZD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnRYZD.Caption = "入院诊断";
            this.gridColumnRYZD.FieldName = "ADMITDIAGNOSIS";
            this.gridColumnRYZD.Name = "gridColumnRYZD";
            this.gridColumnRYZD.Visible = true;
            this.gridColumnRYZD.VisibleIndex = 8;
            this.gridColumnRYZD.Width = 228;
            // 
            // gridColumnKouFenRE
            // 
            this.gridColumnKouFenRE.Caption = "扣分原因";
            this.gridColumnKouFenRE.ColumnEdit = this.repositoryItemMemoEdit1;
            this.gridColumnKouFenRE.FieldName = "KOUFENLIYOU";
            this.gridColumnKouFenRE.Name = "gridColumnKouFenRE";
            this.gridColumnKouFenRE.Visible = true;
            this.gridColumnKouFenRE.VisibleIndex = 9;
            this.gridColumnKouFenRE.Width = 374;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnPrint);
            this.panelControl1.Controls.Add(this.btnExport);
            this.panelControl1.Controls.Add(this.lookUpEditorStatus);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.textEditName);
            this.panelControl1.Controls.Add(this.textEditPatID);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.dateEditEndInTime);
            this.panelControl1.Controls.Add(this.dateEditBeginInTime);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.btnQuery);
            this.panelControl1.Controls.Add(this.lookUpEditorDepartment);
            this.panelControl1.Controls.Add(this.labelControlInfo);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1195, 46);
            this.panelControl1.TabIndex = 2;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(1093, 11);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 48;
            this.btnPrint.Text = "打印";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(1009, 11);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 47;
            this.btnExport.Text = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lookUpEditorStatus
            // 
            this.lookUpEditorStatus.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorStatus.ListWindow = this.lookUpWindowStatus;
            this.lookUpEditorStatus.Location = new System.Drawing.Point(831, 13);
            this.lookUpEditorStatus.Name = "lookUpEditorStatus";
            this.lookUpEditorStatus.ShowFormImmediately = true;
            this.lookUpEditorStatus.ShowSButton = true;
            this.lookUpEditorStatus.Size = new System.Drawing.Size(87, 20);
            this.lookUpEditorStatus.TabIndex = 6;
            // 
            // lookUpWindowStatus
            // 
            this.lookUpWindowStatus.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowStatus.GenShortCode = null;
            this.lookUpWindowStatus.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowStatus.Owner = null;
            this.lookUpWindowStatus.SqlHelper = null;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(777, 16);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 46;
            this.labelControl6.Text = "病人状态";
            // 
            // textEditName
            // 
            this.textEditName.Location = new System.Drawing.Point(350, 13);
            this.textEditName.Name = "textEditName";
            this.textEditName.Size = new System.Drawing.Size(87, 21);
            this.textEditName.TabIndex = 3;
            // 
            // textEditPatID
            // 
            this.textEditPatID.Location = new System.Drawing.Point(201, 13);
            this.textEditPatID.Name = "textEditPatID";
            this.textEditPatID.Size = new System.Drawing.Size(87, 21);
            this.textEditPatID.TabIndex = 2;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(296, 16);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 43;
            this.labelControl3.Text = "病人姓名";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(159, 16);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 14);
            this.labelControl2.TabIndex = 42;
            this.labelControl2.Text = "病历号";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(627, 14);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(8, 14);
            this.labelControl5.TabIndex = 41;
            this.labelControl5.Text = "--";
            // 
            // dateEditEndInTime
            // 
            this.dateEditEndInTime.EditValue = null;
            this.dateEditEndInTime.Location = new System.Drawing.Point(637, 11);
            this.dateEditEndInTime.Name = "dateEditEndInTime";
            this.dateEditEndInTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditEndInTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateEditEndInTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditEndInTime.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateEditEndInTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditEndInTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditEndInTime.Size = new System.Drawing.Size(123, 21);
            this.dateEditEndInTime.TabIndex = 5;
            // 
            // dateEditBeginInTime
            // 
            this.dateEditBeginInTime.EditValue = null;
            this.dateEditBeginInTime.Location = new System.Drawing.Point(498, 11);
            this.dateEditBeginInTime.Name = "dateEditBeginInTime";
            this.dateEditBeginInTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditBeginInTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateEditBeginInTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditBeginInTime.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateEditBeginInTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditBeginInTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditBeginInTime.Size = new System.Drawing.Size(123, 21);
            this.dateEditBeginInTime.TabIndex = 4;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(444, 14);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 38;
            this.labelControl4.Text = "入院日期";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(924, 11);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lookUpEditorDepartment
            // 
            this.lookUpEditorDepartment.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDepartment.ListWindow = this.lookUpWindowDepartment;
            this.lookUpEditorDepartment.Location = new System.Drawing.Point(39, 13);
            this.lookUpEditorDepartment.Name = "lookUpEditorDepartment";
            this.lookUpEditorDepartment.ShowSButton = true;
            this.lookUpEditorDepartment.Size = new System.Drawing.Size(114, 20);
            this.lookUpEditorDepartment.TabIndex = 1;
            // 
            // lookUpWindowDepartment
            // 
            this.lookUpWindowDepartment.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDepartment.GenShortCode = null;
            this.lookUpWindowDepartment.MatchType = DrectSoft.Common.Library.ShowListMatchType.Full;
            this.lookUpWindowDepartment.Owner = null;
            this.lookUpWindowDepartment.SqlHelper = null;
            // 
            // labelControlInfo
            // 
            this.labelControlInfo.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControlInfo.Location = new System.Drawing.Point(9, 16);
            this.labelControlInfo.Name = "labelControlInfo";
            this.labelControlInfo.Size = new System.Drawing.Size(24, 14);
            this.labelControlInfo.TabIndex = 1;
            this.labelControlInfo.Text = "科室";
            // 
            // OutMedicalScore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControl1);
            this.Name = "OutMedicalScore";
            this.Size = new System.Drawing.Size(1195, 622);
            this.Load += new System.EventHandler(this.OutMedicalScore_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPatID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEndInTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEndInTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBeginInTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBeginInTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnRYZD;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DrectSoft.Common.Library.LookUpEditor lookUpEditorDepartment;
        private DrectSoft.Common.Library.LookUpWindow lookUpWindowDepartment;
        private DevExpress.XtraEditors.LabelControl labelControlInfo;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.DateEdit dateEditEndInTime;
        private DevExpress.XtraEditors.DateEdit dateEditBeginInTime;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit textEditName;
        private DevExpress.XtraEditors.TextEdit textEditPatID;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DrectSoft.Common.Library.LookUpEditor lookUpEditorStatus;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DrectSoft.Common.Library.LookUpWindow lookUpWindowStatus;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnKouFenRE;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
    }
}
