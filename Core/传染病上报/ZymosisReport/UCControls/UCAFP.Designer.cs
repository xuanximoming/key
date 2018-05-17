namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCAFP
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
            this.ucDiagnosisDate1 = new DrectSoft.Core.ZymosisReport.UCControls.UCDiagnosisDate();
            this.ucPalsySymptom1 = new DrectSoft.Core.ZymosisReport.UCControls.UCPalsySymptom();
            this.ucPalsyDate1 = new DrectSoft.Core.ZymosisReport.UCControls.UCPalsyDate();
            this.ucDetailAdresse1 = new DrectSoft.Core.ZymosisReport.UCControls.UCDetailAdresse();
            this.ucHouseholdAddress1 = new DrectSoft.Core.ZymosisReport.UCControls.UCHouseholdAddress();
            this.ucHouseholdscope1 = new DrectSoft.Core.ZymosisReport.UCControls.UCHouseholdscope();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Location = new System.Drawing.Point(49, 148);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(7, 14);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "*";
            // 
            // ucDiagnosisDate1
            // 
            this.ucDiagnosisDate1.Location = new System.Drawing.Point(19, 202);
            this.ucDiagnosisDate1.Name = "ucDiagnosisDate1";
            this.ucDiagnosisDate1.Size = new System.Drawing.Size(257, 26);
            this.ucDiagnosisDate1.TabIndex = 6;
            this.ucDiagnosisDate1.Tag = "DIAGNOSISDATE";
            // 
            // ucPalsySymptom1
            // 
            this.ucPalsySymptom1.Location = new System.Drawing.Point(58, 172);
            this.ucPalsySymptom1.Name = "ucPalsySymptom1";
            this.ucPalsySymptom1.Size = new System.Drawing.Size(452, 27);
            this.ucPalsySymptom1.TabIndex = 4;
            this.ucPalsySymptom1.Tag = "PALSYSYMPTOM";
            // 
            // ucPalsyDate1
            // 
            this.ucPalsyDate1.AccessibleName = "麻痹日期";
            this.ucPalsyDate1.Location = new System.Drawing.Point(58, 141);
            this.ucPalsyDate1.Name = "ucPalsyDate1";
            this.ucPalsyDate1.Size = new System.Drawing.Size(199, 26);
            this.ucPalsyDate1.TabIndex = 3;
            this.ucPalsyDate1.Tag = "PALSYDATE";
            // 
            // ucDetailAdresse1
            // 
            this.ucDetailAdresse1.Location = new System.Drawing.Point(27, 107);
            this.ucDetailAdresse1.Name = "ucDetailAdresse1";
            this.ucDetailAdresse1.Size = new System.Drawing.Size(606, 29);
            this.ucDetailAdresse1.TabIndex = 2;
            this.ucDetailAdresse1.Tag = "ADDRESS";
            // 
            // ucHouseholdAddress1
            // 
            this.ucHouseholdAddress1.Location = new System.Drawing.Point(33, 35);
            this.ucHouseholdAddress1.Name = "ucHouseholdAddress1";
            this.ucHouseholdAddress1.Size = new System.Drawing.Size(416, 75);
            this.ucHouseholdAddress1.StrAddress = null;
            this.ucHouseholdAddress1.TabIndex = 1;
            this.ucHouseholdAddress1.Tag = "HOUSEHOLDADDRESS";
            // 
            // ucHouseholdscope1
            // 
            this.ucHouseholdscope1.AccessibleName = "户籍所在地";
            this.ucHouseholdscope1.Location = new System.Drawing.Point(44, 3);
            this.ucHouseholdscope1.Name = "ucHouseholdscope1";
            this.ucHouseholdscope1.Size = new System.Drawing.Size(589, 27);
            this.ucHouseholdscope1.TabIndex = 0;
            this.ucHouseholdscope1.Tag = "HOUSEHOLDSCOPE";
            // 
            // UCAFP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucDiagnosisDate1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.ucPalsySymptom1);
            this.Controls.Add(this.ucPalsyDate1);
            this.Controls.Add(this.ucDetailAdresse1);
            this.Controls.Add(this.ucHouseholdAddress1);
            this.Controls.Add(this.ucHouseholdscope1);
            this.Name = "UCAFP";
            this.Size = new System.Drawing.Size(694, 244);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCHouseholdscope ucHouseholdscope1;
        private UCHouseholdAddress ucHouseholdAddress1;
        private UCDetailAdresse ucDetailAdresse1;
        private UCPalsyDate ucPalsyDate1;
        private UCPalsySymptom ucPalsySymptom1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private UCDiagnosisDate ucDiagnosisDate1;
    }
}
