namespace YidanSoft.Core.IEMMainPage
{
    partial class IemNewOperInfo
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.teOperDate = new DevExpress.XtraEditors.TimeEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.deOperDate = new DevExpress.XtraEditors.DateEdit();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.lueAnaesthesiaUser = new YidanSoft.Common.Library.LookUpEditor();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lueCloseLevel = new YidanSoft.Common.Library.LookUpEditor();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lueAnaesthesiaType = new YidanSoft.Common.Library.LookUpEditor();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.lueExecute3 = new YidanSoft.Common.Library.LookUpEditor();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lueExecute2 = new YidanSoft.Common.Library.LookUpEditor();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lueExecute1 = new YidanSoft.Common.Library.LookUpEditor();
            this.labelControl46 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl36 = new DevExpress.XtraEditors.LabelControl();
            this.lueOperCode = new YidanSoft.Common.Library.LookUpEditor();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teOperDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deOperDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deOperDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueAnaesthesiaUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueCloseLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueAnaesthesiaType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueExecute3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueExecute2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueExecute1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOperCode)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.teOperDate);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.deOperDate);
            this.panelControl1.Controls.Add(this.btnConfirm);
            this.panelControl1.Controls.Add(this.lueAnaesthesiaUser);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.lueCloseLevel);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.lueAnaesthesiaType);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.lueExecute3);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.lueExecute2);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.lueExecute1);
            this.panelControl1.Controls.Add(this.labelControl46);
            this.panelControl1.Controls.Add(this.labelControl36);
            this.panelControl1.Controls.Add(this.lueOperCode);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(599, 147);
            this.panelControl1.TabIndex = 0;
            // 
            // teOperDate
            // 
            this.teOperDate.EditValue = new System.DateTime(2011, 3, 5, 0, 0, 0, 0);
            this.teOperDate.Location = new System.Drawing.Point(515, 12);
            this.teOperDate.Name = "teOperDate";
            this.teOperDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.teOperDate.Properties.Mask.EditMask = "HH:mm";
            this.teOperDate.Size = new System.Drawing.Size(59, 21);
            this.teOperDate.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(333, 107);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // deOperDate
            // 
            this.deOperDate.EditValue = null;
            this.deOperDate.Location = new System.Drawing.Point(330, 12);
            this.deOperDate.Name = "deOperDate";
            this.deOperDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deOperDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deOperDate.Size = new System.Drawing.Size(186, 21);
            this.deOperDate.TabIndex = 2;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(190, 107);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(87, 27);
            this.btnConfirm.TabIndex = 10;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lueAnaesthesiaUser
            // 
            this.lueAnaesthesiaUser.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lueAnaesthesiaUser.ListWindow = null;
            this.lueAnaesthesiaUser.Location = new System.Drawing.Point(469, 76);
            this.lueAnaesthesiaUser.Name = "lueAnaesthesiaUser";
            this.lueAnaesthesiaUser.ShowSButton = true;
            this.lueAnaesthesiaUser.Size = new System.Drawing.Size(105, 21);
            this.lueAnaesthesiaUser.TabIndex = 9;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(411, 79);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 99;
            this.labelControl4.Text = "麻醉医师";
            // 
            // lueCloseLevel
            // 
            this.lueCloseLevel.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lueCloseLevel.ListWindow = null;
            this.lueCloseLevel.Location = new System.Drawing.Point(283, 76);
            this.lueCloseLevel.Name = "lueCloseLevel";
            this.lueCloseLevel.ShowSButton = true;
            this.lueCloseLevel.Size = new System.Drawing.Size(105, 21);
            this.lueCloseLevel.TabIndex = 8;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(205, 79);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(72, 14);
            this.labelControl5.TabIndex = 97;
            this.labelControl5.Text = "切口愈合等级";
            // 
            // lueAnaesthesiaType
            // 
            this.lueAnaesthesiaType.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lueAnaesthesiaType.ListWindow = null;
            this.lueAnaesthesiaType.Location = new System.Drawing.Point(87, 76);
            this.lueAnaesthesiaType.Name = "lueAnaesthesiaType";
            this.lueAnaesthesiaType.ShowSButton = true;
            this.lueAnaesthesiaType.Size = new System.Drawing.Size(105, 21);
            this.lueAnaesthesiaType.TabIndex = 7;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(21, 79);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 95;
            this.labelControl6.Text = "麻醉方式";
            // 
            // lueExecute3
            // 
            this.lueExecute3.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lueExecute3.ListWindow = null;
            this.lueExecute3.Location = new System.Drawing.Point(453, 44);
            this.lueExecute3.Name = "lueExecute3";
            this.lueExecute3.ShowSButton = true;
            this.lueExecute3.Size = new System.Drawing.Size(121, 21);
            this.lueExecute3.TabIndex = 6;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(411, 47);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(20, 14);
            this.labelControl3.TabIndex = 93;
            this.labelControl3.Text = "II助";
            // 
            // lueExecute2
            // 
            this.lueExecute2.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lueExecute2.ListWindow = null;
            this.lueExecute2.Location = new System.Drawing.Point(267, 44);
            this.lueExecute2.Name = "lueExecute2";
            this.lueExecute2.ShowSButton = true;
            this.lueExecute2.Size = new System.Drawing.Size(121, 21);
            this.lueExecute2.TabIndex = 5;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(242, 47);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(16, 14);
            this.labelControl2.TabIndex = 91;
            this.labelControl2.Text = "I助";
            // 
            // lueExecute1
            // 
            this.lueExecute1.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lueExecute1.ListWindow = null;
            this.lueExecute1.Location = new System.Drawing.Point(87, 44);
            this.lueExecute1.Name = "lueExecute1";
            this.lueExecute1.ShowSButton = true;
            this.lueExecute1.Size = new System.Drawing.Size(121, 21);
            this.lueExecute1.TabIndex = 4;
            // 
            // labelControl46
            // 
            this.labelControl46.Location = new System.Drawing.Point(21, 47);
            this.labelControl46.Name = "labelControl46";
            this.labelControl46.Size = new System.Drawing.Size(24, 14);
            this.labelControl46.TabIndex = 89;
            this.labelControl46.Text = "术者";
            // 
            // labelControl36
            // 
            this.labelControl36.Location = new System.Drawing.Point(252, 16);
            this.labelControl36.Name = "labelControl36";
            this.labelControl36.Size = new System.Drawing.Size(72, 14);
            this.labelControl36.TabIndex = 66;
            this.labelControl36.Text = "手术操作日期";
            // 
            // lueOperCode
            // 
            this.lueOperCode.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lueOperCode.ListWindow = null;
            this.lueOperCode.Location = new System.Drawing.Point(87, 13);
            this.lueOperCode.Name = "lueOperCode";
            this.lueOperCode.ShowSButton = true;
            this.lueOperCode.Size = new System.Drawing.Size(153, 21);
            this.lueOperCode.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(21, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "手术诊断码";
            // 
            // IemNewOperInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 147);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "IemNewOperInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "手术信息";
            this.Load += new System.EventHandler(this.IemNewOperInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teOperDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deOperDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deOperDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueAnaesthesiaUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueCloseLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueAnaesthesiaType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueExecute3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueExecute2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueExecute1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOperCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private YidanSoft.Common.Library.LookUpEditor lueOperCode;
        private DevExpress.XtraEditors.TimeEdit teOperDate;
        private DevExpress.XtraEditors.DateEdit deOperDate;
        private DevExpress.XtraEditors.LabelControl labelControl36;
        private YidanSoft.Common.Library.LookUpEditor lueExecute1;
        private DevExpress.XtraEditors.LabelControl labelControl46;
        private YidanSoft.Common.Library.LookUpEditor lueExecute3;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private YidanSoft.Common.Library.LookUpEditor lueExecute2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private YidanSoft.Common.Library.LookUpEditor lueAnaesthesiaUser;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private YidanSoft.Common.Library.LookUpEditor lueCloseLevel;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private YidanSoft.Common.Library.LookUpEditor lueAnaesthesiaType;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}