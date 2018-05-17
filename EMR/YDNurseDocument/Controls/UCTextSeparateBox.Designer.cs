namespace DrectSoft.Core.NurseDocument.Controls
{
    partial class UCTextSeparateBox
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
            this.txtSBP = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtDBP = new DevExpress.XtraEditors.TextEdit();
            this.textEdit3 = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDBP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSBP
            // 
            this.txtSBP.EditValue = "";
            this.txtSBP.Location = new System.Drawing.Point(3, 1);
            this.txtSBP.Name = "txtSBP";
            this.txtSBP.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtSBP.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.txtSBP.Properties.Appearance.Options.UseBackColor = true;
            this.txtSBP.Properties.Appearance.Options.UseFont = true;
            this.txtSBP.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtSBP.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSBP.Size = new System.Drawing.Size(41, 17);
            this.txtSBP.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl1.Appearance.Options.UseBackColor = true;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(46, 1);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(5, 16);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "/";
            // 
            // txtDBP
            // 
            this.txtDBP.Location = new System.Drawing.Point(53, 1);
            this.txtDBP.Name = "txtDBP";
            this.txtDBP.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtDBP.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.txtDBP.Properties.Appearance.Options.UseBackColor = true;
            this.txtDBP.Properties.Appearance.Options.UseFont = true;
            this.txtDBP.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtDBP.Size = new System.Drawing.Size(39, 17);
            this.txtDBP.TabIndex = 2;
            // 
            // textEdit3
            // 
            this.textEdit3.Location = new System.Drawing.Point(0, 0);
            this.textEdit3.Name = "textEdit3";
            this.textEdit3.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textEdit3.Properties.Appearance.Options.UseBackColor = true;
            this.textEdit3.Size = new System.Drawing.Size(95, 21);
            this.textEdit3.TabIndex = 3;
            // 
            // UCTextSeparateBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDBP);
            this.Controls.Add(this.txtSBP);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.textEdit3);
            this.Name = "UCTextSeparateBox";
            this.Size = new System.Drawing.Size(95, 21);
            this.Load += new System.EventHandler(this.UCTextSeparateBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtSBP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDBP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtSBP;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtDBP;
        private DevExpress.XtraEditors.TextEdit textEdit3;
    }
}
