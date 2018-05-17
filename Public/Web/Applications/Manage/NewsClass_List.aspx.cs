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
    public partial class NewsClass_List : System.Web.UI.Page
    {
        NewsPub news = new NewsPub();
        NewsClass newsClass = new NewsClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["action"] != null)
                {

                    try {

                        string classID = Request.QueryString["ClassID"].ToString();
                        newsClass.ID = new Guid(classID);
                        news.DeleteClass(newsClass);
                        NewsPub.Show(this,"删除成功!");
                    }
                    catch (Exception ex)
                    {
                        NewsPub.Show(this, "删除失败【"+ex.Message+"】");
                    }
                    
                     
                }
                Repeater_News_List_DataBind();
            }
        }
        protected void DelSelect_Click(object sender, EventArgs e)
        {
            string define_checkbox = Request.Form["define_checkbox"];
                String[] CheckboxArray = define_checkbox.Split(',');
                define_checkbox = null;
                for (int i = 0; i < CheckboxArray.Length; i++)
                {
                    newsClass.ID =new Guid(CheckboxArray[i].ToString());
                    news.DeleteClass(newsClass);
                }
                NewsPub.Show(this, "删除成功！");
            Repeater_News_List_DataBind();
        }
        protected void Repeater_News_List_DataBind()
        {
            DataTable dt = new NewsPub().SelectAll().Tables[0];
            if (dt != null)
            {
                dt.Columns.Add("Operate", typeof(string));   //操作(修改,删除)
                dt.Columns.Add("Colum", typeof(String));     //在dt中增加字段名为Colum的列
                DataRow[] drs = dt.Select("");
                foreach (DataRow dr in drs)
                {
                    string strchar = null;
                    string classID = dr["ID"].ToString();
                    dr["operate"] = "<a href=\"NewsClass_Add.aspx?action=modify&ClassID=" + classID + "\"  class=\"op_normal\">修改</a><a href=\"NewsClass_List.aspx?action=delone_class&ClassID=" + classID + "\" "
                        +" class=\"op_normal\"  onclick=\"{if(confirm('确认删除吗？')){return true;}return false;}\">删除</a>"
                       //  + "<a href=\"NewsClass_List.aspx?action=valid&ClassID=" + classID + "\"  class=\"op_normal\"  onclick=\"{if(confirm('确认将新闻作过期处理吗？')){return true;}return false;}\">过期</a>"
                        +" <input type='checkbox' name='define_checkbox' id='define_checkbox' value=\"" + classID + "\"/>";

                    strchar += "<tr class=\"TR_BG_list\" onmouseover=\"this.style.backgroundColor='#BEFBD1'\" onmouseout=\"this.style.backgroundColor='#FFFFFF'\">";
                    strchar += "<td width=\"30%\" align=\"left\" valign=\"middle\">" + dr["ClassName"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["Summary"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["operate"].ToString() + "</td>";
                    strchar += "</tr>";
                    dr["Colum"] = strchar;
                }

                Repeater_NewsClass_List.DataSource = dt;
                Repeater_NewsClass_List.DataBind();
            }

        }
    }
}
