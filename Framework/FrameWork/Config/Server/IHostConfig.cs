using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace DrectSoft.FrameWork.Config
{
   /// <summary>
   /// 服务宿主配置
   /// </summary>
   [ServiceContract(ProtectionLevel = System.Net.Security.ProtectionLevel.None)]
   public interface IHostConfig
   {
       /// <summary>
       /// 获取服务宿主配置信息
       /// </summary>
       /// <returns></returns>
      [OperationContract]
      string GetConfigString();
   }
}
