﻿namespace DrectSoft.Library.EmrEditor.Src.Gui
{
    partial class LookupEditorForm
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
            this.lookUpEditor1 = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindow1 = new DrectSoft.Common.Library.LookUpWindow(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.buttonOK = new DevExpress.XtraEditors.SimpleButton();
            this.buttonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelName = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lookUpEditor1
            // 
            this.lookUpEditor1.ListWindow = this.lookUpWindow1;
            this.lookUpEditor1.Location = new System.Drawing.Point(12, 28);
            this.lookUpEditor1.Name = "lookUpEditor1";
            this.lookUpEditor1.ShowFormImmediately = true;
            this.lookUpEditor1.ShowSButton = true;
            this.lookUpEditor1.Size = new System.Drawing.Size(199, 21);
            this.lookUpEditor1.TabIndex = 5;
            // 
            // lookUpWindow1
            // 
            this.lookUpWindow1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindow1.GenShortCode = null;
            this.lookUpWindow1.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindow1.Owner = null;
            this.lookUpWindow1.SqlHelper = null;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelName);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.buttonCancel);
            this.panelControl1.Controls.Add(this.buttonOK);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(223, 82);
            this.panelControl1.TabIndex = 8;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(57, 53);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(68, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "确定";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(135, 53);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(68, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 7);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "当前元素：";
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(83, 7);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(55, 14);
            this.labelName.TabIndex = 3;
            this.labelName.Text = "labelName";
            // 
            // LookupEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 82);
            this.Controls.Add(this.lookUpEditor1);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LookupEditorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LookupEditorForm";
            this.Load += new System.EventHandler(this.LookupEditorForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LookupEditorForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DrectSoft.Common.Library.LookUpEditor lookUpEditor1;
        private DrectSoft.Common.Library.LookUpWindow lookUpWindow1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton buttonCancel;
        private DevExpress.XtraEditors.SimpleButton buttonOK;
        private DevExpress.XtraEditors.LabelControl labelName;
    }
}