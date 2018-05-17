namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCCaseType
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
            this.chk_qing = new DevExpress.XtraEditors.CheckEdit();
            this.chk_zhong = new DevExpress.XtraEditors.CheckEdit();
            this.chk_wei = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_qing.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_zhong.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_wei.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(7, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(118, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "病例分类(根据病情)：";
            // 
            // chk_qing
            // 
            this.chk_qing.Location = new System.Drawing.Point(124, 3);
            this.chk_qing.Name = "chk_qing";
            this.chk_qing.Properties.Caption = "轻症病例";
            this.chk_qing.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_qing.Properties.RadioGroupIndex = 0;
            this.chk_qing.Size = new System.Drawing.Size(70, 19);
            this.chk_qing.TabIndex = 1;
            this.chk_qing.Tag = "1";
            // 
            // chk_zhong
            // 
            this.chk_zhong.Location = new System.Drawing.Point(200, 3);
            this.chk_zhong.Name = "chk_zhong";
            this.chk_zhong.Properties.Caption = "重症病例";
            this.chk_zhong.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_zhong.Properties.RadioGroupIndex = 0;
            this.chk_zhong.Size = new System.Drawing.Size(69, 19);
            this.chk_zhong.TabIndex = 2;
            this.chk_zhong.Tag = "2";
            // 
            // chk_wei
            // 
            this.chk_wei.Location = new System.Drawing.Point(275, 3);
            this.chk_wei.Name = "chk_wei";
            this.chk_wei.Properties.Caption = "危重病例";
            this.chk_wei.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_wei.Properties.RadioGroupIndex = 0;
            this.chk_wei.Size = new System.Drawing.Size(70, 19);
            this.chk_wei.TabIndex = 3;
            this.chk_wei.Tag = "3";
            // 
            // UCCaseType
            // 
            this.AccessibleName = "病例分类(根据病情)";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_wei);
            this.Controls.Add(this.chk_zhong);
            this.Controls.Add(this.chk_qing);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCCaseType";
            this.Size = new System.Drawing.Size(408, 23);
            this.Tag = "CASETYPE";
            ((System.ComponentModel.ISupportInitialize)(this.chk_qing.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_zhong.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_wei.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chk_qing;
        private DevExpress.XtraEditors.CheckEdit chk_zhong;
        private DevExpress.XtraEditors.CheckEdit chk_wei;
    }
}
