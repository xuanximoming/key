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

namespace DrectSoft.Emr.QcManager
{
    class OperationOfZhuyuanzhi
    {
        IEmrHost m_App;
        SqlManger m_SqlManger;

        public OperationOfZhuyuanzhi(IEmrHost app)
        {
            m_App = app;
            m_SqlManger = new SqlManger(app);
        }

        #region 作废 2012-11-27 by wyt
        ///// <summary>
        ///// AB自动检测
        ///// </summary>
        ///// <param name="recorddetialid"></param>
        ///// <param name="mr_class"></param>
        ///// <returns></returns>
        //public void GetResultPoint(string recorddetialid, string mr_class, string noofinpat, string automarkrecordid, string pname)
        //{
        //    try
        //    {
        //        XmlDocument xmlRecord = new XmlDocument();
        //        //DataTable recordContent =
        //        //    m_App.SqlHelper.ExecuteDataTable(
        //        //    string.Format(@"select content from recorddetail where id='{0}' and substr(name, 0, 4) = '入院记录' ", recorddetialid), CommandType.Text);
        //        //edit by wyt 增加住院志文件有效性筛选  2012-11-23
        //        DataTable recordContent =
        //            m_App.SqlHelper.ExecuteDataTable(
        //            string.Format(@"select content from recorddetail where id='{0}' and substr(name, 0, 4) = '入院记录' and valid = 1", recorddetialid), CommandType.Text);
        //        //如果有病历内容
        //        if (recordContent == null || recordContent.Rows.Count <= 0)
        //        {
        //            return;
        //        }
        //        xmlRecord.LoadXml(recordContent.Rows[0]["content"].ToString());
        //        XmlNode body = xmlRecord.SelectSingleNode("//body");
        //        int tipcount = body.SelectNodes("//roelement").Count;
        //        ArrayList arr_element = new ArrayList();// 元素的名称
        //        for (int i = 0; i < tipcount; i++)
        //        {
        //            arr_element.Add(body.SelectNodes("//roelement")[i].InnerText.Replace(" ", "").Replace("　", ""));//将此病历内容中的标签加入到链表
        //        }
        //        //将取到的元素标签放到链表中，方便下面评分遍历

