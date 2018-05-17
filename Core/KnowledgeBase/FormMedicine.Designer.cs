namespace DrectSoft.Core.KnowledgeBase
{
    partial class FormMedicine
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMedicine));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.treeList_Medicine = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn6 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn8 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtApplyTo = new System.Windows.Forms.TextBox();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtReferenceUsage = new System.Windows.Forms.TextBox();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtSpecification = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList_Medicine)).BeginInit();
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
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(645, 730);
            this.panelControl1.TabIndex = 0;
            // 
            // treeList_Medicine
            // 
            this.treeList_Medicine.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn6,
            this.treeListColumn8});
            this.treeList_Medicine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList_Medicine.Location = new System.Drawing.Point(2, 2);
            this.treeList_Medicine.Name = "treeList_Medicine";
            this.treeList_Medicine.OptionsBehavior.Editable = false;
            this.treeList_Medicine.OptionsView.ShowColumns = false;
            this.treeList_Medicine.OptionsView.ShowIndicator = false;
            this.treeList_Medicine.Size = new System.Drawing.Size(641, 726);
            this.treeList_Medicine.TabIndex = 2;
            this.treeList_Medicine.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList_Medicine_FocusedNodeChanged);
            this.treeList_Medicine.DoubleClick += new System.EventHandler(this.treeList_Medicine_DoubleClick);
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
            // treeListColumn8
            // 
            this.treeListColumn8.Caption = "treeListColumn8";
            this.treeListColumn8.FieldName = "KeyID";
            this.treeListColumn8.Name = "treeListColumn8";
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(645, 0);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 730);
            this.splitterControl1.TabIndex = 1;
            this.splitterControl1.TabStop = false;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.groupControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(650, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(720, 730);
            this.panelControl2.TabIndex = 2;
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl1.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl1.Controls.Add(this.txtApplyTo);
            this.groupControl1.Controls.Add(this.txtMemo);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.txtReferenceUsage);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.txtSpecification);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.txtName);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(716, 726);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "药品使用说明";
            // 
            // txtApplyTo
            // 
            this.txtApplyTo.Location = new System.Drawing.Point(126, 201);
            this.txtApplyTo.Multiline = true;
            this.txtApplyTo.Name = "txtApplyTo";
            this.txtApplyTo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtApplyTo.Size = new System.Drawing.Size(699, 105);
            this.txtApplyTo.TabIndex = 3;
            // 
            // txtMemo
            // 
            this.txtMemo.Location = new System.Drawing.Point(126, 496);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMemo.Size = new System.Drawing.Size(699, 105);
            this.txtMemo.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(56, 80);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "药品名称：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(56, 142);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "药品规格：";
            // 
            // txtReferenceUsage
            // 
            this.txtReferenceUsage.Location = new System.Drawing.Point(126, 348);
            this.txtReferenceUsage.Multiline = true;
            this.txtReferenceUsage.Name = "txtReferenceUsage";
            this.txtReferenceUsage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReferenceUsage.Size = new System.Drawing.Size(699, 105);
            this.txtReferenceUsage.TabIndex = 3;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(70, 244);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 1;
            this.labelControl3.Text = "适用症：";
            // 
            // txtSpecification
            // 
            this.txtSpecification.Location = new System.Drawing.Point(126, 136);
            this.txtSpecification.Name = "txtSpecification";
            this.txtSpecification.Size = new System.Drawing.Size(700, 20);
            this.txtSpecification.TabIndex = 2;
            this.txtSpecification.ToolTip = "药品规格";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(84, 399);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "用法：";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(126, 75);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(700, 20);
            this.txtName.TabIndex = 2;
            this.txtName.ToolTip = "药品名称";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(84, 540);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 1;
            this.labelControl5.Text = "备注：";
            // 
            // FormMedicine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 730);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMedicine";
            this.Text = "药品浏览";
            this.Load += new System.EventHandler(this.FormMedicine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList_Medicine)).EndInit();
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
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn8;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtSpecification;
        private DevExpress.XtraEditors.TextEdit txtName;
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.TextBox txtReferenceUsage;
        private System.Windows.Forms.TextBox txtApplyTo;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}

