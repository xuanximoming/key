using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DrectWeb.Business.Service.Search;
using DrectSoft.Emr.Web.Business.Service;
namespace DrectWeb.Search2.yaopin
{
    public partial class Search3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["yp"] != null)
            {
                String yp = Request["yp"].ToString();
                SetControlValue(yp);
                return;
            }
        }
        private void SetControlValue(String yp)
        {

            DataTable dt = new Search().GetMedicaineDirect(String.Format("select * from Medicine where id={0}", yp));
            if (dt != null && dt.Rows.Count > 0)
            {
                lbTitle.Text = dt.Rows[0]["Name"].ConvertNull2OString();


                lb_SPECIFICATION.Text = dt.Rows[0]["SPECIFICATION"].ConvertNull2OString();//规格
                lb_APPLYTO.Text = dt.Rows[0]["APPLYTO"].ConvertNull2OString();//适用症
                lb_REFERENCEUSAGE.Text = dt.Rows[0]["REFERENCEUSAGE"].ConvertNull2OString();//用法
                lb_MENO.Text = dt.Rows[0]["MENO"].ConvertNull2OString();//注意事项
                lb_CATEGORYTHREE.Text = dt.Rows[0]["CATEGORYTHREE"].ConvertNull2OString();//三级分类
                lb_CATEGORYTWO.Text = dt.Rows[0]["CATEGORYTWO"].ConvertNull2OString();//二级分类
                lb_CATEGORYONE.Text = dt.Rows[0]["CATEGORYONE"].ConvertNull2OString();//一级分类
      

            }
        }
    }
}
