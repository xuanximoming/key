namespace DrectSoft.Emr.TemplateFactory
{
    partial class SearchFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchFrm));
            this.cmdReplace = new System.Windows.Forms.Button();
            this.cmdSearch1 = new System.Windows.Forms.Button();
            this.txtSearch1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Search = new System.Windows.Forms.TabPage();
            this.Replace = new System.Windows.Forms.TabPage();
            this.cmdSearch2 = new System.Windows.Forms.Button();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearch2 = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.Search.SuspendLayout();
            this.Replace.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdReplace
            // 
            this.cmdReplace.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdReplace.Location = new System.Drawing.Point(219, 153);
            this.cmdReplace.Name = "cmdReplace";
            this.cmdReplace.Size = new System.Drawing.Size(93, 27);
            this.cmdReplace.TabIndex = 3;
            this.cmdReplace.Text = "替换 (&R)";
            this.cmdReplace.Click += new System.EventHandler(this.cmdReplace_Click);
            // 
            // cmdSearch1
            // 
            this.cmdSearch1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdSearch1.Location = new System.Drawing.Point(197, 132);
            this.cmdSearch1.Name = "cmdSearch1";
            this.cmdSearch1.Size = new System.Drawing.Size(117, 27);
            this.cmdSearch1.TabIndex = 1;
            this.cmdSearch1.Text = "查找下一个 (&N)";
            this.cmdSearch1.Click += new System.EventHandler(this.cmdSearch1_Click);
            // 
            // txtSearch1
            // 
            this.txtSearch1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtSearch1.Location = new System.Drawing.Point(12, 71);
            this.txtSearch1.Name = "txtSearch1";
            this.txtSearch1.Size = new System.Drawing.Size(301, 22);
            this.txtSearch1.TabIndex = 0;
            this.txtSearch1.TextChanged += new System.EventHandler(this.txtSearch1_TextChanged);
            this.txtSearch1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch1_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(9, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "查找内容：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Search);
            this.tabControl1.Controls.Add(this.Replace);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(339, 236);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // Search
            // 
            this.Search.Controls.Add(this.label1);
            this.Search.Controls.Add(this.cmdSearch1);
            this.Search.Controls.Add(this.txtSearch1);
            this.Search.Location = new System.Drawing.Point(4, 23);
            this.Search.Name = "Search";
            this.Search.Padding = new System.Windows.Forms.Padding(3);
            this.Search.Size = new System.Drawing.Size(331, 209);
            this.Search.TabIndex = 0;
            this.Search.Text = "查找";
            this.Search.UseVisualStyleBackColor = true;
            // 
            // Replace
            // 
            this.Replace.Controls.Add(this.cmdSearch2);
            this.Replace.Controls.Add(this.cmdReplace);
            this.Replace.Controls.Add(this.txtReplace);
            this.Replace.Controls.Add(this.label3);
            this.Replace.Controls.Add(this.label2);
            this.Replace.Controls.Add(this.txtSearch2);
            this.Replace.Location = new System.Drawing.Point(4, 23);
            this.Replace.Name = "Replace";
            this.Replace.Padding = new System.Windows.Forms.Padding(3);
            this.Replace.Size = new System.Drawing.Size(331, 209);
            this.Replace.TabIndex = 1;
            this.Replace.Text = "替换";
            this.Replace.UseVisualStyleBackColor = true;
            // 
            // cmdSearch2
            // 
            this.cmdSearch2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdSearch2.Location = new System.Drawing.Point(96, 153);
            this.cmdSearch2.Name = "cmdSearch2";
            this.cmdSearch2.Size = new System.Drawing.Size(117, 27);
            this.cmdSearch2.TabIndex = 2;
            this.cmdSearch2.Text = "查找下一个 (&N)";
            this.cmdSearch2.Click += new System.EventHandler(this.cmdSearch2_Click);
            // 
            // txtReplace
            // 
            this.txtReplace.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtReplace.Location = new System.Drawing.Point(9, 108);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(303, 22);
            this.txtReplace.TabIndex = 1;
            this.txtReplace.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtReplace_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(7, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "替换为：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(7, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "查找内容：";
            // 
            // txtSearch2
            // 
            this.txtSearch2.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtSearch2.Location = new System.Drawing.Point(9, 52);
            this.txtSearch2.Name = "txtSearch2";
            this.txtSearch2.Size = new System.Drawing.Size(303, 22);
            this.txtSearch2.TabIndex = 0;
            this.txtSearch2.TextChanged += new System.EventHandler(this.txtSearch2_TextChanged);
            this.txtSearch2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmdSearch2_KeyDown);
            // 
            // SearchFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 236);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "查找和替换";
            this.Load += new System.EventHandler(this.Search_Load);
            this.tabControl1.ResumeLayout(false);
            this.Search.ResumeLayout(false);
            this.Search.PerformLayout();
            this.Replace.ResumeLayout(false);
            this.Replace.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdReplace;
        private System.Windows.Forms.Button cmdSearch1;
        private System.Windows.Forms.TextBox txtSearch1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Search;
        private System.Windows.Forms.TabPage Replace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSearch2;
        private System.Windows.Forms.Button cmdSearch2;
        private System.Windows.Forms.TextBox txtReplace;
    }
}