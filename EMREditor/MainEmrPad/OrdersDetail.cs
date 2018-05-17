using DrectSoft.Common.Ctrs.FORM;
using System;
using System.Data;
using System.Text;

namespace DrectSoft.Core.MainEmrPad
{
    public partial class OrdersDetail : DevBaseForm
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
        public OrdersDetail(DataTable dataSource)
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
                if (row["order_text"].ToString().Trim() != "")
                {
                    builder.Append(row["order_text"].ToString());
                    builder.Append(" ");
                    builder.Append(row["dosage"].ToString());
                    builder.Append(row["dosage_units"].ToString());
                    builder.Append(" ");
                    builder.Append(row["administration"].ToString());
                    builder.Append(" ");
                    builder.Append(row["frequency"].ToString());

                    builder.Append(row["ORDER_CLASS"].ToString());
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
            CheckAll();
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
