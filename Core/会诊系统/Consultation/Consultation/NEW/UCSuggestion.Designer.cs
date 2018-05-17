namespace Consultation.NEW
{
    partial class UCSuggestion
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.memoEditSuggestion = new DevExpress.XtraEditors.MemoEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCompelete = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnCompelete);
            this.groupControl1.Controls.Add(this.btnSave);
            this.groupControl1.Controls.Add(this.memoEditSuggestion);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(795, 195);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "会诊意见";
            // 
            // memoEditSuggestion
            // 
            this.memoEditSuggestion.EditValue = "";
            this.memoEditSuggestion.Location = new System.Drawing.Point(17, 33);
            this.memoEditSuggestion.Name = "memoEditSuggestion";
            this.memoEditSuggestion.Properties.MaxLength = 1500;
            this.memoEditSuggestion.Size = new System.Drawing.Size(762, 123);
            this.memoEditSuggestion.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::DrectSoft.Core.Consultation.Properties.Resources.保存;
            this.btnSave.Location = new System.Drawing.Point(533, 162);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存为草稿(&S)";
            // 
            // btnCompelete
            // 
            this.btnCompelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompelete.Image = global::DrectSoft.Core.Consultation.Properties.Resources.确定;
            this.btnCompelete.Location = new System.Drawing.Point(659, 162);
            this.btnCompelete.Name = "btnCompelete";
            this.btnCompelete.Size = new System.Drawing.Size(120, 23);
            this.btnCompelete.TabIndex = 2;
            this.btnCompelete.Text = "会诊已完成(&Z)";
            // 
            // UCSuggestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Name = "UCSuggestion";
            this.Size = new System.Drawing.Size(795, 195);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.GroupControl groupControl1;
        public DevExpress.XtraEditors.MemoEdit memoEditSuggestion;
        public DevExpress.XtraEditors.SimpleButton btnCompelete;
        public DevExpress.XtraEditors.SimpleButton btnSave;



    }
}
