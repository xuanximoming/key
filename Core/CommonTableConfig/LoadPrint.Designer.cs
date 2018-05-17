namespace DrectSoft.Core.CommonTableConfig
{
    partial class LoadPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadPrint));
            this.btnDel = new DrectSoft.Common.Ctrs.OTHER.DevButtonDelete();
            this.btnAdd = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd();
            this.gridControl1Print = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnLiulang = new DevExpress.XtraEditors.SimpleButton();
            this.txtFilestr = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtPrintName = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtPrintFileName = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1Print)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFilestr.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrintName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrintFileName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDel
            // 
            this.btnDel.Image = ((System.Drawing.Image)(resources.GetObject("btnDel.Image")));
            this.btnDel.Location = new System.Drawing.Point(601, 103);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(80, 23);
            this.btnDel.TabIndex = 1;
            this.btnDel.Text = "删除(&D)";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(501, 103);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "新增(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gridControl1Print
            // 
            this.gridControl1Print.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1Print.Location = new System.Drawing.Point(0, 0);
            this.gridControl1Print.MainView = this.gridView1;
            this.gridControl1Print.Name = "gridControl1Print";
            this.gridControl1Print.Size = new System.Drawing.Size(925, 394);
            this.gridControl1Print.TabIndex = 1;
            this.gridControl1Print.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gridView1.GridControl = this.gridControl1Print;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsDetail.ShowDetailTabs = false;
            this.gridView1.OptionsDetail.SmartDetailExpand = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridView1.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridView1.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridView1.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "文件名";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 246;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "单据名称";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 661;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnDel);
            this.panelControl2.Controls.Add(this.btnLiulang);
            this.panelControl2.Controls.Add(this.btnAdd);
            this.panelControl2.Controls.Add(this.txtFilestr);
            this.panelControl2.Controls.Add(this.txtPrintName);
            this.panelControl2.Controls.Add(this.txtPrintFileName);
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.btnSave);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 394);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(925, 140);
            this.panelControl2.TabIndex = 2;
            // 
            // btnLiulang
            // 
            this.btnLiulang.Location = new System.Drawing.Point(709, 62);
            this.btnLiulang.Name = "btnLiulang";
            this.btnLiulang.Size = new System.Drawing.Size(67, 23);
            this.btnLiulang.TabIndex = 9;
            this.btnLiulang.Text = "浏览";
            this.btnLiulang.Click += new System.EventHandler(this.btnLiulang_Click);
            // 
            // txtFilestr
            // 
            this.txtFilestr.IsEnterChangeBgColor = false;
            this.txtFilestr.IsEnterKeyToNextControl = false;
            this.txtFilestr.IsNumber = false;
            this.txtFilestr.Location = new System.Drawing.Point(81, 64);
            this.txtFilestr.Name = "txtFilestr";
            this.txtFilestr.Size = new System.Drawing.Size(616, 20);
            this.txtFilestr.TabIndex = 8;
            // 
            // txtPrintName
            // 
            this.txtPrintName.IsEnterChangeBgColor = false;
            this.txtPrintName.IsEnterKeyToNextControl = false;
            this.txtPrintName.IsNumber = false;
            this.txtPrintName.Location = new System.Drawing.Point(473, 16);
            this.txtPrintName.Name = "txtPrintName";
            this.txtPrintName.Properties.MaxLength = 50;
            this.txtPrintName.Size = new System.Drawing.Size(306, 20);
            this.txtPrintName.TabIndex = 7;
            // 
            // txtPrintFileName
            // 
            this.txtPrintFileName.IsEnterChangeBgColor = false;
            this.txtPrintFileName.IsEnterKeyToNextControl = false;
            this.txtPrintFileName.IsNumber = false;
            this.txtPrintFileName.Location = new System.Drawing.Point(81, 16);
            this.txtPrintFileName.Name = "txtPrintFileName";
            this.txtPrintFileName.Properties.MaxLength = 50;
            this.txtPrintFileName.Size = new System.Drawing.Size(306, 20);
            this.txtPrintFileName.TabIndex = 6;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(15, 64);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "模板路径：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(407, 16);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "单据名称：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(27, 16);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "文件名：";
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(696, 103);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // LoadPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 534);
            this.Controls.Add(this.gridControl1Print);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoadPrint";
            this.Text = "模板上传";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1Print)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFilestr.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrintName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrintFileName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1Print;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonDelete btnDel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd btnAdd;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnSave;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.SimpleButton btnLiulang;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtFilestr;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtPrintName;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtPrintFileName;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}