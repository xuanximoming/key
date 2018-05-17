using DevExpress.XtraEditors;
using System.Windows.Forms;
namespace DrectSoft.Common.Ctrs.OTHER
{
    partial class DSTextBoxSelFile
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
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLoadFile.FlatAppearance.BorderSize = 0;
            this.btnLoadFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLoadFile.Location = new System.Drawing.Point(209, 0);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(20, 21);
            this.btnLoadFile.TabIndex = 1;
            this.btnLoadFile.Text = "…";
            // 
            // YDTextBoxSelFile
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnLoadFile);
            this.Size = new System.Drawing.Size(229, 21);
            this.ResumeLayout(false);

        }

        #endregion

        public Button btnLoadFile;

    }
}
