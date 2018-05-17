namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCHouseholdscope
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
            this.lc_hujidi = new DevExpress.XtraEditors.LabelControl();
            this.txt_hujidi = new DevExpress.XtraEditors.TextEdit();
            this.chk_benshiqita = new DevExpress.XtraEditors.CheckEdit();
            this.chk_benshengqita = new DevExpress.XtraEditors.CheckEdit();
            this.chk_gangao = new DevExpress.XtraEditors.CheckEdit();
            this.chk_qitasheng = new DevExpress.XtraEditors.CheckEdit();
            this.chk_waiji = new DevExpress.XtraEditors.CheckEdit();
            this.chk_benxianqu = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_hujidi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_benshiqita.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_benshengqita.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_gangao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_qitasheng.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_waiji.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_benxianqu.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lc_hujidi
            // 
            this.lc_hujidi.Location = new System.Drawing.Point(9, 5);
            this.lc_hujidi.Name = "lc_hujidi";
            this.lc_hujidi.Size = new System.Drawing.Size(72, 14);
            this.lc_hujidi.TabIndex = 0;
            this.lc_hujidi.Text = "户籍所在地：";
            // 
            // txt_hujidi
            // 
            this.txt_hujidi.Location = new System.Drawing.Point(83, 3);
            this.txt_hujidi.Name = "txt_hujidi";
            this.txt_hujidi.Size = new System.Drawing.Size(46, 21);
            this.txt_hujidi.TabIndex = 1;
            // 
            // chk_benshiqita
            // 
            this.chk_benshiqita.Location = new System.Drawing.Point(194, 3);
            this.chk_benshiqita.Name = "chk_benshiqita";
            this.chk_benshiqita.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.chk_benshiqita.Properties.Appearance.Options.UseForeColor = true;
            this.chk_benshiqita.Properties.Caption = "2本市其他县区";
            this.chk_benshiqita.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_benshiqita.Properties.RadioGroupIndex = 0;
            this.chk_benshiqita.Size = new System.Drawing.Size(102, 19);
            this.chk_benshiqita.TabIndex = 3;
            this.chk_benshiqita.Tag = "2";
            this.chk_benshiqita.CheckedChanged += new System.EventHandler(this.chk_benxianqu_CheckedChanged);
            // 
            // chk_benshengqita
            // 
            this.chk_benshengqita.Location = new System.Drawing.Point(296, 3);
            this.chk_benshengqita.Name = "chk_benshengqita";
            this.chk_benshengqita.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.chk_benshengqita.Properties.Appearance.Options.UseForeColor = true;
            this.chk_benshengqita.Properties.Caption = "3本省其它城市";
            this.chk_benshengqita.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_benshengqita.Properties.RadioGroupIndex = 0;
            this.chk_benshengqita.Size = new System.Drawing.Size(101, 19);
            this.chk_benshengqita.TabIndex = 4;
            this.chk_benshengqita.Tag = "3";
            this.chk_benshengqita.CheckedChanged += new System.EventHandler(this.chk_benxianqu_CheckedChanged);
            // 
            // chk_gangao
            // 
            this.chk_gangao.Location = new System.Drawing.Point(466, 3);
            this.chk_gangao.Name = "chk_gangao";
            this.chk_gangao.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.chk_gangao.Properties.Appearance.Options.UseForeColor = true;
            this.chk_gangao.Properties.Caption = "5港澳台";
            this.chk_gangao.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_gangao.Properties.RadioGroupIndex = 0;
            this.chk_gangao.Size = new System.Drawing.Size(67, 19);
            this.chk_gangao.TabIndex = 6;
            this.chk_gangao.Tag = "5";
            this.chk_gangao.CheckedChanged += new System.EventHandler(this.chk_benxianqu_CheckedChanged);
            // 
            // chk_qitasheng
            // 
            this.chk_qitasheng.Location = new System.Drawing.Point(400, 3);
            this.chk_qitasheng.Name = "chk_qitasheng";
            this.chk_qitasheng.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.chk_qitasheng.Properties.Appearance.Options.UseForeColor = true;
            this.chk_qitasheng.Properties.Caption = "4其它省";
            this.chk_qitasheng.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_qitasheng.Properties.RadioGroupIndex = 0;
            this.chk_qitasheng.Size = new System.Drawing.Size(66, 19);
            this.chk_qitasheng.TabIndex = 5;
            this.chk_qitasheng.Tag = "4";
            this.chk_qitasheng.CheckedChanged += new System.EventHandler(this.chk_benxianqu_CheckedChanged);
            // 
            // chk_waiji
            // 
            this.chk_waiji.Location = new System.Drawing.Point(532, 3);
            this.chk_waiji.Name = "chk_waiji";
            this.chk_waiji.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.chk_waiji.Properties.Appearance.Options.UseForeColor = true;
            this.chk_waiji.Properties.Caption = "6外籍";
            this.chk_waiji.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_waiji.Properties.RadioGroupIndex = 0;
            this.chk_waiji.Size = new System.Drawing.Size(56, 19);
            this.chk_waiji.TabIndex = 7;
            this.chk_waiji.Tag = "6";
            this.chk_waiji.CheckedChanged += new System.EventHandler(this.chk_benxianqu_CheckedChanged);
            // 
            // chk_benxianqu
            // 
            this.chk_benxianqu.Location = new System.Drawing.Point(131, 3);
            this.chk_benxianqu.Name = "chk_benxianqu";
            this.chk_benxianqu.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.chk_benxianqu.Properties.Appearance.Options.UseForeColor = true;
            this.chk_benxianqu.Properties.Caption = "1本县区";
            this.chk_benxianqu.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_benxianqu.Properties.RadioGroupIndex = 0;
            this.chk_benxianqu.Size = new System.Drawing.Size(65, 19);
            this.chk_benxianqu.TabIndex = 2;
            this.chk_benxianqu.Tag = "1";
            this.chk_benxianqu.CheckedChanged += new System.EventHandler(this.chk_benxianqu_CheckedChanged);
            // 
            // UCHouseholdscope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_benxianqu);
            this.Controls.Add(this.chk_waiji);
            this.Controls.Add(this.chk_qitasheng);
            this.Controls.Add(this.chk_gangao);
            this.Controls.Add(this.chk_benshengqita);
            this.Controls.Add(this.chk_benshiqita);
            this.Controls.Add(this.txt_hujidi);
            this.Controls.Add(this.lc_hujidi);
            this.Name = "UCHouseholdscope";
            this.Size = new System.Drawing.Size(645, 27);
            ((System.ComponentModel.ISupportInitialize)(this.txt_hujidi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_benshiqita.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_benshengqita.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_gangao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_qitasheng.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_waiji.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_benxianqu.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lc_hujidi;
        private DevExpress.XtraEditors.TextEdit txt_hujidi;
        private DevExpress.XtraEditors.CheckEdit chk_benshiqita;
        private DevExpress.XtraEditors.CheckEdit chk_benshengqita;
        private DevExpress.XtraEditors.CheckEdit chk_gangao;
        private DevExpress.XtraEditors.CheckEdit chk_qitasheng;
        private DevExpress.XtraEditors.CheckEdit chk_waiji;
        private DevExpress.XtraEditors.CheckEdit chk_benxianqu;
    }
}
