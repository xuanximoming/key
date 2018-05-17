using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Execptions
{
   /// <summary>
   /// 框架Exception基类
   /// </summary>
   public abstract class FrameWorkException : Exception
   {
      /// <summary>
      /// 错误等级
      /// </summary>
      private ExceptionLevel _level= ExceptionLevel.Low;

      /// <summary>
      /// 初始化一个异常
      /// </summary>
      public FrameWorkException()
      {
      }

      /// <summary>
      /// 初始化一个异常
      /// </summary>
      /// <param name="message">错误消息</param>
      public FrameWorkException(string message)
         : base(message)
      { }

      /// <summary>
      /// 初始化一个异常
      /// </summary>
      /// <param name="message">错误消息</param>
      /// <param name="exception">内部异常</param>
      public FrameWorkException(string message, Exception exception)
         : base(message, exception)
      { }

      /// <summary>
      /// 设置或获取错误等级
      /// </summary>
      public ExceptionLevel Level
      {
         get { return _level; }
      }
   }
}
