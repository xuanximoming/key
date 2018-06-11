namespace DrectSoft.MainFrame.Login
{
    partial class FormReg
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
            this.IP = new DevExpress.XtraEditors.LabelControl();
            this.IPInput = new DevExpress.XtraEditors.TextEdit();
            this.LabDelIp = new DevExpress.XtraEditors.LabelControl();
            this.SBOK = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonDel = new DevExpress.XtraEditors.SimpleButton();
            this.lookUpEditorIP = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowIp = new DrectSoft.Common.Library.LookUpWindow(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.IPInput.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorIP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowIp)).BeginInit();
            this.SuspendLayout();
            // 
            // IP
            // 
            this.IP.Location = new System.Drawing.Point(27, 53);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(47, 14);
            this.IP.TabIndex = 0;
            this.IP.Text = "本机IP：";
            // 
            // IPInput
            // 
            this.IPInput.Enabled = false;
            this.IPInput.Location = new System.Drawing.Point(77, 50);
            this.IPInput.Name = "IPInput";
            this.IPInput.Size = new System.Drawing.Size(163, 20);
            this.IPInput.TabIndex = 1;
            // 
            // LabDelIp
            // 
            this.LabDelIp.Location = new System.Drawing.Point(9, 115);
            this.LabDelIp.Name = "LabDelIp";
            this.LabDelIp.Size = new System.Drawing.Size(71, 14);
            this.LabDelIp.TabIndex = 5;
            this.LabDelIp.Text = "已经注册IP：";
            // 
            // SBOK
            // 
            this.SBOK.Image = global::DrectSoft.MainFrame.Properties.Resources.新增;
            this.SBOK.Location = new System.Drawing.Point(261, 44);
            this.SBOK.Name = "SBOK";
            this.SBOK.Size = new System.Drawing.Size(75, 32);
            this.SBOK.TabIndex = 6;
            this.SBOK.Text = "添加";
            this.SBOK.Click += new System.EventHandler(this.SBOK_Click);
            // 
            // ButtonDel
            // 
            this.ButtonDel.Image = global::DrectSoft.MainFrame.Properties.Resources.删除;
            this.ButtonDel.Location = new System.Drawing.Point(261, 106);
            this.ButtonDel.Name = "ButtonDel";
            this.ButtonDel.Size = new System.Drawing.Size(75, 32);
            this.ButtonDel.TabIndex = 7;
            this.ButtonDel.Text = "删除";
            this.ButtonDel.Click += new System.EventHandler(this.ButtonDel_Click);
            // 
            // lookUpEditorIP
            // 
            this.lookUpEditorIP.EnterMoveNextControl = true;
            this.lookUpEditorIP.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorIP.ListWindow = this.lookUpWindowIp;
            this.lookUpEditorIP.Location = new System.Drawing.Point(80, 114);
            this.lookUpEditorIP.Name = "lookUpEditorIP";
            this.lookUpEditorIP.ShowSButton = true;
            this.lookUpEditorIP.Size = new System.Drawing.Size(160, 18);
            this.lookUpEditorIP.TabIndex = 4;
            this.lookUpEditorIP.ToolTip = "IP";
            // 
            // lookUpWindowIp
            // 
            this.lookUpWindowIp.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowIp.GenShortCode = null;
            this.lookUpWindowIp.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowIp.Owner = null;
            this.lookUpWindowIp.SqlHelper = null;
            // 
            // FormReg
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 203);
            this.Controls.Add(this.ButtonDel);
            this.Controls.Add(this.SBOK);
            this.Controls.Add(this.LabDelIp);
            this.Controls.Add(this.lookUpEditorIP);
            this.Controls.Add(this.IPInput);
            this.Controls.Add(this.IP);
            this.Name = "FormReg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormReg";
            ((System.ComponentModel.ISupportInitialize)(this.IPInput.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorIP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowIp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl IP;
        private DevExpress.XtraEditors.TextEdit IPInput;
        private Common.Library.LookUpWindow lookUpWindowIp;
        private Common.Library.LookUpEditor lookUpEditorIP;
        private DevExpress.XtraEditors.LabelControl LabDelIp;
        private DevExpress.XtraEditors.SimpleButton SBOK;
        private DevExpress.XtraEditors.SimpleButton ButtonDel;
    }
}