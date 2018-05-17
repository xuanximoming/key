namespace DrectSoft.MainFrame
{
    partial class FormMain
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.barStatus = new DevExpress.XtraBars.Bar();
            this.barStaticUserName = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonName = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonDept = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItemWard = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItemTime = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemIP = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.timer1 = new System.Windows.Forms.Timer();
            this.timerLock = new System.Windows.Forms.Timer();
            this.panelControlTopRight = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonNormal = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonHelp = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonChangeArea = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonReLogin = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonMinimum = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonChangePassword = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonClose = new DevExpress.XtraEditors.SimpleButton();
            this.timerMessageWindow = new System.Windows.Forms.Timer();
            this.timerGarbageCollect = new System.Windows.Forms.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTopRight)).BeginInit();
            this.panelControlTopRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.MdiParent = this;
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barStatus});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barStaticUserName,
            this.barButtonDept,
            this.barStaticItemTime,
            this.barStaticItemIP,
            this.barButtonName,
            this.barEditItemWard});
            this.barManager1.MaxItemId = 0;
            this.barManager1.StatusBar = this.barStatus;
            // 
            // barStatus
            // 
            this.barStatus.BarName = "Status bar";
            this.barStatus.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.barStatus.DockCol = 0;
            this.barStatus.DockRow = 0;
            this.barStatus.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.barStatus.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticUserName),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonName),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonDept, true),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.barEditItemWard, "", false, true, true, 265),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemTime, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemIP)});
            this.barStatus.OptionsBar.AllowQuickCustomization = false;
            this.barStatus.OptionsBar.DrawDragBorder = false;
            this.barStatus.OptionsBar.UseWholeRow = true;
            this.barStatus.Text = "Status bar";
            // 
            // barStaticUserName
            // 
            this.barStaticUserName.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
            this.barStaticUserName.Caption = "用户:";
            this.barStaticUserName.Id = 22;
            this.barStaticUserName.Name = "barStaticUserName";
            this.barStaticUserName.TextAlignment = System.Drawing.StringAlignment.Near;
            this.barStaticUserName.Width = 32;
            // 
            // barButtonName
            // 
            this.barButtonName.Caption = " xxx     ";
            this.barButtonName.Id = 34;
            this.barButtonName.Name = "barButtonName";
            // 
            // barButtonDept
            // 
            this.barButtonDept.Caption = "科室病区";
            this.barButtonDept.Id = 26;
            this.barButtonDept.Name = "barButtonDept";
            // 
            // barEditItemWard
            // 
            this.barEditItemWard.Id = 35;
            this.barEditItemWard.Name = "barEditItemWard";
            // 
            // barStaticItemTime
            // 
            this.barStaticItemTime.Caption = "系统时间";
            this.barStaticItemTime.Id = 28;
            this.barStaticItemTime.Name = "barStaticItemTime";
            this.barStaticItemTime.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItemIP
            // 
            this.barStaticItemIP.Caption = "IP:192.168.2.202";
            this.barStaticItemIP.Id = 30;
            this.barStaticItemIP.Name = "barStaticItemIP";
            this.barStaticItemIP.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(961, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 617);
            this.barDockControlBottom.Size = new System.Drawing.Size(961, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 617);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(961, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 617);
            // 
            // timer1
            // 
            this.timer1.Interval = 600;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerLock
            // 
            this.timerLock.Interval = 100000;
            this.timerLock.Tick += new System.EventHandler(this.timerLock_Tick);
            // 
            // panelControlTopRight
            // 
            this.panelControlTopRight.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControlTopRight.Controls.Add(this.simpleButtonNormal);
            this.panelControlTopRight.Controls.Add(this.simpleButtonHelp);
            this.panelControlTopRight.Controls.Add(this.simpleButtonChangeArea);
            this.panelControlTopRight.Controls.Add(this.simpleButtonReLogin);
            this.panelControlTopRight.Controls.Add(this.simpleButtonMinimum);
            this.panelControlTopRight.Controls.Add(this.simpleButtonChangePassword);
            this.panelControlTopRight.Controls.Add(this.simpleButtonClose);
            this.panelControlTopRight.Location = new System.Drawing.Point(634, 49);
            this.panelControlTopRight.Name = "panelControlTopRight";
            this.panelControlTopRight.Size = new System.Drawing.Size(156, 24);
            this.panelControlTopRight.TabIndex = 5;
            // 
            // simpleButtonNormal
            // 
            this.simpleButtonNormal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonNormal.Location = new System.Drawing.Point(183, -1);
            this.simpleButtonNormal.Name = "simpleButtonNormal";
            this.simpleButtonNormal.Size = new System.Drawing.Size(27, 27);
            this.simpleButtonNormal.TabIndex = 16;
            this.simpleButtonNormal.Tag = "关闭";
            this.simpleButtonNormal.Visible = false;
            // 
            // simpleButtonHelp
            // 
            this.simpleButtonHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonHelp.Location = new System.Drawing.Point(113, -1);
            this.simpleButtonHelp.Name = "simpleButtonHelp";
            this.simpleButtonHelp.Size = new System.Drawing.Size(27, 27);
            this.simpleButtonHelp.TabIndex = 10;
            this.simpleButtonHelp.Tag = "帮助";
            // 
            // simpleButtonChangeArea
            // 
            this.simpleButtonChangeArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonChangeArea.Location = new System.Drawing.Point(8, -1);
            this.simpleButtonChangeArea.Name = "simpleButtonChangeArea";
            this.simpleButtonChangeArea.Size = new System.Drawing.Size(27, 27);
            this.simpleButtonChangeArea.TabIndex = 11;
            this.simpleButtonChangeArea.Tag = "切换病区";
            // 
            // simpleButtonReLogin
            // 
            this.simpleButtonReLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonReLogin.Location = new System.Drawing.Point(43, -1);
            this.simpleButtonReLogin.Name = "simpleButtonReLogin";
            this.simpleButtonReLogin.Size = new System.Drawing.Size(27, 27);
            this.simpleButtonReLogin.TabIndex = 12;
            this.simpleButtonReLogin.Tag = "重新登陆";
            // 
            // simpleButtonMinimum
            // 
            this.simpleButtonMinimum.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonMinimum.Location = new System.Drawing.Point(148, -1);
            this.simpleButtonMinimum.Name = "simpleButtonMinimum";
            this.simpleButtonMinimum.Size = new System.Drawing.Size(27, 27);
            this.simpleButtonMinimum.TabIndex = 14;
            this.simpleButtonMinimum.Tag = "最小化";
            this.simpleButtonMinimum.Visible = false;
            // 
            // simpleButtonChangePassword
            // 
            this.simpleButtonChangePassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonChangePassword.Location = new System.Drawing.Point(78, -1);
            this.simpleButtonChangePassword.Name = "simpleButtonChangePassword";
            this.simpleButtonChangePassword.Size = new System.Drawing.Size(27, 27);
            this.simpleButtonChangePassword.TabIndex = 13;
            this.simpleButtonChangePassword.Tag = "修改密码";
            // 
            // simpleButtonClose
            // 
            this.simpleButtonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonClose.Location = new System.Drawing.Point(218, -1);
            this.simpleButtonClose.Name = "simpleButtonClose";
            this.simpleButtonClose.Size = new System.Drawing.Size(27, 27);
            this.simpleButtonClose.TabIndex = 15;
            this.simpleButtonClose.Tag = "关闭";
            this.simpleButtonClose.Visible = false;
            // 
            // timerGarbageCollect
            // 
            this.timerGarbageCollect.Interval = 60000;
            this.timerGarbageCollect.Tick += new System.EventHandler(this.timerGarbageCollect_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 644);
            this.Controls.Add(this.panelControlTopRight);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.Text = "电子病历系统";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMain_Paint);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTopRight)).EndInit();
            this.panelControlTopRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar barStatus;
        private DevExpress.XtraEditors.PanelControl panelControlTopRight;
        private DevExpress.XtraEditors.SimpleButton simpleButtonMinimum;
        private DevExpress.XtraEditors.SimpleButton simpleButtonClose;
        private DevExpress.XtraEditors.SimpleButton simpleButtonChangePassword;
        private DevExpress.XtraEditors.SimpleButton simpleButtonReLogin;
        private DevExpress.XtraEditors.SimpleButton simpleButtonChangeArea;
        private DevExpress.XtraEditors.SimpleButton simpleButtonHelp;
        private System.Windows.Forms.Timer timerLock;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraBars.BarStaticItem barStaticItemTime;
        private DevExpress.XtraBars.BarStaticItem barStaticItemIP;
        private DevExpress.XtraBars.BarButtonItem barButtonName;
        private DevExpress.XtraBars.BarButtonItem barEditItemWard;
        private DevExpress.XtraBars.BarStaticItem barStaticUserName;
        private DevExpress.XtraBars.BarButtonItem barButtonDept;
        private System.Windows.Forms.Timer timerMessageWindow;
        private System.Windows.Forms.Timer timerGarbageCollect;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraEditors.SimpleButton simpleButtonNormal;
    }
}