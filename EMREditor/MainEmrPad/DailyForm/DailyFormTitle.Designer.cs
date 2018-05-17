namespace DrectSoft.Core.MainEmrPad.DailyForm
{
    partial class DailyFormTitle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DailyFormTitle));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.textEditTitleName = new DevExpress.XtraEditors.TextEdit();
            this.simpleButtonOK = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.simpleButtonCancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.textEditTitleName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "病程标题：";
            // 
            // textEditTitleName
            // 
            this.textEditTitleName.EnterMoveNextControl = true;
            this.textEditTitleName.Location = new System.Drawing.Point(82, 15);
            this.textEditTitleName.Name = "textEditTitleName";
            this.textEditTitleName.Size = new System.Drawing.Size(200, 20);
            this.textEditTitleName.TabIndex = 0;
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonOK.Image")));
            this.simpleButtonOK.Location = new System.Drawing.Point(115, 48);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonOK.TabIndex = 1;
            this.simpleButtonOK.Text = "确定(&Y)";
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButtonOK_Click);
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonCancel.Image")));
            this.simpleButtonCancel.Location = new System.Drawing.Point(202, 48);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonCancel.TabIndex = 2;
            this.simpleButtonCancel.Text = "取消(&C)";
            this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
            // 
            // DailyFormTitle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 86);
            this.Controls.Add(this.simpleButtonCancel);
            this.Controls.Add(this.simpleButtonOK);
            this.Controls.Add(this.textEditTitleName);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DailyFormTitle";
            this.Text = "修改";
            ((System.ComponentModel.ISupportInitialize)(this.textEditTitleName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textEditTitleName;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK simpleButtonOK;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel simpleButtonCancel;
    }
}