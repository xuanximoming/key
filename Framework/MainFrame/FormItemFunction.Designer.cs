namespace DrectSoft.MainFrame
{
    partial class FormItemFunction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormItemFunction));
            this.panel1 = new DevExpress.XtraEditors.PanelControl();
            this.lookUpEditorDepart = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowDepart = new DrectSoft.Common.Library.LookUpWindow(this.components);
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.buttonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.buttonOK = new DevExpress.XtraEditors.SimpleButton();
            this.labelChangeWard = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepart)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panel1.Controls.Add(this.lookUpEditorDepart);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.LookAndFeel.SkinName = "Blue";
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(333, 138);
            this.panel1.TabIndex = 0;
            // 
            // lookUpEditorDepart
            // 
            this.lookUpEditorDepart.EnterMoveNextControl = true;
            this.lookUpEditorDepart.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDepart.ListWindow = this.lookUpWindowDepart;
            this.lookUpEditorDepart.Location = new System.Drawing.Point(82, 42);
            this.lookUpEditorDepart.Name = "lookUpEditorDepart";
            this.lookUpEditorDepart.ShowSButton = true;
            this.lookUpEditorDepart.Size = new System.Drawing.Size(210, 18);
            this.lookUpEditorDepart.TabIndex = 0;
            this.lookUpEditorDepart.ToolTip = "支持科室编码、科室名称(汉字/拼音/五笔)检索";
            // 
            // lookUpWindowDepart
            // 
            this.lookUpWindowDepart.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDepart.GenShortCode = null;
            this.lookUpWindowDepart.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDepart.Owner = null;
            this.lookUpWindowDepart.SqlHelper = null;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 44);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "科室名称：";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonCancel.Appearance.Options.UseFont = true;
            this.buttonCancel.Image = global::DrectSoft.MainFrame.Properties.Resources.取消;
            this.buttonCancel.Location = new System.Drawing.Point(212, 85);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 27);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "取消 (&C)";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOK.Appearance.Options.UseFont = true;
            this.buttonOK.Image = global::DrectSoft.MainFrame.Properties.Resources.确定;
            this.buttonOK.Location = new System.Drawing.Point(126, 85);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(80, 27);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "确定 (&Y)";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelChangeWard
            // 
            this.labelChangeWard.AutoSize = true;
            this.labelChangeWard.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelChangeWard.Location = new System.Drawing.Point(39, 45);
            this.labelChangeWard.Name = "labelChangeWard";
            this.labelChangeWard.Size = new System.Drawing.Size(63, 14);
            this.labelChangeWard.TabIndex = 19;
            this.labelChangeWard.Text = "切换病区";
            this.labelChangeWard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormItemFunction
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 138);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "Blue";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormItemFunction";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "切换科室";
            this.Load += new System.EventHandler(this.FormItemFunction_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panel1;

        private System.Windows.Forms.Label labelChangeWard;
        private DevExpress.XtraEditors.SimpleButton buttonCancel;
        private DevExpress.XtraEditors.SimpleButton buttonOK;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private Common.Library.LookUpEditor lookUpEditorDepart;
        private Common.Library.LookUpWindow lookUpWindowDepart;
    }
}