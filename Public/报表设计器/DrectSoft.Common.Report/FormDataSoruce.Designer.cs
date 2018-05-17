namespace DrectSoft.Common.Report
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
			this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			((System.ComponentModel.ISupportInitialize)(this.memoEditSql.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textEditPath.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// memoEditSql
			// 
			this.memoEditSql.Location = new System.Drawing.Point(14, 35);
			this.memoEditSql.Name = "memoEditSql";
			this.memoEditSql.Size = new System.Drawing.Size(409, 68);
			this.memoEditSql.TabIndex = 0;
			this.memoEditSql.UseOptimizedRendering = true;
			// 
			// labelControlShow
			// 
			this.labelControlShow.Location = new System.Drawing.Point(14, 12);
			this.labelControlShow.Name = "labelControlShow";
			this.labelControlShow.Size = new System.Drawing.Size(195, 14);
			this.labelControlShow.TabIndex = 1;
			this.labelControlShow.Text = "输入SQL语句或存储过程(需传参）：";
			// 
			// simpleButtonExec
			// 
			this.simpleButtonExec.Location = new System.Drawing.Point(216, 153);
			this.simpleButtonExec.Name = "simpleButtonExec";
			this.simpleButtonExec.Size = new System.Drawing.Size(100, 27);
			this.simpleButtonExec.TabIndex = 2;
			this.simpleButtonExec.Text = "执行SQL";
			this.simpleButtonExec.Click += new System.EventHandler(this.simpleButtonExec_Click);
			// 
			// simpleButton2
			// 
			this.simpleButton2.Location = new System.Drawing.Point(412, 153);
			this.simpleButton2.Name = "simpleButton2";
			this.simpleButton2.Size = new System.Drawing.Size(12, 27);
			this.simpleButton2.TabIndex = 3;
			this.simpleButton2.Text = "执行存储过程";
			this.simpleButton2.Visible = false;
			this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
			// 
			// textEditPath
			// 
			this.textEditPath.EditValue = "C:\\New.repx";
			this.textEditPath.Location = new System.Drawing.Point(14, 121);
			this.textEditPath.Name = "textEditPath";
			this.textEditPath.Size = new System.Drawing.Size(325, 20);
			this.textEditPath.TabIndex = 4;
			// 
			// simpleButtonSelect
			// 
			this.simpleButtonSelect.Location = new System.Drawing.Point(346, 119);
			this.simpleButtonSelect.Name = "simpleButtonSelect";
			this.simpleButtonSelect.Size = new System.Drawing.Size(87, 27);
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
			this.labelControlWarning.Location = new System.Drawing.Point(234, 12);
			this.labelControlWarning.Name = "labelControlWarning";
			this.labelControlWarning.Size = new System.Drawing.Size(0, 14);
			this.labelControlWarning.TabIndex = 6;
			// 
			// comboBoxEdit1
			// 
			this.comboBoxEdit1.Location = new System.Drawing.Point(95, 156);
			this.comboBoxEdit1.Name = "comboBoxEdit1";
			this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.comboBoxEdit1.Size = new System.Drawing.Size(115, 20);
			this.comboBoxEdit1.TabIndex = 7;
			// 
			// labelControl1
			// 
			this.labelControl1.Location = new System.Drawing.Point(14, 159);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(36, 14);
			this.labelControl1.TabIndex = 8;
			this.labelControl1.Text = "数据源";
			// 
			// FormDataSoruce
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(437, 192);
			this.Controls.Add(this.labelControl1);
			this.Controls.Add(this.comboBoxEdit1);
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
			((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
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
		private DevExpress.XtraEditors.LabelControl labelControlWarning;
		private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}