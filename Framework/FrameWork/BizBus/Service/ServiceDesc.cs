using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.BizBus.Service
{
   /// <summary>
   /// 服务描述
   /// </summary>
   public class ServiceDesc
   {
      Type serviceType;    //服务类型
      string key;          //服务关键字
      Type impleType;      //实现类型
      bool isDefault;      //是否为缺省服务


      /// <summary>
      /// 构造函数
      /// </summary>
      /// <param name="servicetype">服务类型</param>
      /// <param name="key">服务关键字</param>
      /// <param name="impletype">服务实现类型</param>
       /// <param name="isdefault"></param>
      public ServiceDesc(Type servicetype, string key, Type impletype,bool isdefault)
      {
         this.serviceType = servicetype;
         this.key = key;
         this.impleType = impletype;
         this.isDefault = isdefault;
      }

      /// <summary>
      /// 设置或获取服务类型
      /// </summary>
      public Type ServiceType
      {
         get { return serviceType; }
         set { serviceType = value; }
      }

      /// <summary>
      /// 设置或获取服务关键字
      /// </summary>
      public string Key
      {
         get { return key; }
         set { key = value; }
      }

      /// <summary>
      /// 设置或获取服务实现类型
      /// </summary>
      public Type ImpleType
      {
         get { return impleType; }
         set { impleType = value; }
      }
   }
}
