namespace DrectSoft.SysTableEdit
{
    partial class UCEditComplications
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SORTINDEX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VALID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MEMO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txt_search = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.btn_reset = new DevExpress.XtraEditors.SimpleButton();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_add = new DevExpress.XtraEditors.SimpleButton();
            this.btn_edit = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.cmb_valid = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txt_memo = new DevExpress.XtraEditors.TextEdit();
            this.txt_sortIndex = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txt_py = new DevExpress.XtraEditors.TextEdit();
            this.txt_wb = new DevExpress.XtraEditors.TextEdit();
            this.txt_name = new DevExpress.XtraEditors.TextEdit();
            this.txt_ICDCode = new DevExpress.XtraEditors.TextEdit();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpWindowValid = new DrectSoft.Common.Library.LookUpWindow();
            this.xtraGridBlending1 = new DevExpress.XtraGrid.Blending.XtraGridBlending();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_valid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_memo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_sortIndex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_py.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_wb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ICDCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowValid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 41);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(800, 321);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.TabStop = false;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.Click += new System.EventHandler(this.gridControl1_Click);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.CODE,
            this.NAME,
            this.SORTINDEX,
            this.VALID,
            this.MEMO});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.IndicatorWidth = 40;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
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
            this.gridColumn1.Caption = "序号";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Width = 40;
            // 
            // CODE
            // 
            this.CODE.AppearanceHeader.Options.UseTextOptions = true;
            this.CODE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CODE.Caption = "ICD编码";
            this.CODE.FieldName = "CODE";
            this.CODE.Name = "CODE";
            this.CODE.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.CODE.OptionsFilter.AllowAutoFilter = false;
            this.CODE.OptionsFilter.AllowFilter = false;
            this.CODE.Visible = true;
            this.CODE.VisibleIndex = 0;
            this.CODE.Width = 80;
            // 
            // NAME
            // 
            this.NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.NAME.Caption = "并发症名称";
            this.NAME.FieldName = "NAME";
            this.NAME.Name = "NAME";
            this.NAME.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.NAME.OptionsFilter.AllowAutoFilter = false;
            this.NAME.OptionsFilter.AllowFilter = false;
            this.NAME.Visible = true;
            this.NAME.VisibleIndex = 1;
            this.NAME.Width = 120;
            // 
            // SORTINDEX
            // 
            this.SORTINDEX.AppearanceCell.Options.UseTextOptions = true;
            this.SORTINDEX.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.SORTINDEX.AppearanceHeader.Options.UseTextOptions = true;
            this.SORTINDEX.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.SORTINDEX.Caption = "排序号";
            this.SORTINDEX.FieldName = "SORTINDEX";
            this.SORTINDEX.Name = "SORTINDEX";
            this.SORTINDEX.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.SORTINDEX.OptionsFilter.AllowAutoFilter = false;
            this.SORTINDEX.OptionsFilter.AllowFilter = false;
            this.SORTINDEX.Visible = true;
            this.SORTINDEX.VisibleIndex = 2;
            this.SORTINDEX.Width = 60;
            // 
            // VALID
            // 
            this.VALID.AppearanceCell.Options.UseTextOptions = true;
            this.VALID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.VALID.AppearanceHeader.Options.UseTextOptions = true;
            this.VALID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.VALID.Caption = "是否有效";
            this.VALID.FieldName = "VALID";
            this.VALID.Name = "VALID";
            this.VALID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.VALID.OptionsFilter.AllowAutoFilter = false;
            this.VALID.OptionsFilter.AllowFilter = false;
            this.VALID.Visible = true;
            this.VALID.VisibleIndex = 3;
            this.VALID.Width = 58;
            // 
            // MEMO
            // 
            this.MEMO.AppearanceHeader.Options.UseTextOptions = true;
            this.MEMO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MEMO.Caption = "备注";
            this.MEMO.FieldName = "MEMO";
            this.MEMO.Name = "MEMO";
            this.MEMO.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.MEMO.OptionsFilter.AllowAutoFilter = false;
            this.MEMO.OptionsFilter.AllowFilter = false;
            this.MEMO.Visible = true;
            this.MEMO.VisibleIndex = 4;
            // 
            // txt_search
            // 
            this.txt_search.EnterMoveNextControl = true;
            this.txt_search.Location = new System.Drawing.Point(170, 10);
            this.txt_search.Name = "txt_search";
            this.txt_search.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_search.Size = new System.Drawing.Size(200, 21);
            this.txt_search.TabIndex = 0;
            this.txt_search.ToolTip = "支持ICD编码、并发症名称(汉字/拼音/五笔)、排序号、是否有效、备注检索";
            this.txt_search.EditValueChanged += new System.EventHandler(this.txt_search_EditValueChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // labelControl15
            // 
            this.labelControl15.Location = new System.Drawing.Point(134, 13);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(36, 14);
            this.labelControl15.TabIndex = 2;
            this.labelControl15.Text = "检索：";
            // 
            // btn_reset
            // 
            this.btn_reset.Image = global::DrectSoft.SysTableEdit.Properties.Resources.取消;
            this.btn_reset.Location = new System.Drawing.Point(604, 104);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(80, 23);
            this.btn_reset.TabIndex = 11;
            this.btn_reset.Text = "重置 (&B)";
            this.btn_reset.ToolTip = "重置";
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Image = global::DrectSoft.SysTableEdit.Properties.Resources.取消;
            this.btn_cancel.Location = new System.Drawing.Point(690, 104);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(80, 23);
            this.btn_cancel.TabIndex = 12;
            this.btn_cancel.Text = "取消 (&C)";
            this.btn_cancel.ToolTip = "取消";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Image = global::DrectSoft.SysTableEdit.Properties.Resources.删除;
            this.btn_delete.Location = new System.Drawing.Point(432, 104);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(80, 23);
            this.btn_delete.TabIndex = 9;
            this.btn_delete.Text = "删除 (&E)";
            this.btn_delete.ToolTip = "删除";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_save
            // 
            this.btn_save.Image = global::DrectSoft.SysTableEdit.Properties.Resources.保存;
            this.btn_save.Location = new System.Drawing.Point(518, 104);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(80, 23);
            this.btn_save.TabIndex = 10;
            this.btn_save.Text = "保存 (&S)";
            this.btn_save.ToolTip = "保存";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_add
            // 
            this.btn_add.Image = global::DrectSoft.SysTableEdit.Properties.Resources.新增;
            this.btn_add.Location = new System.Drawing.Point(260, 104);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(80, 23);
            this.btn_add.TabIndex = 7;
            this.btn_add.Text = "新增 (&A)";
            this.btn_add.ToolTip = "新增";
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_edit
            // 
            this.btn_edit.Image = global::DrectSoft.SysTableEdit.Properties.Resources.编辑;
            this.btn_edit.Location = new System.Drawing.Point(346, 104);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(80, 23);
            this.btn_edit.TabIndex = 8;
            this.btn_edit.Text = "编辑 (&E)";
            this.btn_edit.ToolTip = "编辑";
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.btn_reset);
            this.panelControl3.Controls.Add(this.labelControl7);
            this.panelControl3.Controls.Add(this.btn_cancel);
            this.panelControl3.Controls.Add(this.btn_delete);
            this.panelControl3.Controls.Add(this.cmb_valid);
            this.panelControl3.Controls.Add(this.btn_save);
            this.panelControl3.Controls.Add(this.labelControl6);
            this.panelControl3.Controls.Add(this.btn_add);
            this.panelControl3.Controls.Add(this.txt_memo);
            this.panelControl3.Controls.Add(this.btn_edit);
            this.panelControl3.Controls.Add(this.txt_sortIndex);
            this.panelControl3.Controls.Add(this.labelControl3);
            this.panelControl3.Controls.Add(this.labelControl2);
            this.panelControl3.Controls.Add(this.txt_py);
            this.panelControl3.Controls.Add(this.txt_wb);
            this.panelControl3.Controls.Add(this.txt_name);
            this.panelControl3.Controls.Add(this.txt_ICDCode);
            this.panelControl3.Controls.Add(this.labelControl12);
            this.panelControl3.Controls.Add(this.labelControl11);
            this.panelControl3.Controls.Add(this.labelControl8);
            this.panelControl3.Controls.Add(this.labelControl5);
            this.panelControl3.Controls.Add(this.labelControl4);
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 362);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(800, 138);
            this.panelControl3.TabIndex = 1;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl7.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl7.Location = new System.Drawing.Point(774, 49);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(8, 14);
            this.labelControl7.TabIndex = 21;
            this.labelControl7.Text = "*";
            // 
            // cmb_valid
            // 
            this.cmb_valid.EnterMoveNextControl = true;
            this.cmb_valid.Location = new System.Drawing.Point(620, 42);
            this.cmb_valid.Name = "cmb_valid";
            this.cmb_valid.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_valid.Properties.Items.AddRange(new object[] {
            "否",
            "是"});
            this.cmb_valid.Size = new System.Drawing.Size(150, 21);
            this.cmb_valid.TabIndex = 5;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(560, 45);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 14);
            this.labelControl6.TabIndex = 20;
            this.labelControl6.Text = "是否有效：";
            // 
            // txt_memo
            // 
            this.txt_memo.EnterMoveNextControl = true;
            this.txt_memo.Location = new System.Drawing.Point(76, 72);
            this.txt_memo.Name = "txt_memo";
            this.txt_memo.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_memo.Size = new System.Drawing.Size(694, 21);
            this.txt_memo.TabIndex = 6;
            this.txt_memo.ToolTip = "备注";
            // 
            // txt_sortIndex
            // 
            this.txt_sortIndex.EnterMoveNextControl = true;
            this.txt_sortIndex.Location = new System.Drawing.Point(620, 10);
            this.txt_sortIndex.Name = "txt_sortIndex";
            this.txt_sortIndex.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_sortIndex.Size = new System.Drawing.Size(150, 21);
            this.txt_sortIndex.TabIndex = 2;
            this.txt_sortIndex.ToolTip = "排序号";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl3.Location = new System.Drawing.Point(510, 16);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(8, 14);
            this.labelControl3.TabIndex = 16;
            this.labelControl3.Text = "*";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(230, 16);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(8, 14);
            this.labelControl2.TabIndex = 14;
            this.labelControl2.Text = "*";
            // 
            // txt_py
            // 
            this.txt_py.Enabled = false;
            this.txt_py.EnterMoveNextControl = true;
            this.txt_py.Location = new System.Drawing.Point(76, 41);
            this.txt_py.Name = "txt_py";
            this.txt_py.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_py.Size = new System.Drawing.Size(150, 21);
            this.txt_py.TabIndex = 3;
            this.txt_py.ToolTip = "拼音";
            // 
            // txt_wb
            // 
            this.txt_wb.Enabled = false;
            this.txt_wb.EnterMoveNextControl = true;
            this.txt_wb.Location = new System.Drawing.Point(356, 41);
            this.txt_wb.Name = "txt_wb";
            this.txt_wb.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_wb.Size = new System.Drawing.Size(150, 21);
            this.txt_wb.TabIndex = 4;
            this.txt_wb.ToolTip = "五笔";
            // 
            // txt_name
            // 
            this.txt_name.EnterMoveNextControl = true;
            this.txt_name.Location = new System.Drawing.Point(356, 9);
            this.txt_name.Name = "txt_name";
            this.txt_name.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_name.Size = new System.Drawing.Size(150, 21);
            this.txt_name.TabIndex = 1;
            this.txt_name.ToolTip = "并发症名称";
            this.txt_name.TextChanged += new System.EventHandler(this.txt_name_TextChanged);
            // 
            // txt_ICDCode
            // 
            this.txt_ICDCode.EnterMoveNextControl = true;
            this.txt_ICDCode.Location = new System.Drawing.Point(76, 9);
            this.txt_ICDCode.Name = "txt_ICDCode";
            this.txt_ICDCode.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txt_ICDCode.Size = new System.Drawing.Size(150, 21);
            this.txt_ICDCode.TabIndex = 0;
            this.txt_ICDCode.ToolTip = "ICD编码";
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(40, 75);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(36, 14);
            this.labelControl12.TabIndex = 22;
            this.labelControl12.Text = "备注：";
            this.labelControl12.ToolTip = "备注";
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(572, 13);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(48, 14);
            this.labelControl11.TabIndex = 17;
            this.labelControl11.Text = "排序号：";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(320, 45);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(36, 14);
            this.labelControl8.TabIndex = 19;
            this.labelControl8.Text = "五笔：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(40, 45);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 18;
            this.labelControl5.Text = "拼音：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(21, 12);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(55, 14);
            this.labelControl4.TabIndex = 13;
            this.labelControl4.Text = "ICD编码：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(284, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 15;
            this.labelControl1.Text = "并发症名称：";
            // 
            // lookUpWindowValid
            // 
            this.lookUpWindowValid.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowValid.GenShortCode = null;
            this.lookUpWindowValid.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowValid.Owner = null;
            this.lookUpWindowValid.SqlHelper = null;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.txt_search);
            this.panelControl1.Controls.Add(this.labelControl15);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(800, 41);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(17, 13);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(75, 14);
            this.labelControl9.TabIndex = 1;
            this.labelControl9.Text = "检索条件>>>";
            // 
            // UCEditComplications
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl1);
            this.Name = "UCEditComplications";
            this.Size = new System.Drawing.Size(800, 500);
            this.Load += new System.EventHandler(this.UCEditComplications_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_valid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_memo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_sortIndex.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_py.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_wb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ICDCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowValid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_py;
        private DevExpress.XtraEditors.TextEdit txt_wb;
        private DevExpress.XtraEditors.TextEdit txt_name;
        private DevExpress.XtraEditors.TextEdit txt_ICDCode;
        private Common.Library.LookUpWindow lookUpWindowValid;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraEditors.SimpleButton btn_add;
        private DevExpress.XtraEditors.SimpleButton btn_edit;
        private DevExpress.XtraEditors.TextEdit txt_search;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txt_sortIndex;
        private DevExpress.XtraEditors.SimpleButton btn_reset;
        private DevExpress.XtraEditors.TextEdit txt_memo;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_valid;
        private DevExpress.XtraGrid.Blending.XtraGridBlending xtraGridBlending1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn CODE;
        private DevExpress.XtraGrid.Columns.GridColumn NAME;
        private DevExpress.XtraGrid.Columns.GridColumn SORTINDEX;
        private DevExpress.XtraGrid.Columns.GridColumn VALID;
        private DevExpress.XtraGrid.Columns.GridColumn MEMO;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}
