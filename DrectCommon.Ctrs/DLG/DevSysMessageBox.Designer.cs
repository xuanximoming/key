namespace DrectSoft.Common.Ctrs.DLG
{
    partial class DevSysMessageBox
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevSysMessageBox));
            this.richTxtInfo = new System.Windows.Forms.RichTextBox();
            this.btnEnter = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // richTxtInfo
            // 
            this.richTxtInfo.Location = new System.Drawing.Point(14, 13);
            this.richTxtInfo.Name = "richTxtInfo";
            this.richTxtInfo.Size = new System.Drawing.Size(372, 143);
            this.richTxtInfo.TabIndex = 1;
            this.richTxtInfo.Text = "";
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(311, 162);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(75, 23);
            this.btnEnter.TabIndex = 3;
            this.btnEnter.Text = "确定(&E)";
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // DevSysMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 219);
            this.Controls.Add(this.btnEnter);
            this.Controls.Add(this.richTxtInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DevSysMessageBox";
            this.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "详细信息";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTxtInfo;
        private DevExpress.XtraEditors.SimpleButton btnEnter;

    }
}
