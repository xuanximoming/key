namespace DrectSoft.Emr.TemplateFactory
{
    partial class AddNewTemplet_Header
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewTemplet_Header));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditorHospitel = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowHospitel = new DrectSoft.Common.Library.LookUpWindow();
            this.btnSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave();
            this.btnClear = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorHospitel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowHospitel)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(44, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "使用医院：";
            // 
            // txtName
            // 
            this.txtName.EnterMoveNextControl = true;
            this.txtName.Location = new System.Drawing.Point(104, 73);
            this.txtName.Name = "txtName";
            this.txtName.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txtName.Size = new System.Drawing.Size(180, 20);
            this.txtName.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(44, 76);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "页眉名称：";
            // 
            // lookUpEditorHospitel
            // 
            this.lookUpEditorHospitel.EnterMoveNextControl = true;
            this.lookUpEditorHospitel.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorHospitel.ListWindow = this.lookUpWindowHospitel;
            this.lookUpEditorHospitel.Location = new System.Drawing.Point(104, 32);
            this.lookUpEditorHospitel.Name = "lookUpEditorHospitel";
            this.lookUpEditorHospitel.ShowFormImmediately = true;
            this.lookUpEditorHospitel.ShowSButton = true;
            this.lookUpEditorHospitel.Size = new System.Drawing.Size(180, 18);
            this.lookUpEditorHospitel.TabIndex = 0;
            // 
            // lookUpWindowHospitel
            // 
            this.lookUpWindowHospitel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowHospitel.GenShortCode = null;
            this.lookUpWindowHospitel.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowHospitel.Owner = null;
            this.lookUpWindowHospitel.SqlHelper = null;
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(118, 118);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 27);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.Location = new System.Drawing.Point(204, 118);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 27);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "取消(&C)";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl3.Location = new System.Drawing.Point(287, 38);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(8, 14);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "*";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl4.Location = new System.Drawing.Point(287, 80);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(8, 14);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "*";
            // 
            // AddNewTemplet_Header
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 171);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lookUpEditorHospitel);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddNewTemplet_Header";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "保存页眉/页脚";
            this.Load += new System.EventHandler(this.AddNewTemplet_Header_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorHospitel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowHospitel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private Common.Library.LookUpEditor lookUpEditorHospitel;
        private Common.Library.LookUpWindow lookUpWindowHospitel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnSave;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel btnClear;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}