namespace DrectSoft.Core.DoctorAdvice
{
   partial class FrequencyInputor
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
            this.frequencyInput = new DrectSoft.Common.Library.LookUpEditor();
            this.frequencyDetail = new DevExpress.XtraEditors.PopupContainerEdit();
            this.popupFrequencySetting = new DevExpress.XtraEditors.PopupContainerControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.listBoxHour = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.listBoxWeekDay = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.panelContainer = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyDetail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupFrequencySetting)).BeginInit();
            this.popupFrequencySetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxWeekDay)).BeginInit();
            this.panelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // frequencyInput
            // 
            this.frequencyInput.EnterMoveNextControl = true;
            this.frequencyInput.ListWindow = null;
            this.frequencyInput.Location = new System.Drawing.Point(0, 0);
            this.frequencyInput.Margin = new System.Windows.Forms.Padding(0);
            this.frequencyInput.Name = "frequencyInput";
            this.frequencyInput.ShowFormImmediately = true;
            this.frequencyInput.Size = new System.Drawing.Size(79, 20);
            this.frequencyInput.TabIndex = 2;
            this.frequencyInput.CodeValueChanged += new System.EventHandler(this.frequencyInput_CodeValueChanged);
            // 
            // frequencyDetail
            // 
            this.frequencyDetail.EditValue = "";
            this.frequencyDetail.Location = new System.Drawing.Point(79, 0);
            this.frequencyDetail.Margin = new System.Windows.Forms.Padding(0);
            this.frequencyDetail.Name = "frequencyDetail";
            this.frequencyDetail.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.frequencyDetail.Properties.PopupControl = this.popupFrequencySetting;
            this.frequencyDetail.Properties.PopupSizeable = false;
            this.frequencyDetail.Properties.ShowPopupCloseButton = false;
            this.frequencyDetail.Size = new System.Drawing.Size(143, 21);
            this.frequencyDetail.TabIndex = 3;
            this.frequencyDetail.TabStop = false;
            this.frequencyDetail.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.frequencyDetail_Closed);
            this.frequencyDetail.TextChanged += new System.EventHandler(this.frequencyDetail_TextChanged);
            // 
            // popupFrequencySetting
            // 
            this.popupFrequencySetting.Controls.Add(this.groupControl1);
            this.popupFrequencySetting.Controls.Add(this.groupControl2);
            this.popupFrequencySetting.Location = new System.Drawing.Point(3, 30);
            this.popupFrequencySetting.Name = "popupFrequencySetting";
            this.popupFrequencySetting.Size = new System.Drawing.Size(215, 188);
            this.popupFrequencySetting.TabIndex = 43;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.listBoxHour);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(76, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(139, 188);
            this.groupControl1.TabIndex = 16;
            this.groupControl1.Text = "执行时间";
            // 
            // listBoxHour
            // 
            this.listBoxHour.CheckOnClick = true;
            this.listBoxHour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxHour.ItemHeight = 17;
            this.listBoxHour.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("00"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("01"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("02"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("03"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("04"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("05"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("06"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("07"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("08"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("09"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("10"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("11"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("12"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("13"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("14"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("15"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("16"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("17"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("18"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("19"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("20"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("21"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("22"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("23")});
            this.listBoxHour.Location = new System.Drawing.Point(2, 23);
            this.listBoxHour.MultiColumn = true;
            this.listBoxHour.Name = "listBoxHour";
            this.listBoxHour.Size = new System.Drawing.Size(135, 163);
            this.listBoxHour.TabIndex = 28;
            this.listBoxHour.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.listBox_DrawItem);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.listBoxWeekDay);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(76, 188);
            this.groupControl2.TabIndex = 18;
            this.groupControl2.Text = "周代码";
            // 
            // listBoxWeekDay
            // 
            this.listBoxWeekDay.CheckOnClick = true;
            this.listBoxWeekDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxWeekDay.ItemHeight = 17;
            this.listBoxWeekDay.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("日"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("一"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("二"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("三"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("四"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("五"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("六")});
            this.listBoxWeekDay.Location = new System.Drawing.Point(2, 23);
            this.listBoxWeekDay.Name = "listBoxWeekDay";
            this.listBoxWeekDay.Size = new System.Drawing.Size(72, 163);
            this.listBoxWeekDay.TabIndex = 23;
            this.listBoxWeekDay.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.listBox_DrawItem);
            // 
            // panelContainer
            // 
            this.panelContainer.AutoSize = true;
            this.panelContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelContainer.Controls.Add(this.frequencyInput);
            this.panelContainer.Controls.Add(this.frequencyDetail);
            this.panelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelContainer.Margin = new System.Windows.Forms.Padding(0);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(222, 21);
            this.panelContainer.TabIndex = 44;
            // 
            // FrequencyInputor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.popupFrequencySetting);
            this.LookAndFeel.SkinName = "Blue";
            this.MinimumSize = new System.Drawing.Size(175, 23);
            this.Name = "FrequencyInputor";
            this.Size = new System.Drawing.Size(222, 222);
            this.Resize += new System.EventHandler(this.FrequencyInputor_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.frequencyInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyDetail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupFrequencySetting)).EndInit();
            this.popupFrequencySetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxWeekDay)).EndInit();
            this.panelContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private DrectSoft.Common.Library.LookUpEditor frequencyInput;
      private DevExpress.XtraEditors.PopupContainerEdit frequencyDetail;
      private DevExpress.XtraEditors.PopupContainerControl popupFrequencySetting;
      private DevExpress.XtraEditors.GroupControl groupControl1;
      private DevExpress.XtraEditors.CheckedListBoxControl listBoxHour;
      private DevExpress.XtraEditors.GroupControl groupControl2;
      private DevExpress.XtraEditors.CheckedListBoxControl listBoxWeekDay;
      private System.Windows.Forms.FlowLayoutPanel panelContainer;
   }
}
