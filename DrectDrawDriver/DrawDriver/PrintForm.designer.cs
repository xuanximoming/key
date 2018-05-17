namespace DrectSoft.DrawDriver
{
    partial class PrintForm
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintForm));
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.pnltop = new System.Windows.Forms.Panel();
            this.rabLeftRight = new System.Windows.Forms.RadioButton();
            this.rabUpDown = new System.Windows.Forms.RadioButton();
            this.checkEditPrintDuplex = new DevExpress.XtraEditors.CheckEdit();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonPrint = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditPageSize = new DevExpress.XtraEditors.LookUpEdit();
            this.btnLoadPdf = new DevExpress.XtraEditors.SimpleButton();
            this.spinEditPrintCount = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditPrinter = new DevExpress.XtraEditors.LookUpEdit();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.trackBarControl1 = new DevExpress.XtraEditors.TrackBarControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.printPreviewControl1 = new System.Windows.Forms.PrintPreviewControl();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.pnltop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPrintDuplex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPageSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPrintCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPrinter.Properties)).BeginInit();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // pnltop
            // 
            this.pnltop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.pnltop.Controls.Add(this.rabLeftRight);
            this.pnltop.Controls.Add(this.rabUpDown);
            this.pnltop.Controls.Add(this.checkEditPrintDuplex);
            this.pnltop.Controls.Add(this.spinEdit1);
            this.pnltop.Controls.Add(this.labelControl5);
            this.pnltop.Controls.Add(this.simpleButtonPrint);
            this.pnltop.Controls.Add(this.labelControl1);
            this.pnltop.Controls.Add(this.labelControl2);
            this.pnltop.Controls.Add(this.lookUpEditPageSize);
            this.pnltop.Controls.Add(this.btnLoadPdf);
            this.pnltop.Controls.Add(this.spinEditPrintCount);
            this.pnltop.Controls.Add(this.labelControl4);
            this.pnltop.Controls.Add(this.btnPrint);
            this.pnltop.Controls.Add(this.labelControl3);
            this.pnltop.Controls.Add(this.lookUpEditPrinter);
            this.pnltop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnltop.Location = new System.Drawing.Point(0, 0);
            this.pnltop.Name = "pnltop";
            this.pnltop.Size = new System.Drawing.Size(1035, 73);
            this.pnltop.TabIndex = 0;
            // 
            // rabLeftRight
            // 
            this.rabLeftRight.AutoSize = true;
            this.rabLeftRight.Location = new System.Drawing.Point(726, 55);
            this.rabLeftRight.Name = "rabLeftRight";
            this.rabLeftRight.Size = new System.Drawing.Size(71, 16);
            this.rabLeftRight.TabIndex = 21;
            this.rabLeftRight.TabStop = true;
            this.rabLeftRight.Text = "左右翻转";
            this.rabLeftRight.UseVisualStyleBackColor = true;
            this.rabLeftRight.Visible = false;
            // 
            // rabUpDown
            // 
            this.rabUpDown.AutoSize = true;
            this.rabUpDown.Location = new System.Drawing.Point(647, 55);
            this.rabUpDown.Name = "rabUpDown";
            this.rabUpDown.Size = new System.Drawing.Size(71, 16);
            this.rabUpDown.TabIndex = 20;
            this.rabUpDown.TabStop = true;
            this.rabUpDown.Text = "上下翻转";
            this.rabUpDown.UseVisualStyleBackColor = true;
            this.rabUpDown.Visible = false;
            // 
            // checkEditPrintDuplex
            // 
            this.checkEditPrintDuplex.Location = new System.Drawing.Point(646, 25);
            this.checkEditPrintDuplex.Name = "checkEditPrintDuplex";
            this.checkEditPrintDuplex.Properties.Caption = "双面打印";
            this.checkEditPrintDuplex.Size = new System.Drawing.Size(71, 19);
            this.checkEditPrintDuplex.TabIndex = 19;
            this.checkEditPrintDuplex.CheckedChanged += new System.EventHandler(this.checkEditPrintDuplex_CheckedChanged);
            // 
            // spinEdit1
            // 
            this.spinEdit1.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(562, 25);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEdit1.Properties.IsFloatValue = false;
            this.spinEdit1.Properties.Mask.EditMask = "N00";
            this.spinEdit1.Properties.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEdit1.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEdit1.Size = new System.Drawing.Size(61, 20);
            this.spinEdit1.TabIndex = 3;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(627, 28);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(12, 14);
            this.labelControl5.TabIndex = 11;
            this.labelControl5.Text = "页";
            // 
            // simpleButtonPrint
            // 
            this.simpleButtonPrint.Location = new System.Drawing.Point(809, 23);
            this.simpleButtonPrint.Name = "simpleButtonPrint";
            this.simpleButtonPrint.Size = new System.Drawing.Size(93, 23);
            this.simpleButtonPrint.TabIndex = 5;
            this.simpleButtonPrint.Text = "打印全部(&A)";
            this.simpleButtonPrint.Click += new System.EventHandler(this.simpleButtonPrint_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(523, 28);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "打印第";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(373, 28);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 9;
            this.labelControl2.Text = "纸张大小：";
            // 
            // lookUpEditPageSize
            // 
            this.lookUpEditPageSize.Location = new System.Drawing.Point(436, 25);
            this.lookUpEditPageSize.Name = "lookUpEditPageSize";
            this.lookUpEditPageSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditPageSize.Size = new System.Drawing.Size(76, 20);
            this.lookUpEditPageSize.TabIndex = 2;
            // 
            // btnLoadPdf
            // 
            this.btnLoadPdf.Location = new System.Drawing.Point(913, 23);
            this.btnLoadPdf.Name = "btnLoadPdf";
            this.btnLoadPdf.Size = new System.Drawing.Size(93, 23);
            this.btnLoadPdf.TabIndex = 6;
            this.btnLoadPdf.Text = "导出PDF(&I)";
            this.btnLoadPdf.Visible = false;
            this.btnLoadPdf.Click += new System.EventHandler(this.btnLoadPdf_Click);
            // 
            // spinEditPrintCount
            // 
            this.spinEditPrintCount.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditPrintCount.Location = new System.Drawing.Point(73, 25);
            this.spinEditPrintCount.Name = "spinEditPrintCount";
            this.spinEditPrintCount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEditPrintCount.Properties.IsFloatValue = false;
            this.spinEditPrintCount.Properties.Mask.EditMask = "N00";
            this.spinEditPrintCount.Properties.MaxValue = new decimal(new int[] {
            1,
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
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(11, 28);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "打印份数：";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(725, 23);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "打印(&P)";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(139, 28);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "打印机：";
            // 
            // lookUpEditPrinter
            // 
            this.lookUpEditPrinter.Location = new System.Drawing.Point(190, 25);
            this.lookUpEditPrinter.Name = "lookUpEditPrinter";
            this.lookUpEditPrinter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditPrinter.Size = new System.Drawing.Size(171, 20);
            this.lookUpEditPrinter.TabIndex = 1;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.trackBarControl1);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBottom.Location = new System.Drawing.Point(0, 73);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1035, 42);
            this.pnlBottom.TabIndex = 1;
            // 
            // trackBarControl1
            // 
            this.trackBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarControl1.EditValue = 100;
            this.trackBarControl1.Location = new System.Drawing.Point(0, 0);
            this.trackBarControl1.Name = "trackBarControl1";
            this.trackBarControl1.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.trackBarControl1.Properties.Appearance.Options.UseBackColor = true;
            this.trackBarControl1.Properties.Maximum = 149;
            this.trackBarControl1.Properties.Minimum = 50;
            this.trackBarControl1.Properties.ShowValueToolTip = true;
            this.trackBarControl1.Size = new System.Drawing.Size(1035, 42);
            this.trackBarControl1.TabIndex = 0;
            this.trackBarControl1.Value = 100;
            this.trackBarControl1.EditValueChanged += new System.EventHandler(this.trackBarControl1_EditValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.printPreviewControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 115);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1035, 438);
            this.panel1.TabIndex = 2;
            // 
            // printPreviewControl1
            // 
            this.printPreviewControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.printPreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPreviewControl1.Location = new System.Drawing.Point(0, 0);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new System.Drawing.Size(1035, 438);
            this.printPreviewControl1.TabIndex = 0;
            this.printPreviewControl1.Click += new System.EventHandler(this.printPreviewControl1_Click);
            // 
            // PrintForm
            // 
            this.AcceptButton = this.simpleButtonPrint;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(1035, 553);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnltop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrintForm";
            this.Text = "打印预览";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PrintForm_Load);
            this.pnltop.ResumeLayout(false);
            this.pnltop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPrintDuplex.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPageSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPrintCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPrinter.Properties)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}