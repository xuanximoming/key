namespace DrectSoft.Core.TemplatePerson
{
    partial class TemplatePersonForm
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplatePersonForm));
            this.gridControlTemplatePerson = new DevExpress.XtraGrid.GridControl();
            this.gridViewTemplatePerson = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTemplateID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnMemo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnUsed = new DevExpress.XtraGrid.Columns.GridColumn();
            this.defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            this.simpleButtonConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonRecover = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonExit = new DevExpress.XtraEditors.SimpleButton();
            this.labelSearch = new DevExpress.XtraEditors.LabelControl();
            this.imageCollectionTreeList = new DevExpress.Utils.ImageCollection();
            this.barManagerModel = new DevExpress.XtraBars.BarManager();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItemExpendNode = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemUnExpendNode = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemAddFolder = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemRename = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDelete = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenuTreeList = new DevExpress.XtraBars.PopupMenu();
            this.treeListTemplatePerson = new DevExpress.XtraTreeList.TreeList();
            this.ModelName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.TemplateID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.TemplatePersonID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelControlSearch = new DevExpress.XtraEditors.PanelControl();
            this.lookUpEditorTemplateName = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowTemplateName = new DrectSoft.Common.Library.LookUpWindow();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTemplatePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTemplatePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionTreeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuTreeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListTemplatePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlSearch)).BeginInit();
            this.panelControlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorTemplateName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowTemplateName)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlTemplatePerson
            // 
            this.gridControlTemplatePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlTemplatePerson.Cursor = System.Windows.Forms.Cursors.Default;
            gridLevelNode1.RelationName = "Level1";
            this.gridControlTemplatePerson.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControlTemplatePerson.Location = new System.Drawing.Point(265, 40);
            this.gridControlTemplatePerson.MainView = this.gridViewTemplatePerson;
            this.gridControlTemplatePerson.Name = "gridControlTemplatePerson";
            this.gridControlTemplatePerson.Size = new System.Drawing.Size(527, 487);
            this.gridControlTemplatePerson.TabIndex = 1;
            this.gridControlTemplatePerson.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTemplatePerson});
            this.gridControlTemplatePerson.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridControlTemplatePerson_MouseDown);
            this.gridControlTemplatePerson.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridControlTemplatePerson_MouseMove);
            // 
            // gridViewTemplatePerson
            // 
            this.gridViewTemplatePerson.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnID,
            this.gridColumnName,
            this.gridColumnTemplateID,
            this.gridColumnMemo,
            this.gridColumnUsed});
            this.gridViewTemplatePerson.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewTemplatePerson.GridControl = this.gridControlTemplatePerson;
            this.gridViewTemplatePerson.Name = "gridViewTemplatePerson";
            this.gridViewTemplatePerson.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.gridViewTemplatePerson.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewTemplatePerson.OptionsView.ShowGroupPanel = false;
            this.gridViewTemplatePerson.OptionsView.ShowIndicator = false;
            // 
            // gridColumnID
            // 
            this.gridColumnID.Caption = "ID";
            this.gridColumnID.FieldName = "ID";
            this.gridColumnID.Name = "gridColumnID";
            this.gridColumnID.Width = 40;
            // 
            // gridColumnName
            // 
            this.gridColumnName.Caption = "名称";
            this.gridColumnName.FieldName = "Name";
            this.gridColumnName.Name = "gridColumnName";
            this.gridColumnName.Visible = true;
            this.gridColumnName.VisibleIndex = 0;
            this.gridColumnName.Width = 155;
            // 
            // gridColumnTemplateID
            // 
            this.gridColumnTemplateID.Caption = "TemplateID";
            this.gridColumnTemplateID.FieldName = "TemplateID";
            this.gridColumnTemplateID.Name = "gridColumnTemplateID";
            // 
            // gridColumnMemo
            // 
            this.gridColumnMemo.Caption = "备注";
            this.gridColumnMemo.FieldName = "Memo";
            this.gridColumnMemo.Name = "gridColumnMemo";
            this.gridColumnMemo.Visible = true;
            this.gridColumnMemo.VisibleIndex = 1;
            this.gridColumnMemo.Width = 279;
            // 
            // gridColumnUsed
            // 
            this.gridColumnUsed.Caption = "使用";
            this.gridColumnUsed.FieldName = "Used";
            this.gridColumnUsed.Name = "gridColumnUsed";
            this.gridColumnUsed.OptionsColumn.AllowEdit = false;
            this.gridColumnUsed.Visible = true;
            this.gridColumnUsed.VisibleIndex = 2;
            this.gridColumnUsed.Width = 39;
            // 
            // defaultLookAndFeel
            // 
            this.defaultLookAndFeel.LookAndFeel.SkinName = "Blue";
            // 
            // simpleButtonConfirm
            // 
            this.simpleButtonConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonConfirm.Location = new System.Drawing.Point(521, 538);
            this.simpleButtonConfirm.Name = "simpleButtonConfirm";
            this.simpleButtonConfirm.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonConfirm.TabIndex = 2;
            this.simpleButtonConfirm.Text = "确定";
            this.simpleButtonConfirm.Click += new System.EventHandler(this.simpleButtonConfirm_Click);
            // 
            // simpleButtonRecover
            // 
            this.simpleButtonRecover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonRecover.Location = new System.Drawing.Point(614, 538);
            this.simpleButtonRecover.Name = "simpleButtonRecover";
            this.simpleButtonRecover.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonRecover.TabIndex = 3;
            this.simpleButtonRecover.Text = "还原";
            this.simpleButtonRecover.Click += new System.EventHandler(this.simpleButtonRecover_Click);
            // 
            // simpleButtonExit
            // 
            this.simpleButtonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonExit.Location = new System.Drawing.Point(705, 538);
            this.simpleButtonExit.Name = "simpleButtonExit";
            this.simpleButtonExit.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonExit.TabIndex = 4;
            this.simpleButtonExit.Text = "关闭";
            this.simpleButtonExit.Click += new System.EventHandler(this.simpleButtonExit_Click);
            // 
            // labelSearch
            // 
            this.labelSearch.Location = new System.Drawing.Point(26, 11);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(72, 14);
            this.labelSearch.TabIndex = 11;
            this.labelSearch.Text = "个人模板检索";
            // 
            // imageCollectionTreeList
            // 
            this.imageCollectionTreeList.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollectionTreeList.ImageStream")));
            // 
            // barManagerModel
            // 
            this.barManagerModel.DockControls.Add(this.barDockControlTop);
            this.barManagerModel.DockControls.Add(this.barDockControlBottom);
            this.barManagerModel.DockControls.Add(this.barDockControlLeft);
            this.barManagerModel.DockControls.Add(this.barDockControlRight);
            this.barManagerModel.Form = this;
            this.barManagerModel.Images = this.imageCollectionTreeList;
            this.barManagerModel.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemExpendNode,
            this.barButtonItemUnExpendNode,
            this.barButtonItemAddFolder,
            this.barButtonItemRename,
            this.barButtonItemDelete});
            this.barManagerModel.MaxItemId = 5;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(792, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 571);
            this.barDockControlBottom.Size = new System.Drawing.Size(792, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 571);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(792, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 571);
            // 
            // barButtonItemExpendNode
            // 
            this.barButtonItemExpendNode.Caption = "展开节点";
            this.barButtonItemExpendNode.Id = 0;
            this.barButtonItemExpendNode.ImageIndex = 6;
            this.barButtonItemExpendNode.Name = "barButtonItemExpendNode";
            this.barButtonItemExpendNode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemExpendNode_ItemClick);
            // 
            // barButtonItemUnExpendNode
            // 
            this.barButtonItemUnExpendNode.Caption = "收缩节点";
            this.barButtonItemUnExpendNode.Id = 1;
            this.barButtonItemUnExpendNode.ImageIndex = 5;
            this.barButtonItemUnExpendNode.Name = "barButtonItemUnExpendNode";
            this.barButtonItemUnExpendNode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemUnExpendNode_ItemClick);
            // 
            // barButtonItemAddFolder
            // 
            this.barButtonItemAddFolder.Caption = "添加文件夹";
            this.barButtonItemAddFolder.Id = 2;
            this.barButtonItemAddFolder.ImageIndex = 3;
            this.barButtonItemAddFolder.Name = "barButtonItemAddFolder";
            this.barButtonItemAddFolder.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAddFolder_ItemClick);
            // 
            // barButtonItemRename
            // 
            this.barButtonItemRename.Caption = "重命名";
            this.barButtonItemRename.Id = 3;
            this.barButtonItemRename.ImageIndex = 4;
            this.barButtonItemRename.Name = "barButtonItemRename";
            this.barButtonItemRename.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemRename_ItemClick);
            // 
            // barButtonItemDelete
            // 
            this.barButtonItemDelete.Caption = "删除";
            this.barButtonItemDelete.Id = 4;
            this.barButtonItemDelete.ImageIndex = 0;
            this.barButtonItemDelete.Name = "barButtonItemDelete";
            this.barButtonItemDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDelete_ItemClick);
            // 
            // popupMenuTreeList
            // 
            this.popupMenuTreeList.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemExpendNode),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemUnExpendNode),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemAddFolder),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemRename),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemDelete)});
            this.popupMenuTreeList.Manager = this.barManagerModel;
            this.popupMenuTreeList.Name = "popupMenuTreeList";
            // 
            // treeListTemplatePerson
            // 
            this.treeListTemplatePerson.AllowDrop = true;
            this.treeListTemplatePerson.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeListTemplatePerson.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.ModelName,
            this.TemplateID,
            this.TemplatePersonID});
            this.treeListTemplatePerson.Location = new System.Drawing.Point(0, 40);
            this.treeListTemplatePerson.Name = "treeListTemplatePerson";
            this.treeListTemplatePerson.OptionsBehavior.DragNodes = true;
            this.treeListTemplatePerson.OptionsBehavior.ImmediateEditor = false;
            this.treeListTemplatePerson.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeListTemplatePerson.OptionsView.ShowHorzLines = false;
            this.treeListTemplatePerson.OptionsView.ShowIndicator = false;
            this.treeListTemplatePerson.OptionsView.ShowVertLines = false;
            this.treeListTemplatePerson.SelectImageList = this.imageCollectionTreeList;
            this.treeListTemplatePerson.Size = new System.Drawing.Size(263, 487);
            this.treeListTemplatePerson.TabIndex = 13;
            this.treeListTemplatePerson.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListPersonModel_FocusedNodeChanged);
            this.treeListTemplatePerson.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeListTemplatePerson_DragDrop);
            this.treeListTemplatePerson.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeListTemplatePerson_DragEnter);
            this.treeListTemplatePerson.DoubleClick += new System.EventHandler(this.treeListTemplatePerson_DoubleClick);
            this.treeListTemplatePerson.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeListTemplatePerson_MouseUp);
            // 
            // ModelName
            // 
            this.ModelName.Caption = "名称";
            this.ModelName.FieldName = "ModelName";
            this.ModelName.MinWidth = 33;
            this.ModelName.Name = "ModelName";
            this.ModelName.OptionsColumn.AllowEdit = false;
            this.ModelName.Visible = true;
            this.ModelName.VisibleIndex = 0;
            // 
            // TemplateID
            // 
            this.TemplateID.Caption = "TemplateID";
            this.TemplateID.FieldName = "TemplateID";
            this.TemplateID.Name = "TemplateID";
            this.TemplateID.OptionsColumn.AllowEdit = false;
            // 
            // TemplatePersonID
            // 
            this.TemplatePersonID.Caption = "TemplatePersonID";
            this.TemplatePersonID.FieldName = "TemplatePersonID";
            this.TemplatePersonID.Name = "TemplatePersonID";
            this.TemplatePersonID.OptionsColumn.AllowEdit = false;
            // 
            // panelControlSearch
            // 
            this.panelControlSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControlSearch.Controls.Add(this.lookUpEditorTemplateName);
            this.panelControlSearch.Controls.Add(this.labelSearch);
            this.panelControlSearch.Location = new System.Drawing.Point(0, 0);
            this.panelControlSearch.Name = "panelControlSearch";
            this.panelControlSearch.Size = new System.Drawing.Size(792, 36);
            this.panelControlSearch.TabIndex = 14;
            // 
            // lookUpEditorTemplateName
            // 
            this.lookUpEditorTemplateName.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorTemplateName.ListWindow = this.lookUpWindowTemplateName;
            this.lookUpEditorTemplateName.Location = new System.Drawing.Point(104, 8);
            this.lookUpEditorTemplateName.Name = "lookUpEditorTemplateName";
            this.lookUpEditorTemplateName.ShowSButton = true;
            this.lookUpEditorTemplateName.Size = new System.Drawing.Size(585, 18);
            this.lookUpEditorTemplateName.TabIndex = 12;
            this.lookUpEditorTemplateName.CodeValueChanged += new System.EventHandler(this.lookUpEditorTemplateName_CodeValueChanged);
            // 
            // lookUpWindowTemplateName
            // 
            this.lookUpWindowTemplateName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowTemplateName.GenShortCode = null;
            this.lookUpWindowTemplateName.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowTemplateName.Owner = null;
            this.lookUpWindowTemplateName.SqlHelper = null;
            // 
            // TemplatePersonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 571);
            this.Controls.Add(this.panelControlSearch);
            this.Controls.Add(this.treeListTemplatePerson);
            this.Controls.Add(this.simpleButtonExit);
            this.Controls.Add(this.simpleButtonRecover);
            this.Controls.Add(this.simpleButtonConfirm);
            this.Controls.Add(this.gridControlTemplatePerson);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TemplatePersonForm";
            this.Text = "个人模板维护";
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTemplatePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTemplatePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionTreeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuTreeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListTemplatePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlSearch)).EndInit();
            this.panelControlSearch.ResumeLayout(false);
            this.panelControlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorTemplateName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowTemplateName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Data.DataTable m_DataTableTemplatePerson;
        private DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hitInfo = null;
        private DevExpress.XtraTreeList.Nodes.TreeListNode m_FocusNode;
        private DevExpress.XtraGrid.GridControl gridControlTemplatePerson;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTemplatePerson;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTemplateID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnMemo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnUsed;
        private DevExpress.XtraEditors.SimpleButton simpleButtonConfirm;
        private DevExpress.XtraEditors.SimpleButton simpleButtonRecover;
        private DevExpress.XtraEditors.SimpleButton simpleButtonExit;
        private DevExpress.XtraEditors.LabelControl labelSearch;
        private DevExpress.Utils.ImageCollection imageCollectionTreeList;
        private DevExpress.XtraBars.BarManager barManagerModel;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.PopupMenu popupMenuTreeList;
        private DevExpress.XtraBars.BarButtonItem barButtonItemExpendNode;
        private DevExpress.XtraBars.BarButtonItem barButtonItemUnExpendNode;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAddFolder;
        private DevExpress.XtraBars.BarButtonItem barButtonItemRename;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDelete;
        private DevExpress.XtraEditors.PanelControl panelControlSearch;
        private DevExpress.XtraTreeList.TreeList treeListTemplatePerson;
        private DevExpress.XtraTreeList.Columns.TreeListColumn ModelName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn TemplateID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn TemplatePersonID;
        private DrectSoft.Common.Library.LookUpEditor lookUpEditorTemplateName;
        private DrectSoft.Common.Library.LookUpWindow lookUpWindowTemplateName;
    }
}