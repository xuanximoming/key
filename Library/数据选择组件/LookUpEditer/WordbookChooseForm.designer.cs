namespace DrectSoft.Common.Library
{
    partial class WordbookChooseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WordbookChooseForm));
            this.splitContainer1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.WordbookTree = new DrectSoft.Wordbook.UCtrlWordbookTree();
            this.panel2 = new DevExpress.XtraEditors.PanelControl();
            this.gridCtrlPara = new DevExpress.XtraGrid.GridControl();
            this.gridViewPara = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColParaName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColParaType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColParaEnabled = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColParaValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColParaMemo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCtrlData = new DevExpress.XtraGrid.GridControl();
            this.gridViewData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnPreviewData = new DevExpress.XtraEditors.SimpleButton();
            this.tBoxName = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panel2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlPara)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPara)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBoxName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.WordbookTree);
            this.splitContainer1.Panel1.MinSize = 170;
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(721, 416);
            this.splitContainer1.SplitterPosition = 237;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // WordbookTree
            // 
            this.WordbookTree.Location = new System.Drawing.Point(0, 2);
            this.WordbookTree.Name = "WordbookTree";
            this.WordbookTree.Size = new System.Drawing.Size(235, 410);
            this.WordbookTree.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridCtrlPara);
            this.panel2.Controls.Add(this.gridCtrlData);
            this.panel2.Controls.Add(this.btnPreviewData);
            this.panel2.Controls.Add(this.tBoxName);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(479, 386);
            this.panel2.TabIndex = 23;
            // 
            // gridCtrlPara
            // 
            this.gridCtrlPara.Location = new System.Drawing.Point(76, 266);
            this.gridCtrlPara.MainView = this.gridViewPara;
            this.gridCtrlPara.Name = "gridCtrlPara";
            this.gridCtrlPara.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gridCtrlPara.Size = new System.Drawing.Size(388, 110);
            this.gridCtrlPara.TabIndex = 37;
            this.gridCtrlPara.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewPara});
            this.gridCtrlPara.Leave += new System.EventHandler(this.gridCtrlPara_Leave);
            // 
            // gridViewPara
            // 
            this.gridViewPara.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColParaName,
            this.gridColParaType,
            this.gridColParaEnabled,
            this.gridColParaValue,
            this.gridColParaMemo});
            this.gridViewPara.GridControl = this.gridCtrlPara;
            this.gridViewPara.Name = "gridViewPara";
            this.gridViewPara.OptionsBehavior.AutoPopulateColumns = false;
            this.gridViewPara.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewPara.OptionsCustomization.AllowColumnResizing = false;
            this.gridViewPara.OptionsCustomization.AllowFilter = false;
            this.gridViewPara.OptionsCustomization.AllowGroup = false;
            this.gridViewPara.OptionsCustomization.AllowSort = false;
            this.gridViewPara.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewPara.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewPara.OptionsMenu.EnableColumnMenu = false;
            this.gridViewPara.OptionsMenu.EnableFooterMenu = false;
            this.gridViewPara.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewPara.OptionsView.ColumnAutoWidth = false;
            this.gridViewPara.OptionsView.ShowGroupPanel = false;
            this.gridViewPara.OptionsView.ShowIndicator = false;
            this.gridViewPara.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridViewPara_CustomRowCellEdit);
            // 
            // gridColParaName
            // 
            this.gridColParaName.Caption = "名称";
            this.gridColParaName.FieldName = "Name";
            this.gridColParaName.Name = "gridColParaName";
            this.gridColParaName.OptionsColumn.AllowEdit = false;
            this.gridColParaName.Visible = true;
            this.gridColParaName.VisibleIndex = 0;
            // 
            // gridColParaType
            // 
            this.gridColParaType.Caption = "字符型";
            this.gridColParaType.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColParaType.FieldName = "IsString";
            this.gridColParaType.Name = "gridColParaType";
            this.gridColParaType.OptionsColumn.AllowEdit = false;
            this.gridColParaType.Visible = true;
            this.gridColParaType.VisibleIndex = 1;
            this.gridColParaType.Width = 50;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // gridColParaEnabled
            // 
            this.gridColParaEnabled.Caption = "启用";
            this.gridColParaEnabled.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColParaEnabled.FieldName = "Enable";
            this.gridColParaEnabled.Name = "gridColParaEnabled";
            this.gridColParaEnabled.Visible = true;
            this.gridColParaEnabled.VisibleIndex = 2;
            this.gridColParaEnabled.Width = 38;
            // 
            // gridColParaValue
            // 
            this.gridColParaValue.Caption = "默认值";
            this.gridColParaValue.FieldName = "Value";
            this.gridColParaValue.Name = "gridColParaValue";
            this.gridColParaValue.Visible = true;
            this.gridColParaValue.VisibleIndex = 3;
            this.gridColParaValue.Width = 100;
            // 
            // gridColParaMemo
            // 
            this.gridColParaMemo.Caption = "备注";
            this.gridColParaMemo.FieldName = "Description";
            this.gridColParaMemo.Name = "gridColParaMemo";
            this.gridColParaMemo.OptionsColumn.AllowEdit = false;
            this.gridColParaMemo.Visible = true;
            this.gridColParaMemo.VisibleIndex = 4;
            this.gridColParaMemo.Width = 200;
            // 
            // gridCtrlData
            // 
            this.gridCtrlData.Location = new System.Drawing.Point(76, 27);
            this.gridCtrlData.MainView = this.gridViewData;
            this.gridCtrlData.Name = "gridCtrlData";
            this.gridCtrlData.Size = new System.Drawing.Size(388, 236);
            this.gridCtrlData.TabIndex = 36;
            this.gridCtrlData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewData});
            // 
            // gridViewData
            // 
            this.gridViewData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewData.GridControl = this.gridCtrlData;
            this.gridViewData.Name = "gridViewData";
            this.gridViewData.OptionsBehavior.Editable = false;
            this.gridViewData.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewData.OptionsCustomization.AllowFilter = false;
            this.gridViewData.OptionsCustomization.AllowGroup = false;
            this.gridViewData.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewData.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewData.OptionsMenu.EnableColumnMenu = false;
            this.gridViewData.OptionsMenu.EnableFooterMenu = false;
            this.gridViewData.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewData.OptionsView.ColumnAutoWidth = false;
            this.gridViewData.OptionsView.ShowGroupPanel = false;
            this.gridViewData.OptionsView.ShowIndicator = false;
            // 
            // btnPreviewData
            // 
            this.btnPreviewData.Location = new System.Drawing.Point(5, 30);
            this.btnPreviewData.Name = "btnPreviewData";
            this.btnPreviewData.Size = new System.Drawing.Size(65, 25);
            this.btnPreviewData.TabIndex = 35;
            this.btnPreviewData.Text = "数据预览";
            this.btnPreviewData.Click += new System.EventHandler(this.btnPreviewData_Click);
            // 
            // tBoxName
            // 
            this.tBoxName.Location = new System.Drawing.Point(76, 3);
            this.tBoxName.Name = "tBoxName";
            this.tBoxName.Properties.Appearance.BackColor = System.Drawing.SystemColors.Info;
            this.tBoxName.Properties.Appearance.Options.UseBackColor = true;
            this.tBoxName.Size = new System.Drawing.Size(389, 20);
            this.tBoxName.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 273);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 31;
            this.label4.Text = "过滤参数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "字典名称";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancle);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 386);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(479, 30);
            this.panel1.TabIndex = 22;
            // 
            // btnCancle
            // 
            this.btnCancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancle.Location = new System.Drawing.Point(280, 3);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(85, 25);
            this.btnCancle.TabIndex = 14;
            this.btnCancle.Text = "取消";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(87, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(85, 25);
            this.btnOk.TabIndex = 13;
            this.btnOk.Text = "确定";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 100;
            this.toolTip1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // WordbookChooseForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancle;
            this.ClientSize = new System.Drawing.Size(721, 416);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 337);
            this.Name = "WordbookChooseForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择并设置需要使用的代码字典";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panel2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlPara)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPara)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBoxName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainer1;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.PanelControl panel1;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
        private System.Windows.Forms.ToolTip toolTip1;
        private DevExpress.XtraEditors.TextEdit tBoxName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton btnPreviewData;
        private DevExpress.XtraEditors.PanelControl panel2;
        private DevExpress.XtraGrid.GridControl gridCtrlData;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewData;
        private DevExpress.XtraGrid.GridControl gridCtrlPara;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewPara;
        private DevExpress.XtraGrid.Columns.GridColumn gridColParaName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColParaType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColParaEnabled;
        private DevExpress.XtraGrid.Columns.GridColumn gridColParaValue;
        private DevExpress.XtraGrid.Columns.GridColumn gridColParaMemo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private Wordbook.UCtrlWordbookTree WordbookTree;

    }
}