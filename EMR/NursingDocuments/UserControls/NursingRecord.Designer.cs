namespace DrectSoft.Core.NursingDocuments
{
    partial class NursingRecord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NursingRecord));
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtAge = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.linkchangePat = new System.Windows.Forms.LinkLabel();
            this.txtBedID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.ucNursingRecordTable1 = new DrectSoft.Core.NursingDocuments.UserControls.UCNursingRecordTable();
            this.txtInpatName = new DevExpress.XtraEditors.TextEdit();
            this.dateEdit = new DevExpress.XtraEditors.DateEdit();
            this.txtPatID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpWState = new DrectSoft.Common.Library.LookUpWindow();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAge.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBedID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInpatName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = global::DrectSoft.Core.NursingDocuments.Properties.Resources.关闭;
            this.btnCancel.Location = new System.Drawing.Point(738, 539);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 27);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "关闭 (&T)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::DrectSoft.Core.NursingDocuments.Properties.Resources.保存;
            this.btnSave.Location = new System.Drawing.Point(652, 539);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 27);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存 (&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.txtAge);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.linkchangePat);
            this.panelControl1.Controls.Add(this.txtBedID);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.linkLabel1);
            this.panelControl1.Controls.Add(this.ucNursingRecordTable1);
            this.panelControl1.Controls.Add(this.txtInpatName);
            this.panelControl1.Controls.Add(this.dateEdit);
            this.panelControl1.Controls.Add(this.txtPatID);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Location = new System.Drawing.Point(8, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(810, 494);
            this.panelControl1.TabIndex = 0;
            // 
            // txtAge
            // 
            this.txtAge.Enabled = false;
            this.txtAge.EnterMoveNextControl = true;
            this.txtAge.Location = new System.Drawing.Point(302, 48);
            this.txtAge.Name = "txtAge";
            this.txtAge.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.txtAge.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtAge.Properties.Appearance.Options.UseBackColor = true;
            this.txtAge.Properties.Appearance.Options.UseForeColor = true;
            this.txtAge.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txtAge.Size = new System.Drawing.Size(129, 21);
            this.txtAge.TabIndex = 4;
            this.txtAge.ToolTip = "年龄";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(266, 51);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 11;
            this.labelControl5.Text = "年龄：";
            // 
            // linkchangePat
            // 
            this.linkchangePat.AutoSize = true;
            this.linkchangePat.Font = new System.Drawing.Font("Tahoma", 9F);
            this.linkchangePat.LinkColor = System.Drawing.Color.Blue;
            this.linkchangePat.Location = new System.Drawing.Point(729, 51);
            this.linkchangePat.Name = "linkchangePat";
            this.linkchangePat.Size = new System.Drawing.Size(71, 14);
            this.linkchangePat.TabIndex = 6;
            this.linkchangePat.TabStop = true;
            this.linkchangePat.Text = "↔切换病人↔";
            this.linkchangePat.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkchangePat_LinkClicked);
            // 
            // txtBedID
            // 
            this.txtBedID.Enabled = false;
            this.txtBedID.EnterMoveNextControl = true;
            this.txtBedID.Location = new System.Drawing.Point(59, 48);
            this.txtBedID.Name = "txtBedID";
            this.txtBedID.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.txtBedID.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtBedID.Properties.Appearance.Options.UseBackColor = true;
            this.txtBedID.Properties.Appearance.Options.UseForeColor = true;
            this.txtBedID.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txtBedID.Size = new System.Drawing.Size(127, 21);
            this.txtBedID.TabIndex = 3;
            this.txtBedID.ToolTip = "床位号";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(11, 51);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 10;
            this.labelControl4.Text = "床位号：";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(721, 13);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(79, 14);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "病人状态维护";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // ucNursingRecordTable1
            // 
            this.ucNursingRecordTable1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucNursingRecordTable1.CurrentOperTime = new System.DateTime(((long)(0)));
            this.ucNursingRecordTable1.Location = new System.Drawing.Point(11, 75);
            this.ucNursingRecordTable1.Name = "ucNursingRecordTable1";
            this.ucNursingRecordTable1.Size = new System.Drawing.Size(792, 414);
            this.ucNursingRecordTable1.TabIndex = 12;
            // 
            // txtInpatName
            // 
            this.txtInpatName.Enabled = false;
            this.txtInpatName.EnterMoveNextControl = true;
            this.txtInpatName.Location = new System.Drawing.Point(302, 10);
            this.txtInpatName.Name = "txtInpatName";
            this.txtInpatName.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.txtInpatName.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtInpatName.Properties.Appearance.Options.UseBackColor = true;
            this.txtInpatName.Properties.Appearance.Options.UseForeColor = true;
            this.txtInpatName.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txtInpatName.Size = new System.Drawing.Size(129, 21);
            this.txtInpatName.TabIndex = 1;
            this.txtInpatName.ToolTip = "病人姓名";
            // 
            // dateEdit
            // 
            this.dateEdit.EditValue = null;
            this.dateEdit.EnterMoveNextControl = true;
            this.dateEdit.Location = new System.Drawing.Point(547, 10);
            this.dateEdit.Name = "dateEdit";
            this.dateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit.Size = new System.Drawing.Size(129, 21);
            this.dateEdit.TabIndex = 2;
            this.dateEdit.ToolTip = "录入日期";
            this.dateEdit.DateTimeChanged += new System.EventHandler(this.dateEdit_DateTimeChanged);
            // 
            // txtPatID
            // 
            this.txtPatID.Enabled = false;
            this.txtPatID.EnterMoveNextControl = true;
            this.txtPatID.Location = new System.Drawing.Point(59, 10);
            this.txtPatID.Name = "txtPatID";
            this.txtPatID.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.txtPatID.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtPatID.Properties.Appearance.Options.UseBackColor = true;
            this.txtPatID.Properties.Appearance.Options.UseForeColor = true;
            this.txtPatID.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txtPatID.Size = new System.Drawing.Size(127, 21);
            this.txtPatID.TabIndex = 0;
            this.txtPatID.ToolTip = "住院号";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(487, 13);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "录入日期：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(242, 13);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "病人姓名：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(11, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "住院号：";
            // 
            // lookUpWState
            // 
            this.lookUpWState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWState.GenShortCode = null;
            this.lookUpWState.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWState.Owner = null;
            this.lookUpWState.SqlHelper = null;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barButtonItem6,
            this.barButtonItem7,
            this.barButtonItem8,
            this.barButtonItem9});
            this.barManager1.MaxItemId = 9;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem6, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem7, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem8, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem9, true)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.Text = "Tools";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "※/E";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "※";
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "米";
            this.barButtonItem3.Id = 2;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "┃";
            this.barButtonItem4.Id = 3;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "/C";
            this.barButtonItem5.Id = 4;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "Ⅰ";
            this.barButtonItem6.Id = 5;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_ItemClick);
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "Ⅱ";
            this.barButtonItem7.Id = 6;
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_ItemClick);
            // 
            // barButtonItem8
            // 
            this.barButtonItem8.Caption = "Ⅲ";
            this.barButtonItem8.Id = 7;
            this.barButtonItem8.Name = "barButtonItem8";
            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_ItemClick);
            // 
            // barButtonItem9
            // 
            this.barButtonItem9.Caption = "/";
            this.barButtonItem9.Id = 8;
            this.barButtonItem9.Name = "barButtonItem9";
            this.barButtonItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.Appearance.Options.UseTextOptions = true;
            this.barDockControlTop.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.barDockControlTop.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.barDockControlTop.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(827, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 578);
            this.barDockControlBottom.Size = new System.Drawing.Size(827, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 547);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(827, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 547);
            // 
            // barDockControl1
            // 
            this.barDockControl1.CausesValidation = false;
            this.barDockControl1.Location = new System.Drawing.Point(0, 0);
            this.barDockControl1.Size = new System.Drawing.Size(0, 0);
            // 
            // NursingRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 578);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.barDockControl1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NursingRecord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "护理信息录入";
            this.Load += new System.EventHandler(this.NursingRecord_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAge.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBedID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInpatName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtInpatName;
        private DevExpress.XtraEditors.DateEdit dateEdit;
        private DevExpress.XtraEditors.TextEdit txtPatID;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DrectSoft.Core.NursingDocuments.UserControls.UCNursingRecordTable ucNursingRecordTable1;
        private DevExpress.XtraBars.BarDockControl barDockControl1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
        private Common.Library.LookUpWindow lookUpWState;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkchangePat;
        private DevExpress.XtraEditors.TextEdit txtBedID;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtAge;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}