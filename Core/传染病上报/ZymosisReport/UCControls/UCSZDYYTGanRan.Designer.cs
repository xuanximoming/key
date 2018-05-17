namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCSZDYYTGanRan
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
            this.lpkSZDGanRan = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.lpkSZDGanRan.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(132, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "生殖道沙眼衣原体感染：";
            // 
            // lpkSZDGanRan
            // 
            this.lpkSZDGanRan.Location = new System.Drawing.Point(144, 3);
            this.lpkSZDGanRan.Name = "lpkSZDGanRan";
            this.lpkSZDGanRan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lpkSZDGanRan.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "编号"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "名称")});
            this.lpkSZDGanRan.Properties.NullText = "请选择...";
            this.lpkSZDGanRan.Size = new System.Drawing.Size(150, 21);
            this.lpkSZDGanRan.TabIndex = 1;
            // 
            // UCSZDYYTGanRan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lpkSZDGanRan);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCSZDYYTGanRan";
            this.Size = new System.Drawing.Size(312, 29);
            ((System.ComponentModel.ISupportInitialize)(this.lpkSZDGanRan.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit lpkSZDGanRan;
    }
}
