namespace DrectSoft.EMR.ThreeRecordAll
{
    partial class ThreeRecordMainZT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreeRecordMainZT));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.scrolThreeRecord = new DevExpress.XtraEditors.XtraScrollableControl();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(272, 560);
            this.panelControl1.TabIndex = 0;
            // 
            // scrolThreeRecord
            // 
            this.scrolThreeRecord.Appearance.BackColor = System.Drawing.Color.White;
            this.scrolThreeRecord.Appearance.Options.UseBackColor = true;
            this.scrolThreeRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrolThreeRecord.Location = new System.Drawing.Point(277, 0);
            this.scrolThreeRecord.Name = "scrolThreeRecord";
            this.scrolThreeRecord.Size = new System.Drawing.Size(720, 560);
            this.scrolThreeRecord.TabIndex = 2;
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(272, 0);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 560);
            this.splitterControl1.TabIndex = 2;
            this.splitterControl1.TabStop = false;
            // 
            // ThreeRecordMainZT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 560);
            this.Controls.Add(this.scrolThreeRecord);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ThreeRecordMainZT";
            this.Text = "三测单整体录入";
            this.Load += new System.EventHandler(this.ThreeRecordMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.XtraScrollableControl scrolThreeRecord;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
    }
}