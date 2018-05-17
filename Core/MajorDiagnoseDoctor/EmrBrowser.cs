using DevExpress.Utils;
using System;
using System.Windows.Forms;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Service;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Emr.Util;

namespace DrectSoft.Core.MajorDiagnoseDoctor
{
    /// <summary>
    /// <title>显示病历内容窗体</title>
    /// <auth>wyt</auth>
    /// <date>2012-11-09</date>
    /// </summary>
    public partial class EmrBrowser : DevBaseForm
    {
        private string m_NoOfFirstPage = string.Empty;
        private IEmrHost m_Host;
        private string m_ConsultApplySn = string.Empty;
        private FloderState floderState = FloderState.None;

        public EmrBrowser()
        {
            InitializeComponent();
        }

        public EmrBrowser(string noOfFirstPage, IEmrHost host)
            : this()
        {
            m_NoOfFirstPage = noOfFirstPage;
            m_Host = host;
        }


        #region 病历内容

        /// <summary>
        /// 病历内容窗体
        /// </summary>
        UCEmrInput m_UCEmrInput;
        bool m_IsLoadedEmrContent = false;

        private void AddEmrInput()
        {
            //m_UCEmrInput = new UCEmrInput("正在加载病历信息...", floderState);
            m_UCEmrInput = new UCEmrInput("正在加载病历信息...");
            m_UCEmrInput.CurrentInpatient = null;
            m_UCEmrInput.HideBar();
            RecordDal m_RecordDal = new RecordDal(m_Host.SqlHelper);
            m_UCEmrInput.SetInnerVar(m_Host, m_RecordDal);
            this.Controls.Add(m_UCEmrInput);
            m_UCEmrInput.Dock = DockStyle.Fill;

            if (!string.IsNullOrEmpty(m_NoOfFirstPage) && !m_IsLoadedEmrContent)
            {
                m_UCEmrInput.PatientChangedByIEmrHost(Convert.ToDecimal(m_NoOfFirstPage));
                m_UCEmrInput.HideBar();
                m_IsLoadedEmrContent = true;
            }
        }
        #endregion


        #region 病历内容 - 新版

        /// <summary>
        /// 病历内容窗体
        /// </summary>
        DrectSoft.Core.MainEmrPad.New.UCEmrInput m_UCEmrInputNew;

        /// <summary>
        /// 加载病历
        /// edit by Yanqiao.Cai 2012-11-23
        /// 1、add try ... catch
        /// </summary>
        private void AddEmrInputNew()
        {
            try
            {
                WaitDialogForm m_WaitDialog = new WaitDialogForm("正在加载病人信息...", "请稍候");
                m_Host.ChoosePatient(Convert.ToDecimal(m_NoOfFirstPage));//切换病人
                DS_Common.HideWaitDialog(m_WaitDialog);

                m_UCEmrInputNew = new DrectSoft.Core.MainEmrPad.New.UCEmrInput(m_Host.CurrentPatientInfo, m_Host, floderState);
                m_UCEmrInputNew.SetVarData(m_Host);
                this.Controls.Add(m_UCEmrInputNew);
                m_UCEmrInputNew.HideBar();
                m_UCEmrInputNew.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        private void EmrBrowser_Load(object sender, EventArgs e)
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
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
