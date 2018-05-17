namespace DrectSoft.Core.Consultation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelControlTop = new DevExpress.XtraEditors.PanelControl();
            this.trackBarControl1 = new DevExpress.XtraEditors.TrackBarControl();
            this.lookUpEditPageSize = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonExport = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.spinEditPrintCount = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonPrint = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditPrinter = new DevExpress.XtraEditors.LookUpEdit();
            this.button1 = new DevExpress.XtraEditors.SimpleButton();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).BeginInit();
            this.panelControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPageSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPrintCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPrinter.Properties)).BeginInit();
            this.panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(289, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(198, 113);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // panelControlTop
            // 
            this.panelControlTop.Controls.Add(this.trackBarControl1);
            this.panelControlTop.Controls.Add(this.lookUpEditPageSize);
            this.panelControlTop.Controls.Add(this.labelControl2);
            this.panelControlTop.Controls.Add(this.simpleButtonExport);
            this.panelControlTop.Controls.Add(this.labelControl1);
            this.panelControlTop.Controls.Add(this.spinEditPrintCount);
            this.panelControlTop.Controls.Add(this.labelControl4);
            this.panelControlTop.Controls.Add(this.simpleButtonPrint);
            this.panelControlTop.Controls.Add(this.labelControl3);
            this.panelControlTop.Controls.Add(this.lookUpEditPrinter);
            this.panelControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlTop.Location = new System.Drawing.Point(0, 0);
            this.panelControlTop.Name = "panelControlTop";
            this.panelControlTop.Size = new System.Drawing.Size(900, 81);
            this.panelControlTop.TabIndex = 0;
            // 
            // trackBarControl1
            // 
            this.trackBarControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarControl1.EditValue = 100;
            this.trackBarControl1.Location = new System.Drawing.Point(78, 39);
            this.trackBarControl1.Name = "trackBarControl1";
            this.trackBarControl1.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.trackBarControl1.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.trackBarControl1.Properties.Maximum = 149;
            this.trackBarControl1.Properties.Minimum = 50;
            this.trackBarControl1.Properties.ShowValueToolTip = true;
            this.trackBarControl1.Size = new System.Drawing.Size(772, 45);
            this.trackBarControl1.TabIndex = 9;
            this.trackBarControl1.Value = 100;
            this.trackBarControl1.EditValueChanged += new System.EventHandler(this.trackBarControl1_EditValueChanged_1);
            // 
            // lookUpEditPageSize
            // 
            this.lookUpEditPageSize.Location = new System.Drawing.Point(521, 9);
            this.lookUpEditPageSize.Name = "lookUpEditPageSize";
            this.lookUpEditPageSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditPageSize.Size = new System.Drawing.Size(76, 20);
            this.lookUpEditPageSize.TabIndex = 2;
            this.lookUpEditPageSize.ToolTip = "纸张大小";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(461, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "纸张大小：";
            // 
            // simpleButtonExport
            // 
            this.simpleButtonExport.Cursor = System.Windows.Forms.Cursors.Default;
            this.simpleButtonExport.Image = global::DrectSoft.Core.Consultation.Properties.Resources.导出;
            this.simpleButtonExport.Location = new System.Drawing.Point(730, 7);
            this.simpleButtonExport.Name = "simpleButtonExport";
            this.simpleButtonExport.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonExport.TabIndex = 4;
            this.simpleButtonExport.Text = "导出 (&I)";
            this.simpleButtonExport.Click += new System.EventHandler(this.simpleButtonExport_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 47);
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
            this.spinEditPrintCount.Location = new System.Drawing.Point(72, 9);
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
            this.spinEditPrintCount.Size = new System.Drawing.Size(55, 20);
            this.spinEditPrintCount.TabIndex = 0;
            this.spinEditPrintCount.ToolTip = "打印份数";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 12);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 5;
            this.labelControl4.Text = "打印份数：";
            // 
            // simpleButtonPrint
            // 
            this.simpleButtonPrint.Image = global::DrectSoft.Core.Consultation.Properties.Resources.打印;
            this.simpleButtonPrint.Location = new System.Drawing.Point(634, 7);
            this.simpleButtonPrint.Name = "simpleButtonPrint";
            this.simpleButtonPrint.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonPrint.TabIndex = 3;
            this.simpleButtonPrint.Text = "打印 (&P)";
            this.simpleButtonPrint.Click += new System.EventHandler(this.simpleButtonPrint_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(156, 12);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "打印机：";
            // 
            // lookUpEditPrinter
            // 
            this.lookUpEditPrinter.Location = new System.Drawing.Point(204, 9);
            this.lookUpEditPrinter.Name = "lookUpEditPrinter";
            this.lookUpEditPrinter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditPrinter.Size = new System.Drawing.Size(228, 20);
            this.lookUpEditPrinter.TabIndex = 1;
            this.lookUpEditPrinter.ToolTip = "打印机";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(-7, -5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(10, 10);
            this.button1.TabIndex = 0;
            // 
            // panelContainer
            // 
            this.panelContainer.AutoScroll = true;
            this.panelContainer.Controls.Add(this.pictureBox2);
            this.panelContainer.Controls.Add(this.pictureBox1);
            this.panelContainer.Controls.Add(this.button1);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 81);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(900, 307);
            this.panelContainer.TabIndex = 1;
            this.panelContainer.Click += new System.EventHandler(this.panelContainer_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(289, 146);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(198, 113);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 388);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.panelControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintForm";
            this.Text = "会诊记录单打印";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PrintForm_FormClosed);
            this.Load += new System.EventHandler(this.PrintForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).EndInit();
            this.panelControlTop.ResumeLayout(false);
            this.panelControlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPageSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPrintCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPrinter.Properties)).EndInit();
            this.panelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.PanelControl panelControlTop;
        private DevExpress.XtraEditors.TrackBarControl trackBarControl1;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditPageSize;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButtonExport;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit spinEditPrintCount;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton simpleButtonPrint;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditPrinter;
        private DevExpress.XtraEditors.SimpleButton button1;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}