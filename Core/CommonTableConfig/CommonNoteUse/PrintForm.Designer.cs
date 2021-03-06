﻿
namespace YiDanSoft.Core.CommonTableConfig.CommonNoteUse
{
    partial class PrintForm
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
            this.panelControlTop = new DevExpress.XtraEditors.PanelControl();
            this.btnLoadPdf = new DevExpress.XtraEditors.SimpleButton();
            this.rabLeftRight = new System.Windows.Forms.RadioButton();
            this.rabUpDown = new System.Windows.Forms.RadioButton();
            this.cmbPrintPage = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.checkEditPrintDuplex = new DevExpress.XtraEditors.CheckEdit();
            this.trackBarControl1 = new DevExpress.XtraEditors.TrackBarControl();
            this.lookUpEditPageSize = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.spinEditPrintCount = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonPrint = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditPrinter = new DevExpress.XtraEditors.LookUpEdit();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).BeginInit();
            this.panelControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPrintPage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPrintDuplex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPageSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPrintCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPrinter.Properties)).BeginInit();
            this.panelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControlTop
            // 
            this.panelControlTop.Controls.Add(this.btnLoadPdf);
            this.panelControlTop.Controls.Add(this.rabLeftRight);
            this.panelControlTop.Controls.Add(this.rabUpDown);
            this.panelControlTop.Controls.Add(this.cmbPrintPage);
            this.panelControlTop.Controls.Add(this.labelControl5);
            this.panelControlTop.Controls.Add(this.checkEditPrintDuplex);
            this.panelControlTop.Controls.Add(this.trackBarControl1);
            this.panelControlTop.Controls.Add(this.lookUpEditPageSize);
            this.panelControlTop.Controls.Add(this.labelControl2);
            this.panelControlTop.Controls.Add(this.labelControl1);
            this.panelControlTop.Controls.Add(this.spinEditPrintCount);
            this.panelControlTop.Controls.Add(this.labelControl4);
            this.panelControlTop.Controls.Add(this.simpleButtonPrint);
            this.panelControlTop.Controls.Add(this.labelControl3);
            this.panelControlTop.Controls.Add(this.lookUpEditPrinter);
            this.panelControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlTop.Location = new System.Drawing.Point(0, 0);
            this.panelControlTop.Name = "panelControlTop";
            this.panelControlTop.Size = new System.Drawing.Size(988, 98);
            this.panelControlTop.TabIndex = 3;
            // 
            // btnLoadPdf
            // 
            this.btnLoadPdf.Location = new System.Drawing.Point(889, 5);
            this.btnLoadPdf.Name = "btnLoadPdf";
            this.btnLoadPdf.Size = new System.Drawing.Size(75, 23);
            this.btnLoadPdf.TabIndex = 19;
            this.btnLoadPdf.Text = "导出PDF";
            this.btnLoadPdf.Click += new System.EventHandler(this.btnLoadPdf_Click);
            // 
            // rabLeftRight
            // 
            this.rabLeftRight.AutoSize = true;
            this.rabLeftRight.Location = new System.Drawing.Point(796, 39);
            this.rabLeftRight.Name = "rabLeftRight";
            this.rabLeftRight.Size = new System.Drawing.Size(73, 18);
            this.rabLeftRight.TabIndex = 18;
            this.rabLeftRight.TabStop = true;
            this.rabLeftRight.Text = "左右翻转";
            this.rabLeftRight.UseVisualStyleBackColor = true;
            this.rabLeftRight.Visible = false;
            // 
            // rabUpDown
            // 
            this.rabUpDown.AutoSize = true;
            this.rabUpDown.Location = new System.Drawing.Point(717, 39);
            this.rabUpDown.Name = "rabUpDown";
            this.rabUpDown.Size = new System.Drawing.Size(73, 18);
            this.rabUpDown.TabIndex = 17;
            this.rabUpDown.TabStop = true;
            this.rabUpDown.Text = "上下翻转";
            this.rabUpDown.UseVisualStyleBackColor = true;
            this.rabUpDown.Visible = false;
            // 
            // cmbPrintPage
            // 
            this.cmbPrintPage.Location = new System.Drawing.Point(616, 9);
            this.cmbPrintPage.Name = "cmbPrintPage";
            this.cmbPrintPage.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPrintPage.Properties.Items.AddRange(new object[] {
            "打印第一页",
            "打印第二页",
            "全部打印"});
            this.cmbPrintPage.Size = new System.Drawing.Size(87, 21);
            this.cmbPrintPage.TabIndex = 16;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(565, 12);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 15;
            this.labelControl5.Text = "打印页：";
            // 
            // checkEditPrintDuplex
            // 
            this.checkEditPrintDuplex.Location = new System.Drawing.Point(716, 9);
            this.checkEditPrintDuplex.Name = "checkEditPrintDuplex";
            this.checkEditPrintDuplex.Properties.Caption = "双面打印";
            this.checkEditPrintDuplex.Size = new System.Drawing.Size(71, 19);
            this.checkEditPrintDuplex.TabIndex = 14;
            this.checkEditPrintDuplex.CheckedChanged += new System.EventHandler(this.checkEditPrintDuplex_CheckedChanged);
            // 
            // trackBarControl1
            // 
            this.trackBarControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarControl1.EditValue = 100;
            this.trackBarControl1.Location = new System.Drawing.Point(78, 46);
            this.trackBarControl1.Name = "trackBarControl1";
            this.trackBarControl1.Properties.Maximum = 149;
            this.trackBarControl1.Properties.Minimum = 50;
            this.trackBarControl1.Properties.ShowValueToolTip = true;
            this.trackBarControl1.Size = new System.Drawing.Size(860, 45);
            this.trackBarControl1.TabIndex = 13;
            this.trackBarControl1.Value = 100;
            this.trackBarControl1.EditValueChanged += new System.EventHandler(this.trackBarControl1_EditValueChanged);
            // 
            // lookUpEditPageSize
            // 
            this.lookUpEditPageSize.Location = new System.Drawing.Point(482, 9);
            this.lookUpEditPageSize.Name = "lookUpEditPageSize";
            this.lookUpEditPageSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditPageSize.Size = new System.Drawing.Size(76, 21);
            this.lookUpEditPageSize.TabIndex = 12;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(418, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "纸张大小：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 54);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "显示比例：";
            // 
            // spinEditPrintCount
            // 
            this.spinEditPrintCount.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditPrintCount.Location = new System.Drawing.Point(74, 9);
            this.spinEditPrintCount.Name = "spinEditPrintCount";
            this.spinEditPrintCount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEditPrintCount.Properties.IsFloatValue = false;
            this.spinEditPrintCount.Properties.Mask.EditMask = "N00";
            this.spinEditPrintCount.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.spinEditPrintCount.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditPrintCount.Size = new System.Drawing.Size(55, 21);
            this.spinEditPrintCount.TabIndex = 2;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 12);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "打印份数：";
            // 
            // simpleButtonPrint
            // 
            this.simpleButtonPrint.Location = new System.Drawing.Point(796, 5);
            this.simpleButtonPrint.Name = "simpleButtonPrint";
            this.simpleButtonPrint.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonPrint.TabIndex = 7;
            this.simpleButtonPrint.Text = "打印";
            this.simpleButtonPrint.Click += new System.EventHandler(this.simpleButtonClick);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(135, 12);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 3;
            this.labelControl3.Text = "打印机：";
            // 
            // lookUpEditPrinter
            // 
            this.lookUpEditPrinter.Location = new System.Drawing.Point(182, 9);
            this.lookUpEditPrinter.Name = "lookUpEditPrinter";
            this.lookUpEditPrinter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditPrinter.Size = new System.Drawing.Size(228, 21);
            this.lookUpEditPrinter.TabIndex = 4;
            // 
            // panelContainer
            // 
            this.panelContainer.AutoScroll = true;
            this.panelContainer.Controls.Add(this.simpleButton1);
            this.panelContainer.Controls.Add(this.button1);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 98);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(988, 248);
            this.panelContainer.TabIndex = 4;
            this.panelContainer.Click += new System.EventHandler(this.panelContainer_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(-8, -5);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(10, 10);
            this.simpleButton1.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(-9, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(10, 10);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(988, 346);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.panelControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintForm";
            this.Text = "打印预览";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PrintForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).EndInit();
            this.panelControlTop.ResumeLayout(false);
            this.panelControlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPrintPage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPrintDuplex.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPageSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPrintCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPrinter.Properties)).EndInit();
            this.panelContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlTop;
        private DevExpress.XtraEditors.TrackBarControl trackBarControl1;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditPageSize;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit spinEditPrintCount;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton simpleButtonPrint;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditPrinter;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.CheckEdit checkEditPrintDuplex;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ComboBoxEdit cmbPrintPage;
        private System.Windows.Forms.RadioButton rabLeftRight;
        private System.Windows.Forms.RadioButton rabUpDown;
        private DevExpress.XtraEditors.SimpleButton btnLoadPdf;
    }
}