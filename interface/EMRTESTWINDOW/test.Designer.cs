namespace EMRTESTWINDOW
{
    partial class test
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
            this.testbt = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // testbt
            // 
            this.testbt.Location = new System.Drawing.Point(2, 376);
            this.testbt.Name = "testbt";
            this.testbt.Size = new System.Drawing.Size(75, 23);
            this.testbt.TabIndex = 1;
            this.testbt.Text = "testbt";
            this.testbt.Visible = false;
            this.testbt.Click += new System.EventHandler(this.testbt_Click);
            // 
            // test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 398);
            this.Controls.Add(this.testbt);
            this.IsMdiContainer = true;
            this.Name = "test";
            this.Text = "test";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.test_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton testbt;



    }
}