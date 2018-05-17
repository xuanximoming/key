namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCDiagnosisDate
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
            this.dt_jiuzhen = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_jiuzhen.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_jiuzhen.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "来现就诊地日期：";
            // 
            // dt_jiuzhen
            // 
            this.dt_jiuzhen.EditValue = null;
            this.dt_jiuzhen.Location = new System.Drawing.Point(108, 0);
            this.dt_jiuzhen.Name = "dt_jiuzhen";
            this.dt_jiuzhen.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_jiuzhen.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dt_jiuzhen.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dt_jiuzhen.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dt_jiuzhen.Size = new System.Drawing.Size(100, 21);
            this.dt_jiuzhen.TabIndex = 1;
            this.dt_jiuzhen.Tag = "1";
            // 
            // UCDiagnosisDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dt_jiuzhen);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCDiagnosisDate";
            this.Size = new System.Drawing.Size(257, 26);
            this.Tag = "DIAGNOSISDATE";
            ((System.ComponentModel.ISupportInitialize)(this.dt_jiuzhen.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_jiuzhen.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dt_jiuzhen;
    }
}
