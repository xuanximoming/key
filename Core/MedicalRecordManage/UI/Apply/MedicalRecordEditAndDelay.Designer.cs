namespace MedicalRecordManage.UI
{
    partial class MedicalRecordEditAndDelay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MedicalRecordEditAndDelay));
            this.panelBody = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtApproveDate = new DevExpress.XtraEditors.TextEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.txtTimes = new DevExpress.XtraEditors.SpinEdit();
            this.labelDay = new DevExpress.XtraEditors.LabelControl();
            this.memoReason = new DevExpress.XtraEditors.MemoEdit();
            this.labelSubMessage = new DevExpress.XtraEditors.LabelControl();
            this.labelMessage = new DevExpress.XtraEditors.LabelControl();
            this.panelHead = new DevExpress.XtraEditors.PanelControl();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.txtNumber = new DevExpress.XtraEditors.TextEdit();
            this.txtDept = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelBody)).BeginInit();
            this.panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtApproveDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoReason.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelHead)).BeginInit();
            this.panelHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDept.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelBody
            // 
            this.panelBody.Appearance.BackColor = System.Drawing.Color.White;
            this.panelBody.Appearance.Options.UseBackColor = true;
            this.panelBody.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelBody.Controls.Add(this.labelControl1);
            this.panelBody.Controls.Add(this.txtApproveDate);
            this.panelBody.Controls.Add(this.btnCancel);
            this.panelBody.Controls.Add(this.btnSave);
            this.panelBody.Controls.Add(this.btnSubmit);
            this.panelBody.Controls.Add(this.txtTimes);
            this.panelBody.Controls.Add(this.labelDay);
            this.panelBody.Controls.Add(this.memoReason);
            this.panelBody.Controls.Add(this.labelSubMessage);
            this.panelBody.Controls.Add(this.labelMessage);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 49);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(531, 262);
            this.panelBody.TabIndex = 16;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelControl1.Location = new System.Drawing.Point(74, 136);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(151, 14);
            this.labelControl1.TabIndex = 46;
            this.labelControl1.Text = "(注：最多可输入200个字。)";
            // 
            // txtApproveDate
            // 
            this.txtApproveDate.Location = new System.Drawing.Point(68, 166);
            this.txtApproveDate.Name = "txtApproveDate";
            this.txtApproveDate.Size = new System.Drawing.Size(129, 21);
            this.txtApproveDate.TabIndex = 45;
            this.txtApproveDate.Visible = false;
            this.txtApproveDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtApproveDate_KeyPress);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(273, 203);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 23);
            this.btnCancel.TabIndex = 44;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(166, 203);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(101, 23);
            this.btnSave.TabIndex = 43;
            this.btnSave.Text = "保存为草稿";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Image = ((System.Drawing.Image)(resources.GetObject("btnSubmit.Image")));
            this.btnSubmit.Location = new System.Drawing.Point(68, 203);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(90, 23);
            this.btnSubmit.TabIndex = 42;
            this.btnSubmit.Text = "提交";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // txtTimes
            // 
            this.txtTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtTimes.Location = new System.Drawing.Point(68, 166);
            this.txtTimes.Name = "txtTimes";
            this.txtTimes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtTimes.Properties.MaxValue = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.txtTimes.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtTimes.Size = new System.Drawing.Size(100, 21);
            this.txtTimes.TabIndex = 5;
            this.txtTimes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTimes_KeyPress);
            // 
            // labelDay
            // 
            this.labelDay.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelDay.Location = new System.Drawing.Point(203, 169);
            this.labelDay.Name = "labelDay";
            this.labelDay.Size = new System.Drawing.Size(43, 14);
            this.labelDay.TabIndex = 41;
            this.labelDay.Text = "天（*）";
            // 
            // memoReason
            // 
            this.memoReason.EditValue = "";
            this.memoReason.Location = new System.Drawing.Point(68, 3);
            this.memoReason.Name = "memoReason";
            this.memoReason.Properties.MaxLength = 200;
            this.memoReason.Size = new System.Drawing.Size(446, 122);
            this.memoReason.TabIndex = 4;
            this.memoReason.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.memoReason_KeyPress);
            // 
            // labelSubMessage
            // 
            this.labelSubMessage.Location = new System.Drawing.Point(12, 169);
            this.labelSubMessage.Name = "labelSubMessage";
            this.labelSubMessage.Size = new System.Drawing.Size(60, 14);
            this.labelSubMessage.TabIndex = 37;
            this.labelSubMessage.Text = "借阅期限：";
            // 
            // labelMessage
            // 
            this.labelMessage.Location = new System.Drawing.Point(12, 31);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(60, 14);
            this.labelMessage.TabIndex = 36;
            this.labelMessage.Text = "借阅目的：";
            // 
            // panelHead
            // 
            this.panelHead.Appearance.BackColor = System.Drawing.Color.White;
            this.panelHead.Appearance.Options.UseBackColor = true;
            this.panelHead.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelHead.Controls.Add(this.txtName);
            this.panelHead.Controls.Add(this.txtNumber);
            this.panelHead.Controls.Add(this.txtDept);
            this.panelHead.Controls.Add(this.labelControl6);
            this.panelHead.Controls.Add(this.labelControl3);
            this.panelHead.Controls.Add(this.labelControl2);
            this.panelHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHead.Location = new System.Drawing.Point(0, 0);
            this.panelHead.Name = "panelHead";
            this.panelHead.Size = new System.Drawing.Size(531, 49);
            this.panelHead.TabIndex = 15;
            // 
            // txtName
            // 
            this.txtName.Enabled = false;
            this.txtName.Location = new System.Drawing.Point(244, 14);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(91, 21);
            this.txtName.TabIndex = 2;
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            // 
            // txtNumber
            // 
            this.txtNumber.Enabled = false;
            this.txtNumber.Location = new System.Drawing.Point(68, 14);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(103, 21);
            this.txtNumber.TabIndex = 1;
            this.txtNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumber_KeyPress);
            // 
            // txtDept
            // 
            this.txtDept.Enabled = false;
            this.txtDept.Location = new System.Drawing.Point(396, 15);
            this.txtDept.Name = "txtDept";
            this.txtDept.Size = new System.Drawing.Size(118, 21);
            this.txtDept.TabIndex = 3;
            this.txtDept.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDept_KeyPress);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(178, 16);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 14);
            this.labelControl6.TabIndex = 32;
            this.labelControl6.Text = "病人姓名：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(344, 16);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(44, 14);
            this.labelControl3.TabIndex = 31;
            this.labelControl3.Text = "科  室：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 15);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 30;
            this.labelControl2.Text = "住院号：";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // MedicalRecordEditAndDelay
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 311);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelHead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MedicalRecordEditAndDelay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "申请延期";
            this.Load += new System.EventHandler(this.MedicalRecordEditAndDelay_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MedicalRecordEditAndDelay_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.panelBody)).EndInit();
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtApproveDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoReason.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelHead)).EndInit();
            this.panelHead.ResumeLayout(false);
            this.panelHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDept.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelBody;
        private DevExpress.XtraEditors.PanelControl panelHead;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.TextEdit txtNumber;
        private DevExpress.XtraEditors.TextEdit txtDept;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelSubMessage;
        private DevExpress.XtraEditors.LabelControl labelMessage;
        private DevExpress.XtraEditors.MemoEdit memoReason;
        private DevExpress.XtraEditors.LabelControl labelDay;
        private DevExpress.XtraEditors.SpinEdit txtTimes;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private DevExpress.XtraEditors.TextEdit txtApproveDate;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}