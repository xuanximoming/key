using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraCharts;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.QCDeptReport
{
    public partial class RepDeductPointPie: DevBaseForm
    {
        public DataTable m_DataSource;


        public RepDeductPointPie()
        {
            InitializeComponent();
        }

        private void RepDeductPointPie_Load(object sender, EventArgs e)
        {

            //this.PieChart.DataSource = m_DataSource;
            BindDate();
        }

        private void BindDate()
        {
            Series series1 = new Series("Pie Series 1", ViewType.Pie);

            series1.ArgumentScaleType = ScaleType.Qualitative;
            series1.ValueScaleType = ScaleType.Numerical;
            for (int i = 0; i < m_DataSource.Rows.Count; i++)
            {
                string name = m_DataSource.Rows[i]["DeductPointResult"].ToString();
                double value = Convert.ToDouble(m_DataSource.Rows[i]["DeductPointCnt"].ToString());
                series1.Points.Add(new SeriesPoint(name, new double[] { value }));
            }

            series1.SeriesPointsSorting = SortingMode.Ascending;
            series1.SeriesPointsSortingKey = SeriesPointKey.Value_1;
            ((PieSeriesView)series1.View).Rotation = 90;

            ((PieSeriesLabel)series1.Label).Position = PieSeriesLabelPosition.TwoColumns;
            ((PiePointOptions)series1.PointOptions).PointView = PointView.ArgumentAndValues;

            series1.DataSource = m_DataSource;
            
            PieChart.Series.Add(series1);

        }
    }
}