namespace DrectSoft.Core.MainEmrPad.HistoryEMR
{
    partial class HistoryEmrBatchInForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryEmrBatchInForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnReset = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset(this.components);
            this.btnQuery = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey(this.components);
            this.dateEditEnd = new DevExpress.XtraEditors.DateEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditBegin = new DevExpress.XtraEditors.DateEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.btnOk = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.gridControlHistoryEmr = new DevExpress.XtraGrid.GridControl();
            this.gridViewHistoryEmr = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnNoofinpat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnInpatientName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnAdmitDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDiagName = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBegin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlHistoryEmr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewHistoryEmr)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.btnReset);
            this.panelControl1.Controls.Add(this.btnQuery);
            this.panelControl1.Controls.Add(this.dateEditEnd);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.dateEditBegin);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(716, 49);
            this.panelControl1.TabIndex = 2;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(27, 18);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "入院日期：";
            // 
            // btnReset
            // 
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.Location = new System.Drawing.Point(418, 14);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 23);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "重置(&B)";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnQuery.Image")));
            this.btnQuery.Location = new System.Drawing.Point(331, 14);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 23);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "查询(&Q)";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // dateEditEnd
            // 
            this.dateEditEnd.EditValue = null;
            this.dateEditEnd.Location = new System.Drawing.Point(213, 15);
            this.dateEditEnd.Name = "dateEditEnd";
            this.dateEditEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditEnd.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.dateEditEnd.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateEditEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEditEnd.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateEditEnd.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEditEnd.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dateEditEnd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dateEditEnd.Size = new System.Drawing.Size(100, 20);
            this.dateEditEnd.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(195, 18);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(12, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "至";
            // 
            // dateEditBegin
            // 
            this.dateEditBegin.EditValue = null;
            this.dateEditBegin.Location = new System.Drawing.Point(89, 15);
            this.dateEditBegin.Name = "dateEditBegin";
            this.dateEditBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditBegin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditBegin.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.dateEditBegin.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateEditBegin.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditBegin.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateEditBegin.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditBegin.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dateEditBegin.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dateEditBegin.Size = new System.Drawing.Size(100, 20);
            this.dateEditBegin.TabIndex = 1;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Controls.Add(this.gridControlHistoryEmr);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 49);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(716, 417);
            this.panelControl2.TabIndex = 3;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.btnCancel);
            this.panelControl3.Controls.Add(this.btnOk);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(2, 375);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(712, 40);
            this.panelControl3.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(619, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消(&C)";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(533, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "确定(&Y)";
            // 
            // gridControlHistoryEmr
            // 
            this.gridControlHistoryEmr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlHistoryEmr.Location = new System.Drawing.Point(2, 2);
            this.gridControlHistoryEmr.MainView = this.gridViewHistoryEmr;
            this.gridControlHistoryEmr.Name = "gridControlHistoryEmr";
            this.gridControlHistoryEmr.Size = new System.Drawing.Size(712, 413);
            this.gridControlHistoryEmr.TabIndex = 5;
            this.gridControlHistoryEmr.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewHistoryEmr});
            // 
            // gridViewHistoryEmr
            // 
            this.gridViewHistoryEmr.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnNoofinpat,
            this.gridColumnInpatientName,
            this.gridColumnAdmitDate,
            this.gridColumnDiagName});
            this.gridViewHistoryEmr.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewHistoryEmr.GridControl = this.gridControlHistoryEmr;
            this.gridViewHistoryEmr.Name = "gridViewHistoryEmr";
            this.gridViewHistoryEmr.OptionsBehavior.Editable = false;
            this.gridViewHistoryEmr.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewHistoryEmr.OptionsCustomization.AllowColumnResizing = false;
            this.gridViewHistoryEmr.OptionsCustomization.AllowFilter = false;
            this.gridViewHistoryEmr.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewHistoryEmr.OptionsFilter.AllowFilterEditor = false;
            this.gridViewHistoryEmr.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewHistoryEmr.OptionsFind.AllowFindPanel = false;
            this.gridViewHistoryEmr.OptionsMenu.EnableColumnMenu = false;
            this.gridViewHistoryEmr.OptionsMenu.EnableFooterMenu = false;
            this.gridViewHistoryEmr.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewHistoryEmr.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewHistoryEmr.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewHistoryEmr.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewHistoryEmr.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewHistoryEmr.OptionsView.ShowGroupPanel = false;
            this.gridViewHistoryEmr.OptionsView.ShowIndicator = false;
            // 
            // gridColumnNoofinpat
            // 
            this.gridColumnNoofinpat.Caption = "gridColumn1";
            this.gridColumnNoofinpat.FieldName = "NOOFINPAT";
            this.gridColumnNoofinpat.Name = "gridColumnNoofinpat";
            // 
            // gridColumnInpatientName
            // 
            this.gridColumnInpatientName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnInpatientName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnInpatientName.Caption = "病人姓名";
            this.gridColumnInpatientName.FieldName = "NAME";
            this.gridColumnInpatientName.Name = "gridColumnInpatientName";
            this.gridColumnInpatientName.Visible = true;
            this.gridColumnInpatientName.VisibleIndex = 0;
            // 
            // gridColumnAdmitDate
            // 
            this.gridColumnAdmitDate.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnAdmitDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnAdmitDate.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnAdmitDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnAdmitDate.Caption = "入院时间";
            this.gridColumnAdmitDate.FieldName = "ADMITDATE";
            this.gridColumnAdmitDate.Name = "gridColumnAdmitDate";
            this.gridColumnAdmitDate.Visible = true;
            this.gridColumnAdmitDate.VisibleIndex = 1;
            this.gridColumnAdmitDate.Width = 140;
            // 
            // gridColumnDiagName
            // 
            this.gridColumnDiagName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDiagName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDiagName.Caption = "诊断";
            this.gridColumnDiagName.FieldName = "DIAGNAME";
            this.gridColumnDiagName.Name = "gridColumnDiagName";
            this.gridColumnDiagName.Visible = true;
            this.gridColumnDiagName.VisibleIndex = 2;
            this.gridColumnDiagName.Width = 123;
            // 
            // HistoryEmrBatchInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 466);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HistoryEmrBatchInForm";
            this.Text = "历史病历批量导入";
            this.Load += new System.EventHandler(this.HistoryEmrBatchInForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBegin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBegin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlHistoryEmr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewHistoryEmr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset btnReset;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey btnQuery;
        private DevExpress.XtraEditors.DateEdit dateEditEnd;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit dateEditBegin;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraGrid.GridControl gridControlHistoryEmr;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewHistoryEmr;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnNoofinpat;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnInpatientName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnAdmitDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDiagName;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel btnCancel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK btnOk;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}