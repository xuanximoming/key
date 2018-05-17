using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace YidanSoft.Core.Communication
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICommunicationCallback))]
    public interface ICommunicationService
    {
        /// <summary>
        /// 用户加入
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        //IsOneWay = false等待服务器完成对方法处理;IsInitiating = true启动Session会话,IsTerminating = false 设置服务器发送回复后关闭会话
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        void Join(string name);

        /// <summary>
        /// 用户离开
        /// </summary>
        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
        void Leave(CommunicationUser user);

        /// <summary>
        /// 用户发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="user"></param>
        [OperationContract(IsOneWay = true, IsInitiating = true, IsTerminating = false)]
        void SendMessage(string message, CommunicationUser userFrom, CommunicationUser userTo);

        /// <summary>
        /// 获得当前在线的所有用户列表
        /// </summary>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        List<CommunicationUser> GetAllOnLineUsers();
    }

    interface ICommunicationCallback
    {
        /// <summary>
        /// 用户离开
        /// </summary>
        /// <param name="name"></param>
        [OperationContract(IsOneWay = true)]
        void UserLeave(CommunicationUser user);

        /// <summary>
        /// 显示用户发送过来的消息
        /// </summary>
        /// <param name="message"></param>
        [OperationContract(IsOneWay = true)]
        void ShowMessage(string message, CommunicationUser user);
    }

    /// <summary>
    /// 设定消息的类型
    /// </summary>
    public enum MessageType 
    { 
        /// <summary>
        /// 实用户退出系统
        /// </summary>
        UserLeave,

        /// <summary>
        /// 发送消息
        /// </summary>
        SendMessage
    };

    /// <summary>
    /// 定义一个本例的事件消息类. 创建包含有关事件的其他有用的信息的变量，只要派生自EventArgs即可。
    /// </summary>
    public class ChatEventArgs : EventArgs
    {
        public MessageType MessageType;
        public CommunicationUser User;
        public string Message;
    }
}
