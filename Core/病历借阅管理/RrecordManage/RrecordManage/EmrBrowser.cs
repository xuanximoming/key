using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.Emr.Util;
using DrectSoft.Service;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Ctrs.DLG;
using DevExpress.Utils;

namespace DrectSoft.Core.RecordManage
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

        public EmrBrowser(string noOfFirstPage, IEmrHost host, FloderState state)
            : this()
        {
            m_NoOfFirstPage = noOfFirstPage;
            m_Host = host;
            floderState = state;
        }

        #region 病历内容 - 老版
        /// <summary>
        /// 病历内容窗体
        /// </summary>
        UCEmrInput m_UCEmrInput;
        bool m_IsLoadedEmrContent = false;

        /// <summary>
        /// 加载病历
        /// edit by Yanqiao.Cai 2012-11-23
        /// 1、add try ... catch
        /// </summary>
        private void AddEmrInput()
        {
            try
            {
                /// <summary>
                /// 病案管理模块标识
                /// 0-默认(非病案管理)
                /// 1-未归档病历
                /// 2-非未归档病历
                /// </summary>
                m_UCEmrInput = new UCEmrInput("正在加载病历信息...", floderState);

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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                m_Host.ChoosePatient(Convert.ToDecimal(m_NoOfFirstPage), floderState.ToString());//切换病人
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

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-23
        /// 1、add try ... catch
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
                MyMessageBox.Show(1,ex.Message);
            }
        }
    }
}
