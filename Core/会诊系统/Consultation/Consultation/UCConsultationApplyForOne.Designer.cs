namespace YidanSoft.Core.Consultation
{
    partial class UCConsultationApplyForOne
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
            this.simpleButtonConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonClear = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonExit = new DevExpress.XtraEditors.SimpleButton();
            this.UCApplyInfoForOne = new YidanSoft.Core.Consultation.UCConsultationInfoForOne();
            this.UCPatientInfoForOne = new YidanSoft.Core.Consultation.UCPatientInfo();
            this.SuspendLayout();
            // 
            // simpleButtonConfirm
            // 
            this.simpleButtonConfirm.Location = new System.Drawing.Point(534, 530);
            this.simpleButtonConfirm.Name = "simpleButtonConfirm";
            this.simpleButtonConfirm.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonConfirm.TabIndex = 9;
            this.simpleButtonConfirm.Text = "确定";
            this.simpleButtonConfirm.Click += new System.EventHandler(this.simpleButtonConfirm_Click);
            // 
            // simpleButtonClear
            // 
            this.simpleButtonClear.Location = new System.Drawing.Point(442, 530);
            this.simpleButtonClear.Name = "simpleButtonClear";
            this.simpleButtonClear.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonClear.TabIndex = 8;
            this.simpleButtonClear.Text = "清屏";
            this.simpleButtonClear.Click += new System.EventHandler(this.simpleButtonClear_Click);
            // 
            // simpleButtonExit
            // 
            this.simpleButtonExit.Location = new System.Drawing.Point(624, 530);
            this.simpleButtonExit.Name = "simpleButtonExit";
            this.simpleButtonExit.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonExit.TabIndex = 10;
            this.simpleButtonExit.Text = "关闭";
            this.simpleButtonExit.Click += new System.EventHandler(this.simpleButtonExit_Click);
            // 
            // UCApplyInfoForOne
            // 
            this.UCApplyInfoForOne.Location = new System.Drawing.Point(0, 106);
            this.UCApplyInfoForOne.Name = "UCApplyInfoForOne";
            this.UCApplyInfoForOne.Size = new System.Drawing.Size(700, 418);
            this.UCApplyInfoForOne.TabIndex = 7;
            // 
            // UCPatientInfoForOne
            // 
            this.UCPatientInfoForOne.Location = new System.Drawing.Point(0, 0);
            this.UCPatientInfoForOne.Name = "UCPatientInfoForOne";
            this.UCPatientInfoForOne.Size = new System.Drawing.Size(700, 100);
            this.UCPatientInfoForOne.TabIndex = 6;
            // 
            // UCConsultationApplyForOne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.simpleButtonExit);
            this.Controls.Add(this.simpleButtonConfirm);
            this.Controls.Add(this.simpleButtonClear);
            this.Controls.Add(this.UCApplyInfoForOne);
            this.Controls.Add(this.UCPatientInfoForOne);
            this.Name = "UCConsultationApplyForOne";
            this.Size = new System.Drawing.Size(719, 648);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButtonConfirm;
        private DevExpress.XtraEditors.SimpleButton simpleButtonClear;
        private UCConsultationInfoForOne UCApplyInfoForOne;
        private UCPatientInfo UCPatientInfoForOne;
        private DevExpress.XtraEditors.SimpleButton simpleButtonExit;

    }
}
