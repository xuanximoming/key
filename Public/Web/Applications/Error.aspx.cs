using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DrectSoft.Emr.Web.Applications
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lblError.Text = "系统执行出现错误，点击此处查看更多详细信息<a href='#' onclick='QueryError()'><img src='Images/to.gif' border='0' /><img src='Images/to.gif' border='0' /><img src='Images/to.gif' border='0' /></a>";
                ErrorInfo.InnerHtml = CutStr("<br />"+Application["error"].ToString(),200,500000);
            }
        }
        protected string CutStr(string str,int len,int max)
        {
            string s = "";
            string sheng = "";
            if (str.Length > max)
            {
                str = str.Substring(0, max);
                sheng = "";
            }
            for (int i = 0; i < str.Length; i++)
            {
                int r = i % len;
                int last = (str.Length / len) * len;
                if (i != 0 && i <= last)
                {
                    if (r == 0)
                    {
                        s += str.Substring(i - len, len) + "<br />";
                    }

                }
                else if (i > last)
                {
                    s += str.Substring(i - 1);
                    break;
                }

            }
            return s + sheng;
        } 
    }
}
