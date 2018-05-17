namespace DrectSoft.Emr.TemplateFactory
{
    partial class TemplatePagek
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplatePagek));
            this.Sb_save = new DevExpress.XtraEditors.SimpleButton();
            this.Sb_del = new DevExpress.XtraEditors.SimpleButton();
            this.Lbc_pacgename = new DevExpress.XtraEditors.LabelControl();
            this.Te_name = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditorTemplate = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowtemplate = new DrectSoft.Common.Library.LookUpWindow();
            ((System.ComponentModel.ISupportInitialize)(this.Te_name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowtemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // Sb_save
            // 
            this.Sb_save.Location = new System.Drawing.Point(41, 171);
            this.Sb_save.Name = "Sb_save";
            this.Sb_save.Size = new System.Drawing.Size(75, 23);
            this.Sb_save.TabIndex = 1;
            this.Sb_save.Text = "保存";
            this.Sb_save.Click += new System.EventHandler(this.Sb_save_Click);
            // 
            // Sb_del
            // 
            this.Sb_del.Location = new System.Drawing.Point(149, 171);
            this.Sb_del.Name = "Sb_del";
            this.Sb_del.Size = new System.Drawing.Size(75, 23);
            this.Sb_del.TabIndex = 2;
            this.Sb_del.Text = "删除";
            // 
            // Lbc_pacgename
            // 
            this.Lbc_pacgename.Location = new System.Drawing.Point(30, 30);
            this.Lbc_pacgename.Name = "Lbc_pacgename";
            this.Lbc_pacgename.Size = new System.Drawing.Size(72, 14);
            this.Lbc_pacgename.TabIndex = 3;
            this.Lbc_pacgename.Text = "新模板包名称";
            // 
            // Te_name
            // 
            this.Te_name.Location = new System.Drawing.Point(112, 27);
            this.Te_name.Name = "Te_name";
            this.Te_name.Size = new System.Drawing.Size(115, 20);
            this.Te_name.TabIndex = 4;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.lookUpEditorTemplate);
            this.panelControl1.Controls.Add(this.Sb_save);
            this.panelControl1.Controls.Add(this.Te_name);
            this.panelControl1.Controls.Add(this.Sb_del);
            this.panelControl1.Controls.Add(this.Lbc_pacgename);
            this.panelControl1.Location = new System.Drawing.Point(0, -1);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(264, 206);
            this.panelControl1.TabIndex = 5;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(30, 85);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "模板包名称";
            // 
            // lookUpEditorTemplate
            // 
            this.lookUpEditorTemplate.EnterMoveNextControl = true;
            this.lookUpEditorTemplate.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorTemplate.ListWindow = null;
            this.lookUpEditorTemplate.Location = new System.Drawing.Point(112, 82);
            this.lookUpEditorTemplate.Name = "lookUpEditorTemplate";
            this.lookUpEditorTemplate.ShowSButton = true;
            this.lookUpEditorTemplate.Size = new System.Drawing.Size(115, 18);
            this.lookUpEditorTemplate.TabIndex = 5;
            this.lookUpEditorTemplate.ToolTip = "支持科室编码、科室名称(汉字/拼音/五笔)检索";
            // 
            // lookUpWindowtemplate
            // 
            this.lookUpWindowtemplate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowtemplate.GenShortCode = null;
            this.lookUpWindowtemplate.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowtemplate.Owner = null;
            this.lookUpWindowtemplate.SqlHelper = null;
            // 
            // TemplatePagek
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 205);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TemplatePagek";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加模板包";
            ((System.ComponentModel.ISupportInitialize)(this.Te_name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowtemplate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton Sb_save;
        private DevExpress.XtraEditors.SimpleButton Sb_del;
        private DevExpress.XtraEditors.LabelControl Lbc_pacgename;
        private DevExpress.XtraEditors.TextEdit Te_name;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private Common.Library.LookUpWindow lookUpWindowtemplate;
        private Common.Library.LookUpEditor lookUpEditorTemplate;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}