namespace MedicalRecordManage.UCControl
{
    partial class MedIemArchive
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MedIemArchive));
            this.scrolInpIemInfo = new DevExpress.XtraEditors.XtraScrollableControl();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gcInpatient = new DevExpress.XtraGrid.GridControl();
            this.gvInpatient = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.txtpatid = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.dateLeaveEnd = new DevExpress.XtraEditors.DateEdit();
            this.dateLeaveStart = new DevExpress.XtraEditors.DateEdit();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_reback = new DevExpress.XtraEditors.SimpleButton();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.cbxLockStatus = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblIslock = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditorDepartment = new DrectSoft.Common.Library.LookUpEditor();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpWindowDepartment = new DrectSoft.Common.Library.LookUpWindow(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcInpatient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInpatient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtpatid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateLeaveEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateLeaveEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateLeaveStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateLeaveStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxLockStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).BeginInit();
            this.SuspendLayout();
            // 
            // scrolInpIemInfo
            // 
            this.scrolInpIemInfo.Appearance.BackColor = System.Drawing.Color.White;
            this.scrolInpIemInfo.Appearance.Options.UseBackColor = true;
            this.scrolInpIemInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrolInpIemInfo.Location = new System.Drawing.Point(277, 0);
            this.scrolInpIemInfo.Name = "scrolInpIemInfo";
            this.scrolInpIemInfo.Size = new System.Drawing.Size(782, 631);
            this.scrolInpIemInfo.TabIndex = 4;
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(272, 0);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 631);
            this.splitterControl1.TabIndex = 5;
            this.splitterControl1.TabStop = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(272, 631);
            this.panelControl1.TabIndex = 3;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gcInpatient);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(2, 177);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(268, 452);
            this.panelControl3.TabIndex = 1;
            // 
            // gcInpatient
            // 
            this.gcInpatient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcInpatient.Location = new System.Drawing.Point(2, 2);
            this.gcInpatient.MainView = this.gvInpatient;
            this.gcInpatient.Name = "gcInpatient";
            this.gcInpatient.Size = new System.Drawing.Size(264, 448);
            this.gcInpatient.TabIndex = 8;
            this.gcInpatient.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvInpatient});
            // 
            // gvInpatient
            // 
            this.gvInpatient.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gvInpatient.GridControl = this.gcInpatient;
            this.gvInpatient.IndicatorWidth = 40;
            this.gvInpatient.Name = "gvInpatient";
            this.gvInpatient.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvInpatient.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvInpatient.OptionsBehavior.Editable = false;
            this.gvInpatient.OptionsCustomization.AllowColumnMoving = false;
            this.gvInpatient.OptionsCustomization.AllowFilter = false;
            this.gvInpatient.OptionsCustomization.AllowGroup = false;
            this.gvInpatient.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvInpatient.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gvInpatient.OptionsFilter.AllowFilterEditor = false;
            this.gvInpatient.OptionsFilter.AllowMRUFilterList = false;
            this.gvInpatient.OptionsMenu.EnableColumnMenu = false;
            this.gvInpatient.OptionsView.ShowGroupPanel = false;
            this.gvInpatient.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvInpatient_CustomDrawRowIndicator);
            this.gvInpatient.DoubleClick += new System.EventHandler(this.gvInpatient_DoubleClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "姓名";
            this.gridColumn1.FieldName = "NAME";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 64;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "住院号";
            this.gridColumn2.FieldName = "PATID";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 89;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "床号";
            this.gridColumn3.FieldName = "OUTBED";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 69;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "gridColumn4";
            this.gridColumn4.FieldName = "islock";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.txtpatid);
            this.panelControl2.Controls.Add(this.labelControl7);
            this.panelControl2.Controls.Add(this.dateLeaveEnd);
            this.panelControl2.Controls.Add(this.dateLeaveStart);
            this.panelControl2.Controls.Add(this.txtName);
            this.panelControl2.Controls.Add(this.labelControl6);
            this.panelControl2.Controls.Add(this.labelControl5);
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.btn_reback);
            this.panelControl2.Controls.Add(this.btnQuery);
            this.panelControl2.Controls.Add(this.cbxLockStatus);
            this.panelControl2.Controls.Add(this.lblIslock);
            this.panelControl2.Controls.Add(this.labelControl9);
            this.panelControl2.Controls.Add(this.labelControl4);
            this.panelControl2.Controls.Add(this.lookUpEditorDepartment);
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(2, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(268, 175);
            this.panelControl2.TabIndex = 0;
            // 
            // txtpatid
            // 
            this.txtpatid.Location = new System.Drawing.Point(186, 52);
            this.txtpatid.Name = "txtpatid";
            this.txtpatid.Size = new System.Drawing.Size(79, 20);
            this.txtpatid.TabIndex = 45;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(139, 55);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(48, 14);
            this.labelControl7.TabIndex = 44;
            this.labelControl7.Text = "住院号：";
            // 
            // dateLeaveEnd
            // 
            this.dateLeaveEnd.EditValue = null;
            this.dateLeaveEnd.Location = new System.Drawing.Point(174, 77);
            this.dateLeaveEnd.Name = "dateLeaveEnd";
            this.dateLeaveEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateLeaveEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateLeaveEnd.Size = new System.Drawing.Size(94, 20);
            this.dateLeaveEnd.TabIndex = 43;
            // 
            // dateLeaveStart
            // 
            this.dateLeaveStart.EditValue = null;
            this.dateLeaveStart.Location = new System.Drawing.Point(57, 77);
            this.dateLeaveStart.Name = "dateLeaveStart";
            this.dateLeaveStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateLeaveStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateLeaveStart.Size = new System.Drawing.Size(102, 20);
            this.dateLeaveStart.TabIndex = 42;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(59, 52);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(79, 20);
            this.txtName.TabIndex = 41;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl6.Location = new System.Drawing.Point(2, 156);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(104, 14);
            this.labelControl6.TabIndex = 40;
            this.labelControl6.Text = "第三步：归档操作";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl5.Location = new System.Drawing.Point(1, 131);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(234, 14);
            this.labelControl5.TabIndex = 39;
            this.labelControl5.Text = "第二步：双击病人列表右侧展现病案首页";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(186, 152);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 23);
            this.btnCancel.TabIndex = 38;
            this.btnCancel.Text = "撤销归档";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btn_reback
            // 
            this.btn_reback.Image = ((System.Drawing.Image)(resources.GetObject("btn_reback.Image")));
            this.btn_reback.Location = new System.Drawing.Point(109, 152);
            this.btn_reback.Name = "btn_reback";
            this.btn_reback.Size = new System.Drawing.Size(75, 23);
            this.btn_reback.TabIndex = 37;
            this.btn_reback.Text = "归档 (&R)";
            this.btn_reback.Click += new System.EventHandler(this.btn_reback_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnQuery.Image")));
            this.btnQuery.Location = new System.Drawing.Point(174, 102);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "查询 (&Q)";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // cbxLockStatus
            // 
            this.cbxLockStatus.EditValue = "已提交";
            this.cbxLockStatus.Location = new System.Drawing.Point(59, 103);
            this.cbxLockStatus.Name = "cbxLockStatus";
            this.cbxLockStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxLockStatus.Properties.Items.AddRange(new object[] {
            "已提交",
              "补写提交",
            "已归档",
            "撤销归档"});
            this.cbxLockStatus.Size = new System.Drawing.Size(100, 20);
            this.cbxLockStatus.TabIndex = 5;
            // 
            // lblIslock
            // 
            this.lblIslock.Location = new System.Drawing.Point(3, 106);
            this.lblIslock.Name = "lblIslock";
            this.lblIslock.Size = new System.Drawing.Size(60, 14);
            this.lblIslock.TabIndex = 35;
            this.lblIslock.Text = "归档状态：";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(161, 80);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(12, 14);
            this.labelControl9.TabIndex = 33;
            this.labelControl9.Text = "至";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(3, 79);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 26;
            this.labelControl4.Text = "入院日期：";
            // 
            // lookUpEditorDepartment
            // 
            this.lookUpEditorDepartment.EnterMoveNextControl = true;
            this.lookUpEditorDepartment.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDepartment.ListWindow = null;
            this.lookUpEditorDepartment.ListWordbookName = "";
            this.lookUpEditorDepartment.Location = new System.Drawing.Point(59, 26);
            this.lookUpEditorDepartment.Name = "lookUpEditorDepartment";
            this.lookUpEditorDepartment.ShowFormImmediately = true;
            this.lookUpEditorDepartment.ShowSButton = true;
            this.lookUpEditorDepartment.Size = new System.Drawing.Size(104, 18);
            this.lookUpEditorDepartment.TabIndex = 1;
            this.lookUpEditorDepartment.ToolTip = "支持科室编码、科室名称(汉字/拼音/五笔)查询";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(3, 55);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "病人姓名：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(2, 27);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "所在科室：";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Location = new System.Drawing.Point(1, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(130, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "第一步：选择筛选条件";
            // 
            // lookUpWindowDepartment
            // 
            this.lookUpWindowDepartment.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDepartment.GenShortCode = null;
            this.lookUpWindowDepartment.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDepartment.Owner = null;
            this.lookUpWindowDepartment.SqlHelper = null;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // MedIemArchive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scrolInpIemInfo);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.panelControl1);
            this.Name = "MedIemArchive";
            this.Size = new System.Drawing.Size(1059, 631);
            this.Load += new System.EventHandler(this.MedIemArchive_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcInpatient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInpatient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtpatid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateLeaveEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateLeaveEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateLeaveStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateLeaveStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxLockStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.XtraScrollableControl scrolInpIemInfo;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DrectSoft.Common.Library.LookUpEditor lookUpEditorDepartment;
        private DrectSoft.Common.Library.LookUpWindow lookUpWindowDepartment;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.ComboBoxEdit cbxLockStatus;
        private DevExpress.XtraEditors.LabelControl lblIslock;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btn_reback;
        private DevExpress.XtraGrid.GridControl gcInpatient;
        public DevExpress.XtraGrid.Views.Grid.GridView gvInpatient;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.DateEdit dateLeaveStart;
        private DevExpress.XtraEditors.DateEdit dateLeaveEnd;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraEditors.TextEdit txtpatid;
        private DevExpress.XtraEditors.LabelControl labelControl7;
    }
}
