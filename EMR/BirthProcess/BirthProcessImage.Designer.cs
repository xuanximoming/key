namespace DrectSoft.Core.BirthProcess
{
    partial class BirthProcessImage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BirthProcessImage));
            this.panel1 = new System.Windows.Forms.Panel();
            this.DevButtonImportExcel1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonImportExcel();
            this.DevButtonPrint1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonPrint();
            this.DevButtonRefresh1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonRefresh();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.DevButtonImportExcel1);
            this.panel1.Controls.Add(this.DevButtonPrint1);
            this.panel1.Controls.Add(this.DevButtonRefresh1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(721, 29);
            this.panel1.TabIndex = 2;
            // 
            // DevButtonImportExcel1
            // 
            this.DevButtonImportExcel1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonImportExcel1.Image")));
            this.DevButtonImportExcel1.ImageSelect = DrectSoft.Common.Ctrs.OTHER.DevButtonImportExcel.ImageTypes.ImportExcel;
            this.DevButtonImportExcel1.Location = new System.Drawing.Point(174, 5);
            this.DevButtonImportExcel1.Name = "DevButtonImportExcel1";
            this.DevButtonImportExcel1.Size = new System.Drawing.Size(69, 20);
            this.DevButtonImportExcel1.TabIndex = 2;
            this.DevButtonImportExcel1.Text = "导出(&I)";
            // 
            // DevButtonPrint1
            // 
            this.DevButtonPrint1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonPrint1.Image")));
            this.DevButtonPrint1.Location = new System.Drawing.Point(92, 5);
            this.DevButtonPrint1.Name = "DevButtonPrint1";
            this.DevButtonPrint1.Size = new System.Drawing.Size(69, 20);
            this.DevButtonPrint1.TabIndex = 1;
            this.DevButtonPrint1.Text = "打印(&P)";
            // 
            // DevButtonRefresh1
            // 
            this.DevButtonRefresh1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonRefresh1.Image")));
            this.DevButtonRefresh1.Location = new System.Drawing.Point(10, 5);
            this.DevButtonRefresh1.Name = "DevButtonRefresh1";
            this.DevButtonRefresh1.Size = new System.Drawing.Size(69, 20);
            this.DevButtonRefresh1.TabIndex = 0;
            this.DevButtonRefresh1.Text = "刷新(&R)";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.pictureBoxImage);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(721, 472);
            this.panel2.TabIndex = 3;
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.BackColor = System.Drawing.Color.White;
            this.pictureBoxImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxImage.Location = new System.Drawing.Point(63, 33);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(563, 315);
            this.pictureBoxImage.TabIndex = 1;
            this.pictureBoxImage.TabStop = false;
            // 
            // BirthProcessImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 501);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BirthProcessImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "产程图";
            this.Load += new System.EventHandler(this.BirthProcessImage_Load);
            this.Resize += new System.EventHandler(this.BirthProcessImage_Resize);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonImportExcel DevButtonImportExcel1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonPrint DevButtonPrint1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonRefresh DevButtonRefresh1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBoxImage;


    }
}