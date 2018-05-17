namespace IemMainPageExtension
{
    partial class UcLookUpEdit
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
            this.lookUpEditorIem = new DrectSoft.Common.Library.LookUpEditor();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorIem)).BeginInit();
            this.SuspendLayout();
            // 
            // lookUpEditorIem
            // 
            this.lookUpEditorIem.IsNeedPaint = true;
            this.lookUpEditorIem.ListWindow = null;
            this.lookUpEditorIem.Location = new System.Drawing.Point(0, 1);
            this.lookUpEditorIem.Name = "lookUpEditorIem";
            this.lookUpEditorIem.ShowSButton = true;
            this.lookUpEditorIem.Size = new System.Drawing.Size(99, 20);
            this.lookUpEditorIem.TabIndex = 1;
            // 
            // UcLookUpEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lookUpEditorIem);
            this.Name = "UcLookUpEdit";
            this.Size = new System.Drawing.Size(105, 21);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorIem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DrectSoft.Common.Library.LookUpEditor lookUpEditorIem;
    }
}
