using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace DrectSoft.FrameWork.BizBus.Service
{
   /// <summary>
   /// 服务接口,每一个服务由服务接口,服务key或者服务接口,服务实现类型两部分组成
   /// 每一个服务都有三个关键描述,1.服务接口;2.服务key;3.服务实现类型
   /// 服务key或者服务实现类型只要有一个就好,当服务key不写入时,用服务实现类型的fullname作为key
   /// </summary>
    [CLSCompliantAttribute(false)]
   public interface IService
   {
   }
}
