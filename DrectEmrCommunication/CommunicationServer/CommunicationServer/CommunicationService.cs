using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;

namespace YidanSoft.Core.Communication
{
    /// <summary>
    /// 通讯服务类
    /// InstanceContextMode.PerSession 服务器为每个客户会话创建一个新的上下文对象
    /// ConcurrencyMode.Multiple 异步的多线程实例
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CommunicationService : ICommunicationService
    {
        /// <summary>
        /// 定义一个静态对象用于线程部份代码块的锁定，用于lock操作，适用于m_OnlyOneLoginUsers
        /// </summary>
        private static Object m_SyncObjForOnlyOneLoginUsers = new Object();

        /// <summary>
        /// 定义一个静态对象用于线程部份代码块的锁定，用于lock操作，适用于m_CurrentOnLineUsers
        /// </summary>
        private static Object m_SyncObjForCurrentOnLineUsers = new Object();
        
        /// <summary>
        /// 回调通讯接口
        /// </summary>
        ICommunicationCallback m_Callback = null;
        
        /// <summary>
        /// 定义用于把处理程序赋予给事件的委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CommunicationEventHandler(object sender, ChatEventArgs e);

        //适用于【单点登录】，创建一个静态Dictionary（表示键和值）集合(字典)，用于记录在线成员
        static Dictionary<CommunicationUser, CommunicationEventHandler> m_OnlyOneLoginUsers = new Dictionary<CommunicationUser, CommunicationEventHandler>();
        //适用于【发送消息】，创建一个静态Dictionary（表示键和值）集合(字典)，用于记录在线成员
        static Dictionary<CommunicationUser, CommunicationEventHandler> m_CurrentOnLineUsers = new Dictionary<CommunicationUser, CommunicationEventHandler>();

        public void Join(string name)
        {
            try
            {
                if (name == "" || name == null) return;

                //实例化通讯用户对象
                CommunicationUser user = new CommunicationUser
                {
                    UserID = name.Split('_')[0],
                    UserName = name.Split('_')[1],
                    IP = name.Split('_')[2],
                    ProcessID = name.Split('_')[3]
                };

                Join1(user);
                Join2(user);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 控制单点登录
        /// <summary>
        /// 记录加入的客户端的用户,用于单点登录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        void Join1(CommunicationUser user)
        {
            try
            {
                bool userAdded = false;

                lock (m_SyncObjForOnlyOneLoginUsers)//线程的同步性，同步访问多个线程的任何变量，利用lock(独占锁)，确保数据访问的唯一性。
                {
                    //同一个用户、同一台电脑、同一个进程的重复请求不用处理
                    //if ((m_OnlyOneLoginUsers.Where((pair) => { return pair.Key.ToString() == user.ToString(); })).Count() > 0) return;
                    if (m_OnlyOneLoginUsers.Keys.Contains(user)) return;

                    //向已经登录的用户发送消息
                    //foreach (CommunicationUser key in m_OnlyOneLoginUsers.Keys)
                    for (int i = m_OnlyOneLoginUsers.Count() - 1; i >= 0; i--)
                    {
                        CommunicationUser key = m_OnlyOneLoginUsers.Keys.ElementAt(i);

                        //只能向以当前用户账号登录系统的用户发送消息
                        if (key.UserID == user.UserID && key.UserName == user.UserName && m_OnlyOneLoginUsers[key] != null)
                        {
                            //给其他用户发送需要退出系统的消息
                            ChatEventArgs e = new ChatEventArgs();
                            e.MessageType = MessageType.UserLeave;
                            e.User = key;

                            //循环将在线的用户广播信息
                            foreach (CommunicationEventHandler handler in m_OnlyOneLoginUsers[key].GetInvocationList())
                            {
                                //异步方式调用多路广播委托的调用列表中的ChatEventHandler 
                                handler.BeginInvoke(this, e, null, null);
                            }

                            //发送完成后移除用户
                            m_OnlyOneLoginUsers.Remove(key);
                            RemoveUserInDGV(key);
                        }
                    }                    

                    //记录下当前登录的用户
                    //if ((m_OnlyOneLoginUsers.Where((pair) => { return pair.Key.ToString() == user.ToString(); })).Count() == 0)
                    if (!m_OnlyOneLoginUsers.Keys.Contains(user))
                    {
                        m_OnlyOneLoginUsers.Add(user, MyEventHandler);
                        userAdded = true;
                        AddUserInDGV(user);
                    }
                }

                if (userAdded)
                {
                    //获取当前操作客户端实例的通道给IChatCallback接口的实例callback,此通道是一个定义为IChatCallback类型的泛类型,通道的类型是事先服务契约协定好的双工机制。
                    m_Callback = OperationContext.Current.GetCallbackChannel<ICommunicationCallback>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用户退出系统
        /// </summary>
        /// <param name="user"></param>
        public void Leave(CommunicationUser user)
        {
            try
            {
                if (user == null) return;

                lock (m_SyncObjForOnlyOneLoginUsers)
                {
                    m_OnlyOneLoginUsers.Remove(user);
                }

                lock (m_SyncObjForCurrentOnLineUsers)
                {
                    m_CurrentOnLineUsers.Remove(user);
                }

                RemoveUserInDGV(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 记录当前在线的用户
        /// <summary>
        /// 记录当前在线的用户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="user"></param>
        void Join2(CommunicationUser user)
        {
            try
            {
                lock (m_SyncObjForCurrentOnLineUsers)//线程的同步性，同步访问多个线程的任何变量，利用lock(独占锁)，确保数据访问的唯一性。
                {
                    //记录下当前登录的用户
                    //if ((m_CurrentOnLineUsers.Where((pair) => { return pair.Key.ToString() == user.ToString(); })).Count() == 0)
                    if (!m_CurrentOnLineUsers.Keys.Contains(user))
                    {
                        m_CurrentOnLineUsers.Add(user, MyEventHandler);
                        AddUserInDGV(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userFrom"></param>
        /// <param name="userTo"></param>
        public void SendMessage(string message, CommunicationUser userFrom, CommunicationUser userTo)
        {
            //给指定的用户发送消息
            ChatEventArgs e = new ChatEventArgs();
            e.MessageType = MessageType.UserLeave;
            e.User = userTo;
            e.Message = userFrom.UserName + ": " + message;

            //循环将在线的用户广播信息
            var users = (m_CurrentOnLineUsers.Where((pair) => { return pair.ToString() == userTo.ToString(); })).ToList();
            users.ForEach((pair) => 
            {
                pair.Value.GetInvocationList().ToList().ForEach((handler) =>
                {
                    ((CommunicationEventHandler)handler).BeginInvoke(this, e, null, null);
                });
            });
        }

        /// <summary>
        /// 获得所有在线用户
        /// </summary>
        /// <returns></returns>
        public List<CommunicationUser> GetAllOnLineUsers()
        {
            return m_CurrentOnLineUsers.Keys.ToList();
        }

        //回调总入口点，根据客户端动作通知对应客户端执行对应的操作
        private void MyEventHandler(object sender, ChatEventArgs e)
        {
            try
            {
                switch (e.MessageType)
                {
                    case MessageType.UserLeave:
                        m_Callback.UserLeave(e.User);
                        break;
                    case MessageType.SendMessage:
                        m_Callback.ShowMessage(e.Message, e.User);
                        break;
                }
            }
            catch
            {
                Leave(e.User);
            }
        }

        /// <summary>
        /// DGV中移除指定用户
        /// </summary>
        /// <param name="user"></param>
        private void RemoveUserInDGV(CommunicationUser user)
        {
            try
            {
                DataTable dt = FormMain.DGVAllCurrentUsers.DataSource as DataTable;
                if (dt != null)
                {
                    var users = (from DataRow dr in dt.Rows
                                 where dr["USERID"].ToString().Equals(user.UserID)
                                 select dr).ToList();
                    users.ForEach((dr) => { dt.Rows.Remove(dr); });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// DGV中增加指定用户
        /// </summary>
        /// <param name="user"></param>
        private void AddUserInDGV(CommunicationUser user)
        {
            try
            {
                DataTable dt = FormMain.DGVAllCurrentUsers.DataSource as DataTable;
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("USERID");
                    dt.Columns.Add("USERNAME");
                    FormMain.DGVAllCurrentUsers.DataSource = dt;
                }
                if (dt.Rows.Cast<DataRow>().Where((dr) => { return dr["USERID"].ToString() == user.UserID; }).Count() == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["USERID"] = user.UserID;
                    dr["USERNAME"] = user.UserName;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// 通讯的用户类
    /// </summary>
    public class CommunicationUser
    {
        /// <summary>
        /// 用户工号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户所在客户端的IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 用户所在客户端的进程ID
        /// </summary>
        public string ProcessID { get; set; }

        public override string ToString()
        {
            try
            {
                return UserID + "_" + UserName + "_" + IP + "_" + ProcessID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool  Equals(object obj)
        {
            CommunicationUser user = obj as CommunicationUser;
            if (user != null)
            {
                return this.ToString().Equals(user.ToString());
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
