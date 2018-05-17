namespace DrectSoft.Core.DoctorTasks
{
    partial class FormInpatientOrder
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
            this.gridMedQCAnalysis = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
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
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.dateBegin = new DevExpress.XtraEditors.DateEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dateEnd = new DevExpress.XtraEditors.DateEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridMedQCAnalysis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridMedQCAnalysis
            // 
            this.gridMedQCAnalysis.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridMedQCAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMedQCAnalysis.Location = new System.Drawing.Point(2, 2);
            this.gridMedQCAnalysis.MainView = this.gridView1;
            this.gridMedQCAnalysis.Name = "gridMedQCAnalysis";
            this.gridMedQCAnalysis.Size = new System.Drawing.Size(896, 475);
            this.gridMedQCAnalysis.TabIndex = 1;
            this.gridMedQCAnalysis.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
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
            this.gridView1.GridControl = this.gridMedQCAnalysis;
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
            // 
            // gridColumnDoseUnit
            // 
            this.gridColumnDoseUnit.Caption = "剂量单位";
            this.gridColumnDoseUnit.FieldName = "DOSE_UNIT";
            this.gridColumnDoseUnit.Name = "gridColumnDoseUnit";
            // 
            // gridColumnSpecs
            // 
            this.gridColumnSpecs.Caption = "规格";
            this.gridColumnSpecs.FieldName = "SPECS";
            this.gridColumnSpecs.Name = "gridColumnSpecs";
            // 
            // gridColumnDfqCexp
            // 
            this.gridColumnDfqCexp.Caption = "频率";
            this.gridColumnDfqCexp.FieldName = "DFQ_CEXP";
            this.gridColumnDfqCexp.Name = "gridColumnDfqCexp";
            // 
            // gridColumnQtyTot
            // 
            this.gridColumnQtyTot.Caption = "总量";
            this.gridColumnQtyTot.FieldName = "QTY_TOT";
            this.gridColumnQtyTot.Name = "gridColumnQtyTot";
            // 
            // gridColumnMinUnit
            // 
            this.gridColumnMinUnit.Caption = "单位";
            this.gridColumnMinUnit.FieldName = "MIN_UNIT";
            this.gridColumnMinUnit.Name = "gridColumnMinUnit";
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
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn7.Caption = "数量";
            this.gridColumn7.FieldName = "ypjl";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 4;
            this.gridColumn7.Width = 58;
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn8.Caption = "单位";
            this.gridColumn8.FieldName = "jldw";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 6;
            this.gridColumn8.Width = 44;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn9.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn9.Caption = "用法";
            this.gridColumn9.FieldName = "ypyf";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 7;
            this.gridColumn9.Width = 50;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn10.Caption = "频次";
            this.gridColumn10.FieldName = "pcdm";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 8;
            this.gridColumn10.Width = 39;
            // 
            // panelControl2
            // 
            this.panelControl2.ContentImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.btnQuery);
            this.panelControl2.Controls.Add(this.dateBegin);
            this.panelControl2.Controls.Add(this.dateEnd);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(900, 42);
            this.panelControl2.TabIndex = 11;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(248, 13);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(12, 14);
            this.labelControl3.TabIndex = 10;
            this.labelControl3.Text = "至";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(23, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "开始时间：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(269, 13);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "结束时间：";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(527, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 9;
            this.btnQuery.Text = "查找";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // dateBegin
            // 
            this.dateBegin.EditValue = null;
            this.dateBegin.Location = new System.Drawing.Point(89, 10);
            this.dateBegin.Name = "dateBegin";
            this.dateBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateBegin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateBegin.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.dateBegin.Size = new System.Drawing.Size(151, 20);
            this.dateBegin.TabIndex = 6;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // dateEnd
            // 
            this.dateEnd.EditValue = null;
            this.dateEnd.Location = new System.Drawing.Point(335, 12);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEnd.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.dateEnd.Size = new System.Drawing.Size(141, 20);
            this.dateEnd.TabIndex = 7;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridMedQCAnalysis);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 42);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(900, 479);
            this.panelControl1.TabIndex = 12;
            // 
            // FormInpatientOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Name = "FormInpatientOrder";
            this.Size = new System.Drawing.Size(900, 521);
            ((System.ComponentModel.ISupportInitialize)(this.gridMedQCAnalysis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        private DevExpress.XtraGrid.GridControl gridMedQCAnalysis;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.DateEdit dateBegin;
        private DevExpress.XtraEditors.DateEdit dateEnd;
        private DevExpress.XtraEditors.PanelControl panelControl1;
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
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnBeginDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnContent;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEndDate;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

    }
}