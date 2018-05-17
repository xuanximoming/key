namespace DrectSoft.Core.BirthProcess
{
    partial class FrmEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEdit));
            this.dateEdit = new DevExpress.XtraEditors.DateEdit();
            this.txtTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textEditTimes = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.DevButtonOK1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK();
            this.DevButtonClose1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditTimes.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // dateEdit
            // 
            this.dateEdit.EditValue = null;
            this.dateEdit.Location = new System.Drawing.Point(92, 41);
            this.dateEdit.Name = "dateEdit";
            this.dateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit.Size = new System.Drawing.Size(67, 20);
            this.dateEdit.TabIndex = 55;
            // 
            // txtTime
            // 
            this.txtTime.EditValue = new System.DateTime(2011, 3, 5, 0, 0, 0, 0);
            this.txtTime.Location = new System.Drawing.Point(163, 42);
            this.txtTime.Name = "txtTime";
            this.txtTime.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtTime.Properties.Mask.EditMask = "HH:mm";
            this.txtTime.Size = new System.Drawing.Size(56, 18);
            this.txtTime.TabIndex = 54;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(5, 44);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(84, 14);
            this.labelControl2.TabIndex = 26;
            this.labelControl2.Text = "宫缩开始时间：";
            // 
            // textEditTimes
            // 
            this.textEditTimes.Location = new System.Drawing.Point(92, 17);
            this.textEditTimes.Name = "textEditTimes";
            this.textEditTimes.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.textEditTimes.Properties.Appearance.Options.UseBackColor = true;
            this.textEditTimes.Size = new System.Drawing.Size(67, 20);
            this.textEditTimes.TabIndex = 22;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(40, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 23;
            this.labelControl1.Text = "孕产次：";
            // 
            // DevButtonOK1
            // 
            this.DevButtonOK1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonOK1.Image")));
            this.DevButtonOK1.Location = new System.Drawing.Point(35, 81);
            this.DevButtonOK1.Name = "DevButtonOK1";
            this.DevButtonOK1.Size = new System.Drawing.Size(69, 20);
            this.DevButtonOK1.TabIndex = 56;
            this.DevButtonOK1.Text = "确定(&Y)";
            // 
            // DevButtonClose1
            // 
            this.DevButtonClose1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonClose1.Image")));
            this.DevButtonClose1.Location = new System.Drawing.Point(126, 81);
            this.DevButtonClose1.Name = "DevButtonClose1";
            this.DevButtonClose1.Size = new System.Drawing.Size(69, 20);
            this.DevButtonClose1.TabIndex = 57;
            this.DevButtonClose1.Text = "关闭(&T)";
            // 
            // FrmEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 126);
            this.Controls.Add(this.DevButtonClose1);
            this.Controls.Add(this.DevButtonOK1);
            this.Controls.Add(this.dateEdit);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.textEditTimes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑孕产信息";
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditTimes.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit dateEdit;
        private DevExpress.XtraEditors.TimeEdit txtTime;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEditTimes;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK DevButtonOK1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClose DevButtonClose1;
    }
}