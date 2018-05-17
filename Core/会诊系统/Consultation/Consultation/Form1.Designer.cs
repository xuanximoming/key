namespace YidanSoft.Core.Consultation
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lookUpEditor1 = new YidanSoft.Common.Library.LookUpEditor();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditor1)).BeginInit();
            this.SuspendLayout();
            // 
            // lookUpEditor1
            // 
            this.lookUpEditor1.ListWindow = null;
            this.lookUpEditor1.Location = new System.Drawing.Point(78, 99);
            this.lookUpEditor1.Name = "lookUpEditor1";
            this.lookUpEditor1.Size = new System.Drawing.Size(100, 20);
            this.lookUpEditor1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.lookUpEditor1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditor1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Library.LookUpEditor lookUpEditor1;
    }
}