using System.Linq;
using YidanSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using YidanSoft.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.Wordbook;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
//using YidanSoft.Core.TimeLimitQC;
using DevExpress.Utils;
using Yidansoft.Core.MainEmrPad;
using YindanSoft.Emr.Util;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Threading;
using System.Xml;

namespace YindanSoft.Emr.QcManagerNew
{
    class OperationOfFirstCheck
    {
        IYidanEmrHost m_App;
        SqlManger m_SqlManger;

        public OperationOfFirstCheck(IYidanEmrHost app)
        {
            m_App = app;
            m_SqlManger = new SqlManger(app);
        }

        /// <summary>
        /// 查找病人责任医生
        /// 王冀 2012 11 28
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

        /// <summary>
        /// 自动检测
        /// </summary>
        /// <param name="recorddetialid"></param>
        /// <param name="mr_class"></param>
        /// <returns></returns>
        public void GetResultPoint(string noofinpat, string pname, string id)
        {
            try
            {
                int hours;
                DataTable dt = new DataTable();
                string sql;
                string errordoctor = GetRESIDENT(noofinpat);
                //先判断有无病案首页
                //sql = string.Format(@"select t.iem_mainpage_no from IEM_MAINPAGE_BASICINFO_2012 t where t.noofinpat='{0}' ", noofinpat);
                //edit by wyt 增加有效性验证 2012-11-23
                sql = string.Format(@"select t.iem_mainpage_no from IEM_MAINPAGE_BASICINFO_2012 t where t.noofinpat='{0}' and t.valide = 1", noofinpat);
                dt = m_App.SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count < 1)
                {
                    //无病案首页

                    m_SqlManger.InsertDB(341, "5", id, noofinpat, pname, "无首页", errordoctor);
                }
                //出院（死亡）记录
                //                sql = string.Format(@"select nvl(round(to_number(to_date(r.createtime, 'yyyy-mm-dd hh24:mi:ss') - to_date(i.outhosdate, 'yyyy-mm-dd hh24:mi:ss')) * 24),-1) hours,
                //       r.name from inpatient i,recorddetail r where r.noofinpat = i.noofinpat  and r.noofinpat = '{0}' and (substr(r.name, 0, 4) = '出院记录' or substr(r.name, 0, 4) = '死亡记录') order by incount", noofinpat);
                //edit by wyt 增加有效性验证 edit by wyt 2012-11-23
                //sql = string.Format(@"select nvl(round(to_number(to_date(r.createtime, 'yyyy-mm-dd hh24:mi:ss') - to_date(i.outhosdate, 'yyyy-mm-dd hh24:mi:ss')) * 24),-1) hours,
                //r.name ,r.owner from inpatient i,recorddetail r where r.noofinpat = i.noofinpat  and r.noofinpat = '{0}' and (substr(r.name, 0, 4) = '出院记录' or substr(r.name, 0, 4) = '死亡记录') and r.valid = 1 order by incount", noofinpat);
                sql = string.Format(@"select  r.id from  recorddetail r where   r.noofinpat = '{0}' and (substr(r.name, 0, 4) = '出院记录' or substr(r.name, 0, 4) = '死亡记录') and r.valid = 1 ", noofinpat);
                dt = m_App.SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    sql = string.Format(@" select i.outhosdate  from inpatient i where i.noofinpat = '{0}' ", noofinpat);
                    dt = m_App.SqlHelper.ExecuteDataTable(sql);
                    if (dt.Rows[0][0].ToString().Trim() != "")
                    {
                        m_SqlManger.InsertDB(343, "10", id, noofinpat, pname, "缺患者出院（死亡）记录", errordoctor);
                    }
                }
                else
                {
                    //if (dt.Rows.Count > 0)
                    //{
                    //    if (int.Parse(dt.Rows[0][0].ToString()) > 24)
                    //    {
                    //        m_SqlManger.InsertDB(343, "10", id, noofinpat, pname, "患者出院（死亡）起24小时未完成", dt.Rows[0]["OWNER"].ToString());
                    //    }
                    //}
                }
                //入院记录
                //                sql = string.Format(@"select nvl(round(to_number(to_date(r.createtime, 'yyyy-mm-dd hh24:mi:ss') -
                //                       to_date(i.outhosdate, 'yyyy-mm-dd hh24:mi:ss')) * 24),-1) hours, r.name from inpatient i, recorddetail r 
                //                       where r.noofinpat = i.noofinpat and r.noofinpat = '{0}' and substr(r.name, 0, 4) = '入院记录'", noofinpat);
                //edit by wyt 增加有效性验证 2012-11-23
                //edit by wyt 修改入院时间绑定错误 2012-11-26 
                //                sql = string.Format(@"select nvl(round(to_number(to_date(r.createtime, 'yyyy-mm-dd hh24:mi:ss') -
                //                       to_date(i.outhosdate, 'yyyy-mm-dd hh24:mi:ss')) * 24),-1) hours, r.name from inpatient i, recorddetail r 
                //                       where r.noofinpat = i.noofinpat and r.noofinpat = '{0}' and substr(r.name, 0, 4) = '入院记录' and r.valid = 1", noofinpat);
                sql = string.Format(@"select t.id from recorddetail t where r.noofinpat = i.noofinpat and r.noofinpat = '{0}' and substr(r.name, 0, 4) = '入院记录' and r.valid = 1", noofinpat);
                dt = m_App.SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    //if (int.Parse(dt.Rows[0][0].ToString()) > 24)
                    //{
                    //    m_SqlManger.InsertDB(347, "25", id, noofinpat, pname, "入院记录未在患者入院后24小时内完成", dt.Rows[0]["OWNER"].ToString());
                    //}
                }
                if (dt.Rows.Count < 1)
                {
                    m_SqlManger.InsertDB(347, "25", id, noofinpat, pname, "缺入院记录", errordoctor);
                }
                //首次病程记录
                //sql = string.Format(@"select t.id from recorddetail t where  substr(t.name, 0, 4) = '首次病程' and t.FIRSTDAILYFLAG='1' and t.noofinpat= '{0}'", noofinpat);
                //edit by wyt 增加病程记录文件有效性验证 2012-11-23 
                sql = string.Format(@"select t.id from recorddetail t where  substr(t.name, 0, 4) = '首次病程' and t.FIRSTDAILYFLAG='1' and t.noofinpat= '{0}' and t.valid = 1", noofinpat);
                dt = m_App.SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count < 1)
                {
                    m_SqlManger.InsertDB(370, "25", id, noofinpat, pname, "缺首次病程记录", errordoctor);
                }
                //
                else
                {
                    //                    //                    sql = string.Format(@"select nvl(round(to_number(to_date(r.createtime, 'yyyy-mm-dd hh24:mi:ss') -
                    //                    //                       to_date(i.inwarddate, 'yyyy-mm-dd hh24:mi:ss')) * 24),-1) hours, r.name  from inpatient i, recorddetail r
                    //                    // where r.noofinpat = i.noofinpat    and r.noofinpat = '{0}' and substr(r.name, 0, 4) = '首次病程'", noofinpat);
                    //                    //edit by wyt 增加首次病程文件有效性验证 2012-11-23
                    //                    sql = string.Format(@"select nvl(round(to_number(to_date(r.createtime, 'yyyy-mm-dd hh24:mi:ss') -
                    //                       to_date(i.inwarddate, 'yyyy-mm-dd hh24:mi:ss')) * 24),-1) hours, r.name ,r.owner  from inpatient i, recorddetail r
                    // where r.noofinpat = i.noofinpat    and r.noofinpat = '{0}' and substr(r.name, 0, 4) = '首次病程' and r.valid = 1", noofinpat);
                    //                    dt = m_App.SqlHelper.ExecuteDataTable(sql);
                    //                    if (dt.Rows.Count > 0)
                    //                    {
                    //                        hours = int.Parse(dt.Rows[0][0].ToString());
                    //                        if (hours > 8)
                    //                        {
                    //                            m_SqlManger.InsertDB(371, "25", id, noofinpat, pname, "未在入院后8小时内完成", dt.Rows[0]["OWNER"].ToString());
                    //                        }
                    //                    }
                }

                #region 注释
                //                //上级首次查房
                //                //                sql = string.Format(@"select  round(to_number(to_date(t.createtime, 'yyyy-mm-dd hh24:mi:ss') -
                //                //                       to_date(r.inwarddate, 'yyyy-mm-dd hh24:mi:ss')) * 24) hours
                //                //                          from recorddetail t,inpatient r
                //                //                         where substr(t.name, 0, 4) = '上级查房'
                //                //                           and t.noofinpat = '{0}'
                //                //                          and r.noofinpat = '{0}'
                //                //                           and rownum = 1
                //                //                         order by t.createtime asc", noofinpat);
                //                //edit by wyt 增加上级查房文件有效性验证 2012-11-23
                //                sql = string.Format(@"select  round(to_number(to_date(t.createtime, 'yyyy-mm-dd hh24:mi:ss') -
                //                       to_date(r.inwarddate, 'yyyy-mm-dd hh24:mi:ss')) * 24) hours , t.owner
                //                      from recorddetail t,inpatient r
                //                     where substr(t.name, 0, 4) = '上级查房'
                //                       and t.noofinpat = '{0}'
                //                      and r.noofinpat = '{0}'
                //                      /*and (r.chief=t.owner or r.ATTEND=t.owner)*/
                //                      and t.valid = 1
                //                       and rownum = 1
                //                     order by t.createtime asc", noofinpat);
                //                dt = m_App.SqlHelper.ExecuteDataTable(sql);
                //                if (dt == null || dt.Rows.Count == 0)
                //                {
                //                    m_SqlManger.InsertDB(376, "10", id, noofinpat, pname, "未在患者入院后48小时内完成", errordoctor);
                //                }
                //                else
                //                {
                //                    hours = int.Parse(dt.Rows[0][0].ToString());
                //                    if (hours > 48)
                //                    {
                //                        m_SqlManger.InsertDB(376, "10", id, noofinpat, pname, "未在患者入院后48小时内完成", dt.Rows[0]["OWNER"].ToString());
                //                    }
                //                }
                //                //主任副主任医师首次查房
                //                //                sql = string.Format(@"select  round(to_number(to_date(t.createtime, 'yyyy-mm-dd hh24:mi:ss') -
                //                //                       to_date(r.inwarddate, 'yyyy-mm-dd hh24:mi:ss')) * 24) hours
                //                //                      from recorddetail t,inpatient r
                //                //                     where substr(t.name, 0, 4) = '上级查房'
                //                //                       and t.noofinpat = '{0}'
                //                //                       and r.noofinpat = '{0}'
                //                //                       and r.chief=t.owner
                //                //                       and rownum = 1
                //                //                     order by t.createtime asc", noofinpat);
                //                //edit by wyt 增加上级查房文件有效性验证 2012-11-23
                //                sql = string.Format(@"select  round(to_number(to_date(t.createtime, 'yyyy-mm-dd hh24:mi:ss') -
                //                       to_date(r.inwarddate, 'yyyy-mm-dd hh24:mi:ss')) * 24) hours,t.owner
                //                      from recorddetail t,inpatient r
                //                     where substr(t.name, 0, 4) = '上级查房'
                //                       and t.noofinpat = '{0}'
                //                       and r.noofinpat = '{0}'
                //                       and r.chief=t.owner
                //                       and t.valid = 1
                //                       and rownum = 1
                //                     order by t.createtime asc", noofinpat);
                //                dt = m_App.SqlHelper.ExecuteDataTable(sql);
                //                if (dt == null || dt.Rows.Count == 0)
                //                {
                //                    m_SqlManger.InsertDB(377, "3", id, noofinpat, pname, "主任、副主任医师未在72小时内查房", errordoctor);
                //                }
                //                else
                //                {
                //                    hours = int.Parse(dt.Rows[0][0].ToString());
                //                    if (hours > 72)
                //                    {
                //                        m_SqlManger.InsertDB(377, "3", id, noofinpat, pname, "主任、副主任医师未在72小时内查房", dt.Rows[0]["OWNER"].ToString());
                //                    }
                //                }


                //                //会诊申请
                //                //                sql = string.Format(@"select nvl(round(to_number(to_date(t.createtime, 'yyyy-mm-dd hh24:mi:ss') -
                //                //                       to_date(t.applytime, 'yyyy-mm-dd hh24:mi:ss')) * 24),-1) hours from consultapply t where t.noofinpat='{0}'", noofinpat);
                //                //edit by wyt 增加会诊申请记录有效性验证 2012-11-23
                //                sql = string.Format(@"select nvl(round(to_number(to_date(t.createtime, 'yyyy-mm-dd hh24:mi:ss') -
                //                       to_date(t.applytime, 'yyyy-mm-dd hh24:mi:ss')) * 24),-1) hours from consultapply t where t.noofinpat='{0}' and t.valid = 1", noofinpat);
                //                dt = m_App.SqlHelper.ExecuteDataTable(sql);
                //                if (dt.Rows.Count > 0)
                //                {
                //                    foreach (DataRow dr in dt.Rows)
                //                    {
                //                        if (Convert.ToInt32(dr[0]) > 48)
                //                        {
                //                            m_SqlManger.InsertDB(382, "2", id, noofinpat, pname, "在申请发出后48小时未完成一次", errordoctor);
                //                        }
                //                    }
                //                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
