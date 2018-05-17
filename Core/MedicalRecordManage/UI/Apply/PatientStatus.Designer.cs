namespace MedicalRecordManage.UI
{
    partial class PatientStatus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatientStatus));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Cancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.txt_patientId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_recordStatus = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_VisitId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_RecordNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.rb_recordID = new System.Windows.Forms.RadioButton();
            this.rb_name = new System.Windows.Forms.RadioButton();
            this.rb_ID = new System.Windows.Forms.RadioButton();
            this.imageListXB = new System.Windows.Forms.ImageList();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_Cancel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 383);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(394, 39);
            this.panelControl1.TabIndex = 0;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cancel.Image")));
            this.btn_Cancel.Location = new System.Drawing.Point(308, 6);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(80, 27);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "取消(&C)";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.txt_patientId);
            this.panelControl2.Controls.Add(this.label5);
            this.panelControl2.Controls.Add(this.txt_recordStatus);
            this.panelControl2.Controls.Add(this.label4);
            this.panelControl2.Controls.Add(this.txt_VisitId);
            this.panelControl2.Controls.Add(this.label3);
            this.panelControl2.Controls.Add(this.txt_RecordNo);
            this.panelControl2.Controls.Add(this.label2);
            this.panelControl2.Controls.Add(this.txt_name);
            this.panelControl2.Controls.Add(this.label1);
            this.panelControl2.Controls.Add(this.groupBox1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(394, 383);
            this.panelControl2.TabIndex = 1;
            // 
            // txt_patientId
            // 
            this.txt_patientId.Location = new System.Drawing.Point(82, 142);
            this.txt_patientId.Name = "txt_patientId";
            this.txt_patientId.Size = new System.Drawing.Size(100, 22);
            this.txt_patientId.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 11;
            this.label5.Text = "患 者  ID：";
            // 
            // txt_recordStatus
            // 
            this.txt_recordStatus.Location = new System.Drawing.Point(82, 176);
            this.txt_recordStatus.Name = "txt_recordStatus";
            this.txt_recordStatus.Size = new System.Drawing.Size(100, 22);
            this.txt_recordStatus.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 9;
            this.label4.Text = "病历状态：";
            // 
            // txt_VisitId
            // 
            this.txt_VisitId.Location = new System.Drawing.Point(273, 141);
            this.txt_VisitId.Name = "txt_VisitId";
            this.txt_VisitId.Size = new System.Drawing.Size(100, 22);
            this.txt_VisitId.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(200, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 7;
            this.label3.Text = "住院次数：";
            // 
            // txt_RecordNo
            // 
            this.txt_RecordNo.Location = new System.Drawing.Point(273, 109);
            this.txt_RecordNo.Name = "txt_RecordNo";
            this.txt_RecordNo.Size = new System.Drawing.Size(100, 22);
            this.txt_RecordNo.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(200, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "病 案  号：";
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(82, 109);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(100, 22);
            this.txt_name.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "患者姓名：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_search);
            this.groupBox1.Controls.Add(this.txt_search);
            this.groupBox1.Controls.Add(this.rb_recordID);
            this.groupBox1.Controls.Add(this.rb_name);
            this.groupBox1.Controls.Add(this.rb_ID);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(361, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "条件";
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(224, 52);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(75, 23);
            this.btn_search.TabIndex = 4;
            this.btn_search.Text = "查询（Q）";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(224, 18);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(100, 22);
            this.txt_search.TabIndex = 3;
            // 
            // rb_recordID
            // 
            this.rb_recordID.AutoSize = true;
            this.rb_recordID.Location = new System.Drawing.Point(7, 46);
            this.rb_recordID.Name = "rb_recordID";
            this.rb_recordID.Size = new System.Drawing.Size(61, 18);
            this.rb_recordID.TabIndex = 2;
            this.rb_recordID.Text = "病案号";
            this.rb_recordID.UseVisualStyleBackColor = true;
            // 
            // rb_name
            // 
            this.rb_name.AutoSize = true;
            this.rb_name.Location = new System.Drawing.Point(120, 22);
            this.rb_name.Name = "rb_name";
            this.rb_name.Size = new System.Drawing.Size(73, 18);
            this.rb_name.TabIndex = 1;
            this.rb_name.Text = "患者姓名";
            this.rb_name.UseVisualStyleBackColor = true;
            // 
            // rb_ID
            // 
            this.rb_ID.AutoSize = true;
            this.rb_ID.Checked = true;
            this.rb_ID.Location = new System.Drawing.Point(7, 22);
            this.rb_ID.Name = "rb_ID";
            this.rb_ID.Size = new System.Drawing.Size(61, 18);
            this.rb_ID.TabIndex = 0;
            this.rb_ID.TabStop = true;
            this.rb_ID.Text = "患者ID";
            this.rb_ID.UseVisualStyleBackColor = true;
            // 
            // imageListXB
            // 
            this.imageListXB.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListXB.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListXB.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // PatientStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 422);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PatientStatus";
            this.Text = "病案状态查询";
            this.Load += new System.EventHandler(this.ChoosePatOrBaby_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel btn_Cancel;
        private System.Windows.Forms.ImageList imageListXB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb_recordID;
        private System.Windows.Forms.RadioButton rb_name;
        private System.Windows.Forms.RadioButton rb_ID;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_RecordNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_VisitId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_recordStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_patientId;
        private System.Windows.Forms.Label label5;
    }
}