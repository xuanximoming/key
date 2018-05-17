namespace DrectSoft.EMR.ThreeRecordAll
{
    partial class ToolForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolForm));
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroupTSZF = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroupControlContainer1 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.imageListBoxControlSymbol = new DevExpress.XtraEditors.ImageListBoxControl();
            this.navBarGroupControlContainer2 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.treeListPersonTemplate = new DevExpress.XtraTreeList.TreeList();
            this.colCode = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageCollection_Tree = new DevExpress.Utils.ImageCollection();
            this.toolTipControllerTreeList = new DevExpress.Utils.ToolTipController();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_SearchTemplete = new DevExpress.XtraEditors.SimpleButton();
            this.txt_SearchTemplete = new DevExpress.XtraEditors.TextEdit();
            this.navBarGroupKSMB = new DevExpress.XtraNavBar.NavBarGroup();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnRight_DeptTemplete = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenu_DeptTemplate = new DevExpress.XtraBars.PopupMenu();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            this.navBarControl1.SuspendLayout();
            this.navBarGroupControlContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControlSymbol)).BeginInit();
            this.navBarGroupControlContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListPersonTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection_Tree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_SearchTemplete.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu_DeptTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.navBarGroupTSZF;
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer1);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer2);
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroupTSZF,
            this.navBarGroupKSMB});
            this.navBarControl1.Location = new System.Drawing.Point(0, 0);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 186;
            this.navBarControl1.OptionsNavPane.ShowExpandButton = false;
            this.navBarControl1.OptionsNavPane.ShowOverflowButton = false;
            this.navBarControl1.OptionsNavPane.ShowOverflowPanel = false;
            this.navBarControl1.Size = new System.Drawing.Size(293, 507);
            this.navBarControl1.TabIndex = 0;
            this.navBarControl1.Text = "navBarControl1";
            this.navBarControl1.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("DevExpress Style");
            this.navBarControl1.ActiveGroupChanged += new DevExpress.XtraNavBar.NavBarGroupEventHandler(this.navBarControl1_ActiveGroupChanged);
            // 
            // navBarGroupTSZF
            // 
            this.navBarGroupTSZF.Caption = "特殊字符(鼠标拖拽)";
            this.navBarGroupTSZF.ControlContainer = this.navBarGroupControlContainer1;
            this.navBarGroupTSZF.Expanded = true;
            this.navBarGroupTSZF.GroupClientHeight = 80;
            this.navBarGroupTSZF.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroupTSZF.Name = "navBarGroupTSZF";
            // 
            // navBarGroupControlContainer1
            // 
            this.navBarGroupControlContainer1.Controls.Add(this.imageListBoxControlSymbol);
            this.navBarGroupControlContainer1.Name = "navBarGroupControlContainer1";
            this.navBarGroupControlContainer1.Size = new System.Drawing.Size(293, 409);
            this.navBarGroupControlContainer1.TabIndex = 0;
            // 
            // imageListBoxControlSymbol
            // 
            this.imageListBoxControlSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListBoxControlSymbol.GlyphAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.imageListBoxControlSymbol.Location = new System.Drawing.Point(0, 0);
            this.imageListBoxControlSymbol.Name = "imageListBoxControlSymbol";
            this.imageListBoxControlSymbol.Size = new System.Drawing.Size(293, 409);
            this.imageListBoxControlSymbol.TabIndex = 0;
            this.imageListBoxControlSymbol.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeListPersonTemplate_MouseDown);
            this.imageListBoxControlSymbol.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageListBoxControlSymbol_MouseMove);
            this.imageListBoxControlSymbol.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imageListBoxControlSymbol_MouseUp);
            // 
            // navBarGroupControlContainer2
            // 
            this.navBarGroupControlContainer2.Controls.Add(this.treeListPersonTemplate);
            this.navBarGroupControlContainer2.Controls.Add(this.panelControl1);
            this.navBarGroupControlContainer2.Name = "navBarGroupControlContainer2";
            this.navBarGroupControlContainer2.Size = new System.Drawing.Size(293, 409);
            this.navBarGroupControlContainer2.TabIndex = 1;
            // 
            // treeListPersonTemplate
            // 
            this.treeListPersonTemplate.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colCode,
            this.treeListColumn2,
            this.colType});
            this.treeListPersonTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListPersonTemplate.Location = new System.Drawing.Point(0, 42);
            this.treeListPersonTemplate.Name = "treeListPersonTemplate";
            this.treeListPersonTemplate.OptionsMenu.EnableColumnMenu = false;
            this.treeListPersonTemplate.OptionsMenu.EnableFooterMenu = false;
            this.treeListPersonTemplate.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeListPersonTemplate.OptionsView.ShowColumns = false;
            this.treeListPersonTemplate.OptionsView.ShowIndicator = false;
            this.treeListPersonTemplate.Size = new System.Drawing.Size(293, 367);
            this.treeListPersonTemplate.StateImageList = this.imageCollection_Tree;
            this.treeListPersonTemplate.TabIndex = 1;
            this.treeListPersonTemplate.ToolTipController = this.toolTipControllerTreeList;
            this.treeListPersonTemplate.GetStateImage += new DevExpress.XtraTreeList.GetStateImageEventHandler(this.treeListPersonTemplate_GetStateImage);
            this.treeListPersonTemplate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeListPersonTemplate_MouseDown);
            this.treeListPersonTemplate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeListPersonTemplate_MouseMove);
            this.treeListPersonTemplate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeListPersonTemplate_MouseUp);
            // 
            // colCode
            // 
            this.colCode.Caption = "treeListColumn1";
            this.colCode.FieldName = "COLCODEID";
            this.colCode.Name = "colCode";
            this.colCode.OptionsColumn.AllowEdit = false;
            this.colCode.OptionsColumn.AllowMove = false;
            this.colCode.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.colCode.OptionsColumn.ShowInCustomizationForm = false;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "treeListColumn2";
            this.treeListColumn2.FieldName = "ITEMNAME";
            this.treeListColumn2.MinWidth = 33;
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.OptionsColumn.AllowEdit = false;
            this.treeListColumn2.OptionsColumn.AllowMove = false;
            this.treeListColumn2.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.treeListColumn2.OptionsColumn.ShowInCustomizationForm = false;
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            // 
            // colType
            // 
            this.colType.Caption = "treeListColumn3";
            this.colType.FieldName = "NODETYPE";
            this.colType.Name = "colType";
            this.colType.OptionsColumn.AllowEdit = false;
            this.colType.OptionsColumn.AllowMove = false;
            this.colType.OptionsColumn.ShowInCustomizationForm = false;
            // 
            // imageCollection_Tree
            // 
            this.imageCollection_Tree.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection_Tree.ImageStream")));
            this.imageCollection_Tree.Images.SetKeyName(0, "NewDocument.png");
            this.imageCollection_Tree.Images.SetKeyName(1, "DocumentLock.png");
            this.imageCollection_Tree.Images.SetKeyName(2, "Folder_Closed.png");
            this.imageCollection_Tree.Images.SetKeyName(3, "Folder_Open.png");
            this.imageCollection_Tree.Images.SetKeyName(4, "Bold_16.png");
            this.imageCollection_Tree.Images.SetKeyName(5, "FontItalic.png");
            this.imageCollection_Tree.Images.SetKeyName(6, "Redo_16.png");
            this.imageCollection_Tree.Images.SetKeyName(7, "Subscript_16.png");
            this.imageCollection_Tree.Images.SetKeyName(8, "Superscript_16.png");
            this.imageCollection_Tree.Images.SetKeyName(9, "Text_Snderline.png");
            this.imageCollection_Tree.Images.SetKeyName(10, "Undo_16.png");
            this.imageCollection_Tree.Images.SetKeyName(11, "lockFolder.jpg");
            this.imageCollection_Tree.Images.SetKeyName(12, "lockfile.gif");
            this.imageCollection_Tree.Images.SetKeyName(13, "Checked.png");
            this.imageCollection_Tree.Images.SetKeyName(14, "FirstCheck.png");
            this.imageCollection_Tree.Images.SetKeyName(15, "NotArchived_16.png");
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_SearchTemplete);
            this.panelControl1.Controls.Add(this.txt_SearchTemplete);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(293, 42);
            this.panelControl1.TabIndex = 0;
            // 
            // btn_SearchTemplete
            // 
            this.btn_SearchTemplete.Image = ((System.Drawing.Image)(resources.GetObject("btn_SearchTemplete.Image")));
            this.btn_SearchTemplete.Location = new System.Drawing.Point(179, 9);
            this.btn_SearchTemplete.Name = "btn_SearchTemplete";
            this.btn_SearchTemplete.Size = new System.Drawing.Size(80, 23);
            this.btn_SearchTemplete.TabIndex = 4;
            this.btn_SearchTemplete.Text = "搜索 (&Q)";
            this.btn_SearchTemplete.Click += new System.EventHandler(this.btn_SearchTemplete_Click);
            // 
            // txt_SearchTemplete
            // 
            this.txt_SearchTemplete.Location = new System.Drawing.Point(9, 10);
            this.txt_SearchTemplete.Name = "txt_SearchTemplete";
            this.txt_SearchTemplete.Size = new System.Drawing.Size(161, 20);
            this.txt_SearchTemplete.TabIndex = 0;
            // 
            // navBarGroupKSMB
            // 
            this.navBarGroupKSMB.Caption = "小模板(鼠标拖拽)";
            this.navBarGroupKSMB.ControlContainer = this.navBarGroupControlContainer2;
            this.navBarGroupKSMB.GroupClientHeight = 80;
            this.navBarGroupKSMB.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroupKSMB.Name = "navBarGroupKSMB";
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnRight_DeptTemplete});
            this.barManager1.MaxItemId = 1;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(293, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 507);
            this.barDockControlBottom.Size = new System.Drawing.Size(293, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 507);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(293, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 507);
            // 
            // btnRight_DeptTemplete
            // 
            this.btnRight_DeptTemplete.Caption = "科室小模板";
            this.btnRight_DeptTemplete.Id = 0;
            this.btnRight_DeptTemplete.Name = "btnRight_DeptTemplete";
            this.btnRight_DeptTemplete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRight_DeptTemplete_ItemClick);
            // 
            // popupMenu_DeptTemplate
            // 
            this.popupMenu_DeptTemplate.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRight_DeptTemplete)});
            this.popupMenu_DeptTemplate.Manager = this.barManager1;
            this.popupMenu_DeptTemplate.Name = "popupMenu_DeptTemplate";
            // 
            // ToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 507);
            this.Controls.Add(this.navBarControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ToolForm";
            this.Text = "护理工具箱";
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            this.navBarControl1.ResumeLayout(false);
            this.navBarGroupControlContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageListBoxControlSymbol)).EndInit();
            this.navBarGroupControlContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListPersonTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection_Tree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_SearchTemplete.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu_DeptTemplate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroupTSZF;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroupKSMB;
        private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer1;
        private DevExpress.XtraEditors.ImageListBoxControl imageListBoxControlSymbol;
        private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraTreeList.TreeList treeListPersonTemplate;
        private DevExpress.XtraEditors.TextEdit txt_SearchTemplete;
        private DevExpress.XtraEditors.SimpleButton btn_SearchTemplete;
        private DevExpress.Utils.ToolTipController toolTipControllerTreeList;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.PopupMenu popupMenu_DeptTemplate;
        private DevExpress.XtraBars.BarButtonItem btnRight_DeptTemplete;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colCode;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colType;
        private DevExpress.Utils.ImageCollection imageCollection_Tree;
    }
}