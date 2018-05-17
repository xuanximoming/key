using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.FrameWork.BizBus.Service;

namespace DrectSoft.FrameWork.Config.Check
{
    /// <summary>
    /// 配置校验器
    /// </summary>
   public class ConfigValidator
   {

       /// <summary>
       /// 校验服务
       /// </summary>
       /// <param name="desc"></param>
      public static void ValidateServiceDesc(ServiceDesc desc)
      {
         if (desc == null)
            throw new ArgumentNullException("传入服务描述为空");

         if (desc.ServiceType == null)
            throw new ArgumentNullException("serviceType", "服务类型为空");
         if (desc.ImpleType == null)
            throw new ArgumentNullException("impleType", "实现类型为空");

         if (!TypeUtil.ConfirmInherit(desc.ServiceType, typeof(IService)))
            throw new ServiceInvalidException(desc.ServiceType.ToString(), "未实现服务接口");

         if (!TypeUtil.ConfirmInherit(desc.ImpleType, desc.ServiceType))
            throw new ServiceInvalidException(desc.ServiceType.ToString(), "实现类型未实现服务接口");
      }
   }
}
