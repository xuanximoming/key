using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.QCDeptReport
{
    public partial class DiagramForm : DevBaseForm
    {
        DataTable m_DataSource;
        string m_X_Name;
        string m_X_NameID;

        string m_ColumnID = string.Empty;
        string m_ColumnName = string.Empty;

        public DiagramForm(DataTable dt, string xName, string xNameID)
        {
            m_DataSource = dt;
            m_X_Name = xName;
            m_X_NameID = xNameID;
            InitializeComponent();
        }

        public DiagramForm()
        {
            InitializeComponent();
        }

        public void ResetCheckBox()
        {
            foreach (Control control in panel1.Controls)
            {
                CheckEdit checkEdit = control as CheckEdit;
                if (checkEdit != null)
                {
                    checkEdit.Checked = false;
                }
            }
        }

        private void checkEditBar_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            if (!checkEdit.Checked) return;
            string value = checkEdit.Tag.ToString();
            InitSpline2(value, checkEdit.Text);
        }

        private void checkEditPie_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            if (!checkEdit.Checked) return;
            string value = checkEdit.Tag.ToString();
            InitSpline2(value, checkEdit.Text);
        }

        private void checkEditLine_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            if (!checkEdit.Checked) return;
            string value = checkEdit.Tag.ToString();
            InitSpline2(value, checkEdit.Text);
        }

        private void checkEditStepLine_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            if (!checkEdit.Checked) return;
            string value = checkEdit.Tag.ToString();
            InitSpline2(value, checkEdit.Text);
        }

        private void checkEditSpline_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            if (!checkEdit.Checked) return;
            string value = checkEdit.Tag.ToString();
            InitSpline2(value, checkEdit.Text);
        }

        private void InitSpline(string enumValue, string titleName)
        {
            DataTable dt = m_DataSource.Copy();

            m_ColumnID = lookUpEdit1.EditValue.ToString();
            m_ColumnName = lookUpEdit1.Text;

            List<string> list = new List<string>();

            foreach (DataColumn dataColumn in dt.Columns)
            {
                if (dataColumn.ColumnName.Split('#')[0].ToUpper() != m_ColumnID.ToUpper() && dataColumn.ColumnName.Split('#')[0].ToUpper() != m_X_NameID.ToUpper())
                {
                    list.Add(dataColumn.ColumnName);
                }
            }

            foreach (string name in list)
            {
                dt.Columns.Remove(name);
            }
            dt.AcceptChanges();

            chartControl1.Series.Clear();
            Series series = new Series(titleName, (ViewType)Enum.Parse(typeof(ViewType), enumValue));
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.PieSeriesLabel pieSeriesLabel1 = new DevExpress.XtraCharts.PieSeriesLabel();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySizeSeriesLabel1 = new SideBySideBarSeriesLabel();

            //************************************************************BeginInit()********************************************************
            ((System.ComponentModel.ISupportInitialize)(chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySizeSeriesLabel1)).BeginInit();

            series.ArgumentScaleType = ScaleType.Qualitative;

            DevExpress.XtraCharts.PiePointOptions piePointOptions1 = new DevExpress.XtraCharts.PiePointOptions();
            piePointOptions1.PointView = DevExpress.XtraCharts.PointView.ArgumentAndValues;
            series.PointOptions = piePointOptions1;

            series.LegendText = m_ColumnName;
            series.View.Color = Color.Red;
            foreach (DataRow dataRow in dt.Rows)
            {
                string name = dataRow[m_X_NameID + "#" + m_X_Name].ToString();
                if (name == "总计") continue;
                string value = dataRow[m_ColumnID + "#" + m_ColumnName].ToString();
                SeriesPoint sp = new SeriesPoint(name, value);
                series.Points.Add(sp);
            }
            chartControl1.Series.Add(series);

            PointSeriesView pointView = series.View as PointSeriesView;
            if (pointView != null)
            {
                pointView.PointMarkerOptions.Kind = MarkerKind.Circle;
            }

            //针对饼图的处理
            if (series.View is PieSeriesView)
            {
                this.chartControl1.RuntimeSelection = false;
                ((PieSeriesView)series.View).RuntimeExploding = true;
            }
            else
            {
                this.chartControl1.RuntimeSelection = true;
            }

            ChartTitle ct1 = new ChartTitle();
            ct1.Text = titleName;
            chartControl1.Titles.Clear();
            chartControl1.Titles.Add(ct1);

            if (series.Label is PointSeriesLabel)
            {
                pointSeriesLabel1.LineLength = 15;
                pointSeriesLabel1.LineVisible = true;
                pointSeriesLabel1.Antialiasing = true;
                pointSeriesLabel1.Angle = 30;
                pointSeriesLabel1.ResolveOverlappingMinIndent = 10;
                pointSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAllAroundPoint;
                series.Label = pointSeriesLabel1;
            }
            else if (series.Label is PieSeriesLabel)
            {
                pieSeriesLabel1.LineLength = 15;
                pieSeriesLabel1.LineVisible = true;
                pieSeriesLabel1.Antialiasing = true;
                pieSeriesLabel1.ResolveOverlappingMinIndent = 10;
                pieSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAllAroundPoint;
                series.Label = pieSeriesLabel1;
            }
            else if (series.Label is SideBySideBarSeriesLabel)
            {
                sideBySizeSeriesLabel1.LineLength = 15;
                sideBySizeSeriesLabel1.LineVisible = true;
                sideBySizeSeriesLabel1.Antialiasing = true;
                sideBySizeSeriesLabel1.Position = BarSeriesLabelPosition.Top;
                sideBySizeSeriesLabel1.ResolveOverlappingMinIndent = 10;
                sideBySizeSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAllAroundPoint;
                series.Label = sideBySizeSeriesLabel1;
            }
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(chartControl1)).EndInit();
            //************************************************************EndInit()********************************************************
        }

        private void InitSpline2(string enumValue, string titleName)
        {
            chartControl1.Series.Clear();

            DataTable dt = m_DataSource.Copy();

            string[] columnsID = checkedComboBoxEdit1.EditValue.ToString().Split(',');
            string[] columnsName = checkedComboBoxEdit1.Text.ToString().Split(',');

            for (int i = 0; i < columnsID.Length; i++)
            {
                if (string.IsNullOrEmpty(columnsID[i])) break;

                m_ColumnID = columnsID[i].Trim();
                m_ColumnName = columnsName[i].Trim();

                List<string> list = new List<string>();

                foreach (DataColumn dataColumn in dt.Columns)
                {
                    if (dataColumn.ColumnName.Split('#')[0].ToUpper() != m_ColumnID.ToUpper() && dataColumn.ColumnName.Split('#')[0].ToUpper() != m_X_NameID.ToUpper())
                    {
                        list.Add(dataColumn.ColumnName);
                    }
                }

                DataTable dtTemp = dt.Copy();
                foreach (string name in list)
                {
                    dtTemp.Columns.Remove(name);
                }
                dtTemp.AcceptChanges();

                Color color = GetColor(i);
                InitSpline2Inner(enumValue, titleName, dtTemp, color);
            }
        }

        Color GetColor(int index)
        {
            switch (index % 4)
            {
                case 0: return Color.Red;
                case 1: return Color.Blue;
                case 2: return Color.Green;
                case 3: return Color.Gold;
                default: break;
            }
            return Color.Black;
        }

        private void InitSpline2Inner(string enumValue, string titleName, DataTable dt, Color color)
        {
            Series series = new Series(titleName, (ViewType)Enum.Parse(typeof(ViewType), enumValue));
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.PieSeriesLabel pieSeriesLabel1 = new DevExpress.XtraCharts.PieSeriesLabel();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySizeSeriesLabel1 = new SideBySideBarSeriesLabel();

            //************************************************************BeginInit()********************************************************
            ((System.ComponentModel.ISupportInitialize)(chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySizeSeriesLabel1)).BeginInit();

            series.ArgumentScaleType = ScaleType.Qualitative;

            DevExpress.XtraCharts.PiePointOptions piePointOptions1 = new DevExpress.XtraCharts.PiePointOptions();
            piePointOptions1.PointView = DevExpress.XtraCharts.PointView.ArgumentAndValues;
            series.PointOptions = piePointOptions1;

            series.LegendText = m_ColumnName;
            series.View.Color = color;
            foreach (DataRow dataRow in dt.Rows)
            {
                string name = dataRow[m_X_NameID + "#" + m_X_Name].ToString();
                if (name == "总计") continue;
                string value = dataRow[m_ColumnID + "#" + m_ColumnName].ToString();
                SeriesPoint sp = new SeriesPoint(name, value);
                series.Points.Add(sp);
            }
            chartControl1.Series.Add(series);

            PointSeriesView pointView = series.View as PointSeriesView;
            if (pointView != null)
            {
                pointView.PointMarkerOptions.Kind = MarkerKind.Circle;
            }

            //针对饼图的处理
            if (series.View is PieSeriesView)
            {
                this.chartControl1.RuntimeSelection = false;
                ((PieSeriesView)series.View).RuntimeExploding = true;
            }
            else
            {
                this.chartControl1.RuntimeSelection = true;
            }

            ChartTitle ct1 = new ChartTitle();
            ct1.Text = titleName;
            chartControl1.Titles.Clear();
            chartControl1.Titles.Add(ct1);

            if (series.Label is PointSeriesLabel)
            {
                pointSeriesLabel1.LineLength = 15;
                pointSeriesLabel1.LineVisible = true;
                pointSeriesLabel1.Antialiasing = true;
                pointSeriesLabel1.Angle = 30;
                pointSeriesLabel1.ResolveOverlappingMinIndent = 10;
                pointSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAllAroundPoint;
                series.Label = pointSeriesLabel1;
            }
            else if (series.Label is PieSeriesLabel)
            {
                pieSeriesLabel1.LineLength = 15;
                pieSeriesLabel1.LineVisible = true;
                pieSeriesLabel1.Antialiasing = true;
                pieSeriesLabel1.ResolveOverlappingMinIndent = 10;
                pieSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAllAroundPoint;
                series.Label = pieSeriesLabel1;
            }
            else if (series.Label is SideBySideBarSeriesLabel)
            {
                sideBySizeSeriesLabel1.LineLength = 15;
                sideBySizeSeriesLabel1.LineVisible = true;
                sideBySizeSeriesLabel1.Antialiasing = true;
                sideBySizeSeriesLabel1.Position = BarSeriesLabelPosition.Top;
                sideBySizeSeriesLabel1.ResolveOverlappingMinIndent = 10;
                sideBySizeSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAllAroundPoint;
                series.Label = sideBySizeSeriesLabel1;
            }
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(chartControl1)).EndInit();
            //************************************************************EndInit()********************************************************
        }

        private void DiagramForm_Load(object sender, EventArgs e)
        {
            InitLookUpEditer();
            InitCheckComboBox();
            this.chartControl1.PieSeriesPointExploded += new PieSeriesPointExplodedEventHandler(chartControl1_PieSeriesPointExploded);

            checkEditBar.Checked = true;
        }

        void chartControl1_PieSeriesPointExploded(object sender, PieSeriesPointExplodedEventArgs e)
        {
        }

        public static DataTable GetDiagramDataSource(GridControl gridControl)
        {
            DataTable dataSource = new DataTable();
            DataTable dataSourceColumn = new DataTable();

            if (gridControl.DataSource != null)
            {
                dataSource = gridControl.DataSource as DataTable;
            }

            if (dataSource != null)
            {
                //Add Columns
                GridView view = gridControl.MainView as GridView;
                if (view != null)
                {
                    foreach (GridColumn gridColumn in view.Columns)
                    {
                        if (gridColumn.Visible == true)
                        {
                            dataSourceColumn.Columns.Add(gridColumn.FieldName + "#" + gridColumn.Caption);
                        }
                    }
                }

                //Add Rows
                for (int i = 0; i < dataSource.Rows.Count; i++)
                {
                    DataRow dataRow = dataSource.Rows[i];
                    DataRow dr = dataSourceColumn.NewRow();
                    for (int j = 0; j < dataSource.Columns.Count; j++)
                    {
                        string dataSourceColumnName = dataSource.Columns[j].ColumnName;
                        for (int m = 0; m < dataSourceColumn.Columns.Count; m++)
                        {
                            string dataSourceColumnNameTemp = dataSourceColumn.Columns[m].ColumnName.Split('#')[0];
                            if (dataSourceColumnName == dataSourceColumnNameTemp)
                            {
                                dr[dataSourceColumn.Columns[m].ColumnName] = dataRow[dataSourceColumnName];
                            }
                        }
                    }
                    dataSourceColumn.Rows.Add(dr);
                }
            }

            return dataSourceColumn;
        }

        private void InitLookUpEditer()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            if (m_DataSource != null)
            {
                foreach (DataColumn dataColumn in m_DataSource.Columns)
                {
                    string id = dataColumn.ColumnName.Split('#')[0];
                    string name = dataColumn.ColumnName.Split('#')[1];

                    if (id.ToUpper() == m_X_NameID.ToUpper()) continue;

                    DataRow dr = dt.NewRow();
                    dr["ID"] = id;
                    dr["Name"] = name;
                    dt.Rows.Add(dr);
                }
                lookUpEdit1.Properties.DataSource = dt;
                lookUpEdit1.Properties.DisplayMember = "Name";
                lookUpEdit1.Properties.ValueMember = "ID";

                if (dt.Rows.Count > 0)
                {
                    string id = dt.Rows[0]["ID"].ToString();
                    lookUpEdit1.EditValue = id;
                }
            }
        }

        private void InitCheckComboBox()
        {
            checkedComboBoxEdit1.Properties.SelectAllItemVisible = false;

            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            if (m_DataSource != null)
            {
                foreach (DataColumn dataColumn in m_DataSource.Columns)
                {
                    string id = dataColumn.ColumnName.Split('#')[0];
                    string name = dataColumn.ColumnName.Split('#')[1];

                    if (id.ToUpper() == m_X_NameID.ToUpper()) continue;

                    DataRow dr = dt.NewRow();
                    dr["ID"] = id;
                    dr["Name"] = name;
                    dt.Rows.Add(dr);
                }
                checkedComboBoxEdit1.Properties.DataSource = dt;
                checkedComboBoxEdit1.Properties.DisplayMember = "Name";
                checkedComboBoxEdit1.Properties.ValueMember = "ID";

                if (dt.Rows.Count > 0)
                {
                    string id = dt.Rows[0]["ID"].ToString();
                    checkedComboBoxEdit1.SetEditValue(id);
                }
            }
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            List<string> list = GetCheckEditValue();
            if (list.Count > 0)
            {
                InitSpline2(list[0], list[1]);
            }
        }

        private List<string> GetCheckEditValue()
        {
            List<string> list = new List<string>();
            foreach (Control control in panel1.Controls)
            {
                CheckEdit ce = control as CheckEdit;
                if (ce != null && ce.Checked)
                {
                    list.Add(ce.Tag.ToString());
                    list.Add(ce.Text);
                    return list;
                }
            }
            return list;
        }

        private void checkedComboBoxEdit1_EditValueChanged(object sender, EventArgs e)
        {
            List<string> list = GetCheckEditValue();
            if (list.Count > 0)
            {
                InitSpline2(list[0], list[1]);
            }
        }

        public bool CanMouseDown = true;
        private void chartControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (CanMouseDown)
            {
                ChartHitInfo hitInfo = chartControl1.CalcHitInfo(chartControl1.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)));
                if (hitInfo.SeriesPoint != null)
                {
                    if (!hitInfo.SeriesPoint.IsEmpty)
                    {
                        int count = Convert.ToInt32(hitInfo.SeriesPoint.Values[0]);
                        string name = "【" + hitInfo.SeriesPoint.Argument + "】" + hitInfo.Series.LegendText;
                        DynamicCreateSubDiagram(count, name);
                    }
                }
            }
        }

        private string GetRandomValue(int value)
        {
            Random r = new Random(GetRandomSeed());
            return r.Next(value).ToString();
        }

        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        private void DynamicCreateSubDiagram(int totalNum, string name)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID#" + name);
            dt.Columns.Add("Name#科室名称");

            #region
            DataRow dr = dt.NewRow();
            int value = Convert.ToInt32(GetRandomValue(Convert.ToInt32(totalNum / 11f)));
            dr[0] = value;
            dr[1] = "1月";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            totalNum -= value;
            value = Convert.ToInt32(GetRandomValue(Convert.ToInt32(totalNum / 10f)));
            dr[0] = value;
            dr[1] = "2月";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            totalNum -= value;
            value = Convert.ToInt32(GetRandomValue(Convert.ToInt32(totalNum / 9f)));
            dr[0] = value;
            dr[1] = "3月";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            totalNum -= value;
            value = Convert.ToInt32(GetRandomValue(Convert.ToInt32(totalNum / 8f)));
            dr[0] = value;
            dr[1] = "4月";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            totalNum -= value;
            value = Convert.ToInt32(GetRandomValue(Convert.ToInt32(totalNum / 7f)));
            dr[0] = value;
            dr[1] = "5月";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            totalNum -= value;
            value = Convert.ToInt32(GetRandomValue(Convert.ToInt32(totalNum / 6f)));
            dr[0] = value;
            dr[1] = "6月";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            totalNum -= value;
            value = Convert.ToInt32(GetRandomValue(Convert.ToInt32(totalNum / 5f)));
            dr[0] = value;
            dr[1] = "7月";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            totalNum -= value;
            value = Convert.ToInt32(GetRandomValue(Convert.ToInt32(totalNum / 4f)));
            dr[0] = value;
            dr[1] = "8月";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            totalNum -= value;
            value = Convert.ToInt32(GetRandomValue(Convert.ToInt32(totalNum / 3f)));
            dr[0] = value;
            dr[1] = "9月";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            totalNum -= value;
            value = Convert.ToInt32(GetRandomValue(Convert.ToInt32(totalNum / 2f)));
            dr[0] = value;
            dr[1] = "10月";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            totalNum -= value;
            value = Convert.ToInt32(GetRandomValue(Convert.ToInt32(totalNum / 1f)));
            dr[0] = value;
            dr[1] = "11月";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            totalNum -= value;
            dr[0] = totalNum;
            dr[1] = "12月";
            dt.Rows.Add(dr);
            #endregion

            DiagramForm form = new DiagramForm(dt, "科室名称", "Name");
            form.CanMouseDown = false;
            form.ShowDialog();
        }
    }
}