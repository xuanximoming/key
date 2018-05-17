namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    partial class BaseDataMaintain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseDataMaintain));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonClose = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose();
            this.simpleButtonPYWBUpdate = new DevExpress.XtraEditors.SimpleButton();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.ucDictionary1 = new DrectSoft.Emr.TemplateFactory.BaseDataMaintain.UCDictionary();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.ucMacro1 = new DrectSoft.Emr.TemplateFactory.BaseDataMaintain.UCMacro();
            this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.ucSpecialCharacter1 = new DrectSoft.Emr.TemplateFactory.BaseDataMaintain.UCSpecialCharacter();
            this.XTOPERPAGE = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageDiag = new DevExpress.XtraTab.XtraTabPage();
            this.tabPageDiagChinese = new DevExpress.XtraTab.XtraTabPage();
            this.TabPageDeptDiag = new DevExpress.XtraTab.XtraTabPage();
            this.TabPageModel = new DevExpress.XtraTab.XtraTabPage();
            this.TabPageDiagButton = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            this.xtraTabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButtonClose);
            this.panelControl1.Controls.Add(this.simpleButtonPYWBUpdate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 545);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(985, 44);
            this.panelControl1.TabIndex = 1;
            // 
            // simpleButtonClose
            // 
            this.simpleButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonClose.Image")));
            this.simpleButtonClose.Location = new System.Drawing.Point(900, 8);
            this.simpleButtonClose.Name = "simpleButtonClose";
            this.simpleButtonClose.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonClose.TabIndex = 1;
            this.simpleButtonClose.Text = "关闭(&T)";
            this.simpleButtonClose.Click += new System.EventHandler(this.simpleButtonClose_Click);
            // 
            // simpleButtonPYWBUpdate
            // 
            this.simpleButtonPYWBUpdate.ImageIndex = 0;
            this.simpleButtonPYWBUpdate.ImageList = this.imageCollection1;
            this.simpleButtonPYWBUpdate.Location = new System.Drawing.Point(754, 8);
            this.simpleButtonPYWBUpdate.Name = "simpleButtonPYWBUpdate";
            this.simpleButtonPYWBUpdate.Size = new System.Drawing.Size(140, 27);
            this.simpleButtonPYWBUpdate.TabIndex = 0;
            this.simpleButtonPYWBUpdate.Text = "拼音、五笔批量更新";
            this.simpleButtonPYWBUpdate.UseWaitCursor = true;
            this.simpleButtonPYWBUpdate.Visible = false;
            this.simpleButtonPYWBUpdate.Click += new System.EventHandler(this.simpleButtonPYWBUpdate_Click);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "刷新.ico");
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.HeaderAutoFill = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControl1.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Left;
            this.xtraTabControl1.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(985, 545);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage4,
            this.XTOPERPAGE,
            this.tabPageDiag,
            this.tabPageDiagChinese,
            this.TabPageDeptDiag,
            this.TabPageModel,
            this.TabPageDiagButton,
            this.xtraTabPage3});
            this.xtraTabControl1.TabStop = false;
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.ucDictionary1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(893, 539);
            this.xtraTabPage1.Text = "常用字典";
            // 
            // ucDictionary1
            // 
            this.ucDictionary1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDictionary1.Host = null;
            this.ucDictionary1.Location = new System.Drawing.Point(0, 0);
            this.ucDictionary1.Name = "ucDictionary1";
            this.ucDictionary1.Size = new System.Drawing.Size(893, 539);
            this.ucDictionary1.TabIndex = 0;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.ucMacro1);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(893, 539);
            this.xtraTabPage2.Text = "常用词";
            // 
            // ucMacro1
            // 
            this.ucMacro1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMacro1.Host = null;
            this.ucMacro1.Location = new System.Drawing.Point(0, 0);
            this.ucMacro1.Name = "ucMacro1";
            this.ucMacro1.Size = new System.Drawing.Size(893, 539);
            this.ucMacro1.TabIndex = 0;
            // 
            // xtraTabPage4
            // 
            this.xtraTabPage4.Controls.Add(this.ucSpecialCharacter1);
            this.xtraTabPage4.Name = "xtraTabPage4";
            this.xtraTabPage4.Size = new System.Drawing.Size(893, 539);
            this.xtraTabPage4.Text = "特殊字符";
            // 
            // ucSpecialCharacter1
            // 
            this.ucSpecialCharacter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSpecialCharacter1.Host = null;
            this.ucSpecialCharacter1.Location = new System.Drawing.Point(0, 0);
            this.ucSpecialCharacter1.Name = "ucSpecialCharacter1";
            this.ucSpecialCharacter1.Size = new System.Drawing.Size(893, 539);
            this.ucSpecialCharacter1.TabIndex = 0;
            // 
            // XTOPERPAGE
            // 
            this.XTOPERPAGE.Name = "XTOPERPAGE";
            this.XTOPERPAGE.Size = new System.Drawing.Size(893, 539);
            this.XTOPERPAGE.Text = "手术信息维护";
            // 
            // tabPageDiag
            // 
            this.tabPageDiag.Name = "tabPageDiag";
            this.tabPageDiag.Size = new System.Drawing.Size(893, 539);
            this.tabPageDiag.Text = "西医诊断维护";
            // 
            // tabPageDiagChinese
            // 
            this.tabPageDiagChinese.Name = "tabPageDiagChinese";
            this.tabPageDiagChinese.Size = new System.Drawing.Size(893, 539);
            this.tabPageDiagChinese.Text = "中医诊断维护";
            // 
            // TabPageDeptDiag
            // 
            this.TabPageDeptDiag.Name = "TabPageDeptDiag";
            this.TabPageDeptDiag.Size = new System.Drawing.Size(893, 539);
            this.TabPageDeptDiag.Text = "科室常用病种";
            // 
            // TabPageModel
            // 
            this.TabPageModel.Name = "TabPageModel";
            this.TabPageModel.Size = new System.Drawing.Size(893, 539);
            this.TabPageModel.Text = "复用项目维护";
            // 
            // TabPageDiagButton
            // 
            this.TabPageDiagButton.Name = "TabPageDiagButton";
            this.TabPageDiagButton.Size = new System.Drawing.Size(893, 539);
            this.TabPageDiagButton.Text = "诊断按钮事件";
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(893, 539);
            this.xtraTabPage3.Text = "并发症维护";
            // 
            // BaseDataMaintain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 589);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaseDataMaintain";
            this.Text = "诊断手术维护";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage4;
        private UCDictionary ucDictionary1;
        private UCMacro ucMacro1;
        private UCSpecialCharacter ucSpecialCharacter1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonPYWBUpdate;
        private DevExpress.XtraTab.XtraTabPage tabPageDiag;
        private DevExpress.XtraTab.XtraTabPage TabPageDeptDiag;
        private DevExpress.XtraTab.XtraTabPage TabPageModel;
        private DevExpress.XtraTab.XtraTabPage TabPageDiagButton;
        private DevExpress.XtraTab.XtraTabPage tabPageDiagChinese;
        private DevExpress.XtraTab.XtraTabPage XTOPERPAGE;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClose simpleButtonClose;
        private DevExpress.Utils.ImageCollection imageCollection1;

    }
}