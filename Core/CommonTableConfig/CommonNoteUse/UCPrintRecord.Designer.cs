namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    partial class UCPrintRecord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCPrintRecord));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnPrintCurrent = new DevExpress.XtraEditors.SimpleButton();
            this.btnRefresh = new DrectSoft.Common.Ctrs.OTHER.DevButtonRefresh(this.components);
            this.spinEditLineCount = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.xPrint = new XDesigner.Report.XPrintControlExt();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditLineCount.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btnPrintCurrent);
            this.panelControl1.Controls.Add(this.btnRefresh);
            this.panelControl1.Controls.Add(this.spinEditLineCount);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(526, 32);
            this.panelControl1.TabIndex = 3;
            // 
            // btnPrintCurrent
            // 
            this.btnPrintCurrent.Image = ((System.Drawing.Image)(resources.GetObject("btnPrintCurrent.Image")));
            this.btnPrintCurrent.Location = new System.Drawing.Point(213, 5);
            this.btnPrintCurrent.Name = "btnPrintCurrent";
            this.btnPrintCurrent.Size = new System.Drawing.Size(107, 23);
            this.btnPrintCurrent.TabIndex = 3;
            this.btnPrintCurrent.Text = "打印当前页(&P)";
            this.btnPrintCurrent.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(124, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新(&R)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // spinEditLineCount
            // 
            this.spinEditLineCount.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEditLineCount.Location = new System.Drawing.Point(72, 5);
            this.spinEditLineCount.Name = "spinEditLineCount";
            this.spinEditLineCount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEditLineCount.Properties.IsFloatValue = false;
            this.spinEditLineCount.Properties.Mask.EditMask = "N00";
            this.spinEditLineCount.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.spinEditLineCount.Size = new System.Drawing.Size(42, 21);
            this.spinEditLineCount.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 8);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "添加空行：";
            // 
            // xPrint
            // 
            this.xPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xPrint.LabelEditHotKeysString = "Enter, F2";
            this.xPrint.Location = new System.Drawing.Point(0, 32);
            this.xPrint.Name = "xPrint";
            this.xPrint.RefreshButtonVisible = true;
            this.xPrint.ShowVersionInfo = true;
            this.xPrint.Size = new System.Drawing.Size(526, 494);
            this.xPrint.TabIndex = 2;
            this.xPrint.ToolbarVisible = true;
            // 
            // UCPrintRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xPrint);
            this.Controls.Add(this.panelControl1);
            this.Name = "UCPrintRecord";
            this.Size = new System.Drawing.Size(526, 526);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditLineCount.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private XDesigner.Report.XPrintControlExt xPrint;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SpinEdit spinEditLineCount;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonRefresh btnRefresh;
        private DevExpress.XtraEditors.SimpleButton btnPrintCurrent;
    }
}
