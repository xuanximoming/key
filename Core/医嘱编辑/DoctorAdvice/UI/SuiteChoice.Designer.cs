namespace DrectSoft.Core.DoctorAdvice
{
   partial class SuiteChoice
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
            this.gridCtrlSuit = new DevExpress.XtraGrid.GridControl();
            this.gridViewSuit = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColSuitName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerialNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.popupMenuSuite = new DevExpress.XtraBars.PopupMenu();
            this.barItemAdd = new DevExpress.XtraBars.BarButtonItem();
            this.baItemModify = new DevExpress.XtraBars.BarButtonItem();
            this.barItemDelete = new DevExpress.XtraBars.BarButtonItem();
            this.suitebarManager = new DevExpress.XtraBars.BarManager();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.navCtrlOrderSuit = new DevExpress.XtraNavBar.NavBarControl();
            this.navGroupPersonal = new DevExpress.XtraNavBar.NavBarGroup();
            this.navGroupPersonalContainer = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.navGroupDeptContainer = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.navGroupHospitalContainer = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.navGroupDept = new DevExpress.XtraNavBar.NavBarGroup();
            this.navGroupHospital = new DevExpress.XtraNavBar.NavBarGroup();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlSuit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSuit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuSuite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.suitebarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navCtrlOrderSuit)).BeginInit();
            this.navCtrlOrderSuit.SuspendLayout();
            this.navGroupPersonalContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridCtrlSuit
            // 
            this.gridCtrlSuit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCtrlSuit.Location = new System.Drawing.Point(0, 0);
            this.gridCtrlSuit.LookAndFeel.SkinName = "Blue";
            this.gridCtrlSuit.MainView = this.gridViewSuit;
            this.gridCtrlSuit.Name = "gridCtrlSuit";
            this.gridCtrlSuit.Size = new System.Drawing.Size(181, 73);
            this.gridCtrlSuit.TabIndex = 0;
            this.gridCtrlSuit.TabStop = false;
            this.gridCtrlSuit.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewSuit});
            this.gridCtrlSuit.DoubleClick += new System.EventHandler(this.gridCtrlSuit_DoubleClick);
            // 
            // gridViewSuit
            // 
            this.gridViewSuit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gridViewSuit.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColSuitName,
            this.gridColSerialNo});
            this.gridViewSuit.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewSuit.GridControl = this.gridCtrlSuit;
            this.gridViewSuit.Name = "gridViewSuit";
            this.gridViewSuit.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewSuit.OptionsBehavior.AutoPopulateColumns = false;
            this.gridViewSuit.OptionsBehavior.Editable = false;
            this.gridViewSuit.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewSuit.OptionsCustomization.AllowFilter = false;
            this.gridViewSuit.OptionsCustomization.AllowGroup = false;
            this.gridViewSuit.OptionsDetail.EnableMasterViewMode = false;
            this.gridViewSuit.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewSuit.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewSuit.OptionsMenu.EnableColumnMenu = false;
            this.gridViewSuit.OptionsMenu.EnableFooterMenu = false;
            this.gridViewSuit.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewSuit.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewSuit.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewSuit.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewSuit.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewSuit.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridViewSuit.OptionsView.ShowColumnHeaders = false;
            this.gridViewSuit.OptionsView.ShowDetailButtons = false;
            this.gridViewSuit.OptionsView.ShowGroupPanel = false;
            this.gridViewSuit.OptionsView.ShowIndicator = false;
            this.gridViewSuit.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewSuit_FocusedRowChanged);
            // 
            // gridColSuitName
            // 
            this.gridColSuitName.Caption = "成套名称";
            this.gridColSuitName.FieldName = "Name";
            this.gridColSuitName.Name = "gridColSuitName";
            this.gridColSuitName.Visible = true;
            this.gridColSuitName.VisibleIndex = 0;
            // 
            // gridColSerialNo
            // 
            this.gridColSerialNo.Caption = "序号";
            this.gridColSerialNo.FieldName = "GroupID";
            this.gridColSerialNo.Name = "gridColSerialNo";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // popupMenuSuite
            // 
            this.popupMenuSuite.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemAdd),
            new DevExpress.XtraBars.LinkPersistInfo(this.baItemModify),
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemDelete)});
            this.popupMenuSuite.Manager = this.suitebarManager;
            this.popupMenuSuite.Name = "popupMenuSuite";
            // 
            // barItemAdd
            // 
            this.barItemAdd.Caption = "添加";
            this.barItemAdd.Id = 0;
            this.barItemAdd.Name = "barItemAdd";
            this.barItemAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItemAdd_ItemClick);
            // 
            // baItemModify
            // 
            this.baItemModify.Caption = "修改";
            this.baItemModify.Id = 1;
            this.baItemModify.Name = "baItemModify";
            this.baItemModify.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.baItemModify_ItemClick);
            // 
            // barItemDelete
            // 
            this.barItemDelete.Caption = "删除";
            this.barItemDelete.Id = 2;
            this.barItemDelete.Name = "barItemDelete";
            this.barItemDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItemDelete_ItemClick);
            // 
            // suitebarManager
            // 
            this.suitebarManager.Controller = this.barAndDockingController1;
            this.suitebarManager.DockControls.Add(this.barDockControlTop);
            this.suitebarManager.DockControls.Add(this.barDockControlBottom);
            this.suitebarManager.DockControls.Add(this.barDockControlLeft);
            this.suitebarManager.DockControls.Add(this.barDockControlRight);
            this.suitebarManager.Form = this;
            this.suitebarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barItemAdd,
            this.baItemModify,
            this.barItemDelete});
            this.suitebarManager.MaxItemId = 3;
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.PaintStyleName = "Skin";
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(189, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 721);
            this.barDockControlBottom.Size = new System.Drawing.Size(189, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 721);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(189, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 721);
            // 
            // navCtrlOrderSuit
            // 
            this.navCtrlOrderSuit.ActiveGroup = this.navGroupPersonal;
            this.navCtrlOrderSuit.Controls.Add(this.navGroupPersonalContainer);
            this.navCtrlOrderSuit.Controls.Add(this.navGroupDeptContainer);
            this.navCtrlOrderSuit.Controls.Add(this.navGroupHospitalContainer);
            this.navCtrlOrderSuit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navCtrlOrderSuit.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navGroupPersonal,
            this.navGroupDept,
            this.navGroupHospital});
            this.navCtrlOrderSuit.Location = new System.Drawing.Point(0, 0);
            this.navCtrlOrderSuit.Name = "navCtrlOrderSuit";
            this.navCtrlOrderSuit.OptionsNavPane.ExpandedWidth = 189;
            this.navCtrlOrderSuit.Size = new System.Drawing.Size(189, 721);
            this.navCtrlOrderSuit.TabIndex = 0;
            this.navCtrlOrderSuit.Text = "navBarControl1";
            // 
            // navGroupPersonal
            // 
            this.navGroupPersonal.Caption = "个人模板";
            this.navGroupPersonal.ControlContainer = this.navGroupPersonalContainer;
            this.navGroupPersonal.Expanded = true;
            this.navGroupPersonal.GroupClientHeight = 80;
            this.navGroupPersonal.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navGroupPersonal.Name = "navGroupPersonal";
            // 
            // navGroupPersonalContainer
            // 
            this.navGroupPersonalContainer.Controls.Add(this.gridCtrlSuit);
            this.navGroupPersonalContainer.Name = "navGroupPersonalContainer";
            this.navGroupPersonalContainer.Size = new System.Drawing.Size(181, 73);
            this.navGroupPersonalContainer.TabIndex = 0;
            // 
            // navGroupDeptContainer
            // 
            this.navGroupDeptContainer.Name = "navGroupDeptContainer";
            this.navGroupDeptContainer.Size = new System.Drawing.Size(181, 73);
            this.navGroupDeptContainer.TabIndex = 0;
            // 
            // navGroupHospitalContainer
            // 
            this.navGroupHospitalContainer.Name = "navGroupHospitalContainer";
            this.navGroupHospitalContainer.Size = new System.Drawing.Size(181, 73);
            this.navGroupHospitalContainer.TabIndex = 1;
            // 
            // navGroupDept
            // 
            this.navGroupDept.Caption = "科室模板";
            this.navGroupDept.ControlContainer = this.navGroupDeptContainer;
            this.navGroupDept.Expanded = true;
            this.navGroupDept.GroupClientHeight = 80;
            this.navGroupDept.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navGroupDept.Name = "navGroupDept";
            // 
            // navGroupHospital
            // 
            this.navGroupHospital.Caption = "全院模板";
            this.navGroupHospital.ControlContainer = this.navGroupHospitalContainer;
            this.navGroupHospital.Expanded = true;
            this.navGroupHospital.GroupClientHeight = 80;
            this.navGroupHospital.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navGroupHospital.Name = "navGroupHospital";
            // 
            // SuiteChoice
            // 
            this.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Appearance.Options.UseFont = true;
            this.Controls.Add(this.navCtrlOrderSuit);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "SuiteChoice";
            this.Size = new System.Drawing.Size(189, 721);
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlSuit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSuit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuSuite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.suitebarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navCtrlOrderSuit)).EndInit();
            this.navCtrlOrderSuit.ResumeLayout(false);
            this.navGroupPersonalContainer.ResumeLayout(false);
            this.ResumeLayout(false);

      }

      #endregion


      private System.Windows.Forms.ImageList imageList1;
      private DevExpress.XtraBars.PopupMenu popupMenuSuite;
      private DevExpress.XtraBars.BarManager suitebarManager;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      private DevExpress.XtraBars.BarButtonItem barItemAdd;
      private DevExpress.XtraBars.BarButtonItem baItemModify;
      private DevExpress.XtraBars.BarButtonItem barItemDelete;
      private DevExpress.XtraGrid.GridControl gridCtrlSuit;
      private DevExpress.XtraGrid.Views.Grid.GridView gridViewSuit;
      private DevExpress.XtraGrid.Columns.GridColumn gridColSuitName;
      private DevExpress.XtraGrid.Columns.GridColumn gridColSerialNo;
      private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
      private DevExpress.XtraNavBar.NavBarControl navCtrlOrderSuit;
      private DevExpress.XtraNavBar.NavBarGroup navGroupPersonal;
      private DevExpress.XtraNavBar.NavBarGroupControlContainer navGroupPersonalContainer;
      private DevExpress.XtraNavBar.NavBarGroupControlContainer navGroupDeptContainer;
      private DevExpress.XtraNavBar.NavBarGroupControlContainer navGroupHospitalContainer;
      private DevExpress.XtraNavBar.NavBarGroup navGroupDept;
      private DevExpress.XtraNavBar.NavBarGroup navGroupHospital;

   }
}