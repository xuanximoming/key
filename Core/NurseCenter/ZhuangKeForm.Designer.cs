namespace DrectSoft.Emr.NurseCenter
{
    partial class ZhuangKeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZhuangKeForm));
            this.lookUpEditorDept = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowDept = new DrectSoft.Common.Library.LookUpWindow();
            this.lookUpEditorWard = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowWard = new DrectSoft.Common.Library.LookUpWindow();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorWard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowWard)).BeginInit();
            this.SuspendLayout();
            // 
            // lookUpEditorDept
            // 
            this.lookUpEditorDept.EnterMoveNextControl = true;
            this.lookUpEditorDept.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDept.ListWindow = this.lookUpWindowDept;
            this.lookUpEditorDept.Location = new System.Drawing.Point(72, 21);
            this.lookUpEditorDept.Name = "lookUpEditorDept";
            this.lookUpEditorDept.ShowSButton = true;
            this.lookUpEditorDept.Size = new System.Drawing.Size(150, 18);
            this.lookUpEditorDept.TabIndex = 0;
            this.lookUpEditorDept.ToolTip = "科室";
            this.lookUpEditorDept.CodeValueChanged += new System.EventHandler(this.lookUpEditorDept_CodeValueChanged);
            // 
            // lookUpWindowDept
            // 
            this.lookUpWindowDept.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDept.GenShortCode = null;
            this.lookUpWindowDept.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDept.Owner = null;
            this.lookUpWindowDept.SqlHelper = null;
            // 
            // lookUpEditorWard
            // 
            this.lookUpEditorWard.EnterMoveNextControl = true;
            this.lookUpEditorWard.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorWard.ListWindow = this.lookUpWindowWard;
            this.lookUpEditorWard.Location = new System.Drawing.Point(72, 60);
            this.lookUpEditorWard.Name = "lookUpEditorWard";
            this.lookUpEditorWard.ShowSButton = true;
            this.lookUpEditorWard.Size = new System.Drawing.Size(150, 18);
            this.lookUpEditorWard.TabIndex = 1;
            this.lookUpEditorWard.ToolTip = "病区";
            // 
            // lookUpWindowWard
            // 
            this.lookUpWindowWard.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowWard.GenShortCode = null;
            this.lookUpWindowWard.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowWard.Owner = null;
            this.lookUpWindowWard.SqlHelper = null;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(36, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "科室：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(36, 63);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "病区：";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Image = global::DrectSoft.Emr.NurseCenter.Properties.Resources.确定;
            this.btnOk.Location = new System.Drawing.Point(56, 101);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 27);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "确定 (&Y)";
            this.btnOk.ToolTip = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancle.Image = global::DrectSoft.Emr.NurseCenter.Properties.Resources.取消;
            this.btnCancle.Location = new System.Drawing.Point(142, 101);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(80, 27);
            this.btnCancle.TabIndex = 5;
            this.btnCancle.Text = "取消 (&C)";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // ZhuangKeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 149);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.lookUpEditorWard);
            this.Controls.Add(this.lookUpEditorDept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ZhuangKeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "病人转科";
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorWard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowWard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.Library.LookUpEditor lookUpEditorDept;
        private Common.Library.LookUpWindow lookUpWindowDept;
        private Common.Library.LookUpEditor lookUpEditorWard;
        private Common.Library.LookUpWindow lookUpWindowWard;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
    }
}