        //        DataTable PointConfig = new DataTable();
        //        PointConfig = m_SqlManger.GetPointConfig(mr_class);
        //        //存放要评分的项名称和规则
        //        Dictionary<string, string> m_pointconfig = new Dictionary<string, string>();
        //        if (PointConfig.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < PointConfig.Rows.Count; i++)
        //            {
        //                m_pointconfig.Add(PointConfig.Rows[i]["id"].ToString(), PointConfig.Rows[i]["childname"].ToString() + PointConfig.Rows[i]["problem_desc"].ToString());
        //            }
        //        }
        //        //if (!arr_element.Contains("主诉"))
        //        //{
        //        //    m_SqlManger.InsertDB(351, "3", automarkrecordid, noofinpat, pname, "缺主诉");
        //        //}
        //        //edit by wyt 2012-11-23
        //        if (!IsConatainStr(arr_element, "主诉"))
        //        {
        //            m_SqlManger.InsertDB(351, "3", automarkrecordid, noofinpat, pname, "缺主诉");
        //        }
        //        //主诉字数
        //        else
        //        {
        //            string allstr = body.InnerText;
        //            for (int j = 0; j < tipcount; j++)
        //            {
        //                if (body.SelectNodes("//roelement")[j].InnerText.ToString().Trim().Replace("　", "").Replace(" ", "").StartsWith("主诉"))
        //                {
        //                    string nexttip = body.SelectNodes("//roelement")[j + 1].InnerText.ToString().Replace(":", "").Replace("　", "").Replace(" ", "");//下一个元素名称
        //                    string startstr = "主诉";
        //                    int startop = allstr.Replace("　", "").Replace(" ", "").IndexOf(startstr, 0) + startstr.Length; //开始位置
        //                    int endop = allstr.Replace("　", "").Replace(" ", "").IndexOf(nexttip, startop); //结束位置
        //                    string my = allstr.Replace("　", "").Replace(" ", "").Substring(startop, endop - startop).Replace("：", ""); //取搜索的条数，用结束的位置-开始的位置,并返回 
        //                    if (my.Length > 20)
        //                    {
        //                        m_SqlManger.InsertDB(352, "2", automarkrecordid, noofinpat, pname, "主诉超过20个字");
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        //if (!arr_element.Contains("现病史"))
        //        //{
        //        //    m_SqlManger.InsertDB(353, "5", automarkrecordid, noofinpat, pname, "缺现病史");
        //        //}
        //        //if (!arr_element.Contains("既往史"))
        //        //{
        //        //    m_SqlManger.InsertDB(354, "3", automarkrecordid, noofinpat, pname, "缺既往史");
        //        //}
        //        //if (!arr_element.Contains("个人史"))
        //        //{
        //        //    m_SqlManger.InsertDB(356, "2", automarkrecordid, noofinpat, pname, "缺个人史");
        //        //}
        //        //if (!arr_element.Contains("家族史"))
        //        //{
        //        //    m_SqlManger.InsertDB(358, "1", automarkrecordid, noofinpat, pname, "缺家族史");
        //        //}
        //        //if (!arr_element.Contains("体格检查"))
        //        //{
        //        //    m_SqlManger.InsertDB(363, "5", automarkrecordid, noofinpat, pname, "缺体格检查");
        //        //}
        //        //if (!arr_element.Contains("专科情况"))
        //        //{
        //        //    m_SqlManger.InsertDB(364, "2", automarkrecordid, noofinpat, pname, "缺专科情况");
        //        //}
        //        //if (!arr_element.Contains("辅助检查"))
        //        //{
        //        //    m_SqlManger.InsertDB(365, "1", automarkrecordid, noofinpat, pname, "辅助检查结果未记录");
        //        //}
        //        //if (!arr_element.Contains("病史小结"))
        //        //{
        //        //    m_SqlManger.InsertDB(366, "1", automarkrecordid, noofinpat, pname, "缺病史小结");
        //        //}
        //        //if (!arr_element.Contains("初步诊断"))
        //        //{
        //        //    m_SqlManger.InsertDB(367, "1", automarkrecordid, noofinpat, pname, "缺初步诊断");
        //        //}
        //        //edit by wyt 2012-11-23
        //        if (!IsConatainStr(arr_element, "现病史"))
        //        {
        //            m_SqlManger.InsertDB(353, "5", automarkrecordid, noofinpat, pname, "缺现病史");
        //        }
        //        if (!IsConatainStr(arr_element, "既往史"))
        //        {
        //            m_SqlManger.InsertDB(354, "3", automarkrecordid, noofinpat, pname, "缺既往史");
        //        }
        //        if (!IsConatainStr(arr_element, "个人史"))
        //        {
        //            m_SqlManger.InsertDB(356, "2", automarkrecordid, noofinpat, pname, "缺个人史");
        //        }
        //        if (!IsConatainStr(arr_element, "家族史"))
        //        {
        //            m_SqlManger.InsertDB(358, "1", automarkrecordid, noofinpat, pname, "缺家族史");
        //        }
        //        if (!IsConatainStr(arr_element, "体格检查"))
        //        {
        //            m_SqlManger.InsertDB(363, "5", automarkrecordid, noofinpat, pname, "缺体格检查");
        //        }
        //        if (!IsConatainStr(arr_element, "专科情况"))
        //        {
        //            m_SqlManger.InsertDB(364, "2", automarkrecordid, noofinpat, pname, "缺专科情况");
        //        }
        //        if (!IsConatainStr(arr_element, "辅助检查"))
        //        {
        //            m_SqlManger.InsertDB(365, "1", automarkrecordid, noofinpat, pname, "辅助检查结果未记录");
        //        }
        //        if (IsConatainStr(arr_element, "病史小结"))
        //        {
        //            m_SqlManger.InsertDB(366, "1", automarkrecordid, noofinpat, pname, "缺病史小结");
        //        }
        //        if (!IsConatainStr(arr_element, "初步诊断"))
        //        {
        //            m_SqlManger.InsertDB(367, "1", automarkrecordid, noofinpat, pname, "缺初步诊断");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        #endregion

