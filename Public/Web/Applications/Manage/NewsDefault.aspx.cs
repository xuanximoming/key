using DevExpress.Web.ASPxTabControl;
using System;
using System.Data;
using System.Web.UI;
using DrectSoft.Emr.Web.Business.Service;

namespace DrectSoft.Emr.Web.Applications.Manage
{
    public partial class NewsDefault : System.Web.UI.Page
    {
        NewsContentPub news = new NewsContentPub();
        NewsPub newsClass = new NewsPub();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxPageControl1_Init(object sender, EventArgs e)
        {
            DataTable objDtBep = newsClass.SelectAll().Tables[0];
            for (int i = 0; i < objDtBep.Rows.Count; i++)//循环写入标题 
            {
                TabPage objTp = new TabPage();
                string ClassID = objDtBep.Rows[0]["ID"].ToString();
                objTp.Text = objDtBep.Rows[0]["ClassName"].ToString();
                string userdpet = string.Empty;
                if (Session["userdept"] != null)
                    userdpet = Session["userdept"].ToString();
                else
                    userdpet = "全院";

                DataTable objDtParen = news.GetNewsByClassId(ClassID, userdpet).Tables[0]; //此函数的第二个参数为登录人的部门代码
                for (int y = 0; y < objDtParen.Rows.Count; y++)//循环写入内容 
                {
                    if (y % 7 == 0 && y != 0)
                    {
                        Control objControl2 = TemplateControl.ParseControl("<br>");
                        objTp.Controls.Add(objControl2);
                    }
                    Control objControl = TemplateControl.ParseControl("<span class='list_date'>" + objDtParen.Rows[y]["AddTime"].ToString() + "</span><asp:HyperLink  runat='server' NavigateUrl='NewsDetails.aspx?NewsID=" + objDtParen.Rows[y]["ID"].ToString() + "' Width='50%'>" + objDtParen.Rows[y]["Title"].ToString() + "</asp:HyperLink><br/>");//面板 
                    objTp.Controls.Add(objControl);//向一个标签内加入面板 
                }
                ASPxPageControl1.TabPages.Add(objTp);//向控件中加入标签 
            }
        }
    }
}
