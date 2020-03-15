namespace DrectSoft.MainFrame.Login
{
    partial class ConnectionConfig
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
            this.teip = new DevExpress.XtraEditors.TextEdit();
            this.teport = new DevExpress.XtraEditors.TextEdit();
            this.tecase = new DevExpress.XtraEditors.TextEdit();
            this.teusername = new DevExpress.XtraEditors.TextEdit();
            this.tepassword = new DevExpress.XtraEditors.TextEdit();
            this.lbconnection = new DevExpress.XtraEditors.LabelControl();
            this.sbsave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.teip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teport.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tecase.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teusername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tepassword.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // teip
            // 
            this.teip.EditValue = "192.168.1.254";
            this.teip.Location = new System.Drawing.Point(109, 47);
            this.teip.Name = "teip";
            this.teip.Properties.NullValuePrompt = "IP地址";
            this.teip.Properties.NullValuePromptShowForEmptyValue = true;
            this.teip.Size = new System.Drawing.Size(168, 20);
            this.teip.TabIndex = 0;
            this.teip.Tag = "";
            // 
            // teport
            // 
            this.teport.EditValue = "1521";
            this.teport.Location = new System.Drawing.Point(109, 85);
            this.teport.Name = "teport";
            this.teport.Size = new System.Drawing.Size(168, 20);
            this.teport.TabIndex = 1;
            this.teport.Tag = "";
            // 
            // tecase
            // 
            this.tecase.EditValue = "orcl";
            this.tecase.Location = new System.Drawing.Point(109, 124);
            this.tecase.Name = "tecase";
            this.tecase.Properties.NullValuePrompt = "实例名称";
            this.tecase.Properties.NullValuePromptShowForEmptyValue = true;
            this.tecase.Size = new System.Drawing.Size(168, 20);
            this.tecase.TabIndex = 2;
            this.tecase.Tag = "";
            // 
            // teusername
            // 
            this.teusername.EditValue = "emr";
            this.teusername.Location = new System.Drawing.Point(109, 161);
            this.teusername.Name = "teusername";
            this.teusername.Properties.NullValuePrompt = "用户";
            this.teusername.Properties.NullValuePromptShowForEmptyValue = true;
            this.teusername.Size = new System.Drawing.Size(168, 20);
            this.teusername.TabIndex = 3;
            this.teusername.Tag = "";
            // 
            // tepassword
            // 
            this.tepassword.EditValue = "emr";
            this.tepassword.Location = new System.Drawing.Point(109, 196);
            this.tepassword.Name = "tepassword";
            this.tepassword.Properties.NullValuePrompt = "密码";
            this.tepassword.Properties.NullValuePromptShowForEmptyValue = true;
            this.tepassword.Size = new System.Drawing.Size(168, 20);
            this.tepassword.TabIndex = 4;
            this.tepassword.Tag = "";
            // 
            // lbconnection
            // 
            this.lbconnection.Location = new System.Drawing.Point(153, 12);
            this.lbconnection.Name = "lbconnection";
            this.lbconnection.Size = new System.Drawing.Size(72, 14);
            this.lbconnection.TabIndex = 5;
            this.lbconnection.Text = "连接地址配置";
            // 
            // sbsave
            // 
            this.sbsave.Location = new System.Drawing.Point(150, 243);
            this.sbsave.Name = "sbsave";
            this.sbsave.Size = new System.Drawing.Size(75, 23);
            this.sbsave.TabIndex = 6;
            this.sbsave.Text = "保存";
            this.sbsave.Click += new System.EventHandler(this.sbsave_Click);
            // 
            // ConnectionConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 278);
            this.Controls.Add(this.sbsave);
            this.Controls.Add(this.lbconnection);
            this.Controls.Add(this.tepassword);
            this.Controls.Add(this.teusername);
            this.Controls.Add(this.tecase);
            this.Controls.Add(this.teport);
            this.Controls.Add(this.teip);
            this.Name = "ConnectionConfig";
            this.Text = "地址配置";
            ((System.ComponentModel.ISupportInitialize)(this.teip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teport.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tecase.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teusername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tepassword.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit teip;
        private DevExpress.XtraEditors.TextEdit teport;
        private DevExpress.XtraEditors.TextEdit tecase;
        private DevExpress.XtraEditors.TextEdit teusername;
        private DevExpress.XtraEditors.TextEdit tepassword;
        private DevExpress.XtraEditors.LabelControl lbconnection;
        private DevExpress.XtraEditors.SimpleButton sbsave;
    }
}