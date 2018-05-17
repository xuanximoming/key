namespace DrectSoft.Core.Permission
{
    partial class UCPicSign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCPicSign));
            this.lblDepartment = new DevExpress.XtraEditors.LabelControl();
            this.lblUserId = new DevExpress.XtraEditors.LabelControl();
            this.txtUserId = new DevExpress.XtraEditors.TextEdit();
            this.lblUserName = new DevExpress.XtraEditors.LabelControl();
            this.txtUserName = new DevExpress.XtraEditors.TextEdit();
            this.btnSearch = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey(this.components);
            this.btnEdit = new DrectSoft.Common.Ctrs.OTHER.DevButtonEdit(this.components);
            this.pcOperate = new DevExpress.XtraEditors.PanelControl();
            this.gvPicSign = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcmUserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcmUserId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcmDepartment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcmValide = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUserPicFlow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPicSignList = new DevExpress.XtraGrid.GridControl();
            this.lookUpEditorDepartment = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowDepartment = new DrectSoft.Common.Library.LookUpWindow(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcOperate)).BeginInit();
            this.pcOperate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPicSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPicSignList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDepartment
            // 
            this.lblDepartment.Location = new System.Drawing.Point(138, 19);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(36, 14);
            this.lblDepartment.TabIndex = 0;
            this.lblDepartment.Text = "科室：";
            // 
            // lblUserId
            // 
            this.lblUserId.Location = new System.Drawing.Point(366, 19);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(36, 14);
            this.lblUserId.TabIndex = 16;
            this.lblUserId.Text = "工号：";
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(404, 16);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(170, 21);
            this.txtUserId.TabIndex = 2;
            this.txtUserId.ToolTip = "工号";
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new System.Drawing.Point(595, 19);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(36, 14);
            this.lblUserName.TabIndex = 18;
            this.lblUserName.Text = "姓名：";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(631, 16);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(170, 21);
            this.txtUserName.TabIndex = 3;
            this.txtUserName.ToolTip = "姓名";
            // 
            // btnSearch
            // 
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(817, 15);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "查询(&Q)";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.Location = new System.Drawing.Point(30, 7);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 23);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "编辑(&E)";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // pcOperate
            // 
            this.pcOperate.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.pcOperate.Controls.Add(this.btnEdit);
            this.pcOperate.Dock = System.Windows.Forms.DockStyle.Top;
            this.pcOperate.Location = new System.Drawing.Point(0, 50);
            this.pcOperate.Name = "pcOperate";
            this.pcOperate.Size = new System.Drawing.Size(985, 35);
            this.pcOperate.TabIndex = 23;
            // 
            // gvPicSign
            // 
            this.gvPicSign.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcmUserName,
            this.gcmUserId,
            this.gcmDepartment,
            this.gcmValide,
            this.gcUserPicFlow});
            this.gvPicSign.GridControl = this.gcPicSignList;
            this.gvPicSign.IndicatorWidth = 40;
            this.gvPicSign.Name = "gvPicSign";
            this.gvPicSign.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvPicSign.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvPicSign.OptionsBehavior.AllowPartialRedrawOnScrolling = false;
            this.gvPicSign.OptionsBehavior.Editable = false;
            this.gvPicSign.OptionsCustomization.AllowColumnMoving = false;
            this.gvPicSign.OptionsCustomization.AllowFilter = false;
            this.gvPicSign.OptionsCustomization.AllowGroup = false;
            this.gvPicSign.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvPicSign.OptionsDetail.AllowZoomDetail = false;
            this.gvPicSign.OptionsDetail.ShowDetailTabs = false;
            this.gvPicSign.OptionsDetail.SmartDetailExpand = false;
            this.gvPicSign.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gvPicSign.OptionsFilter.AllowFilterEditor = false;
            this.gvPicSign.OptionsFilter.AllowMRUFilterList = false;
            this.gvPicSign.OptionsMenu.EnableColumnMenu = false;
            this.gvPicSign.OptionsMenu.EnableFooterMenu = false;
            this.gvPicSign.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvPicSign.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvPicSign.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvPicSign.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvPicSign.OptionsView.ShowGroupPanel = false;
            this.gvPicSign.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvPicSign_CustomDrawRowIndicator);
            // 
            // gcmUserName
            // 
            this.gcmUserName.Caption = "姓名";
            this.gcmUserName.FieldName = "USERNAME";
            this.gcmUserName.Name = "gcmUserName";
            this.gcmUserName.Visible = true;
            this.gcmUserName.VisibleIndex = 0;
            this.gcmUserName.Width = 170;
            // 
            // gcmUserId
            // 
            this.gcmUserId.Caption = "工号";
            this.gcmUserId.FieldName = "USERID";
            this.gcmUserId.Name = "gcmUserId";
            this.gcmUserId.Visible = true;
            this.gcmUserId.VisibleIndex = 1;
            this.gcmUserId.Width = 170;
            // 
            // gcmDepartment
            // 
            this.gcmDepartment.Caption = "科室";
            this.gcmDepartment.FieldName = "DEPARTMENTNAME";
            this.gcmDepartment.Name = "gcmDepartment";
            this.gcmDepartment.Visible = true;
            this.gcmDepartment.VisibleIndex = 2;
            this.gcmDepartment.Width = 361;
            // 
            // gcmValide
            // 
            this.gcmValide.Caption = "是否存在签名图片";
            this.gcmValide.FieldName = "VALIDE";
            this.gcmValide.Name = "gcmValide";
            this.gcmValide.Visible = true;
            this.gcmValide.VisibleIndex = 3;
            this.gcmValide.Width = 170;
            // 
            // gcUserPicFlow
            // 
            this.gcUserPicFlow.Caption = "编号";
            this.gcUserPicFlow.FieldName = "USERPICFLOW";
            this.gcUserPicFlow.Name = "gcUserPicFlow";
            // 
            // gcPicSignList
            // 
            this.gcPicSignList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPicSignList.Location = new System.Drawing.Point(0, 85);
            this.gcPicSignList.MainView = this.gvPicSign;
            this.gcPicSignList.Name = "gcPicSignList";
            this.gcPicSignList.Size = new System.Drawing.Size(985, 730);
            this.gcPicSignList.TabIndex = 6;
            this.gcPicSignList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPicSign});
            this.gcPicSignList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gcPicSignList_MouseDoubleClick);
            // 
            // lookUpEditorDepartment
            // 
            this.lookUpEditorDepartment.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDepartment.ListWindow = this.lookUpWindowDepartment;
            this.lookUpEditorDepartment.ListWordbookName = "";
            this.lookUpEditorDepartment.Location = new System.Drawing.Point(176, 16);
            this.lookUpEditorDepartment.Name = "lookUpEditorDepartment";
            this.lookUpEditorDepartment.ShowSButton = true;
            this.lookUpEditorDepartment.Size = new System.Drawing.Size(170, 20);
            this.lookUpEditorDepartment.TabIndex = 1;
            this.lookUpEditorDepartment.ToolTipTitle = "科室";
            // 
            // lookUpWindowDepartment
            // 
            this.lookUpWindowDepartment.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(135)));
            this.lookUpWindowDepartment.GenShortCode = null;
            this.lookUpWindowDepartment.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDepartment.Owner = null;
            this.lookUpWindowDepartment.SqlHelper = null;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.txtUserId);
            this.panelControl1.Controls.Add(this.lblDepartment);
            this.panelControl1.Controls.Add(this.lookUpEditorDepartment);
            this.panelControl1.Controls.Add(this.lblUserId);
            this.panelControl1.Controls.Add(this.lblUserName);
            this.panelControl1.Controls.Add(this.btnSearch);
            this.panelControl1.Controls.Add(this.txtUserName);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(985, 50);
            this.panelControl1.TabIndex = 26;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(32, 19);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(75, 14);
            this.labelControl9.TabIndex = 26;
            this.labelControl9.Text = "检索条件>>>";
            // 
            // UCPicSign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcPicSignList);
            this.Controls.Add(this.pcOperate);
            this.Controls.Add(this.panelControl1);
            this.Name = "UCPicSign";
            this.Size = new System.Drawing.Size(985, 815);
            ((System.ComponentModel.ISupportInitialize)(this.txtUserId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcOperate)).EndInit();
            this.pcOperate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvPicSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPicSignList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblDepartment;
        private DevExpress.XtraEditors.LabelControl lblUserId;
        private DevExpress.XtraEditors.TextEdit txtUserId;
        private DevExpress.XtraEditors.LabelControl lblUserName;
        private DevExpress.XtraEditors.TextEdit txtUserName;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey btnSearch;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonEdit btnEdit;
        private DevExpress.XtraEditors.PanelControl pcOperate;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPicSign;
        private DevExpress.XtraGrid.Columns.GridColumn gcmUserName;
        private DevExpress.XtraGrid.Columns.GridColumn gcmUserId;
        private DevExpress.XtraGrid.GridControl gcPicSignList;
        private DevExpress.XtraGrid.Columns.GridColumn gcmDepartment;
        private DevExpress.XtraGrid.Columns.GridColumn gcmValide;
        private Common.Library.LookUpEditor lookUpEditorDepartment;
        private Common.Library.LookUpWindow lookUpWindowDepartment;
        private DevExpress.XtraGrid.Columns.GridColumn gcUserPicFlow;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl9;
    }
}
