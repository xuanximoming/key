using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.DSSqlHelper;

namespace DrectSoft.Emr.QcManager
{
    public partial class DiagGroupInfo : UserControl
    {
        public DiagGroupInfo()
        {
            InitializeComponent();
            string sql = "select * from DIAGNOSIS_NEW";
            this.gridControl1.DataSource = DS_SqlHelper.ExecuteDataTable(sql);
        }

        private void DevButtonQurey1_Click(object sender, EventArgs e)
        {
            string sql = "select * from DIAGNOSIS_NEW where 1=1";
            if (textBox1.Text != "")
            {
                sql += " and PY like" + "'" + '%'+textBox1.Text+'%' + "'";
            }
            if (textBox2.Text != "")
            {
                sql += " and WB like" + "'" + '%' + textBox2.Text + '%' + "'";
            }
            this.gridControl1.DataSource = DS_SqlHelper.ExecuteDataTable(sql);
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
    }
}
