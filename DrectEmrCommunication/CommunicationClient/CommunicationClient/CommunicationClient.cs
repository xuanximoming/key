using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.Diagnostics;

namespace YidanSoft.Core.Communication
{
    public partial class CommunicationClient : ICommunicationServiceCallback
    {
        Form m_MainForm;

        /// <summary>
        /// 客户端通讯代理类
        /// </summary>
        CommunicationServiceClient proxy;

        public CommunicationClient(Form form)
        {
            m_MainForm = form;
        }

        /// <summary>
        /// 发送用户登录系统的消息
        /// </summary>
        public void Login(string userID, string userName)
        {
            Application.DoEvents();
            proxy = new CommunicationServiceClient(new InstanceContext(this));
            string currentHostIP = GetCurrentHostIP();
            string currentProcessID = GetCurrentProcessID();
            string name = userID + "_" + userName + "_" + currentHostIP + "_" + currentProcessID;
            //proxy.BeginJoin(name, null, null);
            proxy.Join(name);
        }

        public void Leave(string userID, string userName)
        {
            CommunicationUser user = new CommunicationUser
            {
                UserID = userID,
                UserName = userName,
                IP = GetCurrentHostIP(),
                ProcessID = GetCurrentProcessID()
            };
            proxy.Leave(user);
        }

        /// <summary>
        /// 用户离开
        /// </summary>
        public void UserLeave(CommunicationUser user)
        {
            if (m_MainForm != null)
            {
                m_MainForm.Close();
            }
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="user"></param>
        public void ShowMessage(string message, YidanSoft.Core.Communication.CommunicationUser user)
        {
 
        }
        
        /// <summary>
        /// 获得当前进程的ID
        /// </summary>
        /// <returns></returns>
        public string GetCurrentProcessID()
        {
            return Process.GetCurrentProcess().Id.ToString();
        }

        /// <summary>
        /// 获得当前主机的IP
        /// </summary>
        /// <returns></returns>
        public string GetCurrentHostIP()
        {
            string strIP = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0].ToString();
            return strIP;
        }
    }
}
