using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;

namespace DrectSoft.Core.DSReportManager
{
    /// <summary>
    /// 进入此页面的人员身份
    /// </summary>
    public abstract class UserRole
    {
        public UserRole()
        {
        }

        public abstract void RefreshValue(string noofinpat);
    }

    /// <summary>
    /// 申请人
    /// </summary>
    public class Applicant : UserRole
    {
        SqlHelper m_SqlHelper;
        IEmrHost m_App;
        public Applicant(SqlHelper sqlHelper, IEmrHost app)
        {
            m_SqlHelper = sqlHelper;
            m_App = app;
        }

        public override void RefreshValue(string reportID)
        {
            InitValue(reportID, m_SqlHelper, m_App);
        }

        private void InitValue(string reportID, SqlHelper sqlHelper, IEmrHost app)
        {
            DataTable dt = sqlHelper.GetReportCardInfo(reportID);
            if (dt.Rows.Count > 0)
            {
                string status = dt.Rows[0]["state"].ToString();
                string usercode = dt.Rows[0]["create_usercode"].ToString();
                ReportState rs = (ReportState)Enum.Parse(typeof(ReportState), status);
                if (app.User.DoctorId == usercode)
                {
                    if (rs == ReportState.Save || rs == ReportState.Withdraw || rs == ReportState.UnPassApprove)
                    {
                        //提交的表单处于“保存”“撤回”“否决”状态，且表单的创建人必须是系统登录人
                        m_CanSave = true;
                        m_CanDelete = true;
                        //add by yxy
                        m_CanSubmit = true;
                    }
                    else
                    {
                        m_CanSave = false;
                        m_CanDelete = false;
                        m_CanSubmit = false;
                    }
                }
            }
        }

        bool m_CanSave;
        /// <summary>
        /// 保存
        /// </summary>
        public bool CanSave
        {
            get { return m_CanSave; }
        }

        bool m_CanSubmit;
        /// <summary>
        /// 提交
        /// </summary>
        public bool CanSubmit
        {
            get { return m_CanSubmit; }
        }

        bool m_CanDelete;
        /// <summary>
        /// 删除
        /// </summary>
        public bool CanDelete
        {
            get { return m_CanDelete; }
        }

    }

    public class Auditor : UserRole
    {
        SqlHelper m_SqlHelper;
        public Auditor(SqlHelper sqlHelper)
        {
            m_SqlHelper = sqlHelper;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-12
        /// 1、add try ... catch
        /// <param name="reportID"></param>
        public override void RefreshValue(string reportID)
        {
            try
            {
                InitValue(reportID, m_SqlHelper);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-12
        /// 1、add try ... catch
        /// <param name="reportID"></param>
        /// <param name="sqlHelper"></param>
        private void InitValue(string reportID, SqlHelper sqlHelper)
        {
            try
            {
                DataTable dt = sqlHelper.GetReportCardInfo(reportID);
                if (dt.Rows.Count > 0)
                {
                    string status = dt.Rows[0]["state"].ToString();
                    if ((ReportState)Enum.Parse(typeof(ReportState), status) == ReportState.Submit)
                    {
                        //审核的记录必须是处于提交状态
                        m_CanPass = true;
                        m_CanReject = true;
                    }
                    else
                    {
                        m_CanPass = false;
                        m_CanReject = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool m_CanPass;
        /// <summary>
        /// 审核通过
        /// </summary>
        public bool CanPass
        {
            get { return m_CanPass; }
        }

        bool m_CanReject;
        /// <summary>
        /// 审核不通过
        /// </summary>
        public bool CanReject
        {
            get { return m_CanReject; }
        }

    }
}
