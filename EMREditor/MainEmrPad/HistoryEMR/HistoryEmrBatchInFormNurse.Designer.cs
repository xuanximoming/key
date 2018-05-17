namespace DrectSoft.Core.MainEmrPad.HistoryEMR
{
    partial class HistoryEmrBatchInFormNurse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryEmrBatchInFormNurse));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey(this.components);
            this.dateEditEnd = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditBegin = new DevExpress.XtraEditors.DateEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupControlEmrContent = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.checkEditCheckAll = new DevExpress.XtraEditors.CheckEdit();
            this.checkedListBoxControlEmrNode = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.splitterControl3 = new DevExpress.XtraEditors.SplitterControl();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.gridControlInpatientList = new DevExpress.XtraGrid.GridControl();
            this.gridViewInpatientList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnNoofinpat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnInpatientName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnAdmitDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnOutHosDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDiagName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.btnOk = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.toolTipControllerEmrNode = new DevExpress.Utils.ToolTipController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBegin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlEmrContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditCheckAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControlEmrNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlInpatientList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewInpatientList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.btnQuery);
            this.panelControl1.Controls.Add(this.dateEditEnd);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.dateEditBegin);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(934, 39);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(23, 13);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "入院日期";
            // 
            // btnQuery
            // 
            this.btnQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnQuery.Image")));
            this.btnQuery.Location = new System.Drawing.Point(306, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 23);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "查询(&Q)";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // dateEditEnd
            // 
            this.dateEditEnd.EditValue = null;
            this.dateEditEnd.Location = new System.Drawing.Point(200, 10);
            this.dateEditEnd.Name = "dateEditEnd";
            this.dateEditEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dateEditEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditEnd.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateEditEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEditEnd.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateEditEnd.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEditEnd.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dateEditEnd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dateEditEnd.Properties.NullValuePrompt = "请输入终止日期";
            this.dateEditEnd.Size = new System.Drawing.Size(100, 20);
            this.dateEditEnd.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(184, 13);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(12, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "至";
            // 
            // dateEditBegin
            // 
            this.dateEditBegin.EditValue = null;
            this.dateEditBegin.Location = new System.Drawing.Point(79, 10);
            this.dateEditBegin.Name = "dateEditBegin";
            this.dateEditBegin.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dateEditBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditBegin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditBegin.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateEditBegin.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditBegin.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateEditBegin.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditBegin.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dateEditBegin.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dateEditBegin.Properties.NullValuePrompt = "请输入起始日期";
            this.dateEditBegin.Size = new System.Drawing.Size(100, 20);
            this.dateEditBegin.TabIndex = 1;
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.groupControlEmrContent);
            this.panelControl2.Controls.Add(this.groupControl2);
            this.panelControl2.Controls.Add(this.splitterControl3);
            this.panelControl2.Controls.Add(this.splitterControl1);
            this.panelControl2.Controls.Add(this.panelControl4);
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 39);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(934, 623);
            this.panelControl2.TabIndex = 1;
            // 
            // groupControlEmrContent
            // 
            this.groupControlEmrContent.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControlEmrContent.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControlEmrContent.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.groupControlEmrContent.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.groupControlEmrContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlEmrContent.Location = new System.Drawing.Point(265, 142);
            this.groupControlEmrContent.Name = "groupControlEmrContent";
            this.groupControlEmrContent.Size = new System.Drawing.Size(669, 445);
            this.groupControlEmrContent.TabIndex = 13;
            this.groupControlEmrContent.Text = "病历内容查看";
            // 
            // groupControl2
            // 
            this.groupControl2.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl2.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl2.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.groupControl2.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.groupControl2.Controls.Add(this.checkEditCheckAll);
            this.groupControl2.Controls.Add(this.checkedListBoxControlEmrNode);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl2.Location = new System.Drawing.Point(5, 142);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(260, 445);
            this.groupControl2.TabIndex = 13;
            this.groupControl2.Text = "病历列表";
            // 
            // checkEditCheckAll
            // 
            this.checkEditCheckAll.Location = new System.Drawing.Point(3, 2);
            this.checkEditCheckAll.Name = "checkEditCheckAll";
            this.checkEditCheckAll.Properties.Caption = "全选";
            this.checkEditCheckAll.Size = new System.Drawing.Size(60, 19);
            this.checkEditCheckAll.TabIndex = 1;
            // 
            // checkedListBoxControlEmrNode
            // 
            this.checkedListBoxControlEmrNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxControlEmrNode.Location = new System.Drawing.Point(2, 22);
            this.checkedListBoxControlEmrNode.Name = "checkedListBoxControlEmrNode";
            this.checkedListBoxControlEmrNode.Size = new System.Drawing.Size(256, 421);
            this.checkedListBoxControlEmrNode.TabIndex = 0;
            this.checkedListBoxControlEmrNode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkedListBoxControlEmrNode_MouseDown);
            // 
            // splitterControl3
            // 
            this.splitterControl3.Location = new System.Drawing.Point(0, 142);
            this.splitterControl3.Name = "splitterControl3";
            this.splitterControl3.Size = new System.Drawing.Size(5, 445);
            this.splitterControl3.TabIndex = 11;
            this.splitterControl3.TabStop = false;
            // 
            // splitterControl1
            // 
            this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterControl1.Location = new System.Drawing.Point(0, 137);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(934, 5);
            this.splitterControl1.TabIndex = 8;
            this.splitterControl1.TabStop = false;
            // 
            // panelControl4
            // 
            this.panelControl4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl4.Controls.Add(this.gridControlInpatientList);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl4.Location = new System.Drawing.Point(0, 0);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(934, 137);
            this.panelControl4.TabIndex = 2;
            // 
            // gridControlInpatientList
            // 
            this.gridControlInpatientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlInpatientList.Location = new System.Drawing.Point(0, 0);
            this.gridControlInpatientList.MainView = this.gridViewInpatientList;
            this.gridControlInpatientList.Name = "gridControlInpatientList";
            this.gridControlInpatientList.Size = new System.Drawing.Size(934, 137);
            this.gridControlInpatientList.TabIndex = 8;
            this.gridControlInpatientList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewInpatientList});
            // 
            // gridViewInpatientList
            // 
            this.gridViewInpatientList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnNoofinpat,
            this.gridColumnInpatientName,
            this.gridColumnAdmitDate,
            this.gridColumnOutHosDate,
            this.gridColumnDiagName});
            this.gridViewInpatientList.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewInpatientList.GridControl = this.gridControlInpatientList;
            this.gridViewInpatientList.Name = "gridViewInpatientList";
            this.gridViewInpatientList.OptionsBehavior.Editable = false;
            this.gridViewInpatientList.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewInpatientList.OptionsCustomization.AllowColumnResizing = false;
            this.gridViewInpatientList.OptionsCustomization.AllowFilter = false;
            this.gridViewInpatientList.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewInpatientList.OptionsFilter.AllowFilterEditor = false;
            this.gridViewInpatientList.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewInpatientList.OptionsFind.AllowFindPanel = false;
            this.gridViewInpatientList.OptionsMenu.EnableColumnMenu = false;
            this.gridViewInpatientList.OptionsMenu.EnableFooterMenu = false;
            this.gridViewInpatientList.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewInpatientList.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewInpatientList.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewInpatientList.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewInpatientList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewInpatientList.OptionsView.ShowGroupPanel = false;
            this.gridViewInpatientList.OptionsView.ShowIndicator = false;
            this.gridViewInpatientList.OptionsView.ShowViewCaption = true;
            this.gridViewInpatientList.ViewCaption = "病人历次住院记录";
            // 
            // gridColumnNoofinpat
            // 
            this.gridColumnNoofinpat.Caption = "gridColumn1";
            this.gridColumnNoofinpat.FieldName = "NOOFINPAT";
            this.gridColumnNoofinpat.Name = "gridColumnNoofinpat";
            // 
            // gridColumnInpatientName
            // 
            this.gridColumnInpatientName.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnInpatientName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnInpatientName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumnInpatientName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnInpatientName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnInpatientName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumnInpatientName.Caption = "病人姓名";
            this.gridColumnInpatientName.FieldName = "NAME";
            this.gridColumnInpatientName.Name = "gridColumnInpatientName";
            this.gridColumnInpatientName.Visible = true;
            this.gridColumnInpatientName.VisibleIndex = 0;
            this.gridColumnInpatientName.Width = 100;
            // 
            // gridColumnAdmitDate
            // 
            this.gridColumnAdmitDate.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnAdmitDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnAdmitDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumnAdmitDate.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnAdmitDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnAdmitDate.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumnAdmitDate.Caption = "入院时间";
            this.gridColumnAdmitDate.FieldName = "ADMITDATE";
            this.gridColumnAdmitDate.Name = "gridColumnAdmitDate";
            this.gridColumnAdmitDate.Visible = true;
            this.gridColumnAdmitDate.VisibleIndex = 1;
            this.gridColumnAdmitDate.Width = 140;
            // 
            // gridColumnOutHosDate
            // 
            this.gridColumnOutHosDate.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnOutHosDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnOutHosDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumnOutHosDate.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnOutHosDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnOutHosDate.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumnOutHosDate.Caption = "出院时间";
            this.gridColumnOutHosDate.FieldName = "OUTHOSDATE";
            this.gridColumnOutHosDate.Name = "gridColumnOutHosDate";
            this.gridColumnOutHosDate.Visible = true;
            this.gridColumnOutHosDate.VisibleIndex = 2;
            this.gridColumnOutHosDate.Width = 140;
            // 
            // gridColumnDiagName
            // 
            this.gridColumnDiagName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDiagName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDiagName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumnDiagName.Caption = "诊断";
            this.gridColumnDiagName.FieldName = "DIAGNAME";
            this.gridColumnDiagName.Name = "gridColumnDiagName";
            this.gridColumnDiagName.Visible = true;
            this.gridColumnDiagName.VisibleIndex = 3;
            this.gridColumnDiagName.Width = 330;
            // 
            // panelControl3
            // 
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Controls.Add(this.btnCancel);
            this.panelControl3.Controls.Add(this.btnOk);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 587);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(934, 36);
            this.panelControl3.TabIndex = 6;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(324, 14);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "点击确定按钮后，将会导入上方“病历列表”里勾中的病历";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(843, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消(&C)";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(757, 7);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "确定(&Y)";
            // 
            // HistoryEmrBatchInFormNurse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 662);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HistoryEmrBatchInFormNurse";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
            ((System.ComponentModel.ISupportInitialize)(this.groupControlEmrContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEditCheckAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControlEmrNode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlInpatientList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewInpatientList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey btnQuery;
        private DevExpress.XtraEditors.DateEdit dateEditEnd;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit dateEditBegin;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel btnCancel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK btnOk;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraGrid.GridControl gridControlInpatientList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewInpatientList;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnNoofinpat;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnInpatientName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnAdmitDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnOutHosDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDiagName;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControlEmrContent;
        private DevExpress.XtraEditors.SplitterControl splitterControl3;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.CheckedListBoxControl checkedListBoxControlEmrNode;
        private DevExpress.XtraEditors.CheckEdit checkEditCheckAll;
        private DevExpress.Utils.ToolTipController toolTipControllerEmrNode;
    }
}