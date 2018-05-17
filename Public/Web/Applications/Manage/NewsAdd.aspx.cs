using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DrectSoft.Emr.Web.Business.Entities;
using DrectSoft.Emr.Web.Business.Service;

namespace DrectSoft.Emr.Web.Applications.Manage
{
    public partial class NewsAdd : System.Web.UI.Page
    {
        NewsContentPub news = new NewsContentPub();
        NewsPub newspub = new NewsPub();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindNewsClass();
                BindDept();
                if (!string.IsNullOrEmpty(Session["userid"] == null ? "" : Session["userid"].ToString()))
                {
                    Author.Value = news.GetUserName(Session["userid"].ToString());
                }
                if (Request.QueryString["action"] != null)
                {
                    string Action = Request.QueryString["action"].ToString();
                    if (Action == "modify")
                    {
                        string NewsId = Request.QueryString["NewsID"].ToString();
                        NewsContent newscontent = news.GetNewsById(NewsId);
                        ArticleClass.Value = newscontent.ClassID.ToString();
                        Dept.Value = newscontent.DepartMentID == "全院" ? "" : newscontent.DepartMentID;
                        Title.Text = newscontent.Title;
                        Author.Value = news.GetUserName(NewsId, newscontent.Author);
                        Content.Value = newspub.UnzipContent(newscontent.Content);
                        btnSave.Text = "修改新闻";
                    }
                }
            }
        }
        private void BindDept()
        {

            DataTable dtDept = newspub.SelectAllDept().Tables[0];
            Dept.DataSource = dtDept.DefaultView;
            Dept.TextField = dtDept.Columns["Name"].ColumnName;
            Dept.ValueField = dtDept.Columns["ID"].ColumnName;
            Dept.DataBind();
        }
        private void BindNewsClass()
        {
            IList<NewsClass> list = NewsClass.AllClass;
            ArticleClass.DataSource = list;
            ArticleClass.TextField = "ClassName";
            ArticleClass.ValueField = "ID";
            ArticleClass.DataBind();
        }

        protected void modifybt_Click(object sender, EventArgs e)
        {
            NewsContent newsContent = new NewsContent();
            try
            {
                if (Request.QueryString["action"] != null)
                {
                    string Action = Request.QueryString["action"].ToString();
                    if (Action == "modify")
                    {

                        newsContent.ClassID = ArticleClass.Value.ToString();
                        newsContent.DepartMentID = Dept.Value == null ? "全院" : Dept.Value.ToString();
                        newsContent.ID = Request.QueryString["NewsID"].ToString();
                        newsContent.Title = Title.Text;
                        newsContent.Content = newspub.ZipContent(Content.Value);
                        newsContent.Valid = 1;
                        newsContent.DefaultImage = "1111";
                        news.UpdateNews(newsContent);
                        NewsPub.ShowAlert("修改成功", "NewsList.aspx");
                    }
                }
                else
                {
                    newsContent.ClassID = ArticleClass.Value.ToString();
                    newsContent.DepartMentID = Dept.Value == null ? "全院" : Dept.Value.ToString();
                    newsContent.Title = Title.Text;
                    newsContent.Author = Session["userid"].ToString();
                    newsContent.Content = newspub.ZipContent(Content.Value);
                    newsContent.AddTime = DateTime.Now;
                    newsContent.Valid = 1;
                    newsContent.DefaultImage = "1111";
                    news.InsertNews(newsContent);
                    NewsPub.ShowAlert("添加成功!", "NewsList.aspx");
                }

            }
            catch (Exception EX)
            {
                Label_Message.Text = EX.Message;
            }
        }

    }
}
