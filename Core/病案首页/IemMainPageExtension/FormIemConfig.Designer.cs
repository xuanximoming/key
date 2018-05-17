namespace IemMainPageExtension
{
    partial class FormIemConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIemConfig));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnDown = new DevExpress.XtraEditors.SimpleButton();
            this.btnUp = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnDel = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.btnSaveIem = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gvMainPage = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DATEELEMENTFLOW = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ELEMENTNAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSearchLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ElementTypeDescribe = new DevExpress.XtraGrid.Columns.GridColumn();
            this.IEMEXNAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.IEMOTHERNAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.IEMCONTROL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ISOTHERLINE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMainPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnDown);
            this.panelControl1.Controls.Add(this.btnUp);
            this.panelControl1.Controls.Add(this.btnAdd);
            this.panelControl1.Controls.Add(this.btnDel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(80, 461);
            this.panelControl1.TabIndex = 0;
            // 
            // btnDown
            // 
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(10, 118);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(60, 20);
            this.btnDown.TabIndex = 13;
            // 
            // btnUp
            // 
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(10, 89);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(60, 20);
            this.btnUp.TabIndex = 12;
            // 
            // btnAdd
            // 
            this.btnAdd.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Appearance.Options.UseForeColor = true;
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdd.Location = new System.Drawing.Point(10, 31);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(60, 20);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "新增行";
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(10, 60);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(60, 20);
            this.btnDel.TabIndex = 15;
            this.btnDel.Text = "删除行";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.panelControl4);
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(80, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(669, 461);
            this.panelControl2.TabIndex = 1;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.btnSaveIem);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl4.Location = new System.Drawing.Point(2, 406);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(665, 53);
            this.panelControl4.TabIndex = 3;
            // 
            // btnSaveIem
            // 
            this.btnSaveIem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveIem.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveIem.Image")));
            this.btnSaveIem.Location = new System.Drawing.Point(542, 20);
            this.btnSaveIem.Name = "btnSaveIem";
            this.btnSaveIem.Size = new System.Drawing.Size(80, 23);
            this.btnSaveIem.TabIndex = 1;
            this.btnSaveIem.Text = "保存(&S)";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gridControl1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(2, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(665, 457);
            this.panelControl3.TabIndex = 2;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gvMainPage;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemSearchLookUpEdit1});
            this.gridControl1.Size = new System.Drawing.Size(661, 453);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMainPage});
            // 
            // gvMainPage
            // 
            this.gvMainPage.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvMainPage.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Green;
            this.gvMainPage.Appearance.ViewCaption.Options.UseFont = true;
            this.gvMainPage.Appearance.ViewCaption.Options.UseForeColor = true;
            this.gvMainPage.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DATEELEMENTFLOW,
            this.ELEMENTNAME,
            this.ElementTypeDescribe,
            this.IEMEXNAME,
            this.IEMOTHERNAME,
            this.IEMCONTROL,
            this.ISOTHERLINE});
            this.gvMainPage.GridControl = this.gridControl1;
            this.gvMainPage.IndicatorWidth = 35;
            this.gvMainPage.Name = "gvMainPage";
            this.gvMainPage.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvMainPage.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvMainPage.OptionsCustomization.AllowColumnMoving = false;
            this.gvMainPage.OptionsCustomization.AllowColumnResizing = false;
            this.gvMainPage.OptionsCustomization.AllowFilter = false;
            this.gvMainPage.OptionsCustomization.AllowGroup = false;
            this.gvMainPage.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvMainPage.OptionsCustomization.AllowRowSizing = true;
            this.gvMainPage.OptionsCustomization.AllowSort = false;
            this.gvMainPage.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gvMainPage.OptionsFilter.AllowFilterEditor = false;
            this.gvMainPage.OptionsFilter.AllowMRUFilterList = false;
            this.gvMainPage.OptionsMenu.EnableColumnMenu = false;
            this.gvMainPage.OptionsMenu.EnableFooterMenu = false;
            this.gvMainPage.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvMainPage.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvMainPage.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvMainPage.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvMainPage.OptionsView.ShowGroupPanel = false;
            this.gvMainPage.OptionsView.ShowViewCaption = true;
            this.gvMainPage.ViewCaption = "病案首页扩展维护";
            // 
            // DATEELEMENTFLOW
            // 
            this.DATEELEMENTFLOW.Caption = "数据元ID";
            this.DATEELEMENTFLOW.FieldName = "DateElementFlow";
            this.DATEELEMENTFLOW.Name = "DATEELEMENTFLOW";
            this.DATEELEMENTFLOW.OptionsColumn.AllowEdit = false;
            this.DATEELEMENTFLOW.Visible = true;
            this.DATEELEMENTFLOW.VisibleIndex = 0;
            // 
            // ELEMENTNAME
            // 
            this.ELEMENTNAME.Caption = "数据元名称";
            this.ELEMENTNAME.ColumnEdit = this.repositoryItemSearchLookUpEdit1;
            this.ELEMENTNAME.FieldName = "ElementName";
            this.ELEMENTNAME.Name = "ELEMENTNAME";
            this.ELEMENTNAME.Visible = true;
            this.ELEMENTNAME.VisibleIndex = 1;
            // 
            // repositoryItemSearchLookUpEdit1
            // 
            this.repositoryItemSearchLookUpEdit1.AutoHeight = false;
            this.repositoryItemSearchLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSearchLookUpEdit1.DisplayMember = "ElementName";
            this.repositoryItemSearchLookUpEdit1.Name = "repositoryItemSearchLookUpEdit1";
            this.repositoryItemSearchLookUpEdit1.NullText = "";
            this.repositoryItemSearchLookUpEdit1.ValueMember = "ElementName";
            this.repositoryItemSearchLookUpEdit1.View = this.gridView1;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowColumnResizing = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridView1.OptionsCustomization.AllowRowSizing = true;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "编码";
            this.gridColumn1.FieldName = "ElementId";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "名称";
            this.gridColumn2.FieldName = "ElementName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "拼音码";
            this.gridColumn3.FieldName = "ElementPYM";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // ElementTypeDescribe
            // 
            this.ElementTypeDescribe.Caption = "数据类型";
            this.ElementTypeDescribe.FieldName = "ElementTypeDescribe";
            this.ElementTypeDescribe.Name = "ElementTypeDescribe";
            this.ElementTypeDescribe.Visible = true;
            this.ElementTypeDescribe.VisibleIndex = 2;
            // 
            // IEMEXNAME
            // 
            this.IEMEXNAME.Caption = "列名";
            this.IEMEXNAME.FieldName = "IemExName";
            this.IEMEXNAME.Name = "IEMEXNAME";
            this.IEMEXNAME.Visible = true;
            this.IEMEXNAME.VisibleIndex = 3;
            // 
            // IEMOTHERNAME
            // 
            this.IEMOTHERNAME.Caption = "别名";
            this.IEMOTHERNAME.FieldName = "IemOtherName";
            this.IEMOTHERNAME.Name = "IEMOTHERNAME";
            this.IEMOTHERNAME.Visible = true;
            this.IEMOTHERNAME.VisibleIndex = 4;
            // 
            // IEMCONTROL
            // 
            this.IEMCONTROL.Caption = "控件名";
            this.IEMCONTROL.FieldName = "IemControl";
            this.IEMCONTROL.Name = "IEMCONTROL";
            this.IEMCONTROL.Visible = true;
            this.IEMCONTROL.VisibleIndex = 6;
            // 
            // ISOTHERLINE
            // 
            this.ISOTHERLINE.Caption = "是否换行";
            this.ISOTHERLINE.ColumnEdit = this.repositoryItemComboBox1;
            this.ISOTHERLINE.FieldName = "IsOtherLine";
            this.ISOTHERLINE.Name = "ISOTHERLINE";
            this.ISOTHERLINE.Visible = true;
            this.ISOTHERLINE.VisibleIndex = 5;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "是",
            "否"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // FormIemConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 461);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormIemConfig";
            this.Text = "病案首页扩展维护";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMainPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnDown;
        private DevExpress.XtraEditors.SimpleButton btnUp;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnDel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnSaveIem;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMainPage;
        private DevExpress.XtraGrid.Columns.GridColumn DATEELEMENTFLOW;
        private DevExpress.XtraGrid.Columns.GridColumn ELEMENTNAME;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit repositoryItemSearchLookUpEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn ElementTypeDescribe;
        private DevExpress.XtraGrid.Columns.GridColumn IEMEXNAME;
        private DevExpress.XtraGrid.Columns.GridColumn IEMOTHERNAME;
        private DevExpress.XtraGrid.Columns.GridColumn ISOTHERLINE;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraGrid.Columns.GridColumn IEMCONTROL;
    }
}