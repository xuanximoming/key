namespace DrectSoft.Core.MouldList
{
    partial class FormMouldList
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMouldList));
            this.panelZoom = new System.Windows.Forms.Panel();
            this.vScrollBarRight = new DevExpress.XtraEditors.VScrollBar();
            this.panelZoom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelZoom
            // 
            this.panelZoom.Controls.Add(this.vScrollBarRight);
            this.panelZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelZoom.Location = new System.Drawing.Point(0, 0);
            this.panelZoom.Name = "panelZoom";
            this.panelZoom.Size = new System.Drawing.Size(1008, 587);
            this.panelZoom.TabIndex = 21;
            this.panelZoom.Paint += new System.Windows.Forms.PaintEventHandler(this.panelZoom_Paint);
            // 
            // vScrollBarRight
            // 
            this.vScrollBarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBarRight.Location = new System.Drawing.Point(978, 0);
            this.vScrollBarRight.Name = "vScrollBarRight";
            this.vScrollBarRight.Size = new System.Drawing.Size(30, 587);
            this.vScrollBarRight.TabIndex = 1;
            // 
            // FormMouldList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1008, 587);
            this.Controls.Add(this.panelZoom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMouldList";
            this.Text = "电子病历平台";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.FormMouldList_SizeChanged);
            this.panelZoom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelZoom;
        private DevExpress.XtraEditors.VScrollBar vScrollBarRight;




    }
}

