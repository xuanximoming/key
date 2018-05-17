namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCAffirmdate
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.date_queren = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.date_queren.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_queren.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.AccessibleName = "确认（替代策略）检测阳性日期";
            this.labelControl1.Location = new System.Drawing.Point(40, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(180, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "确认（替代策略）检测阳性日期：";
            // 
            // date_queren
            // 
            this.date_queren.EditValue = null;
            this.date_queren.Location = new System.Drawing.Point(222, 2);
            this.date_queren.Name = "date_queren";
            this.date_queren.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.date_queren.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.date_queren.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.date_queren.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.date_queren.Size = new System.Drawing.Size(117, 21);
            this.date_queren.TabIndex = 1;
            // 
            // UCAffirmdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.date_queren);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCAffirmdate";
            this.Size = new System.Drawing.Size(364, 29);
            ((System.ComponentModel.ISupportInitialize)(this.date_queren.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_queren.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit date_queren;
    }
}
