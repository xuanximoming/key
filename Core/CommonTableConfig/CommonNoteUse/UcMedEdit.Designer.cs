namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    partial class UcMedEdit
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcMedEdit));
            this.memoEditValue = new DevExpress.XtraEditors.MemoEdit();
            this.btnOk = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.btnCancle = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditValue.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // memoEditValue
            // 
            this.memoEditValue.AllowDrop = true;
            this.memoEditValue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.memoEditValue.Location = new System.Drawing.Point(12, 10);
            this.memoEditValue.Name = "memoEditValue";
            this.memoEditValue.ShowToolTips = false;
            this.memoEditValue.Size = new System.Drawing.Size(416, 138);
            this.memoEditValue.TabIndex = 1;
            this.memoEditValue.UseOptimizedRendering = true;
            this.memoEditValue.DragDrop += new System.Windows.Forms.DragEventHandler(this.memoEditValue_DragDrop);
            this.memoEditValue.DragEnter += new System.Windows.Forms.DragEventHandler(this.memoEditValue_DragEnter);
            // 
            // btnOk
            // 
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(262, 153);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定(&Y)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Image = ((System.Drawing.Image)(resources.GetObject("btnCancle.Image")));
            this.btnCancle.Location = new System.Drawing.Point(349, 153);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(80, 23);
            this.btnCancle.TabIndex = 3;
            this.btnCancle.Text = "取消(&C)";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl1.Location = new System.Drawing.Point(12, 153);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(236, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "支持从工具箱拖拽“特殊字符”和“科室小模板”";
            // 
            // UcMedEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 185);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.memoEditValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UcMedEdit";
            this.Text = "编辑文本";
            ((System.ComponentModel.ISupportInitialize)(this.memoEditValue.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraEditors.MemoEdit memoEditValue;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel btnCancle;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        public DrectSoft.Common.Ctrs.OTHER.DevButtonOK btnOk;
    }
}
