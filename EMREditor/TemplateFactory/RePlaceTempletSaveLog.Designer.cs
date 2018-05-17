namespace DrectSoft.Emr.TemplateFactory
{
    partial class RePlaceTempletSaveLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RePlaceTempletSaveLog));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.treeList_Template = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn6 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn8 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_check = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection();
            this.lookUpWindowHeader = new DrectSoft.Common.Library.LookUpWindow();
            this.lookUpWindowFoot = new DrectSoft.Common.Library.LookUpWindow();
            this.simpleButton = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset();
            this.btn_Close = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList_Template)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowFoot)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.treeList_Template);
            this.panelControl1.Location = new System.Drawing.Point(35, 26);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(321, 553);
            this.panelControl1.TabIndex = 4;
            // 
            // treeList_Template
            // 
            this.treeList_Template.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn6,
            this.treeListColumn8});
            this.treeList_Template.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList_Template.Location = new System.Drawing.Point(2, 2);
            this.treeList_Template.Name = "treeList_Template";
            this.treeList_Template.OptionsBehavior.Editable = false;
            this.treeList_Template.OptionsView.ShowCheckBoxes = true;
            this.treeList_Template.OptionsView.ShowColumns = false;
            this.treeList_Template.OptionsView.ShowIndicator = false;
            this.treeList_Template.Size = new System.Drawing.Size(317, 549);
            this.treeList_Template.TabIndex = 0;
            this.treeList_Template.TabStop = false;
            this.treeList_Template.BeforeCheckNode += new DevExpress.XtraTreeList.CheckNodeEventHandler(this.treeList_Template_BeforeCheckNode);
            this.treeList_Template.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.treeList_Template_AfterCheckNode);
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
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gridControl1);
            this.panelControl2.Location = new System.Drawing.Point(474, 26);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(321, 497);
            this.panelControl2.TabIndex = 6;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(317, 493);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.TabStop = false;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.IndicatorWidth = 40;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridView1.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridView1.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "模板名称";
            this.gridColumn1.FieldName = "NAME";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // btn_check
            // 
            this.btn_check.Location = new System.Drawing.Point(393, 255);
            this.btn_check.Name = "btn_check";
            this.btn_check.Size = new System.Drawing.Size(51, 23);
            this.btn_check.TabIndex = 5;
            this.btn_check.TabStop = false;
            this.btn_check.Text = ">>";
            this.btn_check.ToolTip = "右移";
            this.btn_check.Click += new System.EventHandler(this.btn_check_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "模板列表：";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl4.Location = new System.Drawing.Point(474, 529);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(287, 14);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "注：此功能将删除模板中Savelogs节点中的日志信息。";
            // 
            // btn_Save
            // 
            this.btn_Save.ImageIndex = 0;
            this.btn_Save.ImageList = this.imageCollection1;
            this.btn_Save.Location = new System.Drawing.Point(543, 553);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(80, 27);
            this.btn_Save.TabIndex = 0;
            this.btn_Save.Text = "执行 (&E)";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "执行.ico");
            // 
            // lookUpWindowHeader
            // 
            this.lookUpWindowHeader.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowHeader.GenShortCode = null;
            this.lookUpWindowHeader.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowHeader.Owner = null;
            this.lookUpWindowHeader.SqlHelper = null;
            // 
            // lookUpWindowFoot
            // 
            this.lookUpWindowFoot.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowFoot.GenShortCode = null;
            this.lookUpWindowFoot.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowFoot.Owner = null;
            this.lookUpWindowFoot.SqlHelper = null;
            // 
            // simpleButton
            // 
            this.simpleButton.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton.Image")));
            this.simpleButton.Location = new System.Drawing.Point(629, 553);
            this.simpleButton.Name = "simpleButton";
            this.simpleButton.Size = new System.Drawing.Size(80, 27);
            this.simpleButton.TabIndex = 1;
            this.simpleButton.Text = "重置(&B)";
            this.simpleButton.Click += new System.EventHandler(this.simpleButton_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Image = ((System.Drawing.Image)(resources.GetObject("btn_Close.Image")));
            this.btn_Close.Location = new System.Drawing.Point(715, 553);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(80, 27);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "关闭(&T)";
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // RePlaceTempletSaveLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 591);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.simpleButton);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btn_check);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RePlaceTempletSaveLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "模板页眉页脚批量替换工具";
            this.Load += new System.EventHandler(this.RePlaceTempletTitle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList_Template)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowFoot)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btn_check;
        private DevExpress.XtraTreeList.TreeList treeList_Template;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn6;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn8;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton btn_Save;
        private Common.Library.LookUpWindow lookUpWindowHeader;
        private Common.Library.LookUpWindow lookUpWindowFoot;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset simpleButton;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClose btn_Close;
    }
}