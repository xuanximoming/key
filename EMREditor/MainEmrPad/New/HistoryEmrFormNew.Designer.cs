namespace DrectSoft.Core.MainEmrPad
{
    partial class HistoryEmrFormNew
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryEmrFormNew));
            this.panelEmrContent = new System.Windows.Forms.Panel();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barLargeButtonItemCopy = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemInsert = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItemClose = new DevExpress.XtraBars.BarLargeButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEmrContent
            // 
            this.panelEmrContent.AutoScroll = true;
            this.panelEmrContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmrContent.Location = new System.Drawing.Point(0, 60);
            this.panelEmrContent.Name = "panelEmrContent";
            this.panelEmrContent.Size = new System.Drawing.Size(832, 509);
            this.panelEmrContent.TabIndex = 2;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barLargeButtonItemCopy,
            this.barLargeButtonItemInsert,
            this.barLargeButtonItemClose});
            this.barManager1.LargeImages = this.imageCollection1;
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 6;
            this.barManager1.StatusBar = this.bar1;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemCopy),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemInsert),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItemClose)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barLargeButtonItemCopy
            // 
            this.barLargeButtonItemCopy.Caption = "复制";
            this.barLargeButtonItemCopy.Id = 3;
            this.barLargeButtonItemCopy.LargeImageIndex = 0;
            this.barLargeButtonItemCopy.Name = "barLargeButtonItemCopy";
            toolTipTitleItem1.Text = "复制";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "复制选中的病历内容";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.barLargeButtonItemCopy.SuperTip = superToolTip1;
            this.barLargeButtonItemCopy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemCopy_ItemClick);
            // 
            // barLargeButtonItemInsert
            // 
            this.barLargeButtonItemInsert.Caption = "插入";
            this.barLargeButtonItemInsert.Id = 4;
            this.barLargeButtonItemInsert.LargeImageIndex = 1;
            this.barLargeButtonItemInsert.Name = "barLargeButtonItemInsert";
            toolTipTitleItem2.Text = "插入";
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "将选中的内容插入病历并关闭窗体";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            this.barLargeButtonItemInsert.SuperTip = superToolTip2;
            this.barLargeButtonItemInsert.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemInsert_ItemClick);
            // 
            // barLargeButtonItemClose
            // 
            this.barLargeButtonItemClose.Caption = "关闭";
            this.barLargeButtonItemClose.Id = 5;
            this.barLargeButtonItemClose.LargeImageIndex = 2;
            this.barLargeButtonItemClose.Name = "barLargeButtonItemClose";
            toolTipTitleItem3.Text = "关闭";
            toolTipItem3.LeftIndent = 6;
            toolTipItem3.Text = "关闭窗体";
            superToolTip3.Items.Add(toolTipTitleItem3);
            superToolTip3.Items.Add(toolTipItem3);
            this.barLargeButtonItemClose.SuperTip = superToolTip3;
            this.barLargeButtonItemClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItemClose_ItemClick);
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 3";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 3";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(832, 60);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 569);
            this.barDockControlBottom.Size = new System.Drawing.Size(832, 23);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 60);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 509);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(832, 60);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 509);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageSize = new System.Drawing.Size(32, 32);
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "Copy_32.png");
            this.imageCollection1.Images.SetKeyName(1, "RecordEditor_32.png");
            this.imageCollection1.Images.SetKeyName(2, "Delete_32.png");
            // 
            // HistoryEmrFormNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 592);
            this.Controls.Add(this.panelEmrContent);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "HistoryEmrFormNew";
            this.Text = "历史病历";
            this.Load += new System.EventHandler(this.HistoryEmrForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelEmrContent;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemCopy;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemInsert;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItemClose;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraBars.Bar bar1;
    }
}