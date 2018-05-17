using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Emr.QcManager
{
    public partial class QcManagerForm : Form, IStartPlugIn
    {
        IEmrHost m_app;
        InWardPatRecInfo m_WardInfo;
        OutHospitalNoSubmit m_OutHospital;
        OutHospitalNoLock m_OutHospitalLock;
        QualityMedicalRecord m_QualityMedicalRecord;

        OutMedicalScore m_OutMedicalScore;

        public QcManagerForm()
        {
            InitializeComponent();
        }



        private void LoadWardInfo()
        {

            if (m_WardInfo == null)
                m_WardInfo = new InWardPatRecInfo(m_app);
            m_WardInfo.Dock = DockStyle.Fill;
            xtraTabPage1.Controls.Add(m_WardInfo);
            m_WardInfo.RefreshData();
        }

        /// <summary>
        /// 绑定出院未提交信息
        /// </summary>
        private void LoadOutHospital()
        {

            if (m_OutHospital == null)
                m_OutHospital = new OutHospitalNoSubmit(m_app);

            m_OutHospital.Dock = DockStyle.Fill;
            xtraTabPageOutHospital.Controls.Add(m_OutHospital);
            xtraTabPageOutHospital.PageVisible = true;
            m_OutHospital.RefreshData();

            xtraTabControl1.SelectedTabPage = xtraTabPageOutHospital;

        }

        /// <summary>
        /// 绑定出院未归档病历
        /// </summary>
        private void LoadOutHospitalLock()
        {

            if (m_OutHospitalLock == null)
                m_OutHospitalLock = new OutHospitalNoLock(m_app);

            m_OutHospitalLock.Dock = DockStyle.Fill;
            xtraTabPageOutHospitalLock.Controls.Add(m_OutHospitalLock);
            xtraTabPageOutHospitalLock.PageVisible = true;
            m_OutHospitalLock.RefreshData();

            xtraTabControl1.SelectedTabPage = xtraTabPageOutHospitalLock;

        }

        /// <summary>
        /// 绑定时限质控
        /// 
        /// </summary>
        private void LoadQualityMedicalRecord()
        {
            xTabPageEmrScore.PageVisible = false;

            if (m_QualityMedicalRecord == null)
                m_QualityMedicalRecord = new QualityMedicalRecord(m_app);

            m_QualityMedicalRecord.Dock = DockStyle.Fill;
            xtraTabPageQualityMedicalRecord.Controls.Add(m_QualityMedicalRecord);
            xtraTabPageQualityMedicalRecord.PageVisible = true;

            xtraTabControl1.SelectedTabPage = xtraTabPageQualityMedicalRecord;

        }


        #region IStartPlugIn 成员

        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {
            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;

            return plg;
        }

        #endregion
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QcManagerForm_Load(object sender, EventArgs e)
        {
            //LoadWardInfo();
            LoadQualityMedicalRecord();
        }
        /// <summary>
        /// 出院未提交患者 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadOutHospital();
        }
        /// <summary>
        /// 出院未归档病历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadOutHospitalLock();
        }
        /// <summary>
        /// 标准评分大项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QCScoreType qcscoretype = new QCScoreType(m_app);
            qcscoretype.ShowDialog();
        }
        /// <summary>
        /// 标准评分细项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QCScoreItem qcscoreitem = new QCScoreItem(m_app);
            qcscoreitem.ShowDialog();
        }

        /// <summary>
        /// 医师医疗质量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_qc_doctor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QC_Doctor_Query m_QC_Doctor_Query = new QC_Doctor_Query(m_app);
            m_QC_Doctor_Query.ShowDialog();
        }

        /// <summary>
        /// 科室医疗质量统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_qc_dept_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QC_Dept_Query m_QC_Dept_Query = new QC_Dept_Query(m_app);
            m_QC_Dept_Query.ShowDialog();
        }

        /// <summary>
        /// 全院病历质控率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_hospital_info_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QC_Hospital_Rate m_QC_Hospital_Rate = new QC_Hospital_Rate(m_app);
            m_QC_Hospital_Rate.ShowDialog();
        }

        /// <summary>
        /// 科室病历质控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_dept_rate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QC_Dept_Record_Rate m_QC_Dept_Record_Rate = new QC_Dept_Record_Rate(m_app);
            m_QC_Dept_Record_Rate.ShowDialog();
        }

        /// <summary>
        /// 死亡统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QC_Die_Info m_QC_Die_Info = new QC_Die_Info(m_app);
            m_QC_Die_Info.ShowDialog();
        }

        /// <summary>
        /// 抢救统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QC_Rescue_Info m_QC_Rescue_Info = new QC_Rescue_Info(m_app);
            m_QC_Rescue_Info.ShowDialog();
        }

        /// <summary>
        /// 手术统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QC_Operat_Info m_QC_Operat_Info = new QC_Operat_Info(m_app);
            m_QC_Operat_Info.ShowDialog();
        }

        /// <summary>
        /// 单病种医疗质量统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_qc_SingleDisease_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QC_SingleDisease_Query m_QC_SingleDisease_Query = new QC_SingleDisease_Query(m_app);
            m_QC_SingleDisease_Query.ShowDialog();
        }

        /// <summary>
        /// 自动监控设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_monitor_item_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QC_monitor_item m_QC_monitor_item = new QC_monitor_item(m_app);
            m_QC_monitor_item.ShowDialog();
        }

        /// <summary>
        /// 单病种设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Disease_Level_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QC_Disease_Level m_QC_Disease_Level = new QC_Disease_Level(m_app);
            m_QC_Disease_Level.ShowDialog();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page.Name == "xtraTabPage1")
            {
                LoadWardInfo();
            }
        }
        /// <summary>
        /// 新增的病历评分报表页面
        /// add by ywk 2012年4月5日10:45:07
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnPointSum_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadMedicalSorce();
        }
        /// <summary>
        /// 病历评分页面
        /// </summary>
        private void LoadMedicalSorce()
        {
            if (m_OutMedicalScore == null)
                m_OutMedicalScore = new OutMedicalScore(m_app);

            m_OutMedicalScore.Dock = DockStyle.Fill;
            xTabPageEmrScore.Controls.Add(m_OutMedicalScore);
            xTabPageEmrScore.PageVisible = true;
            //m_OutMedicalScore.RefreshData();

            xtraTabControl1.SelectedTabPage = xTabPageEmrScore;

        }
        /// <summary>
        /// 科室质控人员配置
        /// add by ywk 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnConfigQCAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ConfigQCAudit configaudit = new ConfigQCAudit(m_app);
            configaudit.StartPosition = FormStartPosition.CenterScreen;
            configaudit.ShowDialog();

        }

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex < 1) return;
            else
                xtraTabControl1.SelectedTabPage.PageVisible = false;

        }
    }
}
