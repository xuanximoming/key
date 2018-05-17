namespace DrectSoft.Core.MedicalRecordQuery
{
    partial class UCMaintenance
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMaintenance));
            this.pcTop = new DevExpress.XtraEditors.PanelControl();
            this.btnDelete = new DrectSoft.Common.Ctrs.OTHER.DevButtonDelete(this.components);
            this.btnEdit = new DrectSoft.Common.Ctrs.OTHER.DevButtonEdit(this.components);
            this.btnAdd = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd(this.components);
            this.txtGroupName = new DevExpress.XtraEditors.TextEdit();
            this.lblGroupName = new DevExpress.XtraEditors.LabelControl();
            this.lblSearch = new DevExpress.XtraEditors.LabelControl();
            this.pcFill = new DevExpress.XtraEditors.PanelControl();
            this.gcGroupList = new DevExpress.XtraGrid.GridControl();
            this.gvList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcmGroupName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcmGroupList = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pcTop)).BeginInit();
            this.pcTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcFill)).BeginInit();
            this.pcFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcGroupList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).BeginInit();
            this.SuspendLayout();
            // 
            // pcTop
            // 
            this.pcTop.Controls.Add(this.btnDelete);
            this.pcTop.Controls.Add(this.btnEdit);
            this.pcTop.Controls.Add(this.btnAdd);
            this.pcTop.Controls.Add(this.txtGroupName);
            this.pcTop.Controls.Add(this.lblGroupName);
            this.pcTop.Controls.Add(this.lblSearch);
            this.pcTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pcTop.Location = new System.Drawing.Point(0, 0);
            this.pcTop.Name = "pcTop";
            this.pcTop.Size = new System.Drawing.Size(966, 54);
            this.pcTop.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Location = new System.Drawing.Point(537, 17);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "删除(&D)";
            // 
            // btnEdit
            // 
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.Location = new System.Drawing.Point(444, 17);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "编辑(&E)";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(351, 17);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "新增(&A)";
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(210, 17);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(100, 21);
            this.txtGroupName.TabIndex = 2;
            // 
            // lblGroupName
            // 
            this.lblGroupName.Location = new System.Drawing.Point(147, 20);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(60, 14);
            this.lblGroupName.TabIndex = 1;
            this.lblGroupName.Text = "组合名称：";
            // 
            // lblSearch
            // 
            this.lblSearch.Location = new System.Drawing.Point(48, 19);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(75, 14);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "检索条件>>>";
            // 
            // pcFill
            // 
            this.pcFill.Controls.Add(this.gcGroupList);
            this.pcFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcFill.Location = new System.Drawing.Point(0, 54);
            this.pcFill.Name = "pcFill";
            this.pcFill.Size = new System.Drawing.Size(966, 378);
            this.pcFill.TabIndex = 1;
            // 
            // gcGroupList
            // 
            this.gcGroupList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcGroupList.Location = new System.Drawing.Point(2, 2);
            this.gcGroupList.MainView = this.gvList;
            this.gcGroupList.Name = "gcGroupList";
            this.gcGroupList.Size = new System.Drawing.Size(962, 374);
            this.gcGroupList.TabIndex = 0;
            this.gcGroupList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvList});
            // 
            // gvList
            // 
            this.gvList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcmGroupName,
            this.gcmGroupList});
            this.gvList.GridControl = this.gcGroupList;
            this.gvList.IndicatorWidth = 43;
            this.gvList.Name = "gvList";
            this.gvList.OptionsView.ShowGroupPanel = false;
            // 
            // gcmGroupName
            // 
            this.gcmGroupName.AppearanceHeader.Options.UseTextOptions = true;
            this.gcmGroupName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcmGroupName.Caption = "组合名称";
            this.gcmGroupName.Name = "gcmGroupName";
            this.gcmGroupName.Visible = true;
            this.gcmGroupName.VisibleIndex = 0;
            // 
            // gcmGroupList
            // 
            this.gcmGroupList.AppearanceHeader.Options.UseTextOptions = true;
            this.gcmGroupList.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcmGroupList.Caption = "组合病种";
            this.gcmGroupList.Name = "gcmGroupList";
            this.gcmGroupList.Visible = true;
            this.gcmGroupList.VisibleIndex = 1;
            // 
            // UCMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pcFill);
            this.Controls.Add(this.pcTop);
            this.Name = "UCMaintenance";
            this.Size = new System.Drawing.Size(966, 432);
            ((System.ComponentModel.ISupportInitialize)(this.pcTop)).EndInit();
            this.pcTop.ResumeLayout(false);
            this.pcTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcFill)).EndInit();
            this.pcFill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcGroupList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pcTop;
        private DevExpress.XtraEditors.PanelControl pcFill;
        private DevExpress.XtraEditors.LabelControl lblSearch;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonDelete btnDelete;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonEdit btnEdit;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd btnAdd;
        private DevExpress.XtraEditors.TextEdit txtGroupName;
        private DevExpress.XtraEditors.LabelControl lblGroupName;
        private DevExpress.XtraGrid.GridControl gcGroupList;
        private DevExpress.XtraGrid.Views.Grid.GridView gvList;
        private DevExpress.XtraGrid.Columns.GridColumn gcmGroupName;
        private DevExpress.XtraGrid.Columns.GridColumn gcmGroupList;
    }
}
