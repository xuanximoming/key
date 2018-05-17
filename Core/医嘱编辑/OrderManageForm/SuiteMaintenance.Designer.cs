namespace DrectSoft.Core.OrderManage
{
   partial class SuiteMaintenance
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
          this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
          this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
          this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
          this.propertyGridSuiteInfo = new System.Windows.Forms.PropertyGrid();
          this.dockPanel2 = new DevExpress.XtraBars.Docking.DockPanel();
          this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
          this.hideContainerLeft = new DevExpress.XtraBars.Docking.AutoHideContainer();
          this.hideContainerRight = new DevExpress.XtraBars.Docking.AutoHideContainer();
          ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
          this.dockPanel1.SuspendLayout();
          this.dockPanel1_Container.SuspendLayout();
          this.dockPanel2.SuspendLayout();
          this.hideContainerLeft.SuspendLayout();
          this.hideContainerRight.SuspendLayout();
          this.SuspendLayout();
          // 
          // dockManager1
          // 
          this.dockManager1.AutoHideContainers.AddRange(new DevExpress.XtraBars.Docking.AutoHideContainer[] {
            this.hideContainerLeft,
            this.hideContainerRight});
          this.dockManager1.Form = this;
          this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
          // 
          // dockPanel1
          // 
          this.dockPanel1.Controls.Add(this.dockPanel1_Container);
          this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
          this.dockPanel1.ID = new System.Guid("8b70131f-2718-4a85-966c-07ba93f10626");
          this.dockPanel1.Location = new System.Drawing.Point(0, 0);
          this.dockPanel1.Name = "dockPanel1";
          this.dockPanel1.Options.ShowCloseButton = false;
          this.dockPanel1.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Right;
          this.dockPanel1.SavedIndex = 0;
          this.dockPanel1.Size = new System.Drawing.Size(196, 458);
          this.dockPanel1.Tag = "";
          this.dockPanel1.Text = "医嘱工具箱";
          this.dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
          // 
          // dockPanel1_Container
          // 
          this.dockPanel1_Container.Controls.Add(this.propertyGridSuiteInfo);
          this.dockPanel1_Container.Location = new System.Drawing.Point(3, 25);
          this.dockPanel1_Container.Name = "dockPanel1_Container";
          this.dockPanel1_Container.Size = new System.Drawing.Size(190, 430);
          this.dockPanel1_Container.TabIndex = 0;
          // 
          // propertyGridSuiteInfo
          // 
          this.propertyGridSuiteInfo.Dock = System.Windows.Forms.DockStyle.Fill;
          this.propertyGridSuiteInfo.HelpVisible = false;
          this.propertyGridSuiteInfo.Location = new System.Drawing.Point(0, 0);
          this.propertyGridSuiteInfo.MinimumSize = new System.Drawing.Size(183, 400);
          this.propertyGridSuiteInfo.Name = "propertyGridSuiteInfo";
          this.propertyGridSuiteInfo.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
          this.propertyGridSuiteInfo.Size = new System.Drawing.Size(190, 430);
          this.propertyGridSuiteInfo.TabIndex = 2;
          this.propertyGridSuiteInfo.Tag = "医嘱工具箱";
          // 
          // dockPanel2
          // 
          this.dockPanel2.Controls.Add(this.dockPanel2_Container);
          this.dockPanel2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
          this.dockPanel2.ID = new System.Guid("7960c990-8569-4a2e-bc2c-ee1faf17de22");
          this.dockPanel2.Location = new System.Drawing.Point(0, 0);
          this.dockPanel2.Name = "dockPanel2";
          this.dockPanel2.Options.ShowCloseButton = false;
          this.dockPanel2.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Left;
          this.dockPanel2.SavedIndex = 1;
          this.dockPanel2.Size = new System.Drawing.Size(200, 458);
          this.dockPanel2.Text = "成套医嘱列表";
          this.dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
          // 
          // dockPanel2_Container
          // 
          this.dockPanel2_Container.Location = new System.Drawing.Point(3, 25);
          this.dockPanel2_Container.Name = "dockPanel2_Container";
          this.dockPanel2_Container.Size = new System.Drawing.Size(194, 430);
          this.dockPanel2_Container.TabIndex = 0;
          // 
          // hideContainerLeft
          // 
          this.hideContainerLeft.Controls.Add(this.dockPanel2);
          this.hideContainerLeft.Dock = System.Windows.Forms.DockStyle.Left;
          this.hideContainerLeft.Location = new System.Drawing.Point(0, 0);
          this.hideContainerLeft.Name = "hideContainerLeft";
          this.hideContainerLeft.Size = new System.Drawing.Size(20, 458);
          // 
          // hideContainerRight
          // 
          this.hideContainerRight.Controls.Add(this.dockPanel1);
          this.hideContainerRight.Dock = System.Windows.Forms.DockStyle.Right;
          this.hideContainerRight.Location = new System.Drawing.Point(649, 0);
          this.hideContainerRight.Name = "hideContainerRight";
          this.hideContainerRight.Size = new System.Drawing.Size(20, 458);
          // 
          // SuiteMaintenance
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(669, 458);
          this.Controls.Add(this.hideContainerLeft);
          this.Controls.Add(this.hideContainerRight);
          this.Name = "SuiteMaintenance";
          this.ShowIcon = false;
          this.ShowInTaskbar = false;
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
          this.Text = "成套医嘱维护";
          this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
          ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
          this.dockPanel1.ResumeLayout(false);
          this.dockPanel1_Container.ResumeLayout(false);
          this.dockPanel2.ResumeLayout(false);
          this.hideContainerLeft.ResumeLayout(false);
          this.hideContainerRight.ResumeLayout(false);
          this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.Docking.DockManager dockManager1;
      private System.Windows.Forms.PropertyGrid propertyGridSuiteInfo;
      private DevExpress.XtraBars.Docking.DockPanel dockPanel2;
      private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
      private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
      private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
      private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerLeft;
      private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerRight;

   }
}