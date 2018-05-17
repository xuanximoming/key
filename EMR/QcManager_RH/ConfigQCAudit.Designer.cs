namespace DrectSoft.Emr.QcManager
{
    partial class ConfigQCAudit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigQCAudit));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEZhuRen = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWZhuren = new DrectSoft.Common.Library.LookUpWindow();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEQCPerson = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWQCPerson = new DrectSoft.Common.Library.LookUpWindow();
            this.simpleButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditorDepartment = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWDept = new DrectSoft.Common.Library.LookUpWindow();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.grdViewConfigAudit = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonSearch = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditorDepartmentSearch = new DrectSoft.Common.Library.LookUpEditor();
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEZhuRen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWZhuren)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEQCPerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWQCPerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewConfigAudit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartmentSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.lookUpEZhuRen);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.lookUpEQCPerson);
            this.groupControl1.Controls.Add(this.simpleButtonOK);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.lookUpEditorDepartment);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(608, 67);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "人员设置";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(327, 34);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 17;
            this.labelControl2.Text = "科室主任：";
            // 
            // lookUpEZhuRen
            // 
            this.lookUpEZhuRen.Enabled = false;
            this.lookUpEZhuRen.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEZhuRen.ListWindow = this.lookUpWZhuren;
            this.lookUpEZhuRen.Location = new System.Drawing.Point(392, 32);
            this.lookUpEZhuRen.Name = "lookUpEZhuRen";
            this.lookUpEZhuRen.ShowSButton = true;
            this.lookUpEZhuRen.Size = new System.Drawing.Size(80, 18);
            this.lookUpEZhuRen.TabIndex = 16;
            // 
            // lookUpWZhuren
            // 
            this.lookUpWZhuren.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWZhuren.GenShortCode = null;
            this.lookUpWZhuren.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWZhuren.Owner = null;
            this.lookUpWZhuren.SqlHelper = null;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(156, 34);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 14);
            this.labelControl3.TabIndex = 15;
            this.labelControl3.Text = "科室质控员：";
            // 
            // lookUpEQCPerson
            // 
            this.lookUpEQCPerson.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEQCPerson.ListWindow = this.lookUpWQCPerson;
            this.lookUpEQCPerson.Location = new System.Drawing.Point(229, 32);
            this.lookUpEQCPerson.Name = "lookUpEQCPerson";
            this.lookUpEQCPerson.ShowSButton = true;
            this.lookUpEQCPerson.Size = new System.Drawing.Size(80, 18);
            this.lookUpEQCPerson.TabIndex = 14;
            // 
            // lookUpWQCPerson
            // 
            this.lookUpWQCPerson.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWQCPerson.GenShortCode = null;
            this.lookUpWQCPerson.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWQCPerson.Owner = null;
            this.lookUpWQCPerson.SqlHelper = null;
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Location = new System.Drawing.Point(492, 32);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(64, 20);
            this.simpleButtonOK.TabIndex = 8;
            this.simpleButtonOK.Text = "确定";
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButtonOK_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(25, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "科室：";
            // 
            // lookUpEditorDepartment
            // 
            this.lookUpEditorDepartment.Cursor = System.Windows.Forms.Cursors.Default;
            this.lookUpEditorDepartment.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDepartment.ListWindow = this.lookUpWDept;
            this.lookUpEditorDepartment.Location = new System.Drawing.Point(65, 32);
            this.lookUpEditorDepartment.Name = "lookUpEditorDepartment";
            this.lookUpEditorDepartment.ShowSButton = true;
            this.lookUpEditorDepartment.Size = new System.Drawing.Size(80, 18);
            this.lookUpEditorDepartment.TabIndex = 0;
            // 
            // lookUpWDept
            // 
            this.lookUpWDept.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWDept.GenShortCode = null;
            this.lookUpWDept.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWDept.Owner = null;
            this.lookUpWDept.SqlHelper = null;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.gridControl1);
            this.groupControl2.Controls.Add(this.panelControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 67);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(608, 297);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "结果查询";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(2, 59);
            this.gridControl1.MainView = this.grdViewConfigAudit;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(604, 236);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewConfigAudit});
            // 
            // grdViewConfigAudit
            // 
            this.grdViewConfigAudit.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn11,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn1});
            this.grdViewConfigAudit.GridControl = this.gridControl1;
            this.grdViewConfigAudit.Name = "grdViewConfigAudit";
            this.grdViewConfigAudit.OptionsCustomization.AllowFilter = false;
            this.grdViewConfigAudit.OptionsFilter.AllowColumnMRUFilterList = false;
            this.grdViewConfigAudit.OptionsFilter.AllowFilterEditor = false;
            this.grdViewConfigAudit.OptionsFilter.AllowMRUFilterList = false;
            this.grdViewConfigAudit.OptionsMenu.EnableColumnMenu = false;
            this.grdViewConfigAudit.OptionsMenu.EnableFooterMenu = false;
            this.grdViewConfigAudit.OptionsMenu.EnableGroupPanelMenu = false;
            this.grdViewConfigAudit.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grdViewConfigAudit.OptionsView.ShowGroupPanel = false;
            this.grdViewConfigAudit.OptionsView.ShowIndicator = false;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "编号";
            this.gridColumn11.FieldName = "ID";
            this.gridColumn11.Name = "gridColumn11";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "科室";
            this.gridColumn2.FieldName = "DEPTNAME";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "科室质控员";
            this.gridColumn3.FieldName = "QCMANAGERNAME";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "科室主任";
            this.gridColumn4.FieldName = "CHIEFNAME";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "是否有效";
            this.gridColumn1.FieldName = "VALIDNAME";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButtonSearch);
            this.panelControl1.Controls.Add(this.labelControl8);
            this.panelControl1.Controls.Add(this.lookUpEditorDepartmentSearch);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(2, 22);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(604, 37);
            this.panelControl1.TabIndex = 0;
            // 
            // simpleButtonSearch
            // 
            this.simpleButtonSearch.Location = new System.Drawing.Point(162, 10);
            this.simpleButtonSearch.Name = "simpleButtonSearch";
            this.simpleButtonSearch.Size = new System.Drawing.Size(64, 20);
            this.simpleButtonSearch.TabIndex = 16;
            this.simpleButtonSearch.Text = "查询";
            this.simpleButtonSearch.Click += new System.EventHandler(this.simpleButtonSearch_Click);
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(23, 12);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(36, 14);
            this.labelControl8.TabIndex = 10;
            this.labelControl8.Text = "科室：";
            // 
            // lookUpEditorDepartmentSearch
            // 
            this.lookUpEditorDepartmentSearch.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDepartmentSearch.ListWindow = this.lookUpWDept;
            this.lookUpEditorDepartmentSearch.Location = new System.Drawing.Point(63, 10);
            this.lookUpEditorDepartmentSearch.Name = "lookUpEditorDepartmentSearch";
            this.lookUpEditorDepartmentSearch.ShowFormImmediately = true;
            this.lookUpEditorDepartmentSearch.ShowSButton = true;
            this.lookUpEditorDepartmentSearch.Size = new System.Drawing.Size(80, 18);
            this.lookUpEditorDepartmentSearch.TabIndex = 8;
            // 
            // barDockControl1
            // 
            this.barDockControl1.CausesValidation = false;
            this.barDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl1.Location = new System.Drawing.Point(0, 0);
            this.barDockControl1.Size = new System.Drawing.Size(608, 0);
            // 
            // barDockControl2
            // 
            this.barDockControl2.CausesValidation = false;
            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControl2.Location = new System.Drawing.Point(0, 364);
            this.barDockControl2.Size = new System.Drawing.Size(608, 0);
            // 
            // barDockControl3
            // 
            this.barDockControl3.CausesValidation = false;
            this.barDockControl3.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControl3.Location = new System.Drawing.Point(0, 0);
            this.barDockControl3.Size = new System.Drawing.Size(0, 364);
            // 
            // barDockControl4
            // 
            this.barDockControl4.CausesValidation = false;
            this.barDockControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl4.Location = new System.Drawing.Point(608, 0);
            this.barDockControl4.Size = new System.Drawing.Size(0, 364);
            // 
            // popupMenu1
            // 
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.MaxItemId = 0;
            this.barManager1.StatusBar = this.bar1;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            this.bar1.Visible = false;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 1;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar2.FloatLocation = new System.Drawing.Point(549, 552);
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            this.bar2.Visible = false;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 2;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            this.bar3.Visible = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(608, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 364);
            this.barDockControlBottom.Size = new System.Drawing.Size(608, 81);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 364);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(608, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 364);
            // 
            // ConfigQCAudit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 445);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.barDockControl1);
            this.Controls.Add(this.barDockControl2);
            this.Controls.Add(this.barDockControl3);
            this.Controls.Add(this.barDockControl4);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigQCAudit";
            this.Text = "质控人员配置";
            this.Load += new System.EventHandler(this.ConfigQCAudit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEZhuRen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWZhuren)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEQCPerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWQCPerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewConfigAudit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartmentSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonOK;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DrectSoft.Common.Library.LookUpEditor lookUpEditorDepartment;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSearch;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewConfigAudit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        public DrectSoft.Common.Library.LookUpEditor lookUpEditorDepartmentSearch;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DrectSoft.Common.Library.LookUpEditor lookUpEZhuRen;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DrectSoft.Common.Library.LookUpEditor lookUpEQCPerson;
        private DrectSoft.Common.Library.LookUpWindow lookUpWDept;
        private DrectSoft.Common.Library.LookUpWindow lookUpWQCPerson;
        private DrectSoft.Common.Library.LookUpWindow lookUpWZhuren;
        //private DevExpress.XtraBars.PopupMenu popupMenu1;
        //private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControl1;
        private DevExpress.XtraBars.BarDockControl barDockControl2;
        private DevExpress.XtraBars.BarDockControl barDockControl3;
        private DevExpress.XtraBars.BarDockControl barDockControl4;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;

    }
}