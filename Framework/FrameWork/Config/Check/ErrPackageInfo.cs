using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Config
{
    /// <summary>
    /// 服务异常信息
    /// </summary>
   public class ErrServiceInfo
   {
      string servicekey;
      int errCode;

       /// <summary>
       /// 服务名称
       /// </summary>
      public string ServiceKey
      {
         get { return servicekey; }
         set { servicekey = value; }
      }

       /// <summary>
       /// 服务代码
       /// </summary>
      public int ErrCode
      {
         get { return errCode; }
         set { errCode = value; }
      }
   }
}
