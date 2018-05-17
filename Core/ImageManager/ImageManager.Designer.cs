namespace DrectSoft.Core.ImageManager
{
    partial class ImageManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageManager));
            this.openFileDialogImage = new System.Windows.Forms.OpenFileDialog();
            this.panelControlTop = new DevExpress.XtraEditors.PanelControl();
            this.gridImage = new DevExpress.XtraGrid.GridControl();
            this.layoutViewImage = new DevExpress.XtraGrid.Views.Layout.LayoutView();
            this.ColumnName = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_layoutViewColumn1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.ColumnMemo = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.layoutViewField_layoutViewColumn2 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.ColumnImage = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.layoutViewField_layoutViewColumn1_1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.layoutViewCard1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewCard();
            this.panelControlbottom = new DevExpress.XtraEditors.PanelControl();
            this.groupControlInfo = new DevExpress.XtraEditors.GroupControl();
            this.panelImage = new System.Windows.Forms.Panel();
            this.pictureEdit = new DevExpress.XtraEditors.PictureEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.memoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textName = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelMemo = new DevExpress.XtraEditors.LabelControl();
            this.groupControlTool = new DevExpress.XtraEditors.GroupControl();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonDel = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonModify = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonSaveNew = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btn_LoadImage = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).BeginInit();
            this.panelControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn1_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlbottom)).BeginInit();
            this.panelControlbottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlInfo)).BeginInit();
            this.groupControlInfo.SuspendLayout();
            this.panelImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTool)).BeginInit();
            this.groupControlTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialogImage
            // 
            this.openFileDialogImage.Filter = resources.GetString("openFileDialogImage.Filter");
            // 
            // panelControlTop
            // 
            this.panelControlTop.Controls.Add(this.gridImage);
            this.panelControlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlTop.Location = new System.Drawing.Point(0, 0);
            this.panelControlTop.Name = "panelControlTop";
            this.panelControlTop.Size = new System.Drawing.Size(586, 656);
            this.panelControlTop.TabIndex = 1;
            // 
            // gridImage
            // 
            this.gridImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImage.Location = new System.Drawing.Point(2, 2);
            this.gridImage.MainView = this.layoutViewImage;
            this.gridImage.Name = "gridImage";
            this.gridImage.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemPictureEdit1,
            this.repositoryItemMemoEdit1});
            this.gridImage.Size = new System.Drawing.Size(582, 652);
            this.gridImage.TabIndex = 0;
            this.gridImage.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.layoutViewImage});
            // 
            // layoutViewImage
            // 
            this.layoutViewImage.CardCaptionFormat = "图片{0}";
            this.layoutViewImage.CardMinSize = new System.Drawing.Size(476, 330);
            this.layoutViewImage.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] {
            this.ColumnName,
            this.ColumnMemo,
            this.ColumnImage});
            this.layoutViewImage.GridControl = this.gridImage;
            this.layoutViewImage.Name = "layoutViewImage";
            this.layoutViewImage.OptionsBehavior.AllowRuntimeCustomization = false;
            this.layoutViewImage.OptionsBehavior.ScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.layoutViewImage.OptionsCustomization.AllowFilter = false;
            this.layoutViewImage.OptionsCustomization.AllowSort = false;
            this.layoutViewImage.OptionsCustomization.ShowGroupLayoutTreeView = false;
            this.layoutViewImage.OptionsCustomization.ShowGroupView = false;
            this.layoutViewImage.OptionsCustomization.ShowSaveLoadLayoutButtons = false;
            this.layoutViewImage.OptionsFilter.AllowColumnMRUFilterList = false;
            this.layoutViewImage.OptionsFilter.AllowFilterEditor = false;
            this.layoutViewImage.OptionsFilter.AllowMRUFilterList = false;
            this.layoutViewImage.OptionsView.AllowHotTrackFields = false;
            this.layoutViewImage.OptionsView.ViewMode = DevExpress.XtraGrid.Views.Layout.LayoutViewMode.Carousel;
            this.layoutViewImage.PaintStyleName = "Style3D";
            this.layoutViewImage.TemplateCard = this.layoutViewCard1;
            this.layoutViewImage.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.layoutViewImage_FocusedRowChanged);
            // 
            // ColumnName
            // 
            this.ColumnName.Caption = "名称";
            this.ColumnName.FieldName = "NAME";
            this.ColumnName.LayoutViewField = this.layoutViewField_layoutViewColumn1;
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.OptionsColumn.AllowEdit = false;
            // 
            // layoutViewField_layoutViewColumn1
            // 
            this.layoutViewField_layoutViewColumn1.EditorPreferredWidth = 135;
            this.layoutViewField_layoutViewColumn1.Location = new System.Drawing.Point(288, 0);
            this.layoutViewField_layoutViewColumn1.Name = "layoutViewField_layoutViewColumn1";
            this.layoutViewField_layoutViewColumn1.Size = new System.Drawing.Size(170, 18);
            this.layoutViewField_layoutViewColumn1.TextSize = new System.Drawing.Size(28, 14);
            // 
            // ColumnMemo
            // 
            this.ColumnMemo.Caption = "备注";
            this.ColumnMemo.ColumnEdit = this.repositoryItemMemoEdit1;
            this.ColumnMemo.FieldName = "MEMO";
            this.ColumnMemo.LayoutViewField = this.layoutViewField_layoutViewColumn2;
            this.ColumnMemo.Name = "ColumnMemo";
            this.ColumnMemo.OptionsColumn.AllowEdit = false;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // layoutViewField_layoutViewColumn2
            // 
            this.layoutViewField_layoutViewColumn2.EditorPreferredWidth = 135;
            this.layoutViewField_layoutViewColumn2.Location = new System.Drawing.Point(288, 18);
            this.layoutViewField_layoutViewColumn2.Name = "layoutViewField_layoutViewColumn2";
            this.layoutViewField_layoutViewColumn2.Size = new System.Drawing.Size(170, 23);
            this.layoutViewField_layoutViewColumn2.TextSize = new System.Drawing.Size(28, 14);
            // 
            // ColumnImage
            // 
            this.ColumnImage.ColumnEdit = this.repositoryItemPictureEdit1;
            this.ColumnImage.FieldName = "IMAGE";
            this.ColumnImage.LayoutViewField = this.layoutViewField_layoutViewColumn1_1;
            this.ColumnImage.Name = "ColumnImage";
            this.ColumnImage.OptionsColumn.AllowEdit = false;
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            // 
            // layoutViewField_layoutViewColumn1_1
            // 
            this.layoutViewField_layoutViewColumn1_1.EditorPreferredWidth = 284;
            this.layoutViewField_layoutViewColumn1_1.Location = new System.Drawing.Point(0, 0);
            this.layoutViewField_layoutViewColumn1_1.Name = "layoutViewField_layoutViewColumn1_1";
            this.layoutViewField_layoutViewColumn1_1.Size = new System.Drawing.Size(288, 41);
            this.layoutViewField_layoutViewColumn1_1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutViewField_layoutViewColumn1_1.TextToControlDistance = 0;
            this.layoutViewField_layoutViewColumn1_1.TextVisible = false;
            // 
            // layoutViewCard1
            // 
            this.layoutViewCard1.CustomizationFormText = "layoutViewCard1";
            this.layoutViewCard1.ExpandButtonLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.layoutViewCard1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutViewField_layoutViewColumn1_1,
            this.layoutViewField_layoutViewColumn1,
            this.layoutViewField_layoutViewColumn2});
            this.layoutViewCard1.Name = "layoutViewTemplateCard";
            this.layoutViewCard1.ShowInCustomizationForm = false;
            this.layoutViewCard1.Text = "TemplateCard";
            // 
            // panelControlbottom
            // 
            this.panelControlbottom.Controls.Add(this.groupControlInfo);
            this.panelControlbottom.Controls.Add(this.groupControlTool);
            this.panelControlbottom.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControlbottom.Location = new System.Drawing.Point(586, 0);
            this.panelControlbottom.Name = "panelControlbottom";
            this.panelControlbottom.Size = new System.Drawing.Size(651, 656);
            this.panelControlbottom.TabIndex = 1;
            // 
            // groupControlInfo
            // 
            this.groupControlInfo.Controls.Add(this.panelImage);
            this.groupControlInfo.Controls.Add(this.panel1);
            this.groupControlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlInfo.Location = new System.Drawing.Point(2, 2);
            this.groupControlInfo.Name = "groupControlInfo";
            this.groupControlInfo.Size = new System.Drawing.Size(647, 587);
            this.groupControlInfo.TabIndex = 3;
            this.groupControlInfo.Text = "  图片信息";
            // 
            // panelImage
            // 
            this.panelImage.Controls.Add(this.pictureEdit);
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(2, 210);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(643, 375);
            this.panelImage.TabIndex = 7;
            // 
            // pictureEdit
            // 
            this.pictureEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit.Name = "pictureEdit";
            this.pictureEdit.Size = new System.Drawing.Size(643, 375);
            this.pictureEdit.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.memoEdit);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.textName);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.labelMemo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(643, 188);
            this.panel1.TabIndex = 6;
            // 
            // memoEdit
            // 
            this.memoEdit.Location = new System.Drawing.Point(93, 83);
            this.memoEdit.Name = "memoEdit";
            this.memoEdit.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.memoEdit.Size = new System.Drawing.Size(289, 71);
            this.memoEdit.TabIndex = 3;
            this.memoEdit.UseOptimizedRendering = true;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(35, 160);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "图片";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(93, 28);
            this.textName.Name = "textName";
            this.textName.Properties.ContextMenuStrip = this.contextMenuStrip;
            this.textName.Size = new System.Drawing.Size(163, 20);
            this.textName.TabIndex = 0;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(35, 31);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "名称";
            // 
            // labelMemo
            // 
            this.labelMemo.Location = new System.Drawing.Point(35, 85);
            this.labelMemo.Name = "labelMemo";
            this.labelMemo.Size = new System.Drawing.Size(24, 14);
            this.labelMemo.TabIndex = 2;
            this.labelMemo.Text = "描述";
            // 
            // groupControlTool
            // 
            this.groupControlTool.Controls.Add(this.btnAdd);
            this.groupControlTool.Controls.Add(this.ButtonDel);
            this.groupControlTool.Controls.Add(this.ButtonModify);
            this.groupControlTool.Controls.Add(this.ButtonSaveNew);
            this.groupControlTool.Controls.Add(this.groupControl1);
            this.groupControlTool.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControlTool.Location = new System.Drawing.Point(2, 589);
            this.groupControlTool.Name = "groupControlTool";
            this.groupControlTool.Size = new System.Drawing.Size(647, 65);
            this.groupControlTool.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::DrectSoft.Core.ImageManager.Properties.Resources.新增;
            this.btnAdd.Location = new System.Drawing.Point(30, 35);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "新增(&N)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ButtonDel
            // 
            this.ButtonDel.Image = global::DrectSoft.Core.ImageManager.Properties.Resources.删除;
            this.ButtonDel.Location = new System.Drawing.Point(309, 35);
            this.ButtonDel.Name = "ButtonDel";
            this.ButtonDel.Size = new System.Drawing.Size(80, 23);
            this.ButtonDel.TabIndex = 3;
            this.ButtonDel.Text = "删除(&D)";
            this.ButtonDel.Click += new System.EventHandler(this.ButtonDel_Click);
            // 
            // ButtonModify
            // 
            this.ButtonModify.Image = global::DrectSoft.Core.ImageManager.Properties.Resources.编辑;
            this.ButtonModify.Location = new System.Drawing.Point(216, 35);
            this.ButtonModify.Name = "ButtonModify";
            this.ButtonModify.Size = new System.Drawing.Size(80, 23);
            this.ButtonModify.TabIndex = 2;
            this.ButtonModify.Text = "编辑(&E)";
            this.ButtonModify.Click += new System.EventHandler(this.ButtonModify_Click);
            // 
            // ButtonSaveNew
            // 
            this.ButtonSaveNew.Enabled = false;
            this.ButtonSaveNew.Image = global::DrectSoft.Core.ImageManager.Properties.Resources.取消;
            this.ButtonSaveNew.Location = new System.Drawing.Point(402, 35);
            this.ButtonSaveNew.Name = "ButtonSaveNew";
            this.ButtonSaveNew.Size = new System.Drawing.Size(80, 23);
            this.ButtonSaveNew.TabIndex = 1;
            this.ButtonSaveNew.Text = "取消(&C)";
            this.ButtonSaveNew.Click += new System.EventHandler(this.ButtonSaveNew_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btn_LoadImage);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl1.Location = new System.Drawing.Point(2, 7);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(643, 56);
            this.groupControl1.TabIndex = 3;
            // 
            // btn_LoadImage
            // 
            this.btn_LoadImage.Location = new System.Drawing.Point(123, 28);
            this.btn_LoadImage.Name = "btn_LoadImage";
            this.btn_LoadImage.Size = new System.Drawing.Size(80, 23);
            this.btn_LoadImage.TabIndex = 5;
            this.btn_LoadImage.Text = "加载图片";
            this.btn_LoadImage.Click += new System.EventHandler(this.btn_LoadImage_Click);
            // 
            // ImageManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1237, 656);
            this.Controls.Add(this.panelControlTop);
            this.Controls.Add(this.panelControlbottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImageManager";
            this.ShowIcon = false;
            this.Text = "医学图像维护";
            this.Load += new System.EventHandler(this.ImageManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).EndInit();
            this.panelControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn1_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlbottom)).EndInit();
            this.panelControlbottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlInfo)).EndInit();
            this.groupControlInfo.ResumeLayout(false);
            this.panelImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTool)).EndInit();
            this.groupControlTool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton ButtonSaveNew;
        private DevExpress.XtraEditors.SimpleButton ButtonDel;
        private DevExpress.XtraEditors.SimpleButton ButtonModify;
        private DevExpress.XtraEditors.SimpleButton btnAdd;

        private System.Windows.Forms.OpenFileDialog openFileDialogImage;
        private DevExpress.XtraEditors.PanelControl panelControlTop;
        private DevExpress.XtraGrid.GridControl gridImage;
        private DevExpress.XtraEditors.PanelControl panelControlbottom;
        private DevExpress.XtraGrid.Views.Layout.LayoutView layoutViewImage;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn ColumnName;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn ColumnMemo;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn ColumnImage;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.GroupControl groupControlInfo;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.MemoEdit memoEdit;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelMemo;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn2;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn1_1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewCard layoutViewCard1;
        private DevExpress.XtraEditors.GroupControl groupControlTool;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit;
        private DevExpress.XtraEditors.SimpleButton btn_LoadImage;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;


    }
}