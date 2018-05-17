namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCLIVERBIOPSY
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
            this.chk_jixing = new DevExpress.XtraEditors.CheckEdit();
            this.chk_manxing = new DevExpress.XtraEditors.CheckEdit();
            this.chk_weice = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_jixing.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_manxing.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_weice.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(84, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "肝穿检测结果：";
            // 
            // chk_jixing
            // 
            this.chk_jixing.Location = new System.Drawing.Point(93, 0);
            this.chk_jixing.Name = "chk_jixing";
            this.chk_jixing.Properties.Caption = "急性病变";
            this.chk_jixing.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_jixing.Properties.RadioGroupIndex = 0;
            this.chk_jixing.Size = new System.Drawing.Size(71, 19);
            this.chk_jixing.TabIndex = 1;
            this.chk_jixing.Tag = "1";
            // 
            // chk_manxing
            // 
            this.chk_manxing.Location = new System.Drawing.Point(170, 0);
            this.chk_manxing.Name = "chk_manxing";
            this.chk_manxing.Properties.Caption = "慢性病变";
            this.chk_manxing.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_manxing.Properties.RadioGroupIndex = 0;
            this.chk_manxing.Size = new System.Drawing.Size(71, 19);
            this.chk_manxing.TabIndex = 2;
            this.chk_manxing.Tag = "2";
            // 
            // chk_weice
            // 
            this.chk_weice.Location = new System.Drawing.Point(247, 0);
            this.chk_weice.Name = "chk_weice";
            this.chk_weice.Properties.Caption = "未测";
            this.chk_weice.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_weice.Properties.RadioGroupIndex = 0;
            this.chk_weice.Size = new System.Drawing.Size(49, 19);
            this.chk_weice.TabIndex = 3;
            this.chk_weice.Tag = "3";
            // 
            // UCLIVERBIOPSY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_weice);
            this.Controls.Add(this.chk_manxing);
            this.Controls.Add(this.chk_jixing);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCLIVERBIOPSY";
            this.Size = new System.Drawing.Size(394, 22);
            this.Tag = "LIVERBIOPSY";
            ((System.ComponentModel.ISupportInitialize)(this.chk_jixing.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_manxing.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_weice.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chk_jixing;
        private DevExpress.XtraEditors.CheckEdit chk_manxing;
        private DevExpress.XtraEditors.CheckEdit chk_weice;
    }
}
