namespace YidanSoft.Core.Consultation
{
    partial class UCRecordResultForOne
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lookUpEditorDepartment = new YidanSoft.Common.Library.LookUpEditor();
            this.lookUpWindowDepartment = new YidanSoft.Common.Library.LookUpWindow(this.components);
            this.lookUpEditorEmployee = new YidanSoft.Common.Library.LookUpEditor();
            this.lookUpWindowEmployee = new YidanSoft.Common.Library.LookUpWindow(this.components);
            this.lookUpEditorHospital = new YidanSoft.Common.Library.LookUpEditor();
            this.lookUpWindowHospital = new YidanSoft.Common.Library.LookUpWindow(this.components);
            this.timeEditConsultationTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditConsultationDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.memoEditSuggestion = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorEmployee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowEmployee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorHospital)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowHospital)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEditConsultationTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lookUpEditorDepartment);
            this.groupControl1.Controls.Add(this.lookUpEditorEmployee);
            this.groupControl1.Controls.Add(this.lookUpEditorHospital);
            this.groupControl1.Controls.Add(this.timeEditConsultationTime);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.dateEditConsultationDate);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.memoEditSuggestion);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(700, 215);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "会诊记录信息";
            // 
            // lookUpEditorDepartment
            // 
            this.lookUpEditorDepartment.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDepartment.ListWindow = this.lookUpWindowDepartment;
            this.lookUpEditorDepartment.Location = new System.Drawing.Point(422, 157);
            this.lookUpEditorDepartment.Name = "lookUpEditorDepartment";
            this.lookUpEditorDepartment.ShowSButton = true;
            this.lookUpEditorDepartment.Size = new System.Drawing.Size(180, 21);
            this.lookUpEditorDepartment.TabIndex = 3;
            // 
            // lookUpWindowDepartment
            // 
            this.lookUpWindowDepartment.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDepartment.GenShortCode = null;
            this.lookUpWindowDepartment.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDepartment.Owner = null;
            this.lookUpWindowDepartment.SqlHelper = null;
            // 
            // lookUpEditorEmployee
            // 
            this.lookUpEditorEmployee.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorEmployee.ListWindow = this.lookUpWindowEmployee;
            this.lookUpEditorEmployee.Location = new System.Drawing.Point(156, 184);
            this.lookUpEditorEmployee.Name = "lookUpEditorEmployee";
            this.lookUpEditorEmployee.ShowSButton = true;
            this.lookUpEditorEmployee.Size = new System.Drawing.Size(175, 21);
            this.lookUpEditorEmployee.TabIndex = 4;
            // 
            // lookUpWindowEmployee
            // 
            this.lookUpWindowEmployee.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowEmployee.GenShortCode = null;
            this.lookUpWindowEmployee.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowEmployee.Owner = null;
            this.lookUpWindowEmployee.SqlHelper = null;
            // 
            // lookUpEditorHospital
            // 
            this.lookUpEditorHospital.Kind = YidanSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorHospital.ListWindow = this.lookUpWindowHospital;
            this.lookUpEditorHospital.Location = new System.Drawing.Point(156, 157);
            this.lookUpEditorHospital.Name = "lookUpEditorHospital";
            this.lookUpEditorHospital.ShowSButton = true;
            this.lookUpEditorHospital.Size = new System.Drawing.Size(175, 21);
            this.lookUpEditorHospital.TabIndex = 2;
            // 
            // lookUpWindowHospital
            // 
            this.lookUpWindowHospital.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowHospital.GenShortCode = null;
            this.lookUpWindowHospital.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowHospital.Owner = null;
            this.lookUpWindowHospital.SqlHelper = null;
            // 
            // timeEditConsultationTime
            // 
            this.timeEditConsultationTime.EditValue = new System.DateTime(2011, 5, 16, 0, 0, 0, 0);
            this.timeEditConsultationTime.Location = new System.Drawing.Point(532, 185);
            this.timeEditConsultationTime.Name = "timeEditConsultationTime";
            this.timeEditConsultationTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeEditConsultationTime.Size = new System.Drawing.Size(70, 21);
            this.timeEditConsultationTime.TabIndex = 6;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(90, 188);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 20;
            this.labelControl3.Text = "会诊医师：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(356, 160);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 18;
            this.labelControl2.Text = "会诊科室：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(90, 160);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 14);
            this.labelControl5.TabIndex = 22;
            this.labelControl5.Text = "会诊医院：";
            // 
            // dateEditConsultationDate
            // 
            this.dateEditConsultationDate.EditValue = null;
            this.dateEditConsultationDate.Location = new System.Drawing.Point(422, 185);
            this.dateEditConsultationDate.Name = "dateEditConsultationDate";
            this.dateEditConsultationDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditConsultationDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateEditConsultationDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditConsultationDate.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateEditConsultationDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditConsultationDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditConsultationDate.Size = new System.Drawing.Size(110, 21);
            this.dateEditConsultationDate.TabIndex = 5;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(356, 188);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 10;
            this.labelControl4.Text = "会诊时间：";
            // 
            // memoEditSuggestion
            // 
            this.memoEditSuggestion.Location = new System.Drawing.Point(17, 53);
            this.memoEditSuggestion.Name = "memoEditSuggestion";
            this.memoEditSuggestion.Size = new System.Drawing.Size(666, 96);
            this.memoEditSuggestion.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 33);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(84, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "会诊医师意见：";
            // 
            // UCRecordResultForOne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Name = "UCRecordResultForOne";
            this.Size = new System.Drawing.Size(700, 220);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorEmployee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowEmployee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorHospital)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowHospital)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEditConsultationTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSuggestion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit memoEditSuggestion;
        private DevExpress.XtraEditors.TimeEdit timeEditConsultationTime;
        private DevExpress.XtraEditors.DateEdit dateEditConsultationDate;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private YidanSoft.Common.Library.LookUpEditor lookUpEditorDepartment;
        private YidanSoft.Common.Library.LookUpEditor lookUpEditorEmployee;
        private YidanSoft.Common.Library.LookUpEditor lookUpEditorHospital;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowHospital;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowDepartment;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowEmployee;
    }
}
