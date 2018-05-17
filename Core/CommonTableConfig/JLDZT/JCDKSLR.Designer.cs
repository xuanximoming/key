namespace DrectSoft.Core.CommonTableConfig.JLDZT
{
    partial class JCDKSLR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JCDKSLR));
            this.tabControl = new DevExpress.XtraTab.XtraTabControl();
            this.tabpageAdd = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageAll = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabPage = this.tabpageAdd;
            this.tabControl.Size = new System.Drawing.Size(816, 507);
            this.tabControl.TabIndex = 0;
            this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPageAll,
            this.tabpageAdd});
            // 
            // tabpageAdd
            // 
            this.tabpageAdd.Appearance.PageClient.BackColor = System.Drawing.Color.White;
            this.tabpageAdd.Appearance.PageClient.Options.UseBackColor = true;
            this.tabpageAdd.Name = "tabpageAdd";
            this.tabpageAdd.Size = new System.Drawing.Size(810, 478);
            this.tabpageAdd.Text = "新增记录";
            // 
            // tabPageAll
            // 
            this.tabPageAll.Name = "tabPageAll";
            this.tabPageAll.Size = new System.Drawing.Size(810, 478);
            this.tabPageAll.Text = "当天记录";
            // 
            // JCDKSLR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 507);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "JCDKSLR";
            this.Text = "JCDKSLR";
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabControl;
        private DevExpress.XtraTab.XtraTabPage tabpageAdd;
        private DevExpress.XtraTab.XtraTabPage tabPageAll;
    }
}