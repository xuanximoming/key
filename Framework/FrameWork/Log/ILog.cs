using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Log
{
   /// <summary>
   /// 日志接口
   /// </summary>
   public interface ILog
   {
      /// <summary>
      /// 写日志
      /// </summary>
      void Write(string message);
   }
}
