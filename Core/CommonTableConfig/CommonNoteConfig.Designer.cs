namespace DrectSoft.Core.CommonTableConfig
{
    partial class CommonNoteConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonNoteConfig));
            this.gcDataCommon = new DevExpress.XtraGrid.GridControl();
            this.gvDataCommon = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnModelWeiHu = new DevExpress.XtraEditors.SimpleButton();
            this.btnCopy = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd(this.components);
            this.btnEdit = new DrectSoft.Common.Ctrs.OTHER.DevButtonEdit(this.components);
            this.btnDel = new DrectSoft.Common.Ctrs.OTHER.DevButtonDelete(this.components);
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.txtCommonNoteName = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.btnSearch = new DrectSoft.Common.Ctrs.OTHER.DevButtonFind(this.components);
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gcDataCommon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataCommon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCommonNoteName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gcDataCommon
            // 
            this.gcDataCommon.Cursor = System.Windows.Forms.Cursors.Default;
            this.gcDataCommon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDataCommon.Location = new System.Drawing.Point(0, 81);
            this.gcDataCommon.MainView = this.gvDataCommon;
            this.gcDataCommon.Name = "gcDataCommon";
            this.gcDataCommon.Size = new System.Drawing.Size(907, 495);
            this.gcDataCommon.TabIndex = 9;
            this.gcDataCommon.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataCommon});
            // 
            // gvDataCommon
            // 
            this.gvDataCommon.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gvDataCommon.GridControl = this.gcDataCommon;
            this.gvDataCommon.IndicatorWidth = 40;
            this.gvDataCommon.Name = "gvDataCommon";
            this.gvDataCommon.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvDataCommon.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvDataCommon.OptionsBehavior.Editable = false;
            this.gvDataCommon.OptionsCustomization.AllowColumnMoving = false;
            this.gvDataCommon.OptionsCustomization.AllowFilter = false;
            this.gvDataCommon.OptionsCustomization.AllowGroup = false;
            this.gvDataCommon.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvDataCommon.OptionsDetail.EnableMasterViewMode = false;
            this.gvDataCommon.OptionsDetail.ShowDetailTabs = false;
            this.gvDataCommon.OptionsDetail.SmartDetailExpand = false;
            this.gvDataCommon.OptionsMenu.EnableColumnMenu = false;
            this.gvDataCommon.OptionsMenu.EnableFooterMenu = false;
            this.gvDataCommon.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDataCommon.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvDataCommon.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvDataCommon.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvDataCommon.OptionsView.ShowGroupPanel = false;
            this.gvDataCommon.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvDataCommon_CustomDrawRowIndicator);
            this.gvDataCommon.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDataCommon_FocusedRowChanged);
            this.gvDataCommon.DoubleClick += new System.EventHandler(this.gvDataCommon_DoubleClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "通用单名称";
            this.gridColumn1.FieldName = "CommonNoteName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn1.OptionsFilter.AllowFilter = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "使用科室";
            this.gridColumn2.FieldName = "DepartForShow";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "使用病区";
            this.gridColumn3.FieldName = "AreasForShow";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "创建时间";
            this.gridColumn4.FieldName = "CreateDateTime";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnModelWeiHu);
            this.panelControl1.Controls.Add(this.btnCopy);
            this.panelControl1.Controls.Add(this.btnAdd);
            this.panelControl1.Controls.Add(this.btnEdit);
            this.panelControl1.Controls.Add(this.btnDel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 44);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(907, 37);
            this.panelControl1.TabIndex = 3;
            // 
            // btnModelWeiHu
            // 
            this.btnModelWeiHu.Image = ((System.Drawing.Image)(resources.GetObject("btnModelWeiHu.Image")));
            this.btnModelWeiHu.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnModelWeiHu.Location = new System.Drawing.Point(373, 7);
            this.btnModelWeiHu.Name = "btnModelWeiHu";
            this.btnModelWeiHu.Size = new System.Drawing.Size(100, 23);
            this.btnModelWeiHu.TabIndex = 8;
            this.btnModelWeiHu.Text = "模板维护(&V)";
            this.btnModelWeiHu.ToolTip = "模板维护";
            this.btnModelWeiHu.Click += new System.EventHandler(this.btnModelWeiHu_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.Location = new System.Drawing.Point(291, 7);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 7;
            this.btnCopy.Text = "复制(&C)";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(32, 7);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "新增(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.Location = new System.Drawing.Point(118, 7);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 23);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "编辑(&E)";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDel
            // 
            this.btnDel.Image = ((System.Drawing.Image)(resources.GetObject("btnDel.Image")));
            this.btnDel.Location = new System.Drawing.Point(204, 7);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(80, 23);
            this.btnDel.TabIndex = 6;
            this.btnDel.Text = "删除(&D)";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.txtCommonNoteName);
            this.panelControl2.Controls.Add(this.btnSearch);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(907, 44);
            this.panelControl2.TabIndex = 0;
            // 
            // txtCommonNoteName
            // 
            this.txtCommonNoteName.EnterMoveNextControl = true;
            this.txtCommonNoteName.IsEnterChangeBgColor = false;
            this.txtCommonNoteName.IsEnterKeyToNextControl = false;
            this.txtCommonNoteName.IsNumber = false;
            this.txtCommonNoteName.Location = new System.Drawing.Point(94, 14);
            this.txtCommonNoteName.Name = "txtCommonNoteName";
            this.txtCommonNoteName.Size = new System.Drawing.Size(241, 20);
            this.txtCommonNoteName.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(345, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "搜索(&F)";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(-88, 13);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(52, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "通用单名:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(34, 17);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "通用单名：";
            // 
            // CommonNoteConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 576);
            this.Controls.Add(this.gcDataCommon);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommonNoteConfig";
            this.Text = "通用单据配置";
            ((System.ComponentModel.ISupportInitialize)(this.gcDataCommon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataCommon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCommonNoteName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.GridControl gcDataCommon;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataCommon;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtCommonNoteName;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonFind btnSearch;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonDelete btnDel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonEdit btnEdit;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd btnAdd;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCopy;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.SimpleButton btnModelWeiHu;
    }
}