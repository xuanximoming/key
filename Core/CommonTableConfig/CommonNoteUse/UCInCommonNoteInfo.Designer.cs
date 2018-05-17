namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    partial class UCInCommonNote
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
            this.tabcontrol = new DevExpress.XtraTab.XtraTabControl();
            this.tabPagePrint = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.tabcontrol)).BeginInit();
            this.tabcontrol.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabcontrol
            // 
            this.tabcontrol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabcontrol.Location = new System.Drawing.Point(0, 0);
            this.tabcontrol.Name = "tabcontrol";
            this.tabcontrol.SelectedTabPage = this.tabPagePrint;
            this.tabcontrol.Size = new System.Drawing.Size(982, 593);
            this.tabcontrol.TabIndex = 0;
            this.tabcontrol.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPagePrint});
            this.tabcontrol.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tabcontrol_SelectedPageChanged);
            // 
            // tabPagePrint
            // 
            this.tabPagePrint.Name = "tabPagePrint";
            this.tabPagePrint.Size = new System.Drawing.Size(976, 565);
            this.tabPagePrint.Text = "打印预览";
            // 
            // UCInCommonNote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabcontrol);
            this.Name = "UCInCommonNote";
            this.Size = new System.Drawing.Size(982, 593);
            ((System.ComponentModel.ISupportInitialize)(this.tabcontrol)).EndInit();
            this.tabcontrol.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabcontrol;
        private DevExpress.XtraTab.XtraTabPage tabPagePrint;
    }
}
