using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using DrectSoft.FrameWork.BizBus.Service;
using System.ServiceModel;

namespace DrectSoft.Core
{
    /// <summary>
    /// 日志类接口
    /// </summary>
    [ServiceContract(ProtectionLevel = System.Net.Security.ProtectionLevel.None)]
    public interface IDrectSoftLog
    {
        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="message">Message</param>
        [OperationContract]
        void Debug(object message);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message">Message</param>
        [OperationContract]
        void Info(object message);

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="message">Message</param>
        [OperationContract]
        void Warn(object message);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">Message</param>
        [OperationContract]
        void Error(object message);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="message">Message</param>
        [OperationContract]
        void Fatal(object message);
    }
}
