namespace DrectSoft.Core.NurseDocument.Controls
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgBloodPressure));
            this.DevButtonOK1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.splitTextBox1 = new DrectSoft.Core.NurseDocument.Controls.SplitTextBox(this.components);
            this.SuspendLayout();
            // 
            // DevButtonOK1
            // 
            this.DevButtonOK1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DevButtonOK1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonOK1.Image")));
            this.DevButtonOK1.Location = new System.Drawing.Point(41, 45);
            this.DevButtonOK1.Name = "DevButtonOK1";
            this.DevButtonOK1.Size = new System.Drawing.Size(69, 27);
            this.DevButtonOK1.TabIndex = 1;
            this.DevButtonOK1.Text = "确定(&Y)";
            this.DevButtonOK1.Click += new System.EventHandler(this.DevButtonOK1_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(15, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "上午";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(68, 3);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(28, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = " 下午";
            // 
            // splitTextBox1
            // 
            this.splitTextBox1.Location = new System.Drawing.Point(0, 20);
            this.splitTextBox1.Name = "splitTextBox1";
            this.splitTextBox1.Size = new System.Drawing.Size(116, 22);
            this.splitTextBox1.TabIndex = 0;
            // 
            // DlgBloodPressure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(116, 78);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.splitTextBox1);
            this.Controls.Add(this.DevButtonOK1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgBloodPressure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "血压";
            this.Shown += new System.EventHandler(this.DlgBloodPressure_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK DevButtonOK1;
        private SplitTextBox splitTextBox1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
	}
}