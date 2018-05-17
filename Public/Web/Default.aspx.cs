using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using DrectSoft.Emr.Web.Business.Service;

namespace DrectSoft.Emr.Web
{
    public partial class Default : System.Web.UI.Page
    {
        #region 变量

        private string m_UserName = string.Empty;

        private string m_UserPassWord = string.Empty;

        private string m_ErrorText = string.Empty;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lbtn_Hidden_Click(object sender, System.EventArgs e)
        {
            Session.RemoveAll();
            this.Request.Cookies.Clear();
            m_UserName = this._txtLoginName.Value.Trim();
            m_UserPassWord = this._txtPassword.Value.Trim();
            string lockID = this.lockID.Value.Trim();

            if (m_UserName == "")
            {
                this.lMsg.Text = "请输入正确的用户登录信息！";
                return;
            }
            ////超过最大在线人数，不能登录
            //if (Application["user_sessions"] == null)
            //{
            //    Application["user_sessions"] = 0;
            //}
            //if (int.Parse(Application["user_sessions"].ToString().Trim()) >= int.Parse(ConfigurationSettings.AppSettings["MaxOnLine"].ToString().Trim()))
            //{
            //    lMsg.Text = "登录系统人数不能超过" + ConfigurationSettings.AppSettings["MaxOnLine"].ToString().Trim() + "人";
            //    return;
            //}
            if (UserLogin(m_UserName, m_UserPassWord))
            {
                Public.UserId = m_UserName;
                Response.Redirect("Master.aspx");
            }
            else
            {
                this.lMsg.Text = m_ErrorText;
            }

        }
        public bool UserLogin(string strUserId, string strPwd)
        {
            Public service = new Public();
            return service.IsUserOrNot(strUserId, strPwd, out m_ErrorText);
        }
    }
}
