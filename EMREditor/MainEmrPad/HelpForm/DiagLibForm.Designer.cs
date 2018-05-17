namespace DrectSoft.Core.MainEmrPad.HelpForm
{
    partial class DiagLibForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagLibForm));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            this.treeListDiagRep = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn4 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.imageCollection_Tree = new DevExpress.Utils.ImageCollection(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.btnRight_AddCategory = new DevExpress.XtraBars.BarButtonItem();
            this.btnRight_AddDiag = new DevExpress.XtraBars.BarButtonItem();
            this.btnRight_ModifyDiag = new DevExpress.XtraBars.BarButtonItem();
            this.btnRight_DeleteDiag = new DevExpress.XtraBars.BarButtonItem();
            this.btnRight_ModifyCategory = new DevExpress.XtraBars.BarButtonItem();
            this.btnRight_DeleteCategory = new DevExpress.XtraBars.BarButtonItem();
            this.memoEditContent = new DevExpress.XtraEditors.MemoEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.simpleButtonInsert1 = new DevExpress.XtraEditors.SimpleButton();
            this.lookUpEditType = new DevExpress.XtraEditors.LookUpEdit();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.btnQurey = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey(this.components);
            this.txtQuery = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.btnClear = new DrectSoft.Common.Ctrs.OTHER.DevButtonClear(this.components);
            this.DevButtonReset1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset(this.components);
            this.simpleButtonExit = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.treeListDiagRep)).BeginInit();
            this.treeListDiagRep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection_Tree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditContent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuery.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // treeListDiagRep
            // 
            this.treeListDiagRep.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.colType,
            this.treeListColumn4});
            this.treeListDiagRep.Controls.Add(this.barDockControlLeft);
            this.treeListDiagRep.Controls.Add(this.barDockControlRight);
            this.treeListDiagRep.Controls.Add(this.barDockControlBottom);
            this.treeListDiagRep.Controls.Add(this.barDockControlTop);
            this.treeListDiagRep.KeyFieldName = "NODE";
            this.treeListDiagRep.Location = new System.Drawing.Point(9, 50);
            this.treeListDiagRep.Name = "treeListDiagRep";
            this.treeListDiagRep.OptionsBehavior.Editable = false;
            this.treeListDiagRep.OptionsMenu.EnableColumnMenu = false;
            this.treeListDiagRep.OptionsMenu.EnableFooterMenu = false;
            this.treeListDiagRep.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeListDiagRep.OptionsView.ShowIndicator = false;
            this.treeListDiagRep.ParentFieldName = "PARENT_NODE";
            this.treeListDiagRep.Size = new System.Drawing.Size(219, 391);
            this.treeListDiagRep.StateImageList = this.imageCollection_Tree;
            this.treeListDiagRep.TabIndex = 5;
            this.treeListDiagRep.TabStop = false;
            this.treeListDiagRep.GetStateImage += new DevExpress.XtraTreeList.GetStateImageEventHandler(this.treeListDiagRep_GetStateImage);
            this.treeListDiagRep.AfterExpand += new DevExpress.XtraTreeList.NodeEventHandler(this.treeListDiagRep_AfterExpand);
            this.treeListDiagRep.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListDiagRep_FocusedNodeChanged);
            this.treeListDiagRep.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeListDiagRep_MouseUp);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "名称";
            this.treeListColumn1.FieldName = "TITLE";
            this.treeListColumn1.MinWidth = 33;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.OptionsColumn.AllowEdit = false;
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // colType
            // 
            this.colType.Caption = "类别";
            this.colType.FieldName = "NODETYPE";
            this.colType.Name = "colType";
            // 
            // treeListColumn4
            // 
            this.treeListColumn4.Caption = "CONTENT";
            this.treeListColumn4.FieldName = "CONTENT";
            this.treeListColumn4.Name = "treeListColumn4";
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 391);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(219, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 391);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 391);
            this.barDockControlBottom.Size = new System.Drawing.Size(219, 0);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(219, 0);
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
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnRight_AddCategory,
            this.btnRight_AddDiag,
            this.btnRight_ModifyDiag,
            this.btnRight_DeleteDiag,
            this.btnRight_ModifyCategory,
            this.btnRight_DeleteCategory});
            this.barManager1.MaxItemId = 12;
            this.barManager1.QueryShowPopupMenu += new DevExpress.XtraBars.QueryShowPopupMenuEventHandler(this.barManager1_QueryShowPopupMenu);
            // 
            // btnRight_AddCategory
            // 
            this.btnRight_AddCategory.Caption = "新增分类";
            this.btnRight_AddCategory.Id = 9;
            this.btnRight_AddCategory.Name = "btnRight_AddCategory";
            this.btnRight_AddCategory.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRight_AddCategory_ItemClick);
            // 
            // btnRight_AddDiag
            // 
            this.btnRight_AddDiag.Caption = "新增诊断";
            this.btnRight_AddDiag.Id = 3;
            this.btnRight_AddDiag.Name = "btnRight_AddDiag";
            this.btnRight_AddDiag.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRight_AddDiag_ItemClick);
            // 
            // btnRight_ModifyDiag
            // 
            this.btnRight_ModifyDiag.Caption = "修改诊断";
            this.btnRight_ModifyDiag.Id = 5;
            this.btnRight_ModifyDiag.Name = "btnRight_ModifyDiag";
            toolTipTitleItem1.Text = "修改诊断";
            superToolTip1.Items.Add(toolTipTitleItem1);
            this.btnRight_ModifyDiag.SuperTip = superToolTip1;
            this.btnRight_ModifyDiag.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRight_ModifyDiag_ItemClick);
            // 
            // btnRight_DeleteDiag
            // 
            this.btnRight_DeleteDiag.Caption = "删除诊断";
            this.btnRight_DeleteDiag.Id = 4;
            this.btnRight_DeleteDiag.Name = "btnRight_DeleteDiag";
            toolTipTitleItem2.Text = "删除诊断";
            superToolTip2.Items.Add(toolTipTitleItem2);
            this.btnRight_DeleteDiag.SuperTip = superToolTip2;
            this.btnRight_DeleteDiag.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRight_DeleteDiag_ItemClick);
            // 
            // btnRight_ModifyCategory
            // 
            this.btnRight_ModifyCategory.Caption = "修改分类";
            this.btnRight_ModifyCategory.Id = 10;
            this.btnRight_ModifyCategory.Name = "btnRight_ModifyCategory";
            this.btnRight_ModifyCategory.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRight_ModifyCategory_ItemClick);
            // 
            // btnRight_DeleteCategory
            // 
            this.btnRight_DeleteCategory.Caption = "删除分类";
            this.btnRight_DeleteCategory.Id = 11;
            this.btnRight_DeleteCategory.Name = "btnRight_DeleteCategory";
            this.btnRight_DeleteCategory.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRight_DeleteCategory_ItemClick);
            // 
            // memoEditContent
            // 
            this.memoEditContent.Location = new System.Drawing.Point(234, 50);
            this.memoEditContent.Name = "memoEditContent";
            this.memoEditContent.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.memoEditContent.Size = new System.Drawing.Size(481, 391);
            this.memoEditContent.TabIndex = 6;
            this.memoEditContent.TabStop = false;
            this.memoEditContent.UseOptimizedRendering = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // simpleButtonInsert1
            // 
            this.simpleButtonInsert1.Image = global::Drectsoft.Core.MainEmrPad.Properties.Resources.新增;
            this.simpleButtonInsert1.Location = new System.Drawing.Point(479, 447);
            this.simpleButtonInsert1.Name = "simpleButtonInsert1";
            this.simpleButtonInsert1.Size = new System.Drawing.Size(150, 27);
            this.simpleButtonInsert1.TabIndex = 7;
            this.simpleButtonInsert1.Text = "添加内容紧跟其后 (&A)";
            this.simpleButtonInsert1.Click += new System.EventHandler(this.simpleButtonInsert1_Click);
            // 
            // lookUpEditType
            // 
            this.lookUpEditType.EditValue = "2";
            this.lookUpEditType.EnterMoveNextControl = true;
            this.lookUpEditType.Location = new System.Drawing.Point(9, 14);
            this.lookUpEditType.Name = "lookUpEditType";
            this.lookUpEditType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "类别名称"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.lookUpEditType.Size = new System.Drawing.Size(219, 20);
            this.lookUpEditType.TabIndex = 0;
            this.lookUpEditType.EditValueChanged += new System.EventHandler(this.lookUpEditType_EditValueChanged);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRight_AddCategory),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRight_ModifyCategory),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRight_DeleteCategory),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRight_AddDiag),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRight_ModifyDiag),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRight_DeleteDiag)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // btnQurey
            // 
            this.btnQurey.Image = ((System.Drawing.Image)(resources.GetObject("btnQurey.Image")));
            this.btnQurey.Location = new System.Drawing.Point(456, 11);
            this.btnQurey.Name = "btnQurey";
            this.btnQurey.Size = new System.Drawing.Size(80, 27);
            this.btnQurey.TabIndex = 2;
            this.btnQurey.Text = "查询(&Q)";
            this.btnQurey.Click += new System.EventHandler(this.BtnQurey_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.EnterMoveNextControl = true;
            this.txtQuery.IsEnterChangeBgColor = false;
            this.txtQuery.IsEnterKeyToNextControl = false;
            this.txtQuery.IsNumber = false;
            this.txtQuery.Location = new System.Drawing.Point(234, 14);
            this.txtQuery.MenuManager = this.barManager1;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(216, 20);
            this.txtQuery.TabIndex = 1;
            // 
            // btnClear
            // 
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.Location = new System.Drawing.Point(542, 11);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 27);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "清空(&L)";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // DevButtonReset1
            // 
            this.DevButtonReset1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonReset1.Image")));
            this.DevButtonReset1.Location = new System.Drawing.Point(629, 11);
            this.DevButtonReset1.Name = "DevButtonReset1";
            this.DevButtonReset1.Size = new System.Drawing.Size(80, 27);
            this.DevButtonReset1.TabIndex = 4;
            this.DevButtonReset1.Text = "重置(&B)";
            this.DevButtonReset1.Click += new System.EventHandler(this.DevButtonReset1_Click);
            // 
            // simpleButtonExit
            // 
            this.simpleButtonExit.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonExit.Image")));
            this.simpleButtonExit.Location = new System.Drawing.Point(635, 447);
            this.simpleButtonExit.Name = "simpleButtonExit";
            this.simpleButtonExit.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonExit.TabIndex = 8;
            this.simpleButtonExit.Text = "关闭(&T)";
            this.simpleButtonExit.Click += new System.EventHandler(this.simpleButtonExit_Click);
            // 
            // DiagLibForm
            // 
            this.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Appearance.Options.UseFont = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 479);
            this.Controls.Add(this.simpleButtonExit);
            this.Controls.Add(this.DevButtonReset1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.btnQurey);
            this.Controls.Add(this.lookUpEditType);
            this.Controls.Add(this.simpleButtonInsert1);
            this.Controls.Add(this.memoEditContent);
            this.Controls.Add(this.treeListDiagRep);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DiagLibForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "诊断管理";
            this.Load += new System.EventHandler(this.DiagLibForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.treeListDiagRep)).EndInit();
            this.treeListDiagRep.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection_Tree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditContent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuery.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList treeListDiagRep;
        private DevExpress.XtraEditors.MemoEdit memoEditContent;
        private DevExpress.XtraEditors.SimpleButton simpleButtonInsert1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn4;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditType;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnRight_AddDiag;
        private DevExpress.XtraBars.BarButtonItem btnRight_DeleteDiag;
        private DevExpress.XtraBars.BarButtonItem btnRight_ModifyDiag;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem btnRight_AddCategory;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colType;//王冀 2012-1-23
        private DevExpress.Utils.ImageCollection imageCollection_Tree;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtQuery;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey btnQurey;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClear btnClear;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset DevButtonReset1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClose simpleButtonExit;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraBars.BarButtonItem btnRight_ModifyCategory;
        private DevExpress.XtraBars.BarButtonItem btnRight_DeleteCategory;
    }
}