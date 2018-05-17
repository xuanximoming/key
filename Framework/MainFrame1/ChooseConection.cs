using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Core;
using DrectSoft.Common.Ctrs.FORM;
using System.Xml;
using System.IO;
using System.Collections;

namespace DrectSoft.MainFrame
{
    /// <summary>
    /// 用于一个客户端程序选择多个数据库的功能
    /// add by ywk 2013年4月22日9:44:52 
    /// </summary>
    public partial class ChooseConection : DevBaseForm
    {
        /// <summary>
        /// 当前配置数据库连接源的路径
        /// </summary>
        private string currentsoftpath;

        public string CurrentSoftPath
        {
            get { return currentsoftpath; }
            set { currentsoftpath = value; }
        }

        public ChooseConection()
        {
            InitializeComponent();
        }

        private bool isselectedsource;
        /// <summary>
        /// 是否选择了数据源
        /// </summary>
        public bool IsSelectedSource
        {
            get { return isselectedsource; }
            set { isselectedsource = value; }
        }
        private string selectedsource;
        /// <summary>
        /// 选择的数据源是哪个
        /// add by ywk 
        /// </summary>
        public string SelectedSource
        {
            get { return selectedsource; }
            set { selectedsource = value; }
        }

        private string chooseDataText;
        /// <summary>
        /// 返回选择的哪个库，追加到主界面用户更好的区分
        /// add by ywk 2013年4月23日11:21:07 
        /// </summary>
        public string ChooseDataText
        {
            get { return chooseDataText; }
            set { chooseDataText = value; }
        }
        public ChooseConection(string currentsoftpath)
            : this()
        {
            CurrentSoftPath = currentsoftpath;
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ChooseConection_Load(object sender, EventArgs e)
        {
            BindDataSourceText();
            //MessageBox.Show("a");
        }

        private XmlDocument xmlDoc = null;
        private Dictionary<string, string> m_dataSourceitem;
        /// <summary>
        /// 下拉框选项改为动态读取，以便后期维护,不采用绑定死的
        /// add by ywk 2013年4月22日11:19:45 
        /// </summary>
        private void BindDataSourceText()
        {
            try
            {
                xmlDoc = new XmlDocument();
                m_dataSourceitem = new Dictionary<string, string>();
                ArrayList arraySource = new ArrayList();
                if (File.Exists(CurrentSoftPath + "ConnectServer.xml"))
                {
                    xmlDoc.Load(CurrentSoftPath + "ConnectServer.xml");
                    XmlNodeList xnodeList = xmlDoc.SelectNodes("/Connection/SHOWCONTENT");
                    foreach (XmlNode node in xnodeList)
                    {
                        m_dataSourceitem.Add(node.Attributes["TEXT"].Value.ToString(), node.Attributes["target"].Value.ToString());
                        arraySource.Add(node.Attributes["TEXT"].Value.ToString());
                    }
                }
                for (int i = 0; i < arraySource.Count; i++)
                {
                    cmbDataSource.Properties.Items.Add(arraySource[i].ToString());
                }
                cmbDataSource.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("BindDataSourceText方法出错:" + ex.Message);
            }

        }
        /// <summary>
        /// 是否选择了数据源
        /// add by ywk 2013年4月22日11:04:29  
        /// </summary>
        /// <returns></returns>
        //public bool IsSelected()
        //{
        //    IsSelectedSource = false;
        //    return IsSelectedSource;
        //}
        /// <summary>
        /// 确定了选择的数据源
        /// add by ywk 2013年4月22日11:18:32 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonOK1_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dataSourceitem.ContainsKey(this.cmbDataSource.Text))
                {
                    SelectedSource = m_dataSourceitem[this.cmbDataSource.Text].ToString();
                    IsSelectedSource = true;
                    ChooseDataText = this.cmbDataSource.Text.ToString().Trim();
                    this.Close();
                }
                else
                {
                    IsSelectedSource = false;
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您选择的数据源尚未配置！请联系管理员");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void ChooseConection_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.ExitThread();
        }

        private void panelControl3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DevButtonCancel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbDataSource_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("ad");
            //选择数据源后，回车焦点对应到确定add by ywk  2013年7月12日16:11:00
            if (e.KeyValue==13)
            {
                DevButtonOK1.Focus();
            }
        }

        private void cmbDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        //public  void ShowUI()
        //{
        //    var sql_helper = DataAccessFactory.DefaultDataAccess;
        //    string sql = @"select * from emr_data_version order by createdatetime desc;";
        //    DataTable dtVerson = sql_helper.ExecuteDataTable(sql, System.Data.CommandType.Text);
        //    string dataVersion = dtVerson.Rows[0]["VERSIONID"].ToString();  // 获取数据库版本号
        //    MessageBox.Show(dataVersion);
        //}
    }
}