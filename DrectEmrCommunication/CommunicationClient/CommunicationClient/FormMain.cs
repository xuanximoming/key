using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace YidanSoft.Core.Communication
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// 客户端通讯类
        /// </summary>
        CommunicationClient client;

        /// <summary>
        /// .ctor
        /// </summary>
        public FormMain()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK);
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnLogin.Text == "登录")
                {
                    if (textBoxUserID.Text.Trim() != "" && textBoxUserName.Text.Trim() != "")
                    {
                        client = new CommunicationClient(this);
                        client.Login(textBoxUserID.Text, textBoxUserName.Text);
                        btnLogin.Text = "下线";
                    }
                }
                else
                {
                    client.Leave(textBoxUserID.Text.Trim(), textBoxUserName.Text.Trim());
                    btnLogin.Text = "登录";
                }
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("未连接上主机!", "错误", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK);
            }
        }
    }
}
