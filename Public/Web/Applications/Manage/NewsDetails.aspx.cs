using System;
using System.Data;
using System.Web.UI.WebControls;
using DrectSoft.Emr.Web.Business.Entities;
using DrectSoft.Emr.Web.Business.Service;

namespace DrectSoft.Emr.Web.Applications.Manage
{
    public partial class NewsDetails : System.Web.UI.Page
    {
        NewsContentPub newsContent = new NewsContentPub();
        NewsPub newspub = new NewsPub();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["NewsID"]))
                    {
                        string NewsID = Request.QueryString["NewsID"].ToString();
                        DataSet ds = new DataSet();
                        ds = newsContent.SelectAllUnOtherByID(NewsID);
                        NewsTitle.Text = ds.Tables[0].Rows[0]["Title"].ToString();
                        NewsAuthor.Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                        NewsAddTime.Text = ds.Tables[0].Rows[0]["AddTime"].ToString();
                        NewsDepartMentName.Text = ds.Tables[0].Rows[0]["DepartMentName"].ToString();
                        NewsClass.Text = ds.Tables[0].Rows[0]["ClassName"].ToString();
                        Content.InnerHtml = newspub.UnzipContent(ds.Tables[0].Rows[0]["Content"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
