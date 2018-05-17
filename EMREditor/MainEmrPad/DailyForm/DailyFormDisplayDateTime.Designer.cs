namespace DrectSoft.Core.MainEmrPad.DailyForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DailyFormDisplayDateTime));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditData = new DevExpress.XtraEditors.DateEdit();
            this.timeEditTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControlTip = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonOK = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.simpleButtonCancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditData.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEditTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(15, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "病程时间：";
            // 
            // dateEditData
            // 
            this.dateEditData.EditValue = null;
            this.dateEditData.Location = new System.Drawing.Point(75, 15);
            this.dateEditData.Name = "dateEditData";
            this.dateEditData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditData.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditData.Size = new System.Drawing.Size(120, 20);
            this.dateEditData.TabIndex = 0;
            // 
            // timeEditTime
            // 
            this.timeEditTime.EditValue = new System.DateTime(2011, 8, 5, 0, 0, 0, 0);
            this.timeEditTime.Location = new System.Drawing.Point(194, 15);
            this.timeEditTime.Name = "timeEditTime";
            this.timeEditTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeEditTime.Size = new System.Drawing.Size(80, 20);
            this.timeEditTime.TabIndex = 1;
            // 
            // labelControlTip
            // 
            this.labelControlTip.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelControlTip.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelControlTip.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.labelControlTip.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControlTip.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControlTip.Location = new System.Drawing.Point(75, 49);
            this.labelControlTip.Name = "labelControlTip";
            this.labelControlTip.Size = new System.Drawing.Size(199, 45);
            this.labelControlTip.TabIndex = 6;
            this.labelControlTip.Text = "时间: 2011-08-01 12:05:05 ~ 2011-08-10 12:05:05";
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonOK.Image")));
            this.simpleButtonOK.Location = new System.Drawing.Point(108, 104);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonOK.TabIndex = 2;
            this.simpleButtonOK.Text = "确定(&Y)";
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButtonOK_Click);
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonCancel.Image")));
            this.simpleButtonCancel.Location = new System.Drawing.Point(194, 104);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonCancel.TabIndex = 3;
            this.simpleButtonCancel.Text = "取消(&C)";
            this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelControl2.Location = new System.Drawing.Point(15, 50);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "提示信息：";
            // 
            // DailyFormDisplayDateTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 142);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.simpleButtonCancel);
            this.Controls.Add(this.simpleButtonOK);
            this.Controls.Add(this.labelControlTip);
            this.Controls.Add(this.timeEditTime);
            this.Controls.Add(this.dateEditData);
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

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dateEditData;
        private DevExpress.XtraEditors.TimeEdit timeEditTime;
        private DevExpress.XtraEditors.LabelControl labelControlTip;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK simpleButtonOK;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel simpleButtonCancel;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}