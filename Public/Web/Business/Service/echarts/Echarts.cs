
using System.Data;
namespace DrectWeb.Business.Service.echarts
{
    public class Echarts
    {
        private string StrJson = null;
        private string StrSeries = null, StrXaxis = null, StrTitle = null;

        /// <summary>
        /// Initial sector statistics
        /// </summary>
        /// <param name="DataTable">DataTable for Date</param>
        /// <param name="DataTableTitle">DataTable for Title</param>
        public string InitDatePie(DataTable DataTable, DataTable DataTableTitle)
        {
            string StrJsonTmp = null;
            if (DataTable.Rows.Count > 0 && DataTableTitle.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow rowyear in DataTableTitle.Rows)
                {
                    SetNull();
                    StrJson = StrJsonTmp + "var myChart = echarts.init(document.getElementById('main" + i.ToString() + "'));option = {";
                    i++;
                    foreach (DataRow row in DataTable.Rows)
                    {
                        if (row["C_S"].ToString().Trim() == rowyear["C_S"].ToString().Trim())
                        {
                            SetPointPie(row["C_X"].ToString() + row["C_U"].ToString(), row["C_Y"].ToString());
                        }
                    }
                    StrJson = StrJson + "title: { text :'" + rowyear["C_S"] + "', x:'center'},";
                    StrJson = StrJson + " tooltip: {trigger: 'item',formatter: '{a} <br/>{b} : {c} ({d}%)'},";
                    StrJson = StrJson + "legend: {orient: 'vertical',left: 'left',data:[" + StrXaxis.Substring(0, StrXaxis.Length - 1) + "]},";
                    StrJson = StrJson + "series: [{ name:'" + rowyear["C_S"] + "',type: 'pie',radius : '50%', center: ['50%', '60%'],data:[" + StrSeries.Substring(0, StrSeries.Length - 1) + "],";
                    StrJson = StrJson + "itemStyle: { emphasis: { shadowBlur: 10, shadowOffsetX: 0, shadowColor: 'rgba(0, 0, 0, 0.5)' } } } ]";
                    StrJson = StrJson + "};myChart.setOption(option);";
                    StrJsonTmp = StrJson;
                }
            }
            return StrJson;
        }
        /// <summary>
        /// Set null Date 
        /// </summary>
        private void SetNull()
        {
            StrJson = null;
            StrSeries = null;
            StrXaxis = null;
        }
        /// <summary>
        /// Set Point for sector
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetPointPie(string x, string y)
        {
            StrXaxis = StrXaxis + "'" + x + "',";
            StrSeries = StrSeries + "{ value:" + y + ", name:'" + x + "'},";
        }

        private void SetPointLine(string x, string y)
        {
            if (StrXaxis.IndexOf(x) == 0)
                StrXaxis = StrXaxis + "'" + x + "',";
            StrSeries = StrSeries + "{ value:" + y + ", name:'" + x + "'},";
        }
        /// <summary>
        /// Write div
        /// </summary>
        /// <param name="DataTableTitle"></param>
        /// <returns></returns>
        public string GetHtml(DataTable DataTableTitle, string type)
        {
            string StrHtml = null;
            if (type == "line") return "<div id='main' style='width: 400px; height: 300px;'></div>";
            if (DataTableTitle.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow row in DataTableTitle.Rows)
                {
                    StrHtml = StrHtml + "<div id='main" + i.ToString() + "' style='width: 400px; height: 300px;'></div>";
                    i++;
                }
            }
            return StrHtml;
        }

        public string InitDateLine(DataTable DataTable, DataTable DataTableTitle)
        {
            string StrJsonTmp = null;
            if (DataTable.Rows.Count > 0 && DataTableTitle.Rows.Count > 0)
            {
                SetNull();
                int i = 0;
                StrJson = StrJsonTmp + "var myChart = echarts.init(document.getElementById('main'));option = {";
                i++;
                StrJson = StrJson + "title: { text :'eeeeee'},";
                StrJson = StrJson + " tooltip: {trigger: 'axis'},";
                foreach (DataRow rowyear in DataTableTitle.Rows)
                {
                    StrTitle = StrTitle + "'" + rowyear["C_S"] + "',";
                    string StrSeriesY = null;
                    foreach (DataRow row in DataTable.Rows)
                    {
                        if (row["C_S"].ToString().Trim() == rowyear["C_S"].ToString().Trim())
                        {
                            StrSeriesY = StrSeriesY + row["C_Y"].ToString() + ",";
                        }
                    }
                    StrSeries = StrSeries + "{ name:'" + rowyear["C_S"].ToString() + "',type:'line',data:[" + StrSeriesY.Substring(0, StrSeriesY.Length - 1) + "]},";
                }
                StrJson = StrJson + "legend: {data:[" + StrTitle.Substring(0, StrTitle.Length - 1) + "]},";
                StrJson = StrJson + " grid: {left: '3%', right: '4%', bottom: '3%', containLabel: true },";
                StrJson = StrJson + " toolbox: { feature: { saveAsImage: {}}},";
                StrXaxis = "";
                foreach (DataRow row in DataTable.Rows)
                {
                    if (StrXaxis.IndexOf((row["C_X"].ToString() + row["C_U"].ToString())) == -1)
                        StrXaxis = StrXaxis + "'" + row["C_X"].ToString() + row["C_U"].ToString() + "',";
                }
                StrJson = StrJson + "xAxis: {type: 'category', boundaryGap: false,data: [" + StrXaxis.Substring(0, StrXaxis.Length - 1) + "]},";
                StrJson = StrJson + "  yAxis: { type: 'value' },";
                StrJson = StrJson + "series: [" + StrSeries.Substring(0, StrSeries.Length - 1) + "]";
                StrJson = StrJson + "};myChart.setOption(option);";
                StrJsonTmp = StrJson;
            }
            return StrJson;
        }

    }
}