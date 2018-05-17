using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.DSSqlHelper;
using System.Data;
using DrectSoft.Common.Eop;
using System.Xml;
using Consultation.NEW;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Windows.Forms;
using DrectSoft.Core.Consultation.NEW.Enum;

namespace DrectSoft.Core.Consultation.NEW
{
    /// <summary>
    /// Add by wwj 2013-03-01
    /// 会诊相关逻辑
    /// </summary>
    public class ProcessClickConsultatonListLogic
    {
        IEmrHost m_app;
        string noofinpat;//病人病案号

        /// <summary>
        /// 构造函数
        /// Add xlb 2013-03-01
        /// </summary>
        /// <param name="host"></param>
        public ProcessClickConsultatonListLogic(IEmrHost host,string nOofinpat)
        {
            try
            {
                m_app = host;
                noofinpat = nOofinpat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 医生工作站中点击右侧会诊列表的逻辑判断
        /// </summary>
        /// <param name="currentUserID">当前系统登录人</param>
        /// <param name="consultApplySN">会诊申请单号</param>
        /// <returns>操作是否成功 true:成功 false:失败</returns>
        public bool ProcessLogic(string currentUserID, string consultApplySN)
        {
            try
            {
                bool returnValue = true;
                Enum.ConsultStatus consultStatus = GetConsultationState(consultApplySN);
                switch (consultStatus)
                {
                    case Enum.ConsultStatus.ApplySave: //会诊申请记录保存
                        returnValue = ProcessApplySave(currentUserID, consultApplySN);
                        break;
                    case Enum.ConsultStatus.WaitApprove: //待审核的会诊记录
                        returnValue = ProcessWaitApprove(currentUserID, consultApplySN);
                        break;
                    case Enum.ConsultStatus.WaitConsultation: //处理待会诊的记录
                    case Enum.ConsultStatus.RecordeSave: //会诊记录单保存的记录
                        returnValue = ProcessWaitConsultationOrRecordeSave(currentUserID, consultApplySN);
                        break;
                    case Enum.ConsultStatus.RecordeComplete: //会诊记录完成的记录
                        returnValue = ProcessRecordeComplete(currentUserID, consultApplySN);
                        break;
                    case Enum.ConsultStatus.Reject: //被否决的会诊记录
                        returnValue = ProcessReject(currentUserID, consultApplySN);
                        break;
                    case Enum.ConsultStatus.CancelConsultion: //被取消的会诊记录
                        returnValue = ProcessCancelConsultion(currentUserID, consultApplySN);
                        break;
                    case Enum.ConsultStatus.CallBackConsultation://已经撤销的会诊记录
                        returnValue = ProcessCallBackConsultation(currentUserID, consultApplySN);
                        break;
                    default:
                        throw new Exception("无此会诊状态的处理程序！");
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过会诊单号获取有效的会诊的主表信息
        /// </summary>
        /// <param name="consultApplySN"></param>
        /// <returns></returns>
        private DataTable GetConsultationBySN(string consultApplySN)
        {
            try
            {
                string sqlGetConsultation = "select * from consultapply where consultapplysn = '{0}' and valid = '1'";
                return DS_SqlHelper.ExecuteDataTable(string.Format(sqlGetConsultation, consultApplySN), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过会诊单号获取有效的会诊受邀医师情况
        /// </summary>
        /// <param name="consultApplySN"></param>
        /// <returns></returns>
        private DataTable GetConsultationDepartmentBySN(string consultApplySN)
        {
            try
            {
                string sqlGetConsultationDepartment = "select * from consultrecorddepartment where consultapplysn = '{0}' and valid = '1'";
                return DS_SqlHelper.ExecuteDataTable(string.Format(sqlGetConsultationDepartment, consultApplySN), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取会诊单的状态
        /// </summary>
        /// <param name="consultApplySN">会诊申请单号</param>
        /// <returns>会诊单状态</returns>
        private Enum.ConsultStatus GetConsultationState(string consultApplySN)
        {
            try
            {
                DataTable dt = GetConsultationBySN(consultApplySN);
                if (dt.Rows.Count > 0)
                {
                    return (Enum.ConsultStatus)System.Enum.Parse(typeof(Enum.ConsultStatus), dt.Rows[0]["STATEID"].ToString());
                }
                throw new Exception("会诊单：" + consultApplySN + " 已经作废！");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理会诊申请记录保存 只有申请人可见，双击后弹出会诊申请的界面，可以重新修改原来的已保存的记录，然后保存或提交。
        /// Edit by xlb 2013-03-01
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="consultApplySN"></param>
        /// <returns></returns>
        private bool ProcessApplySave(string currentUserID, string consultApplySN)
        {
            try
            {
                DataTable dt = GetConsultationBySN(consultApplySN);

                //验证会诊申请人是否是当前系统登录人
                if (dt==null||dt.Rows.Count <= 0)
                {
                    return false;//返回FALSE则表示打开失败
                }
                if (dt.Rows[0]["APPLYUSER"].ToString().Equals(currentUserID))//登录人是申请人，可以编辑
                {
                    //TODO 调用可以编辑的会诊申请界面
                    InitFormConsultApply(consultApplySN, false);
                }
                else//登录人不是申请人，只能查看
                {
                    //TODO 调用只能查看的会诊申请界面 
                    InitFormConsultApply(consultApplySN, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打开会诊申请窗体
        /// Add xlb 2013-03-01
        /// </summary>
        private bool InitFormConsultApply(string consultApplySn,bool readOnly/*是否只读*/)
        {
            try
            {
                FormApplyForMultiply formapply = new FormApplyForMultiply(noofinpat, m_app, consultApplySn, readOnly/*只读*/);
                if (formapply == null)
                {
                    return false;
                }
                formapply.StartPosition = FormStartPosition.CenterScreen;
                formapply.ShowDialog();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理待审核的会诊记录  审核人对已经提交的记录进行审核操作，审核前需要在数据库中确认一下记录情况，防止审核前申请记录已经被申请人撤销。 其他人只能查看
        /// Edit by xlb 2013-03-01
        /// </summary>
        /// <param name="consultApplySN"></param>
        /// <returns></returns>
        private bool ProcessWaitApprove(string currentUserID, string consultApplySN)
        {
            try
            {
                DataTable dt = GetConsultationBySN(consultApplySN);//审核人ID 或级别 至少一个不为空
                //验证会诊审核人是否是当前系统登录人
                if (dt.Rows.Count > 0)
                {
                    string auditUserID = dt.Rows[0]["AUDITUSERID"].ToString().Trim();//会诊审核人ID
                    string auditLevel = dt.Rows[0]["AUDITLEVEL"].ToString().Trim();//会诊审核人级别
                    string applyUser = dt.Rows[0]["APPLYUSER"].ToString().Trim();//申请人ID
                    if (auditUserID.Length > 0 && auditUserID == currentUserID)//会诊审核人是当前系统登录人
                    {
                        //TODO 调用会诊审核界面
                        InitConsultReview(consultApplySN, false);
                    }
                    else if (auditUserID.Length == 0 && auditLevel.Length > 0)
                    {
                        Employee emp = new Employee(currentUserID);
                        emp.ReInitializeProperties();
                        if (emp.Grade == auditLevel)
                        {
                            //TODO 调用会诊审核界面
                            InitConsultReview(consultApplySN, false);
                        }
                        else if (applyUser == currentUserID)
                        {
                            InitConsultReview(consultApplySN, true);
                        }
                    }
                    else
                    {
                        //TODO 调用只能查看的会诊审核界面
                        //会诊审核意见区域不可编辑
                        InitConsultReview(consultApplySN, true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打开会诊审核窗体
        /// Add xlb 2013-03-01
        /// </summary>
        /// <param name="consultApplySn">会诊申请单号</param>
        /// <param name="readOnly">会诊审核意见是否只读</param>
        /// <returns></returns>
        private bool InitConsultReview(string consultApplySn,bool readOnly)
        {
            try
            {
                FrmConsultForReview frmReview = new FrmConsultForReview(noofinpat, m_app, consultApplySn, readOnly);
                if (frmReview == null)
                {
                    return false;
                }
                //窗体居中
                frmReview.StartPosition = FormStartPosition.CenterParent;
                frmReview.ShowDialog();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理待会诊的记录 和 会诊记录单保存的记录
        /// 根据配置来决定会诊记录单由谁来填写
        /// 会诊类型是 单科会诊 且由申请医师填写会诊记录，则只有申请医师可以编辑会诊记录界面，其他人只能查看不能编辑
        /// 会诊类型是 单科会诊 且由受邀医师填写会诊记录，则只有受邀医师可以编辑会诊记录界面，其他人只能查看不能编辑
        /// 会诊类型是 多科会诊 且由申请医师填写会诊记录，则只有申请医师可以编辑会诊记录界面，其他人只能查看不能编辑
        /// 会诊类型是 多科会诊 且由受邀医师填写会诊记录，则受邀医师只能编辑自己的会诊意见，同时可以查看其他受邀医师的会诊意见，但是不能编辑会诊记录界面
        ///                                            则申请医师只能查看其他受邀医师的会诊意见，但是可以编辑会诊记录界面，对受邀医师的会诊意见进行汇总
        /// </summary>
        /// <param name="consultApplySN"></param>
        /// <returns></returns>
        private bool ProcessWaitConsultationOrRecordeSave(string currentUserID, string consultApplySN)
        {
            try
            {
                DataTable dtConsultation = GetConsultationBySN(consultApplySN);
                if (dtConsultation.Rows.Count > 0)
                {
                    string configValue = GetConfigValueByKey("ConsultSuggestionWrite");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(configValue);
                    XmlNode nodeSingleNode = doc.SelectSingleNode("/root/SingleConsultation");//单个受邀医师 1：申请医师填写会诊意见 2：受邀医师填写会诊意见
                    XmlNode nodeMultipleNode = doc.SelectSingleNode("/root/MultipleConsultation");//多个受邀医师 1：申请医师填写会诊意见 2：受邀医师填写会诊意见

                    string applyUserID = dtConsultation.Rows[0]["APPLYUSER"].ToString();//会诊申请医师
                    DataTable dtConsultationDepartment = GetConsultationDepartmentBySN(consultApplySN);
                    if (dtConsultationDepartment.Rows.Count > 1)//多个受邀医师
                    {
                        if (nodeMultipleNode.InnerText.Trim() == "1")//1：申请医师填写会诊意见
                        {
                            //只有申请医师可以编辑会诊记录界面，其他人只能查看不能编辑
                            if (applyUserID == currentUserID)
                            {
                                //TODO 调用会诊记录界面，可以编辑
                                InitFrmConsultRecord(consultApplySN,false,ConsultRecordForWrite.MultiApply,0);
                            }
                            else
                            {
                                //TODO 调用会诊记录界面，只能查看
                                InitFrmConsultRecord(consultApplySN,true,ConsultRecordForWrite.MultiApply,2);
                            }
                        }
                        else//2：受邀医师填写会诊意见
                        {
                            //只有受邀医师和申请医师可以编辑会诊记录界面，其他人只能查看不能编辑
                            var employeeList = from DataRow dr in dtConsultationDepartment.Rows
                                               where dr["EMPLOYEECODE"].ToString().Equals(currentUserID)
                                               select dr["EMPLOYEECODE"].ToString();
                            if (employeeList.Count() > 0)
                            {
                                //TODO 调用会诊记录界面 受邀医师只能编辑自己的会诊意见，同时可以查看其他受邀医师的会诊意见，但是不能编辑会诊记录界面
                                InitFrmConsultRecord(consultApplySN,true,ConsultRecordForWrite.MultiEmployee,1);
                            }
                            else if (applyUserID == currentUserID)
                            {
                                //TODO 调用会诊记录界面 申请医师只能查看其他受邀医师的会诊意见，但是可以编辑会诊记录界面，对受邀医师的会诊意见进行汇总
                                InitFrmConsultRecord(consultApplySN, false, ConsultRecordForWrite.MultiEmployee, 0);
                            }
                            else
                            {
                                //TODO 调用会诊记录界面 只能查看
                                InitFrmConsultRecord(consultApplySN, true, ConsultRecordForWrite.MultiEmployee, 2);
                            }
                        }
                    }
                    else if (dtConsultationDepartment.Rows.Count > 0)//单个受邀医师
                    {
                        if (nodeSingleNode.InnerText.Trim() == "1")//申请医师填写会诊意见
                        {
                            //只有申请医师可以编辑会诊记录界面，其他人只能查看不能编辑
                            if (applyUserID == currentUserID)
                            {
                                //TODO 调用会诊记录界面，可以编辑
                                InitFrmConsultRecord(consultApplySN, false, ConsultRecordForWrite.SingleApply, 0);
                            }
                            else
                            {
                                //TODO 调用会诊记录界面，只能查看
                                InitFrmConsultRecord(consultApplySN, true, ConsultRecordForWrite.SingleApply, 2);
                            }
                        }
                        else//受邀医师填写会诊意见
                        {
                            //只有受邀医师可以编辑会诊记录界面，其他人只能查看不能编辑
                            var employeeList = from DataRow dr in dtConsultationDepartment.Rows
                                               where dr["EMPLOYEECODE"].ToString().Equals(currentUserID)
                                               select dr["EMPLOYEECODE"].ToString();
                            if (employeeList.Count() > 0)
                            {
                                //TODO 调用会诊记录界面，可以编辑
                                InitFrmConsultRecord(consultApplySN, false, ConsultRecordForWrite.SingleEmployee, 1);
                            }
                            else
                            {
                                //TODO 调用会诊记录界面，只能查看
                                InitFrmConsultRecord(consultApplySN,true,ConsultRecordForWrite.SingleEmployee,2);
                            }
                        }
                    }
                    else//没有受邀医师
                    { }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打开会诊记录界面
        /// Add xlb 2013-03-05
        /// </summary>
        private bool InitFrmConsultRecord(string consultApplySn,bool readOnly,ConsultRecordForWrite consultForWrite,int level)
        {
            try
            {
                FrmRecordConsult frmRecord = new FrmRecordConsult(noofinpat, consultApplySn, m_app, readOnly,consultForWrite,level);
                if (frmRecord == null)
                {
                    return false;
                }
                frmRecord.ShowDialog();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理会诊记录完成的记录
        /// edit by xlb 2013-03-15
        /// </summary>
        /// <param name="consultApplySN"></param>
        /// <returns></returns>
        private bool ProcessRecordeComplete(string currentUserID, string consultApplySN)
        {
            try
            {
                DataTable dtConsultation = GetConsultationBySN(consultApplySN);
                if (dtConsultation.Rows.Count > 0)
                {
                    string configValue = GetConfigValueByKey("ConsultSuggestionWrite");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(configValue);
                    XmlNode nodeSingleNode = doc.SelectSingleNode("/root/SingleConsultation");//单个受邀医师 1：申请医师填写会诊意见 2：受邀医师填写会诊意见
                    XmlNode nodeMultipleNode = doc.SelectSingleNode("/root/MultipleConsultation");//多个受邀医师 1：申请医师填写会诊意见 2：受邀医师填写会诊意见

                    string applyUserID = dtConsultation.Rows[0]["APPLYUSER"].ToString();//会诊申请医师
                    DataTable dtConsultationDepartment = GetConsultationDepartmentBySN(consultApplySN);
                    //会诊完成后是否开发再次编辑
                    string openWrite = GetConfigValueByKey("SetConsultCompeleteEdit").Trim();
                    if (dtConsultationDepartment.Rows.Count > 1)//多个受邀医师
                    {
                        if (nodeMultipleNode.InnerText.Trim() == "1")//1：申请医师填写会诊意见
                        {
                            //只有申请医师可以编辑会诊记录界面，其他人只能查看不能编辑
                            if (applyUserID == currentUserID)
                            {
                                if (openWrite == "1")//开放编辑则申请人可以再次编辑
                                {
                                    //TODO 调用会诊记录界面，可以编辑
                                    InitFrmConsultRecord(consultApplySN, false, ConsultRecordForWrite.MultiApply, 0);
                                }
                                else
                                {
                                    InitFrmConsultRecord(consultApplySN, true, ConsultRecordForWrite.MultiApply,0);
                                }
                            }
                            else
                            {
                                //TODO 调用会诊记录界面，只能查看
                                InitFrmConsultRecord(consultApplySN, true, ConsultRecordForWrite.MultiApply, 2);
                            }
                        }
                        else
                        {
                            if (applyUserID == currentUserID)
                            {
                                if (openWrite == "1")
                                {
                                    InitFrmConsultRecord(consultApplySN, false, ConsultRecordForWrite.MultiEmployee, 0);
                                }
                                else
                                {
                                    InitFrmConsultRecord(consultApplySN, true, ConsultRecordForWrite.MultiEmployee, 2);
                                }
                            }
                            else
                            {
                                InitFrmConsultRecord(consultApplySN, true, ConsultRecordForWrite.MultiEmployee,2);
                            }
                        }
                    }
                    else if (dtConsultationDepartment.Rows.Count > 0)//单个受邀医师
                    {
                        if (nodeSingleNode.InnerText.Trim() == "1")//申请医师填写会诊意见
                        {
                            //只有申请医师可以编辑会诊记录界面，其他人只能查看不能编辑
                            if (applyUserID == currentUserID&&openWrite=="1")
                            {
                                //TODO 调用会诊记录界面，可以编辑
                                InitFrmConsultRecord(consultApplySN, false, ConsultRecordForWrite.SingleApply, 0);
                            }
                            else
                            {
                                //TODO 调用会诊记录界面，只能查看
                                InitFrmConsultRecord(consultApplySN, true, ConsultRecordForWrite.SingleApply, 2);
                            }
                        }
                        else//受邀医师填写会诊意见
                        {
                            //只有受邀医师可以编辑会诊记录界面，其他人只能查看不能编辑
                            var employeeList = from DataRow dr in dtConsultationDepartment.Rows
                                               where dr["EMPLOYEECODE"].ToString().Equals(currentUserID)
                                               select dr["EMPLOYEECODE"].ToString();
                            if (employeeList.Count() > 0&&openWrite=="1")
                            {
                                //TODO 调用会诊记录界面，可以编辑
                                InitFrmConsultRecord(consultApplySN, false, ConsultRecordForWrite.SingleEmployee, 1);
                            }
                            else
                            {
                                //TODO 调用会诊记录界面，只能查看
                                InitFrmConsultRecord(consultApplySN, true, ConsultRecordForWrite.SingleEmployee, 2);
                            }
                        }
                    }
                    else//没有受邀医师
                    {
                        InitFrmConsultRecord(consultApplySN, true, ConsultRecordForWrite.SingleEmployee, 2);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理被否决的会诊记录
        /// </summary>
        /// <param name="consultApplySN"></param>
        /// <returns></returns>
        private bool ProcessReject(string currentUserID, string consultApplySN)
        {
            try
            {
                DataTable dt = GetConsultationBySN(consultApplySN);

                //验证会诊申请人是否是当前系统登录人
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["APPLYUSER"].ToString().Equals(currentUserID))//登录人是申请人，可以编辑
                    {
                        //TODO 调用可以编辑的会诊申请界面，同时需要显示原来审核时被否决的原因
                        InitFormConsultApply(consultApplySN,false);
                    }
                    else//登录人不是申请人，只能查看
                    {
                        //TODO 调用只能查看的会诊申请界面，同时需要显示原来审核时被否决的原因
                        InitFormConsultApply(consultApplySN, true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理被取消的会诊记录
        /// </summary>
        /// <param name="consultApplySN"></param>
        /// <returns></returns>
        private bool ProcessCancelConsultion(string currentUserID, string consultApplySN)
        {
            try
            {
                //暂不处理
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理已经撤销的会诊记录
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="consultApplySN"></param>
        /// <returns></returns>
        private bool ProcessCallBackConsultation(string currentUserID, string consultApplySN)
        {
            try
            {
                ProcessApplySave(currentUserID, consultApplySN);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetConfigValueByKey(string key)
        {
            try
            {
                string sqlGetConfig = "select * from appcfg where configkey = '{0}'";
                DataTable dt = DS_SqlHelper.ExecuteDataTable(string.Format(sqlGetConfig, key), CommandType.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["value"].ToString();
                }
                throw new Exception("配置项：" + key + " 不存在！");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
