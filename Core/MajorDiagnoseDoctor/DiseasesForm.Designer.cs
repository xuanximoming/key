namespace DrectSoft.Core.MajorDiagnoseDoctor
{
    partial class DiseasesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiseasesForm));
            this.pcTop = new DevExpress.XtraEditors.PanelControl();
            this.btn_clear = new DrectSoft.Common.Ctrs.OTHER.DevButtonClear();
            this.btn_reset = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txt_search = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.lblDiseaseName = new DevExpress.XtraEditors.LabelControl();
            this.lblSearch = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl_disease = new DevExpress.XtraGrid.GridControl();
            this.gridView_disease = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcmSelect = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcmDiseaseCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcmDiseaseName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txt_memo = new DevExpress.XtraEditors.TextEdit();
            this.txt_diseaseArea = new DevExpress.XtraEditors.MemoEdit();
            this.lblGroupName = new DevExpress.XtraEditors.LabelControl();
            this.txt_groupName = new DevExpress.XtraEditors.TextEdit();
            this.lblGroupList = new DevExpress.XtraEditors.LabelControl();
            this.chb_continue = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btnClose = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose();
            this.btnSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave();
            ((System.ComponentModel.ISupportInitialize)(this.pcTop)).BeginInit();
            this.pcTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_disease)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_disease)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_memo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_diseaseArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_groupName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chb_continue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pcTop
            // 
            this.pcTop.Appearance.BackColor = System.Drawing.Color.White;
            this.pcTop.Appearance.Options.UseBackColor = true;
            this.pcTop.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pcTop.Controls.Add(this.btn_clear);
            this.pcTop.Controls.Add(this.btn_reset);
            this.pcTop.Controls.Add(this.labelControl2);
            this.pcTop.Controls.Add(this.txt_search);
            this.pcTop.Controls.Add(this.lblDiseaseName);
            this.pcTop.Controls.Add(this.lblSearch);
            this.pcTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pcTop.Location = new System.Drawing.Point(2, 2);
            this.pcTop.Name = "pcTop";
            this.pcTop.Size = new System.Drawing.Size(804, 60);
            this.pcTop.TabIndex = 0;
            // 
            // btn_clear
            // 
            this.btn_clear.Image = ((System.Drawing.Image)(resources.GetObject("btn_clear.Image")));
            this.btn_clear.Location = new System.Drawing.Point(433, 8);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(80, 27);
            this.btn_clear.TabIndex = 2;
            this.btn_clear.Text = "清空(&L)";
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // btn_reset
            // 
            this.btn_reset.Image = ((System.Drawing.Image)(resources.GetObject("btn_reset.Image")));
            this.btn_reset.Location = new System.Drawing.Point(347, 8);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(80, 27);
            this.btn_reset.TabIndex = 1;
            this.btn_reset.Text = "重置(&B)";
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelControl2.Location = new System.Drawing.Point(13, 39);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(324, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "请先填写组合名称，然后在列表中勾选病种，最后点击保存。";
            // 
            // txt_search
            // 
            this.txt_search.EnterMoveNextControl = true;
            this.txt_search.Location = new System.Drawing.Point(170, 11);
            this.txt_search.Name = "txt_search";
            this.txt_search.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_search.Size = new System.Drawing.Size(150, 20);
            this.txt_search.TabIndex = 0;
            this.txt_search.EditValueChanged += new System.EventHandler(this.txt_search_EditValueChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // lblDiseaseName
            // 
            this.lblDiseaseName.Location = new System.Drawing.Point(110, 14);
            this.lblDiseaseName.Name = "lblDiseaseName";
            this.lblDiseaseName.Size = new System.Drawing.Size(60, 14);
            this.lblDiseaseName.TabIndex = 4;
            this.lblDiseaseName.Text = "病种名称：";
            // 
            // lblSearch
            // 
            this.lblSearch.Location = new System.Drawing.Point(13, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(75, 14);
            this.lblSearch.TabIndex = 3;
            this.lblSearch.Text = "检索条件>>>";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl5);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.pcTop);
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(808, 522);
            this.panelControl1.TabIndex = 0;
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.panelControl2);
            this.panelControl5.Controls.Add(this.panelControl4);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl5.Location = new System.Drawing.Point(2, 62);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(804, 418);
            this.panelControl5.TabIndex = 1;
            // 
            // panelControl2
            // 
            this.panelControl2.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl2.Appearance.Options.UseBackColor = true;
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.gridControl_disease);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(2, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(800, 264);
            this.panelControl2.TabIndex = 1;
            // 
            // gridControl_disease
            // 
            this.gridControl_disease.AccessibleName = "";
            this.gridControl_disease.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_disease.Location = new System.Drawing.Point(0, 0);
            this.gridControl_disease.MainView = this.gridView_disease;
            this.gridControl_disease.Name = "gridControl_disease";
            this.gridControl_disease.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.repositoryItemCheckEdit2});
            this.gridControl_disease.Size = new System.Drawing.Size(800, 264);
            this.gridControl_disease.TabIndex = 0;
            this.gridControl_disease.TabStop = false;
            this.gridControl_disease.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_disease});
            // 
            // gridView_disease
            // 
            this.gridView_disease.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcmSelect,
            this.gcmDiseaseCode,
            this.gcmDiseaseName,
            this.gridColumn1,
            this.gridColumn2});
            this.gridView_disease.GridControl = this.gridControl_disease;
            this.gridView_disease.IndicatorWidth = 40;
            this.gridView_disease.Name = "gridView_disease";
            this.gridView_disease.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView_disease.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView_disease.OptionsCustomization.AllowColumnMoving = false;
            this.gridView_disease.OptionsCustomization.AllowFilter = false;
            this.gridView_disease.OptionsMenu.EnableColumnMenu = false;
            this.gridView_disease.OptionsMenu.EnableFooterMenu = false;
            this.gridView_disease.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView_disease.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridView_disease.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridView_disease.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridView_disease.OptionsView.ShowGroupPanel = false;
            this.gridView_disease.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_disease_CustomDrawRowIndicator);
            this.gridView_disease.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView_disease_CellValueChanging);
            // 
            // gcmSelect
            // 
            this.gcmSelect.AppearanceCell.Options.UseTextOptions = true;
            this.gcmSelect.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gcmSelect.AppearanceHeader.Options.UseTextOptions = true;
            this.gcmSelect.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcmSelect.Caption = "选择";
            this.gcmSelect.FieldName = "FLAG";
            this.gcmSelect.MaxWidth = 40;
            this.gcmSelect.Name = "gcmSelect";
            this.gcmSelect.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gcmSelect.Visible = true;
            this.gcmSelect.VisibleIndex = 0;
            this.gcmSelect.Width = 40;
            // 
            // gcmDiseaseCode
            // 
            this.gcmDiseaseCode.AppearanceCell.Options.UseTextOptions = true;
            this.gcmDiseaseCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gcmDiseaseCode.AppearanceHeader.Options.UseTextOptions = true;
            this.gcmDiseaseCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcmDiseaseCode.Caption = "病种编码";
            this.gcmDiseaseCode.FieldName = "ICD";
            this.gcmDiseaseCode.MaxWidth = 150;
            this.gcmDiseaseCode.MinWidth = 100;
            this.gcmDiseaseCode.Name = "gcmDiseaseCode";
            this.gcmDiseaseCode.OptionsColumn.AllowEdit = false;
            this.gcmDiseaseCode.Visible = true;
            this.gcmDiseaseCode.VisibleIndex = 1;
            this.gcmDiseaseCode.Width = 150;
            // 
            // gcmDiseaseName
            // 
            this.gcmDiseaseName.AppearanceCell.Options.UseTextOptions = true;
            this.gcmDiseaseName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gcmDiseaseName.AppearanceHeader.Options.UseTextOptions = true;
            this.gcmDiseaseName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcmDiseaseName.Caption = "病种名称";
            this.gcmDiseaseName.FieldName = "NAME";
            this.gcmDiseaseName.Name = "gcmDiseaseName";
            this.gcmDiseaseName.OptionsColumn.AllowEdit = false;
            this.gcmDiseaseName.Visible = true;
            this.gcmDiseaseName.VisibleIndex = 2;
            this.gcmDiseaseName.Width = 445;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "拼音";
            this.gridColumn1.FieldName = "PY";
            this.gridColumn1.MaxWidth = 80;
            this.gridColumn1.MinWidth = 50;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            this.gridColumn1.Width = 70;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "五笔";
            this.gridColumn2.FieldName = "WB";
            this.gridColumn2.MaxWidth = 80;
            this.gridColumn2.MinWidth = 50;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 4;
            this.gridColumn2.Width = 70;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.ValueChecked = null;
            this.repositoryItemCheckEdit1.ValueUnchecked = null;
            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.Caption = "Check";
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            // 
            // panelControl4
            // 
            this.panelControl4.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl4.Appearance.Options.UseBackColor = true;
            this.panelControl4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl4.Controls.Add(this.labelControl4);
            this.panelControl4.Controls.Add(this.labelControl3);
            this.panelControl4.Controls.Add(this.txt_memo);
            this.panelControl4.Controls.Add(this.txt_diseaseArea);
            this.panelControl4.Controls.Add(this.lblGroupName);
            this.panelControl4.Controls.Add(this.txt_groupName);
            this.panelControl4.Controls.Add(this.lblGroupList);
            this.panelControl4.Controls.Add(this.chb_continue);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl4.Location = new System.Drawing.Point(2, 266);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(800, 150);
            this.panelControl4.TabIndex = 0;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl4.Location = new System.Drawing.Point(624, 16);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(7, 14);
            this.labelControl4.TabIndex = 5;
            this.labelControl4.Text = "*";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(45, 98);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 14);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "备注：";
            // 
            // txt_memo
            // 
            this.txt_memo.EnterMoveNextControl = true;
            this.txt_memo.Location = new System.Drawing.Point(81, 95);
            this.txt_memo.Name = "txt_memo";
            this.txt_memo.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_memo.Size = new System.Drawing.Size(539, 20);
            this.txt_memo.TabIndex = 2;
            // 
            // txt_diseaseArea
            // 
            this.txt_diseaseArea.Enabled = false;
            this.txt_diseaseArea.EnterMoveNextControl = true;
            this.txt_diseaseArea.Location = new System.Drawing.Point(81, 37);
            this.txt_diseaseArea.Name = "txt_diseaseArea";
            this.txt_diseaseArea.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.txt_diseaseArea.Properties.Appearance.Options.UseBackColor = true;
            this.txt_diseaseArea.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_diseaseArea.Properties.MaxLength = 255;
            this.txt_diseaseArea.Size = new System.Drawing.Size(539, 50);
            this.txt_diseaseArea.TabIndex = 1;
            this.txt_diseaseArea.ToolTip = "申请原因";
            this.txt_diseaseArea.UseOptimizedRendering = true;
            // 
            // lblGroupName
            // 
            this.lblGroupName.Location = new System.Drawing.Point(21, 13);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(60, 14);
            this.lblGroupName.TabIndex = 4;
            this.lblGroupName.Text = "组合名称：";
            // 
            // txt_groupName
            // 
            this.txt_groupName.EnterMoveNextControl = true;
            this.txt_groupName.Location = new System.Drawing.Point(81, 9);
            this.txt_groupName.Name = "txt_groupName";
            this.txt_groupName.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_groupName.Size = new System.Drawing.Size(539, 20);
            this.txt_groupName.TabIndex = 0;
            // 
            // lblGroupList
            // 
            this.lblGroupList.Location = new System.Drawing.Point(21, 56);
            this.lblGroupList.Name = "lblGroupList";
            this.lblGroupList.Size = new System.Drawing.Size(60, 14);
            this.lblGroupList.TabIndex = 6;
            this.lblGroupList.Text = "组合病种：";
            // 
            // chb_continue
            // 
            this.chb_continue.Location = new System.Drawing.Point(19, 124);
            this.chb_continue.Name = "chb_continue";
            this.chb_continue.Properties.Caption = "连续新增";
            this.chb_continue.Size = new System.Drawing.Size(75, 19);
            this.chb_continue.TabIndex = 3;
            this.chb_continue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chb_KeyPress);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(670, 218);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(0, 14);
            this.labelControl1.TabIndex = 2;
            // 
            // panelControl3
            // 
            this.panelControl3.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl3.Appearance.Options.UseBackColor = true;
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.btnClose);
            this.panelControl3.Controls.Add(this.btnSave);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(2, 480);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(804, 40);
            this.panelControl3.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(714, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 27);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭(&T)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(628, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 27);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // DiseasesForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(808, 522);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DiseasesForm";
            this.Text = "病种组合新增/编辑";
            this.Load += new System.EventHandler(this.DiseasesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pcTop)).EndInit();
            this.pcTop.ResumeLayout(false);
            this.pcTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_disease)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_disease)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_memo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_diseaseArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_groupName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chb_continue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pcTop;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.TextEdit txt_search;
        private DevExpress.XtraEditors.LabelControl lblDiseaseName;
        private DevExpress.XtraEditors.LabelControl lblSearch;
        private DevExpress.XtraGrid.GridControl gridControl_disease;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_disease;
        private DevExpress.XtraGrid.Columns.GridColumn gcmSelect;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gcmDiseaseCode;
        private DevExpress.XtraGrid.Columns.GridColumn gcmDiseaseName;
        private DevExpress.XtraEditors.CheckEdit chb_continue;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblGroupList;
        private DevExpress.XtraEditors.TextEdit txt_groupName;
        private DevExpress.XtraEditors.LabelControl lblGroupName;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClose btnClose;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnSave;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.MemoEdit txt_diseaseArea;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txt_memo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClear btn_clear;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset btn_reset;

    }
}