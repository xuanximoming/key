namespace DrectSoft.Core.NursingDocuments
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaientStatus));
            this.lookUpWState = new DrectSoft.Common.Library.LookUpWindow();
            this.gridControlState = new DevExpress.XtraGrid.GridControl();
            this.gViewConfigStatus = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnDel = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btn_close = new DevExpress.XtraEditors.SimpleButton();
            this.BtnClear = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnADD = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dateEdit = new DevExpress.XtraEditors.DateEdit();
            this.lookUpState = new DrectSoft.Common.Library.LookUpEditor();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gViewConfigStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lookUpWState
            // 
            this.lookUpWState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWState.GenShortCode = null;
            this.lookUpWState.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWState.Owner = null;
            this.lookUpWState.SqlHelper = null;
            // 
            // gridControlState
            // 
            this.gridControlState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlState.Location = new System.Drawing.Point(2, 2);
            this.gridControlState.MainView = this.gViewConfigStatus;
            this.gridControlState.Name = "gridControlState";
            this.gridControlState.Size = new System.Drawing.Size(442, 257);
            this.gridControlState.TabIndex = 0;
            this.gridControlState.TabStop = false;
            this.gridControlState.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gViewConfigStatus,
            this.gridView2});
            this.gridControlState.Click += new System.EventHandler(this.gridControlState_Click);
            // 
            // gViewConfigStatus
            // 
            this.gViewConfigStatus.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.gViewConfigStatus.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gViewConfigStatus.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn3,
            this.gridColumn2,
            this.gridColumn4,
            this.ID,
            this.gridColumn5});
            this.gViewConfigStatus.GridControl = this.gridControlState;
            this.gViewConfigStatus.IndicatorWidth = 40;
            this.gViewConfigStatus.Name = "gViewConfigStatus";
            this.gViewConfigStatus.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gViewConfigStatus.OptionsBehavior.Editable = false;
            this.gViewConfigStatus.OptionsCustomization.AllowColumnMoving = false;
            this.gViewConfigStatus.OptionsCustomization.AllowFilter = false;
            this.gViewConfigStatus.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gViewConfigStatus.OptionsFilter.AllowMRUFilterList = false;
            this.gViewConfigStatus.OptionsMenu.EnableColumnMenu = false;
            this.gViewConfigStatus.OptionsMenu.EnableFooterMenu = false;
            this.gViewConfigStatus.OptionsMenu.EnableGroupPanelMenu = false;
            this.gViewConfigStatus.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gViewConfigStatus.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gViewConfigStatus.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gViewConfigStatus.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gViewConfigStatus.OptionsView.ShowGroupPanel = false;
            this.gViewConfigStatus.OptionsView.ShowViewCaption = true;
            this.gViewConfigStatus.ViewCaption = " 病人状态列表";
            this.gViewConfigStatus.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gViewConfigStatus_CustomDrawRowIndicator);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "首页序号";
            this.gridColumn1.FieldName = "NOOFINPAT";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "病人姓名";
            this.gridColumn3.FieldName = "PATNAME";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 80;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "病人状态";
            this.gridColumn2.FieldName = "NAME";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 100;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "时间";
            this.gridColumn4.FieldName = "DOTIME";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 190;
            // 
            // ID
            // 
            this.ID.AppearanceCell.Options.UseTextOptions = true;
            this.ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.ID.AppearanceHeader.Options.UseTextOptions = true;
            this.ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ID.Caption = "ID";
            this.ID.Name = "ID";
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "状态编号";
            this.gridColumn5.FieldName = "CCODE";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControlState;
            this.gridView2.Name = "gridView2";
            // 
            // btnDel
            // 
            this.btnDel.Image = global::DrectSoft.Core.NursingDocuments.Properties.Resources.删除;
            this.btnDel.Location = new System.Drawing.Point(95, 5);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(80, 27);
            this.btnDel.TabIndex = 1;
            this.btnDel.Text = "删除 (&D)";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btn_close);
            this.panelControl2.Controls.Add(this.BtnClear);
            this.panelControl2.Controls.Add(this.btnDel);
            this.panelControl2.Controls.Add(this.btnSave);
            this.panelControl2.Controls.Add(this.btnADD);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(2, 36);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(442, 37);
            this.panelControl2.TabIndex = 0;
            // 
            // btn_close
            // 
            this.btn_close.Image = global::DrectSoft.Core.NursingDocuments.Properties.Resources.关闭;
            this.btn_close.Location = new System.Drawing.Point(354, 5);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(80, 27);
            this.btn_close.TabIndex = 4;
            this.btn_close.Text = "关闭 (&T)";
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Image = global::DrectSoft.Core.NursingDocuments.Properties.Resources.取消;
            this.BtnClear.Location = new System.Drawing.Point(268, 5);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(80, 27);
            this.BtnClear.TabIndex = 3;
            this.BtnClear.Text = "取消 (&C)";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::DrectSoft.Core.NursingDocuments.Properties.Resources.保存;
            this.btnSave.Location = new System.Drawing.Point(182, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 27);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "保存 (&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnADD
            // 
            this.btnADD.Image = global::DrectSoft.Core.NursingDocuments.Properties.Resources.新增;
            this.btnADD.Location = new System.Drawing.Point(9, 5);
            this.btnADD.Name = "btnADD";
            this.btnADD.Size = new System.Drawing.Size(80, 27);
            this.btnADD.TabIndex = 0;
            this.btnADD.Text = "新增 (&A)";
            this.btnADD.ToolTip = "新增";
            this.btnADD.Click += new System.EventHandler(this.btnADD_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.labelControl2);
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Controls.Add(this.dateEdit);
            this.panelControl3.Controls.Add(this.lookUpState);
            this.panelControl3.Controls.Add(this.labelControl5);
            this.panelControl3.Controls.Add(this.txtTime);
            this.panelControl3.Controls.Add(this.labelControl4);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(2, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(442, 71);
            this.panelControl3.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(230, 14);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(8, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "*";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Location = new System.Drawing.Point(419, 14);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(8, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "*";
            // 
            // dateEdit
            // 
            this.dateEdit.EditValue = null;
            this.dateEdit.Location = new System.Drawing.Point(51, 7);
            this.dateEdit.Name = "dateEdit";
            this.dateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit.Size = new System.Drawing.Size(120, 21);
            this.dateEdit.TabIndex = 0;
            this.dateEdit.ToolTip = "日期";
            // 
            // lookUpState
            // 
            this.lookUpState.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpState.ListWindow = this.lookUpWState;
            this.lookUpState.Location = new System.Drawing.Point(296, 7);
            this.lookUpState.Name = "lookUpState";
            this.lookUpState.ShowFormImmediately = true;
            this.lookUpState.ShowSButton = true;
            this.lookUpState.Size = new System.Drawing.Size(120, 20);
            this.lookUpState.TabIndex = 2;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(260, 10);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 5;
            this.labelControl5.Text = "状态：";
            // 
            // txtTime
            // 
            this.txtTime.EditValue = new System.DateTime(2011, 3, 5, 0, 0, 0, 0);
            this.txtTime.Location = new System.Drawing.Point(171, 8);
            this.txtTime.Name = "txtTime";
            this.txtTime.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtTime.Properties.Mask.EditMask = "HH:mm";
            this.txtTime.Size = new System.Drawing.Size(55, 19);
            this.txtTime.TabIndex = 1;
            this.txtTime.ToolTip = "时间";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(15, 10);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "时间：";
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.panelControl2);
            this.panelControl4.Controls.Add(this.panelControl3);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl4.Location = new System.Drawing.Point(0, 261);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(446, 75);
            this.panelControl4.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridControlState);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(446, 261);
            this.panelControl1.TabIndex = 1;
            // 
            // PaientStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 336);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaientStatus";
            this.ShowIcon = false;
            this.Text = "病人状态维护";
            this.Load += new System.EventHandler(this.PaientStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gViewConfigStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Library.LookUpWindow lookUpWState;
        private DevExpress.XtraGrid.GridControl gridControlState;
        private DevExpress.XtraGrid.Views.Grid.GridView gViewConfigStatus;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.SimpleButton btnDel;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton BtnClear;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnADD;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private Common.Library.LookUpEditor lookUpState;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TimeEdit txtTime;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.DateEdit dateEdit;
        private DevExpress.XtraGrid.Columns.GridColumn ID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_close;
    }
}