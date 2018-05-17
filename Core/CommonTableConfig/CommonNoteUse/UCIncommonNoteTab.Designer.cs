namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    partial class UCIncommonNoteTab
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCIncommonNoteTab));
            this.btnDel = new DrectSoft.Common.Ctrs.OTHER.DevButtonDelete(this.components);
            this.btnAdd = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd(this.components);
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanelInfo = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.scorlInfo = new DevExpress.XtraEditors.XtraScrollableControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btnSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave(this.components);
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTopAdd = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd(this.components);
            this.btnAddNew = new DevExpress.XtraEditors.SimpleButton();
            this.btnRemove = new DevExpress.XtraEditors.SimpleButton();
            this.btnTopSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave(this.components);
            this.btnAddColName = new DevExpress.XtraEditors.SimpleButton();
            this.btnEdit = new DrectSoft.Common.Ctrs.OTHER.DevButtonEdit(this.components);
            this.btnTongJi = new DevExpress.XtraEditors.SimpleButton();
            this.btnHis = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelHis = new DevExpress.XtraEditors.SimpleButton();
            this.dtEnd = new DevExpress.XtraEditors.DateEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItemEdit = new DevExpress.XtraBars.BarButtonItem();
            this.dtStart = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.radAll = new System.Windows.Forms.RadioButton();
            this.radFromDate = new System.Windows.Forms.RadioButton();
            this.radToday = new System.Windows.Forms.RadioButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridViewTab = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanelInfo.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStart.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTab)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDel
            // 
            this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDel.Image = ((System.Drawing.Image)(resources.GetObject("btnDel.Image")));
            this.btnDel.Location = new System.Drawing.Point(579, 4);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(80, 23);
            this.btnDel.TabIndex = 101;
            this.btnDel.Text = "删除(&D)";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(490, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 100;
            this.btnAdd.Text = "新增(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.HiddenPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanelInfo});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // dockPanelInfo
            // 
            this.dockPanelInfo.AutoScroll = true;
            this.dockPanelInfo.Controls.Add(this.dockPanel1_Container);
            this.dockPanelInfo.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.dockPanelInfo.ID = new System.Guid("7d0a7a55-ac96-4f67-b46e-c5e36201fdea");
            this.dockPanelInfo.Location = new System.Drawing.Point(0, 360);
            this.dockPanelInfo.Name = "dockPanelInfo";
            this.dockPanelInfo.Options.AllowDockFill = false;
            this.dockPanelInfo.Options.AllowDockLeft = false;
            this.dockPanelInfo.Options.AllowDockRight = false;
            this.dockPanelInfo.Options.AllowDockTop = false;
            this.dockPanelInfo.Options.AllowFloating = false;
            this.dockPanelInfo.Options.FloatOnDblClick = false;
            this.dockPanelInfo.Options.ShowCloseButton = false;
            this.dockPanelInfo.Options.ShowMaximizeButton = false;
            this.dockPanelInfo.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanelInfo.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.dockPanelInfo.SavedIndex = 0;
            this.dockPanelInfo.Size = new System.Drawing.Size(811, 200);
            this.dockPanelInfo.Text = "编辑内容";
            this.dockPanelInfo.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.scorlInfo);
            this.dockPanel1_Container.Controls.Add(this.panelControl3);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(803, 173);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // scorlInfo
            // 
            this.scorlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scorlInfo.Location = new System.Drawing.Point(0, 0);
            this.scorlInfo.Name = "scorlInfo";
            this.scorlInfo.Size = new System.Drawing.Size(803, 137);
            this.scorlInfo.TabIndex = 2;
            // 
            // panelControl3
            // 
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.btnDel);
            this.panelControl3.Controls.Add(this.btnSave);
            this.panelControl3.Controls.Add(this.btnAdd);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 137);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(803, 36);
            this.panelControl3.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(668, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 102;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // splitterControl1
            // 
            this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterControl1.Location = new System.Drawing.Point(0, 555);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(811, 5);
            this.splitterControl1.TabIndex = 3;
            this.splitterControl1.TabStop = false;
            this.splitterControl1.Visible = false;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.flowLayoutPanel1);
            this.panelControl1.Controls.Add(this.dtEnd);
            this.panelControl1.Controls.Add(this.dtStart);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.radAll);
            this.panelControl1.Controls.Add(this.radFromDate);
            this.panelControl1.Controls.Add(this.radToday);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(811, 85);
            this.panelControl1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnTopAdd);
            this.flowLayoutPanel1.Controls.Add(this.btnAddNew);
            this.flowLayoutPanel1.Controls.Add(this.btnRemove);
            this.flowLayoutPanel1.Controls.Add(this.btnTopSave);
            this.flowLayoutPanel1.Controls.Add(this.btnAddColName);
            this.flowLayoutPanel1.Controls.Add(this.btnEdit);
            this.flowLayoutPanel1.Controls.Add(this.btnTongJi);
            this.flowLayoutPanel1.Controls.Add(this.btnHis);
            this.flowLayoutPanel1.Controls.Add(this.btnDelHis);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 48);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(811, 37);
            this.flowLayoutPanel1.TabIndex = 14;
            // 
            // btnTopAdd
            // 
            this.btnTopAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnTopAdd.Image")));
            this.btnTopAdd.Location = new System.Drawing.Point(3, 3);
            this.btnTopAdd.Name = "btnTopAdd";
            this.btnTopAdd.Size = new System.Drawing.Size(80, 23);
            this.btnTopAdd.TabIndex = 1;
            this.btnTopAdd.Text = "新增(&A)";
            this.btnTopAdd.ToolTip = "新增记录，默认记录时间为当前时间";
            this.btnTopAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNew.Image")));
            this.btnAddNew.Location = new System.Drawing.Point(89, 3);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(75, 23);
            this.btnAddNew.TabIndex = 2;
            this.btnAddNew.Text = "追加记录";
            this.btnAddNew.ToolTip = "新增记录默认时间为上一条的时间";
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.Location = new System.Drawing.Point(170, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "删除(&D)";
            this.btnRemove.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnTopSave
            // 
            this.btnTopSave.Image = ((System.Drawing.Image)(resources.GetObject("btnTopSave.Image")));
            this.btnTopSave.Location = new System.Drawing.Point(251, 3);
            this.btnTopSave.Name = "btnTopSave";
            this.btnTopSave.Size = new System.Drawing.Size(80, 23);
            this.btnTopSave.TabIndex = 4;
            this.btnTopSave.Text = "保存(&S)";
            this.btnTopSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddColName
            // 
            this.btnAddColName.Image = ((System.Drawing.Image)(resources.GetObject("btnAddColName.Image")));
            this.btnAddColName.Location = new System.Drawing.Point(337, 3);
            this.btnAddColName.Name = "btnAddColName";
            this.btnAddColName.Size = new System.Drawing.Size(80, 23);
            this.btnAddColName.TabIndex = 5;
            this.btnAddColName.Text = "指定列名";
            this.btnAddColName.Visible = false;
            this.btnAddColName.Click += new System.EventHandler(this.btnAddColName_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.Location = new System.Drawing.Point(423, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 23);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "编辑(&E)";
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnTongJi
            // 
            this.btnTongJi.Image = ((System.Drawing.Image)(resources.GetObject("btnTongJi.Image")));
            this.btnTongJi.Location = new System.Drawing.Point(509, 3);
            this.btnTongJi.Name = "btnTongJi";
            this.btnTongJi.Size = new System.Drawing.Size(80, 23);
            this.btnTongJi.TabIndex = 13;
            this.btnTongJi.Text = "统计";
            this.btnTongJi.Click += new System.EventHandler(this.btnTongJi_Click);
            // 
            // btnHis
            // 
            this.btnHis.Image = ((System.Drawing.Image)(resources.GetObject("btnHis.Image")));
            this.btnHis.Location = new System.Drawing.Point(595, 3);
            this.btnHis.Name = "btnHis";
            this.btnHis.Size = new System.Drawing.Size(104, 23);
            this.btnHis.TabIndex = 14;
            this.btnHis.Text = "修改历史查询";
            this.btnHis.ToolTip = "加*记录为存在修改历史";
            this.btnHis.Click += new System.EventHandler(this.btnHis_Click);
            // 
            // btnDelHis
            // 
            this.btnDelHis.Image = ((System.Drawing.Image)(resources.GetObject("btnDelHis.Image")));
            this.btnDelHis.Location = new System.Drawing.Point(705, 3);
            this.btnDelHis.Name = "btnDelHis";
            this.btnDelHis.Size = new System.Drawing.Size(103, 23);
            this.btnDelHis.TabIndex = 15;
            this.btnDelHis.Text = "删除历史查询";
            this.btnDelHis.Click += new System.EventHandler(this.btnDelHis_Click);
            // 
            // dtEnd
            // 
            this.dtEnd.EditValue = null;
            this.dtEnd.Location = new System.Drawing.Point(263, 15);
            this.dtEnd.MenuManager = this.barManager1;
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEnd.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtEnd.Size = new System.Drawing.Size(100, 21);
            this.dtEnd.TabIndex = 12;
            this.dtEnd.EditValueChanged += new System.EventHandler(this.dtStart_EditValueChanged);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemEdit});
            this.barManager1.MaxItemId = 1;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(811, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 560);
            this.barDockControlBottom.Size = new System.Drawing.Size(811, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 560);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(811, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 560);
            // 
            // barButtonItemEdit
            // 
            this.barButtonItemEdit.Caption = "编辑";
            this.barButtonItemEdit.Id = 0;
            this.barButtonItemEdit.Name = "barButtonItemEdit";
            // 
            // dtStart
            // 
            this.dtStart.EditValue = null;
            this.dtStart.Location = new System.Drawing.Point(133, 15);
            this.dtStart.MenuManager = this.barManager1;
            this.dtStart.Name = "dtStart";
            this.dtStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtStart.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtStart.Size = new System.Drawing.Size(100, 21);
            this.dtStart.TabIndex = 11;
            this.dtStart.EditValueChanged += new System.EventHandler(this.dtStart_EditValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(239, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 14);
            this.label1.TabIndex = 10;
            this.label1.Text = "至";
            // 
            // radAll
            // 
            this.radAll.AutoSize = true;
            this.radAll.Location = new System.Drawing.Point(395, 18);
            this.radAll.Name = "radAll";
            this.radAll.Size = new System.Drawing.Size(49, 18);
            this.radAll.TabIndex = 9;
            this.radAll.Text = "全部";
            this.radAll.UseVisualStyleBackColor = true;
            this.radAll.CheckedChanged += new System.EventHandler(this.radToday_CheckedChanged);
            // 
            // radFromDate
            // 
            this.radFromDate.AutoSize = true;
            this.radFromDate.Location = new System.Drawing.Point(97, 18);
            this.radFromDate.Name = "radFromDate";
            this.radFromDate.Size = new System.Drawing.Size(37, 18);
            this.radFromDate.TabIndex = 8;
            this.radFromDate.Text = "从";
            this.radFromDate.UseVisualStyleBackColor = true;
            this.radFromDate.CheckedChanged += new System.EventHandler(this.radToday_CheckedChanged);
            // 
            // radToday
            // 
            this.radToday.AutoSize = true;
            this.radToday.Checked = true;
            this.radToday.Location = new System.Drawing.Point(25, 18);
            this.radToday.Name = "radToday";
            this.radToday.Size = new System.Drawing.Size(49, 18);
            this.radToday.TabIndex = 7;
            this.radToday.TabStop = true;
            this.radToday.Text = "当天";
            this.radToday.UseVisualStyleBackColor = true;
            this.radToday.CheckedChanged += new System.EventHandler(this.radToday_CheckedChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemEdit)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.gridControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 85);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(811, 470);
            this.panelControl2.TabIndex = 13;
            // 
            // gridControl1
            // 
            this.gridControl1.AllowDrop = true;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridViewTab;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(811, 470);
            this.gridControl1.TabIndex = 9;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTab});
            this.gridControl1.DragDrop += new System.Windows.Forms.DragEventHandler(this.gridControl1_DragDrop);
            this.gridControl1.DragEnter += new System.Windows.Forms.DragEventHandler(this.gridControl1_DragEnter);
            // 
            // gridViewTab
            // 
            this.gridViewTab.GridControl = this.gridControl1;
            this.gridViewTab.IndicatorWidth = 40;
            this.gridViewTab.Name = "gridViewTab";
            this.gridViewTab.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewTab.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewTab.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewTab.OptionsCustomization.AllowFilter = false;
            this.gridViewTab.OptionsCustomization.AllowGroup = false;
            this.gridViewTab.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridViewTab.OptionsDetail.AllowZoomDetail = false;
            this.gridViewTab.OptionsDetail.SmartDetailExpand = false;
            this.gridViewTab.OptionsFind.AllowFindPanel = false;
            this.gridViewTab.OptionsMenu.EnableColumnMenu = false;
            this.gridViewTab.OptionsMenu.EnableFooterMenu = false;
            this.gridViewTab.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewTab.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewTab.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewTab.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewTab.OptionsView.ColumnAutoWidth = false;
            this.gridViewTab.OptionsView.RowAutoHeight = true;
            this.gridViewTab.OptionsView.ShowGroupPanel = false;
            this.gridViewTab.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridViewTab_CustomDrawRowIndicator);
            this.gridViewTab.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewTab_FocusedRowChanged);
            this.gridViewTab.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewTab_CellValueChanged);
            this.gridViewTab.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridViewTab_MouseUp);
            // 
            // UCIncommonNoteTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UCIncommonNoteTab";
            this.Size = new System.Drawing.Size(811, 560);
            this.Load += new System.EventHandler(this.UCIncommonNoteTab_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanelInfo.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStart.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTab)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelInfo;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraEditors.XtraScrollableControl scorlInfo;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonDelete btnDel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd btnAdd;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnSave;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnTopSave;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd btnTopAdd;
        private DevExpress.XtraEditors.SimpleButton btnRemove;
        private DevExpress.XtraEditors.SimpleButton btnAddColName;
        private DevExpress.XtraEditors.SimpleButton btnAddNew;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemEdit;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTab;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonEdit btnEdit;
        private DevExpress.XtraEditors.DateEdit dtEnd;
        private DevExpress.XtraEditors.DateEdit dtStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radAll;
        private System.Windows.Forms.RadioButton radFromDate;
        private System.Windows.Forms.RadioButton radToday;
        private DevExpress.XtraEditors.SimpleButton btnTongJi;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btnHis;
        private DevExpress.XtraEditors.SimpleButton btnDelHis;
    }
}
