using DrectSoft.Common.Eop;
using DrectSoft.Core.RedactPatientInfo.PublicSet;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Core.RedactPatientInfo.UserControls
{
    public partial class UCClinicalInfo : UserControl
    {
        IEmrHost m_App;
        IDataAccess sql_Helper;

        UCBaseInfo m_UCBaseInfo;
        UCLinkman m_UCLinkman;
        UCDiacrisis m_UCDiacrisis;

        UCFamilyHistory m_UCFamilyHistory;
        UCPersonalHistory m_UCPersonalHistory;
        UCAllergicHistory m_UCAllergicHistory;
        UCOperationHistory m_UCOperationHistory;
        UCDiseaseHistory m_UCDiseaseHistory;
        DrectSoft.Core.InPatientReport.UCInpatientReport m_UCReport;
        //DrectSoft.Core.NursingDocuments.MainNursingMeasure m_NursingMeasure;


        /// <summary>
        /// 病人基本信息
        /// </summary>
        public Inpatient CurrentPat
        {
            get { return _currentPat; }
            set { _currentPat = value; }


        }
        private Inpatient _currentPat;


        DataTable m_Table;

        public UCClinicalInfo(IEmrHost app)
        {
            InitializeComponent();
            m_App = app;

            this.Load += new EventHandler(UCClinicalInfo_Load);
        }

        void UCClinicalInfo_Load(object sender, EventArgs e)
        {
            //初始化窗体控件
            InitForm();
        }

        private void InitForm()
        {
            SqlUtil.App = m_App;
            if (CurrentPat == null)
            {
                m_App.CustomMessageBox.MessageShow("请选择病患！");
                return;
            }
            else
            {
                //获取病人基本信息
                m_Table = SqlUtil.GetRedactPatientInfoFrm("14", "", CurrentPat.NoOfFirstPage.ToString());

                if (m_Table.Rows.Count <= 0)
                {
                    m_App.CustomMessageBox.MessageShow("病人没有基本信息！");
                    //return;
                }
            }


            //加载基本信息
            m_UCBaseInfo = new UCBaseInfo(m_Table, CurrentPat.NoOfFirstPage.ToString());
            m_UCBaseInfo.Dock = DockStyle.Fill;
            xtraTabPageBasInfo.Controls.Add(m_UCBaseInfo);

            //加载疾病史
            m_UCDiseaseHistory = new UCDiseaseHistory(CurrentPat.NoOfFirstPage.ToString());
            m_UCDiseaseHistory.Dock = DockStyle.Fill;
            xtraTabPageDiagInfo.Controls.Add(m_UCDiseaseHistory);

            //加载手术史
            m_UCOperationHistory = new UCOperationHistory(CurrentPat.NoOfFirstPage.ToString());
            m_UCOperationHistory.Dock = DockStyle.Fill;
            xtraTabPageOper.Controls.Add(m_UCOperationHistory);

            m_UCReport = new DrectSoft.Core.InPatientReport.UCInpatientReport(m_App);
            //m_UCReport.NoOfInpat = CurrentPat.NoOfFirstPage.ToString();
            m_UCReport.Dock = DockStyle.Fill;
            xtraTabPageLISRIS.Controls.Add(m_UCReport);
            m_UCReport.Visible = true;

            //m_NursingMeasure = new DrectSoft.Core.NursingDocuments.MainNursingMeasure(m_App);
            //m_NursingMeasure.Dock = DockStyle.Fill;
            //m_NursingMeasure.CurrentPat = CurrentPat;
            //xtraTabPagePhysical.Controls.Add(m_NursingMeasure);


        }


    }
}

