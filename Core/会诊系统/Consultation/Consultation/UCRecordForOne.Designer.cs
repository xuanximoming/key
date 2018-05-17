namespace YidanSoft.Core.Consultation
{
    partial class UCRecordForOne
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
            this.simpleButtonSave = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonComplete = new DevExpress.XtraEditors.SimpleButton();
            this.ucRecordResultForOne = new YidanSoft.Core.Consultation.UCRecordResultForOne();
            this.ucApplyInfoForOne = new YidanSoft.Core.Consultation.UCConsultationInfoForOne();
            this.ucPatientInfoForOne = new YidanSoft.Core.Consultation.UCPatientInfo();
            this.SuspendLayout();
            // 
            // simpleButtonSave
            // 
            this.simpleButtonSave.Enabled = false;
            this.simpleButtonSave.Location = new System.Drawing.Point(265, 259);
            this.simpleButtonSave.Name = "simpleButtonSave";
            this.simpleButtonSave.Size = new System.Drawing.Size(87, 35);
            this.simpleButtonSave.TabIndex = 2;
            this.simpleButtonSave.Text = "保存";
            // 
            // simpleButtonComplete
            // 
            this.simpleButtonComplete.Enabled = false;
            this.simpleButtonComplete.Location = new System.Drawing.Point(444, 259);
            this.simpleButtonComplete.Name = "simpleButtonComplete";
            this.simpleButtonComplete.Size = new System.Drawing.Size(87, 35);
            this.simpleButtonComplete.TabIndex = 3;
            this.simpleButtonComplete.Text = "会诊完成";
            // 
            // ucRecordResultForOne
            // 
            this.ucRecordResultForOne.Location = new System.Drawing.Point(0, 0);
            this.ucRecordResultForOne.Name = "ucRecordResultForOne";
            this.ucRecordResultForOne.Size = new System.Drawing.Size(817, 252);
            this.ucRecordResultForOne.TabIndex = 1;
            // 
            // ucApplyInfoForOne
            // 
            this.ucApplyInfoForOne.Location = new System.Drawing.Point(0, 425);
            this.ucApplyInfoForOne.Name = "ucApplyInfoForOne";
            this.ucApplyInfoForOne.Size = new System.Drawing.Size(817, 499);
            this.ucApplyInfoForOne.TabIndex = 5;
            // 
            // ucPatientInfoForOne
            // 
            this.ucPatientInfoForOne.Location = new System.Drawing.Point(0, 301);
            this.ucPatientInfoForOne.Name = "ucPatientInfoForOne";
            this.ucPatientInfoForOne.Size = new System.Drawing.Size(817, 117);
            this.ucPatientInfoForOne.TabIndex = 4;
            // 
            // UCRecordForOne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.simpleButtonComplete);
            this.Controls.Add(this.ucRecordResultForOne);
            this.Controls.Add(this.simpleButtonSave);
            this.Controls.Add(this.ucApplyInfoForOne);
            this.Controls.Add(this.ucPatientInfoForOne);
            this.Name = "UCRecordForOne";
            this.Size = new System.Drawing.Size(821, 915);
            this.ResumeLayout(false);

        }

        #endregion

        private UCPatientInfo ucPatientInfoForOne;
        private UCConsultationInfoForOne ucApplyInfoForOne;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSave;
        private UCRecordResultForOne ucRecordResultForOne;
        private DevExpress.XtraEditors.SimpleButton simpleButtonComplete;
    }
}
