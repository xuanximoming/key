namespace DrectSoft.Core.MainEmrPad
{
    partial class OrdersDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrdersDetail));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.checkEditAll = new DevExpress.XtraEditors.CheckEdit();
            this.btn_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColCheck = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColORDER_TEXT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColDOSAGE_UNITS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColADMINISTRATION = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColFREQUENCY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcolORDER_CLASS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDOSAGE = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.checkEditAll);
            this.panelControl1.Controls.Add(this.btn_Cancel);
            this.panelControl1.Controls.Add(this.btn_Save);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 332);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(611, 41);
            this.panelControl1.TabIndex = 1;
            // 
            // checkEditAll
            // 
            this.checkEditAll.Location = new System.Drawing.Point(14, 10);
            this.checkEditAll.Name = "checkEditAll";
            this.checkEditAll.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkEditAll.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.checkEditAll.Properties.Appearance.Options.UseFont = true;
            this.checkEditAll.Properties.Appearance.Options.UseForeColor = true;
            this.checkEditAll.Properties.Caption = "全选";
            this.checkEditAll.Size = new System.Drawing.Size(54, 19);
            this.checkEditAll.TabIndex = 4;
            this.checkEditAll.CheckedChanged += new System.EventHandler(this.checkEditAll_CheckedChanged);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(477, 8);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(87, 27);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Save.Location = new System.Drawing.Point(363, 7);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(87, 27);
            this.btn_Save.TabIndex = 0;
            this.btn_Save.Text = "插入";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(611, 332);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColCheck,
            this.gridColORDER_TEXT,
            this.gridColumnDOSAGE,
            this.gridColDOSAGE_UNITS,
            this.gridColADMINISTRATION,
            this.gridColFREQUENCY,
            this.gridcolORDER_CLASS});
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
            // gridColORDER_TEXT
            // 
            this.gridColORDER_TEXT.Caption = "医嘱";
            this.gridColORDER_TEXT.FieldName = "ORDER_TEXT";
            this.gridColORDER_TEXT.Name = "gridColORDER_TEXT";
            this.gridColORDER_TEXT.OptionsColumn.AllowEdit = false;
            this.gridColORDER_TEXT.Visible = true;
            this.gridColORDER_TEXT.VisibleIndex = 1;
            this.gridColORDER_TEXT.Width = 89;
            // 
            // gridColDOSAGE_UNITS
            // 
            this.gridColDOSAGE_UNITS.Caption = "剂量单位";
            this.gridColDOSAGE_UNITS.FieldName = "DOSAGE_UNITS";
            this.gridColDOSAGE_UNITS.Name = "gridColDOSAGE_UNITS";
            this.gridColDOSAGE_UNITS.OptionsColumn.AllowEdit = false;
            this.gridColDOSAGE_UNITS.Visible = true;
            this.gridColDOSAGE_UNITS.VisibleIndex = 3;
            this.gridColDOSAGE_UNITS.Width = 87;
            // 
            // gridColADMINISTRATION
            // 
            this.gridColADMINISTRATION.Caption = "途径";
            this.gridColADMINISTRATION.FieldName = "ADMINISTRATION";
            this.gridColADMINISTRATION.Name = "gridColADMINISTRATION";
            this.gridColADMINISTRATION.OptionsColumn.AllowEdit = false;
            this.gridColADMINISTRATION.Visible = true;
            this.gridColADMINISTRATION.VisibleIndex = 4;
            this.gridColADMINISTRATION.Width = 79;
            // 
            // gridColFREQUENCY
            // 
            this.gridColFREQUENCY.Caption = "执行频率";
            this.gridColFREQUENCY.FieldName = "FREQUENCY";
            this.gridColFREQUENCY.Name = "gridColFREQUENCY";
            this.gridColFREQUENCY.OptionsColumn.AllowEdit = false;
            this.gridColFREQUENCY.Visible = true;
            this.gridColFREQUENCY.VisibleIndex = 5;
            this.gridColFREQUENCY.Width = 89;
            // 
            // gridcolORDER_CLASS
            // 
            this.gridcolORDER_CLASS.Caption = "医嘱类别";
            this.gridcolORDER_CLASS.FieldName = "ORDER_CLASS";
            this.gridcolORDER_CLASS.Name = "gridcolORDER_CLASS";
            this.gridcolORDER_CLASS.OptionsColumn.AllowEdit = false;
            this.gridcolORDER_CLASS.Visible = true;
            this.gridcolORDER_CLASS.VisibleIndex = 6;
            this.gridcolORDER_CLASS.Width = 91;
            // 
            // gridColumnDOSAGE
            // 
            this.gridColumnDOSAGE.Caption = "剂量";
            this.gridColumnDOSAGE.FieldName = "DOSAGE";
            this.gridColumnDOSAGE.Name = "gridColumnDOSAGE";
            this.gridColumnDOSAGE.Visible = true;
            this.gridColumnDOSAGE.VisibleIndex = 2;
            // 
            // OrdersDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 373);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OrdersDetail";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "医嘱调阅";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
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
        private DevExpress.XtraGrid.Columns.GridColumn gridColDOSAGE_UNITS;
        private DevExpress.XtraGrid.Columns.GridColumn gridColADMINISTRATION;
        private DevExpress.XtraGrid.Columns.GridColumn gridColFREQUENCY;
        private DevExpress.XtraGrid.Columns.GridColumn gridcolORDER_CLASS;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColORDER_TEXT;
        private DevExpress.XtraEditors.CheckEdit checkEditAll;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDOSAGE;
    }
}