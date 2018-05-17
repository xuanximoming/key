namespace DrectSoft.Core.ZymosisReport
{
    partial class UCJobDisease
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCJobDisease));
            this.simpleButtonExport = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonSearch = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditAnalyseEnd = new DevExpress.XtraEditors.DateEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.dateEditAnalyseFrom = new DevExpress.XtraEditors.DateEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.checkedComboBoxEditDisease = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.gridControlAnalyse = new DevExpress.XtraGrid.GridControl();
            this.gridViewAnalyse = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnJob = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDiseaseCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDeadCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_reset = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseEnd.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseFrom.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEditDisease.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAnalyse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAnalyse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // simpleButtonExport
            // 
            this.simpleButtonExport.Image = global::DrectSoft.Core.ZymosisReport.Properties.Resources.导出;
            this.simpleButtonExport.Location = new System.Drawing.Point(801, 12);
            this.simpleButtonExport.Name = "simpleButtonExport";
            this.simpleButtonExport.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonExport.TabIndex = 5;
            this.simpleButtonExport.Text = "导出 (&I)";
            this.simpleButtonExport.Click += new System.EventHandler(this.simpleButtonExport_Click);
            // 
            // simpleButtonSearch
            // 
            this.simpleButtonSearch.Image = global::DrectSoft.Core.ZymosisReport.Properties.Resources.查询;
            this.simpleButtonSearch.Location = new System.Drawing.Point(629, 12);
            this.simpleButtonSearch.Name = "simpleButtonSearch";
            this.simpleButtonSearch.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonSearch.TabIndex = 3;
            this.simpleButtonSearch.Text = "查询 (&Q)";
            this.simpleButtonSearch.Click += new System.EventHandler(this.simpleButtonSearch_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "录入日期：";
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
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(182, 19);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(12, 14);
            this.labelControl10.TabIndex = 7;
            this.labelControl10.Text = "至";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(347, 19);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 14);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "疾病：";
            // 
            // checkedComboBoxEditDisease
            // 
            this.checkedComboBoxEditDisease.Location = new System.Drawing.Point(389, 16);
            this.checkedComboBoxEditDisease.Name = "checkedComboBoxEditDisease";
            this.checkedComboBoxEditDisease.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.checkedComboBoxEditDisease.Properties.PopupFormSize = new System.Drawing.Size(200, 200);
            this.checkedComboBoxEditDisease.Size = new System.Drawing.Size(200, 21);
            this.checkedComboBoxEditDisease.TabIndex = 2;
            // 
            // gridControlAnalyse
            // 
            this.gridControlAnalyse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlAnalyse.Location = new System.Drawing.Point(0, 50);
            this.gridControlAnalyse.MainView = this.gridViewAnalyse;
            this.gridControlAnalyse.Name = "gridControlAnalyse";
            this.gridControlAnalyse.Size = new System.Drawing.Size(995, 530);
            this.gridControlAnalyse.TabIndex = 1;
            this.gridControlAnalyse.TabStop = false;
            this.gridControlAnalyse.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewAnalyse});
            // 
            // gridViewAnalyse
            // 
            this.gridViewAnalyse.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnJob,
            this.gridColumnDiseaseCount,
            this.gridColumnDeadCount});
            this.gridViewAnalyse.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewAnalyse.GridControl = this.gridControlAnalyse;
            this.gridViewAnalyse.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridViewAnalyse.Name = "gridViewAnalyse";
            this.gridViewAnalyse.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridViewAnalyse.OptionsBehavior.Editable = false;
            this.gridViewAnalyse.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewAnalyse.OptionsCustomization.AllowFilter = false;
            this.gridViewAnalyse.OptionsCustomization.AllowSort = false;
            this.gridViewAnalyse.OptionsMenu.EnableColumnMenu = false;
            this.gridViewAnalyse.OptionsMenu.EnableFooterMenu = false;
            this.gridViewAnalyse.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewAnalyse.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewAnalyse.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewAnalyse.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewAnalyse.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewAnalyse.OptionsView.ColumnAutoWidth = false;
            this.gridViewAnalyse.OptionsView.ShowGroupPanel = false;
            this.gridViewAnalyse.OptionsView.ShowIndicator = false;
            this.gridViewAnalyse.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gridViewAnalyse.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(this.gridViewAnalyse_CustomDrawColumnHeader);
            // 
            // gridColumnJob
            // 
            this.gridColumnJob.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnJob.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnJob.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnJob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnJob.Caption = "职业";
            this.gridColumnJob.FieldName = "JOBNAME";
            this.gridColumnJob.Name = "gridColumnJob";
            this.gridColumnJob.Tag = "病种";
            this.gridColumnJob.Width = 170;
            // 
            // gridColumnDiseaseCount
            // 
            this.gridColumnDiseaseCount.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDiseaseCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnDiseaseCount.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDiseaseCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDiseaseCount.Caption = "发病数";
            this.gridColumnDiseaseCount.FieldName = "SUM_DISEASE";
            this.gridColumnDiseaseCount.Name = "gridColumnDiseaseCount";
            this.gridColumnDiseaseCount.Tag = "合并";
            this.gridColumnDiseaseCount.Width = 70;
            // 
            // gridColumnDeadCount
            // 
            this.gridColumnDeadCount.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDeadCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnDeadCount.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDeadCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDeadCount.Caption = "死亡数";
            this.gridColumnDeadCount.FieldName = "SUM_DIE";
            this.gridColumnDeadCount.Name = "gridColumnDeadCount";
            this.gridColumnDeadCount.Tag = "合并";
            this.gridColumnDeadCount.Width = 70;
            // 
            // btn_reset
            // 
            this.btn_reset.Image = ((System.Drawing.Image)(resources.GetObject("btn_reset.Image")));
            this.btn_reset.Location = new System.Drawing.Point(715, 12);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(80, 27);
            this.btn_reset.TabIndex = 4;
            this.btn_reset.Text = "重置(&B)";
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btn_reset);
            this.panelControl1.Controls.Add(this.dateEditAnalyseFrom);
            this.panelControl1.Controls.Add(this.simpleButtonExport);
            this.panelControl1.Controls.Add(this.labelControl10);
            this.panelControl1.Controls.Add(this.simpleButtonSearch);
            this.panelControl1.Controls.Add(this.checkedComboBoxEditDisease);
            this.panelControl1.Controls.Add(this.dateEditAnalyseEnd);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(995, 50);
            this.panelControl1.TabIndex = 0;
            // 
            // UCJobDisease
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlAnalyse);
            this.Controls.Add(this.panelControl1);
            this.Name = "UCJobDisease";
            this.Size = new System.Drawing.Size(995, 580);
            this.Load += new System.EventHandler(this.UCJobDisease_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseEnd.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseFrom.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAnalyseFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEditDisease.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAnalyse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAnalyse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckedComboBoxEdit checkedComboBoxEditDisease;
        private DevExpress.XtraGrid.GridControl gridControlAnalyse;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewAnalyse;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnJob;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDiseaseCount;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDeadCount;
        private DevExpress.XtraEditors.SimpleButton simpleButtonExport;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSearch;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dateEditAnalyseEnd;
        private DevExpress.XtraEditors.DateEdit dateEditAnalyseFrom;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset btn_reset;
    }
}
