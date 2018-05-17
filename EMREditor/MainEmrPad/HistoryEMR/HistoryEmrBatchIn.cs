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
using DrectSoft.Service;
using DrectSoft.Common;
using DrectSoft.DSSqlHelper;

namespace DrectSoft.Core.MainEmrPad.HistoryEMR
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
            try
            {
                m_NoOfInpat = noOfInpat;
                m_App = app;
                m_RecordDal = recordDal;
                m_CurrentInpatient = currentInpatient;
                m_patUtil = patUtil;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 批量导入病例
        /// </summary>
        public bool ExecuteBatchIn()
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("正在处理病历...", "请稍等");
                bool boo = BeforeBatchInCheck();
                if (!boo)
                {//检查不通过
                    return false;
                }
                BeforeProcEMR();
                //edit by cyq 2013-03-01
                ProcEMRNew(GetPatAllEmr());
                //ProcEMR(GetPatAllEmr());
                DS_Common.HideWaitDialog(m_WaitDialog);
                return true;
            }
            catch (Exception ex)
            {
                DS_Common.HideWaitDialog(m_WaitDialog);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 导入前的检查操作
        /// </summary>
        /// 是否可导入历史病历
        private bool BeforeBatchInCheck()
        {
            try
            {
                bool boo = false;
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在检查病历...");
                string sqlGetDailyEmrCount = "SELECT COUNT (1) FROM recorddetail WHERE noofinpat = '{0}' AND sortid = 'AC' AND valid = '1'";
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(string.Format(sqlGetDailyEmrCount, m_CurrentInpatient.NoOfFirstPage.ToString()), CommandType.Text);
                string cnt = dt.Rows[0][0].ToString();
                if (cnt.Equals("0"))
                {//无病历
                    return true;
                }

                string question = "此病人已存在病程 \n\r 选择【是】则删除现有病程，并导入历史病程记录； \n\r 选择【否】则保留现有病程，不导入历史病程记录。";
                if (m_App.CustomMessageBox.MessageShow(question, DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    m_IsClearAllDailyEmr = true;
                    m_IsNeedBatchInDailyEmr = true;
                    boo = true;
                }
                else
                {
                    m_IsClearAllDailyEmr = false;
                    m_IsNeedBatchInDailyEmr = false;
                    boo = false;
                }
                DS_Common.HideWaitDialog(m_WaitDialog);
                return boo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 导入前的处理
        /// </summary>
        private void BeforeProcEMR()
        {
            try
            {
                if (m_IsClearAllDailyEmr)
                {
                    DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在删除现有病程...");
                    string sqlClearDailyEMR = "update recorddetail set valid = '0' where noofinpat = '{0}' and sortid = 'AC' and valid = '1' ";
                    m_App.SqlHelper.ExecuteNoneQuery(string.Format(sqlClearDailyEMR, m_CurrentInpatient.NoOfFirstPage.ToString()), CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到病人某一次就诊的所有病例
        /// </summary>
        /// <returns></returns>
        private DataTable GetPatAllEmr()
        {
            try
            {
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在捞取历史病历...");
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        string GetDataByNameForMacro(string name)
        {
            try
            {
                //此处应该写具体的实现方法
                return MacroUtil.FillMarcValue(m_CurrentInpatient.NoOfFirstPage.ToString(), name, m_App.User.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 导入处理过的病历
        /// </summary>
        private void ProcEMR(DataTable dt)
        {
            //增加捕获异常验证 add by ywk 2012年12月10日16:22:49 
            try
            {
                DS_Common.SetWaitDialogCaption(m_WaitDialog,"正在处理历史病历...");
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
                DS_Common.SetWaitDialogCaption(m_WaitDialog,"正在保存病历...");
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                UCEmrInput UcEmr = new UCEmrInput();
                List<EmrModel> listmodel = new List<EmrModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["sortid"].ToString() == ContainerCatalog.BingChengJiLu && !m_IsNeedBatchInDailyEmr)//如果是病程记录，并且不需要导入病程，则暂时不做任何动作
                    {
                        //TODO
                    }
                    else
                    {
                        EmrModel model = new EmrModel(dr);
                        //如果不为空，则遍历到病程记录时，就代表对首程进行操作时 ,遍历其他病程记录时不会进这个方法 
                        //edit by ywk 2012年12月10日15:33:59
                        listmodel.Add(model);
                        if (dr["content"].ToString().Trim() != "")
                        {
                            //XmlDocument doc = new XmlDocument();
                            //doc.PreserveWhitespace = true;
                            doc.LoadXml(dr["content"].ToString());
                            model.ModelContent = doc;
                        }
                        #region 注释掉的
                        //XmlDocument doc = new XmlDocument();
                        //doc.PreserveWhitespace = true;
                        ////改为病程特殊处理
                        //if (dr["content"].ToString().Trim() != "" && dr["sortid"].ToString() != "AC")
                        //{

                        //    doc.LoadXml(dr["content"].ToString());
                        //    model.ModelContent = doc;
                        //}
                        //if (dr["sortid"].ToString() == "AC")
                        //{
                        //    if (dr["FIRSTDAILYFLAG"].ToString() == "1")//是首程
                        //    {
                        //        doc.LoadXml(dr["content"].ToString());
                        //        model.ModelContent = doc;
                        //    }
                        //    else
                        //    {

                        //    }
                        //}
                        #endregion
                        m_RecordDal.InsertModelInstance(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                    }
                }
                UcEmr.SaveDocumentByModelList(listmodel, m_RecordDal, m_CurrentInpatient);
                #region 注释掉的
                //HistoryEMRBLL bll = new HistoryEMRBLL(m_App, m_CurrentInpatient, m_RecordDal, "AC");
                ////处理有的病程记录有节点，右侧没内容的情况   add by ywk 2012年12月10日16:45:07 
                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (dr["sortid"].ToString() == "AC")
                //    {
                //        if (!bll.CheckExistRecordDetail(Int32.Parse(dr["ID"].ToString()), dr["captiondatetime"].ToString()))
                //        {
                //            //YD_SqlService.UpdateRecordValid(Int32.Parse(dr["ID"].ToString()), false);
                         
                //            bll.UpdateRecordValid(Int32.Parse(dr["ID"].ToString()), false);
                //        }
                //    }

                //}
                #endregion 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 导入处理过的病历---优化(转科)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-20</date>
        /// <param name="dt"></param>
        private void ProcEMRNew(DataTable dt)
        {
            try
            {
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在处理历史病历...");
                string userID = m_App.User.Id;
                string userName = m_App.User.Name;
                int day = 0;

                List<DataRow> allRecords = new List<DataRow>();
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
                    dr["ip"] = DS_Common.GetIPHost();
                    dr["departcode"] = DS_Common.currentUser.CurrentDeptId;
                    dr["wardcode"] = DS_Common.currentUser.CurrentWardId;

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
                        dr["captiondatetime"] = CurrentDateTime;
                    }
                    allRecords.Add(dr);
                }
                //【2】处理病历记录和首程内容
                ///获取病例中所有首次病程
                var firstRecords = allRecords.Where(p => p["sortid"].ToString() == ContainerCatalog.BingChengJiLu && p["FIRSTDAILYFLAG"].ToString() == "1");
                ///获取所有病历内容集合(包含header和footer,包含所有首次病程中的病历)
                List<string[]> allRecordContents = new List<string[]>();
                if (null != firstRecords && firstRecords.Count() > 0)
                {
                    for (int i = 0; i < firstRecords.Count();i++)
                    {
                        DataRow firstRecord = firstRecords.ElementAt(i);
                        XmlDocument xmlRecord = NewXmlDocument(firstRecord["content"].ToString());
                        if (null != allRecordContents && allRecordContents.Count() > 0)
                        {
                            List<string[]> thisRecordContents = DS_BaseService.GetRecoedsByXML(xmlRecord);
                            if (null != thisRecordContents && thisRecordContents.Count() > 0)
                            {
                                var onlyRecordContents = thisRecordContents.Where(p => p[0] != "header" && p[0] != "footer");
                                allRecordContents = allRecordContents.AsEnumerable().Union(onlyRecordContents).ToList();
                            }
                        }
                        else
                        {
                            allRecordContents = DS_BaseService.GetRecoedsByXML(xmlRecord);
                        }
                    }
                    ///将首次病程设为普通病历(非首次病程)
                    firstRecords = SetFirstDailyToDaily(firstRecords);
                }
                ///获取所有病历内容对应的时间集合
                List<string> xmlRecordTimes = allRecordContents.Select(p => p[0]).ToList();
                ///去除病例中无内容的节点
                allRecords = allRecords.Where(p => (p["sortid"].ToString() != ContainerCatalog.BingChengJiLu && !string.IsNullOrEmpty(p["content"].ToString())) || (p["sortid"].ToString() == ContainerCatalog.BingChengJiLu && xmlRecordTimes.Contains(p["captiondatetime"].ToString()))).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                ///首次病程内容
                string newEMRContent = string.Empty;
                ///获取过滤后的病程记录
                var emrRecords = allRecords.Where(p => p["sortid"].ToString() == ContainerCatalog.BingChengJiLu).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString()));
                ///需要过滤(去除)的病历时间
                List<string> toDeleteTime = new List<string>();
                foreach (DataRow emr in emrRecords)
                {
                    string[] contentArray = allRecordContents.FirstOrDefault(p => p[0] == emr["captiondatetime"].ToString());
                    if (null == contentArray || string.IsNullOrEmpty(contentArray[1].Trim()))
                    {
                        toDeleteTime.Add(emr["captiondatetime"].ToString());
                        continue;
                    }
                    string newContent = SetCaptionTime(emr["captiondatetime"].ToString(), emr["captiondatetimeTemp"].ToString(), contentArray[1]);
                    if (!string.IsNullOrEmpty(newContent.Trim()))
                    {
                        newEMRContent += newContent;
                        emr["captiondatetime"] = emr["captiondatetimeTemp"];
                    }
                    else
                    {
                        toDeleteTime.Add(emr["captiondatetime"].ToString());
                    }
                }
                if (null != toDeleteTime && toDeleteTime.Count > 0)
                {
                    allRecords = allRecords.Where(p => !toDeleteTime.Contains(p["captiondatetime"].ToString())).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                }
                if (null != allRecordContents && allRecordContents.Count() > 0)
                {
                    string[] header = allRecordContents.FirstOrDefault(p => p[0] == "header");
                    if (null != header && !string.IsNullOrEmpty(header[1]))
                    {
                        newEMRContent = header[1] + newEMRContent;
                    }
                    string[] footer = allRecordContents.FirstOrDefault(p => p[0] == "footer");
                    if (null != footer && !string.IsNullOrEmpty(footer[1]))
                    {
                        newEMRContent += footer[1];
                    }
                }
                if (null != emrRecords && emrRecords.Count() > 0)
                {
                    DataRow firstEmr = emrRecords.FirstOrDefault();
                    if (null != firstEmr)
                    {
                        firstEmr["FIRSTDAILYFLAG"] = "1";
                        firstEmr["content"] = newEMRContent;
                    }
                }

                //【3】保存到数据库
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在保存病历...");
                SaveRecordsInTran(allRecords);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病历中标题时间
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-21</date>
        /// <param name="oldcaptiontime"></param>
        /// <param name="newcaptiontime"></param>
        /// <param name="emrContent"></param>
        /// <returns></returns>
        private string SetCaptionTime(string oldcaptiontime, string newcaptiontime, string emrContent)
        {
            try
            {
                if (string.IsNullOrEmpty(oldcaptiontime) || string.IsNullOrEmpty(newcaptiontime) || string.IsNullOrEmpty(emrContent))
                {
                    return string.Empty;
                }
                //添加父节点以便于xml读取
                emrContent = "<p>" + emrContent + "</p>";
                XmlDocument xmlRecord = new XmlDocument();
                xmlRecord.PreserveWhitespace = true;
                xmlRecord.LoadXml(emrContent);
                XmlNodeList nodeList = xmlRecord.GetElementsByTagName("text");
                foreach (XmlElement ele in nodeList)
                {
                    if (ele.HasAttribute("type") && ele.HasAttribute("name"))
                    {
                        if (ele.Attributes["type"].Value == "text" && ele.Attributes["name"].Value == "记录日期")
                        {
                            if (ele.InnerText.Trim() == oldcaptiontime)
                            {
                                ele.InnerXml = newcaptiontime;
                                return null == xmlRecord.FirstChild ? string.Empty : xmlRecord.FirstChild.InnerXml;
                            }
                        }
                    }
                }
                return string.Empty; ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存所有病历
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-21</date>
        /// <param name="list"></param>
        private void SaveRecordsInTran(List<DataRow> list)
        {
            try
            {
                if (null == list || list.Count() == 0)
                {
                    return;
                }
                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.BeginTransaction();
                foreach (DataRow dr in list)
                {
                    if (dr["sortid"].ToString() == ContainerCatalog.BingChengJiLu && !m_IsNeedBatchInDailyEmr)//如果是病程记录，并且不需要导入病程，则暂时不做任何动作
                    {
                        //TODO
                    }
                    else
                    {
                        EmrModel model = new EmrModel(dr);
                        if (null != dr["content"] && !string.IsNullOrEmpty(dr["content"].ToString().Trim()))
                        {
                            model.ModelContent = NewXmlDocument(dr["content"].ToString());
                        }
                        m_RecordDal.InsertModelInstanceInTran(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                    }
                }
                DS_SqlHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                DS_SqlHelper.AppDbTransaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 新建一个XmlDocument
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-21</date>
        /// <param name="content">XML内容</param>
        /// <returns></returns>
        private XmlDocument NewXmlDocument(string content)
        {
            try
            {
                if (string.IsNullOrEmpty(content))
                {
                    return new XmlDocument();
                }
                XmlDocument xmlRecord = new XmlDocument();
                xmlRecord.PreserveWhitespace = true;
                xmlRecord.LoadXml(content);

                return xmlRecord;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将首次病程设置为普通比例节点
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-21</date>
        /// <param name="firstEmrs">首次病程</param>
        /// <returns></returns>
        private IEnumerable<DataRow> SetFirstDailyToDaily(IEnumerable<DataRow> firstEmrs)
        {
            try
            {
                if (null == firstEmrs || firstEmrs.Count() == 0)
                {
                    return null;
                }
                foreach (DataRow drow in firstEmrs)
                {
                    if (drow.Table.Columns.Contains("FIRSTDAILYFLAG"))
                    {
                        drow["FIRSTDAILYFLAG"] = "0";
                    }
                    if (drow.Table.Columns.Contains("content"))
                    {
                        drow["content"] = string.Empty;
                    }
                }
                return firstEmrs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

        #region public methods of WaitDialog 已注销 by xlb 2013-01-04
        //public void SetWaitDialogCaption(string caption)
        //{
        //    if (m_WaitDialog != null)
        //    {
        //        if (!m_WaitDialog.Visible)
        //            m_WaitDialog.Visible = true;
        //        m_WaitDialog.Caption = caption;
        //    }

        //}

        //public void HideWaitDialog()
        //{
        //    if (m_WaitDialog != null)
        //        m_WaitDialog.Hide();
        //}
        #endregion
    }
}
