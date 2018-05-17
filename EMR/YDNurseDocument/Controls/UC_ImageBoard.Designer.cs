namespace DrectSoft.Core.NurseDocument
{
    partial class UC_ImageBoard
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
            this.labelBottom = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelBottom
            // 
            this.labelBottom.Location = new System.Drawing.Point(3, 0);
            this.labelBottom.Name = "labelBottom";
            this.labelBottom.Size = new System.Drawing.Size(100, 23);
            this.labelBottom.TabIndex = 0;
            // 
            // UC_ImageBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.labelBottom);
            this.Name = "UC_ImageBoard";
            this.Size = new System.Drawing.Size(845, 400);
            this.Load += new System.EventHandler(this.UC_ImageBoard_Load);
            this.SizeChanged += new System.EventHandler(this.UC_ImageBoard_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelBottom;


    }
}
