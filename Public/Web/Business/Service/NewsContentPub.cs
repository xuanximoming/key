using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DrectSoft.Core;
using DrectSoft.FrameWork;
using System.Data;
using System.Data.SqlClient;
using DrectSoft.Emr.Web.Business.Entities;

namespace DrectSoft.Emr.Web.Business.Service
{
    public class NewsContentPub
    {
        /// <summary>
        /// DB操作相关
        /// </summary>
        IDataAccess m_SqlHelper = DataAccessFactory.DefaultDataAccess;

        #region SQLString
        const string sql_Select = " SELECT ID,Title,Author,AddTime,DepartMentID,ClassID,DefaultImage,Content,Valid FROM NewsArticle Where Valid=1";
        const string sql_SelectById = " SELECT  ID , Title , Author , AddTime , DepartMentID , ClassID , DefaultImage , Content , Valid  FROM  NewsArticle  Where  Valid =1 and ID=@ID ";
        const string sql_Insert = " Insert into NewsArticle(ID,Title,Author,AddTime,DepartMentID,ClassID,DefaultImage,Content,Valid) Values(@ID,@Title,@Author,@AddTime,@DepartMentID,@ClassID,@DefaultImage,@Content,@Valid)";
        const string sql_DelById = " Delete From NewsArticle Where ID=@ID ";
        const string sql_UpdateById = " Update NewsArticle set Title=@Title,DepartMentID=@DepartMentID,ClassID=@ClassID,DefaultImage=@DefaultImage,Content=@Content,Valid=@Valid Where ID=@ID ";
        const string sql_SelectDept = "Select ID,Name,Py,Valid From Department Where Valid=1";
        const string sql_SelectByPy = "Select ID,Name,Py,Valid From Department Where Valid=1 and Py like'%@Py%'";
        const string sql_SelectUnOther = "Select n.ID, n.Title as Title,c.ClassName as ClassName,"
                                    +" n.AddTime as AddTime,u. Name  as UserName,"
                                    +" case when n.DepartMentID='全院' then '全院' else d. Name  end as DepartMentName From NewsArticle n "
                                    +" Left join Users u on u.ID=n.Author "
                                    +" Left join Department d on d.ID=n.DepartMentID"
                                   + " Left join NewsArticle_Class c on c.ID=UPPER(n.ClassID) Where n.Valid=1";/*******Modified by dxj 2011/6/22 修改原因：两个表的数据大小写不一致*****/
        const string sql_SelectUnOtherByID = "Select n.ID, n.Title as Title,c.ClassName as ClassName,"
                                    + " n.AddTime as AddTime,u. Name  as UserName,n.Content as Content,"
                                    + " case when n.DepartMentID='全院' then '全院' else d. Name  end as DepartMentName From NewsArticle n "
                                    + " Left join Users u on u.ID=n.Author "
                                    + " Left join Department d on d.ID=n.DepartMentID"
                                   + " Left join NewsArticle_Class c on c.ID=UPPER(n.ClassID) Where n.Valid=1 AND n.ID=@ID";/*******Modified by dxj 2011/6/22 修改原因：两个表的数据大小写不一致*****/
        const string sql_SelectByTitle = "Select n.ID, n.Title as Title,c.ClassName as ClassName,"
                               + " n.AddTime as AddTime,u. Name  as UserName,n.Content as Content,"
                               + " case when n.DepartMentID='全院' then '全院' else d. Name  end as DepartMentName From NewsArticle n "
                               + " Left join Users u on u.ID=n.Author "
                               + " Left join Department d on d.ID=n.DepartMentID"
                              + " Left join NewsArticle_Class c on c.ID=UPPER(n.ClassID) Where n.Valid=1 AND n.Title like N'%{0}%'";/*******Modified by dxj 2011/6/22 修改原因：两个表的数据大小写不一致*****/
        const string sql_ValidSql = " Update NewsArticle Set Valid=0 Where ID=@ID ";
        const string sql_SelectUserName = "Select u.Name as UserName From NewsArticle n left join Users u on n.Author=u.ID WHERE n.Author=@Author and n.ID=@ID";
        const string sql_SelectByClassId = "SELECT  ID , Title , Author , AddTime , DepartMentID , ClassID , DefaultImage , Content , Valid  FROM  NewsArticle  Where  Valid =1 and ClassID=lower(@ClassID) and (DepartMentID=@DepartMentID or DepartMentID='全院' and ClassID=lower(@ClassID))";
        /// <summary>
        /// 字段列名
        /// </summary>
        const string col_ID = "ID";
        const string col_Title = "Title";
        const string col_Author = "Author";
        const string col_AddTime = "AddTime";
        const string col_DepartmentID = "DepartMentID";
        const string col_ClassID = "ClassID";
        const string col_DefaultImage = "DefaultImage";
        const string col_Content = "Content";
        const string col_Valid = "Valid";

