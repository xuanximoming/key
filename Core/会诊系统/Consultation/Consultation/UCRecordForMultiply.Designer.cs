namespace DrectSoft.Core.Consultation
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.ucRecordResultForMultiply = new DrectSoft.Core.Consultation.UCRecordResultForMultiply();
            this.panelControl6 = new DevExpress.XtraEditors.PanelControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ucApplyInfoForMultipy = new DrectSoft.Core.Consultation.UCConsultationInfoForMultiply();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ucPatientInfoForMultipy = new DrectSoft.Core.Consultation.UCPatientInfo();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlApprove)).BeginInit();
            this.groupControlApprove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).BeginInit();
            this.panelControl6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // simpleButtonSave
            // 
            this.simpleButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.simpleButtonSave.Image = global::DrectSoft.Core.Consultation.Properties.Resources.保存;
            this.simpleButtonSave.Location = new System.Drawing.Point(799, 7);
            this.simpleButtonSave.Name = "simpleButtonSave";
            this.simpleButtonSave.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonSave.TabIndex = 0;
            this.simpleButtonSave.Text = "保存 (&S)";
            this.simpleButtonSave.ToolTip = "保存";
            // 
            // simpleButtonComplete
            // 
            this.simpleButtonComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.simpleButtonComplete.Image = global::DrectSoft.Core.Consultation.Properties.Resources.确定;
            this.simpleButtonComplete.Location = new System.Drawing.Point(885, 7);
            this.simpleButtonComplete.Name = "simpleButtonComplete";
            this.simpleButtonComplete.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonComplete.TabIndex = 1;
            this.simpleButtonComplete.Text = "会诊完成";
            // 
            // groupControlApprove
            // 
            this.groupControlApprove.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControlApprove.AppearanceCaption.Options.UseFont = true;
            this.groupControlApprove.Controls.Add(this.memoEditSuggestion);
            this.groupControlApprove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlApprove.Location = new System.Drawing.Point(0, 0);
            this.groupControlApprove.Name = "groupControlApprove";
            this.groupControlApprove.Size = new System.Drawing.Size(1000, 81);
            this.groupControlApprove.TabIndex = 0;
            this.groupControlApprove.Text = "审核意见";
            // 
            // memoEditSuggestion
            // 
            this.memoEditSuggestion.Location = new System.Drawing.Point(27, 29);
            this.memoEditSuggestion.Name = "memoEditSuggestion";
            this.memoEditSuggestion.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.memoEditSuggestion.Size = new System.Drawing.Size(940, 45);
            this.memoEditSuggestion.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.panel7);
            this.panelControl4.Controls.Add(this.panel6);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl4.Location = new System.Drawing.Point(0, 0);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(1000, 335);
            this.panelControl4.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.simpleButtonComplete);
            this.panel7.Controls.Add(this.simpleButtonSave);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(2, 292);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(996, 41);
            this.panel7.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.ucRecordResultForMultiply);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(2, 2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(996, 290);
            this.panel6.TabIndex = 0;
            // 
            // ucRecordResultForMultiply
            // 
            this.ucRecordResultForMultiply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucRecordResultForMultiply.Location = new System.Drawing.Point(0, 0);
            this.ucRecordResultForMultiply.Name = "ucRecordResultForMultiply";
            this.ucRecordResultForMultiply.Size = new System.Drawing.Size(996, 290);
            this.ucRecordResultForMultiply.TabIndex = 0;
            this.ucRecordResultForMultiply.TabStop = false;
            // 
            // panelControl6
            // 
            this.panelControl6.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl6.Controls.Add(this.panel4);
            this.panelControl6.Controls.Add(this.panel3);
            this.panelControl6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl6.Location = new System.Drawing.Point(0, 0);
            this.panelControl6.Name = "panelControl6";
            this.panelControl6.Size = new System.Drawing.Size(1000, 970);
            this.panelControl6.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 425);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1000, 541);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupControlApprove);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 460);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1000, 81);
            this.panel5.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ucApplyInfoForMultipy);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1000, 460);
            this.panel2.TabIndex = 1;
            // 
            // ucApplyInfoForMultipy
            // 
            this.ucApplyInfoForMultipy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucApplyInfoForMultipy.Location = new System.Drawing.Point(0, 0);
            this.ucApplyInfoForMultipy.Name = "ucApplyInfoForMultipy";
            this.ucApplyInfoForMultipy.Size = new System.Drawing.Size(1000, 460);
            this.ucApplyInfoForMultipy.TabIndex = 0;
            this.ucApplyInfoForMultipy.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ucPatientInfoForMultipy);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1000, 425);
            this.panel3.TabIndex = 0;
            // 
            // ucPatientInfoForMultipy
            // 
            this.ucPatientInfoForMultipy.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPatientInfoForMultipy.Location = new System.Drawing.Point(0, 335);
            this.ucPatientInfoForMultipy.Name = "ucPatientInfoForMultipy";
            this.ucPatientInfoForMultipy.Size = new System.Drawing.Size(1000, 90);
            this.ucPatientInfoForMultipy.TabIndex = 0;
            this.ucPatientInfoForMultipy.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelControl4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 335);
            this.panel1.TabIndex = 0;
            // 
            // UCRecordForMultiply
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Controls.Add(this.panelControl6);
            this.Name = "UCRecordForMultiply";
            this.Size = new System.Drawing.Size(1000, 970);
            this.Load += new System.EventHandler(this.UCRecordForMultiply_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlApprove)).EndInit();
            this.groupControlApprove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).EndInit();
            this.panelControl6.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.PanelControl panelControl6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
    }
}
