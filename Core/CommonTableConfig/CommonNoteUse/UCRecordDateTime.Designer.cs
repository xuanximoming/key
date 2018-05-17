namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    partial class UCRecordDateTime
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
            this.dateEditRecordDate = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditRecordDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditRecordDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // dateEditRecordDate
            // 
            this.dateEditRecordDate.EditValue = null;
            this.dateEditRecordDate.EnterMoveNextControl = true;
            this.dateEditRecordDate.Location = new System.Drawing.Point(91, 8);
            this.dateEditRecordDate.Name = "dateEditRecordDate";
            this.dateEditRecordDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditRecordDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateEditRecordDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEditRecordDate.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateEditRecordDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEditRecordDate.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
            this.dateEditRecordDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.dateEditRecordDate.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;
            this.dateEditRecordDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditRecordDate.Size = new System.Drawing.Size(180, 21);
            this.dateEditRecordDate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "记录时间:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UCRecordDateTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateEditRecordDate);
            this.Name = "UCRecordDateTime";
            this.Size = new System.Drawing.Size(300, 35);
            ((System.ComponentModel.ISupportInitialize)(this.dateEditRecordDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditRecordDate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit dateEditRecordDate;
        private System.Windows.Forms.Label label1;
    }
}
