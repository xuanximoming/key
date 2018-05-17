namespace DrectSoft.Wordbook
{
   partial class FormNormalWordbook
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNormalWordbook));
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.wordbookTree = new DrectSoft.Wordbook.UCtrlWordbookTree();
         this.tabControl1 = new System.Windows.Forms.TabControl();
         this.tabPageBasic = new System.Windows.Forms.TabPage();
         this.tBoxExtra = new System.Windows.Forms.TextBox();
         this.tBoxQuery = new System.Windows.Forms.TextBox();
         this.tBoxClassName = new System.Windows.Forms.TextBox();
         this.tBoxName = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.tabPageValue = new System.Windows.Forms.TabPage();
         this.label7 = new System.Windows.Forms.Label();
         this.tBoxDefValue = new System.Windows.Forms.TextBox();
         this.treeViewFields = new System.Windows.Forms.TreeView();
         this.btnDown = new System.Windows.Forms.Button();
         this.imageList1 = new System.Windows.Forms.ImageList(this.components);
         this.btnUp = new System.Windows.Forms.Button();
         this.dgvInputParas = new System.Windows.Forms.DataGridView();
         this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.ColIsString = new System.Windows.Forms.DataGridViewCheckBoxColumn();
         this.ColEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
         this.ColValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.ColDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.ColDataSort = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.ColFieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.ColSign = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.label6 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.tabPageShow = new System.Windows.Forms.TabPage();
         this.dataGridView2 = new System.Windows.Forms.DataGridView();
         this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.radioButton1 = new System.Windows.Forms.RadioButton();
         this.panel1 = new System.Windows.Forms.Panel();
         this.btnCancle = new System.Windows.Forms.Button();
         this.btnOk = new System.Windows.Forms.Button();
         this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         this.tabControl1.SuspendLayout();
         this.tabPageBasic.SuspendLayout();
         this.tabPageValue.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgvInputParas)).BeginInit();
         this.tabPageShow.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
         this.panel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // splitContainer1
         // 
         this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point(0, 0);
         this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
         this.splitContainer1.Name = "splitContainer1";
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.wordbookTree);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
         this.splitContainer1.Panel2.Controls.Add(this.panel1);
         this.splitContainer1.Size = new System.Drawing.Size(657, 312);
         this.splitContainer1.SplitterDistance = 233;
         this.splitContainer1.TabIndex = 0;
         this.splitContainer1.Text = "splitContainer1";
         // 
         // wordbookTree
         // 
         this.wordbookTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.wordbookTree.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
         this.wordbookTree.Location = new System.Drawing.Point(0, 0);
         this.wordbookTree.Name = "wordbookTree";
         this.wordbookTree.Size = new System.Drawing.Size(231, 310);
         this.wordbookTree.TabIndex = 0;
         this.wordbookTree.TreeDoubleClick += new System.EventHandler<System.EventArgs>(this.wordbookTree_TreeDoubleClick);
         // 
         // tabControl1
         // 
         this.tabControl1.Controls.Add(this.tabPageBasic);
         this.tabControl1.Controls.Add(this.tabPageValue);
         this.tabControl1.Controls.Add(this.tabPageShow);
         this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabControl1.Location = new System.Drawing.Point(0, 0);
         this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
         this.tabControl1.Multiline = true;
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.SelectedIndex = 0;
         this.tabControl1.Size = new System.Drawing.Size(418, 280);
         this.tabControl1.TabIndex = 23;
         // 
         // tabPageBasic
         // 
         this.tabPageBasic.AutoScroll = true;
         this.tabPageBasic.BackColor = System.Drawing.SystemColors.Control;
         this.tabPageBasic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tabPageBasic.Controls.Add(this.tBoxExtra);
         this.tabPageBasic.Controls.Add(this.tBoxQuery);
         this.tabPageBasic.Controls.Add(this.tBoxClassName);
         this.tabPageBasic.Controls.Add(this.tBoxName);
         this.tabPageBasic.Controls.Add(this.label4);
         this.tabPageBasic.Controls.Add(this.label3);
         this.tabPageBasic.Controls.Add(this.label2);
         this.tabPageBasic.Controls.Add(this.label1);
         this.tabPageBasic.Location = new System.Drawing.Point(4, 21);
         this.tabPageBasic.Margin = new System.Windows.Forms.Padding(0);
         this.tabPageBasic.Name = "tabPageBasic";
         this.tabPageBasic.Size = new System.Drawing.Size(410, 255);
         this.tabPageBasic.TabIndex = 0;
         this.tabPageBasic.Text = "基本信息";
         // 
         // tBoxExtra
         // 
         this.tBoxExtra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxExtra.Location = new System.Drawing.Point(64, 177);
         this.tBoxExtra.Multiline = true;
         this.tBoxExtra.Name = "tBoxExtra";
         this.tBoxExtra.ScrollBars = System.Windows.Forms.ScrollBars.Both;
         this.tBoxExtra.Size = new System.Drawing.Size(336, 73);
         this.tBoxExtra.TabIndex = 0;
         // 
         // tBoxQuery
         // 
         this.tBoxQuery.BackColor = System.Drawing.SystemColors.Info;
         this.tBoxQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxQuery.Location = new System.Drawing.Point(64, 26);
         this.tBoxQuery.Multiline = true;
         this.tBoxQuery.Name = "tBoxQuery";
         this.tBoxQuery.ReadOnly = true;
         this.tBoxQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
         this.tBoxQuery.Size = new System.Drawing.Size(336, 148);
         this.tBoxQuery.TabIndex = 3;
         // 
         // tBoxClassName
         // 
         this.tBoxClassName.BackColor = System.Drawing.SystemColors.Info;
         this.tBoxClassName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxClassName.Location = new System.Drawing.Point(228, 2);
         this.tBoxClassName.Name = "tBoxClassName";
         this.tBoxClassName.ReadOnly = true;
         this.tBoxClassName.Size = new System.Drawing.Size(156, 21);
         this.tBoxClassName.TabIndex = 2;
         // 
         // tBoxName
         // 
         this.tBoxName.BackColor = System.Drawing.SystemColors.Info;
         this.tBoxName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxName.Location = new System.Drawing.Point(64, 2);
         this.tBoxName.Name = "tBoxName";
         this.tBoxName.ReadOnly = true;
         this.tBoxName.Size = new System.Drawing.Size(124, 21);
         this.tBoxName.TabIndex = 1;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(6, 177);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(53, 12);
         this.label4.TabIndex = 25;
         this.label4.Text = "附加条件";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(6, 26);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(53, 12);
         this.label3.TabIndex = 24;
         this.label3.Text = "查询语句";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(6, 6);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(53, 12);
         this.label2.TabIndex = 23;
         this.label2.Text = "字典名称";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(194, 6);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(29, 12);
         this.label1.TabIndex = 22;
         this.label1.Text = "类名";
         // 
         // tabPageValue
         // 
         this.tabPageValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tabPageValue.Controls.Add(this.label7);
         this.tabPageValue.Controls.Add(this.tBoxDefValue);
         this.tabPageValue.Controls.Add(this.treeViewFields);
         this.tabPageValue.Controls.Add(this.btnDown);
         this.tabPageValue.Controls.Add(this.btnUp);
         this.tabPageValue.Controls.Add(this.dgvInputParas);
         this.tabPageValue.Controls.Add(this.label6);
         this.tabPageValue.Controls.Add(this.label5);
         this.tabPageValue.Location = new System.Drawing.Point(4, 21);
         this.tabPageValue.Margin = new System.Windows.Forms.Padding(0);
         this.tabPageValue.Name = "tabPageValue";
         this.tabPageValue.Size = new System.Drawing.Size(410, 255);
         this.tabPageValue.TabIndex = 1;
         this.tabPageValue.Text = "初始值";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(7, 227);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(53, 12);
         this.label7.TabIndex = 34;
         this.label7.Text = "新默认值";
         // 
         // tBoxDefValue
         // 
         this.tBoxDefValue.BackColor = System.Drawing.SystemColors.Info;
         this.tBoxDefValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tBoxDefValue.Location = new System.Drawing.Point(61, 225);
         this.tBoxDefValue.Name = "tBoxDefValue";
         this.tBoxDefValue.ReadOnly = true;
         this.tBoxDefValue.Size = new System.Drawing.Size(341, 21);
         this.tBoxDefValue.TabIndex = 33;
         // 
         // treeViewFields
         // 
         this.treeViewFields.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.treeViewFields.FullRowSelect = true;
         this.treeViewFields.HideSelection = false;
         this.treeViewFields.Indent = 16;
         this.treeViewFields.ItemHeight = 16;
         this.treeViewFields.Location = new System.Drawing.Point(61, 3);
         this.treeViewFields.Name = "treeViewFields";
         this.treeViewFields.ShowLines = false;
         this.treeViewFields.ShowNodeToolTips = true;
         this.treeViewFields.ShowRootLines = false;
         this.treeViewFields.Size = new System.Drawing.Size(303, 88);
         this.treeViewFields.TabIndex = 32;
         // 
         // btnDown
         // 
         this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
         this.btnDown.ImageKey = "ArrowDown";
         this.btnDown.ImageList = this.imageList1;
         this.btnDown.Location = new System.Drawing.Point(370, 56);
         this.btnDown.Name = "btnDown";
         this.btnDown.Size = new System.Drawing.Size(32, 32);
         this.btnDown.TabIndex = 2;
         this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
         // 
         // imageList1
         // 
         this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
         this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
         this.imageList1.Images.SetKeyName(0, "ArrowDown");
         this.imageList1.Images.SetKeyName(1, "ArrowUp");
         // 
         // btnUp
         // 
         this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
         this.btnUp.ImageKey = "ArrowUp";
         this.btnUp.ImageList = this.imageList1;
         this.btnUp.Location = new System.Drawing.Point(370, 7);
         this.btnUp.Name = "btnUp";
         this.btnUp.Size = new System.Drawing.Size(32, 32);
         this.btnUp.TabIndex = 1;
         this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
         // 
         // dgvInputParas
         // 
         this.dgvInputParas.AllowUserToAddRows = false;
         this.dgvInputParas.AllowUserToDeleteRows = false;
         this.dgvInputParas.BackgroundColor = System.Drawing.SystemColors.Info;
         this.dgvInputParas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
         this.dgvInputParas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
         this.dgvInputParas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColIsString,
            this.ColEnable,
            this.ColValue,
            this.ColDescription,
            this.ColDataSort,
            this.ColFieldName,
            this.ColSign});
         dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
         dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Info;
         dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
         dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
         dataGridViewCellStyle6.FormatProvider = new System.Globalization.CultureInfo("zh-CN");
         dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
         dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
         dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
         this.dgvInputParas.DefaultCellStyle = dataGridViewCellStyle6;
         this.dgvInputParas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
         this.dgvInputParas.Location = new System.Drawing.Point(61, 95);
         this.dgvInputParas.MultiSelect = false;
         this.dgvInputParas.Name = "dgvInputParas";
         this.dgvInputParas.RowHeadersWidth = 8;
         this.dgvInputParas.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
         this.dgvInputParas.RowTemplate.Height = 23;
         this.dgvInputParas.Size = new System.Drawing.Size(341, 127);
         this.dgvInputParas.TabIndex = 3;
         this.dgvInputParas.Text = "dataGridView1";
         this.dgvInputParas.Leave += new System.EventHandler(this.dgvInputParas_Leave);
         // 
         // ColName
         // 
         this.ColName.DataPropertyName = "Name";
         this.ColName.HeaderText = "名称";
         this.ColName.Name = "ColName";
         this.ColName.ReadOnly = true;
         this.ColName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
         this.ColName.Width = 70;
         // 
         // ColIsString
         // 
         this.ColIsString.DataPropertyName = "IsString";
         this.ColIsString.FalseValue = "false";
         this.ColIsString.HeaderText = "字符型";
         this.ColIsString.Name = "ColIsString";
         this.ColIsString.ReadOnly = true;
         this.ColIsString.TrueValue = "true";
         this.ColIsString.Width = 50;
         // 
         // ColEnable
         // 
         this.ColEnable.DataPropertyName = "Enable";
         dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
         dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
         dataGridViewCellStyle4.FormatProvider = new System.Globalization.CultureInfo("zh-CN");
         dataGridViewCellStyle4.NullValue = false;
         this.ColEnable.DefaultCellStyle = dataGridViewCellStyle4;
         this.ColEnable.HeaderText = "启用";
         this.ColEnable.Name = "ColEnable";
         this.ColEnable.Width = 40;
         // 
         // ColValue
         // 
         this.ColValue.DataPropertyName = "Value";
         dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
         dataGridViewCellStyle5.FormatProvider = new System.Globalization.CultureInfo("zh-CN");
         this.ColValue.DefaultCellStyle = dataGridViewCellStyle5;
         this.ColValue.HeaderText = "默认值";
         this.ColValue.Name = "ColValue";
         this.ColValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
         // 
         // ColDescription
         // 
         this.ColDescription.DataPropertyName = "Description";
         this.ColDescription.HeaderText = "描述";
         this.ColDescription.Name = "ColDescription";
         this.ColDescription.ReadOnly = true;
         // 
         // ColDataSort
         // 
         this.ColDataSort.DataPropertyName = "DataSort";
         this.ColDataSort.HeaderText = "数据类别";
         this.ColDataSort.Name = "ColDataSort";
         this.ColDataSort.ReadOnly = true;
         this.ColDataSort.Width = 70;
         // 
         // ColFieldName
         // 
         this.ColFieldName.DataPropertyName = "FieldName";
         this.ColFieldName.HeaderText = "对应字段";
         this.ColFieldName.Name = "ColFieldName";
         this.ColFieldName.ReadOnly = true;
         this.ColFieldName.Width = 70;
         // 
         // ColSign
         // 
         this.ColSign.DataPropertyName = "Sign";
         this.ColSign.HeaderText = "操作符";
         this.ColSign.Name = "ColSign";
         this.ColSign.ReadOnly = true;
         this.ColSign.Resizable = System.Windows.Forms.DataGridViewTriState.True;
         this.ColSign.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
         this.ColSign.Width = 60;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(7, 100);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(53, 12);
         this.label6.TabIndex = 31;
         this.label6.Text = "过滤参数";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(7, 7);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(53, 12);
         this.label5.TabIndex = 30;
         this.label5.Text = "代码字段";
         // 
         // tabPageShow
         // 
         this.tabPageShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.tabPageShow.Controls.Add(this.dataGridView2);
         this.tabPageShow.Controls.Add(this.radioButton1);
         this.tabPageShow.Location = new System.Drawing.Point(4, 21);
         this.tabPageShow.Margin = new System.Windows.Forms.Padding(0);
         this.tabPageShow.Name = "tabPageShow";
         this.tabPageShow.Size = new System.Drawing.Size(410, 255);
         this.tabPageShow.TabIndex = 2;
         this.tabPageShow.Text = "列显示方案";
         // 
         // dataGridView2
         // 
         this.dataGridView2.AllowUserToAddRows = false;
         this.dataGridView2.AllowUserToDeleteRows = false;
         this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
         this.dataGridView2.Location = new System.Drawing.Point(22, 2);
         this.dataGridView2.Name = "dataGridView2";
         this.dataGridView2.ReadOnly = true;
         this.dataGridView2.RowTemplate.Height = 23;
         this.dataGridView2.Size = new System.Drawing.Size(380, 42);
         this.dataGridView2.TabIndex = 1;
         this.dataGridView2.Text = "dataGridView2";
         // 
         // Column1
         // 
         this.Column1.Frozen = true;
         this.Column1.HeaderText = "列名1";
         this.Column1.Name = "Column1";
         this.Column1.ReadOnly = true;
         // 
         // Column2
         // 
         this.Column2.HeaderText = "Column2";
         this.Column2.Name = "Column2";
         this.Column2.ReadOnly = true;
         // 
         // radioButton1
         // 
         this.radioButton1.AutoSize = true;
         this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
         this.radioButton1.Location = new System.Drawing.Point(5, 13);
         this.radioButton1.Name = "radioButton1";
         this.radioButton1.Size = new System.Drawing.Size(13, 12);
         this.radioButton1.TabIndex = 0;
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.btnCancle);
         this.panel1.Controls.Add(this.btnOk);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panel1.Location = new System.Drawing.Point(0, 280);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(418, 30);
         this.panel1.TabIndex = 22;
         // 
         // btnCancle
         // 
         this.btnCancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
         this.btnCancle.Location = new System.Drawing.Point(274, 3);
         this.btnCancle.Name = "btnCancle";
         this.btnCancle.Size = new System.Drawing.Size(85, 25);
         this.btnCancle.TabIndex = 14;
         this.btnCancle.Text = "取消";
         // 
         // btnOk
         // 
         this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
         this.btnOk.Location = new System.Drawing.Point(71, 3);
         this.btnOk.Name = "btnOk";
         this.btnOk.Size = new System.Drawing.Size(85, 25);
         this.btnOk.TabIndex = 13;
         this.btnOk.Text = "确定";
         this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
         // 
         // toolTip1
         // 
         this.toolTip1.AutomaticDelay = 100;
         this.toolTip1.BackColor = System.Drawing.SystemColors.MenuHighlight;
         this.toolTip1.IsBalloon = true;
         this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
         // 
         // FormNormalWordbook
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(657, 312);
         this.Controls.Add(this.splitContainer1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.MinimumSize = new System.Drawing.Size(600, 337);
         this.Name = "FormNormalWordbook";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "选择并设置需要使用的代码字典";
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel2.ResumeLayout(false);
         this.splitContainer1.ResumeLayout(false);
         this.tabControl1.ResumeLayout(false);
         this.tabPageBasic.ResumeLayout(false);
         this.tabPageBasic.PerformLayout();
         this.tabPageValue.ResumeLayout(false);
         this.tabPageValue.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgvInputParas)).EndInit();
         this.tabPageShow.ResumeLayout(false);
         this.tabPageShow.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
         this.panel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.Button btnOk;
      private System.Windows.Forms.ImageList imageList1;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Button btnCancle;
      private System.Windows.Forms.TabControl tabControl1;
      private System.Windows.Forms.TabPage tabPageBasic;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TabPage tabPageValue;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TabPage tabPageShow;
      private System.Windows.Forms.TextBox tBoxExtra;
      private System.Windows.Forms.TextBox tBoxQuery;
      private System.Windows.Forms.TextBox tBoxClassName;
      private System.Windows.Forms.TextBox tBoxName;
      private System.Windows.Forms.Button btnDown;
      private System.Windows.Forms.Button btnUp;
      private System.Windows.Forms.DataGridView dgvInputParas;
      private System.Windows.Forms.DataGridView dataGridView2;
      private System.Windows.Forms.RadioButton radioButton1;
      private System.Windows.Forms.TreeView treeViewFields;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.TextBox tBoxDefValue;
      private System.Windows.Forms.ToolTip toolTip1;
      private UCtrlWordbookTree wordbookTree;
      private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
      private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsString;
      private System.Windows.Forms.DataGridViewCheckBoxColumn ColEnable;
      private System.Windows.Forms.DataGridViewTextBoxColumn ColValue;
      private System.Windows.Forms.DataGridViewTextBoxColumn ColDescription;
      private System.Windows.Forms.DataGridViewTextBoxColumn ColDataSort;
      private System.Windows.Forms.DataGridViewTextBoxColumn ColFieldName;
      private System.Windows.Forms.DataGridViewTextBoxColumn ColSign;
      private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
      private System.Windows.Forms.DataGridViewTextBoxColumn Column2;

   }
}