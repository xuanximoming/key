using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.FrameWork.Execptions;

namespace DrectSoft.FrameWork.Execptions
{
    /// <summary> 
    /// 警告型异常，所有只是需要提出警告的异常都从此类继承
    /// </summary>
    public class WarningException:FrameWorkException
    {
        /// <summary>
        /// 初始化一个异常
        /// </summary>
        public WarningException() 
        {
        }

        /// <summary>
        /// 初始化一个异常
        /// </summary>
        /// <param name="message">错误消息</param>
        public WarningException(string message)
            : base(message)
        { }

        /// <summary>
        /// 初始化一个异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="exception">内部异常</param>
        public WarningException(string message, Exception exception)
            : base(message, exception)
        { }
    }
}