namespace DrectSoft.Core.NursingDocuments.UserControls
{
    partial class UCThreeMeasureTable
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
            this.pictureBoxMeasureTable = new System.Windows.Forms.PictureBox();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMeasureTable)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxMeasureTable
            // 
            this.pictureBoxMeasureTable.Location = new System.Drawing.Point(20, 15);
            this.pictureBoxMeasureTable.Name = "pictureBoxMeasureTable";
            this.pictureBoxMeasureTable.Size = new System.Drawing.Size(117, 58);
            this.pictureBoxMeasureTable.TabIndex = 0;
            this.pictureBoxMeasureTable.TabStop = false;
            this.pictureBoxMeasureTable.Click += new System.EventHandler(this.pictureBoxMeasureTable_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(-8, -7);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(10, 10);
            this.simpleButton1.TabIndex = 1;
            // 
            // UCThreeMeasureTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.pictureBoxMeasureTable);
            this.Name = "UCThreeMeasureTable";
            this.Size = new System.Drawing.Size(175, 175);
            this.Click += new System.EventHandler(this.UCThreeMeasureTable_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMeasureTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxMeasureTable;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}
