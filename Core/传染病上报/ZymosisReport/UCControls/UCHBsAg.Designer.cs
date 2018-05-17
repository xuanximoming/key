namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCHBsAg
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
            this.chk_dayu6 = new DevExpress.XtraEditors.CheckEdit();
            this.chk_xiaoyu6 = new DevExpress.XtraEditors.CheckEdit();
            this.chk_buxiang = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_dayu6.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_xiaoyu6.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_buxiang.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(28, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(95, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "HBsAg阳性时间：";
            // 
            // chk_dayu6
            // 
            this.chk_dayu6.Location = new System.Drawing.Point(122, 2);
            this.chk_dayu6.Name = "chk_dayu6";
            this.chk_dayu6.Properties.Caption = ">6个月";
            this.chk_dayu6.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_dayu6.Properties.RadioGroupIndex = 0;
            this.chk_dayu6.Size = new System.Drawing.Size(65, 19);
            this.chk_dayu6.TabIndex = 1;
            this.chk_dayu6.Tag = "1";
            // 
            // chk_xiaoyu6
            // 
            this.chk_xiaoyu6.Location = new System.Drawing.Point(193, 2);
            this.chk_xiaoyu6.Name = "chk_xiaoyu6";
            this.chk_xiaoyu6.Properties.Caption = "6个月内由阴性转为阳性";
            this.chk_xiaoyu6.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_xiaoyu6.Properties.RadioGroupIndex = 0;
            this.chk_xiaoyu6.Size = new System.Drawing.Size(150, 19);
            this.chk_xiaoyu6.TabIndex = 2;
            this.chk_xiaoyu6.Tag = "2";
            // 
            // chk_buxiang
            // 
            this.chk_buxiang.Location = new System.Drawing.Point(349, 2);
            this.chk_buxiang.Name = "chk_buxiang";
            this.chk_buxiang.Properties.Caption = "既往未检测或结果不详";
            this.chk_buxiang.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_buxiang.Properties.RadioGroupIndex = 0;
            this.chk_buxiang.Size = new System.Drawing.Size(144, 19);
            this.chk_buxiang.TabIndex = 3;
            this.chk_buxiang.Tag = "3";
            // 
            // UCHBsAg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_buxiang);
            this.Controls.Add(this.chk_xiaoyu6);
            this.Controls.Add(this.chk_dayu6);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCHBsAg";
            this.Size = new System.Drawing.Size(590, 24);
            this.Tag = "HBSAGDATE";
            ((System.ComponentModel.ISupportInitialize)(this.chk_dayu6.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_xiaoyu6.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_buxiang.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chk_dayu6;
        private DevExpress.XtraEditors.CheckEdit chk_xiaoyu6;
        private DevExpress.XtraEditors.CheckEdit chk_buxiang;
    }
}
