namespace DrectSoft.Core.Consultation
{
    partial class UCConsultationApplyForMultiply
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCConsultationApplyForMultiply));
            this.groupControlApprove = new DevExpress.XtraEditors.GroupControl();
            this.memoEditSuggestion = new DevExpress.XtraEditors.MemoEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonExit = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonClear = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.UCApplyInfoForMultiple = new DrectSoft.Core.Consultation.UCConsultationInfoForMultiply();
            this.UCPatientInfoForMultiple = new DrectSoft.Core.Consultation.UCPatientInfo();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlApprove)).BeginInit();
            this.groupControlApprove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControlApprove
            // 
            this.groupControlApprove.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControlApprove.AppearanceCaption.Options.UseFont = true;
            this.groupControlApprove.Controls.Add(this.memoEditSuggestion);
            this.groupControlApprove.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControlApprove.Location = new System.Drawing.Point(0, 590);
            this.groupControlApprove.Name = "groupControlApprove";
            this.groupControlApprove.Size = new System.Drawing.Size(1000, 95);
            this.groupControlApprove.TabIndex = 2;
            this.groupControlApprove.Text = "审核意见";
            // 
            // memoEditSuggestion
            // 
            this.memoEditSuggestion.Location = new System.Drawing.Point(28, 34);
            this.memoEditSuggestion.Name = "memoEditSuggestion";
            this.memoEditSuggestion.Properties.ReadOnly = true;
            this.memoEditSuggestion.Size = new System.Drawing.Size(941, 50);
            this.memoEditSuggestion.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButtonExit);
            this.panelControl1.Controls.Add(this.simpleButtonClear);
            this.panelControl1.Controls.Add(this.simpleButtonConfirm);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 550);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1000, 40);
            this.panelControl1.TabIndex = 1;
            this.panelControl1.TabStop = true;
            // 
            // simpleButtonExit
            // 
            this.simpleButtonExit.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonExit.Image")));
            this.simpleButtonExit.Location = new System.Drawing.Point(888, 7);
            this.simpleButtonExit.Name = "simpleButtonExit";
            this.simpleButtonExit.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonExit.TabIndex = 2;
            this.simpleButtonExit.Text = "关闭 (&T)";
            this.simpleButtonExit.ToolTip = "关闭";
            this.simpleButtonExit.Click += new System.EventHandler(this.simpleButtonExit_Click);
            // 
            // simpleButtonClear
            // 
            this.simpleButtonClear.Image = global::DrectSoft.Core.Consultation.Properties.Resources.清空;
            this.simpleButtonClear.Location = new System.Drawing.Point(802, 7);
            this.simpleButtonClear.Name = "simpleButtonClear";
            this.simpleButtonClear.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonClear.TabIndex = 1;
            this.simpleButtonClear.Text = "清空 (&L)";
            this.simpleButtonClear.ToolTip = "清空";
            this.simpleButtonClear.Click += new System.EventHandler(this.simpleButtonClear_Click);
            // 
            // simpleButtonConfirm
            // 
            this.simpleButtonConfirm.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonConfirm.Image")));
            this.simpleButtonConfirm.Location = new System.Drawing.Point(716, 8);
            this.simpleButtonConfirm.Name = "simpleButtonConfirm";
            this.simpleButtonConfirm.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonConfirm.TabIndex = 0;
            this.simpleButtonConfirm.Text = "保存 (&S)";
            this.simpleButtonConfirm.ToolTip = "保存";
            this.simpleButtonConfirm.Click += new System.EventHandler(this.simpleButtonConfirm_Click);
            // 
            // UCApplyInfoForMultiple
            // 
            this.UCApplyInfoForMultiple.Dock = System.Windows.Forms.DockStyle.Top;
            this.UCApplyInfoForMultiple.Location = new System.Drawing.Point(0, 90);
            this.UCApplyInfoForMultiple.Name = "UCApplyInfoForMultiple";
            this.UCApplyInfoForMultiple.Size = new System.Drawing.Size(1000, 460);
            this.UCApplyInfoForMultiple.TabIndex = 0;
            this.UCApplyInfoForMultiple.Load += new System.EventHandler(this.UCApplyInfoForMultiple_Load);
            // 
            // UCPatientInfoForMultiple
            // 
            this.UCPatientInfoForMultiple.Dock = System.Windows.Forms.DockStyle.Top;
            this.UCPatientInfoForMultiple.Location = new System.Drawing.Point(0, 0);
            this.UCPatientInfoForMultiple.Name = "UCPatientInfoForMultiple";
            this.UCPatientInfoForMultiple.Size = new System.Drawing.Size(1000, 90);
            this.UCPatientInfoForMultiple.TabIndex = 3;
            this.UCPatientInfoForMultiple.TabStop = false;
            // 
            // UCConsultationApplyForMultiply
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupControlApprove);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.UCApplyInfoForMultiple);
            this.Controls.Add(this.UCPatientInfoForMultiple);
            this.Name = "UCConsultationApplyForMultiply";
            this.Size = new System.Drawing.Size(1000, 690);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlApprove)).EndInit();
            this.groupControlApprove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControlApprove;
        private DevExpress.XtraEditors.MemoEdit memoEditSuggestion;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonExit;
        private DevExpress.XtraEditors.SimpleButton simpleButtonClear;
        private DevExpress.XtraEditors.SimpleButton simpleButtonConfirm;
        private UCConsultationInfoForMultiply UCApplyInfoForMultiple;
        private UCPatientInfo UCPatientInfoForMultiple;

    }
}
