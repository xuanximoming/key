namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCDetailAdresse
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
            this.txt_xiangxidizhi = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txt_xiangxidizhi.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_xiangxidizhi
            // 
            this.txt_xiangxidizhi.Location = new System.Drawing.Point(100, 2);
            this.txt_xiangxidizhi.Name = "txt_xiangxidizhi";
            this.txt_xiangxidizhi.Size = new System.Drawing.Size(264, 21);
            this.txt_xiangxidizhi.TabIndex = 1;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(14, 5);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(84, 14);
            this.labelControl7.TabIndex = 0;
            this.labelControl7.Text = "户籍详细地址：";
            // 
            // UCDetailAdresse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt_xiangxidizhi);
            this.Controls.Add(this.labelControl7);
            this.Name = "UCDetailAdresse";
            this.Size = new System.Drawing.Size(505, 29);
            ((System.ComponentModel.ISupportInitialize)(this.txt_xiangxidizhi.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txt_xiangxidizhi;
        private DevExpress.XtraEditors.LabelControl labelControl7;
    }
}
