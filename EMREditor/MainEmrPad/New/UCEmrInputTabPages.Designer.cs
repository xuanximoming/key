namespace DrectSoft.Core.MainEmrPad.New
{
    partial class UCEmrInputTabPages
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCEmrInputTabPages));
            this.xtraTabControlEmrContent = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageIEMMainPage = new DevExpress.XtraTab.XtraTabPage();
            this.toolTipControllerSplit = new DevExpress.Utils.ToolTipController(this.components);
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlEmrContent)).BeginInit();
            this.xtraTabControlEmrContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControlEmrContent
            // 
            this.xtraTabControlEmrContent.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
            this.xtraTabControlEmrContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControlEmrContent.HeaderAutoFill = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControlEmrContent.HeaderButtons = ((DevExpress.XtraTab.TabButtons)(((DevExpress.XtraTab.TabButtons.Prev | DevExpress.XtraTab.TabButtons.Next) 
            | DevExpress.XtraTab.TabButtons.Close)));
            this.xtraTabControlEmrContent.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControlEmrContent.Name = "xtraTabControlEmrContent";
            this.xtraTabControlEmrContent.SelectedTabPage = this.xtraTabPageIEMMainPage;
            this.xtraTabControlEmrContent.ShowToolTips = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControlEmrContent.Size = new System.Drawing.Size(647, 448);
            this.xtraTabControlEmrContent.TabIndex = 0;
            this.xtraTabControlEmrContent.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageIEMMainPage});
            this.xtraTabControlEmrContent.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControlEmrContent_SelectedPageChanged);
            this.xtraTabControlEmrContent.CloseButtonClick += new System.EventHandler(this.xtraTabControlEmrContent_CloseButtonClick);
            // 
            // xtraTabPageIEMMainPage
            // 
            this.xtraTabPageIEMMainPage.Name = "xtraTabPageIEMMainPage";
            this.xtraTabPageIEMMainPage.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabPageIEMMainPage.Size = new System.Drawing.Size(641, 419);
            this.xtraTabPageIEMMainPage.Text = "病案首页";
            // 
            // toolTipControllerSplit
            // 
            this.toolTipControllerSplit.ImageList = this.imageCollection1;
            this.toolTipControllerSplit.Rounded = true;
            this.toolTipControllerSplit.ShowBeak = true;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageSize = new System.Drawing.Size(40, 40);
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "updown.png");
            // 
            // UCEmrInputTabPages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControlEmrContent);
            this.Name = "UCEmrInputTabPages";
            this.Size = new System.Drawing.Size(647, 448);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlEmrContent)).EndInit();
            this.xtraTabControlEmrContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControlEmrContent;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageIEMMainPage;
        private DevExpress.Utils.ToolTipController toolTipControllerSplit;
        private DevExpress.Utils.ImageCollection imageCollection1;
    }
}
