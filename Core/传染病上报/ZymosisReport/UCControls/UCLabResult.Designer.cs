namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCLabResult
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chkcb_jieguo = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chkcb_jieguo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(7, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "实验室结果：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(187, 3);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(228, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "（病例分类为实验室诊断病例时必须填写）";
            // 
            // chkcb_jieguo
            // 
            this.chkcb_jieguo.EditValue = "";
            this.chkcb_jieguo.Location = new System.Drawing.Point(81, 0);
            this.chkcb_jieguo.Name = "chkcb_jieguo";
            this.chkcb_jieguo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.chkcb_jieguo.Properties.HideSelection = false;
            this.chkcb_jieguo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "请选择...", System.Windows.Forms.CheckState.Unchecked, false),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "EV71"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "Cox A16"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "其他肠道病毒")});
            this.chkcb_jieguo.Properties.SelectAllItemVisible = false;
            this.chkcb_jieguo.Properties.SynchronizeEditValueWithCheckedItems = false;
            this.chkcb_jieguo.Size = new System.Drawing.Size(100, 21);
            this.chkcb_jieguo.TabIndex = 1;
            this.chkcb_jieguo.Tag = "1";
            // 
            // UCLabResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkcb_jieguo);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCLabResult";
            this.Size = new System.Drawing.Size(426, 25);
            this.Tag = "LABRESULT";
            ((System.ComponentModel.ISupportInitialize)(this.chkcb_jieguo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckedComboBoxEdit chkcb_jieguo;
    }
}
