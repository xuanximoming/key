namespace DrectSoft.Basic.Paint.NET
{
    partial class NamedStyleSettingForm
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
            this.cmbHatch = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.lstNamedHatch = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.rbHatch = new System.Windows.Forms.RadioButton();
            this.rbTexture = new System.Windows.Forms.RadioButton();
            this.imgTexture = new System.Windows.Forms.PictureBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgTexture)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbHatch
            // 
            this.cmbHatch.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbHatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHatch.FormattingEnabled = true;
            this.cmbHatch.Location = new System.Drawing.Point(113, 12);
            this.cmbHatch.Name = "cmbHatch";
            this.cmbHatch.Size = new System.Drawing.Size(127, 22);
            this.cmbHatch.TabIndex = 1;
            this.cmbHatch.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbHatch_DrawItem);
            this.cmbHatch.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.cmbHatch_MeasureItem);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(113, 77);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(127, 21);
            this.txtName.TabIndex = 5;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(58, 104);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(139, 104);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 7;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // lstNamedHatch
            // 
            this.lstNamedHatch.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstNamedHatch.FormattingEnabled = true;
            this.lstNamedHatch.ItemHeight = 12;
            this.lstNamedHatch.Location = new System.Drawing.Point(12, 131);
            this.lstNamedHatch.Name = "lstNamedHatch";
            this.lstNamedHatch.Size = new System.Drawing.Size(227, 172);
            this.lstNamedHatch.TabIndex = 8;
            this.lstNamedHatch.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstNamedHatch_DrawItem);
            this.lstNamedHatch.SelectedIndexChanged += new System.EventHandler(this.lstNamedHatch_SelectedIndexChanged);
            this.lstNamedHatch.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lstNamedHatch_MeasureItem);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(164, 309);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // rbHatch
            // 
            this.rbHatch.AutoSize = true;
            this.rbHatch.Checked = true;
            this.rbHatch.Location = new System.Drawing.Point(12, 13);
            this.rbHatch.Name = "rbHatch";
            this.rbHatch.Size = new System.Drawing.Size(83, 16);
            this.rbHatch.TabIndex = 0;
            this.rbHatch.TabStop = true;
            this.rbHatch.Text = "预定义样式";
            this.rbHatch.UseVisualStyleBackColor = true;
            this.rbHatch.CheckedChanged += new System.EventHandler(this.rbHatch_CheckedChanged);
            // 
            // rbTexture
            // 
            this.rbTexture.AutoSize = true;
            this.rbTexture.Location = new System.Drawing.Point(12, 46);
            this.rbTexture.Name = "rbTexture";
            this.rbTexture.Size = new System.Drawing.Size(83, 16);
            this.rbTexture.TabIndex = 2;
            this.rbTexture.Text = "自定义样式";
            this.rbTexture.UseVisualStyleBackColor = true;
            this.rbTexture.CheckedChanged += new System.EventHandler(this.rbTexture_CheckedChanged);
            // 
            // imgTexture
            // 
            this.imgTexture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.imgTexture.Location = new System.Drawing.Point(113, 44);
            this.imgTexture.Name = "imgTexture";
            this.imgTexture.Size = new System.Drawing.Size(24, 24);
            this.imgTexture.TabIndex = 8;
            this.imgTexture.TabStop = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(164, 43);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "样式名称";
            // 
            // NamedHatchStyleSettingForm
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 344);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.imgTexture);
            this.Controls.Add(this.rbTexture);
            this.Controls.Add(this.rbHatch);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lstNamedHatch);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cmbHatch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NamedHatchStyleSettingForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "命名刷子样式";
            ((System.ComponentModel.ISupportInitialize)(this.imgTexture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbHatch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.ListBox lstNamedHatch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RadioButton rbHatch;
        private System.Windows.Forms.RadioButton rbTexture;
        private System.Windows.Forms.PictureBox imgTexture;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
    }
}