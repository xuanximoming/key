namespace DrectSoft.Core.MainEmrPad.New
{
    partial class UCEmrInputPreView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCEmrInputPreView));
            this.panelControlTop = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonUp = new DevExpress.XtraEditors.SimpleButton();
            this.imageCollectionBtn = new DevExpress.Utils.ImageCollection();
            this.toolTipControllerBtnUp = new DevExpress.Utils.ToolTipController();
            this.imageCollectionBtnTip = new DevExpress.Utils.ImageCollection();
            this.simpleButtonRestore = new DevExpress.XtraEditors.SimpleButton();
            this.toolTipControllerBtnRestore = new DevExpress.Utils.ToolTipController();
            this.simpleButtonDown = new DevExpress.XtraEditors.SimpleButton();
            this.toolTipControllerBtnDown = new DevExpress.Utils.ToolTipController();
            this.panelControlBody = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).BeginInit();
            this.panelControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionBtnTip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBody)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlTop
            // 
            this.panelControlTop.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControlTop.Controls.Add(this.simpleButtonUp);
            this.panelControlTop.Controls.Add(this.simpleButtonRestore);
            this.panelControlTop.Controls.Add(this.simpleButtonDown);
            this.panelControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlTop.Location = new System.Drawing.Point(0, 0);
            this.panelControlTop.Name = "panelControlTop";
            this.panelControlTop.Size = new System.Drawing.Size(472, 25);
            this.panelControlTop.TabIndex = 0;
            this.panelControlTop.SizeChanged += new System.EventHandler(this.panelControlTop_SizeChanged);
            // 
            // simpleButtonUp
            // 
            this.simpleButtonUp.Appearance.BackColor = System.Drawing.Color.White;
            this.simpleButtonUp.Appearance.BackColor2 = System.Drawing.Color.White;
            this.simpleButtonUp.Appearance.BorderColor = System.Drawing.Color.White;
            this.simpleButtonUp.Appearance.Options.UseBackColor = true;
            this.simpleButtonUp.Appearance.Options.UseBorderColor = true;
            this.simpleButtonUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButtonUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleButtonUp.ImageIndex = 2;
            this.simpleButtonUp.ImageList = this.imageCollectionBtn;
            this.simpleButtonUp.Location = new System.Drawing.Point(153, 0);
            this.simpleButtonUp.Name = "simpleButtonUp";
            this.simpleButtonUp.Size = new System.Drawing.Size(168, 25);
            this.simpleButtonUp.TabIndex = 2;
            this.simpleButtonUp.Text = "全屏显示更多病程";
            this.simpleButtonUp.ToolTip = "点击后全屏显示下方的病程预览区，上方的病程编辑区域将会被隐藏";
            this.simpleButtonUp.ToolTipController = this.toolTipControllerBtnUp;
            this.simpleButtonUp.ToolTipTitle = "全屏显示更多病程";
            this.simpleButtonUp.Click += new System.EventHandler(this.simpleButtonUp_Click);
            // 
            // imageCollectionBtn
            // 
            this.imageCollectionBtn.ImageSize = new System.Drawing.Size(20, 20);
            this.imageCollectionBtn.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollectionBtn.ImageStream")));
            this.imageCollectionBtn.Images.SetKeyName(0, "Down.png");
            this.imageCollectionBtn.Images.SetKeyName(1, "Restore.png");
            this.imageCollectionBtn.Images.SetKeyName(2, "Up.png");
            // 
            // toolTipControllerBtnUp
            // 
            this.toolTipControllerBtnUp.AutoPopDelay = 10000;
            this.toolTipControllerBtnUp.IconSize = DevExpress.Utils.ToolTipIconSize.Large;
            this.toolTipControllerBtnUp.ImageIndex = 2;
            this.toolTipControllerBtnUp.ImageList = this.imageCollectionBtnTip;
            this.toolTipControllerBtnUp.Rounded = true;
            this.toolTipControllerBtnUp.ShowBeak = true;
            this.toolTipControllerBtnUp.ToolTipLocation = DevExpress.Utils.ToolTipLocation.TopRight;
            // 
            // imageCollectionBtnTip
            // 
            this.imageCollectionBtnTip.ImageSize = new System.Drawing.Size(40, 40);
            this.imageCollectionBtnTip.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollectionBtnTip.ImageStream")));
            this.imageCollectionBtnTip.Images.SetKeyName(0, "Down.png");
            this.imageCollectionBtnTip.Images.SetKeyName(1, "Restore.png");
            this.imageCollectionBtnTip.Images.SetKeyName(2, "Up.png");
            // 
            // simpleButtonRestore
            // 
            this.simpleButtonRestore.Appearance.BackColor = System.Drawing.Color.White;
            this.simpleButtonRestore.Appearance.BackColor2 = System.Drawing.Color.White;
            this.simpleButtonRestore.Appearance.BorderColor = System.Drawing.Color.White;
            this.simpleButtonRestore.Appearance.Options.UseBackColor = true;
            this.simpleButtonRestore.Appearance.Options.UseBorderColor = true;
            this.simpleButtonRestore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.simpleButtonRestore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButtonRestore.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButtonRestore.ImageIndex = 1;
            this.simpleButtonRestore.ImageList = this.imageCollectionBtn;
            this.simpleButtonRestore.Location = new System.Drawing.Point(0, 0);
            this.simpleButtonRestore.Name = "simpleButtonRestore";
            this.simpleButtonRestore.Size = new System.Drawing.Size(153, 25);
            this.simpleButtonRestore.TabIndex = 1;
            this.simpleButtonRestore.Text = "恢复病程预览区";
            this.simpleButtonRestore.ToolTip = "点击后恢复预览区至原始状态，病程编辑区域在上方，病程预览区在下方";
            this.simpleButtonRestore.ToolTipController = this.toolTipControllerBtnRestore;
            this.simpleButtonRestore.ToolTipTitle = "恢复病程预览区";
            this.simpleButtonRestore.Click += new System.EventHandler(this.simpleButtonRestore_Click);
            // 
            // toolTipControllerBtnRestore
            // 
            this.toolTipControllerBtnRestore.AutoPopDelay = 10000;
            this.toolTipControllerBtnRestore.IconSize = DevExpress.Utils.ToolTipIconSize.Large;
            this.toolTipControllerBtnRestore.ImageIndex = 1;
            this.toolTipControllerBtnRestore.ImageList = this.imageCollectionBtnTip;
            this.toolTipControllerBtnRestore.Rounded = true;
            this.toolTipControllerBtnRestore.ShowBeak = true;
            this.toolTipControllerBtnRestore.ToolTipLocation = DevExpress.Utils.ToolTipLocation.TopRight;
            // 
            // simpleButtonDown
            // 
            this.simpleButtonDown.Appearance.BackColor = System.Drawing.Color.White;
            this.simpleButtonDown.Appearance.BackColor2 = System.Drawing.Color.White;
            this.simpleButtonDown.Appearance.BorderColor = System.Drawing.Color.White;
            this.simpleButtonDown.Appearance.Options.UseBackColor = true;
            this.simpleButtonDown.Appearance.Options.UseBorderColor = true;
            this.simpleButtonDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButtonDown.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonDown.ImageIndex = 0;
            this.simpleButtonDown.ImageList = this.imageCollectionBtn;
            this.simpleButtonDown.Location = new System.Drawing.Point(321, 0);
            this.simpleButtonDown.Name = "simpleButtonDown";
            this.simpleButtonDown.Size = new System.Drawing.Size(151, 25);
            this.simpleButtonDown.TabIndex = 0;
            this.simpleButtonDown.Text = "隐藏病程预览区";
            this.simpleButtonDown.ToolTip = "点击后隐藏下方的病程预览区，上方的病程编辑区域将全屏显示";
            this.simpleButtonDown.ToolTipController = this.toolTipControllerBtnDown;
            this.simpleButtonDown.ToolTipTitle = "隐藏病程预览区";
            this.simpleButtonDown.Click += new System.EventHandler(this.simpleButtonDown_Click);
            // 
            // toolTipControllerBtnDown
            // 
            this.toolTipControllerBtnDown.AutoPopDelay = 10000;
            this.toolTipControllerBtnDown.IconSize = DevExpress.Utils.ToolTipIconSize.Large;
            this.toolTipControllerBtnDown.ImageIndex = 0;
            this.toolTipControllerBtnDown.ImageList = this.imageCollectionBtnTip;
            this.toolTipControllerBtnDown.Rounded = true;
            this.toolTipControllerBtnDown.ShowBeak = true;
            this.toolTipControllerBtnDown.ToolTipLocation = DevExpress.Utils.ToolTipLocation.TopRight;
            // 
            // panelControlBody
            // 
            this.panelControlBody.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlBody.Location = new System.Drawing.Point(0, 25);
            this.panelControlBody.Name = "panelControlBody";
            this.panelControlBody.Size = new System.Drawing.Size(472, 449);
            this.panelControlBody.TabIndex = 2;
            // 
            // UCEmrInputPreView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControlBody);
            this.Controls.Add(this.panelControlTop);
            this.Name = "UCEmrInputPreView";
            this.Size = new System.Drawing.Size(472, 474);
            this.Load += new System.EventHandler(this.UCEmrInputPreView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).EndInit();
            this.panelControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionBtnTip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBody)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlTop;
        private DevExpress.XtraEditors.SimpleButton simpleButtonDown;
        private DevExpress.XtraEditors.PanelControl panelControlBody;
        private DevExpress.XtraEditors.SimpleButton simpleButtonRestore;
        private DevExpress.Utils.ToolTipController toolTipControllerBtnRestore;
        private DevExpress.XtraEditors.SimpleButton simpleButtonUp;
        private DevExpress.Utils.ImageCollection imageCollectionBtn;
        private DevExpress.Utils.ImageCollection imageCollectionBtnTip;
        private DevExpress.Utils.ToolTipController toolTipControllerBtnUp;
        private DevExpress.Utils.ToolTipController toolTipControllerBtnDown;

    }
}
