namespace DrectSoft.MainFrame
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barStatus = new DevExpress.XtraBars.Bar();
            this.barStaticUserName = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticDept = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemTime = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemIP = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonUserName = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItemWard = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timerLock = new System.Windows.Forms.Timer(this.components);
            this.panelControlTopRight = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonNormal = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonHelp = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonChangeArea = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonReLogin = new DevExpress.XtraEditors.SimpleButton();
            this.sbtnchooseDatabase = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonChangePassword = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonClose = new DevExpress.XtraEditors.SimpleButton();
            this.timerMessageWindow = new System.Windows.Forms.Timer(this.components);
            this.timerGarbageCollect = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTopRight)).BeginInit();
            this.panelControlTopRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.AppearancePage.Header.Options.UseFont = true;
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
            this.barStaticItemTime,
            this.barStaticItemIP,
            this.barButtonUserName,
            this.barEditItemWard,
            this.barStaticDept,
            this.barStaticItem1});
            this.barManager1.MaxItemId = 37;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticDept),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemTime),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemIP)});
            this.barStatus.OptionsBar.AllowQuickCustomization = false;
            this.barStatus.OptionsBar.DrawDragBorder = false;
            this.barStatus.OptionsBar.UseWholeRow = true;
            this.barStatus.Text = "Status bar";
            // 
            // barStaticUserName
            // 
            this.barStaticUserName.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
            this.barStaticUserName.Caption = "用户：";
            this.barStaticUserName.Id = 22;
            this.barStaticUserName.Name = "barStaticUserName";
            this.barStaticUserName.TextAlignment = System.Drawing.StringAlignment.Near;
            this.barStaticUserName.Width = 150;
            // 
            // barStaticDept
            // 
            this.barStaticDept.Caption = "科室病区：";
            this.barStaticDept.Id = 0;
            this.barStaticDept.Name = "barStaticDept";
            this.barStaticDept.TextAlignment = System.Drawing.StringAlignment.Near;
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
            this.barDockControlTop.Size = new System.Drawing.Size(943, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 617);
            this.barDockControlBottom.Size = new System.Drawing.Size(943, 27);
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
            this.barDockControlRight.Location = new System.Drawing.Point(943, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 617);
            // 
            // barButtonUserName
            // 
            this.barButtonUserName.Caption = " xxx     ";
            this.barButtonUserName.Id = 34;
            this.barButtonUserName.Name = "barButtonUserName";
            // 
            // barEditItemWard
            // 
            this.barEditItemWard.Id = 35;
            this.barEditItemWard.Name = "barEditItemWard";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "--";
            this.barStaticItem1.Id = 36;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
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
            this.panelControlTopRight.Controls.Add(this.sbtnchooseDatabase);
            this.panelControlTopRight.Controls.Add(this.simpleButtonChangePassword);
            this.panelControlTopRight.Controls.Add(this.simpleButtonClose);
            this.panelControlTopRight.Location = new System.Drawing.Point(520, 48);
            this.panelControlTopRight.Name = "panelControlTopRight";
            this.panelControlTopRight.Size = new System.Drawing.Size(423, 24);
            this.panelControlTopRight.TabIndex = 0;
            // 
            // simpleButtonNormal
            // 
            this.simpleButtonNormal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonNormal.Location = new System.Drawing.Point(506, 0);
            this.simpleButtonNormal.Name = "simpleButtonNormal";
            this.simpleButtonNormal.Size = new System.Drawing.Size(10, 21);
            this.simpleButtonNormal.TabIndex = 6;
            this.simpleButtonNormal.Tag = "关闭";
            this.simpleButtonNormal.Visible = false;
            // 
            // simpleButtonHelp
            // 
            this.simpleButtonHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonHelp.Location = new System.Drawing.Point(260, 0);
            this.simpleButtonHelp.Name = "simpleButtonHelp";
            this.simpleButtonHelp.Size = new System.Drawing.Size(52, 21);
            this.simpleButtonHelp.TabIndex = 3;
            this.simpleButtonHelp.Tag = "";
            this.simpleButtonHelp.Text = "帮助";
            this.simpleButtonHelp.ToolTip = "用户手册";
            // 
            // simpleButtonChangeArea
            // 
            this.simpleButtonChangeArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonChangeArea.Location = new System.Drawing.Point(3, 0);
            this.simpleButtonChangeArea.Name = "simpleButtonChangeArea";
            this.simpleButtonChangeArea.Size = new System.Drawing.Size(80, 21);
            this.simpleButtonChangeArea.TabIndex = 0;
            this.simpleButtonChangeArea.Tag = "切换科室";
            this.simpleButtonChangeArea.Text = "切换科室";
            // 
            // simpleButtonReLogin
            // 
            this.simpleButtonReLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonReLogin.Location = new System.Drawing.Point(88, 0);
            this.simpleButtonReLogin.Name = "simpleButtonReLogin";
            this.simpleButtonReLogin.Size = new System.Drawing.Size(80, 21);
            this.simpleButtonReLogin.TabIndex = 1;
            this.simpleButtonReLogin.Tag = "重新登录";
            this.simpleButtonReLogin.Text = "重新登录";
            // 
            // sbtnchooseDatabase
            // 
            this.sbtnchooseDatabase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.sbtnchooseDatabase.Location = new System.Drawing.Point(452, 3);
            this.sbtnchooseDatabase.Name = "sbtnchooseDatabase";
            this.sbtnchooseDatabase.Size = new System.Drawing.Size(33, 21);
            this.sbtnchooseDatabase.TabIndex = 5;
            this.sbtnchooseDatabase.Tag = "";
            this.sbtnchooseDatabase.Text = " 切换数据库";
            this.sbtnchooseDatabase.Visible = false;
            // 
            // simpleButtonChangePassword
            // 
            this.simpleButtonChangePassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonChangePassword.Location = new System.Drawing.Point(174, 0);
            this.simpleButtonChangePassword.Name = "simpleButtonChangePassword";
            this.simpleButtonChangePassword.Size = new System.Drawing.Size(80, 21);
            this.simpleButtonChangePassword.TabIndex = 2;
            this.simpleButtonChangePassword.Tag = "修改密码";
            this.simpleButtonChangePassword.Text = "修改密码";
            // 
            // simpleButtonClose
            // 
            this.simpleButtonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.simpleButtonClose.Image = global::DrectSoft.MainFrame.Properties.Resources.退出;
            this.simpleButtonClose.Location = new System.Drawing.Point(318, 0);
            this.simpleButtonClose.Name = "simpleButtonClose";
            this.simpleButtonClose.Size = new System.Drawing.Size(80, 21);
            this.simpleButtonClose.TabIndex = 4;
            this.simpleButtonClose.Tag = "";
            this.simpleButtonClose.Text = "退出系统";
            this.simpleButtonClose.ToolTip = "退出系统";
            // 
            // timerMessageWindow
            // 
            this.timerMessageWindow.Tick += new System.EventHandler(this.timerMessageWindow_Tick);
            // 
            // timerGarbageCollect
            // 
            this.timerGarbageCollect.Interval = 20000;
            this.timerGarbageCollect.Tick += new System.EventHandler(this.timerGarbageCollect_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 644);
            this.Controls.Add(this.panelControlTopRight);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "结构化电子病历系统";
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

        #endregion

        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar barStatus;
        private DevExpress.XtraEditors.PanelControl panelControlTopRight;
        private DevExpress.XtraEditors.SimpleButton sbtnchooseDatabase;
        private DevExpress.XtraEditors.SimpleButton simpleButtonClose;
        private DevExpress.XtraEditors.SimpleButton simpleButtonChangePassword;
        private DevExpress.XtraEditors.SimpleButton simpleButtonReLogin;
        private DevExpress.XtraEditors.SimpleButton simpleButtonChangeArea;
        private DevExpress.XtraEditors.SimpleButton simpleButtonHelp;
        private System.Windows.Forms.Timer timerLock;
        private System.Windows.Forms.Timer timer1;

        private DevExpress.XtraBars.BarStaticItem barStaticItemTime;
        private DevExpress.XtraBars.BarStaticItem barStaticItemIP;
        private DevExpress.XtraBars.BarButtonItem barButtonUserName;
        private DevExpress.XtraBars.BarButtonItem barEditItemWard;
        private System.Windows.Forms.Timer timerMessageWindow;
        private System.Windows.Forms.Timer timerGarbageCollect;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraEditors.SimpleButton simpleButtonNormal;
        private DevExpress.XtraBars.BarStaticItem barStaticDept;
        public DevExpress.XtraBars.BarStaticItem barStaticUserName;
        public DevExpress.XtraBars.BarStaticItem barStaticItem1;

    }
}
