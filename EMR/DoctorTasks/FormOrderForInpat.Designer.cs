namespace DrectSoft.Core.DoctorTasks
{
    partial class FormOrderForInpat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOrderForInpat));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnPrint = new DrectSoft.Common.Ctrs.OTHER.DevButtonPrint();
            this.btnQuery = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey();
            this.btnReset = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset();
            this.lookUpEditorDept = new DrectSoft.Common.Library.LookUpEditor();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditEnd = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditBegin = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.rdgDate = new DevExpress.XtraEditors.RadioGroup();
            this.rdgOrderStyle = new DevExpress.XtraEditors.RadioGroup();
            this.gridControlOrder = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemSearchLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.repositoryItemSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnBeginDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnClassName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnItemName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDrugType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDoseOnce = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDoseUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnSpecs = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDfqCexp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnQtyTot = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnMinUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnContent = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnEndDate = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBegin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdgDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdgOrderStyle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnPrint);
            this.panelControl1.Controls.Add(this.btnQuery);
            this.panelControl1.Controls.Add(this.btnReset);
            this.panelControl1.Controls.Add(this.lookUpEditorDept);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.dateEditEnd);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.dateEditBegin);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.rdgDate);
            this.panelControl1.Controls.Add(this.rdgOrderStyle);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(771, 58);
            this.panelControl1.TabIndex = 0;
            // 
            // btnPrint
            // 
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.Location = new System.Drawing.Point(686, 33);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(69, 20);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "打印(&P)";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnQuery.Image")));
            this.btnQuery.Location = new System.Drawing.Point(535, 33);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(69, 20);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "查询(&Q)";
            // 
            // btnReset
            // 
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.Location = new System.Drawing.Point(610, 33);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(69, 20);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "重置(&B)";
            // 
            // lookUpEditorDept
            // 
            this.lookUpEditorDept.EnterMoveNextControl = true;
            this.lookUpEditorDept.ListWindow = null;
            this.lookUpEditorDept.Location = new System.Drawing.Point(294, 5);
            this.lookUpEditorDept.Name = "lookUpEditorDept";
            this.lookUpEditorDept.ShowSButton = true;
            this.lookUpEditorDept.Size = new System.Drawing.Size(106, 18);
            this.lookUpEditorDept.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(258, 10);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "科室：";
            // 
            // dateEditEnd
            // 
            this.dateEditEnd.EditValue = null;
            this.dateEditEnd.Location = new System.Drawing.Point(421, 35);
            this.dateEditEnd.Name = "dateEditEnd";
            this.dateEditEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditEnd.Size = new System.Drawing.Size(106, 20);
            this.dateEditEnd.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(405, 38);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(12, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "至";
            // 
            // dateEditBegin
            // 
            this.dateEditBegin.EditValue = null;
            this.dateEditBegin.Location = new System.Drawing.Point(294, 35);
            this.dateEditBegin.Name = "dateEditBegin";
            this.dateEditBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditBegin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditBegin.Size = new System.Drawing.Size(106, 20);
            this.dateEditBegin.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(196, 38);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(108, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "医嘱开始时间起始：";
            // 
            // rdgDate
            // 
            this.rdgDate.Location = new System.Drawing.Point(26, 32);
            this.rdgDate.Name = "rdgDate";
            this.rdgDate.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdgDate.Properties.Appearance.Options.UseBackColor = true;
            this.rdgDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdgDate.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "全部时间"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "选择时间")});
            this.rdgDate.Size = new System.Drawing.Size(156, 27);
            this.rdgDate.TabIndex = 2;
            // 
            // rdgOrderStyle
            // 
            this.rdgOrderStyle.Location = new System.Drawing.Point(26, 4);
            this.rdgOrderStyle.Name = "rdgOrderStyle";
            this.rdgOrderStyle.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdgOrderStyle.Properties.Appearance.Options.UseBackColor = true;
            this.rdgOrderStyle.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdgOrderStyle.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "临时医嘱"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "长期医嘱")});
            this.rdgOrderStyle.Size = new System.Drawing.Size(156, 27);
            this.rdgOrderStyle.TabIndex = 0;
            // 
            // gridControlOrder
            // 
            this.gridControlOrder.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControlOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlOrder.Location = new System.Drawing.Point(0, 58);
            this.gridControlOrder.MainView = this.gridView1;
            this.gridControlOrder.Name = "gridControlOrder";
            this.gridControlOrder.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSearchLookUpEdit1});
            this.gridControlOrder.Size = new System.Drawing.Size(771, 349);
            this.gridControlOrder.TabIndex = 1;
            this.gridControlOrder.TabStop = false;
            this.gridControlOrder.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnBeginDate,
            this.gridColumnTypeName,
            this.gridColumnClassName,
            this.gridColumnItemName,
            this.gridColumnDrugType,
            this.gridColumnDoseOnce,
            this.gridColumnDoseUnit,
            this.gridColumnSpecs,
            this.gridColumnDfqCexp,
            this.gridColumnQtyTot,
            this.gridColumnMinUnit,
            this.gridColumnContent,
            this.gridColumnEndDate});
            this.gridView1.GridControl = this.gridControlOrder;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridView1.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridView1.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // repositoryItemSearchLookUpEdit1
            // 
            this.repositoryItemSearchLookUpEdit1.AutoHeight = false;
            this.repositoryItemSearchLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSearchLookUpEdit1.Name = "repositoryItemSearchLookUpEdit1";
            this.repositoryItemSearchLookUpEdit1.View = this.repositoryItemSearchLookUpEdit1View;
            // 
            // repositoryItemSearchLookUpEdit1View
            // 
            this.repositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemSearchLookUpEdit1View.Name = "repositoryItemSearchLookUpEdit1View";
            this.repositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumnBeginDate
            // 
            this.gridColumnBeginDate.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnBeginDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnBeginDate.Caption = "开始时间";
            this.gridColumnBeginDate.FieldName = "DATE_BGN";
            this.gridColumnBeginDate.Name = "gridColumnBeginDate";
            this.gridColumnBeginDate.Visible = true;
            this.gridColumnBeginDate.VisibleIndex = 0;
            this.gridColumnBeginDate.Width = 155;
            // 
            // gridColumnTypeName
            // 
            this.gridColumnTypeName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnTypeName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnTypeName.Caption = "医嘱类型";
            this.gridColumnTypeName.FieldName = "TYPE_NAME";
            this.gridColumnTypeName.Name = "gridColumnTypeName";
            this.gridColumnTypeName.Visible = true;
            this.gridColumnTypeName.VisibleIndex = 1;
            this.gridColumnTypeName.Width = 97;
            // 
            // gridColumnClassName
            // 
            this.gridColumnClassName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnClassName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnClassName.Caption = "分类";
            this.gridColumnClassName.FieldName = "CLASS_NAME";
            this.gridColumnClassName.Name = "gridColumnClassName";
            this.gridColumnClassName.Visible = true;
            this.gridColumnClassName.VisibleIndex = 2;
            this.gridColumnClassName.Width = 58;
            // 
            // gridColumnItemName
            // 
            this.gridColumnItemName.Caption = "医嘱";
            this.gridColumnItemName.FieldName = "ITEM_NAME";
            this.gridColumnItemName.Name = "gridColumnItemName";
            this.gridColumnItemName.Visible = true;
            this.gridColumnItemName.VisibleIndex = 5;
            this.gridColumnItemName.Width = 142;
            // 
            // gridColumnDrugType
            // 
            this.gridColumnDrugType.Caption = "药品类型";
            this.gridColumnDrugType.FieldName = "DRUG_TYPE";
            this.gridColumnDrugType.Name = "gridColumnDrugType";
            // 
            // gridColumnDoseOnce
            // 
            this.gridColumnDoseOnce.Caption = "剂量";
            this.gridColumnDoseOnce.FieldName = "DOSE_ONCE";
            this.gridColumnDoseOnce.Name = "gridColumnDoseOnce";
            this.gridColumnDoseOnce.Visible = true;
            this.gridColumnDoseOnce.VisibleIndex = 6;
            // 
            // gridColumnDoseUnit
            // 
            this.gridColumnDoseUnit.Caption = "剂量单位";
            this.gridColumnDoseUnit.FieldName = "DOSE_UNIT";
            this.gridColumnDoseUnit.Name = "gridColumnDoseUnit";
            this.gridColumnDoseUnit.Visible = true;
            this.gridColumnDoseUnit.VisibleIndex = 7;
            // 
            // gridColumnSpecs
            // 
            this.gridColumnSpecs.Caption = "规格";
            this.gridColumnSpecs.FieldName = "SPECS";
            this.gridColumnSpecs.Name = "gridColumnSpecs";
            this.gridColumnSpecs.Visible = true;
            this.gridColumnSpecs.VisibleIndex = 8;
            // 
            // gridColumnDfqCexp
            // 
            this.gridColumnDfqCexp.Caption = "频率";
            this.gridColumnDfqCexp.FieldName = "DFQ_CEXP";
            this.gridColumnDfqCexp.Name = "gridColumnDfqCexp";
            this.gridColumnDfqCexp.Visible = true;
            this.gridColumnDfqCexp.VisibleIndex = 9;
            // 
            // gridColumnQtyTot
            // 
            this.gridColumnQtyTot.Caption = "总量";
            this.gridColumnQtyTot.FieldName = "QTY_TOT";
            this.gridColumnQtyTot.Name = "gridColumnQtyTot";
            this.gridColumnQtyTot.Visible = true;
            this.gridColumnQtyTot.VisibleIndex = 10;
            // 
            // gridColumnMinUnit
            // 
            this.gridColumnMinUnit.Caption = "单位";
            this.gridColumnMinUnit.FieldName = "MIN_UNIT";
            this.gridColumnMinUnit.Name = "gridColumnMinUnit";
            this.gridColumnMinUnit.Visible = true;
            this.gridColumnMinUnit.VisibleIndex = 11;
            // 
            // gridColumnContent
            // 
            this.gridColumnContent.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnContent.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnContent.Caption = "医嘱内容";
            this.gridColumnContent.FieldName = "CONTENT";
            this.gridColumnContent.Name = "gridColumnContent";
            this.gridColumnContent.Visible = true;
            this.gridColumnContent.VisibleIndex = 3;
            this.gridColumnContent.Width = 407;
            // 
            // gridColumnEndDate
            // 
            this.gridColumnEndDate.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnEndDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnEndDate.Caption = "结束时间";
            this.gridColumnEndDate.FieldName = "DATE_END";
            this.gridColumnEndDate.Name = "gridColumnEndDate";
            this.gridColumnEndDate.Visible = true;
            this.gridColumnEndDate.VisibleIndex = 4;
            this.gridColumnEndDate.Width = 177;
            // 
            // FormOrderForInpat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlOrder);
            this.Controls.Add(this.panelControl1);
            this.Name = "FormOrderForInpat";
            this.Size = new System.Drawing.Size(771, 407);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBegin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditBegin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdgDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdgOrderStyle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.RadioGroup rdgOrderStyle;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonPrint btnPrint;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey btnQuery;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset btnReset;
        private Common.Library.LookUpEditor lookUpEditorDept;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.DateEdit dateEditEnd;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit dateEditBegin;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.RadioGroup rdgDate;
        private DevExpress.XtraGrid.GridControl gridControlOrder;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit repositoryItemSearchLookUpEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemSearchLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnBeginDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTypeName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnClassName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnItemName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDrugType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDoseOnce;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDoseUnit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnSpecs;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDfqCexp;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnQtyTot;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnMinUnit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnContent;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEndDate;
    }
}