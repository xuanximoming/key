namespace DrectSoft.Core.Consultation
{
    partial class UCPatientInfo
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
            this.components = new System.ComponentModel.Container();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.textEditAge = new DevExpress.XtraEditors.TextEdit();
            this.textEditGender = new DevExpress.XtraEditors.TextEdit();
            this.textEditJob = new DevExpress.XtraEditors.TextEdit();
            this.textEditPatientSN = new DevExpress.XtraEditors.TextEdit();
            this.textEditMarriage = new DevExpress.XtraEditors.TextEdit();
            this.textEditBedCode = new DevExpress.XtraEditors.TextEdit();
            this.textEditDepartment = new DevExpress.XtraEditors.TextEdit();
            this.textEditName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonInpatientInfo = new DevExpress.XtraEditors.SimpleButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditAge.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditGender.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditJob.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPatientSN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditMarriage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditBedCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditDepartment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.textEditAge);
            this.groupControl1.Controls.Add(this.textEditGender);
            this.groupControl1.Controls.Add(this.textEditJob);
            this.groupControl1.Controls.Add(this.textEditPatientSN);
            this.groupControl1.Controls.Add(this.textEditMarriage);
            this.groupControl1.Controls.Add(this.textEditBedCode);
            this.groupControl1.Controls.Add(this.textEditDepartment);
            this.groupControl1.Controls.Add(this.textEditName);
            this.groupControl1.Controls.Add(this.labelControl10);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.simpleButtonInpatientInfo);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1000, 90);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "病人基本信息";
            // 
            // textEditAge
            // 
            this.textEditAge.Enabled = false;
            this.textEditAge.Location = new System.Drawing.Point(700, 29);
            this.textEditAge.Name = "textEditAge";
            this.textEditAge.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.textEditAge.Properties.Appearance.Options.UseBackColor = true;
            this.textEditAge.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditAge.Size = new System.Drawing.Size(150, 21);
            this.textEditAge.TabIndex = 3;
            this.textEditAge.ToolTip = "年龄";
            this.textEditAge.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // textEditGender
            // 
            this.textEditGender.Enabled = false;
            this.textEditGender.Location = new System.Drawing.Point(489, 28);
            this.textEditGender.Name = "textEditGender";
            this.textEditGender.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.textEditGender.Properties.Appearance.Options.UseBackColor = true;
            this.textEditGender.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditGender.Size = new System.Drawing.Size(150, 21);
            this.textEditGender.TabIndex = 2;
            this.textEditGender.ToolTip = "性别";
            this.textEditGender.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // textEditJob
            // 
            this.textEditJob.Enabled = false;
            this.textEditJob.Location = new System.Drawing.Point(700, 62);
            this.textEditJob.Name = "textEditJob";
            this.textEditJob.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.textEditJob.Properties.Appearance.Options.UseBackColor = true;
            this.textEditJob.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditJob.Size = new System.Drawing.Size(150, 21);
            this.textEditJob.TabIndex = 7;
            this.textEditJob.ToolTip = "职业";
            this.textEditJob.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // textEditPatientSN
            // 
            this.textEditPatientSN.Enabled = false;
            this.textEditPatientSN.Location = new System.Drawing.Point(276, 27);
            this.textEditPatientSN.Name = "textEditPatientSN";
            this.textEditPatientSN.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditPatientSN.Properties.ReadOnly = true;
            this.textEditPatientSN.Size = new System.Drawing.Size(150, 21);
            this.textEditPatientSN.TabIndex = 1;
            this.textEditPatientSN.ToolTip = "病历号";
            this.textEditPatientSN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // textEditMarriage
            // 
            this.textEditMarriage.Enabled = false;
            this.textEditMarriage.Location = new System.Drawing.Point(489, 62);
            this.textEditMarriage.Name = "textEditMarriage";
            this.textEditMarriage.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.textEditMarriage.Properties.Appearance.Options.UseBackColor = true;
            this.textEditMarriage.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditMarriage.Size = new System.Drawing.Size(150, 21);
            this.textEditMarriage.TabIndex = 6;
            this.textEditMarriage.ToolTip = "婚姻";
            this.textEditMarriage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // textEditBedCode
            // 
            this.textEditBedCode.Enabled = false;
            this.textEditBedCode.Location = new System.Drawing.Point(53, 64);
            this.textEditBedCode.Name = "textEditBedCode";
            this.textEditBedCode.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.textEditBedCode.Properties.Appearance.Options.UseBackColor = true;
            this.textEditBedCode.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditBedCode.Size = new System.Drawing.Size(150, 21);
            this.textEditBedCode.TabIndex = 4;
            this.textEditBedCode.ToolTip = "床号";
            this.textEditBedCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // textEditDepartment
            // 
            this.textEditDepartment.Enabled = false;
            this.textEditDepartment.Location = new System.Drawing.Point(276, 62);
            this.textEditDepartment.Name = "textEditDepartment";
            this.textEditDepartment.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.textEditDepartment.Properties.Appearance.Options.UseBackColor = true;
            this.textEditDepartment.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditDepartment.Size = new System.Drawing.Size(150, 21);
            this.textEditDepartment.TabIndex = 5;
            this.textEditDepartment.ToolTip = "科别";
            this.textEditDepartment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // textEditName
            // 
            this.textEditName.Enabled = false;
            this.textEditName.Location = new System.Drawing.Point(53, 29);
            this.textEditName.Name = "textEditName";
            this.textEditName.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.textEditName.Properties.Appearance.Options.UseBackColor = true;
            this.textEditName.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.textEditName.Size = new System.Drawing.Size(150, 21);
            this.textEditName.TabIndex = 0;
            this.textEditName.ToolTip = "姓名";
            this.textEditName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(664, 65);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(36, 14);
            this.labelControl10.TabIndex = 16;
            this.labelControl10.Text = "职业：";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(453, 65);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(36, 14);
            this.labelControl8.TabIndex = 15;
            this.labelControl8.Text = "婚姻：";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(240, 66);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 14);
            this.labelControl6.TabIndex = 14;
            this.labelControl6.Text = "科别：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 31);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "姓名：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(453, 31);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 11;
            this.labelControl4.Text = "性别：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(17, 66);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 14);
            this.labelControl2.TabIndex = 13;
            this.labelControl2.Text = "床号：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(664, 31);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 12;
            this.labelControl5.Text = "年龄：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(228, 31);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 10;
            this.labelControl3.Text = "病历号：";
            // 
            // simpleButtonInpatientInfo
            // 
            this.simpleButtonInpatientInfo.Image = global::DrectSoft.Core.Consultation.Properties.Resources.查看详细;
            this.simpleButtonInpatientInfo.Location = new System.Drawing.Point(875, 30);
            this.simpleButtonInpatientInfo.Name = "simpleButtonInpatientInfo";
            this.simpleButtonInpatientInfo.Size = new System.Drawing.Size(107, 50);
            this.simpleButtonInpatientInfo.TabIndex = 8;
            this.simpleButtonInpatientInfo.Text = "病人详细信息";
            this.simpleButtonInpatientInfo.ToolTip = "病人详细信息";
            this.simpleButtonInpatientInfo.Click += new System.EventHandler(this.simpleButtonInpatientInfo_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // UCPatientInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupControl1);
            this.Name = "UCPatientInfo";
            this.Size = new System.Drawing.Size(1000, 90);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditAge.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditGender.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditJob.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPatientSN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditMarriage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditBedCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditDepartment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonInpatientInfo;
        private DevExpress.XtraEditors.TextEdit textEditAge;
        private DevExpress.XtraEditors.TextEdit textEditGender;
        private DevExpress.XtraEditors.TextEdit textEditJob;
        private DevExpress.XtraEditors.TextEdit textEditPatientSN;
        private DevExpress.XtraEditors.TextEdit textEditMarriage;
        private DevExpress.XtraEditors.TextEdit textEditBedCode;
        private DevExpress.XtraEditors.TextEdit textEditDepartment;
        private DevExpress.XtraEditors.TextEdit textEditName;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}
