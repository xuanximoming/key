namespace DrectSoft.Core.IEMMainPage
{
    partial class IemNewDiagInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IemNewDiagInfoForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.bwj1 = new DevTextBoxAndButton.Bwj(this.components);
            this.chkDiagType1 = new DevExpress.XtraEditors.CheckEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.chkStatus4 = new DevExpress.XtraEditors.CheckEdit();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.chkStatus3 = new DevExpress.XtraEditors.CheckEdit();
            this.chkStatus2 = new DevExpress.XtraEditors.CheckEdit();
            this.chkStatus1 = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl59 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl58 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chkOutStatus1 = new DevExpress.XtraEditors.CheckEdit();
            this.chkOutStatus2 = new DevExpress.XtraEditors.CheckEdit();
            this.chkOutStatus3 = new DevExpress.XtraEditors.CheckEdit();
            this.chkOutStatus4 = new DevExpress.XtraEditors.CheckEdit();
            this.chkOutStatus5 = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkDiagType1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOutStatus1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOutStatus2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOutStatus3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOutStatus4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOutStatus5.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Controls.Add(this.chkOutStatus5);
            this.panelControl1.Controls.Add(this.chkOutStatus4);
            this.panelControl1.Controls.Add(this.chkOutStatus3);
            this.panelControl1.Controls.Add(this.chkOutStatus2);
            this.panelControl1.Controls.Add(this.chkOutStatus1);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.bwj1);
            this.panelControl1.Controls.Add(this.chkDiagType1);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.chkStatus4);
            this.panelControl1.Controls.Add(this.btnConfirm);
            this.panelControl1.Controls.Add(this.chkStatus3);
            this.panelControl1.Controls.Add(this.chkStatus2);
            this.panelControl1.Controls.Add(this.chkStatus1);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.labelControl59);
            this.panelControl1.Controls.Add(this.labelControl58);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(610, 189);
            this.panelControl1.TabIndex = 0;
            // 
            // bwj1
            // 
            this.bwj1.BackColor = System.Drawing.Color.White;
            this.bwj1.DiaCode = "";
            this.bwj1.DiaValue = "";
            this.bwj1.Location = new System.Drawing.Point(99, 39);
            this.bwj1.Multiline = true;
            this.bwj1.Name = "bwj1";
            this.bwj1.Size = new System.Drawing.Size(426, 25);
            this.bwj1.TabIndex = 121;
            this.bwj1.WaterText = "请按回车键检索";
            this.bwj1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.bwj1_KeyPress);
            // 
            // chkDiagType1
            // 
            this.chkDiagType1.EditValue = true;
            this.chkDiagType1.Location = new System.Drawing.Point(97, 9);
            this.chkDiagType1.Name = "chkDiagType1";
            this.chkDiagType1.Properties.Caption = "1.西医";
            this.chkDiagType1.Properties.RadioGroupIndex = 2;
            this.chkDiagType1.Size = new System.Drawing.Size(59, 19);
            this.chkDiagType1.TabIndex = 119;
            this.chkDiagType1.CheckedChanged += new System.EventHandler(this.chkDiagType1_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(268, 150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 118;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkStatus4
            // 
            this.chkStatus4.Location = new System.Drawing.Point(389, 84);
            this.chkStatus4.Name = "chkStatus4";
            this.chkStatus4.Properties.Caption = "4.无";
            this.chkStatus4.Properties.RadioGroupIndex = 0;
            this.chkStatus4.Size = new System.Drawing.Size(53, 19);
            this.chkStatus4.TabIndex = 3;
            this.chkStatus4.TabStop = false;
            this.chkStatus4.Tag = "无";
            this.chkStatus4.Visible = true;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(134, 150);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(87, 27);
            this.btnConfirm.TabIndex = 117;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // chkStatus3
            // 
            this.chkStatus3.Location = new System.Drawing.Point(282, 83);
            this.chkStatus3.Name = "chkStatus3";
            this.chkStatus3.Properties.Caption = "3.情况不明";
            this.chkStatus3.Properties.RadioGroupIndex = 0;
            this.chkStatus3.Size = new System.Drawing.Size(87, 19);
            this.chkStatus3.TabIndex = 2;
            this.chkStatus3.TabStop = false;
            this.chkStatus3.Tag = "情况不明";
            this.chkStatus3.Visible = true;
            // 
            // chkStatus2
            // 
            this.chkStatus2.Location = new System.Drawing.Point(172, 83);
            this.chkStatus2.Name = "chkStatus2";
            this.chkStatus2.Properties.Caption = "2.临床未确定";
            this.chkStatus2.Properties.RadioGroupIndex = 0;
            this.chkStatus2.Size = new System.Drawing.Size(96, 19);
            this.chkStatus2.TabIndex = 1;
            this.chkStatus2.TabStop = false;
            this.chkStatus2.Tag = "临床未确定";
            this.chkStatus2.Visible = true;
            // 
            // chkStatus1
            // 
            this.chkStatus1.Location = new System.Drawing.Point(99, 84);
            this.chkStatus1.Name = "chkStatus1";
            this.chkStatus1.Properties.Caption = "1.有";
            this.chkStatus1.Properties.RadioGroupIndex = 0;
            this.chkStatus1.Size = new System.Drawing.Size(53, 19);
            this.chkStatus1.TabIndex = 0;
            this.chkStatus1.TabStop = false;
            this.chkStatus1.Tag = "有";
            this.chkStatus1.Visible = true;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(34, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 116;
            this.labelControl1.Text = "诊断类别";
            // 
            // labelControl59
            // 
            this.labelControl59.Location = new System.Drawing.Point(34, 42);
            this.labelControl59.Name = "labelControl59";
            this.labelControl59.Size = new System.Drawing.Size(48, 14);
            this.labelControl59.TabIndex = 116;
            this.labelControl59.Text = "出院诊断";
            // 
            // labelControl58
            // 
            this.labelControl58.Location = new System.Drawing.Point(34, 86);
            this.labelControl58.Name = "labelControl58";
            this.labelControl58.Size = new System.Drawing.Size(48, 14);
            this.labelControl58.TabIndex = 115;
            this.labelControl58.Text = "入院病情";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(34, 120);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 122;
            this.labelControl2.Text = "出院情况";
            // 
            // chkOutStatus1
            // 
            this.chkOutStatus1.Location = new System.Drawing.Point(97, 115);
            this.chkOutStatus1.Name = "chkOutStatus1";
            this.chkOutStatus1.Properties.Caption = "1.治愈";
            this.chkOutStatus1.Properties.RadioGroupIndex = 1;
            this.chkOutStatus1.Size = new System.Drawing.Size(53, 19);
            this.chkOutStatus1.TabIndex = 123;
            this.chkOutStatus1.TabStop = false;
            this.chkOutStatus1.Tag = "治愈";
            this.chkOutStatus1.Visible = true;
            // 
            // chkOutStatus2
            // 
            this.chkOutStatus2.Location = new System.Drawing.Point(172, 117);
            this.chkOutStatus2.Name = "chkOutStatus2";
            this.chkOutStatus2.Properties.Caption = "2.好转";
            this.chkOutStatus2.Properties.RadioGroupIndex = 1;
            this.chkOutStatus2.Size = new System.Drawing.Size(53, 19);
            this.chkOutStatus2.TabIndex = 124;
            this.chkOutStatus2.TabStop = false;
            this.chkOutStatus2.Tag = "好转";
            this.chkOutStatus2.Visible = true;
            // 
            // chkOutStatus3
            // 
            this.chkOutStatus3.Location = new System.Drawing.Point(248, 117);
            this.chkOutStatus3.Name = "chkOutStatus3";
            this.chkOutStatus3.Properties.Caption = "3.未愈";
            this.chkOutStatus3.Properties.RadioGroupIndex = 1;
            this.chkOutStatus3.Size = new System.Drawing.Size(53, 19);
            this.chkOutStatus3.TabIndex = 125;
            this.chkOutStatus3.TabStop = false;
            this.chkOutStatus3.Tag = "未愈";
            this.chkOutStatus3.Visible = true;
            // 
            // chkOutStatus4
            // 
            this.chkOutStatus4.Location = new System.Drawing.Point(341, 115);
            this.chkOutStatus4.Name = "chkOutStatus4";
            this.chkOutStatus4.Properties.Caption = "4.死亡";
            this.chkOutStatus4.Properties.RadioGroupIndex = 1;
            this.chkOutStatus4.Size = new System.Drawing.Size(53, 19);
            this.chkOutStatus4.TabIndex = 126;
            this.chkOutStatus4.TabStop = false;
            this.chkOutStatus4.Tag = "死亡";
            this.chkOutStatus4.Visible = true;
            // 
            // chkOutStatus5
            // 
            this.chkOutStatus5.Location = new System.Drawing.Point(419, 117);
            this.chkOutStatus5.Name = "chkOutStatus5";
            this.chkOutStatus5.Properties.Caption = "5.其他";
            this.chkOutStatus5.Properties.RadioGroupIndex = 1;
            this.chkOutStatus5.Size = new System.Drawing.Size(53, 19);
            this.chkOutStatus5.TabIndex = 127;
            this.chkOutStatus5.TabStop = false;
            this.chkOutStatus5.Tag = "其他";
            this.chkOutStatus5.Visible = true;
            // 
            // IemNewDiagInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 189);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IemNewDiagInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "出院诊断";
            this.Load += new System.EventHandler(this.IemNewDiagInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkDiagType1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOutStatus1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOutStatus2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOutStatus3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOutStatus4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOutStatus5.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl59;
        private DevExpress.XtraEditors.CheckEdit chkStatus3;
        private DevExpress.XtraEditors.CheckEdit chkStatus2;
        private DevExpress.XtraEditors.CheckEdit chkStatus1;
        private DevExpress.XtraEditors.LabelControl labelControl58;
        private DevExpress.XtraEditors.CheckEdit chkStatus4;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chkDiagType1;
        private DevTextBoxAndButton.Bwj bwj1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit chkOutStatus5;
        private DevExpress.XtraEditors.CheckEdit chkOutStatus4;
        private DevExpress.XtraEditors.CheckEdit chkOutStatus3;
        private DevExpress.XtraEditors.CheckEdit chkOutStatus2;
        private DevExpress.XtraEditors.CheckEdit chkOutStatus1;
    }
}