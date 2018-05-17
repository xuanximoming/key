namespace DrectSoft.MainFrame
{
    partial class ChooseConection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseConection));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.DevButtonCancel1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.cmbDataSource = new DevExpress.XtraEditors.ComboBoxEdit();
            this.DevButtonOK1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.lbl = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDataSource.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(478, 118);
            this.panelControl1.TabIndex = 0;
            // 
            // panelControl3
            // 
            this.panelControl3.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl3.Appearance.Options.UseBackColor = true;
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.DevButtonCancel1);
            this.panelControl3.Controls.Add(this.cmbDataSource);
            this.panelControl3.Controls.Add(this.DevButtonOK1);
            this.panelControl3.Controls.Add(this.lbl);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(478, 118);
            this.panelControl3.TabIndex = 4;
            this.panelControl3.Paint += new System.Windows.Forms.PaintEventHandler(this.panelControl3_Paint);
            // 
            // DevButtonCancel1
            // 
            this.DevButtonCancel1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonCancel1.Image")));
            this.DevButtonCancel1.Location = new System.Drawing.Point(345, 62);
            this.DevButtonCancel1.Name = "DevButtonCancel1";
            this.DevButtonCancel1.Size = new System.Drawing.Size(80, 23);
            this.DevButtonCancel1.TabIndex = 3;
            this.DevButtonCancel1.Text = "取消(&C)";
            this.DevButtonCancel1.Click += new System.EventHandler(this.DevButtonCancel1_Click);
            // 
            // cmbDataSource
            // 
            this.cmbDataSource.Location = new System.Drawing.Point(150, 25);
            this.cmbDataSource.Name = "cmbDataSource";
            this.cmbDataSource.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDataSource.Properties.NullText = "请选择...";
            this.cmbDataSource.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbDataSource.Size = new System.Drawing.Size(275, 20);
            this.cmbDataSource.TabIndex = 1;
            this.cmbDataSource.SelectedIndexChanged += new System.EventHandler(this.cmbDataSource_SelectedIndexChanged);
            this.cmbDataSource.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbDataSource_KeyDown);
            // 
            // DevButtonOK1
            // 
            this.DevButtonOK1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonOK1.Image")));
            this.DevButtonOK1.Location = new System.Drawing.Point(259, 62);
            this.DevButtonOK1.Name = "DevButtonOK1";
            this.DevButtonOK1.Size = new System.Drawing.Size(80, 23);
            this.DevButtonOK1.TabIndex = 2;
            this.DevButtonOK1.Text = "确定(&Y)";
            this.DevButtonOK1.Click += new System.EventHandler(this.DevButtonOK1_Click);
            // 
            // lbl
            // 
            this.lbl.Appearance.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.lbl.Appearance.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbl.Location = new System.Drawing.Point(61, 28);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(83, 14);
            this.lbl.TabIndex = 3;
            this.lbl.Text = "选择数据库:";
            // 
            // ChooseConection
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 118);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseConection";
            this.Text = "选择连接数据源";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChooseConection_FormClosed);
            this.Load += new System.EventHandler(this.ChooseConection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDataSource.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit cmbDataSource;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK DevButtonOK1;
        private DevExpress.XtraEditors.LabelControl lbl;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel DevButtonCancel1;
    }
}