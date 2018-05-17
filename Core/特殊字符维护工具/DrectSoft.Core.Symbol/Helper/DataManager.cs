using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Net;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Common.Object2Editor.Encoding;
using YidanSoft.Common.Controls;
using System.IO.Compression;


namespace YidanSoft.Core.Symbol
{
    /// <summary>
    /// 数据操作
    /// </summary>
    class DataManager
    {
        private static IYidanEmrHost m_app;
        private static IDataAccess m_SqlHelper;

        #region ctor
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="app"></param>
        /// <param name="sqlHelper"></param>

        public DataManager(IYidanEmrHost app, IDataAccess sqlHelper)
        {
            m_app = app;
            m_SqlHelper = sqlHelper;

        }

        public DataTable GetSymbolType()
        {
            string sql = string.Format(@"SELECT symbolCategory.ID,symbolCategory.Name,symbolCategory.Memo FROM SymbolCategory symbolCategory");
            return m_SqlHelper.ExecuteDataTable(sql);

        }


        public DataTable GetSymbolDetail(string TypeID)
        {
            string sql = string.Format(@"SELECT symbols.ID,symbols.RTF,symbols.RTF RTFstr,symbols.CategroyID,symbols.Length,symbols.Memo 
                                            FROM Symbols symbols WHERE symbols.CategroyID = '{0}'", TypeID);


            return m_SqlHelper.ExecuteDataTable(sql);

            //return UnZipTable(m_SqlHelper.ExecuteDataTable(sql));
        }
        /// <summary>
        /// 新增特殊符号类别
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Memo"></param>
        public bool AddSymbolType(string Name, string Memo)
        {
            try
            {
                SqlParameter[] sqlParam1 = new SqlParameter[]
                {
                    new SqlParameter("@type",SqlDbType.VarChar),
                    new SqlParameter("@SymbolCategoryName",SqlDbType.VarChar),
                    new SqlParameter("@SymbolCategoryMemo",SqlDbType.VarChar)
                };
                sqlParam1[0].Value = "InsertSymCategory";
                sqlParam1[1].Value = Name;
                sqlParam1[2].Value = Memo;

                m_SqlHelper.ExecuteDataSet("usp_SymbolManager", sqlParam1, CommandType.StoredProcedure);

                m_app.CustomMessageBox.MessageShow("新增成功");
                return true;
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("保存失败:" + ex.Message + "", CustomMessageBoxKind.ErrorOk);
                return false;
            }

            //            try
            //            {
            //                string sql = string.Format(@"INSERT INTO SymbolCategory
            //                                        ( Name, Memo )
            //                                        VALUES
            //                                        ( '{0}'   /* Name	*/,
            //	                                      '{1}'   /* Memo	*/  )",
            //                                         Name,
            //                                         Memo);


            //                m_SqlHelper.ExecuteNoneQuery(sql);

            //                m_app.CustomMessageBox.MessageShow("新增成功");
            //                return true;

            //            }
            //            catch (Exception ex)
            //            {
            //                m_app.CustomMessageBox.MessageShow("保存失败:" + ex.Message + "", CustomMessageBoxKind.ErrorOk);
            //                return false;
            //            }
        }

        /// <summary>
        /// 修改特殊符号类别
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="Memo"></param>
        public bool EditSymbolType(string ID, string Name, string Memo)
        {
            try
            {
                string sql = string.Format(@"UPDATE SymbolCategory 
                                                SET Name = '{0}',
                                                Memo = '{1}'
                                                WHERE ID ='{2}'",
                                                             Name,
                                                             Memo, ID);


                m_SqlHelper.ExecuteNoneQuery(sql);

                m_app.CustomMessageBox.MessageShow("修改成功");

                return true;

            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("修改失败:" + ex.Message + "", CustomMessageBoxKind.ErrorOk);
                return false;
            }
        }

        /// <summary>
        /// 删除特别字符类别
        /// </summary>
        /// <param name="ID"></param>
        public bool DelSymbolType(string ID)
        {
            try
            {
                string sql = string.Format(@"SELECT * FROM Symbols symbols WHERE symbols.CategroyID = '{0}'", ID);

                DataTable dt = m_SqlHelper.ExecuteDataTable(sql);

                if (dt.Rows.Count > 0)
                {
                    if (m_app.CustomMessageBox.MessageShow(string.Format("该类别下有【{0}】个特殊符号，删除类别则该类别下特殊符号将一起删除，是否删除？", dt.Rows.Count), CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
                    {
                        try
                        {
                            //开启事务
                            m_SqlHelper.BeginTransaction();
                            sql = string.Format(@"delete FROM SymbolCategory  WHERE ID = '{0}'", ID);
                            m_SqlHelper.ExecuteNoneQuery(sql);
                            sql = string.Format(@"delete FROM Symbols WHERE CategroyID = '{0}'", ID);
                            m_SqlHelper.ExecuteNoneQuery(sql);
                            //提交事务
                            m_SqlHelper.CommitTransaction();
                            m_app.CustomMessageBox.MessageShow("删除成功");
                            return true;
                        }
                        catch
                        {
                            m_SqlHelper.RollbackTransaction();
                            throw;
                        }
                    }
                    else
                        return false;
                }
                else
                {
                    sql = string.Format(@"delete FROM SymbolCategory  WHERE ID = '{0}'", ID);
                    m_SqlHelper.ExecuteNoneQuery(sql);
                    m_app.CustomMessageBox.MessageShow("删除成功");
                    return true;
                }
            }
            catch
            {
                m_app.CustomMessageBox.MessageShow("删除失败");
                return false;
            }
        }

        /// <summary>
        /// 新增特殊符号
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="RTF"></param>
        /// <param name="CategoryID"></param>
        /// <param name="Length"></param>
        /// <param name="Memo"></param>
        public bool ADDSymbols(string ID, string RTF, string CategoryID, string Length, string Memo)
        {
            try
            {
                //RTF = ZipContent(RTF);
                SqlParameter[] parmas = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.Int),
                new SqlParameter("@RTF", SqlDbType.NVarChar,4000),
                new SqlParameter("@CategoryID", SqlDbType.NVarChar),
                new SqlParameter("@Length", SqlDbType.NVarChar),
                new SqlParameter("@Memo", SqlDbType.NVarChar),
                 };
                parmas[0].Value = ID;
                parmas[1].Value = RTF;
                parmas[2].Value = CategoryID;
                parmas[3].Value = Length;
                parmas[4].Value = Memo;

                string sql = string.Format(@"INSERT INTO Symbols(ID,RTF,CategroyID,Length,Memo) VALUES(@ID,@RTF,@CategoryID, @Length,@Memo)");


                m_SqlHelper.ExecuteNoneQuery(sql, parmas);
                //RtfPrintNativeMethods.SetSelectionSubscript(

                //RtfEncoding.GetRtfCoreContent(editor.Rtf).Replace(RtfEncoding.RtfNewLine, "");// EmrSymbolEngine.EjectorRTFString(editor);
                m_app.CustomMessageBox.MessageShow("新增成功");
                return true;

            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("保存失败:" + ex.Message + "", CustomMessageBoxKind.ErrorOk);
                return false;
            }
        }

        /// <summary>
        /// 修改特殊符号类别
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="Memo"></param>
        public bool EditSymbols(string ID, string RTF, string CategoryID, string Length, string Memo)
        {
            try
            {
                SqlParameter[] parmas = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.Int),
                new SqlParameter("@RTF", SqlDbType.NVarChar,4000),
                new SqlParameter("@CategoryID", SqlDbType.NVarChar),
                new SqlParameter("@Length", SqlDbType.NVarChar),
                new SqlParameter("@Memo", SqlDbType.VarChar),
                 };
                parmas[0].Value = ID;
                parmas[1].Value = RTF;
                parmas[2].Value = CategoryID;
                parmas[3].Value = Length;
                parmas[4].Value = Memo;

                string sql = string.Format(@"UPDATE Symbols
                                                SET
                                                RTF = @RTF,
                                                CategroyID = @CategoryID,
                                                Length = @Length,
                                                Memo = @Memo
                                             where ID =@ID");


                m_SqlHelper.ExecuteNoneQuery(sql, parmas);

                m_app.CustomMessageBox.MessageShow("修改成功");
                return true;

            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("修改失败:" + ex.Message + "", CustomMessageBoxKind.ErrorOk);
                return false;
            }
        }

