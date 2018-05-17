using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Eop;
using System.Data;
using DrectSoft.Emr.Util;
using System.IO;
using System.Xml;
using DrectSoft.Service;
using System.Data.SqlClient;
using DrectSoft.DSSqlHelper;
using DrectSoft.Core.MainEmrPad.HistoryEMR;
using DrectSoft.Common;
using DrectSoft.Core.MainEmrPad.New;

namespace DrectSoft.Core.MainEmrPad
{
    /// <summary>
    /// 历史病历的相关业务逻辑
    /// </summary>
    public class HistoryEMRBLLNew
    {
        IEmrHost m_Host;
        Inpatient m_Inpatient;

        /// <summary>
        /// 重载构造
        /// </summary>
        /// <param name="host"></param>
        /// <param name="currentPatient"></param>
        /// <param name="recordDal"></param>
        public HistoryEMRBLLNew(IEmrHost host, Inpatient inpatient)
        {
            try
            {
                m_Host = host;
                m_Inpatient = inpatient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过Noofinpat获取病人转科情况
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetDeptListByInpatient(string noofinpat)
        {
            try
            {
                string sqlGetDeptList = string.Format(
                    @"select i.id, d.name deptname, w.name wardname, i.createtime inTime
                        from inpatientchangeinfo i, department d, ward w
                       where i.newdeptid = d.id and i.newwardid = w.id and i.noofinpat = '{0}' and i.valid = 1 and i.changestyle in('0','1','2') order by i.createtime ", noofinpat);
                return DS_SqlHelper.ExecuteDataTable(sqlGetDeptList, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 通过noofinpat 和 deptid 获取病历记录
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public DataTable GetEmrListByNoofinpatAndDeptChangeID(string noofinpat, string deptChangeid)
        {
            try
            {
                string sqlGetEmrList = string.Format(
                    @"select r.id, r.name, r.sortid, r.captiondatetime, '' content, e.mr_name, '' emrtitle, 
                             r.templateid, r.firstdailyflag as isfirstdaily, e.isyihuangoutong, e.isconfigpagesize
                        from recorddetail r
                        left outer join emrtemplet e on r.templateid = e.templet_id and e.valid = '1'
                       where r.valid = 1 and r.noofinpat = '{0}' and (r.changeid = '{1}' or r.changeid is null)
                    and r.sortid not in('AI','AJ','AK') order by r.sortid, r.captiondatetime", noofinpat, deptChangeid);
                return DS_SqlHelper.ExecuteDataTable(sqlGetEmrList, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 通过noofinpat 获取护理记录
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public DataTable GetNurseEmrListByNoofinpat(int noofinpat)
        {
            try
            {
                string sqlGetNurseEmrList = string.Format(
                    @"select r.id, r.name, r.sortid, r.captiondatetime, '' content, e.mr_name, '' emrtitle, 
                             r.templateid, r.firstdailyflag as isfirstdaily, e.isyihuangoutong, e.isconfigpagesize
                        from recorddetail r
                        left outer join emrtemplet e on r.templateid = e.templet_id and e.valid = '1'
                       where r.valid = 1 and r.noofinpat = {0}
                       and r.sortid in('AI','AJ','AK')
                    order by r.captiondatetime", noofinpat);
                return DS_SqlHelper.ExecuteDataTable(sqlGetNurseEmrList, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 通过ID获取病历内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetEmrContentByID(string id)
        {
            try
            {
                string sqlGetEmrContent = string.Format("select content from recorddetail where id = '{0}'", id);
                return DS_SqlHelper.ExecuteScalar(sqlGetEmrContent, CommandType.Text).ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public void HistoryEmrBatchIn(List<EmrNode> emrNodeList, string deptChangeID)
        {
            try
            {
                //【1】捞取病历内容
                foreach (EmrNode node in emrNodeList)
                {
                    if (node.DataRowItem["content"].ToString().Trim() == "")
                    {
                        node.DataRowItem["content"] = GetEmrContentByID(node.ID);
                    }
                }

                //【2】修改病程中对应的时间和标题
                foreach (EmrNode node in emrNodeList)
                {
                    if (node.DataRowItem["sortid"].ToString().Trim() == "AC")
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.PreserveWhitespace = true;
                        string emrContent = node.DataRowItem["content"].ToString();
                        if (emrContent.Trim() != "")
                        {
                            doc.LoadXml(emrContent);
                            doc.GetElementsByTagName("text").OfType<XmlElement>().ToList().ForEach(ele =>
                                {
                                    if (ele.HasAttribute("name") && ele.Attributes["name"].Value == "记录日期")
                                    {
                                        ele.InnerText = node.DataRowItem["captiondatetime"].ToString();
                                    }
                                    else if (ele.HasAttribute("name") && ele.Attributes["name"].Value == "标题")
                                    {
                                        ele.InnerText = node.DataRowItem["EMRTITLE"].ToString();
                                    }
                                });
                            node.DataRowItem["content"] = doc.InnerXml;
                        }
                    }
                    //更改病历中宏元素的值
                    string content = node.DataRowItem["content"].ToString().Trim();
                    if (content.Trim() != "")
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.PreserveWhitespace = true;
                        doc.LoadXml(content);
                        FillModelMacro(doc, deptChangeID);
                        node.DataRowItem["content"] = doc.InnerXml;
                    }
                }


                //【3】保存病历
                foreach (EmrNode node in emrNodeList)
                {
                    HistoryEmrBatchInInner(node, deptChangeID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HistoryEmrBatchInInner(EmrNode node, string deptChangeID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                    new SqlParameter("@noofinpat", SqlDbType.VarChar),
                    new SqlParameter("@templateid", SqlDbType.VarChar),
                    new SqlParameter("@name", SqlDbType.VarChar),
                    new SqlParameter("@content", SqlDbType.VarChar),
                    new SqlParameter("@sortid", SqlDbType.VarChar),
                    new SqlParameter("@owner", SqlDbType.VarChar),
                    new SqlParameter("@createtime", SqlDbType.VarChar),
                    new SqlParameter("@captiondatetime", SqlDbType.VarChar),
                    new SqlParameter("@firstdailyflag", SqlDbType.VarChar),
                    new SqlParameter("@isyihuangougong", SqlDbType.VarChar),
                    new SqlParameter("@ip", SqlDbType.VarChar),
                    new SqlParameter("@isconfigpagesize", SqlDbType.VarChar),
                    new SqlParameter("@departcode", SqlDbType.VarChar),
                    new SqlParameter("@wardcode", SqlDbType.VarChar),
                    new SqlParameter("@changeid", SqlDbType.VarChar)
                };
                parms[0].Value = m_Inpatient.NoOfFirstPage;
                parms[1].Value = node.DataRowItem["templateid"].ToString();
                if (node.DataRowItem["sortid"].ToString() == "AC")
                {
                    parms[2].Value = node.DataRowItem["mr_name"].ToString() + " " + node.DataRowItem["captiondatetime"].ToString() + " " + m_Host.User.Name;
                }
                else
                {
                    parms[2].Value = node.DataRowItem["mr_name"].ToString() + " " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + m_Host.User.Name;
                }
                string emrContent = node.DataRowItem["content"].ToString();
                emrContent = Util.ProcessEmrContent(emrContent);//处理病历中的SaveLog节点和creator、deleter属性的值
                parms[3].Value = emrContent;
                parms[4].Value = node.DataRowItem["sortid"].ToString();
                parms[5].Value = m_Host.User.Id;
                parms[6].Value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                parms[7].Value = node.DataRowItem["captiondatetime"].ToString();
                parms[8].Value = node.DataRowItem["isfirstdaily"].ToString();
                parms[9].Value = node.DataRowItem["isyihuangoutong"].ToString();
                parms[10].Value = DS_Common.GetIPHost();
                parms[11].Value = node.DataRowItem["isconfigpagesize"].ToString();
                parms[12].Value = m_Host.User.CurrentDeptId;
                parms[13].Value = m_Host.User.CurrentWardId;
                parms[14].Value = null == deptChangeID ? string.Empty : deptChangeID;

                string sqlInsert = @"INSERT INTO recorddetail(id, noofinpat, templateid, name, content, sortid, owner,
                                            createtime, valid, hassubmit, hasprint, hassign,
                                            captiondatetime, islock, firstdailyflag,
                                            isyihuangoutong, ip, isconfigpagesize, departcode,
                                            wardcode, changeid)
                                VALUES(seq_recorddetail_id.nextval, @noofinpat, @templateid, @name, @content, @sortid, @owner,
                                        @createtime, '1', '4600', '3600', '0',
                                        @captiondatetime, '4700', @firstdailyflag,
                                        @isyihuangougong, @ip, @isconfigpagesize, @departcode,
                                        @wardcode, @changeid)";

                DS_SqlHelper.ExecuteNonQuery(sqlInsert, parms, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断该病人是否存在首次病程
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public bool IsHasFirstDailyEmr(string noofinpat)
        {
            try
            {
                string sqlGetFirstDaily = string.Format(
                        @"select count(1) from recorddetail r where r.noofinpat = '{0}' and r.valid = '1' and r.firstdailyflag = '1'", noofinpat);
                string cnt = DS_SqlHelper.ExecuteScalar(sqlGetFirstDaily, CommandType.Text).ToString();
                if (cnt != "0")
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取数据库中首次病程的时间
        /// </summary>
        /// <returns></returns>
        public string GetFirstDailyEmrCaptionDateTime(string noofinpat)
        {
            try
            {
                string sqlGetCaptionDateTime = string.Format(
                    @"SELECT r.captiondatetime FROM recorddetail r WHERE r.noofinpat = '{0}' and r.valid = '1' and r.sortid = 'AC' and r.firstdailyflag = 1", noofinpat);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlGetCaptionDateTime, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillModelMacro(XmlDocument emrDoc, string deptChangeID)
        {
            try
            {
                #region 替换文档中的宏

                //获得所有宏元素列表
                XmlNodeList nodes = emrDoc.GetElementsByTagName("macro");

                string deptName = string.Empty;
                string wardName = string.Empty;
                string bedID = string.Empty;

                if (!string.IsNullOrEmpty(deptChangeID))
                {
                    DataTable dtDeptInfo = GetDeptChangeInfo(deptChangeID);
                    if (dtDeptInfo.Rows.Count > 0)
                    {
                        foreach (DataColumn dc in dtDeptInfo.Columns)
                        {
                            switch (dc.ColumnName.ToLower())
                            {
                                case "deptname": deptName = dtDeptInfo.Rows[0]["deptname"].ToString();
                                    break;
                                case "wardname": wardName = dtDeptInfo.Rows[0]["wardname"].ToString();
                                    break;
                                case "bedid": bedID = dtDeptInfo.Rows[0]["bedid"].ToString();
                                    break;
                            }
                        }
                    }
                }

                //循环每个宏元素，根据宏元素的Name属性，查询并赋值线Text属性
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["name"].Value.Trim() == "科室")
                    {
                        node.InnerText = deptName;
                        continue;
                    }
                    if (node.Attributes["name"].Value.Trim() == "病区")
                    {
                        node.InnerText = wardName;
                        continue;
                    }
                    if (node.Attributes["name"].Value.Trim() == "床号")
                    {
                        node.InnerText = bedID;
                        continue;
                    }

                    //if (node.Attributes["name"].Value.Trim() == "科室" && node.InnerText.Trim() != "科室")
                    //    continue;
                    //if (node.Attributes["name"].Value.Trim() == "病区" && node.InnerText.Trim() != "病区")
                    //    continue;
                    //if (node.Attributes["name"].Value.Trim() == "床号" && node.InnerText.Trim() != "床号")
                    //    continue;
                    //if (node.Attributes["name"].Value.Trim() == "住院号" && node.InnerText.Trim() != "住院号")
                    //    continue;
                    //if (node.Attributes["name"].Value.Trim() == "医师签名" && node.InnerText.Trim() != "医师签名")
                    //    continue;
                    //if (node.Attributes["name"].Value.Trim() == "当前日期" && node.InnerText.Trim() != "当前日期")
                    //    continue;
                    //if (node.Attributes["name"].Value.Trim() == "当前时间" && node.InnerText.Trim() != "当前时间")
                    //    continue;

                    node.InnerText = MacroUtil.FillMarcValue(m_Inpatient.NoOfFirstPage.ToString(), node.Attributes["name"].Value, m_Host.User.Id);
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取转科信息
        /// </summary>
        /// <param name="deptChangeID"></param>
        /// <returns></returns>
        public DataTable GetDeptChangeInfo(string deptChangeID)
        {
            try
            {
                string sqlGetDeptChangeInfo = string.Format(
                    @"select d.name deptName, w.name wardName, i.newbedid bedID
                        from inpatientchangeinfo i
                  left outer join department d on d.id = i.newdeptid and d.valid = 1
                  left outer join ward w on w.id = i.newwardid and w.valid = 1
                       where i.id = '{0}' ", deptChangeID);
                return DS_SqlHelper.ExecuteDataTable(sqlGetDeptChangeInfo, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
