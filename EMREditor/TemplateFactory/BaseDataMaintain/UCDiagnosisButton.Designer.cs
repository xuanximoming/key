namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    partial class UCDiagnosisButton
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDiagnosisButton));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.textEditName = new DevExpress.XtraEditors.TextEdit();
            this.gridControlDiag = new DevExpress.XtraGrid.GridControl();
            this.gridViewDiag = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.simpleButtonCancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel();
            this.simpleButtonSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave();
            this.simpleButtonDelete = new DrectSoft.Common.Ctrs.OTHER.DevButtonDelete();
            this.simpleButtonEdit = new DrectSoft.Common.Ctrs.OTHER.DevButtonEdit();
            this.simpleButtonAdd = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiag)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.simpleButtonAdd);
            this.panelControl1.Controls.Add(this.simpleButtonEdit);
            this.panelControl1.Controls.Add(this.simpleButtonDelete);
            this.panelControl1.Controls.Add(this.simpleButtonSave);
            this.panelControl1.Controls.Add(this.simpleButtonCancel);
            this.panelControl1.Controls.Add(this.textEditName);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 260);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(710, 60);
            this.panelControl1.TabIndex = 0;
            // 
            // textEditName
            // 
            this.textEditName.Location = new System.Drawing.Point(76, 20);
            this.textEditName.Name = "textEditName";
            this.textEditName.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditName.Size = new System.Drawing.Size(120, 21);
            this.textEditName.TabIndex = 5;
            // 
            // gridControlDiag
            // 
            this.gridControlDiag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlDiag.Location = new System.Drawing.Point(0, 0);
            this.gridControlDiag.MainView = this.gridViewDiag;
            this.gridControlDiag.Name = "gridControlDiag";
            this.gridControlDiag.Size = new System.Drawing.Size(710, 260);
            this.gridControlDiag.TabIndex = 1;
            this.gridControlDiag.TabStop = false;
            this.gridControlDiag.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDiag});
            // 
            // gridViewDiag
            // 
            this.gridViewDiag.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnName});
            this.gridViewDiag.GridControl = this.gridControlDiag;
            this.gridViewDiag.IndicatorWidth = 40;
            this.gridViewDiag.Name = "gridViewDiag";
            this.gridViewDiag.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewDiag.OptionsBehavior.Editable = false;
            this.gridViewDiag.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewDiag.OptionsCustomization.AllowColumnResizing = false;
            this.gridViewDiag.OptionsCustomization.AllowFilter = false;
            this.gridViewDiag.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewDiag.OptionsFilter.AllowFilterEditor = false;
            this.gridViewDiag.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewDiag.OptionsMenu.EnableColumnMenu = false;
            this.gridViewDiag.OptionsMenu.EnableFooterMenu = false;
            this.gridViewDiag.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewDiag.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewDiag.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewDiag.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewDiag.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewDiag.OptionsView.ShowGroupPanel = false;
            this.gridViewDiag.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridViewDiag_CustomDrawRowIndicator);
            this.gridViewDiag.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewDiag_FocusedRowChanged);
            // 
            // gridColumnName
            // 
            this.gridColumnName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnName.Caption = "名称";
            this.gridColumnName.FieldName = "DIAGNAME";
            this.gridColumnName.Name = "gridColumnName";
            this.gridColumnName.Visible = true;
            this.gridColumnName.VisibleIndex = 0;
            this.gridColumnName.Width = 370;
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonCancel.Image")));
            this.simpleButtonCancel.Location = new System.Drawing.Point(592, 17);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonCancel.TabIndex = 4;
            this.simpleButtonCancel.Text = "取消(&C)";
            this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
            // 
            // simpleButtonSave
            // 
            this.simpleButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonSave.Image")));
            this.simpleButtonSave.Location = new System.Drawing.Point(506, 17);
            this.simpleButtonSave.Name = "simpleButtonSave";
            this.simpleButtonSave.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonSave.TabIndex = 3;
            this.simpleButtonSave.Text = "保存(&S)";
            this.simpleButtonSave.Click += new System.EventHandler(this.simpleButtonSave_Click);
            // 
            // simpleButtonDelete
            // 
            this.simpleButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonDelete.Image")));
            this.simpleButtonDelete.Location = new System.Drawing.Point(420, 17);
            this.simpleButtonDelete.Name = "simpleButtonDelete";
            this.simpleButtonDelete.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonDelete.TabIndex = 2;
            this.simpleButtonDelete.Text = "删除(&D)";
            this.simpleButtonDelete.Click += new System.EventHandler(this.simpleButtonDelete_Click);
            // 
            // simpleButtonEdit
            // 
            this.simpleButtonEdit.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEdit.Image")));
            this.simpleButtonEdit.Location = new System.Drawing.Point(334, 17);
            this.simpleButtonEdit.Name = "simpleButtonEdit";
            this.simpleButtonEdit.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonEdit.TabIndex = 1;
            this.simpleButtonEdit.Text = "编辑(&E)";
            this.simpleButtonEdit.Click += new System.EventHandler(this.simpleButtonEdit_Click);
            // 
            // simpleButtonAdd
            // 
            this.simpleButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonAdd.Image")));
            this.simpleButtonAdd.Location = new System.Drawing.Point(248, 17);
            this.simpleButtonAdd.Name = "simpleButtonAdd";
            this.simpleButtonAdd.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonAdd.TabIndex = 0;
            this.simpleButtonAdd.Text = "新增(&A)";
            this.simpleButtonAdd.Click += new System.EventHandler(this.simpleButtonAdd_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "按钮名称：";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(200, 27);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(8, 14);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "*";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // UCDiagnosisButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlDiag);
            this.Controls.Add(this.panelControl1);
            this.Name = "UCDiagnosisButton";
            this.Size = new System.Drawing.Size(710, 320);
            this.Load += new System.EventHandler(this.UCDiagnosisButton_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiag)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gridControlDiag;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDiag;
        private DevExpress.XtraEditors.TextEdit textEditName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd simpleButtonAdd;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonEdit simpleButtonEdit;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonDelete simpleButtonDelete;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave simpleButtonSave;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel simpleButtonCancel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}
