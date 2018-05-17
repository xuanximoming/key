using System;
using System.Data;
using DrectSoft.Emr.Web.Business.Service;
using DrectWeb.Business.Service.echarts;
namespace DrectWeb.Applications.inquire
{
    public partial class Inppat : System.Web.UI.Page
    {
        private Echarts Echarts = new Echarts();
        private DataTable m_DataTable
        {
            get
            {
                if (ViewState["m_DataTable"] == null)
                { ViewState["m_DataTable"] = new DataTable(); }
                return (DataTable)ViewState["m_DataTable"];
            }
            set
            {
                ViewState["m_DataTable"] = value;
            }
        }
        private DataTable m_DataTableYear
        {
            get
            {
                if (ViewState["m_DataTableYear"] == null)
                { ViewState["m_DataTableYear"] = new DataTable(); }
                return (DataTable)ViewState["m_DataTableYear"];
            }
            set
            {
                ViewState["m_DataTableYear"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = @"select C_S,to_number(replace(C_X,'月','')) C_X,'月' C_U,C_Y from system.PER_TEMP t order by C_S,C_X asc;";
            string sqlyear = @"select distinct c_s from system.PER_TEMP;";
            Public publics = new Public();
            if (m_DataTable.Rows.Count <= 0)
            {
                m_DataTable = publics.GetData(sql);
            }
            if (m_DataTableYear.Rows.Count <= 0)
            {
                m_DataTableYear = publics.GetData(sqlyear);
            }
        }
        #region fuction
        public string GetJson()
        {
            return Echarts.InitDatePie(m_DataTable, m_DataTableYear);
        }
        public string GetHtml()
        {
            return Echarts.GetHtml(m_DataTableYear, "pie");
        }
        #endregion
    }
}