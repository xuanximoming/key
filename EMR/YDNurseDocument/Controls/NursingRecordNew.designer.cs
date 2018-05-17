namespace DrectSoft.Core.NurseDocument.Controls
{
    partial class NursingRecordNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NursingRecordNew));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnChangePatient = new DevExpress.XtraEditors.SimpleButton();
            this.btnPatientState = new DevExpress.XtraEditors.SimpleButton();
            this.txtBedID = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtInpatName = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.dateEdit = new DevExpress.XtraEditors.DateEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtPatID = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
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
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnClose = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose(this.components);
            this.btnSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave(this.components);
            this.ucNursingRecordTableNew1 = new DrectSoft.Core.NurseDocument.Controls.UCNursingRecordTableNew();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBedID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInpatName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btnChangePatient);
            this.panelControl1.Controls.Add(this.btnPatientState);
            this.panelControl1.Controls.Add(this.txtBedID);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.txtInpatName);
            this.panelControl1.Controls.Add(this.dateEdit);
            this.panelControl1.Controls.Add(this.txtPatID);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 31);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(827, 44);
            this.panelControl1.TabIndex = 10;
            // 
            // btnChangePatient
            // 
            this.btnChangePatient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangePatient.Location = new System.Drawing.Point(781, 10);
            this.btnChangePatient.Name = "btnChangePatient";
            this.btnChangePatient.Size = new System.Drawing.Size(87, 27);
            this.btnChangePatient.TabIndex = 5;
            this.btnChangePatient.Text = "切换病人";
            this.btnChangePatient.Visible = false;
            this.btnChangePatient.Click += new System.EventHandler(this.ChagePatientClicked);
            // 
            // btnPatientState
            // 
            this.btnPatientState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPatientState.Location = new System.Drawing.Point(678, 3);
            this.btnPatientState.Name = "btnPatientState";
            this.btnPatientState.Size = new System.Drawing.Size(87, 27);
            this.btnPatientState.TabIndex = 4;
            this.btnPatientState.Text = "患者状态维护";
            this.btnPatientState.Click += new System.EventHandler(this.PatientStateClicked);
            // 
            // txtBedID
            // 
            this.txtBedID.IsEnterChangeBgColor = false;
            this.txtBedID.IsEnterKeyToNextControl = false;
            this.txtBedID.IsNumber = false;
            this.txtBedID.Location = new System.Drawing.Point(567, 9);
            this.txtBedID.Name = "txtBedID";
            this.txtBedID.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.txtBedID.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtBedID.Properties.Appearance.Options.UseBackColor = true;
            this.txtBedID.Properties.Appearance.Options.UseForeColor = true;
            this.txtBedID.Properties.ReadOnly = true;
            this.txtBedID.Size = new System.Drawing.Size(76, 20);
            this.txtBedID.TabIndex = 3;
            this.txtBedID.TabStop = false;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(533, 10);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 9;
            this.labelControl4.Text = "床号：";
            // 
            // txtInpatName
            // 
            this.txtInpatName.IsEnterChangeBgColor = false;
            this.txtInpatName.IsEnterKeyToNextControl = false;
            this.txtInpatName.IsNumber = false;
            this.txtInpatName.Location = new System.Drawing.Point(427, 9);
            this.txtInpatName.Name = "txtInpatName";
            this.txtInpatName.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.txtInpatName.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtInpatName.Properties.Appearance.Options.UseBackColor = true;
            this.txtInpatName.Properties.Appearance.Options.UseForeColor = true;
            this.txtInpatName.Properties.ReadOnly = true;
            this.txtInpatName.Size = new System.Drawing.Size(76, 20);
            this.txtInpatName.TabIndex = 1;
            this.txtInpatName.TabStop = false;
            // 
            // dateEdit
            // 
            this.dateEdit.EditValue = null;
            this.dateEdit.Location = new System.Drawing.Point(58, 9);
            this.dateEdit.Name = "dateEdit";
            this.dateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.dateEdit.Size = new System.Drawing.Size(120, 20);
            this.dateEdit.TabIndex = 2;
            this.dateEdit.DateTimeChanged += new System.EventHandler(this.dateEdit_DateTimeChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // txtPatID
            // 
            this.txtPatID.IsEnterChangeBgColor = false;
            this.txtPatID.IsEnterKeyToNextControl = false;
            this.txtPatID.IsNumber = false;
            this.txtPatID.Location = new System.Drawing.Point(236, 9);
            this.txtPatID.Name = "txtPatID";
            this.txtPatID.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.txtPatID.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtPatID.Properties.Appearance.Options.UseBackColor = true;
            this.txtPatID.Properties.Appearance.Options.UseForeColor = true;
            this.txtPatID.Properties.ReadOnly = true;
            this.txtPatID.Size = new System.Drawing.Size(119, 20);
            this.txtPatID.TabIndex = 0;
            this.txtPatID.TabStop = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(21, 9);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 14);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "日期：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(369, 10);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "病人姓名：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(186, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "住院号：";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(827, 44);
            this.label1.TabIndex = 10;
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowShowToolbarsPopup = false;
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
            this.barManager1.MaxItemId = 10;
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
            this.barButtonItem1.Caption = "E";
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
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 546);
            this.barDockControlBottom.Size = new System.Drawing.Size(827, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 515);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(827, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 515);
            // 
            // barDockControl1
            // 
            this.barDockControl1.CausesValidation = false;
            this.barDockControl1.Location = new System.Drawing.Point(0, 0);
            this.barDockControl1.Size = new System.Drawing.Size(0, 0);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnClose);
            this.panelControl2.Controls.Add(this.btnSave);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 505);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(827, 41);
            this.panelControl2.TabIndex = 23;
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(663, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭(&T)";
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(577, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ucNursingRecordTableNew1
            // 
            this.ucNursingRecordTableNew1.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.ucNursingRecordTableNew1.Appearance.Options.UseBackColor = true;
            this.ucNursingRecordTableNew1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNursingRecordTableNew1.Location = new System.Drawing.Point(0, 75);
            this.ucNursingRecordTableNew1.Name = "ucNursingRecordTableNew1";
            this.ucNursingRecordTableNew1.Size = new System.Drawing.Size(827, 430);
            this.ucNursingRecordTableNew1.TabIndex = 24;
            // 
            // NursingRecordNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 546);
            this.Controls.Add(this.ucNursingRecordTableNew1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NursingRecordNew";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "体征录入";
            this.Load += new System.EventHandler(this.NursingRecord_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBedID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInpatName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dateEdit;
        private DevExpress.XtraEditors.LabelControl labelControl3;
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
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnPatientState;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtInpatName;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtPatID;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtBedID;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraEditors.SimpleButton btnChangePatient;
        private UCNursingRecordTableNew ucNursingRecordTableNew1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClose btnClose;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnSave;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}