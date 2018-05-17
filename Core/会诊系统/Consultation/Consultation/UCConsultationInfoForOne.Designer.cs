using System;
using System.Windows.Forms;
namespace YidanSoft.Core.Consultation
{
    partial class UCConsultationInfoForOne
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lookUpEditorDirector = new YidanSoft.Common.Library.LookUpEditor();
            this.lookUpEditorApplyEmployee = new YidanSoft.Common.Library.LookUpEditor();
            this.lookUpEditorEmployee = new YidanSoft.Common.Library.LookUpEditor();
            this.lookUpEditorDepartment = new YidanSoft.Common.Library.LookUpEditor();
            this.lookUpEditorHospital = new YidanSoft.Common.Library.LookUpEditor();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditApplyDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.timeEditApplyTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.textEditLocation = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditConsultationDate = new DevExpress.XtraEditors.DateEdit();
            this.timeEditConsultationTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.memoEditPurpose = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.memoEditAbstract = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.checkEditEmergency = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditNormal = new DevExpress.XtraEditors.CheckEdit();
            this.lookUpWindowHospital = new YidanSoft.Common.Library.LookUpWindow(this.components);
            this.lookUpWindowDepartment = new YidanSoft.Common.Library.LookUpWindow(this.components);
            this.lookUpWindowEmployee = new YidanSoft.Common.Library.LookUpWindow(this.components);
            this.lookUpWindowApplyEmployee = new YidanSoft.Common.Library.LookUpWindow(this.components);
            this.lookUpWindowDirector = new YidanSoft.Common.Library.LookUpWindow(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDirector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorApplyEmployee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorEmployee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorHospital)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditApplyDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditApplyDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEditApplyTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditLocation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEditConsultationTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditPurpose.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditAbstract.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditEmergency.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditNormal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowHospital)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowEmployee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowApplyEmployee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDirector)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.panelControl1);
            this.groupControl1.Controls.Add(this.memoEditPurpose);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.memoEditAbstract);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.checkEditEmergency);
            this.groupControl1.Controls.Add(this.checkEditNormal);
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(700, 415);
            this.groupControl1.TabIndex = 11;
            this.groupControl1.Text = "会诊申请信息";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lookUpEditorDirector);
            this.panelControl1.Controls.Add(this.lookUpEditorApplyEmployee);
            this.panelControl1.Controls.Add(this.lookUpEditorEmployee);
            this.panelControl1.Controls.Add(this.lookUpEditorDepartment);
            this.panelControl1.Controls.Add(this.lookUpEditorHospital);
            this.panelControl1.Controls.Add(this.labelControl7);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.dateEditApplyDate);
            this.panelControl1.Controls.Add(this.labelControl12);
            this.panelControl1.Controls.Add(this.labelControl11);
            this.panelControl1.Controls.Add(this.timeEditApplyTime);
            this.panelControl1.Controls.Add(this.labelControl10);
            this.panelControl1.Controls.Add(this.textEditLocation);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.dateEditConsultationDate);
            this.panelControl1.Controls.Add(this.timeEditConsultationTime);
            this.panelControl1.Controls.Add(this.labelControl8);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Location = new System.Drawing.Point(22, 288);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(651, 116);
            this.panelControl1.TabIndex = 14;
            // 
            // lookUpEditorDirector
            // 
            this.lookUpEditorDirector.ListWindow = null;
            this.lookUpEditorDirector.Location = new System.Drawing.Point(513, 80);
            this.lookUpEditorDirector.Name = "lookUpEditorDirector";
            this.lookUpEditorDirector.ShowSButton = true;
            this.lookUpEditorDirector.Size = new System.Drawing.Size(126, 21);
            this.lookUpEditorDirector.TabIndex = 14;
            // 
            // lookUpEditorApplyEmployee
            // 
            this.lookUpEditorApplyEmployee.ListWindow = null;
            this.lookUpEditorApplyEmployee.Location = new System.Drawing.Point(324, 80);
            this.lookUpEditorApplyEmployee.Name = "lookUpEditorApplyEmployee";
            this.lookUpEditorApplyEmployee.ShowSButton = true;
            this.lookUpEditorApplyEmployee.Size = new System.Drawing.Size(117, 21);
            this.lookUpEditorApplyEmployee.TabIndex = 13;
            // 
            // lookUpEditorEmployee
            // 
            this.lookUpEditorEmployee.ListWindow = null;
            this.lookUpEditorEmployee.Location = new System.Drawing.Point(513, 12);
            this.lookUpEditorEmployee.Name = "lookUpEditorEmployee";
            this.lookUpEditorEmployee.ShowSButton = true;
            this.lookUpEditorEmployee.Size = new System.Drawing.Size(126, 21);
            this.lookUpEditorEmployee.TabIndex = 7;
            // 
            // lookUpEditorDepartment
            // 
            this.lookUpEditorDepartment.ListWindow = null;
            this.lookUpEditorDepartment.Location = new System.Drawing.Point(324, 12);
            this.lookUpEditorDepartment.Name = "lookUpEditorDepartment";
            this.lookUpEditorDepartment.ShowSButton = true;
            this.lookUpEditorDepartment.Size = new System.Drawing.Size(117, 21);
            this.lookUpEditorDepartment.TabIndex = 6;
            // 
            // lookUpEditorHospital
            // 
            this.lookUpEditorHospital.ListWindow = null;
            this.lookUpEditorHospital.Location = new System.Drawing.Point(87, 12);
            this.lookUpEditorHospital.Name = "lookUpEditorHospital";
            this.lookUpEditorHospital.ShowSButton = true;
            this.lookUpEditorHospital.Size = new System.Drawing.Size(160, 21);
            this.lookUpEditorHospital.TabIndex = 5;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(451, 15);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(60, 14);
            this.labelControl7.TabIndex = 30;
            this.labelControl7.Text = "受邀医师：";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(9, 15);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 14);
            this.labelControl6.TabIndex = 29;
            this.labelControl6.Text = "受邀医院：";
            // 
            // dateEditApplyDate
            // 
            this.dateEditApplyDate.EditValue = null;
            this.dateEditApplyDate.Location = new System.Drawing.Point(87, 80);
            this.dateEditApplyDate.Name = "dateEditApplyDate";
            this.dateEditApplyDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditApplyDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateEditApplyDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditApplyDate.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateEditApplyDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditApplyDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditApplyDate.Size = new System.Drawing.Size(88, 21);
            this.dateEditApplyDate.TabIndex = 11;
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(451, 83);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(48, 14);
            this.labelControl12.TabIndex = 26;
            this.labelControl12.Text = "科主任：";
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(258, 83);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(60, 14);
            this.labelControl11.TabIndex = 25;
            this.labelControl11.Text = "申请医师：";
            // 
            // timeEditApplyTime
            // 
            this.timeEditApplyTime.EditValue = new System.DateTime(2011, 5, 16, 0, 0, 0, 0);
            this.timeEditApplyTime.Location = new System.Drawing.Point(175, 80);
            this.timeEditApplyTime.Name = "timeEditApplyTime";
            this.timeEditApplyTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeEditApplyTime.Size = new System.Drawing.Size(72, 21);
            this.timeEditApplyTime.TabIndex = 12;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(9, 83);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(60, 14);
            this.labelControl10.TabIndex = 21;
            this.labelControl10.Text = "申请时间：";
            // 
            // textEditLocation
            // 
            this.textEditLocation.Location = new System.Drawing.Point(324, 47);
            this.textEditLocation.Name = "textEditLocation";
            this.textEditLocation.Size = new System.Drawing.Size(315, 21);
            this.textEditLocation.TabIndex = 10;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(258, 50);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(60, 14);
            this.labelControl9.TabIndex = 19;
            this.labelControl9.Text = "会诊地点：";
            // 
            // dateEditConsultationDate
            // 
            this.dateEditConsultationDate.EditValue = null;
            this.dateEditConsultationDate.Location = new System.Drawing.Point(87, 47);
            this.dateEditConsultationDate.Name = "dateEditConsultationDate";
            this.dateEditConsultationDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditConsultationDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateEditConsultationDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditConsultationDate.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateEditConsultationDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dateEditConsultationDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditConsultationDate.Size = new System.Drawing.Size(88, 21);
            this.dateEditConsultationDate.TabIndex = 8;
            // 
            // timeEditConsultationTime
            // 
            this.timeEditConsultationTime.EditValue = new System.DateTime(2011, 5, 16, 0, 0, 0, 0);
            this.timeEditConsultationTime.Location = new System.Drawing.Point(175, 47);
            this.timeEditConsultationTime.Name = "timeEditConsultationTime";
            this.timeEditConsultationTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeEditConsultationTime.Size = new System.Drawing.Size(72, 21);
            this.timeEditConsultationTime.TabIndex = 9;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(9, 50);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(72, 14);
            this.labelControl8.TabIndex = 16;
            this.labelControl8.Text = "拟会诊时间：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(258, 15);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 14);
            this.labelControl5.TabIndex = 11;
            this.labelControl5.Text = "受邀科室：";
            // 
            // memoEditPurpose
            // 
            this.memoEditPurpose.Location = new System.Drawing.Point(22, 196);
            this.memoEditPurpose.Name = "memoEditPurpose";
            this.memoEditPurpose.Size = new System.Drawing.Size(651, 82);
            this.memoEditPurpose.TabIndex = 4;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(22, 176);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(120, 14);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "申请会诊目的和要求：";
            // 
            // memoEditAbstract
            // 
            this.memoEditAbstract.Location = new System.Drawing.Point(22, 73);
            this.memoEditAbstract.Name = "memoEditAbstract";
            this.memoEditAbstract.Size = new System.Drawing.Size(651, 96);
            this.memoEditAbstract.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(22, 53);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "病历摘要：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 30);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "紧急度：";
            // 
            // checkEditEmergency
            // 
            this.checkEditEmergency.Location = new System.Drawing.Point(151, 28);
            this.checkEditEmergency.Name = "checkEditEmergency";
            this.checkEditEmergency.Properties.Caption = "急会诊";
            this.checkEditEmergency.Size = new System.Drawing.Size(75, 19);
            this.checkEditEmergency.TabIndex = 2;
            // 
            // checkEditNormal
            // 
            this.checkEditNormal.Location = new System.Drawing.Point(76, 28);
            this.checkEditNormal.Name = "checkEditNormal";
            this.checkEditNormal.Properties.Caption = "普通会诊";
            this.checkEditNormal.Size = new System.Drawing.Size(75, 19);
            this.checkEditNormal.TabIndex = 1;
            // 
            // lookUpWindowHospital
            // 
            this.lookUpWindowHospital.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowHospital.GenShortCode = null;
            this.lookUpWindowHospital.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowHospital.Owner = null;
            this.lookUpWindowHospital.SqlHelper = null;
            // 
            // lookUpWindowDepartment
            // 
            this.lookUpWindowDepartment.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDepartment.GenShortCode = null;
            this.lookUpWindowDepartment.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDepartment.Owner = null;
            this.lookUpWindowDepartment.SqlHelper = null;
            // 
            // lookUpWindowEmployee
            // 
            this.lookUpWindowEmployee.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowEmployee.GenShortCode = null;
            this.lookUpWindowEmployee.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowEmployee.Owner = null;
            this.lookUpWindowEmployee.SqlHelper = null;
            // 
            // lookUpWindowApplyEmployee
            // 
            this.lookUpWindowApplyEmployee.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowApplyEmployee.GenShortCode = null;
            this.lookUpWindowApplyEmployee.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowApplyEmployee.Owner = null;
            this.lookUpWindowApplyEmployee.SqlHelper = null;
            // 
            // lookUpWindowDirector
            // 
            this.lookUpWindowDirector.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDirector.GenShortCode = null;
            this.lookUpWindowDirector.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDirector.Owner = null;
            this.lookUpWindowDirector.SqlHelper = null;
            // 
            // UCConsultationInfoForOne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Name = "UCConsultationInfoForOne";
            this.Size = new System.Drawing.Size(700, 500);
            this.Load += new System.EventHandler(this.UCConsultationInfoForOne_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDirector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorApplyEmployee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorEmployee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorHospital)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditApplyDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditApplyDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEditApplyTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditLocation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditConsultationDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEditConsultationTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditPurpose.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditAbstract.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditEmergency.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditNormal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowHospital)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowEmployee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowApplyEmployee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDirector)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckEdit checkEditNormal;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit checkEditEmergency;
        private DevExpress.XtraEditors.MemoEdit memoEditPurpose;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.MemoEdit memoEditAbstract;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TimeEdit timeEditApplyTime;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.TextEdit textEditLocation;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.DateEdit dateEditConsultationDate;
        private DevExpress.XtraEditors.TimeEdit timeEditConsultationTime;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.DateEdit dateEditApplyDate;
        private YidanSoft.Common.Library.LookUpEditor lookUpEditorEmployee;
        private YidanSoft.Common.Library.LookUpEditor lookUpEditorDepartment;
        private YidanSoft.Common.Library.LookUpEditor lookUpEditorHospital;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowHospital;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowDepartment;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowEmployee;
        private YidanSoft.Common.Library.LookUpEditor lookUpEditorDirector;
        private YidanSoft.Common.Library.LookUpEditor lookUpEditorApplyEmployee;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowApplyEmployee;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowDirector;
    }
}
