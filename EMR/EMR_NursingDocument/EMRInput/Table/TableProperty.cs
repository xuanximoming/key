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
namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class TableProperty : Form
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
