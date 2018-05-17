namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCHospitalStatus
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
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.dt_zhuyuan = new DevExpress.XtraEditors.DateEdit();
            this.dt_chuyuan = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_shi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_fou.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_zhuyuan.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_zhuyuan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_chuyuan.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_chuyuan.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "是否住院：";
            // 
            // chk_shi
            // 
            this.chk_shi.Location = new System.Drawing.Point(69, 2);
            this.chk_shi.Name = "chk_shi";
            this.chk_shi.Properties.Caption = "是";
            this.chk_shi.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_shi.Properties.RadioGroupIndex = 0;
            this.chk_shi.Size = new System.Drawing.Size(34, 19);
            this.chk_shi.TabIndex = 1;
            this.chk_shi.Tag = "1";
            this.chk_shi.CheckedChanged += new System.EventHandler(this.chk_shi_CheckedChanged);
            // 
            // chk_fou
            // 
            this.chk_fou.Location = new System.Drawing.Point(109, 2);
            this.chk_fou.Name = "chk_fou";
            this.chk_fou.Properties.Caption = "否";
            this.chk_fou.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.chk_fou.Properties.RadioGroupIndex = 0;
            this.chk_fou.Size = new System.Drawing.Size(35, 19);
            this.chk_fou.TabIndex = 2;
            this.chk_fou.Tag = "2";
            this.chk_fou.CheckedChanged += new System.EventHandler(this.chk_fou_CheckedChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(9, 30);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "住院日期：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(9, 59);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "出院日期：";
            // 
            // dt_zhuyuan
            // 
            this.dt_zhuyuan.EditValue = null;
            this.dt_zhuyuan.Enabled = false;
            this.dt_zhuyuan.Location = new System.Drawing.Point(71, 27);
            this.dt_zhuyuan.Name = "dt_zhuyuan";
            this.dt_zhuyuan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_zhuyuan.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dt_zhuyuan.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dt_zhuyuan.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dt_zhuyuan.Size = new System.Drawing.Size(100, 21);
            this.dt_zhuyuan.TabIndex = 3;
            this.dt_zhuyuan.Tag = "住院日期";
            // 
            // dt_chuyuan
            // 
            this.dt_chuyuan.EditValue = null;
            this.dt_chuyuan.Enabled = false;
            this.dt_chuyuan.Location = new System.Drawing.Point(71, 58);
            this.dt_chuyuan.Name = "dt_chuyuan";
            this.dt_chuyuan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_chuyuan.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dt_chuyuan.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dt_chuyuan.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dt_chuyuan.Size = new System.Drawing.Size(100, 21);
            this.dt_chuyuan.TabIndex = 4;
            this.dt_chuyuan.Tag = "出院日期";
            // 
            // UCHospitalStatus
            // 
            this.AccessibleName = "是否住院";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dt_chuyuan);
            this.Controls.Add(this.dt_zhuyuan);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.chk_fou);
            this.Controls.Add(this.chk_shi);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCHospitalStatus";
            this.Size = new System.Drawing.Size(193, 82);
            this.Tag = "HOSPITALSTATUS";
            ((System.ComponentModel.ISupportInitialize)(this.chk_shi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_fou.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_zhuyuan.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_zhuyuan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_chuyuan.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_chuyuan.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chk_shi;
        private DevExpress.XtraEditors.CheckEdit chk_fou;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.DateEdit dt_zhuyuan;
        private DevExpress.XtraEditors.DateEdit dt_chuyuan;
    }
}
