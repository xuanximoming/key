using DevExpress.Web.ASPxNavBar;
using System;
using System.Data;
using DrectSoft.Emr.Web.Business.Service;

namespace DrectSoft.Emr.Web
{
    public partial class Master1 : System.Web.UI.Page
    {
        #region members
        /// <summary>
        /// navbargroup
        /// </summary>
        private DataTable m_DataTableGroup
        {
            get
            {
                if (ViewState["m_DataTableGroup"] == null)
                { ViewState["m_DataTableGroup"] = new DataTable(); }
                return (DataTable)ViewState["m_DataTableGroup"];
            }
            set
            {
                ViewState["m_DataTableGroup"] = value;
            }
        }
        /// <summary>
        /// navbaritem
        /// </summary>
        private DataTable m_DataTableItem
        {
            get
            {
                if (ViewState["m_DataTableItem"] == null)
                { ViewState["m_DataTableItem"] = new DataTable(); }
                return (DataTable)ViewState["m_DataTableItem"];
            }
            set
            {
                ViewState["m_DataTableItem"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];

            if (!IsPostBack)
            {
                InitNavBar();
                if (string.IsNullOrEmpty(username))
                    this.LabelUser.Text = Public.UserName;
                else
                    this.LabelUser.Text = username;
            }
        }

        #region methods
        private void InitNavBar()
        {
            InitNavGroup();
        }

        private void InitNavGroup()
        {
            Public publics = new Public();
            if (m_DataTableGroup.Rows.Count == 0)
                m_DataTableGroup = publics.GetNavBarGroupData();
            foreach (DataRow dataRowGroup in m_DataTableGroup.Rows)
            {
                string idGroup = dataRowGroup["ID"].ToString();
                string nameGroup = dataRowGroup["Name"].ToString();
                string urlGroup = dataRowGroup["Url"].ToString();
                NavBarGroup group = ASPxNavBarTree.Groups.Add(nameGroup, idGroup, "", urlGroup, "");
                InitNavItem(int.Parse(idGroup), publics, group);
            }
        }

        private void InitNavItem(int idGroup, Public publics, NavBarGroup group)
        {
            //string url = @"Applications/QualityControl/QCStaticQuery.aspx";
            m_DataTableItem = publics.GetNavBarItemData(idGroup);
            foreach (DataRow dataRowItem in m_DataTableItem.Rows)
            {
                string idItem = dataRowItem["ID"].ToString();
                string nameItem = dataRowItem["Name"].ToString();
                string urlItem = String.Empty;
                if (dataRowItem["Url"].ToString().Contains("?"))//(dataRowItem["ParentId"].ToString() == "13")
                    urlItem = dataRowItem["Url"].ToString() + "&TreeID=" + dataRowItem["ID"].ToString();
                else
                    urlItem = dataRowItem["Url"].ToString() + "?TreeID=" + dataRowItem["ID"].ToString();
                group.Items.Add(nameItem, idItem, "", urlItem, "MainFrame");
            }
        }
        #endregion
    }
}
