namespace DrectSoft.EMR.ThreeRecordAll
{
    partial class InPatListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InPatListForm));
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtPatId = new DevExpress.XtraEditors.TextEdit();
            this.gcInpatient = new DevExpress.XtraGrid.GridControl();
            this.gvInpatient = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcInpatient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInpatient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(12, 32);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(258, 25);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "注：可输入姓名、住院号、床号查询";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(8, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "筛选条件>>>";
            // 
            // txtPatId
            // 
            this.txtPatId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPatId.Location = new System.Drawing.Point(87, 9);
            this.txtPatId.Name = "txtPatId";
            this.txtPatId.Size = new System.Drawing.Size(167, 20);
            this.txtPatId.TabIndex = 0;
            this.txtPatId.EditValueChanged += new System.EventHandler(this.txtPatId_EditValueChanged);
            this.txtPatId.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPatId_KeyUp);
            // 
            // gcInpatient
            // 
            this.gcInpatient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcInpatient.Location = new System.Drawing.Point(0, 83);
            this.gcInpatient.MainView = this.gvInpatient;
            this.gcInpatient.Name = "gcInpatient";
            this.gcInpatient.Size = new System.Drawing.Size(264, 424);
            this.gcInpatient.TabIndex = 7;
            this.gcInpatient.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvInpatient});
            // 
            // gvInpatient
            // 
            this.gvInpatient.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gvInpatient.GridControl = this.gcInpatient;
            this.gvInpatient.IndicatorWidth = 40;
            this.gvInpatient.Name = "gvInpatient";
            this.gvInpatient.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvInpatient.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvInpatient.OptionsBehavior.Editable = false;
            this.gvInpatient.OptionsCustomization.AllowColumnMoving = false;
            this.gvInpatient.OptionsCustomization.AllowFilter = false;
            this.gvInpatient.OptionsCustomization.AllowGroup = false;
            this.gvInpatient.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvInpatient.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gvInpatient.OptionsFilter.AllowFilterEditor = false;
            this.gvInpatient.OptionsFilter.AllowMRUFilterList = false;
            this.gvInpatient.OptionsMenu.EnableColumnMenu = false;
            this.gvInpatient.OptionsView.ShowGroupPanel = false;
            this.gvInpatient.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvDataElement_CustomDrawRowIndicator);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "姓名";
            this.gridColumn1.FieldName = "NAME";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 64;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "住院号";
            this.gridColumn2.FieldName = "PATID";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 89;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "床号";
            this.gridColumn3.FieldName = "OUTBED";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 69;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(264, 83);
            this.panelControl1.TabIndex = 8;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Location = new System.Drawing.Point(33, 64);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(52, 14);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "双击病人";
            // 
            // InPatListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 507);
            this.Controls.Add(this.gcInpatient);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtPatId);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InPatListForm";
            this.Text = "InPatListForm";
            this.Load += new System.EventHandler(this.InPatListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtPatId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcInpatient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInpatient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtPatId;
        private DevExpress.XtraGrid.GridControl gcInpatient;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        public DevExpress.XtraGrid.Views.Grid.GridView gvInpatient;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}