namespace DrectSoft.Core.CommonTableConfig
{
    partial class DataElementInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataElementInfo));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cboElementClass = new System.Windows.Forms.ComboBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.chboxIsDataElement = new System.Windows.Forms.CheckBox();
            this.cboElementType = new System.Windows.Forms.ComboBox();
            this.medOptins = new DevExpress.XtraEditors.MemoEdit();
            this.medElementDescribe = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.txtElementId = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtElementName = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.txtElementForm = new DrectSoft.Common.Ctrs.OTHER.DevTextEdit();
            this.btnLoad = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave();
            this.btnClose = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose();
            ((System.ComponentModel.ISupportInitialize)(this.medOptins.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.medElementDescribe.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementForm.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(58, 33);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "数据元ID：";
            // 
            // cboElementClass
            // 
            this.cboElementClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboElementClass.FormattingEnabled = true;
            this.cboElementClass.Location = new System.Drawing.Point(348, 70);
            this.cboElementClass.MaxDropDownItems = 16;
            this.cboElementClass.MaxLength = 50;
            this.cboElementClass.Name = "cboElementClass";
            this.cboElementClass.Size = new System.Drawing.Size(128, 22);
            this.cboElementClass.TabIndex = 5;
            this.cboElementClass.SelectedValueChanged += new System.EventHandler(this.cboElementClass_SelectedValueChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 135);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(106, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "数据项目（XML）：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(272, 33);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 14);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "数据元名称：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(503, 33);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "数据类型：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(58, 74);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 14);
            this.labelControl5.TabIndex = 8;
            this.labelControl5.Text = "数据格式：";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(284, 74);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 14);
            this.labelControl6.TabIndex = 10;
            this.labelControl6.Text = "所属类别：";
            // 
            // chboxIsDataElement
            // 
            this.chboxIsDataElement.AutoSize = true;
            this.chboxIsDataElement.Checked = true;
            this.chboxIsDataElement.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chboxIsDataElement.Location = new System.Drawing.Point(506, 72);
            this.chboxIsDataElement.Name = "chboxIsDataElement";
            this.chboxIsDataElement.Size = new System.Drawing.Size(98, 18);
            this.chboxIsDataElement.TabIndex = 6;
            this.chboxIsDataElement.Text = "卫生部数据元";
            this.chboxIsDataElement.UseVisualStyleBackColor = true;
            // 
            // cboElementType
            // 
            this.cboElementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboElementType.FormattingEnabled = true;
            this.cboElementType.Location = new System.Drawing.Point(564, 25);
            this.cboElementType.MaxDropDownItems = 16;
            this.cboElementType.MaxLength = 50;
            this.cboElementType.Name = "cboElementType";
            this.cboElementType.Size = new System.Drawing.Size(130, 22);
            this.cboElementType.TabIndex = 3;
            // 
            // medOptins
            // 
            this.medOptins.Location = new System.Drawing.Point(122, 133);
            this.medOptins.Name = "medOptins";
            this.medOptins.Properties.MaxLength = 4000;
            this.medOptins.Size = new System.Drawing.Size(570, 91);
            this.medOptins.TabIndex = 8;
            this.medOptins.UseOptimizedRendering = true;
            // 
            // medElementDescribe
            // 
            this.medElementDescribe.Location = new System.Drawing.Point(122, 242);
            this.medElementDescribe.Name = "medElementDescribe";
            this.medElementDescribe.Properties.MaxLength = 4000;
            this.medElementDescribe.Size = new System.Drawing.Size(570, 91);
            this.medElementDescribe.TabIndex = 9;
            this.medElementDescribe.UseOptimizedRendering = true;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(46, 244);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(72, 14);
            this.labelControl7.TabIndex = 17;
            this.labelControl7.Text = "数据元描述：";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl9.Location = new System.Drawing.Point(259, 35);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(7, 14);
            this.labelControl9.TabIndex = 21;
            this.labelControl9.Text = "*";
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl10.Location = new System.Drawing.Point(480, 35);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(7, 14);
            this.labelControl10.TabIndex = 22;
            this.labelControl10.Text = "*";
            // 
            // labelControl11
            // 
            this.labelControl11.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl11.Location = new System.Drawing.Point(700, 35);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(7, 14);
            this.labelControl11.TabIndex = 23;
            this.labelControl11.Text = "*";
            // 
            // labelControl12
            // 
            this.labelControl12.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl12.Location = new System.Drawing.Point(480, 77);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(7, 14);
            this.labelControl12.TabIndex = 24;
            this.labelControl12.Text = "*";
            // 
            // txtElementId
            // 
            this.txtElementId.EnterMoveNextControl = true;
            this.txtElementId.IsEnterChangeBgColor = false;
            this.txtElementId.IsEnterKeyToNextControl = false;
            this.txtElementId.IsNumber = false;
            this.txtElementId.Location = new System.Drawing.Point(122, 29);
            this.txtElementId.Name = "txtElementId";
            this.txtElementId.Size = new System.Drawing.Size(130, 20);
            this.txtElementId.TabIndex = 1;
            // 
            // txtElementName
            // 
            this.txtElementName.EnterMoveNextControl = true;
            this.txtElementName.IsEnterChangeBgColor = false;
            this.txtElementName.IsEnterKeyToNextControl = false;
            this.txtElementName.IsNumber = false;
            this.txtElementName.Location = new System.Drawing.Point(348, 29);
            this.txtElementName.Name = "txtElementName";
            this.txtElementName.Size = new System.Drawing.Size(130, 20);
            this.txtElementName.TabIndex = 2;
            // 
            // txtElementForm
            // 
            this.txtElementForm.EnterMoveNextControl = true;
            this.txtElementForm.IsEnterChangeBgColor = false;
            this.txtElementForm.IsEnterKeyToNextControl = false;
            this.txtElementForm.IsNumber = false;
            this.txtElementForm.Location = new System.Drawing.Point(122, 71);
            this.txtElementForm.Name = "txtElementForm";
            this.txtElementForm.Size = new System.Drawing.Size(130, 20);
            this.txtElementForm.TabIndex = 4;
            // 
            // btnLoad
            // 
            this.btnLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnLoad.Image")));
            this.btnLoad.Location = new System.Drawing.Point(619, 104);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 7;
            this.btnLoad.Text = "导入XML";
            this.btnLoad.Click += new System.EventHandler(this.hyLoadType_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(528, 343);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存(&S)";
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(614, 343);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "关闭(&T)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DataElementInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 378);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtElementForm);
            this.Controls.Add(this.txtElementName);
            this.Controls.Add(this.txtElementId);
            this.Controls.Add(this.labelControl12);
            this.Controls.Add(this.labelControl11);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.medElementDescribe);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.medOptins);
            this.Controls.Add(this.cboElementType);
            this.Controls.Add(this.chboxIsDataElement);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cboElementClass);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataElementInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.UCDataElementInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.medOptins.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.medElementDescribe.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementForm.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.ComboBox cboElementClass;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private System.Windows.Forms.CheckBox chboxIsDataElement;
        private System.Windows.Forms.ComboBox cboElementType;
        private DevExpress.XtraEditors.MemoEdit medOptins;
        private DevExpress.XtraEditors.MemoEdit medElementDescribe;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtElementId;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtElementName;
        private DrectSoft.Common.Ctrs.OTHER.DevTextEdit txtElementForm;
        private DevExpress.XtraEditors.SimpleButton btnLoad;
        public DrectSoft.Common.Ctrs.OTHER.DevButtonSave btnSave;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClose btnClose;
    }
}
