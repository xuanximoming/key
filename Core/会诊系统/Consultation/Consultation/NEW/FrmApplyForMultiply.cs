using DevExpress.Utils;
using System;
using System.Data;
using System.Windows.Forms;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Service;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;
using DrectSoft.Emr.Util;

namespace Consultation.NEW
{
    /// <summary>
    /// 会诊申请界面
    /// xlb 2013-02-17
    /// </summary>
    public partial class FormApplyForMultiply : DevBaseForm
    {

        string nOofinpat = string.Empty;//病人首页序号
        private FloderState floderState = FloderState.None;
        IEmrHost m_app;
        string consultApplyId;
        UCEmrInput m_UCEmrInput;
        DrectSoft.Core.MainEmrPad.New.UCEmrInput m_UCEmrInputNew;
        bool m_IsLoadedEmrContent = false;
        bool m_IsLoadedEmrContentNew = false;

        #region 方法 by xlb 2013-02-17

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormApplyForMultiply()
        {
            try
            {
                InitializeComponent();
                //非设计模式执行
                if (!DesignMode)
                {
                    DS_SqlHelper.CreateSqlHelper();
                    Register();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 构造函数的重载
        /// 院内会诊
        /// Add xlb 2013-03-01
        /// </summary>
        /// <param name="noofinpat">病案号</param>
        /// <param name="host"></param>
        /// <param name="consultApplySn">申请会诊单号</param>
        public FormApplyForMultiply(string noofinpat, IEmrHost host, string consultApplySn, bool readOnly/*是否只读*/)
            : this()
        {
            try
            {
                nOofinpat = noofinpat;
                m_app = host;
                consultApplyId = consultApplySn;
                ucConsultationApplyForMultiplyNew1.Init(nOofinpat, m_app, consultApplySn, readOnly);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="host"></param>
        public FormApplyForMultiply(string noofinpat, IEmrHost host)
            : this()
        {
            try
            {
                nOofinpat = noofinpat;
                m_app = host;
                ucConsultationApplyForMultiplyNew1.Init(nOofinpat, m_app, "", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 注册事件
        /// Add xlb 2013-02-21
        /// </summary>
        private void Register()
        {
            try
            {
                this.Load += new EventHandler(FormApplyForMultiply_Load);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据配置来控制是否显示审核意见
        /// Add xlb 2013-02-22
        /// </summary>
        private void ShowReviewYj()
        {
            try
            {
                string isNeedApprove = ConsultCommon.GetConfigKey("ConsultAuditConfig").Split('|')[0] == "1" ? "1" : "0";
                if (isNeedApprove == "0" || string.IsNullOrEmpty(consultApplyId))
                {
                    groupControlReview.Visible = false;
                    this.Height += 4;
                    return;
                }
                InitApprove(consultApplyId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region  病历信息相关内容 使用原先的方法 by 项令波 2013-03-01

        /// <summary>
        /// 加载病历内容
        /// Add xlb 2013-03-01
        /// </summary>
        private void LoadEmrContent()
        {
            try
            {
                if (!string.IsNullOrEmpty(nOofinpat) && !m_IsLoadedEmrContent)
                {
                    m_UCEmrInput.PatientChangedByIEmrHost(Convert.ToDecimal(nOofinpat));
                    m_UCEmrInput.HideBar();
                    m_IsLoadedEmrContent = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载文书录入界面
        /// Add xlb 2013-03-01
        /// </summary>
        private void AddEmrInput()
        {
            try
            {
                m_UCEmrInput = new UCEmrInput();
                m_UCEmrInput.CurrentInpatient = null;
                m_UCEmrInput.HideBar();
                RecordDal m_RecordDal = new RecordDal(m_app.SqlHelper);
                m_UCEmrInput.SetInnerVar(m_app, m_RecordDal);
                xtraEmrInfo.Controls.Add(m_UCEmrInput);
                m_UCEmrInput.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 新版文书录入
        /// <summary>
        /// 加载病历内容
        /// </summary>
        private void LoadEmrContentNew()
        {
            try
            {
                if (!string.IsNullOrEmpty(nOofinpat) && !m_IsLoadedEmrContentNew)
                {
                    m_UCEmrInputNew.PatientChangedByIEmrHost(Convert.ToDecimal(nOofinpat));
                    m_UCEmrInputNew.HideBar();
                    m_IsLoadedEmrContentNew = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载文书录入界面
        /// </summary>
        private void AddEmrInputNew()
        {
            try
            {
                WaitDialogForm m_WaitDialog = new WaitDialogForm("正在加载病人信息...", "请稍候");
                m_app.ChoosePatient(Convert.ToDecimal(nOofinpat));//切换病人
                DS_Common.HideWaitDialog(m_WaitDialog);

                m_UCEmrInputNew = new DrectSoft.Core.MainEmrPad.New.UCEmrInput(m_app.CurrentPatientInfo, m_app, floderState);
                m_UCEmrInputNew.SetVarData(m_app);
                xtraEmrInfo.Controls.Add(m_UCEmrInputNew);
                m_UCEmrInputNew.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 初始化审核意见区
        /// Add xlb 2013-02-22
        /// Modify by xlb 2013-03-18
        /// </summary>
        /// <param name="consultApplySn"></param>
        private void InitApprove(string consultApplySn)
        {
            try
            {
                DataSet ds = DrectSoft.Core.Consultation.Dal.DataAccess.GetConsultationDataSet(consultApplySn, "20");//, Convert.ToString((int)ConsultType.More));
                DataTable dtConsultApply = ds.Tables[0];
                if (dtConsultApply == null || dtConsultApply.Rows.Count <= 0
                    || string.IsNullOrEmpty(dtConsultApply.Rows[0]["RejectReason"].ToString().Trim()))
                {
                    groupControlReview.Visible = false;
                    this.Height += 4;
                }
                memoEditSuggestion.Text = dtConsultApply.Rows[0]["RejectReason"].ToString().Trim();
                if (!string.IsNullOrEmpty(memoEditSuggestion.Text.Trim()))
                {
                    this.Height += groupControlReview.Height;//有否决意见则调整界面大小以展示否决意见
                    //int x = this.Location.X;//当前界面起始位置的横坐标
                    //int y = this.Location.Y;//起始位置的纵坐标
                    CenterToScreen();//窗体居中
                    //this.Location = new Point();//调整界面的大小后引发的部门区域被隐藏
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 窗体加载事件
        /// Add xlb 2013-02-21
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormApplyForMultiply_Load(object sender, EventArgs e)
        {
            try
            {
                string config = DS_SqlService.GetConfigValueByKey("IsNewUcInput");
                if (null != config && config.Trim() == "1")
                {
                    AddEmrInputNew();
                }
                else
                {
                    AddEmrInput();
                }
                ShowReviewYj();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// tabControl切换事件
        /// Add xlb 2013-02-21
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == xtraEmrInfo)
                {
                    string config = DS_SqlService.GetConfigValueByKey("IsNewUcInput");
                    if (null != config && config.Trim() == "1")
                    {
                        LoadEmrContentNew();
                    }
                    else
                    {
                        LoadEmrContent();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}