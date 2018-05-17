namespace DrectSoft.Emr.TemplateFactory
{
    partial class TableInsert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableInsert));
            this.label1 = new System.Windows.Forms.Label();
            this.tbHeader = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudRow = new System.Windows.Forms.NumericUpDown();
            this.nudColumn = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.rbFix = new System.Windows.Forms.RadioButton();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel();
            this.btnOK = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK();
            ((System.ComponentModel.ISupportInitialize)(this.nudRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 6;
            this.label1.Text = "名称：";
            // 
            // tbHeader
            // 
            this.tbHeader.Location = new System.Drawing.Point(71, 12);
            this.tbHeader.Name = "tbHeader";
            this.tbHeader.Size = new System.Drawing.Size(240, 22);
            this.tbHeader.TabIndex = 0;
            this.tbHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.win_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "表格大小";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(80, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 2);
            this.label3.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 14);
            this.label4.TabIndex = 9;
            this.label4.Text = "行：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 14);
            this.label5.TabIndex = 10;
            this.label5.Text = "列：";
            // 
            // nudRow
            // 
            this.nudRow.Location = new System.Drawing.Point(71, 69);
            this.nudRow.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudRow.Name = "nudRow";
            this.nudRow.Size = new System.Drawing.Size(140, 22);
            this.nudRow.TabIndex = 1;
            this.nudRow.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudRow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.win_KeyPress);
            // 
            // nudColumn
            // 
            this.nudColumn.Location = new System.Drawing.Point(71, 100);
            this.nudColumn.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudColumn.Name = "nudColumn";
            this.nudColumn.Size = new System.Drawing.Size(140, 22);
            this.nudColumn.TabIndex = 2;
            this.nudColumn.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudColumn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.win_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 14);
            this.label6.TabIndex = 11;
            this.label6.Text = "＂自动调整＂操作";
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(146, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(168, 2);
            this.label7.TabIndex = 12;
            this.label7.Text = "label7";
            // 
            // rbAuto
            // 
            this.rbAuto.AutoSize = true;
            this.rbAuto.Checked = true;
            this.rbAuto.Location = new System.Drawing.Point(33, 178);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new System.Drawing.Size(73, 18);
            this.rbAuto.TabIndex = 13;
            this.rbAuto.TabStop = true;
            this.rbAuto.Text = "自动列宽";
            this.rbAuto.UseVisualStyleBackColor = true;
            this.rbAuto.CheckedChanged += new System.EventHandler(this.rbAuto_CheckedChanged);
            // 
            // rbFix
            // 
            this.rbFix.AutoSize = true;
            this.rbFix.Location = new System.Drawing.Point(33, 205);
            this.rbFix.Name = "rbFix";
            this.rbFix.Size = new System.Drawing.Size(73, 18);
            this.rbFix.TabIndex = 14;
            this.rbFix.Text = "固定列宽";
            this.rbFix.UseVisualStyleBackColor = true;
            this.rbFix.CheckedChanged += new System.EventHandler(this.rbFix_CheckedChanged);
            // 
            // nudWidth
            // 
            this.nudWidth.DecimalPlaces = 1;
            this.nudWidth.Enabled = false;
            this.nudWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudWidth.Location = new System.Drawing.Point(122, 199);
            this.nudWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(140, 22);
            this.nudWidth.TabIndex = 3;
            this.nudWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.win_KeyPress);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Location = new System.Drawing.Point(16, 241);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(297, 2);
            this.label8.TabIndex = 16;
            this.label8.Text = "label8";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(269, 208);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 14);
            this.label9.TabIndex = 15;
            this.label9.Text = "厘米";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(220, 261);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(93, 31);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.Location = new System.Drawing.Point(120, 261);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(93, 31);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定(&Y)";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // TableInsert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 310);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.nudWidth);
            this.Controls.Add(this.rbFix);
            this.Controls.Add(this.rbAuto);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudColumn);
            this.Controls.Add(this.nudRow);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbHeader);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TableInsert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "插入表格";
            ((System.ComponentModel.ISupportInitialize)(this.nudRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudRow;
        private System.Windows.Forms.NumericUpDown nudColumn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton rbFix;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.Label label9;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel btnCancel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK btnOK;
    }
}