        /// <summary>
        /// 删除特别字符类别
        /// </summary>
        /// <param name="ID"></param>
        public bool DelSymbols(string ID)
        {
            try
            {

                string sql = string.Format(@"delete FROM Symbols WHERE ID = '{0}'", ID);

                m_SqlHelper.ExecuteNoneQuery(sql);

                m_app.CustomMessageBox.MessageShow("删除成功");
                return true;
            }
            catch
            {
                m_app.CustomMessageBox.MessageShow("删除失败");
                return false;
            }
        }

        /// <summary>
        /// 获取最新特殊字符编码
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public string GetNewSymbolID(string CategoryID)
        {

            SqlParameter[] sqlParam1 = new SqlParameter[]
                {
                    new SqlParameter("@type",SqlDbType.VarChar),
                    new SqlParameter("@SymbolCategroyID",SqlDbType.VarChar)
 
                };
            sqlParam1[0].Value = "SelectSymbolID";
            sqlParam1[1].Value = CategoryID;


            DataTable dt = m_SqlHelper.ExecuteDataSet("usp_SymbolManager", sqlParam1, CommandType.StoredProcedure).Tables[0];


            return dt.Rows[0][0].ToString();
            //string sql = string.Format(@"SELECT isnull((SELECT max(a.ID) + 1 FROM Symbols a WHERE a.CategroyID = '{0}'),{0}*1000+0)", CategoryID);
            //return m_SqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString();
        }

        /// <summary>
        /// 获取最新特殊字符类别编码
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public string GetNewSymbolCategoryID()
        {

            SqlParameter[] sqlParam1 = new SqlParameter[]
                {
                    new SqlParameter("@type",SqlDbType.VarChar)
 
                };
            sqlParam1[0].Value = "SelectSymCategoryID";

            DataTable dt = m_SqlHelper.ExecuteDataSet("usp_SymbolManager", sqlParam1, CommandType.StoredProcedure).Tables[0];
            return dt.Rows[0][0].ToString();

            //string sql = string.Format(@"SELECT isnull((SELECT max(a.ID) + 1 FROM SymbolCategory a ),1)");
            //return m_SqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString();
        }

        private string UnzipContent(string emrContent)
        {
            try
            {
                if (emrContent == string.Empty)
                    return "";
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

        private string ZipContent(string emrContent)
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


        private DataTable UnZipTable(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dr["RTF"] = UnzipContent(dr["RTF"].ToString());
            }
            return dt;
        }


        #endregion
    }
}
