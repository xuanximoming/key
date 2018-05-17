namespace DrectSoft.Core.NurseDocument.Controls
{
    partial class PaientStatus
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaientStatus));
            this.gridControlState = new DevExpress.XtraGrid.GridControl();
            this.gViewConfigStatus = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.BtnClear = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.btnSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave(this.components);
            this.btnDel = new DrectSoft.Common.Ctrs.OTHER.DevButtonDelete(this.components);
            this.btnADD = new DrectSoft.Common.Ctrs.OTHER.DevButtonAdd(this.components);
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.lookUpState = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWState = new DrectSoft.Common.Library.LookUpWindow(this.components);
            this.dateEdit = new DevExpress.XtraEditors.DateEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gViewConfigStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlState
            // 
            this.gridControlState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlState.Location = new System.Drawing.Point(2, 2);
            this.gridControlState.MainView = this.gViewConfigStatus;
            this.gridControlState.Name = "gridControlState";
            this.gridControlState.Size = new System.Drawing.Size(412, 260);
            this.gridControlState.TabIndex = 0;
            this.gridControlState.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gViewConfigStatus,
            this.gridView2});
            this.gridControlState.Click += new System.EventHandler(this.gridControlState_Click);
            // 
            // gViewConfigStatus
            // 
            this.gViewConfigStatus.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn3,
            this.gridColumn2,
            this.gridColumn4,
            this.ID,
            this.gridColumn5});
            this.gViewConfigStatus.GridControl = this.gridControlState;
            this.gViewConfigStatus.IndicatorWidth = 42;
            this.gViewConfigStatus.Name = "gViewConfigStatus";
            this.gViewConfigStatus.OptionsBehavior.Editable = false;
            this.gViewConfigStatus.OptionsCustomization.AllowColumnMoving = false;
            this.gViewConfigStatus.OptionsMenu.EnableColumnMenu = false;
            this.gViewConfigStatus.OptionsMenu.EnableFooterMenu = false;
            this.gViewConfigStatus.OptionsMenu.EnableGroupPanelMenu = false;
            this.gViewConfigStatus.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gViewConfigStatus.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gViewConfigStatus.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gViewConfigStatus.OptionsMenu.ShowGroupSummaryEditorItem = true;
            this.gViewConfigStatus.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gViewConfigStatus.OptionsView.ShowGroupPanel = false;
            this.gViewConfigStatus.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gViewConfigStatus_CustomDrawRowIndicator);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "首页序号";
            this.gridColumn1.FieldName = "NOOFINPAT";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn1.OptionsFilter.AllowFilter = false;
            this.gridColumn1.OptionsFilter.ImmediateUpdateAutoFilter = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "患者姓名";
            this.gridColumn3.FieldName = "PATNAME";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.OptionsFilter.ImmediateUpdateAutoFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn2.Caption = "患者状态";
            this.gridColumn2.FieldName = "NAME";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.OptionsFilter.ImmediateUpdateAutoFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "时间";
            this.gridColumn4.FieldName = "DOTIME";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn4.OptionsFilter.AllowFilter = false;
            this.gridColumn4.OptionsFilter.ImmediateUpdateAutoFilter = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            // 
            // ID
            // 
            this.ID.Caption = "ID";
            this.ID.Name = "ID";
            this.ID.OptionsFilter.AllowAutoFilter = false;
            this.ID.OptionsFilter.AllowFilter = false;
            this.ID.OptionsFilter.ImmediateUpdateAutoFilter = false;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "状态编号";
            this.gridColumn5.FieldName = "CCODE";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn5.OptionsFilter.AllowFilter = false;
            this.gridColumn5.OptionsFilter.ImmediateUpdateAutoFilter = false;
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControlState;
            this.gridView2.Name = "gridView2";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl2);
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl3);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(416, 338);
            this.splitContainerControl1.SplitterPosition = 264;
            this.splitContainerControl1.TabIndex = 3;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridControlState);
            this.panelControl1.Controls.Add(this.panelControl5);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(416, 264);
            this.panelControl1.TabIndex = 0;
            // 
            // panelControl5
            // 
            this.panelControl5.Location = new System.Drawing.Point(2, 2);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(401, 256);
            this.panelControl5.TabIndex = 2;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.BtnClear);
            this.panelControl2.Controls.Add(this.btnSave);
            this.panelControl2.Controls.Add(this.btnDel);
            this.panelControl2.Controls.Add(this.btnADD);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 30);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(416, 39);
            this.panelControl2.TabIndex = 38;
            // 
            // BtnClear
            // 
            this.BtnClear.Image = ((System.Drawing.Image)(resources.GetObject("BtnClear.Image")));
            this.BtnClear.Location = new System.Drawing.Point(315, 10);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(80, 23);
            this.BtnClear.TabIndex = 3;
            this.BtnClear.Text = "取消(&C)";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(225, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDel
            // 
            this.btnDel.Image = ((System.Drawing.Image)(resources.GetObject("btnDel.Image")));
            this.btnDel.Location = new System.Drawing.Point(132, 10);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(80, 23);
            this.btnDel.TabIndex = 1;
            this.btnDel.Text = "删除(&D)";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnADD
            // 
            this.btnADD.Image = ((System.Drawing.Image)(resources.GetObject("btnADD.Image")));
            this.btnADD.Location = new System.Drawing.Point(42, 10);
            this.btnADD.Name = "btnADD";
            this.btnADD.Size = new System.Drawing.Size(80, 23);
            this.btnADD.TabIndex = 0;
            this.btnADD.Text = "新增(&A)";
            this.btnADD.Click += new System.EventHandler(this.btnADD_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.lookUpState);
            this.panelControl3.Controls.Add(this.dateEdit);
            this.panelControl3.Controls.Add(this.labelControl5);
            this.panelControl3.Controls.Add(this.txtTime);
            this.panelControl3.Controls.Add(this.labelControl4);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(416, 69);
            this.panelControl3.TabIndex = 1;
            // 
            // lookUpState
            // 
            this.lookUpState.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpState.ListWindow = this.lookUpWState;
            this.lookUpState.Location = new System.Drawing.Point(299, 4);
            this.lookUpState.Name = "lookUpState";
            this.lookUpState.ShowFormImmediately = true;
            this.lookUpState.ShowSButton = true;
            this.lookUpState.Size = new System.Drawing.Size(100, 18);
            this.lookUpState.TabIndex = 1;
            // 
            // lookUpWState
            // 
            this.lookUpWState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWState.GenShortCode = null;
            this.lookUpWState.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWState.Owner = null;
            this.lookUpWState.SqlHelper = null;
            // 
            // dateEdit
            // 
            this.dateEdit.EditValue = null;
            this.dateEdit.Location = new System.Drawing.Point(56, 4);
            this.dateEdit.Name = "dateEdit";
            this.dateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.dateEdit.Size = new System.Drawing.Size(129, 20);
            this.dateEdit.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(268, 7);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(24, 14);
            this.labelControl5.TabIndex = 51;
            this.labelControl5.Text = "状态";
            // 
            // txtTime
            // 
            this.txtTime.EditValue = new System.DateTime(2011, 3, 5, 0, 0, 0, 0);
            this.txtTime.Location = new System.Drawing.Point(194, 5);
            this.txtTime.Name = "txtTime";
            this.txtTime.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtTime.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txtTime.Properties.Mask.EditMask = "HH:mm";
            this.txtTime.Size = new System.Drawing.Size(51, 18);
            this.txtTime.TabIndex = 50;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(20, 7);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(24, 14);
            this.labelControl4.TabIndex = 49;
            this.labelControl4.Text = "时间";
            // 
            // PaientStatus
            // 
            this.AcceptButton = this.btnADD;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 338);
            this.Controls.Add(this.splitContainerControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaientStatus";
            this.Text = "患者状态维护";
            this.Load += new System.EventHandler(this.PaientStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gViewConfigStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlState;
        private DevExpress.XtraGrid.Views.Grid.GridView gViewConfigStatus;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TimeEdit txtTime;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.DateEdit dateEdit;
        private DevExpress.XtraGrid.Columns.GridColumn ID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private Common.Library.LookUpWindow lookUpWState;
        private Common.Library.LookUpEditor lookUpState;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel BtnClear;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnSave;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonDelete btnDel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonAdd btnADD;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}