using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Wordbook;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.MainFrame.Login
{
    public partial class FormReg : DevBaseForm
    {
        public FormReg()
        {
            InitializeComponent();
        }
        public FormReg(string IP)
        {
            InitializeComponent();
            this.IPInput.Text = IP;
            InitIp();
        }

        private void SBOK_Click(object sender, System.EventArgs e)
        {
            DataTable DtCount = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable("select * from CLIENT_LOG", CommandType.Text);
            string RegNum = PublicClass.DecryptDES(ConfigurationManager.AppSettings["RegNum"], "hospname");
            if (int.Parse(RegNum) <= DtCount.Rows.Count)
            {
                MessageBox.Show("已经超过注册数量！");
                return;
            }
            DataTable dt_client_ip = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(string.Format("select * from CLIENT_LOG where ip = '{0}'", this.IPInput.Text.Trim()), CommandType.Text);

            if (dt_client_ip.Rows.Count > 0)
            {
                MessageBox.Show("该机器已经注册，请不要重复注册！");
                return;
            }
            string SqlInsert = @"insert into CLIENT_LOG (IP, IP_CODE) values ('{0}', '{1}')";
            SqlInsert = string.Format(SqlInsert, this.IPInput.Text.Trim(), PublicClass.EncryptDES(this.IPInput.Text.Trim(), "ip__code"));
            int result = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(SqlInsert, CommandType.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void InitIp()
        {
            DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable("select IP from CLIENT_LOG", CommandType.Text);
            if (dt.Rows.Count > 0)
            {

                Dictionary<string, int> columnwidth = new Dictionary<string, int>();
                SqlWordbook sqlWordBook = null;
                dt.Columns["IP"].Caption = "IP地址";
                columnwidth.Add("IP", 150);
                sqlWordBook = new SqlWordbook("queryip", dt, "IP", "IP", columnwidth, "IP");
                this.lookUpEditorIP.SqlWordbook = sqlWordBook;
            }
        }

        private void ButtonDel_Click(object sender, System.EventArgs e)
        {
            int result = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(string.Format("delete from CLIENT_LOG where ip = '{0}'", this.lookUpEditorIP.Text.Trim()), CommandType.Text);
        }
    }
}
