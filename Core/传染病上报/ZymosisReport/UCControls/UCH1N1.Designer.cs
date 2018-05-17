namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCH1N1
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.ucIsOverseas1 = new DrectSoft.Core.ZymosisReport.UCControls.UCIsOverseas();
            this.ucIsCure1 = new DrectSoft.Core.ZymosisReport.UCControls.UCIsCure();
            this.ucHospitalStatus1 = new DrectSoft.Core.ZymosisReport.UCControls.UCHospitalStatus();
            this.ucCaseType1 = new DrectSoft.Core.ZymosisReport.UCControls.UCCaseType();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Location = new System.Drawing.Point(52, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(7, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "*";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(112, 64);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(7, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "*";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl3.Location = new System.Drawing.Point(113, 125);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(7, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "*";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl4.Location = new System.Drawing.Point(86, 179);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(7, 14);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "*";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl5.Location = new System.Drawing.Point(113, 37);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(7, 14);
            this.labelControl5.TabIndex = 8;
            this.labelControl5.Text = "*";
            // 
            // ucIsOverseas1
            // 
            this.ucIsOverseas1.AccessibleName = "是否境外输入";
            this.ucIsOverseas1.Location = new System.Drawing.Point(80, 174);
            this.ucIsOverseas1.Name = "ucIsOverseas1";
            this.ucIsOverseas1.Size = new System.Drawing.Size(182, 18);
            this.ucIsOverseas1.TabIndex = 4;
            this.ucIsOverseas1.Tag = "ISOVERSEAS";
            // 
            // ucIsCure1
            // 
            this.ucIsCure1.AccessibleName = "是否治愈";
            this.ucIsCure1.Location = new System.Drawing.Point(110, 119);
            this.ucIsCure1.Name = "ucIsCure1";
            this.ucIsCure1.Size = new System.Drawing.Size(193, 49);
            this.ucIsCure1.TabIndex = 3;
            this.ucIsCure1.Tag = "ISCURE";
            // 
            // ucHospitalStatus1
            // 
            this.ucHospitalStatus1.AccessibleName = "是否住院";
            this.ucHospitalStatus1.Location = new System.Drawing.Point(110, 31);
            this.ucHospitalStatus1.Name = "ucHospitalStatus1";
            this.ucHospitalStatus1.Size = new System.Drawing.Size(193, 82);
            this.ucHospitalStatus1.TabIndex = 2;
            this.ucHospitalStatus1.Tag = "HOSPITALSTATUS";
            // 
            // ucCaseType1
            // 
            this.ucCaseType1.AccessibleName = "病例分类(根据病情)";
            this.ucCaseType1.Location = new System.Drawing.Point(55, 3);
            this.ucCaseType1.Name = "ucCaseType1";
            this.ucCaseType1.Size = new System.Drawing.Size(359, 23);
            this.ucCaseType1.TabIndex = 1;
            this.ucCaseType1.Tag = "CASETYPE";
            // 
            // UCH1N1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.ucIsOverseas1);
            this.Controls.Add(this.ucIsCure1);
            this.Controls.Add(this.ucHospitalStatus1);
            this.Controls.Add(this.ucCaseType1);
            this.Name = "UCH1N1";
            this.Size = new System.Drawing.Size(650, 228);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCCaseType ucCaseType1;
        private UCHospitalStatus ucHospitalStatus1;
        private UCIsCure ucIsCure1;
        private UCIsOverseas ucIsOverseas1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}
