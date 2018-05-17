namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    partial class PrintForm1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintForm1));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.speNum = new DevExpress.XtraEditors.SpinEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPrintHistory = new DevExpress.XtraEditors.SimpleButton();
            this.cboDuplex = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrintNow = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.spinEditPage = new DevExpress.XtraEditors.SpinEdit();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.trackBarControl1 = new DevExpress.XtraEditors.TrackBarControl();
            this.printPreviewControl1 = new System.Windows.Forms.PrintPreviewControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speNum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDuplex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.speNum);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.btnPrintHistory);
            this.panelControl1.Controls.Add(this.cboDuplex);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.btnPrintNow);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.spinEditPage);
            this.panelControl1.Controls.Add(this.btnPrint);
            this.panelControl1.Controls.Add(this.trackBarControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(841, 89);
            this.panelControl1.TabIndex = 1;
            // 
            // speNum
            // 
            this.speNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.speNum.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speNum.Location = new System.Drawing.Point(188, 14);
            this.speNum.Name = "speNum";
            this.speNum.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speNum.Properties.IsFloatValue = false;
            this.speNum.Properties.Mask.EditMask = "N00";
            this.speNum.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.speNum.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speNum.Size = new System.Drawing.Size(54, 20);
            this.speNum.TabIndex = 25;
            this.speNum.EditValueChanged += new System.EventHandler(this.speNum_EditValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 14);
            this.label2.TabIndex = 24;
            this.label2.Text = "开始页序号：";
            // 
            // btnPrintHistory
            // 
            this.btnPrintHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintHistory.Image = ((System.Drawing.Image)(resources.GetObject("btnPrintHistory.Image")));
            this.btnPrintHistory.Location = new System.Drawing.Point(738, 11);
            this.btnPrintHistory.Name = "btnPrintHistory";
            this.btnPrintHistory.Size = new System.Drawing.Size(95, 23);
            this.btnPrintHistory.TabIndex = 23;
            this.btnPrintHistory.Text = "打印历史(&H)";
            this.btnPrintHistory.Click += new System.EventHandler(this.btnPrintHistory_Click);
            // 
            // cboDuplex
            // 
            this.cboDuplex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDuplex.EditValue = "默认打印";
            this.cboDuplex.EnterMoveNextControl = true;
            this.cboDuplex.Location = new System.Drawing.Point(316, 13);
            this.cboDuplex.Name = "cboDuplex";
            this.cboDuplex.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDuplex.Properties.Items.AddRange(new object[] {
            "默认打印",
            "上下翻转双面打印",
            "左右翻转双面打印"});
            this.cboDuplex.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboDuplex.Size = new System.Drawing.Size(120, 20);
            this.cboDuplex.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(248, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 21;
            this.label1.Text = "打印方式：";
            // 
            // btnPrintNow
            // 
            this.btnPrintNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintNow.Image = ((System.Drawing.Image)(resources.GetObject("btnPrintNow.Image")));
            this.btnPrintNow.Location = new System.Drawing.Point(557, 12);
            this.btnPrintNow.Name = "btnPrintNow";
            this.btnPrintNow.Size = new System.Drawing.Size(75, 23);
            this.btnPrintNow.TabIndex = 19;
            this.btnPrintNow.Text = "打印(&P)";
            this.btnPrintNow.Click += new System.EventHandler(this.btnPrintNow_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Location = new System.Drawing.Point(539, 17);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(12, 14);
            this.labelControl2.TabIndex = 18;
            this.labelControl2.Text = "页";
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Location = new System.Drawing.Point(449, 17);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 17;
            this.labelControl1.Text = "打印第";
            // 
            // spinEditPage
            // 
            this.spinEditPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.spinEditPage.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditPage.Location = new System.Drawing.Point(491, 14);
            this.spinEditPage.Name = "spinEditPage";
            this.spinEditPage.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEditPage.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEditPage.Properties.IsFloatValue = false;
            this.spinEditPage.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.spinEditPage.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditPage.Size = new System.Drawing.Size(39, 20);
            this.spinEditPage.TabIndex = 16;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.Location = new System.Drawing.Point(638, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(95, 23);
            this.btnPrint.TabIndex = 15;
            this.btnPrint.Text = "打印全部(&A)";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // trackBarControl1
            // 
            this.trackBarControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarControl1.EditValue = 100;
            this.trackBarControl1.Location = new System.Drawing.Point(3, 44);
            this.trackBarControl1.Name = "trackBarControl1";
            this.trackBarControl1.Properties.Maximum = 149;
            this.trackBarControl1.Properties.Minimum = 50;
            this.trackBarControl1.Properties.ShowValueToolTip = true;
            this.trackBarControl1.Size = new System.Drawing.Size(838, 45);
            this.trackBarControl1.TabIndex = 14;
            this.trackBarControl1.Value = 100;
            this.trackBarControl1.EditValueChanged += new System.EventHandler(this.trackBarControl1_EditValueChanged);
            // 
            // printPreviewControl1
            // 
            this.printPreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPreviewControl1.Location = new System.Drawing.Point(0, 89);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new System.Drawing.Size(841, 339);
            this.printPreviewControl1.TabIndex = 2;
            // 
            // PrintForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 428);
            this.Controls.Add(this.printPreviewControl1);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrintForm1";
            this.Text = "PrintForm";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speNum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDuplex.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.PrintPreviewControl printPreviewControl1;
        private DevExpress.XtraEditors.TrackBarControl trackBarControl1;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraEditors.SpinEdit spinEditPage;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnPrintNow;
        private DevExpress.XtraEditors.ComboBoxEdit cboDuplex;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnPrintHistory;
        private DevExpress.XtraEditors.SpinEdit speNum;
        private System.Windows.Forms.Label label2;

    }
}
