using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class TableProperty : DevBaseForm
    {
        private TPTextTable table = null;
        
        private TPTextCell cell = null;

        public TableProperty()
        {
            InitializeComponent();
        }

        public TableProperty( TPTextTable table, TPTextCell cell)
        {
            InitializeComponent();
            this.table = table;
            this.cell = cell;

            txtHeader.Text = this.table.Header;
            switch (this.table.HorizontalAlignment)
            {
                case 1:
                    checkBox1_Click(null,null);
                    break;
                case 2:
                    checkBox2_Click(null, null);
                    break;
                case 3:
                    checkBox3_Click(null, null);
                    break;
            }
            checkBoxTable.Checked = this.table.HiddenAllBorder;
            this.txtHeader.Focus();
        }

        #region 事件
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.table.Header = txtHeader.Text.Trim();
            if (cbLeftAlign.Checked == true)
            {
                this.table.HorizontalAlignment = 1;
            }
            else if (cbCenterAlign.Checked == true)
            {
                this.table.HorizontalAlignment = 2;
            }
            else
            {
                this.table.HorizontalAlignment = 3;
            }
            this.table.HiddenAllBorder = checkBoxTable.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            cbLeftAlign.Checked = true;
            cbCenterAlign.Checked = false;
            cbRightAlign.Checked = false;

            InitCheckBoxColor();
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            cbLeftAlign.Checked = false;
            cbCenterAlign.Checked = true;
            cbRightAlign.Checked = false;

            InitCheckBoxColor();
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            cbLeftAlign.Checked = false;
            cbCenterAlign.Checked = false;
            cbRightAlign.Checked = true;

            InitCheckBoxColor();
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

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-07</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    DS_Common.cbx_KeyPress(sender);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        private void InitCheckBoxColor()
        {
            ArrayList checkList = new ArrayList();
            checkList.Add(cbLeftAlign);
            checkList.Add(cbCenterAlign);
            checkList.Add(cbRightAlign);

            foreach (CheckBox cb in checkList)
            {
                if (cb.Checked == true)
                {
                    cb.BackColor = Color.Blue;
                }
                if (cb.Checked == false)
                {
                    cb.BackColor = Color.Transparent;
                }
            }
        }

        public TPTextTable Table
        {
            get { return table; }
            set { table = value; }
        }
        public TPTextCell Cell
        {
            get { return cell; }
            set { cell = value; }
        }
    }
}
