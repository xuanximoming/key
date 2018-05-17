using System.Linq;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DrectSoft.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Wordbook;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
//using DrectSoft.Core.TimeLimitQC;
using DevExpress.Utils;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.Emr.Util;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;

namespace DrectSoft.Emr.QcManagerNew
{
    class OperationOfBingchengjilu
    {
        IEmrHost m_App;
        SqlManger m_SqlManger;

        public OperationOfBingchengjilu(IEmrHost app)
        {
            m_App = app;
            m_SqlManger = new SqlManger(app);
        }

        #region 作废 2012-11-27 by wyt
        ///// <summary>
        ///// AC自动检测
        ///// </summary>
        ///// <param name="recorddetialid"></param>
        ///// <param name="mr_class"></param>
        ///// <returns></returns>
        //public void GetResultPoint(string recorddetialid, string mr_class, string noofinpat, string automarkrecordid, string pname)
        //{
        //    try
        //    {
        //        int hours;
        //        DataTable dt = new DataTable();
        //        string sql;
        //        //先判断是否首程
        //        sql = string.Format(@"select firstdailyflag from recorddetail where id='{0}' ", recorddetialid);
        //        dt = m_App.SqlHelper.ExecuteDataTable(sql);
        //        if (dt.Rows[0][0].ToString() == "1")
        //        {
        //            //判断是否在入院8小时内完成首程
        //            //sql = @"select round(to_number(to_date(r.createtime,'yyyy-mm-dd hh24:mi:ss')-to_date(t.inwarddate,'yyyy-mm-dd hh24:mi:ss'))*24) hours from inpatient t ,recorddetail r where t.noofinpat='{0}' and r.noofinpat='{0}' and r.firstdailyflag='1' and r.sortid='AC'";
        //            //sql = string.Format(sql, noofinpat);
        //            //dt = m_App.SqlHelper.ExecuteDataTable(sql);
        //            //if (dt == null || dt.Rows.Count == 0)
        //            //{
        //            //    return;
        //            //}
        //            //hours = int.Parse(dt.Rows[0][0].ToString());
        //            //if (hours > 8)
        //            //{
        //            //    m_SqlManger.InsertDB(371, "25", automarkrecordid, noofinpat, pname, "未在入院后8小时内完成");
        //            //}

        //            XmlDocument xmlRecord = new XmlDocument();
        //            //DataTable recordContent =
        //            //    m_App.SqlHelper.ExecuteDataTable(
        //            //    string.Format(@"select content from recorddetail where id='{0}'", recorddetialid), CommandType.Text);
        //            //edit by wyt 增加病程文件有效性筛选 2012-11-23
        //            DataTable recordContent =
        //                m_App.SqlHelper.ExecuteDataTable(
        //                string.Format(@"select content from recorddetail where id='{0}' and valid = 1", recorddetialid), CommandType.Text);
        //            //如果有病历内容
        //            if (recordContent == null && recordContent.Rows.Count <= 0)
        //            {
        //                return;
        //            }
        //            xmlRecord.LoadXml(recordContent.Rows[0]["content"].ToString());
        //            XmlNode body = xmlRecord.SelectSingleNode("//body");
        //            int tipcount = body.SelectNodes("//roelement").Count;
        //            ArrayList arr_element = new ArrayList();// 元素的名称
        //            //Regex rg = new Regex()
        //            for (int i = 0; i < tipcount; i++)
        //            {
        //                arr_element.Add(body.SelectNodes("//roelement")[i].InnerText.Replace(" ", "").Replace("　", ""));//将此病历内容中的标签加入到链表
        //            }
        //            //将取到的元素标签放到链表中，方便下面评分遍历

