using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.Data;
using YidanSoft.FrameWork;
using YidanSoft.FrameWork.WinForm;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.FrameWork.Plugin.Manager;

namespace YidanSoft.Common.Report
{
    public class CustomDesignForm : DevExpress.XtraReports.UserDesigner.XRDesignFormExBase, YidanSoft.FrameWork.IStartPlugIn
    {
        #region
        private DevExpress.XtraReports.UserDesigner.XRDesignBarManager xrDesignBarManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraReports.UserDesigner.RecentlyUsedItemsComboBox ricbFontName;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ricbFontSize;
        private DevExpress.XtraReports.UserDesigner.DesignBar designBar1;
        private DevExpress.XtraReports.UserDesigner.DesignBar designBar2;
        private DevExpress.XtraReports.UserDesigner.DesignBar designBar3;
        private DevExpress.XtraReports.UserDesigner.DesignBar designBar4;
        private DevExpress.XtraReports.UserDesigner.DesignBar designBar5;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraBars.BarEditItem barEditItem2;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem1;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem2;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem3;
        private DevExpress.XtraReports.UserDesigner.CommandColorBarItem commandColorBarItem1;
        private DevExpress.XtraReports.UserDesigner.CommandColorBarItem commandColorBarItem2;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem4;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem5;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem6;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem7;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem8;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem9;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem10;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem11;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem12;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem13;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem14;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem15;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem16;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem17;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem18;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem19;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem20;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem21;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem22;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem23;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem24;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem25;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem26;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem27;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem28;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem29;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem30;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem31;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem32;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem33;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem34;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem35;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem36;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem37;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem38;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarSubItem barSubItem3;
        private DevExpress.XtraReports.UserDesigner.BarReportTabButtonsListItem barReportTabButtonsListItem1;
        private DevExpress.XtraBars.BarSubItem barSubItem4;
        private DevExpress.XtraReports.UserDesigner.XRBarToolbarsListItem xrBarToolbarsListItem1;
        private DevExpress.XtraBars.BarSubItem barSubItem5;
        private DevExpress.XtraReports.UserDesigner.BarDockPanelsListItem barDockPanelsListItem1;
        private DevExpress.XtraBars.BarSubItem barSubItem6;
        private DevExpress.XtraBars.BarSubItem barSubItem7;
        private DevExpress.XtraBars.BarSubItem barSubItem8;
        private DevExpress.XtraBars.BarSubItem barSubItem9;
        private DevExpress.XtraBars.BarSubItem barSubItem10;
        private DevExpress.XtraBars.BarSubItem barSubItem11;
        private DevExpress.XtraBars.BarSubItem barSubItem12;
        private DevExpress.XtraBars.BarSubItem barSubItem13;
        private DevExpress.XtraBars.BarSubItem barSubItem14;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem39;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem40;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem41;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem42;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem43;
        private DevExpress.XtraBars.BarSubItem bsiLookAndFeel;
        private DevExpress.XtraReports.UserDesigner.XRDesignDockManager xrDesignDockManager1;
        private DevExpress.XtraReports.UserDesigner.FieldListDockPanel fieldListDockPanel1;
        private DevExpress.XtraReports.UserDesigner.DesignControlContainer fieldListDockPanel1_Container;
        private DevExpress.XtraReports.UserDesigner.PropertyGridDockPanel propertyGridDockPanel1;
        private DevExpress.XtraReports.UserDesigner.DesignControlContainer propertyGridDockPanel1_Container;
        private DevExpress.XtraReports.UserDesigner.ReportExplorerDockPanel reportExplorerDockPanel1;
        private DevExpress.XtraReports.UserDesigner.DesignControlContainer reportExplorerDockPanel1_Container;
        private DevExpress.XtraReports.UserDesigner.ToolBoxDockPanel toolBoxDockPanel1;
        private DevExpress.XtraReports.UserDesigner.DesignControlContainer toolBoxDockPanel1_Container;
        private DevExpress.XtraReports.UserDesigner.DesignDockPanel panelContainer1;
        private DevExpress.XtraReports.UserDesigner.DesignDockPanel panelContainer2;
        private DevExpress.XtraReports.UserDesigner.DesignRepositoryItemComboBox designRepositoryItemComboBox1;
        private DevExpress.XtraReports.UserDesigner.DesignBar designBar6;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem44;
        private DevExpress.XtraReports.UserDesigner.XRZoomBarEditItem xrZoomBarEditItem1;
        private DevExpress.XtraReports.UserDesigner.CommandBarItem commandBarItem45;
        private System.ComponentModel.Container components = null;
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomDesignForm));
            this.xrDesignBarManager1 = new DevExpress.XtraReports.UserDesigner.XRDesignBarManager();
            this.designBar1 = new DevExpress.XtraReports.UserDesigner.DesignBar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.commandBarItem31 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem39 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem32 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem33 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem40 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem41 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.commandBarItem37 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem38 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem34 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem35 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem36 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem42 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem43 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
            this.barReportTabButtonsListItem1 = new DevExpress.XtraReports.UserDesigner.BarReportTabButtonsListItem();
            this.barSubItem4 = new DevExpress.XtraBars.BarSubItem();
            this.xrBarToolbarsListItem1 = new DevExpress.XtraReports.UserDesigner.XRBarToolbarsListItem();
            this.designBar2 = new DevExpress.XtraReports.UserDesigner.DesignBar();
            this.designBar3 = new DevExpress.XtraReports.UserDesigner.DesignBar();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.ricbFontName = new DevExpress.XtraReports.UserDesigner.RecentlyUsedItemsComboBox();
            this.barEditItem2 = new DevExpress.XtraBars.BarEditItem();
            this.ricbFontSize = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.commandBarItem1 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem2 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem3 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandColorBarItem1 = new DevExpress.XtraReports.UserDesigner.CommandColorBarItem();
            this.commandColorBarItem2 = new DevExpress.XtraReports.UserDesigner.CommandColorBarItem();
            this.commandBarItem4 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem5 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem6 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem7 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.designBar4 = new DevExpress.XtraReports.UserDesigner.DesignBar();
            this.commandBarItem8 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem9 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem10 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem11 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem12 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem14 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem15 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem16 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem17 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem18 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem19 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem20 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem21 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem22 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem23 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem25 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem26 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem27 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem28 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem29 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem30 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.designBar5 = new DevExpress.XtraReports.UserDesigner.DesignBar();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.designBar6 = new DevExpress.XtraReports.UserDesigner.DesignBar();
            this.commandBarItem44 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.xrZoomBarEditItem1 = new DevExpress.XtraReports.UserDesigner.XRZoomBarEditItem();
            this.designRepositoryItemComboBox1 = new DevExpress.XtraReports.UserDesigner.DesignRepositoryItemComboBox();
            this.commandBarItem45 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.xrDesignDockManager1 = new DevExpress.XtraReports.UserDesigner.XRDesignDockManager();
            this.panelContainer1 = new DevExpress.XtraReports.UserDesigner.DesignDockPanel();
            this.panelContainer2 = new DevExpress.XtraReports.UserDesigner.DesignDockPanel();
            this.reportExplorerDockPanel1 = new DevExpress.XtraReports.UserDesigner.ReportExplorerDockPanel();
            this.reportExplorerDockPanel1_Container = new DevExpress.XtraReports.UserDesigner.DesignControlContainer();
            this.fieldListDockPanel1 = new DevExpress.XtraReports.UserDesigner.FieldListDockPanel();
            this.fieldListDockPanel1_Container = new DevExpress.XtraReports.UserDesigner.DesignControlContainer();
            this.propertyGridDockPanel1 = new DevExpress.XtraReports.UserDesigner.PropertyGridDockPanel();
            this.propertyGridDockPanel1_Container = new DevExpress.XtraReports.UserDesigner.DesignControlContainer();
            this.toolBoxDockPanel1 = new DevExpress.XtraReports.UserDesigner.ToolBoxDockPanel();
            this.toolBoxDockPanel1_Container = new DevExpress.XtraReports.UserDesigner.DesignControlContainer();
            this.commandBarItem13 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.commandBarItem24 = new DevExpress.XtraReports.UserDesigner.CommandBarItem();
            this.barSubItem5 = new DevExpress.XtraBars.BarSubItem();
            this.barDockPanelsListItem1 = new DevExpress.XtraReports.UserDesigner.BarDockPanelsListItem();
            this.barSubItem6 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem7 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem8 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem9 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem10 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem11 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem12 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem13 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem14 = new DevExpress.XtraBars.BarSubItem();
            this.bsiLookAndFeel = new DevExpress.XtraBars.BarSubItem();
            ((System.ComponentModel.ISupportInitialize)(this.xrDesignPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrDesignBarManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricbFontName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricbFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.designRepositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrDesignDockManager1)).BeginInit();
            this.panelContainer1.SuspendLayout();
            this.panelContainer2.SuspendLayout();
            this.reportExplorerDockPanel1.SuspendLayout();
            this.fieldListDockPanel1.SuspendLayout();
            this.propertyGridDockPanel1.SuspendLayout();
            this.toolBoxDockPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xrDesignPanel
            // 
            this.xrDesignPanel.Location = new System.Drawing.Point(198, 80);
            this.xrDesignPanel.Size = new System.Drawing.Size(334, 464);
            // 
            // xrDesignBarManager1
            // 
            this.xrDesignBarManager1.AllowQuickCustomization = false;
            this.xrDesignBarManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.designBar1,
            this.designBar2,
            this.designBar3,
            this.designBar4,
            this.designBar5,
            this.designBar6});
            this.xrDesignBarManager1.DockControls.Add(this.barDockControlTop);
            this.xrDesignBarManager1.DockControls.Add(this.barDockControlBottom);
            this.xrDesignBarManager1.DockControls.Add(this.barDockControlLeft);
            this.xrDesignBarManager1.DockControls.Add(this.barDockControlRight);
            this.xrDesignBarManager1.DockManager = this.xrDesignDockManager1;
            this.xrDesignBarManager1.FontNameBox = this.ricbFontName;
            this.xrDesignBarManager1.FontNameEdit = this.barEditItem1;
            this.xrDesignBarManager1.FontSizeBox = this.ricbFontSize;
            this.xrDesignBarManager1.FontSizeEdit = this.barEditItem2;
            this.xrDesignBarManager1.Form = this;
            this.xrDesignBarManager1.FormattingToolbar = this.designBar3;
            this.xrDesignBarManager1.HintStaticItem = this.barStaticItem1;
            this.xrDesignBarManager1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("xrDesignBarManager1.ImageStream")));
            this.xrDesignBarManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barEditItem1,
            this.barEditItem2,
            this.commandBarItem1,
            this.commandBarItem2,
            this.commandBarItem3,
            this.commandColorBarItem1,
            this.commandColorBarItem2,
            this.commandBarItem4,
            this.commandBarItem5,
            this.commandBarItem6,
            this.commandBarItem7,
            this.commandBarItem8,
            this.commandBarItem9,
            this.commandBarItem10,
            this.commandBarItem11,
            this.commandBarItem12,
            this.commandBarItem13,
            this.commandBarItem14,
            this.commandBarItem15,
            this.commandBarItem16,
            this.commandBarItem17,
            this.commandBarItem18,
            this.commandBarItem19,
            this.commandBarItem20,
            this.commandBarItem21,
            this.commandBarItem22,
            this.commandBarItem23,
            this.commandBarItem24,
            this.commandBarItem25,
            this.commandBarItem26,
            this.commandBarItem27,
            this.commandBarItem28,
            this.commandBarItem29,
            this.commandBarItem30,
            this.commandBarItem31,
            this.commandBarItem32,
            this.commandBarItem33,
            this.commandBarItem34,
            this.commandBarItem35,
            this.commandBarItem36,
            this.commandBarItem37,
            this.commandBarItem38,
            this.barStaticItem1,
            this.barSubItem1,
            this.barSubItem2,
            this.barSubItem3,
            this.barReportTabButtonsListItem1,
            this.barSubItem4,
            this.xrBarToolbarsListItem1,
            this.barSubItem5,
            this.barDockPanelsListItem1,
            this.barSubItem6,
            this.barSubItem7,
            this.barSubItem8,
            this.barSubItem9,
            this.barSubItem10,
            this.barSubItem11,
            this.barSubItem12,
            this.barSubItem13,
            this.barSubItem14,
            this.commandBarItem39,
            this.commandBarItem40,
            this.commandBarItem41,
            this.commandBarItem42,
            this.commandBarItem43,
            this.bsiLookAndFeel,
            this.commandBarItem44,
            this.xrZoomBarEditItem1,
            this.commandBarItem45});
            this.xrDesignBarManager1.LayoutToolbar = this.designBar4;
            this.xrDesignBarManager1.MainMenu = this.designBar1;
            this.xrDesignBarManager1.MaxItemId = 69;
            this.xrDesignBarManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ricbFontName,
            this.ricbFontSize,
            this.designRepositoryItemComboBox1});
            this.xrDesignBarManager1.StatusBar = this.designBar5;
            this.xrDesignBarManager1.Toolbar = this.designBar2;
            this.xrDesignBarManager1.XRDesignPanel = this.xrDesignPanel;
            this.xrDesignBarManager1.ZoomItem = this.xrZoomBarEditItem1;
            // 
            // designBar1
            // 
            this.designBar1.BarName = "designBar1";
            this.designBar1.DockCol = 0;
            this.designBar1.DockRow = 0;
            this.designBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.designBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Caption, this.barSubItem1, "文件(&F)"),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem3)});
            this.designBar1.OptionsBar.MultiLine = true;
            this.designBar1.OptionsBar.UseWholeRow = true;
            this.designBar1.Text = "主菜单";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "文件";
            this.barSubItem1.Id = 43;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem31),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem39),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem32),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem33, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem40),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem41, true)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // commandBarItem31
            // 
            this.commandBarItem31.Caption = "新增(&N)";
            this.commandBarItem31.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.NewReport;
            this.commandBarItem31.Hint = "Create a new blank report";
            this.commandBarItem31.Id = 34;
            this.commandBarItem31.ImageIndex = 9;
            this.commandBarItem31.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
            this.commandBarItem31.Name = "commandBarItem31";
            // 
            // commandBarItem39
            // 
            this.commandBarItem39.Caption = "使用导航功能，创建一个新报表";
            this.commandBarItem39.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.NewReportWizard;
            this.commandBarItem39.Hint = "使用导航功能，创建一个新报表";
            this.commandBarItem39.Id = 60;
            this.commandBarItem39.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W));
            this.commandBarItem39.Name = "commandBarItem39";
            // 
            // commandBarItem32
            // 
            this.commandBarItem32.Caption = "打开文件(&O)";
            this.commandBarItem32.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.OpenFile;
            this.commandBarItem32.Hint = "Open a report";
            this.commandBarItem32.Id = 35;
            this.commandBarItem32.ImageIndex = 10;
            this.commandBarItem32.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O));
            this.commandBarItem32.Name = "commandBarItem32";
            // 
            // commandBarItem33
            // 
            this.commandBarItem33.Caption = "保存(&S)";
            this.commandBarItem33.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SaveFile;
            this.commandBarItem33.Enabled = false;
            this.commandBarItem33.Hint = "Save a report";
            this.commandBarItem33.Id = 36;
            this.commandBarItem33.ImageIndex = 11;
            this.commandBarItem33.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S));
            this.commandBarItem33.Name = "commandBarItem33";
            // 
            // commandBarItem40
            // 
            this.commandBarItem40.Caption = "另存为...";
            this.commandBarItem40.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SaveFileAs;
            this.commandBarItem40.Enabled = false;
            this.commandBarItem40.Hint = "Save a report with a new name";
            this.commandBarItem40.Id = 61;
            this.commandBarItem40.Name = "commandBarItem40";
            // 
            // commandBarItem41
            // 
            this.commandBarItem41.Caption = "返回";
            this.commandBarItem41.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Exit;
            this.commandBarItem41.Hint = "Close the designer";
            this.commandBarItem41.Id = 62;
            this.commandBarItem41.Name = "commandBarItem41";
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "编辑(&E)";
            this.barSubItem2.Id = 44;
            this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem37, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem38),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem34, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem35),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem36),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem42),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem43, true)});
            this.barSubItem2.Name = "barSubItem2";
            // 
            // commandBarItem37
            // 
            this.commandBarItem37.Caption = "撤销(&U)";
            this.commandBarItem37.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Undo;
            this.commandBarItem37.Enabled = false;
            this.commandBarItem37.Hint = "Undo the last operation";
            this.commandBarItem37.Id = 40;
            this.commandBarItem37.ImageIndex = 15;
            this.commandBarItem37.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z));
            this.commandBarItem37.Name = "commandBarItem37";
            // 
            // commandBarItem38
            // 
            this.commandBarItem38.Caption = "重复(&R)";
            this.commandBarItem38.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Redo;
            this.commandBarItem38.Enabled = false;
            this.commandBarItem38.Hint = "Redo the last operation";
            this.commandBarItem38.Id = 41;
            this.commandBarItem38.ImageIndex = 16;
            this.commandBarItem38.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y));
            this.commandBarItem38.Name = "commandBarItem38";
            // 
            // commandBarItem34
            // 
            this.commandBarItem34.Caption = "剪切(&t)";
            this.commandBarItem34.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Cut;
            this.commandBarItem34.Enabled = false;
            this.commandBarItem34.Hint = "Delete the control and copy it to the clipboard";
            this.commandBarItem34.Id = 37;
            this.commandBarItem34.ImageIndex = 12;
            this.commandBarItem34.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X));
            this.commandBarItem34.Name = "commandBarItem34";
            // 
            // commandBarItem35
            // 
            this.commandBarItem35.Caption = "赋值(&C)";
            this.commandBarItem35.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Copy;
            this.commandBarItem35.Enabled = false;
            this.commandBarItem35.Hint = "Copy the control to the clipboard";
            this.commandBarItem35.Id = 38;
            this.commandBarItem35.ImageIndex = 13;
            this.commandBarItem35.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C));
            this.commandBarItem35.Name = "commandBarItem35";
            // 
            // commandBarItem36
            // 
            this.commandBarItem36.Caption = "粘贴(&P)";
            this.commandBarItem36.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Paste;
            this.commandBarItem36.Enabled = false;
            this.commandBarItem36.Hint = "Add the control from the clipboard";
            this.commandBarItem36.Id = 39;
            this.commandBarItem36.ImageIndex = 14;
            this.commandBarItem36.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V));
            this.commandBarItem36.Name = "commandBarItem36";
            // 
            // commandBarItem42
            // 
            this.commandBarItem42.Caption = "删除(&D)";
            this.commandBarItem42.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Delete;
            this.commandBarItem42.Enabled = false;
            this.commandBarItem42.Hint = "Delete the control";
            this.commandBarItem42.Id = 63;
            this.commandBarItem42.Name = "commandBarItem42";
            // 
            // commandBarItem43
            // 
            this.commandBarItem43.Caption = "全选(&A)";
            this.commandBarItem43.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SelectAll;
            this.commandBarItem43.Enabled = false;
            this.commandBarItem43.Hint = "Select all the controls in the document";
            this.commandBarItem43.Id = 64;
            this.commandBarItem43.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A));
            this.commandBarItem43.Name = "commandBarItem43";
            // 
            // barSubItem3
            // 
            this.barSubItem3.Caption = "视图(&V)";
            this.barSubItem3.Id = 45;
            this.barSubItem3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barReportTabButtonsListItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem4, true)});
            this.barSubItem3.Name = "barSubItem3";
            // 
            // barReportTabButtonsListItem1
            // 
            this.barReportTabButtonsListItem1.Caption = "Tab Buttons";
            this.barReportTabButtonsListItem1.Id = 46;
            this.barReportTabButtonsListItem1.Name = "barReportTabButtonsListItem1";
            // 
            // barSubItem4
            // 
            this.barSubItem4.Caption = "工具栏(&T)";
            this.barSubItem4.Id = 47;
            this.barSubItem4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.xrBarToolbarsListItem1)});
            this.barSubItem4.Name = "barSubItem4";
            // 
            // xrBarToolbarsListItem1
            // 
            this.xrBarToolbarsListItem1.Caption = "工具栏(&T)";
            this.xrBarToolbarsListItem1.Id = 48;
            this.xrBarToolbarsListItem1.Name = "xrBarToolbarsListItem1";
            // 
            // designBar2
            // 
            this.designBar2.BarName = "ToolBar";
            this.designBar2.DockCol = 0;
            this.designBar2.DockRow = 1;
            this.designBar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.designBar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.commandBarItem31, false),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.commandBarItem32, false),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem33),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem34, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem35),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem36),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem37, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem38)});
            this.designBar2.Text = "Main Toolbar";
            // 
            // designBar3
            // 
            this.designBar3.BarName = "FormattingToolBar";
            this.designBar3.DockCol = 1;
            this.designBar3.DockRow = 1;
            this.designBar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.designBar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barEditItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barEditItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandColorBarItem1, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandColorBarItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem4, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem5),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem6),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem7)});
            this.designBar3.Text = "Formatting Toolbar";
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "字体";
            this.barEditItem1.Edit = this.ricbFontName;
            this.barEditItem1.Hint = "Font Name";
            this.barEditItem1.Id = 0;
            this.barEditItem1.Name = "barEditItem1";
            this.barEditItem1.Width = 120;
            // 
            // ricbFontName
            // 
            this.ricbFontName.AutoHeight = false;
            this.ricbFontName.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ricbFontName.DropDownRows = 12;
            this.ricbFontName.Name = "ricbFontName";
            // 
            // barEditItem2
            // 
            this.barEditItem2.Caption = "字号";
            this.barEditItem2.Edit = this.ricbFontSize;
            this.barEditItem2.Hint = "Font Size";
            this.barEditItem2.Id = 1;
            this.barEditItem2.Name = "barEditItem2";
            this.barEditItem2.Width = 55;
            // 
            // ricbFontSize
            // 
            this.ricbFontSize.AutoHeight = false;
            this.ricbFontSize.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ricbFontSize.Items.AddRange(new object[] {
            ((byte)(8)),
            ((byte)(9)),
            ((byte)(10)),
            ((byte)(11)),
            ((byte)(12)),
            ((byte)(14)),
            ((byte)(16)),
            ((byte)(18)),
            ((byte)(20)),
            ((byte)(22)),
            ((byte)(24)),
            ((byte)(26)),
            ((byte)(28)),
            ((byte)(36)),
            ((byte)(48)),
            ((byte)(72))});
            this.ricbFontSize.Name = "ricbFontSize";
            // 
            // commandBarItem1
            // 
            this.commandBarItem1.Caption = "加粗(&B)";
            this.commandBarItem1.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.FontBold;
            this.commandBarItem1.Enabled = false;
            this.commandBarItem1.Hint = "Make the font bold";
            this.commandBarItem1.Id = 2;
            this.commandBarItem1.ImageIndex = 0;
            this.commandBarItem1.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B));
            this.commandBarItem1.Name = "commandBarItem1";
            // 
            // commandBarItem2
            // 
            this.commandBarItem2.Caption = "倾斜(&I)";
            this.commandBarItem2.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.FontItalic;
            this.commandBarItem2.Enabled = false;
            this.commandBarItem2.Hint = "Make the font italic";
            this.commandBarItem2.Id = 3;
            this.commandBarItem2.ImageIndex = 1;
            this.commandBarItem2.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I));
            this.commandBarItem2.Name = "commandBarItem2";
            // 
            // commandBarItem3
            // 
            this.commandBarItem3.Caption = "下划线(&U)";
            this.commandBarItem3.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.FontUnderline;
            this.commandBarItem3.Enabled = false;
            this.commandBarItem3.Hint = "Underline the font";
            this.commandBarItem3.Id = 4;
            this.commandBarItem3.ImageIndex = 2;
            this.commandBarItem3.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U));
            this.commandBarItem3.Name = "commandBarItem3";
            // 
            // commandColorBarItem1
            // 
            this.commandColorBarItem1.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.commandColorBarItem1.Caption = "字体颜色(&E)";
            this.commandColorBarItem1.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.ForeColor;
            this.commandColorBarItem1.Enabled = false;
            this.commandColorBarItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("commandColorBarItem1.Glyph")));
            this.commandColorBarItem1.Hint = "Set the foreground color of the control";
            this.commandColorBarItem1.Id = 5;
            this.commandColorBarItem1.Name = "commandColorBarItem1";
            // 
            // commandColorBarItem2
            // 
            this.commandColorBarItem2.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.commandColorBarItem2.Caption = "字体背景色(&K)";
            this.commandColorBarItem2.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.BackColor;
            this.commandColorBarItem2.Enabled = false;
            this.commandColorBarItem2.Glyph = ((System.Drawing.Image)(resources.GetObject("commandColorBarItem2.Glyph")));
            this.commandColorBarItem2.Hint = "Set the background color of the control";
            this.commandColorBarItem2.Id = 6;
            this.commandColorBarItem2.Name = "commandColorBarItem2";
            // 
            // commandBarItem4
            // 
            this.commandBarItem4.Caption = "文本左对齐(&L)";
            this.commandBarItem4.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.JustifyLeft;
            this.commandBarItem4.Enabled = false;
            this.commandBarItem4.Hint = "文本左对齐";
            this.commandBarItem4.Id = 7;
            this.commandBarItem4.ImageIndex = 5;
            this.commandBarItem4.Name = "commandBarItem4";
            // 
            // commandBarItem5
            // 
            this.commandBarItem5.Caption = "居中(&C)";
            this.commandBarItem5.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.JustifyCenter;
            this.commandBarItem5.Enabled = false;
            this.commandBarItem5.Hint = "居中";
            this.commandBarItem5.Id = 8;
            this.commandBarItem5.ImageIndex = 6;
            this.commandBarItem5.Name = "commandBarItem5";
            // 
            // commandBarItem6
            // 
            this.commandBarItem6.Caption = "文本右对齐(&R)";
            this.commandBarItem6.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.JustifyRight;
            this.commandBarItem6.Enabled = false;
            this.commandBarItem6.Hint = "文本右对齐";
            this.commandBarItem6.Id = 9;
            this.commandBarItem6.ImageIndex = 7;
            this.commandBarItem6.Name = "commandBarItem6";
            // 
            // commandBarItem7
            // 
            this.commandBarItem7.Caption = "两端对齐(&J)";
            this.commandBarItem7.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.JustifyJustify;
            this.commandBarItem7.Enabled = false;
            this.commandBarItem7.Hint = "两端对齐";
            this.commandBarItem7.Id = 10;
            this.commandBarItem7.ImageIndex = 8;
            this.commandBarItem7.Name = "commandBarItem7";
            // 
            // designBar4
            // 
            this.designBar4.BarName = "LayoutToolBar";
            this.designBar4.DockCol = 0;
            this.designBar4.DockRow = 2;
            this.designBar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.designBar4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem8),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem9, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem10),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem11),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem12, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem14),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem15, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem16),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem17),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem18),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem19, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem20),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem21),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem22),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem23, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem25),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem26),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem27, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem28),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem29, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem30)});
            this.designBar4.Offset = 2;
            this.designBar4.Text = "Layout Toolbar";
            // 
            // commandBarItem8
            // 
            this.commandBarItem8.Caption = "贴齐网格线(&G)";
            this.commandBarItem8.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignToGrid;
            this.commandBarItem8.Enabled = false;
            this.commandBarItem8.Hint = "贴齐网格线";
            this.commandBarItem8.Id = 11;
            this.commandBarItem8.ImageIndex = 17;
            this.commandBarItem8.Name = "commandBarItem8";
            // 
            // commandBarItem9
            // 
            this.commandBarItem9.Caption = "靠左对齐(&L)";
            this.commandBarItem9.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignLeft;
            this.commandBarItem9.Enabled = false;
            this.commandBarItem9.Hint = "靠左对齐";
            this.commandBarItem9.Id = 12;
            this.commandBarItem9.ImageIndex = 18;
            this.commandBarItem9.Name = "commandBarItem9";
            // 
            // commandBarItem10
            // 
            this.commandBarItem10.Caption = "垂直居中对齐(&C)";
            this.commandBarItem10.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignVerticalCenters;
            this.commandBarItem10.Enabled = false;
            this.commandBarItem10.Hint = "垂直居中对齐";
            this.commandBarItem10.Id = 13;
            this.commandBarItem10.ImageIndex = 19;
            this.commandBarItem10.Name = "commandBarItem10";
            // 
            // commandBarItem11
            // 
            this.commandBarItem11.Caption = "靠左对齐(&R)";
            this.commandBarItem11.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignRight;
            this.commandBarItem11.Enabled = false;
            this.commandBarItem11.Hint = "靠左对齐";
            this.commandBarItem11.Id = 14;
            this.commandBarItem11.ImageIndex = 20;
            this.commandBarItem11.Name = "commandBarItem11";
            // 
            // commandBarItem12
            // 
            this.commandBarItem12.Caption = "上对齐(&T)";
            this.commandBarItem12.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignTop;
            this.commandBarItem12.Enabled = false;
            this.commandBarItem12.Hint = "上对齐";
            this.commandBarItem12.Id = 15;
            this.commandBarItem12.ImageIndex = 21;
            this.commandBarItem12.Name = "commandBarItem12";
            // 
            // commandBarItem14
            // 
            this.commandBarItem14.Caption = "下对齐(&B)";
            this.commandBarItem14.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignBottom;
            this.commandBarItem14.Enabled = false;
            this.commandBarItem14.Hint = "下对齐";
            this.commandBarItem14.Id = 17;
            this.commandBarItem14.ImageIndex = 23;
            this.commandBarItem14.Name = "commandBarItem14";
            // 
            // commandBarItem15
            // 
            this.commandBarItem15.Caption = "调整宽度(&W)";
            this.commandBarItem15.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SizeToControlWidth;
            this.commandBarItem15.Enabled = false;
            this.commandBarItem15.Hint = "调整宽度";
            this.commandBarItem15.Id = 18;
            this.commandBarItem15.ImageIndex = 24;
            this.commandBarItem15.Name = "commandBarItem15";
            // 
            // commandBarItem16
            // 
            this.commandBarItem16.Caption = "贴齐网格线(&D)";
            this.commandBarItem16.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SizeToGrid;
            this.commandBarItem16.Enabled = false;
            this.commandBarItem16.Hint = "贴齐网格线";
            this.commandBarItem16.Id = 19;
            this.commandBarItem16.ImageIndex = 25;
            this.commandBarItem16.Name = "commandBarItem16";
            // 
            // commandBarItem17
            // 
            this.commandBarItem17.Caption = "调整高度(&H)";
            this.commandBarItem17.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SizeToControlHeight;
            this.commandBarItem17.Enabled = false;
            this.commandBarItem17.Hint = "选中的控件具有相同的高度";
            this.commandBarItem17.Id = 20;
            this.commandBarItem17.ImageIndex = 26;
            this.commandBarItem17.Name = "commandBarItem17";
            // 
            // commandBarItem18
            // 
            this.commandBarItem18.Caption = "调整大小(&B)";
            this.commandBarItem18.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SizeToControl;
            this.commandBarItem18.Enabled = false;
            this.commandBarItem18.Hint = "选中的控件具有相同大小";
            this.commandBarItem18.Id = 21;
            this.commandBarItem18.ImageIndex = 27;
            this.commandBarItem18.Name = "commandBarItem18";
            // 
            // commandBarItem19
            // 
            this.commandBarItem19.Caption = "水平间距等距(&E)";
            this.commandBarItem19.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.HorizSpaceMakeEqual;
            this.commandBarItem19.Enabled = false;
            this.commandBarItem19.Hint = "水平间距等距";
            this.commandBarItem19.Id = 22;
            this.commandBarItem19.ImageIndex = 28;
            this.commandBarItem19.Name = "commandBarItem19";
            // 
            // commandBarItem20
            // 
            this.commandBarItem20.Caption = "增加水平间距(I)";
            this.commandBarItem20.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.HorizSpaceIncrease;
            this.commandBarItem20.Enabled = false;
            this.commandBarItem20.Hint = "增加水平间距";
            this.commandBarItem20.Id = 23;
            this.commandBarItem20.ImageIndex = 29;
            this.commandBarItem20.Name = "commandBarItem20";
            // 
            // commandBarItem21
            // 
            this.commandBarItem21.Caption = "减少水平间距(&D)";
            this.commandBarItem21.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.HorizSpaceDecrease;
            this.commandBarItem21.Enabled = false;
            this.commandBarItem21.Hint = "减少水平间距";
            this.commandBarItem21.Id = 24;
            this.commandBarItem21.ImageIndex = 30;
            this.commandBarItem21.Name = "commandBarItem21";
            // 
            // commandBarItem22
            // 
            this.commandBarItem22.Caption = "删除水平间距(&R)";
            this.commandBarItem22.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.HorizSpaceConcatenate;
            this.commandBarItem22.Enabled = false;
            this.commandBarItem22.Hint = "删除水平间距";
            this.commandBarItem22.Id = 25;
            this.commandBarItem22.ImageIndex = 31;
            this.commandBarItem22.Name = "commandBarItem22";
            // 
            // commandBarItem23
            // 
            this.commandBarItem23.Caption = "垂直等距(&E)";
            this.commandBarItem23.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.VertSpaceMakeEqual;
            this.commandBarItem23.Enabled = false;
            this.commandBarItem23.Hint = "垂直等距";
            this.commandBarItem23.Id = 26;
            this.commandBarItem23.ImageIndex = 32;
            this.commandBarItem23.Name = "commandBarItem23";
            // 
            // commandBarItem25
            // 
            this.commandBarItem25.Caption = "&Decrease";
            this.commandBarItem25.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.VertSpaceDecrease;
            this.commandBarItem25.Enabled = false;
            this.commandBarItem25.Hint = "Decrease the spacing between the selected controls";
            this.commandBarItem25.Id = 28;
            this.commandBarItem25.ImageIndex = 34;
            this.commandBarItem25.Name = "commandBarItem25";
            // 
            // commandBarItem26
            // 
            this.commandBarItem26.Caption = "减少垂直间距(&R)";
            this.commandBarItem26.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.VertSpaceConcatenate;
            this.commandBarItem26.Enabled = false;
            this.commandBarItem26.Hint = "减少垂直间距";
            this.commandBarItem26.Id = 29;
            this.commandBarItem26.ImageIndex = 35;
            this.commandBarItem26.Name = "commandBarItem26";
            // 
            // commandBarItem27
            // 
            this.commandBarItem27.Caption = "水平居中(&H)";
            this.commandBarItem27.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.CenterHorizontally;
            this.commandBarItem27.Enabled = false;
            this.commandBarItem27.Hint = "水平居中";
            this.commandBarItem27.Id = 30;
            this.commandBarItem27.ImageIndex = 36;
            this.commandBarItem27.Name = "commandBarItem27";
            // 
            // commandBarItem28
            // 
            this.commandBarItem28.Caption = "水平居中(&V)";
            this.commandBarItem28.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.CenterVertically;
            this.commandBarItem28.Enabled = false;
            this.commandBarItem28.Hint = "水平居中";
            this.commandBarItem28.Id = 31;
            this.commandBarItem28.ImageIndex = 37;
            this.commandBarItem28.Name = "commandBarItem28";
            // 
            // commandBarItem29
            // 
            this.commandBarItem29.Caption = "前端显示(&B)";
            this.commandBarItem29.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.BringToFront;
            this.commandBarItem29.Enabled = false;
            this.commandBarItem29.Hint = "前端显示";
            this.commandBarItem29.Id = 32;
            this.commandBarItem29.ImageIndex = 38;
            this.commandBarItem29.Name = "commandBarItem29";
            // 
            // commandBarItem30
            // 
            this.commandBarItem30.Caption = "后端隐藏(&S)";
            this.commandBarItem30.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SendToBack;
            this.commandBarItem30.Enabled = false;
            this.commandBarItem30.Hint = "后端隐藏";
            this.commandBarItem30.Id = 33;
            this.commandBarItem30.ImageIndex = 39;
            this.commandBarItem30.Name = "commandBarItem30";
            // 
            // designBar5
            // 
            this.designBar5.BarName = "StatusBar";
            this.designBar5.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.designBar5.DockCol = 0;
            this.designBar5.DockRow = 0;
            this.designBar5.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.designBar5.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1)});
            this.designBar5.OptionsBar.AllowQuickCustomization = false;
            this.designBar5.OptionsBar.DrawDragBorder = false;
            this.designBar5.OptionsBar.UseWholeRow = true;
            this.designBar5.Text = "Status Bar";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring;
            this.barStaticItem1.Id = 42;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            this.barStaticItem1.Width = 32;
            // 
            // designBar6
            // 
            this.designBar6.BarName = "Zoom Bar";
            this.designBar6.DockCol = 1;
            this.designBar6.DockRow = 2;
            this.designBar6.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.designBar6.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem44),
            new DevExpress.XtraBars.LinkPersistInfo(this.xrZoomBarEditItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem45)});
            this.designBar6.Offset = 594;
            this.designBar6.Text = "Zoom Bar";
            // 
            // commandBarItem44
            // 
            this.commandBarItem44.Caption = "缩小";
            this.commandBarItem44.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.ZoomOut;
            this.commandBarItem44.Enabled = false;
            this.commandBarItem44.Hint = "Zoom out the design surface";
            this.commandBarItem44.Id = 66;
            this.commandBarItem44.ImageIndex = 40;
            this.commandBarItem44.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Subtract));
            this.commandBarItem44.Name = "commandBarItem44";
            // 
            // xrZoomBarEditItem1
            // 
            this.xrZoomBarEditItem1.Caption = "缩放比率";
            this.xrZoomBarEditItem1.Edit = this.designRepositoryItemComboBox1;
            this.xrZoomBarEditItem1.Enabled = false;
            this.xrZoomBarEditItem1.Hint = "Select or input the zoom factor";
            this.xrZoomBarEditItem1.Id = 67;
            this.xrZoomBarEditItem1.Name = "xrZoomBarEditItem1";
            this.xrZoomBarEditItem1.Width = 70;
            // 
            // designRepositoryItemComboBox1
            // 
            this.designRepositoryItemComboBox1.AutoComplete = false;
            this.designRepositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.designRepositoryItemComboBox1.Name = "designRepositoryItemComboBox1";
            // 
            // commandBarItem45
            // 
            this.commandBarItem45.Caption = "放大";
            this.commandBarItem45.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.ZoomIn;
            this.commandBarItem45.Enabled = false;
            this.commandBarItem45.Hint = "Zoom in the design surface";
            this.commandBarItem45.Id = 68;
            this.commandBarItem45.ImageIndex = 41;
            this.commandBarItem45.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Add));
            this.commandBarItem45.Name = "commandBarItem45";
            // 
            // xrDesignDockManager1
            // 
            this.xrDesignDockManager1.Form = this;
            this.xrDesignDockManager1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("xrDesignDockManager1.ImageStream")));
            this.xrDesignDockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.panelContainer1,
            this.toolBoxDockPanel1});
            this.xrDesignDockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "System.Windows.Forms.StatusBar"});
            this.xrDesignDockManager1.XRDesignPanel = this.xrDesignPanel;
            // 
            // panelContainer1
            // 
            this.panelContainer1.Controls.Add(this.panelContainer2);
            this.panelContainer1.Controls.Add(this.propertyGridDockPanel1);
            this.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.panelContainer1.ID = new System.Guid("bafa93fd-70de-4353-a132-3f8543b20f17");
            this.panelContainer1.Location = new System.Drawing.Point(532, 80);
            this.panelContainer1.Name = "panelContainer1";
            this.panelContainer1.Size = new System.Drawing.Size(300, 464);
            // 
            // panelContainer2
            // 
            this.panelContainer2.ActiveChild = this.reportExplorerDockPanel1;
            this.panelContainer2.Controls.Add(this.reportExplorerDockPanel1);
            this.panelContainer2.Controls.Add(this.fieldListDockPanel1);
            this.panelContainer2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelContainer2.ID = new System.Guid("d90a9a1f-b677-42b9-8561-4a921c8cd7e5");
            this.panelContainer2.ImageIndex = 2;
            this.panelContainer2.Location = new System.Drawing.Point(0, 0);
            this.panelContainer2.Name = "panelContainer2";
            this.panelContainer2.Size = new System.Drawing.Size(300, 252);
            this.panelContainer2.Tabbed = true;
            // 
            // reportExplorerDockPanel1
            // 
            this.reportExplorerDockPanel1.Controls.Add(this.reportExplorerDockPanel1_Container);
            this.reportExplorerDockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.reportExplorerDockPanel1.ID = new System.Guid("fb3ec6cc-3b9b-4b9c-91cf-cff78c1edbf1");
            this.reportExplorerDockPanel1.ImageIndex = 2;
            this.reportExplorerDockPanel1.Location = new System.Drawing.Point(3, 29);
            this.reportExplorerDockPanel1.Name = "reportExplorerDockPanel1";
            this.reportExplorerDockPanel1.Size = new System.Drawing.Size(294, 198);
            this.reportExplorerDockPanel1.Text = "报表管理器";
            this.reportExplorerDockPanel1.XRDesignPanel = this.xrDesignPanel;
            // 
            // reportExplorerDockPanel1_Container
            // 
            this.reportExplorerDockPanel1_Container.Location = new System.Drawing.Point(0, 0);
            this.reportExplorerDockPanel1_Container.Name = "reportExplorerDockPanel1_Container";
            this.reportExplorerDockPanel1_Container.Size = new System.Drawing.Size(294, 198);
            this.reportExplorerDockPanel1_Container.TabIndex = 0;
            // 
            // fieldListDockPanel1
            // 
            this.fieldListDockPanel1.Controls.Add(this.fieldListDockPanel1_Container);
            this.fieldListDockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.fieldListDockPanel1.ID = new System.Guid("faf69838-a93f-4114-83e8-d0d09cc5ce95");
            this.fieldListDockPanel1.ImageIndex = 0;
            this.fieldListDockPanel1.Location = new System.Drawing.Point(3, 29);
            this.fieldListDockPanel1.Name = "fieldListDockPanel1";
            this.fieldListDockPanel1.Size = new System.Drawing.Size(294, 198);
            this.fieldListDockPanel1.Text = "字段列表";
            this.fieldListDockPanel1.XRDesignPanel = this.xrDesignPanel;
            // 
            // fieldListDockPanel1_Container
            // 
            this.fieldListDockPanel1_Container.Location = new System.Drawing.Point(0, 0);
            this.fieldListDockPanel1_Container.Name = "fieldListDockPanel1_Container";
            this.fieldListDockPanel1_Container.Size = new System.Drawing.Size(294, 198);
            this.fieldListDockPanel1_Container.TabIndex = 0;
            // 
            // propertyGridDockPanel1
            // 
            this.propertyGridDockPanel1.Controls.Add(this.propertyGridDockPanel1_Container);
            this.propertyGridDockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.propertyGridDockPanel1.ID = new System.Guid("b38d12c3-cd06-4dec-b93d-63a0088e495a");
            this.propertyGridDockPanel1.ImageIndex = 1;
            this.propertyGridDockPanel1.Location = new System.Drawing.Point(0, 252);
            this.propertyGridDockPanel1.Name = "propertyGridDockPanel1";
            this.propertyGridDockPanel1.Size = new System.Drawing.Size(300, 212);
            this.propertyGridDockPanel1.Text = "属性栏";
            this.propertyGridDockPanel1.XRDesignPanel = this.xrDesignPanel;
            // 
            // propertyGridDockPanel1_Container
            // 
            this.propertyGridDockPanel1_Container.Location = new System.Drawing.Point(3, 29);
            this.propertyGridDockPanel1_Container.Name = "propertyGridDockPanel1_Container";
            this.propertyGridDockPanel1_Container.Size = new System.Drawing.Size(294, 180);
            this.propertyGridDockPanel1_Container.TabIndex = 0;
            // 
            // toolBoxDockPanel1
            // 
            this.toolBoxDockPanel1.Controls.Add(this.toolBoxDockPanel1_Container);
            this.toolBoxDockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.toolBoxDockPanel1.ID = new System.Guid("161a5a1a-d9b9-4f06-9ac4-d0c3e507c54f");
            this.toolBoxDockPanel1.ImageIndex = 3;
            this.toolBoxDockPanel1.Location = new System.Drawing.Point(0, 80);
            this.toolBoxDockPanel1.Name = "toolBoxDockPanel1";
            this.toolBoxDockPanel1.Size = new System.Drawing.Size(198, 464);
            this.toolBoxDockPanel1.Text = "工具栏";
            this.toolBoxDockPanel1.XRDesignPanel = this.xrDesignPanel;
            // 
            // toolBoxDockPanel1_Container
            // 
            this.toolBoxDockPanel1_Container.Location = new System.Drawing.Point(3, 29);
            this.toolBoxDockPanel1_Container.Name = "toolBoxDockPanel1_Container";
            this.toolBoxDockPanel1_Container.Size = new System.Drawing.Size(192, 432);
            this.toolBoxDockPanel1_Container.TabIndex = 0;
            // 
            // commandBarItem13
            // 
            this.commandBarItem13.Caption = "&Middles";
            this.commandBarItem13.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignHorizontalCenters;
            this.commandBarItem13.Enabled = false;
            this.commandBarItem13.Hint = "Align the centers of the selected controls horizontally";
            this.commandBarItem13.Id = 16;
            this.commandBarItem13.ImageIndex = 22;
            this.commandBarItem13.Name = "commandBarItem13";
            // 
            // commandBarItem24
            // 
            this.commandBarItem24.Caption = "增加垂直间距(&I)";
            this.commandBarItem24.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.VertSpaceIncrease;
            this.commandBarItem24.Enabled = false;
            this.commandBarItem24.Hint = "增加垂直间距";
            this.commandBarItem24.Id = 27;
            this.commandBarItem24.ImageIndex = 33;
            this.commandBarItem24.Name = "commandBarItem24";
            // 
            // barSubItem5
            // 
            this.barSubItem5.Caption = "其他窗体(&W)";
            this.barSubItem5.Id = 49;
            this.barSubItem5.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barDockPanelsListItem1)});
            this.barSubItem5.Name = "barSubItem5";
            // 
            // barDockPanelsListItem1
            // 
            this.barDockPanelsListItem1.Caption = "其他窗体(&W)";
            this.barDockPanelsListItem1.Id = 50;
            this.barDockPanelsListItem1.Name = "barDockPanelsListItem1";
            this.barDockPanelsListItem1.ShowCustomizationItem = false;
            this.barDockPanelsListItem1.ShowDockPanels = true;
            this.barDockPanelsListItem1.ShowToolbars = false;
            // 
            // barSubItem6
            // 
            this.barSubItem6.Caption = "Fo&rmat";
            this.barSubItem6.Id = 51;
            this.barSubItem6.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandColorBarItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandColorBarItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem7, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem8),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem9, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem10),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem11, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem12),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem13, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem14, true)});
            this.barSubItem6.Name = "barSubItem6";
            // 
            // barSubItem7
            // 
            this.barSubItem7.Caption = "&Font";
            this.barSubItem7.Id = 52;
            this.barSubItem7.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem1, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem3)});
            this.barSubItem7.Name = "barSubItem7";
            // 
            // barSubItem8
            // 
            this.barSubItem8.Caption = "&Justify";
            this.barSubItem8.Id = 53;
            this.barSubItem8.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem4, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem5),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem6),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem7)});
            this.barSubItem8.Name = "barSubItem8";
            // 
            // barSubItem9
            // 
            this.barSubItem9.Caption = "&Align";
            this.barSubItem9.Id = 54;
            this.barSubItem9.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem9, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem10),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem11),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem12, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem13),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem14),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem8, true)});
            this.barSubItem9.Name = "barSubItem9";
            // 
            // barSubItem10
            // 
            this.barSubItem10.Caption = "&Make Same Size";
            this.barSubItem10.Id = 55;
            this.barSubItem10.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem15, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem16),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem17),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem18)});
            this.barSubItem10.Name = "barSubItem10";
            // 
            // barSubItem11
            // 
            this.barSubItem11.Caption = "&Horizontal Spacing";
            this.barSubItem11.Id = 56;
            this.barSubItem11.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem19, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem20),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem21),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem22)});
            this.barSubItem11.Name = "barSubItem11";
            // 
            // barSubItem12
            // 
            this.barSubItem12.Caption = "&Vertical Spacing";
            this.barSubItem12.Id = 57;
            this.barSubItem12.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem23, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem24),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem25),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem26)});
            this.barSubItem12.Name = "barSubItem12";
            // 
            // barSubItem13
            // 
            this.barSubItem13.Caption = "&Center in Form";
            this.barSubItem13.Id = 58;
            this.barSubItem13.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem27, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem28)});
            this.barSubItem13.Name = "barSubItem13";
            // 
            // barSubItem14
            // 
            this.barSubItem14.Caption = "&Order";
            this.barSubItem14.Id = 59;
            this.barSubItem14.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem29, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.commandBarItem30)});
            this.barSubItem14.Name = "barSubItem14";
            // 
            // bsiLookAndFeel
            // 
            this.bsiLookAndFeel.Caption = "&Look and Feel";
            this.bsiLookAndFeel.Id = 65;
            this.bsiLookAndFeel.Name = "bsiLookAndFeel";
            // 
            // CustomDesignForm
            // 
            this.ClientSize = new System.Drawing.Size(832, 571);
            this.Controls.Add(this.panelContainer1);
            this.Controls.Add(this.toolBoxDockPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MinimumSize = new System.Drawing.Size(420, 214);
            this.Name = "CustomDesignForm";
            this.Text = "报表设计器";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.CustomDesignForm_Load);
            this.Controls.SetChildIndex(this.barDockControlTop, 0);
            this.Controls.SetChildIndex(this.barDockControlBottom, 0);
            this.Controls.SetChildIndex(this.barDockControlRight, 0);
            this.Controls.SetChildIndex(this.barDockControlLeft, 0);
            this.Controls.SetChildIndex(this.toolBoxDockPanel1, 0);
            this.Controls.SetChildIndex(this.panelContainer1, 0);
            this.Controls.SetChildIndex(this.xrDesignPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.xrDesignPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrDesignBarManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricbFontName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricbFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.designRepositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrDesignDockManager1)).EndInit();
            this.panelContainer1.ResumeLayout(false);
            this.panelContainer2.ResumeLayout(false);
            this.reportExplorerDockPanel1.ResumeLayout(false);
            this.fieldListDockPanel1.ResumeLayout(false);
            this.propertyGridDockPanel1.ResumeLayout(false);
            this.toolBoxDockPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        public CustomDesignForm()
        {
            InitializeComponent();
            BarItem item = new XtraReportsDemos.BarLookAndFeelListItem(DevExpress.LookAndFeel.UserLookAndFeel.Default);
            xrDesignBarManager1.Items.Add(item);
            bsiLookAndFeel.AddItem(item);
        }

        protected override void SaveLayout() { }
        protected override void RestoreLayout() { }
        //[STAThread]
        //public static void Main()
        //{
        //    Application.Run(new CustomDesignForm());
        //}

        private DataTable m_ReportDataSoruce = new DataTable();
        private string m_Path = string.Empty;
        private delegate void DeClose();
        private XReport _reportUtil;

        /// <summary>
        /// 是否跳过  FormDataSoruce 页面
        /// 是：由其他程序调用当前配置页面
        /// 否:由主架框架调用当前页面需要执行sql语句
        /// </summary>
        public bool SkipQueryData
        {
            get { return _skipQueryData; }
            set { _skipQueryData = value; }
        }
        private bool _skipQueryData = false;

        

        
        private void CustomDesignForm_Load(object sender, EventArgs e)
        {
            if (!SkipQueryData)
                NeedQueryData();
        }

        private void NeedQueryData()
        {

            FormDataSoruce formSoruce = new FormDataSoruce();
            if (formSoruce.ShowDialog() == DialogResult.OK)
            {
                m_ReportDataSoruce = formSoruce.ReportDataSoruce.Copy();
                m_Path = formSoruce.Path;
                this.WindowState = FormWindowState.Maximized;
                DataSet ds = new DataSet();
                ds.Tables.Add(m_ReportDataSoruce);
                _reportUtil = new XReport(ds, m_Path);
                OpenReport(_reportUtil.CurrentReport);
                //ReportDesign rp = new ReportDesign(m_Path, ds, this);
            }
            else
            {
                this.BeginInvoke(new DeClose(this.Close));
            }
        }





        void CustomDesignForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.FormClosed -= new FormClosedEventHandler(CustomDesignForm_FormClosed);
            this.Dispose();
        }
        //protected override void OnVisibleChanged(EventArgs e)
        //{
        //    //base.OnVisibleChanged(e);
        //    //if (!this.Visible)
        //    //    this.Close();
        //}

        #region IStartup 成员

        /// <summary>
        /// 启动类
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public IPlugIn Run(IYidanEmrHost application)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            CustomDesignForm customDesignForm = new CustomDesignForm();
            PlugIn plg = new PlugIn(this.GetType().ToString(), customDesignForm);
            return plg;
        }
        #endregion



    }
}