        /// <summary>
        /// 参数
        /// </summary>
        const string param_ID = "ID";
        const string param_Title = "Title";
        const string param_Author = "Author";
        const string param_AddTime = "AddTime";
        const string param_DepartmentID = "DepartMentID";
        const string param_ClassID = "ClassID";
        const string param_DefaultImage = "DefaultImage";
        const string param_Content = "Content";
        const string param_Valid = "Valid";
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
        /// 关联其它表的所有信息
        /// </summary>
        /// <returns></returns>
        public DataSet SelectAllUnOther()
        {
            return m_SqlHelper.ExecuteDataSet(sql_SelectUnOther);
        }
        /// <summary>
        /// 关联其它表的所有信息(查询对应ID)
        /// </summary>
        /// <returns></returns>
        public DataSet SelectAllUnOtherByID(string Id)
        {
            SqlParameter paramNewsId = new SqlParameter(param_ID, SqlDbType.VarChar, 34);
            paramNewsId.Value = Id;
            return m_SqlHelper.ExecuteDataSet(sql_SelectUnOtherByID,new SqlParameter[] { paramNewsId});
        }
        /// <summary>
        /// 取得新闻集合
        /// </summary>
        /// <returns></returns>
        public IList<NewsContent> GetNewsList()
        {
            IList<NewsContent> newsList = new List<NewsContent>();
            DataSet dsNews = SelectAll();
            if (dsNews != null && dsNews.Tables.Count > 0)
            {
                DataTable dtNews = dsNews.Tables[0];
                for (int i = 0; i < dtNews.Rows.Count; i++)
                    newsList.Add(DataRowNewsClass(dtNews.Rows[i]));
            }
            return newsList;
        }
        public DataSet GetNewsByClassId(string Id,string DeptID)
        {
            SqlParameter paramNewsId = new SqlParameter(param_ClassID, SqlDbType.VarChar, 34);
            SqlParameter paramDeptId = new SqlParameter(param_DepartmentID, SqlDbType.VarChar, 12);
            paramNewsId.Value = Id;
            paramDeptId.Value = DeptID;
            return m_SqlHelper.ExecuteDataSet(sql_SelectByClassId, new SqlParameter[] { paramNewsId,paramDeptId });
        }
        public string GetUserName(string Id, string Author)
        {
            SqlParameter paramNewsId = new SqlParameter(param_ID, SqlDbType.VarChar, 34);
            SqlParameter paramAuthorId = new SqlParameter("Author", SqlDbType.VarChar, 4);
            paramNewsId.Value = Id;
            paramAuthorId.Value = Author;
            return m_SqlHelper.ExecuteDataSet(sql_SelectUserName, new SqlParameter[] { paramNewsId, paramAuthorId }).Tables[0].Rows[0]["UserName"].ToString() ;
           
        }
        public string GetUserName(string Author)
        {
            return m_SqlHelper.ExecuteDataSet("Select Name as UserName From Users Where ID='"+Author+"'").Tables[0].Rows[0]["UserName"].ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newsTitle">新闻标题</param>
        /// <returns></returns>
        public DataSet GetNewsByTitle(string newsTitle)
        {
            string sql = string.Format(sql_SelectByTitle, newsTitle);
            return m_SqlHelper.ExecuteDataSet(sql);
        }
        
        /// <summary>
        /// 取得指定新闻
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public NewsContent GetNewsById(string Id)
        {
            SqlParameter paramNewsId = new SqlParameter(param_ID, SqlDbType.VarChar, 34);
            paramNewsId.Value = Id;
            DataTable dtNews = m_SqlHelper.ExecuteDataTable(sql_SelectById
                , new SqlParameter[] { paramNewsId });
            if (dtNews == null || dtNews.Rows.Count == 0) return null;
            DataRow drNews = dtNews.Rows[0];
            return DataRowNewsClass(drNews);
        }
        NewsContent DataRowNewsClass(DataRow dataRow)
        {
            if (dataRow == null) throw new ArgumentNullException("dataRow");
            string Id = dataRow[col_ID].ToString();
            string Title = dataRow[col_Title].ToString();
            string author = dataRow[col_Author].ToString();
            string addtime = dataRow[col_AddTime].ToString();
            string departmentid = dataRow[col_DepartmentID].ToString();
            string classid = dataRow[col_ClassID].ToString();
            string defaultimage = dataRow[col_DefaultImage].ToString();
            string content = dataRow[col_Content].ToString();
            string valid = dataRow[col_Valid].ToString();
            NewsContent news = new NewsContent(Id, Title, author, Convert.ToDateTime(addtime), classid, departmentid, defaultimage, content, int.Parse(valid));
            return news;
        }
        public void InsertNews(NewsContent news)
        {
            if (news == null) throw new ArgumentNullException("news");
            SqlParameter[] insertParam = InitNews(news);
            m_SqlHelper.ExecuteNoneQuery(sql_Insert, insertParam);
        }
        public void UpdateNews(NewsContent news)
        {
            if (news == null) throw new ArgumentNullException("news");
            SqlParameter[] updateParam = ModifyNews(news);
            m_SqlHelper.ExecuteNoneQuery(sql_UpdateById, updateParam);

        }
        public void DeleteNews(NewsContent news)
        {
            if (news == null) throw new ArgumentNullException("news");
            SqlParameter paramClassId = new SqlParameter(param_ID, SqlDbType.VarChar, 34);
            paramClassId.Value = news.ID;
            m_SqlHelper.ExecuteNoneQuery(sql_DelById, new SqlParameter[] { paramClassId });
        }
        public void ValidNews(NewsContent news)
        {
            if (news == null) throw new ArgumentNullException("news");
            SqlParameter paramClassId = new SqlParameter(param_ID, SqlDbType.VarChar, 34);
            paramClassId.Value = news.ID;
            m_SqlHelper.ExecuteNoneQuery(sql_ValidSql, new SqlParameter[] { paramClassId });
          
        }
        SqlParameter[] InitNews(NewsContent news)
        {
            SqlParameter paramid = new SqlParameter(param_ID, SqlDbType.VarChar, 34);
            SqlParameter paramTitle = new SqlParameter(param_Title, SqlDbType.VarChar, 100);
            SqlParameter paramAuthor = new SqlParameter(param_Author, SqlDbType.VarChar, 4);
            SqlParameter paramAddTime = new SqlParameter(param_AddTime, SqlDbType.Time);
            SqlParameter paramDepartMentID = new SqlParameter(param_DepartmentID, SqlDbType.VarChar, 12);
            SqlParameter paramClassID = new SqlParameter(param_ClassID, SqlDbType.VarChar, 34);
            SqlParameter paramDefaultImage = new SqlParameter(param_DefaultImage, SqlDbType.VarChar, 200);
            SqlParameter paramContent = new SqlParameter(param_Content, SqlDbType.NVarChar);
            SqlParameter paramValid = new SqlParameter(param_Valid, SqlDbType.Int, 32);
            paramid.Value = Guid.NewGuid().ToString();
            paramTitle.Value = news.Title;
            paramAuthor.Value = news.Author;
            paramAddTime.Value = news.AddTime;
            paramDepartMentID.Value = news.DepartMentID;
            paramClassID.Value = news.ClassID;
            paramDefaultImage.Value = news.DefaultImage;
            paramContent.Value = news.Content;
            paramValid.Value = news.Valid;
            return new SqlParameter[] { paramid, paramTitle, paramAuthor, paramAddTime, paramDepartMentID, paramClassID, paramDefaultImage, paramContent, paramValid };
        }
        SqlParameter[] ModifyNews(NewsContent news)
        {
            SqlParameter paramid = new SqlParameter(param_ID, SqlDbType.VarChar, 34);
            SqlParameter paramTitle = new SqlParameter(param_Title, SqlDbType.VarChar, 100);
            SqlParameter paramDepartMentID = new SqlParameter(param_DepartmentID, SqlDbType.VarChar, 12);
            SqlParameter paramClassID = new SqlParameter(param_ClassID, SqlDbType.VarChar, 34);
            SqlParameter paramDefaultImage = new SqlParameter(param_DefaultImage, SqlDbType.VarChar, 200);
            SqlParameter paramContent = new SqlParameter(param_Content, SqlDbType.VarChar);
            SqlParameter paramValid = new SqlParameter(param_Valid, SqlDbType.Int, 32);
             paramid.Value = news.ID;
            paramTitle.Value = news.Title;
            paramDepartMentID.Value = news.DepartMentID;
            paramClassID.Value = news.ClassID;
            paramDefaultImage.Value = news.DefaultImage;
            paramContent.Value = news.Content;
            paramValid.Value = news.Valid;
            return new SqlParameter[] { paramid, paramTitle,paramDepartMentID, paramClassID, paramDefaultImage, paramContent, paramValid };
        }
        #endregion

  

    }
}