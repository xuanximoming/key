namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    partial class SearchFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchFrm));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageFind = new DevExpress.XtraTab.XtraTabPage();
            this.cmdSearch1 = new DevExpress.XtraEditors.SimpleButton();
            this.txtSearch1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabPageReplace = new DevExpress.XtraTab.XtraTabPage();
            this.cmdReplace = new DevExpress.XtraEditors.SimpleButton();
            this.cmdSearch2 = new DevExpress.XtraEditors.SimpleButton();
            this.txtReplace = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtSearch2 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPageFind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch1.Properties)).BeginInit();
            this.xtraTabPageReplace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtReplace.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPageFind;
            this.xtraTabControl1.Size = new System.Drawing.Size(292, 167);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageFind,
            this.xtraTabPageReplace});
            // 
            // xtraTabPageFind
            // 
            this.xtraTabPageFind.Controls.Add(this.cmdSearch1);
            this.xtraTabPageFind.Controls.Add(this.txtSearch1);
            this.xtraTabPageFind.Controls.Add(this.labelControl1);
            this.xtraTabPageFind.Name = "xtraTabPageFind";
            this.xtraTabPageFind.Size = new System.Drawing.Size(286, 138);
            this.xtraTabPageFind.Text = "查找";
            // 
            // cmdSearch1
            // 
            this.cmdSearch1.Location = new System.Drawing.Point(198, 87);
            this.cmdSearch1.Name = "cmdSearch1";
            this.cmdSearch1.Size = new System.Drawing.Size(75, 23);
            this.cmdSearch1.TabIndex = 1;
            this.cmdSearch1.Text = "查找下一个";
            this.cmdSearch1.Click += new System.EventHandler(this.cmdSearch1_Click);
            // 
            // txtSearch1
            // 
            this.txtSearch1.Location = new System.Drawing.Point(11, 42);
            this.txtSearch1.Name = "txtSearch1";
            this.txtSearch1.Size = new System.Drawing.Size(261, 20);
            this.txtSearch1.TabIndex = 0;
            this.txtSearch1.EditValueChanged += new System.EventHandler(this.txtSearch1_EditValueChanged);
            this.txtSearch1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch1_KeyDown);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(11, 16);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "查找内容:";
            // 
            // xtraTabPageReplace
            // 
            this.xtraTabPageReplace.Controls.Add(this.cmdReplace);
            this.xtraTabPageReplace.Controls.Add(this.cmdSearch2);
            this.xtraTabPageReplace.Controls.Add(this.txtReplace);
            this.xtraTabPageReplace.Controls.Add(this.labelControl3);
            this.xtraTabPageReplace.Controls.Add(this.txtSearch2);
            this.xtraTabPageReplace.Controls.Add(this.labelControl2);
            this.xtraTabPageReplace.Name = "xtraTabPageReplace";
            this.xtraTabPageReplace.Size = new System.Drawing.Size(286, 138);
            this.xtraTabPageReplace.Text = "替换";
            // 
            // cmdReplace
            // 
            this.cmdReplace.Location = new System.Drawing.Point(199, 108);
            this.cmdReplace.Name = "cmdReplace";
            this.cmdReplace.Size = new System.Drawing.Size(75, 23);
            this.cmdReplace.TabIndex = 7;
            this.cmdReplace.Text = "替换";
            this.cmdReplace.Click += new System.EventHandler(this.cmdReplace_Click);
            this.cmdReplace.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmdReplace_KeyDown);
            // 
            // cmdSearch2
            // 
            this.cmdSearch2.Location = new System.Drawing.Point(116, 108);
            this.cmdSearch2.Name = "cmdSearch2";
            this.cmdSearch2.Size = new System.Drawing.Size(75, 23);
            this.cmdSearch2.TabIndex = 6;
            this.cmdSearch2.Text = "查找下一个";
            this.cmdSearch2.Click += new System.EventHandler(this.cmdSearch2_Click);
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(12, 77);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(261, 20);
            this.txtReplace.TabIndex = 5;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 57);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(40, 14);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "替换为:";
            // 
            // txtSearch2
            // 
            this.txtSearch2.Location = new System.Drawing.Point(12, 28);
            this.txtSearch2.Name = "txtSearch2";
            this.txtSearch2.Size = new System.Drawing.Size(261, 20);
            this.txtSearch2.TabIndex = 3;
            this.txtSearch2.EditValueChanged += new System.EventHandler(this.txtSearch2_EditValueChanged);
            this.txtSearch2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch2_KeyDown);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 8);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(52, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "查找内容:";
            // 
            // SearchFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 167);
            this.Controls.Add(this.xtraTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchFrm";
            this.Text = "查找和替换";
            this.Activated += new System.EventHandler(this.SearchFrm_Activated);
            this.Load += new System.EventHandler(this.SearchFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPageFind.ResumeLayout(false);
            this.xtraTabPageFind.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch1.Properties)).EndInit();
            this.xtraTabPageReplace.ResumeLayout(false);
            this.xtraTabPageReplace.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtReplace.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch2.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageFind;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageReplace;
        private DevExpress.XtraEditors.TextEdit txtSearch1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton cmdSearch1;
        private DevExpress.XtraEditors.TextEdit txtSearch2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtReplace;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton cmdReplace;
        private DevExpress.XtraEditors.SimpleButton cmdSearch2;
    }
}