        /// <summary>
        /// AB自动检测
        /// </summary>
        /// add by wyt 2012-11-27
        /// <param name="recorddetialid"></param>
        /// <param name="mr_class"></param>
        /// <returns></returns>
        public void GetResultPoint(string recorddetialid, string mr_class, string noofinpat, string automarkrecordid, string pname, string errordoctor)
        {
            try
            {
                XmlDocument xmlRecord = new XmlDocument();
                //DataTable recordContent =
                //    m_App.SqlHelper.ExecuteDataTable(
                //    string.Format(@"select content from recorddetail where id='{0}' and substr(name, 0, 4) = '入院记录' ", recorddetialid), CommandType.Text);
                //edit by wyt 增加住院志文件有效性筛选  2012-11-23
                DataTable recordContent =
                    m_App.SqlHelper.ExecuteDataTable(
                    string.Format(@"select content from recorddetail where id='{0}' and substr(name, 0, 4) = '入院记录' and valid = 1", recorddetialid), CommandType.Text);
                //如果有病历内容
                if (recordContent == null || recordContent.Rows.Count <= 0)
                {
                    return;
                }
                xmlRecord.LoadXml(recordContent.Rows[0]["content"].ToString());
                XmlNode body = xmlRecord.SelectSingleNode("//body");
                if (!IsHaveValue(body, "主诉", 1, 0))
                {
                    m_SqlManger.InsertDB(351, "3", automarkrecordid, noofinpat, pname, "缺主诉", errordoctor);
                }
                else if (!IsHaveValue(body, "主诉", 0, 20))
                {
                    {
                        m_SqlManger.InsertDB(352, "2", automarkrecordid, noofinpat, pname, "主诉超过20个字", errordoctor);
                    }
                }
                if (!IsHaveValue(body, "现病史", 1, 0))
                {
                    m_SqlManger.InsertDB(353, "5", automarkrecordid, noofinpat, pname, "缺现病史", errordoctor);
                }
                if (!IsHaveValue(body, "既往史", 1, 0))
                {
                    m_SqlManger.InsertDB(354, "3", automarkrecordid, noofinpat, pname, "缺既往史", errordoctor);
                }
                if (!IsHaveValue(body, "个人史", 1, 0))
                {
                    m_SqlManger.InsertDB(356, "2", automarkrecordid, noofinpat, pname, "缺个人史", errordoctor);
                }
                if (!IsHaveValue(body, "家族史", 1, 0))
                {
                    m_SqlManger.InsertDB(358, "1", automarkrecordid, noofinpat, pname, "缺家族史", errordoctor);
                }
                if (!IsHaveValue(body, "体格检查", 1, 0))
                {
                    m_SqlManger.InsertDB(363, "5", automarkrecordid, noofinpat, pname, "缺体格检查", errordoctor);
                }
                if (!IsHaveValue(body, "专科情况", 1, 0))
                {
                    m_SqlManger.InsertDB(364, "2", automarkrecordid, noofinpat, pname, "缺专科情况", errordoctor);
                }
                if (!IsHaveValue(body, "辅助检查", 1, 0))
                {
                    m_SqlManger.InsertDB(365, "1", automarkrecordid, noofinpat, pname, "辅助检查结果未记录", errordoctor);
                }
                if (!IsHaveValue(body, "病史小结", 1, 0))
                {
                    m_SqlManger.InsertDB(366, "1", automarkrecordid, noofinpat, pname, "缺病史小结", errordoctor);
                }
                if (!IsHaveValue(body, "初步诊断") && !IsHaveValue(body, "初步诊断", 1, 0))
                {
                    m_SqlManger.InsertDB(367, "1", automarkrecordid, noofinpat, pname, "缺初步诊断", errordoctor);
                }
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
        ///// AB自动检测
        ///// </summary>
        ///// add by wyt 2012-11-26
        ///// <param name="recorddetialid"></param>
        ///// <param name="mr_class"></param>
        ///// <returns></returns>
        //public void GetResultPoint(string recorddetialid, string mr_class, string noofinpat, string automarkrecordid, string pname)
        //{
        //    try
        //    {
        //        XmlDocument xmlRecord = new XmlDocument();
        //        //DataTable recordContent =
        //        //    m_App.SqlHelper.ExecuteDataTable(
        //        //    string.Format(@"select content from recorddetail where id='{0}' and substr(name, 0, 4) = '入院记录' ", recorddetialid), CommandType.Text);
        //        //edit by wyt 增加住院志文件有效性筛选  2012-11-23
        //        DataTable recordContent =
        //            m_App.SqlHelper.ExecuteDataTable(
        //            string.Format(@"select content from recorddetail where id='{0}' and substr(name, 0, 4) = '入院记录' and valid = 1", recorddetialid), CommandType.Text);
        //        //如果有病历内容
        //        if (recordContent == null || recordContent.Rows.Count <= 0)
        //        {
        //            return;
        //        }
        //        xmlRecord.LoadXml(recordContent.Rows[0]["content"].ToString());
        //        XmlNode body = xmlRecord.SelectSingleNode("//body");
        //        //将取到的元素标签放到链表中，方便下面评分遍历//判断主诉字数
        //        int num = 0;//主诉字数
        //        int tipcount = body.SelectNodes("//macro").Count;
        //        List<string> arr_element = new List<string>();// 元素的名称
        //        for (int i = 0; i < tipcount; i++)
        //        {
        //            //判断节点内容不为空或者冒号（可能还有其他筛选...）
        //            if (body.SelectNodes("//macro")[i].InnerText.Trim() != "" &&
        //                body.SelectNodes("//macro")[i].InnerText.Trim() != ":" &&
        //                body.SelectNodes("//macro")[i].InnerText.Trim() != "：")
        //            {
        //                if (body.SelectNodes("//macro")[i].Attributes["name"] != null)
        //                {
        //                    arr_element.Add(body.SelectNodes("//macro")[i].Attributes["name"].Value);//将此病历内容中的标签加入到链表
        //                    if (body.SelectNodes("//macro")[i].Attributes["name"].Value == "主诉")//累计主诉字数
        //                    {
        //                        num += body.SelectNodes("//macro")[i].InnerText.Length;
        //                    }
        //                }
        //            }
        //        }
        //        tipcount = body.SelectNodes("//span").Count;
        //        for (int i = 0; i < tipcount; i++)
        //        {
        //            //判断节点内容不为空或者冒号（可能还有其他筛选...）
        //            if (body.SelectNodes("//span")[i].InnerText.Trim() != ""&&
        //                body.SelectNodes("//span")[i].InnerText.Trim() != ":" &&
        //                body.SelectNodes("//span")[i].InnerText.Trim() != "：")
        //            {
        //                if (body.SelectNodes("//span")[i].Attributes["name"] != null)
        //                {
        //                    arr_element.Add(body.SelectNodes("//span")[i].Attributes["name"].Value);//将此病历内容中的标签加入到链表
        //                    if (body.SelectNodes("//span")[i].Attributes["name"].Value == "主诉")//累计主诉字数
        //                    {
        //                        num += body.SelectNodes("//span")[i].InnerText.Length;
        //                    }
        //                }
        //            }
        //        }
        //        tipcount = body.SelectNodes("//text").Count;
        //        for (int i = 0; i < tipcount; i++)
        //        {
        //            //判断节点内容不为空或者冒号（可能还有其他筛选...）
        //            if (body.SelectNodes("//text")[i].InnerText.Trim() != "" &&
        //                body.SelectNodes("//text")[i].InnerText.Trim() != ":" &&
        //                body.SelectNodes("//text")[i].InnerText.Trim() != "：")
        //            {
        //                if (body.SelectNodes("//fnumelement")[i].Attributes["name"] != null)
        //                {
        //                    arr_element.Add(body.SelectNodes("//text")[i].Attributes["name"].Value);//将此病历内容中的标签加入到链表
        //                    if (body.SelectNodes("//text")[i].Attributes["name"].Value == "主诉")//累计主诉字数
        //                    {
        //                        num += body.SelectNodes("//text")[i].InnerText.Length;
        //                    }
        //                }
        //            }
        //        }

        //        tipcount = body.SelectNodes("//fnumelement").Count;
        //        for (int i = 0; i < tipcount; i++)
        //        {
        //            //判断节点内容不为空或者冒号（可能还有其他筛选...）
        //            if (body.SelectNodes("//fnumelement")[i].InnerText.Trim() != "" &&
        //                body.SelectNodes("//fnumelement")[i].InnerText.Trim() != ":" &&
        //                body.SelectNodes("//fnumelement")[i].InnerText.Trim() != "：")
        //            {
        //                if (body.SelectNodes("//fnumelement")[i].Attributes["name"] != null)
        //                {
        //                    arr_element.Add(body.SelectNodes("//fnumelement")[i].Attributes["name"].Value);//将此病历内容中的标签加入到链表

        //                    if (body.SelectNodes("//fnumelement")[i].Attributes["name"].Value == "主诉")//累计主诉字数
        //                    {
        //                        num += body.SelectNodes("//fnumelement")[i].InnerText.Length;
        //                    }
        //                }
        //            }
        //        }
        //        //(可能还存在其他类型元素....)

        //        DataTable PointConfig = new DataTable();
        //        PointConfig = m_SqlManger.GetPointConfig(mr_class);
        //        //存放要评分的项名称和规则
        //        Dictionary<string, string> m_pointconfig = new Dictionary<string, string>();
        //        if (PointConfig.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < PointConfig.Rows.Count; i++)
        //            {
        //                m_pointconfig.Add(PointConfig.Rows[i]["id"].ToString(), PointConfig.Rows[i]["childname"].ToString() + PointConfig.Rows[i]["problem_desc"].ToString());
        //            }
        //        }
        //        //if (!arr_element.Contains("主诉"))
        //        //{
        //        //    m_SqlManger.InsertDB(351, "3", automarkrecordid, noofinpat, pname, "缺主诉");
        //        //}
        //        //edit by wyt 2012-11-23

        //        ////主诉字数
        //        //else
        //        //{
        //        //    string allstr = body.InnerText;
        //        //    for (int j = 0; j < tipcount; j++)
        //        //    {
        //        //        if (body.SelectNodes("//span"))
        //        //        {
        //        //            string nexttip = body.SelectNodes("//span")[j + 1].InnerText.ToString();//下一个元素名称
        //        //            string startstr = "主诉";
        //        //            int startop = allstr.Replace("　", "").Replace(" ", "").IndexOf(startstr, 0) + startstr.Length; //开始位置
        //        //            int endop = allstr.Replace("　", "").Replace(" ", "").IndexOf(nexttip, startop); //结束位置
        //        //            string my = allstr.Replace("　", "").Replace(" ", "").Substring(startop, endop - startop).Replace("：", ""); //取搜索的条数，用结束的位置-开始的位置,并返回 
        //        //            if (my.Length > 20)
        //        //            {
        //        //                m_SqlManger.InsertDB(352, "2", automarkrecordid, noofinpat, pname, "主诉超过20个字");
        //        //                break;
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        //if (!arr_element.Contains("现病史"))
        //        //{
        //        //    m_SqlManger.InsertDB(353, "5", automarkrecordid, noofinpat, pname, "缺现病史");
        //        //}
        //        //if (!arr_element.Contains("既往史"))
        //        //{
        //        //    m_SqlManger.InsertDB(354, "3", automarkrecordid, noofinpat, pname, "缺既往史");
        //        //}
        //        //if (!arr_element.Contains("个人史"))
        //        //{
        //        //    m_SqlManger.InsertDB(356, "2", automarkrecordid, noofinpat, pname, "缺个人史");
        //        //}
        //        //if (!arr_element.Contains("家族史"))
        //        //{
        //        //    m_SqlManger.InsertDB(358, "1", automarkrecordid, noofinpat, pname, "缺家族史");
        //        //}
        //        //if (!arr_element.Contains("体格检查"))
        //        //{
        //        //    m_SqlManger.InsertDB(363, "5", automarkrecordid, noofinpat, pname, "缺体格检查");
        //        //}
        //        //if (!arr_element.Contains("专科情况"))
        //        //{
        //        //    m_SqlManger.InsertDB(364, "2", automarkrecordid, noofinpat, pname, "缺专科情况");
        //        //}
        //        //if (!arr_element.Contains("辅助检查"))
        //        //{
        //        //    m_SqlManger.InsertDB(365, "1", automarkrecordid, noofinpat, pname, "辅助检查结果未记录");
        //        //}
        //        //if (!arr_element.Contains("病史小结"))
        //        //{
        //        //    m_SqlManger.InsertDB(366, "1", automarkrecordid, noofinpat, pname, "缺病史小结");
        //        //}
        //        //if (!arr_element.Contains("初步诊断"))
        //        //{
        //        //    m_SqlManger.InsertDB(367, "1", automarkrecordid, noofinpat, pname, "缺初步诊断");
        //        //}
        //        //edit by wyt 2012-11-23 //代码结构以后可根据配置更改，待修改... edit by wyt 2012-11-26
        //        if(!arr_element.Contains("主诉")) 
        //        {
        //            m_SqlManger.InsertDB(351, "3", automarkrecordid, noofinpat, pname, "缺主诉");
        //        }
        //        else
        //        {
        //            if (num > 20)
        //            {
        //                m_SqlManger.InsertDB(352, "2", automarkrecordid, noofinpat, pname, "主诉超过20个字");
        //            }
        //        }
        //        if (!arr_element.Contains("现病史"))
        //        {
        //            m_SqlManger.InsertDB(353, "5", automarkrecordid, noofinpat, pname, "缺现病史");
        //        }
        //        if (!arr_element.Contains("既往史")) 
        //        {
        //            m_SqlManger.InsertDB(354, "3", automarkrecordid, noofinpat, pname, "缺既往史");
        //        }
        //        if (!arr_element.Contains("个人史"))
        //        {
        //            m_SqlManger.InsertDB(356, "2", automarkrecordid, noofinpat, pname, "缺个人史");
        //        }
        //        if (!arr_element.Contains("家族史"))
        //        {
        //            m_SqlManger.InsertDB(358, "1", automarkrecordid, noofinpat, pname, "缺家族史");
        //        }
        //        if (!arr_element.Contains("体格检查"))
        //        {
        //            m_SqlManger.InsertDB(363, "5", automarkrecordid, noofinpat, pname, "缺体格检查");
        //        }
        //        if (!arr_element.Contains("专科情况"))
        //        {
        //            m_SqlManger.InsertDB(364, "2", automarkrecordid, noofinpat, pname, "缺专科情况");
        //        }
        //        if (!arr_element.Contains("辅助检查"))
        //        {
        //            m_SqlManger.InsertDB(365, "1", automarkrecordid, noofinpat, pname, "辅助检查结果未记录");
        //        }
        //        if (!arr_element.Contains("病史小结"))
        //        {
        //            m_SqlManger.InsertDB(366, "1", automarkrecordid, noofinpat, pname, "缺病史小结");
        //        }
        //        if (!arr_element.Contains("初步诊断"))
        //        {
        //            m_SqlManger.InsertDB(367, "1", automarkrecordid, noofinpat, pname, "缺初步诊断");
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

    }
}
