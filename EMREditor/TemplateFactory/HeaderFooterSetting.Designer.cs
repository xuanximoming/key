namespace DrectSoft.Emr.TemplateFactory
{
    partial class HeaderFooterSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeaderFooterSetting));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.spinEditHeaderHeight = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.spinEditFooterHeight = new DevExpress.XtraEditors.SpinEdit();
            this.simpleButtonOK = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK();
            this.simpleButtonCancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditHeaderHeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditFooterHeight.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(20, 22);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "页眉高度：";
            // 
            // spinEditHeaderHeight
            // 
            this.spinEditHeaderHeight.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEditHeaderHeight.EnterMoveNextControl = true;
            this.spinEditHeaderHeight.Location = new System.Drawing.Point(80, 19);
            this.spinEditHeaderHeight.Name = "spinEditHeaderHeight";
            this.spinEditHeaderHeight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEditHeaderHeight.Size = new System.Drawing.Size(120, 20);
            this.spinEditHeaderHeight.TabIndex = 0;
            this.spinEditHeaderHeight.ToolTip = "页眉高度";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(20, 59);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "页脚高度：";
            // 
            // spinEditFooterHeight
            // 
            this.spinEditFooterHeight.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEditFooterHeight.EnterMoveNextControl = true;
            this.spinEditFooterHeight.Location = new System.Drawing.Point(80, 56);
            this.spinEditFooterHeight.Name = "spinEditFooterHeight";
            this.spinEditFooterHeight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEditFooterHeight.Size = new System.Drawing.Size(120, 20);
            this.spinEditFooterHeight.TabIndex = 1;
            this.spinEditFooterHeight.ToolTip = "页脚高度";
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonOK.Image")));
            this.simpleButtonOK.Location = new System.Drawing.Point(34, 96);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonOK.TabIndex = 2;
            this.simpleButtonOK.Text = "确定(&Y)";
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButtonOK_Click);
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonCancel.Image")));
            this.simpleButtonCancel.Location = new System.Drawing.Point(120, 96);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(80, 27);
            this.simpleButtonCancel.TabIndex = 3;
            this.simpleButtonCancel.Text = "取消(&C)";
            this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
            // 
            // HeaderFooterSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 141);
            this.Controls.Add(this.simpleButtonCancel);
            this.Controls.Add(this.simpleButtonOK);
            this.Controls.Add(this.spinEditFooterHeight);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.spinEditHeaderHeight);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HeaderFooterSetting";
            this.Text = "页眉页脚设置";
            this.Load += new System.EventHandler(this.HeaderFooterSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.spinEditHeaderHeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditFooterHeight.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit spinEditHeaderHeight;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit spinEditFooterHeight;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK simpleButtonOK;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel simpleButtonCancel;

    }
}