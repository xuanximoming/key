using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WebApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            INIT();
            string path = HttpContext.Current.Server.MapPath("test1.repx");
            YidanSoft.Common.Report.XReport xreport = new YidanSoft.Common.Report.XReport(ds, path);
            this.ReportViewer1.Report = xreport.CurrentReport;
        }

        private DataSet ds = new DataSet();
        private void INIT()
        {
            int col = DateTime.Now.Second % 9;
            col = col + 1;
            DataTable dt = new DataTable("test1");
            for (int i = 0; i < col; i++)
            {
                dt.Columns.Add(new DataColumn(string.Format("col{0}", i)));
            }
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            for (int i = 0; i < col; i++)
            {
                if (i > 3)
                    dr[i] = string.Format("测试{0}", i);
                else
                    dr[i] = i.ToString();
            }
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            dt.ImportRow(dr);
            ds.Tables.Clear();
            ds.Tables.Add(dt);
        }
    }
}
