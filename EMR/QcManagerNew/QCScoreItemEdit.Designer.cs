namespace YindanSoft.Emr.QcManagerNew
{
    partial class QCScoreItemEdit
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
            this.components = new System.ComponentModel.Container();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.txtname = new DevExpress.XtraEditors.TextEdit();
            this.txtInstruction = new DevExpress.XtraEditors.TextEdit();
            this.txtmemo = new DevExpress.XtraEditors.TextEdit();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.spinDefaultScore = new DevExpress.XtraEditors.SpinEdit();
            this.spinStandardScore = new DevExpress.XtraEditors.SpinEdit();
            this.spinDefaultTarget = new DevExpress.XtraEditors.SpinEdit();
            this.spinTargetStandard = new DevExpress.XtraEditors.SpinEdit();
            this.spinScoreStandard = new DevExpress.XtraEditors.SpinEdit();
            this.spinOrder = new DevExpress.XtraEditors.SpinEdit();
            this.cmbcategory = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lookUpWindowType = new YidanSoft.Common.Library.LookUpWindow(this.components);
            this.lookUpEditorType = new YidanSoft.Common.Library.LookUpEditor();
            ((System.ComponentModel.ISupportInitialize)(this.txtname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInstruction.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmemo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinDefaultScore.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinStandardScore.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinDefaultTarget.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinTargetStandard.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinScoreStandard.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOrder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbcategory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorType)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(39, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "项目名称：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(282, 24);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 12;
            this.labelControl2.Text = "项目说明：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(39, 61);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 13;
            this.labelControl3.Text = "默认评分：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(282, 61);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 14;
            this.labelControl4.Text = "标准评分：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(39, 209);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(64, 14);
            this.labelControl5.TabIndex = 21;
            this.labelControl5.Text = "备       注：";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::YindanSoft.Emr.QcManagerNew.Properties.Resources.保存;
            this.btnAdd.Location = new System.Drawing.Point(294, 250);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 27);
            this.btnAdd.TabIndex = 22;
            this.btnAdd.Text = "保存(&S)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtname
            // 
            this.txtname.Location = new System.Drawing.Point(106, 21);
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(120, 21);
            this.txtname.TabIndex = 0;
            this.txtname.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // txtInstruction
            // 
            this.txtInstruction.Location = new System.Drawing.Point(348, 21);
            this.txtInstruction.Name = "txtInstruction";
            this.txtInstruction.Size = new System.Drawing.Size(120, 21);
            this.txtInstruction.TabIndex = 1;
            this.txtInstruction.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // txtmemo
            // 
            this.txtmemo.Location = new System.Drawing.Point(105, 206);
            this.txtmemo.Name = "txtmemo";
            this.txtmemo.Size = new System.Drawing.Size(363, 21);
            this.txtmemo.TabIndex = 10;
            this.txtmemo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // btnClear
            // 
            this.btnClear.Image = global::YindanSoft.Emr.QcManagerNew.Properties.Resources.重置;
            this.btnClear.Location = new System.Drawing.Point(388, 250);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 27);
            this.btnClear.TabIndex = 23;
            this.btnClear.Text = "重置(&B)";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(39, 98);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 14);
            this.labelControl6.TabIndex = 15;
            this.labelControl6.Text = "评分类别：";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(282, 98);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(60, 14);
            this.labelControl7.TabIndex = 16;
            this.labelControl7.Text = "默认指标：";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(39, 135);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(60, 14);
            this.labelControl8.TabIndex = 17;
            this.labelControl8.Text = "指标标准：";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(282, 135);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(60, 14);
            this.labelControl9.TabIndex = 18;
            this.labelControl9.Text = "评分标准：";
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(39, 172);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(60, 14);
            this.labelControl10.TabIndex = 19;
            this.labelControl10.Text = "排列顺序：";
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(282, 172);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(60, 14);
            this.labelControl11.TabIndex = 20;
            this.labelControl11.Text = "对应大项：";
            // 
            // spinDefaultScore
            // 
            this.spinDefaultScore.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinDefaultScore.Location = new System.Drawing.Point(106, 54);
            this.spinDefaultScore.Name = "spinDefaultScore";
            this.spinDefaultScore.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinDefaultScore.Size = new System.Drawing.Size(120, 21);
            this.spinDefaultScore.TabIndex = 2;
            this.spinDefaultScore.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // spinStandardScore
            // 
            this.spinStandardScore.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinStandardScore.Location = new System.Drawing.Point(348, 58);
            this.spinStandardScore.Name = "spinStandardScore";
            this.spinStandardScore.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinStandardScore.Size = new System.Drawing.Size(120, 21);
            this.spinStandardScore.TabIndex = 3;
            this.spinStandardScore.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // spinDefaultTarget
            // 
            this.spinDefaultTarget.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinDefaultTarget.Location = new System.Drawing.Point(348, 95);
            this.spinDefaultTarget.Name = "spinDefaultTarget";
            this.spinDefaultTarget.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinDefaultTarget.Size = new System.Drawing.Size(120, 21);
            this.spinDefaultTarget.TabIndex = 5;
            this.spinDefaultTarget.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // spinTargetStandard
            // 
            this.spinTargetStandard.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinTargetStandard.Location = new System.Drawing.Point(106, 132);
            this.spinTargetStandard.Name = "spinTargetStandard";
            this.spinTargetStandard.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinTargetStandard.Size = new System.Drawing.Size(120, 21);
            this.spinTargetStandard.TabIndex = 6;
            this.spinTargetStandard.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // spinScoreStandard
            // 
            this.spinScoreStandard.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinScoreStandard.Location = new System.Drawing.Point(348, 132);
            this.spinScoreStandard.Name = "spinScoreStandard";
            this.spinScoreStandard.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinScoreStandard.Size = new System.Drawing.Size(120, 21);
            this.spinScoreStandard.TabIndex = 7;
            this.spinScoreStandard.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // spinOrder
            // 
            this.spinOrder.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinOrder.Location = new System.Drawing.Point(106, 169);
            this.spinOrder.Name = "spinOrder";
            this.spinOrder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinOrder.Size = new System.Drawing.Size(120, 21);
            this.spinOrder.TabIndex = 8;
            this.spinOrder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // cmbcategory
            // 
            this.cmbcategory.Location = new System.Drawing.Point(106, 95);
            this.cmbcategory.Name = "cmbcategory";
            this.cmbcategory.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbcategory.Properties.Items.AddRange(new object[] {
            "个人",
            "科室",
            "医务处"});
            this.cmbcategory.Size = new System.Drawing.Size(120, 21);
            this.cmbcategory.TabIndex = 4;
            this.cmbcategory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // lookUpWindowType
            // 
            this.lookUpWindowType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowType.GenShortCode = null;
            this.lookUpWindowType.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowType.Owner = null;
            this.lookUpWindowType.SqlHelper = null;
            // 
            // lookUpEditorType
            // 
            this.lookUpEditorType.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorType.ListWindow = this.lookUpWindowType;
            this.lookUpEditorType.Location = new System.Drawing.Point(348, 169);
            this.lookUpEditorType.Name = "lookUpEditorType";
            this.lookUpEditorType.ShowFormImmediately = true;
            this.lookUpEditorType.Size = new System.Drawing.Size(120, 20);
            this.lookUpEditorType.TabIndex = 9;
            this.lookUpEditorType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            // 
            // QCScoreItemEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 287);
            this.Controls.Add(this.lookUpEditorType);
            this.Controls.Add(this.cmbcategory);
            this.Controls.Add(this.spinOrder);
            this.Controls.Add(this.spinScoreStandard);
            this.Controls.Add(this.spinTargetStandard);
            this.Controls.Add(this.spinDefaultTarget);
            this.Controls.Add(this.spinStandardScore);
            this.Controls.Add(this.spinDefaultScore);
            this.Controls.Add(this.txtmemo);
            this.Controls.Add(this.txtInstruction);
            this.Controls.Add(this.txtname);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl11);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.Name = "QCScoreItemEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "标准评分大项详细维护";
            this.Load += new System.EventHandler(this.QCScoreItemEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInstruction.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmemo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinDefaultScore.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinStandardScore.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinDefaultTarget.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinTargetStandard.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinScoreStandard.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOrder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbcategory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.TextEdit txtname;
        private DevExpress.XtraEditors.TextEdit txtInstruction;
        private DevExpress.XtraEditors.TextEdit txtmemo;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.SpinEdit spinDefaultScore;
        private DevExpress.XtraEditors.SpinEdit spinStandardScore;
        private DevExpress.XtraEditors.SpinEdit spinDefaultTarget;
        private DevExpress.XtraEditors.SpinEdit spinTargetStandard;
        private DevExpress.XtraEditors.SpinEdit spinScoreStandard;
        private DevExpress.XtraEditors.SpinEdit spinOrder;
        private DevExpress.XtraEditors.ComboBoxEdit cmbcategory;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowType;
        private YidanSoft.Common.Library.LookUpEditor lookUpEditorType;
    }
}