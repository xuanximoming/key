namespace DrectSoft.Core.PersonTtemTemplet
{
    partial class ItemCatalog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemCatalog));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton2 = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel();
            this.simpleButton1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.chkperson = new DevExpress.XtraEditors.CheckEdit();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController();
            this.chkdept = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditor1 = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindow1 = new DrectSoft.Common.Library.LookUpWindow();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.memoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.txtITEMNAME = new DevExpress.XtraEditors.TextEdit();
            this.txtCreateUser = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkperson.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkdept.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtITEMNAME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateUser.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 237);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(349, 36);
            this.panelControl1.TabIndex = 1;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(257, 5);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(80, 27);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "取消(&C)";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(171, 5);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(80, 27);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "确定(&Y)";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.chkperson);
            this.groupControl1.Controls.Add(this.chkdept);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.lookUpEditor1);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.memoEdit);
            this.groupControl1.Controls.Add(this.txtITEMNAME);
            this.groupControl1.Controls.Add(this.txtCreateUser);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(349, 237);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "模板分类";
            // 
            // chkperson
            // 
            this.chkperson.Location = new System.Drawing.Point(91, 166);
            this.chkperson.Name = "chkperson";
            this.chkperson.Properties.Caption = "个人小模板";
            this.chkperson.Properties.RadioGroupIndex = 0;
            this.chkperson.Size = new System.Drawing.Size(105, 19);
            this.chkperson.TabIndex = 4;
            this.chkperson.TabStop = false;
            this.chkperson.ToolTipController = this.toolTipController1;
            this.chkperson.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chkperson_MouseMove);
            // 
            // toolTipController1
            // 
            this.toolTipController1.AutoPopDelay = 10000;
            this.toolTipController1.InitialDelay = 10;
            this.toolTipController1.Rounded = true;
            this.toolTipController1.ToolTipLocation = DevExpress.Utils.ToolTipLocation.RightTop;
            this.toolTipController1.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            // 
            // chkdept
            // 
            this.chkdept.Location = new System.Drawing.Point(202, 166);
            this.chkdept.Name = "chkdept";
            this.chkdept.Properties.Caption = "科室小模板";
            this.chkdept.Properties.RadioGroupIndex = 0;
            this.chkdept.Size = new System.Drawing.Size(112, 19);
            this.chkdept.TabIndex = 3;
            this.chkdept.TabStop = false;
            this.chkdept.ToolTipController = this.toolTipController1;
            this.chkdept.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chkdept_MouseMove);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(36, 203);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 10;
            this.labelControl5.Text = "创建人";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(24, 169);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "模板类型";
            // 
            // lookUpEditor1
            // 
            this.lookUpEditor1.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditor1.ListWindow = this.lookUpWindow1;
            this.lookUpEditor1.Location = new System.Drawing.Point(93, 132);
            this.lookUpEditor1.Name = "lookUpEditor1";
            this.lookUpEditor1.ShowSButton = true;
            this.lookUpEditor1.Size = new System.Drawing.Size(210, 18);
            this.lookUpEditor1.TabIndex = 2;
            this.lookUpEditor1.ToolTipController = this.toolTipController1;
            this.lookUpEditor1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lookUpEditor1_MouseMove);
            // 
            // lookUpWindow1
            // 
            this.lookUpWindow1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindow1.GenShortCode = null;
            this.lookUpWindow1.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindow1.Owner = null;
            this.lookUpWindow1.SqlHelper = null;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 135);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 8;
            this.labelControl4.Text = "适应病历夹";
            // 
            // memoEdit
            // 
            this.memoEdit.Location = new System.Drawing.Point(93, 68);
            this.memoEdit.Name = "memoEdit";
            this.memoEdit.Properties.MaxLength = 255;
            this.memoEdit.Size = new System.Drawing.Size(210, 45);
            this.memoEdit.TabIndex = 1;
            this.memoEdit.UseOptimizedRendering = true;
            // 
            // txtITEMNAME
            // 
            this.txtITEMNAME.Location = new System.Drawing.Point(93, 30);
            this.txtITEMNAME.Name = "txtITEMNAME";
            this.txtITEMNAME.Size = new System.Drawing.Size(210, 20);
            this.txtITEMNAME.TabIndex = 0;
            // 
            // txtCreateUser
            // 
            this.txtCreateUser.Enabled = false;
            this.txtCreateUser.Location = new System.Drawing.Point(93, 199);
            this.txtCreateUser.Name = "txtCreateUser";
            this.txtCreateUser.Properties.MaxLength = 12;
            this.txtCreateUser.Size = new System.Drawing.Size(210, 20);
            this.txtCreateUser.TabIndex = 5;
            this.txtCreateUser.TabStop = false;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(50, 85);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "备注";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "分类名称";
            // 
            // ItemCatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 273);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ItemCatalog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "模板分类";
            this.Load += new System.EventHandler(this.ItemCatalog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkperson.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkdept.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtITEMNAME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateUser.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit memoEdit;
        private DevExpress.XtraEditors.TextEdit txtCreateUser;
        private DrectSoft.Common.Library.LookUpEditor lookUpEditor1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DrectSoft.Common.Library.LookUpWindow lookUpWindow1;
        private DevExpress.XtraEditors.CheckEdit chkperson;
        private DevExpress.XtraEditors.CheckEdit chkdept;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtITEMNAME;
        private DevExpress.Utils.ToolTipController toolTipController1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel simpleButton2;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK simpleButton1;
    }
}