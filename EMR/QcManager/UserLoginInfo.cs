using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.DSSqlHelper;

namespace DrectSoft.Emr.QcManager
{
    public partial class UserLoginInfo : DevExpress.XtraEditors.XtraUserControl
    {
        public UserLoginInfo()
        {
            InitializeComponent();
            string sql="select * from USERLOGIN";
            this.gridControl1.DataSource=DS_SqlHelper.ExecuteDataTable(sql);
            
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void DevButtonPrint1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            comboBox2.Text = "";
            comboBox1.Text = "";
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void labelControl22_Click(object sender, EventArgs e)
        {

        }

        private void DevButtonImportExcel1_Click(object sender, EventArgs e)
        {

        }

        private void DevButtonQurey1_Click(object sender, EventArgs e)
        {
            string sql = "select * from USERLOGIN where 1=1";
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

        private void dateEdit_end_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {

        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

        }

        private void dateEdit_begin_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelControl3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
