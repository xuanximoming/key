using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using DrectSoft.Emr.Web.Business.Service;
using DrectWeb.Search2.yaodian;

namespace DrectWeb.Search2
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["type"] != null && Request["type"].ToString() != "")
                {
                    foreach (var item in tbs.Tabs)
                    {
                        if (((RadTab)item).Value == Request["type"].ToString())
                        {
                            ((RadTab)item).Selected = true;
                        }
                    }
                }
                CurrentFileInfos = new FileInfos();
                yaodianSearch3 _yaodianSearch3 = new yaodianSearch3();
                _yaodianSearch3.GetTreeView(new TreeView());
                CurrentFileInfos = yaodianSearch3.CurrentFileInfos;
            }
            //cbxClientCode.Focus();
        }
        public void search_Click(Object sender, EventArgs e)
        {

            if (cbxClientCode.SelectedValue == null || cbxClientCode.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('请选择一条记录再点击搜索！可以通过拼音首字母进行过滤');", true);
                return;
            }
            string i = cbxClientCode.SelectedValue;
            if (tbs.SelectedTab.Value == "yp")
            {
                Response.Redirect("yaopin/Search3.aspx?yp=" + cbxClientCode.SelectedValue);
                return;
            }
            if (tbs.SelectedTab.Value == "sms")
            {
                Response.Redirect("shuomingshu/Search3.aspx?sms=" + cbxClientCode.SelectedValue);

                return;
            }
            if (tbs.SelectedTab.Value == "yd")
            {
                Response.Redirect( cbxClientCode.SelectedValue.Replace("../../","../"));

                return;
            }
         

        }

        protected void cbxClientCode_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            cbxClientCode.DataSource = null;
            if (e.Text.ToString().Trim() == "") return;
            //if (cbxCategory.SelectedItem == null) return;
            if (tbs.SelectedTab.Value == "yp")
            {
                StringBuilder sql = new StringBuilder();
                string[] arr = e.Text.ToUpper().Split(' ');
                string formatStr = " and Pinyin like '%{0}%' ";
                StringBuilder where = new StringBuilder();
                for (int i = 0; i < arr.Length; i++)
                {
                    where.AppendFormat(formatStr, arr[i].ToString());
                }
                sql.AppendFormat(" select * from Medicine WHERE  rownum<31 and  (  1=1   {0} )", where.ToString());
                e.EndOfItems = true;

                cbxClientCode.DataSource = new Search().GetMedicaine(sql.ToString());

                cbxClientCode.DataTextField = "Name";
                cbxClientCode.DataValueField = "ID";
                cbxClientCode.DataBind();
                cbxClientCode.EmptyMessage = "请选择";
                return;
            }
            if (tbs.SelectedTab.Value == "sms")
            {
                StringBuilder sql = new StringBuilder();
                string[] arr = e.Text.ToUpper().Split(' ');
                string formatStr = " and PinYin like '%{0}%' ";
                StringBuilder where = new StringBuilder();
                for (int i = 0; i < arr.Length; i++)
                {
                    where.AppendFormat(formatStr, arr[i].ToString());
                }
                sql.AppendFormat(" SELECT   * FROM   MedicineDirect WHERE  rownum<31    and (  1=1   {0} )", where.ToString());
                e.EndOfItems = true;

                cbxClientCode.DataSource = new Search().GetMedicaineDirect(sql.ToString());

                cbxClientCode.DataTextField = "DirectTitle";
                cbxClientCode.DataValueField = "ID";
                cbxClientCode.DataBind();
                cbxClientCode.EmptyMessage = "请选择";
            }

            if (tbs.SelectedTab.Value == "yd")
            {


                cbxClientCode.DataSource = CurrentFileInfos.GetMatch(e.Text.Trim());

                cbxClientCode.DataTextField = "Name";
                cbxClientCode.DataValueField = "Url";
                cbxClientCode.DataBind();
                cbxClientCode.EmptyMessage = "请选择";
            }
        }

        protected void cbxClientCode_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }

        protected void tbs_TabClick(object sender, RadTabStripEventArgs e)
        {
            cbxClientCode.DataSource = null;
            //cbxClientCode.Focus();
            //if (e.Tab.Value == "yd")
            //{
            //    CurrentFileInfos = new FileInfos();
            //    yaodianSearch3 _yaodianSearch3 = new yaodianSearch3();
            //    _yaodianSearch3.GetTreeView(new TreeView());
            //    CurrentFileInfos = yaodianSearch3.CurrentFileInfos;
            //}
        }
        private static FileInfos _CurrentFileInfos;

        public static FileInfos CurrentFileInfos
        {
            get
            {
                if (_CurrentFileInfos == null)
                    _CurrentFileInfos = new FileInfos();
                return _CurrentFileInfos;
            }
            set { _CurrentFileInfos = value; }
        }

    }
}