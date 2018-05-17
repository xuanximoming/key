namespace DrectSoft.Core.MainEmrPad.HelpForm
{
    partial class OperForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperForm));
            this.lookUpEditDiagType = new DevExpress.XtraEditors.LookUpEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_deleteOperation = new DrectSoft.Common.Ctrs.OTHER.DevButtonDelete(this.components);
            this.btn_editOperation = new DrectSoft.Common.Ctrs.OTHER.DevButtonEdit(this.components);
            this.btn_addOperation = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd(this.components);
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridViewOper = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn19 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn22 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn23 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn24 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn25 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn26 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn27 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn28 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn29 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn30 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn31 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn32 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn33 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn34 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn35 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn36 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn37 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn38 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.DevButtonClose1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose(this.components);
            this.DevButtonSave1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave(this.components);
            this.simpleButtonInsert = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditDiagType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lookUpEditDiagType
            // 
            this.lookUpEditDiagType.Location = new System.Drawing.Point(21, 12);
            this.lookUpEditDiagType.Name = "lookUpEditDiagType";
            this.lookUpEditDiagType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditDiagType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DIAGNAME", 30, "诊断名称")});
            this.lookUpEditDiagType.Size = new System.Drawing.Size(147, 20);
            this.lookUpEditDiagType.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_deleteOperation);
            this.panelControl1.Controls.Add(this.btn_editOperation);
            this.panelControl1.Controls.Add(this.btn_addOperation);
            this.panelControl1.Controls.Add(this.lookUpEditDiagType);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(999, 48);
            this.panelControl1.TabIndex = 7;
            // 
            // btn_deleteOperation
            // 
            this.btn_deleteOperation.Image = ((System.Drawing.Image)(resources.GetObject("btn_deleteOperation.Image")));
            this.btn_deleteOperation.Location = new System.Drawing.Point(358, 11);
            this.btn_deleteOperation.Name = "btn_deleteOperation";
            this.btn_deleteOperation.Size = new System.Drawing.Size(80, 27);
            this.btn_deleteOperation.TabIndex = 8;
            this.btn_deleteOperation.Text = "删除(&D)";
            this.btn_deleteOperation.ToolTip = "删除手术信息";
            this.btn_deleteOperation.Click += new System.EventHandler(this.btn_deleteOperation_Click);
            // 
            // btn_editOperation
            // 
            this.btn_editOperation.Image = ((System.Drawing.Image)(resources.GetObject("btn_editOperation.Image")));
            this.btn_editOperation.Location = new System.Drawing.Point(272, 11);
            this.btn_editOperation.Name = "btn_editOperation";
            this.btn_editOperation.Size = new System.Drawing.Size(80, 27);
            this.btn_editOperation.TabIndex = 7;
            this.btn_editOperation.Text = "编辑(&E)";
            this.btn_editOperation.ToolTip = "编辑手术信息";
            this.btn_editOperation.Click += new System.EventHandler(this.btn_editOperation_Click);
            // 
            // btn_addOperation
            // 
            this.btn_addOperation.Image = ((System.Drawing.Image)(resources.GetObject("btn_addOperation.Image")));
            this.btn_addOperation.Location = new System.Drawing.Point(186, 11);
            this.btn_addOperation.Name = "btn_addOperation";
            this.btn_addOperation.Size = new System.Drawing.Size(80, 27);
            this.btn_addOperation.TabIndex = 6;
            this.btn_addOperation.Text = "新增(&A)";
            this.btn_addOperation.ToolTip = "新增手术信息";
            this.btn_addOperation.Click += new System.EventHandler(this.btn_addOperation_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gridControl2);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 48);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(999, 277);
            this.panelControl2.TabIndex = 8;
            // 
            // gridControl2
            // 
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(2, 2);
            this.gridControl2.MainView = this.gridViewOper;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(995, 273);
            this.gridControl2.TabIndex = 4;
            this.gridControl2.TabStop = false;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewOper});
            // 
            // gridViewOper
            // 
            this.gridViewOper.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn14,
            this.gridColumn15,
            this.gridColumn16,
            this.gridColumn17,
            this.gridColumn18,
            this.gridColumn19,
            this.gridColumn20,
            this.gridColumn21,
            this.gridColumn22,
            this.gridColumn23,
            this.gridColumn24,
            this.gridColumn25,
            this.gridColumn26,
            this.gridColumn27,
            this.gridColumn28,
            this.gridColumn29,
            this.gridColumn30,
            this.gridColumn31,
            this.gridColumn32,
            this.gridColumn33,
            this.gridColumn34,
            this.gridColumn35,
            this.gridColumn36,
            this.gridColumn37,
            this.gridColumn38});
            this.gridViewOper.GridControl = this.gridControl2;
            this.gridViewOper.IndicatorWidth = 40;
            this.gridViewOper.Name = "gridViewOper";
            this.gridViewOper.OptionsCustomization.AllowFilter = false;
            this.gridViewOper.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewOper.OptionsFilter.AllowFilterEditor = false;
            this.gridViewOper.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewOper.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewOper.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "手术操作码";
            this.gridColumn14.FieldName = "OPERATION_CODE";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.OptionsColumn.AllowEdit = false;
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 0;
            this.gridColumn14.Width = 60;
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "手术操作日期";
            this.gridColumn15.FieldName = "OPERATION_DATE";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.OptionsColumn.AllowEdit = false;
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 1;
            this.gridColumn15.Width = 70;
            // 
            // gridColumn16
            // 
            this.gridColumn16.Caption = "手术等级";
            this.gridColumn16.FieldName = "OPERATION_LEVEL_NAME";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.OptionsColumn.AllowEdit = false;
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 2;
            this.gridColumn16.Width = 45;
            // 
            // gridColumn17
            // 
            this.gridColumn17.Caption = "手术等级编号";
            this.gridColumn17.FieldName = "OPERATION_LEVEL";
            this.gridColumn17.Name = "gridColumn17";
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "手术操作名称";
            this.gridColumn18.FieldName = "OPERATION_NAME";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.OptionsColumn.AllowEdit = false;
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 3;
            this.gridColumn18.Width = 60;
            // 
            // gridColumn19
            // 
            this.gridColumn19.Caption = "术者";
            this.gridColumn19.FieldName = "EXECUTE_USER1_NAME";
            this.gridColumn19.Name = "gridColumn19";
            this.gridColumn19.OptionsColumn.AllowEdit = false;
            this.gridColumn19.Visible = true;
            this.gridColumn19.VisibleIndex = 4;
            this.gridColumn19.Width = 52;
            // 
            // gridColumn20
            // 
            this.gridColumn20.Caption = "术者ID";
            this.gridColumn20.FieldName = "Execute_User1";
            this.gridColumn20.Name = "gridColumn20";
            this.gridColumn20.OptionsColumn.AllowEdit = false;
            // 
            // gridColumn21
            // 
            this.gridColumn21.Caption = "I助";
            this.gridColumn21.FieldName = "EXECUTE_USER2_NAME";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.OptionsColumn.AllowEdit = false;
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 5;
            this.gridColumn21.Width = 52;
            // 
            // gridColumn22
            // 
            this.gridColumn22.Caption = "I助ID";
            this.gridColumn22.FieldName = "Execute_User2";
            this.gridColumn22.Name = "gridColumn22";
            this.gridColumn22.OptionsColumn.AllowEdit = false;
            // 
            // gridColumn23
            // 
            this.gridColumn23.Caption = "II助";
            this.gridColumn23.FieldName = "EXECUTE_USER3_NAME";
            this.gridColumn23.Name = "gridColumn23";
            this.gridColumn23.OptionsColumn.AllowEdit = false;
            this.gridColumn23.Visible = true;
            this.gridColumn23.VisibleIndex = 6;
            this.gridColumn23.Width = 52;
            // 
            // gridColumn24
            // 
            this.gridColumn24.Caption = "II助ID";
            this.gridColumn24.FieldName = "Execute_User3";
            this.gridColumn24.Name = "gridColumn24";
            this.gridColumn24.OptionsColumn.AllowEdit = false;
            // 
            // gridColumn25
            // 
            this.gridColumn25.Caption = "麻醉方式";
            this.gridColumn25.FieldName = "ANAESTHESIA_TYPE_NAME";
            this.gridColumn25.Name = "gridColumn25";
            this.gridColumn25.OptionsColumn.AllowEdit = false;
            this.gridColumn25.Visible = true;
            this.gridColumn25.VisibleIndex = 7;
            this.gridColumn25.Width = 50;
            // 
            // gridColumn26
            // 
            this.gridColumn26.Caption = "麻醉方式ID";
            this.gridColumn26.FieldName = "Anaesthesia_Type_Id";
            this.gridColumn26.Name = "gridColumn26";
            this.gridColumn26.OptionsColumn.AllowEdit = false;
            // 
            // gridColumn27
            // 
            this.gridColumn27.Caption = "切口愈合等级";
            this.gridColumn27.FieldName = "CLOSE_LEVEL_NAME";
            this.gridColumn27.Name = "gridColumn27";
            this.gridColumn27.OptionsColumn.AllowEdit = false;
            this.gridColumn27.Visible = true;
            this.gridColumn27.VisibleIndex = 8;
            this.gridColumn27.Width = 50;
            // 
            // gridColumn28
            // 
            this.gridColumn28.Caption = "切口愈合等级ID";
            this.gridColumn28.FieldName = "Close_Level";
            this.gridColumn28.Name = "gridColumn28";
            this.gridColumn28.OptionsColumn.AllowEdit = false;
            // 
            // gridColumn29
            // 
            this.gridColumn29.Caption = "麻醉医师";
            this.gridColumn29.FieldName = "ANAESTHESIA_USER_NAME";
            this.gridColumn29.Name = "gridColumn29";
            this.gridColumn29.OptionsColumn.AllowEdit = false;
            this.gridColumn29.Visible = true;
            this.gridColumn29.VisibleIndex = 9;
            this.gridColumn29.Width = 60;
            // 
            // gridColumn30
            // 
            this.gridColumn30.Caption = "麻醉医师ID";
            this.gridColumn30.FieldName = "Anaesthesia_User";
            this.gridColumn30.Name = "gridColumn30";
            this.gridColumn30.OptionsColumn.AllowEdit = false;
            // 
            // gridColumn31
            // 
            this.gridColumn31.Caption = "是否择期手术";
            this.gridColumn31.FieldName = "ISCHOOSEDATENAME";
            this.gridColumn31.Name = "gridColumn31";
            this.gridColumn31.OptionsColumn.AllowEdit = false;
            this.gridColumn31.Visible = true;
            this.gridColumn31.VisibleIndex = 10;
            this.gridColumn31.Width = 60;
            // 
            // gridColumn32
            // 
            this.gridColumn32.Caption = "是否无菌手术";
            this.gridColumn32.FieldName = "ISCLEAROPENAME";
            this.gridColumn32.Name = "gridColumn32";
            this.gridColumn32.OptionsColumn.AllowEdit = false;
            this.gridColumn32.Visible = true;
            this.gridColumn32.VisibleIndex = 11;
            this.gridColumn32.Width = 60;
            // 
            // gridColumn33
            // 
            this.gridColumn33.Caption = "是否感染";
            this.gridColumn33.FieldName = "ISGANRANNAME";
            this.gridColumn33.Name = "gridColumn33";
            this.gridColumn33.OptionsColumn.AllowEdit = false;
            this.gridColumn33.Visible = true;
            this.gridColumn33.VisibleIndex = 12;
            this.gridColumn33.Width = 60;
            // 
            // gridColumn34
            // 
            this.gridColumn34.Caption = "是否择期手术编号";
            this.gridColumn34.FieldName = "ISCHOOSEDATEID";
            this.gridColumn34.Name = "gridColumn34";
            // 
            // gridColumn35
            // 
            this.gridColumn35.Caption = "是否无菌手术编号";
            this.gridColumn35.FieldName = "ISCLEAROPEID";
            this.gridColumn35.Name = "gridColumn35";
            // 
            // gridColumn36
            // 
            this.gridColumn36.Caption = "是否感染编号";
            this.gridColumn36.FieldName = "ISGANRANID";
            this.gridColumn36.Name = "gridColumn36";
            // 
            // gridColumn37
            // 
            this.gridColumn37.Caption = "麻醉分级";
            this.gridColumn37.FieldName = "ANESTHESIA_LEVEL";
            this.gridColumn37.Name = "gridColumn37";
            this.gridColumn37.Visible = true;
            this.gridColumn37.VisibleIndex = 13;
            // 
            // gridColumn38
            // 
            this.gridColumn38.Caption = "手术并发症";
            this.gridColumn38.FieldName = "OPERCOMPLICATION_CODE";
            this.gridColumn38.Name = "gridColumn38";
            this.gridColumn38.Visible = true;
            this.gridColumn38.VisibleIndex = 14;
            this.gridColumn38.Width = 60;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.DevButtonClose1);
            this.panelControl3.Controls.Add(this.DevButtonSave1);
            this.panelControl3.Controls.Add(this.simpleButtonInsert);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 325);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(999, 50);
            this.panelControl3.TabIndex = 9;
            // 
            // DevButtonClose1
            // 
            this.DevButtonClose1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DevButtonClose1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonClose1.Image")));
            this.DevButtonClose1.Location = new System.Drawing.Point(911, 13);
            this.DevButtonClose1.Name = "DevButtonClose1";
            this.DevButtonClose1.Size = new System.Drawing.Size(80, 23);
            this.DevButtonClose1.TabIndex = 9;
            this.DevButtonClose1.Text = "关闭(&T)";
            this.DevButtonClose1.Click += new System.EventHandler(this.simpleButtonExit_Click);
            // 
            // DevButtonSave1
            // 
            this.DevButtonSave1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DevButtonSave1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonSave1.Image")));
            this.DevButtonSave1.Location = new System.Drawing.Point(659, 13);
            this.DevButtonSave1.Name = "DevButtonSave1";
            this.DevButtonSave1.Size = new System.Drawing.Size(80, 23);
            this.DevButtonSave1.TabIndex = 8;
            this.DevButtonSave1.Text = "保存(&S)";
            this.DevButtonSave1.Click += new System.EventHandler(this.simpleButtonSave_Click);
            // 
            // simpleButtonInsert
            // 
            this.simpleButtonInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonInsert.Location = new System.Drawing.Point(745, 13);
            this.simpleButtonInsert.Name = "simpleButtonInsert";
            this.simpleButtonInsert.Size = new System.Drawing.Size(160, 23);
            this.simpleButtonInsert.TabIndex = 6;
            this.simpleButtonInsert.Text = "插入手术并关闭";
            this.simpleButtonInsert.Click += new System.EventHandler(this.simpleButtonInsert_Click);
            // 
            // OperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 375);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OperForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "手术管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiagForm_FormClosing);
            this.Load += new System.EventHandler(this.DiagForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditDiagType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit lookUpEditDiagType;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButtonInsert;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave DevButtonSave1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClose DevButtonClose1;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewOper;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn19;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn20;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn21;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn22;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn23;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn24;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn25;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn26;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn27;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn28;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn29;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn30;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn31;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn32;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn33;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn34;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn35;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn36;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn37;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn38;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonDelete btn_deleteOperation;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonEdit btn_editOperation;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd btn_addOperation;
    }
}