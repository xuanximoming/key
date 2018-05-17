using DevExpress.Utils;
using System;
using System.Data;
using System.Windows.Forms;
using DrectSoft.Core.RedactPatientInfo.PublicSet;
using DrectSoft.Core.RedactPatientInfo.UserControls;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.RedactPatientInfo
{
    public partial class BasePatientInfo : DevExpress.XtraEditors.XtraForm
    {
        IEmrHost m_app;
        IDataAccess sql_Helper;

        UCBaseInfo m_UCBaseInfo;
        UCLinkman m_UCLinkman;
        UCDiacrisis m_UCDiacrisis;

        //UCFamilyHistory m_UCFamilyHistory;
        //UCPersonalHistory m_UCPersonalHistory;
        //UCAllergicHistory m_UCAllergicHistory;
        //UCOperationHistory m_UCOperationHistory;
        //UCDiseaseHistory m_UCDiseaseHistory;


        string m_NoOfInpat;
        DataTable m_Table;

        public BasePatientInfo(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            //m_WaitDialog = new WaitDialogForm("创建用户界面……", "请稍等。");
        }

        #region IStartPlugIn 成员

        #endregion

        private WaitDialogForm m_WaitDialog;

        public void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }

        }

        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }

        /// <summary>
        /// 设置首页序号
        /// </summary>
        public DialogResult ShowCurrentPatInfo()
        {
            m_NoOfInpat = m_app.CurrentPatientInfo.NoOfFirstPage.ToString();
            return ShowDialog();

        }

        /// <summary>
        /// 设置首页序号
        /// </summary>
        /// <param name="NoOfInpat">首页序号</param>
        public DialogResult ShowCurrentPatInfo(string NoOfInpat)
        {
            m_NoOfInpat = NoOfInpat;
            return ShowDialog();

        }
        private void BasePatientInfo_Load(object sender, EventArgs e)
        {
            SqlUtil.App = m_app;

            SetWaitDialogCaption("正在读取患者基本信息");

            if (string.IsNullOrEmpty(m_NoOfInpat))
            {
                SqlUtil.App.CustomMessageBox.MessageShow("无法找到病人基本信息！");
                btnSave.Enabled = false;
                //return;
            }
            else
            {
                //获取病人基本信息
                m_Table = SqlUtil.GetRedactPatientInfoFrm("14", "", m_NoOfInpat);

                if (m_Table.Rows.Count <= 0)
                {
                    SqlUtil.App.CustomMessageBox.MessageShow("病人没有基本信息！");
                    btnSave.Enabled = false;
                    //return;
                }
            }

            //加载基本信息
            m_UCBaseInfo = new UCBaseInfo(m_Table, m_NoOfInpat);
            m_UCBaseInfo.Dock = DockStyle.Fill;
            tabPageBaseInfo.Controls.Add(m_UCBaseInfo);

            //加载第一联系人
            m_UCLinkman = new UCLinkman(m_NoOfInpat);
            m_UCLinkman.Dock = DockStyle.Fill;
            tabPageLinkman.Controls.Add(m_UCLinkman);

            // SetWaitDialogCaption("正在读取患者就诊信息");
            //加载就诊信息
            m_UCDiacrisis = new UCDiacrisis(m_Table, m_NoOfInpat);
            m_UCDiacrisis.Dock = DockStyle.Fill;
            tabPageDiacrisis.Controls.Add(m_UCDiacrisis);

            //SetWaitDialogCaption("正在读取患者家族史");
            ////加载家族史
            //m_UCFamilyHistory = new UCFamilyHistory(m_NoOfInpat);
            //m_UCFamilyHistory.Dock = DockStyle.Fill;
            //this.tabPageFamilyHistory.Controls.Add(m_UCFamilyHistory);

            ////加载个人史
            //m_UCPersonalHistory = new UCPersonalHistory(m_NoOfInpat);
            //m_UCPersonalHistory.Dock = DockStyle.Fill;
            //this.tabPagePersonalHistory.Controls.Add(m_UCPersonalHistory);

            ////加载过敏史
            //m_UCAllergicHistory = new UCAllergicHistory(m_NoOfInpat);
            //m_UCAllergicHistory.Dock = DockStyle.Fill;
            //this.tabPageAllergicHistory.Controls.Add(m_UCAllergicHistory);

            ////加载手术史
            //m_UCOperationHistory = new UCOperationHistory(m_NoOfInpat);
            //m_UCOperationHistory.Dock = DockStyle.Fill;
            //tabPageOperationHistory.Controls.Add(m_UCOperationHistory);

            ////加载疾病史
            //m_UCDiseaseHistory = new UCDiseaseHistory(m_NoOfInpat);
            //m_UCDiseaseHistory.Dock = DockStyle.Fill;
            //tabPageDiseaseHistory.Controls.Add(m_UCDiseaseHistory);
            //HideWaitDialog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            if (tabPatientInfo.TabPages.TabControl.SelectedTabPage == tabPageBaseInfo)
            {
                //保存基本信息
                m_UCBaseInfo.SaveBaseInfo();
            }
            else if (tabPatientInfo.TabPages.TabControl.SelectedTabPage == tabPageLinkman)
            {
                //保存第一联系人信息
                m_UCLinkman.SaveUCLinkmanInfo(true);
            }
            else if (tabPatientInfo.TabPages.TabControl.SelectedTabPage == tabPageDiacrisis)
            {
                //保存就诊信息
                m_UCDiacrisis.SaveUCDiacrisisInfo();
            }
            //else if (tabPatientInfo.TabPages.TabControl.SelectedTabPage == tabPageFamilyHistory)
            //{
            //    //保存家族史信息
            //    m_UCFamilyHistory.SaveUCFamilyHistoryInfo(true);
            //}
            //else if (tabPatientInfo.TabPages.TabControl.SelectedTabPage == tabPagePersonalHistory)
            //{
            //    //保存个人史信息
            //    m_UCPersonalHistory.SaveUCPersonalHistory();
            //}
            //else if (tabPatientInfo.TabPages.TabControl.SelectedTabPage == tabPageAllergicHistory)
            //{
            //    //保存过敏史信息
            //    m_UCAllergicHistory.SaveUCAllergicHistoryInfo(true);
            //}
            //else if (tabPatientInfo.TabPages.TabControl.SelectedTabPage == tabPageOperationHistory)
            //{
            //    //保存手术史信息
            //    m_UCOperationHistory.SaveUCOperationHistoryInfo(true);
            //}
            //else if (tabPatientInfo.TabPages.TabControl.SelectedTabPage == tabPageDiseaseHistory)
            //{
            //    //保存疾病史信息
            //    m_UCDiseaseHistory.SaveUCDiseaseHistoryInfo(true);
            //}

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }









    }
}