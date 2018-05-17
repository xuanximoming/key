namespace MedicalRecordManage.UCControl
{
    partial class MedicalRecordBrowse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MedicalRecordBrowse));
            this.panelHead = new DevExpress.XtraEditors.PanelControl();
            this.btnRefresh = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset();
            this.btnQuery = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey();
            this.btnApply = new DevExpress.XtraEditors.SimpleButton();
            this.dateEnd = new DevExpress.XtraEditors.DateEdit();
            this.dateStart = new DevExpress.XtraEditors.DateEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.dbGrid = new DevExpress.XtraGrid.GridControl();
            this.dbGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.C1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C17 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelHead)).BeginInit();
            this.panelHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStart.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHead
            // 
            this.panelHead.Appearance.BackColor = System.Drawing.Color.White;
            this.panelHead.Appearance.Options.UseBackColor = true;
            this.panelHead.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelHead.Controls.Add(this.btnRefresh);
            this.panelHead.Controls.Add(this.btnQuery);
            this.panelHead.Controls.Add(this.btnApply);
            this.panelHead.Controls.Add(this.dateEnd);
            this.panelHead.Controls.Add(this.dateStart);
            this.panelHead.Controls.Add(this.labelControl8);
            this.panelHead.Controls.Add(this.labelControl4);
            this.panelHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHead.Location = new System.Drawing.Point(5, 5);
            this.panelHead.Name = "panelHead";
            this.panelHead.Size = new System.Drawing.Size(1049, 59);
            this.panelHead.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(428, 17);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 23);
            this.btnRefresh.TabIndex = 60;
            this.btnRefresh.Text = "重置(&B)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnQuery.Image")));
            this.btnQuery.Location = new System.Drawing.Point(343, 17);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 23);
            this.btnQuery.TabIndex = 50;
            this.btnQuery.Text = "查询(&Q)";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnApply
            // 
            this.btnApply.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.Image")));
            this.btnApply.Location = new System.Drawing.Point(513, 17);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 70;
            this.btnApply.Text = "借阅申请";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // dateEnd
            // 
            this.dateEnd.EditValue = null;
            this.dateEnd.Location = new System.Drawing.Point(224, 19);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEnd.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEnd.Size = new System.Drawing.Size(100, 21);
            this.dateEnd.TabIndex = 40;
            this.dateEnd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dateEnd_KeyPress);
            // 
            // dateStart
            // 
            this.dateStart.EditValue = null;
            this.dateStart.Location = new System.Drawing.Point(80, 19);
            this.dateStart.Name = "dateStart";
            this.dateStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateStart.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateStart.Size = new System.Drawing.Size(100, 21);
            this.dateStart.TabIndex = 30;
            this.dateStart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dateStart_KeyPress);
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(194, 20);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(12, 14);
            this.labelControl8.TabIndex = 38;
            this.labelControl8.Text = "至";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(10, 20);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 37;
            this.labelControl4.Text = "申请日期：";
            // 
            // dbGrid
            // 
            this.dbGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbGrid.Location = new System.Drawing.Point(5, 64);
            this.dbGrid.MainView = this.dbGridView;
            this.dbGrid.Name = "dbGrid";
            this.dbGrid.Size = new System.Drawing.Size(1049, 430);
            this.dbGrid.TabIndex = 9;
            this.dbGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dbGridView});
            this.dbGrid.DoubleClick += new System.EventHandler(this.dbGrid_DoubleClick);
            // 
            // dbGridView
            // 
            this.dbGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.C1,
            this.C3,
            this.C4,
            this.C5,
            this.C6,
            this.C7,
            this.C8,
            this.C9,
            this.C10,
            this.C11,
            this.C12,
            this.C13,
            this.C15,
            this.C16,
            this.C14,
            this.C2,
            this.C17});
            this.dbGridView.GridControl = this.dbGrid;
            this.dbGridView.IndicatorWidth = 40;
            this.dbGridView.Name = "dbGridView";
            this.dbGridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dbGridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dbGridView.OptionsBehavior.Editable = false;
            this.dbGridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseUp;
            this.dbGridView.OptionsCustomization.AllowColumnMoving = false;
            this.dbGridView.OptionsCustomization.AllowFilter = false;
            this.dbGridView.OptionsFilter.AllowColumnMRUFilterList = false;
            this.dbGridView.OptionsFilter.AllowFilterEditor = false;
            this.dbGridView.OptionsFilter.AllowMRUFilterList = false;
            this.dbGridView.OptionsMenu.EnableColumnMenu = false;
            this.dbGridView.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.dbGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.dbGridView.OptionsView.ShowDetailButtons = false;
            this.dbGridView.OptionsView.ShowGroupPanel = false;
            this.dbGridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.dbGridView_CustomDrawRowIndicator);
            this.dbGridView.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.dbGridView_CustomDrawCell);
            // 
            // C1
            // 
            this.C1.AppearanceHeader.Options.UseTextOptions = true;
            this.C1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C1.Caption = "ID";
            this.C1.FieldName = "ID";
            this.C1.ImageAlignment = System.Drawing.StringAlignment.Far;
            this.C1.Name = "C1";
            // 
            // C3
            // 
            this.C3.AppearanceCell.Options.UseTextOptions = true;
            this.C3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.C3.AppearanceHeader.Options.UseTextOptions = true;
            this.C3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C3.Caption = "申请日期";
            this.C3.FieldName = "SQSJ";
            this.C3.MinWidth = 130;
            this.C3.Name = "C3";
            this.C3.Visible = true;
            this.C3.VisibleIndex = 0;
            this.C3.Width = 130;
            // 
            // C4
            // 
            this.C4.AppearanceCell.Options.UseTextOptions = true;
            this.C4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.C4.AppearanceHeader.Options.UseTextOptions = true;
            this.C4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C4.Caption = "住院号";
            this.C4.FieldName = "PATID";
            this.C4.MinWidth = 80;
            this.C4.Name = "C4";
            this.C4.Visible = true;
            this.C4.VisibleIndex = 1;
            this.C4.Width = 80;
            // 
            // C5
            // 
            this.C5.AppearanceCell.Options.UseTextOptions = true;
            this.C5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.C5.AppearanceHeader.Options.UseTextOptions = true;
            this.C5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C5.Caption = "姓名";
            this.C5.FieldName = "NAME";
            this.C5.MinWidth = 80;
            this.C5.Name = "C5";
            this.C5.Visible = true;
            this.C5.VisibleIndex = 2;
            this.C5.Width = 80;
            // 
            // C6
            // 
            this.C6.AppearanceCell.Options.UseTextOptions = true;
            this.C6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.C6.AppearanceHeader.Options.UseTextOptions = true;
            this.C6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C6.Caption = "出院科室";
            this.C6.FieldName = "CYKS";
            this.C6.Name = "C6";
            this.C6.Visible = true;
            this.C6.VisibleIndex = 3;
            this.C6.Width = 85;
            // 
            // C7
            // 
            this.C7.AppearanceCell.Options.UseTextOptions = true;
            this.C7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.C7.AppearanceHeader.Options.UseTextOptions = true;
            this.C7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C7.Caption = "年龄";
            this.C7.FieldName = "AGE";
            this.C7.MinWidth = 50;
            this.C7.Name = "C7";
            this.C7.Visible = true;
            this.C7.VisibleIndex = 4;
            this.C7.Width = 50;
            // 
            // C8
            // 
            this.C8.AppearanceCell.Options.UseTextOptions = true;
            this.C8.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.C8.AppearanceHeader.Options.UseTextOptions = true;
            this.C8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C8.Caption = "床位";
            this.C8.FieldName = "OUTBED";
            this.C8.Name = "C8";
            this.C8.Visible = true;
            this.C8.VisibleIndex = 5;
            this.C8.Width = 39;
            // 
            // C9
            // 
            this.C9.AppearanceCell.Options.UseTextOptions = true;
            this.C9.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.C9.AppearanceHeader.Options.UseTextOptions = true;
            this.C9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C9.Caption = "出院日期";
            this.C9.FieldName = "OUTHOSDATE";
            this.C9.MinWidth = 130;
            this.C9.Name = "C9";
            this.C9.Visible = true;
            this.C9.VisibleIndex = 6;
            this.C9.Width = 130;
            // 
            // C10
            // 
            this.C10.AppearanceCell.Options.UseTextOptions = true;
            this.C10.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.C10.AppearanceHeader.Options.UseTextOptions = true;
            this.C10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C10.Caption = "出院诊断";
            this.C10.FieldName = "CYZD";
            this.C10.Name = "C10";
            this.C10.Visible = true;
            this.C10.VisibleIndex = 7;
            this.C10.Width = 167;
            // 
            // C11
            // 
            this.C11.AppearanceCell.Options.UseTextOptions = true;
            this.C11.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.C11.AppearanceHeader.Options.UseTextOptions = true;
            this.C11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C11.Caption = "申请理由";
            this.C11.FieldName = "APPLYCONTENT";
            this.C11.Name = "C11";
            this.C11.Visible = true;
            this.C11.VisibleIndex = 8;
            this.C11.Width = 93;
            // 
            // C12
            // 
            this.C12.AppearanceCell.Options.UseTextOptions = true;
            this.C12.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.C12.AppearanceHeader.Options.UseTextOptions = true;
            this.C12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C12.Caption = "申请期限";
            this.C12.FieldName = "APPLYTIMES";
            this.C12.MinWidth = 70;
            this.C12.Name = "C12";
            this.C12.Visible = true;
            this.C12.VisibleIndex = 9;
            this.C12.Width = 70;
            // 
            // C13
            // 
            this.C13.AppearanceCell.Options.UseTextOptions = true;
            this.C13.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C13.AppearanceHeader.Options.UseTextOptions = true;
            this.C13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C13.Caption = "延期申请";
            this.C13.FieldName = "YQ";
            this.C13.MinWidth = 70;
            this.C13.Name = "C13";
            this.C13.Visible = true;
            this.C13.VisibleIndex = 10;
            this.C13.Width = 70;
            // 
            // C15
            // 
            this.C15.AppearanceCell.Options.UseTextOptions = true;
            this.C15.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.C15.AppearanceHeader.Options.UseTextOptions = true;
            this.C15.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C15.Caption = "审核意见";
            this.C15.FieldName = "APPROVECONTENT";
            this.C15.Name = "C15";
            this.C15.OptionsColumn.ReadOnly = true;
            this.C15.Width = 56;
            // 
            // C16
            // 
            this.C16.AppearanceCell.Options.UseTextOptions = true;
            this.C16.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.C16.AppearanceHeader.Options.UseTextOptions = true;
            this.C16.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C16.Caption = "审核日期";
            this.C16.FieldName = "SHSJ";
            this.C16.MinWidth = 130;
            this.C16.Name = "C16";
            this.C16.OptionsColumn.ReadOnly = true;
            this.C16.Visible = true;
            this.C16.VisibleIndex = 11;
            this.C16.Width = 130;
            // 
            // C14
            // 
            this.C14.AppearanceCell.Options.UseTextOptions = true;
            this.C14.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.C14.AppearanceHeader.Options.UseTextOptions = true;
            this.C14.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C14.Caption = "申请状态";
            this.C14.FieldName = "STATUSDES";
            this.C14.Name = "C14";
            this.C14.Width = 46;
            // 
            // C2
            // 
            this.C2.AppearanceCell.Options.UseTextOptions = true;
            this.C2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.C2.AppearanceHeader.Options.UseTextOptions = true;
            this.C2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.C2.Caption = "序号";
            this.C2.FieldName = "SEQ";
            this.C2.Name = "C2";
            this.C2.Width = 43;
            // 
            // C17
            // 
            this.C17.Caption = "主键";
            this.C17.FieldName = "noofinpat";
            this.C17.Name = "C17";
            // 
            // MedicalRecordBrowse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dbGrid);
            this.Controls.Add(this.panelHead);
            this.Name = "MedicalRecordBrowse";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(1059, 499);
            ((System.ComponentModel.ISupportInitialize)(this.panelHead)).EndInit();
            this.panelHead.ResumeLayout(false);
            this.panelHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStart.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelHead;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset btnRefresh;
        private DevExpress.XtraEditors.SimpleButton btnApply;
        private DevExpress.XtraEditors.DateEdit dateEnd;
        private DevExpress.XtraEditors.DateEdit dateStart;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraGrid.GridControl dbGrid;
        private DevExpress.XtraGrid.Columns.GridColumn C1;
        private DevExpress.XtraGrid.Columns.GridColumn C3;
        private DevExpress.XtraGrid.Columns.GridColumn C4;
        private DevExpress.XtraGrid.Columns.GridColumn C5;
        private DevExpress.XtraGrid.Columns.GridColumn C6;
        private DevExpress.XtraGrid.Columns.GridColumn C7;
        private DevExpress.XtraGrid.Columns.GridColumn C8;
        private DevExpress.XtraGrid.Columns.GridColumn C9;
        private DevExpress.XtraGrid.Columns.GridColumn C10;
        private DevExpress.XtraGrid.Columns.GridColumn C11;
        private DevExpress.XtraGrid.Columns.GridColumn C12;
        private DevExpress.XtraGrid.Columns.GridColumn C13;
        private DevExpress.XtraGrid.Columns.GridColumn C15;
        private DevExpress.XtraGrid.Columns.GridColumn C16;
        private DevExpress.XtraGrid.Columns.GridColumn C14;
        private DevExpress.XtraGrid.Columns.GridColumn C2;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey btnQuery;
        public DevExpress.XtraGrid.Views.Grid.GridView dbGridView;
        private DevExpress.XtraGrid.Columns.GridColumn C17;

    }
}
