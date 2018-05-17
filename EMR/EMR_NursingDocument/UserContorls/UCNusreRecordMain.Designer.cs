namespace DrectSoft.Core.EMR_NursingDocument.UserContorls
{
    partial class UCNusreRecordMain
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
            this.xPrint = new XDesigner.Report.XPrintControlExt();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btnLuRu = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // xPrint
            // 
            this.xPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xPrint.LabelEditHotKeysString = "Enter, F2";
            this.xPrint.Location = new System.Drawing.Point(0, 64);
            this.xPrint.Name = "xPrint";
            this.xPrint.RefreshButtonVisible = true;
            this.xPrint.ShowVersionInfo = true;
            this.xPrint.Size = new System.Drawing.Size(929, 527);
            this.xPrint.TabIndex = 0;
            this.xPrint.ToolbarVisible = true;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.btnPrint);
            this.panelControl3.Controls.Add(this.btnLuRu);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(929, 64);
            this.panelControl3.TabIndex = 38;
            // 
            // btnLuRu
            // 
            this.btnLuRu.Location = new System.Drawing.Point(39, 22);
            this.btnLuRu.Name = "btnLuRu";
            this.btnLuRu.Size = new System.Drawing.Size(75, 23);
            this.btnLuRu.TabIndex = 0;
            this.btnLuRu.Text = "录入";
            this.btnLuRu.Click += new System.EventHandler(this.btnLuRu_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(151, 22);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "打印";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // UCNusreOperRecordMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xPrint);
            this.Controls.Add(this.panelControl3);
            this.Name = "UCNusreOperRecordMain";
            this.Size = new System.Drawing.Size(929, 591);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private XDesigner.Report.XPrintControlExt xPrint;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton btnLuRu;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
    }
}
