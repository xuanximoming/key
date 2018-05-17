namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCANTIHBC
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
            this.chk_yangxing = new DevExpress.XtraEditors.CheckEdit();
            this.chk_yinxing = new DevExpress.XtraEditors.CheckEdit();
            this.chk_weice = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_yangxing.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_yinxing.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_weice.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(25, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(164, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "抗-HBc IgM 1:1000检测结果：";
            // 
            // chk_yangxing
            // 
            this.chk_yangxing.Location = new System.Drawing.Point(189, 2);
            this.chk_yangxing.Name = "chk_yangxing";
            this.chk_yangxing.Properties.Caption = "阳性";
            this.chk_yangxing.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_yangxing.Properties.RadioGroupIndex = 0;
            this.chk_yangxing.Size = new System.Drawing.Size(48, 19);
            this.chk_yangxing.TabIndex = 1;
            this.chk_yangxing.Tag = "1";
            // 
            // chk_yinxing
            // 
            this.chk_yinxing.Location = new System.Drawing.Point(243, 2);
            this.chk_yinxing.Name = "chk_yinxing";
            this.chk_yinxing.Properties.Caption = "阴性";
            this.chk_yinxing.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_yinxing.Properties.RadioGroupIndex = 0;
            this.chk_yinxing.Size = new System.Drawing.Size(47, 19);
            this.chk_yinxing.TabIndex = 2;
            this.chk_yinxing.Tag = "2";
            // 
            // chk_weice
            // 
            this.chk_weice.Location = new System.Drawing.Point(296, 2);
            this.chk_weice.Name = "chk_weice";
            this.chk_weice.Properties.Caption = "未测";
            this.chk_weice.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_weice.Properties.RadioGroupIndex = 0;
            this.chk_weice.Size = new System.Drawing.Size(46, 19);
            this.chk_weice.TabIndex = 3;
            this.chk_weice.Tag = "3";
            // 
            // UCANTIHBC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_weice);
            this.Controls.Add(this.chk_yinxing);
            this.Controls.Add(this.chk_yangxing);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCANTIHBC";
            this.Size = new System.Drawing.Size(445, 22);
            this.Tag = "ANTIHBC";
            ((System.ComponentModel.ISupportInitialize)(this.chk_yangxing.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_yinxing.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_weice.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chk_yangxing;
        private DevExpress.XtraEditors.CheckEdit chk_yinxing;
        private DevExpress.XtraEditors.CheckEdit chk_weice;
    }
}
