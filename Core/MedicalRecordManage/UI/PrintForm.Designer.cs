namespace MedicalRecordManage.UI
{
    partial class PrintForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintForm));
            this.panelControlTop = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditPrinter = new DevExpress.XtraEditors.LookUpEdit();
            this.lue_PrintCount = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btn_Print = new DrectSoft.Common.Ctrs.OTHER.DevButtonPrint();
            this.btn_Close = new DrectSoft.Common.Ctrs.OTHER.DevButtonClose();
            this.che_End = new DevExpress.XtraEditors.SpinEdit();
            this.che_Begin = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.che_PointPage = new DevExpress.XtraEditors.CheckEdit();
            this.che_All = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).BeginInit();
            this.panelControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPrinter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lue_PrintCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.che_End.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.che_Begin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.che_PointPage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.che_All.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.panelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControlTop
            // 
            this.panelControlTop.Controls.Add(this.labelControl3);
            this.panelControlTop.Controls.Add(this.lookUpEditPrinter);
            this.panelControlTop.Controls.Add(this.lue_PrintCount);
            this.panelControlTop.Controls.Add(this.labelControl4);
            this.panelControlTop.Controls.Add(this.btn_Print);
            this.panelControlTop.Controls.Add(this.btn_Close);
            this.panelControlTop.Controls.Add(this.che_End);
            this.panelControlTop.Controls.Add(this.che_Begin);
            this.panelControlTop.Controls.Add(this.labelControl2);
            this.panelControlTop.Controls.Add(this.che_PointPage);
            this.panelControlTop.Controls.Add(this.che_All);
            this.panelControlTop.Controls.Add(this.labelControl1);
            this.panelControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlTop.Location = new System.Drawing.Point(0, 0);
            this.panelControlTop.Name = "panelControlTop";
            this.panelControlTop.Size = new System.Drawing.Size(1203, 45);
            this.panelControlTop.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(519, 16);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 39;
            this.labelControl3.Text = "打印机：";
            // 
            // lookUpEditPrinter
            // 
            this.lookUpEditPrinter.Location = new System.Drawing.Point(567, 13);
            this.lookUpEditPrinter.Name = "lookUpEditPrinter";
            this.lookUpEditPrinter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditPrinter.Size = new System.Drawing.Size(228, 20);
            this.lookUpEditPrinter.TabIndex = 38;
            // 
            // lue_PrintCount
            // 
            this.lue_PrintCount.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lue_PrintCount.Location = new System.Drawing.Point(447, 13);
            this.lue_PrintCount.Name = "lue_PrintCount";
            this.lue_PrintCount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.lue_PrintCount.Properties.IsFloatValue = false;
            this.lue_PrintCount.Properties.Mask.EditMask = "N00";
            this.lue_PrintCount.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.lue_PrintCount.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lue_PrintCount.Size = new System.Drawing.Size(55, 20);
            this.lue_PrintCount.TabIndex = 37;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(386, 16);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 36;
            this.labelControl4.Text = "打印份数：";
            // 
            // btn_Print
            // 
            this.btn_Print.Image = ((System.Drawing.Image)(resources.GetObject("btn_Print.Image")));
            this.btn_Print.Location = new System.Drawing.Point(826, 11);
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Size = new System.Drawing.Size(80, 23);
            this.btn_Print.TabIndex = 35;
            this.btn_Print.Text = "打印(&P)";
            this.btn_Print.Click += new System.EventHandler(this.btn_Print_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Image = ((System.Drawing.Image)(resources.GetObject("btn_Close.Image")));
            this.btn_Close.Location = new System.Drawing.Point(912, 11);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(80, 23);
            this.btn_Close.TabIndex = 34;
            this.btn_Close.Text = "关闭(&T)";
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // che_End
            // 
            this.che_End.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.che_End.Location = new System.Drawing.Point(316, 12);
            this.che_End.Name = "che_End";
            this.che_End.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.che_End.Properties.IsFloatValue = false;
            this.che_End.Properties.Mask.EditMask = "N00";
            this.che_End.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.che_End.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.che_End.Properties.NullText = "1";
            this.che_End.Size = new System.Drawing.Size(56, 20);
            this.che_End.TabIndex = 13;
            // 
            // che_Begin
            // 
            this.che_Begin.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.che_Begin.Location = new System.Drawing.Point(242, 12);
            this.che_Begin.Name = "che_Begin";
            this.che_Begin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.che_Begin.Properties.IsFloatValue = false;
            this.che_Begin.Properties.Mask.EditMask = "N00";
            this.che_Begin.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.che_Begin.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.che_Begin.Properties.NullText = "1";
            this.che_Begin.Size = new System.Drawing.Size(56, 20);
            this.che_Begin.TabIndex = 12;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(302, 15);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(12, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "至";
            // 
            // che_PointPage
            // 
            this.che_PointPage.Location = new System.Drawing.Point(150, 13);
            this.che_PointPage.Name = "che_PointPage";
            this.che_PointPage.Properties.Caption = "打印指定页：";
            this.che_PointPage.Properties.RadioGroupIndex = 0;
            this.che_PointPage.Size = new System.Drawing.Size(95, 19);
            this.che_PointPage.TabIndex = 2;
            this.che_PointPage.TabStop = false;
            // 
            // che_All
            // 
            this.che_All.Location = new System.Drawing.Point(72, 13);
            this.che_All.Name = "che_All";
            this.che_All.Properties.Caption = "全部打印";
            this.che_All.Properties.RadioGroupIndex = 0;
            this.che_All.Size = new System.Drawing.Size(75, 19);
            this.che_All.TabIndex = 1;
            this.che_All.TabStop = false;
            this.che_All.CheckedChanged += new System.EventHandler(this.che_All_CheckedChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "打印方式：";
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageSize = new System.Drawing.Size(20, 20);
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "配置.png");
            this.imageCollection1.Images.SetKeyName(1, "提交.png");
            // 
            // panelContainer
            // 
            this.panelContainer.AutoScroll = true;
            this.panelContainer.AutoSize = true;
            this.panelContainer.Controls.Add(this.button1);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 45);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(1203, 507);
            this.panelContainer.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(-8, -8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(10, 10);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 552);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.panelControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrintForm";
            this.Text = "打印预览";
            this.Load += new System.EventHandler(this.PrintForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).EndInit();
            this.panelControlTop.ResumeLayout(false);
            this.panelControlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPrinter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lue_PrintCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.che_End.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.che_Begin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.che_PointPage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.che_All.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.panelContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlTop;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit che_PointPage;
        private DevExpress.XtraEditors.CheckEdit che_All;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit che_End;
        private DevExpress.XtraEditors.SpinEdit che_Begin;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonClose btn_Close;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonPrint btn_Print;
        private DevExpress.XtraEditors.SpinEdit lue_PrintCount;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditPrinter;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.Button button1;

    }
}