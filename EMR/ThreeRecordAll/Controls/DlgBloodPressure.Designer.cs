namespace DrectSoft.EMR.ThreeRecordAll.Controls
{
    partial class DlgBloodPressure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgBloodPressure));
            this.DevButtonOK1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK();
            this.splitTextBox1 = new SplitTextBox();
            this.SuspendLayout();
            // 
            // DevButtonOK1
            // 
            this.DevButtonOK1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DevButtonOK1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonOK1.Image")));
            this.DevButtonOK1.Location = new System.Drawing.Point(34, 28);
            this.DevButtonOK1.Name = "DevButtonOK1";
            this.DevButtonOK1.Size = new System.Drawing.Size(67, 23);
            this.DevButtonOK1.TabIndex = 31;
            this.DevButtonOK1.Text = "确定(&Y)";
            this.DevButtonOK1.Click += new System.EventHandler(this.DevButtonOK1_Click);
            // 
            // splitTextBox1
            // 
            this.splitTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitTextBox1.Location = new System.Drawing.Point(0, 0);
            this.splitTextBox1.Name = "splitTextBox1";
            this.splitTextBox1.Size = new System.Drawing.Size(102, 22);
            this.splitTextBox1.TabIndex = 0;
            // 
            // DlgBloodPressure
            // 
            this.AcceptButton = this.DevButtonOK1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(102, 52);
            this.Controls.Add(this.DevButtonOK1);
            this.Controls.Add(this.splitTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(118, 90);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(118, 90);
            this.Name = "DlgBloodPressure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "血压";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SplitTextBox splitTextBox1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK DevButtonOK1;
    }
}