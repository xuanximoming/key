using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DrectSoft.Emr.Web.Business.Entities;
using DrectSoft.Emr.Web.Business.Service;
using System.Text.RegularExpressions;

namespace DrectSoft.Emr.Web.Applications.Manage
{
    public partial class NewsList : System.Web.UI.Page
    {
        NewsContent newscontent = new NewsContent();
        NewsContentPub newsPub = new NewsContentPub();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["userid"] != null && Request.QueryString["username"] != null && Request.QueryString["userdept"] != null)
                {
                    Session["userid"] = Request.QueryString["userid"].ToString();
                    Session["username"] = Request.QueryString["username"].ToString();
                    Session["userdept"] = Request.QueryString["userdept"].ToString();

                }
                else
                {

                }
                
                if (Request.QueryString["action"] != null)
                {
                    string Action = Request.QueryString["action"].ToString();
                    if (Action == "delone_News")
                    {
                        try
                        {

                            string newsID = Request.QueryString["NewsID"].ToString();
                            newscontent.ID = newsID;
                            newsPub.DeleteNews(newscontent);
                            NewsPub.Show(this, "删除成功!");
                        }
                        catch (Exception ex)
                        {
                            NewsPub.Show(this, "删除失败【" + ex.Message + "】");
                        }


                    }
                    if (Action == "valid")
                    {
                        try
                        {

                            string newsID = Request.QueryString["NewsID"].ToString();
                            newscontent.ID = newsID;
                            newsPub.ValidNews(newscontent);
                            NewsPub.Show(this, "过期新闻操作成功!");
                        }
                        catch (Exception ex)
                        {
                            NewsPub.Show(this, "过期失败【" + ex.Message + "】");
                        }

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
                newscontent.ID = CheckboxArray[i].ToString();
                newsPub.DeleteNews(newscontent);
            }
            NewsPub.Show(this, "删除多条新闻成功！");
            Repeater_News_List_DataBind();
        }
        protected void ValidSelect_Click(object sender, EventArgs e)
        {
            string define_checkbox = Request.Form["define_checkbox"];
            String[] CheckboxArray = define_checkbox.Split(',');
            define_checkbox = null;
            for (int i = 0; i < CheckboxArray.Length; i++)
            {
                newscontent.ID = CheckboxArray[i].ToString();
                newsPub.ValidNews(newscontent);
            }
            NewsPub.Show(this, "过期多条新闻成功！");
            Repeater_News_List_DataBind();
        }
        protected void Repeater_News_List_DataBind()
        {
            DataTable dt = new NewsContentPub().SelectAllUnOther().Tables[0];
            if (dt != null)
            {
                dt.Columns.Add("Operate", typeof(string));   //操作(修改,删除)
                dt.Columns.Add("Colum", typeof(String));     //在dt中增加字段名为Colum的列
                DataRow[] drs = dt.Select("");
                foreach (DataRow dr in drs)
                {
                    string strchar = null;
                    string newsID = dr["ID"].ToString();
                    dr["operate"] = "<a href=\"NewsAdd.aspx?action=modify&NewsID=" + newsID + "\"  class=\"op_normal\">修改</a><a href=\"NewsList.aspx?action=delone_News&NewsID=" + newsID + "\" "
                        + " class=\"op_normal\"  onclick=\"{if(confirm('确认删除吗？')){return true;}return false;}\">删除</a>"
                          + "<a href=\"NewsList.aspx?action=valid&NewsID=" + newsID + "\"  class=\"op_normal\"  onclick=\"{if(confirm('确认将新闻作过期处理吗？')){return true;}return false;}\">过期</a>"
                        + " <input type='checkbox' name='define_checkbox' id='define_checkbox' value=\"" + newsID + "\"/>";

                    strchar += "<tr class=\"TR_BG_list\" onmouseover=\"this.style.backgroundColor='#BEFBD1'\" onmouseout=\"this.style.backgroundColor='#FFFFFF'\">";
                    strchar += "<td width=\"30%\" align=\"left\" valign=\"middle\">" + dr["Title"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["ClassName"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["AddTime"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["UserName"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["DepartMentName"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["operate"].ToString() + "</td>";
                    strchar += "</tr>";
                    dr["Colum"] = strchar;
                }
                AspNetPager1.RecordCount = dt.Rows.Count;
                PagedDataSource pds = new PagedDataSource();
                pds.AllowPaging = true;
                pds.PageSize = AspNetPager1.PageSize;
                pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
                pds.DataSource = dt.DefaultView;
                Repeater_NewsClass_List.DataSource = dt;
                Repeater_NewsClass_List.DataBind();
            }
      

        }
        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {

            Repeater_News_List_DataBind();
        }

        protected void SearchNews_Click(object sender, EventArgs e)
        {
            Repeater_News_Search();
        }
        private void Repeater_News_Search()
        {
           string html=NewsTitle.Value.Trim();
            DataTable dt = new NewsContentPub().GetNewsByTitle(html).Tables[0];
            if (dt != null)
            {
                dt.Columns.Add("Operate", typeof(string));   //操作(修改,删除)
                dt.Columns.Add("Colum", typeof(String));     //在dt中增加字段名为Colum的列
                DataRow[] drs = dt.Select("");
                foreach (DataRow dr in drs)
                {
                    string strchar = null;
                    string newsID = dr["ID"].ToString();
                    dr["operate"] = "<a href=\"NewsAdd.aspx?action=modify&NewsID=" + newsID + "\"  class=\"op_normal\">修改</a><a href=\"NewsList.aspx?action=delone_News&NewsID=" + newsID + "\" "
                        + " class=\"op_normal\"  onclick=\"{if(confirm('确认删除吗？')){return true;}return false;}\">删除</a>"
                          + "<a href=\"NewsList.aspx?action=valid&NewsID=" + newsID + "\"  class=\"op_normal\"  onclick=\"{if(confirm('确认将新闻作过期处理吗？')){return true;}return false;}\">过期</a>"
                        + " <input type='checkbox' name='define_checkbox' id='define_checkbox' value=\"" + newsID + "\"/>";

                    strchar += "<tr class=\"TR_BG_list\" onmouseover=\"this.style.backgroundColor='#BEFBD1'\" onmouseout=\"this.style.backgroundColor='#FFFFFF'\">";
                    strchar += "<td width=\"30%\" align=\"left\" valign=\"middle\">" + dr["Title"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["ClassName"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["AddTime"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["UserName"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["DepartMentName"].ToString() + "</td>";
                    strchar += "<td align=\"center\" valign=\"middle\" >" + dr["operate"].ToString() + "</td>";
                    strchar += "</tr>";
                    dr["Colum"] = strchar;
                }
                AspNetPager1.RecordCount = dt.Rows.Count;
                PagedDataSource pds = new PagedDataSource();
                pds.AllowPaging = true;
                pds.PageSize = AspNetPager1.PageSize;
                pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
                pds.DataSource = dt.DefaultView;
                Repeater_NewsClass_List.DataSource = dt;
                Repeater_NewsClass_List.DataBind();
            }
        }
    }
}
