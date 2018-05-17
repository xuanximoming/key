namespace DrectSoft.Core.NurseDocument.Controls
{
    partial class UCTemperatureEditor
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
            this.components = new System.ComponentModel.Container();
            this.lookUpWayOfSurvey = new DevExpress.XtraEditors.LookUpEdit();
            this.txtTemperature1 = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWayOfSurvey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTemperature1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lookUpWayOfSurvey
            // 
            this.lookUpWayOfSurvey.AllowDrop = true;
            this.lookUpWayOfSurvey.Dock = System.Windows.Forms.DockStyle.Right;
            this.lookUpWayOfSurvey.Location = new System.Drawing.Point(54, 0);
            this.lookUpWayOfSurvey.Name = "lookUpWayOfSurvey";
            this.lookUpWayOfSurvey.Properties.Appearance.BackColor = System.Drawing.Color.PowderBlue;
            this.lookUpWayOfSurvey.Properties.Appearance.Options.UseBackColor = true;
            this.lookUpWayOfSurvey.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpWayOfSurvey.Properties.NullText = "";
            this.lookUpWayOfSurvey.Size = new System.Drawing.Size(48, 20);
            this.lookUpWayOfSurvey.TabIndex = 1;
            this.lookUpWayOfSurvey.Enter += new System.EventHandler(this.lookUpWayOfSurvey_Enter);
            // 
            // txtTemperature1
            // 
            this.txtTemperature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTemperature1.Location = new System.Drawing.Point(0, 0);
            this.txtTemperature1.Name = "txtTemperature1";
            this.txtTemperature1.Properties.ContextMenuStrip = this.contextMenuStrip1;
            this.txtTemperature1.Size = new System.Drawing.Size(54, 20);
            this.txtTemperature1.TabIndex = 0;
            this.txtTemperature1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTemperature1_KeyPress);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // UCTemperatureEditor
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtTemperature1);
            this.Controls.Add(this.lookUpWayOfSurvey);
            this.Name = "UCTemperatureEditor";
            this.Size = new System.Drawing.Size(102, 19);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWayOfSurvey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTemperature1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit lookUpWayOfSurvey;
        private DevExpress.XtraEditors.TextEdit txtTemperature1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}
