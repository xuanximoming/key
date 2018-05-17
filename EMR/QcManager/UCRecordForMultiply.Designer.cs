namespace DrectSoft.Emr.QcManager
{
    partial class UCRecordForMultiply
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.simpleButtonSave = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonComplete = new DevExpress.XtraEditors.SimpleButton();
            this.groupControlApprove = new DevExpress.XtraEditors.GroupControl();
            this.memoEditSuggestion = new DevExpress.XtraEditors.MemoEdit();
            this.ucRecordResultForMultiply = new DrectSoft.Emr.QcManager.UCRecordResultForMultiply();
            this.ucApplyInfoForMultipy = new DrectSoft.Emr.QcManager.UCConsultationInfoForMultiply();
            this.ucPatientInfoForMultipy = new DrectSoft.Emr.QcManager.UCPatientInfo();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlApprove)).BeginInit();
            this.groupControlApprove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButtonSave
            // 
            this.simpleButtonSave.Image = global::DrectSoft.Emr.QcManager.Properties.Resources.保存;
            this.simpleButtonSave.Location = new System.Drawing.Point(278, 339);
            this.simpleButtonSave.Name = "simpleButtonSave";
            this.simpleButtonSave.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonSave.TabIndex = 2;
            this.simpleButtonSave.Text = "保存(&S)";
            this.simpleButtonSave.Visible = false;
            // 
            // simpleButtonComplete
            // 
            this.simpleButtonComplete.Image = global::DrectSoft.Emr.QcManager.Properties.Resources.确定;
            this.simpleButtonComplete.Location = new System.Drawing.Point(449, 339);
            this.simpleButtonComplete.Name = "simpleButtonComplete";
            this.simpleButtonComplete.Size = new System.Drawing.Size(100, 27);
            this.simpleButtonComplete.TabIndex = 3;
            this.simpleButtonComplete.Text = "会诊完成(&Y)";
            this.simpleButtonComplete.Visible = false;
            // 
            // groupControlApprove
            // 
            this.groupControlApprove.Controls.Add(this.memoEditSuggestion);
            this.groupControlApprove.Location = new System.Drawing.Point(0, 857);
            this.groupControlApprove.Name = "groupControlApprove";
            this.groupControlApprove.Size = new System.Drawing.Size(817, 100);
            this.groupControlApprove.TabIndex = 6;
            this.groupControlApprove.Text = "审核意见";
            // 
            // memoEditSuggestion
            // 
            this.memoEditSuggestion.Enabled = false;
            this.memoEditSuggestion.Location = new System.Drawing.Point(24, 29);
            this.memoEditSuggestion.Name = "memoEditSuggestion";
            this.memoEditSuggestion.Size = new System.Drawing.Size(761, 63);
            this.memoEditSuggestion.TabIndex = 0;
            // 
            // ucRecordResultForMultiply
            // 
            this.ucRecordResultForMultiply.Location = new System.Drawing.Point(0, 0);
            this.ucRecordResultForMultiply.Name = "ucRecordResultForMultiply";
            this.ucRecordResultForMultiply.Size = new System.Drawing.Size(817, 333);
            this.ucRecordResultForMultiply.TabIndex = 1;
            // 
            // ucApplyInfoForMultipy
            // 
            this.ucApplyInfoForMultipy.Location = new System.Drawing.Point(0, 459);
            this.ucApplyInfoForMultipy.Name = "ucApplyInfoForMultipy";
            this.ucApplyInfoForMultipy.Size = new System.Drawing.Size(817, 408);
            this.ucApplyInfoForMultipy.TabIndex = 5;
            // 
            // ucPatientInfoForMultipy
            // 
            this.ucPatientInfoForMultipy.Enabled = false;
            this.ucPatientInfoForMultipy.Location = new System.Drawing.Point(0, 375);
            this.ucPatientInfoForMultipy.Name = "ucPatientInfoForMultipy";
            this.ucPatientInfoForMultipy.Size = new System.Drawing.Size(817, 96);
            this.ucPatientInfoForMultipy.TabIndex = 4;
            // 
            // UCRecordForMultiply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControlApprove);
            this.Controls.Add(this.simpleButtonComplete);
            this.Controls.Add(this.ucRecordResultForMultiply);
            this.Controls.Add(this.simpleButtonSave);
            this.Controls.Add(this.ucApplyInfoForMultipy);
            this.Controls.Add(this.ucPatientInfoForMultipy);
            this.Name = "UCRecordForMultiply";
            this.Size = new System.Drawing.Size(821, 961);
            this.Load += new System.EventHandler(this.UCRecordForMultiply_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlApprove)).EndInit();
            this.groupControlApprove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UCPatientInfo ucPatientInfoForMultipy;
        private UCConsultationInfoForMultiply ucApplyInfoForMultipy;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSave;
        private UCRecordResultForMultiply ucRecordResultForMultiply;
        private DevExpress.XtraEditors.SimpleButton simpleButtonComplete;
        private DevExpress.XtraEditors.GroupControl groupControlApprove;
        private DevExpress.XtraEditors.MemoEdit memoEditSuggestion;
    }
}
