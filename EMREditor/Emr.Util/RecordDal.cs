//页面大小的通用配置 Add By wwj 2012-03-31 【1】

using DrectSoft.Common;
using DrectSoft.Common.Eop;
using DrectSoft.Core;
using DrectSoft.DSSqlHelper;
using DrectSoft.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
#pragma warning disable 0618
namespace DrectSoft.Emr.Util
{
    public class RecordDal
    {

        #region  const model data fields
        const string col_flag = "flag";
        const string col_minstanceid = "indx";
        const string col_Index = "ID";

        const string col_mpatid = "NoOfInpat";
        const string col_mmodelid = "TEMPLATEID";
        const string col_fileName = "FileNAME";
        const string col_mName = "Name";
        const string col_mdescription = "RecordDesc";
        const string col_mcontent = "filecontent";
        const string col_content = "Content";
        const string col_mcatalog = "SortID";
        const string col_mjlzt = "Valid";
        const string col_mcjys = "Owner";
        const string col_mcjsj = "CreateTime";
        const string col_mtjzt = "HasSubmit";
        const string col_mdyzt = "HasPrint";
        const string col_mshys = "Auditor";
        const string col_mshsj = "AuditTime";
        const string col_dzqmbz = "HasSign";
        const string col_cdt = "CaptionDateTime";
        const string col_fdf = "FIRSTDAILYFLAG";
        const string col_IsShowFileName = "isshowfilename";
        const string s_ModelTableName = "RecordDetail";
        const string col_py = "PY";
        const string col_wb = "WB";
        const string col_yihuangoutong = "isyihuangoutong";
        const string col_IsReadConfigPageSize = "isconfigpagesize";//【1】


        string Sql_LoadModelInstance = String.Format(CultureInfo.CurrentCulture
          , @"select {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {14}, {15}, {16}, {17}, {18} 
              from {13} where {0} = @{0} order by {5},{15}"
          , new string[] { col_Index, col_mpatid, col_mmodelid, col_mName, col_content // 0~4
            , col_mcatalog, col_mjlzt, col_mtjzt, col_mdyzt, col_mcjys, col_mcjsj, col_mshys, col_mshsj // 5~12
            , s_ModelTableName, col_dzqmbz, col_cdt, col_fdf, col_yihuangoutong   // 13~17
            , col_IsReadConfigPageSize/*【1】*/});  // 18


        string Sql_UpdateModelInstance = String.Format(CultureInfo.CurrentCulture
         , " update {0}"
         + " set {3} = @{3}, {4} = @{4}, {5} = @{5}, {6} = @{6}, {7} = @{7}, {8} = @{8}, {9} = @{9}, {10} = @{10}, {11} = @{11}, {12} = @{12}"
         + ", {13} = @{13}"
         + " where {1} = @{1} and {2} = @{2}"
         , new string[] { s_ModelTableName, col_mpatid, col_minstanceid, col_mmodelid, col_fileName // 0~4
            , col_mdescription, col_mcontent, col_mcatalog, col_mjlzt, col_mtjzt, col_mdyzt , col_mshys, col_mshsj, col_dzqmbz}); // 5~14

        string Sql_UpdateModelPrintSigned = String.Format(CultureInfo.CurrentCulture
         , " update {0} set {1} = @{1} where {2} = @{2} "
         , new string[] { s_ModelTableName, col_mdyzt, col_minstanceid });

        /// <summary>
        /// 作废病历
        /// </summary>
        string Sql_UpdateModelValid = String.Format(CultureInfo.CurrentCulture
         , " update {0} set {1} = '0' where {2} = @{2} "
         , new string[] { s_ModelTableName, col_mjlzt, col_Index });

        string Sql_GetMaxFileNo = string.Format("select max(ID) from {0}", s_ModelTableName);
        string Sql_GetQCCode = "select QC_CODE from emrtemplet where TEMPLET_ID='{0}' ";

        #endregion

        IDataAccess sql_Helper;

        /// <summary>
        /// 编辑页面传递的病程XML
        /// </summary>
        private XmlDocument xmlRecord;
        /// <summary>
        /// 编辑器传递的病程内容
        /// </summary>
        private string editorEmrContent;
        /// <summary>
        /// 数据库中的病程内容
        /// </summary>
        private string dbEmrContent;

        public List<EmrModel> modelList;

