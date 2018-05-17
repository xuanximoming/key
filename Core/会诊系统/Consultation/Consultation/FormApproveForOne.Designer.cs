namespace YidanSoft.Core.Consultation
{
    partial class FormApproveForOne
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
            this.groupControlApprove = new DevExpress.XtraEditors.GroupControl();
            this.simpleButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonReject = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.memoEditSuggestion = new DevExpress.XtraEditors.MemoEdit();
            this.ApplyForOne = new YidanSoft.Core.Consultation.UCConsultationApplyForOne();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlApprove)).BeginInit();
            this.groupControlApprove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControlApprove
            // 
            this.groupControlApprove.Controls.Add(this.simpleButtonOK);
            this.groupControlApprove.Controls.Add(this.simpleButtonReject);
            this.groupControlApprove.Controls.Add(this.labelControl1);
            this.groupControlApprove.Controls.Add(this.memoEditSuggestion);
            this.groupControlApprove.Location = new System.Drawing.Point(11, 8);
            this.groupControlApprove.Name = "groupControlApprove";
            this.groupControlApprove.Size = new System.Drawing.Size(699, 138);
            this.groupControlApprove.TabIndex = 0;
            this.groupControlApprove.Text = "审核";
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Location = new System.Drawing.Point(232, 110);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(88, 23);
            this.simpleButtonOK.TabIndex = 1;
            this.simpleButtonOK.Text = "通过";
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButtonOK_Click);
            // 
            // simpleButtonReject
            // 
            this.simpleButtonReject.Location = new System.Drawing.Point(384, 110);
            this.simpleButtonReject.Name = "simpleButtonReject";
            this.simpleButtonReject.Size = new System.Drawing.Size(88, 23);
            this.simpleButtonReject.TabIndex = 2;
            this.simpleButtonReject.Text = "否决";
            this.simpleButtonReject.Click += new System.EventHandler(this.simpleButtonReject_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(21, 30);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "审核意见";
            // 
            // memoEditSuggestion
            // 
            this.memoEditSuggestion.Location = new System.Drawing.Point(21, 50);
            this.memoEditSuggestion.Name = "memoEditSuggestion";
            this.memoEditSuggestion.Size = new System.Drawing.Size(653, 54);
            this.memoEditSuggestion.TabIndex = 0;
            // 
            // ApplyForOne
            // 
            this.ApplyForOne.Location = new System.Drawing.Point(12, 152);
            this.ApplyForOne.Name = "ApplyForOne";
            this.ApplyForOne.Size = new System.Drawing.Size(705, 524);
            this.ApplyForOne.TabIndex = 1;
            // 
            // FormApproveForOne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 685);
            this.Controls.Add(this.groupControlApprove);
            this.Controls.Add(this.ApplyForOne);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormApproveForOne";
            this.Text = "会诊审核";
            ((System.ComponentModel.ISupportInitialize)(this.groupControlApprove)).EndInit();
            this.groupControlApprove.ResumeLayout(false);
            this.groupControlApprove.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UCConsultationApplyForOne ApplyForOne;
        private DevExpress.XtraEditors.GroupControl groupControlApprove;
        private DevExpress.XtraEditors.SimpleButton simpleButtonReject;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit memoEditSuggestion;
        private DevExpress.XtraEditors.SimpleButton simpleButtonOK;
    }
}