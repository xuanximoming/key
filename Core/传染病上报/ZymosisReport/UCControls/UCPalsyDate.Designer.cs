namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCPalsyDate
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
            this.dt_mabi = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_mabi.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_mabi.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(7, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "麻痹日期：";
            // 
            // dt_mabi
            // 
            this.dt_mabi.EditValue = null;
            this.dt_mabi.Location = new System.Drawing.Point(69, 0);
            this.dt_mabi.Name = "dt_mabi";
            this.dt_mabi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_mabi.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dt_mabi.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dt_mabi.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dt_mabi.Size = new System.Drawing.Size(100, 21);
            this.dt_mabi.TabIndex = 1;
            this.dt_mabi.Tag = "1";
            // 
            // UCPalsyDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dt_mabi);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCPalsyDate";
            this.Size = new System.Drawing.Size(199, 26);
            this.Tag = "PALSYDATE";
            ((System.ComponentModel.ISupportInitialize)(this.dt_mabi.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_mabi.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dt_mabi;
    }
}