        public RecordDal(IDataAccess sqlhelper)
        {
            try
            {
                sql_Helper = sqlhelper;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 病历文件SQL语句的参数
        /// </summary>
        private SqlParameter[] ModelInstanceParams
        {
            get
            {
                if (_modelInstanceParams == null)
                    _modelInstanceParams = GetEmrModelInstanceParameters();
                return _modelInstanceParams;
            }
        }
        private SqlParameter[] _modelInstanceParams;

        /// <summary>
        /// 设置参数, 注意Text类型用sqldbtype.ntext,不要用sqldbtype.text
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 设置参数, 注意Text类型用sqldbtype.ntext,不要用sqldbtype.text
        /// </summary>
        /// <returns></returns>
        private SqlParameter[] GetEmrModelInstanceParameters()
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                    new SqlParameter(col_flag,SqlDbType.Int,10),    
                    new SqlParameter(col_minstanceid,SqlDbType.Int,10),
                    new SqlParameter(col_mpatid, SqlDbType.Int, 32),
                    new SqlParameter(col_mmodelid, SqlDbType.VarChar, 64),
                    new SqlParameter(col_fileName,SqlDbType.VarChar,64),
                    //new SqlParameter(col_mdescription,SqlDbType.VarChar, 255),
                    new SqlParameter(col_mcontent,SqlDbType.Text, int.MaxValue),
                    new SqlParameter(col_mcatalog, SqlDbType.VarChar,12),
                    new SqlParameter(col_mcjys, SqlDbType.VarChar,6),
                    new SqlParameter(col_mcjsj, SqlDbType.VarChar,19)
                    ,new SqlParameter(col_mshys, SqlDbType.VarChar, 6)
                    ,new SqlParameter(col_mshsj, SqlDbType.VarChar, 19)
                    ,new SqlParameter(col_dzqmbz, SqlDbType.SmallInt,10)
                    ,new SqlParameter(col_mjlzt, SqlDbType.SmallInt)
                    ,new SqlParameter(col_mtjzt, SqlDbType.SmallInt)
                    ,new SqlParameter(col_mdyzt, SqlDbType.SmallInt)
                    ,new SqlParameter(col_cdt, SqlDbType.VarChar, 19)//设置的病程时间CaptionDateTime
                    ,new SqlParameter(col_fdf, SqlDbType.VarChar, 1)//首次病程标志位
                    ,new SqlParameter(col_py, SqlDbType.VarChar, 20)//拼音
                    ,new SqlParameter(col_wb, SqlDbType.VarChar, 20)//五笔
                    ,new SqlParameter(col_yihuangoutong, SqlDbType.VarChar, 1)
                    ,new SqlParameter(col_IsReadConfigPageSize,SqlDbType.VarChar, 1)//【1】
                    ,new SqlParameter("departcode",SqlDbType.VarChar, 50)
                    ,new SqlParameter("wardcode",SqlDbType.VarChar, 50)
                };
                return parms;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static string ZipEmrXml(string emrContent)
        {
            try
            {
                byte[] buffUnzipXml = Encoding.UTF8.GetBytes(emrContent);
                MemoryStream ms = new MemoryStream();
                GZipStream dfs = new GZipStream(ms, CompressionMode.Compress, true);
                dfs.Write(buffUnzipXml, 0, buffUnzipXml.Length);
                dfs.Close();
                ms.Seek(0, SeekOrigin.Begin);
                byte[] buffZipXml = new byte[ms.Length];
                ms.Read(buffZipXml, 0, buffZipXml.Length);
                return Convert.ToBase64String(buffZipXml);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string UnzipEmrXml(string emrContent)
        {
            try
            {
                byte[] rbuff = Convert.FromBase64String(emrContent);
                MemoryStream ms = new MemoryStream(rbuff);
                GZipStream dfs = new GZipStream(ms, CompressionMode.Decompress, true);
                StreamReader sr = new StreamReader(dfs, Encoding.UTF8);
                string sXml = sr.ReadToEnd();
                sr.Close();
                dfs.Close();
                return sXml;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
                return emrContent;
            }
        }

        //作废
        //private void UpdateEmrContent(string emrContent, string indx)
        //{
        //    if (emrContent == "") return;
        //    OracleLob lob;
        //    OracleTransaction txn = null;
        //    OracleConnection conn = null;
        //    OracleCommand cmd = null;
        //    OracleDataReader dr = null;
        //    string strSql = string.Empty;
        //    string content = string.Empty;
        //    Exception exception = null;
        //    try
        //    {
        //        conn = new OracleConnection(sql_Helper.GetDbConnection().ConnectionString);
        //        conn.Open();
        //        txn = conn.BeginTransaction();
        //        cmd = new OracleCommand(strSql, conn, txn);

        //        try
        //        {
        //            //用于在服务器上保存病历的副本 Add By wwj 2012-01-10
        //            SaveEmrFile(emrContent, indx);

        //            //保存修改主机的IP Add By wwj 2012-01-10
        //            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
        //            if (IpEntry.AddressList.Length > 0)
        //            {
        //                IPAddress ipAddr = IpEntry.AddressList[0];
        //                sql_Helper.ExecuteNoneQuery("update recorddetail set ip ='" + ipAddr.ToString() + "' where id=" + indx);
        //            }
        //        }
        //        catch (Exception ex)
        //        { }

        //        cmd.CommandText = "SELECT content FROM recorddetail where ID=" + indx + " FOR UPDATE";
        //        dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            lob = dr.GetOracleLob(0);
        //            lob.Erase();
        //            if (lob != OracleLob.Null)
        //            {
        //                //content = lob.Value.ToString();
        //                content = emrContent;
        //                byte[] buffer = System.Text.Encoding.Unicode.GetBytes(content);
        //                lob.Write(buffer, 0, buffer.Length);
        //            }
        //        }
        //        txn.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        txn.Rollback();
        //        exception = ex;
        //    }
        //    finally
        //    {
        //        dr.Close();
        //        conn.Close();
        //        cmd.Dispose();
        //    }

        //    if (exception != null)
        //    {
        //        throw exception;
        //    }
        //}

        delegate void SaveEmrFileDelegate(string emrContent, string indx);
        private void SaveEmrFileMethod(string emrContent, string indx)
        {
            try
            {
                //【1.1】在服务器上保存病历的副本
                SaveEmrFile(emrContent, indx);

                //【1.2】保存修改主机的IP
                IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
                if (IpEntry.AddressList.Length > 0)
                {
                    IPAddress ipAddr = IpEntry.AddressList[0];
                    sql_Helper.ExecuteNoneQuery("update recorddetail set ip ='" + ipAddr.ToString() + "' where id=" + indx);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存病历 
        /// </summary>
        /// <param name="emrContent"></param>
        /// <param name="indx"></param>
        /// <param name="patid">病人首页序号</param>
        /// <param name="ModelCatalog">标识，AC为病程 </param>
        private void UpdateEmrContent(string emrContent, string indx, string patid, string ModelCatalog)
        {
            try
            {
                UpdateEmrContent2(emrContent, indx, patid, ModelCatalog);

                #region editby cyq 2012-11-19 处理病程保存时的并发问题
                ////【1】以文件的形式保存病历副本
                //(new SaveEmrFileDelegate(SaveEmrFileMethod)).BeginInvoke(emrContent, indx, null, null);

                ////【2】将病历数据保存到数据库中
                //OracleConnection conn = new OracleConnection(sql_Helper.GetDbConnection().ConnectionString);
                //OracleCommand cmd = conn.CreateCommand();
                //try
                //{
                //    //处理emrContent的xml文件(处理病程) add by ywk 
                //    if (ModelCatalog == "AC")//对于是病程的病历文件的保存进行判断处理
                //    {
                //        XmlDocument xmlRecord = new XmlDocument();
                //        xmlRecord.LoadXml(emrContent);
                //        int RecordCount = GetRecordData(patid).Rows.Count;//从数据库得到关于病程的有效记录是几条

                //        XmlNode body = xmlRecord.SelectSingleNode("//body");
                //        //XmlDocument m_xmlContent = new XmlDocument();//存储处于body范围内的内容
                //        //string m_Content = body.InnerXml;
                //        ArrayList alRecordTime = new ArrayList();
                //        XmlNodeList inbody = body.ChildNodes;
                //        foreach (XmlNode p in inbody)
                //        {
                //            XmlNodeList inp = p.ChildNodes;
                //            foreach (XmlNode node in inp)
                //            {
                //                //string str = node.Attributes.["name"].ToString();
                //                foreach (XmlNode n in node.Attributes)
                //                {
                //                    if (n.Value == "记录日期")
                //                    {
                //                        alRecordTime.Add(node.InnerText);
                //                        break;
                //                    }
                //                }
                //            }
                //        }

                //        //XmlDocument myXmlRecord = xmlRecord;//声明一个自己的XML文档
                //        ////XmlNode mBodyNode=xmlRecord.SelectSingleNode(
                //        //xmlRecord.InnerXml.Replace(m_Content, "");
                //        //XmlNode myNode = m_xmlNode;//声明一个自己的节点保存，以上的NODE值 
                //        //xmlRecord.RemoveChild(m_xmlNode);
                //        //int findcount = 0;//查找记录日期出现的次数
                //        //ArrayList findedtime = new ArrayList();//用来记录查找到的记录日期 
                //        //for (int i = 0; (i = m_Content.IndexOf("记录日期", i)) >= 0; i = i + "记录日期".Length)
                //        //{
                //        //    findcount++;
                //        //    findedtime.Add(m_Content.Substring((i = m_Content.IndexOf("记录日期", i)) + 6, 19));
                //        //}
                //        //if (RecordCount == findcount)//无增加修改操作 
                //        //{
                //        //    conn.Open();
                //        //    cmd.CommandText = "update recorddetail set content = :data where id =" + indx;
                //        //    cmd.CommandType = CommandType.Text;
                //        //    cmd.Parameters.Clear();
                //        //    OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
                //        //    paraClob.Value = emrContent;
                //        //    cmd.Parameters.Add(paraClob);
                //        //    cmd.ExecuteNonQuery();
                //        //}
                //        //else
                //        //{
                //        //通过日期对比增加的内容 (数据库中的captiondatetime)
                //        DataTable dt = GetRecordData(patid);
                //        ArrayList alCapTime = new ArrayList();//用来存储各个病程时间点的链表
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            alCapTime.Add(dt.Rows[i]["CAPTIONDATETIME"].ToString());
                //        }
                //        //通过arrCapTime中的时间,在病程内容文件中进行查找相应的节点
                //        string differentStr = string.Empty;//用来保存病程中不一致的内容
                //        ArrayList differentTime = new ArrayList();//用来保存不一致的时间链表
                //        //以此链表arrCapTime(数据库)为主，查找findedtime(要保存的病程内容)中的time，不相同的保存到differentTime里
                //        foreach (string capTime in alCapTime)//arrCapTime数据库中的  
                //        {
                //            if (alRecordTime.Contains(capTime))
                //            { }
                //            else
                //            {
                //                //RemoveRecordData(capTime);//删除XML中不存在的时间点的记录
                //            }
                //        }

                //        //XmlNodeList xnl = xmlRecord.SelectNodes("//body/p [@align=0]/text [@name='记录日期']");
                //        //先把原来的body节点内容先移除
                //        //xmlRecord.RemoveChild(m_xmlNode);
                //        //xmlRecord.RemoveAll();
                //        //然后再把处理完成的加到上面

                //        //for (int i = 0; i < differentTime.Count; i++)
                //        //{
                //        //    for (int j = 0; j < xnl.Count; j++)
                //        //    {
                //        //        if (xnl[j].InnerText.ToString().Trim() == differentTime[i].ToString().Trim())
                //        //        {
                //        //m_xmlNode.RemoveChild(xnl[3].ParentNode.NextSibling);//删除下一级别
                //        //m_xmlNode.RemoveChild(xnl[3].ParentNode);//从body级别移除
                //        //        }
                //        //    }
                //        //}
                //        //xmlRecord.AppendChild(m_xmlNode);
                //        //string myContent = xmlRecord.InnerXml;//处理后的病程内容
                //        //conn.Open();
                //        //cmd.CommandText = "update recorddetail set content = :data where id =" + indx;
                //        //cmd.CommandType = CommandType.Text;
                //        //cmd.Parameters.Clear();
                //        //OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
                //        //paraClob.Value = emrContent;
                //        //cmd.Parameters.Add(paraClob);
                //        //cmd.ExecuteNonQuery();
                //        //}
                //    }

                //    //else
                //    //{
                //    conn.Open();
                //    cmd.CommandText = "update recorddetail set content = :data where id =" + indx;
                //    cmd.CommandType = CommandType.Text;
                //    cmd.Parameters.Clear();
                //    OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
                //    paraClob.Value = emrContent;
                //    cmd.Parameters.Add(paraClob);
                //    cmd.ExecuteNonQuery();

                //    //}
                //    //conn.Open();
                //    //cmd.CommandText = "update recorddetail set content = :data where id =" + indx;
                //    //cmd.CommandType = CommandType.Text;
                //    //cmd.Parameters.Clear();
                //    //OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
                //    //paraClob.Value = emrContent;
                //    //cmd.Parameters.Add(paraClob);
                //    //cmd.ExecuteNonQuery();
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
                //finally
                //{
                //    cmd.Dispose();
                //    conn.Close();
                //    conn.Dispose();
                //}
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存病历 --- 处理并发问题
        /// 1、先查找数据库中当前存在的节点数
        /// 2、一次遍历这些节点，找出数据库中对应的病历内容
        /// 3、如果与所编辑的病程存在此节点但内容不一致，则以编辑的此节点病历替换数据库的此节点病历
        /// 4、如果所编辑病程不存在此节点，则从数据库中读取此节点及对应病历，添加至新病历
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-19</date>
        /// <param name="emrContent">所编辑的病程</param>
        /// <param name="indx">首次病程ID</param>
        /// <param name="patid">病人首页序号</param>
        /// <param name="ModelCatalog">标识，AC为病程 </param>
        private void UpdateEmrContent2(string emrContent, string indx, string patid, string ModelCatalog)
        {
            #region 已注释 by cyq 2013-01-30 改用bwj封装的底层库
            /**
            //【1】以文件的形式保存病历副本
            (new SaveEmrFileDelegate(SaveEmrFileMethod)).BeginInvoke(emrContent, indx, null, null);
            //【2】将病历数据保存到数据库中
            OracleConnection conn = new OracleConnection(sql_Helper.GetDbConnection().ConnectionString);
            OracleCommand cmd = conn.CreateCommand();

            try
            {
                if (null == indx || string.IsNullOrEmpty(indx.Trim()) || !Tool.IsInt(indx.Trim()))
                {
                    throw new Exception("首程不存在或已经被删除，请联系管理员");
                }
                int theFirstRecordID = int.Parse(indx.Trim());
                //编辑记录
                string newEmrContent = string.Empty;
                //设置编辑器传递的病程内容
                editorEmrContent = emrContent;
                if (ModelCatalog == "AC")
                {
                    //处理emrContent的xml文件(处理病程) add by ywk 
                    xmlRecord = new XmlDocument();
                    xmlRecord.PreserveWhitespace = true;
                    xmlRecord.LoadXml(emrContent);
                    xmlRecord = AddNodesToXmlDocument(xmlRecord);
                    //数据库中实时记录
                    //DataTable realRecordDt = GetRecordData(patid); //从数据库得到关于病程的有效记录数据集
                    string dbContent = GetFirstRecord(indx);
                    //设置数据库中的病程内容
                    dbEmrContent = dbContent;
                    XmlDocument dbXmlRecord = new XmlDocument();
                    dbXmlRecord.PreserveWhitespace = true;
                    if (!string.IsNullOrEmpty(dbContent.Trim()))
                    {
                        dbXmlRecord.LoadXml(dbContent);
                        dbXmlRecord = AddNodesToXmlDocument(dbXmlRecord);
                        //List<DataRow> dbRecords = GetThisPageRecords(realRecordDt, indx);//realRecordDt.AsEnumerable().Where(q => null != q["captiondatetime"] && q["captiondatetime"].ToString().Trim() != "").OrderBy(p => DateTime.Parse(p["captiondatetime"].ToString())).ToList();
                        List<DataRow> dbRecords = GetThisPageRecords(theFirstRecordID, xmlRecord);
                        newEmrContent = GetEmrContents(dbXmlRecord, dbRecords, patid);
                        if (string.IsNullOrEmpty(newEmrContent.Trim()))
                        {
                            newEmrContent = emrContent;
                        }
                        else if (newEmrContent.Contains("<p align=\"0\"></body></document>"))
                        {
                            newEmrContent = newEmrContent.Replace("<p align=\"0\"></body></document>", "</body></document>");
                        }
                    }
                    else
                    {
                        newEmrContent = emrContent;
                    }
                }
                else
                {
                    newEmrContent = emrContent;
                }

                conn.Open();
                cmd.CommandText = "update recorddetail set content = :data where id =" + theFirstRecordID;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
                paraClob.Value = newEmrContent;
                cmd.Parameters.Add(paraClob);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
            **/
            #endregion

            //【1】以文件的形式保存病历副本
            (new SaveEmrFileDelegate(SaveEmrFileMethod)).BeginInvoke(emrContent, indx, null, null);
            //【2】将病历数据保存到数据库中

            try
            {
                if (null == indx || string.IsNullOrEmpty(indx.Trim()) || !Tool.IsInt(indx.Trim()))
                {
                    throw new Exception("首程不存在或已经被删除，请联系管理员");
                }
                int theFirstRecordID = int.Parse(indx.Trim());
                //编辑记录
                string newEmrContent = string.Empty;
                //设置编辑器传递的病程内容
                editorEmrContent = emrContent;
                if (ModelCatalog == "AC")
                {
                    //处理emrContent的xml文件(处理病程) add by ywk 
                    xmlRecord = new XmlDocument();
                    xmlRecord.PreserveWhitespace = true;
                    xmlRecord.LoadXml(emrContent);
                    xmlRecord = AddNodesToXmlDocument(xmlRecord);
                    //数据库中实时记录
                    string dbContent = GetFirstRecord(indx);
                    //设置数据库中的病程内容
                    dbEmrContent = dbContent;
                    XmlDocument dbXmlRecord = new XmlDocument();
                    dbXmlRecord.PreserveWhitespace = true;
                    if (!string.IsNullOrEmpty(dbContent.Trim()))
                    {
                        dbXmlRecord.LoadXml(dbContent);
                        dbXmlRecord = AddNodesToXmlDocument(dbXmlRecord);
                        List<DataRow> dbRecords = GetThisPageRecords(theFirstRecordID, xmlRecord);
                        newEmrContent = GetEmrContents(dbXmlRecord, dbRecords, patid);
                        if (string.IsNullOrEmpty(newEmrContent.Trim()))
                        {
                            newEmrContent = emrContent;
                        }
                        else if (newEmrContent.Contains("<p align=\"0\"></body></document>"))
                        {
                            newEmrContent = newEmrContent.Replace("<p align=\"0\"></body></document>", "</body></document>");
                        }
                        CheckRecordException(newEmrContent, int.Parse(indx), 0, false);
                    }
                    else
                    {
                        newEmrContent = emrContent;
                        CheckRecordException(newEmrContent, int.Parse(indx), 1, false);
                    }
                }
                else
                {
                    newEmrContent = emrContent;
                    CheckRecordException(newEmrContent, int.Parse(indx), 2, false);
                }

                OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
                paraClob.Value = newEmrContent;
                DS_SqlHelper.ExecuteNonQuery("update recorddetail set content = :data where id =" + theFirstRecordID, new OracleParameter[] { paraClob }, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存病历 --- 处理并发问题
        /// 1、先查找数据库中当前存在的节点数
        /// 2、一次遍历这些节点，找出数据库中对应的病历内容
        /// 3、如果与所编辑的病程存在此节点但内容不一致，则以编辑的此节点病历替换数据库的此节点病历
        /// 4、如果所编辑病程不存在此节点，则从数据库中读取此节点及对应病历，添加至新病历
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-11</date>
        /// <param name="emrContent">所编辑的病程</param>
        /// <param name="indx">首次病程ID</param>
        /// <param name="patid">病人首页序号</param>
        /// <param name="ModelCatalog">标识，AC为病程 </param>
        private void UpdateEmrContent2InTran(string emrContent, string indx, string patid, string ModelCatalog)
        {
            //【1】以文件的形式保存病历副本
            (new SaveEmrFileDelegate(SaveEmrFileMethod)).BeginInvoke(emrContent, indx, null, null);
            //【2】将病历数据保存到数据库中

            try
            {
                if (null == indx || string.IsNullOrEmpty(indx.Trim()) || !Tool.IsInt(indx.Trim()))
                {
                    throw new Exception("首程不存在或已经被删除，请联系管理员");
                }
                int theFirstRecordID = int.Parse(indx.Trim());
                //编辑记录
                string newEmrContent = string.Empty;
                //设置编辑器传递的病程内容
                editorEmrContent = emrContent;
                if (ModelCatalog == "AC")
                {
                    //处理emrContent的xml文件(处理病程) add by ywk 
                    xmlRecord = new XmlDocument();
                    xmlRecord.PreserveWhitespace = true;
                    xmlRecord.LoadXml(emrContent);
                    xmlRecord = AddNodesToXmlDocument(xmlRecord);
                    //数据库中实时记录
                    string dbContent = GetFirstRecordInTran(indx);
                    //设置数据库中的病程内容
                    dbEmrContent = dbContent;
                    XmlDocument dbXmlRecord = new XmlDocument();
                    dbXmlRecord.PreserveWhitespace = true;
                    if (!string.IsNullOrEmpty(dbContent.Trim()))
                    {
                        dbXmlRecord.LoadXml(dbContent);
                        dbXmlRecord = AddNodesToXmlDocument(dbXmlRecord);
                        List<DataRow> dbRecords = GetThisPageRecordsInTran(theFirstRecordID, xmlRecord);
                        newEmrContent = GetEmrContentsInTran(dbXmlRecord, dbRecords, patid);
                        if (string.IsNullOrEmpty(newEmrContent.Trim()))
                        {
                            newEmrContent = emrContent;
                        }
                        else if (newEmrContent.Contains("<p align=\"0\"></body></document>"))
                        {
                            newEmrContent = newEmrContent.Replace("<p align=\"0\"></body></document>", "</body></document>");
                        }
                        CheckRecordException(newEmrContent, int.Parse(indx), 0, true);
                    }
                    else
                    {
                        newEmrContent = emrContent;
                        CheckRecordException(newEmrContent, int.Parse(indx), 1, true);
                    }
                }
                else
                {
                    newEmrContent = emrContent;
                    CheckRecordException(newEmrContent, int.Parse(indx), 2, true);
                }

                OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
                paraClob.Value = newEmrContent;
                DS_SqlHelper.ExecuteNonQueryInTran("update recorddetail set content = :data where id =" + theFirstRecordID, new OracleParameter[] { paraClob }, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //cmd.Dispose();
                //conn.Close();
                //conn.Dispose();
            }
        }

        /// <summary>
        /// 获取合并后病程
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-19</date>
        /// <param name="dbXmlRecord">数据库中病程XML</param>
        /// <param name="xmlRecord">页面病程XML---已改为全局变量</param>
        /// <param name="dbRecords">数据库中时间记录</param>
        /// <param name="modelList">全局变量(页面传递病历节点集合)</param>
        /// <param name="noofinpat">住院号</param>
        /// <returns></returns>
        private string GetEmrContents(XmlDocument dbXmlRecord, List<DataRow> dbRecords, string noofinpat)
        {
            #region 已注释 by cyq 2012-01-14
            /**
            try
            {
                string newEmrContent = string.Empty;
                if (null != dbRecords && dbRecords.Count() > 0)
                {
                    //数据库中病程上病历对应时间集合
                    List<string> dbTimeList = DS_BaseService.GetEditEmrContentTimes(dbXmlRecord);
                    //编辑页面病程上病历对应时间集合
                    List<string> timeList = modelList.OrderBy(q => q.DisplayTime).Select(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss")).ToList();

                    //查找首次病程节点
                    DataRow firstDaylyRow = dbRecords.FirstOrDefault(p => null != p["firstdailyflag"] && p["firstdailyflag"].ToString().Trim() == "1");
                    if (null == firstDaylyRow)
                    {
                        return string.Empty;
                    }
                    if (null == firstDaylyRow["captiondatetime"])
                    {
                        throw new Exception("首次病程时间为空，请联系管理员。");
                    }
                    string theTimeFirst = DateTime.Parse(firstDaylyRow["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    //添加首次病程之前所有内容
                    newEmrContent = GetContentBeforeFirstRecord(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTimeFirst);
                    if (string.IsNullOrEmpty(newEmrContent.Trim()))
                    {
                        UpdateRecordValid(int.Parse(firstDaylyRow["ID"].ToString()), false);//edit by ywk 2013年1月6日14:17:15 
                    }
                    //添加首次病程内容 edit by cyq 2012-12-18 修复首次病程前可能存在节点的bug
                    bool existTheTime = modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTimeFirst);
                    string contentFirst = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTimeFirst, !existTheTime, 0);
                    if (!string.IsNullOrEmpty(contentFirst.Trim()))
                    {
                        //若节点存在病历
                        newEmrContent += contentFirst;
                    }
                    else
                    {
                        //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                        UpdateRecordValid(int.Parse(firstDaylyRow["ID"].ToString()), false);
                    }
                    //遍历数据库中病历节点集合
                    for (int i = 0; i < dbRecords.Count(); i++)
                    {
                        string theTime = DateTime.Parse(dbRecords[i]["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        if (dbRecords[i]["ID"].ToString() == firstDaylyRow["ID"].ToString())
                        {//首程
                            //#region 注释 edit by cyq 2012-12-18 修复首次病程前可能存在节点的bug
                            //bool existTheTime = modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTime);
                            //string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, !existTheTime,0);
                            //if (!string.IsNullOrEmpty(content.Trim()))
                            //{
                            //    //若节点存在病历
                            //    newEmrContent += content;
                            //}
                            //else
                            //{
                            //    //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                            //    DS_SqlService.UpdateRecordValid(int.Parse(dbRecords[0]["ID"].ToString()), false);
                            //}
                            #endregion
                            #region 注释 edit by cyq 2012-12-11
                            //if (modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTime))
                            //{
                            //    newEmrContent += GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, false);
                            //}
                            //else
                            //{
                            //    newEmrContent += GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTimeFirst, true);
                            //}
                            #endregion
                            continue;
                        }

                        if (i > 0)
                        {
                            string preTime = DateTime.Parse(dbRecords[i - 1]["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            if (modelList.Any(p => p.DisplayTime < DateTime.Parse(theTime) && p.DisplayTime > DateTime.Parse(preTime)))
                            {//先将时间较小的病历插入到新病程中
                                var currentTimes = modelList.Where(p => p.DisplayTime < DateTime.Parse(theTime) && p.DisplayTime > DateTime.Parse(preTime)).OrderBy(q => q.DisplayTime).ToList();
                                if (null != currentTimes && currentTimes.Count() > 0)
                                {
                                    for (int j = 0; j < currentTimes.Count(); j++)
                                    {
                                        string theTi = currentTimes[j].DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                        if (!newEmrContent.Contains(theTi))
                                        {
                                            string nextTi = null;
                                            if (j < currentTimes.Count() - 1)
                                            {
                                                nextTi = currentTimes[j + 1].DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                            }
                                            else
                                            {
                                                EmrModel nModel = modelList.Where(p => p.DisplayTime > DateTime.Parse(theTi)).OrderBy(q => q.DisplayTime).FirstOrDefault();
                                                nextTi = null == nModel ? null : nModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                            }
                                            string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTi, false, 0);
                                            if (!string.IsNullOrEmpty(content.Trim()))
                                            {
                                                //若节点存在病历
                                                newEmrContent += content;
                                            }
                                            else
                                            {
                                                //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                                UpdateRecordValidByTime(theTi, YD_Common.currentUser.CurrentDeptId, YD_Common.currentUser.CurrentWardId, false);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (modelList.Any(p => p.DisplayTime == DateTime.Parse(theTime)))
                        {//编辑病历与数据库病历时间相同
                            EmrModel model = modelList.FirstOrDefault(p => p.DisplayTime == DateTime.Parse(theTime) && p.CreatorXH == YD_Common.currentUser.Id);
                            if (null != model && model.InstanceId == -1)
                            {//新增病历&&创建人为当前登录人
                                //1、添加其他人创建的病历(数据库中已存在)
                                string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, true, 0);
                                if (!string.IsNullOrEmpty(content.Trim()))
                                {
                                    //若节点存在病历
                                    newEmrContent += content;
                                }
                                else
                                {
                                    //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                    UpdateRecordValid(int.Parse(dbRecords[i]["ID"].ToString()), false);
                                }

                                //为3、做伏笔(先截取病历，再改时间)
                                string newRecord = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, false, 0);
                                //2、将此病历时间加一秒，然后与数据库所存在的病历时间对比：
                                //   若有冲突，则将此病历时间加一秒，若还有冲突，将此病历时间再加一秒，以此类推...
                                //注：后移操作包括：1、节点标题时间；2、数据库字段时间；3、病历内时间；
                                modelList = SetRepTimeRecords(dbRecords, theTime, noofinpat);
                                //3、添加此病历(推移后)
                                string newTime = DateTime.Parse(theTime).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                                newRecord = newRecord.Replace(theTime, newTime);
                                if (!string.IsNullOrEmpty(newRecord.Trim()))
                                {
                                    //若节点存在病历
                                    newEmrContent += newRecord;
                                }
                                else
                                {
                                    //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                    UpdateRecordValidByTime(theTime, YD_Common.currentUser.CurrentDeptId, YD_Common.currentUser.CurrentWardId, false);
                                }
                            }
                            else
                            {//编辑病历
                                string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, false, 0);
                                if (!string.IsNullOrEmpty(content.Trim()))
                                {
                                    //若节点存在病历
                                    newEmrContent += content;
                                }
                                else
                                {
                                    //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                    //DS_SqlService.UpdateRecordValid(int.Parse(dbRecords[i]["ID"].ToString()), false);
                                    UpdateRecordValid(int.Parse(dbRecords[i]["ID"].ToString()), false);
                                }
                            }
                        }
                        else
                        {
                            newEmrContent += GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, true, 0);
                        }
                    }
                    if (newEmrContent.Contains("<document>") && newEmrContent.Contains("<body>") && !newEmrContent.Contains("</body></document>"))
                    {
                        newEmrContent += "</body></document>";
                    }
                }
                return newEmrContent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            **/
            #endregion

            try
            {
                string newEmrContent = string.Empty;
                if (null != dbRecords && dbRecords.Count() > 0)
                {
                    //数据库中病程上病历对应时间集合
                    List<string> dbTimeList = DS_BaseService.GetEditEmrContentTimes(dbXmlRecord);
                    //编辑页面病程上病历对应时间集合
                    List<string> timeList = modelList.OrderBy(q => q.DisplayTime).Select(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss")).ToList();

                    //查找首次病程节点
                    DataRow firstDaylyRow = dbRecords.Where(p => null != p["firstdailyflag"] && p["firstdailyflag"].ToString().Trim() == "1").OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                    if (null == firstDaylyRow || null == firstDaylyRow["captiondatetime"])
                    {
                        throw new Exception("该首次病程不存在或首次病程时间为空，请联系管理员。");
                    }

                    //遍历数据库中病历节点集合
                    for (int i = 0; i < dbRecords.Count(); i++)
                    {
                        string theTime = DateTime.Parse(dbRecords[i]["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        if (string.IsNullOrEmpty(newEmrContent.Trim()))
                        {//此病历为第一个病历
                            //添加第一个病历之前所有内容
                            newEmrContent = GetContentBeforeFirstRecord(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime);
                            if (string.IsNullOrEmpty(newEmrContent.Trim()))
                            {
                                UpdateRecordValid(int.Parse(dbRecords[i]["ID"].ToString()), false);//edit by ywk 2013年1月6日14:17:15 
                            }
                        }
                        if (i == 0)
                        {
                            //添加此病历内容
                            bool existTheTime = modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTime);
                            string contentFirst = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, !existTheTime, 0);
                            if (!string.IsNullOrEmpty(contentFirst.Trim()))
                            {
                                //若节点存在病历
                                newEmrContent += contentFirst;
                            }
                            else
                            {
                                //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                UpdateRecordValid(int.Parse(dbRecords[i]["ID"].ToString()), false);
                            }
                        }
                        else
                        {
                            string preTime = DateTime.Parse(dbRecords[i - 1]["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            if (modelList.Any(p => p.DisplayTime < DateTime.Parse(theTime) && p.DisplayTime > DateTime.Parse(preTime)))
                            {//先将时间较小的病历插入到新病程中
                                var currentTimes = modelList.Where(p => p.DisplayTime < DateTime.Parse(theTime) && p.DisplayTime > DateTime.Parse(preTime)).OrderBy(q => q.DisplayTime).ToList();
                                if (null != currentTimes && currentTimes.Count() > 0)
                                {
                                    for (int j = 0; j < currentTimes.Count(); j++)
                                    {
                                        string theTi = currentTimes[j].DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                        if (!newEmrContent.Contains(theTi))
                                        {
                                            string nextTi = null;
                                            if (j < currentTimes.Count() - 1)
                                            {
                                                nextTi = currentTimes[j + 1].DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                            }
                                            else
                                            {
                                                EmrModel nModel = modelList.Where(p => p.DisplayTime > DateTime.Parse(theTi)).OrderBy(q => q.DisplayTime).FirstOrDefault();
                                                nextTi = null == nModel ? null : nModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                            }
                                            string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTi, false, 0);
                                            if (!string.IsNullOrEmpty(content.Trim()))
                                            {
                                                //若节点存在病历
                                                newEmrContent += content;
                                            }
                                            else
                                            {
                                                //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                                UpdateRecordValidByTime(theTi, DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.CurrentWardId, false);
                                            }
                                        }
                                    }
                                }
                            }

                            if (modelList.Any(p => p.DisplayTime == DateTime.Parse(theTime)))
                            {//编辑病历与数据库病历时间相同
                                EmrModel model = modelList.FirstOrDefault(p => p.DisplayTime == DateTime.Parse(theTime) && p.CreatorXH == DS_Common.currentUser.Id);
                                if (null != model && model.InstanceId == -1)
                                {//新增病历&&创建人为当前登录人
                                    //1、添加其他人创建的病历(数据库中已存在)
                                    string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, true, 0);
                                    if (!string.IsNullOrEmpty(content.Trim()))
                                    {
                                        //若节点存在病历
                                        newEmrContent += content;
                                    }
                                    else
                                    {
                                        //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                        UpdateRecordValid(int.Parse(dbRecords[i]["ID"].ToString()), false);
                                    }

                                    //为3、做伏笔(先截取病历，再改时间)
                                    string newRecord = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, false, 0);
                                    //2、将此病历时间加一秒，然后与数据库所存在的病历时间对比：
                                    //   若有冲突，则将此病历时间加一秒，若还有冲突，将此病历时间再加一秒，以此类推...
                                    //注：后移操作包括：1、节点标题时间；2、数据库字段时间；3、病历内时间；
                                    modelList = SetRepTimeRecords(dbRecords, theTime, noofinpat);
                                    //3、添加此病历(推移后)
                                    string newTime = DateTime.Parse(theTime).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                                    newRecord = newRecord.Replace(theTime, newTime);
                                    if (!string.IsNullOrEmpty(newRecord.Trim()))
                                    {
                                        //若节点存在病历
                                        newEmrContent += newRecord;
                                    }
                                    else
                                    {
                                        //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                        UpdateRecordValidByTime(theTime, DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.CurrentWardId, false);
                                    }
                                }
                                else
                                {//编辑病历
                                    string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, false, 0);
                                    if (!string.IsNullOrEmpty(content.Trim()))
                                    {
                                        //若节点存在病历
                                        newEmrContent += content;
                                    }
                                    else
                                    {
                                        //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                        //DS_SqlService.UpdateRecordValid(int.Parse(dbRecords[i]["ID"].ToString()), false);
                                        UpdateRecordValid(int.Parse(dbRecords[i]["ID"].ToString()), false);
                                    }
                                }
                            }
                            else
                            {
                                newEmrContent += GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, true, 0);
                            }
                        }
                    }
                    if (newEmrContent.Contains("<document>") && newEmrContent.Contains("<body>") && !newEmrContent.Contains("</body></document>"))
                    {
                        newEmrContent += "</body></document>";
                    }
                }
                return newEmrContent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取合并后病程
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-19</date>
        /// <param name="dbXmlRecord">数据库中病程XML</param>
        /// <param name="xmlRecord">页面病程XML---已改为全局变量</param>
        /// <param name="dbRecords">数据库中时间记录</param>
        /// <param name="modelList">全局变量(页面传递病历节点集合)</param>
        /// <param name="noofinpat">住院号</param>
        /// <returns></returns>
        private string GetEmrContentsInTran(XmlDocument dbXmlRecord, List<DataRow> dbRecords, string noofinpat)
        {
            #region 已注释 by cyq 2012-01-14
            /**
            try
            {
                string newEmrContent = string.Empty;
                if (null != dbRecords && dbRecords.Count() > 0)
                {
                    //数据库中病程上病历对应时间集合
                    List<string> dbTimeList = DS_BaseService.GetEditEmrContentTimes(dbXmlRecord);
                    //编辑页面病程上病历对应时间集合
                    List<string> timeList = modelList.OrderBy(q => q.DisplayTime).Select(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss")).ToList();

                    //查找首次病程节点
                    DataRow firstDaylyRow = dbRecords.FirstOrDefault(p => null != p["firstdailyflag"] && p["firstdailyflag"].ToString().Trim() == "1");
                    if (null == firstDaylyRow)
                    {
                        return string.Empty;
                    }
                    if (null == firstDaylyRow["captiondatetime"])
                    {
                        throw new Exception("首次病程时间为空，请联系管理员。");
                    }
                    string theTimeFirst = DateTime.Parse(firstDaylyRow["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    //添加首次病程之前所有内容
                    newEmrContent = GetContentBeforeFirstRecord(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTimeFirst);
                    if (string.IsNullOrEmpty(newEmrContent.Trim()))
                    {
                        //UpdateRecordValid(int.Parse(firstDaylyRow["ID"].ToString()), false);//edit by ywk 2013年1月6日14:17:15 //edit by cyq 2013-01-11 处理事务
                        UpdateRecordValidInTran(int.Parse(firstDaylyRow["ID"].ToString()), false);
                    }
                    //添加首次病程内容 edit by cyq 2012-12-18 修复首次病程前可能存在节点的bug
                    bool existTheTime = modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTimeFirst);
                    string contentFirst = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTimeFirst, !existTheTime, 0);
                    if (!string.IsNullOrEmpty(contentFirst.Trim()))
                    {
                        //若节点存在病历
                        newEmrContent += contentFirst;
                    }
                    else
                    {
                        //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                        //UpdateRecordValid(int.Parse(firstDaylyRow["ID"].ToString()), false); //edit by cyq 2013-01-11 处理事务
                        UpdateRecordValidInTran(int.Parse(firstDaylyRow["ID"].ToString()), false);
                    }
                    //遍历数据库中病历节点集合
                    for (int i = 0; i < dbRecords.Count(); i++)
                    {
                        string theTime = DateTime.Parse(dbRecords[i]["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        if (dbRecords[i]["ID"].ToString() == firstDaylyRow["ID"].ToString())
                        {//首程
                            //#region 注释 edit by cyq 2012-12-18 修复首次病程前可能存在节点的bug
                            //bool existTheTime = modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTime);
                            //string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, !existTheTime,0);
                            //if (!string.IsNullOrEmpty(content.Trim()))
                            //{
                            //    //若节点存在病历
                            //    newEmrContent += content;
                            //}
                            //else
                            //{
                            //    //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                            //    DS_SqlService.UpdateRecordValid(int.Parse(dbRecords[0]["ID"].ToString()), false);
                            //}
                            #endregion
                            #region 注释 edit by cyq 2012-12-11
                            //if (modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTime))
                            //{
                            //    newEmrContent += GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, false);
                            //}
                            //else
                            //{
                            //    newEmrContent += GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTimeFirst, true);
                            //}
                            #endregion
                            continue;
                        }

                        if (i > 0)
                        {
                            string preTime = DateTime.Parse(dbRecords[i - 1]["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            if (modelList.Any(p => p.DisplayTime < DateTime.Parse(theTime) && p.DisplayTime > DateTime.Parse(preTime)))
                            {//先将时间较小的病历插入到新病程中
                                var currentTimes = modelList.Where(p => p.DisplayTime < DateTime.Parse(theTime) && p.DisplayTime > DateTime.Parse(preTime)).OrderBy(q => q.DisplayTime).ToList();
                                if (null != currentTimes && currentTimes.Count() > 0)
                                {
                                    for (int j = 0; j < currentTimes.Count(); j++)
                                    {
                                        string theTi = currentTimes[j].DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                        if (!newEmrContent.Contains(theTi))
                                        {
                                            string nextTi = null;
                                            if (j < currentTimes.Count() - 1)
                                            {
                                                nextTi = currentTimes[j + 1].DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                            }
                                            else
                                            {
                                                EmrModel nModel = modelList.Where(p => p.DisplayTime > DateTime.Parse(theTi)).OrderBy(q => q.DisplayTime).FirstOrDefault();
                                                nextTi = null == nModel ? null : nModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                            }
                                            string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTi, false, 0);
                                            if (!string.IsNullOrEmpty(content.Trim()))
                                            {
                                                //若节点存在病历
                                                newEmrContent += content;
                                            }
                                            else
                                            {
                                                //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                                //UpdateRecordValidByTime(theTi, YD_Common.currentUser.CurrentDeptId, YD_Common.currentUser.CurrentWardId, false);//edit by cyq 2013-01-11 处理事务
                                                UpdateRecordValidByTimeInTran(theTi, YD_Common.currentUser.CurrentDeptId, YD_Common.currentUser.CurrentWardId, false);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (modelList.Any(p => p.DisplayTime == DateTime.Parse(theTime)))
                        {//编辑病历与数据库病历时间相同
                            EmrModel model = modelList.FirstOrDefault(p => p.DisplayTime == DateTime.Parse(theTime) && p.CreatorXH == YD_Common.currentUser.Id);
                            if (null != model && model.InstanceId == -1)
                            {//新增病历&&创建人为当前登录人
                                //1、添加其他人创建的病历(数据库中已存在)
                                string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, true, 0);
                                if (!string.IsNullOrEmpty(content.Trim()))
                                {
                                    //若节点存在病历
                                    newEmrContent += content;
                                }
                                else
                                {
                                    //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                    //UpdateRecordValid(int.Parse(dbRecords[i]["ID"].ToString()), false);//edit by cyq 2013-01-11 处理事务
                                    UpdateRecordValidInTran(int.Parse(dbRecords[i]["ID"].ToString()), false);
                                }

                                //为3、做伏笔(先截取病历，再改时间)
                                string newRecord = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, false, 0);
                                //2、将此病历时间加一秒，然后与数据库所存在的病历时间对比：
                                //   若有冲突，则将此病历时间加一秒，若还有冲突，将此病历时间再加一秒，以此类推...
                                //注：后移操作包括：1、节点标题时间；2、数据库字段时间；3、病历内时间；
                                //modelList = SetRepTimeRecords(dbRecords, theTime, noofinpat);//edit by cyq 2013-01-11 处理事务
                                modelList = SetRepTimeRecordsInTran(dbRecords, theTime, noofinpat);
                                //3、添加此病历(推移后)
                                string newTime = DateTime.Parse(theTime).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                                newRecord = newRecord.Replace(theTime, newTime);
                                if (!string.IsNullOrEmpty(newRecord.Trim()))
                                {
                                    //若节点存在病历
                                    newEmrContent += newRecord;
                                }
                                else
                                {
                                    //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                    //UpdateRecordValidByTime(theTime, YD_Common.currentUser.CurrentDeptId, YD_Common.currentUser.CurrentWardId, false);//edit by cyq 2013-01-11 处理事务
                                    UpdateRecordValidByTimeInTran(theTime, YD_Common.currentUser.CurrentDeptId, YD_Common.currentUser.CurrentWardId, false);
                                }
                            }
                            else
                            {//编辑病历
                                string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, false, 0);
                                if (!string.IsNullOrEmpty(content.Trim()))
                                {
                                    //若节点存在病历
                                    newEmrContent += content;
                                }
                                else
                                {
                                    //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                    //DS_SqlService.UpdateRecordValid(int.Parse(dbRecords[i]["ID"].ToString()), false);
                                    //UpdateRecordValid(int.Parse(dbRecords[i]["ID"].ToString()), false);//edit by cyq 2013-01-11 处理事务
                                    UpdateRecordValidInTran(int.Parse(dbRecords[i]["ID"].ToString()), false);
                                }
                            }
                        }
                        else
                        {
                            newEmrContent += GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, true, 0);
                        }
                    }
                    if (newEmrContent.Contains("<document>") && newEmrContent.Contains("<body>") && !newEmrContent.Contains("</body></document>"))
                    {
                        newEmrContent += "</body></document>";
                    }
                }
                return newEmrContent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            **/
            #endregion

            try
            {
                string newEmrContent = string.Empty;
                if (null != dbRecords && dbRecords.Count() > 0)
                {
                    //数据库中病程上病历对应时间集合
                    List<string> dbTimeList = DS_BaseService.GetEditEmrContentTimes(dbXmlRecord);//非数据操作
                    //编辑页面病程上病历对应时间集合
                    List<string> timeList = modelList.OrderBy(q => q.DisplayTime).Select(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss")).ToList();

                    //查找首次病程节点
                    DataRow firstDaylyRow = dbRecords.Where(p => null != p["firstdailyflag"] && p["firstdailyflag"].ToString().Trim() == "1").OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                    if (null == firstDaylyRow || null == firstDaylyRow["captiondatetime"])
                    {
                        throw new Exception("首次病程时间为空，请联系管理员。");
                    }

                    //遍历数据库中病历节点集合
                    for (int i = 0; i < dbRecords.Count(); i++)
                    {
                        string theTime = DateTime.Parse(dbRecords[i]["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        if (string.IsNullOrEmpty(newEmrContent.Trim()))
                        {//此病历为第一个病历
                            //添加第一个病历之前所有内容
                            newEmrContent = GetContentBeforeFirstRecord(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime);//非数据操作
                            if (string.IsNullOrEmpty(newEmrContent.Trim()))
                            {
                                UpdateRecordValidInTran(int.Parse(dbRecords[i]["ID"].ToString()), false);//edit by ywk 2013年1月6日14:17:15 
                            }
                        }
                        if (i == 0)
                        {
                            //添加此病历内容
                            bool existTheTime = modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTime);
                            string contentFirst = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, !existTheTime, 0);//非数据操作
                            if (!string.IsNullOrEmpty(contentFirst.Trim()))
                            {
                                //若节点存在病历
                                newEmrContent += contentFirst;
                            }
                            else
                            {
                                //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                UpdateRecordValidInTran(int.Parse(dbRecords[i]["ID"].ToString()), false);
                            }
                        }
                        else
                        {
                            string preTime = DateTime.Parse(dbRecords[i - 1]["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            if (modelList.Any(p => p.DisplayTime < DateTime.Parse(theTime) && p.DisplayTime > DateTime.Parse(preTime)))
                            {//先将时间较小的病历插入到新病程中
                                var currentTimes = modelList.Where(p => p.DisplayTime < DateTime.Parse(theTime) && p.DisplayTime > DateTime.Parse(preTime)).OrderBy(q => q.DisplayTime).ToList();
                                if (null != currentTimes && currentTimes.Count() > 0)
                                {
                                    for (int j = 0; j < currentTimes.Count(); j++)
                                    {
                                        string theTi = currentTimes[j].DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                        if (!newEmrContent.Contains(theTi))
                                        {
                                            string nextTi = null;
                                            if (j < currentTimes.Count() - 1)
                                            {
                                                nextTi = currentTimes[j + 1].DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                            }
                                            else
                                            {
                                                EmrModel nModel = modelList.Where(p => p.DisplayTime > DateTime.Parse(theTi)).OrderBy(q => q.DisplayTime).FirstOrDefault();
                                                nextTi = null == nModel ? null : nModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                                            }
                                            string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTi, false, 0);
                                            if (!string.IsNullOrEmpty(content.Trim()))
                                            {
                                                //若节点存在病历
                                                newEmrContent += content;
                                            }
                                            else
                                            {
                                                //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                                UpdateRecordValidByTimeInTran(theTi, DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.CurrentWardId, false);
                                            }
                                        }
                                    }
                                }
                            }

                            if (modelList.Any(p => p.DisplayTime == DateTime.Parse(theTime)))
                            {//编辑病历与数据库病历时间相同
                                EmrModel model = modelList.FirstOrDefault(p => p.DisplayTime == DateTime.Parse(theTime) && p.CreatorXH == DS_Common.currentUser.Id);
                                if (null != model && model.InstanceId == -1)
                                {//新增病历&&创建人为当前登录人
                                    //1、添加其他人创建的病历(数据库中已存在)
                                    string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, true, 0);
                                    if (!string.IsNullOrEmpty(content.Trim()))
                                    {
                                        //若节点存在病历
                                        newEmrContent += content;
                                    }
                                    else
                                    {
                                        //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                        UpdateRecordValidInTran(int.Parse(dbRecords[i]["ID"].ToString()), false);
                                    }

                                    //为3、做伏笔(先截取病历，再改时间)
                                    string newRecord = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, false, 0);
                                    //2、将此病历时间加一秒，然后与数据库所存在的病历时间对比：
                                    //   若有冲突，则将此病历时间加一秒，若还有冲突，将此病历时间再加一秒，以此类推...
                                    //注：后移操作包括：1、节点标题时间；2、数据库字段时间；3、病历内时间；
                                    modelList = SetRepTimeRecordsInTran(dbRecords, theTime, noofinpat);
                                    //3、添加此病历(推移后)
                                    string newTime = DateTime.Parse(theTime).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                                    newRecord = newRecord.Replace(theTime, newTime);
                                    if (!string.IsNullOrEmpty(newRecord.Trim()))
                                    {
                                        //若节点存在病历
                                        newEmrContent += newRecord;
                                    }
                                    else
                                    {
                                        //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                        UpdateRecordValidByTimeInTran(theTime, DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.CurrentWardId, false);
                                    }
                                }
                                else
                                {//编辑病历
                                    string content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, false, 0);
                                    if (!string.IsNullOrEmpty(content.Trim()))
                                    {
                                        //若节点存在病历
                                        newEmrContent += content;
                                    }
                                    else
                                    {
                                        //若节点不存在病历则(逻辑)删除该节点 add by cyq 2012-12-11
                                        UpdateRecordValidInTran(int.Parse(dbRecords[i]["ID"].ToString()), false);
                                    }
                                }
                            }
                            else
                            {
                                newEmrContent += GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, true, 0);
                            }
                        }
                    }
                    if (newEmrContent.Contains("<document>") && newEmrContent.Contains("<body>") && !newEmrContent.Contains("</body></document>"))
                    {
                        newEmrContent += "</body></document>";
                    }
                }
                return newEmrContent;
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
        private XmlDocument AddNodesToXmlDocument(XmlDocument doc)
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
        /// 更新病历的有效状态
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-11</date>
        /// <param name="datetime">病历时间</param>
        /// <param name="deptCode">科室编码</param>
        /// <param name="wardCode">病区编码</param>
        /// <param name="boo">是否有效</param>
        /// <return></return>
        public void UpdateRecordValidByTime(string datetime, string deptCode, string wardCode, bool boo)
        {
            try
            {
                int isvalid = boo ? 1 : 0;
                string sqlStr = string.Format(" update recorddetail set valid='{0}' where departcode='{1}' and wardcode='{2}' and substr(nvl(captiondatetime,'1990-01-01 00:00:00'),0,19)='{3}' ", isvalid, deptCode, wardCode, datetime);
                sql_Helper.ExecuteNoneQuery(sqlStr, CommandType.Text);
                //DbParameter[] sqlParams = new DbParameter[]
                //{
                //    new SqlParameter("@datetime",SqlDbType.Char),
                //    new SqlParameter("@deptCode",SqlDbType.Char),
                //    new SqlParameter("@wardCode",SqlDbType.Char),
                //    new SqlParameter("@valid",SqlDbType.Int)
                //};
                //sqlParams[0].Value = datetime;
                //sqlParams[1].Value = deptCode;
                //sqlParams[2].Value = wardCode;
                //sqlParams[3].Value = boo ? 1 : 0;
                //DS_SqlHelper.CreateSqlHelper();
                //DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病历的有效状态
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-11</date>
        /// <param name="datetime">病历时间</param>
        /// <param name="deptCode">科室编码</param>
        /// <param name="wardCode">病区编码</param>
        /// <param name="boo">是否有效</param>
        /// <return></return>
        public void UpdateRecordValidByTimeInTran(string datetime, string deptCode, string wardCode, bool boo)
        {
            try
            {
                int isvalid = boo ? 1 : 0;
                string sqlStr = string.Format(" update recorddetail set valid='{0}' where departcode='{1}' and wardcode='{2}' and substr(nvl(captiondatetime,'1990-01-01 00:00:00'),0,19)='{3}' ", isvalid, deptCode, wardCode, datetime);
                DS_SqlHelper.ExecuteNonQueryInTran(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病历的有效状态
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-10</date>
        /// oracle 9i调用有问题，先放到这  edit by ywk 2012年12月29日8:35:20  
        /// <param name="id">病历ID</param>
        /// <param name="boo">是否有效</param>
        /// <return></return>
        public void UpdateRecordValid(int id, bool boo)
        {
            try
            {
                //string sqlStr = " update recorddetail set valid=@valid where id=@id ";
                //DbParameter[] sqlParams = new DbParameter[]
                //{
                //    new SqlParameter("@id",SqlDbType.Int),
                //    new SqlParameter("@valid",SqlDbType.Int)
                //};
                //sqlParams[0].Value = id;
                //sqlParams[1].Value = boo ? 1 : 0;
                int isvalid = boo ? 1 : 0;
                string sqlStr = string.Format("update recorddetail set valid='{0}' where id='{1}' ", isvalid, id);
                //sql_Helper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
                sql_Helper.ExecuteNoneQuery(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病历的有效状态
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-10</date>
        /// oracle 9i调用有问题，先放到这  edit by ywk 2012年12月29日8:35:20  
        /// <param name="id">病历ID</param>
        /// <param name="boo">是否有效</param>
        /// <return></return>
        public void UpdateRecordValidInTran(int id, bool boo)
        {
            try
            {
                int isvalid = boo ? 1 : 0;
                string sqlStr = string.Format("update recorddetail set valid='{0}' where id='{1}' ", isvalid, id);

                DS_SqlHelper.ExecuteNonQueryInTran(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 时间重复的病历处理(后移)
        /// 规则：与数据库所存在的病历时间对比，若有冲突，则将冲突病历后移一秒，若再冲突，将再冲突的病历后移一秒，以此类推...
        /// 后移：
        ///     1、节点标题中时间；
        ///     2、数据库字段时间；
        ///     3、病历内容中时间；
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="xmlRecord">数据库中病历内容XML --- 全局变量</param>
        /// <param name="dbRecords">数据库中病历记录集合（自作参照，不修改）</param>
        /// <param name="modelList">编辑页面节点集（修改时间回传）</param>
        /// <param name="theTime">冲突时间</param>
        /// <param name="noofinpat">住院号</param>
        /// <returns></returns>
        private List<EmrModel> SetRepTimeRecords(List<DataRow> dbRecords, string theTime, string noofinpat)
        {
            try
            {
                if (!modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTime))
                {
                    return modelList;
                }
                string newTime = DateTime.Parse(theTime).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                if (null != modelList && modelList.Count() > 0)
                {
                    //将原List中的冲突时间变更为新时间
                    for (int i = 0; i < modelList.Count(); i++)
                    {
                        if (modelList[i].DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTime)
                        {
                            if (modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == newTime))
                            {//依然冲突(与编辑页面记录)
                                modelList = SetRepTimeRecords(dbRecords, newTime, noofinpat);
                            }
                            modelList[i].DisplayTime = DateTime.Parse(newTime);
                        }
                    }
                }
                //1、更改病历内容中时间
                xmlRecord.InnerXml = xmlRecord.InnerXml.Replace(theTime, newTime);
                //2、更改节点标题中时间
                //3、更改数据库字段时间
                UpdateRecordTimes(theTime, newTime, noofinpat);

                return modelList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 时间重复的病历处理(后移)
        /// 规则：与数据库所存在的病历时间对比，若有冲突，则将冲突病历后移一秒，若再冲突，将再冲突的病历后移一秒，以此类推...
        /// 后移：
        ///     1、节点标题中时间；
        ///     2、数据库字段时间；
        ///     3、病历内容中时间；
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="xmlRecord">数据库中病历内容XML --- 全局变量</param>
        /// <param name="dbRecords">数据库中病历记录集合（自作参照，不修改）</param>
        /// <param name="modelList">编辑页面节点集（修改时间回传）</param>
        /// <param name="theTime">冲突时间</param>
        /// <param name="noofinpat">住院号</param>
        /// <returns></returns>
        private List<EmrModel> SetRepTimeRecordsInTran(List<DataRow> dbRecords, string theTime, string noofinpat)
        {
            try
            {
                if (!modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTime))
                {
                    return modelList;
                }
                string newTime = DateTime.Parse(theTime).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                if (null != modelList && modelList.Count() > 0)
                {
                    //将原List中的冲突时间变更为新时间
                    for (int i = 0; i < modelList.Count(); i++)
                    {
                        if (modelList[i].DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == theTime)
                        {
                            if (modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == newTime))
                            {//依然冲突(与编辑页面记录)
                                modelList = SetRepTimeRecordsInTran(dbRecords, newTime, noofinpat);
                            }
                            modelList[i].DisplayTime = DateTime.Parse(newTime);
                        }
                    }
                }
                //1、更改病历内容中时间
                xmlRecord.InnerXml = xmlRecord.InnerXml.Replace(theTime, newTime);
                //2、更改节点标题中时间
                //3、更改数据库字段时间
                UpdateRecordTimesInTran(theTime, newTime, noofinpat);

                return modelList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据时间获取病程记录
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-11</date>
        /// <param name="dbXmlRecord">数据库病程</param>
        /// <param name="xmlRecord">编辑页面病程</param>
        /// <param name="dbTimeList">数据库中病历上的时间记录集合(yyyy-MM-dd HH:mm:ss)</param>
        /// <param name="timeList">编辑页面上时间记录集合(yyyy-MM-dd HH:mm:ss)</param>
        /// <param name="theTime">首次病程时间(即所获取病历所在时间)</param>
        /// <returns></returns>
        private string GetContentBeforeFirstRecord(XmlDocument dbXmlRecord, XmlDocument xmlRecord, List<string> dbTimeList, List<string> timeList, string theFirstTime)
        {
            try
            {
                string content = string.Empty;
                if (!string.IsNullOrEmpty(theFirstTime))
                {
                    string firstRecordDb = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theFirstTime, true, 0);
                    string firstRecordEdit = GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theFirstTime, false, 0);
                    string beforeContentDb = string.Empty;
                    string beforeContentEdit = string.Empty;
                    if (!string.IsNullOrEmpty(firstRecordDb))
                    {
                        //add by cyq 2012-12-21
                        int len = dbXmlRecord.InnerXml.IndexOf(firstRecordDb);
                        //int len = dbEmrContent.IndexOf(firstRecordDb);
                        beforeContentDb = dbXmlRecord.InnerXml.Substring(0, len < 0 ? 0 : len);
                        //beforeContentDb = dbEmrContent.Substring(0, len < 0 ? 0 : len);
                    }
                    if (!string.IsNullOrEmpty(firstRecordEdit))
                    {
                        //add by cyq 2012-12-21 病程空格问题
                        int len = xmlRecord.InnerXml.IndexOf(firstRecordEdit);
                        //int len = editorEmrContent.IndexOf(firstRecordEdit);
                        beforeContentEdit = xmlRecord.InnerXml.Substring(0, len < 0 ? 0 : len);
                        //beforeContentEdit = editorEmrContent.Substring(0, len < 0 ? 0 : len);
                    }
                    if ((!string.IsNullOrEmpty(beforeContentDb) || !string.IsNullOrEmpty(beforeContentEdit)) && !beforeContentDb.Equals(beforeContentEdit))
                    {
                        content = !string.IsNullOrEmpty(beforeContentEdit) ? beforeContentEdit : beforeContentDb;
                    }
                    else
                    {
                        content = beforeContentDb;
                    }
                }
                return content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 已注释 方法添加至通用库
        /// <summary>
        /// 获取编辑病程的所有节点日期时间
        /// 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-19</date>
        /// <param name="body">病程body</param>
        /// <returns></returns>
        //private List<string> GetEditEmrContentTimes(XmlDocument xmlRecord)
        //{
        //    try
        //    {
        //        List<string> alRecordTime = new List<string>();
        //        XmlNodeList inbody = xmlRecord.SelectSingleNode("//body").ChildNodes;
        //        foreach (XmlNode p in inbody)
        //        {
        //            XmlNodeList inp = p.ChildNodes;
        //            foreach (XmlNode node in inp)
        //            {
        //                foreach (XmlNode n in node.Attributes)
        //                {
        //                    if (n.Value == "记录日期" && !string.IsNullOrEmpty(node.InnerText))
        //                    {
        //                        alRecordTime.Add(DateTime.Parse(node.InnerText).ToString("yyyy-MM-dd HH:mm:ss"));
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        return alRecordTime.OrderBy(p => DateTime.Parse(p)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        /// <summary>
        /// 根据时间获取病程记录标题
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-19</date>
        /// <param name="patid"></param>
        /// <returns></returns>
        //private string GetEmrRecordTitle(XmlDocument doc, string theTime)
        //{
        //    try
        //    {
        //        XmlNodeList nodelist = doc.SelectNodes("//p//text");
        //        foreach (XmlNode node in nodelist)
        //        {
        //            if (node.InnerText == theTime)
        //            {
        //                return node.ParentNode.OuterXml;
        //            }
        //        }
        //        return string.Empty;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        #endregion

        /// <summary>
        /// 根据病人的首页序号获得病程的记录在数据库中总共有几个
        /// </summary>
        /// <param name="patid"></param>
        /// <returns></returns>
        private DataTable GetRecordData(string patid)
        {
            try
            {
                //DataTable dt = new DataTable();
                //int m_Count = 0;
                string sql = string.Format(@"select * from recorddetail where noofinpat='{0}' and valid='1' and sortid='AC' order by captiondatetime ", patid);
                DataTable m_Dt = sql_Helper.ExecuteDataTable(sql, CommandType.Text);
                //if (m_Dt.Rows.Count > 0)
                //{
                //    m_Count = m_Dt.Rows.Count;
                //}
                return m_Dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取病程记录
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="noofinpat">病案号</param>
        /// <param name="datetime">时间（yyyy-MM-dd HH:mm:ss）</param>
        /// <returns></returns>
        private DataTable GetRecordData(string noofinpat, string datetime)
        {
            try
            {
                string sql = string.Format(@"select * from recorddetail where noofinpat='{0}' and owner='{1}' and valid='1' and sortid='AC' and to_char(to_date(captiondatetime, 'yyyy-MM-dd hh24:mi:ss'), 'yyyy-MM-dd hh24:mi:ss')='{2}' order by captiondatetime ", noofinpat, DS_Common.currentUser.Id, datetime);

                return sql_Helper.ExecuteDataTable(sql, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取病程记录
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="noofinpat">病案号</param>
        /// <param name="datetime">时间（yyyy-MM-dd HH:mm:ss）</param>
        /// <returns></returns>
        private DataTable GetRecordDataInTran(string noofinpat, string datetime)
        {
            try
            {
                string sql = string.Format(@"select * from recorddetail where noofinpat='{0}' and owner='{1}' and valid='1' and sortid='AC' and to_char(to_date(captiondatetime, 'yyyy-MM-dd hh24:mi:ss'), 'yyyy-MM-dd hh24:mi:ss')='{2}' order by captiondatetime ", noofinpat, DS_Common.currentUser.Id, datetime);

                return DS_SqlHelper.ExecuteDataTableInTran(sql, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病程记录
        /// 1、更新name（时间）
        /// 2、更新captiondatetime字段
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="name">节点名称</param>
        /// <param name="datetime">时间（yyyy-MM-dd HH:mm:ss）</param>
        /// <param name="id">病历ID</param>
        /// <returns></returns>
        private void UpdateRecordTimes(string theTime, string newTime, string noofinpat)
        {
            try
            {
                DataTable dt = GetRecordData(noofinpat, theTime);
                if (null != dt && dt.Rows.Count > 0)
                {
                    string name = dt.Rows[0]["NAME"].ToString().Replace(theTime, newTime);
                    UpdateRecordTimesByID(name, newTime, dt.Rows[0]["ID"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病程记录
        /// 1、更新name（时间）
        /// 2、更新captiondatetime字段
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="name">节点名称</param>
        /// <param name="datetime">时间（yyyy-MM-dd HH:mm:ss）</param>
        /// <param name="id">病历ID</param>
        /// <returns></returns>
        private void UpdateRecordTimesInTran(string theTime, string newTime, string noofinpat)
        {
            try
            {
                DataTable dt = GetRecordDataInTran(noofinpat, theTime);
                if (null != dt && dt.Rows.Count > 0)
                {
                    string name = dt.Rows[0]["NAME"].ToString().Replace(theTime, newTime);
                    UpdateRecordTimesByIDInTran(name, newTime, dt.Rows[0]["ID"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病程记录
        /// 1、更新name（时间）
        /// 2、更新captiondatetime字段
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="name">节点名称</param>
        /// <param name="datetime">时间（yyyy-MM-dd HH:mm:ss）</param>
        /// <param name="id">病历ID</param>
        /// <returns></returns>
        private void UpdateRecordTimesByID(string name, string datetime, string id)
        {
            try
            {
                string sql = string.Format(@"update recorddetail set name='{0}',captiondatetime='{1}' where id={2} ", name, datetime, id);

                sql_Helper.ExecuteNoneQuery(sql, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病程记录
        /// 1、更新name（时间）
        /// 2、更新captiondatetime字段
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="name">节点名称</param>
        /// <param name="datetime">时间（yyyy-MM-dd HH:mm:ss）</param>
        /// <param name="id">病历ID</param>
        /// <returns></returns>
        private void UpdateRecordTimesByIDInTran(string name, string datetime, string id)
        {
            try
            {
                string sql = string.Format(@"update recorddetail set name='{0}',captiondatetime='{1}' where id={2} ", name, datetime, id);

                DS_SqlHelper.ExecuteNonQueryInTran(sql, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据ID获得病程(全部)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-19</date>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetFirstRecord(string index)
        {
            try
            {
                if (!string.IsNullOrEmpty(index))
                {
                    string sql = string.Format(@"select * from recorddetail where id='{0}' and valid='1' and sortid='AC' and firstdailyflag='1' ", index);
                    DataTable m_Dt = sql_Helper.ExecuteDataTable(sql, CommandType.Text);
                    if (null != m_Dt && m_Dt.Rows.Count > 0)
                    {
                        return m_Dt.Rows[0]["content"].ToString();
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
        /// 根据ID获得病程(全部) --- 事务专用
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-19</date>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetFirstRecordInTran(string index)
        {
            try
            {
                if (!string.IsNullOrEmpty(index))
                {
                    string sql = string.Format(@"select * from recorddetail where id='{0}' and valid='1' and sortid='AC' and firstdailyflag='1' ", index);
                    DataTable m_Dt = DS_SqlHelper.ExecuteDataTableInTran(sql, CommandType.Text);
                    if (null != m_Dt && m_Dt.Rows.Count > 0)
                    {
                        return m_Dt.Rows[0]["content"].ToString();
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
        /// 根据时间获取病程记录
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="dbXmlRecord">数据库病程</param>
        /// <param name="xmlRecord">编辑页面病程</param>
        /// <param name="dbTimeList">数据库中病历上的时间记录集合(yyyy-MM-dd HH:mm:ss)</param>
        /// <param name="timeList">编辑页面上时间记录集合(yyyy-MM-dd HH:mm:ss)</param>
        /// <param name="theTime">病历时间(即所获取病历所在时间)</param>
        /// <param name="flag">获取病历标识：true - 从数据库中获取；false - 从页面获取</param>
        /// <param name="count">递归级数</param>
        /// <returns></returns>
        private string GetEmrRecordDetail(XmlDocument dbXmlRecord, XmlDocument xmlRecord, List<string> dbTimeList, List<string> timeList, string theTime, bool flag, int count)
        {
            try
            {
                #region "已注释"
                //if (string.IsNullOrEmpty(theTime))
                //{
                //    return string.Empty;
                //}
                //string recordDetail = string.Empty;
                //XmlDocument doc = flag ? dbXmlRecord : xmlRecord;
                //string emrContent = flag ? dbEmrContent : editorEmrContent;//add by cyq 2012-12-21 病程空格问题
                //List<string> list = flag ? dbTimeList : timeList;

                //string theTitle = DS_BaseService.GetEmrRecordTitle(doc, theTime);
                //if (string.IsNullOrEmpty(theTitle.Trim()) && count == 1)
                //{
                //    return string.Empty;
                //}
                //string nextTime = list.Where(p => DateTime.Parse(p) > DateTime.Parse(theTime)).OrderBy(q => DateTime.Parse(q)).FirstOrDefault();
                //string nextTitle = !string.IsNullOrEmpty(nextTime) ? DS_BaseService.GetEmrRecordTitle(doc, nextTime) : "</body></document>";
                //if ((string.IsNullOrEmpty(theTitle.Trim()) || (!string.IsNullOrEmpty(nextTime) && string.IsNullOrEmpty(nextTitle.Trim()))) && count == 0)
                //{
                //    return GetEmrRecordDetail(dbXmlRecord, xmlRecord, dbTimeList, timeList, theTime, !flag, count + 1);
                //}
                ////add by cyq 2012-12-21 病程空格问题
                ////int startIndex = doc.InnerXml.IndexOf(theTitle);
                ////int endIndex = doc.InnerXml.IndexOf(nextTitle);
                //int startIndex = emrContent.IndexOf(theTitle);
                //int endIndex = emrContent.IndexOf(nextTitle);

                //if (startIndex < 0 || endIndex < 0 || startIndex > endIndex)
                //{
                //    return string.Empty;
                //}
                ////add by cyq 2012-12-21 病程空格问题
                ////return doc.InnerXml.Substring(startIndex, endIndex - startIndex);
                //return emrContent.Substring(startIndex, endIndex - startIndex);
                #endregion

                return GetEmrRecordDetail(dbXmlRecord, xmlRecord, theTime, flag, count);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据时间获取病程记录
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="dbXmlRecord">数据库病程</param>
        /// <param name="xmlRecord">编辑页面病程</param>
        /// <param name="dbTimeList">数据库中病历上的时间记录集合(yyyy-MM-dd HH:mm:ss)</param>
        /// <param name="timeList">编辑页面上时间记录集合(yyyy-MM-dd HH:mm:ss)</param>
        /// <param name="theTime">病历时间(即所获取病历所在时间)</param>
        /// <param name="flag">获取病历标识：true - 从数据库中获取；false - 从页面获取</param>
        /// <param name="count">递归级数</param>
        /// <returns></returns>
        private string GetEmrRecordDetail(XmlDocument dbXmlRecord, XmlDocument xmlRecord, string theTime, bool flag, int count)
        {
            try
            {
                if (string.IsNullOrEmpty(theTime))
                {
                    return string.Empty;
                }
                string content = string.Empty;
                XmlDocument doc = flag ? dbXmlRecord : xmlRecord;
                List<string[]> titleList = new List<string[]>();
                XmlNodeList nodelist = doc.SelectNodes("//p//text");
                foreach (XmlNode node in nodelist)
                {
                    if (null != node.Attributes["name"] && node.Attributes["name"].Value == "记录日期")
                    {
                        string[] array = new string[2];
                        array[0] = DateTime.Parse(node.InnerText).ToString("yyyy-MM-dd HH:mm:ss");
                        if (doc.InnerXml.Substring(doc.InnerXml.IndexOf(node.OuterXml) - 30, node.OuterXml.Length + 30).Contains("<p"))
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

                for (int i = 0; i < titleList.Count(); i++)
                {
                    string[] arr = titleList[i];
                    if (arr[0] == theTime)
                    {
                        if (i == titleList.Count() - 1)
                        {
                            int startIndex = doc.InnerXml.IndexOf(arr[1]);
                            int endIndex = doc.InnerXml.IndexOf("</body></document>");
                            if (startIndex >= 0 && endIndex >= 0 && startIndex <= endIndex)
                            {
                                content = doc.InnerXml.Substring(startIndex, endIndex - startIndex);
                                break;
                            }
                        }
                        else
                        {
                            int startIndex = doc.InnerXml.IndexOf(arr[1]);
                            int endIndex = doc.InnerXml.IndexOf(titleList[i + 1][1]);
                            if (startIndex >= 0 && endIndex >= 0 && startIndex > endIndex)
                            {
                                endIndex = doc.InnerXml.LastIndexOf(titleList[i + 1][1]);
                            }
                            if (startIndex >= 0 && endIndex >= 0 && startIndex <= endIndex)
                            {
                                content = doc.InnerXml.Substring(startIndex, endIndex - startIndex);
                                break;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(content) && count <= 1)
                {
                    content = GetEmrRecordDetail(dbXmlRecord, xmlRecord, theTime, !flag, count + 1);
                }

                return content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 已弃用 by cyq 2013-01-23
        ///// <summary>
        ///// 根据首程ID获取该首程下所有病程记录
        ///// </summary>
        ///// <auth>Yanqiao.Cai</auth>
        ///// <date>2012-12-01</date>
        ///// <param name="patid"></param>
        ///// <returns></returns>
        //private List<DataRow> GetThisPageRecords(DataTable recordDt, string index)
        //{
        //    try
        //    {
        //        List<DataRow> dbRecords = new List<DataRow>();
        //        var enuList = recordDt.AsEnumerable();
        //        DataRow drow = enuList.FirstOrDefault(p => p["ID"].ToString() == index);
        //        //获取该病人所有首程记录(包含转科)
        //        DataTable firstRecords = DS_SqlService.GetFirstRecordsByDept(int.Parse(drow["ID"].ToString()), string.Empty);
        //        //获取该首程下一个首程
        //        DataRow nextrow = firstRecords.Select(" captiondatetime > '" + drow["captiondatetime"].ToString() + "' ").OrderBy(p => p["captiondatetime"]).FirstOrDefault();
        //        DateTime dtStart = DateTime.Parse(drow["captiondatetime"].ToString());
        //        DateTime dtEnd = null == nextrow ? DateTime.Now : DateTime.Parse(nextrow["captiondatetime"].ToString());

        //        if (null != drow)
        //        {
        //            var thisList = enuList.Where(q => null != q["captiondatetime"] && q["captiondatetime"].ToString().Trim() != "" && q["departcode"].ToString() == drow["departcode"].ToString() && DateTime.Parse(q["captiondatetime"].ToString()) >= dtStart && DateTime.Parse(q["captiondatetime"].ToString()) <= dtEnd);
        //            dbRecords = thisList.OrderBy(p => DateTime.Parse(p["captiondatetime"].ToString())).ToList();
        //        }
        //        return dbRecords;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        #endregion

        /// <summary>
        /// 根据首程ID获取该首程对应的所有病历记录
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-01</date>
        /// <param name="index">首程ID</param>
        /// <param name="record">编辑器xml</param>
        /// <returns></returns>
        private List<DataRow> GetThisPageRecords(int index, XmlDocument record)
        {
            #region 已注释 by cyq 2013-01-22 11:15 修复A->B->A后删除B科室所有病历 保存出错问题
            /**
            try
            {
                List<DataRow> dbRecords = new List<DataRow>();
                DataTable theFirstRecord = DS_SqlService.GetRecordByID(index);
                if (null == theFirstRecord || theFirstRecord.Rows.Count == 0)
                {
                    return dbRecords;
                }
                int noofinpat = int.Parse(theFirstRecord.Rows[0]["noofinpat"].ToString());
                //获取该病人(数据库中)所有的病历记录
                DataTable allRecords = DS_SqlService.GetRecordsByNoofinpat(noofinpat);
                if (null == allRecords || allRecords.Rows.Count == 0)
                {
                    return dbRecords;
                }
                //获取该病人所有首程记录(包含转科)
                var firstRecords = allRecords.Select(" firstdailyflag = " + 1);
                if (null == firstRecords || firstRecords.Count() == 0)
                {
                    return dbRecords;
                }
                //获取该首程上一个首程
                DataRow preFirstDaily = firstRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < DateTime.Parse(theFirstRecord.Rows[0]["captiondatetime"].ToString())).OrderByDescending(p => p["captiondatetime"]).FirstOrDefault();
                //获取该首程下一个首程
                DataRow nextFirstDaily = firstRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > DateTime.Parse(theFirstRecord.Rows[0]["captiondatetime"].ToString())).OrderBy(p => p["captiondatetime"]).FirstOrDefault();
                //获取该首程下病历时间段
                DateTime dtStart = DateTime.Parse(theFirstRecord.Rows[0]["captiondatetime"].ToString());
                DateTime dtEnd = DateTime.Now;
                //开始时间(如果存在上一个首程，则为上一个首程时间；否则为第一个病历时间)
                if (null == preFirstDaily || null == preFirstDaily["captiondatetime"] || string.IsNullOrEmpty(preFirstDaily["captiondatetime"].ToString().Trim()))
                {
                    dtStart = DateTime.Parse(allRecords.Rows[0]["captiondatetime"].ToString());
                }
                else
                {
                    dtStart = DateTime.Parse(preFirstDaily["captiondatetime"].ToString());
                }
                //结束时间(如果存在下一个首程，则为下一个首程时间；否则为最后一个病历时间)
                if (null == nextFirstDaily || null == nextFirstDaily["captiondatetime"] || string.IsNullOrEmpty(nextFirstDaily["captiondatetime"].ToString().Trim()))
                {
                    dtEnd = DateTime.Parse(allRecords.Rows[allRecords.Rows.Count - 1]["captiondatetime"].ToString());
                }
                else
                {
                    dtEnd = DateTime.Parse(nextFirstDaily["captiondatetime"].ToString());
                }
                string dept = null == theFirstRecord.Rows[0]["departcode"] ? YD_Common.currentUser.CurrentDeptId : theFirstRecord.Rows[0]["departcode"].ToString().Trim();
                dbRecords = allRecords.Select(" departcode = '" + dept + "' ").Where(p => DateTime.Parse(p["captiondatetime"].ToString()) >= dtStart && DateTime.Parse(p["captiondatetime"].ToString()) <= dtEnd).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                //获取页面xml拆分出的节点记录
                List<string> editorTimes = DS_BaseService.GetEditEmrContentTimes(record);
                List<string> dbTimes = dbRecords.Select(p => DateTime.Parse(p["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")).ToList();
                var notInTimes = editorTimes.Where(p => !dbTimes.Contains(p)).ToList();
                if (null != notInTimes && notInTimes.Count() > 0)
                {
                    string notInTimesSQL = YD_Common.CombineSQLStringByList(notInTimes);
                    var notInList = DS_SqlService.GetRecordByDateTimes(noofinpat, notInTimesSQL).Select(" departcode = '" + dept + "' ");
                    foreach (DataRow drow in notInList)
                    {
                        dbRecords.Add(drow);
                    }
                    dbRecords = dbRecords.OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                }
                return dbRecords;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            **/
            #endregion

            #region 已注释 by cyq 2013-01-28 优化A->B->A 删除B科室所有病历后 A科室新增病历时间段的问题
            /**
            try
            {
                List<DataRow> dbRecords = new List<DataRow>();
                DataTable theFirstRecord = DS_SqlService.GetRecordByID(index);
                if (null == theFirstRecord || theFirstRecord.Rows.Count == 0)
                {
                    return dbRecords;
                }
                //该首次病程时间
                DateTime captiontime = DateTime.Parse(theFirstRecord.Rows[0]["captiondatetime"].ToString());
                //该首次病程所在科室
                string dept = null == theFirstRecord.Rows[0]["departcode"] ? YD_Common.currentUser.CurrentDeptId : theFirstRecord.Rows[0]["departcode"].ToString().Trim();
                //该病人首页序号
                int noofinpat = int.Parse(theFirstRecord.Rows[0]["noofinpat"].ToString());
                //获取该病人所有的病历记录(包含无效)
                DataTable allRecords = DS_SqlService.GetRecordsByNoofinpatContainDel(noofinpat);
                if (null == allRecords || allRecords.Rows.Count == 0)
                {
                    return dbRecords;
                }
                //获取该病人所有非本科室首程记录(包含无效)
                var otherFirstRecords = allRecords.Select(" firstdailyflag = '1' and departcode <> '" + dept + "' ");
                if (null == otherFirstRecords || otherFirstRecords.Count() == 0)
                {
                    return dbRecords;
                }
                //获取该首程上一个非本科室首程(包含无效)
                DataRow preFirstDaily = otherFirstRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < captiontime).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                //获取该首程下一个非本科室首程(包含无效)
                DataRow nextFirstDaily = otherFirstRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > captiontime).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                //获取该首程下病历时间段
                DateTime dtStart = captiontime;
                DateTime dtEnd = DateTime.Now;
                //开始时间(如果存在上一个首程，则为上一个首程时间；否则为第一个病历时间)
                if (null == preFirstDaily || null == preFirstDaily["captiondatetime"] || string.IsNullOrEmpty(preFirstDaily["captiondatetime"].ToString().Trim()))
                {
                    dtStart = DateTime.Parse(allRecords.Rows[0]["captiondatetime"].ToString());
                }
                else
                {
                    dtStart = DateTime.Parse(preFirstDaily["captiondatetime"].ToString());
                }
                //结束时间(如果存在下一个首程，则为下一个首程时间；否则为最后一个病历时间)
                if (null == nextFirstDaily || null == nextFirstDaily["captiondatetime"] || string.IsNullOrEmpty(nextFirstDaily["captiondatetime"].ToString().Trim()))
                {
                    dtEnd = DateTime.Parse(allRecords.Rows[allRecords.Rows.Count - 1]["captiondatetime"].ToString());
                }
                else
                {
                    dtEnd = DateTime.Parse(nextFirstDaily["captiondatetime"].ToString());
                }

                dbRecords = allRecords.Select(" departcode = '" + dept + "' ").Where(p => DateTime.Parse(p["captiondatetime"].ToString()) >= dtStart && DateTime.Parse(p["captiondatetime"].ToString()) <= dtEnd).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                //获取页面xml拆分出的节点记录
                List<string> editorTimes = DS_BaseService.GetEditEmrContentTimes(record);
                List<string> dbTimes = dbRecords.Select(p => DateTime.Parse(p["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")).ToList();
                var notInTimes = editorTimes.Where(p => !dbTimes.Contains(p)).ToList();
                if (null != notInTimes && notInTimes.Count() > 0)
                {
                    string notInTimesSQL = YD_Common.CombineSQLStringByList(notInTimes);
                    var notInList = DS_SqlService.GetRecordByDateTimes(noofinpat, notInTimesSQL).Select(" departcode = '" + dept + "' ");
                    foreach (DataRow drow in notInList)
                    {
                        dbRecords.Add(drow);
                    }
                    dbRecords = dbRecords.OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                }
                return dbRecords;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            **/
            #endregion

            try
            {
                List<DataRow> dbRecords = new List<DataRow>();
                DataTable theFirstRecord = DS_SqlService.GetRecordByID(index);
                if (null == theFirstRecord || theFirstRecord.Rows.Count == 0)
                {
                    return dbRecords;
                }
                //该首次病程时间
                DateTime captiontime = DateTime.Parse(theFirstRecord.Rows[0]["captiondatetime"].ToString());
                //该首次病程所在科室
                string dept = null == theFirstRecord.Rows[0]["departcode"] ? DS_Common.currentUser.CurrentDeptId : theFirstRecord.Rows[0]["departcode"].ToString().Trim();
                //该病人首页序号
                int noofinpat = int.Parse(theFirstRecord.Rows[0]["noofinpat"].ToString());
                //获取该病人所有的病历记录(包含无效)
                DataTable allRecords = DS_SqlService.GetRecordsByNoofinpatContainDel(noofinpat);
                if (null == allRecords || allRecords.Rows.Count == 0)
                {
                    return dbRecords;
                }
                //获取该病人所有的其它科室病历记录(包含无效)
                var otherRecords = allRecords.Select(" departcode <> '" + dept + "' ");
                //获取该病人所有非本科室首程记录(包含无效)
                var otherFirstRecords = allRecords.Select(" firstdailyflag = '1' and departcode <> '" + dept + "' ");
                if (null == otherFirstRecords || otherFirstRecords.Count() == 0)
                {
                    return dbRecords;
                }
                //获取该首程上一个非本科室首程(包含无效)
                DataRow preFirstDaily = otherFirstRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < captiontime).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                //获取该首程下一个非本科室首程(包含无效)
                DataRow nextFirstDaily = otherFirstRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > captiontime).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                //获取该首程下病历时间段
                DateTime dtStart = captiontime;
                DateTime dtEnd = DateTime.Now;
                //开始时间(如果存在上一个首程，则为上一个其它科室病历时间；否则为第一个病历时间)
                if (null == preFirstDaily || null == preFirstDaily["captiondatetime"] || string.IsNullOrEmpty(preFirstDaily["captiondatetime"].ToString().Trim()))
                {
                    dtStart = DateTime.Parse(allRecords.Rows[0]["captiondatetime"].ToString());
                }
                else
                {
                    DataRow preOtherDaily = otherRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < captiontime).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                    if (null != preOtherDaily && null != preOtherDaily["captiondatetime"])
                    {
                        dtStart = DateTime.Parse(preOtherDaily["captiondatetime"].ToString());
                    }
                    else
                    {
                        dtStart = DateTime.Parse(preFirstDaily["captiondatetime"].ToString());
                    }
                }
                //结束时间(如果存在下一个首程，则为下一个其它科室病历时间；否则为最后一个病历时间)
                if (null == nextFirstDaily || null == nextFirstDaily["captiondatetime"] || string.IsNullOrEmpty(nextFirstDaily["captiondatetime"].ToString().Trim()))
                {
                    dtEnd = DateTime.Parse(allRecords.Rows[allRecords.Rows.Count - 1]["captiondatetime"].ToString());
                }
                else
                {
                    DataRow nextOtherDaily = otherRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > captiontime).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                    if (null != nextOtherDaily && null != nextOtherDaily["captiondatetime"])
                    {
                        dtEnd = DateTime.Parse(nextOtherDaily["captiondatetime"].ToString());
                    }
                    else
                    {
                        dtEnd = DateTime.Parse(nextFirstDaily["captiondatetime"].ToString());
                    }
                }

                dbRecords = allRecords.Select(" valid = 1 and departcode = '" + dept + "' ").Where(p => DateTime.Parse(p["captiondatetime"].ToString()) >= dtStart && DateTime.Parse(p["captiondatetime"].ToString()) <= dtEnd).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                //获取页面xml拆分出的节点记录
                List<string> editorTimes = DS_BaseService.GetEditEmrContentTimes(record);
                List<string> dbTimes = dbRecords.Select(p => DateTime.Parse(p["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")).ToList();
                var notInTimes = editorTimes.Where(p => !dbTimes.Contains(p)).ToList();
                if (null != notInTimes && notInTimes.Count() > 0)
                {
                    string notInTimesSQL = DS_Common.CombineSQLStringByList(notInTimes);
                    var notInList = DS_SqlService.GetRecordByDateTimes(noofinpat, notInTimesSQL).Select(" departcode = '" + dept + "' ");
                    foreach (DataRow drow in notInList)
                    {
                        dbRecords.Add(drow);
                    }
                    dbRecords = dbRecords.OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                }
                return dbRecords;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据首程ID获取该首程对应的所有病历记录
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-12</date>
        /// <param name="index">首程ID</param>
        /// <param name="record">编辑器xml</param>
        /// <returns></returns>
        private List<DataRow> GetThisPageRecordsInTran(int index, XmlDocument record)
        {
            #region 已注释 by cyq 2013-01-12
            /**
            try
            {
                List<DataRow> dbRecords = new List<DataRow>();
                DataTable theFirstRecord = DS_SqlService.GetRecordByIDInTran(index);
                if (null == theFirstRecord || theFirstRecord.Rows.Count == 0)
                {
                    return dbRecords;
                }
                //获取该病人所有首程记录(包含转科)
                DataTable firstRecords = DS_SqlService.GetFirstRecordsByDeptInTran(int.Parse(theFirstRecord.Rows[0]["noofinpat"].ToString()), string.Empty);
                //获取该首程下一个首程
                DataRow drow = firstRecords.Select(" captiondatetime > '" + theFirstRecord.Rows[0]["captiondatetime"].ToString() + "' ").OrderBy(p => p["captiondatetime"]).FirstOrDefault();
                DateTime dtStart = DateTime.Parse(theFirstRecord.Rows[0]["captiondatetime"].ToString());
                DateTime dtEnd = DateTime.Now;
                if (null == drow || null == drow["captiondatetime"] || string.IsNullOrEmpty(drow["captiondatetime"].ToString().Trim()))
                {
                    DataTable allRecords = DS_SqlService.GetRecordsByNoofinpatInTran(int.Parse(theFirstRecord.Rows[0]["noofinpat"].ToString()));
                    if (null != allRecords && allRecords.Rows.Count > 0)
                    {
                        dtEnd = DateTime.Parse(allRecords.Rows[allRecords.Rows.Count - 1]["captiondatetime"].ToString());
                    }
                }
                else
                {
                    dtEnd = DateTime.Parse(drow["captiondatetime"].ToString());
                }

                DataTable dt = DS_SqlService.GetRecordsByTimeDivInTran(int.Parse(theFirstRecord.Rows[0]["noofinpat"].ToString().Trim()), dtStart, dtEnd);

                if (null != dt && dt.Rows.Count > 0)
                {
                    dbRecords = dt.Select(" departcode = '" + (null == theFirstRecord.Rows[0]["departcode"] ? YD_Common.currentUser.CurrentDeptId : theFirstRecord.Rows[0]["departcode"].ToString()) + "' ").OrderBy(p => DateTime.Parse(p["captiondatetime"].ToString())).ToList();
                }
                return dbRecords;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            * **/
            #endregion

            #region 已注释 by cyq 2013-01-22 11:15 修复A->B->A后删除B科室所有病历BUG
            /**
            try
            {
                List<DataRow> dbRecords = new List<DataRow>();
                DataTable theFirstRecord = DS_SqlService.GetRecordByIDInTran(index);
                if (null == theFirstRecord || theFirstRecord.Rows.Count == 0)
                {
                    return dbRecords;
                }
                int noofinpat = int.Parse(theFirstRecord.Rows[0]["noofinpat"].ToString());
                //获取该病人(数据库中)所有的病历记录
                DataTable allRecords = DS_SqlService.GetRecordsByNoofinpatInTran(noofinpat);
                if (null == allRecords || allRecords.Rows.Count == 0)
                {
                    return dbRecords;
                }
                //获取该病人所有首程记录(包含转科)
                var firstRecords = allRecords.Select(" firstdailyflag = '1' ");
                if (null == firstRecords || firstRecords.Count() == 0)
                {
                    return dbRecords;
                }
                //获取该首程上一个首程
                DataRow preFirstDaily = firstRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < DateTime.Parse(theFirstRecord.Rows[0]["captiondatetime"].ToString())).OrderByDescending(p => p["captiondatetime"]).FirstOrDefault();
                //获取该首程下一个首程
                DataRow nextFirstDaily = firstRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > DateTime.Parse(theFirstRecord.Rows[0]["captiondatetime"].ToString())).OrderBy(p => p["captiondatetime"]).FirstOrDefault();
                //获取该首程下病历时间段
                DateTime dtStart = DateTime.Parse(theFirstRecord.Rows[0]["captiondatetime"].ToString());
                DateTime dtEnd = DateTime.Now;
                //开始时间(如果存在上一个首程，则为上一个首程时间；否则为第一个病历时间)
                if (null == preFirstDaily || null == preFirstDaily["captiondatetime"] || string.IsNullOrEmpty(preFirstDaily["captiondatetime"].ToString().Trim()))
                {
                    dtStart = DateTime.Parse(allRecords.Rows[0]["captiondatetime"].ToString());
                }
                else
                {
                    dtStart = DateTime.Parse(preFirstDaily["captiondatetime"].ToString());
                }
                //结束时间(如果存在下一个首程，则为下一个首程时间；否则为最后一个病历时间)
                if (null == nextFirstDaily || null == nextFirstDaily["captiondatetime"] || string.IsNullOrEmpty(nextFirstDaily["captiondatetime"].ToString().Trim()))
                {
                    dtEnd = DateTime.Parse(allRecords.Rows[allRecords.Rows.Count - 1]["captiondatetime"].ToString());
                }
                else
                {
                    dtEnd = DateTime.Parse(nextFirstDaily["captiondatetime"].ToString());
                }
                string dept = null == theFirstRecord.Rows[0]["departcode"] ? YD_Common.currentUser.CurrentDeptId : theFirstRecord.Rows[0]["departcode"].ToString().Trim();
                dbRecords = allRecords.Select(" departcode = '" + dept + "' ").Where(p => DateTime.Parse(p["captiondatetime"].ToString()) >= dtStart && DateTime.Parse(p["captiondatetime"].ToString()) <= dtEnd).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                //获取页面xml拆分出的节点记录
                List<string> editorTimes = DS_BaseService.GetEditEmrContentTimes(record);
                List<string> dbTimes = dbRecords.Select(p => DateTime.Parse(p["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")).ToList();
                var notInTimes = editorTimes.Where(p => !dbTimes.Contains(p)).ToList();
                if (null != notInTimes && notInTimes.Count() > 0)
                {
                    string notInTimesSQL = YD_Common.CombineSQLStringByList(notInTimes);
                    var notInList = DS_SqlService.GetRecordByDateTimesInTran(noofinpat, notInTimesSQL).Select(" departcode = '" + dept + "' ");
                    foreach (DataRow drow in notInList)
                    {
                        dbRecords.Add(drow);
                    }
                    dbRecords = dbRecords.OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                }
                return dbRecords;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            **/
            #endregion

            try
            {
                List<DataRow> dbRecords = new List<DataRow>();
                DataTable theFirstRecord = DS_SqlService.GetRecordByIDInTran(index);
                if (null == theFirstRecord || theFirstRecord.Rows.Count == 0)
                {
                    return dbRecords;
                }
                //该首次病程时间
                DateTime captiontime = DateTime.Parse(theFirstRecord.Rows[0]["captiondatetime"].ToString());
                //该首次病程所在科室
                string dept = null == theFirstRecord.Rows[0]["departcode"] ? DS_Common.currentUser.CurrentDeptId : theFirstRecord.Rows[0]["departcode"].ToString().Trim();
                //该病人首页序号
                int noofinpat = int.Parse(theFirstRecord.Rows[0]["noofinpat"].ToString());
                //获取该病人所有的病历记录(包含无效)
                DataTable allRecords = DS_SqlService.GetRecordsByNoofinpatContainDelInTran(noofinpat);
                if (null == allRecords || allRecords.Rows.Count == 0)
                {
                    return dbRecords;
                }
                //获取该病人所有的其它科室病历记录(包含无效)
                var otherRecords = allRecords.Select(" departcode <> '" + dept + "' ");
                //获取该病人所有非本科室首程记录(包含无效)
                var otherFirstRecords = allRecords.Select(" firstdailyflag = '1' and departcode <> '" + dept + "' ");
                //获取该首程上一个非本科室首程(包含无效)
                DataRow preFirstDaily = otherFirstRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < captiontime).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                //获取该首程下一个非本科室首程(包含无效)
                DataRow nextFirstDaily = otherFirstRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > captiontime).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                //获取该首程下病历时间段
                DateTime dtStart = captiontime;
                DateTime dtEnd = DateTime.Now;
                //开始时间(如果存在上一个首程，则为上一个其它科室病历时间；否则为第一个病历时间)
                if (null == preFirstDaily || null == preFirstDaily["captiondatetime"] || string.IsNullOrEmpty(preFirstDaily["captiondatetime"].ToString().Trim()))
                {
                    dtStart = DateTime.Parse(allRecords.Rows[0]["captiondatetime"].ToString());
                }
                else
                {
                    DataRow preOtherDaily = otherRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < captiontime).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                    if (null != preOtherDaily && null != preOtherDaily["captiondatetime"])
                    {
                        dtStart = DateTime.Parse(preOtherDaily["captiondatetime"].ToString());
                    }
                    else
                    {
                        dtStart = DateTime.Parse(preFirstDaily["captiondatetime"].ToString());
                    }
                }
                //结束时间(如果存在下一个首程，则为下一个其它科室病历时间；否则为最后一个病历时间)
                if (null == nextFirstDaily || null == nextFirstDaily["captiondatetime"] || string.IsNullOrEmpty(nextFirstDaily["captiondatetime"].ToString().Trim()))
                {
                    dtEnd = DateTime.Parse(allRecords.Rows[allRecords.Rows.Count - 1]["captiondatetime"].ToString());
                }
                else
                {
                    DataRow nextOtherDaily = otherRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > captiontime).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                    if (null != nextOtherDaily && null != nextOtherDaily["captiondatetime"])
                    {
                        dtEnd = DateTime.Parse(nextOtherDaily["captiondatetime"].ToString());
                    }
                    else
                    {
                        dtEnd = DateTime.Parse(nextFirstDaily["captiondatetime"].ToString());
                    }
                }

                dbRecords = allRecords.Select(" valid = 1 and departcode = '" + dept + "' ").Where(p => DateTime.Parse(p["captiondatetime"].ToString()) >= dtStart && DateTime.Parse(p["captiondatetime"].ToString()) <= dtEnd).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                //获取页面xml拆分出的节点记录
                List<string> editorTimes = DS_BaseService.GetEditEmrContentTimes(record);
                List<string> dbTimes = dbRecords.Select(p => DateTime.Parse(p["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")).ToList();
                var notInTimes = editorTimes.Where(p => !dbTimes.Contains(p)).ToList();
                if (null != notInTimes && notInTimes.Count() > 0)
                {
                    string notInTimesSQL = DS_Common.CombineSQLStringByList(notInTimes);
                    var notInList = DS_SqlService.GetRecordByDateTimesInTran(noofinpat, notInTimesSQL).Select(" departcode = '" + dept + "' ");
                    foreach (DataRow drow in notInList)
                    {
                        dbRecords.Add(drow);
                    }
                    dbRecords = dbRecords.OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).ToList();
                }
                return dbRecords;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 病历标题前加空格
        /// 注：专为修复loadXML方法丢失空格
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-22</date>
        /// <param name="patid"></param>
        private string RaplaceContentAddSpaces(string content)
        {
            try
            {
                content = content.Replace("<span fontbold=\"1\"></span>", "<span fontbold=\"1\">      </span>");
                content = content.Replace("<span fontbold=\"1\" creator=\"1\"></span>", "<span fontbold=\"1\" creator=\"1\">      </span>");
                content = content.Replace("<span fontbold=\"1\" creator=\"11\"></span>", "<span fontbold=\"1\" creator=\"11\">      </span>");
                content = content.Replace("<span fontbold=\"1\" creator=\"2\"></span>", "<span fontbold=\"1\" creator=\"2\">      </span>");
                content = content.Replace("<span fontbold=\"1\" creator=\"22\"></span>", "<span fontbold=\"1\" creator=\"22\">      </span>");
                content = content.Replace("<span fontbold=\"1\" creator=\"3\"></span>", "<span fontbold=\"1\" creator=\"3\">      </span>");
                content = content.Replace("<span fontbold=\"1\" creator=\"33\"></span>", "<span fontbold=\"1\" creator=\"33\">      </span>");
                content = content.Replace("<span fontbold=\"1\" deleter=\"1\"></span>", "<span fontbold=\"1\" deleter=\"1\">      </span>");
                content = content.Replace("<span fontbold=\"1\" deleter=\"2\"></span>", "<span fontbold=\"1\" deleter=\"2\">      </span>");
                content = content.Replace("<span fontbold=\"1\" deleter=\"3\"></span>", "<span fontbold=\"1\" deleter=\"3\">      </span>");
                return content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据病人病程的CAPTIONDATETIME删除该病程的记
        /// </summary>
        /// <param name="CAPTIONDATETIME"></param>
        /// <returns></returns>
        private void RemoveRecordData(string CAPTIONDATETIME)
        {
            try
            {
                string sql = string.Format(@"delete from recorddetail where CAPTIONDATETIME='{0}'", CAPTIONDATETIME);
                sql_Helper.ExecuteNoneQuery(sql, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetEmrModelInstanceParametersValue(EmrModel model, SqlParameter[] parms, int update, int patid)
        {
            try
            {
                if (model == null) throw new ArgumentNullException("model");
                if (parms == null) throw new ArgumentNullException("parms");
                parms[0].Value = update;
                parms[1].Value = model.InstanceId;
                parms[2].Value = patid;
                parms[3].Value = model.TempIdentity;
                parms[4].Value = model.ModelName;
                //parms[4].Value = model.Description;
                if (model.ModelContent != null)
                {
                    //if (update == 2)
                    //{
                    //    parms[5].Value = DBNull.Value;
                    //}
                    //else
                    //{
                    //    parms[5].Value = ZipEmrXml(model.ModelContent.OuterXml);
                    //}
                    parms[5].Value = " ";
                }
                parms[6].Value = model.ModelCatalog;

                parms[7].Value = model.CreatorXH;
                parms[8].Value = model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                {
                    if (String.IsNullOrEmpty(model.ExaminerXH))
                        parms[9].Value = "";
                    else
                        parms[9].Value = model.ExaminerXH;
                    if (model.ExamineTime > DateTime.MinValue)
                        parms[10].Value = model.ExamineTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                    else
                        parms[10].Value = "";
                }
                parms[11].Value = model.HadSigned ? 1 : 0;
                parms[12].Value = model.Valid ? 1 : 0; // 有效记录
                parms[13].Value = (int)model.State;
                parms[14].Value = model.IsModifiedAfterPrint ? (int)PrintState.ChangedAfterPrint : (model.IsAfterPrint ? (int)PrintState.HadPrinted : (int)PrintState.IsNotPrinted);
                //if (model.State != ExamineState.NotSubmit) 审核属性有值就保存
                parms[15].Value = model.DisplayTime == null ? "" : model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                parms[16].Value = model.FirstDailyEmrModel == true ? "1" : "0";

                //得到拼音和五筆的编码
                GenerateShortCode shortCode = new GenerateShortCode(sql_Helper);
                string[] code = shortCode.GenerateStringShortCode(model.ModelName);
                parms[17].Value = code[0]; //PY
                parms[18].Value = code[1]; //WB

                parms[19].Value = model.IsYiHuanGouTong;
                parms[20].Value = model.IsReadConfigPageSize ? "1" : "0";//【1】
                parms[21].Value = model.DepartCode;
                parms[22].Value = model.WardCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 插入方法
        /// </summary>
        /// edit by Yanqiao.Cai 2012-11-21
        /// add try ... catch
        /// <param name="model"></param>
        /// <param name="patid">住院号</param>
        public int InsertModelInstance(EmrModel model, int patid)
        {
            if (model == null) throw new ArgumentNullException("model");
            int identity = -1;

            try
            {
                ////读取最大记录
                //using (IDataReader reader = sql_Helper.ExecuteReader(Sql_GetMaxFileNo))
                //{
                //    reader.Read();
                //    if (reader.IsDBNull(0))
                //        identity = 0;
                //    else
                //        identity = reader.GetInt32(0) + 1;
                //    model.InstanceId = identity;
                //}
                model.CreateTime = DateTime.Now;

                //给参数赋值
                //model.ModelContent.PreserveWhitespace = true;
                SetEmrModelInstanceParametersValue(model, ModelInstanceParams, 1, patid);
                //插入sql
                DataTable dt = sql_Helper.ExecuteDataTable("usp_emr_patrecfile", ModelInstanceParams, CommandType.StoredProcedure);
                identity = Convert.ToInt32(dt.Rows[0][0]);

                //返回id；
                if (identity >= 0)
                {
                    model.InstanceId = identity;
                    if (model.ModelContent != null && model.ModelContent.OuterXml != "")
                    {
                        //modified by wwj
                        //UpdateEmrContent(ZipEmrXml(model.ModelContent.OuterXml), model.InstanceId.ToString());
                        UpdateEmrContent(model.ModelContent.OuterXml, model.InstanceId.ToString(), patid.ToString(), model.ModelCatalog);
                    }
                    model.IsModified = false;
                }
                //保存时限
                //UpdateQCRules(model, patid);
            }
            catch (Exception ex)
            {
                model.InstanceId = -1;
                System.Diagnostics.Trace.WriteLine("新增数据有误");
                System.Diagnostics.Trace.WriteLine(ex);
                throw ex;
            }
            return identity;
        }

        /// <summary>
        /// 插入方法
        /// </summary>
        /// edit by Yanqiao.Cai 2012-11-21
        /// add try ... catch
        /// <param name="model"></param>
        /// <param name="patid">住院号</param>
        public int InsertModelInstanceInTran(EmrModel model, int patid)
        {
            if (model == null) throw new ArgumentNullException("model");
            int identity = -1;
            try
            {
                model.CreateTime = DateTime.Now;
                //给参数赋值
                List<OracleParameter> paramsList = GetRecordParams(model, patid, EditState.Add);
                //插入sql
                identity = DS_SqlService.InsertRecordInTran(paramsList);

                //返回id；
                if (identity >= 0)
                {
                    model.InstanceId = identity;
                    if (model.ModelContent != null && model.ModelContent.OuterXml != "")
                    {
                        UpdateEmrContent2InTran(model.ModelContent.OuterXml, model.InstanceId.ToString(), patid.ToString(), model.ModelCatalog);
                    }
                    model.IsModified = false;
                }
                //保存时限
                //UpdateQCRules(model, patid);
            }
            catch (Exception ex)
            {
                model.InstanceId = -1;
                System.Diagnostics.Trace.WriteLine("新增数据有误");
                System.Diagnostics.Trace.WriteLine(ex);
                throw ex;
            }
            return identity;
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// edit by Yanqiao.Cai 2012-11-21
        /// add try ... catch
        /// <param name="model"></param>
        /// <param name="patid">住院号</param>
        public void UpdateModelInstance(EmrModel model, int patid)
        {
            try
            {
                if (model == null) throw new ArgumentNullException("model");

                #region 已注释 edit by cyq 2013-03-15 换用新底层更新节点
                SetEmrModelInstanceParametersValue(model, ModelInstanceParams, 2, patid);
                sql_Helper.ExecuteDataTable("usp_emr_patrecfile", ModelInstanceParams, CommandType.StoredProcedure);
                #endregion

                //给参数赋值
                List<OracleParameter> paramsList = GetRecordParams(model, patid, EditState.Edit);
                int num = DS_SqlService.UpdateRecord(paramsList);
                if (num <= 0)
                {
                    return;
                }

                if (model.ModelContent != null && model.ModelContent.OuterXml.Trim() != "")
                {
                    //modified by wwj
                    //UpdateEmrContent(ZipEmrXml(model.ModelContent.OuterXml), model.InstanceId.ToString());

                    UpdateEmrContent(model.ModelContent.OuterXml, model.InstanceId.ToString(), patid.ToString(), model.ModelCatalog);
                }

                model.IsModified = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新方法
        /// </summary>
        /// edit by Yanqiao.Cai 2012-11-21
        /// add try ... catch
        /// <param name="model"></param>
        /// <param name="patid">住院号</param>
        public void UpdateModelInstanceInTran(EmrModel model, int patid)
        {
            try
            {
                if (model == null) throw new ArgumentNullException("model");

                //给参数赋值
                List<OracleParameter> paramsList = GetRecordParams(model, patid, EditState.Edit);
                int num = DS_SqlService.UpdateRecordInTran(paramsList);
                if (num <= 0)
                {
                    return;
                }
                if (model.ModelContent != null && model.ModelContent.OuterXml.Trim() != "")
                {
                    UpdateEmrContent2InTran(model.ModelContent.OuterXml, model.InstanceId.ToString(), patid.ToString(), model.ModelCatalog);
                }

                model.IsModified = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EmrModel LoadModelInstance(int instanceid)
        {
            try
            {
                SqlParameter[] parmas = new SqlParameter[]{
                    new SqlParameter(col_Index, SqlDbType.Int, 32),
                };
                parmas[0].Value = instanceid;
                string sql = @"select ID, NoOfInpat, TEMPLATEID, Name, Content, 
                        SortID, Valid, HasSubmit, HasPrint, Owner, 
                        CreateTime, Auditor, AuditTime, HasSign, CaptionDateTime, 
                        FIRSTDAILYFLAG, isyihuangoutong, isconfigpagesize,departcode,wardcode 
                        from RecordDetail where ID = @ID order by SortID,CaptionDateTime";
                using (IDataReader record = sql_Helper.ExecuteReader(sql, parmas))
                {
                    if (!record.Read()) return null;

                    try
                    {
                        EmrModel model = new EmrModel();
                        model.InstanceId = instanceid;
                        model.TempIdentity = record.GetString(2);
                        model.ModelName = record.GetString(3);
                        model.Description = model.ModelName;
                        model.ModelContent = new System.Xml.XmlDocument();
                        model.ModelContent.PreserveWhitespace = true;
                        string emrDoc = string.Empty;
                        if (!record.IsDBNull(4))
                        {
                            //modified by wwj
                            //emrDoc = UnzipEmrXml(record.GetString(4));
                            emrDoc = record.GetString(4);
                            if (emrDoc.Trim() != "")
                            {
                                model.ModelContent.LoadXml(emrDoc);
                            }
                        }

                        model.ModelCatalog = record.GetString(5);



                        // jlzt (记录状态,1 有效)
                        model.Valid = (record.GetInt16(6) == 1);

                        // tjzt (提交状态)
                        model.State = (ExamineState)record.GetInt16(7);
                        // dyzt(打印状态)
                        if (record.GetInt16(8) == (int)PrintState.HadPrinted) //(cstDylbbh + 1))
                            model.IsAfterPrint = true;
                        else
                            if (record.GetInt16(8) == (int)PrintState.ChangedAfterPrint) //(cstDylbbh + 2))
                            {
                                model.IsAfterPrint = true;
                                model.IsModifiedAfterPrint = true;
                            }
                            else
                                model.IsAfterPrint = false;

                        if (!record.IsDBNull(9) && record.GetString(9) != null)
                            model.CreatorXH = record.GetString(9);   //返回创建者
                        if (!record.IsDBNull(10))
                        {
                            string strTime = record.GetValue(10).ToString();
                            if (!string.IsNullOrEmpty(strTime.Trim()))
                                model.CreateTime = DateTime.Parse(strTime, CultureInfo.CurrentCulture);
                            else
                                model.CreateTime = DateTime.MinValue;
                        }
                        if (!record.IsDBNull(11) && !string.IsNullOrEmpty(record.GetString(11).Trim()))
                            model.ExaminerXH = record.GetString(11);
                        if (!record.IsDBNull(12) && !string.IsNullOrEmpty(record.GetString(12).Trim()))
                            model.ExamineTime = DateTime.Parse(record.GetString(12));
                        model.HadSigned = (!record.IsDBNull(13) && (record.GetInt16(13) == 1));


                        if (!record.IsDBNull(14))
                        {
                            model.DisplayTime = Convert.ToDateTime(record.GetString(14).ToString());
                        }
                        if (!record.IsDBNull(15))
                        {
                            model.FirstDailyEmrModel = record.GetString(15) == "1" ? true : false;
                        }
                        //model.IsShowFileName = record.GetString(16);
                        //model.FileName = record.GetString(17);
                        if (!record.IsDBNull(16))
                        {
                            model.IsYiHuanGouTong = record.GetString(16);
                        }
                        if (!record.IsDBNull(17))//【1】
                        {
                            model.IsReadConfigPageSize = record.GetString(17) == "1" ? true : false;
                        }
                        if (!record.IsDBNull(18))//【1】
                        {
                            model.DepartCode = record.GetString(18);
                        }
                        if (!record.IsDBNull(19))//【1】
                        {
                            model.WardCode = record.GetString(19);
                        }

                        return model;
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Trace.WriteLine("model数据有误");
                        System.Diagnostics.Trace.WriteLine(e);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EmrModel LoadModelInstance(EmrModel model)
        {
            try
            {
                SqlParameter[] parmas = new SqlParameter[]{
                    new SqlParameter(col_Index, SqlDbType.Int, 32),
                };
                parmas[0].Value = model.InstanceId;
                using (IDataReader record = sql_Helper.ExecuteReader(Sql_LoadModelInstance, parmas))
                {
                    if (!record.Read()) return null;

                    try
                    {
                        model.TempIdentity = record.GetString(2);
                        model.ModelName = record.GetString(3);
                        model.Description = model.ModelName;
                        model.ModelContent = new System.Xml.XmlDocument();
                        model.ModelContent.PreserveWhitespace = true;
                        string emrDoc = string.Empty;
                        if (!record.IsDBNull(4))
                        {
                            //modified by wwj
                            //emrDoc = UnzipEmrXml(record.GetString(4));
                            emrDoc = record.GetString(4);
                            model.ModelContent.LoadXml(emrDoc);
                        }

                        model.ModelCatalog = record.GetString(5);

                        // jlzt (记录状态,1 有效)
                        model.Valid = (record.GetInt16(6) == 1);

                        // tjzt (提交状态)
                        model.State = (ExamineState)record.GetInt16(7);
                        // dyzt(打印状态)
                        if (record.GetInt16(8) == (int)PrintState.HadPrinted) //(cstDylbbh + 1))
                            model.IsAfterPrint = true;
                        else
                            if (record.GetInt16(8) == (int)PrintState.ChangedAfterPrint) //(cstDylbbh + 2))
                            {
                                model.IsAfterPrint = true;
                                model.IsModifiedAfterPrint = true;
                            }
                            else
                                model.IsAfterPrint = false;

                        if (record.GetString(9) != null)
                            model.CreatorXH = record.GetString(9);   //返回创建者

                        string strTime = record.GetString(10).Trim();
                        if (!string.IsNullOrEmpty(strTime))
                            model.CreateTime = DateTime.Parse(strTime, CultureInfo.CurrentCulture);
                        else
                            model.CreateTime = DateTime.MinValue;
                        if (!record.IsDBNull(11) && !string.IsNullOrEmpty(record.GetString(11).Trim()))
                            model.ExaminerXH = record.GetString(11);
                        if (!record.IsDBNull(12) && !string.IsNullOrEmpty(record.GetString(12).Trim()))
                            model.ExamineTime = DateTime.Parse(record.GetString(12));
                        model.HadSigned = (!record.IsDBNull(13) && (record.GetInt16(13) == 1));

                        if (!record.IsDBNull(14))
                        {
                            model.DisplayTime = Convert.ToDateTime(record.GetString(14));
                        }
                        if (!record.IsDBNull(15))
                        {
                            model.FirstDailyEmrModel = record.GetString(15) == "1" ? true : false;
                        }

                        //model.IsShowFileName = record.GetString(16);
                        //model.FileName = record.GetString(17);
                        if (!record.IsDBNull(16))
                        {
                            model.IsYiHuanGouTong = record.GetString(16);
                        }

                        if (!record.IsDBNull(17))//【1】
                        {
                            model.IsReadConfigPageSize = record.GetString(17) == "1" ? true : false;
                        }
                        return model;
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Trace.WriteLine("model数据有误");
                        System.Diagnostics.Trace.WriteLine(e);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 作废病历
        /// </summary>
        /// <param name="instanceid"></param>
        public void CancelEmrModel(int instanceid)
        {
            try
            {
                SqlParameter[] parmas = new SqlParameter[]{
                    new SqlParameter(col_Index, SqlDbType.Int, 32),
                };
                parmas[0].Value = instanceid;
                sql_Helper.ExecuteReader(Sql_UpdateModelValid, parmas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 作废病历(逻辑删除)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-31</date>
        /// <param name="instanceid"></param>
        public void CancelEmrModelInTran(int instanceid)
        {
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                OracleParameter param0 = new OracleParameter("id", OracleType.Int32);
                param0.Value = instanceid;
                parameters.Add(param0);
                OracleParameter param1 = new OracleParameter("valid", OracleType.Int32);
                param1.Value = 0;
                parameters.Add(param1);

                DS_SqlService.UpdateRecordInTran(parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// edit by Yanqiao.Cai 2012-12-18
        /// add try ... catch
        /// </summary>
        //private void UpdateQCRules(EmrModel model, int patid)
        //{
        //    try
        //    {
        //        //if (QC_Sqlhelper == null)
        //        //    QC_Sqlhelper = new Qcsv();
        //        //获取监控结果
        //        string qcCode = QueryQCCode(model.TempIdentity);
        //        if (string.IsNullOrEmpty(qcCode))
        //        {
        //            return;
        //        }

        //        QCResult qcResult = QC_Sqlhelper.SelectQCReslut(qcCode);
        //        //更新病历时限相关记录
        //        QC_Sqlhelper.UpdateRuleRecord(patid, model.InstanceId, model.CreatorXH, QCResultType.TimeChange, qcResult, DateTime.Now);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        /// <summary>
        /// 获取当前病历的监控代码
        /// TODO:周辉
        /// 应该将病历的模板的监控代码合并到模型中
        /// 并扩展recodedetail库，增加qccode字段
        /// edit by Yanqiao.Cai 2012-12-18 add try ... catch
        /// </summary>
        /// <param name="templeteid"></param>
        /// <returns></returns>
        private string QueryQCCode(string templeteid)
        {
            try
            {
                DataTable dt = sql_Helper.ExecuteDataTable(string.Format(Sql_GetQCCode, templeteid));
                if (null != dt && dt.Rows.Count > 0)
                {
                    IDataReader reader = sql_Helper.ExecuteReader(string.Format(Sql_GetQCCode, templeteid));
                    if (null != reader && !reader.IsClosed)
                    {
                        using (reader)
                        {
                            reader.Read();
                            return reader.IsDBNull(0) ? string.Empty : reader.GetValue(0).ToString();
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
        /// 另存个人模板
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="memo"></param>
        public bool InsertTemplatePerson(EmrModel model, string userID, string memo, string content, string type, string deptid)
        {
            try
            {
                bool isOK = true;
                SqlParameter[] para = new SqlParameter[11];
                para[0] = new SqlParameter("@TEMPLATEID", SqlDbType.VarChar);
                para[0].Value = model.TempIdentity;
                para[1] = new SqlParameter("@NAME", SqlDbType.VarChar);
                para[1].Value = model.ModelName.Trim().Split(' ')[0];
                para[2] = new SqlParameter("@USERID", SqlDbType.VarChar);
                para[2].Value = userID;
                para[3] = new SqlParameter("@CONTENT", SqlDbType.VarChar, int.MaxValue);
                if (model.ModelContent != null)
                {
                    para[3].Value = string.Empty;
                    //para[3].Value = ZipEmrXml(content);
                }
                para[4] = new SqlParameter("@SORTID", SqlDbType.VarChar);
                para[4].Value = model.ModelCatalog;
                para[5] = new SqlParameter("@MEMO", SqlDbType.VarChar);
                para[5].Value = memo;

                string[] code = GetPYWB(model.ModelName.Trim().Split(' ')[0]);
                para[6] = new SqlParameter("@PY", SqlDbType.VarChar);
                para[6].Value = code[0];
                para[7] = new SqlParameter("@WB", SqlDbType.VarChar);
                para[7].Value = code[1];
                para[8] = new SqlParameter("@TYPE", SqlDbType.VarChar);
                para[8].Value = type;
                //【1】
                para[9] = new SqlParameter("@ISCONFIGPAGESIZE", SqlDbType.VarChar);
                para[9].Value = model.IsReadConfigPageSize ? "1" : "0";

                para[10] = new SqlParameter("@DEPTID", SqlDbType.VarChar);
                para[10].Value = (type == "2") ? deptid : ""; //个人模板不记科室

                DataTable dt = sql_Helper.ExecuteDataTable("EMR_RECORD_INPUT.usp_InsertTemplatePerson", para, CommandType.StoredProcedure);
                int identity = -1;

                identity = Convert.ToInt32(dt.Rows[0][0]);
                //返回id；
                if (identity >= 0)
                {
                    if (!string.IsNullOrEmpty(content))
                    {
                        OracleConnection conn = new OracleConnection(sql_Helper.GetDbConnection().ConnectionString);
                        OracleCommand cmd = conn.CreateCommand();
                        try
                        {
                            conn.Open();
                            cmd.CommandText = "update template_person set content = :data where id =" + identity;
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Clear();
                            OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
                            //paraClob.Value = ZipEmrXml(content); 
                            //另存为模板时未把病历中的空格保存进去
                            //edit by ywk 2013年2月18日11:21:27
                            //paraClob.Value=ZipEmrXml(ClearTempletSaveLog(UnzipEmrXml(ZipEmrXml(content))));
                            paraClob.Value = ZipEmrXml(ClearTempletSaveLog(content));
                            cmd.Parameters.Add(paraClob);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            isOK = false;
                            throw ex;
                        }
                        finally
                        {
                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                }
                return isOK;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 清空模板中savelogs标签中值
        /// add by ywk 2013年2月18日13:27:36 
        /// 为解决将病历另存为模板时空格消失的问题
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        private string ClearTempletSaveLog(string xmlDocument)
        {
            XmlDocument content = new XmlDocument();
            content.PreserveWhitespace = true;
            content.LoadXml(xmlDocument);
            XmlNode xmlelement = content.GetElementsByTagName("savelogs")[0];
            if (xmlelement.InnerXml == "")
            {
                return xmlDocument;
            }
            return xmlDocument.Replace(xmlelement.InnerXml, "");
        }

        /// <summary>
        /// 得到拼音和五笔
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string[] GetPYWB(string name)
        {
            try
            {
                GenerateShortCode shortCode = new GenerateShortCode(sql_Helper);
                string[] code = shortCode.GenerateStringShortCode(name);

                //string py = code[0]; //PY
                //string wb = code[1]; //WB
                return code;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 通过类别码得到具体的类别名称
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public string GetCatalog(string catalog)
        {
            try
            {
                string sqlGetCatalog = " select cname from DICT_CATALOG where ccode = '" + catalog + "' ";
                return sql_Helper.ExecuteScalar(sqlGetCatalog, CommandType.Text).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //获得可替换项
        public DataTable GetReplaceItem(string modelName)
        {
            try
            {
                //string sqlGetReplaceImte = " select * from EMR_REPLACE_ITEM where instr('" + modelName + "',dest_emrname) = 1 " + " and valid = '1' ";

                string sqlGetReplaceImte = " select * from EMR_REPLACE_ITEM where valid = '1'";

                if (!string.IsNullOrEmpty(modelName))
                {
                    sqlGetReplaceImte = sqlGetReplaceImte + " and ( instr('" + modelName + "',dest_emrname) = 1 or dest_emrname is null) ";
                }
                return sql_Helper.ExecuteDataTable(sqlGetReplaceImte, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //通过目标病历获得源病历
        public List<string> GetSourceEMRByDestEmrName(string destEmrModelName, DataTable dt)
        {
            try
            {
                List<string> replaceEmrList = new List<string>();
                if (dt != null)
                {
                    var sourceEMR = (from DataRow dr in dt.Rows
                                     select dr["source_emrname"].ToString()).Distinct();
                    foreach (var q in sourceEMR)
                    {
                        replaceEmrList.Add(q.ToString());
                    }
                }
                return replaceEmrList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //通过目标项和目标病历获得源病历
        public List<string> GetSourceEMRByDestItemAndDestEmrName(string destEmrModelName, string destItem, DataTable dt)
        {
            try
            {
                List<string> list = new List<string>();
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["dest_itemname"].ToString() == destItem)
                        {
                            string source_emrname = dr["source_emrname"].ToString();
                            string source_itemname = dr["source_itemname"].ToString();
                            list.Add(source_emrname);
                            list.Add(source_itemname);
                            break;
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

        public List<EmrModelContainer> GetConsultRecrod(DataRow dr, string noofinpat)
        {
            try
            {
                string sqlConsultRecord = @"select '会诊记录' || ' ' || applytime || ' ' || (select name from users where users.id = applyuser) cname, consultapplysn 
                                        from consultapply where noofinpat={0} and stateid in('6730','6740','6741') ";
                DataTable dt = sql_Helper.ExecuteDataTable(string.Format(sqlConsultRecord, noofinpat), CommandType.Text);
                List<EmrModelContainer> list = new List<EmrModelContainer>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr["CNAME"] = dt.Rows[i]["cname"].ToString();
                    EmrModelContainer container = new EmrModelContainer(dr);
                    container.Args = dt.Rows[i]["consultapplysn"].ToString();
                    list.Add(container);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 用于在服务器上保存病历的副本  Add By wwj 2012-01-10
        /// </summary>
        /// <param name="content"></param>
        /// <param name="indx"></param>
        private void SaveEmrFile(string content, string indx)
        {
            try
            {
                DataTable dt = sql_Helper.ExecuteDataTable(
                    @"SELECT i.noofinpat || '_' || i.name inpatient_name, r.name emr_name 
                    FROM recorddetail r, inpatient i 
                   WHERE r.noofinpat = i.noofinpat AND r.valid = '1' AND r.id =" + indx);
                if (dt.Rows.Count > 0)
                {
                    string inpatientName = dt.Rows[0]["inpatient_name"].ToString();
                    string emrName = dt.Rows[0]["emr_name"].ToString();

                    string config = sql_Helper.ExecuteScalar("SELECT value FROM appcfg WHERE appcfg.configkey = 'EmrContentCopy' AND appcfg.valid = '1'").ToString();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(config);

                    string sourcefoldername = doc.GetElementsByTagName("sourcefoldername")[0].InnerText.Trim();
                    string destfoldername = doc.GetElementsByTagName("destfoldername")[0].InnerText.Trim();
                    string username = doc.GetElementsByTagName("username")[0].InnerText.Trim();
                    string password = doc.GetElementsByTagName("password")[0].InnerText.Trim();

                    string folderName = sourcefoldername;
                    string destFolderName = destfoldername;

                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }
                    folderName += @"\" + inpatientName;
                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }
                    string fileName = folderName + @"\" + "ID=" + indx + " " + emrName.Replace(':', '-') + ".txt";
                    FileStream fs = File.Create(fileName);

                    Byte[] info = new UTF8Encoding(true).GetBytes(content);
                    fs.Write(info, 0, info.Length);
                    fs.Close();

                    CopyDirectory(sourcefoldername, destfoldername, username, password);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 病历副本备份  Add By wwj 2012-01-10

        /// <summary>
        /// 拷贝文件夹到服务器
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private void CopyDirectory(String source, String destination, string user, string pwd)
        {
            try
            {
                //System.Diagnostics.Process.Start("net.exe ", @"use   \\192.168.2.13\病历备份   /user:" + user + "   " + pwd + " ");

                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("net.exe ", @"use   " + destination + "   /user:" + user + "   " + pwd + " ");
                psi.RedirectStandardOutput = true;
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                System.Diagnostics.Process.Start(psi);

                if (Directory.Exists(source) && Directory.Exists(destination))
                {
                    CopyDirectoryInner(source, destination);

                    //拷贝完成后将原目录删除
                    Directory.Delete(source, true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        private void CopyDirectoryInner(String source, String destination)
        {
            DirectoryInfo info = new DirectoryInfo(source);
            FileSystemInfo[] sysInfos = info.GetFileSystemInfos();

            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                string destName = Path.Combine(destination, fsi.Name);
                string srcName = Path.Combine(source, fsi.Name);

                //if file ,then copy it
                if (fsi is System.IO.FileInfo)
                {
                    string sourceName = Path.Combine(source, fsi.Name);
                    //just in case the destination is readonly
                    if (File.Exists(destName))
                    {
                        new FileInfo(destName).IsReadOnly = false;
                    }

                    File.Copy(fsi.FullName, destName, true);
                }
                else if (fsi is System.IO.DirectoryInfo)
                {
                    if (!Directory.Exists(destName))
                    {
                        Directory.CreateDirectory(destName);
                    }
                    CopyDirectoryInner(srcName, destName);
                }
            }
        }
        #endregion

        /// <summary>
        /// 得到诊断事件
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiagButtonEvent()
        {
            try
            {
                DataTable dt = sql_Helper.ExecuteDataTable(sqlGetDiag, CommandType.Text);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private string sqlGetDiag = "SELECT * FROM patdiagtype order by code";

        #region 历史病历相关

        /// <summary>
        /// 得到历史病人记录
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetHistoryInpatient(int noofinpat)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("NOOFINPAT", SqlDbType.VarChar)
                };
                sqlParams[0].Value = noofinpat;
                return sql_Helper.ExecuteDataTable("emr_record_input.usp_getHistoryInpatient", sqlParams, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到历史病人文件夹
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetHistoryInpatientFolder(int noofinpat)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("NOOFINPAT", SqlDbType.VarChar)
                };
                sqlParams[0].Value = noofinpat;
                return sql_Helper.ExecuteDataTable("emr_record_input.usp_getHistoryInpatientFolder", sqlParams, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到病人入院次数
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public string GetHistoryInCount(int noofinpat)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("NOOFINPAT", SqlDbType.VarChar)
                };
                sqlParams[0].Value = noofinpat;
                DataTable dt = sql_Helper.ExecuteDataTable("emr_record_input.usp_getHistoryInCount", sqlParams, CommandType.StoredProcedure);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 得到病人入院次数
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetPatientInfo(int noofinpat)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("NOOFINPAT", SqlDbType.VarChar)
                };
                sqlParams[0].Value = noofinpat;
                return sql_Helper.ExecuteDataTable("emr_record_input.usp_getPatinetInfo", sqlParams, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到病人历史病历
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetPatientHistoryEMR(int noofinpat, string sortid)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("NOOFINPAT", SqlDbType.VarChar),
                    new SqlParameter("sortid", SqlDbType.VarChar)
                };
                sqlParams[0].Value = noofinpat;
                sqlParams[1].Value = sortid;
                return sql_Helper.ExecuteDataTable("emr_record_input.usp_getHistoryEMR", sqlParams, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到具体的病历
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public string GetEmrContentByID(string recorddetailid)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("recorddetailid", SqlDbType.VarChar)
                };
                sqlParams[0].Value = recorddetailid;
                DataTable dt = sql_Helper.ExecuteDataTable("emr_record_input.usp_EmrContentByID", sqlParams, CommandType.StoredProcedure);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["content"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到病人病历的数据
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public int GetEmrCountByNoofinpat(string noofinpat)
        {
            try
            {
                //排除护理病历
                string sqlGetEmrCount = " select count(1) from recorddetail where noofinpat = '{0}' and valid = '1' and sortid not in('AI','AJ','AK') ";
                int inCount = Convert.ToInt32(sql_Helper.ExecuteDataTable(string.Format(sqlGetEmrCount, noofinpat), CommandType.Text).Rows[0][0]);
                return inCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetEmrRecordByNoofinpat(string noofinpat)
        {
            try
            {
                string sqlGetEmrRecordByNoofinpat = @"select id, name from recorddetail where noofinpat = '{0}' and valid = '1' order by sortid, captiondatetime";
                return sql_Helper.ExecuteDataTable(string.Format(sqlGetEmrRecordByNoofinpat, noofinpat));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到个人模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTemplatePersonContent(string id)
        {
            try
            {
                string getTemplatePersonContent = " select template_person.content from template_person where template_person.id = '{0}' ";
                DataTable dt = this.sql_Helper.ExecuteDataTable(string.Format(getTemplatePersonContent, id), CommandType.Text);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取病历参数
        /// </summary>
        /// 注：只插入或更新除病历内容外的项(病历内容单独更新)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-14</date>
        /// <param name="model"></param>
        /// <param name="state">操作状态</param>
        /// <returns></returns>
        public List<OracleParameter> GetRecordParams(EmrModel model, int noofinpat, EditState state)
        {
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                if (model == null)
                {
                    throw new ArgumentNullException("model");
                }

                if (state == EditState.Add)
                {
                    //noofinpat
                    OracleParameter param1 = new OracleParameter("noofinpat", OracleType.Int32);
                    param1.Value = noofinpat;
                    parameters.Add(param1);
                    //templateid
                    OracleParameter param2 = new OracleParameter("templateid", OracleType.VarChar);
                    param2.Value = null == model.TempIdentity ? "" : model.TempIdentity;
                    parameters.Add(param2);
                    //sortid
                    OracleParameter param6 = new OracleParameter("sortid", OracleType.VarChar);
                    param6.Value = null == model.ModelCatalog ? "" : model.ModelCatalog;
                    parameters.Add(param6);
                    //owner
                    OracleParameter param7 = new OracleParameter("owner", OracleType.VarChar);
                    param7.Value = null == model.CreatorXH ? "" : model.CreatorXH;
                    parameters.Add(param7);
                    //auditor(param8) 暂无

                    //createtime
                    OracleParameter param9 = new OracleParameter("createtime", OracleType.VarChar);
                    param9.Value = model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                    parameters.Add(param9);
                    //auditor
                    OracleParameter param10 = new OracleParameter("auditor", OracleType.VarChar);
                    param10.Value = null == model.ExaminerXH ? "" : model.ExaminerXH;
                    parameters.Add(param10);
                    //audittime
                    OracleParameter param11 = new OracleParameter("audittime", OracleType.VarChar);
                    param11.Value = null == model.ExamineTime ? "" : model.ExamineTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                    parameters.Add(param11);
                    //hassubmit
                    OracleParameter param12 = new OracleParameter("hassubmit", OracleType.Int32);
                    param12.Value = (int)model.State;
                    parameters.Add(param12);
                    //hasprint
                    OracleParameter param13 = new OracleParameter("hasprint", OracleType.Int32);
                    param13.Value = model.IsModifiedAfterPrint ? (int)PrintState.ChangedAfterPrint : (model.IsAfterPrint ? (int)PrintState.HadPrinted : (int)PrintState.IsNotPrinted);
                    parameters.Add(param13);
                    //hassign
                    OracleParameter param14 = new OracleParameter("hassign", OracleType.Int32);
                    param14.Value = model.HadSigned ? 1 : 0;
                    parameters.Add(param14);
                    //islock 暂无
                    //firstdailyflag
                    OracleParameter param17 = new OracleParameter("firstdailyflag", OracleType.VarChar);
                    param17.Value = model.FirstDailyEmrModel == true ? "1" : "0";
                    parameters.Add(param17);
                    //isyihuangoutong 
                    OracleParameter param18 = new OracleParameter("isyihuangoutong", OracleType.VarChar);
                    param18.Value = null == model.IsYiHuanGouTong ? "" : model.IsYiHuanGouTong;
                    parameters.Add(param18);
                    //isconfigpagesize
                    OracleParameter param20 = new OracleParameter("isconfigpagesize", OracleType.VarChar);
                    param20.Value = model.IsReadConfigPageSize ? "1" : "0";
                    parameters.Add(param20);

                }
                else if (state == EditState.Edit)
                {
                    //id
                    OracleParameter param0 = new OracleParameter("id", OracleType.Int32);
                    param0.Value = model.InstanceId;
                    parameters.Add(param0);
                }
                //name
                OracleParameter param3 = new OracleParameter("name", OracleType.VarChar);
                param3.Value = null == model.ModelName ? "" : model.ModelName;
                parameters.Add(param3);
                //recorddesc 
                OracleParameter param4 = new OracleParameter("recorddesc", OracleType.VarChar);
                param4.Value = null == model.Description ? "" : model.Description;
                parameters.Add(param4);

                //content(param5)
                //此项在首程中单独更新

                //valid
                OracleParameter param15 = new OracleParameter("valid", OracleType.Int32);
                param15.Value = model.Valid ? 1 : 0;
                parameters.Add(param15);
                //captiondatetime
                OracleParameter param16 = new OracleParameter("captiondatetime", OracleType.VarChar);
                param16.Value = model.DisplayTime == null ? "" : model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                parameters.Add(param16);
                //ip
                OracleParameter param19 = new OracleParameter("ip", OracleType.VarChar);
                param19.Value = DS_Common.GetIPHost();
                parameters.Add(param19);
                //departcode
                OracleParameter param21 = new OracleParameter("departcode", OracleType.VarChar);
                param21.Value = null == model.DepartCode ? "" : model.DepartCode;
                parameters.Add(param21);
                //wardcode
                OracleParameter param22 = new OracleParameter("wardcode", OracleType.VarChar);
                param22.Value = null == model.WardCode ? "" : model.WardCode;
                parameters.Add(param22);
                if (model.FirstDailyEmrModel)
                {
                    //startupdateflag
                    OracleParameter param23 = new OracleParameter("startupdateflag", OracleType.Char);
                    param23.Value = Guid.NewGuid().ToString();
                    parameters.Add(param23);
                    //endupdateflag
                    OracleParameter param24 = new OracleParameter("endupdateflag", OracleType.Char);
                    param24.Value = Guid.NewGuid().ToString();
                    parameters.Add(param24);
                }

                return parameters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取历史记录病历参数
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-07</date>
        /// <param name="index">病历ID</param>
        /// <param name="type">异常类型(0-病程；1-编辑器；2-非病程；)</param>
        /// <returns></returns>
        public List<OracleParameter> GetHistoryRecordParams(DataRow theRecord, int type, EditState state)
        {
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                if (null == theRecord)
                {
                    return parameters;
                }
                if (state == EditState.Add)
                {
                    //noofinpat
                    OracleParameter param1 = new OracleParameter("noofinpat", OracleType.Int32);
                    param1.Value = int.Parse(theRecord["noofinpat"].ToString());
                    parameters.Add(param1);
                    //templateid  
                    OracleParameter param2 = new OracleParameter("templateid", OracleType.VarChar);
                    param2.Value = null == theRecord["templateid"] ? "" : theRecord["templateid"].ToString();
                    parameters.Add(param2);
                    //sortid
                    OracleParameter param6 = new OracleParameter("sortid", OracleType.VarChar);
                    param6.Value = null == theRecord["sortid"] ? "" : theRecord["sortid"].ToString();
                    parameters.Add(param6);
                    //owner
                    OracleParameter param7 = new OracleParameter("owner", OracleType.VarChar);
                    param7.Value = DS_Common.currentUser.Id;
                    parameters.Add(param7);
                    //createtime
                    OracleParameter param9 = new OracleParameter("createtime", OracleType.VarChar);
                    param9.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    parameters.Add(param9);
                    //auditor
                    OracleParameter param10 = new OracleParameter("auditor", OracleType.VarChar);
                    param10.Value = null == theRecord["auditor"] ? "" : theRecord["auditor"].ToString();
                    parameters.Add(param10);
                    //audittime
                    OracleParameter param11 = new OracleParameter("audittime", OracleType.VarChar);
                    param11.Value = null == theRecord["audittime"] ? "" : theRecord["audittime"].ToString();
                    parameters.Add(param11);
                    //hassubmit
                    OracleParameter param12 = new OracleParameter("hassubmit", OracleType.Int32);
                    param12.Value = null == theRecord["hassubmit"] ? 0 : int.Parse(theRecord["hassubmit"].ToString());
                    parameters.Add(param12);
                    //hasprint
                    OracleParameter param13 = new OracleParameter("hasprint", OracleType.Int32);
                    param13.Value = null == theRecord["hasprint"] ? 0 : int.Parse(theRecord["hasprint"].ToString());
                    parameters.Add(param13);
                    //islock 暂无
                    //firstdailyflag
                    OracleParameter param17 = new OracleParameter("firstdailyflag", OracleType.VarChar);
                    param17.Value = null == theRecord["firstdailyflag"] ? "" : theRecord["firstdailyflag"].ToString();
                    parameters.Add(param17);
                    //isyihuangoutong 
                    OracleParameter param18 = new OracleParameter("isyihuangoutong", OracleType.VarChar);
                    param18.Value = null == theRecord["isyihuangoutong"] ? "" : theRecord["isyihuangoutong"].ToString();
                    parameters.Add(param18);
                }
                else if (state == EditState.Edit)
                {
                    //id
                    OracleParameter param0 = new OracleParameter("id", OracleType.Int32);
                    param0.Value = null == theRecord["id"] ? -1 : int.Parse(theRecord["id"].ToString());
                    parameters.Add(param0);
                }
                //name
                OracleParameter param3 = new OracleParameter("name", OracleType.VarChar);
                param3.Value = "(" + (type == 0 ? "病程" : (type == 1 ? "编辑器" : "非病程")) + ") " + (null == theRecord["name"] ? "" : theRecord["name"].ToString());
                parameters.Add(param3);

                //content(param5)
                OracleParameter param5 = new OracleParameter("content", OracleType.Clob);
                param5.Value = null == theRecord["content"] ? "" : theRecord["content"].ToString();
                parameters.Add(param5);

                //valid
                OracleParameter param15 = new OracleParameter("valid", OracleType.Int32);
                param15.Value = null == theRecord["valid"] ? 1 : int.Parse(theRecord["valid"].ToString());
                parameters.Add(param15);
                //captiondatetime
                OracleParameter param16 = new OracleParameter("captiondatetime", OracleType.VarChar);
                param16.Value = null == theRecord["captiondatetime"] ? "" : theRecord["captiondatetime"].ToString();
                parameters.Add(param16);
                //ip
                OracleParameter param19 = new OracleParameter("ip", OracleType.VarChar);
                param19.Value = DS_Common.GetIPHost();
                parameters.Add(param19);
                //departcode
                OracleParameter param21 = new OracleParameter("departcode", OracleType.VarChar);
                param21.Value = null == theRecord["departcode"] ? "" : theRecord["departcode"].ToString();
                parameters.Add(param21);
                //wardcode
                OracleParameter param22 = new OracleParameter("wardcode", OracleType.VarChar);
                param22.Value = null == theRecord["wardcode"] ? "" : theRecord["wardcode"].ToString();
                parameters.Add(param22);
                //opertype 
                OracleParameter param23 = new OracleParameter("opertype", OracleType.VarChar);
                param23.Value = state == EditState.Edit ? "UPDATE" : "INSERT";
                parameters.Add(param23);
                //optime 
                OracleParameter param24 = new OracleParameter("optime", OracleType.DateTime);
                param24.Value = DateTime.Now;
                parameters.Add(param24);

                return parameters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检查病历XML规范性
        /// </summary>
        /// <param name="newEmrContent"></param>
        /// <param name="index">病历ID</param>
        /// <param name="type">0-病历；1-编辑器；2-住院志；</param>
        /// <param name="isInTran">是否为事务</param>
        public void CheckRecordException(string newEmrContent, int index, int type, bool isInTran)
        {
            try
            {
                if (!string.IsNullOrEmpty(newEmrContent.Trim()))
                {
                    XmlDocument saveXML = new XmlDocument();
                    saveXML.LoadXml(newEmrContent);
                }
            }
            catch (Exception)
            {
                ///先获取参数后，再回滚事务。
                ///原因：节点记录未提交，(如果是新增记录)数据库中不存在
                List<OracleParameter> parameters = new List<OracleParameter>();
                DataTable dt = new DataTable();
                if (isInTran)
                {
                    dt = DS_SqlService.GetRecordByIDInTran(index);
                    DS_SqlHelper.AppDbTransaction.Rollback();
                }
                else
                {
                    dt = DS_SqlService.GetRecordByID(index);
                }
                if (null == dt || dt.Rows.Count == 0)
                {
                    return;
                }
                parameters = GetHistoryRecordParams(dt.Rows[0], type, EditState.Add);

                if (type != 0 && null != parameters && parameters.Any(p => p.ParameterName == "content"))
                {
                    OracleParameter param = parameters.FirstOrDefault(p => p.ParameterName == "content");
                    if (null == param.Value || string.IsNullOrEmpty(param.Value.ToString().Trim()))
                    {
                        param.Value = newEmrContent;
                    }
                }

                if (null != parameters && parameters.Count() > 0)
                {
                    DS_SqlService.InsertHistoryRecord(parameters);
                }
                throw new Exception("该病历格式异常，请刷新后重试或联系管理员。");
            }
        }

        #region 小编辑器(新版)相关方法
        /// <summary>
        /// 插入病历内容
        /// </summary>
        /// <param name="model"></param>
        /// <param name="patid">住院号</param>
        public void InsertModelInstanceNew(EmrModel model, int patid)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException("InsertModelInstance方法中参数model不能为空");
                }
                List<OracleParameter> emrlist = GetRecordParamsNew(model, patid, EditState.Add);

                int identity = DS_SqlService.InsertRecord(emrlist);
                if (identity < 0)
                {
                    throw new Exception("保存失败");
                }
                model.InstanceId = identity;
                model.IsModified = false;
            }
            catch (Exception ex)
            {
                model.InstanceId = -1;
                throw ex;
            }
        }

        /// <summary>
        /// 更新病历内容
        /// </summary>
        /// <param name="model"></param>
        /// <param name="patid">住院号</param>
        public void UpdateModelInstanceNew(EmrModel model, int patid)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException("InsertModelInstance方法中参数model不能为空");
                }

                //给参数赋值
                List<OracleParameter> paramsList = GetRecordParamsNew(model, patid, EditState.Edit);
                DS_SqlService.UpdateRecord(paramsList);
                model.IsModified = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取插入病历参数
        /// </summary>
        /// 注：插入项包含病历内容
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-07</date>
        /// <param name="model"></param>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        public List<OracleParameter> GetRecordParamsNew(EmrModel model, int noofinpat, EditState editState)
        {
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                if (model == null)
                {
                    throw new ArgumentNullException("model");
                }

                //id
                OracleParameter param0 = new OracleParameter("id", OracleType.Int32);
                if (editState == EditState.Add)
                {
                    param0.Value = DS_SqlService.GetNextIdFromRecordDetail();
                }
                else
                {
                    param0.Value = model.InstanceId;
                }
                parameters.Add(param0);
                //noofinpat
                OracleParameter param1 = new OracleParameter("noofinpat", OracleType.Int32);
                param1.Value = noofinpat;
                parameters.Add(param1);
                //templateid
                OracleParameter param2 = new OracleParameter("templateid", OracleType.VarChar);
                param2.Value = null == model.TempIdentity ? "" : model.TempIdentity;
                parameters.Add(param2);
                //name
                OracleParameter param3 = new OracleParameter("name", OracleType.VarChar);
                param3.Value = null == model.ModelName ? "" : model.ModelName;
                parameters.Add(param3);
                //recorddesc 
                OracleParameter param4 = new OracleParameter("recorddesc", OracleType.VarChar);
                param4.Value = null == model.Description ? "" : model.Description;
                parameters.Add(param4);
                //content(param5)
                OracleParameter param5 = new OracleParameter("content", OracleType.Clob);
                param5.Value = null == model.ModelContent ? "" : model.ModelContent.OuterXml;
                parameters.Add(param5);
                //sortid
                OracleParameter param6 = new OracleParameter("sortid", OracleType.VarChar);
                param6.Value = null == model.ModelCatalog ? "" : model.ModelCatalog;
                parameters.Add(param6);
                //owner
                OracleParameter param7 = new OracleParameter("owner", OracleType.VarChar);
                param7.Value = null == model.CreatorXH ? "" : model.CreatorXH;
                parameters.Add(param7);
                if ((int)model.State != 4600 && (int)model.State != 4604)
                {
                    //auditor(param8)
                    OracleParameter param8 = new OracleParameter("auditor", OracleType.VarChar);
                    param8.Value = model.ExaminerXH;
                    parameters.Add(param8);
                    //audittime
                    OracleParameter param10 = new OracleParameter("audittime", OracleType.VarChar);
                    param10.Value = model.ExamineTime.ToString("yyyy-MM-dd HH:mm:ss");
                    parameters.Add(param10);
                }
                //createtime
                OracleParameter param9 = new OracleParameter("createtime", OracleType.VarChar);
                param9.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                parameters.Add(param9);
                //islock 暂无
                if (editState == EditState.Add)
                {
                    OracleParameter param11 = new OracleParameter("islock", OracleType.VarChar);
                    param11.Value = "4700";
                    parameters.Add(param11);
                }
                //hassubmit
                OracleParameter param12 = new OracleParameter("hassubmit", OracleType.Int32);
                param12.Value = (int)model.State;
                parameters.Add(param12);
                //hasprint
                OracleParameter param13 = new OracleParameter("hasprint", OracleType.Int32);
                param13.Value = model.IsModifiedAfterPrint ? (int)PrintState.ChangedAfterPrint : (model.IsAfterPrint ? (int)PrintState.HadPrinted : (int)PrintState.IsNotPrinted);
                parameters.Add(param13);
                //hassign
                OracleParameter param14 = new OracleParameter("hassign", OracleType.Int32);
                param14.Value = model.HadSigned ? 1 : 0;
                parameters.Add(param14);
                //valid
                OracleParameter param15 = new OracleParameter("valid", OracleType.Int32);
                param15.Value = model.Valid ? 1 : 0;
                parameters.Add(param15);
                //captiondatetime
                OracleParameter param16 = new OracleParameter("captiondatetime", OracleType.VarChar);
                param16.Value = model.DisplayTime == null ? "" : model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                parameters.Add(param16);
                //firstdailyflag
                OracleParameter param17 = new OracleParameter("firstdailyflag", OracleType.VarChar);
                param17.Value = model.FirstDailyEmrModel == true ? "1" : "0";
                parameters.Add(param17);
                //isyihuangoutong 
                OracleParameter param18 = new OracleParameter("isyihuangoutong", OracleType.VarChar);
                param18.Value = null == model.IsYiHuanGouTong ? "" : model.IsYiHuanGouTong;
                parameters.Add(param18);
                //ip
                OracleParameter param19 = new OracleParameter("ip", OracleType.VarChar);
                param19.Value = DS_Common.GetIPHost();
                parameters.Add(param19);
                //isconfigpagesize
                OracleParameter param20 = new OracleParameter("isconfigpagesize", OracleType.VarChar);
                param20.Value = model.IsReadConfigPageSize ? "1" : "0";
                parameters.Add(param20);
                //departcode
                OracleParameter param21 = new OracleParameter("departcode", OracleType.VarChar);
                param21.Value = null == model.DepartCode ? "" : model.DepartCode;
                parameters.Add(param21);
                //wardcode
                OracleParameter param22 = new OracleParameter("wardcode", OracleType.VarChar);
                param22.Value = null == model.WardCode ? "" : model.WardCode;
                parameters.Add(param22);
                //changeid
                OracleParameter param23 = new OracleParameter("changeid", OracleType.Int32);
                if (null == model.DeptChangeID || string.IsNullOrEmpty(model.DeptChangeID.Trim()))
                {
                    if (model.DepartCode == null)
                        param23.Value = -1;
                    else
                        param23.Value = DS_SqlService.GetInpChangeInfoID(noofinpat, model.DepartCode);
                }
                else
                {
                    param23.Value = Convert.ToInt32(model.DeptChangeID.Trim());
                }
                parameters.Add(param23);

                return parameters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 加载病历
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public EmrModel LoadModelInstanceNew(EmrModel model)
        {
            try
            {
                if (null == model)
                {
                    return model;
                }
                DataTable dt = DS_SqlService.GetRecordByIDContainsDel(model.InstanceId);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return model;
                }

                return SetEmrModel(model, dt.Rows[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmrModel SetEmrModel(EmrModel model, DataRow row)
        {
            try
            {
                if (null == row)
                {
                    return model;
                }
                model.TempIdentity = null == row["templateid"] ? string.Empty : row["templateid"].ToString();
                model.ModelName = null == row["name"] ? string.Empty : row["name"].ToString();
                model.Description = null == row["recorddesc"] ? string.Empty : row["recorddesc"].ToString();
                model.ModelContent = new XmlDocument();
                model.ModelContent.PreserveWhitespace = true;
                if (null != row["content"] && !string.IsNullOrEmpty(row["content"].ToString().Trim()))
                {
                    model.ModelContent.LoadXml(row["content"].ToString());
                }
                model.ModelCatalog = null == row["sortid"] ? string.Empty : row["sortid"].ToString();
                model.Valid = null != row["valid"] && row["valid"].ToString() == "1";
                model.State = (ExamineState)int.Parse(row["hassubmit"].ToString());
                if (null != row["hasprint"] && !string.IsNullOrEmpty(row["hasprint"].ToString().Trim()) && Tool.IsInt(row["hasprint"].ToString()))
                {
                    if (int.Parse(row["hasprint"].ToString()) == (int)PrintState.HadPrinted)
                    {
                        model.IsAfterPrint = true;
                    }
                    else if (int.Parse(row["hasprint"].ToString()) == (int)PrintState.ChangedAfterPrint)
                    {
                        model.IsAfterPrint = true;
                        model.IsModifiedAfterPrint = true;
                    }
                    else
                    {
                        model.IsAfterPrint = false;
                    }
                }
                else
                {
                    model.IsAfterPrint = false;
                }
                model.CreatorXH = null == row["owner"] ? string.Empty : row["owner"].ToString();
                model.CreateTime = (null == row["createtime"] || string.IsNullOrEmpty(row["createtime"].ToString().Trim())) ? DateTime.MinValue : DateTime.Parse(row["createtime"].ToString());
                model.ExaminerXH = null == row["auditor"] ? string.Empty : row["auditor"].ToString();
                model.ExamineTime = (null == row["audittime"] || string.IsNullOrEmpty(row["audittime"].ToString().Trim())) ? DateTime.MinValue : DateTime.Parse(row["audittime"].ToString());
                model.HadSigned = null != row["hassign"] && !string.IsNullOrEmpty(row["hassign"].ToString().Trim()) && Tool.IsInt(row["hassign"].ToString()) && int.Parse(row["hassign"].ToString()) == 1;
                model.DisplayTime = (null == row["captiondatetime"] || string.IsNullOrEmpty(row["captiondatetime"].ToString().Trim())) ? DateTime.MinValue : DateTime.Parse(row["captiondatetime"].ToString());
                model.FirstDailyEmrModel = (null != row["firstdailyflag"] && row["firstdailyflag"].ToString().Trim() == "1") ? true : false;
                model.IsYiHuanGouTong = null == row["isyihuangoutong"] ? string.Empty : row["isyihuangoutong"].ToString();
                model.IsReadConfigPageSize = (null != row["isconfigpagesize"] && row["isconfigpagesize"].ToString().Trim() == "1") ? true : false;
                model.DepartCode = null == row["departcode"] ? string.Empty : row["departcode"].ToString().Trim();
                model.WardCode = null == row["wardcode"] ? string.Empty : row["wardcode"].ToString().Trim();
                model.DeptChangeID = (row.Table.Columns.Contains("changeid") && null != row["changeid"]) ? row["changeid"].ToString().Trim() : string.Empty;

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取插入转科记录参数
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-07</date>
        /// <param name="model"></param>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        public List<OracleParameter> GetInsertChangeDeptParamsNew(EmrModel model, int noofinpat)
        {
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                if (model == null)
                {
                    throw new ArgumentNullException("model");
                }

                //id
                OracleParameter param0 = new OracleParameter("id", OracleType.Int32);
                param0.Value = DS_SqlService.GetNextIdFromInpChangeDept();
                parameters.Add(param0);
                //noofinpat
                OracleParameter param1 = new OracleParameter("noofinpat", OracleType.Int32);
                param1.Value = noofinpat;
                parameters.Add(param1);
                //newdeptid
                OracleParameter param2 = new OracleParameter("newdeptid", OracleType.VarChar);
                param2.Value = null == model.DepartCode ? "" : model.DepartCode;
                parameters.Add(param2);
                //newwardid
                OracleParameter param3 = new OracleParameter("newwardid", OracleType.VarChar);
                param3.Value = null == model.WardCode ? "" : model.WardCode;
                parameters.Add(param3);
                //newbedid 
                OracleParameter param4 = new OracleParameter("newbedid", OracleType.VarChar);
                param4.Value = DS_SqlService.GetInpatientOutBedNo(noofinpat);
                parameters.Add(param4);
                //changestyle
                OracleParameter param5 = new OracleParameter("changestyle", OracleType.Char);
                param5.Value = "1";
                parameters.Add(param5);
                //createuser
                OracleParameter param6 = new OracleParameter("createuser", OracleType.VarChar);
                param6.Value = null == model.CreatorXH ? "" : model.CreatorXH;
                parameters.Add(param6);
                //createtime
                OracleParameter param7 = new OracleParameter("createtime", OracleType.VarChar);
                param7.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                parameters.Add(param7);
                //valid
                OracleParameter param8 = new OracleParameter("valid", OracleType.Int32);
                param8.Value = model.Valid ? 1 : 0;
                parameters.Add(param8);

                return parameters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
