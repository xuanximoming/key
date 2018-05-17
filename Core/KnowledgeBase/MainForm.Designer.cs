namespace DrectSoft.Core.KnowledgeBase
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabpageMedicine = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageMedicine = new DevExpress.XtraTab.XtraTabPage();
            this.tabpageMedicineDirect = new DevExpress.XtraTab.XtraTabPage();
            this.tablePageTreatment = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.tabpageMedicine)).BeginInit();
            this.tabpageMedicine.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabpageMedicine
            // 
            this.tabpageMedicine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabpageMedicine.Location = new System.Drawing.Point(0, 0);
            this.tabpageMedicine.Name = "tabpageMedicine";
            this.tabpageMedicine.SelectedTabPage = this.xtraTabPageMedicine;
            this.tabpageMedicine.Size = new System.Drawing.Size(982, 599);
            this.tabpageMedicine.TabIndex = 0;
            this.tabpageMedicine.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageMedicine,
            this.tabpageMedicineDirect,
            this.tablePageTreatment});
            this.tabpageMedicine.TabStop = false;
            this.tabpageMedicine.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // xtraTabPageMedicine
            // 
            this.xtraTabPageMedicine.Name = "xtraTabPageMedicine";
            this.xtraTabPageMedicine.Size = new System.Drawing.Size(976, 570);
            this.xtraTabPageMedicine.Text = "药品咨询";
            // 
            // tabpageMedicineDirect
            // 
            this.tabpageMedicineDirect.Name = "tabpageMedicineDirect";
            this.tabpageMedicineDirect.Size = new System.Drawing.Size(976, 570);
            this.tabpageMedicineDirect.Text = "药品说明书";
            // 
            // tablePageTreatment
            // 
            this.tablePageTreatment.Name = "tablePageTreatment";
            this.tablePageTreatment.Size = new System.Drawing.Size(976, 570);
            this.tablePageTreatment.Text = "诊疗方案";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 599);
            this.Controls.Add(this.tabpageMedicine);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "临床知识库";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabpageMedicine)).EndInit();
            this.tabpageMedicine.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabpageMedicine;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageMedicine;
        private DevExpress.XtraTab.XtraTabPage tabpageMedicineDirect;
        private DevExpress.XtraTab.XtraTabPage tablePageTreatment;
    }
}