namespace DrectSoft.Core.MainEmrPad.HelpForm
{
    partial class NewChildNode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewChildNode));
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpWindow1 = new DrectSoft.Common.Library.LookUpWindow(this.components);
            this.ContentEdit = new DevExpress.XtraEditors.MemoEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtITEMNAME = new DevExpress.XtraEditors.TextEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtClass = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton2 = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.simpleButton1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContentEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtITEMNAME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtClass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(19, 36);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 14);
            this.labelControl5.TabIndex = 3;
            this.labelControl5.Text = "诊断类别：";
            // 
            // lookUpWindow1
            // 
            this.lookUpWindow1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindow1.GenShortCode = null;
            this.lookUpWindow1.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindow1.Owner = null;
            this.lookUpWindow1.SqlHelper = null;
            // 
            // ContentEdit
            // 
            this.ContentEdit.EnterMoveNextControl = true;
            this.ContentEdit.Location = new System.Drawing.Point(79, 89);
            this.ContentEdit.Name = "ContentEdit";
            this.ContentEdit.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.ContentEdit.Size = new System.Drawing.Size(225, 165);
            this.ContentEdit.TabIndex = 2;
            this.ContentEdit.UseOptimizedRendering = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // txtITEMNAME
            // 
            this.txtITEMNAME.EnterMoveNextControl = true;
            this.txtITEMNAME.Location = new System.Drawing.Point(79, 61);
            this.txtITEMNAME.Name = "txtITEMNAME";
            this.txtITEMNAME.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txtITEMNAME.Size = new System.Drawing.Size(225, 20);
            this.txtITEMNAME.TabIndex = 1;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.ContentEdit);
            this.groupControl1.Controls.Add(this.txtITEMNAME);
            this.groupControl1.Controls.Add(this.txtClass);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(322, 252);
            this.groupControl1.TabIndex = 0;
            // 
            // txtClass
            // 
            this.txtClass.Enabled = false;
            this.txtClass.EnterMoveNextControl = true;
            this.txtClass.Location = new System.Drawing.Point(79, 33);
            this.txtClass.Name = "txtClass";
            this.txtClass.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txtClass.Size = new System.Drawing.Size(225, 20);
            this.txtClass.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(19, 92);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "诊断内容：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(19, 64);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "子类名称：";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 252);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(322, 40);
            this.panelControl1.TabIndex = 1;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(224, 7);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(80, 27);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "取消(&C)";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(138, 7);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(80, 27);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "确定(&Y)";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // NewChildNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 292);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewChildNode";
            this.Text = "新增诊断";
            this.Load += new System.EventHandler(this.NewChildNode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContentEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtITEMNAME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtClass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DrectSoft.Common.Library.LookUpWindow lookUpWindow1;
        private DevExpress.XtraEditors.MemoEdit ContentEdit;
        private DevExpress.XtraEditors.TextEdit txtITEMNAME;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit txtClass;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel simpleButton2;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK simpleButton1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}