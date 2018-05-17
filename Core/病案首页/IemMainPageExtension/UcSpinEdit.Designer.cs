namespace IemMainPageExtension
{
    partial class UcSpinEdit
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
            this.spinEditIem = new DrectSoft.Common.Ctrs.OTHER.DSSpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditIem.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // spinEditIem
            // 
            this.spinEditIem.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEditIem.Location = new System.Drawing.Point(1, 0);
            this.spinEditIem.Name = "spinEditIem";
            this.spinEditIem.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEditIem.Size = new System.Drawing.Size(99, 21);
            this.spinEditIem.TabIndex = 4;
            // 
            // UcSpinEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.spinEditIem);
            this.Name = "UcSpinEdit";
            this.Size = new System.Drawing.Size(108, 21);
            ((System.ComponentModel.ISupportInitialize)(this.spinEditIem.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DrectSoft.Common.Ctrs.OTHER.DSSpinEdit spinEditIem;
    }
}
