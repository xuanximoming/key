namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.HistoryEMR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryEmrBatchInForm));
            this.gridControlHistoryEmr = new DevExpress.XtraGrid.GridControl();
            this.gridViewHistoryEmr = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnNoofinpat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnInpatientName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnAdmitDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDiagName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.simpleButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonExit = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlHistoryEmr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewHistoryEmr)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlHistoryEmr
            // 
            this.gridControlHistoryEmr.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridControlHistoryEmr.Location = new System.Drawing.Point(0, 0);
            this.gridControlHistoryEmr.MainView = this.gridViewHistoryEmr;
            this.gridControlHistoryEmr.Name = "gridControlHistoryEmr";
            this.gridControlHistoryEmr.Size = new System.Drawing.Size(340, 258);
            this.gridControlHistoryEmr.TabIndex = 0;
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
            this.gridViewHistoryEmr.OptionsCustomization.AllowSort = false;
            this.gridViewHistoryEmr.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewHistoryEmr.OptionsFilter.AllowFilterEditor = false;
            this.gridViewHistoryEmr.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewHistoryEmr.OptionsFind.AllowFindPanel = false;
            this.gridViewHistoryEmr.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewHistoryEmr.OptionsView.ShowGroupPanel = false;
            this.gridViewHistoryEmr.OptionsView.ShowIndicator = false;
            this.gridViewHistoryEmr.DoubleClick += new System.EventHandler(this.gridViewHistoryEmr_DoubleClick);
            // 
            // gridColumnNoofinpat
            // 
            this.gridColumnNoofinpat.Caption = "gridColumn1";
            this.gridColumnNoofinpat.FieldName = "NOOFINPAT";
            this.gridColumnNoofinpat.Name = "gridColumnNoofinpat";
            // 
            // gridColumnInpatientName
            // 
            this.gridColumnInpatientName.Caption = "患者";
            this.gridColumnInpatientName.FieldName = "NAME";
            this.gridColumnInpatientName.Name = "gridColumnInpatientName";
            this.gridColumnInpatientName.Visible = true;
            this.gridColumnInpatientName.VisibleIndex = 0;
            // 
            // gridColumnAdmitDate
            // 
            this.gridColumnAdmitDate.Caption = "入院时间";
            this.gridColumnAdmitDate.FieldName = "ADMITDATE";
            this.gridColumnAdmitDate.Name = "gridColumnAdmitDate";
            this.gridColumnAdmitDate.Visible = true;
            this.gridColumnAdmitDate.VisibleIndex = 1;
            this.gridColumnAdmitDate.Width = 140;
            // 
            // gridColumnDiagName
            // 
            this.gridColumnDiagName.Caption = "诊断";
            this.gridColumnDiagName.FieldName = "DIAGNAME";
            this.gridColumnDiagName.Name = "gridColumnDiagName";
            this.gridColumnDiagName.Visible = true;
            this.gridColumnDiagName.VisibleIndex = 2;
            this.gridColumnDiagName.Width = 123;
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Location = new System.Drawing.Point(81, 265);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonOK.TabIndex = 1;
            this.simpleButtonOK.Text = "确定";
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButtonExit
            // 
            this.simpleButtonExit.Location = new System.Drawing.Point(182, 265);
            this.simpleButtonExit.Name = "simpleButtonExit";
            this.simpleButtonExit.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonExit.TabIndex = 1;
            this.simpleButtonExit.Text = "取消";
            this.simpleButtonExit.Click += new System.EventHandler(this.simpleButtonExit_Click);
            // 
            // HistoryEmrBatchInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 293);
            this.Controls.Add(this.simpleButtonExit);
            this.Controls.Add(this.simpleButtonOK);
            this.Controls.Add(this.gridControlHistoryEmr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HistoryEmrBatchInForm";
            this.Text = "历史病历批量导入";
            this.Load += new System.EventHandler(this.HistoryEmrBatchInForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlHistoryEmr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewHistoryEmr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlHistoryEmr;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewHistoryEmr;
        private DevExpress.XtraEditors.SimpleButton simpleButtonOK;
        private DevExpress.XtraEditors.SimpleButton simpleButtonExit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnNoofinpat;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnInpatientName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnAdmitDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDiagName;
    }
}