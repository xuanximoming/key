using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DrectSoft.Emr.Web.Business.Service;
using DrectWeb.Business.Service.Search;
namespace DrectWeb.Search2.shuomingshu
{
    public partial class Search3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["sms"] != null)
            {
                String sms = Request["sms"].ToString();
                SetControlValue(sms);
                return;
            }

        }
        private void SetControlValue(String sms)
        {

            DataTable dt = new Search().GetMedicaineDirect(String.Format("select * from MedicineDirect where id={0}",sms));
            if (dt != null && dt.Rows.Count > 0)
            {
                lbTitle.Text = dt.Rows[0]["DIRECTTITLE"].ConvertNull2OString();
                lbContent.Text = dt.Rows[0]["DIRECTCONTENT"].ConvertNull2OString().Replace("\r\n", "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;").Replace("img", "zzz").Replace("【", "<br/>【");
            }
        }


    }
}