using DrectSoft.Core;
using DrectSoft.Emr.Web.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace DrectSoft.Emr.Web.Business.Service
{
    public class NewsPub
    {
        /// <summary>
        /// DB操作相关
        /// </summary>
        IDataAccess m_SqlHelper = DataAccessFactory.GetSqlDataAccess("EMRDB");

        #region SqlString

        /// <summary>
        /// SQL语句
        /// </summary>
        const string sql_Select = " Select ID,ClassName,Summary From NewsArticle_Class ";
        const string sql_SelectById = " Select ID,ClassName,Summary From NewsArticle_Class where ID=@ID ";
        const string sql_Insert = " Insert into NewsArticle_Class(ID,ClassName,Summary) Values(@ID,@ClassName,@Summary)";
        const string sql_DelById = " Delete From NewsArticle_Class Where ID=@ID ";
        const string sql_UpdateById = " Update NewsArticle_Class set ClassName=@ClassName,Summary=@Summary Where ID=@ID ";
        const string sql_SelectDept = "Select ID,Name,Py,Valid From Department Where Valid=1";
        const string sql_SelectByPy = "Select ID,Name,Py,Valid From Department Where Valid=1 and Py like'%@Py%'";
        /// <summary>
        /// 字段列名
        /// </summary>
        const string col_ID = "ID";
        const string col_ClassName = "ClassName";
        const string col_Summary = "Summary";

        /// <summary>
        /// 参数
        /// </summary>
        const string param_ID = "ID";
        const string param_ClassName = "ClassName";
        const string param_Summary = "Summary";
        const string param_py = "Py";
        #endregion

        #region NewsClass Function
        /// <summary>
        /// 取得所有分类
        /// </summary>
        /// <returns></returns>
        public DataSet SelectAll()
        {
            return m_SqlHelper.ExecuteDataSet(sql_Select);
        }
        /// <summary>
        /// 取得分类集合
        /// </summary>
        /// <returns></returns>
        public IList<NewsClass> GetClassList()
        {
            IList<NewsClass> newsClassList = new List<NewsClass>();
            DataSet dsClass = SelectAll();
            if (dsClass != null && dsClass.Tables.Count > 0)
            {
                DataTable dtClass = dsClass.Tables[0];
                for (int i = 0; i < dtClass.Rows.Count; i++)
                    newsClassList.Add(DataRowNewsClass(dtClass.Rows[i]));
            }
            return newsClassList;
        }
        /// <summary>
        /// 取得指定分类
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public NewsClass GetClassById(string Id)
        {
            SqlParameter paramClassId = new SqlParameter(param_ID, SqlDbType.VarChar, 34);
            paramClassId.Value = Id;
            DataTable dtClass = m_SqlHelper.ExecuteDataTable(sql_SelectById
                , new SqlParameter[] { paramClassId });
            if (dtClass == null || dtClass.Rows.Count == 0) return null;
            DataRow drClass = dtClass.Rows[0];
            return DataRowNewsClass(drClass);
        }
        NewsClass DataRowNewsClass(DataRow dataRow)
        {
            if (dataRow == null) throw new ArgumentNullException("dataRow");
            string Id = dataRow[col_ID].ToString();
            string className = dataRow[col_ClassName].ToString();
            string summary = dataRow[col_Summary].ToString();
            NewsClass news = new NewsClass(new Guid(Id), className, summary);
            return news;
        }
        public void InsertClass(NewsClass newsClass, string Action)
        {
            if (newsClass == null) throw new ArgumentNullException("newsClass");
            SqlParameter[] insertParam = InitNewsClass(newsClass, "");
            m_SqlHelper.ExecuteNoneQuery(sql_Insert, insertParam);
        }
        public void UpdateClass(NewsClass newsClass, string Action)
        {
            if (newsClass == null) throw new ArgumentNullException("newsClass");
            SqlParameter[] updateParam = InitNewsClass(newsClass, "modify");
            m_SqlHelper.ExecuteNoneQuery(sql_UpdateById, updateParam);

        }
        public void DeleteClass(NewsClass newsClass)
        {
            if (newsClass == null) throw new ArgumentNullException("newsClass");
            SqlParameter paramClassId = new SqlParameter(param_ID, SqlDbType.VarChar, 34);
            paramClassId.Value = newsClass.ID;
            m_SqlHelper.ExecuteNoneQuery(sql_DelById, new SqlParameter[] { paramClassId });
        }
        SqlParameter[] InitNewsClass(NewsClass newclass, string Action)
        {
            SqlParameter paramid = new SqlParameter(param_ID, SqlDbType.VarChar, 34);
            SqlParameter paramClassName = new SqlParameter(param_ClassName, SqlDbType.VarChar, 100);
            SqlParameter paramSummary = new SqlParameter(param_Summary, SqlDbType.VarChar, 200);
            if (Action == "modify")
            {
                paramid.Value = newclass.ID;
            }
            else if (Action == "")
            {
                paramid.Value = Guid.NewGuid().ToString();
            }

            paramClassName.Value = newclass.ClassName;
            paramSummary.Value = newclass.Summary;
            return new SqlParameter[] { paramid, paramClassName, paramSummary };
        }
        #endregion

        #region News Function
        /// <summary>
        /// 取得所有分类
        /// </summary>
        /// <returns></returns>
        public DataSet SelectAllDept()
        {
            return m_SqlHelper.ExecuteDataSet(sql_SelectDept);
        }
        public DataSet SelectAllDeptByPy(string Py)
        {
            SqlParameter paramPy = new SqlParameter(param_py, SqlDbType.VarChar, 8);
            paramPy.Value = Py;
            return m_SqlHelper.ExecuteDataSet(sql_SelectByPy, new SqlParameter[] { paramPy });
        }
        #endregion

        #region Common
        /// <summary>
        /// 显示消息提示对话框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void Show(System.Web.UI.Page page, string msg)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript'>alert('" + msg.ToString() + "');</script>");

        }
        public static void ShowAlert(string message, string url)
        {
            if (message == null)
                message = "";
            message = message.Replace("  ", "");
            System.Web.HttpContext.Current.Response.Write("<script>alert('" + message + "');location='" + url + "';</script>");
        }
        public static void ShowConfirmAlert(string message, string confirmurl, string cancelurl)
        {
            if (message == null)
                message = "";
            message = message.Replace("  ", "");
            System.Web.HttpContext.Current.Response.Write("<script Language='Javascript'>if( confirm('" + message + "') ) {document.location.href='" + confirmurl + "'; } else { document.location.href='" + cancelurl + "' }</script>");
        }
        public static void ShowConfirmAlert(string message, string confirmurl)
        {
            if (message == null)
                message = "";
            message = message.Replace("  ", "");
            System.Web.HttpContext.Current.Response.Write("<script Language='Javascript'>if( confirm('" + message + "') ) {document.location.href='" + confirmurl + "'; } else { window.history.back(); }</script>");
        }

        public string UnzipContent(string emrContent)
        {
            try
            {
                byte[] rbuff = Convert.FromBase64String(emrContent);
                MemoryStream ms = new MemoryStream(rbuff);
                DeflateStream dfs = new DeflateStream(ms, CompressionMode.Decompress, true);
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




        public string ZipContent(string emrContent)
        {
            byte[] buffUnzipXml = Encoding.UTF8.GetBytes(emrContent);
            MemoryStream ms = new MemoryStream();
            DeflateStream dfs = new DeflateStream(ms, CompressionMode.Compress, true);
            dfs.Write(buffUnzipXml, 0, buffUnzipXml.Length);
            dfs.Close();
            ms.Seek(0, SeekOrigin.Begin);
            byte[] buffZipXml = new byte[ms.Length];
            ms.Read(buffZipXml, 0, buffZipXml.Length);
            return Convert.ToBase64String(buffZipXml);
        }

        #endregion

    }
}
