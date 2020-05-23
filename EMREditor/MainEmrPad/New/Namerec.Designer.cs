namespace Drectsoft.Core.MainEmrPad.New
{
    partial class Namerec
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Namerec));
            this.textEdit_Name = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btn_Save = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.btn_Cancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Name.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textEdit_Name
            // 
            this.textEdit_Name.EnterMoveNextControl = true;
            this.textEdit_Name.Location = new System.Drawing.Point(124, 45);
            this.textEdit_Name.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textEdit_Name.Name = "textEdit_Name";
            this.textEdit_Name.Size = new System.Drawing.Size(333, 28);
            this.textEdit_Name.TabIndex = 4;
            this.textEdit_Name.ToolTip = "病程标题";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(24, 47);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 22);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "病程名称：";
            // 
            // btn_Save
            // 
            this.btn_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Save.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.Image")));
            this.btn_Save.Location = new System.Drawing.Point(59, 120);
            this.btn_Save.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(114, 42);
            this.btn_Save.TabIndex = 6;
            this.btn_Save.Text = "确定(&Y)";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cancel.Image")));
            this.btn_Cancel.Location = new System.Drawing.Point(301, 120);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(114, 42);
            this.btn_Cancel.TabIndex = 7;
            this.btn_Cancel.Text = "取消(&C)";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // Namerec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 197);
            this.ControlBox = false;
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.textEdit_Name);
            this.Controls.Add(this.labelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Namerec";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "病程名称";
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Name.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textEdit_Name;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK btn_Save;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel btn_Cancel;
    }
}