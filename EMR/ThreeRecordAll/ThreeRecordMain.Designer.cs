namespace DrectSoft.EMR.ThreeRecordAll
{
    partial class ThreeRecordMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreeRecordMain));
            this.tabControlSCD = new DevExpress.XtraTab.XtraTabControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnSCDSin = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnSCDOPL = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnHLDSing = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnHLDPL = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItem1 = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageList1 = new System.Windows.Forms.ImageList();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlSCD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlSCD
            // 
            this.tabControlSCD.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
            this.tabControlSCD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSCD.Location = new System.Drawing.Point(0, 71);
            this.tabControlSCD.Name = "tabControlSCD";
            this.tabControlSCD.Size = new System.Drawing.Size(816, 436);
            this.tabControlSCD.TabIndex = 0;
            this.tabControlSCD.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tabControlSCD_SelectedPageChanged);
            this.tabControlSCD.CloseButtonClick += new System.EventHandler(this.tabControlSCD_CloseButtonClick);
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnSCDSin,
            this.btnSCDOPL,
            this.btnHLDSing,
            this.btnHLDPL,
            this.barLargeButtonItem1});
            this.barManager1.LargeImages = this.imageList1;
            this.barManager1.MaxItemId = 19;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnSCDSin, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnSCDOPL, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnHLDSing, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnHLDPL, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItem1, true)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // btnSCDSin
            // 
            this.btnSCDSin.Caption = "三测单单人录入";
            this.btnSCDSin.Id = 14;
            this.btnSCDSin.LargeImageIndex = 2;
            this.btnSCDSin.LargeImageIndexDisabled = 0;
            this.btnSCDSin.Name = "btnSCDSin";
            this.btnSCDSin.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSCDSing_ItemClick);
            // 
            // btnSCDOPL
            // 
            this.btnSCDOPL.Caption = "三测单多人录入";
            this.btnSCDOPL.Id = 15;
            this.btnSCDOPL.LargeImageIndex = 3;
            this.btnSCDOPL.LargeImageIndexDisabled = 0;
            this.btnSCDOPL.Name = "btnSCDOPL";
            this.btnSCDOPL.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSCDPL_ItemClick);
            // 
            // btnHLDSing
            // 
            this.btnHLDSing.Caption = "护理单单人录入";
            this.btnHLDSing.Id = 16;
            this.btnHLDSing.LargeImageIndex = 4;
            this.btnHLDSing.Name = "btnHLDSing";
            this.btnHLDSing.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnJLDSing_ItemClick);
            // 
            // btnHLDPL
            // 
            this.btnHLDPL.Caption = "护理单多人录入";
            this.btnHLDPL.Id = 17;
            this.btnHLDPL.LargeImageIndex = 1;
            this.btnHLDPL.Name = "btnHLDPL";
            this.btnHLDPL.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHLDPL_ItemClick);
            // 
            // barLargeButtonItem1
            // 
            this.barLargeButtonItem1.Caption = "    退出    ";
            this.barLargeButtonItem1.Id = 18;
            this.barLargeButtonItem1.LargeImageIndex = 5;
            this.barLargeButtonItem1.Name = "barLargeButtonItem1";
            this.barLargeButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItem1_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(816, 71);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 507);
            this.barDockControlBottom.Size = new System.Drawing.Size(816, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 71);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 436);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(816, 71);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 436);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "病案借阅.png");
            this.imageList1.Images.SetKeyName(1, "体征单（多人）.png");
            this.imageList1.Images.SetKeyName(2, "sancedan.png");
            this.imageList1.Images.SetKeyName(3, "三测单（小）.png");
            this.imageList1.Images.SetKeyName(4, "体征单(单人).png");
            this.imageList1.Images.SetKeyName(5, "退出2.png");
            // 
            // ThreeRecordMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 507);
            this.Controls.Add(this.tabControlSCD);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ThreeRecordMain";
            this.Text = "护理数据录入";
            this.Load += new System.EventHandler(this.ThreeRecordMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlSCD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabControlSCD;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;

        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraBars.BarLargeButtonItem btnSCDSin;
        private DevExpress.XtraBars.BarLargeButtonItem btnSCDOPL;
        private DevExpress.XtraBars.BarLargeButtonItem btnHLDSing;
        private DevExpress.XtraBars.BarLargeButtonItem btnHLDPL;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItem1;
    }
}