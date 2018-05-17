namespace MedicalRecordManage.UCControl
{
    partial class MedicalRecordCfg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MedicalRecordCfg));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.label1 = new System.Windows.Forms.Label();
            this.txtReadTime = new DevExpress.XtraEditors.TextEdit();
            this.txtDelayTimes = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtReadAmount = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDelay = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemind = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Controls.Add(this.btnSubmit);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(731, 553);
            this.panelControl1.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Image = ((System.Drawing.Image)(resources.GetObject("btnSubmit.Image")));
            this.btnSubmit.Location = new System.Drawing.Point(583, 499);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(90, 23);
            this.btnSubmit.TabIndex = 24;
            this.btnSubmit.Text = "提交";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Controls.Add(this.txtReadTime);
            this.groupControl1.Controls.Add(this.txtDelayTimes);
            this.groupControl1.Controls.Add(this.label5);
            this.groupControl1.Controls.Add(this.label4);
            this.groupControl1.Controls.Add(this.txtReadAmount);
            this.groupControl1.Controls.Add(this.label6);
            this.groupControl1.Controls.Add(this.txtDelay);
            this.groupControl1.Controls.Add(this.label3);
            this.groupControl1.Controls.Add(this.txtRemind);
            this.groupControl1.Location = new System.Drawing.Point(139, 120);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(534, 362);
            this.groupControl1.TabIndex = 44;
            this.groupControl1.Text = "参数配置";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 295);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 53;
            this.label1.Text = "延期次数";
            // 
            // txtReadTime
            // 
            this.txtReadTime.Location = new System.Drawing.Point(172, 85);
            this.txtReadTime.Name = "txtReadTime";
            this.txtReadTime.Size = new System.Drawing.Size(257, 22);
            this.txtReadTime.TabIndex = 47;
            // 
            // txtDelayTimes
            // 
            this.txtDelayTimes.Location = new System.Drawing.Point(172, 290);
            this.txtDelayTimes.Name = "txtDelayTimes";
            this.txtDelayTimes.Size = new System.Drawing.Size(257, 22);
            this.txtDelayTimes.TabIndex = 52;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(111, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 14);
            this.label5.TabIndex = 46;
            this.label5.Text = "到期提醒";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(111, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 44;
            this.label4.Text = "申请期限";
            // 
            // txtReadAmount
            // 
            this.txtReadAmount.Location = new System.Drawing.Point(172, 136);
            this.txtReadAmount.Name = "txtReadAmount";
            this.txtReadAmount.Size = new System.Drawing.Size(257, 22);
            this.txtReadAmount.TabIndex = 48;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(109, 242);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 14);
            this.label6.TabIndex = 49;
            this.label6.Text = "延期期限 ";
            // 
            // txtDelay
            // 
            this.txtDelay.Location = new System.Drawing.Point(172, 237);
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.Size = new System.Drawing.Size(257, 22);
            this.txtDelay.TabIndex = 51;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 14);
            this.label3.TabIndex = 45;
            this.label3.Text = "批量申请病历数量 ";
            // 
            // txtRemind
            // 
            this.txtRemind.Location = new System.Drawing.Point(172, 185);
            this.txtRemind.Name = "txtRemind";
            this.txtRemind.Size = new System.Drawing.Size(257, 22);
            this.txtRemind.TabIndex = 50;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(440, 88);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(12, 14);
            this.labelControl1.TabIndex = 54;
            this.labelControl1.Text = "天";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(441, 189);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(12, 14);
            this.labelControl2.TabIndex = 55;
            this.labelControl2.Text = "天";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(441, 241);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(12, 14);
            this.labelControl3.TabIndex = 56;
            this.labelControl3.Text = "天";
            // 
            // MedicalRecordCfg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Name = "MedicalRecordCfg";
            this.Size = new System.Drawing.Size(731, 553);
            this.Load += new System.EventHandler(this.MedicalRecordCfg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtReadTime;
        private DevExpress.XtraEditors.TextEdit txtDelayTimes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtReadAmount;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txtDelay;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txtRemind;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;

    }
}
