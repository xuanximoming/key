namespace DrectSoft.Core.DoctorAdvice
{
   partial class SkinTestInputForm
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

      #region 组件设计器生成的代码

      /// <summary> 
      /// 设计器支持所需的方法 - 不要
      /// 使用代码编辑器修改此方法的内容。
      /// </summary>
      private void InitializeComponent()
      {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkinTestInputForm));
            this.groupControlTitle = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelSelect = new DevExpress.XtraEditors.PanelControl();
            this.labMessage = new System.Windows.Forms.Label();
            this.selResult = new DevExpress.XtraEditors.RadioGroup();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTitle)).BeginInit();
            this.groupControlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelSelect)).BeginInit();
            this.panelSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selResult.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControlTitle
            // 
            this.groupControlTitle.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControlTitle.AppearanceCaption.Options.UseFont = true;
            this.groupControlTitle.Controls.Add(this.panelControl1);
            this.groupControlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlTitle.Location = new System.Drawing.Point(0, 0);
            this.groupControlTitle.Name = "groupControlTitle";
            this.groupControlTitle.Size = new System.Drawing.Size(276, 139);
            this.groupControlTitle.TabIndex = 6;
            this.groupControlTitle.Text = "请选择皮试结果：";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelSelect);
            this.panelControl1.Controls.Add(this.btnOk);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(2, 25);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(272, 112);
            this.panelControl1.TabIndex = 5;
            // 
            // panelSelect
            // 
            this.panelSelect.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelSelect.Controls.Add(this.labMessage);
            this.panelSelect.Controls.Add(this.selResult);
            this.panelSelect.Controls.Add(this.label1);
            this.panelSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSelect.Location = new System.Drawing.Point(2, 2);
            this.panelSelect.Margin = new System.Windows.Forms.Padding(0);
            this.panelSelect.Name = "panelSelect";
            this.panelSelect.Size = new System.Drawing.Size(268, 76);
            this.panelSelect.TabIndex = 4;
            // 
            // labMessage
            // 
            this.labMessage.AutoSize = true;
            this.labMessage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labMessage.Location = new System.Drawing.Point(3, 52);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(57, 12);
            this.labMessage.TabIndex = 5;
            this.labMessage.Text = "显示消息";
            this.labMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // selResult
            // 
            this.selResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selResult.EditValue = 0;
            this.selResult.Location = new System.Drawing.Point(0, 0);
            this.selResult.Margin = new System.Windows.Forms.Padding(4);
            this.selResult.Name = "selResult";
            this.selResult.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selResult.Properties.Appearance.Options.UseFont = true;
            this.selResult.Properties.Columns = 3;
            this.selResult.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "阳性"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "阴性"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "阴性(3天)")});
            this.selResult.Size = new System.Drawing.Size(269, 42);
            this.selResult.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(48, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.Image = global::DrectSoft.Core.DoctorAdvice.Properties.Resources.确定;
            this.btnOk.Location = new System.Drawing.Point(93, 82);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 27);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定 (&Y)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::DrectSoft.Core.DoctorAdvice.Properties.Resources.取消;
            this.btnCancel.Location = new System.Drawing.Point(181, 82);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 27);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消 (&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SkinTestInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 139);
            this.ControlBox = false;
            this.Controls.Add(this.groupControlTitle);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(282, 124);
            this.Name = "SkinTestInputForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = 1;
            this.Shown += new System.EventHandler(this.SkinTestInputForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTitle)).EndInit();
            this.groupControlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelSelect)).EndInit();
            this.panelSelect.ResumeLayout(false);
            this.panelSelect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selResult.Properties)).EndInit();
            this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.GroupControl groupControlTitle;
      private DevExpress.XtraEditors.PanelControl panelControl1;
      private System.Windows.Forms.Label labMessage;
      private DevExpress.XtraEditors.PanelControl panelSelect;
      private DevExpress.XtraEditors.RadioGroup selResult;
      private System.Windows.Forms.Label label1;
      private DevExpress.XtraEditors.SimpleButton btnOk;
      private DevExpress.XtraEditors.SimpleButton btnCancel;
   }
}
