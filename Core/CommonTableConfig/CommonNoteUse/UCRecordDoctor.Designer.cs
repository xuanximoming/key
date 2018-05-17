namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    partial class UCRecordDoctor
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
            this.txtRecordDoctor = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordDoctor.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRecordDoctor
            // 
            this.txtRecordDoctor.EnterMoveNextControl = true;
            this.txtRecordDoctor.Location = new System.Drawing.Point(91, 9);
            this.txtRecordDoctor.Name = "txtRecordDoctor";
            this.txtRecordDoctor.Size = new System.Drawing.Size(180, 21);
            this.txtRecordDoctor.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "记录人：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UCRecordDoctor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRecordDoctor);
            this.Name = "UCRecordDoctor";
            this.Size = new System.Drawing.Size(300, 35);
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordDoctor.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtRecordDoctor;
        private System.Windows.Forms.Label label1;
    }
}
