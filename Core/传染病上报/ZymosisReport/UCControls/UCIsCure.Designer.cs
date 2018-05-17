namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCIsCure
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
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dt_zhiyu = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_shi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_fou.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_zhiyu.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_zhiyu.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "是否治愈：";
            // 
            // chk_shi
            // 
            this.chk_shi.Location = new System.Drawing.Point(69, 0);
            this.chk_shi.Name = "chk_shi";
            this.chk_shi.Properties.Caption = "是";
            this.chk_shi.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_shi.Properties.RadioGroupIndex = 0;
            this.chk_shi.Size = new System.Drawing.Size(35, 19);
            this.chk_shi.TabIndex = 1;
            this.chk_shi.Tag = "1";
            this.chk_shi.CheckedChanged += new System.EventHandler(this.chk_shi_CheckedChanged);
            // 
            // chk_fou
            // 
            this.chk_fou.Location = new System.Drawing.Point(110, 0);
            this.chk_fou.Name = "chk_fou";
            this.chk_fou.Properties.Caption = "否";
            this.chk_fou.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_fou.Properties.RadioGroupIndex = 0;
            this.chk_fou.Size = new System.Drawing.Size(34, 19);
            this.chk_fou.TabIndex = 2;
            this.chk_fou.Tag = "2";
            this.chk_fou.CheckedChanged += new System.EventHandler(this.chk_fou_CheckedChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(9, 28);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "治愈日期：";
            // 
            // dt_zhiyu
            // 
            this.dt_zhiyu.EditValue = null;
            this.dt_zhiyu.Enabled = false;
            this.dt_zhiyu.Location = new System.Drawing.Point(71, 25);
            this.dt_zhiyu.Name = "dt_zhiyu";
            this.dt_zhiyu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_zhiyu.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dt_zhiyu.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dt_zhiyu.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dt_zhiyu.Size = new System.Drawing.Size(100, 21);
            this.dt_zhiyu.TabIndex = 3;
            this.dt_zhiyu.Tag = "治愈日期";
            // 
            // UCIsCure
            // 
            this.AccessibleName = "是否治愈";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dt_zhiyu);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.chk_fou);
            this.Controls.Add(this.chk_shi);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCIsCure";
            this.Size = new System.Drawing.Size(193, 49);
            this.Tag = "ISCURE";
            ((System.ComponentModel.ISupportInitialize)(this.chk_shi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_fou.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_zhiyu.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_zhiyu.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chk_shi;
        private DevExpress.XtraEditors.CheckEdit chk_fou;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit dt_zhiyu;
    }
}
