namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCPalsySymptom
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txt_zhengzhuang = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_zhengzhuang.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(7, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "麻痹症状：";
            // 
            // txt_zhengzhuang
            // 
            this.txt_zhengzhuang.Location = new System.Drawing.Point(69, 0);
            this.txt_zhengzhuang.Name = "txt_zhengzhuang";
            this.txt_zhengzhuang.Size = new System.Drawing.Size(304, 21);
            this.txt_zhengzhuang.TabIndex = 1;
            this.txt_zhengzhuang.Tag = "1";
            // 
            // UCPalsySymptom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt_zhengzhuang);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCPalsySymptom";
            this.Size = new System.Drawing.Size(452, 27);
            this.Tag = "PALSYSYMPTOM";
            ((System.ComponentModel.ISupportInitialize)(this.txt_zhengzhuang.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_zhengzhuang;
    }
}
