namespace DrectSoft.Core.OrderManage
{
   partial class AdviceInputor
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
            this.panelEditContent = new DevExpress.XtraEditors.PanelControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager();
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.m_SuiteChoice = new DrectSoft.Core.DoctorAdvice.SuiteChoice();
            ((System.ComponentModel.ISupportInitialize)(this.panelEditContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEditContent
            // 
            this.panelEditContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditContent.Location = new System.Drawing.Point(0, 0);
            this.panelEditContent.LookAndFeel.SkinName = "Blue";
            this.panelEditContent.Name = "panelEditContent";
            this.panelEditContent.Size = new System.Drawing.Size(229, 305);
            this.panelEditContent.TabIndex = 0;
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
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
            this.dockPanel1.ID = new System.Guid("00cecb73-ac93-4e9f-a269-d1da489d0fe9");
            this.dockPanel1.Location = new System.Drawing.Point(229, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanel1.Size = new System.Drawing.Size(200, 305);
            this.dockPanel1.TabStop = false;
            this.dockPanel1.Text = "病历工具箱";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.m_SuiteChoice);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(192, 278);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // m_SuiteChoice
            // 
            this.m_SuiteChoice.App = null;
            this.m_SuiteChoice.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.m_SuiteChoice.Appearance.Options.UseFont = true;
            this.m_SuiteChoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_SuiteChoice.Editable = false;
            this.m_SuiteChoice.Location = new System.Drawing.Point(0, 0);
            this.m_SuiteChoice.LookAndFeel.SkinName = "Blue";
            this.m_SuiteChoice.Name = "m_SuiteChoice";
            this.m_SuiteChoice.Size = new System.Drawing.Size(192, 278);
            this.m_SuiteChoice.TabIndex = 0;
            this.m_SuiteChoice.TabStop = false;
            // 
            // AdviceInputor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(429, 305);
            this.Controls.Add(this.panelEditContent);
            this.Controls.Add(this.dockPanel1);
            this.KeyPreview = true;
            this.Name = "AdviceInputor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "医嘱录入";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.AdviceInputor_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AdviceInputor_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.panelEditContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelEditContent;
      private DevExpress.XtraBars.Docking.DockManager dockManager1;
      private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
      private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
      private DrectSoft.Core.DoctorAdvice.SuiteChoice m_SuiteChoice;


   }
}