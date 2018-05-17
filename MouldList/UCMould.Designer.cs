namespace DrectSoft.Core.MouldList
{
    partial class UCMould
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMould));
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.labModuleName = new DevExpress.XtraEditors.LabelControl();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(1, 6);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.ReadOnly = true;
            this.pictureEdit1.Size = new System.Drawing.Size(70, 68);
            this.pictureEdit1.TabIndex = 0;
            this.pictureEdit1.Visible = false;
            this.pictureEdit1.DoubleClick += new System.EventHandler(this.pictureEdit1_DoubleClick);
            this.pictureEdit1.MouseEnter += new System.EventHandler(this.pictureEdit1_MouseEnter);
            // 
            // labModuleName
            // 
            this.labModuleName.Appearance.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labModuleName.Appearance.ForeColor = System.Drawing.Color.White;
            this.labModuleName.Location = new System.Drawing.Point(56, 34);
            this.labModuleName.Name = "labModuleName";
            this.labModuleName.Size = new System.Drawing.Size(130, 20);
            this.labModuleName.TabIndex = 1;
            this.labModuleName.Text = "labelControl1";
            this.labModuleName.Visible = false;
            this.labModuleName.DoubleClick += new System.EventHandler(this.labModuleName_DoubleClick);
            this.labModuleName.MouseEnter += new System.EventHandler(this.labModuleName_MouseEnter);
            // 
            // UCMould
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labModuleName);
            this.Controls.Add(this.pictureEdit1);
            this.Name = "UCMould";
            this.Size = new System.Drawing.Size(252, 80);
            this.Load += new System.EventHandler(this.UCMould_Load);
            this.DoubleClick += new System.EventHandler(this.UCMould_DoubleClick);
            this.Click += new System.EventHandler(UCMould_Click);
            this.MouseEnter += new System.EventHandler(this.UCMould_MouseEnter);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.LabelControl labModuleName;
        private DevExpress.Utils.ToolTipController toolTipController1;

    }
}
