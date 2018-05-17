using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.Configuration;
using System.ServiceModel.Channels;

namespace YidanSoft.Core.Communication
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// 宿主服务
        /// </summary>
        ServiceHost host;

        public static DataGridView DGVAllCurrentUsers;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            DGVAllCurrentUsers = dataGridViewAllCurrentUser;
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        Uri BeginListener()
        {
            try
            {
                NetTcpBinding bind = new NetTcpBinding();
                Uri uri = new Uri(ConfigurationManager.AppSettings["conAddress"]);//从配置文件中读取服务的Url
                host = new ServiceHost(typeof(CommunicationService), uri);
                //if中的代码可以没有，但是如果想利用Svctuil.exe生成代理类的时候，就需要下面的代码，否则将会报错，无法解析元数据
                if (host.Description.Behaviors.Find<System.ServiceModel.Description.ServiceMetadataBehavior>() == null)
                {
                    BindingElement metaElement = new TcpTransportBindingElement();
                    CustomBinding metaBind = new CustomBinding(metaElement);
                    host.Description.Behaviors.Add(new System.ServiceModel.Description.ServiceMetadataBehavior());
                    host.AddServiceEndpoint(typeof(System.ServiceModel.Description.IMetadataExchange), metaBind, "MEX");
                }
                host.Open();

                return uri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        void CloseListener()
        {
            try
            {
                if (host != null)
                {
                    host.Abort();
                    host.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnBeginListener_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnBeginListener.Text == "开始监听")
                {
                    btnBeginListener.Text = "结束监听";
                    Uri uri = BeginListener();

                    listBoxMessage.Items.Clear();
                    listBoxMessage.Items.Add("----------------------------------------------------------------------");
                    listBoxMessage.Items.Add(System.DateTime.Now.ToString("yyyy年MM月dd HH时mm分ss秒"));
                    listBoxMessage.Items.Add(string.Format("服务器开始监听: endpoint {0}", uri.ToString()));
                    listBoxMessage.Items.Add("");
                }
                else
                {
                    btnBeginListener.Text = "开始监听";
                    CloseListener();

                    listBoxMessage.Items.Add(System.DateTime.Now.ToString("yyyy年MM月dd HH时mm分ss秒"));
                    listBoxMessage.Items.Add("服务器结束监听");
                    listBoxMessage.Items.Add("----------------------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK);
            }
        }

        public void ShowAllCurrentUsers()
        {
            
        }

        private void buttonHide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;    //使关闭时窗口向右下角缩小的效果
            notifyIcon1.Visible = true;
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Show();
            WindowState = FormWindowState.Normal;
            this.Focus();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
