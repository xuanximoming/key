using DevExpress.XtraCharts;
using System;
using System.Data;
using DrectSoft.Emr.Web.Business.Service;
namespace DrectWeb.Applications.inquire
{
    public partial class InpPat1 : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = @"select c_s,c_x,c_y from system.PER_TEMP where c_s = '北京';";
            Public publics = new Public();
            if (m_DataTable.Rows.Count <= 0)
            {
                m_DataTable = publics.GetData(sql);
            }
            CreateCustomerChart(m_DataTable, this.WebChartControl1, "erqerwe");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="webChartContro"></param>
        /// <param name="title"></param>
        private void CreateCustomerChart(DataTable dt, DevExpress.XtraCharts.Web.WebChartControl webChartContro, string title)
        {
            webChartContro.Series.Clear();
            if (dt.Rows.Count > 0)
            {
                Series series = new Series("客户分布", ViewType.Pie);//首先new一个对象  
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                foreach (DataRow row in dt.Rows)//将传入的表dt中每行数据都加入进去  
                {
                    SeriesPoint point = new SeriesPoint(row["C_X"].ToString(), row["C_Y"].ToString());
                    series.Points.Add(point);
                }
                //}

                webChartContro.Series.Add(series);

                //样式设置  
                series.ArgumentScaleType = ScaleType.Qualitative;
                series.ValueScaleType = ScaleType.Numerical;

                ((PieSeriesView)series.View).Rotation = 90;//从90度方向开始  
                series.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;//显示为百分比形式  
                series.LegendPointOptions.PointView = PointView.Argument;//右边的图例  
                series.PointOptions.PointView = PointView.ArgumentAndValues;//左边饼上的图例  
                ((PiePointOptions)(series.PointOptions)).PercentOptions.PercentageAccuracy = 3;//保留三位小数  

                ChartTitle CTitl = new ChartTitle();//加标题  
                CTitl.Text = title;

                webChartContro.Titles.Clear();
                webChartContro.Titles.Add(CTitl);
                webChartContro.Visible = true;
            }
        }
    }
}