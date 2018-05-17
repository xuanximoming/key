using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using System;
using System.Windows.Forms;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class ApartCell : DevBaseForm
    {

        public ApartCell()
        {
            InitializeComponent();
        }
        int incriRow = 1;
        int incriCol = 1;
        public ApartCell(int row, int col)
        {
            InitializeComponent();
            this.incriRow = row;
            this.incriCol = col;
            if (row > 1)
            {
                this.nudRow.Maximum = row;
                this.nudRow.Value = row;
            }

            this.nudColumn.Value = col;
            this.nudColumn.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (intRow % incriRow != 0 && intColumn % incriCol == 0 && intRow > 1)
            {
                MessageBox.Show("行数必须是 " + incriRow.ToString() + " 的约数", "不能拆分", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (intColumn % incriCol != 0 && intRow % incriRow == 0 && intColumn > 1)
            {
                MessageBox.Show("列数必须是 " + incriCol.ToString() + " 的约数", "不能拆分", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (intColumn % incriCol != 0 && intRow % incriRow != 0 && intRow > 1 && intColumn > 1)
            {
                MessageBox.Show("行数必须是 " + incriRow.ToString() + " 的约数\n列数必须是 " + incriCol.ToString() + " 的约数", "不能拆分", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 要拆分的行数
        /// </summary>
        public int intRow
        {
            get
            {
                return Convert.ToInt32(this.nudRow.Value);
            }
        }

        /// <summary>
        /// 要拆分的列数
        /// </summary>
        public int intColumn
        {
            get
            {
                return Convert.ToInt32(this.nudColumn.Value);
            }
        }

        /// <summary>
        /// 回车切换焦点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-07</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                DS_Common.win_KeyPress(e);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
