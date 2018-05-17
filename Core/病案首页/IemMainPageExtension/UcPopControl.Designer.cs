namespace IemMainPageExtension
{
    partial class UcPopControl
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
            this.popupContainerControl1 = new DevExpress.XtraEditors.PopupContainerControl();
            this.chkListBoxControlDX = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.popupContainerEditDX = new DevExpress.XtraEditors.PopupContainerEdit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkListBoxControlDX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEditDX.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.chkListBoxControlDX);
            this.popupContainerControl1.Location = new System.Drawing.Point(76, 35);
            this.popupContainerControl1.Name = "popupContainerControl1";
            this.popupContainerControl1.Size = new System.Drawing.Size(102, 185);
            this.popupContainerControl1.TabIndex = 0;
            // 
            // chkListBoxControlDX
            // 
            this.chkListBoxControlDX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkListBoxControlDX.Location = new System.Drawing.Point(0, 0);
            this.chkListBoxControlDX.Name = "chkListBoxControlDX";
            this.chkListBoxControlDX.Size = new System.Drawing.Size(102, 185);
            this.chkListBoxControlDX.TabIndex = 3;
            // 
            // popupContainerEditDX
            // 
            this.popupContainerEditDX.Location = new System.Drawing.Point(0, 1);
            this.popupContainerEditDX.Name = "popupContainerEditDX";
            this.popupContainerEditDX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.popupContainerEditDX.Properties.PopupControl = this.popupContainerControl1;
            this.popupContainerEditDX.Size = new System.Drawing.Size(99, 21);
            this.popupContainerEditDX.TabIndex = 2;
            // 
            // UcPopControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.popupContainerEditDX);
            this.Controls.Add(this.popupContainerControl1);
            this.Name = "UcPopControl";
            this.Size = new System.Drawing.Size(107, 21);
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkListBoxControlDX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEditDX.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PopupContainerControl popupContainerControl1;
        private DevExpress.XtraEditors.CheckedListBoxControl chkListBoxControlDX;
        private DevExpress.XtraEditors.PopupContainerEdit popupContainerEditDX;


    }
}
