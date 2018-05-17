namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCMaritalStatus
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
            this.chk_weihun = new DevExpress.XtraEditors.CheckEdit();
            this.chk_yihun = new DevExpress.XtraEditors.CheckEdit();
            this.chk_liyi = new DevExpress.XtraEditors.CheckEdit();
            this.chk_buxiang = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_weihun.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_yihun.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_liyi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_buxiang.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl1.Location = new System.Drawing.Point(23, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "婚姻状况：";
            // 
            // chk_weihun
            // 
            this.chk_weihun.Location = new System.Drawing.Point(82, 0);
            this.chk_weihun.Name = "chk_weihun";
            this.chk_weihun.Properties.Caption = "未婚";
            this.chk_weihun.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_weihun.Properties.RadioGroupIndex = 0;
            this.chk_weihun.Size = new System.Drawing.Size(52, 19);
            this.chk_weihun.TabIndex = 1;
            this.chk_weihun.Tag = "1";
            // 
            // chk_yihun
            // 
            this.chk_yihun.Location = new System.Drawing.Point(140, 0);
            this.chk_yihun.Name = "chk_yihun";
            this.chk_yihun.Properties.Caption = "已婚有配偶";
            this.chk_yihun.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_yihun.Properties.RadioGroupIndex = 0;
            this.chk_yihun.Size = new System.Drawing.Size(87, 19);
            this.chk_yihun.TabIndex = 2;
            this.chk_yihun.Tag = "2";
            // 
            // chk_liyi
            // 
            this.chk_liyi.Location = new System.Drawing.Point(233, 0);
            this.chk_liyi.Name = "chk_liyi";
            this.chk_liyi.Properties.Caption = "离异或丧偶";
            this.chk_liyi.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_liyi.Properties.RadioGroupIndex = 0;
            this.chk_liyi.Size = new System.Drawing.Size(87, 19);
            this.chk_liyi.TabIndex = 3;
            this.chk_liyi.Tag = "3";
            // 
            // chk_buxiang
            // 
            this.chk_buxiang.Location = new System.Drawing.Point(326, 0);
            this.chk_buxiang.Name = "chk_buxiang";
            this.chk_buxiang.Properties.Caption = "不详";
            this.chk_buxiang.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_buxiang.Properties.RadioGroupIndex = 0;
            this.chk_buxiang.Size = new System.Drawing.Size(49, 19);
            this.chk_buxiang.TabIndex = 4;
            this.chk_buxiang.Tag = "4";
            // 
            // UCMaritalStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_buxiang);
            this.Controls.Add(this.chk_liyi);
            this.Controls.Add(this.chk_yihun);
            this.Controls.Add(this.chk_weihun);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCMaritalStatus";
            this.Size = new System.Drawing.Size(420, 20);
            ((System.ComponentModel.ISupportInitialize)(this.chk_weihun.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_yihun.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_liyi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_buxiang.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chk_weihun;
        private DevExpress.XtraEditors.CheckEdit chk_yihun;
        private DevExpress.XtraEditors.CheckEdit chk_liyi;
        private DevExpress.XtraEditors.CheckEdit chk_buxiang;
    }
}
