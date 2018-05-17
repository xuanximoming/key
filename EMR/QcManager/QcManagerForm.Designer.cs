namespace DrectSoft.Emr.QcManager
{
    partial class QcManagerForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("病历评分项目统计", 5, 5);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("出院未提交病历统计", 5, 5);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("出院病历统计", 5, 5);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("死亡信息统计", 5, 5);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("手术信息统计", 5, 5);
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("抢救信息统计", 5, 5);
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("全院病历质控率", 5, 5);
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("质控失分项目统计", 5, 5);
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("质控评分记录统计", 5, 5);
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("质控统计部分", 5, 5, new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("质控人员配置");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("质控评分项配置");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("病案首页评分项配置");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("质控配置部分", new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode12,
            treeNode13});
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("医师书写病历统计", 4, 4);
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("诊断和手术病案统计", 4, 4);
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("会诊明细统计", 4, 4);
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("医师医疗质量控制", 4, 4);
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("科室医疗质量控制", 4, 4);
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("单病种医疗质量统计", 4, 4);
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("病人门急诊诊断明细统计", 4, 4);
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("汇总统计部分", 4, 4, new System.Windows.Forms.TreeNode[] {
            treeNode15,
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20,
            treeNode21});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QcManagerForm));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.btn_hospital_rate = new DevExpress.XtraBars.BarButtonItem();
            this.btn_dept_rate = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
            this.btn_monitor_item = new DevExpress.XtraBars.BarButtonItem();
            this.btn_Disease_Level = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem4 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnPointSum = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnConfigQCAudit = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            this.btn_qc_dept = new DevExpress.XtraBars.BarButtonItem();
            this.btn_qc_SingleDisease = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemConsult = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btn_qc_doctor = new DevExpress.XtraBars.BarButtonItem();
            this.xtraTabQcManager = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageQualityMedicalRecord = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageOutHospital = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageOutHospitalLock = new DevExpress.XtraTab.XtraTabPage();
            this.xTabPageEmrScore = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabDieInfo = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabQCOperatInfo = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageLostCat = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageScore = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabQCRescueInfo = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabHosRate = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabDepRate = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabDeptQuery = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabDocQuery = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabSingleDisease = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabReuPoint = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabRecorByDoctor = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabDiagOperRecord = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageConsult = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageDia = new DevExpress.XtraTab.XtraTabPage();
            this.xtabUserLoginInfo = new DevExpress.XtraTab.XtraTabPage();
            this.xtabActiveUser = new DevExpress.XtraTab.XtraTabPage();
            this.xtabDiagGroupInfo = new DevExpress.XtraTab.XtraTabPage();
            this.xtabDataBaseTable = new DevExpress.XtraTab.XtraTabPage();
            this.barConfigQCPoint = new DevExpress.XtraBars.BarButtonItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabQcManager)).BeginInit();
            this.xtraTabQcManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barSubItem2,
            this.barSubItem3,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barBtnConfigQCAudit,
            this.barButtonItem3,
            this.barBtnPointSum,
            this.barButtonItem6,
            this.barButtonItem7,
            this.barCheckItem1,
            this.btn_qc_doctor,
            this.barSubItem4,
            this.barButtonItem9,
            this.barButtonItem10,
            this.btn_qc_dept,
            this.btn_hospital_rate,
            this.btn_dept_rate,
            this.btn_qc_SingleDisease,
            this.barButtonItem5,
            this.barButtonItem8,
            this.barButtonItem11,
            this.btn_monitor_item,
            this.btn_Disease_Level,
            this.barButtonItemConsult});
            this.barManager1.MaxItemId = 35;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.FloatLocation = new System.Drawing.Point(286, 265);
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem4)});
            this.bar1.Text = "Tools";
            this.bar1.Visible = false;
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "质量统计";
            this.barSubItem1.Id = 2;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_hospital_rate),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_dept_rate)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // btn_hospital_rate
            // 
            this.btn_hospital_rate.Caption = "全院病历质控率";
            this.btn_hospital_rate.Id = 21;
            this.btn_hospital_rate.Name = "btn_hospital_rate";
            this.btn_hospital_rate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_hospital_info_ItemClick);
            // 
            // btn_dept_rate
            // 
            this.btn_dept_rate.Caption = "科室病历质控率";
            this.btn_dept_rate.Id = 22;
            this.btn_dept_rate.Name = "btn_dept_rate";
            this.btn_dept_rate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_dept_rate_ItemClick);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "汇总统计";
            this.barSubItem2.Id = 5;
            this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem8, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem11)});
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "医师医疗质量";
            this.barButtonItem5.Id = 24;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_doctor_ItemClick);
            // 
            // barButtonItem8
            // 
            this.barButtonItem8.Caption = "科室医疗质量统计";
            this.barButtonItem8.Id = 25;
            this.barButtonItem8.Name = "barButtonItem8";
            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_dept_ItemClick);
            // 
            // barButtonItem11
            // 
            this.barButtonItem11.Caption = "单病种医疗质量统计";
            this.barButtonItem11.Id = 26;
            this.barButtonItem11.Name = "barButtonItem11";
            this.barButtonItem11.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_SingleDisease_ItemClick);
            // 
            // barSubItem3
            // 
            this.barSubItem3.Caption = "质控标准设置";
            this.barSubItem3.Id = 6;
            this.barSubItem3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_monitor_item),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_Disease_Level)});
            this.barSubItem3.Name = "barSubItem3";
            // 
            // btn_monitor_item
            // 
            this.btn_monitor_item.Caption = "自动监控设置";
            this.btn_monitor_item.Id = 27;
            this.btn_monitor_item.Name = "btn_monitor_item";
            this.btn_monitor_item.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_monitor_item_ItemClick);
            // 
            // btn_Disease_Level
            // 
            this.btn_Disease_Level.Caption = "单病种质量评分标准";
            this.btn_Disease_Level.Id = 28;
            this.btn_Disease_Level.Name = "btn_Disease_Level";
            this.btn_Disease_Level.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_Disease_Level_ItemClick);
            // 
            // barSubItem4
            // 
            this.barSubItem4.Caption = "标准评分项";
            this.barSubItem4.Id = 17;
            this.barSubItem4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem9),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem10)});
            this.barSubItem4.Name = "barSubItem4";
            // 
            // barButtonItem9
            // 
            this.barButtonItem9.Caption = "标准评分大项";
            this.barButtonItem9.Id = 18;
            this.barButtonItem9.Name = "barButtonItem9";
            this.barButtonItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem9_ItemClick);
            // 
            // barButtonItem10
            // 
            this.barButtonItem10.Caption = "标准评分细项";
            this.barButtonItem10.Id = 19;
            this.barButtonItem10.Name = "barButtonItem10";
            this.barButtonItem10.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem10_ItemClick);
            // 
            // bar2
            // 
            this.bar2.BarName = "Custom 3";
            this.bar2.DockCol = 1;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.FloatLocation = new System.Drawing.Point(175, 140);
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnPointSum),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnConfigQCAudit),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem6),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem7),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_qc_dept),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_qc_SingleDisease),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemConsult)});
            this.bar2.Text = "Custom 3";
            this.bar2.Visible = false;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "在院患者";
            this.barButtonItem1.Id = 7;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "出院未提交患者";
            this.barButtonItem2.Id = 8;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "出院未归档病历";
            this.barButtonItem3.Id = 9;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barBtnPointSum
            // 
            this.barBtnPointSum.Caption = "病历评分统计";
            this.barBtnPointSum.Id = 30;
            this.barBtnPointSum.Name = "barBtnPointSum";
            this.barBtnPointSum.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnPointSum_ItemClick);
            // 
            // barBtnConfigQCAudit
            // 
            this.barBtnConfigQCAudit.Caption = "质控人员配置";
            this.barBtnConfigQCAudit.Id = 32;
            this.barBtnConfigQCAudit.Name = "barBtnConfigQCAudit";
            this.barBtnConfigQCAudit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnConfigQCAudit_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "手术统计";
            this.barButtonItem6.Id = 12;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "抢救统计";
            this.barButtonItem7.Id = 13;
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem7_ItemClick);
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.Caption = "死亡统计";
            this.barCheckItem1.Id = 14;
            this.barCheckItem1.Name = "barCheckItem1";
            this.barCheckItem1.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem1_CheckedChanged);
            // 
            // btn_qc_dept
            // 
            this.btn_qc_dept.Caption = "科室医疗质量统计";
            this.btn_qc_dept.Id = 20;
            this.btn_qc_dept.Name = "btn_qc_dept";
            this.btn_qc_dept.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btn_qc_dept.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_dept_ItemClick);
            // 
            // btn_qc_SingleDisease
            // 
            this.btn_qc_SingleDisease.Caption = "单病种医疗质量统计";
            this.btn_qc_SingleDisease.Id = 23;
            this.btn_qc_SingleDisease.Name = "btn_qc_SingleDisease";
            this.btn_qc_SingleDisease.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btn_qc_SingleDisease.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_SingleDisease_ItemClick);
            // 
            // barButtonItemConsult
            // 
            this.barButtonItemConsult.Caption = "会诊明细";
            this.barButtonItemConsult.Id = 34;
            this.barButtonItemConsult.Name = "barButtonItemConsult";
            this.barButtonItemConsult.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemConsult_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1255, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 693);
            this.barDockControlBottom.Size = new System.Drawing.Size(1255, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 662);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1255, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 662);
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
            this.dockPanel1.FloatVertical = true;
            this.dockPanel1.ID = new System.Guid("56d6fe78-b996-44fb-9c85-4d7296b62d63");
            this.dockPanel1.Location = new System.Drawing.Point(0, 31);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Options.ShowCloseButton = false;
            this.dockPanel1.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanel1.Size = new System.Drawing.Size(200, 662);
            this.dockPanel1.Text = "导航";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.treeView1);
            this.dockPanel1_Container.Dock = System.Windows.Forms.DockStyle.Left;
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(192, 635);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.White;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.ItemHeight = 17;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.ImageIndex = 5;
            treeNode1.Name = "NodOutMedicalScore";
            treeNode1.NodeFont = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            treeNode1.SelectedImageIndex = 5;
            treeNode1.Text = "病历评分项目统计";
            treeNode2.ImageIndex = 5;
            treeNode2.Name = "NodOutHosNoSubmit";
            treeNode2.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode2.SelectedImageIndex = 5;
            treeNode2.Text = "出院未提交病历统计";
            treeNode3.ImageIndex = 5;
            treeNode3.Name = "NoOutHosNoLock";
            treeNode3.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode3.SelectedImageIndex = 5;
            treeNode3.Text = "出院病历统计";
            treeNode4.ImageIndex = 5;
            treeNode4.Name = "NoQC_Die_Info";
            treeNode4.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode4.SelectedImageIndex = 5;
            treeNode4.Text = "死亡信息统计";
            treeNode5.ImageIndex = 5;
            treeNode5.Name = "NoQC_Operat_Info";
            treeNode5.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode5.SelectedImageIndex = 5;
            treeNode5.Text = "手术信息统计";
            treeNode6.ImageIndex = 5;
            treeNode6.Name = "NodQCRescueInfo";
            treeNode6.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode6.SelectedImageIndex = 5;
            treeNode6.Text = "抢救信息统计";
            treeNode7.ImageIndex = 5;
            treeNode7.Name = "NodDept_Rate";
            treeNode7.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode7.SelectedImageIndex = 5;
            treeNode7.Text = "全院病历质控率";
            treeNode8.ImageIndex = 5;
            treeNode8.Name = "NodeLostCat";
            treeNode8.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode8.SelectedImageIndex = 5;
            treeNode8.Text = "质控失分项目统计";
            treeNode9.ImageIndex = 5;
            treeNode9.Name = "Nodscore";
            treeNode9.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode9.SelectedImageIndex = 5;
            treeNode9.Text = "质控评分记录统计";
            treeNode10.ForeColor = System.Drawing.Color.Black;
            treeNode10.ImageIndex = 5;
            treeNode10.Name = "zhikongtongji";
            treeNode10.NodeFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            treeNode10.SelectedImageIndex = 5;
            treeNode10.Text = "质控统计部分";
            treeNode11.Name = "NodPerson";
            treeNode11.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode11.Text = "质控人员配置";
            treeNode12.Name = "NodPointConfig";
            treeNode12.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode12.Text = "质控评分项配置";
            treeNode13.Name = "MainpageQC";
            treeNode13.Text = "病案首页评分项配置";
            treeNode14.ImageIndex = 6;
            treeNode14.Name = "节点15";
            treeNode14.Text = "质控配置部分";
            treeNode15.ImageIndex = 4;
            treeNode15.Name = "RecordByDoctor";
            treeNode15.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode15.SelectedImageIndex = 4;
            treeNode15.Text = "医师书写病历统计";
            treeNode16.ImageIndex = 4;
            treeNode16.Name = "DiagOperRecord";
            treeNode16.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode16.SelectedImageIndex = 4;
            treeNode16.Text = "诊断和手术病案统计";
            treeNode17.ImageIndex = 4;
            treeNode17.Name = "Consulting";
            treeNode17.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode17.SelectedImageIndex = 4;
            treeNode17.Text = "会诊明细统计";
            treeNode18.ImageIndex = 4;
            treeNode18.Name = "NodDocQuery";
            treeNode18.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode18.SelectedImageIndex = 4;
            treeNode18.Text = "医师医疗质量控制";
            treeNode19.ImageIndex = 4;
            treeNode19.Name = "NodDept_Rate";
            treeNode19.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode19.SelectedImageIndex = 4;
            treeNode19.Text = "科室医疗质量控制";
            treeNode20.ImageIndex = 4;
            treeNode20.Name = "NodSingleDisease";
            treeNode20.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode20.SelectedImageIndex = 4;
            treeNode20.Text = "单病种医疗质量统计";
            treeNode21.ImageIndex = 4;
            treeNode21.Name = "Nodpatdiadetail";
            treeNode21.NodeFont = new System.Drawing.Font("宋体", 10F);
            treeNode21.SelectedImageIndex = 4;
            treeNode21.Text = "病人门急诊诊断明细统计";
            treeNode22.ImageIndex = 4;
            treeNode22.Name = "Node15";
            treeNode22.NodeFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            treeNode22.SelectedImageIndex = 4;
            treeNode22.Text = "汇总统计部分";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode14,
            treeNode22});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(192, 635);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "WaitingShift_16.png");
            this.imageList1.Images.SetKeyName(1, "DocmentArchives_16.png");
            this.imageList1.Images.SetKeyName(2, "DocmentArchives_32.png");
            this.imageList1.Images.SetKeyName(3, "PrintDesign_24.png");
            this.imageList1.Images.SetKeyName(4, "00426.ico");
            this.imageList1.Images.SetKeyName(5, "02105.ico");
            this.imageList1.Images.SetKeyName(6, "09648.ico");
            // 
            // btn_qc_doctor
            // 
            this.btn_qc_doctor.Caption = "医师医疗质量";
            this.btn_qc_doctor.Id = 15;
            this.btn_qc_doctor.Name = "btn_qc_doctor";
            this.btn_qc_doctor.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_doctor_ItemClick);
            // 
            // xtraTabQcManager
            // 
            this.xtraTabQcManager.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
            this.xtraTabQcManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabQcManager.Location = new System.Drawing.Point(2, 2);
            this.xtraTabQcManager.Name = "xtraTabQcManager";
            this.xtraTabQcManager.SelectedTabPage = this.xtraTabPageQualityMedicalRecord;
            this.xtraTabQcManager.Size = new System.Drawing.Size(1047, 654);
            this.xtraTabQcManager.TabIndex = 0;
            this.xtraTabQcManager.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageQualityMedicalRecord,
            this.xtraTabPage1,
            this.xtraTabPageOutHospital,
            this.xtraTabPageOutHospitalLock,
            this.xTabPageEmrScore,
            this.xtraTabDieInfo,
            this.xtraTabQCOperatInfo,
            this.xtraTabPageLostCat,
            this.xtraTabPageScore,
            this.xtraTabQCRescueInfo,
            this.xtraTabHosRate,
            this.xtraTabDepRate,
            this.xtraTabDeptQuery,
            this.xtraTabDocQuery,
            this.xtraTabSingleDisease,
            this.xtraTabReuPoint,
            this.xtraTabRecorByDoctor,
            this.xtraTabDiagOperRecord,
            this.xtraTabPageConsult,
            this.xtraTabPageDia,
            this.xtabUserLoginInfo,
            this.xtabActiveUser,
            this.xtabDiagGroupInfo,
            this.xtabDataBaseTable});
            this.xtraTabQcManager.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            this.xtraTabQcManager.CloseButtonClick += new System.EventHandler(this.xtraTabControl1_CloseButtonClick);
            // 
            // xtraTabPageQualityMedicalRecord
            // 
            this.xtraTabPageQualityMedicalRecord.Name = "xtraTabPageQualityMedicalRecord";
            this.xtraTabPageQualityMedicalRecord.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabPageQualityMedicalRecord.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabPageQualityMedicalRecord.Text = "全院质控";
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.PageVisible = false;
            this.xtraTabPage1.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabPage1.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabPage1.Text = "在院患者列表";
            // 
            // xtraTabPageOutHospital
            // 
            this.xtraTabPageOutHospital.Name = "xtraTabPageOutHospital";
            this.xtraTabPageOutHospital.PageVisible = false;
            this.xtraTabPageOutHospital.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabPageOutHospital.Text = "出院未提交病历统计";
            this.xtraTabPageOutHospital.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.xtraTabPageOutHospital_ControlRemoved);
            // 
            // xtraTabPageOutHospitalLock
            // 
            this.xtraTabPageOutHospitalLock.Name = "xtraTabPageOutHospitalLock";
            this.xtraTabPageOutHospitalLock.PageVisible = false;
            this.xtraTabPageOutHospitalLock.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabPageOutHospitalLock.Text = "出院病历统计";
            // 
            // xTabPageEmrScore
            // 
            this.xTabPageEmrScore.Name = "xTabPageEmrScore";
            this.xTabPageEmrScore.Size = new System.Drawing.Size(1041, 625);
            this.xTabPageEmrScore.Text = "病历评分列表";
            // 
            // xtraTabDieInfo
            // 
            this.xtraTabDieInfo.Name = "xtraTabDieInfo";
            this.xtraTabDieInfo.PageVisible = false;
            this.xtraTabDieInfo.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabDieInfo.Text = "死亡信息统计";
            // 
            // xtraTabQCOperatInfo
            // 
            this.xtraTabQCOperatInfo.Name = "xtraTabQCOperatInfo";
            this.xtraTabQCOperatInfo.PageVisible = false;
            this.xtraTabQCOperatInfo.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabQCOperatInfo.Text = "手术信息统计";
            // 
            // xtraTabPageLostCat
            // 
            this.xtraTabPageLostCat.Name = "xtraTabPageLostCat";
            this.xtraTabPageLostCat.PageVisible = false;
            this.xtraTabPageLostCat.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabPageLostCat.Text = "质控失分项目统计";
            // 
            // xtraTabPageScore
            // 
            this.xtraTabPageScore.Name = "xtraTabPageScore";
            this.xtraTabPageScore.PageVisible = false;
            this.xtraTabPageScore.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabPageScore.Text = "质控评分记录统计";
            // 
            // xtraTabQCRescueInfo
            // 
            this.xtraTabQCRescueInfo.Name = "xtraTabQCRescueInfo";
            this.xtraTabQCRescueInfo.PageVisible = false;
            this.xtraTabQCRescueInfo.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabQCRescueInfo.Text = "抢救信息统计";
            // 
            // xtraTabHosRate
            // 
            this.xtraTabHosRate.Name = "xtraTabHosRate";
            this.xtraTabHosRate.PageVisible = false;
            this.xtraTabHosRate.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabHosRate.Text = "全院病历质控率";
            // 
            // xtraTabDepRate
            // 
            this.xtraTabDepRate.Name = "xtraTabDepRate";
            this.xtraTabDepRate.PageVisible = false;
            this.xtraTabDepRate.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabDepRate.Text = "科室病历质控率";
            // 
            // xtraTabDeptQuery
            // 
            this.xtraTabDeptQuery.Name = "xtraTabDeptQuery";
            this.xtraTabDeptQuery.PageVisible = false;
            this.xtraTabDeptQuery.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabDeptQuery.Text = "科室医疗质量统计";
            // 
            // xtraTabDocQuery
            // 
            this.xtraTabDocQuery.Name = "xtraTabDocQuery";
            this.xtraTabDocQuery.PageVisible = false;
            this.xtraTabDocQuery.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabDocQuery.Text = "医师医疗质量统计";
            // 
            // xtraTabSingleDisease
            // 
            this.xtraTabSingleDisease.Name = "xtraTabSingleDisease";
            this.xtraTabSingleDisease.PageVisible = false;
            this.xtraTabSingleDisease.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabSingleDisease.Text = "单病种医疗质量分析";
            // 
            // xtraTabReuPoint
            // 
            this.xtraTabReuPoint.Name = "xtraTabReuPoint";
            this.xtraTabReuPoint.PageVisible = false;
            this.xtraTabReuPoint.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabReuPoint.Text = "病历评分缺陷项统计";
            // 
            // xtraTabRecorByDoctor
            // 
            this.xtraTabRecorByDoctor.Name = "xtraTabRecorByDoctor";
            this.xtraTabRecorByDoctor.PageVisible = false;
            this.xtraTabRecorByDoctor.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabRecorByDoctor.Text = "医师书写病历统计";
            // 
            // xtraTabDiagOperRecord
            // 
            this.xtraTabDiagOperRecord.Name = "xtraTabDiagOperRecord";
            this.xtraTabDiagOperRecord.PageVisible = false;
            this.xtraTabDiagOperRecord.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabDiagOperRecord.Text = "诊断和手术病案统计";
            // 
            // xtraTabPageConsult
            // 
            this.xtraTabPageConsult.Name = "xtraTabPageConsult";
            this.xtraTabPageConsult.PageVisible = false;
            this.xtraTabPageConsult.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabPageConsult.Text = "会诊明细统计";
            // 
            // xtraTabPageDia
            // 
            this.xtraTabPageDia.Name = "xtraTabPageDia";
            this.xtraTabPageDia.PageVisible = false;
            this.xtraTabPageDia.Size = new System.Drawing.Size(1041, 625);
            this.xtraTabPageDia.Text = "病人门急诊诊断明细统计";
            // 
            // xtabUserLoginInfo
            // 
            this.xtabUserLoginInfo.Name = "xtabUserLoginInfo";
            this.xtabUserLoginInfo.PageVisible = false;
            this.xtabUserLoginInfo.Size = new System.Drawing.Size(1041, 625);
            this.xtabUserLoginInfo.Text = "用户登录日志审计";
            // 
            // xtabActiveUser
            // 
            this.xtabActiveUser.Name = "xtabActiveUser";
            this.xtabActiveUser.PageVisible = false;
            this.xtabActiveUser.Size = new System.Drawing.Size(1041, 625);
            this.xtabActiveUser.Text = "活跃用户统计";
            // 
            // xtabDiagGroupInfo
            // 
            this.xtabDiagGroupInfo.Name = "xtabDiagGroupInfo";
            this.xtabDiagGroupInfo.PageVisible = false;
            this.xtabDiagGroupInfo.Size = new System.Drawing.Size(1041, 625);
            this.xtabDiagGroupInfo.Text = "同类疾病分组主索引";
            // 
            // xtabDataBaseTable
            // 
            this.xtabDataBaseTable.Name = "xtabDataBaseTable";
            this.xtabDataBaseTable.PageVisible = false;
            this.xtabDataBaseTable.Size = new System.Drawing.Size(1041, 625);
            this.xtabDataBaseTable.Text = "数据库表信息审计";
            // 
            // barConfigQCPoint
            // 
            this.barConfigQCPoint.Caption = "科室质控人员配置";
            this.barConfigQCPoint.Id = 31;
            this.barConfigQCPoint.Name = "barConfigQCPoint";
            // 
            // panelControl1
            // 
            this.panelControl1.Location = new System.Drawing.Point(2, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(222, 654);
            this.panelControl1.TabIndex = 1;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.xtraTabQcManager);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(2, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1051, 658);
            this.panelControl2.TabIndex = 0;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.panelControl2);
            this.panelControl3.Controls.Add(this.panelControl1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(200, 31);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1055, 662);
            this.panelControl3.TabIndex = 0;
            // 
            // QcManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1255, 693);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QcManagerForm";
            this.Text = "全院质量控制";
            this.Load += new System.EventHandler(this.QcManagerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabQcManager)).EndInit();
            this.xtraTabQcManager.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarSubItem barSubItem3;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private DevExpress.XtraBars.BarButtonItem btn_qc_doctor;
        private DevExpress.XtraTab.XtraTabControl xtraTabQcManager;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageOutHospital;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageOutHospitalLock;
        private DevExpress.XtraBars.BarSubItem barSubItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
        private DevExpress.XtraBars.BarButtonItem barButtonItem10;
        private DevExpress.XtraBars.BarButtonItem btn_qc_dept;
        private DevExpress.XtraBars.BarButtonItem btn_hospital_rate;
        private DevExpress.XtraBars.BarButtonItem btn_dept_rate;
        private DevExpress.XtraBars.BarButtonItem btn_qc_SingleDisease;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
        private DevExpress.XtraBars.BarButtonItem barButtonItem11;
        private DevExpress.XtraBars.BarButtonItem btn_monitor_item;
        private DevExpress.XtraBars.BarButtonItem btn_Disease_Level;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageQualityMedicalRecord;
        private DevExpress.XtraBars.BarButtonItem barBtnPointSum;
        private DevExpress.XtraTab.XtraTabPage xTabPageEmrScore;
        private DevExpress.XtraBars.BarButtonItem barConfigQCPoint;
        private DevExpress.XtraBars.BarButtonItem barBtnConfigQCAudit;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageConsult;
        private DevExpress.XtraBars.BarButtonItem barButtonItemConsult;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraTab.XtraTabPage xtraTabDieInfo;
        private DevExpress.XtraTab.XtraTabPage xtraTabQCOperatInfo;
        private DevExpress.XtraTab.XtraTabPage xtraTabQCRescueInfo;
        private DevExpress.XtraTab.XtraTabPage xtraTabHosRate;
        private DevExpress.XtraTab.XtraTabPage xtraTabDepRate;
        private DevExpress.XtraTab.XtraTabPage xtraTabDeptQuery;
        private DevExpress.XtraTab.XtraTabPage xtraTabDocQuery;
        private DevExpress.XtraTab.XtraTabPage xtraTabSingleDisease;
        private DevExpress.XtraTab.XtraTabPage xtraTabReuPoint;
        private DevExpress.XtraTab.XtraTabPage xtraTabRecorByDoctor;
        private DevExpress.XtraTab.XtraTabPage xtraTabDiagOperRecord;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageDia;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageLostCat;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageScore;
        private DevExpress.XtraTab.XtraTabPage xtabUserLoginInfo;
        private DevExpress.XtraTab.XtraTabPage xtabActiveUser;
        private DevExpress.XtraTab.XtraTabPage xtabDiagGroupInfo;
        private DevExpress.XtraTab.XtraTabPage xtabDataBaseTable;
    }
}