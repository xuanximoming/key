namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    partial class UCMacro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMacro));
            this.gridControlMacro = new DevExpress.XtraGrid.GridControl();
            this.gridViewMacro = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnMacroName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnMemo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnColumnName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTableName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnSql = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonAdd = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd();
            this.simpleButtonModify = new DrectSoft.Common.Ctrs.OTHER.DevButtonEdit();
            this.simpleButtonDelete = new DrectSoft.Common.Ctrs.OTHER.DevButtonDelete();
            this.simpleButtonSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave();
            this.simpleButtonCancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel();
            this.memoEditSql = new DevExpress.XtraEditors.MemoEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.textEditTable = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.textEditColumn = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.textEditExample = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.textEditName = new DevExpress.XtraEditors.TextEdit();
            this.btn_reset = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMacro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMacro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSql.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditColumn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditExample.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlMacro
            // 
            this.gridControlMacro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlMacro.Location = new System.Drawing.Point(0, 0);
            this.gridControlMacro.MainView = this.gridViewMacro;
            this.gridControlMacro.Name = "gridControlMacro";
            this.gridControlMacro.Size = new System.Drawing.Size(835, 395);
            this.gridControlMacro.TabIndex = 1;
            this.gridControlMacro.TabStop = false;
            this.gridControlMacro.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMacro});
            // 
            // gridViewMacro
            // 
            this.gridViewMacro.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnMacroName,
            this.gridColumnMemo,
            this.gridColumnColumnName,
            this.gridColumnTableName,
            this.gridColumnSql});
            this.gridViewMacro.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewMacro.GridControl = this.gridControlMacro;
            this.gridViewMacro.IndicatorWidth = 40;
            this.gridViewMacro.Name = "gridViewMacro";
            this.gridViewMacro.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewMacro.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewMacro.OptionsBehavior.Editable = false;
            this.gridViewMacro.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewMacro.OptionsCustomization.AllowFilter = false;
            this.gridViewMacro.OptionsCustomization.AllowGroup = false;
            this.gridViewMacro.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridViewMacro.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewMacro.OptionsFilter.AllowFilterEditor = false;
            this.gridViewMacro.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewMacro.OptionsMenu.EnableColumnMenu = false;
            this.gridViewMacro.OptionsMenu.EnableFooterMenu = false;
            this.gridViewMacro.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewMacro.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewMacro.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewMacro.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewMacro.OptionsView.ShowGroupPanel = false;
            this.gridViewMacro.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gridViewMacro.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridViewMacro_CustomDrawRowIndicator);
            this.gridViewMacro.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewMacro_FocusedRowChanged);
            this.gridViewMacro.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridViewMacro_MouseDown);
            // 
            // gridColumnMacroName
            // 
            this.gridColumnMacroName.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnMacroName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnMacroName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnMacroName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnMacroName.Caption = "名称";
            this.gridColumnMacroName.FieldName = "D_NAME";
            this.gridColumnMacroName.Name = "gridColumnMacroName";
            this.gridColumnMacroName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnMacroName.Visible = true;
            this.gridColumnMacroName.VisibleIndex = 0;
            this.gridColumnMacroName.Width = 120;
            // 
            // gridColumnMemo
            // 
            this.gridColumnMemo.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnMemo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnMemo.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnMemo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnMemo.Caption = "备注";
            this.gridColumnMemo.FieldName = "EXAMPLE";
            this.gridColumnMemo.Name = "gridColumnMemo";
            this.gridColumnMemo.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnMemo.Visible = true;
            this.gridColumnMemo.VisibleIndex = 1;
            this.gridColumnMemo.Width = 120;
            // 
            // gridColumnColumnName
            // 
            this.gridColumnColumnName.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnColumnName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnColumnName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnColumnName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnColumnName.Caption = "列名";
            this.gridColumnColumnName.FieldName = "D_COLUMN";
            this.gridColumnColumnName.Name = "gridColumnColumnName";
            this.gridColumnColumnName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnColumnName.Visible = true;
            this.gridColumnColumnName.VisibleIndex = 2;
            this.gridColumnColumnName.Width = 100;
            // 
            // gridColumnTableName
            // 
            this.gridColumnTableName.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnTableName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnTableName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnTableName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnTableName.Caption = "表名";
            this.gridColumnTableName.FieldName = "D_TABLE";
            this.gridColumnTableName.Name = "gridColumnTableName";
            this.gridColumnTableName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnTableName.Visible = true;
            this.gridColumnTableName.VisibleIndex = 3;
            this.gridColumnTableName.Width = 100;
            // 
            // gridColumnSql
            // 
            this.gridColumnSql.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnSql.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumnSql.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnSql.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnSql.Caption = "语句";
            this.gridColumnSql.FieldName = "D_SQL";
            this.gridColumnSql.Name = "gridColumnSql";
            this.gridColumnSql.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumnSql.Visible = true;
            this.gridColumnSql.VisibleIndex = 4;
            this.gridColumnSql.Width = 353;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.btn_reset);
            this.panelControl4.Controls.Add(this.labelControl1);
            this.panelControl4.Controls.Add(this.simpleButtonAdd);
            this.panelControl4.Controls.Add(this.simpleButtonModify);
            this.panelControl4.Controls.Add(this.simpleButtonDelete);
            this.panelControl4.Controls.Add(this.simpleButtonSave);
            this.panelControl4.Controls.Add(this.simpleButtonCancel);
            this.panelControl4.Controls.Add(this.memoEditSql);
            this.panelControl4.Controls.Add(this.labelControl9);
            this.panelControl4.Controls.Add(this.labelControl8);
            this.panelControl4.Controls.Add(this.textEditTable);
            this.panelControl4.Controls.Add(this.labelControl7);
            this.panelControl4.Controls.Add(this.textEditColumn);
            this.panelControl4.Controls.Add(this.labelControl6);
            this.panelControl4.Controls.Add(this.textEditExample);
            this.panelControl4.Controls.Add(this.labelControl5);
            this.panelControl4.Controls.Add(this.textEditName);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl4.Location = new System.Drawing.Point(0, 395);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(835, 147);
            this.panelControl4.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Location = new System.Drawing.Point(191, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(8, 14);
            this.labelControl1.TabIndex = 12;
            this.labelControl1.Text = "*";
            // 
            // simpleButtonAdd
            // 
            this.simpleButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonAdd.Image")));
            this.simpleButtonAdd.Location = new System.Drawing.Point(302, 111);
            this.simpleButtonAdd.Name = "simpleButtonAdd";
            this.simpleButtonAdd.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonAdd.TabIndex = 5;
            this.simpleButtonAdd.Text = "新增(&A)";
            this.simpleButtonAdd.Click += new System.EventHandler(this.simpleButtonNew_Click);
            // 
            // simpleButtonModify
            // 
            this.simpleButtonModify.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonModify.Image")));
            this.simpleButtonModify.Location = new System.Drawing.Point(388, 111);
            this.simpleButtonModify.Name = "simpleButtonModify";
            this.simpleButtonModify.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonModify.TabIndex = 6;
            this.simpleButtonModify.Text = "编辑(&E)";
            this.simpleButtonModify.Click += new System.EventHandler(this.simpleButtonModify_Click);
            // 
            // simpleButtonDelete
            // 
            this.simpleButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonDelete.Image")));
            this.simpleButtonDelete.Location = new System.Drawing.Point(475, 111);
            this.simpleButtonDelete.Name = "simpleButtonDelete";
            this.simpleButtonDelete.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonDelete.TabIndex = 7;
            this.simpleButtonDelete.Text = "删除(&D)";
            this.simpleButtonDelete.Click += new System.EventHandler(this.simpleButtonDelete_Click);
            // 
            // simpleButtonSave
            // 
            this.simpleButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonSave.Image")));
            this.simpleButtonSave.Location = new System.Drawing.Point(561, 111);
            this.simpleButtonSave.Name = "simpleButtonSave";
            this.simpleButtonSave.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonSave.TabIndex = 8;
            this.simpleButtonSave.Text = "保存(&S)";
            this.simpleButtonSave.Click += new System.EventHandler(this.simpleButtonSave_Click);
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonCancel.Image")));
            this.simpleButtonCancel.Location = new System.Drawing.Point(733, 111);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonCancel.TabIndex = 10;
            this.simpleButtonCancel.Text = "取消(&C)";
            this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
            // 
            // memoEditSql
            // 
            this.memoEditSql.Enabled = false;
            this.memoEditSql.EnterMoveNextControl = true;
            this.memoEditSql.Location = new System.Drawing.Point(57, 45);
            this.memoEditSql.Name = "memoEditSql";
            this.memoEditSql.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.memoEditSql.Size = new System.Drawing.Size(756, 56);
            this.memoEditSql.TabIndex = 4;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(23, 70);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(34, 14);
            this.labelControl9.TabIndex = 16;
            this.labelControl9.Text = "SQL：";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(647, 15);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(36, 14);
            this.labelControl8.TabIndex = 15;
            this.labelControl8.Text = "表名：";
            // 
            // textEditTable
            // 
            this.textEditTable.Enabled = false;
            this.textEditTable.EnterMoveNextControl = true;
            this.textEditTable.Location = new System.Drawing.Point(683, 12);
            this.textEditTable.Name = "textEditTable";
            this.textEditTable.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditTable.Size = new System.Drawing.Size(130, 21);
            this.textEditTable.TabIndex = 3;
            this.textEditTable.ToolTip = "表名";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(438, 15);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(36, 14);
            this.labelControl7.TabIndex = 14;
            this.labelControl7.Text = "列名：";
            // 
            // textEditColumn
            // 
            this.textEditColumn.Enabled = false;
            this.textEditColumn.EnterMoveNextControl = true;
            this.textEditColumn.Location = new System.Drawing.Point(474, 12);
            this.textEditColumn.Name = "textEditColumn";
            this.textEditColumn.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditColumn.Size = new System.Drawing.Size(130, 21);
            this.textEditColumn.TabIndex = 2;
            this.textEditColumn.ToolTip = "列名";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(231, 15);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 14);
            this.labelControl6.TabIndex = 13;
            this.labelControl6.Text = "备注：";
            // 
            // textEditExample
            // 
            this.textEditExample.Enabled = false;
            this.textEditExample.EnterMoveNextControl = true;
            this.textEditExample.Location = new System.Drawing.Point(267, 12);
            this.textEditExample.Name = "textEditExample";
            this.textEditExample.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditExample.Size = new System.Drawing.Size(130, 21);
            this.textEditExample.TabIndex = 1;
            this.textEditExample.ToolTip = "备注";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(21, 15);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 11;
            this.labelControl5.Text = "名称：";
            // 
            // textEditName
            // 
            this.textEditName.Enabled = false;
            this.textEditName.EnterMoveNextControl = true;
            this.textEditName.Location = new System.Drawing.Point(57, 12);
            this.textEditName.Name = "textEditName";
            this.textEditName.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditName.Size = new System.Drawing.Size(130, 21);
            this.textEditName.TabIndex = 0;
            this.textEditName.ToolTip = "名称";
            // 
            // btn_reset
            // 
            this.btn_reset.Image = ((System.Drawing.Image)(resources.GetObject("btn_reset.Image")));
            this.btn_reset.Location = new System.Drawing.Point(647, 111);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(80, 27);
            this.btn_reset.TabIndex = 9;
            this.btn_reset.Text = "重置(&B)";
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // UCMacro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlMacro);
            this.Controls.Add(this.panelControl4);
            this.Name = "UCMacro";
            this.Size = new System.Drawing.Size(835, 542);
            this.Load += new System.EventHandler(this.UCMacro_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMacro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMacro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSql.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditColumn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditExample.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlMacro;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewMacro;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnMacroName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnMemo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnColumnName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTableName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnSql;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.MemoEdit memoEditSql;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit textEditTable;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.TextEdit textEditColumn;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit textEditExample;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit textEditName;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd simpleButtonAdd;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonEdit simpleButtonModify;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonDelete simpleButtonDelete;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave simpleButtonSave;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel simpleButtonCancel;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset btn_reset;
    }
}
