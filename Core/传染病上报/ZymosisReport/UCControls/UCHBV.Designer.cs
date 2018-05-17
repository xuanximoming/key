namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCHBV
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
            this.ucrecoveryhbsag1 = new DrectSoft.Core.ZymosisReport.UCControls.UCRECOVERYHBSAG();
            this.ucliverbiopsy1 = new DrectSoft.Core.ZymosisReport.UCControls.UCLIVERBIOPSY();
            this.ucantihbc1 = new DrectSoft.Core.ZymosisReport.UCControls.UCANTIHBC();
            this.ucalt1 = new DrectSoft.Core.ZymosisReport.UCControls.UCALT();
            this.ucFirstDate1 = new DrectSoft.Core.ZymosisReport.UCControls.UCFirstDate();
            this.uchBsAg1 = new DrectSoft.Core.ZymosisReport.UCControls.UCHBsAg();
            this.SuspendLayout();
            // 
            // ucrecoveryhbsag1
            // 
            this.ucrecoveryhbsag1.Location = new System.Drawing.Point(19, 149);
            this.ucrecoveryhbsag1.Name = "ucrecoveryhbsag1";
            this.ucrecoveryhbsag1.Size = new System.Drawing.Size(402, 21);
            this.ucrecoveryhbsag1.TabIndex = 5;
            this.ucrecoveryhbsag1.Tag = "RECOVERYHBSAG";
            // 
            // ucliverbiopsy1
            // 
            this.ucliverbiopsy1.Location = new System.Drawing.Point(125, 123);
            this.ucliverbiopsy1.Name = "ucliverbiopsy1";
            this.ucliverbiopsy1.Size = new System.Drawing.Size(359, 22);
            this.ucliverbiopsy1.TabIndex = 4;
            this.ucliverbiopsy1.Tag = "LIVERBIOPSY";
            // 
            // ucantihbc1
            // 
            this.ucantihbc1.Location = new System.Drawing.Point(30, 94);
            this.ucantihbc1.Name = "ucantihbc1";
            this.ucantihbc1.Size = new System.Drawing.Size(402, 22);
            this.ucantihbc1.TabIndex = 3;
            this.ucantihbc1.Tag = "ANTIHBC";
            // 
            // ucalt1
            // 
            this.ucalt1.Location = new System.Drawing.Point(154, 61);
            this.ucalt1.Name = "ucalt1";
            this.ucalt1.Size = new System.Drawing.Size(328, 27);
            this.ucalt1.TabIndex = 2;
            this.ucalt1.Tag = "ALT";
            // 
            // ucFirstDate1
            // 
            this.ucFirstDate1.Location = new System.Drawing.Point(51, 34);
            this.ucFirstDate1.Name = "ucFirstDate1";
            this.ucFirstDate1.Size = new System.Drawing.Size(377, 23);
            this.ucFirstDate1.TabIndex = 1;
            this.ucFirstDate1.Tag = "FRISTDATE";
            // 
            // uchBsAg1
            // 
            this.uchBsAg1.Location = new System.Drawing.Point(96, 8);
            this.uchBsAg1.Name = "uchBsAg1";
            this.uchBsAg1.Size = new System.Drawing.Size(545, 24);
            this.uchBsAg1.TabIndex = 0;
            this.uchBsAg1.Tag = "HBSAGDATE";
            // 
            // UCHBV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucrecoveryhbsag1);
            this.Controls.Add(this.ucliverbiopsy1);
            this.Controls.Add(this.ucantihbc1);
            this.Controls.Add(this.ucalt1);
            this.Controls.Add(this.ucFirstDate1);
            this.Controls.Add(this.uchBsAg1);
            this.Name = "UCHBV";
            this.Size = new System.Drawing.Size(669, 202);
            this.ResumeLayout(false);

        }

        #endregion

        private UCHBsAg uchBsAg1;
        private UCFirstDate ucFirstDate1;
        private UCALT ucalt1;
        private UCANTIHBC ucantihbc1;
        private UCLIVERBIOPSY ucliverbiopsy1;
        private UCRECOVERYHBSAG ucrecoveryhbsag1;
    }
}
