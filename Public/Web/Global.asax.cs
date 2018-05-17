using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace DrectSoft.Emr.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //try
            //{
            //    Exception ex = Server.GetLastError();
            //    string error = "发生异常页: " + Request.Url.ToString() + "<br //>";
            //    error += "错误信息为：" + "<br //>" + ex.Message + "<br />" + "异常堆栈路径：" + "<br //>" + ex.Source + ex.StackTrace + "<br //>" + "内部异常信息：" + "<br //>" + ex.InnerException.Message;
            //    Application["error"] = error;
            //    Response.Redirect("../Error.aspx");
            //}
            //catch
            //{
            //    Exception ex = Server.GetLastError();
            //    string error = "发生异常页: " + Request.Url.ToString() + "<br //>";
            //    Response.Redirect("../Error.aspx");
            //}
            
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}