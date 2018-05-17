namespace DrectSoft.Wordbook
{
   partial class FormSqlWordbook
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
         this.tBoxQuery = new System.Windows.Forms.TextBox();
         this.tBoxName = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.tBoxCodeField = new System.Windows.Forms.TextBox();
         this.tBoxNameField = new System.Windows.Forms.TextBox();
         this.tBoxFilter = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.panel1 = new System.Windows.Forms.Panel();
         this.btnCancle = new System.Windows.Forms.Button();
         this.btnOk = new System.Windows.Forms.Button();
         this.label6 = new System.Windows.Forms.Label();
         this.lvColumnStyles = new System.Windows.Forms.ListView();
         this.colHeaderField = new System.Windows.Forms.ColumnHeader();
         this.colHeaderCaption = new System.Windows.Forms.ColumnHeader();
         this.colHeaderWidth = new System.Windows.Forms.ColumnHeader();
         this.panel2 = new System.Windows.Forms.Panel();
         this.btnDelete = new System.Windows.Forms.Button();
         this.btnAdd = new System.Windows.Forms.Button();
         this.tBoxWidth = new System.Windows.Forms.TextBox();
         this.tBoxCaption = new System.Windows.Forms.TextBox();
         this.label9 = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.tBoxFieldName = new System.Windows.Forms.TextBox();
         this.label7 = new System.Windows.Forms.Label();
         this.label10 = new System.Windows.Forms.Label();
         this.panel1.SuspendLayout();
         this.panel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // tBoxQuery
         // 
         this.tBoxQuery.BackColor = System.Drawing.SystemColors.HighlightText;
         this.tBoxQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxQuery.Location = new System.Drawing.Point(70, 28);
         this.tBoxQuery.Multiline = true;
         this.tBoxQuery.Name = "tBoxQuery";
         this.tBoxQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
         this.tBoxQuery.Size = new System.Drawing.Size(336, 148);
         this.tBoxQuery.TabIndex = 1;
         // 
         // tBoxName
         // 
         this.tBoxName.BackColor = System.Drawing.SystemColors.HighlightText;
         this.tBoxName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxName.Location = new System.Drawing.Point(70, 5);
         this.tBoxName.Name = "tBoxName";
         this.tBoxName.Size = new System.Drawing.Size(336, 21);
         this.tBoxName.TabIndex = 0;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(12, 29);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(53, 12);
         this.label3.TabIndex = 28;
         this.label3.Text = "查询语句";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(12, 9);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(53, 12);
         this.label2.TabIndex = 27;
         this.label2.Text = "字典名称";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(213, 182);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(53, 12);
         this.label1.TabIndex = 32;
         this.label1.Text = "名称字段";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(12, 182);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(53, 12);
         this.label4.TabIndex = 31;
         this.label4.Text = "代码字段";
         // 
         // tBoxCodeField
         // 
         this.tBoxCodeField.BackColor = System.Drawing.SystemColors.HighlightText;
         this.tBoxCodeField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxCodeField.Location = new System.Drawing.Point(70, 178);
         this.tBoxCodeField.Name = "tBoxCodeField";
         this.tBoxCodeField.Size = new System.Drawing.Size(124, 21);
         this.tBoxCodeField.TabIndex = 2;
         // 
         // tBoxNameField
         // 
         this.tBoxNameField.BackColor = System.Drawing.SystemColors.HighlightText;
         this.tBoxNameField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxNameField.Location = new System.Drawing.Point(282, 178);
         this.tBoxNameField.Name = "tBoxNameField";
         this.tBoxNameField.Size = new System.Drawing.Size(124, 21);
         this.tBoxNameField.TabIndex = 3;
         // 
         // tBoxFilter
         // 
         this.tBoxFilter.BackColor = System.Drawing.SystemColors.HighlightText;
         this.tBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxFilter.Location = new System.Drawing.Point(70, 202);
         this.tBoxFilter.Name = "tBoxFilter";
         this.tBoxFilter.Size = new System.Drawing.Size(261, 21);
         this.tBoxFilter.TabIndex = 4;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(12, 206);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(53, 12);
         this.label5.TabIndex = 36;
         this.label5.Text = "选择字段";
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.btnCancle);
         this.panel1.Controls.Add(this.btnOk);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panel1.Location = new System.Drawing.Point(0, 379);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(410, 30);
         this.panel1.TabIndex = 37;
         // 
         // btnCancle
         // 
         this.btnCancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
         this.btnCancle.Location = new System.Drawing.Point(274, 3);
         this.btnCancle.Name = "btnCancle";
         this.btnCancle.Size = new System.Drawing.Size(85, 25);
         this.btnCancle.TabIndex = 1;
         this.btnCancle.Text = "取消";
         // 
         // btnOk
         // 
         this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
         this.btnOk.Location = new System.Drawing.Point(71, 3);
         this.btnOk.Name = "btnOk";
         this.btnOk.Size = new System.Drawing.Size(85, 25);
         this.btnOk.TabIndex = 0;
         this.btnOk.Text = "确定";
         this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(12, 227);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(53, 12);
         this.label6.TabIndex = 38;
         this.label6.Text = "显示样式";
         // 
         // lvColumnStyles
         // 
         this.lvColumnStyles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lvColumnStyles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHeaderField,
            this.colHeaderCaption,
            this.colHeaderWidth});
         this.lvColumnStyles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
         this.lvColumnStyles.Location = new System.Drawing.Point(5, 58);
         this.lvColumnStyles.Name = "lvColumnStyles";
         this.lvColumnStyles.Size = new System.Drawing.Size(328, 88);
         this.lvColumnStyles.TabIndex = 39;
         this.lvColumnStyles.UseCompatibleStateImageBehavior = false;
         this.lvColumnStyles.View = System.Windows.Forms.View.Details;
         this.lvColumnStyles.SelectedIndexChanged += new System.EventHandler(this.lvColumnStyles_SelectedIndexChanged);
         // 
         // colHeaderField
         // 
         this.colHeaderField.Text = "字段名";
         this.colHeaderField.Width = 92;
         // 
         // colHeaderCaption
         // 
         this.colHeaderCaption.Text = "显示名称";
         this.colHeaderCaption.Width = 124;
         // 
         // colHeaderWidth
         // 
         this.colHeaderWidth.Text = "列宽";
         this.colHeaderWidth.Width = 58;
         // 
         // panel2
         // 
         this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel2.Controls.Add(this.btnDelete);
         this.panel2.Controls.Add(this.btnAdd);
         this.panel2.Controls.Add(this.tBoxWidth);
         this.panel2.Controls.Add(this.tBoxCaption);
         this.panel2.Controls.Add(this.label9);
         this.panel2.Controls.Add(this.label8);
         this.panel2.Controls.Add(this.tBoxFieldName);
         this.panel2.Controls.Add(this.label7);
         this.panel2.Controls.Add(this.lvColumnStyles);
         this.panel2.Location = new System.Drawing.Point(70, 227);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(336, 149);
         this.panel2.TabIndex = 40;
         // 
         // btnDelete
         // 
         this.btnDelete.Enabled = false;
         this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
         this.btnDelete.Location = new System.Drawing.Point(246, 30);
         this.btnDelete.Name = "btnDelete";
         this.btnDelete.Size = new System.Drawing.Size(85, 21);
         this.btnDelete.TabIndex = 47;
         this.btnDelete.Text = "删除";
         this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
         // 
         // btnAdd
         // 
         this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
         this.btnAdd.Location = new System.Drawing.Point(145, 30);
         this.btnAdd.Name = "btnAdd";
         this.btnAdd.Size = new System.Drawing.Size(85, 21);
         this.btnAdd.TabIndex = 46;
         this.btnAdd.Text = "增加";
         this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
         // 
         // tBoxWidth
         // 
         this.tBoxWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxWidth.Location = new System.Drawing.Point(49, 30);
         this.tBoxWidth.Name = "tBoxWidth";
         this.tBoxWidth.Size = new System.Drawing.Size(75, 21);
         this.tBoxWidth.TabIndex = 45;
         // 
         // tBoxCaption
         // 
         this.tBoxCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxCaption.Location = new System.Drawing.Point(212, 3);
         this.tBoxCaption.Name = "tBoxCaption";
         this.tBoxCaption.Size = new System.Drawing.Size(119, 21);
         this.tBoxCaption.TabIndex = 44;
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(15, 36);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(29, 12);
         this.label9.TabIndex = 43;
         this.label9.Text = "列宽";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(143, 7);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(53, 12);
         this.label8.TabIndex = 42;
         this.label8.Text = "显示名称";
         // 
         // tBoxFieldName
         // 
         this.tBoxFieldName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxFieldName.Location = new System.Drawing.Point(49, 3);
         this.tBoxFieldName.Name = "tBoxFieldName";
         this.tBoxFieldName.Size = new System.Drawing.Size(75, 21);
         this.tBoxFieldName.TabIndex = 41;
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(3, 7);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(41, 12);
         this.label7.TabIndex = 40;
         this.label7.Text = "字段名";
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(329, 206);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(77, 12);
         this.label10.TabIndex = 41;
         this.label10.Text = "(用\"//\"分隔)";
         // 
         // FormSqlWordbook
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(410, 409);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.panel1);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.tBoxFilter);
         this.Controls.Add(this.tBoxNameField);
         this.Controls.Add(this.tBoxCodeField);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.tBoxQuery);
         this.Controls.Add(this.tBoxName);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label10);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FormSqlWordbook";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "使用SQL语句创建字典类";
         this.panel1.ResumeLayout(false);
         this.panel2.ResumeLayout(false);
         this.panel2.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox tBoxQuery;
      private System.Windows.Forms.TextBox tBoxName;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox tBoxCodeField;
      private System.Windows.Forms.TextBox tBoxNameField;
      private System.Windows.Forms.TextBox tBoxFilter;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Button btnCancle;
      private System.Windows.Forms.Button btnOk;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.ListView lvColumnStyles;
      private System.Windows.Forms.ColumnHeader colHeaderField;
      private System.Windows.Forms.ColumnHeader colHeaderCaption;
      private System.Windows.Forms.ColumnHeader colHeaderWidth;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.Button btnAdd;
      private System.Windows.Forms.TextBox tBoxWidth;
      private System.Windows.Forms.TextBox tBoxCaption;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.TextBox tBoxFieldName;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Button btnDelete;
      private System.Windows.Forms.Label label10;
   }
}