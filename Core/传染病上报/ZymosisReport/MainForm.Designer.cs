namespace DrectSoft.Core.ZymosisReport
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barLargeButtonItemSearch = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemNew = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemSave = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemDelete = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemSubmit = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemReport = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemExit = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.gridControlCardList = new DevExpress.XtraGrid.GridControl();
            this.gridViewCardList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkEditOwner = new DevExpress.XtraEditors.CheckEdit();
            this.lookUpEditDept = new DevExpress.XtraEditors.LookUpEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.textEditPatID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.textEditName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.checkButtonReject = new DevExpress.XtraEditors.CheckButton();
            this.checkButtonAudit = new DevExpress.XtraEditors.CheckButton();
            this.checkButtonSubmit = new DevExpress.XtraEditors.CheckButton();
            this.checkButtonSave = new DevExpress.XtraEditors.CheckButton();
            this.checkEditModify = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditFirst = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.barLargeButtonItemWithDraw = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemPass = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemReject = new DevExpress.XtraBars.BarLargeButtonItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.xtraTabControlCardInfo = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageCardInfo = new DevExpress.XtraTab.XtraTabPage();
            this.panelControlReprotCard = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlCardList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCardList)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditOwner.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditDept.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPatID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditModify.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditFirst.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlCardInfo)).BeginInit();
            this.xtraTabControlCardInfo.SuspendLayout();
            this.xtraTabPageCardInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barLargeButtonItemNew,
            this.barLargeButtonItemSave,
            this.barLargeButtonItemSubmit,
            this.barLargeButtonItemWithDraw,
            this.barLargeButtonItemDelete,
            this.barLargeButtonItemSearch,
            this.barLargeButtonItemPass,
            this.barLargeButtonItemReject,
            this.barLargeButtonItemReport,
            this.barLargeButtonItemExit});
            this.barManager1.LargeImages = this.imageList1;
            this.barManager1.MaxItemId = 21;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tool";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemSearch),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemNew, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemSave, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemDelete),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemSubmit, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemReport, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemExit, true)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tool";
            // 
            // barLargeButtonItemSearch
            // 
            this.barLargeButtonItemSearch.Caption = "查询(F1)";
            this.barLargeButtonItemSearch.Id = 15;
            this.barLargeButtonItemSearch.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F1);
            this.barLargeButtonItemSearch.LargeImageIndex = 29;
            this.barLargeButtonItemSearch.Name = "barLargeButtonItemSearch";
            this.barLargeButtonItemSearch.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemSearch_ItemClick);
            // 
            // barLargeButtonItemNew
            // 
            this.barLargeButtonItemNew.Caption = "新增(F2)";
            this.barLargeButtonItemNew.Id = 11;
            this.barLargeButtonItemNew.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2);
            this.barLargeButtonItemNew.LargeImageIndex = 28;
            this.barLargeButtonItemNew.Name = "barLargeButtonItemNew";
            this.barLargeButtonItemNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemNew_ItemClick);
            // 
            // barLargeButtonItemSave
            // 
            this.barLargeButtonItemSave.Caption = "保存(F3)";
            this.barLargeButtonItemSave.Enabled = false;
            this.barLargeButtonItemSave.Id = 12;
            this.barLargeButtonItemSave.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3);
            this.barLargeButtonItemSave.LargeImageIndex = 26;
            this.barLargeButtonItemSave.Name = "barLargeButtonItemSave";
            this.barLargeButtonItemSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemSave_ItemClick);
            // 
            // barLargeButtonItemDelete
            // 
            this.barLargeButtonItemDelete.Caption = "删除(F4)";
            this.barLargeButtonItemDelete.Enabled = false;
            this.barLargeButtonItemDelete.Id = 20;
            this.barLargeButtonItemDelete.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4);
            this.barLargeButtonItemDelete.LargeImageIndex = 30;
            this.barLargeButtonItemDelete.Name = "barLargeButtonItemDelete";
            this.barLargeButtonItemDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemDelete_ItemClick);
            // 
            // barLargeButtonItemSubmit
            // 
            this.barLargeButtonItemSubmit.Caption = "提交(F5)";
            this.barLargeButtonItemSubmit.Enabled = false;
            this.barLargeButtonItemSubmit.Id = 13;
            this.barLargeButtonItemSubmit.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.barLargeButtonItemSubmit.LargeImageIndex = 31;
            this.barLargeButtonItemSubmit.Name = "barLargeButtonItemSubmit";
            this.barLargeButtonItemSubmit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemSubmit_ItemClick);
            // 
            // barLargeButtonItemReport
            // 
            this.barLargeButtonItemReport.Caption = "报表(F6)";
            this.barLargeButtonItemReport.Id = 18;
            this.barLargeButtonItemReport.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6);
            this.barLargeButtonItemReport.LargeImageIndex = 27;
            this.barLargeButtonItemReport.Name = "barLargeButtonItemReport";
            this.barLargeButtonItemReport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemReport_ItemClick);
            // 
            // barLargeButtonItemExit
            // 
            this.barLargeButtonItemExit.Caption = "退出(F7)";
            this.barLargeButtonItemExit.Id = 19;
            this.barLargeButtonItemExit.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F7);
            this.barLargeButtonItemExit.LargeImageIndex = 33;
            this.barLargeButtonItemExit.Name = "barLargeButtonItemExit";
            this.barLargeButtonItemExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemExit_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.Appearance.BorderColor = System.Drawing.Color.White;
            this.barDockControlTop.Appearance.Options.UseBorderColor = true;
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1197, 79);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 729);
            this.barDockControlBottom.Size = new System.Drawing.Size(1197, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 79);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 650);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1197, 79);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 650);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.ID = new System.Guid("07d3d199-bdd8-4620-ab4e-013c64240988");
            this.dockPanel1.Location = new System.Drawing.Point(0, 79);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Options.AllowFloating = false;
            this.dockPanel1.Options.FloatOnDblClick = false;
            this.dockPanel1.Options.ShowCloseButton = false;
            this.dockPanel1.Options.ShowMaximizeButton = false;
            this.dockPanel1.OriginalSize = new System.Drawing.Size(326, 200);
            this.dockPanel1.Size = new System.Drawing.Size(326, 650);
            this.dockPanel1.Text = "工具栏";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.gridControlCardList);
            this.dockPanel1_Container.Controls.Add(this.groupBox2);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(318, 623);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // gridControlCardList
            // 
            this.gridControlCardList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlCardList.Location = new System.Drawing.Point(0, 168);
            this.gridControlCardList.MainView = this.gridViewCardList;
            this.gridControlCardList.MenuManager = this.barManager1;
            this.gridControlCardList.Name = "gridControlCardList";
            this.gridControlCardList.Size = new System.Drawing.Size(318, 455);
            this.gridControlCardList.TabIndex = 1;
            this.gridControlCardList.TabStop = false;
            this.gridControlCardList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewCardList});
            this.gridControlCardList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridControlCardList_MouseDown);
            // 
            // gridViewCardList
            // 
            this.gridViewCardList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gridViewCardList.GridControl = this.gridControlCardList;
            this.gridViewCardList.IndicatorWidth = 40;
            this.gridViewCardList.Name = "gridViewCardList";
            this.gridViewCardList.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewCardList.OptionsBehavior.Editable = false;
            this.gridViewCardList.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewCardList.OptionsCustomization.AllowFilter = false;
            this.gridViewCardList.OptionsMenu.EnableColumnMenu = false;
            this.gridViewCardList.OptionsMenu.EnableFooterMenu = false;
            this.gridViewCardList.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewCardList.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewCardList.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewCardList.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewCardList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewCardList.OptionsView.ShowGroupPanel = false;
            this.gridViewCardList.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridViewCardList_CustomDrawRowIndicator);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "卡号";
            this.gridColumn1.FieldName = "REPORT_NO";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 90;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "病人姓名";
            this.gridColumn2.FieldName = "NAME";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 70;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "类别";
            this.gridColumn3.FieldName = "REPORTTYPENAME";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 101;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "ID";
            this.gridColumn4.FieldName = "REPORT_ID";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkEditOwner);
            this.groupBox2.Controls.Add(this.lookUpEditDept);
            this.groupBox2.Controls.Add(this.labelControl6);
            this.groupBox2.Controls.Add(this.textEditPatID);
            this.groupBox2.Controls.Add(this.labelControl5);
            this.groupBox2.Controls.Add(this.textEditName);
            this.groupBox2.Controls.Add(this.labelControl1);
            this.groupBox2.Controls.Add(this.checkButtonReject);
            this.groupBox2.Controls.Add(this.checkButtonAudit);
            this.groupBox2.Controls.Add(this.checkButtonSubmit);
            this.groupBox2.Controls.Add(this.checkButtonSave);
            this.groupBox2.Controls.Add(this.checkEditModify);
            this.groupBox2.Controls.Add(this.checkEditFirst);
            this.groupBox2.Controls.Add(this.labelControl4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(318, 168);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "查询条件";
            // 
            // checkEditOwner
            // 
            this.checkEditOwner.Location = new System.Drawing.Point(258, 86);
            this.checkEditOwner.MenuManager = this.barManager1;
            this.checkEditOwner.Name = "checkEditOwner";
            this.checkEditOwner.Properties.Caption = "个人";
            this.checkEditOwner.Size = new System.Drawing.Size(56, 19);
            this.checkEditOwner.TabIndex = 3;
            this.checkEditOwner.TabStop = false;
            this.checkEditOwner.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chb_KeyPress);
            // 
            // lookUpEditDept
            // 
            this.lookUpEditDept.EnterMoveNextControl = true;
            this.lookUpEditDept.Location = new System.Drawing.Point(49, 86);
            this.lookUpEditDept.MenuManager = this.barManager1;
            this.lookUpEditDept.Name = "lookUpEditDept";
            this.lookUpEditDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditDept.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "部门编码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "部门名称")});
            this.lookUpEditDept.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.lookUpEditDept.Size = new System.Drawing.Size(187, 20);
            this.lookUpEditDept.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(7, 90);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 14);
            this.labelControl6.TabIndex = 13;
            this.labelControl6.Text = "科室：";
            // 
            // textEditPatID
            // 
            this.textEditPatID.EnterMoveNextControl = true;
            this.textEditPatID.Location = new System.Drawing.Point(216, 54);
            this.textEditPatID.MenuManager = this.barManager1;
            this.textEditPatID.Name = "textEditPatID";
            this.textEditPatID.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditPatID.Size = new System.Drawing.Size(97, 20);
            this.textEditPatID.TabIndex = 1;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(160, 57);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 12;
            this.labelControl5.Text = "病历号：";
            // 
            // textEditName
            // 
            this.textEditName.EnterMoveNextControl = true;
            this.textEditName.Location = new System.Drawing.Point(49, 54);
            this.textEditName.MenuManager = this.barManager1;
            this.textEditName.Name = "textEditName";
            this.textEditName.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditName.Size = new System.Drawing.Size(97, 20);
            this.textEditName.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(7, 57);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "病人：";
            // 
            // checkButtonReject
            // 
            this.checkButtonReject.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.checkButtonReject.GroupIndex = 0;
            this.checkButtonReject.Location = new System.Drawing.Point(240, 125);
            this.checkButtonReject.Name = "checkButtonReject";
            this.checkButtonReject.Size = new System.Drawing.Size(70, 27);
            this.checkButtonReject.TabIndex = 7;
            this.checkButtonReject.TabStop = false;
            this.checkButtonReject.Text = "已否决";
            this.checkButtonReject.CheckedChanged += new System.EventHandler(this.checkButtonReject_CheckedChanged);
            // 
            // checkButtonAudit
            // 
            this.checkButtonAudit.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.checkButtonAudit.GroupIndex = 0;
            this.checkButtonAudit.Location = new System.Drawing.Point(163, 125);
            this.checkButtonAudit.Name = "checkButtonAudit";
            this.checkButtonAudit.Size = new System.Drawing.Size(70, 27);
            this.checkButtonAudit.TabIndex = 6;
            this.checkButtonAudit.TabStop = false;
            this.checkButtonAudit.Text = "已审核";
            this.checkButtonAudit.CheckedChanged += new System.EventHandler(this.checkButtonAudit_CheckedChanged);
            // 
            // checkButtonSubmit
            // 
            this.checkButtonSubmit.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.checkButtonSubmit.GroupIndex = 0;
            this.checkButtonSubmit.Location = new System.Drawing.Point(86, 125);
            this.checkButtonSubmit.Name = "checkButtonSubmit";
            this.checkButtonSubmit.Size = new System.Drawing.Size(70, 27);
            this.checkButtonSubmit.TabIndex = 5;
            this.checkButtonSubmit.TabStop = false;
            this.checkButtonSubmit.Text = "已提交";
            this.checkButtonSubmit.CheckedChanged += new System.EventHandler(this.checkButtonSubmit_CheckedChanged);
            // 
            // checkButtonSave
            // 
            this.checkButtonSave.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.checkButtonSave.Checked = true;
            this.checkButtonSave.GroupIndex = 0;
            this.checkButtonSave.Location = new System.Drawing.Point(9, 125);
            this.checkButtonSave.Name = "checkButtonSave";
            this.checkButtonSave.Size = new System.Drawing.Size(70, 27);
            this.checkButtonSave.TabIndex = 4;
            this.checkButtonSave.Text = "已保存";
            this.checkButtonSave.CheckedChanged += new System.EventHandler(this.checkButtonSave_CheckedChanged);
            // 
            // checkEditModify
            // 
            this.checkEditModify.EditValue = true;
            this.checkEditModify.Location = new System.Drawing.Point(138, 22);
            this.checkEditModify.MenuManager = this.barManager1;
            this.checkEditModify.Name = "checkEditModify";
            this.checkEditModify.Properties.Caption = "订正报告";
            this.checkEditModify.Size = new System.Drawing.Size(87, 19);
            this.checkEditModify.TabIndex = 9;
            this.checkEditModify.TabStop = false;
            this.checkEditModify.CheckedChanged += new System.EventHandler(this.checkEditModify_CheckedChanged);
            this.checkEditModify.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chb_KeyPress);
            // 
            // checkEditFirst
            // 
            this.checkEditFirst.EditValue = true;
            this.checkEditFirst.Location = new System.Drawing.Point(49, 22);
            this.checkEditFirst.MenuManager = this.barManager1;
            this.checkEditFirst.Name = "checkEditFirst";
            this.checkEditFirst.Properties.Caption = "初次报告";
            this.checkEditFirst.Size = new System.Drawing.Size(87, 19);
            this.checkEditFirst.TabIndex = 8;
            this.checkEditFirst.TabStop = false;
            this.checkEditFirst.CheckedChanged += new System.EventHandler(this.checkEditFirst_CheckedChanged);
            this.checkEditFirst.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chb_KeyPress);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(7, 24);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 10;
            this.labelControl4.Text = "类别：";
            // 
            // barLargeButtonItemWithDraw
            // 
            this.barLargeButtonItemWithDraw.Caption = "撤销(F6)";
            this.barLargeButtonItemWithDraw.Id = 14;
            this.barLargeButtonItemWithDraw.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6);
            this.barLargeButtonItemWithDraw.LargeImageIndex = 18;
            this.barLargeButtonItemWithDraw.Name = "barLargeButtonItemWithDraw";
            this.barLargeButtonItemWithDraw.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barLargeButtonItemPass
            // 
            this.barLargeButtonItemPass.Caption = "通过(F5)";
            this.barLargeButtonItemPass.Id = 16;
            this.barLargeButtonItemPass.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.barLargeButtonItemPass.LargeImageIndex = 14;
            this.barLargeButtonItemPass.Name = "barLargeButtonItemPass";
            // 
            // barLargeButtonItemReject
            // 
            this.barLargeButtonItemReject.Caption = "否决";
            this.barLargeButtonItemReject.Id = 17;
            this.barLargeButtonItemReject.LargeImageIndex = 23;
            this.barLargeButtonItemReject.Name = "barLargeButtonItemReject";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "11.png");
            this.imageList1.Images.SetKeyName(1, "12.png");
            this.imageList1.Images.SetKeyName(2, "15.png");
            this.imageList1.Images.SetKeyName(3, "10.png");
            this.imageList1.Images.SetKeyName(4, "7.png");
            this.imageList1.Images.SetKeyName(5, "14.png");
            this.imageList1.Images.SetKeyName(6, "1.png");
            this.imageList1.Images.SetKeyName(7, "13.png");
            this.imageList1.Images.SetKeyName(8, "save.bmp");
            this.imageList1.Images.SetKeyName(9, "12.png");
            this.imageList1.Images.SetKeyName(10, "8.png");
            this.imageList1.Images.SetKeyName(11, "2.png");
            this.imageList1.Images.SetKeyName(12, "ftp.png");
            this.imageList1.Images.SetKeyName(13, "25.png");
            this.imageList1.Images.SetKeyName(14, "20.png");
            this.imageList1.Images.SetKeyName(15, "error.png");
            this.imageList1.Images.SetKeyName(16, "2.png");
            this.imageList1.Images.SetKeyName(17, "3.png");
            this.imageList1.Images.SetKeyName(18, "withdraw.png");
            this.imageList1.Images.SetKeyName(19, "save.png");
            this.imageList1.Images.SetKeyName(20, "new.png");
            this.imageList1.Images.SetKeyName(21, "exit.png");
            this.imageList1.Images.SetKeyName(22, "report.png");
            this.imageList1.Images.SetKeyName(23, "reject.png");
            this.imageList1.Images.SetKeyName(24, "find.png");
            this.imageList1.Images.SetKeyName(25, "find.png");
            this.imageList1.Images.SetKeyName(26, "保存.png");
            this.imageList1.Images.SetKeyName(27, "报表.png");
            this.imageList1.Images.SetKeyName(28, "补录.png");
            this.imageList1.Images.SetKeyName(29, "查询.png");
            this.imageList1.Images.SetKeyName(30, "删除.png");
            this.imageList1.Images.SetKeyName(31, "提交.png");
            this.imageList1.Images.SetKeyName(32, "退出.png");
            this.imageList1.Images.SetKeyName(33, "退出2.png");
            // 
            // xtraTabControlCardInfo
            // 
            this.xtraTabControlCardInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControlCardInfo.Location = new System.Drawing.Point(326, 79);
            this.xtraTabControlCardInfo.Name = "xtraTabControlCardInfo";
            this.xtraTabControlCardInfo.SelectedTabPage = this.xtraTabPageCardInfo;
            this.xtraTabControlCardInfo.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControlCardInfo.Size = new System.Drawing.Size(871, 650);
            this.xtraTabControlCardInfo.TabIndex = 1;
            this.xtraTabControlCardInfo.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageCardInfo});
            // 
            // xtraTabPageCardInfo
            // 
            this.xtraTabPageCardInfo.Appearance.PageClient.BackColor = System.Drawing.Color.LightBlue;
            this.xtraTabPageCardInfo.Appearance.PageClient.Options.UseBackColor = true;
            this.xtraTabPageCardInfo.AutoScroll = true;
            this.xtraTabPageCardInfo.Controls.Add(this.panelControlReprotCard);
            this.xtraTabPageCardInfo.Name = "xtraTabPageCardInfo";
            this.xtraTabPageCardInfo.Size = new System.Drawing.Size(865, 644);
            this.xtraTabPageCardInfo.Text = "报告卡信息";
            // 
            // panelControlReprotCard
            // 
            this.panelControlReprotCard.AutoScroll = true;
            this.panelControlReprotCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlReprotCard.Location = new System.Drawing.Point(0, 0);
            this.panelControlReprotCard.Name = "panelControlReprotCard";
            this.panelControlReprotCard.Size = new System.Drawing.Size(865, 644);
            this.panelControlReprotCard.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1197, 729);
            this.Controls.Add(this.xtraTabControlCardInfo);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "传染病上报";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlCardList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCardList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditOwner.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditDept.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPatID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditModify.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditFirst.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlCardInfo)).EndInit();
            this.xtraTabControlCardInfo.ResumeLayout(false);
            this.xtraTabPageCardInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraEditors.CheckEdit checkEditModify;
        private DevExpress.XtraEditors.CheckEdit checkEditFirst;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraTab.XtraTabControl xtraTabControlCardInfo;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageCardInfo;
        private DevExpress.XtraGrid.GridControl gridControlCardList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewCardList;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.CheckButton checkButtonReject;
        private DevExpress.XtraEditors.CheckButton checkButtonAudit;
        private DevExpress.XtraEditors.CheckButton checkButtonSubmit;
        private DevExpress.XtraEditors.CheckButton checkButtonSave;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit checkEditOwner;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditDept;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit textEditPatID;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit textEditName;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemNew;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemSave;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemSubmit;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemWithDraw;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemSearch;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemPass;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemReject;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemReport;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemExit;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemDelete;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Panel panelControlReprotCard;

    }
}

