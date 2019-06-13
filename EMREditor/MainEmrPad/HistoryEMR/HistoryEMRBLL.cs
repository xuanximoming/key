using DrectSoft.Common.Eop;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;

namespace DrectSoft.Core.MainEmrPad
{
    /// <summary>
    /// 历史病历的相关业务逻辑
    /// </summary>
    public class HistoryEMRBLL
    {
        IEmrHost m_Host;
        Inpatient m_CurrentPatient;
        RecordDal m_RecordDal;
        string m_SortID;

        public HistoryEMRBLL(IEmrHost host, Inpatient currentPatient, RecordDal recordDal, string sortid)
        {
            try
            {
                m_Host = host;
                m_CurrentPatient = currentPatient;
                m_RecordDal = recordDal;
                m_SortID = sortid;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重载构造
        /// xlb 2013-01-14
        /// </summary>
        /// <param name="host"></param>
        /// <param name="currentPatient"></param>
        /// <param name="recordDal"></param>
        public HistoryEMRBLL(IEmrHost host, Inpatient currentPatient, RecordDal recordDal)
        {
            try
            {
                m_Host = host;
                m_CurrentPatient = currentPatient;
                m_RecordDal = recordDal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 得到病人历史记录
        /// xlb 2013-01-14
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetHistoryInpatient(int noofinpat, string dateBegin, string dateEnd)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("NOOFINPAT", SqlDbType.VarChar),
                    new SqlParameter("admitbegindate",SqlDbType.VarChar),
                    new SqlParameter("admitenddate",SqlDbType.VarChar)
                };
                sqlParams[0].Value = noofinpat;
                sqlParams[1].Value = dateBegin;
                sqlParams[2].Value = dateEnd;
                return m_Host.SqlHelper.ExecuteDataTable("emr_record_input.usp_getHistoryInpatient", sqlParams, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 处理病历的状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="boo"></param>
        public void UpdateRecordValid(int id, bool boo)
        {
            try
            {
                string sqlStr = string.Format(" update recorddetail set valid=0 where id='{0}' ", id);
                m_Host.SqlHelper.ExecuteNoneQuery(sqlStr, CommandType.Text);
                //string sqlStr = " update recorddetail set valid=@valid where id=@id ";
                //DbParameter[] sqlParams = new DbParameter[]
                //{
                //    new SqlParameter("@id",SqlDbType.Int),
                //    new SqlParameter("@valid",SqlDbType.Int)
                //};
                //sqlParams[0].Value = id;
                //sqlParams[1].Value = boo ? 1 : 0;
                ////m_
                //DS_SqlHelper.CreateSqlHelper();
                //DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetHistoryEmrRecord()
        {
            try
            {
                //TODO 测试
                DataTable dtInpatient = m_RecordDal.GetHistoryInpatient(Convert.ToInt32(m_CurrentPatient.NoOfFirstPage));
                DataTable allHistoryEMR = new DataTable();
                foreach (DataRow dr in dtInpatient.Rows)
                {
                    string noofinpat = dr["noofinpat"].ToString();
                    string admitdate = dr["admitdate"].ToString();
                    DataTable historyEMR = m_RecordDal.GetPatientHistoryEMR(Convert.ToInt32(noofinpat), m_SortID);

                    if (m_SortID == ContainerCatalog.BingChengJiLu)//由于病程记录都是保存在一块的，所以需要对病程进行拆分
                    {
                        Dictionary<string, string> emrDetail = new Dictionary<string, string>();

                        //找到首次病程，并对首次病程进行拆分
                        foreach (DataRow drRecorddetail in historyEMR.Rows)
                        {
                            if (drRecorddetail["name"].ToString().IndexOf("首次病程") >= 0 || drRecorddetail["isfirstdaily"].ToString() == "1")
                            {
                                string recorddetailid = drRecorddetail["id"].ToString();
                                string emrContent = m_RecordDal.GetEmrContentByID(recorddetailid);
                                emrDetail = ProcessXML(emrContent);
                                break;
                            }
                        }

                        //对病程依次进行赋值
                        foreach (DataRow drRecorddetail in historyEMR.Rows)
                        {
                            string captiondatetime = drRecorddetail["captiondatetime"].ToString();
                            drRecorddetail["content"] = emrDetail[captiondatetime];
                        }
                    }

                    if (historyEMR.Rows.Count > 0)
                    {
                        if (allHistoryEMR.Columns.Count == 0)
                        {
                            allHistoryEMR = historyEMR;
                        }
                        else
                        {
                            foreach (DataRow drHistoryEMR in historyEMR.Rows)
                            {
                                DataRow drNewHistoryEMR = allHistoryEMR.NewRow();
                                drNewHistoryEMR.ItemArray = drHistoryEMR.ItemArray;
                                allHistoryEMR.Rows.Add(drNewHistoryEMR);
                            }
                        }
                    }
                }
                return allHistoryEMR;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 对首次病程进行拆分,返回拆分后的Dictionary
        /// </summary>
        /// <param name="emrContent"></param>
        /// <returns></returns>
        private Dictionary<string, string> ProcessXML(string emrContent)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(emrContent);

                //找到所有的病程的起始段落
                XmlNodeList nodeList = xmlDoc.GetElementsByTagName("text");
                List<XmlNode> pList = new List<XmlNode>();
                foreach (XmlElement ele in nodeList)
                {
                    if (ele.HasAttribute("type") && ele.HasAttribute("name"))
                    {
                        if (ele.Attributes["type"].Value == "text" && ele.Attributes["name"].Value == "记录日期")
                        {
                            if (ele.ParentNode.Name == "p")
                            {
                                pList.Add(ele.ParentNode);
                            }
                        }
                    }
                }

                //补全每个病程的段落剩余段落，有的病程可能包含多个段落
                Dictionary<string, string> listDoc = new Dictionary<string, string>();
                foreach (XmlNode node in pList)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(node.OuterXml);
                    XmlNode siblingNode = node;
                    string key = ((XmlElement)node).GetElementsByTagName("text")[0].InnerText.Trim();
                    while (siblingNode.NextSibling != null)
                    {
                        siblingNode = siblingNode.NextSibling;
                        if (!pList.Contains(siblingNode))
                        {
                            sb.Append(siblingNode.OuterXml);
                        }
                        else
                        {
                            break;
                        }
                    }
                    listDoc.Add(key, sb.ToString());
                }

                //将病程的头和尾补充完全
                Dictionary<string, string> listDocReal = new Dictionary<string, string>();
                int index = 0;
                foreach (string key in listDoc.Keys)
                {
                    StringBuilder sbEmr = new StringBuilder();
                    sbEmr.Append("<document>");
                    sbEmr.Append(xmlDoc.GetElementsByTagName("pagesettings")[0].InnerXml);
                    sbEmr.Append(xmlDoc.GetElementsByTagName("header")[0].InnerXml);
                    sbEmr.Append(xmlDoc.GetElementsByTagName("footer")[0].InnerXml);
                    sbEmr.Append(xmlDoc.GetElementsByTagName("underwritemarks")[0].InnerXml);
                    sbEmr.Append("<savelogs></savelogs>");
                    sbEmr.Append(xmlDoc.GetElementsByTagName("docsetting")[0].InnerXml);
                    sbEmr.Append("<body>");

                    if (index == 0)//首次病程
                    {
                        sbEmr.Append(xmlDoc.GetElementsByTagName("body")[0].ChildNodes[0].InnerXml);
                        sbEmr.Append(listDoc[key]);
                    }
                    else
                    {
                        string strEmr = listDoc[key];
                        int indexEndP = strEmr.IndexOf("</p>");
                        sbEmr.Append(strEmr.Substring(indexEndP + 4));
                    }
                    sbEmr.Append("</body></document>");

                    if (index == 0)
                    {
                        listDocReal.Add(key, ReplaceItem(sbEmr.ToString()));
                    }
                    else
                    {
                        listDocReal.Add(key, sbEmr.ToString());
                    }
                    index++;
                }

                return listDocReal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 把首次病程中的替换为病历模板需要的样式
        ///【修改前】
        /// <p align="0">
        ///   <text fontbold="1" type="text" name="记录日期">2011-11-30 10:55:55</text>
        ///   <span fontbold="1" creator="39" />
        ///   <text type="text" name="标题">首次病程</text>
        ///   <eof />
        ///</p>
        ///【修改后】
        ///<p align="0">
        ///   <roelement type="fixedtext" name="T病程记录1" level="一级" print="False" fontsize="10.5" showborder="False">T病程记录1</roelement>
        ///   <eof />
        ///</p>
        /// </summary>
        private string ReplaceItem(string item)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(item);
                XmlNodeList nodeList = doc.GetElementsByTagName("text");
                foreach (XmlNode node in nodeList)
                {
                    if (((XmlElement)node).Attributes["name"].Value == "记录日期")
                    {
                        XmlElement ele = doc.CreateElement("roelement");
                        ele.SetAttribute("type", "fixedtext");
                        ele.SetAttribute("name", "T病程记录1");
                        ele.SetAttribute("level", "一级");
                        ele.SetAttribute("print", "False");
                        ele.SetAttribute("fontsize", "小四");
                        ele.SetAttribute("showborder", "False");
                        ele.InnerText = "T病程记录1";

                        XmlElement eleEnd = doc.CreateElement("eof");

                        XmlNode parentNode = node.ParentNode;
                        parentNode.RemoveAll();
                        ((XmlElement)parentNode).SetAttribute("align", "0");
                        parentNode.AppendChild(ele);
                        parentNode.AppendChild(eleEnd);
                        break;
                    }
                }
                nodeList = doc.GetElementsByTagName("pageend");
                for (int i = nodeList.Count - 1; i >= 0; i--)
                {
                    nodeList[i].ParentNode.RemoveChild(nodeList[i]);
                }
                return doc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal bool CheckExistRecordDetail(int id, string theTime)
        {
            try
            {
                bool boo = false;
                if (string.IsNullOrEmpty(theTime))
                {
                    return boo;
                }
                theTime = DateTime.Parse(theTime).ToString("yyyy-MM-dd HH:mm:ss");
                string content = DS_SqlService.GetRecordContentsByID(id);
                if (!string.IsNullOrEmpty(content.Trim()))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(content);
                    List<string> list = DS_BaseService.GetEditEmrContentTimes(doc);
                    string theTitle = DS_BaseService.GetEmrRecordTitle(doc, theTime);
                    string nextTime = list.Where(p => DateTime.Parse(p) > DateTime.Parse(theTime)).OrderBy(q => DateTime.Parse(q)).FirstOrDefault();
                    string nextTitle = !string.IsNullOrEmpty(nextTime) ? DS_BaseService.GetEmrRecordTitle(doc, nextTime) : "</body></document>";
                    int startIndex = doc.InnerXml.IndexOf(theTitle);
                    int endIndex = doc.InnerXml.IndexOf(nextTitle);
                    if (startIndex < 0 || endIndex < 0 || startIndex > endIndex)
                    {
                        return boo;
                    }
                    if (!string.IsNullOrEmpty(doc.InnerXml.Substring(startIndex, endIndex - startIndex)))
                    {
                        boo = true;
                    }
                }
                return boo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
