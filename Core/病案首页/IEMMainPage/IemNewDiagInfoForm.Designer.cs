namespace YidanSoft.Core.IEMMainPage
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.lueOutDiag = new YidanSoft.Common.Library.LookUpEditor();
            this.labelControl59 = new DevExpress.XtraEditors.LabelControl();
            this.chkStatus5 = new DevExpress.XtraEditors.CheckEdit();
            this.chkStatus4 = new DevExpress.XtraEditors.CheckEdit();
            this.chkStatus3 = new DevExpress.XtraEditors.CheckEdit();
            this.chkStatus2 = new DevExpress.XtraEditors.CheckEdit();
            this.chkStatus1 = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl58 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueOutDiag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus5.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.chkStatus5);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.chkStatus4);
            this.panelControl1.Controls.Add(this.btnConfirm);
            this.panelControl1.Controls.Add(this.chkStatus3);
            this.panelControl1.Controls.Add(this.lueOutDiag);
            this.panelControl1.Controls.Add(this.chkStatus2);
            this.panelControl1.Controls.Add(this.chkStatus1);
            this.panelControl1.Controls.Add(this.labelControl59);
            this.panelControl1.Controls.Add(this.labelControl58);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(478, 157);
            this.panelControl1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(269, 106);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 118;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(135, 106);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(87, 27);
            this.btnConfirm.TabIndex = 117;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lueOutDiag
            // 
            this.lueOutDiag.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lueOutDiag.ListWindow = null;
            this.lueOutDiag.Location = new System.Drawing.Point(99, 25);
            this.lueOutDiag.Name = "lueOutDiag";
            this.lueOutDiag.ShowSButton = true;
            this.lueOutDiag.Size = new System.Drawing.Size(335, 21);
            this.lueOutDiag.TabIndex = 1;
            // 
            // labelControl59
            // 
            this.labelControl59.Location = new System.Drawing.Point(34, 28);
            this.labelControl59.Name = "labelControl59";
            this.labelControl59.Size = new System.Drawing.Size(48, 14);
            this.labelControl59.TabIndex = 116;
            this.labelControl59.Text = "出院诊断";
            // 
            // chkStatus5
            // 
            this.chkStatus5.Location = new System.Drawing.Point(380, 69);
            this.chkStatus5.Name = "chkStatus5";
            this.chkStatus5.Properties.Caption = "5.其他";
            this.chkStatus5.Properties.RadioGroupIndex = 0;
            this.chkStatus5.Size = new System.Drawing.Size(64, 19);
            this.chkStatus5.TabIndex = 4;
            this.chkStatus5.TabStop = false;
            this.chkStatus5.Tag = "其他";
            // 
            // chkStatus4
            // 
            this.chkStatus4.Location = new System.Drawing.Point(314, 69);
            this.chkStatus4.Name = "chkStatus4";
            this.chkStatus4.Properties.Caption = "4.死亡";
            this.chkStatus4.Properties.RadioGroupIndex = 0;
            this.chkStatus4.Size = new System.Drawing.Size(70, 19);
            this.chkStatus4.TabIndex = 3;
            this.chkStatus4.TabStop = false;
            this.chkStatus4.Tag = "死亡";
            // 
            // chkStatus3
            // 
            this.chkStatus3.Location = new System.Drawing.Point(243, 69);
            this.chkStatus3.Name = "chkStatus3";
            this.chkStatus3.Properties.Caption = "3.未愈";
            this.chkStatus3.Properties.RadioGroupIndex = 0;
            this.chkStatus3.Size = new System.Drawing.Size(70, 19);
            this.chkStatus3.TabIndex = 2;
            this.chkStatus3.TabStop = false;
            this.chkStatus3.Tag = "未愈";
            // 
            // chkStatus2
            // 
            this.chkStatus2.Location = new System.Drawing.Point(169, 69);
            this.chkStatus2.Name = "chkStatus2";
            this.chkStatus2.Properties.Caption = "2.好转";
            this.chkStatus2.Properties.RadioGroupIndex = 0;
            this.chkStatus2.Size = new System.Drawing.Size(66, 19);
            this.chkStatus2.TabIndex = 1;
            this.chkStatus2.TabStop = false;
            this.chkStatus2.Tag = "好转";
            // 
            // chkStatus1
            // 
            this.chkStatus1.Location = new System.Drawing.Point(97, 69);
            this.chkStatus1.Name = "chkStatus1";
            this.chkStatus1.Properties.Caption = "1.治愈";
            this.chkStatus1.Properties.RadioGroupIndex = 0;
            this.chkStatus1.Size = new System.Drawing.Size(76, 19);
            this.chkStatus1.TabIndex = 0;
            this.chkStatus1.TabStop = false;
            this.chkStatus1.Tag = "治愈";
            // 
            // labelControl58
            // 
            this.labelControl58.Location = new System.Drawing.Point(34, 72);
            this.labelControl58.Name = "labelControl58";
            this.labelControl58.Size = new System.Drawing.Size(48, 14);
            this.labelControl58.TabIndex = 115;
            this.labelControl58.Text = "入院状态";
            // 
            // IemNewDiagInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 157);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "IemNewDiagInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "出院诊断";
            this.Load += new System.EventHandler(this.IemNewDiagInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueOutDiag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus5.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStatus1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private YidanSoft.Common.Library.LookUpEditor lueOutDiag;
        private DevExpress.XtraEditors.LabelControl labelControl59;
        private DevExpress.XtraEditors.CheckEdit chkStatus3;
        private DevExpress.XtraEditors.CheckEdit chkStatus2;
        private DevExpress.XtraEditors.CheckEdit chkStatus1;
        private DevExpress.XtraEditors.LabelControl labelControl58;
        private DevExpress.XtraEditors.CheckEdit chkStatus4;
        private DevExpress.XtraEditors.CheckEdit chkStatus5;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
    }
}