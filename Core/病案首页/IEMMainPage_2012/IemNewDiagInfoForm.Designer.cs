namespace DrectSoft.Core.IEMMainPage
{
    partial class IemNewDiagInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IemNewDiagInfoForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel();
            this.btnConfirm = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.chkAdmitInfo3 = new DevExpress.XtraEditors.CheckEdit();
            this.chkAdmitInfo4 = new DevExpress.XtraEditors.CheckEdit();
            this.chkAdmitInfo1 = new DevExpress.XtraEditors.CheckEdit();
            this.chkAdmitInfo2 = new DevExpress.XtraEditors.CheckEdit();
            this.chkStatus4 = new DevExpress.XtraEditors.CheckEdit();
            this.chkStatus3 = new DevExpress.XtraEditors.CheckEdit();
            this.lueOutDiag = new DrectSoft.Common.Library.LookUpEditor();
            this.chkStatus2 = new DevExpress.XtraEditors.CheckEdit();
            this.chkStatus1 = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl59 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl58 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkAdmitInfo3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAdmitInfo4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAdmitInfo1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAdmitInfo2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOutDiag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnConfirm);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.chkAdmitInfo3);
            this.panelControl1.Controls.Add(this.chkAdmitInfo4);
            this.panelControl1.Controls.Add(this.chkAdmitInfo1);
            this.panelControl1.Controls.Add(this.chkAdmitInfo2);
            this.panelControl1.Controls.Add(this.chkStatus4);
            this.panelControl1.Controls.Add(this.chkStatus3);
            this.panelControl1.Controls.Add(this.lueOutDiag);
            this.panelControl1.Controls.Add(this.chkStatus2);
            this.panelControl1.Controls.Add(this.chkStatus1);
            this.panelControl1.Controls.Add(this.labelControl59);
            this.panelControl1.Controls.Add(this.labelControl58);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(432, 141);
            this.panelControl1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(323, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 125;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Image = ((System.Drawing.Image)(resources.GetObject("btnConfirm.Image")));
            this.btnConfirm.Location = new System.Drawing.Point(203, 104);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(87, 27);
            this.btnConfirm.TabIndex = 117;
            this.btnConfirm.Text = "确定(&Y)";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(18, 76);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(82, 14);
            this.labelControl1.TabIndex = 123;
            this.labelControl1.Text = "(子)入院病情：";
            // 
            // chkAdmitInfo3
            // 
            this.chkAdmitInfo3.Location = new System.Drawing.Point(201, 74);
            this.chkAdmitInfo3.Name = "chkAdmitInfo3";
            this.chkAdmitInfo3.Properties.Caption = "3.一般";
            this.chkAdmitInfo3.Properties.RadioGroupIndex = 1;
            this.chkAdmitInfo3.Size = new System.Drawing.Size(59, 19);
            this.chkAdmitInfo3.TabIndex = 121;
            this.chkAdmitInfo3.TabStop = false;
            this.chkAdmitInfo3.Tag = "一般";
            // 
            // chkAdmitInfo4
            // 
            this.chkAdmitInfo4.Location = new System.Drawing.Point(258, 74);
            this.chkAdmitInfo4.Name = "chkAdmitInfo4";
            this.chkAdmitInfo4.Properties.Caption = "4.急";
            this.chkAdmitInfo4.Properties.RadioGroupIndex = 1;
            this.chkAdmitInfo4.Size = new System.Drawing.Size(48, 19);
            this.chkAdmitInfo4.TabIndex = 122;
            this.chkAdmitInfo4.TabStop = false;
            this.chkAdmitInfo4.Tag = "急";
            // 
            // chkAdmitInfo1
            // 
            this.chkAdmitInfo1.Location = new System.Drawing.Point(103, 74);
            this.chkAdmitInfo1.Name = "chkAdmitInfo1";
            this.chkAdmitInfo1.Properties.Caption = "1.危";
            this.chkAdmitInfo1.Properties.RadioGroupIndex = 1;
            this.chkAdmitInfo1.Size = new System.Drawing.Size(52, 19);
            this.chkAdmitInfo1.TabIndex = 119;
            this.chkAdmitInfo1.TabStop = false;
            this.chkAdmitInfo1.Tag = "危";
            // 
            // chkAdmitInfo2
            // 
            this.chkAdmitInfo2.Location = new System.Drawing.Point(153, 74);
            this.chkAdmitInfo2.Name = "chkAdmitInfo2";
            this.chkAdmitInfo2.Properties.Caption = "2.重";
            this.chkAdmitInfo2.Properties.RadioGroupIndex = 1;
            this.chkAdmitInfo2.Size = new System.Drawing.Size(48, 19);
            this.chkAdmitInfo2.TabIndex = 120;
            this.chkAdmitInfo2.TabStop = false;
            this.chkAdmitInfo2.Tag = "重";
            // 
            // chkStatus4
            // 
            this.chkStatus4.Location = new System.Drawing.Point(321, 43);
            this.chkStatus4.Name = "chkStatus4";
            this.chkStatus4.Properties.Caption = "4.无";
            this.chkStatus4.Properties.RadioGroupIndex = 0;
            this.chkStatus4.Size = new System.Drawing.Size(53, 19);
            this.chkStatus4.TabIndex = 3;
            this.chkStatus4.TabStop = false;
            this.chkStatus4.Tag = "无";
            // 
            // chkStatus3
            // 
            this.chkStatus3.Location = new System.Drawing.Point(230, 43);
            this.chkStatus3.Name = "chkStatus3";
            this.chkStatus3.Properties.Caption = "3.情况不明";
            this.chkStatus3.Properties.RadioGroupIndex = 0;
            this.chkStatus3.Size = new System.Drawing.Size(87, 19);
            this.chkStatus3.TabIndex = 2;
            this.chkStatus3.TabStop = false;
            this.chkStatus3.Tag = "情况不明";
            // 
            // lueOutDiag
            // 
            this.lueOutDiag.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lueOutDiag.ListWindow = null;
            this.lueOutDiag.Location = new System.Drawing.Point(75, 12);
            this.lueOutDiag.Name = "lueOutDiag";
            this.lueOutDiag.ShowSButton = true;
            this.lueOutDiag.Size = new System.Drawing.Size(335, 18);
            this.lueOutDiag.TabIndex = 1;
            // 
            // chkStatus2
            // 
            this.chkStatus2.Location = new System.Drawing.Point(130, 43);
            this.chkStatus2.Name = "chkStatus2";
            this.chkStatus2.Properties.Caption = "2.临床未确定";
            this.chkStatus2.Properties.RadioGroupIndex = 0;
            this.chkStatus2.Size = new System.Drawing.Size(96, 19);
            this.chkStatus2.TabIndex = 1;
            this.chkStatus2.TabStop = false;
            this.chkStatus2.Tag = "临床未确定";
            // 
            // chkStatus1
            // 
            this.chkStatus1.Location = new System.Drawing.Point(73, 43);
            this.chkStatus1.Name = "chkStatus1";
            this.chkStatus1.Properties.Caption = "1.有";
            this.chkStatus1.Properties.RadioGroupIndex = 0;
            this.chkStatus1.Size = new System.Drawing.Size(53, 19);
            this.chkStatus1.TabIndex = 0;
            this.chkStatus1.TabStop = false;
            this.chkStatus1.Tag = "有";
            this.chkStatus1.CheckedChanged += new System.EventHandler(this.chkStatus1_CheckedChanged);
            // 
            // labelControl59
            // 
            this.labelControl59.Location = new System.Drawing.Point(10, 15);
            this.labelControl59.Name = "labelControl59";
            this.labelControl59.Size = new System.Drawing.Size(60, 14);
            this.labelControl59.TabIndex = 116;
            this.labelControl59.Text = "出院诊断：";
            // 
            // labelControl58
            // 
            this.labelControl58.Location = new System.Drawing.Point(10, 45);
            this.labelControl58.Name = "labelControl58";
            this.labelControl58.Size = new System.Drawing.Size(60, 14);
            this.labelControl58.TabIndex = 115;
            this.labelControl58.Text = "入院病情：";
            // 
            // IemNewDiagInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 141);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IemNewDiagInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "出院诊断";
            this.Load += new System.EventHandler(this.IemNewDiagInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkAdmitInfo3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAdmitInfo4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAdmitInfo1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAdmitInfo2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOutDiag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DrectSoft.Common.Library.LookUpEditor lueOutDiag;
        private DevExpress.XtraEditors.LabelControl labelControl59;
        private DevExpress.XtraEditors.CheckEdit chkStatus3;
        private DevExpress.XtraEditors.CheckEdit chkStatus2;
        private DevExpress.XtraEditors.CheckEdit chkStatus1;
        private DevExpress.XtraEditors.LabelControl labelControl58;
        private DevExpress.XtraEditors.CheckEdit chkStatus4;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chkAdmitInfo3;
        private DevExpress.XtraEditors.CheckEdit chkAdmitInfo4;
        private DevExpress.XtraEditors.CheckEdit chkAdmitInfo1;
        private DevExpress.XtraEditors.CheckEdit chkAdmitInfo2;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel btnCancel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK btnConfirm;
    }
}