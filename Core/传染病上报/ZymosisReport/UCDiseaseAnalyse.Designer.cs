namespace DrectSoft.Core.ZymosisReport
{
    partial class UCDiseaseAnalyse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDiseaseAnalyse));
            this.gridControlAnalyse = new DevExpress.XtraGrid.GridControl();
            this.gridViewAnalyse = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnDiseaseType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDiseaseName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDiseaseCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDiseasePercent = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDeadCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDeadPercent = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDiseaseDie = new DevExpress.XtraGrid.Columns.GridColumn();
            this.simpleButtonExport = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonSearch = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditAnalyseEnd = new DevExpress.XtraEditors.DateEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.dateEditAnalyseFrom = new DevExpress.XtraEditors.DateEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.btn_reset = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAnalyse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAnalyse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseEnd.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseFrom.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControlAnalyse
            // 
            this.gridControlAnalyse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlAnalyse.Location = new System.Drawing.Point(0, 50);
            this.gridControlAnalyse.MainView = this.gridViewAnalyse;
            this.gridControlAnalyse.Name = "gridControlAnalyse";
            this.gridControlAnalyse.Size = new System.Drawing.Size(777, 555);
            this.gridControlAnalyse.TabIndex = 1;
            this.gridControlAnalyse.TabStop = false;
            this.gridControlAnalyse.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewAnalyse});
            // 
            // gridViewAnalyse
            // 
            this.gridViewAnalyse.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridViewAnalyse.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridViewAnalyse.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.gridViewAnalyse.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gridViewAnalyse.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridViewAnalyse.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gridViewAnalyse.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gridViewAnalyse.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnDiseaseType,
            this.gridColumnDiseaseName,
            this.gridColumnDiseaseCount,
            this.gridColumnDiseasePercent,
            this.gridColumnDeadCount,
            this.gridColumnDeadPercent,
            this.gridColumnDiseaseDie});
            this.gridViewAnalyse.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewAnalyse.GridControl = this.gridControlAnalyse;
            this.gridViewAnalyse.Name = "gridViewAnalyse";
            this.gridViewAnalyse.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridViewAnalyse.OptionsBehavior.Editable = false;
            this.gridViewAnalyse.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewAnalyse.OptionsCustomization.AllowFilter = false;
            this.gridViewAnalyse.OptionsCustomization.AllowSort = false;
            this.gridViewAnalyse.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewAnalyse.OptionsFilter.AllowFilterEditor = false;
            this.gridViewAnalyse.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewAnalyse.OptionsMenu.EnableColumnMenu = false;
            this.gridViewAnalyse.OptionsMenu.EnableFooterMenu = false;
            this.gridViewAnalyse.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewAnalyse.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewAnalyse.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewAnalyse.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewAnalyse.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewAnalyse.OptionsView.AllowCellMerge = true;
            this.gridViewAnalyse.OptionsView.ShowGroupPanel = false;
            this.gridViewAnalyse.OptionsView.ShowIndicator = false;
            this.gridViewAnalyse.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gridViewAnalyse.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(this.gridViewAnalyse_CustomDrawColumnHeader);
            this.gridViewAnalyse.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridViewAnalyse_CustomDrawCell);
            // 
            // gridColumnDiseaseType
            // 
            this.gridColumnDiseaseType.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDiseaseType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnDiseaseType.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumnDiseaseType.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDiseaseType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDiseaseType.Caption = "病种分类";
            this.gridColumnDiseaseType.FieldName = "LEVEL_NAME";
            this.gridColumnDiseaseType.Name = "gridColumnDiseaseType";
            this.gridColumnDiseaseType.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnDiseaseType.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnDiseaseType.Tag = "病种";
            this.gridColumnDiseaseType.Visible = true;
            this.gridColumnDiseaseType.VisibleIndex = 0;
            this.gridColumnDiseaseType.Width = 120;
            // 
            // gridColumnDiseaseName
            // 
            this.gridColumnDiseaseName.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDiseaseName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnDiseaseName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDiseaseName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDiseaseName.Caption = "病种名称";
            this.gridColumnDiseaseName.FieldName = "NAME";
            this.gridColumnDiseaseName.Name = "gridColumnDiseaseName";
            this.gridColumnDiseaseName.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnDiseaseName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnDiseaseName.Tag = "病种";
            this.gridColumnDiseaseName.Visible = true;
            this.gridColumnDiseaseName.VisibleIndex = 1;
            this.gridColumnDiseaseName.Width = 100;
            // 
            // gridColumnDiseaseCount
            // 
            this.gridColumnDiseaseCount.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDiseaseCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnDiseaseCount.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDiseaseCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDiseaseCount.Caption = "发病数";
            this.gridColumnDiseaseCount.FieldName = "CNT";
            this.gridColumnDiseaseCount.Name = "gridColumnDiseaseCount";
            this.gridColumnDiseaseCount.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnDiseaseCount.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnDiseaseCount.Visible = true;
            this.gridColumnDiseaseCount.VisibleIndex = 2;
            this.gridColumnDiseaseCount.Width = 90;
            // 
            // gridColumnDiseasePercent
            // 
            this.gridColumnDiseasePercent.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDiseasePercent.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnDiseasePercent.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDiseasePercent.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDiseasePercent.Caption = "占总发病数的百分比";
            this.gridColumnDiseasePercent.FieldName = "ATTACK_RATE";
            this.gridColumnDiseasePercent.Name = "gridColumnDiseasePercent";
            this.gridColumnDiseasePercent.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnDiseasePercent.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnDiseasePercent.Visible = true;
            this.gridColumnDiseasePercent.VisibleIndex = 3;
            this.gridColumnDiseasePercent.Width = 120;
            // 
            // gridColumnDeadCount
            // 
            this.gridColumnDeadCount.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDeadCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnDeadCount.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDeadCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDeadCount.Caption = "死亡数";
            this.gridColumnDeadCount.FieldName = "DIE_CNT";
            this.gridColumnDeadCount.Name = "gridColumnDeadCount";
            this.gridColumnDeadCount.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnDeadCount.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnDeadCount.Visible = true;
            this.gridColumnDeadCount.VisibleIndex = 4;
            this.gridColumnDeadCount.Width = 90;
            // 
            // gridColumnDeadPercent
            // 
            this.gridColumnDeadPercent.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDeadPercent.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnDeadPercent.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDeadPercent.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDeadPercent.Caption = "占总死亡数的百分比";
            this.gridColumnDeadPercent.FieldName = "DIE_RATE";
            this.gridColumnDeadPercent.Name = "gridColumnDeadPercent";
            this.gridColumnDeadPercent.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnDeadPercent.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnDeadPercent.Visible = true;
            this.gridColumnDeadPercent.VisibleIndex = 5;
            this.gridColumnDeadPercent.Width = 120;
            // 
            // gridColumnDiseaseDie
            // 
            this.gridColumnDiseaseDie.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDiseaseDie.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnDiseaseDie.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDiseaseDie.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDiseaseDie.Caption = "病死率";
            this.gridColumnDiseaseDie.FieldName = "DIERATE";
            this.gridColumnDiseaseDie.Name = "gridColumnDiseaseDie";
            this.gridColumnDiseaseDie.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnDiseaseDie.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnDiseaseDie.Visible = true;
            this.gridColumnDiseaseDie.VisibleIndex = 6;
            this.gridColumnDiseaseDie.Width = 91;
            // 
            // simpleButtonExport
            // 
            this.simpleButtonExport.Image = global::DrectSoft.Core.ZymosisReport.Properties.Resources.导出;
            this.simpleButtonExport.Location = new System.Drawing.Point(512, 12);
            this.simpleButtonExport.Name = "simpleButtonExport";
            this.simpleButtonExport.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonExport.TabIndex = 4;
            this.simpleButtonExport.Text = "导出 (&I)";
            this.simpleButtonExport.Click += new System.EventHandler(this.simpleButtonExport_Click);
            // 
            // simpleButtonSearch
            // 
            this.simpleButtonSearch.Image = global::DrectSoft.Core.ZymosisReport.Properties.Resources.查询;
            this.simpleButtonSearch.Location = new System.Drawing.Point(340, 12);
            this.simpleButtonSearch.Name = "simpleButtonSearch";
            this.simpleButtonSearch.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonSearch.TabIndex = 2;
            this.simpleButtonSearch.Text = "查询 (&Q)";
            this.simpleButtonSearch.Click += new System.EventHandler(this.simpleButtonSearch_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "确诊日期：";
            // 
            // dateEditAnalyseEnd
            // 
            this.dateEditAnalyseEnd.EditValue = null;
            this.dateEditAnalyseEnd.Location = new System.Drawing.Point(200, 16);
            this.dateEditAnalyseEnd.Name = "dateEditAnalyseEnd";
            this.dateEditAnalyseEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditAnalyseEnd.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.dateEditAnalyseEnd.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditAnalyseEnd.Size = new System.Drawing.Size(103, 21);
            this.dateEditAnalyseEnd.TabIndex = 1;
            this.dateEditAnalyseEnd.ToolTip = "结束日期";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // dateEditAnalyseFrom
            // 
            this.dateEditAnalyseFrom.EditValue = null;
            this.dateEditAnalyseFrom.Location = new System.Drawing.Point(73, 16);
            this.dateEditAnalyseFrom.Name = "dateEditAnalyseFrom";
            this.dateEditAnalyseFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditAnalyseFrom.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.dateEditAnalyseFrom.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditAnalyseFrom.Size = new System.Drawing.Size(103, 21);
            this.dateEditAnalyseFrom.TabIndex = 0;
            this.dateEditAnalyseFrom.ToolTip = "开始日期";
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(182, 19);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(12, 14);
            this.labelControl10.TabIndex = 6;
            this.labelControl10.Text = "至";
            // 
            // btn_reset
            // 
            this.btn_reset.Image = ((System.Drawing.Image)(resources.GetObject("btn_reset.Image")));
            this.btn_reset.Location = new System.Drawing.Point(426, 12);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(80, 27);
            this.btn_reset.TabIndex = 3;
            this.btn_reset.Text = "重置(&B)";
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btn_reset);
            this.panelControl1.Controls.Add(this.dateEditAnalyseEnd);
            this.panelControl1.Controls.Add(this.simpleButtonExport);
            this.panelControl1.Controls.Add(this.labelControl10);
            this.panelControl1.Controls.Add(this.simpleButtonSearch);
            this.panelControl1.Controls.Add(this.dateEditAnalyseFrom);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(777, 50);
            this.panelControl1.TabIndex = 0;
            // 
            // UCDiseaseAnalyse
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gridControlAnalyse);
            this.Controls.Add(this.panelControl1);
            this.Name = "UCDiseaseAnalyse";
            this.Size = new System.Drawing.Size(777, 605);
            this.Load += new System.EventHandler(this.UCDiseaseAnalyse_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAnalyse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAnalyse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseEnd.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseFrom.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlAnalyse;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewAnalyse;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDiseaseType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDiseaseName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDiseaseCount;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDiseasePercent;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDeadCount;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDeadPercent;
        private DevExpress.XtraEditors.SimpleButton simpleButtonExport;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSearch;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dateEditAnalyseEnd;
        private DevExpress.XtraEditors.DateEdit dateEditAnalyseFrom;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDiseaseDie;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset btn_reset;

    }
}
