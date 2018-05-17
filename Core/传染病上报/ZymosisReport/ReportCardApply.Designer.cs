namespace DrectSoft.Core.ZymosisReport
{
    partial class ReportCardApply
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportCardApply));
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barLargeButtonItemSave = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemSubmit = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemDelete = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItem1 = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemExit = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.panelReportCard = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barLargeButtonItemSave,
            this.barLargeButtonItemDelete,
            this.barLargeButtonItemSubmit,
            this.barLargeButtonItemExit,
            this.barLargeButtonItem1});
            this.barManager1.LargeImages = this.imageList1;
            this.barManager1.MaxItemId = 5;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemSubmit, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemDelete, true),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barLargeButtonItem1, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemExit, true)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // barLargeButtonItemSave
            // 
            this.barLargeButtonItemSave.Caption = "保存 (&S)";
            this.barLargeButtonItemSave.Id = 0;
            this.barLargeButtonItemSave.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F1);
            this.barLargeButtonItemSave.LargeImageIndex = 27;
            this.barLargeButtonItemSave.Name = "barLargeButtonItemSave";
            this.barLargeButtonItemSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemSave_ItemClick);
            // 
            // barLargeButtonItemSubmit
            // 
            this.barLargeButtonItemSubmit.Caption = "提交 (&M)";
            this.barLargeButtonItemSubmit.Id = 2;
            this.barLargeButtonItemSubmit.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3);
            this.barLargeButtonItemSubmit.LargeImageIndex = 29;
            this.barLargeButtonItemSubmit.Name = "barLargeButtonItemSubmit";
            this.barLargeButtonItemSubmit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemSubmit_ItemClick);
            // 
            // barLargeButtonItemDelete
            // 
            this.barLargeButtonItemDelete.Caption = "删除 (&D)";
            this.barLargeButtonItemDelete.Id = 1;
            this.barLargeButtonItemDelete.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2);
            this.barLargeButtonItemDelete.LargeImageIndex = 28;
            this.barLargeButtonItemDelete.Name = "barLargeButtonItemDelete";
            this.barLargeButtonItemDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemDelete_ItemClick);
            // 
            // barLargeButtonItem1
            // 
            this.barLargeButtonItem1.Caption = "清空 (&L)";
            this.barLargeButtonItem1.Id = 4;
            this.barLargeButtonItem1.LargeImageIndex = 26;
            this.barLargeButtonItem1.Name = "barLargeButtonItem1";
            this.barLargeButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItem1_ItemClick);
            // 
            // barLargeButtonItemExit
            // 
            this.barLargeButtonItemExit.Caption = "退出 (&T)";
            this.barLargeButtonItemExit.Id = 3;
            this.barLargeButtonItemExit.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4);
            this.barLargeButtonItemExit.LargeImageIndex = 30;
            this.barLargeButtonItemExit.Name = "barLargeButtonItemExit";
            this.barLargeButtonItemExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemExit_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(749, 79);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 708);
            this.barDockControlBottom.Size = new System.Drawing.Size(749, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 79);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 629);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(749, 79);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 629);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "11.png");
            this.imageList1.Images.SetKeyName(1, "12.png");
            this.imageList1.Images.SetKeyName(2, "15.png");
            this.imageList1.Images.SetKeyName(3, "10.png");
            this.imageList1.Images.SetKeyName(4, "7.png");
            this.imageList1.Images.SetKeyName(5, "14.png");
            this.imageList1.Images.SetKeyName(6, "1.png");
            this.imageList1.Images.SetKeyName(7, "13.png");
            this.imageList1.Images.SetKeyName(8, "save.bmp");
            this.imageList1.Images.SetKeyName(9, "12.png");
            this.imageList1.Images.SetKeyName(10, "8.png");
            this.imageList1.Images.SetKeyName(11, "2.png");
            this.imageList1.Images.SetKeyName(12, "ftp.png");
            this.imageList1.Images.SetKeyName(13, "25.png");
            this.imageList1.Images.SetKeyName(14, "20.png");
            this.imageList1.Images.SetKeyName(15, "error.png");
            this.imageList1.Images.SetKeyName(16, "2.png");
            this.imageList1.Images.SetKeyName(17, "3.png");
            this.imageList1.Images.SetKeyName(18, "withdraw.png");
            this.imageList1.Images.SetKeyName(19, "save.png");
            this.imageList1.Images.SetKeyName(20, "new.png");
            this.imageList1.Images.SetKeyName(21, "exit.png");
            this.imageList1.Images.SetKeyName(22, "report.png");
            this.imageList1.Images.SetKeyName(23, "reject.png");
            this.imageList1.Images.SetKeyName(24, "find.png");
            this.imageList1.Images.SetKeyName(25, "find.png");
            this.imageList1.Images.SetKeyName(26, "清空.ico");
            this.imageList1.Images.SetKeyName(27, "保存.png");
            this.imageList1.Images.SetKeyName(28, "删除.png");
            this.imageList1.Images.SetKeyName(29, "提交.png");
            this.imageList1.Images.SetKeyName(30, "退出2.png");
            // 
            // panelReportCard
            // 
            this.panelReportCard.AutoScroll = true;
            this.panelReportCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelReportCard.Location = new System.Drawing.Point(0, 79);
            this.panelReportCard.Name = "panelReportCard";
            this.panelReportCard.Size = new System.Drawing.Size(749, 629);
            this.panelReportCard.TabIndex = 4;
            // 
            // ReportCardApply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(749, 708);
            this.Controls.Add(this.panelReportCard);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportCardApply";
            this.Text = "传染病上报";
            this.Load += new System.EventHandler(this.ReportCardApply_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemSave;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemDelete;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemSubmit;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemExit;
        private System.Windows.Forms.Panel panelReportCard;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItem1;
    }
}