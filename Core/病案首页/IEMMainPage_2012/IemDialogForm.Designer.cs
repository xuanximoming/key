namespace DrectSoft.Core.IEMMainPage
{
    partial class IemDialogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IemDialogForm));
            this.lookUpEditDialog = new DrectSoft.Common.Library.LookUpEditor();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnOk = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK();
            this.btnCancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditDialog)).BeginInit();
            this.SuspendLayout();
            // 
            // lookUpEditDialog
            // 
            this.lookUpEditDialog.ListWindow = null;
            this.lookUpEditDialog.Location = new System.Drawing.Point(77, 22);
            this.lookUpEditDialog.Name = "lookUpEditDialog";
            this.lookUpEditDialog.ShowSButton = true;
            this.lookUpEditDialog.Size = new System.Drawing.Size(200, 18);
            this.lookUpEditDialog.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 25);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "疾病诊断：";
            // 
            // btnOk
            // 
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(65, 73);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定(&Y)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(173, 73);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // IemDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 129);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.lookUpEditDialog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IemDialogForm";
            this.Text = "疾病诊断";
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditDialog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.Library.LookUpEditor lookUpEditDialog;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK btnOk;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel btnCancel;
    }
}