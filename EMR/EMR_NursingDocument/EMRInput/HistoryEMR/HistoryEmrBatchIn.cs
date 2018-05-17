using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data;
using DrectSoft.Emr.Util;
using System.Xml;
using DrectSoft.Common.Eop;
using DevExpress.Utils;
using DrectSoft.Core.EMR_NursingDocument.EMRInput.Table;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.HistoryEMR
{
    /// <summary>
    /// 批量导入病人某一次就诊的所有病例
    /// </summary>
    class HistoryEmrBatchIn
    {
        /// <summary>
        /// 历史住院时的首页序号
        /// </summary>
        private string m_NoOfInpat;

        /// <summary>
        /// 当次就诊的病人信息
        /// </summary>
        private Inpatient m_CurrentInpatient;

        private IEmrHost m_App;

        private RecordDal m_RecordDal;

        private PatRecUtil m_patUtil;

        private WaitDialogForm m_WaitDialog;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="noOfInpat"></param>
        public HistoryEmrBatchIn(string noOfInpat, Inpatient currentInpatient, IEmrHost app, RecordDal recordDal, PatRecUtil patUtil)
        {
            m_NoOfInpat = noOfInpat;
            m_App = app;
            m_RecordDal = recordDal;
            m_CurrentInpatient = currentInpatient;
            m_patUtil = patUtil;
        }

        /// <summary>
        /// 批量导入病例
        /// </summary>
        public void ExecuteBatchIn()
        {
            m_WaitDialog = new WaitDialogForm("正在处理病历……", "请稍等");
            BeforeBatchInCheck();
            BeforeProcEMR();
            ProcEMR(GetPatAllEmr());
            HideWaitDialog();
        }

        /// <summary>
        /// 导入前的检查操作
        /// </summary>
        private void BeforeBatchInCheck()
        {
            SetWaitDialogCaption("正在检查病历……");
            string sqlGetDailyEmrCount = "SELECT COUNT (1) FROM recorddetail WHERE noofinpat = '{0}' AND sortid = 'AC' AND valid = '1'";
            DataTable dt = m_App.SqlHelper.ExecuteDataTable(string.Format(sqlGetDailyEmrCount, m_CurrentInpatient.NoOfFirstPage.ToString()), CommandType.Text);
            string cnt = dt.Rows[0][0].ToString();
            if (!cnt.Equals("0"))
            {
                string question = "此病人已存在病程 \n\r 选择【是】则删除现有病程，并导入历史病程记录 \n\r 选择【否】则保留现有病程，不导入历史病程记录";
                if (m_App.CustomMessageBox.MessageShow(question, DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    m_IsClearAllDailyEmr = true;
                    m_IsNeedBatchInDailyEmr = true;
                }
                else
                {
                    m_IsClearAllDailyEmr = false;
                    m_IsNeedBatchInDailyEmr = false;
                }
            }
        }

        /// <summary>
        /// 导入前的处理
        /// </summary>
        private void BeforeProcEMR()
        {
            if (m_IsClearAllDailyEmr)
            {
                SetWaitDialogCaption("正在删除现有病程……");
                string sqlClearDailyEMR = "update recorddetail set valid = '0' where noofinpat = '{0}' and sortid = 'AC' and valid = '1' ";
                m_App.SqlHelper.ExecuteNoneQuery(string.Format(sqlClearDailyEMR, m_CurrentInpatient.NoOfFirstPage.ToString()), CommandType.Text);
            }
        }

        /// <summary>
        /// 得到病人某一次就诊的所有病例
        /// </summary>
        /// <returns></returns>
        private DataTable GetPatAllEmr()
        {
            SetWaitDialogCaption("正在捞取历史病历……");
            //排除护理病历
            string sqlAllPatEMR = " select * from recorddetail where noofinpat = '{0}' and valid = '1' and sortid not in('AI','AJ','AK') order by sortid, captiondatetime ";
            DataTable dt = m_App.SqlHelper.ExecuteDataTable(string.Format(sqlAllPatEMR, m_NoOfInpat), CommandType.Text);

            //Add by wwj 2012-08-10 对病历内容中的宏元素重新赋值
            foreach (DataRow dr in dt.Rows)
            {
                string content = dr["content"].ToString();
                if (content.Trim() != "")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.PreserveWhitespace = true;
                    doc.LoadXml(content);
                    XmlNodeList nodeList = doc.GetElementsByTagName("macro");
                    foreach (XmlNode node in nodeList)
                    {
                        string macroName = node.Attributes["name"].Value;
                        string macroValue = GetDataByNameForMacro(macroName);
                        node.InnerText = macroValue;
                    }
                    dr["content"] = doc.InnerXml;
                }
            }

            return dt;
        }

        string GetDataByNameForMacro(string name)
        {
            //此处应该写具体的实现方法
            return MacroUtil.FillMarcValue(m_CurrentInpatient.NoOfFirstPage.ToString(), name, m_App.User.Id);
        }

        /// <summary>
        /// 导入处理过的病历
        /// </summary>
        private void ProcEMR(DataTable dt)
        {
            SetWaitDialogCaption("正在处理历史病历……");
            string userID = m_App.User.Id;
            string userName = m_App.User.Name;
            int day = 0;
            //【1】初始化病例信息
            dt.Columns.Add("captiondatetimeTemp");
            foreach (DataRow dr in dt.Rows)
            {
                dr["noofinpat"] = m_CurrentInpatient.NoOfFirstPage.ToString();
                dr["owner"] = userID;
                dr["createtime"] = CurrentDateTime;
                dr["auditor"] = "";
                dr["audittime"] = null;
                dr["hassubmit"] = "4600";
                dr["hasprint"] = "3600";
                dr["hassign"] = "0";
                dr["islock"] = "4700";

                if (dr["sortid"].ToString() == ContainerCatalog.BingChengJiLu)//针对病程需要特殊处理，每份病例的时间间距是1天
                {
                    string bingChenDateTime = Convert.ToDateTime(CurrentDateTime).AddDays(day).ToString("yyyy-MM-dd HH:mm:ss");
                    day++;
                    dr["name"] = dr["name"].ToString().Split(' ')[0] + ' ' + bingChenDateTime + ' ' + userName;
                    dr["captiondatetimeTemp"] = bingChenDateTime;
                }
                else
                {
                    dr["name"] = dr["name"].ToString().Split(' ')[0] + ' ' + CurrentDateTime + ' ' + userName;
                    dr["captiondatetime"] = System.DateTime.MinValue;
                }
            }
            //【2】修改病程病例中节点的时间
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["sortid"].ToString() == ContainerCatalog.BingChengJiLu)//针对病程需要特殊处理，每份病例的时间间距是1天
                {
                    if (dr["FIRSTDAILYFLAG"].ToString() == "1")//首次病程中保存了病程的内容
                    {
                        xmlDoc.LoadXml(dr["content"].ToString());
                    }

                    XmlNodeList nodeList = xmlDoc.GetElementsByTagName("text");
                    foreach (XmlElement ele in nodeList)
                    {
                        if (ele.HasAttribute("type") && ele.HasAttribute("name"))
                        {
                            if (ele.Attributes["type"].Value == "text" && ele.Attributes["name"].Value == "记录日期")
                            {
                                if (ele.InnerText.Trim() == dr["captiondatetime"].ToString())
                                {
                                    ele.InnerXml = dr["captiondatetimeTemp"].ToString();
                                    dr["captiondatetime"] = dr["captiondatetimeTemp"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //【3】修改病程保存到首程中
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["sortid"].ToString() == ContainerCatalog.BingChengJiLu)//针对病程需要特殊处理，每份病例的时间间距是1天
                {
                    if (dr["FIRSTDAILYFLAG"].ToString() == "1")//首次病程中保存了病程的内容
                    {
                        dr["content"] = xmlDoc.InnerXml;
                        break;
                    }
                }
            }

            //【4】保存到数据库
            SetWaitDialogCaption("正在保存病历……");
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["sortid"].ToString() == ContainerCatalog.BingChengJiLu && !m_IsNeedBatchInDailyEmr)//如果是病程记录，并且不需要导入病程，则暂时不做任何动作
                {
                    //TODO
                }
                else
                {
                    EmrModel model = new EmrModel(dr);
                    if (dr["content"].ToString().Trim() != "")
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.PreserveWhitespace = true;
                        doc.LoadXml(dr["content"].ToString());
                        model.ModelContent = doc;
                    }
                    m_RecordDal.InsertModelInstance(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                }
            }
        }

        /// <summary>
        /// 得到服务器的时间
        /// </summary>
        /// <returns></returns>
        private string CurrentDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(m_CurrentDateTime))
                {
                    m_CurrentDateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                return m_CurrentDateTime;
            }
        }
        private string m_CurrentDateTime;

        /// <summary>
        /// 保存历史病历前是否删除所有现有的病程
        /// </summary>
        bool m_IsClearAllDailyEmr = false;

        /// <summary>
        /// 是否需要批量导入病程记录
        /// </summary>
        bool m_IsNeedBatchInDailyEmr = true;


        #region public methods of WaitDialog
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
        #endregion
    }
}
