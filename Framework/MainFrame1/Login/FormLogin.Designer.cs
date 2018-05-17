namespace DrectSoft.MainFrame
{
    partial class FormLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.panelForm = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblVersion = new DevExpress.XtraEditors.LabelControl();
            this.progressBarControlWait = new DevExpress.XtraEditors.ProgressBarControl();
            this.labelControlWait = new DevExpress.XtraEditors.LabelControl();
            this.textBoxPassword = new DevExpress.XtraEditors.TextEdit();
            this.textBoxUserID = new DevExpress.XtraEditors.TextEdit();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            this.buttonLogIn = new DevExpress.XtraEditors.SimpleButton();
            this.labelUserName = new System.Windows.Forms.Label();
            this.memoEdit1 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelForm)).BeginInit();
            this.panelForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControlWait.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxUserID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelForm
            // 
            this.panelForm.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.panelForm.Appearance.Options.UseBackColor = true;
            this.panelForm.ContentImage = ((System.Drawing.Image)(resources.GetObject("panelForm.ContentImage")));
            this.panelForm.Controls.Add(this.labelControl1);
            this.panelForm.Controls.Add(this.lblVersion);
            this.panelForm.Controls.Add(this.progressBarControlWait);
            this.panelForm.Controls.Add(this.labelControlWait);
            this.panelForm.Controls.Add(this.textBoxPassword);
            this.panelForm.Controls.Add(this.textBoxUserID);
            this.panelForm.Controls.Add(this.btnExit);
            this.panelForm.Controls.Add(this.buttonLogIn);
            this.panelForm.Controls.Add(this.labelUserName);
            this.panelForm.Controls.Add(this.memoEdit1);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForm.Location = new System.Drawing.Point(0, 0);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(712, 389);
            this.panelForm.TabIndex = 1;
            this.panelForm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelForm_MouseDown);
            this.panelForm.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelForm_MouseMove);
            this.panelForm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelForm_MouseUp);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("微软雅黑", 22F);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl1.Location = new System.Drawing.Point(94, 77);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(351, 39);
            this.labelControl1.TabIndex = 46;
            this.labelControl1.Text = "                   电子病历系统";
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(650, 373);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(0, 14);
            this.lblVersion.TabIndex = 45;
            // 
            // progressBarControlWait
            // 
            this.progressBarControlWait.Location = new System.Drawing.Point(269, 314);
            this.progressBarControlWait.Name = "progressBarControlWait";
            this.progressBarControlWait.Size = new System.Drawing.Size(258, 20);
            this.progressBarControlWait.TabIndex = 44;
            // 
            // labelControlWait
            // 
            this.labelControlWait.Location = new System.Drawing.Point(215, 314);
            this.labelControlWait.Name = "labelControlWait";
            this.labelControlWait.Size = new System.Drawing.Size(48, 14);
            this.labelControlWait.TabIndex = 43;
            this.labelControlWait.Text = "请稍等：";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(290, 207);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Properties.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(187, 20);
            this.textBoxPassword.TabIndex = 1;
            this.textBoxPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPassword_KeyPress);
            // 
            // textBoxUserID
            // 
            this.textBoxUserID.Location = new System.Drawing.Point(290, 168);
            this.textBoxUserID.Name = "textBoxUserID";
            this.textBoxUserID.Size = new System.Drawing.Size(187, 20);
            this.textBoxUserID.TabIndex = 0;
            this.textBoxUserID.Leave += new System.EventHandler(this.textBoxUserID_Leave);
            // 
            // btnExit
            // 
            this.btnExit.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExit.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExit.Appearance.Options.UseFont = true;
            this.btnExit.Appearance.Options.UseForeColor = true;
            this.btnExit.Location = new System.Drawing.Point(402, 267);
            this.btnExit.LookAndFeel.SkinName = "Blue";
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 27);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "退出 (&X)";
            this.btnExit.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonLogIn
            // 
            this.buttonLogIn.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.buttonLogIn.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonLogIn.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonLogIn.Appearance.Options.UseBackColor = true;
            this.buttonLogIn.Appearance.Options.UseFont = true;
            this.buttonLogIn.Appearance.Options.UseForeColor = true;
            this.buttonLogIn.Location = new System.Drawing.Point(312, 267);
            this.buttonLogIn.LookAndFeel.SkinName = "Blue";
            this.buttonLogIn.Name = "buttonLogIn";
            this.buttonLogIn.Size = new System.Drawing.Size(75, 27);
            this.buttonLogIn.TabIndex = 2;
            this.buttonLogIn.Text = "登录 (&L)";
            this.buttonLogIn.Click += new System.EventHandler(this.buttonLogIn_Click);
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.BackColor = System.Drawing.Color.Transparent;
            this.labelUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelUserName.Location = new System.Drawing.Point(325, 167);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(0, 20);
            this.labelUserName.TabIndex = 41;
            this.labelUserName.Visible = false;
            // 
            // memoEdit1
            // 
            this.memoEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.memoEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.memoEdit1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.memoEdit1.Location = new System.Drawing.Point(269, 314);
            this.memoEdit1.Multiline = true;
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(258, 20);
            this.memoEdit1.TabIndex = 37;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormLogin
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.ClientSize = new System.Drawing.Size(712, 389);
            this.Controls.Add(this.panelForm);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormLogin_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.panelForm)).EndInit();
            this.panelForm.ResumeLayout(false);
            this.panelForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControlWait.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxUserID.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox memoEdit1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.PanelControl panelForm;
        
       private System.Windows.Forms.Label labelUserName;
       private DevExpress.XtraEditors.TextEdit textBoxUserID;
       private DevExpress.XtraEditors.SimpleButton btnExit;
       private DevExpress.XtraEditors.SimpleButton buttonLogIn;
       private DevExpress.XtraEditors.TextEdit textBoxPassword;
       private DevExpress.XtraEditors.ProgressBarControl progressBarControlWait;
       private DevExpress.XtraEditors.LabelControl labelControlWait;
       private DevExpress.XtraEditors.LabelControl lblVersion;
       private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}