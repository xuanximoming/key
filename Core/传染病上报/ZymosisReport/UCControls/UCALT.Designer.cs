namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCALT
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
            this.txt_alt = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txt_alt.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(7, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(58, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "本次ALT：";
            // 
            // txt_alt
            // 
            this.txt_alt.Location = new System.Drawing.Point(67, 3);
            this.txt_alt.Name = "txt_alt";
            this.txt_alt.Size = new System.Drawing.Size(100, 21);
            this.txt_alt.TabIndex = 1;
            this.txt_alt.Tag = "1";
            this.txt_alt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_alt_KeyPress);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(173, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(19, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "U/L";
            // 
            // UCALT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txt_alt);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCALT";
            this.Size = new System.Drawing.Size(371, 27);
            this.Tag = "ALT";
            ((System.ComponentModel.ISupportInitialize)(this.txt_alt.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_alt;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}
