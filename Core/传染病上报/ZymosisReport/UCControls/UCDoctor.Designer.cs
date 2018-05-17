namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCDoctor
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txt_yisheng = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_yisheng.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(53, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(120, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "报告人（填卡医生）：";
            // 
            // txt_yisheng
            // 
            this.txt_yisheng.Location = new System.Drawing.Point(175, 3);
            this.txt_yisheng.Name = "txt_yisheng";
            this.txt_yisheng.Size = new System.Drawing.Size(117, 21);
            this.txt_yisheng.TabIndex = 1;
            // 
            // UCDoctor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt_yisheng);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCDoctor";
            this.Size = new System.Drawing.Size(330, 29);
            this.Tag = "DOCTOR";
            ((System.ComponentModel.ISupportInitialize)(this.txt_yisheng.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_yisheng;
    }
}
