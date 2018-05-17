namespace DrectSoft.Emr.QCTimeLimit
{
    partial class FormTest
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxRecordDetailID = new System.Windows.Forms.TextBox();
            this.buttonCompleteRecord = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(113, 49);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "内部服务";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxRecordDetailID
            // 
            this.textBoxRecordDetailID.Location = new System.Drawing.Point(67, 115);
            this.textBoxRecordDetailID.Name = "textBoxRecordDetailID";
            this.textBoxRecordDetailID.Size = new System.Drawing.Size(100, 21);
            this.textBoxRecordDetailID.TabIndex = 1;
            // 
            // buttonCompleteRecord
            // 
            this.buttonCompleteRecord.Location = new System.Drawing.Point(171, 114);
            this.buttonCompleteRecord.Name = "buttonCompleteRecord";
            this.buttonCompleteRecord.Size = new System.Drawing.Size(68, 23);
            this.buttonCompleteRecord.TabIndex = 2;
            this.buttonCompleteRecord.Text = "完成病历";
            this.buttonCompleteRecord.UseVisualStyleBackColor = true;
            this.buttonCompleteRecord.Click += new System.EventHandler(this.buttonCompleteRecord_Click);
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(this.buttonCompleteRecord);
            this.Controls.Add(this.textBoxRecordDetailID);
            this.Controls.Add(this.button1);
            this.Name = "FormTest";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormTest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxRecordDetailID;
        private System.Windows.Forms.Button buttonCompleteRecord;

    }
}

