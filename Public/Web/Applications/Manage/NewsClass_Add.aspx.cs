using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DrectSoft.Emr.Web.Business.Service;
using DrectSoft.Emr.Web.Business.Entities;
namespace DrectSoft.Emr.Web.Applications.Manage
{
    public partial class NewsClass_Add : System.Web.UI.Page
    {
        NewsPub news = new NewsPub();
       
        protected void Page_Load(object sender, EventArgs e)
        {
          if (!IsPostBack)
            {
                if (Request.QueryString["action"]!=null)
                {
                    string Action = Request.QueryString["action"].ToString();
                    if (Action == "modify")
                    {
                        string ClassId = Request.QueryString["ClassID"].ToString();
                        NewsClass newsClass = news.GetClassById(ClassId);
                        TextBox_ClassName.Text = newsClass.ClassName;
                        TextBox_Summary.Text = newsClass.Summary;
                        BtnSave.Text = "修改类别";
                    }
                }
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
           
            NewsClass newsClass = new NewsClass();
            try
            {
                if (Request.QueryString["action"] != null)
                {
                    string Action = Request.QueryString["action"].ToString();
                    if (Action == "modify")
                    {
                        newsClass.ClassName = TextBox_ClassName.Text;
                        newsClass.Summary = TextBox_Summary.Text;
                        newsClass.ID = new Guid(Request.QueryString["ClassID"].ToString());
                        news.UpdateClass(newsClass,Action);
                        //NewsPub.Show(this, "修改成功!");
                        NewsPub.ShowAlert("修改成功", "NewsClass_List.aspx");
                    }
                }
                else
                {
                    newsClass.ClassName = TextBox_ClassName.Text;
                    newsClass.Summary = TextBox_Summary.Text;
                    news.InsertClass(newsClass, "");
                    NewsPub.Show(this, "添加成功!");
                    TextBox_ClassName.Text="";
                    TextBox_Summary.Text="";
                }
             
            }
            catch (Exception EX)
            {
                Label_Message.Text = EX.Message;
            }
        }
    
    }
}
