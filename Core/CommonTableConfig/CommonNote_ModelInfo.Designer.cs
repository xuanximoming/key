namespace CommonTableConfig
{
    partial class CommonNote_ModelInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonNote_ModelInfo));
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtTempName = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.labelFileName = new DevExpress.XtraEditors.LabelControl();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose();
            this.btnSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave();
            this.txtSearch = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtDesc = new DevExpress.XtraEditors.MemoEdit();
            this.labelDesc = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTempName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.txtTempName);
            this.panelControl2.Controls.Add(this.labelFileName);
            this.panelControl2.Controls.Add(this.btnSearch);
            this.panelControl2.Controls.Add(this.btnClose);
            this.panelControl2.Controls.Add(this.btnSave);
            this.panelControl2.Controls.Add(this.txtSearch);
            this.panelControl2.Controls.Add(this.txtDesc);
            this.panelControl2.Controls.Add(this.labelDesc);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(564, 361);
            this.panelControl2.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl3.Location = new System.Drawing.Point(508, 49);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(7, 14);
            this.labelControl3.TabIndex = 11;
            this.labelControl3.Text = "*";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(508, 15);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(7, 14);
            this.labelControl2.TabIndex = 10;
            this.labelControl2.Text = "*";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(35, 274);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "路径：";
            // 
            // txtTempName
            // 
            this.txtTempName.Enabled = false;
            this.txtTempName.EnterMoveNextControl = true;
            this.txtTempName.IsEnterChangeBgColor = false;
            this.txtTempName.IsEnterKeyToNextControl = false;
            this.txtTempName.IsNumber = false;
            this.txtTempName.Location = new System.Drawing.Point(77, 12);
            this.txtTempName.Name = "txtTempName";
            this.txtTempName.Properties.MaxLength = 25;
            this.txtTempName.Size = new System.Drawing.Size(425, 20);
            this.txtTempName.TabIndex = 1;
            this.txtTempName.ToolTip = "文件名，长度小于50";
            // 
            // labelFileName
            // 
            this.labelFileName.Location = new System.Drawing.Point(23, 15);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(48, 14);
            this.labelFileName.TabIndex = 7;
            this.labelFileName.Text = "文件名：";
            // 
            // btnSearch
            // 
            this.btnSearch.Image = global::DrectSoft.Core.CommonTableConfig.Properties.Resources.DevButtonQurey;
            this.btnSearch.Location = new System.Drawing.Point(427, 269);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "浏览(&F)";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(427, 319);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "关闭(&T)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(329, 319);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.EnterMoveNextControl = true;
            this.txtSearch.IsEnterChangeBgColor = false;
            this.txtSearch.IsEnterKeyToNextControl = false;
            this.txtSearch.IsNumber = false;
            this.txtSearch.Location = new System.Drawing.Point(77, 271);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Properties.ReadOnly = true;
            this.txtSearch.Size = new System.Drawing.Size(332, 20);
            this.txtSearch.TabIndex = 5;
            this.txtSearch.ToolTip = "文件路径";
            // 
            // txtDesc
            // 
            this.txtDesc.EnterMoveNextControl = true;
            this.txtDesc.Location = new System.Drawing.Point(77, 46);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Properties.MaxLength = 2000;
            this.txtDesc.Size = new System.Drawing.Size(425, 190);
            this.txtDesc.TabIndex = 2;
            this.txtDesc.ToolTip = "文件描述";
            this.txtDesc.UseOptimizedRendering = true;
            // 
            // labelDesc
            // 
            this.labelDesc.Location = new System.Drawing.Point(35, 49);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(36, 14);
            this.labelDesc.TabIndex = 0;
            this.labelDesc.Text = "描述：";
            // 
            // CommonNote_ModelInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 361);
            this.Controls.Add(this.panelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommonNote_ModelInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CommonNote_ModelInfo";
            this.Load += new System.EventHandler(this.CommonNote_ModelInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTempName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelDesc;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClose btnClose;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnSave;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtSearch;
        private DevExpress.XtraEditors.MemoEdit txtDesc;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtTempName;
        private DevExpress.XtraEditors.LabelControl labelFileName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}