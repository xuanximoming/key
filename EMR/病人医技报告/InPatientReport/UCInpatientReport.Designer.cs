namespace DrectSoft.Core.InPatientReport
{
    partial class UCInpatientReport
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dateBegin = new DevExpress.XtraEditors.DateEdit();
            this.dateEnd = new DevExpress.XtraEditors.DateEdit();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.lookUpWindowInpatient = new DrectSoft.Common.Library.LookUpWindow();
            this.ReportID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControlInpatietnReport = new DevExpress.XtraGrid.GridControl();
            this.gridViewInpatientreport = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColUNIT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColItemName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColResult = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRefer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcolFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcRELEASEDATE = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowInpatient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlInpatietnReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewInpatientreport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.dateBegin);
            this.panelControl2.Controls.Add(this.dateEnd);
            this.panelControl2.Controls.Add(this.btnQuery);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(2, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1174, 313);
            this.panelControl2.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 16;
            this.labelControl1.Text = "开始时间：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(230, 16);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 17;
            this.labelControl2.Text = "结束时间：";
            // 
            // dateBegin
            // 
            this.dateBegin.EditValue = null;
            this.dateBegin.Location = new System.Drawing.Point(83, 12);
            this.dateBegin.Name = "dateBegin";
            this.dateBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateBegin.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateBegin.Size = new System.Drawing.Size(117, 21);
            this.dateBegin.TabIndex = 18;
            // 
            // dateEnd
            // 
            this.dateEnd.EditValue = null;
            this.dateEnd.Location = new System.Drawing.Point(296, 11);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEnd.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEnd.Size = new System.Drawing.Size(117, 21);
            this.dateEnd.TabIndex = 19;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(447, 8);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(87, 27);
            this.btnQuery.TabIndex = 15;
            this.btnQuery.Text = "查找";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lookUpWindowInpatient
            // 
            this.lookUpWindowInpatient.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowInpatient.GenShortCode = null;
            this.lookUpWindowInpatient.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowInpatient.Owner = null;
            this.lookUpWindowInpatient.SqlHelper = null;
            // 
            // ReportID
            // 
            this.ReportID.Caption = "gridColumn8";
            this.ReportID.FieldName = "REPORTID";
            this.ReportID.Name = "ReportID";
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "报表发布日期";
            this.gridColumn7.Name = "gridColumn7";
            // 
            // gridControlInpatietnReport
            // 
            this.gridControlInpatietnReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlInpatietnReport.Location = new System.Drawing.Point(2, 2);
            this.gridControlInpatietnReport.MainView = this.gridViewInpatientreport;
            this.gridControlInpatietnReport.Name = "gridControlInpatietnReport";
            this.gridControlInpatietnReport.Size = new System.Drawing.Size(1173, 262);
            this.gridControlInpatietnReport.TabIndex = 0;
            this.gridControlInpatietnReport.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewInpatientreport});
            // 
            // gridViewInpatientreport
            // 
            this.gridViewInpatientreport.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.ReportID,
            this.gcRELEASEDATE});
            this.gridViewInpatientreport.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewInpatientreport.GridControl = this.gridControlInpatietnReport;
            this.gridViewInpatientreport.Name = "gridViewInpatientreport";
            this.gridViewInpatientreport.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridViewInpatientreport.OptionsBehavior.Editable = false;
            this.gridViewInpatientreport.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewInpatientreport.OptionsCustomization.AllowFilter = false;
            this.gridViewInpatientreport.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewInpatientreport.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewInpatientreport.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewInpatientreport.OptionsView.ShowGroupPanel = false;
            this.gridViewInpatientreport.OptionsView.ShowIndicator = false;
            this.gridViewInpatientreport.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gridViewInpatientreport.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewInpatientreport_FocusedRowChanged);
            this.gridViewInpatientreport.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridViewInpatientreport_MouseDown);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "病历号";
            this.gridColumn1.FieldName = "HOSPITALNO";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "患者姓名";
            this.gridColumn2.FieldName = "PATNAME";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "报告名称";
            this.gridColumn4.FieldName = "APPLYITEMNAME";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "送检医生";
            this.gridColumn5.FieldName = "SUBMITDOCNAME";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "送检时间";
            this.gridColumn6.FieldName = "SUBMITDATE";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            // 
            // panelControl3
            // 
            this.panelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl3.Controls.Add(this.gridControlInpatietnReport);
            this.panelControl3.Location = new System.Drawing.Point(0, 44);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1177, 266);
            this.panelControl3.TabIndex = 1;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1178, 317);
            this.panelControl1.TabIndex = 2;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.gridControl1);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl4.Location = new System.Drawing.Point(0, 317);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(1178, 244);
            this.panelControl4.TabIndex = 5;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1174, 240);
            this.gridControl1.TabIndex = 5;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColUNIT,
            this.gridColItemName,
            this.gridColResult,
            this.gridColRefer,
            this.gridcolFlag});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView1_CustomDrawCell);
            // 
            // gridColUNIT
            // 
            this.gridColUNIT.Caption = "单位";
            this.gridColUNIT.FieldName = "UNIT";
            this.gridColUNIT.Name = "gridColUNIT";
            this.gridColUNIT.Visible = true;
            this.gridColUNIT.VisibleIndex = 2;
            this.gridColUNIT.Width = 89;
            // 
            // gridColItemName
            // 
            this.gridColItemName.Caption = "检验名称";
            this.gridColItemName.FieldName = "ITEMNAME";
            this.gridColItemName.Name = "gridColItemName";
            this.gridColItemName.OptionsColumn.AllowEdit = false;
            this.gridColItemName.Visible = true;
            this.gridColItemName.VisibleIndex = 0;
            this.gridColItemName.Width = 87;
            // 
            // gridColResult
            // 
            this.gridColResult.Caption = "检验结果";
            this.gridColResult.FieldName = "RESULT";
            this.gridColResult.Name = "gridColResult";
            this.gridColResult.OptionsColumn.AllowEdit = false;
            this.gridColResult.Visible = true;
            this.gridColResult.VisibleIndex = 1;
            this.gridColResult.Width = 79;
            // 
            // gridColRefer
            // 
            this.gridColRefer.Caption = "参考值";
            this.gridColRefer.FieldName = "REFERVALUE";
            this.gridColRefer.Name = "gridColRefer";
            this.gridColRefer.OptionsColumn.AllowEdit = false;
            this.gridColRefer.Visible = true;
            this.gridColRefer.VisibleIndex = 3;
            this.gridColRefer.Width = 89;
            // 
            // gridcolFlag
            // 
            this.gridcolFlag.Caption = "异常标志";
            this.gridcolFlag.FieldName = "HIGHFLAG";
            this.gridcolFlag.Name = "gridcolFlag";
            this.gridcolFlag.OptionsColumn.AllowEdit = false;
            this.gridcolFlag.Visible = true;
            this.gridcolFlag.VisibleIndex = 4;
            this.gridcolFlag.Width = 91;
            // 
            // gcRELEASEDATE
            // 
            this.gcRELEASEDATE.Caption = "出检时间";
            this.gcRELEASEDATE.FieldName = "RELEASEDATE";
            this.gcRELEASEDATE.Name = "gcRELEASEDATE";
            this.gcRELEASEDATE.Visible = true;
            this.gcRELEASEDATE.VisibleIndex = 4;
            // 
            // UCInpatientReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl4);
            this.Controls.Add(this.panelControl1);
            this.Name = "UCInpatientReport";
            this.Size = new System.Drawing.Size(1178, 561);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowInpatient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlInpatietnReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewInpatientreport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit dateBegin;
        private DevExpress.XtraEditors.DateEdit dateEnd;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DrectSoft.Common.Library.LookUpWindow lookUpWindowInpatient;
        private DevExpress.XtraGrid.Columns.GridColumn ReportID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.GridControl gridControlInpatietnReport;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewInpatientreport;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColUNIT;
        private DevExpress.XtraGrid.Columns.GridColumn gridColItemName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColResult;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRefer;
        private DevExpress.XtraGrid.Columns.GridColumn gridcolFlag;
        private DevExpress.XtraGrid.Columns.GridColumn gcRELEASEDATE;
    }
}
