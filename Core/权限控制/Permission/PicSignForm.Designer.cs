namespace DrectSoft.Core.Permission
{
    partial class PicSignForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PicSignForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnBrowse = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose(this.components);
            this.btnSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave(this.components);
            this.pePicSign = new DevExpress.XtraEditors.PictureEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.txtDepartment = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.lblDepartment = new DevExpress.XtraEditors.LabelControl();
            this.txtUserId = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.lblUserId = new DevExpress.XtraEditors.LabelControl();
            this.lblUserName = new DevExpress.XtraEditors.LabelControl();
            this.txtUserName = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.ofdBrowse = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pePicSign.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnBrowse);
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 197);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(438, 40);
            this.panelControl1.TabIndex = 0;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Location = new System.Drawing.Point(158, 7);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(80, 27);
            this.btnBrowse.TabIndex = 24;
            this.btnBrowse.Text = "浏览(&B)";
            this.btnBrowse.ToolTip = "浏览";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(336, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 27);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "关闭(&T)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(247, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 27);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pePicSign
            // 
            this.pePicSign.Location = new System.Drawing.Point(43, 37);
            this.pePicSign.Name = "pePicSign";
            this.pePicSign.Properties.AccessibleName = "";
            this.pePicSign.Properties.AllowFocused = false;
            this.pePicSign.Properties.ShowMenu = false;
            this.pePicSign.ShowToolTips = false;
            this.pePicSign.Size = new System.Drawing.Size(374, 160);
            this.pePicSign.TabIndex = 16;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.pePicSign);
            this.panelControl2.Controls.Add(this.txtDepartment);
            this.panelControl2.Controls.Add(this.lblDepartment);
            this.panelControl2.Controls.Add(this.txtUserId);
            this.panelControl2.Controls.Add(this.lblUserId);
            this.panelControl2.Controls.Add(this.lblUserName);
            this.panelControl2.Controls.Add(this.txtUserName);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(438, 197);
            this.panelControl2.TabIndex = 2;
            // 
            // txtDepartment
            // 
            this.txtDepartment.Enabled = false;
            this.txtDepartment.IsEnterChangeBgColor = false;
            this.txtDepartment.IsEnterKeyToNextControl = false;
            this.txtDepartment.IsNumber = false;
            this.txtDepartment.Location = new System.Drawing.Point(322, 14);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(95, 20);
            this.txtDepartment.TabIndex = 15;
            // 
            // lblDepartment
            // 
            this.lblDepartment.Location = new System.Drawing.Point(283, 17);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(36, 14);
            this.lblDepartment.TabIndex = 14;
            this.lblDepartment.Text = "科室：";
            // 
            // txtUserId
            // 
            this.txtUserId.Enabled = false;
            this.txtUserId.IsEnterChangeBgColor = false;
            this.txtUserId.IsEnterKeyToNextControl = false;
            this.txtUserId.IsNumber = false;
            this.txtUserId.Location = new System.Drawing.Point(180, 15);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(95, 20);
            this.txtUserId.TabIndex = 13;
            // 
            // lblUserId
            // 
            this.lblUserId.Location = new System.Drawing.Point(143, 17);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(36, 14);
            this.lblUserId.TabIndex = 12;
            this.lblUserId.Text = "工号：";
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new System.Drawing.Point(7, 17);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(36, 14);
            this.lblUserName.TabIndex = 11;
            this.lblUserName.Text = "姓名：";
            // 
            // txtUserName
            // 
            this.txtUserName.Enabled = false;
            this.txtUserName.IsEnterChangeBgColor = false;
            this.txtUserName.IsEnterKeyToNextControl = false;
            this.txtUserName.IsNumber = false;
            this.txtUserName.Location = new System.Drawing.Point(43, 14);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(95, 20);
            this.txtUserName.TabIndex = 10;
            // 
            // PicSignForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 237);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PicSignForm";
            this.Text = "图片签名";
            this.Load += new System.EventHandler(this.PicSignForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pePicSign.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        public DrectSoft.Common.Ctrs.OTHER.DevButtonClose btnClose;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnSave;
        private DevExpress.XtraEditors.PictureEdit pePicSign;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtDepartment;
        private DevExpress.XtraEditors.LabelControl lblDepartment;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtUserId;
        private DevExpress.XtraEditors.LabelControl lblUserId;
        private DevExpress.XtraEditors.LabelControl lblUserName;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtUserName;
        private System.Windows.Forms.OpenFileDialog ofdBrowse;
        private DevExpress.XtraEditors.SimpleButton btnBrowse;



    }
}