namespace DrectSoft.Core.NursingDocuments
{
    partial class ChangePatient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePatient));
            this.textEditModelName = new DevExpress.XtraEditors.TextEdit();
            this.TemplatePerson = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnMemo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTemplateID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnCatalog = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnContent = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnPY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnWB = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnPersonID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControlTemplatePerson = new DevExpress.XtraGrid.GridControl();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDepartmentID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btn_deldepttemp = new DevExpress.XtraBars.BarButtonItem();
            this.btn_DelPersonTemplate = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.xtraTabPageTemplatePerson = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtSearch = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridViewPaient = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.textEditModelName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TemplatePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTemplatePerson)).BeginInit();
            this.xtraTabPageTemplatePerson.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPaient)).BeginInit();
            this.SuspendLayout();
            // 
            // textEditModelName
            // 
            this.textEditModelName.Location = new System.Drawing.Point(88, 13);
            this.textEditModelName.Name = "textEditModelName";
            this.textEditModelName.Size = new System.Drawing.Size(325, 21);
            this.textEditModelName.TabIndex = 1;
            // 
            // TemplatePerson
            // 
            this.TemplatePerson.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnName,
            this.gridColumnMemo,
            this.gridColumnTemplateID,
            this.gridColumnCatalog,
            this.gridColumnContent,
            this.gridColumnPY,
            this.gridColumnWB,
            this.gridColumnPersonID});
            this.TemplatePerson.GridControl = this.gridControlTemplatePerson;
            this.TemplatePerson.Name = "TemplatePerson";
            this.TemplatePerson.OptionsView.ShowGroupPanel = false;
            this.TemplatePerson.OptionsView.ShowIndicator = false;
            // 
            // gridColumnName
            // 
            this.gridColumnName.Caption = "模板名";
            this.gridColumnName.FieldName = "MR_NAME";
            this.gridColumnName.Name = "gridColumnName";
            this.gridColumnName.OptionsColumn.AllowEdit = false;
            this.gridColumnName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnName.OptionsFilter.AllowFilter = false;
            this.gridColumnName.Visible = true;
            this.gridColumnName.VisibleIndex = 0;
            this.gridColumnName.Width = 194;
            // 
            // gridColumnMemo
            // 
            this.gridColumnMemo.Caption = "备注";
            this.gridColumnMemo.FieldName = "MEMO";
            this.gridColumnMemo.Name = "gridColumnMemo";
            this.gridColumnMemo.OptionsColumn.AllowEdit = false;
            this.gridColumnMemo.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnMemo.OptionsFilter.AllowFilter = false;
            this.gridColumnMemo.Visible = true;
            this.gridColumnMemo.VisibleIndex = 1;
            this.gridColumnMemo.Width = 318;
            // 
            // gridColumnTemplateID
            // 
            this.gridColumnTemplateID.Caption = "模板ID";
            this.gridColumnTemplateID.FieldName = "TEMPLETID";
            this.gridColumnTemplateID.Name = "gridColumnTemplateID";
            // 
            // gridColumnCatalog
            // 
            this.gridColumnCatalog.Caption = "模板类别";
            this.gridColumnCatalog.FieldName = "SORTID";
            this.gridColumnCatalog.Name = "gridColumnCatalog";
            // 
            // gridColumnContent
            // 
            this.gridColumnContent.Caption = "模板内容";
            this.gridColumnContent.FieldName = "CONTENT";
            this.gridColumnContent.Name = "gridColumnContent";
            // 
            // gridColumnPY
            // 
            this.gridColumnPY.Caption = "拼音";
            this.gridColumnPY.FieldName = "PY";
            this.gridColumnPY.Name = "gridColumnPY";
            this.gridColumnPY.Visible = true;
            this.gridColumnPY.VisibleIndex = 2;
            // 
            // gridColumnWB
            // 
            this.gridColumnWB.Caption = "五笔";
            this.gridColumnWB.FieldName = "WB";
            this.gridColumnWB.Name = "gridColumnWB";
            this.gridColumnWB.Visible = true;
            this.gridColumnWB.VisibleIndex = 3;
            // 
            // gridColumnPersonID
            // 
            this.gridColumnPersonID.Caption = "gridColumn11";
            this.gridColumnPersonID.FieldName = "ID";
            this.gridColumnPersonID.Name = "gridColumnPersonID";
            // 
            // gridControlTemplatePerson
            // 
            this.gridControlTemplatePerson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlTemplatePerson.Location = new System.Drawing.Point(0, 0);
            this.gridControlTemplatePerson.MainView = this.TemplatePerson;
            this.gridControlTemplatePerson.Name = "gridControlTemplatePerson";
            this.gridControlTemplatePerson.Size = new System.Drawing.Size(514, 221);
            this.gridControlTemplatePerson.TabIndex = 1;
            this.gridControlTemplatePerson.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.TemplatePerson});
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "模板内容";
            this.gridColumn5.FieldName = "CONTENT";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "拼音";
            this.gridColumn6.FieldName = "PY";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "五笔";
            this.gridColumn7.FieldName = "WB";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 3;
            // 
            // gridColumnDepartmentID
            // 
            this.gridColumnDepartmentID.Caption = "gridColumn11";
            this.gridColumnDepartmentID.FieldName = "ID";
            this.gridColumnDepartmentID.Name = "gridColumnDepartmentID";
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 339);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(520, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 339);
            // 
            // btn_deldepttemp
            // 
            this.btn_deldepttemp.Caption = "删除";
            this.btn_deldepttemp.Id = 0;
            this.btn_deldepttemp.Name = "btn_deldepttemp";
            // 
            // btn_DelPersonTemplate
            // 
            this.btn_DelPersonTemplate.Caption = "删除";
            this.btn_DelPersonTemplate.Id = 1;
            this.btn_DelPersonTemplate.Name = "btn_DelPersonTemplate";
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 339);
            this.barDockControlBottom.Size = new System.Drawing.Size(520, 0);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(520, 0);
            // 
            // xtraTabPageTemplatePerson
            // 
            this.xtraTabPageTemplatePerson.Controls.Add(this.gridControlTemplatePerson);
            this.xtraTabPageTemplatePerson.Name = "xtraTabPageTemplatePerson";
            this.xtraTabPageTemplatePerson.Size = new System.Drawing.Size(514, 221);
            this.xtraTabPageTemplatePerson.Text = "个人模板";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.groupBox1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(429, 62);
            this.panelControl2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(425, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "检索条件";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "搜索：";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(46, 21);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txtSearch.Size = new System.Drawing.Size(200, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.ToolTip = "支持姓名(汉字/拼音/五笔)、性别、住院号、床位号检索";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_Cancel);
            this.panelControl1.Controls.Add(this.btn_OK);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 326);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(429, 45);
            this.panelControl1.TabIndex = 1;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Image = global::DrectSoft.Core.NursingDocuments.Properties.Resources.取消;
            this.btn_Cancel.Location = new System.Drawing.Point(337, 10);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(80, 27);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "取消 (&C)";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OK.Image = global::DrectSoft.Core.NursingDocuments.Properties.Resources.确定;
            this.btn_OK.Location = new System.Drawing.Point(251, 10);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(80, 27);
            this.btn_OK.TabIndex = 0;
            this.btn_OK.Text = "确定 (&Y)";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gridControl1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 62);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(429, 264);
            this.panelControl3.TabIndex = 2;
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gridViewPaient;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(425, 260);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.TabStop = false;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewPaient});
            this.gridControl1.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
            // 
            // gridViewPaient
            // 
            this.gridViewPaient.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.gridViewPaient.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridViewPaient.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn8,
            this.gridColumn4});
            this.gridViewPaient.GridControl = this.gridControl1;
            this.gridViewPaient.IndicatorWidth = 40;
            this.gridViewPaient.Name = "gridViewPaient";
            this.gridViewPaient.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewPaient.OptionsBehavior.Editable = false;
            this.gridViewPaient.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewPaient.OptionsCustomization.AllowFilter = false;
            this.gridViewPaient.OptionsMenu.EnableColumnMenu = false;
            this.gridViewPaient.OptionsMenu.EnableFooterMenu = false;
            this.gridViewPaient.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewPaient.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewPaient.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewPaient.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewPaient.OptionsView.ShowGroupPanel = false;
            this.gridViewPaient.OptionsView.ShowViewCaption = true;
            this.gridViewPaient.ViewCaption = " 病人列表";
            this.gridViewPaient.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridViewPaient_CustomDrawRowIndicator);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "病人姓名";
            this.gridColumn1.FieldName = "PATNAME";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn1.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn1.OptionsFilter.AllowFilter = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 100;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "性别";
            this.gridColumn2.FieldName = "SEXNAME";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 50;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "床位号";
            this.gridColumn3.FieldName = "BEDID";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 80;
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.Caption = "住院号";
            this.gridColumn8.FieldName = "PATID";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn8.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn8.OptionsFilter.AllowFilter = false;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 3;
            this.gridColumn8.Width = 136;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "首页序号";
            this.gridColumn4.FieldName = "NOOFINPAT";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn4.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn4.OptionsFilter.AllowFilter = false;
            // 
            // ChangePatient
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(429, 371);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePatient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "切换病人";
            this.Load += new System.EventHandler(this.ChangePatient_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEditModelName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TemplatePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTemplatePerson)).EndInit();
            this.xtraTabPageTemplatePerson.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPaient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textEditModelName;
        private DevExpress.XtraGrid.Views.Grid.GridView TemplatePerson;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnMemo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTemplateID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnCatalog;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnContent;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnPY;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnWB;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnPersonID;
        private DevExpress.XtraGrid.GridControl gridControlTemplatePerson;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDepartmentID;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btn_deldepttemp;
        private DevExpress.XtraBars.BarButtonItem btn_DelPersonTemplate;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageTemplatePerson;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.TextEdit txtSearch;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_Cancel;
        private DevExpress.XtraEditors.SimpleButton btn_OK;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewPaient;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}