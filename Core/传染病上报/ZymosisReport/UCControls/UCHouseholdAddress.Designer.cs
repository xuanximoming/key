namespace DrectSoft.Core.ZymosisReport.UCControls
{
    partial class UCHouseholdAddress
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
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.lab_guobiao = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEdit_sheng = new DevExpress.XtraEditors.LookUpEdit();
            this.lookUpEdit_xiang = new DevExpress.XtraEditors.LookUpEdit();
            this.lookUpEdit_xian = new DevExpress.XtraEditors.LookUpEdit();
            this.lookUpEdit_shi = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_sheng.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_xiang.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_xian.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_shi.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(4, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(88, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "户籍地址 国标：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(201, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(12, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "省";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(4, 55);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(88, 14);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "户籍地址 国标：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(200, 33);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(34, 14);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "县(区)";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(347, 3);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(12, 14);
            this.labelControl5.TabIndex = 0;
            this.labelControl5.Text = "市";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(347, 33);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(70, 14);
            this.labelControl6.TabIndex = 0;
            this.labelControl6.Text = "乡(镇、街道)";
            // 
            // lab_guobiao
            // 
            this.lab_guobiao.Location = new System.Drawing.Point(94, 55);
            this.lab_guobiao.Name = "lab_guobiao";
            this.lab_guobiao.Size = new System.Drawing.Size(24, 14);
            this.lab_guobiao.TabIndex = 5;
            this.lab_guobiao.Text = "国标";
            // 
            // lookUpEdit_sheng
            // 
            this.lookUpEdit_sheng.Location = new System.Drawing.Point(94, 3);
            this.lookUpEdit_sheng.Name = "lookUpEdit_sheng";
            this.lookUpEdit_sheng.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit_sheng.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NPCCID", "国标"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NPCCNAME", "省份")});
            this.lookUpEdit_sheng.Size = new System.Drawing.Size(100, 21);
            this.lookUpEdit_sheng.TabIndex = 1;
            this.lookUpEdit_sheng.EditValueChanged += new System.EventHandler(this.lookUpEdit_sheng_EditValueChanged);
            // 
            // lookUpEdit_xiang
            // 
            this.lookUpEdit_xiang.Location = new System.Drawing.Point(241, 30);
            this.lookUpEdit_xiang.Name = "lookUpEdit_xiang";
            this.lookUpEdit_xiang.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit_xiang.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NPCCID", "国标"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NPCCNAME", "乡镇")});
            this.lookUpEdit_xiang.Size = new System.Drawing.Size(100, 21);
            this.lookUpEdit_xiang.TabIndex = 4;
            this.lookUpEdit_xiang.EditValueChanged += new System.EventHandler(this.lookUpEdit_xiang_EditValueChanged);
            // 
            // lookUpEdit_xian
            // 
            this.lookUpEdit_xian.Location = new System.Drawing.Point(94, 30);
            this.lookUpEdit_xian.Name = "lookUpEdit_xian";
            this.lookUpEdit_xian.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit_xian.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NPCCID", "国标"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NPCCNAME", "县区")});
            this.lookUpEdit_xian.Size = new System.Drawing.Size(100, 21);
            this.lookUpEdit_xian.TabIndex = 3;
            this.lookUpEdit_xian.EditValueChanged += new System.EventHandler(this.lookUpEdit_xian_EditValueChanged);
            // 
            // lookUpEdit_shi
            // 
            this.lookUpEdit_shi.Location = new System.Drawing.Point(241, 3);
            this.lookUpEdit_shi.Name = "lookUpEdit_shi";
            this.lookUpEdit_shi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit_shi.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NPCCID", "国标"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NPCCNAME", "市")});
            this.lookUpEdit_shi.Size = new System.Drawing.Size(100, 21);
            this.lookUpEdit_shi.TabIndex = 2;
            this.lookUpEdit_shi.EditValueChanged += new System.EventHandler(this.lookUpEdit_shi_EditValueChanged);
            // 
            // UCHouseholdAddress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lookUpEdit_shi);
            this.Controls.Add(this.lookUpEdit_xian);
            this.Controls.Add(this.lookUpEdit_xiang);
            this.Controls.Add(this.lookUpEdit_sheng);
            this.Controls.Add(this.lab_guobiao);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "UCHouseholdAddress";
            this.Size = new System.Drawing.Size(437, 75);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_sheng.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_xiang.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_xian.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_shi.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl lab_guobiao;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit_sheng;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit_xiang;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit_xian;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit_shi;
    }
}
