using DrectSoft.Common.Ctrs.FORM;
using System;
using System.Data;
using System.Drawing;
using System.Text;

namespace DrectSoft.Core.MainEmrPad
{
    public partial class ReportDetail : DevBaseForm
    {

        /// <summary>
        /// 返回值
        /// </summary>
        public string CommitValue { get; set; }

        internal DataTable SourceTable
        {
            get { return _sourceTable; }
        }
        private DataTable _sourceTable;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        public ReportDetail(DataTable dataSource)
        {
            InitializeComponent();

            dataSource.Columns.Add("CHECK", typeof(bool));
            _sourceTable = dataSource.Clone();
            this.gridControl1.DataSource = dataSource;
        }

        private void Commit()
        {
            StringBuilder builder = new StringBuilder();

            DataTable dt = gridControl1.DataSource as DataTable;
            DataTable dttemp = dt.Clone();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["CHECK"].ToString().ToUpper() == "TRUE")
                {
                    DataRow newDataRow = dttemp.NewRow();
                    newDataRow.ItemArray = dr.ItemArray;
                    dttemp.Rows.Add(newDataRow);
                }
            }
            dttemp.AcceptChanges();

            foreach (DataRow row in dttemp.Rows)
            {
                if (row["RESULT"].ToString().Trim() != "")
                {
                    bool is星号 = row["UNIT"].ToString().ToString().Trim().StartsWith("10");

                    builder.Append(" ");
                    builder.Append(row["ITEMNAME"].ToString());
                    builder.Append(" ");
                    builder.Append(row["RESULT"].ToString());

                    if (is星号)
                    {
                        builder.Append("*");//结果和单位用空格区分
                    }
                    else
                    {
                        builder.Append(" ");//结果和单位用空格区分
                    }

                    builder.Append(row["UNIT"].ToString());
                    builder.Append(",");
                }
            }
            CommitValue = builder.ToString().Trim(',') + " ";
        }

        private void CopyRowValue(DataRow sourceRow, DataRow targetRow)
        {

            foreach (DataColumn col in sourceRow.Table.Columns)
            {
                if (targetRow.Table.Columns.Contains(col.ColumnName))
                    targetRow[col.ColumnName] = sourceRow[col.ColumnName];
            }
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.CellValue == null) return;

            DataRowView drv = gridView1.GetRow(e.RowHandle) as DataRowView;
            string value = drv["HIGHFLAG"].ToString().Trim();

            if (value == "正常" || value == "" || value == ("阴性") || value.Contains("N") || value.Contains("M"))
            { }
            else if (value.IndexOf("低") >= 0 || value.Contains("L"))
            {
                e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);
            }
            else if (value.IndexOf("高") >= 0 || value.Contains("H"))
            {
                e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds);
            }
            else if (value.Equals("阳性") || value.Contains("P") || value.Contains("Q"))
            {
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Pink, e.Bounds);
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //if (e.RowHandle < 0) return;
            //if (!e.Column.FieldName.Equals("CHECK"))
            //    return;
            //DataRow currentRow = gridView1.GetDataRow(e.RowHandle);
            //if (e.Value == null) return;

            //if (e.Value.Equals(true))
            //{
            //    //currentRow["ReportCatalog"] = m_tempcatalog;
            //    DataRow newRow = SourceTable.NewRow();
            //    CopyRowValue(currentRow, newRow);
            //    SourceTable.Rows.Add(newRow);
            //}
            //else
            //{
            //    DataRow[] existsRows = SourceTable.Select("Line=" + currentRow["Line"] + "");
            //    if (existsRows.Length > 0)
            //    {
            //        SourceTable.Rows.Remove(existsRows[0]);
            //    }

            //}
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Commit();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 全选 or 全不选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEditAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditAll.Checked)
            {
                checkEdit1.Checked = false;
            }
            CheckAll();
        }

        /// <summary>
        /// 选中异常 or 取消选中异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                checkEditAll.Checked = false;
            }
            CheckNonormal();
        }

        private void CheckNonormal()
        {
            if (checkEdit1.Checked)
            {
                DataTable dt = gridControl1.DataSource as DataTable;
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["HighFlag"].ToString() != "正常" &&
                            dr["HighFlag"].ToString().Trim() != "M" &&
                            dr["HighFlag"].ToString().Trim() != "E" &&
                            dr["HighFlag"].ToString() != "错误" &&
                            dr["HighFlag"].ToString() != "" &&
                            dr["HighFlag"].ToString() != "N")
                        {
                            dr["CHECK"] = true;
                        }
                    }
                }
            }
            else
            {
                DataTable dt = gridControl1.DataSource as DataTable;
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["HighFlag"].ToString() != "正常" &&
                             dr["HighFlag"].ToString().Trim() != "M" &&
                              dr["HighFlag"].ToString().Trim() != "E" &&
                            dr["HighFlag"].ToString() != "错误" &&
                            dr["HighFlag"].ToString() != "")
                        {
                            dr["CHECK"] = false;
                        }
                    }
                }
            }
        }

        private void CheckAll()
        {
            if (checkEditAll.Checked)
            {
                DataTable dt = gridControl1.DataSource as DataTable;
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["CHECK"] = true;
                    }
                }
            }
            else
            {
                DataTable dt = gridControl1.DataSource as DataTable;
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["CHECK"] = false;
                    }
                }
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {

        }
    }
}
