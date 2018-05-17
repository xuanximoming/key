namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCFirstDate
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
            this.dt_frist = new DevExpress.XtraEditors.DateEdit();
            this.chk_buxiang = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_frist.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_frist.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_buxiang.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(0, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(168, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "首次出现乙肝症状和体征时间：";
            // 
            // dt_frist
            // 
            this.dt_frist.EditValue = null;
            this.dt_frist.Location = new System.Drawing.Point(170, 0);
            this.dt_frist.Name = "dt_frist";
            this.dt_frist.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_frist.Properties.Mask.EditMask = "yyyy年MM月";
            this.dt_frist.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dt_frist.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dt_frist.Size = new System.Drawing.Size(100, 21);
            this.dt_frist.TabIndex = 1;
            this.dt_frist.Tag = "1";
            // 
            // chk_buxiang
            // 
            this.chk_buxiang.Location = new System.Drawing.Point(276, 2);
            this.chk_buxiang.Name = "chk_buxiang";
            this.chk_buxiang.Properties.Caption = "不详";
            this.chk_buxiang.Size = new System.Drawing.Size(48, 19);
            this.chk_buxiang.TabIndex = 2;
            this.chk_buxiang.Tag = "2";
            this.chk_buxiang.CheckedChanged += new System.EventHandler(this.chk_buxiang_CheckedChanged);
            // 
            // UCFirstDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_buxiang);
            this.Controls.Add(this.dt_frist);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCFirstDate";
            this.Size = new System.Drawing.Size(327, 23);
            this.Tag = "FRISTDATE";
            ((System.ComponentModel.ISupportInitialize)(this.dt_frist.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_frist.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_buxiang.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dt_frist;
        private DevExpress.XtraEditors.CheckEdit chk_buxiang;
    }
}