        //            DataTable PointConfig = new DataTable();
        //            PointConfig = m_SqlManger.GetPointConfig(mr_class);
        //            //存放要评分的项名称和规则
        //            Dictionary<string, string> m_pointconfig = new Dictionary<string, string>();
        //            if (PointConfig.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < PointConfig.Rows.Count; i++)
        //                {
        //                    m_pointconfig.Add(PointConfig.Rows[i]["id"].ToString(), PointConfig.Rows[i]["childname"].ToString() + PointConfig.Rows[i]["problem_desc"].ToString());
        //                }
        //            }
        //            //if (!arr_element.Contains("诊断"))
        //            //{
        //            //    m_SqlManger.InsertDB(372, "3", automarkrecordid, noofinpat, pname, "无初步诊断");
        //            //}
        //            //edit by wyt 2012-11-23
        //            if (!IsConatainStr(arr_element, "诊断"))
        //            {
        //                m_SqlManger.InsertDB(372, "3", automarkrecordid, noofinpat, pname, "无初步诊断");
        //            }

        //            //字数
        //            string allstr = body.InnerText;
        //            for (int j = 0; j < tipcount; j++)
        //            {
        //                if (body.SelectNodes("//roelement")[j].InnerText.ToString().Trim().Replace("　", "").Replace(" ", "").StartsWith("诊断"))
        //                {
        //                    string nexttip = body.SelectNodes("//roelement")[j + 1].InnerText.ToString();//下一个元素名称
        //                    string startstr = "诊断";
        //                    int startop = allstr.Replace("　", "").Replace(" ", "").IndexOf(startstr, 0) + startstr.Length; //开始位置
        //                    int endop = allstr.Replace("　", "").Replace(" ", "").IndexOf(nexttip, startop); //结束位置
        //                    string my = allstr.Replace("　", "").Replace(" ", "").Substring(startop, endop - startop).Replace("：", ""); //取搜索的条数，用结束的位置-开始的位置,并返回 
        //                    if (my.Length == 0)
        //                    {
        //                        m_SqlManger.InsertDB(372, "3", automarkrecordid, noofinpat, pname, "无初步诊断");
        //                        break;
        //                    }
        //                }
        //            }
        //            //if (!arr_element.Contains("诊断依据"))
        //            //{
        //            //    m_SqlManger.InsertDB(373, "5", automarkrecordid, noofinpat, pname, "无诊断依据");
        //            //}
        //            //if (!arr_element.Contains("鉴别诊断"))
        //            //{
        //            //    m_SqlManger.InsertDB(374, "3", automarkrecordid, noofinpat, pname, "无鉴别诊断");
        //            //}
        //            //if (!arr_element.Contains("诊疗计划"))
        //            //{
        //            //    m_SqlManger.InsertDB(375, "2", automarkrecordid, noofinpat, pname, "无诊疗计划");
        //            //}
        //            //edit by wyt 2012-11-23
        //            if (!IsConatainStr(arr_element, "诊断依据"))
        //            {
        //                m_SqlManger.InsertDB(373, "5", automarkrecordid, noofinpat, pname, "无诊断依据");
        //            }
        //            if (!IsConatainStr(arr_element, "鉴别诊断"))
        //            {
        //                m_SqlManger.InsertDB(374, "3", automarkrecordid, noofinpat, pname, "无鉴别诊断");
        //            }
        //            if (!IsConatainStr(arr_element, "诊疗计划"))
        //            {
        //                m_SqlManger.InsertDB(375, "2", automarkrecordid, noofinpat, pname, "无诊疗计划");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        #endregion


