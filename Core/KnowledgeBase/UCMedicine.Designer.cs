namespace DrectSoft.Core.KnowledgeBase
{
    partial class UCMedicine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMedicine));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.treeList_Medicine = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn6 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn8 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageCollection_Tree = new DevExpress.Utils.ImageCollection(this.components);
            this.panelQuery = new System.Windows.Forms.Panel();
            this.tbQuery = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.btnFind = new DrectSoft.Common.Ctrs.OTHER.DevButtonFind(this.components);
            this.lbQuery = new System.Windows.Forms.Label();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtSpecification = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtName = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtApplyTo = new System.Windows.Forms.TextBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtReferenceUsage = new System.Windows.Forms.TextBox();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList_Medicine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection_Tree)).BeginInit();
            this.panelQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbQuery.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpecification.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.treeList_Medicine);
            this.panelControl1.Controls.Add(this.panelQuery);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(343, 664);
            this.panelControl1.TabIndex = 0;
            // 
            // treeList_Medicine
            // 
            this.treeList_Medicine.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn6,
            this.colType,
            this.treeListColumn8});
            this.treeList_Medicine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList_Medicine.Location = new System.Drawing.Point(2, 103);
            this.treeList_Medicine.Name = "treeList_Medicine";
            this.treeList_Medicine.OptionsBehavior.Editable = false;
            this.treeList_Medicine.OptionsView.ShowColumns = false;
            this.treeList_Medicine.OptionsView.ShowIndicator = false;
            this.treeList_Medicine.Size = new System.Drawing.Size(339, 559);
            this.treeList_Medicine.StateImageList = this.imageCollection_Tree;
            this.treeList_Medicine.TabIndex = 3;
            this.treeList_Medicine.GetStateImage += new DevExpress.XtraTreeList.GetStateImageEventHandler(this.treeList_Medicine_GetStateImage);
            this.treeList_Medicine.AfterExpand += new DevExpress.XtraTreeList.NodeEventHandler(this.treeList_Medicine_AfterExpand);
            this.treeList_Medicine.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList_Medicine_FocusedNodeChanged_1);
            // 
            // treeListColumn6
            // 
            this.treeListColumn6.Caption = "treeListColumn6";
            this.treeListColumn6.FieldName = "NAME";
            this.treeListColumn6.MinWidth = 35;
            this.treeListColumn6.Name = "treeListColumn6";
            this.treeListColumn6.Visible = true;
            this.treeListColumn6.VisibleIndex = 0;
            this.treeListColumn6.Width = 315;
            // 
            // colType
            // 
            this.colType.Caption = "类别";
            this.colType.FieldName = "NODETYPE";
            this.colType.Name = "colType";
            // 
            // treeListColumn8
            // 
            this.treeListColumn8.Caption = "treeListColumn8";
            this.treeListColumn8.FieldName = "KeyID";
            this.treeListColumn8.Name = "treeListColumn8";
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
            // panelQuery
            // 
            this.panelQuery.Controls.Add(this.tbQuery);
            this.panelQuery.Controls.Add(this.labelControl6);
            this.panelQuery.Controls.Add(this.btnFind);
            this.panelQuery.Controls.Add(this.lbQuery);
            this.panelQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelQuery.Location = new System.Drawing.Point(2, 2);
            this.panelQuery.Name = "panelQuery";
            this.panelQuery.Size = new System.Drawing.Size(339, 101);
            this.panelQuery.TabIndex = 4;
            // 
            // tbQuery
            // 
            this.tbQuery.IsEnterChangeBgColor = false;
            this.tbQuery.IsEnterKeyToNextControl = true;
            this.tbQuery.Location = new System.Drawing.Point(84, 20);
            this.tbQuery.Name = "tbQuery";
            this.tbQuery.Size = new System.Drawing.Size(246, 21);
            this.tbQuery.TabIndex = 6;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl6.Location = new System.Drawing.Point(16, 59);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(216, 14);
            this.labelControl6.TabIndex = 5;
            this.labelControl6.Text = "注：支持药品类别和药品名称关键字查询";
            // 
            // btnFind
            // 
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.Location = new System.Drawing.Point(250, 50);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 23);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "搜索(&F)";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lbQuery
            // 
            this.lbQuery.AutoSize = true;
            this.lbQuery.Location = new System.Drawing.Point(16, 23);
            this.lbQuery.Name = "lbQuery";
            this.lbQuery.Size = new System.Drawing.Size(67, 14);
            this.lbQuery.TabIndex = 1;
            this.lbQuery.Text = "搜索条件：";
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(343, 0);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 664);
            this.splitterControl1.TabIndex = 1;
            this.splitterControl1.TabStop = false;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.groupControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(348, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(989, 664);
            this.panelControl2.TabIndex = 2;
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl1.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl1.Controls.Add(this.txtSpecification);
            this.groupControl1.Controls.Add(this.txtName);
            this.groupControl1.Controls.Add(this.txtMemo);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.txtApplyTo);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.txtReferenceUsage);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(985, 660);
            this.groupControl1.TabIndex = 14;
            this.groupControl1.Text = "药品使用说明";
            // 
            // txtSpecification
            // 
            this.txtSpecification.IsEnterChangeBgColor = false;
            this.txtSpecification.IsEnterKeyToNextControl = false;
            this.txtSpecification.Location = new System.Drawing.Point(142, 114);
            this.txtSpecification.Name = "txtSpecification";
            this.txtSpecification.Size = new System.Drawing.Size(700, 21);
            this.txtSpecification.TabIndex = 15;
            this.txtSpecification.ToolTip = "药品规格";
            // 
            // txtName
            // 
            this.txtName.IsEnterChangeBgColor = false;
            this.txtName.IsEnterKeyToNextControl = false;
            this.txtName.Location = new System.Drawing.Point(142, 67);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(700, 21);
            this.txtName.TabIndex = 14;
            this.txtName.ToolTip = "药品名称";
            // 
            // txtMemo
            // 
            this.txtMemo.Location = new System.Drawing.Point(142, 413);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMemo.Size = new System.Drawing.Size(700, 91);
            this.txtMemo.TabIndex = 11;
            this.txtMemo.Tag = "备注";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(76, 72);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "药品名称：";
            // 
            // txtApplyTo
            // 
            this.txtApplyTo.Location = new System.Drawing.Point(142, 165);
            this.txtApplyTo.Multiline = true;
            this.txtApplyTo.Name = "txtApplyTo";
            this.txtApplyTo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtApplyTo.Size = new System.Drawing.Size(700, 91);
            this.txtApplyTo.TabIndex = 12;
            this.txtApplyTo.Tag = "适用症";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(76, 119);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "药品规格：";
            // 
            // txtReferenceUsage
            // 
            this.txtReferenceUsage.Location = new System.Drawing.Point(142, 289);
            this.txtReferenceUsage.Multiline = true;
            this.txtReferenceUsage.Name = "txtReferenceUsage";
            this.txtReferenceUsage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReferenceUsage.Size = new System.Drawing.Size(700, 91);
            this.txtReferenceUsage.TabIndex = 13;
            this.txtReferenceUsage.Tag = "用法";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(88, 209);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "适用症：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(100, 330);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 5;
            this.labelControl4.Text = "用法：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(100, 454);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 6;
            this.labelControl5.Text = "备注：";
            // 
            // UCMedicine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.panelControl1);
            this.Name = "UCMedicine";
            this.Size = new System.Drawing.Size(1337, 664);
            this.Load += new System.EventHandler(this.UCMedicine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList_Medicine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection_Tree)).EndInit();
            this.panelQuery.ResumeLayout(false);
            this.panelQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbQuery.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpecification.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraTreeList.TreeList treeList_Medicine;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn6;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colType;//王冀 2012-1-29
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn8;
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.TextBox txtReferenceUsage;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.Utils.ImageCollection imageCollection_Tree;
        private System.Windows.Forms.Panel panelQuery;
        private System.Windows.Forms.Label lbQuery;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonFind btnFind;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit tbQuery;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtSpecification;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtName;
        private System.Windows.Forms.TextBox txtApplyTo;
    }
}
