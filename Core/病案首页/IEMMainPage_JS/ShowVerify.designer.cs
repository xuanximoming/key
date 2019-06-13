namespace DrectSoft.Core.IEMMainPage
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
            this.richTextBoxVerify = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBoxVerify
            // 
            this.richTextBoxVerify.BackColor = System.Drawing.Color.White;
            this.richTextBoxVerify.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBoxVerify.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxVerify.ForeColor = System.Drawing.Color.Red;
            this.richTextBoxVerify.Location = new System.Drawing.Point(0, 42);
            this.richTextBoxVerify.Name = "richTextBoxVerify";
            this.richTextBoxVerify.ReadOnly = true;
            this.richTextBoxVerify.Size = new System.Drawing.Size(288, 306);
            this.richTextBoxVerify.TabIndex = 2;
            this.richTextBoxVerify.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(288, 36);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "缺少以下信息：";
            // 
            // ShowVerify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 348);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.richTextBoxVerify);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShowVerify";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "提示";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxVerify;
        private System.Windows.Forms.RichTextBox richTextBox1;

    }
}