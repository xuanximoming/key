namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCHFMD
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
            this.ucSeverePatient1 = new DrectSoft.Core.ZymosisReport.UCControls.UCSeverePatient();
            this.ucLabResult1 = new DrectSoft.Core.ZymosisReport.UCControls.UCLabResult();
            this.SuspendLayout();
            // 
            // ucSeverePatient1
            // 
            this.ucSeverePatient1.Location = new System.Drawing.Point(67, 45);
            this.ucSeverePatient1.Name = "ucSeverePatient1";
            this.ucSeverePatient1.Size = new System.Drawing.Size(191, 22);
            this.ucSeverePatient1.TabIndex = 1;
            this.ucSeverePatient1.Tag = "ISSEVERE";
            // 
            // ucLabResult1
            // 
            this.ucLabResult1.Location = new System.Drawing.Point(57, 14);
            this.ucLabResult1.Name = "ucLabResult1";
            this.ucLabResult1.Size = new System.Drawing.Size(426, 25);
            this.ucLabResult1.TabIndex = 0;
            this.ucLabResult1.Tag = "LABRESULT";
            // 
            // UCHFMD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucSeverePatient1);
            this.Controls.Add(this.ucLabResult1);
            this.Name = "UCHFMD";
            this.Size = new System.Drawing.Size(658, 118);
            this.ResumeLayout(false);

        }

        #endregion

        private UCLabResult ucLabResult1;
        private UCSeverePatient ucSeverePatient1;
    }
}
