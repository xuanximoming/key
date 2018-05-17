namespace DrectSoft.Core.Permission
{
    partial class PermissionMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PermissionMainForm));
            this.xtraTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageJobs = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageUsers = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPagePicSign = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).BeginInit();
            this.xtraTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl
            // 
            this.xtraTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl.Name = "xtraTabControl";
            this.xtraTabControl.SelectedTabPage = this.xtraTabPageJobs;
            this.xtraTabControl.Size = new System.Drawing.Size(1197, 742);
            this.xtraTabControl.TabIndex = 0;
            this.xtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageJobs,
            this.xtraTabPageUsers,
            this.xtraTabPagePicSign});
            // 
            // xtraTabPageJobs
            // 
            this.xtraTabPageJobs.Name = "xtraTabPageJobs";
            this.xtraTabPageJobs.Size = new System.Drawing.Size(1191, 713);
            this.xtraTabPageJobs.Text = "岗位信息";
            // 
            // xtraTabPageUsers
            // 
            this.xtraTabPageUsers.Name = "xtraTabPageUsers";
            this.xtraTabPageUsers.Size = new System.Drawing.Size(1191, 713);
            this.xtraTabPageUsers.Text = "员工信息";
            // 
            // xtraTabPagePicSign
            // 
            this.xtraTabPagePicSign.Name = "xtraTabPagePicSign";
            this.xtraTabPagePicSign.Size = new System.Drawing.Size(1191, 713);
            this.xtraTabPagePicSign.Text = "图片签名";
            // 
            // PermissionMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1197, 742);
            this.Controls.Add(this.xtraTabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PermissionMainForm";
            this.Text = "岗位权限设置";
            this.Load += new System.EventHandler(this.PermissionMainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).EndInit();
            this.xtraTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageJobs;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageUsers;
        private DevExpress.XtraTab.XtraTabPage xtraTabPagePicSign;
        //private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;


    }
}