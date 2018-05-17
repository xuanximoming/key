namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    partial class ReportDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportDetail));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditAll = new DevExpress.XtraEditors.CheckEdit();
            this.btn_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColCheck = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColUNIT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColItemName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColResult = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRefer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcolFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.checkEdit1);
            this.panelControl1.Controls.Add(this.checkEditAll);
            this.panelControl1.Controls.Add(this.btn_Cancel);
            this.panelControl1.Controls.Add(this.btn_Save);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 285);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(524, 35);
            this.panelControl1.TabIndex = 1;
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(64, 9);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkEdit1.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.checkEdit1.Properties.Appearance.Options.UseFont = true;
            this.checkEdit1.Properties.Appearance.Options.UseForeColor = true;
            this.checkEdit1.Properties.Caption = "异常";
            this.checkEdit1.Size = new System.Drawing.Size(51, 19);
            this.checkEdit1.TabIndex = 5;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // checkEditAll
            // 
            this.checkEditAll.Location = new System.Drawing.Point(12, 9);
            this.checkEditAll.Name = "checkEditAll";
            this.checkEditAll.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkEditAll.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.checkEditAll.Properties.Appearance.Options.UseFont = true;
            this.checkEditAll.Properties.Appearance.Options.UseForeColor = true;
            this.checkEditAll.Properties.Caption = "全选";
            this.checkEditAll.Size = new System.Drawing.Size(46, 19);
            this.checkEditAll.TabIndex = 4;
            this.checkEditAll.CheckedChanged += new System.EventHandler(this.checkEditAll_CheckedChanged);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(409, 7);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Save.Location = new System.Drawing.Point(311, 6);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 23);
            this.btn_Save.TabIndex = 0;
            this.btn_Save.Text = "插入";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(524, 285);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColCheck,
            this.gridColUNIT,
            this.gridColItemName,
            this.gridColResult,
            this.gridColRefer,
            this.gridcolFlag});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView1_CustomDrawCell);
            this.gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanged);
            // 
            // gridColCheck
            // 
            this.gridColCheck.Caption = " ";
            this.gridColCheck.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColCheck.FieldName = "CHECK";
            this.gridColCheck.Name = "gridColCheck";
            this.gridColCheck.Visible = true;
            this.gridColCheck.VisibleIndex = 0;
            this.gridColCheck.Width = 62;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit1.ValueGrayed = false;
            // 
            // gridColUNIT
            // 
            this.gridColUNIT.Caption = "单位";
            this.gridColUNIT.FieldName = "UNIT";
            this.gridColUNIT.Name = "gridColUNIT";
            this.gridColUNIT.Visible = true;
            this.gridColUNIT.VisibleIndex = 3;
            this.gridColUNIT.Width = 89;
            // 
            // gridColItemName
            // 
            this.gridColItemName.Caption = "检验名称";
            this.gridColItemName.FieldName = "ITEMNAME";
            this.gridColItemName.Name = "gridColItemName";
            this.gridColItemName.OptionsColumn.AllowEdit = false;
            this.gridColItemName.Visible = true;
            this.gridColItemName.VisibleIndex = 1;
            this.gridColItemName.Width = 87;
            // 
            // gridColResult
            // 
            this.gridColResult.Caption = "检验结果";
            this.gridColResult.FieldName = "RESULT";
            this.gridColResult.Name = "gridColResult";
            this.gridColResult.OptionsColumn.AllowEdit = false;
            this.gridColResult.Visible = true;
            this.gridColResult.VisibleIndex = 2;
            this.gridColResult.Width = 79;
            // 
            // gridColRefer
            // 
            this.gridColRefer.Caption = "参考值";
            this.gridColRefer.FieldName = "REFERVALUE";
            this.gridColRefer.Name = "gridColRefer";
            this.gridColRefer.OptionsColumn.AllowEdit = false;
            this.gridColRefer.Visible = true;
            this.gridColRefer.VisibleIndex = 4;
            this.gridColRefer.Width = 89;
            // 
            // gridcolFlag
            // 
            this.gridcolFlag.Caption = "异常标志";
            this.gridcolFlag.FieldName = "HIGHFLAG";
            this.gridcolFlag.Name = "gridcolFlag";
            this.gridcolFlag.OptionsColumn.AllowEdit = false;
            this.gridcolFlag.Visible = true;
            this.gridcolFlag.VisibleIndex = 5;
            this.gridcolFlag.Width = 91;
            // 
            // ReportDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 320);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReportDetail";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "医技报告调阅";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton btn_Cancel;
        private DevExpress.XtraEditors.SimpleButton btn_Save;
        private DevExpress.XtraGrid.Columns.GridColumn gridColCheck;
        private DevExpress.XtraGrid.Columns.GridColumn gridColItemName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColResult;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRefer;
        private DevExpress.XtraGrid.Columns.GridColumn gridcolFlag;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColUNIT;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.CheckEdit checkEditAll;
    }
}