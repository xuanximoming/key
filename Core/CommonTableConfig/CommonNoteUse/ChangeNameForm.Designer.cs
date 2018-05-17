namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    partial class ChangeNameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeNameForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtInCommonName = new DevExpress.XtraEditors.TextEdit();
            this.DevButtonOK1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.DevButtonClose1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtInCommonName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "单据名称：";
            // 
            // txtInCommonName
            // 
            this.txtInCommonName.Location = new System.Drawing.Point(74, 9);
            this.txtInCommonName.Name = "txtInCommonName";
            this.txtInCommonName.Properties.MaxLength = 50;
            this.txtInCommonName.Size = new System.Drawing.Size(230, 20);
            this.txtInCommonName.TabIndex = 1;
            // 
            // DevButtonOK1
            // 
            this.DevButtonOK1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonOK1.Image")));
            this.DevButtonOK1.Location = new System.Drawing.Point(136, 38);
            this.DevButtonOK1.Name = "DevButtonOK1";
            this.DevButtonOK1.Size = new System.Drawing.Size(80, 23);
            this.DevButtonOK1.TabIndex = 2;
            this.DevButtonOK1.Text = "确定(&Y)";
            this.DevButtonOK1.Click += new System.EventHandler(this.DevButtonOK1_Click);
            // 
            // DevButtonClose1
            // 
            this.DevButtonClose1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonClose1.Image")));
            this.DevButtonClose1.Location = new System.Drawing.Point(224, 38);
            this.DevButtonClose1.Name = "DevButtonClose1";
            this.DevButtonClose1.Size = new System.Drawing.Size(80, 23);
            this.DevButtonClose1.TabIndex = 3;
            this.DevButtonClose1.Text = "关闭(&T)";
            this.DevButtonClose1.Click += new System.EventHandler(this.DevButtonClose1_Click);
            // 
            // ChangeNameForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(316, 75);
            this.Controls.Add(this.DevButtonClose1);
            this.Controls.Add(this.DevButtonOK1);
            this.Controls.Add(this.txtInCommonName);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeNameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "修改单据名称";
            ((System.ComponentModel.ISupportInitialize)(this.txtInCommonName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtInCommonName;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK DevButtonOK1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClose DevButtonClose1;
    }
}