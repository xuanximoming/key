namespace DrectSoft.Emr.QcManager
{
    partial class QCScoreTypeEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QCScoreTypeEdit));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txttypename = new DevExpress.XtraEditors.TextEdit();
            this.txttypeinstruction = new DevExpress.XtraEditors.TextEdit();
            this.txttypememo = new DevExpress.XtraEditors.TextEdit();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.txttypeorder = new DevExpress.XtraEditors.SpinEdit();
            this.cmbtypecategory = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txttypename.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txttypeinstruction.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txttypememo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txttypeorder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbtypecategory.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(39, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "项目名称：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(282, 24);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "项目说明：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(39, 75);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "评分类别：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(282, 75);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 8;
            this.labelControl4.Text = "排列顺序：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(39, 126);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(64, 14);
            this.labelControl5.TabIndex = 9;
            this.labelControl5.Text = "备       注：";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::DrectSoft.Emr.QcManager.Properties.Resources.保存;
            this.btnSave.Location = new System.Drawing.Point(282, 168);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 27);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txttypename
            // 
            this.txttypename.Location = new System.Drawing.Point(106, 24);
            this.txttypename.Name = "txttypename";
            this.txttypename.Size = new System.Drawing.Size(120, 20);
            this.txttypename.TabIndex = 0;
            this.txttypename.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // txttypeinstruction
            // 
            this.txttypeinstruction.Location = new System.Drawing.Point(348, 24);
            this.txttypeinstruction.Name = "txttypeinstruction";
            this.txttypeinstruction.Size = new System.Drawing.Size(120, 20);
            this.txttypeinstruction.TabIndex = 1;
            this.txttypeinstruction.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // txttypememo
            // 
            this.txttypememo.Location = new System.Drawing.Point(106, 123);
            this.txttypememo.Name = "txttypememo";
            this.txttypememo.Size = new System.Drawing.Size(362, 20);
            this.txttypememo.TabIndex = 4;
            this.txttypememo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // btnClear
            // 
            this.btnClear.Image = global::DrectSoft.Emr.QcManager.Properties.Resources.重置;
            this.btnClear.Location = new System.Drawing.Point(388, 168);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 27);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "重置(&B)";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txttypeorder
            // 
            this.txttypeorder.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txttypeorder.Location = new System.Drawing.Point(348, 72);
            this.txttypeorder.Name = "txttypeorder";
            this.txttypeorder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txttypeorder.Size = new System.Drawing.Size(120, 20);
            this.txttypeorder.TabIndex = 3;
            this.txttypeorder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // cmbtypecategory
            // 
            this.cmbtypecategory.Location = new System.Drawing.Point(106, 72);
            this.cmbtypecategory.Name = "cmbtypecategory";
            this.cmbtypecategory.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbtypecategory.Properties.Items.AddRange(new object[] {
            "个人",
            "科室",
            "医务处"});
            this.cmbtypecategory.Size = new System.Drawing.Size(120, 20);
            this.cmbtypecategory.TabIndex = 2;
            this.cmbtypecategory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // QCScoreTypeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 206);
            this.Controls.Add(this.cmbtypecategory);
            this.Controls.Add(this.txttypeorder);
            this.Controls.Add(this.txttypememo);
            this.Controls.Add(this.txttypeinstruction);
            this.Controls.Add(this.txttypename);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "QCScoreTypeEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "标准评分大项详细维护";
            this.Load += new System.EventHandler(this.QCScoreTypeEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txttypename.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txttypeinstruction.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txttypememo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txttypeorder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbtypecategory.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.TextEdit txttypename;
        private DevExpress.XtraEditors.TextEdit txttypeinstruction;
        private DevExpress.XtraEditors.TextEdit txttypememo;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.SpinEdit txttypeorder;
        private DevExpress.XtraEditors.ComboBoxEdit cmbtypecategory;
    }
}