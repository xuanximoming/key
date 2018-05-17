namespace DrectSoft.Core.MainEmrPad
{
    partial class ChangeMacroFrom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeMacroFrom));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDept = new System.Windows.Forms.TextBox();
            this.textBoxWard = new System.Windows.Forms.TextBox();
            this.buttonCancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.buttonOK = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "科室：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "病区：";
            // 
            // textBoxDept
            // 
            this.textBoxDept.Location = new System.Drawing.Point(65, 15);
            this.textBoxDept.Name = "textBoxDept";
            this.textBoxDept.Size = new System.Drawing.Size(150, 22);
            this.textBoxDept.TabIndex = 0;
            this.textBoxDept.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Win_KeyPress);
            // 
            // textBoxWard
            // 
            this.textBoxWard.Location = new System.Drawing.Point(65, 48);
            this.textBoxWard.Name = "textBoxWard";
            this.textBoxWard.Size = new System.Drawing.Size(150, 22);
            this.textBoxWard.TabIndex = 1;
            this.textBoxWard.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Win_KeyPress);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
            this.buttonCancel.Location = new System.Drawing.Point(135, 84);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 27);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "取消(&C)";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
            this.buttonOK.Location = new System.Drawing.Point(49, 84);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(80, 27);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "确定(&Y)";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // ChangeMacroFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 123);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxWard);
            this.Controls.Add(this.textBoxDept);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeMacroFrom";
            this.Text = "更改常用词";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxDept;
        private System.Windows.Forms.TextBox textBoxWard;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel buttonCancel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK buttonOK;
    }
}