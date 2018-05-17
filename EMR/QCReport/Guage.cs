using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraCharts;

namespace DrectSoft.Core.QCReport
{
    public partial class Guage : UserControl
    {
        private List<Gauge> guages = new List<Gauge>();
        private ChartType chartType = ChartType.DieRate;//当前选择的统计图
        private DataTable[] tables = new DataTable[3];

        public Guage()
        {
            InitializeComponent();
            guages.AddRange(new Gauge[] { GaugeDieRate, GaugeDia, GaugeConsulatation});
        }

        private void DevButtonQurey1_Click(object sender, EventArgs e)
        {
            try
            {
                chartType = ChartType.DieRate;
                QueryData();
                chartType = ChartType.MatchRate;
                QueryData();
                chartType = ChartType.ConsultationRate;
                QueryData();
                chartType = ChartType.DieRate;
                Gauge_Click(GaugeDieRate, null);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void QueryData()
        {
            string procedureName = string.Empty;
            DataTable dt = null;
            SqlParameter[] sqlParam = null;
            try
            {
                switch (chartType)
                {
                    case ChartType.DieRate:
                        procedureName = "EMRQCREPORT.usp_GetDeptDieInfo";//存储过程名称
                        sqlParam = new SqlParameter[3];
                        sqlParam[0] = new SqlParameter("@datebegin", dateEdit1From.Text);
                        sqlParam[1] = new SqlParameter("@dateend", dateEditTo.Text);
                        sqlParam[2] = new SqlParameter("@result", SqlDbType.Structured);
                        sqlParam[2].Direction = ParameterDirection.Output;
                        dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(procedureName, sqlParam, CommandType.StoredProcedure);
                        tables[0] = dt;
                        float max = 0, val = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            max += int.Parse(dt.Rows[i]["科室出院总人数"].ToString());
                            val += int.Parse(dt.Rows[i]["死亡人数"].ToString());
                        }
                        GaugeDieRate.MaxValue = 100;
                        GaugeDieRate.Value = val / max * 100;
                        GaugeDieRate.DialText = "%";
                        break;
                    case ChartType.MatchRate:
                         procedureName = "EMRQCREPORT.usp_GetDiaInfo";//存储过程名称
                        sqlParam = new SqlParameter[3];
                        sqlParam[0] = new SqlParameter("@datebegin", dateEdit1From.Text);
                        sqlParam[1] = new SqlParameter("@dateend", dateEditTo.Text);
                        sqlParam[2] = new SqlParameter("@result", SqlDbType.Structured);
                        sqlParam[2].Direction = ParameterDirection.Output;
                        dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(procedureName, sqlParam, CommandType.StoredProcedure);
                        tables[1] = dt;
                         float max1 = 0, val1 = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            max1 += int.Parse(dt.Rows[i]["总人数"].ToString()==""? "0":dt.Rows[i]["总人数"].ToString());
                            val1 += int.Parse(dt.Rows[i]["诊断一致数"].ToString() == "" ? "0" : dt.Rows[i]["诊断一致数"].ToString());
                        }
                        GaugeDieRate.MaxValue = 100;
                        GaugeDieRate.Value = val1 / max1 * 100;
                        GaugeDieRate.DialText = "%";
                        break;
                    case ChartType.ConsultationRate:
                        procedureName = "EMRQCREPORT.usp_GetConsultationInfo";
                        sqlParam = new SqlParameter[3];
                        sqlParam[0] = new SqlParameter("@datebegin", dateEdit1From.Text);
                        sqlParam[1] = new SqlParameter("@dateend", dateEditTo.Text);
                        sqlParam[2] = new SqlParameter("@result", SqlDbType.Structured);
                        sqlParam[2].Direction = ParameterDirection.Output;
                        dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(procedureName, sqlParam, CommandType.StoredProcedure);
                        tables[2] = dt;
                        float max2 = 0, val2 = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            max2 += int.Parse(dt.Rows[i]["科室总申请数"].ToString());
                            val2 += int.Parse(dt.Rows[i]["已完成申请数"].ToString());
                        }
                        GaugeConsulatation.MaxValue = 100;
                        GaugeConsulatation.Value = val2 / max2 * 100;
                        GaugeConsulatation.DialText = "%";
                        break;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void ShowDetailInfo(DataTable dt)
        {
            try 
	        {
                this.dataGridView1.DataSource = null;
                this.dataGridView1.DataSource = dt;
                switch (chartType)
                {
                    case ChartType.DieRate:
                        this.groupControlData.Text = "科室死亡人数统计";
                        break;
                    case ChartType.MatchRate:
                        this.groupControlData.Text = "科室字段一致率统计";
                        break;
                    case ChartType.ConsultationRate:
                        this.groupControlData.Text = "科室会诊情况统计";
                        break;
                    case ChartType.Other:
                        this.groupControlData.Text = "科室死亡人数统计";
                        break;
                }    
	        }
	        catch (Exception ex)
	        {
		        throw ex;
	        }
        }

        public void BindChart(DataTable datasource)
        {
            Series series1;
            try
            {
                this.chartControl1.Series.Clear();
                series1 = new Series("", ViewType.Pie);
                series1.ArgumentScaleType = ScaleType.Qualitative;
                series1.LegendPointOptions.PointView = PointView.ArgumentAndValues;
                switch (chartType)
                {
                    case ChartType.DieRate:
                        series1.ArgumentDataMember = "科室名称";
                        series1.ValueDataMembers.AddRange("死亡人数");
                        break;
                    case ChartType.ConsultationRate:
                        series1.ArgumentDataMember = "申请科室";
                        series1.ValueDataMembers.AddRange("已完成申请数");
                        break;
                    case ChartType.MatchRate:
                        series1.ArgumentDataMember = "科室名称";
                        series1.ValueDataMembers.AddRange("诊断一致数");
                        break;
                    default:
                        series1.ArgumentDataMember = "科室名称";
                        series1.ValueDataMembers.AddRange("死亡人数");
                        break;
                }
                this.chartControl1.Series.Add(series1);
                if (datasource == null || datasource.Rows.Count == 0)
                {
                    return;
                }
                this.chartControl1.Series[0].DataSource = null;
                this.chartControl1.Series[0].DataSource = datasource;
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        private void Gauge_Click(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView1.DataSource = null;
                SetGuageSelectState(sender as Gauge);
                DataTable dt = null;
                switch ((sender as Gauge).Tag.ToString())
                {
                    case "0":
                        chartType=ChartType.DieRate;
                        dt = tables[0];
                        break;
                    case "1":
                        chartType=ChartType.MatchRate;
                        dt = tables[1];
                        break;
                    case "2":
                        chartType=ChartType.ConsultationRate;
                        dt = tables[2];
                        break;
                }
                BindChart(dt);
                ShowDetailInfo(dt);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void SetGuageSelectState(Gauge sender)
        {
            try
            {
                foreach(Gauge _guage in guages)
                {
                    if (_guage.Name.Equals(sender.Name))
                    {
                        _guage.Selected = true;
                    }
                    else
                    {
                        _guage.Selected=false;
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
    }

    public enum ChartType
    {
          DieRate=0,
          MatchRate=1,
          ConsultationRate=2,
          Other=3
    }
}
