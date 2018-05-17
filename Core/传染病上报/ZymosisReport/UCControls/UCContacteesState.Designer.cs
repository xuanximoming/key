namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCContacteesState
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chk_you = new DevExpress.XtraEditors.CheckEdit();
            this.chk_wu = new DevExpress.XtraEditors.CheckEdit();
            this.txt_youwu = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.chk_you.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_wu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_youwu.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chk_you
            // 
            this.chk_you.Location = new System.Drawing.Point(268, 1);
            this.chk_you.Name = "chk_you";
            this.chk_you.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.chk_you.Properties.Appearance.Options.UseForeColor = true;
            this.chk_you.Properties.Caption = "1有";
            this.chk_you.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_you.Properties.RadioGroupIndex = 0;
            this.chk_you.Size = new System.Drawing.Size(56, 19);
            this.chk_you.TabIndex = 3;
            this.chk_you.Tag = "1";
            this.chk_you.CheckedChanged += new System.EventHandler(this.chk_wu_CheckedChanged);
            // 
            // chk_wu
            // 
            this.chk_wu.Location = new System.Drawing.Point(216, 1);
            this.chk_wu.Name = "chk_wu";
            this.chk_wu.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.chk_wu.Properties.Appearance.Options.UseForeColor = true;
            this.chk_wu.Properties.Caption = "0无";
            this.chk_wu.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_wu.Properties.RadioGroupIndex = 0;
            this.chk_wu.Size = new System.Drawing.Size(51, 19);
            this.chk_wu.TabIndex = 2;
            this.chk_wu.Tag = "0";
            this.chk_wu.CheckedChanged += new System.EventHandler(this.chk_wu_CheckedChanged);
            // 
            // txt_youwu
            // 
            this.txt_youwu.Location = new System.Drawing.Point(168, 0);
            this.txt_youwu.Name = "txt_youwu";
            this.txt_youwu.Size = new System.Drawing.Size(41, 21);
            this.txt_youwu.TabIndex = 1;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(22, 3);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(144, 14);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "密切接触者有无相同症状：";
            // 
            // UCContacteesState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_you);
            this.Controls.Add(this.chk_wu);
            this.Controls.Add(this.txt_youwu);
            this.Controls.Add(this.labelControl4);
            this.Name = "UCContacteesState";
            this.Size = new System.Drawing.Size(350, 28);
            ((System.ComponentModel.ISupportInitialize)(this.chk_you.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_wu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_youwu.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit chk_you;
        private DevExpress.XtraEditors.CheckEdit chk_wu;
        private DevExpress.XtraEditors.TextEdit txt_youwu;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}
