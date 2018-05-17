namespace DrectSoft.Core.EHRViewer
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.panelTool = new DevExpress.XtraEditors.PanelControl();
            this.txtUrl = new System.Windows.Forms.ComboBox();
            this.DrectDevButtonQurey1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barButtonBack = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonGo = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonLoadID = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.panelTool)).BeginInit();
            this.panelTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTool
            // 
            this.panelTool.Controls.Add(this.txtUrl);
            this.panelTool.Controls.Add(this.DrectDevButtonQurey1);
            this.panelTool.Controls.Add(this.label1);
            this.panelTool.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTool.Location = new System.Drawing.Point(0, 24);
            this.panelTool.Name = "panelTool";
            this.panelTool.Size = new System.Drawing.Size(816, 26);
            this.panelTool.TabIndex = 0;
            // 
            // txtUrl
            // 
            this.txtUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUrl.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.txtUrl.FormattingEnabled = true;
            this.txtUrl.Items.AddRange(new object[] {
            "http://www.baidu.com",
            "http://www.sina.com"});
            this.txtUrl.Location = new System.Drawing.Point(35, 2);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(697, 22);
            this.txtUrl.TabIndex = 3;
            // 
            // DrectDevButtonQurey1
            // 
            this.DrectDevButtonQurey1.Dock = System.Windows.Forms.DockStyle.Right;
            this.DrectDevButtonQurey1.Image = ((System.Drawing.Image)(resources.GetObject("DrectDevButtonQurey1.Image")));
            this.DrectDevButtonQurey1.Location = new System.Drawing.Point(732, 2);
            this.DrectDevButtonQurey1.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.DrectDevButtonQurey1.Name = "DrectDevButtonQurey1";
            this.DrectDevButtonQurey1.Size = new System.Drawing.Size(82, 22);
            this.DrectDevButtonQurey1.TabIndex = 2;
            this.DrectDevButtonQurey1.Text = "查询(&Q)";
            this.DrectDevButtonQurey1.Click += new System.EventHandler(this.DrectDevButtonQurey1_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Url:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 50);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(816, 434);
            this.webBrowser1.TabIndex = 1;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonBack,
            this.barButtonGo,
            this.barButtonLoadID});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 3;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonBack),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonGo),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonLoadID)});
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barButtonBack
            // 
            this.barButtonBack.Caption = "后退";
            this.barButtonBack.Id = 0;
            this.barButtonBack.Name = "barButtonBack";
            this.barButtonBack.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonBack_ItemClick);
            // 
            // barButtonGo
            // 
            this.barButtonGo.Caption = "前进";
            this.barButtonGo.Id = 1;
            this.barButtonGo.Name = "barButtonGo";
            this.barButtonGo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonGo_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(816, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 484);
            this.barDockControlBottom.Size = new System.Drawing.Size(816, 23);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 460);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(816, 24);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 460);
            // 
            // barButtonLoadID
            // 
            this.barButtonLoadID.Caption = "读卡";
            this.barButtonLoadID.Id = 2;
            this.barButtonLoadID.Name = "barButtonLoadID";
            this.barButtonLoadID.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonLoadID_ItemClick);
            // 
            // FrmMain
            // 
            this.AcceptButton = this.DrectDevButtonQurey1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 507);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.panelTool);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.DoubleBuffered = true;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EHR 浏览";
            ((System.ComponentModel.ISupportInitialize)(this.panelTool)).EndInit();
            this.panelTool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelTool;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey DrectDevButtonQurey1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonBack;
        private DevExpress.XtraBars.BarButtonItem barButtonGo;
        private System.Windows.Forms.ComboBox txtUrl;
        private DevExpress.XtraBars.BarButtonItem barButtonLoadID;
    }
}