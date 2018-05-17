namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCAffirmDept
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
            this.txt_danwei = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txt_danwei.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_danwei
            // 
            this.txt_danwei.Location = new System.Drawing.Point(218, 1);
            this.txt_danwei.Name = "txt_danwei";
            this.txt_danwei.Size = new System.Drawing.Size(232, 21);
            this.txt_danwei.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(60, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(156, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "确认（替代策略）检测单位：";
            // 
            // UCAffirmDept
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt_danwei);
            this.Controls.Add(this.labelControl2);
            this.Name = "UCAffirmDept";
            this.Size = new System.Drawing.Size(467, 27);
            ((System.ComponentModel.ISupportInitialize)(this.txt_danwei.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txt_danwei;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}
