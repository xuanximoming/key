using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.DSSqlHelper;
using DrectSoft.Common.Ctrs.DLG;
using System.Data.SqlClient;
using DrectSoft.Core.Consultation.NEW.Enum;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.Emr.Util;
using DrectSoft.Common;
using DrectSoft.Service;
using DevExpress.Utils;

namespace Consultation.NEW
{
    /// <summary>
    /// 新的会诊审核界面
    /// Add xlb 2013-02-17
    /// </summary>
    public partial class FrmConsultForReview :DevBaseForm
    {
        #region 方法 Add xlb 2013-02-20

        string mconsultApplySn = string.Empty;//用来保存申请记录单号
        private FloderState floderState = FloderState.None;
        IEmrHost m_app;
        string nOofinpat = string.Empty;//病人病案号
        UCEmrInput m_UCEmrInput;
        bool m_IsLoadedEmrContent = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmConsultForReview()
        {
            try
            {
                InitializeComponent();
                if (!DesignMode)
                {
                    Register();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 注销 by xlb 2013-03-01
        ///// <summary>
        ///// 重载构造函数
        ///// </summary>
        ///// <param name="noofinpat">病人病案号</param>
        ///// <param name="host">接口</param>
        ///// <param name="consultSn">会诊申请单号</param>
        //public FrmConsultForReview(string noofinpat,IEmrHost host,string consultSn):this()
        //{
        //    try
        //    {
        //        mconsultApplySn = consultSn;
        //        m_app = host;
        //        nOofinpat = noofinpat;
        //        InitInfo();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        /// <summary>
        /// 构造函数重载
        /// Add xlb 2013-03-01
        /// </summary>
        /// <param name="noofinpat">病案号</param>
        /// <param name="host">接口</param>
        /// <param name="consultApplySn">申请会诊单号</param>
        /// <param name="readOnly">审核意见只读性</param>
        public FrmConsultForReview(string noofinpat, IEmrHost host, string consultApplySn, bool readOnly/*是否只读*/)
            : this()
        {
            try
            {
                nOofinpat = noofinpat;
                m_app = host;
                mconsultApplySn = consultApplySn;
                //会诊审核时申请用户控件始终不可编辑 即只读(true)
                ucConsultationApplyForMultiplyNew1.Init(nOofinpat, m_app, consultApplySn, true);
                SetReject(readOnly);
                ContextMenuStrip contextMenuStrip1=new ContextMenuStrip();
                DS_Common.CancelMenu(groupControl1, contextMenuStrip1);
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
                btnOk.Click+=new EventHandler(btnOk_Click);
                btnReject.Click+=new EventHandler(btnReject_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置会诊审核意见是否可编辑
        /// Add xlb 2013-03-01
        /// true:只读 false:可编辑
        /// </summary>
        private void SetReject(bool readOnly)
        {
            try
            {
                memoEditSuggestion.Properties.ReadOnly = readOnly;
                btnOk.Enabled = btnReject.Enabled = !readOnly;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 校验通过或否决时是否填写了相应的原因
        /// Add xlb 2013-02-21
        /// </summary>
        private bool CheckControl(ref string message,bool isOk)//isOk 表示是否通过 false则必须填写意见
        {
            try
            {
                if (string.IsNullOrEmpty(memoEditSuggestion.Text.Trim()) && !isOk)
                {
                    message = "否决时请填写审核意见";
                    memoEditSuggestion.Focus();
                    return false;
                }
                else if (memoEditSuggestion.Text.Trim().Length > 1500)
                {
                    message = "审核意见不能超过1500字";
                    return false;
                }
                string status = GetConsultSate();
                ConsultStatus constatus = (ConsultStatus)Enum.Parse(typeof(ConsultStatus), status);
                string info = constatus == ConsultStatus.CallBackConsultation ? "撤销" : "取消";
                if (status == Convert.ToString((int)ConsultStatus.CancelConsultion) 
                    || status == Convert.ToString((int)ConsultStatus.CallBackConsultation))
                {
                    message="该会诊申请已被" + info + "";
                    return false;
                }
                return true;
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
                xtraEmrPat.Controls.Add(m_UCEmrInput);
                m_UCEmrInput.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改审核状态通过则为待会诊否则为否决
        /// Add xlb 2013-02-21
        /// </summary>
        private void ModitifyConsultation(ConsultStatus consultStatus, string rejectReason, string consultapplySn)
        {
            try
            {
                string stateId = Convert.ToString((int)consultStatus);
                DS_SqlHelper.CreateSqlHelper();
                string isPassed = consultStatus == ConsultStatus.WaitConsultation ? "1" : "0";
                string sql = @"update consultapply set  stateid=@stateId,rejectreason=@rejectReason,ispassed=@ispassed where consultapplysn=@consultapplySn";
                SqlParameter[] sps ={
                                       new SqlParameter("@stateId",stateId),
                                       new SqlParameter("@rejectReason",rejectReason),
                                       new SqlParameter("@ispassed",isPassed),
                                       new SqlParameter("@consultapplysn",consultapplySn)
                                    };
                DS_SqlHelper.ExecuteNonQuery(sql,sps,CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
        /// 获取最新状态 若被取消或撤销则无需审核
        /// </summary>
        private string GetConsultSate()
        {
            try
            {
                string sql = string.Format("select stateID from consultapply where consultapplysn={0}", mconsultApplySn);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return "";
                }
                return dt.Rows[0][0].ToString();;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 新版文书录入
        DrectSoft.Core.MainEmrPad.New.UCEmrInput m_UCEmrInputNew;
        bool m_IsLoadedEmrContentNew = false;
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
                xtraEmrPat.Controls.Add(m_UCEmrInputNew);
                m_UCEmrInputNew.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 事件 Add xlb 2013-02-22

        /// <summary>
        /// 通过事件
        /// Add xlb 2013-02-21
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender,EventArgs e)
        {
            try
            {
                string message = "";
                if (!CheckControl(ref message, true))
                {
                    MessageBox.Show(message);
                    return;
                }
                ModitifyConsultation(ConsultStatus.WaitConsultation, memoEditSuggestion.Text.Trim(), mconsultApplySn);
                MessageBox.Show("审核通过");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 否决事件
        /// Add xlb 2013-02-20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                string message="";
                if (!CheckControl(ref message, false))
                {
                    MessageBox.Show(message);
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("确定否决本次会诊申请？","提示", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                ModitifyConsultation(ConsultStatus.Reject, memoEditSuggestion.Text.Trim(), mconsultApplySn);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmConsultForReview_Load(object sender, EventArgs e)
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
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体激活事件
        /// Add 2013-03-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmConsultForReview_Activated(object sender, EventArgs e)
        {
            try
            {
                groupControl1.Focus();
                memoEditSuggestion.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// tab切换事件
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraControlReview_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (xtraControlReview.SelectedTabPage == xtraEmrPat)
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

        #endregion
    }
}