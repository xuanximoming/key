namespace CommonTableConfig
{
    partial class CommonNote_ModelWeiHu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonNote_ModelWeiHu));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnReSet = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset();
            this.btnSearch = new DrectSoft.Common.Ctrs.OTHER.DevButtonFind();
            this.txtDescription = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtFileName = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnLoad = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DrectSoft.Common.Ctrs.OTHER.DevButtonDelete();
            this.btnEdit = new DrectSoft.Common.Ctrs.OTHER.DevButtonEdit();
            this.btnAdd = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd();
            this.gcCommonModel = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCommonModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnReSet);
            this.panelControl1.Controls.Add(this.btnSearch);
            this.panelControl1.Controls.Add(this.txtDescription);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.txtFileName);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(940, 48);
            this.panelControl1.TabIndex = 0;
            // 
            // btnReSet
            // 
            this.btnReSet.Image = ((System.Drawing.Image)(resources.GetObject("btnReSet.Image")));
            this.btnReSet.Location = new System.Drawing.Point(578, 8);
            this.btnReSet.Name = "btnReSet";
            this.btnReSet.Size = new System.Drawing.Size(80, 23);
            this.btnReSet.TabIndex = 4;
            this.btnReSet.Text = "重置(&B)";
            this.btnReSet.Click += new System.EventHandler(this.btnReSet_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(482, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "搜索(&F)";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.EnterMoveNextControl = true;
            this.txtDescription.IsEnterChangeBgColor = false;
            this.txtDescription.IsEnterKeyToNextControl = false;
            this.txtDescription.IsNumber = false;
            this.txtDescription.Location = new System.Drawing.Point(306, 10);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(147, 20);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.ToolTip = "描述";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(264, 13);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "描述：";
            // 
            // txtFileName
            // 
            this.txtFileName.EnterMoveNextControl = true;
            this.txtFileName.IsEnterChangeBgColor = false;
            this.txtFileName.IsEnterKeyToNextControl = false;
            this.txtFileName.IsNumber = false;
            this.txtFileName.Location = new System.Drawing.Point(92, 10);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(147, 20);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.ToolTip = "文件名";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(38, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "文件名：";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnLoad);
            this.panelControl2.Controls.Add(this.btnDelete);
            this.panelControl2.Controls.Add(this.btnEdit);
            this.panelControl2.Controls.Add(this.btnAdd);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 48);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(940, 37);
            this.panelControl2.TabIndex = 1;
            // 
            // btnLoad
            // 
            this.btnLoad.Image = global::DrectSoft.Core.CommonTableConfig.Properties.Resources._07057;
            this.btnLoad.Location = new System.Drawing.Point(296, 9);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "下载(&L)";
            this.btnLoad.ToolTip = "下载文件";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Location = new System.Drawing.Point(210, 9);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.ToolTip = "删除选中文件";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.Location = new System.Drawing.Point(124, 9);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 23);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "编辑(&E)";
            this.btnEdit.ToolTip = "编辑文件";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(38, 9);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "新增(&A)";
            this.btnAdd.ToolTip = "新增文件";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gcCommonModel
            // 
            this.gcCommonModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCommonModel.Location = new System.Drawing.Point(0, 85);
            this.gcCommonModel.MainView = this.gridView1;
            this.gcCommonModel.Name = "gcCommonModel";
            this.gcCommonModel.Size = new System.Drawing.Size(940, 469);
            this.gcCommonModel.TabIndex = 2;
            this.gcCommonModel.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gcCommonModel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gcCommonModel_MouseDoubleClick);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gridView1.GridControl = this.gcCommonModel;
            this.gridView1.IndicatorWidth = 40;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "文件名";
            this.gridColumn1.FieldName = "TEMPNAME";
            this.gridColumn1.ImageAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "描述";
            this.gridColumn2.FieldName = "TEMPDESC";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "创建时间";
            this.gridColumn3.FieldName = "CREATEDATETIME";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "修改时间";
            this.gridColumn4.FieldName = "MODIFYDATETIME";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // CommonNote_ModelWeiHu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 554);
            this.Controls.Add(this.gcCommonModel);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommonNote_ModelWeiHu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "模板维护";
            this.Load += new System.EventHandler(this.CommonNote_ModelWeiHu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCommonModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtFileName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtDescription;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonFind btnSearch;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonEdit btnEdit;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd btnAdd;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonDelete btnDelete;
        private DevExpress.XtraGrid.GridControl gcCommonModel;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.SimpleButton btnLoad;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset btnReSet;
    }
}