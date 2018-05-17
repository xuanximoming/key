namespace DrectSoft.Core.KnowledgeBase
{
    partial class UCMedicineDirect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMedicineDirect));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.treeListFind = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageCollection_Tree = new DevExpress.Utils.ImageCollection(this.components);
            this.treeList_Medicine = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn6 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn8 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.txtFind = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.btnFind = new DrectSoft.Common.Ctrs.OTHER.DevButtonFind(this.components);
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtDoseform = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtDirectTitle2 = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtDirectTitle = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtDirectContent = new System.Windows.Forms.TextBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListFind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection_Tree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList_Medicine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDoseform.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDirectTitle2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDirectTitle.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.treeListFind);
            this.panelControl1.Controls.Add(this.treeList_Medicine);
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(343, 656);
            this.panelControl1.TabIndex = 0;
            // 
            // treeListFind
            // 
            this.treeListFind.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2,
            this.treeListColumn3});
            this.treeListFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListFind.Location = new System.Drawing.Point(2, 103);
            this.treeListFind.Name = "treeListFind";
            this.treeListFind.OptionsBehavior.Editable = false;
            this.treeListFind.OptionsView.ShowColumns = false;
            this.treeListFind.OptionsView.ShowIndicator = false;
            this.treeListFind.Size = new System.Drawing.Size(339, 551);
            this.treeListFind.StateImageList = this.imageCollection_Tree;
            this.treeListFind.TabIndex = 6;
            this.treeListFind.Visible = false;
            this.treeListFind.GetStateImage += new DevExpress.XtraTreeList.GetStateImageEventHandler(this.treeList_Medicine_GetStateImage);
            this.treeListFind.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList_Medicine_FocusedNodeChanged);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "treeListColumn6";
            this.treeListColumn1.FieldName = "NAME";
            this.treeListColumn1.MinWidth = 35;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 315;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.FieldName = "NODETYPE";
            this.treeListColumn2.Name = "treeListColumn2";
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.Caption = "treeListColumn8";
            this.treeListColumn3.FieldName = "KeyID";
            this.treeListColumn3.Name = "treeListColumn3";
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
            this.treeList_Medicine.Size = new System.Drawing.Size(339, 551);
            this.treeList_Medicine.StateImageList = this.imageCollection_Tree;
            this.treeList_Medicine.TabIndex = 4;
            this.treeList_Medicine.GetStateImage += new DevExpress.XtraTreeList.GetStateImageEventHandler(this.treeList_Medicine_GetStateImage);
            this.treeList_Medicine.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList_Medicine_FocusedNodeChanged);
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
            this.colType.FieldName = "NODETYPE";
            this.colType.Name = "colType";
            // 
            // treeListColumn8
            // 
            this.treeListColumn8.Caption = "treeListColumn8";
            this.treeListColumn8.FieldName = "KeyID";
            this.treeListColumn8.Name = "treeListColumn8";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.txtFind);
            this.panelControl3.Controls.Add(this.btnFind);
            this.panelControl3.Controls.Add(this.labelControl6);
            this.panelControl3.Controls.Add(this.labelControl4);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(2, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(339, 101);
            this.panelControl3.TabIndex = 5;
            // 
            // txtFind
            // 
            this.txtFind.EnterMoveNextControl = true;
            this.txtFind.IsEnterChangeBgColor = false;
            this.txtFind.IsEnterKeyToNextControl = false;
            this.txtFind.IsNumber = false;
            this.txtFind.Location = new System.Drawing.Point(84, 18);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(239, 20);
            this.txtFind.TabIndex = 40;
            // 
            // btnFind
            // 
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.Location = new System.Drawing.Point(243, 59);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 23);
            this.btnFind.TabIndex = 41;
            this.btnFind.Text = "搜索(&F)";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl6.Location = new System.Drawing.Point(14, 59);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(192, 14);
            this.labelControl6.TabIndex = 1;
            this.labelControl6.Text = "注：支持药品说明书名称和拼音搜索";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(17, 22);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "搜索条件：";
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(343, 0);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 656);
            this.splitterControl1.TabIndex = 1;
            this.splitterControl1.TabStop = false;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.groupControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(348, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(963, 656);
            this.panelControl2.TabIndex = 2;
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl1.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl1.Controls.Add(this.txtDoseform);
            this.groupControl1.Controls.Add(this.txtDirectTitle2);
            this.groupControl1.Controls.Add(this.txtDirectTitle);
            this.groupControl1.Controls.Add(this.txtDirectContent);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(959, 652);
            this.groupControl1.TabIndex = 15;
            this.groupControl1.Text = "药品说明书";
            // 
            // txtDoseform
            // 
            this.txtDoseform.IsEnterChangeBgColor = false;
            this.txtDoseform.IsEnterKeyToNextControl = false;
            this.txtDoseform.IsNumber = false;
            this.txtDoseform.Location = new System.Drawing.Point(207, 119);
            this.txtDoseform.Name = "txtDoseform";
            this.txtDoseform.Size = new System.Drawing.Size(623, 20);
            this.txtDoseform.TabIndex = 17;
            this.txtDoseform.ToolTip = "剂型";
            // 
            // txtDirectTitle2
            // 
            this.txtDirectTitle2.IsEnterChangeBgColor = false;
            this.txtDirectTitle2.IsEnterKeyToNextControl = false;
            this.txtDirectTitle2.IsNumber = false;
            this.txtDirectTitle2.Location = new System.Drawing.Point(207, 83);
            this.txtDirectTitle2.Name = "txtDirectTitle2";
            this.txtDirectTitle2.Size = new System.Drawing.Size(623, 20);
            this.txtDirectTitle2.TabIndex = 16;
            this.txtDirectTitle2.ToolTip = "说明书标题2";
            // 
            // txtDirectTitle
            // 
            this.txtDirectTitle.IsEnterChangeBgColor = false;
            this.txtDirectTitle.IsEnterKeyToNextControl = false;
            this.txtDirectTitle.IsNumber = false;
            this.txtDirectTitle.Location = new System.Drawing.Point(206, 47);
            this.txtDirectTitle.Name = "txtDirectTitle";
            this.txtDirectTitle.Size = new System.Drawing.Size(623, 20);
            this.txtDirectTitle.TabIndex = 15;
            this.txtDirectTitle.ToolTip = "说明书标题";
            // 
            // txtDirectContent
            // 
            this.txtDirectContent.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.txtDirectContent.Location = new System.Drawing.Point(207, 160);
            this.txtDirectContent.Multiline = true;
            this.txtDirectContent.Name = "txtDirectContent";
            this.txtDirectContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDirectContent.Size = new System.Drawing.Size(623, 468);
            this.txtDirectContent.TabIndex = 14;
            this.txtDirectContent.Tag = "备注";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(126, 50);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "说明书标题：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(162, 363);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 13;
            this.labelControl5.Text = "备注：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(162, 122);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 14);
            this.labelControl3.TabIndex = 11;
            this.labelControl3.Text = "剂型：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(119, 87);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(79, 14);
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "说明书标题2：";
            // 
            // UCMedicineDirect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.panelControl1);
            this.Name = "UCMedicineDirect";
            this.Size = new System.Drawing.Size(1311, 656);
            this.Load += new System.EventHandler(this.UCMedicineDirect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListFind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection_Tree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList_Medicine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDoseform.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDirectTitle2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDirectTitle.Properties)).EndInit();
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
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.TextBox txtDirectContent;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonFind btnFind;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.Utils.ImageCollection imageCollection_Tree;
        private DevExpress.XtraTreeList.TreeList treeListFind;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtFind;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtDoseform;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtDirectTitle2;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtDirectTitle;
    }
}
