namespace DrectSoft.Core.ZymosisReport
{
    partial class PatientListForNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatientListForNew));
            this.checkEditOutHospital = new DevExpress.XtraEditors.CheckEdit();
            this.textEditBedNo = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.textEditPatID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textEditName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.gridControlPatientList = new DevExpress.XtraGrid.GridControl();
            this.gridViewPatientList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemImageXB = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.imageListXB = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.checkEditOutHospital.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditBedNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPatID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlPatientList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPatientList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageXB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkEditOutHospital
            // 
            this.checkEditOutHospital.Location = new System.Drawing.Point(666, 16);
            this.checkEditOutHospital.Name = "checkEditOutHospital";
            this.checkEditOutHospital.Properties.Caption = "已出院";
            this.checkEditOutHospital.Size = new System.Drawing.Size(59, 19);
            this.checkEditOutHospital.TabIndex = 3;
            this.checkEditOutHospital.CheckedChanged += new System.EventHandler(this.checkEditOutHospital_CheckedChanged);
            this.checkEditOutHospital.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chb_KeyPress);
            // 
            // textEditBedNo
            // 
            this.textEditBedNo.Location = new System.Drawing.Point(534, 15);
            this.textEditBedNo.Name = "textEditBedNo";
            this.textEditBedNo.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditBedNo.Size = new System.Drawing.Size(120, 20);
            this.textEditBedNo.TabIndex = 2;
            this.textEditBedNo.TextChanged += new System.EventHandler(this.textEditBedNo_TextChanged);
            this.textEditBedNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEditBedNo_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(484, 18);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "床位号：";
            // 
            // textEditPatID
            // 
            this.textEditPatID.Location = new System.Drawing.Point(344, 15);
            this.textEditPatID.Name = "textEditPatID";
            this.textEditPatID.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditPatID.Size = new System.Drawing.Size(120, 20);
            this.textEditPatID.TabIndex = 1;
            this.textEditPatID.ToolTip = "住院号";
            this.textEditPatID.TextChanged += new System.EventHandler(this.textEditPatID_TextChanged);
            this.textEditPatID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEditPatID_KeyDown);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(294, 18);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "住院号：";
            // 
            // textEditName
            // 
            this.textEditName.Location = new System.Drawing.Point(156, 15);
            this.textEditName.Name = "textEditName";
            this.textEditName.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditName.Size = new System.Drawing.Size(120, 20);
            this.textEditName.TabIndex = 0;
            this.textEditName.ToolTip = "支持汉字、拼音、五笔检索";
            this.textEditName.TextChanged += new System.EventHandler(this.textEditName_TextChanged);
            this.textEditName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEditName_KeyDown);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(94, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "病人姓名：";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.simpleButtonCancel);
            this.panelControl2.Controls.Add(this.simpleButtonOK);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 489);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(739, 39);
            this.panelControl2.TabIndex = 2;
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButtonCancel.Image = global::DrectSoft.Core.ZymosisReport.Properties.Resources.取消;
            this.simpleButtonCancel.Location = new System.Drawing.Point(642, 6);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonCancel.TabIndex = 1;
            this.simpleButtonCancel.Text = "取消 (&C)";
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.simpleButtonOK.Image = global::DrectSoft.Core.ZymosisReport.Properties.Resources.确定;
            this.simpleButtonOK.Location = new System.Drawing.Point(556, 6);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonOK.TabIndex = 0;
            this.simpleButtonOK.Text = "确定 (&Y)";
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButtonOK_Click);
            // 
            // gridControlPatientList
            // 
            this.gridControlPatientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlPatientList.Location = new System.Drawing.Point(0, 50);
            this.gridControlPatientList.MainView = this.gridViewPatientList;
            this.gridControlPatientList.Name = "gridControlPatientList";
            this.gridControlPatientList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemImageXB});
            this.gridControlPatientList.Size = new System.Drawing.Size(739, 439);
            this.gridControlPatientList.TabIndex = 1;
            this.gridControlPatientList.TabStop = false;
            this.gridControlPatientList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewPatientList});
            this.gridControlPatientList.DoubleClick += new System.EventHandler(this.gridControlPatientList_DoubleClick);
            this.gridControlPatientList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControlPatientList_KeyDown);
            // 
            // gridViewPatientList
            // 
            this.gridViewPatientList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn6,
            this.gridColumn7});
            this.gridViewPatientList.GridControl = this.gridControlPatientList;
            this.gridViewPatientList.IndicatorWidth = 40;
            this.gridViewPatientList.Name = "gridViewPatientList";
            this.gridViewPatientList.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridViewPatientList.OptionsBehavior.Editable = false;
            this.gridViewPatientList.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewPatientList.OptionsCustomization.AllowFilter = false;
            this.gridViewPatientList.OptionsMenu.EnableColumnMenu = false;
            this.gridViewPatientList.OptionsMenu.EnableFooterMenu = false;
            this.gridViewPatientList.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewPatientList.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewPatientList.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewPatientList.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewPatientList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewPatientList.OptionsView.ShowGroupPanel = false;
            this.gridViewPatientList.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridViewPatientList_CustomDrawRowIndicator);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "病人姓名";
            this.gridColumn1.FieldName = "NAME";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 100;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "性别";
            this.gridColumn4.ColumnEdit = this.repositoryItemImageXB;
            this.gridColumn4.FieldName = "SEXID";
            this.gridColumn4.MaxWidth = 60;
            this.gridColumn4.MinWidth = 40;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 60;
            // 
            // repositoryItemImageXB
            // 
            this.repositoryItemImageXB.AutoHeight = false;
            this.repositoryItemImageXB.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageXB.Name = "repositoryItemImageXB";
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "年龄";
            this.gridColumn5.FieldName = "AGESTR";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            this.gridColumn5.Width = 80;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "住院号";
            this.gridColumn2.FieldName = "PATID";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 3;
            this.gridColumn2.Width = 100;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "床位号";
            this.gridColumn3.FieldName = "BEDNO";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 4;
            this.gridColumn3.Width = 70;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "诊断";
            this.gridColumn6.FieldName = "DIAGNOSIS";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            this.gridColumn6.Width = 287;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "首页序号";
            this.gridColumn7.FieldName = "NOOFINPAT";
            this.gridColumn7.Name = "gridColumn7";
            // 
            // panelControl3
            // 
            this.panelControl3.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl3.Appearance.Options.UseBackColor = true;
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.labelControl4);
            this.panelControl3.Controls.Add(this.checkEditOutHospital);
            this.panelControl3.Controls.Add(this.textEditName);
            this.panelControl3.Controls.Add(this.labelControl2);
            this.panelControl3.Controls.Add(this.textEditBedNo);
            this.panelControl3.Controls.Add(this.textEditPatID);
            this.panelControl3.Controls.Add(this.labelControl3);
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(739, 50);
            this.panelControl3.TabIndex = 0;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(5, 19);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(75, 14);
            this.labelControl4.TabIndex = 4;
            this.labelControl4.Text = "检索条件>>>";
            // 
            // imageListXB
            // 
            this.imageListXB.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListXB.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListXB.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // PatientListForNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 528);
            this.Controls.Add(this.gridControlPatientList);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PatientListForNew";
            this.Text = "病人列表";
            this.Load += new System.EventHandler(this.PatientListForNew_Load);
            ((System.ComponentModel.ISupportInitialize)(this.checkEditOutHospital.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditBedNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPatID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlPatientList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPatientList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageXB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textEditBedNo;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit textEditPatID;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEditName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
        private DevExpress.XtraEditors.SimpleButton simpleButtonOK;
        private DevExpress.XtraGrid.GridControl gridControlPatientList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewPatientList;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.CheckEdit checkEditOutHospital;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageXB;
        private System.Windows.Forms.ImageList imageListXB;
    }
}