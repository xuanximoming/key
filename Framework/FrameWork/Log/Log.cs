using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Log
{
   /// <summary>
   /// 日志
   /// </summary>
   public class Log:ILog
   {
      /// <summary>
      /// 写日志
      /// </summary>
      /// <param name="message"></param>
      public void Write(string message)
      {
         Console.WriteLine("Log: "+message);
      }
   }

   /// <summary>
   /// 日志工厂，生产ILog接口
   /// </summary>
   public class LogFactory
   {
      /// <summary>
      /// 创建ILog接口
      /// </summary>
      /// <returns></returns>
      public static ILog Create()
      {
         return new Log();
      }
   }
}
