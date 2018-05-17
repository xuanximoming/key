namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCXinBingShi
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
            this.chkYou = new DevExpress.XtraEditors.CheckEdit();
            this.chkW = new DevExpress.XtraEditors.CheckEdit();
            this.chKBuXian = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chkYou.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkW.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chKBuXian.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(9, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "性病史：";
            // 
            // chkYou
            // 
            this.chkYou.Location = new System.Drawing.Point(56, 1);
            this.chkYou.Name = "chkYou";
            this.chkYou.Properties.Caption = "有";
            this.chkYou.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chkYou.Properties.RadioGroupIndex = 0;
            this.chkYou.Size = new System.Drawing.Size(36, 19);
            this.chkYou.TabIndex = 1;
            this.chkYou.Tag = "1";
            // 
            // chkW
            // 
            this.chkW.Location = new System.Drawing.Point(98, 1);
            this.chkW.Name = "chkW";
            this.chkW.Properties.Caption = "无";
            this.chkW.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chkW.Properties.RadioGroupIndex = 0;
            this.chkW.Size = new System.Drawing.Size(37, 19);
            this.chkW.TabIndex = 2;
            this.chkW.Tag = "2";
            // 
            // chKBuXian
            // 
            this.chKBuXian.Location = new System.Drawing.Point(141, 1);
            this.chKBuXian.Name = "chKBuXian";
            this.chKBuXian.Properties.Caption = "不详";
            this.chKBuXian.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chKBuXian.Properties.RadioGroupIndex = 0;
            this.chKBuXian.Size = new System.Drawing.Size(49, 19);
            this.chKBuXian.TabIndex = 3;
            this.chKBuXian.Tag = "3";
            // 
            // UCXinBingShi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chKBuXian);
            this.Controls.Add(this.chkW);
            this.Controls.Add(this.chkYou);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCXinBingShi";
            this.Size = new System.Drawing.Size(237, 23);
            ((System.ComponentModel.ISupportInitialize)(this.chkYou.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkW.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chKBuXian.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chkYou;
        private DevExpress.XtraEditors.CheckEdit chkW;
        private DevExpress.XtraEditors.CheckEdit chKBuXian;
    }
}
