using DrectSoft.Core.DoctorAdvice;
namespace DrectSoft.Core.OrderManage
{
   partial class SuiteToolBox
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
          this.panelContainer = new DevExpress.XtraEditors.PanelControl();
          this.panelSuite = new DevExpress.XtraEditors.PanelControl();
          this.m_SuiteChoice = new SuiteChoice();
          ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).BeginInit();
          this.panelContainer.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.panelSuite)).BeginInit();
          this.panelSuite.SuspendLayout();
          this.SuspendLayout();
          // 
          // panelContainer
          // 
          this.panelContainer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
          this.panelContainer.Controls.Add(this.panelSuite);
          this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
          this.panelContainer.Location = new System.Drawing.Point(0, 0);
          this.panelContainer.LookAndFeel.SkinName = "Blue";
          this.panelContainer.LookAndFeel.UseDefaultLookAndFeel = true;
          this.panelContainer.Name = "panelContainer";
          this.panelContainer.Size = new System.Drawing.Size(195, 578);
          this.panelContainer.TabIndex = 0;
          // 
          // panelSuite
          // 
          this.panelSuite.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
          this.panelSuite.Controls.Add(this.m_SuiteChoice);
          this.panelSuite.Dock = System.Windows.Forms.DockStyle.Fill;
          this.panelSuite.Location = new System.Drawing.Point(0, 0);
          this.panelSuite.LookAndFeel.SkinName = "Blue";
          this.panelSuite.LookAndFeel.UseDefaultLookAndFeel = true;
          this.panelSuite.Name = "panelSuite";
          this.panelSuite.Size = new System.Drawing.Size(195, 578);
          this.panelSuite.TabIndex = 1;
          // 
          // m_SuiteChoice
          // 
          this.m_SuiteChoice.App = null;
          this.m_SuiteChoice.Dock = System.Windows.Forms.DockStyle.Fill;
          this.m_SuiteChoice.Editable = false;
          this.m_SuiteChoice.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
          this.m_SuiteChoice.Location = new System.Drawing.Point(0, 0);
          this.m_SuiteChoice.Name = "m_SuiteChoice";
          this.m_SuiteChoice.Size = new System.Drawing.Size(195, 578);
          this.m_SuiteChoice.TabIndex = 0;
          // 
          // SuiteToolBox
          // 
          this.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
          this.Appearance.Options.UseFont = true;
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(195, 578);
          this.Controls.Add(this.panelContainer);
          this.LookAndFeel.SkinName = "Blue";
          this.LookAndFeel.UseDefaultLookAndFeel = true;
          this.Name = "SuiteToolBox";
          this.ShowIcon = false;
          this.ShowInTaskbar = false;
          this.Text = "成套工具箱";
          ((System.ComponentModel.ISupportInitialize)(this.panelContainer)).EndInit();
          this.panelContainer.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.panelSuite)).EndInit();
          this.panelSuite.ResumeLayout(false);
          this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelContainer;
      private DevExpress.XtraEditors.PanelControl panelSuite;
      private SuiteChoice m_SuiteChoice;



   }
}