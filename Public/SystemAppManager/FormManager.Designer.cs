namespace DrectSoft.Common.SystemAppManager
{
    partial class FormManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManager));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_Configkey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_Value = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_Descript = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_ParamType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_Hiden = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_Valid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.panelUI = new DevExpress.XtraEditors.PanelControl();
            this.myPanel = new DevExpress.XtraEditors.PanelControl();
            this.txtcanshutype = new DevExpress.XtraEditors.TextEdit();
            this.txtcanshuname = new DevExpress.XtraEditors.TextEdit();
            this.txtcanshu = new DevExpress.XtraEditors.TextEdit();
            this.cmdValid = new System.Windows.Forms.ComboBox();
            this.cmdHide = new System.Windows.Forms.ComboBox();
            this.txtcanshudesp = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelQuery = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbQuery = new System.Windows.Forms.Label();
            this.btnReset = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset(this.components);
            this.btnClear = new DrectSoft.Common.Ctrs.OTHER.DevButtonClear(this.components);
            this.btnQuery = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey(this.components);
            this.txtQuery = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelUI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myPanel)).BeginInit();
            this.myPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtcanshutype.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtcanshuname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtcanshu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtcanshudesp.Properties)).BeginInit();
            this.panelQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuery.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnExit);
            this.panelControl2.Controls.Add(this.btn_Save);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 452);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(890, 40);
            this.panelControl2.TabIndex = 4;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Image = global::DrectSoft.Common.SystemAppManager.Properties.Resources.关闭;
            this.btnExit.Location = new System.Drawing.Point(798, 7);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 27);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "关闭(&T)";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Save.Image = global::DrectSoft.Common.SystemAppManager.Properties.Resources.保存;
            this.btn_Save.Location = new System.Drawing.Point(712, 7);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(80, 27);
            this.btn_Save.TabIndex = 0;
            this.btn_Save.Text = "保存(&S)";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_Configkey,
            this.col_Name,
            this.col_Value,
            this.col_Descript,
            this.col_ParamType,
            this.col_Hiden,
            this.col_Valid});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // col_Configkey
            // 
            this.col_Configkey.AppearanceHeader.Options.UseTextOptions = true;
            this.col_Configkey.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_Configkey.Caption = "参数";
            this.col_Configkey.FieldName = "Configkey";
            this.col_Configkey.MinWidth = 200;
            this.col_Configkey.Name = "col_Configkey";
            this.col_Configkey.Visible = true;
            this.col_Configkey.VisibleIndex = 0;
            this.col_Configkey.Width = 200;
            // 
            // col_Name
            // 
            this.col_Name.AppearanceHeader.Options.UseTextOptions = true;
            this.col_Name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_Name.Caption = "参数名称";
            this.col_Name.FieldName = "Name";
            this.col_Name.MinWidth = 150;
            this.col_Name.Name = "col_Name";
            this.col_Name.Visible = true;
            this.col_Name.VisibleIndex = 1;
            this.col_Name.Width = 150;
            // 
            // col_Value
            // 
            this.col_Value.AppearanceHeader.Options.UseTextOptions = true;
            this.col_Value.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_Value.Caption = "参数值";
            this.col_Value.FieldName = "Value";
            this.col_Value.MinWidth = 120;
            this.col_Value.Name = "col_Value";
            this.col_Value.Visible = true;
            this.col_Value.VisibleIndex = 2;
            this.col_Value.Width = 120;
            // 
            // col_Descript
            // 
            this.col_Descript.AppearanceHeader.Options.UseTextOptions = true;
            this.col_Descript.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_Descript.Caption = "参数描述";
            this.col_Descript.FieldName = "Descript";
            this.col_Descript.Name = "col_Descript";
            this.col_Descript.Visible = true;
            this.col_Descript.VisibleIndex = 3;
            this.col_Descript.Width = 288;
            // 
            // col_ParamType
            // 
            this.col_ParamType.AppearanceHeader.Options.UseTextOptions = true;
            this.col_ParamType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_ParamType.Caption = "参数类型";
            this.col_ParamType.FieldName = "ParamType";
            this.col_ParamType.MaxWidth = 60;
            this.col_ParamType.MinWidth = 60;
            this.col_ParamType.Name = "col_ParamType";
            this.col_ParamType.Visible = true;
            this.col_ParamType.VisibleIndex = 4;
            this.col_ParamType.Width = 60;
            // 
            // col_Hiden
            // 
            this.col_Hiden.AppearanceHeader.Options.UseTextOptions = true;
            this.col_Hiden.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_Hiden.Caption = "隐藏标志";
            this.col_Hiden.FieldName = "Hide";
            this.col_Hiden.MaxWidth = 60;
            this.col_Hiden.MinWidth = 60;
            this.col_Hiden.Name = "col_Hiden";
            this.col_Hiden.Visible = true;
            this.col_Hiden.VisibleIndex = 5;
            this.col_Hiden.Width = 60;
            // 
            // col_Valid
            // 
            this.col_Valid.AppearanceHeader.Options.UseTextOptions = true;
            this.col_Valid.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_Valid.Caption = "是否有效";
            this.col_Valid.FieldName = "Valid";
            this.col_Valid.MaxWidth = 60;
            this.col_Valid.MinWidth = 60;
            this.col_Valid.Name = "col_Valid";
            this.col_Valid.Visible = true;
            this.col_Valid.VisibleIndex = 6;
            this.col_Valid.Width = 60;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 40);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(890, 192);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.TabStop = false;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // panelUI
            // 
            this.panelUI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelUI.Location = new System.Drawing.Point(0, 232);
            this.panelUI.Name = "panelUI";
            this.panelUI.Size = new System.Drawing.Size(890, 150);
            this.panelUI.TabIndex = 2;
            // 
            // myPanel
            // 
            this.myPanel.Controls.Add(this.txtcanshutype);
            this.myPanel.Controls.Add(this.txtcanshuname);
            this.myPanel.Controls.Add(this.txtcanshu);
            this.myPanel.Controls.Add(this.txtcanshudesp);
            this.myPanel.Controls.Add(this.labelControl4);
            this.myPanel.Controls.Add(this.labelControl3);
            this.myPanel.Controls.Add(this.labelControl2);
            this.myPanel.Controls.Add(this.labelControl1);
            this.myPanel.Controls.Add(this.cmdValid);
            this.myPanel.Controls.Add(this.cmdHide);
            this.myPanel.Controls.Add(this.labelControl6);
            this.myPanel.Controls.Add(this.labelControl5);
            this.myPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.myPanel.Location = new System.Drawing.Point(0, 382);
            this.myPanel.Name = "myPanel";
            this.myPanel.Size = new System.Drawing.Size(890, 70);
            this.myPanel.TabIndex = 3;
            this.myPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.myPanel_Paint);
            // 
            // txtcanshutype
            // 
            this.txtcanshutype.Location = new System.Drawing.Point(806, 9);
            this.txtcanshutype.Name = "txtcanshutype";
            this.txtcanshutype.Size = new System.Drawing.Size(60, 21);
            this.txtcanshutype.TabIndex = 2;
            // 
            // txtcanshuname
            // 
            this.txtcanshuname.Location = new System.Drawing.Point(416, 10);
            this.txtcanshuname.Name = "txtcanshuname";
            this.txtcanshuname.Size = new System.Drawing.Size(300, 21);
            this.txtcanshuname.TabIndex = 1;
            // 
            // txtcanshu
            // 
            this.txtcanshu.Location = new System.Drawing.Point(78, 10);
            this.txtcanshu.Name = "txtcanshu";
            this.txtcanshu.Size = new System.Drawing.Size(250, 21);
            this.txtcanshu.TabIndex = 0;
            // 
            // cmdValid
            // 
            this.cmdValid.FormattingEnabled = true;
            this.cmdValid.Items.AddRange(new object[] {
            "0",
            "1"});
            this.cmdValid.Location = new System.Drawing.Point(420, 63);
            this.cmdValid.MaximumSize = new System.Drawing.Size(120, 0);
            this.cmdValid.Name = "cmdValid";
            this.cmdValid.Size = new System.Drawing.Size(80, 20);
            this.cmdValid.TabIndex = 4;
            this.cmdValid.Visible = false;
            // 
            // cmdHide
            // 
            this.cmdHide.FormattingEnabled = true;
            this.cmdHide.Items.AddRange(new object[] {
            "0",
            "1"});
            this.cmdHide.Location = new System.Drawing.Point(618, 64);
            this.cmdHide.Name = "cmdHide";
            this.cmdHide.Size = new System.Drawing.Size(52, 20);
            this.cmdHide.TabIndex = 5;
            this.cmdHide.Visible = false;
            // 
            // txtcanshudesp
            // 
            this.txtcanshudesp.Location = new System.Drawing.Point(78, 39);
            this.txtcanshudesp.Name = "txtcanshudesp";
            this.txtcanshudesp.Properties.ContextMenuStrip = this.contextMenuStrip;
            this.txtcanshudesp.Size = new System.Drawing.Size(788, 21);
            this.txtcanshudesp.TabIndex = 3;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(360, 66);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 14);
            this.labelControl6.TabIndex = 10;
            this.labelControl6.Text = "是否有效：";
            this.labelControl6.Visible = false;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(552, 67);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 14);
            this.labelControl5.TabIndex = 11;
            this.labelControl5.Text = "隐藏标志：";
            this.labelControl5.Visible = false;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(746, 13);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 8;
            this.labelControl4.Text = "参数类型：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(18, 42);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "参数描述：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(356, 13);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "参数名称：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(42, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "参数：";
            // 
            // panelQuery
            // 
            this.panelQuery.Controls.Add(this.label1);
            this.panelQuery.Controls.Add(this.lbQuery);
            this.panelQuery.Controls.Add(this.btnReset);
            this.panelQuery.Controls.Add(this.btnClear);
            this.panelQuery.Controls.Add(this.btnQuery);
            this.panelQuery.Controls.Add(this.txtQuery);
            this.panelQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelQuery.Location = new System.Drawing.Point(0, 0);
            this.panelQuery.Name = "panelQuery";
            this.panelQuery.Size = new System.Drawing.Size(890, 40);
            this.panelQuery.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(114, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "检索：";
            // 
            // lbQuery
            // 
            this.lbQuery.AutoSize = true;
            this.lbQuery.Location = new System.Drawing.Point(15, 15);
            this.lbQuery.Name = "lbQuery";
            this.lbQuery.Size = new System.Drawing.Size(71, 12);
            this.lbQuery.TabIndex = 4;
            this.lbQuery.Text = "筛选条件>>>";
            // 
            // btnReset
            // 
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.Location = new System.Drawing.Point(515, 6);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 27);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "重置(&B)";
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnClear
            // 
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.Location = new System.Drawing.Point(429, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 27);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清空(&L)";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnQuery.Image")));
            this.btnQuery.Location = new System.Drawing.Point(601, 6);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 27);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "查询(&Q)";
            this.btnQuery.Visible = false;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.IsEnterChangeBgColor = false;
            this.txtQuery.IsEnterKeyToNextControl = true;
            this.txtQuery.Location = new System.Drawing.Point(157, 10);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Properties.ContextMenuStrip = this.contextMenuStrip;
            this.txtQuery.Size = new System.Drawing.Size(250, 21);
            toolTipItem1.Text = "支持参数、参数名称、参数值、参数描述检索";
            superToolTip1.Items.Add(toolTipItem1);
            this.txtQuery.SuperTip = superToolTip1;
            this.txtQuery.TabIndex = 0;
            this.txtQuery.EditValueChanged += new System.EventHandler(this.txtQuery_EditValueChanged);
            // 
            // FormManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(890, 492);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelUI);
            this.Controls.Add(this.myPanel);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelQuery);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormManager";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "系统参数配置";
            this.Load += new System.EventHandler(this.FormManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelUI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myPanel)).EndInit();
            this.myPanel.ResumeLayout(false);
            this.myPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtcanshutype.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtcanshuname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtcanshu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtcanshudesp.Properties)).EndInit();
            this.panelQuery.ResumeLayout(false);
            this.panelQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuery.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnExit;
        private DevExpress.XtraEditors.SimpleButton btn_Save;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn col_Configkey;
        private DevExpress.XtraGrid.Columns.GridColumn col_Name;
        private DevExpress.XtraGrid.Columns.GridColumn col_Value;
        private DevExpress.XtraGrid.Columns.GridColumn col_Descript;
        private DevExpress.XtraGrid.Columns.GridColumn col_ParamType;
        private DevExpress.XtraGrid.Columns.GridColumn col_Hiden;
        private DevExpress.XtraGrid.Columns.GridColumn col_Valid;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraEditors.PanelControl panelUI;
        private DevExpress.XtraEditors.PanelControl myPanel;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtcanshudesp;
        private System.Windows.Forms.ComboBox cmdValid;
        private System.Windows.Forms.ComboBox cmdHide;
        private System.Windows.Forms.Panel panelQuery;
        private System.Windows.Forms.Label lbQuery;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset btnReset;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClear btnClear;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey btnQuery;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtQuery;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtcanshutype;
        private DevExpress.XtraEditors.TextEdit txtcanshuname;
        private DevExpress.XtraEditors.TextEdit txtcanshu;
    }
}