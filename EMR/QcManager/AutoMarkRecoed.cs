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

namespace DrectSoft.Emr.QcManager
{
    class AutoMarkRecoed
    {
        IEmrHost m_App;
        SqlManger m_SqlManger;

        public AutoMarkRecoed(IEmrHost app)
        {
            m_App = app;
            m_SqlManger = new SqlManger(app);
        }


        public void Automark(string automarkrecordid, string noofinpat, string pname, QCType type)
        {
            try
            {
                #region 病案首页自动评分 --add by wyt 2012-12-03
                EmrAutoScore mainpage = new EmrAutoScore(m_App);
                mainpage.AutoScoreAA(noofinpat, pname, automarkrecordid);
                #endregion
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                dt1 = GetTypeOneAutoMark();
                if (dt1 == null || dt1.Rows.Count == 0)
                {
                    return;
                }
                if (!GetOperationStatus(noofinpat))
                {
                    dt1 = ToDataTable(dt1.Select("parents <> 'AH'"));
                }
                dt1 = ToDataTable(dt1.Select("parents <> 'AA'"));//病案首页不再判断
                if (type == QCType.PART)
                {
                    string outHosRec = GetOutHosRecord();         //出院记录
                    if (outHosRec != "")
                    {
                        dt1 = ToDataTable(dt1.Select(string.Format(@"children <> '{0}'", outHosRec)));//如果环节质控不检查出院记录
                    }
                }

                if (dt1 == null)
                {
                    return;
                }

                foreach (DataRow dr1 in dt1.Rows)
                {

                    dt2 = IsHaveRecoed(noofinpat, dr1["selectcondition"].ToString());
                    if (dt2.Rows.Count == 0)
                    {
                        m_SqlManger.InsertDB(int.Parse(dr1["id"].ToString()), dr1["REDUCEPOINT"].ToString(), automarkrecordid, noofinpat, pname, dr1["PROBLEM_DESC"].ToString(), GetRESIDENT(noofinpat));

                    }
                    else
                    {
                        dt3 = GetDetailByType(dr1);
                        if (dt3.Rows.Count > 0)
                        {
                            //当为病程记录时，只需检索病程记录
                            if (dr1["PARENTS"].ToString() == "AC")
                            {
                                IsHaveRoelement((dt2.Select("FIRSTDAILYFLAG = '1'"))[0], dt3, automarkrecordid, noofinpat, pname);
                                continue;
                            }

                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                IsHaveRoelement(dr2, dt3, automarkrecordid, noofinpat, pname);
                            }
                        }
                    }
                }
                GetTimeLimitedQc(noofinpat, automarkrecordid, pname);//   时限质控
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取时限质控扣分
        /// </summary>
        public void GetTimeLimitedQc(string noofinpat, string automarkrecordid, string pname)
        {
            try
            {
                string tlqcget = string.Format(@"select * from qcrecord where VALID=1 and FOULSTATE='1' and NOOFINPAT='{0}'", noofinpat);
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(tlqcget);
                foreach (DataRow dr1 in dt.Rows)
                {
                    m_SqlManger.InsertDB(int.Parse(dr1["RULECODE"].ToString()), dr1["SCORE"].ToString(), automarkrecordid, noofinpat, pname, dr1["FOULMESSAGE"].ToString().Remove(0, 3), GetRESIDENT(noofinpat));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取出院记录代码
        /// edit 王冀 2012 12 18
        /// </summary>
        /// <returns></returns>
        public string GetOutHosRecord()
        {
            try
            {
                string sql = @"select t.children from emr_configreduction2 t
 where t.isauto = '0'
   and length(t.selectcondition) > 0
   and t.valid = '1' and t.parents ='AE' ";
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    return "";
                }
                return dt.Rows[0][0].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 数组DataRow[]转化成表DataTable
        /// 王冀 2012 12 5
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public DataTable ToDataTable(DataRow[] rows)
        {
            try
            {
                if (rows == null || rows.Length == 0) return null;
                DataTable tmp = rows[0].Table.Clone();
                foreach (DataRow row in rows)
                    tmp.ImportRow(row);
                return tmp;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取所有要评测的存在记录的项目
        /// </summary>
        /// <returns></returns>
        public DataTable GetTypeOneAutoMark()
        {
            try
            {
                string sql = @"select t.* from emr_configreduction2 t where t.isauto='0' and length(t.selectcondition) >0 and t.valid='1'";
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 判断手术部分是否启用
        /// </summary>
        /// <returns></returns>
        public Boolean GetOperationStatus(string noofinpat)
        {
            try
            {
                //string sql = @"select t.valide  from  iem_mainpage_qc t where t.fields='OPERATION_CODE'";
                DataTable dt = DataAccess.GetIemMainpageInfo(noofinpat, 2);
                bool IsOn = dt.Rows.Count == 0 ? false : true;
                return IsOn;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        /// <summary>
        /// 判断病人该记录是否存在 
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="keyword"></param>
        /// <returns>存在 返回ID 否则返回空</returns>
        public DataTable IsHaveRecoed(string noofinpat, string keyword)
        {
            try
            {
                string sql = string.Format(@"select t.id,t.content,t.owner,t.FIRSTDAILYFLAG  from recorddetail t where t.noofinpat = '{0}' and t.name like '{1}%' and t.valid = 1", noofinpat, keyword);
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 根据记录类型获取要判断节点的项目
        /// </summary>
        /// <returns></returns>
        public DataTable GetDetailByType(DataRow dr)
        {
            try
            {
                string children = dr["children"].ToString();
                string sql = string.Format(@"select * from emr_configreduction2 t where t.children='{0}'and length(t.selectcondition)>0 and t.isauto='1' and t.valid='1'", children);
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 对获取到的病历记录文件进行节点评测
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dt"></param>
        /// <param name="automarkrecordid"></param>
        /// <param name="noofinpat"></param>
        /// <param name="pname"></param>
        /// <returns></returns>
        public void IsHaveRoelement(DataRow dr, DataTable dt, string automarkrecordid, string noofinpat, string pname)
        {
            try
            {
                XmlDocument xmlRecord = new XmlDocument();
                string recorddetailID = dr["id"].ToString();
                string errordoctor = dr["owner"].ToString();
                xmlRecord.LoadXml(dr["content"].ToString());
                XmlNode body = xmlRecord.SelectSingleNode("//body");

                foreach (DataRow row in dt.Rows)
                {
                    string keyword = row["selectcondition"].ToString();
                    int detailid = int.Parse(row["id"].ToString());
                    string point = row["REDUCEPOINT"].ToString();
                    string detail = row["PROBLEM_DESC"].ToString();
                    if (keyword.Contains("诊断"))
                    {
                        //edit by wyt 2012-12-03 诊断类型用两种方法检查
                        //if (!IsHaveValue(body, keyword))
                        //{
                        //    m_SqlManger.InsertDB(detailid, point, automarkrecordid, noofinpat, pname, detail, errordoctor);
                        //}
                        if (!IsHaveValue(body, keyword) && !IsHaveValue(body, keyword, 1, 0))
                        {
                            m_SqlManger.InsertDB(detailid, point, automarkrecordid, noofinpat, pname, detail, errordoctor);
                        }
                    }
                    else
                    {
                        if (!IsHaveValue(body, keyword, 1, 0))
                        {
                            m_SqlManger.InsertDB(detailid, point, automarkrecordid, noofinpat, pname, detail, errordoctor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断病历文件某节点是否有值，并校验字长度范围
        /// </summary>
        /// <param name="body"></param>
        /// <param name="item"></param>
        /// <param name="minLen"></param>
        /// <param name="maxLen"></param>
        /// <returns></returns>
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
        /// 判断病历文件某节点是否有值，并校验字长度范围，专用于诊断类节点校验 节点类型不同
        /// </summary>
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


        /// <summary>
        /// 查找病人责任医生
        /// 王冀 2012 11 29
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public string GetRESIDENT(string noofinpat)
        {
            try
            {
                string sql = string.Format(@"select t.RESIDENT from INPATIENT t where t.noofinpat='{0}'", noofinpat);
                return m_App.SqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString();
            }
            catch (Exception)
            {
                return "";
                throw;
            }

        }
        /// <summary>
        /// 查找病历责任医生
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public string Getrecorddetailowner(string recordID)
        {
            try
            {
                string sql = string.Format(@"select t.owner from recorddetail t where t.ID='{0}'", recordID);
                return m_App.SqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString();
            }
            catch (Exception)
            {
                return "";
                throw;
            }

        }


    }
}
