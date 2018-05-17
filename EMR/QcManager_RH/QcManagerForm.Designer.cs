namespace DrectSoft.Emr.QcManager
{
    partial class QcManagerForm
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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.btn_hospital_rate = new DevExpress.XtraBars.BarButtonItem();
            this.btn_dept_rate = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
            this.btn_monitor_item = new DevExpress.XtraBars.BarButtonItem();
            this.btn_Disease_Level = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem4 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnPointSum = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnConfigQCAudit = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            this.btn_qc_doctor = new DevExpress.XtraBars.BarButtonItem();
            this.btn_qc_dept = new DevExpress.XtraBars.BarButtonItem();
            this.btn_qc_SingleDisease = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageQualityMedicalRecord = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageOutHospital = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageOutHospitalLock = new DevExpress.XtraTab.XtraTabPage();
            this.xTabPageEmrScore = new DevExpress.XtraTab.XtraTabPage();
            this.barConfigQCPoint = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barSubItem2,
            this.barSubItem3,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barBtnConfigQCAudit,
            this.barButtonItem3,
            this.barBtnPointSum,
            this.barButtonItem6,
            this.barButtonItem7,
            this.barCheckItem1,
            this.btn_qc_doctor,
            this.barSubItem4,
            this.barButtonItem9,
            this.barButtonItem10,
            this.btn_qc_dept,
            this.btn_hospital_rate,
            this.btn_dept_rate,
            this.btn_qc_SingleDisease,
            this.barButtonItem5,
            this.barButtonItem8,
            this.barButtonItem11,
            this.btn_monitor_item,
            this.btn_Disease_Level});
            this.barManager1.MaxItemId = 33;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem4)});
            this.bar1.Text = "Tools";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "质量统计";
            this.barSubItem1.Id = 2;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_hospital_rate),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_dept_rate)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // btn_hospital_rate
            // 
            this.btn_hospital_rate.Caption = "全院病历质控率";
            this.btn_hospital_rate.Id = 21;
            this.btn_hospital_rate.Name = "btn_hospital_rate";
            this.btn_hospital_rate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_hospital_info_ItemClick);
            // 
            // btn_dept_rate
            // 
            this.btn_dept_rate.Caption = "科室病历质控率";
            this.btn_dept_rate.Id = 22;
            this.btn_dept_rate.Name = "btn_dept_rate";
            this.btn_dept_rate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_dept_rate_ItemClick);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "汇总统计";
            this.barSubItem2.Id = 5;
            this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem8, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem11)});
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "医师医疗质量";
            this.barButtonItem5.Id = 24;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_doctor_ItemClick);
            // 
            // barButtonItem8
            // 
            this.barButtonItem8.Caption = "科室医疗质量统计";
            this.barButtonItem8.Id = 25;
            this.barButtonItem8.Name = "barButtonItem8";
            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_dept_ItemClick);
            // 
            // barButtonItem11
            // 
            this.barButtonItem11.Caption = "单病种医疗质量统计";
            this.barButtonItem11.Id = 26;
            this.barButtonItem11.Name = "barButtonItem11";
            this.barButtonItem11.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_SingleDisease_ItemClick);
            // 
            // barSubItem3
            // 
            this.barSubItem3.Caption = "质控标准设置";
            this.barSubItem3.Id = 6;
            this.barSubItem3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_monitor_item),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_Disease_Level)});
            this.barSubItem3.Name = "barSubItem3";
            // 
            // btn_monitor_item
            // 
            this.btn_monitor_item.Caption = "自动监控设置";
            this.btn_monitor_item.Id = 27;
            this.btn_monitor_item.Name = "btn_monitor_item";
            this.btn_monitor_item.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_monitor_item_ItemClick);
            // 
            // btn_Disease_Level
            // 
            this.btn_Disease_Level.Caption = "单病种质量评分标准";
            this.btn_Disease_Level.Id = 28;
            this.btn_Disease_Level.Name = "btn_Disease_Level";
            this.btn_Disease_Level.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_Disease_Level_ItemClick);
            // 
            // barSubItem4
            // 
            this.barSubItem4.Caption = "标准评分项";
            this.barSubItem4.Id = 17;
            this.barSubItem4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem9),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem10)});
            this.barSubItem4.Name = "barSubItem4";
            // 
            // barButtonItem9
            // 
            this.barButtonItem9.Caption = "标准评分大项";
            this.barButtonItem9.Id = 18;
            this.barButtonItem9.Name = "barButtonItem9";
            this.barButtonItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem9_ItemClick);
            // 
            // barButtonItem10
            // 
            this.barButtonItem10.Caption = "标准评分细项";
            this.barButtonItem10.Id = 19;
            this.barButtonItem10.Name = "barButtonItem10";
            this.barButtonItem10.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem10_ItemClick);
            // 
            // bar2
            // 
            this.bar2.BarName = "Custom 3";
            this.bar2.DockCol = 1;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnPointSum),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnConfigQCAudit),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem6),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem7),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_qc_doctor),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_qc_dept),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_qc_SingleDisease)});
            this.bar2.Text = "Custom 3";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "在院患者";
            this.barButtonItem1.Id = 7;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "出院未提交患者";
            this.barButtonItem2.Id = 8;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "出院未归档病历";
            this.barButtonItem3.Id = 9;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barBtnPointSum
            // 
            this.barBtnPointSum.Caption = "病历评分统计";
            this.barBtnPointSum.Id = 30;
            this.barBtnPointSum.Name = "barBtnPointSum";
            this.barBtnPointSum.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnPointSum_ItemClick);
            // 
            // barBtnConfigQCAudit
            // 
            this.barBtnConfigQCAudit.Caption = "质控人员配置";
            this.barBtnConfigQCAudit.Id = 32;
            this.barBtnConfigQCAudit.Name = "barBtnConfigQCAudit";
            this.barBtnConfigQCAudit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnConfigQCAudit_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "手术统计";
            this.barButtonItem6.Id = 12;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "抢救统计";
            this.barButtonItem7.Id = 13;
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem7_ItemClick);
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.Caption = "死亡统计";
            this.barCheckItem1.Id = 14;
            this.barCheckItem1.Name = "barCheckItem1";
            this.barCheckItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barCheckItem1.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem1_CheckedChanged);
            // 
            // btn_qc_doctor
            // 
            this.btn_qc_doctor.Caption = "医师医疗质量";
            this.btn_qc_doctor.Id = 15;
            this.btn_qc_doctor.Name = "btn_qc_doctor";
            this.btn_qc_doctor.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btn_qc_doctor.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_doctor_ItemClick);
            // 
            // btn_qc_dept
            // 
            this.btn_qc_dept.Caption = "科室医疗质量统计";
            this.btn_qc_dept.Id = 20;
            this.btn_qc_dept.Name = "btn_qc_dept";
            this.btn_qc_dept.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btn_qc_dept.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_dept_ItemClick);
            // 
            // btn_qc_SingleDisease
            // 
            this.btn_qc_SingleDisease.Caption = "单病种医疗质量统计";
            this.btn_qc_SingleDisease.Id = 23;
            this.btn_qc_SingleDisease.Name = "btn_qc_SingleDisease";
            this.btn_qc_SingleDisease.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btn_qc_SingleDisease.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_qc_SingleDisease_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(933, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 594);
            this.barDockControlBottom.Size = new System.Drawing.Size(933, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 563);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(933, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 563);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders;
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 31);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(933, 563);
            this.xtraTabControl1.TabIndex = 4;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageQualityMedicalRecord,
            this.xtraTabPage1,
            this.xtraTabPageOutHospital,
            this.xtraTabPageOutHospitalLock,
            this.xTabPageEmrScore});
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            this.xtraTabControl1.CloseButtonClick += new System.EventHandler(this.xtraTabControl1_CloseButtonClick);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.PageVisible = false;
            this.xtraTabPage1.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabPage1.Size = new System.Drawing.Size(927, 534);
            this.xtraTabPage1.Text = "在院患者列表";
            // 
            // xtraTabPageQualityMedicalRecord
            // 
            this.xtraTabPageQualityMedicalRecord.Name = "xtraTabPageQualityMedicalRecord";
            this.xtraTabPageQualityMedicalRecord.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabPageQualityMedicalRecord.Size = new System.Drawing.Size(927, 534);
            this.xtraTabPageQualityMedicalRecord.Text = "时限质控";
            // 
            // xtraTabPageOutHospital
            // 
            this.xtraTabPageOutHospital.Name = "xtraTabPageOutHospital";
            this.xtraTabPageOutHospital.PageVisible = false;
            this.xtraTabPageOutHospital.Size = new System.Drawing.Size(927, 534);
            this.xtraTabPageOutHospital.Text = "出院未提交病历";
            // 
            // xtraTabPageOutHospitalLock
            // 
            this.xtraTabPageOutHospitalLock.Name = "xtraTabPageOutHospitalLock";
            this.xtraTabPageOutHospitalLock.PageVisible = false;
            this.xtraTabPageOutHospitalLock.Size = new System.Drawing.Size(927, 534);
            this.xtraTabPageOutHospitalLock.Text = "出院未归档病历";
            // 
            // xTabPageEmrScore
            // 
            this.xTabPageEmrScore.Name = "xTabPageEmrScore";
            this.xTabPageEmrScore.Size = new System.Drawing.Size(927, 534);
            this.xTabPageEmrScore.Text = "病历评分列表";
            // 
            // barConfigQCPoint
            // 
            this.barConfigQCPoint.Caption = "科室质控人员配置";
            this.barConfigQCPoint.Id = 31;
            this.barConfigQCPoint.Name = "barConfigQCPoint";
            // 
            // QcManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 594);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "QcManagerForm";
            this.Text = "全院质量控制";
            this.Load += new System.EventHandler(this.QcManagerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarSubItem barSubItem3;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private DevExpress.XtraBars.BarButtonItem btn_qc_doctor;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageOutHospital;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageOutHospitalLock;
        private DevExpress.XtraBars.BarSubItem barSubItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
        private DevExpress.XtraBars.BarButtonItem barButtonItem10;
        private DevExpress.XtraBars.BarButtonItem btn_qc_dept;
        private DevExpress.XtraBars.BarButtonItem btn_hospital_rate;
        private DevExpress.XtraBars.BarButtonItem btn_dept_rate;
        private DevExpress.XtraBars.BarButtonItem btn_qc_SingleDisease;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
        private DevExpress.XtraBars.BarButtonItem barButtonItem11;
        private DevExpress.XtraBars.BarButtonItem btn_monitor_item;
        private DevExpress.XtraBars.BarButtonItem btn_Disease_Level;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageQualityMedicalRecord;
        private DevExpress.XtraBars.BarButtonItem barBtnPointSum;
        private DevExpress.XtraTab.XtraTabPage xTabPageEmrScore;
        private DevExpress.XtraBars.BarButtonItem barConfigQCPoint;
        private DevExpress.XtraBars.BarButtonItem barBtnConfigQCAudit;
    }
}