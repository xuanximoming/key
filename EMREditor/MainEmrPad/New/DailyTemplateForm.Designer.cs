namespace DrectSoft.Core.MainEmrPad.New
{
    partial class DailyTemplateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DailyTemplateForm));
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControlShowDailyEmrPreView = new DevExpress.XtraEditors.LabelControl();
            this.checkEditIsShowDailyEmrPreView = new DevExpress.XtraEditors.CheckEdit();
            this.btn_Save = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.btn_Cancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.timeEdit_Time = new DevExpress.XtraEditors.TimeEdit();
            this.dateEdit_Date = new DevExpress.XtraEditors.DateEdit();
            this.textEdit_Name = new DevExpress.XtraEditors.TextEdit();
            this.labelControl_Info = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditIsShowDailyEmrPreView.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeEdit_Time.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_Date.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_Date.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Name.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.labelControlShowDailyEmrPreView);
            this.panelControl2.Controls.Add(this.checkEditIsShowDailyEmrPreView);
            this.panelControl2.Controls.Add(this.btn_Save);
            this.panelControl2.Controls.Add(this.btn_Cancel);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 165);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(362, 40);
            this.panelControl2.TabIndex = 1;
            // 
            // labelControlShowDailyEmrPreView
            // 
            this.labelControlShowDailyEmrPreView.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelControlShowDailyEmrPreView.Location = new System.Drawing.Point(34, 13);
            this.labelControlShowDailyEmrPreView.Name = "labelControlShowDailyEmrPreView";
            this.labelControlShowDailyEmrPreView.Size = new System.Drawing.Size(108, 14);
            this.labelControlShowDailyEmrPreView.TabIndex = 3;
            this.labelControlShowDailyEmrPreView.Text = "是否显示病程预览区";
            // 
            // checkEditIsShowDailyEmrPreView
            // 
            this.checkEditIsShowDailyEmrPreView.Location = new System.Drawing.Point(11, 11);
            this.checkEditIsShowDailyEmrPreView.Name = "checkEditIsShowDailyEmrPreView";
            this.checkEditIsShowDailyEmrPreView.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.checkEditIsShowDailyEmrPreView.Properties.Appearance.Options.UseForeColor = true;
            this.checkEditIsShowDailyEmrPreView.Properties.Caption = "是否显示病程预览区";
            this.checkEditIsShowDailyEmrPreView.Size = new System.Drawing.Size(20, 19);
            this.checkEditIsShowDailyEmrPreView.TabIndex = 2;
            // 
            // btn_Save
            // 
            this.btn_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Save.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.Image")));
            this.btn_Save.Location = new System.Drawing.Point(172, 7);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(80, 27);
            this.btn_Save.TabIndex = 0;
            this.btn_Save.Text = "确定(&Y)";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cancel.Image")));
            this.btn_Cancel.Location = new System.Drawing.Point(267, 7);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(80, 27);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "取消(&C)";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.timeEdit_Time);
            this.panelControl1.Controls.Add(this.dateEdit_Date);
            this.panelControl1.Controls.Add(this.textEdit_Name);
            this.panelControl1.Controls.Add(this.labelControl_Info);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(362, 165);
            this.panelControl1.TabIndex = 0;
            // 
            // timeEdit_Time
            // 
            this.timeEdit_Time.EditValue = new System.DateTime(2011, 6, 28, 0, 0, 0, 0);
            this.timeEdit_Time.EnterMoveNextControl = true;
            this.timeEdit_Time.Location = new System.Drawing.Point(238, 68);
            this.timeEdit_Time.Name = "timeEdit_Time";
            this.timeEdit_Time.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeEdit_Time.Size = new System.Drawing.Size(94, 20);
            this.timeEdit_Time.TabIndex = 2;
            this.timeEdit_Time.ToolTip = "病程时间应该大于首次病程时间";
            // 
            // dateEdit_Date
            // 
            this.dateEdit_Date.EditValue = null;
            this.dateEdit_Date.EnterMoveNextControl = true;
            this.dateEdit_Date.Location = new System.Drawing.Point(99, 68);
            this.dateEdit_Date.Name = "dateEdit_Date";
            this.dateEdit_Date.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_Date.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit_Date.Size = new System.Drawing.Size(140, 20);
            this.dateEdit_Date.TabIndex = 1;
            this.dateEdit_Date.ToolTip = "病程日期应该大于首次病程日期";
            // 
            // textEdit_Name
            // 
            this.textEdit_Name.EnterMoveNextControl = true;
            this.textEdit_Name.Location = new System.Drawing.Point(99, 23);
            this.textEdit_Name.Name = "textEdit_Name";
            this.textEdit_Name.Size = new System.Drawing.Size(233, 20);
            this.textEdit_Name.TabIndex = 0;
            this.textEdit_Name.ToolTip = "病程标题";
            // 
            // labelControl_Info
            // 
            this.labelControl_Info.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelControl_Info.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelControl_Info.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.labelControl_Info.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControl_Info.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_Info.Location = new System.Drawing.Point(99, 115);
            this.labelControl_Info.Name = "labelControl_Info";
            this.labelControl_Info.Size = new System.Drawing.Size(233, 31);
            this.labelControl_Info.TabIndex = 6;
            this.labelControl_Info.Text = "提示信息";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(29, 115);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "提示信息：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(29, 71);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "病程时间：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(29, 27);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "病程标题：";
            // 
            // DailyTemplateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 205);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DailyTemplateForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "病程相关信息设置";
            this.Load += new System.EventHandler(this.DailyTemplateForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditIsShowDailyEmrPreView.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeEdit_Time.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_Date.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_Date.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Name.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TimeEdit timeEdit_Time;
        private DevExpress.XtraEditors.DateEdit dateEdit_Date;
        private DevExpress.XtraEditors.TextEdit textEdit_Name;
        private DevExpress.XtraEditors.LabelControl labelControl_Info;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel btn_Cancel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK btn_Save;
        private DevExpress.XtraEditors.LabelControl labelControlShowDailyEmrPreView;
        private DevExpress.XtraEditors.CheckEdit checkEditIsShowDailyEmrPreView;
    }
}