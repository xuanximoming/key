using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DrectSoft.FrameWork.BizBus.Service;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace DrectSoft.FrameWork.Config
{
   /// <summary>
   /// 服务包信息类，一般一个服务包都包括一个接口Assembly，一个实现Assembly和一个服务信息列表
   /// </summary>
   [DataContract]
   public class ServicePackageInfo
   {
      string name;
      String infAssemblyName;      //接口assembly
      String impleAssemblyName;    //实现assembly
      List<ServiceInfo> services;   

      /// <summary>
      /// 构造函数
      /// </summary>
      public ServicePackageInfo()
      {         
         services = new List<ServiceInfo>();
      }

      /// <summary>
      /// 设置或获取服务包名称
      /// </summary>
       [DisplayName("服务包名称")]
       public string Name
      {
         get { return name; }
         set { name = value; }
      }

       /// <summary>
      /// 接口组件名称
       /// </summary>
       [DisplayName("接口组件名称"), ReadOnly(true)]
       public String InfAssemblyName
      {
         get { return infAssemblyName; }
         set { infAssemblyName = value; }
      }

       /// <summary>
      /// 实现组件名称
       /// </summary>
       [DisplayName("实现组件名称"), ReadOnly(true)]
       public String ImpleAssemblyName
      {
         get { return impleAssemblyName; }
         set { impleAssemblyName = value; }
      }

      /// <summary>
      /// 获取服务
      /// </summary>
      public List<ServiceInfo> Services
      {
         get
         {
            return services;
         }
      }
   }
}
