namespace DrectSoft.Core.CommonTableConfig
{
    partial class DataElementConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataElementConfig));
            this.gcdDataElement = new DevExpress.XtraGrid.GridControl();
            this.gvDataElement = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.btnSearch = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey();
            this.txtPYM = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtElementName = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtElementId = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd();
            this.btnDel = new DrectSoft.Common.Ctrs.OTHER.DevButtonDelete();
            this.btnEdit = new DrectSoft.Common.Ctrs.OTHER.DevButtonEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gcdDataElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPYM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcdDataElement
            // 
            this.gcdDataElement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcdDataElement.Location = new System.Drawing.Point(0, 85);
            this.gcdDataElement.MainView = this.gvDataElement;
            this.gcdDataElement.Name = "gcdDataElement";
            this.gcdDataElement.Size = new System.Drawing.Size(971, 492);
            this.gcdDataElement.TabIndex = 20;
            this.gcdDataElement.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataElement});
            this.gcdDataElement.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gcdDataElement_MouseDoubleClick);
            // 
            // gvDataElement
            // 
            this.gvDataElement.ActiveFilterEnabled = false;
            this.gvDataElement.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7});
            this.gvDataElement.GridControl = this.gcdDataElement;
            this.gvDataElement.IndicatorWidth = 40;
            this.gvDataElement.Name = "gvDataElement";
            this.gvDataElement.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvDataElement.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvDataElement.OptionsBehavior.Editable = false;
            this.gvDataElement.OptionsDetail.AllowZoomDetail = false;
            this.gvDataElement.OptionsDetail.EnableMasterViewMode = false;
            this.gvDataElement.OptionsDetail.ShowDetailTabs = false;
            this.gvDataElement.OptionsDetail.SmartDetailExpand = false;
            this.gvDataElement.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gvDataElement.OptionsFilter.AllowFilterEditor = false;
            this.gvDataElement.OptionsFilter.AllowMRUFilterList = false;
            this.gvDataElement.OptionsFind.AllowFindPanel = false;
            this.gvDataElement.OptionsMenu.EnableColumnMenu = false;
            this.gvDataElement.OptionsMenu.EnableFooterMenu = false;
            this.gvDataElement.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDataElement.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvDataElement.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvDataElement.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvDataElement.OptionsView.ShowGroupPanel = false;
            this.gvDataElement.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvDataElement_CustomDrawRowIndicator);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "数据元ID";
            this.gridColumn1.FieldName = "ElementId";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn1.OptionsFilter.AllowFilter = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "数据元名称";
            this.gridColumn2.FieldName = "ElementName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "数据类型";
            this.gridColumn3.FieldName = "ElementType";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "数据格式";
            this.gridColumn4.FieldName = "ElementForm";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn4.OptionsFilter.AllowFilter = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "所属类别";
            this.gridColumn5.FieldName = "ElementClass";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn5.OptionsFilter.AllowFilter = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "卫生部数据元";
            this.gridColumn6.FieldName = "IsDataElemet";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn6.OptionsFilter.AllowFilter = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "拼音码";
            this.gridColumn7.FieldName = "ElementPYM";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn7.OptionsFilter.AllowFilter = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            // 
            // cboClass
            // 
            this.cboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClass.FormattingEnabled = true;
            this.cboClass.Location = new System.Drawing.Point(635, 14);
            this.cboClass.MaxDropDownItems = 16;
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(144, 22);
            this.cboClass.TabIndex = 14;
            // 
            // btnSearch
            // 
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(788, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 23);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "查询(&Q)";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtPYM
            // 
            this.txtPYM.EnterMoveNextControl = true;
            this.txtPYM.IsEnterChangeBgColor = false;
            this.txtPYM.IsEnterKeyToNextControl = false;
            this.txtPYM.IsNumber = false;
            this.txtPYM.Location = new System.Drawing.Point(458, 14);
            this.txtPYM.Name = "txtPYM";
            this.txtPYM.Size = new System.Drawing.Size(100, 20);
            this.txtPYM.TabIndex = 13;
            // 
            // txtElementName
            // 
            this.txtElementName.EnterMoveNextControl = true;
            this.txtElementName.IsEnterChangeBgColor = false;
            this.txtElementName.IsEnterKeyToNextControl = false;
            this.txtElementName.IsNumber = false;
            this.txtElementName.Location = new System.Drawing.Point(290, 14);
            this.txtElementName.Name = "txtElementName";
            this.txtElementName.Size = new System.Drawing.Size(100, 20);
            this.txtElementName.TabIndex = 12;
            // 
            // txtElementId
            // 
            this.txtElementId.EnterMoveNextControl = true;
            this.txtElementId.IsEnterChangeBgColor = false;
            this.txtElementId.IsEnterKeyToNextControl = false;
            this.txtElementId.IsNumber = false;
            this.txtElementId.Location = new System.Drawing.Point(95, 14);
            this.txtElementId.Name = "txtElementId";
            this.txtElementId.Size = new System.Drawing.Size(100, 20);
            this.txtElementId.TabIndex = 11;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(574, 17);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "所属类别：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(406, 17);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "拼音码：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(215, 17);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "数据元名称：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(33, 17);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "数据元ID：";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(971, 48);
            this.label1.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(33, 7);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 17;
            this.btnAdd.Text = "新增(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Image = ((System.Drawing.Image)(resources.GetObject("btnDel.Image")));
            this.btnDel.Location = new System.Drawing.Point(219, 7);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(80, 23);
            this.btnDel.TabIndex = 19;
            this.btnDel.Text = "删除(&D)";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.Location = new System.Drawing.Point(127, 7);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 23);
            this.btnEdit.TabIndex = 18;
            this.btnEdit.Text = "编辑(&E)";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnAdd);
            this.panelControl1.Controls.Add(this.btnEdit);
            this.panelControl1.Controls.Add(this.btnDel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 48);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(971, 37);
            this.panelControl1.TabIndex = 16;
            this.panelControl1.TabStop = true;
            // 
            // DataElementConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 577);
            this.Controls.Add(this.gcdDataElement);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.cboClass);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtElementId);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtPYM);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtElementName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DataElementConfig";
            this.Text = "基础数据元维护";
            ((System.ComponentModel.ISupportInitialize)(this.gcdDataElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPYM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.GridControl gcdDataElement;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataElement;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey btnSearch;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtPYM;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtElementName;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtElementId;
        private System.Windows.Forms.ComboBox cboClass;
        private System.Windows.Forms.Label label1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd btnAdd;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonDelete btnDel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonEdit btnEdit;
        private DevExpress.XtraEditors.PanelControl panelControl1;



    }
}