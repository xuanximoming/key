namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.HelpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagLibForm));
            this.treeListDiagRep = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn4 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.btnAddParent = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemNew2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemModified2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDelet2 = new DevExpress.XtraBars.BarButtonItem();
            this.memoEditContent = new DevExpress.XtraEditors.MemoEdit();
            this.simpleButtonInsert1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonExit = new DevExpress.XtraEditors.SimpleButton();
            this.lookUpEditType = new DevExpress.XtraEditors.LookUpEdit();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu();
            ((System.ComponentModel.ISupportInitialize)(this.treeListDiagRep)).BeginInit();
            this.treeListDiagRep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditContent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeListDiagRep
            // 
            this.treeListDiagRep.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn4});
            this.treeListDiagRep.Controls.Add(this.barDockControlLeft);
            this.treeListDiagRep.Controls.Add(this.barDockControlRight);
            this.treeListDiagRep.Controls.Add(this.barDockControlBottom);
            this.treeListDiagRep.Controls.Add(this.barDockControlTop);
            this.treeListDiagRep.KeyFieldName = "NODE";
            this.treeListDiagRep.Location = new System.Drawing.Point(9, 28);
            this.treeListDiagRep.Name = "treeListDiagRep";
            this.treeListDiagRep.OptionsBehavior.Editable = false;
            this.treeListDiagRep.OptionsMenu.EnableColumnMenu = false;
            this.treeListDiagRep.OptionsMenu.EnableFooterMenu = false;
            this.treeListDiagRep.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeListDiagRep.OptionsView.ShowIndicator = false;
            this.treeListDiagRep.ParentFieldName = "PARENT_NODE";
            this.treeListDiagRep.Size = new System.Drawing.Size(164, 304);
            this.treeListDiagRep.TabIndex = 0;
            this.treeListDiagRep.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListDiagRep_FocusedNodeChanged);
            this.treeListDiagRep.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeListDiagRep_MouseUp);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "名称";
            this.treeListColumn1.FieldName = "TITLE";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.OptionsColumn.AllowEdit = false;
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
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
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 304);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(164, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 304);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 304);
            this.barDockControlBottom.Size = new System.Drawing.Size(164, 0);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(164, 0);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnAddParent,
            this.barButtonItemNew2,
            this.barButtonItemModified2,
            this.barButtonItemDelet2});
            this.barManager1.MaxItemId = 10;
            this.barManager1.QueryShowPopupMenu += new DevExpress.XtraBars.QueryShowPopupMenuEventHandler(this.barManager1_QueryShowPopupMenu);
            // 
            // btnAddParent
            // 
            this.btnAddParent.Caption = "新增大分类";
            this.btnAddParent.Id = 9;
            this.btnAddParent.Name = "btnAddParent";
            this.btnAddParent.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddParent_ItemClick);
            // 
            // barButtonItemNew2
            // 
            this.barButtonItemNew2.Caption = "新增子节点";
            this.barButtonItemNew2.Id = 3;
            this.barButtonItemNew2.Name = "barButtonItemNew2";
            this.barButtonItemNew2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemNew2_ItemClick);
            // 
            // barButtonItemModified2
            // 
            this.barButtonItemModified2.Caption = "修改内容";
            this.barButtonItemModified2.Id = 5;
            this.barButtonItemModified2.Name = "barButtonItemModified2";
            this.barButtonItemModified2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemModified2_ItemClick);
            // 
            // barButtonItemDelet2
            // 
            this.barButtonItemDelet2.Caption = "删除";
            this.barButtonItemDelet2.Id = 4;
            this.barButtonItemDelet2.Name = "barButtonItemDelet2";
            this.barButtonItemDelet2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDelet2_ItemClick);
            // 
            // memoEditContent
            // 
            this.memoEditContent.Location = new System.Drawing.Point(172, 28);
            this.memoEditContent.Name = "memoEditContent";
            this.memoEditContent.Size = new System.Drawing.Size(289, 304);
            this.memoEditContent.TabIndex = 1;
            this.memoEditContent.UseOptimizedRendering = true;
            // 
            // simpleButtonInsert1
            // 
            this.simpleButtonInsert1.Location = new System.Drawing.Point(282, 338);
            this.simpleButtonInsert1.Name = "simpleButtonInsert1";
            this.simpleButtonInsert1.Size = new System.Drawing.Size(111, 20);
            this.simpleButtonInsert1.TabIndex = 2;
            this.simpleButtonInsert1.Text = "添加内容紧跟其后";
            this.simpleButtonInsert1.Click += new System.EventHandler(this.simpleButtonInsert1_Click);
            // 
            // simpleButtonExit
            // 
            this.simpleButtonExit.Location = new System.Drawing.Point(411, 338);
            this.simpleButtonExit.Name = "simpleButtonExit";
            this.simpleButtonExit.Size = new System.Drawing.Size(48, 20);
            this.simpleButtonExit.TabIndex = 4;
            this.simpleButtonExit.Text = "退出";
            this.simpleButtonExit.Click += new System.EventHandler(this.simpleButtonExit_Click);
            // 
            // lookUpEditType
            // 
            this.lookUpEditType.EditValue = "2";
            this.lookUpEditType.Location = new System.Drawing.Point(9, 5);
            this.lookUpEditType.Name = "lookUpEditType";
            this.lookUpEditType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "类别名称"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.lookUpEditType.Size = new System.Drawing.Size(164, 20);
            this.lookUpEditType.TabIndex = 5;
            this.lookUpEditType.EditValueChanged += new System.EventHandler(this.lookUpEditType_EditValueChanged);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAddParent),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemNew2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemDelet2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemModified2)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // DiagLibForm
            // 
            this.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Appearance.Options.UseFont = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 365);
            this.Controls.Add(this.lookUpEditType);
            this.Controls.Add(this.simpleButtonExit);
            this.Controls.Add(this.simpleButtonInsert1);
            this.Controls.Add(this.memoEditContent);
            this.Controls.Add(this.treeListDiagRep);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DiagLibForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "诊断管理";
            this.Load += new System.EventHandler(this.DiagLibForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.treeListDiagRep)).EndInit();
            this.treeListDiagRep.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditContent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList treeListDiagRep;
        private DevExpress.XtraEditors.MemoEdit memoEditContent;
        private DevExpress.XtraEditors.SimpleButton simpleButtonInsert1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonExit;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn4;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditType;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItemNew2;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDelet2;
        private DevExpress.XtraBars.BarButtonItem barButtonItemModified2;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem btnAddParent;
    }
}