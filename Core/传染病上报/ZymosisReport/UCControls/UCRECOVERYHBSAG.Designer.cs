namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCRECOVERYHBSAG
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
            this.chk_shi = new DevExpress.XtraEditors.CheckEdit();
            this.chk_fou = new DevExpress.XtraEditors.CheckEdit();
            this.chk_weice = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_shi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_fou.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_weice.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(191, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "恢复期血清HBsAg阴转,抗HBs阳转：";
            // 
            // chk_shi
            // 
            this.chk_shi.Location = new System.Drawing.Point(200, 2);
            this.chk_shi.Name = "chk_shi";
            this.chk_shi.Properties.Caption = "是";
            this.chk_shi.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_shi.Properties.RadioGroupIndex = 0;
            this.chk_shi.Size = new System.Drawing.Size(37, 19);
            this.chk_shi.TabIndex = 1;
            this.chk_shi.Tag = "1";
            // 
            // chk_fou
            // 
            this.chk_fou.Location = new System.Drawing.Point(243, 2);
            this.chk_fou.Name = "chk_fou";
            this.chk_fou.Properties.Caption = "否";
            this.chk_fou.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_fou.Properties.RadioGroupIndex = 0;
            this.chk_fou.Size = new System.Drawing.Size(37, 19);
            this.chk_fou.TabIndex = 2;
            this.chk_fou.Tag = "2";
            // 
            // chk_weice
            // 
            this.chk_weice.Location = new System.Drawing.Point(286, 2);
            this.chk_weice.Name = "chk_weice";
            this.chk_weice.Properties.Caption = "未测";
            this.chk_weice.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_weice.Properties.RadioGroupIndex = 0;
            this.chk_weice.Size = new System.Drawing.Size(49, 19);
            this.chk_weice.TabIndex = 3;
            this.chk_weice.Tag = "3";
            // 
            // UCRECOVERYHBSAG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_weice);
            this.Controls.Add(this.chk_fou);
            this.Controls.Add(this.chk_shi);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCRECOVERYHBSAG";
            this.Size = new System.Drawing.Size(383, 21);
            this.Tag = "RECOVERYHBSAG";
            ((System.ComponentModel.ISupportInitialize)(this.chk_shi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_fou.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_weice.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chk_shi;
        private DevExpress.XtraEditors.CheckEdit chk_fou;
        private DevExpress.XtraEditors.CheckEdit chk_weice;
    }
}
