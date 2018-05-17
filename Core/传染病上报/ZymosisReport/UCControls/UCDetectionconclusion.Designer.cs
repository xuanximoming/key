namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCDetectionconclusion
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
            this.chk_yangxing = new DevExpress.XtraEditors.CheckEdit();
            this.chk_tidai = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_yangxing.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_tidai.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(29, 9);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "实验室检测结论：";
            // 
            // chk_yangxing
            // 
            this.chk_yangxing.Location = new System.Drawing.Point(124, 7);
            this.chk_yangxing.Name = "chk_yangxing";
            this.chk_yangxing.Properties.Caption = "确认结果阳性";
            this.chk_yangxing.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_yangxing.Properties.RadioGroupIndex = 0;
            this.chk_yangxing.Size = new System.Drawing.Size(97, 19);
            this.chk_yangxing.TabIndex = 1;
            this.chk_yangxing.Tag = "1";
            // 
            // chk_tidai
            // 
            this.chk_tidai.Location = new System.Drawing.Point(227, 7);
            this.chk_tidai.Name = "chk_tidai";
            this.chk_tidai.Properties.Caption = "替代策略检测阳性";
            this.chk_tidai.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_tidai.Properties.RadioGroupIndex = 0;
            this.chk_tidai.Size = new System.Drawing.Size(139, 19);
            this.chk_tidai.TabIndex = 2;
            this.chk_tidai.Tag = "2";
            // 
            // UCDetectionconclusion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_tidai);
            this.Controls.Add(this.chk_yangxing);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCDetectionconclusion";
            this.Size = new System.Drawing.Size(391, 35);
            ((System.ComponentModel.ISupportInitialize)(this.chk_yangxing.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_tidai.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chk_yangxing;
        private DevExpress.XtraEditors.CheckEdit chk_tidai;
    }
}
