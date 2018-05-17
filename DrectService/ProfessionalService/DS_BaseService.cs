using DrectSoft.Common;
using DrectSoft.DSSqlHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Service
{
    /// <summary>
    /// 功能描述：通用数据操作类
    ///    数据通用方法
    ///    目的是集中常用数据操作
    /// 创 建 者：Yanqiao.Cai
    /// 创建日期：2012-12-03
    /// </summary>
    public static class DS_BaseService
    {
        /// <summary>
        /// 获取用户ID集合
        /// 注：ID转为6位长度
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-04</date>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetMaxLengthUserList(string[] users)
        {
            try
            {
                List<string> list = new List<string>();
                if (null != users && users.Length > 0)
                {
                    foreach (string str in users)
                    {
                        if (str == "00" || str.Length >= 6)
                        {
                            list.Add(str);
                        }
                        else
                        {
                            int len = str.Length;
                            string userID = str;
                            for (int i = 0; i < 6 - len; i++)
                            {
                                userID = "0" + userID;
                            }
                            list.Add(userID);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据用户ID获取用户名称和ID显示字符串
        /// 格式：用户名(用户ID)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-16</date>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetUserNameAndID(string userID)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return string.Empty;
                }
                string userStr = userID;
                DataTable userDt = DS_SqlService.GetUserByID(userID);
                if (null != userDt && userDt.Rows.Count > 0)
                {
                    userStr = userDt.Rows[0]["NAME"].ToString() + "(" + userID + ")";
                }
                return userStr;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 判断用户是否为病案室人员
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static bool CheckIfQuatityControlPerson(string userID)
        {
            try
            {
                bool boo = false;
                if (!string.IsNullOrEmpty(userID))
                {
                    string config = DS_SqlService.GetConfigValueByKey("PersonOfRecordRoom");
                    if (!string.IsNullOrEmpty(config))
                    {
                        List<string> str = GetMaxLengthUserList(config.Split(','));
                        if (null != str && str.Count > 0)
                        {
                            boo = str.Contains(userID);
                        }
                    }
                }
                return boo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取科室名称
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-18</date>
        /// <returns></returns>
        public static string GetDeptNameByID(string deptID)
        {
            try
            {
                string deptName = string.Empty;
                DataTable dt = DS_SqlService.GetDepartmentByID(deptID);
                if (null != dt && dt.Rows.Count > 0)
                {
                    deptName = null == dt.Rows[0]["NAME"] ? "" : dt.Rows[0]["NAME"].ToString().Trim();
                }

                return deptName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取病区名称
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-07</date>
        /// <returns></returns>
        public static string GetWardNameByID(string wardID)
        {
            try
            {
                string wardName = string.Empty;
                DataTable dt = DS_SqlService.GetWardByID(wardID);
                if (null != dt && dt.Rows.Count > 0)
                {
                    wardName = null == dt.Rows[0]["NAME"] ? "" : dt.Rows[0]["NAME"].ToString().Trim();
                }

                return wardName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取权限内的科室ID和病区ID集合
        /// 注：string[0]:科室；string[1]:病区
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-18</date>
        /// <returns></returns>
        public static List<string[]> GetDeptAndWardInRight(string userID)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return new List<string[]>();
                }

                List<string[]> list = new List<string[]>();
                DataTable dt = DS_SqlService.GetDeptAndWardInRight(userID);
                if (null != dt && dt.Rows.Count > 0)
                {
                    foreach (DataRow drow in dt.Rows)
                    {
                        string[] array = new string[2];
                        array[0] = null == drow["deptid"] ? "" : drow["deptid"].ToString().Trim();
                        array[1] = null == drow["wardid"] ? "" : drow["wardid"].ToString().Trim();
                        list.Add(array);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检查病人是否已出院
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-01</date>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public static bool CheckIfOutHos(int noofinpat)
        {
            try
            {

                DataTable dt = DS_SqlService.GetInpatientByID(noofinpat);
                if (null != dt && dt.Rows.Count == 1)
                {
                    if (Tool.IsInt(dt.Rows[0]["STATUS"].ToString()) && (int.Parse(dt.Rows[0]["STATUS"].ToString()) == 1502 || int.Parse(dt.Rows[0]["STATUS"].ToString()) == 1503))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-05-27</date>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public static List<string> GetRolesByUserID(string userid)
        {
            try
            {
                DataTable dt = DS_SqlService.GetUserByID(userid);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return null;
                }
                string jobs = dt.Rows[0]["jobid"].ToString();
                if (null == jobs || string.IsNullOrEmpty(jobs.Trim()))
                {
                    return null;
                }
                return jobs.Split(',').ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取入院时间(系统配置)
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public static DateTime GetInHostTime(int noofinpat)
        {
            try
            {
                DataTable dt = DS_SqlService.GetInpatientByID(noofinpat);
                if (null != dt && dt.Rows.Count > 0)
                {
                    string config = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                    if (!string.IsNullOrEmpty(config))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(config);
                        XmlNodeList nodeList = doc.GetElementsByTagName("InHosTimeType");
                        if (null != nodeList && nodeList.Count > 0)
                        {
                            string cfgValue = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                            if (cfgValue == "0")
                            {//入院
                                if (!string.IsNullOrEmpty(dt.Rows[0]["admitdate"].ToString().Trim()))
                                {
                                    return DateTime.Parse(dt.Rows[0]["admitdate"].ToString());
                                }
                                else if (!string.IsNullOrEmpty(dt.Rows[0]["inwarddate"].ToString().Trim()))
                                {
                                    return DateTime.Parse(dt.Rows[0]["inwarddate"].ToString());
                                }
                            }
                            else if (cfgValue == "1")
                            {//入科
                                if (!string.IsNullOrEmpty(dt.Rows[0]["inwarddate"].ToString().Trim()))
                                {
                                    return DateTime.Parse(dt.Rows[0]["inwarddate"].ToString());
                                }
                                else if (!string.IsNullOrEmpty(dt.Rows[0]["admitdate"].ToString().Trim()))
                                {
                                    return DateTime.Parse(dt.Rows[0]["admitdate"].ToString());
                                }
                            }
                        }
                    }
                }
                return DateTime.MinValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取文书录入路径
        /// </summary>
        /// <returns></returns>
        public static string GetUCEmrInputPath()
        {
            try
            {
                string config = DS_SqlService.GetConfigValueByKey("IsNewUcInput");
                string path = string.Empty;
                if (null != config && config.Trim() == "1")
                {
                    path = "DrectSoft.Core.MainEmrPad.New.MainFormNew";
                }
                else
                {
                    path = "DrectSoft.Core.MainEmrPad.MainForm";
                }
                return path;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 关于病历病程
        /// <summary>
        /// 检查病历是否未归档
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// </summary>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        public static bool CheckRecordRebacked(string noofinpat)
        {
            try
            {
                string sqlStr = " select distinct b.islock,a.noofinpat,a.patid,a.name,a.outhosdept,a.outhosward,a.outhosdate,a.outwarddate,a.status from inpatient a left join recorddetail b on a.noofinpat=b.noofinpat where a.noofinpat=@noofinpat and b.valid=1 and trim(a.OutWardDate) is not null and trim(a.inwarddate) is not null and a.status in(1502,1503) and (b.islock in(0,4700,4702,4703) or b.islock is null) ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int,32)
                };
                sqlParams[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);

                bool boo = null != dt && dt.Rows.Count > 0;
                return boo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检查是否存在病历记录
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-10</date>
        /// <param name="id">首次病程ID</param>
        /// <param name="theTime">病历时间</param>
        /// <returns></returns>
        public static bool CheckExistRecordDetail(int id, string theTime)
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
                if (!string.IsNullOrEmpty(content))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(content);
                    List<string> list = GetEditEmrContentTimes(doc);
                    string theTitle = GetEmrRecordTitle(doc, theTime);
                    string nextTime = list.Where(p => DateTime.Parse(p) > DateTime.Parse(theTime)).OrderBy(q => DateTime.Parse(q)).FirstOrDefault();
                    string nextTitle = !string.IsNullOrEmpty(nextTime) ? GetEmrRecordTitle(doc, nextTime) : "</body></document>";
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

        /// <summary>
        /// 检查是否存在病历记录 --- 事务专用
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-10</date>
        /// <param name="id">首次病程ID</param>
        /// <param name="theTime">病历时间</param>
        /// <returns></returns>
        public static bool CheckExistRecordDetailInTran(int id, string theTime)
        {
            try
            {
                bool boo = false;
                if (string.IsNullOrEmpty(theTime))
                {
                    return boo;
                }
                theTime = DateTime.Parse(theTime).ToString("yyyy-MM-dd HH:mm:ss");
                //string content = DS_SqlService.GetRecordContentsByID(id);
                string content = DS_SqlService.GetRecordContentsByIDInTran(id);

                if (!string.IsNullOrEmpty(content))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(content);
                    List<string> list = GetEditEmrContentTimes(doc);
                    string theTitle = GetEmrRecordTitle(doc, theTime);
                    string nextTime = list.Where(p => DateTime.Parse(p) > DateTime.Parse(theTime)).OrderBy(q => DateTime.Parse(q)).FirstOrDefault();
                    string nextTitle = !string.IsNullOrEmpty(nextTime) ? GetEmrRecordTitle(doc, nextTime) : "</body></document>";
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

        /// <summary>
        /// 给缺少<p>的标题附加节点
        /// </summary>
        /// 注：针对交互往某一病历前插入病历时，上一个病历的结束标记<eof /></p>和该病历的开始标记<p>丢失的情况
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-14</date>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static XmlDocument AddNodesToXmlDocument(XmlDocument doc)
        {
            try
            {
                if (null == doc || string.IsNullOrEmpty(doc.InnerXml))
                {
                    return doc;
                }
                XmlNodeList nodeList = doc.SelectNodes("//text");
                if (null == nodeList || nodeList.Count == 0)
                {
                    return doc;
                }
                foreach (XmlNode node in nodeList)
                {
                    if (null == node || null == node.Attributes["name"] || node.Attributes["name"].Value != "记录日期")
                    {
                        continue;
                    }
                    if (node.ParentNode.ChildNodes[0].LocalName != "text")
                    {
                        doc.InnerXml = doc.InnerXml.Replace(node.OuterXml, "<eof /></p><p>" + node.OuterXml);
                    }
                }
                return doc;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将XML拆分成病历数组(包含header和footer)
        /// </summary>
        /// string[0] ---> 病历时间(yyyy-MM-dd HH:mm:ss)
        /// string[1] ---> 病历内容
        /// 注：header的string[0]="header"；
        ///     footer的string[0]="footer"；
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<string[]> GetRecoedsByXML(XmlDocument doc)
        {
            try
            {
                if (null == doc || string.IsNullOrEmpty(doc.InnerXml.Trim()))
                {
                    return null;
                }
                //添加缺失的节点(如果有缺失的话)
                doc = AddNodesToXmlDocument(doc);
                //获取病历标题数组
                List<string[]> titleList = new List<string[]>();
                XmlNodeList nodelist = doc.SelectNodes("//p//text");
                foreach (XmlNode node in nodelist)
                {
                    if (null != node.Attributes["name"] && node.Attributes["name"].Value == "记录日期")
                    {
                        string[] array = new string[2];
                        array[0] = DateTime.Parse(node.InnerText).ToString("yyyy-MM-dd HH:mm:ss");
                        if (doc.InnerXml.Substring(doc.InnerXml.IndexOf(node.OuterXml) - 30, node.OuterXml.Length + 15).Contains("<p"))
                        {
                            array[1] = node.ParentNode.OuterXml;
                        }
                        else
                        {
                            array[1] = node.OuterXml;
                        }
                        titleList.Add(array);
                    }
                }
                if (null == titleList || titleList.Count() == 0)
                {
                    return null;
                }
                //获取对应病历
                List<string[]> recordList = new List<string[]>();
                for (int i = 0; i < titleList.Count(); i++)
                {
                    string[] titleArray = titleList[i];
                    if (i == 0)
                    {//如果是第一个病历，则先添加header
                        string[] headerArray = new string[2];
                        headerArray[0] = "header";
                        headerArray[1] = doc.InnerXml.Substring(0, doc.InnerXml.IndexOf(titleArray[1]));
                        recordList.Add(headerArray);
                    }
                    //添加病历
                    string[] recordArray = new string[2];
                    recordArray[0] = titleArray[0];
                    int startIndex = doc.InnerXml.IndexOf(titleArray[1]);
                    int endIndex = 0;
                    if (i == titleList.Count() - 1)
                    {
                        endIndex = doc.InnerXml.IndexOf("</body></document>");
                    }
                    else
                    {
                        endIndex = doc.InnerXml.IndexOf(titleList[i + 1][1]);
                    }
                    if (startIndex >= 0 && endIndex >= 0 && startIndex <= endIndex)
                    {
                        recordArray[1] = doc.InnerXml.Substring(startIndex, endIndex - startIndex);
                    }
                    recordList.Add(recordArray);
                    if (i == titleList.Count() - 1)
                    {//如果是最后一个病历，则先添加footer
                        string[] footerArray = new string[2];
                        footerArray[0] = "footer";
                        footerArray[1] = "</body></document>";
                        recordList.Add(footerArray);
                    }
                }
                return recordList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据时间获取病程记录标题
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-10</date>
        /// 标题格式：<p align="0"><text fontbold="1" type="text" name="记录日期">2012-12-23 09:38:46</text>
        /// 注：1、为解决空格丢失问题
        ///     2、为解决<p align="0">节点多余或缺少问题
        /// <param name="patid"></param>
        /// <returns></returns>
        public static string GetEmrRecordTitle(XmlDocument doc, string theTime)
        {
            try
            {
                if (null != doc && !string.IsNullOrEmpty(null == doc.InnerXml ? "" : doc.InnerXml.Trim()))
                {
                    XmlNodeList nodelist = doc.SelectNodes("//p//text");
                    foreach (XmlNode node in nodelist)
                    {
                        if (node.InnerText == theTime)
                        {
                            string pTitle = node.ParentNode.OuterXml;
                            string title = string.Empty;
                            if (pTitle.IndexOf("</text>") > 0)
                            {
                                title = pTitle.Substring(0, pTitle.IndexOf(node.OuterXml) + node.OuterXml.Length);
                            }
                            else
                            {
                                title = node.OuterXml;
                            }
                            //修复首程前添加病历编辑器节点丢失的bug
                            if (title.IndexOf("<p align=\"0\"><text") != 0 && title.Length > node.OuterXml.Length * 2)
                            {
                                title = node.OuterXml;
                            }
                            return title;
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取编辑病程的所有节点日期时间
        /// 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-10</date>
        /// <param name="body">病程body</param>
        /// <returns></returns>
        public static List<string> GetEditEmrContentTimes(XmlDocument xmlRecord)
        {
            try
            {
                if (null == xmlRecord || string.IsNullOrEmpty(null == xmlRecord.InnerXml ? "" : xmlRecord.InnerXml.Trim()))
                {
                    return new List<string>();
                }
                List<string> alRecordTime = new List<string>();
                XmlNodeList inbody = xmlRecord.SelectSingleNode("//body").ChildNodes;
                foreach (XmlNode p in inbody)
                {
                    XmlNodeList inp = p.ChildNodes;
                    if (null == inp || inp.Count == 0)
                    {
                        continue;
                    }
                    foreach (XmlNode node in inp)
                    {
                        if (null == node || null == node.Attributes || node.Attributes.Count == 0)
                        {
                            continue;
                        }
                        foreach (XmlNode n in node.Attributes)
                        {
                            if (n.Value == "记录日期" && !string.IsNullOrEmpty(node.InnerText))
                            {
                                alRecordTime.Add(DateTime.Parse(node.InnerText).ToString("yyyy-MM-dd HH:mm:ss"));
                                break;
                            }
                        }
                    }
                }
                return alRecordTime.OrderBy(p => DateTime.Parse(p)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将符合条件的未归档病人病历归档
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-23</date>
        /// <param name="noofinpat"></param>
        public static void SetRecordsRebacked(int noofinpat)
        {
            try
            {
                DataTable dt = DS_SqlService.GetAllRecordsByNoofinpat(noofinpat);
                if (null == dt || dt.Rows.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人没有病历，无法归档。");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您是否要归档该病历？", "归档病历", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                if (CheckRecordRebacked(noofinpat.ToString()))
                {
                    int num = DS_SqlService.SetRecordsRebacked(noofinpat.ToString());
                    if (num > 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("归档成功");
                        return;
                    }
                }
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人已归档。");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 关于病案首页
        /// <summary>
        /// 向Datatable中加入出院情况ID、出院情况两列数据
        /// 注：Datatable中需包含iem_mainpage_diagnosis_no列
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable AddColsDataToDataTable(DataTable dt)
        {
            try
            {
                if (!dt.Columns.Contains("Status_Id_Out"))
                {
                    dt.Columns.Add("Status_Id_Out");
                }
                if (!dt.Columns.Contains("Status_Name_Out"))
                {
                    dt.Columns.Add("Status_Name_Out");
                }
                if (null != dt && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (null != dr && null != dr["iem_mainpage_diagnosis_no"] && !string.IsNullOrEmpty(dr["iem_mainpage_diagnosis_no"].ToString().Trim()) && Tool.IsInt(dr["iem_mainpage_diagnosis_no"].ToString().Trim()))
                        {
                            DataTable coditionDT = DS_SqlService.GetOutHosConditonByID(int.Parse(dr["iem_mainpage_diagnosis_no"].ToString().Trim()));
                            if (null != coditionDT && coditionDT.Rows.Count > 0)
                            {
                                dr["Status_Id_Out"] = coditionDT.Rows[0]["status_id"];
                                dr["Status_Name_Out"] = coditionDT.Rows[0]["Status_Name"];
                            }
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 关于诊断/病种/病种组合
        /// <summary>
        /// 根据病种组合ID获取病种ICD集合
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-29</date>
        /// <param name="groupid">病种组合ID</param>
        /// <returns></returns>
        public static List<string> GetDiseaseIDsByGroupID(int groupid)
        {
            try
            {
                List<string> list = new List<string>();
                DataTable dt = DS_SqlService.GetDiseaseGroupByID(groupid);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return list;
                }
                string IDs = null == dt.Rows[0]["diseaseids"] ? "" : dt.Rows[0]["diseaseids"].ToString();
                return IDs.Split('$').ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据病种组合ID获取病种字符串
        /// 注：name1(icd1)，name2(icd2)， ......
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="groupid">病种组合ID</param>
        /// <returns></returns>
        public static string GetDiseaseStringByGroupID(int groupid)
        {
            try
            {
                //根据组合ID查找ICD集合
                List<string> diseaseIDs = GetDiseaseIDsByGroupID(groupid);
                if (null == diseaseIDs || diseaseIDs.Count() == 0)
                {
                    return string.Empty;
                }
                //将ICD集合拼接成SQL查询的ICD字符串
                string idStr = string.Empty;
                foreach (string diseaseID in diseaseIDs)
                {
                    string newID = diseaseID;
                    if (diseaseID.Contains("'"))
                    {
                        newID = diseaseID.Replace("'", "");
                    }
                    idStr += "'" + newID + "',";
                }
                if (!string.IsNullOrEmpty(idStr))
                {
                    idStr = idStr.Substring(0, idStr.Length - 1);
                }
                //查询病种集合
                DataTable dt = DS_SqlService.GetDiseasesByICDs(idStr);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return string.Empty;
                }
                //将病种集合拼接成所需要格式的字符串
                string content = string.Empty;
                foreach (DataRow drow in dt.Rows)
                {
                    content += drow["NAME"].ToString() + "(" + drow["ICD"].ToString() + ")，";
                }
                if (!string.IsNullOrEmpty(content))
                {
                    content = content.Substring(0, content.Length - 1);
                }
                return content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据用户ID获取病种组合ID集合
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static List<string> GetDiseaseGroupIDsByUserID(string userID)
        {
            try
            {
                List<string> list = new List<string>();
                DataTable dt = DS_SqlService.GetUserMatchDiseaseGroup(userID);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return list;
                }
                string IDs = null == dt.Rows[0]["groupids"] ? "" : dt.Rows[0]["groupids"].ToString();
                return IDs.Split('$').ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据用户ID获取权限内的病种数据集
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static DataTable GetDiagResourceByUserID(string userID)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return new DataTable();
                }

                DataTable groupDt = GetDiseaseGroupsByUserID(userID);
                string diseaseIDs = string.Empty;
                if (null == groupDt || groupDt.Rows.Count == 0)
                {
                    return new DataTable();
                }
                foreach (DataRow drow in groupDt.Rows)
                {
                    string icds = null == drow["diseaseids"] ? "" : drow["diseaseids"].ToString();
                    List<string> icdList = icds.Split('$').ToList();
                    if (null == icdList || icdList.Count() == 0)
                    {
                        continue;
                    }
                    foreach (string str in icdList)
                    {
                        diseaseIDs += "'" + str + "',";
                    }
                }
                if (!string.IsNullOrEmpty(diseaseIDs))
                {
                    diseaseIDs = diseaseIDs.Substring(0, diseaseIDs.Length - 1);
                }
                if (string.IsNullOrEmpty(diseaseIDs))
                {
                    new DataTable();
                }
                return DS_SqlService.GetDiseasesByICDs(diseaseIDs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据用户ID获取权限内的病种组合数据集
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-10</date>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static DataTable GetDiseaseGroupsByUserID(string userID)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return new DataTable();
                }
                DataTable dt = DS_SqlService.GetUserMatchDiseaseGroup(userID);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return new DataTable();
                }
                string groupIDs = dt.Rows[0]["GROUPIDS"].ToString().Replace("$", ",").Replace("'", "");

                return DS_SqlService.GetDiseaseGroupsByIDs(groupIDs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


        /// <summary>
        /// xll 2013-06-06
        /// 读取file.menu中的节点 判断各个节点是否存在
        /// 如果连key属性都没有的话 返回true
        /// </summary>
        /// <param name="keyName">file.menu中key属性的值</param>
        /// <returns></returns>
        public static bool FlieHasKey(string keyName)
        {
            try
            {
                string fliePath = Application.StartupPath + "\\file.menu";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fliePath);
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("/config/plugins/plugin");
                foreach (XmlNode item in xmlNodeList)
                {
                    XmlAttribute xmlAttribute = item.Attributes["key"];
                    if (xmlAttribute == null)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("file.menu中没有添加key属性");
                        return true;
                    }
                    if (xmlAttribute.Value == keyName)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据传进来的KEY返回是否显示此模块相关按钮
        /// add by ywk 2013年6月6日 11:24:45
        /// 针对文书录入主体的控制是否显示LIS PACS 医嘱信息
        /// </summary>
        /// 二次更改（新增一个参数，配置名称+键名称）ywk 2013年6月13日 10:04:09
        /// <returns></returns>
        public static bool IsShowThisMD(string configkeyname, string showkey)
        {
            bool IsShowthismoudle = true;
            string configvalue = DS_SqlService.GetConfigValueByKey(configkeyname);
            Dictionary<string, string> myDic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(configvalue))
            {
                string[] value = configvalue.Split('-');
                if (value.Length > 0)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        myDic.Add(value[i].Substring(0, value[i].ToString().IndexOf(',')), value[i].Substring(value[i].ToString().IndexOf(',') + 1, value[i].ToString().Length - value[i].ToString().IndexOf(',') - 1));
                    }
                    foreach (string item in myDic.Keys)
                    {
                        if (item == showkey && myDic[showkey].ToString() == "0")
                        {
                            IsShowthismoudle = false;
                            break;
                        }
                        else
                        {
                            IsShowthismoudle = true;
                        }
                    }
                }
            }


            return IsShowthismoudle;
        }



        /// <summary>
        /// 出院诊断列表中，存在属于传染病诊断的列表，且已经申报的次数小于该诊断设置的最大申报次数
        /// </summary>
        /// <param name="categoryid">1：传染病，2：肿瘤病（关联数据库表ReportCategory：CreagoryID字段）</param>
        /// <param name="diagnosis">诊断编号</param>
        /// <returns></returns>
        public static Boolean GetReportVialde(string categoryid, string diagnosis)
        {
            //验证逻辑： 出院诊断列表中，存在属于传染病诊断的列表，且已经申报的次数小于该诊断设置的最大申报次数
            string sqlText = "SELECT * FROM zymosis_diagnosis z where z.categoryid=" + Convert.ToInt32(categoryid) + " and z.icd='" + diagnosis + "' ";
            DataTable table = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlText);//new SqlParameter[] { new SqlParameter("@categoryid", categoryid), new SqlParameter("@diagnosis", diagnosis) }
            if (table != null && table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 验证除去当前肿瘤病类，是否还存在符合申报的，诊断
        /// </summary>
        /// <param name="Iem_Mainpage_NO">病案首页号</param>
        /// <param name="diagicd10list">诊断CID10</param>
        /// <param name="CategoryID">报告卡类别</param>
        /// <returns></returns>
        public static DataTable GetRecordValidat(string Iem_Mainpage_NO, string CategoryID)
        {
            string diagicd10 = string.Empty;

            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            string sqlText = "select ie.iem_mainpage_no,z.icd,z.upcount,z.name from iem_mainpage_diagnosis_2012 ie    left join zymosis_diagnosis z on ie.diagnosis_code=z.icd   left join iem_mainpage_basicinfo_2012 imb on imb.iem_mainpage_no=ie.iem_mainpage_no where  z.valid=1 and z.CATEGORYID=" + Convert.ToInt32(CategoryID) + "  and ie.valide=1  and imb.valide=1  and  ie.iem_mainpage_no='" + Iem_Mainpage_NO + "'   AND (SELECT COUNT(*) FROM theriomareportcard zr where 1=1 and zr.diagicd10=z.icd and zr.REPORT_NOOFINPAT=imb.noofinpat and zr.vaild=1)< z.upcount ";

            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sqlText = "select ie.iem_mainpage_no,z.icd,z.upcount,z.name from iem_mainpage_diagnosis_sx ie    left join zymosis_diagnosis z on ie.diagnosis_code=z.icd   left join iem_mainpage_basicinfo_sx imb on imb.iem_mainpage_no=ie.iem_mainpage_no where  z.valid=1  and ie.valide=1 and z.CATEGORYID=" + Convert.ToInt32(CategoryID) + " and imb.valide=1  and  ie.iem_mainpage_no='" + Iem_Mainpage_NO + "'  AND (SELECT COUNT(*) FROM theriomareportcard zr where 1=1 and zr.diagicd10=z.icd and zr.REPORT_NOOFINPAT=imb.noofinpat and zr.vaild=1)< z.upcount ";

            }
            sqlText += "  group by ie.iem_mainpage_no,z.icd,z.upcount,z.name    having count(z.icd)>0 ";

            DataTable table = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlText, CommandType.Text);
            if (table != null && table.Rows.Count > 0)
            {
                return table;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取医院名称
        /// </summary>
        /// <returns></returns>
        public static string GetHosPatInfo()
        {
            try
            {
                string sql = @"select name from hospitalinfo";
                object objectstr = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteScalar(sql, CommandType.Text);
                return objectstr.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
