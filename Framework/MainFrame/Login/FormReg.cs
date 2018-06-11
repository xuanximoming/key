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
        /// <summary>
        /// 添加IP地址功能。1、超过限定数量提示，不做任何操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SBOK_Click(object sender, System.EventArgs e)
        {
            DataTable DtCount = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable("select * from CLIENT_LOG", CommandType.Text);
            string RegNum = PublicClass.DecryptDES(ConfigurationManager.AppSettings["RegNum"], "hospname");
            //超过限定数量提示，不做任何操作
            if (int.Parse(RegNum) <= DtCount.Rows.Count)
            {
                MessageBox.Show("已经超过注册数量！");
                return;
            }
            DataTable dt_client_ip = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(string.Format("select * from CLIENT_LOG where ip = '{0}'", this.IPInput.Text.Trim()), CommandType.Text);
            //该机器已经注册
            if (dt_client_ip.Rows.Count > 0)
            {
                MessageBox.Show("该机器已经注册，请不要重复注册！");
                return;
            }
            //没有超过数量，并且没有注册的IP地址，注册
            string SqlInsert = @"insert into CLIENT_LOG (IP, IP_CODE) values ('{0}', '{1}')";
            SqlInsert = string.Format(SqlInsert, this.IPInput.Text.Trim(), PublicClass.EncryptDES(this.IPInput.Text.Trim(), "ip__code"));
            int result = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(SqlInsert, CommandType.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// 初始化IP地址下拉列表
        /// </summary>
        private void InitIp()
        {
            DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable("select IP from CLIENT_LOG", CommandType.Text);
            if (dt != null)
            {

                Dictionary<string, int> columnwidth = new Dictionary<string, int>();
                SqlWordbook sqlWordBook = null;
                dt.Columns["IP"].Caption = "IP地址";
                columnwidth.Add("IP", 400);
                sqlWordBook = new SqlWordbook("queryip", dt, "IP", "IP", columnwidth, "IP");
                this.lookUpEditorIP.SqlWordbook = sqlWordBook;
            }
        }
        //删除IP
        private void ButtonDel_Click(object sender, System.EventArgs e)
        {
            int result = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(string.Format("delete from CLIENT_LOG where ip = '{0}'", this.lookUpEditorIP.Text.Trim()), CommandType.Text);
            InitIp();
            MessageBox.Show("删除成功！");
        }
    }
}
