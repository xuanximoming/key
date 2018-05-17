namespace YidanSoft.Common.Report
{
    partial class FormDataSoruce
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
            this.memoEditSql = new DevExpress.XtraEditors.MemoEdit();
            this.labelControlShow = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonExec = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.textEditPath = new DevExpress.XtraEditors.TextEdit();
            this.simpleButtonSelect = new DevExpress.XtraEditors.SimpleButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelControlWarning = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSql.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // memoEditSql
            // 
            this.memoEditSql.Location = new System.Drawing.Point(12, 30);
            this.memoEditSql.Name = "memoEditSql";
            this.memoEditSql.Size = new System.Drawing.Size(351, 58);
            this.memoEditSql.TabIndex = 0;
            // 
            // labelControlShow
            // 
            this.labelControlShow.Location = new System.Drawing.Point(12, 10);
            this.labelControlShow.Name = "labelControlShow";
            this.labelControlShow.Size = new System.Drawing.Size(195, 14);
            this.labelControlShow.TabIndex = 1;
            this.labelControlShow.Text = "输入SQL语句或存储过程(需传参）：";
            // 
            // simpleButtonExec
            // 
            this.simpleButtonExec.Location = new System.Drawing.Point(113, 131);
            this.simpleButtonExec.Name = "simpleButtonExec";
            this.simpleButtonExec.Size = new System.Drawing.Size(126, 23);
            this.simpleButtonExec.TabIndex = 2;
            this.simpleButtonExec.Text = "执行SQL";
            this.simpleButtonExec.Click += new System.EventHandler(this.simpleButtonExec_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(353, 131);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(10, 23);
            this.simpleButton2.TabIndex = 3;
            this.simpleButton2.Text = "执行存储过程";
            this.simpleButton2.Visible = false;
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // textEditPath
            // 
            this.textEditPath.EditValue = "C:\\New.repx";
            this.textEditPath.Location = new System.Drawing.Point(12, 104);
            this.textEditPath.Name = "textEditPath";
            this.textEditPath.Size = new System.Drawing.Size(279, 21);
            this.textEditPath.TabIndex = 4;
            // 
            // simpleButtonSelect
            // 
            this.simpleButtonSelect.Location = new System.Drawing.Point(297, 102);
            this.simpleButtonSelect.Name = "simpleButtonSelect";
            this.simpleButtonSelect.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonSelect.TabIndex = 5;
            this.simpleButtonSelect.Text = "选择文件";
            this.simpleButtonSelect.Click += new System.EventHandler(this.simpleButtonSelect_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // labelControlWarning
            // 
            this.labelControlWarning.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControlWarning.Appearance.Options.UseForeColor = true;
            this.labelControlWarning.Location = new System.Drawing.Point(201, 10);
            this.labelControlWarning.Name = "labelControlWarning";
            this.labelControlWarning.Size = new System.Drawing.Size(0, 14);
            this.labelControlWarning.TabIndex = 6;
            // 
            // FormDataSoruce
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 165);
            this.Controls.Add(this.labelControlWarning);
            this.Controls.Add(this.simpleButtonSelect);
            this.Controls.Add(this.textEditPath);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButtonExec);
            this.Controls.Add(this.labelControlShow);
            this.Controls.Add(this.memoEditSql);
            this.MaximizeBox = false;
            this.Name = "FormDataSoruce";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "报表模板选择";
            this.Load += new System.EventHandler(this.FormDataSoruce_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSql.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPath.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit memoEditSql;
        private DevExpress.XtraEditors.LabelControl labelControlShow;
        private DevExpress.XtraEditors.SimpleButton simpleButtonExec;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.TextEdit textEditPath;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSelect;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevExpress.XtraEditors.LabelControl labelControlWarning;
    }
}