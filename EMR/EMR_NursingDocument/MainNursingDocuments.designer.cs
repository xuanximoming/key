namespace DrectSoft.Core.EMR_NursingDocument
{
    partial class MainNursingDocuments
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainNursingDocuments));
            this.tabNursingDocuments = new DevExpress.XtraTab.XtraTabControl();
            this.tabPageVitalSignsRecord = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageInRecord = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageOutRecord = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageOtherRecord = new DevExpress.XtraTab.XtraTabPage();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.tabNursingDocuments)).BeginInit();
            this.tabNursingDocuments.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabNursingDocuments
            // 
            this.tabNursingDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabNursingDocuments.Location = new System.Drawing.Point(8, 8);
            this.tabNursingDocuments.Name = "tabNursingDocuments";
            this.tabNursingDocuments.SelectedTabPage = this.tabPageVitalSignsRecord;
            this.tabNursingDocuments.Size = new System.Drawing.Size(922, 451);
            this.tabNursingDocuments.TabIndex = 0;
            this.tabNursingDocuments.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPageVitalSignsRecord,
            this.tabPageInRecord,
            this.tabPageOutRecord,
            this.tabPageOtherRecord});
            // 
            // tabPageVitalSignsRecord
            // 
            this.tabPageVitalSignsRecord.Name = "tabPageVitalSignsRecord";
            this.tabPageVitalSignsRecord.Size = new System.Drawing.Size(916, 422);
            this.tabPageVitalSignsRecord.Text = "生命体征";
            // 
            // tabPageInRecord
            // 
            this.tabPageInRecord.Name = "tabPageInRecord";
            this.tabPageInRecord.Size = new System.Drawing.Size(916, 422);
            this.tabPageInRecord.Text = "病人入量";
            // 
            // tabPageOutRecord
            // 
            this.tabPageOutRecord.Name = "tabPageOutRecord";
            this.tabPageOutRecord.Size = new System.Drawing.Size(916, 422);
            this.tabPageOutRecord.Text = "病人出量";
            // 
            // tabPageOtherRecord
            // 
            this.tabPageOtherRecord.Name = "tabPageOtherRecord";
            this.tabPageOtherRecord.Size = new System.Drawing.Size(916, 422);
            this.tabPageOtherRecord.Text = "其他";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(835, 465);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "作废";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(734, 465);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 27);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "新增";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.Location = new System.Drawing.Point(631, 465);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(87, 27);
            this.simpleButton1.TabIndex = 5;
            this.simpleButton1.Text = "体温单";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Location = new System.Drawing.Point(530, 465);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(87, 27);
            this.simpleButton2.TabIndex = 6;
            this.simpleButton2.Text = "刷新";
            // 
            // MainNursingDocuments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 499);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabNursingDocuments);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainNursingDocuments";
            this.Text = "护理文书";
            ((System.ComponentModel.ISupportInitialize)(this.tabNursingDocuments)).EndInit();
            this.tabNursingDocuments.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabNursingDocuments;
        private DevExpress.XtraTab.XtraTabPage tabPageVitalSignsRecord;
        private DevExpress.XtraTab.XtraTabPage tabPageInRecord;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraTab.XtraTabPage tabPageOutRecord;
        private DevExpress.XtraTab.XtraTabPage tabPageOtherRecord;
    }
}