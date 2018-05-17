namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.DailyForm
{
    partial class DailyFormDisplayDateTime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DailyFormDisplayDateTime));
            this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditData = new DevExpress.XtraEditors.DateEdit();
            this.timeEditTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControlTip = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditData.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEditTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Location = new System.Drawing.Point(184, 81);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonCancel.TabIndex = 7;
            this.simpleButtonCancel.Text = "取消";
            this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Location = new System.Drawing.Point(87, 81);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonOK.TabIndex = 6;
            this.simpleButtonOK.Text = "确定";
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButtonOK_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(15, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "病程日期";
            // 
            // dateEditData
            // 
            this.dateEditData.EditValue = null;
            this.dateEditData.Location = new System.Drawing.Point(73, 16);
            this.dateEditData.Name = "dateEditData";
            this.dateEditData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditData.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditData.Size = new System.Drawing.Size(146, 20);
            this.dateEditData.TabIndex = 8;
            // 
            // timeEditTime
            // 
            this.timeEditTime.EditValue = new System.DateTime(2011, 8, 5, 0, 0, 0, 0);
            this.timeEditTime.Location = new System.Drawing.Point(218, 16);
            this.timeEditTime.Name = "timeEditTime";
            this.timeEditTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeEditTime.Size = new System.Drawing.Size(101, 20);
            this.timeEditTime.TabIndex = 9;
            // 
            // labelControlTip
            // 
            this.labelControlTip.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControlTip.Location = new System.Drawing.Point(15, 52);
            this.labelControlTip.Name = "labelControlTip";
            this.labelControlTip.Size = new System.Drawing.Size(285, 14);
            this.labelControlTip.TabIndex = 11;
            this.labelControlTip.Text = "时间: 2011-08-01 12:05:05 ~ 2011-08-10 12:05:05";
            // 
            // DailyFormDisplayDateTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 116);
            this.Controls.Add(this.labelControlTip);
            this.Controls.Add(this.timeEditTime);
            this.Controls.Add(this.dateEditData);
            this.Controls.Add(this.simpleButtonCancel);
            this.Controls.Add(this.simpleButtonOK);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DailyFormDisplayDateTime";
            this.Text = "修改";
            ((System.ComponentModel.ISupportInitialize)(this.dateEditData.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEditTime.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
        private DevExpress.XtraEditors.SimpleButton simpleButtonOK;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dateEditData;
        private DevExpress.XtraEditors.TimeEdit timeEditTime;
        private DevExpress.XtraEditors.LabelControl labelControlTip;
    }
}