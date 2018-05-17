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
    public partial class ActiveUser : UserControl
    {
        public ActiveUser()
        {
            InitializeComponent();
            string sql = "select * from ACTIVEUSER";
            this.gridControl1.DataSource = DS_SqlHelper.ExecuteDataTable(sql);
        }

        private void ActiveUser_Load(object sender, EventArgs e)
        {

        }

        private void DevButtonQurey1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "select * from ACTIVEUSER where 1=1";
                if (comboBox2.Text != "")
                {
                    sql += " and DEPTMENT=" + "'" + comboBox2.Text + "'";
                }
                if (comboBox1.Text != "")
                {
                    sql += " and STATE=" + "'" + comboBox1.Text + "'";
                }
                this.gridControl1.DataSource = DS_SqlHelper.ExecuteDataTable(sql);
            }
            catch
            {
                MessageBox.Show("没有数据");

                
            }
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            comboBox2.Text = "";
            comboBox1.Text = "";
        }
    }
}
