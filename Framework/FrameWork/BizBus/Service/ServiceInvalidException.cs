using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace DrectSoft.FrameWork.BizBus.Service
{
   /// <summary>
   /// 服务非法Exception
   /// </summary>
   [Serializable]
   public class ServiceInvalidException:ApplicationException
   {
      string m_servicename;

       /// <summary>
      /// 服务非法Exception
       /// </summary>
       /// <param name="info"></param>
       /// <param name="context"></param>
       public ServiceInvalidException(SerializationInfo info, StreamingContext context)
           : base(info, context)
       {
       }

      /// <summary>
      /// 服务异常
      /// </summary>
      /// <param name="message"></param>
      public ServiceInvalidException(string message)
         : base(message)
      {
      }

      /// <summary>
      /// 初始化服务非法Exception
      /// </summary>
      /// <param name="message">错误信息</param>
      /// <param name="servicename">服务名称</param>
      public ServiceInvalidException(string servicename, string message)
         : base(message)
      {
         this.m_servicename = servicename;
      }

      /// <summary>
      /// 获取出错信息
      /// </summary>
      public override string Message
      {
         get
         {
            string message = base.Message;
            if (!string.IsNullOrEmpty(m_servicename))
            {               
               return (message + Environment.NewLine +  this.m_servicename );
            }
            return message;

         }
      }

      /// <summary>
      /// 获取服务名称
      /// </summary>
      public virtual string ServiceName
      {
         get { return m_servicename; }
      }
   }
}