        /// <summary>
        /// AC自动检测
        /// </summary>
        /// <param name="recorddetialid"></param>
        /// <param name="mr_class"></param>
        /// <returns></returns>
        public void GetResultPoint(string recorddetialid, string mr_class, string noofinpat, string automarkrecordid, string pname, string errordoctor)
        {
            try
            {
                int hours;
                DataTable dt = new DataTable();
                string sql;
                //先判断是否首程
                sql = string.Format(@"select firstdailyflag from recorddetail where id='{0}' ", recorddetialid);
                dt = m_App.SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows[0][0].ToString() == "1")
                {

                    XmlDocument xmlRecord = new XmlDocument();
                    //DataTable recordContent =
                    //    m_App.SqlHelper.ExecuteDataTable(
                    //    string.Format(@"select content from recorddetail where id='{0}'", recorddetialid), CommandType.Text);
                    //edit by wyt 增加病程文件有效性筛选 2012-11-23
                    DataTable recordContent =
                        m_App.SqlHelper.ExecuteDataTable(
                        string.Format(@"select content from recorddetail where id='{0}' and valid = 1", recorddetialid), CommandType.Text);
                    //如果有病历内容
                    if (recordContent == null || recordContent.Rows.Count <= 0)
                    {
                        return;
                    }
                    xmlRecord.LoadXml(recordContent.Rows[0]["content"].ToString());
                    XmlNode body = xmlRecord.SelectSingleNode("//body");

                    //DataTable PointConfig = new DataTable();
                    //PointConfig = m_SqlManger.GetPointConfig(mr_class);
                    ////存放要评分的项名称和规则
                    //Dictionary<string, string> m_pointconfig = new Dictionary<string, string>();
                    //if (PointConfig.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < PointConfig.Rows.Count; i++)
                    //    {
                    //        m_pointconfig.Add(PointConfig.Rows[i]["id"].ToString(), PointConfig.Rows[i]["childname"].ToString() + PointConfig.Rows[i]["problem_desc"].ToString());
                    //    }
                    //}
                    if (!IsHaveValue(body, "诊断") && !IsHaveValue(body, "诊断", 1, 0))
                    {
                        m_SqlManger.InsertDB(372, "3", automarkrecordid, noofinpat, pname, "无初步诊断", errordoctor);
                    }
                    if (!IsHaveValue(body, "诊断依据", 1, 0))
                    {
                        m_SqlManger.InsertDB(373, "5", automarkrecordid, noofinpat, pname, "无诊断依据", errordoctor);
                    }
                    if (!IsHaveValue(body, "诊断") && !IsHaveValue(body, "鉴别诊断", 1, 0))
                    {
                        m_SqlManger.InsertDB(374, "3", automarkrecordid, noofinpat, pname, "无鉴别诊断", errordoctor);
                    }
                    if (!IsHaveValue(body, "诊疗计划", 1, 0))
                    {
                        m_SqlManger.InsertDB(375, "2", automarkrecordid, noofinpat, pname, "无诊疗计划", errordoctor);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 判断病历文件某节点是否有值，并校验字长度范围
        /// </summary>
        /// add by wyt 2012-11-27
        /// <param name="body">病案文件xml主节点</param>
        /// <param name="item">子节点</param>
        /// <param name="minLen">最小长度</param>
        /// <param name="maxLen">最大长度</param>
        /// <returns>是否验证通过</returns>
        private bool IsHaveValue(XmlNode body, string item, int minLen, int maxLen)
        {
            try
            {
                int findedcount = 0;//定义变量记录查找到指定标签的存在链表中的索引值
                string nexttip = string.Empty;//记录要查找的标签的下一个标签
                bool IsChecked = false;//是否验证通过
                //查找整个Body部分的标签元素有多少个
                int tipcount = body.SelectNodes("//roelement").Count;
                string allstr = body.InnerText;

                for (int j = 0; j < tipcount; j++)
                {

                    if (body.SelectNodes("//roelement")[j].InnerText.ToString().Trim().Replace("　", "").Replace(" ", "").Trim(new char[] { '一', '二', '三', '四', '五', '六', '七', '八', '九' }).StartsWith(item))
                    {
                        findedcount = j;
                        string startstr = item;
                        int startop = allstr.Replace("　", "").Replace(" ", "").IndexOf(startstr, 0) + startstr.Length; //开始位置
                        string my = "";
                        if (findedcount == tipcount - 1)
                        {
                            my = allstr.Replace("　", "").Replace(" ", "").Substring(startop).Replace("：", ""); //取搜索的条数，用结束的位置-开始的位置,并返回 
                        }
                        else
                        {
                            nexttip = body.SelectNodes("//roelement")[j + 1].InnerText.ToString().Replace("　", "").Replace(" ", "");//下一个元素名称
                            int endop = allstr.Replace("　", "").Replace(" ", "").IndexOf(nexttip, startop); //结束位置
                            my = allstr.Replace("　", "").Replace(" ", "").Substring(startop, endop - startop).Replace("：", ""); //取搜索的条数，用结束的位置-开始的位置,并返回 
                        }
                        if (my.Length < minLen && minLen != 0)
                        {
                            IsChecked = false;
                            return IsChecked;
                        }
                        else if (my.Length > maxLen && maxLen != 0)
                        {
                            IsChecked = false;
                            return IsChecked;
                        }
                        else
                        {
                            IsChecked = true;
                            return IsChecked;
                        }
                    }
                }
                return IsChecked;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 判断病历文件某节点是否有值，并校验字长度范围，专用于诊断类节点校验
        /// </summary>
        /// add by wyt 2012-11-27
        /// <param name="body">病案文件xml主节点</param>
        /// <param name="item">子节点</param>
        /// <returns>是否验证通过</returns>
        private bool IsHaveValue(XmlNode body, string item)
        {
            try
            {
                int findedcount = 0;//定义变量记录查找到指定标签的存在链表中的索引值
                string nexttip = string.Empty;//记录要查找的标签的下一个标签
                bool IsChecked = false;//是否验证通过
                //查找整个Body部分的标签元素有多少个
                int tipcount = body.SelectNodes("//btnelement").Count;
                string allstr = body.InnerText;

                for (int j = 0; j < tipcount; j++)
                {

                    if (body.SelectNodes("//btnelement")[j].InnerText.ToString().Trim().Replace("　", "").Replace(" ", "").Trim(new char[] { '一', '二', '三', '四', '五', '六', '七', '八', '九' }).StartsWith(item))
                    {
                        findedcount = j;
                        string startstr = item;
                        int startop = allstr.Replace("　", "").Replace(" ", "").IndexOf(startstr, 0) + startstr.Length; //开始位置
                        string my = "";
                        if (findedcount == tipcount - 1)
                        {
                            my = allstr.Replace("　", "").Replace(" ", "").Substring(startop).Replace("：", ""); //取搜索的条数，用结束的位置-开始的位置,并返回 
                        }
                        else
                        {
                            nexttip = body.SelectNodes("//btnelement")[j + 1].InnerText.ToString().Replace("　", "").Replace(" ", "");//下一个元素名称
                            int endop = allstr.Replace("　", "").Replace(" ", "").IndexOf(nexttip, startop); //结束位置
                            my = allstr.Replace("　", "").Replace(" ", "").Substring(startop, endop - startop).Replace("：", ""); //取搜索的条数，用结束的位置-开始的位置,并返回 
                        }
                        if (my.Length > 0)
                        {
                            IsChecked = true;
                            return IsChecked;
                        }
                    }
                }
                return IsChecked;
            }
            catch
            {
                throw;
            }
        }

        ///// <summary>
        ///// AC自动检测
        ///// </summary>
        ///// add by wyt 2012-11-26
        ///// <param name="recorddetialid"></param>
        ///// <param name="mr_class"></param>
        ///// <returns></returns>
        //public void GetResultPoint(string recorddetialid, string mr_class, string noofinpat, string automarkrecordid, string pname)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        string sql;
        //        //先判断是否首程
        //        sql = string.Format(@"select firstdailyflag from recorddetail where id='{0}' ", recorddetialid);
        //        dt = m_App.SqlHelper.ExecuteDataTable(sql);
        //        if (dt.Rows[0][0].ToString() == "1")
        //        {
        //            //判断是否在入院8小时内完成首程
        //            //sql = @"select round(to_number(to_date(r.createtime,'yyyy-mm-dd hh24:mi:ss')-to_date(t.inwarddate,'yyyy-mm-dd hh24:mi:ss'))*24) hours from inpatient t ,recorddetail r where t.noofinpat='{0}' and r.noofinpat='{0}' and r.firstdailyflag='1' and r.sortid='AC'";
        //            //sql = string.Format(sql, noofinpat);
        //            //dt = m_App.SqlHelper.ExecuteDataTable(sql);
        //            //if (dt == null || dt.Rows.Count == 0)
        //            //{
        //            //    return;
        //            //}
        //            //hours = int.Parse(dt.Rows[0][0].ToString());
        //            //if (hours > 8)
        //            //{
        //            //    m_SqlManger.InsertDB(371, "25", automarkrecordid, noofinpat, pname, "未在入院后8小时内完成");
        //            //}

        //            XmlDocument xmlRecord = new XmlDocument();
        //            //DataTable recordContent =
        //            //    m_App.SqlHelper.ExecuteDataTable(
        //            //    string.Format(@"select content from recorddetail where id='{0}'", recorddetialid), CommandType.Text);
        //            //edit by wyt 增加病程文件有效性筛选 2012-11-23
        //            DataTable recordContent =
        //                m_App.SqlHelper.ExecuteDataTable(
        //                string.Format(@"select content from recorddetail where id='{0}' and valid = 1", recorddetialid), CommandType.Text);
        //            //如果有病历内容
        //            if (recordContent == null && recordContent.Rows.Count <= 0)
        //            {
        //                return;
        //            }
        //            xmlRecord.LoadXml(recordContent.Rows[0]["content"].ToString());
        //            XmlNode body = xmlRecord.SelectSingleNode("//body");
        //            //将取到的元素标签放到链表中，方便下面评分遍历
        //            ArrayList arr_element = new ArrayList();// 元素的名称
        //            int tipcount = body.SelectNodes("//macro").Count;
        //            for (int i = 0; i < tipcount; i++)
        //            {
        //                //判断节点内容不为空或者冒号（可能还有其他筛选...）
        //                if (body.SelectNodes("//macro")[i].InnerText.Trim() != "" &&
        //                    body.SelectNodes("//macro")[i].InnerText.Trim() != ":" &&
        //                    body.SelectNodes("//macro")[i].InnerText.Trim() != "：")
        //                {
        //                    if (body.SelectNodes("//span")[i].Attributes["name"] != null)
        //                    {
        //                        arr_element.Add(body.SelectNodes("//macro")[i].Attributes["name"].Value);//将此病历内容中的标签加入到链表
        //                    }
        //                }
        //            }
        //            tipcount = body.SelectNodes("//span").Count;
        //            for (int i = 0; i < tipcount; i++)
        //            {
        //                //判断节点内容不为空或者冒号（可能还有其他筛选...）
        //                if (body.SelectNodes("//span")[i].InnerText.Trim() != "" &&
        //                    body.SelectNodes("//span")[i].InnerText.Trim() != ":" &&
        //                    body.SelectNodes("//span")[i].InnerText.Trim() != "：")
        //                {
        //                    if (body.SelectNodes("//span")[i].Attributes["name"] != null)
        //                    {
        //                        arr_element.Add(body.SelectNodes("//span")[i].Attributes["name"].Value);//将此病历内容中的标签加入到链表
        //                    }
        //                }
        //            }
        //            tipcount = body.SelectNodes("//text").Count;
        //            for (int i = 0; i < tipcount; i++)
        //            {
        //                //判断节点内容不为空或者冒号（可能还有其他筛选...）
        //                if (body.SelectNodes("//text")[i].InnerText.Trim() != "" &&
        //                    body.SelectNodes("//text")[i].InnerText.Trim() != ":" &&
        //                    body.SelectNodes("//text")[i].InnerText.Trim() != "：")
        //                {
        //                    if (body.SelectNodes("//text")[i].Attributes["name"] != null)
        //                    {
        //                        arr_element.Add(body.SelectNodes("//text")[i].Attributes["name"].Value);//将此病历内容中的标签加入到链表
        //                    }
        //                }
        //            }
        //            tipcount = body.SelectNodes("//fnumelement").Count;
        //            for (int i = 0; i < tipcount; i++)
        //            {
        //                //判断节点内容不为空或者冒号（可能还有其他筛选...）
        //                if (body.SelectNodes("//fnumelement")[i].InnerText.Trim() != "" &&
        //                    body.SelectNodes("//fnumelement")[i].InnerText.Trim() != ":" &&
        //                    body.SelectNodes("//fnumelement")[i].InnerText.Trim() != "：")
        //                {
        //                    if (body.SelectNodes("//fnumelement")[i].Attributes["name"] != null)
        //                    {
        //                        arr_element.Add(body.SelectNodes("//fnumelement")[i].Attributes["name"].Value);//将此病历内容中的标签加入到链表
        //                    }
        //                }
        //            }
        //            //(可能还存在其他类型元素....)

        //            DataTable PointConfig = new DataTable();
        //            PointConfig = m_SqlManger.GetPointConfig(mr_class);
        //            //存放要评分的项名称和规则
        //            Dictionary<string, string> m_pointconfig = new Dictionary<string, string>();
        //            if (PointConfig.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < PointConfig.Rows.Count; i++)
        //                {
        //                    m_pointconfig.Add(PointConfig.Rows[i]["id"].ToString(), PointConfig.Rows[i]["childname"].ToString() + PointConfig.Rows[i]["problem_desc"].ToString());
        //                }
        //            }
        //            //edit by wyt 2012-11-23 //代码结构以后可根据配置更改，待修改... edit by wyt 2012-11-26
        //            if (!arr_element.Contains("诊断"))
        //            {
        //                m_SqlManger.InsertDB(372, "3", automarkrecordid, noofinpat, pname, "无初步诊断");
        //            }
        //            if (!arr_element.Contains("诊断依据"))
        //            {
        //                m_SqlManger.InsertDB(373, "5", automarkrecordid, noofinpat, pname, "无诊断依据");
        //            }
        //            if (!arr_element.Contains("鉴别诊断"))
        //            {
        //                m_SqlManger.InsertDB(374, "3", automarkrecordid, noofinpat, pname, "无鉴别诊断");
        //            }
        //            if (!arr_element.Contains("诊疗计划"))
        //            {
        //                m_SqlManger.InsertDB(375, "2", automarkrecordid, noofinpat, pname, "无诊疗计划");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        /// <summary>
        /// 判断list的项目中包含某子字符串
        /// </summary>
        /// <param name="al">list</param>
        /// <param name="str">子字符串</param>
        /// <returns></returns>
        private bool IsConatainStr(ArrayList al, string str)
        {
            if (al.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (string item in al)
                {
                    if (item.Contains(str))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //        public bool InsertDB(int configreductionid, string point, string automarkrecordid, string noofinpat, string pname, string configreductionname)
        //        {
        //            try
        //            {
        //                string sql = @"select  decode(max(id),null,0,max(id))+1 id from emr_automark_record_detail";
        //                int id = int.Parse(m_App.SqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString());

        //                sql = @"insert into emr_automark_record_detail(id,configreductionid,point,automarkrecordid,noofinpat,pname,configreductionname)
        //                                                        values({0},{1},'{2}','{3}','{4}','{5}','{6}')";
        //                sql = string.Format(sql, id, configreductionid, point, automarkrecordid, noofinpat, pname, configreductionname);

        //                m_App.SqlHelper.ExecuteNoneQuery(sql);
        //                return true;
        //            }
        //            catch (Exception)
        //            {
        //                return false;
        //                throw;
        //            }

        //        }


    }
}
