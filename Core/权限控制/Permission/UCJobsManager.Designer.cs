namespace DrectSoft.Core.Permission
{
    partial class UCJobsManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCJobsManager));
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager();
            this.dockPanelJobsAll = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.treeListJobsAll = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumnJobs = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelControlBpttom = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.groupControlBasicInfo = new DevExpress.XtraEditors.GroupControl();
            this.simpleButtonEdit = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonDelete = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonAdd = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonSave = new DevExpress.XtraEditors.SimpleButton();
            this.memoEditMemo = new DevExpress.XtraEditors.MemoEdit();
            this.textEditJobID = new DevExpress.XtraEditors.TextEdit();
            this.textEditJob = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControlPermission = new DevExpress.XtraEditors.GroupControl();
            this.treeListPermission = new DevExpress.XtraTreeList.TreeList();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu();
            this.barButtonItemEdit = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            this.dockPanelJobsAll.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListJobsAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBpttom)).BeginInit();
            this.panelControlBpttom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlBasicInfo)).BeginInit();
            this.groupControlBasicInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditMemo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditJobID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditJob.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlPermission)).BeginInit();
            this.groupControlPermission.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListPermission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // dockManager
            // 
            this.dockManager.Form = this;
            this.dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanelJobsAll});
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // dockPanelJobsAll
            // 
            this.dockPanelJobsAll.Controls.Add(this.dockPanel1_Container);
            this.dockPanelJobsAll.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanelJobsAll.ID = new System.Guid("3b94750f-b131-4bfc-90a9-c4b09411c1ac");
            this.dockPanelJobsAll.Location = new System.Drawing.Point(0, 0);
            this.dockPanelJobsAll.Name = "dockPanelJobsAll";
            this.dockPanelJobsAll.Options.AllowFloating = false;
            this.dockPanelJobsAll.Options.FloatOnDblClick = false;
            this.dockPanelJobsAll.Options.ShowAutoHideButton = false;
            this.dockPanelJobsAll.Options.ShowCloseButton = false;
            this.dockPanelJobsAll.Options.ShowMaximizeButton = false;
            this.dockPanelJobsAll.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanelJobsAll.Size = new System.Drawing.Size(200, 815);
            this.dockPanelJobsAll.Text = "岗位信息";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.treeListJobsAll);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(192, 788);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // treeListJobsAll
            // 
            this.treeListJobsAll.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumnJobs});
            this.treeListJobsAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListJobsAll.Location = new System.Drawing.Point(0, 0);
            this.treeListJobsAll.Name = "treeListJobsAll";
            this.treeListJobsAll.BeginUnboundLoad();
            this.treeListJobsAll.AppendNode(new object[] {
            null}, -1);
            this.treeListJobsAll.AppendNode(new object[] {
            null}, 0);
            this.treeListJobsAll.EndUnboundLoad();
            this.treeListJobsAll.OptionsView.ShowColumns = false;
            this.treeListJobsAll.OptionsView.ShowIndentAsRowStyle = true;
            this.treeListJobsAll.OptionsView.ShowIndicator = false;
            this.treeListJobsAll.OptionsView.ShowRoot = false;
            this.treeListJobsAll.OptionsView.ShowVertLines = false;
            this.treeListJobsAll.Size = new System.Drawing.Size(192, 788);
            this.treeListJobsAll.TabIndex = 0;
            this.treeListJobsAll.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListJobsAll_FocusedNodeChanged);
            this.treeListJobsAll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeListJobsAll_MouseUp);
            // 
            // treeListColumnJobs
            // 
            this.treeListColumnJobs.Caption = "名称";
            this.treeListColumnJobs.FieldName = "Title";
            this.treeListColumnJobs.Name = "treeListColumnJobs";
            this.treeListColumnJobs.OptionsColumn.AllowEdit = false;
            this.treeListColumnJobs.OptionsColumn.ReadOnly = true;
            this.treeListColumnJobs.Visible = true;
            this.treeListColumnJobs.VisibleIndex = 0;
            // 
            // panelControlBpttom
            // 
            this.panelControlBpttom.Controls.Add(this.simpleButtonCancel);
            this.panelControlBpttom.Controls.Add(this.simpleButtonOK);
            this.panelControlBpttom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControlBpttom.Location = new System.Drawing.Point(200, 760);
            this.panelControlBpttom.Name = "panelControlBpttom";
            this.panelControlBpttom.Size = new System.Drawing.Size(1233, 55);
            this.panelControlBpttom.TabIndex = 6;
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonCancel.Image")));
            this.simpleButtonCancel.Location = new System.Drawing.Point(1136, 13);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonCancel.TabIndex = 3;
            this.simpleButtonCancel.Text = "取消(&C)";
            this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonOK.Image")));
            this.simpleButtonOK.Location = new System.Drawing.Point(1030, 13);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonOK.TabIndex = 0;
            this.simpleButtonOK.Text = "确定(&Y)";
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButtonOK_Click);
            // 
            // groupControlBasicInfo
            // 
            this.groupControlBasicInfo.Controls.Add(this.simpleButtonEdit);
            this.groupControlBasicInfo.Controls.Add(this.simpleButtonDelete);
            this.groupControlBasicInfo.Controls.Add(this.simpleButtonAdd);
            this.groupControlBasicInfo.Controls.Add(this.simpleButtonSave);
            this.groupControlBasicInfo.Controls.Add(this.memoEditMemo);
            this.groupControlBasicInfo.Controls.Add(this.textEditJobID);
            this.groupControlBasicInfo.Controls.Add(this.textEditJob);
            this.groupControlBasicInfo.Controls.Add(this.labelControl3);
            this.groupControlBasicInfo.Controls.Add(this.labelControl2);
            this.groupControlBasicInfo.Controls.Add(this.labelControl1);
            this.groupControlBasicInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControlBasicInfo.Location = new System.Drawing.Point(200, 0);
            this.groupControlBasicInfo.Name = "groupControlBasicInfo";
            this.groupControlBasicInfo.Size = new System.Drawing.Size(1233, 215);
            this.groupControlBasicInfo.TabIndex = 0;
            this.groupControlBasicInfo.Text = "基本信息";
            // 
            // simpleButtonEdit
            // 
            this.simpleButtonEdit.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEdit.Image")));
            this.simpleButtonEdit.Location = new System.Drawing.Point(279, 173);
            this.simpleButtonEdit.Name = "simpleButtonEdit";
            this.simpleButtonEdit.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonEdit.TabIndex = 17;
            this.simpleButtonEdit.Text = "编辑(&E)";
            this.simpleButtonEdit.Click += new System.EventHandler(this.simpleButtonEdit_Click);
            // 
            // simpleButtonDelete
            // 
            this.simpleButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonDelete.Image")));
            this.simpleButtonDelete.Location = new System.Drawing.Point(555, 173);
            this.simpleButtonDelete.Name = "simpleButtonDelete";
            this.simpleButtonDelete.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonDelete.TabIndex = 2;
            this.simpleButtonDelete.Text = "删除(&D)";
            this.simpleButtonDelete.Click += new System.EventHandler(this.simpleButtonDelete_Click);
            // 
            // simpleButtonAdd
            // 
            this.simpleButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonAdd.Image")));
            this.simpleButtonAdd.Location = new System.Drawing.Point(141, 173);
            this.simpleButtonAdd.Name = "simpleButtonAdd";
            this.simpleButtonAdd.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonAdd.TabIndex = 0;
            this.simpleButtonAdd.Text = "新增(&N)";
            this.simpleButtonAdd.Click += new System.EventHandler(this.simpleButtonAdd_Click);
            // 
            // simpleButtonSave
            // 
            this.simpleButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonSave.Image")));
            this.simpleButtonSave.Location = new System.Drawing.Point(417, 173);
            this.simpleButtonSave.Name = "simpleButtonSave";
            this.simpleButtonSave.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonSave.TabIndex = 1;
            this.simpleButtonSave.Text = "保存(&S)";
            this.simpleButtonSave.Click += new System.EventHandler(this.simpleButtonSave_Click);
            // 
            // memoEditMemo
            // 
            this.memoEditMemo.Enabled = false;
            this.memoEditMemo.Location = new System.Drawing.Point(141, 92);
            this.memoEditMemo.Name = "memoEditMemo";
            this.memoEditMemo.Size = new System.Drawing.Size(495, 62);
            this.memoEditMemo.TabIndex = 6;
            // 
            // textEditJobID
            // 
            this.textEditJobID.Enabled = false;
            this.textEditJobID.Location = new System.Drawing.Point(492, 43);
            this.textEditJobID.Name = "textEditJobID";
            this.textEditJobID.Size = new System.Drawing.Size(143, 21);
            this.textEditJobID.TabIndex = 5;
            // 
            // textEditJob
            // 
            this.textEditJob.Enabled = false;
            this.textEditJob.Location = new System.Drawing.Point(141, 43);
            this.textEditJob.Name = "textEditJob";
            this.textEditJob.Size = new System.Drawing.Size(143, 21);
            this.textEditJob.TabIndex = 4;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(59, 97);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(64, 14);
            this.labelControl3.TabIndex = 14;
            this.labelControl3.Text = "岗位描述 ：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(425, 47);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(52, 14);
            this.labelControl2.TabIndex = 15;
            this.labelControl2.Text = "岗位ID ：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(59, 47);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(64, 14);
            this.labelControl1.TabIndex = 16;
            this.labelControl1.Text = "岗位名称 ：";
            // 
            // groupControlPermission
            // 
            this.groupControlPermission.Controls.Add(this.treeListPermission);
            this.groupControlPermission.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlPermission.Location = new System.Drawing.Point(200, 215);
            this.groupControlPermission.Name = "groupControlPermission";
            this.groupControlPermission.Size = new System.Drawing.Size(1233, 545);
            this.groupControlPermission.TabIndex = 17;
            this.groupControlPermission.Text = "授权控制";
            // 
            // treeListPermission
            // 
            this.treeListPermission.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListPermission.KeyFieldName = "";
            this.treeListPermission.Location = new System.Drawing.Point(2, 23);
            this.treeListPermission.Name = "treeListPermission";
            this.treeListPermission.OptionsBehavior.AllowIndeterminateCheckState = true;
            this.treeListPermission.OptionsView.ShowColumns = false;
            this.treeListPermission.OptionsView.ShowIndicator = false;
            this.treeListPermission.ParentFieldName = "";
            this.treeListPermission.Size = new System.Drawing.Size(1229, 520);
            this.treeListPermission.TabIndex = 0;
            this.treeListPermission.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.treeListPermission_AfterCheckNode);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemEdit),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemAdd),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemDelete)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // barButtonItemEdit
            // 
            this.barButtonItemEdit.Caption = "修改岗位";
            this.barButtonItemEdit.Id = 1;
            this.barButtonItemEdit.Name = "barButtonItemEdit";
            this.barButtonItemEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemEdit_ItemClick);
            // 
            // barButtonItemAdd
            // 
            this.barButtonItemAdd.Caption = "新增岗位";
            this.barButtonItemAdd.Id = 3;
            this.barButtonItemAdd.Name = "barButtonItemAdd";
            this.barButtonItemAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAdd_ItemClick);
            // 
            // barButtonItemDelete
            // 
            this.barButtonItemDelete.Caption = "删除岗位";
            this.barButtonItemDelete.Id = 4;
            this.barButtonItemDelete.Name = "barButtonItemDelete";
            this.barButtonItemDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDelete_ItemClick);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItemEdit,
            this.barButtonItem3,
            this.barButtonItemAdd,
            this.barButtonItemDelete});
            this.barManager1.MaxItemId = 5;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1433, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 815);
            this.barDockControlBottom.Size = new System.Drawing.Size(1433, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 815);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1433, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 815);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "新增岗位";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "删除岗位";
            this.barButtonItem3.Id = 2;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // UCJobsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControlPermission);
            this.Controls.Add(this.groupControlBasicInfo);
            this.Controls.Add(this.panelControlBpttom);
            this.Controls.Add(this.dockPanelJobsAll);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UCJobsManager";
            this.Size = new System.Drawing.Size(1433, 815);
            this.Load += new System.EventHandler(this.UCJobManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            this.dockPanelJobsAll.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListJobsAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBpttom)).EndInit();
            this.panelControlBpttom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlBasicInfo)).EndInit();
            this.groupControlBasicInfo.ResumeLayout(false);
            this.groupControlBasicInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditMemo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditJobID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditJob.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlPermission)).EndInit();
            this.groupControlPermission.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListPermission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking.DockManager dockManager;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelJobsAll;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraEditors.PanelControl panelControlBpttom;
        private DevExpress.XtraEditors.SimpleButton simpleButtonOK;
        private DevExpress.XtraEditors.GroupControl groupControlBasicInfo;
        private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
        private DevExpress.XtraEditors.SimpleButton simpleButtonDelete;
        private DevExpress.XtraEditors.SimpleButton simpleButtonAdd;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSave;
        private DevExpress.XtraEditors.MemoEdit memoEditMemo;
        private DevExpress.XtraEditors.TextEdit textEditJobID;
        private DevExpress.XtraEditors.TextEdit textEditJob;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControlPermission;
        private DevExpress.XtraTreeList.TreeList treeListPermission;
        private DevExpress.XtraEditors.SimpleButton simpleButtonEdit;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraTreeList.TreeList treeListJobsAll;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumnJobs;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemEdit;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAdd;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDelete;
    }
}
