using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Config
{
    /// <summary>
    /// 异常错误
    /// </summary>
   public class ErrMessages
   {
       /// <summary>
       /// 程序集异常信息
       /// </summary>
       /// <param name="info"></param>
       /// <returns></returns>
      public static string ErrAssemblyMessage(ErrAssemblyInfo info)
      {
         if (!info.HasAssembly)
            return "程序集未能找到";

         if (!info.AssemblyHasAttribute)
            return "程序集未标注特性";

         return "";
      }
   }
}
