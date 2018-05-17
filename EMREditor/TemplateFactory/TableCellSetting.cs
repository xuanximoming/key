using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class TableCellSetting : DevBaseForm
    {
        TPTextCell m_Cell;
        public TableCellSetting(TPTextCell cell)
        {
            InitializeComponent();
            m_Cell = cell;
            InitBorderComboBox();
            InitBorderValue();
            InitSpaceValue();
        }

        private void InitBorderComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");

            DataRow dr = dt.NewRow();
            dr["ID"] = "1";
            dr["NAME"] = "有";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "0";
            dr["NAME"] = "无";
            dt.Rows.Add(dr);

            comboBoxBorderTop.DataSource = dt.Copy();
            comboBoxBorderTop.DisplayMember = "NAME";
            comboBoxBorderTop.ValueMember = "ID";

            comboBoxBorderBottom.DataSource = dt.Copy();
            comboBoxBorderBottom.DisplayMember = "NAME";
            comboBoxBorderBottom.ValueMember = "ID";

            comboBoxBorderLeft.DataSource = dt.Copy();
            comboBoxBorderLeft.DisplayMember = "NAME";
            comboBoxBorderLeft.ValueMember = "ID";

            comboBoxBorderRight.DataSource = dt.Copy();
            comboBoxBorderRight.DisplayMember = "NAME";
            comboBoxBorderRight.ValueMember = "ID";
        }

        private void InitBorderValue()
        {
            comboBoxBorderTop.SelectedValue = m_Cell.BorderWidthTop > 0 ? "1" : "0";
            comboBoxBorderBottom.SelectedValue = m_Cell.BorderWidthBottom > 0 ? "1" : "0";
            comboBoxBorderLeft.SelectedValue = m_Cell.BorderWidthLeft > 0 ? "1" : "0";
            comboBoxBorderRight.SelectedValue = m_Cell.BorderWidthRight > 0 ? "1" : "0";
        }

        private void InitSpaceValue()
        {
            spinEditSpaceTop.EditValue = m_Cell.PaddingTop;
            spinEditSpaceBottom.EditValue = m_Cell.PaddingBottom;
            spinEditSpaceLeft.EditValue = m_Cell.PaddingLeft;
            spinEditSpaceRight.EditValue = m_Cell.PaddingRight;
        }

        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            //单元格边框
            m_Cell.BorderWidthTop = Convert.ToInt32(comboBoxBorderTop.SelectedValue);
            m_Cell.BorderWidthRight = Convert.ToInt32(comboBoxBorderRight.SelectedValue);
            m_Cell.BorderWidthBottom = Convert.ToInt32(comboBoxBorderBottom.SelectedValue);
            m_Cell.BorderWidthLeft = Convert.ToInt32(comboBoxBorderLeft.SelectedValue);

            //单元格边距
            int paddingTop = Convert.ToInt32(spinEditSpaceTop.EditValue);
            int paddingRight = Convert.ToInt32(spinEditSpaceRight.EditValue);
            int paddingBottom = Convert.ToInt32(spinEditSpaceBottom.EditValue);
            int paddingLeft = Convert.ToInt32(spinEditSpaceLeft.EditValue);

            foreach (TPTextCell cell in m_Cell.OwnerRow.Cells)
            {
                if (!cell.Merged)
                {
                    cell.PaddingTop = paddingTop;
                    cell.PaddingRight = paddingRight;
                    cell.PaddingBottom = paddingBottom;
                    cell.PaddingLeft = paddingLeft;
                }
            }

            //重新计算界面中元素的位置，然后重绘界面中的元素
            m_Cell.OwnerDocument.Refresh2();
        }

        private void simpleButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}