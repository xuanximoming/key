namespace DrectSoft.Core.NurseDocument.Controls
{
    partial class CombItems
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CombItems));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DevButtonCancel1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.DevButtonOK1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "青霉素(阴性)",
            "青霉素(阳性)",
            "头孢哌酮舒巴坦钠(阴性)",
            "头孢哌酮舒巴坦钠(阳性)",
            "头孢拉啶(阴性)",
            "头孢拉啶(阳性)"});
            this.comboBox1.Location = new System.Drawing.Point(91, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(162, 22);
            this.comboBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "皮试项目：";
            // 
            // DevButtonCancel1
            // 
            this.DevButtonCancel1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonCancel1.Image")));
            this.DevButtonCancel1.Location = new System.Drawing.Point(155, 47);
            this.DevButtonCancel1.Name = "DevButtonCancel1";
            this.DevButtonCancel1.Size = new System.Drawing.Size(80, 23);
            this.DevButtonCancel1.TabIndex = 2;
            this.DevButtonCancel1.Text = "取消(&C)";
            this.DevButtonCancel1.Click += new System.EventHandler(this.DevButtonCancel1_Click);
            // 
            // DevButtonOK1
            // 
            this.DevButtonOK1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonOK1.Image")));
            this.DevButtonOK1.Location = new System.Drawing.Point(47, 47);
            this.DevButtonOK1.Name = "DevButtonOK1";
            this.DevButtonOK1.Size = new System.Drawing.Size(80, 23);
            this.DevButtonOK1.TabIndex = 1;
            this.DevButtonOK1.Text = "确定(&Y)";
            this.DevButtonOK1.Click += new System.EventHandler(this.DevButtonOK1_Click);
            // 
            // CombItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 80);
            this.Controls.Add(this.DevButtonOK1);
            this.Controls.Add(this.DevButtonCancel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CombItems";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "皮试";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel DevButtonCancel1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK DevButtonOK1;
    }
}