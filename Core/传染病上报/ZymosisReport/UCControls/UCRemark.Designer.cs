namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCRemark
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
            this.txt_beizhu = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txt_beizhu.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_beizhu
            // 
            this.txt_beizhu.Location = new System.Drawing.Point(168, 0);
            this.txt_beizhu.Name = "txt_beizhu";
            this.txt_beizhu.Size = new System.Drawing.Size(405, 21);
            this.txt_beizhu.TabIndex = 1;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(130, 3);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 0;
            this.labelControl5.Text = "备注：";
            // 
            // UCRemark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt_beizhu);
            this.Controls.Add(this.labelControl5);
            this.Name = "UCRemark";
            this.Size = new System.Drawing.Size(591, 28);
            ((System.ComponentModel.ISupportInitialize)(this.txt_beizhu.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txt_beizhu;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}
