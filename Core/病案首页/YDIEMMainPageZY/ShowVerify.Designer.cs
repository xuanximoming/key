namespace DrectSoft.Core.IEMMainPageZY
{
    partial class ShowVerify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowVerify));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBoxVerify = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(270, 36);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "缺少以下信息：";
            // 
            // richTextBoxVerify
            // 
            this.richTextBoxVerify.BackColor = System.Drawing.Color.White;
            this.richTextBoxVerify.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBoxVerify.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.richTextBoxVerify.ForeColor = System.Drawing.Color.Red;
            this.richTextBoxVerify.Location = new System.Drawing.Point(0, 42);
            this.richTextBoxVerify.Name = "richTextBoxVerify";
            this.richTextBoxVerify.ReadOnly = true;
            this.richTextBoxVerify.Size = new System.Drawing.Size(270, 378);
            this.richTextBoxVerify.TabIndex = 1;
            this.richTextBoxVerify.Text = "";
            // 
            // ShowVerify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 420);
            this.Controls.Add(this.richTextBoxVerify);
            this.Controls.Add(this.richTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShowVerify";
            this.Text = "ShowVerify";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBoxVerify;
    }
}