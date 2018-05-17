using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DrectSoft.FrameWork.Config
{
   /// <summary>
   /// 服务信息类
   /// </summary>
   public class ServiceInfo
   {
      String serviceTypeName = "";    //服务类型名称
      String key = "";                //服务关键字
      String impleTypeName = "";      //实现类型名称
      bool isdefault=false;

      /// <summary>
      /// 服务类型名称
      /// </summary>
       [DisplayName("服务类型名称"), ReadOnly(true)]
      public String ServiceTypeName
      {
         get { return serviceTypeName; }
         set { serviceTypeName = value; }
      }

      /// <summary>
      /// 服务关键字
      /// </summary>
       [DisplayName("服务关键字")]
      public String Key
      {
         get { return key; }
         set { key = value; }
      }

      /// <summary>
      /// 服务实现名称
      /// </summary>
       [DisplayName("服务实现名称"), ReadOnly(true)]
       public String ImpleTypeName
      {
         get { return impleTypeName; }
         set { impleTypeName = value; }
      }

      /// <summary>
      /// 设置或获取是否为缺省服务
      /// </summary>
       [DisplayName("是否为缺省服务")]
       public bool IsDefault
      {
         get { return isdefault; }
         set { isdefault = value; }
      }

      /// <summary>
      /// 判断相等
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public override bool Equals(object obj)
      {
         if (obj == null || (!(obj is ServiceInfo)))
            return false;

         ServiceInfo info = obj as ServiceInfo;
         if ((this.serviceTypeName != info.serviceTypeName) ||
            (this.impleTypeName != info.impleTypeName) ||
            (this.key != info.key))
            return false;

         if (isdefault != info.isdefault)
            return false;

         return true;
      }

      /// <summary>
      /// 获取哈希码
      /// </summary>
      /// <returns></returns>
      public override int GetHashCode()
      {
         return this.serviceTypeName.GetHashCode()
            + this.impleTypeName.GetHashCode()
            + this.key.GetHashCode();
      }
   }
}