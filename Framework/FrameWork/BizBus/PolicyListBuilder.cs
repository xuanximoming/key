using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using DrectSoft.FrameWork.BizBus.Service;

namespace DrectSoft.FrameWork.BizBus
{
   /// <summary>
   /// 策略构建器
   /// </summary>
   internal class PolicyListBuilder
   {
      public PolicyListBuilder()
      { }

      /// <summary>
      /// 构建策略
      /// </summary>
      /// <param name="servicedesc">服务描述</param>
      /// <param name="issave">是否保存服务</param>
      /// <param name="parameters">服务传入参数</param>
      /// <returns></returns>
      public PolicyList[] BuildPolicy(ServiceDesc servicedesc, bool issave, object[] parameters)
      {
         PolicyList[] policies = new PolicyList[1];
         PolicyList policylist = new PolicyList();
         policies[0] = policylist;

         //构建影射策略
         TypeMappingPolicy typepolicy = new TypeMappingPolicy(servicedesc.ImpleType, servicedesc.Key);
         policylist.Set<ITypeMappingPolicy>(typepolicy, servicedesc.ServiceType, servicedesc.Key);

         //构建单一构建策略（只有创建了单一创建策略才能够Locate）
         if (issave)
         {
            SingletonPolicy singletonpolicy = new SingletonPolicy(true);
            policylist.Set<ISingletonPolicy>(singletonpolicy, servicedesc.ImpleType, servicedesc.Key);
         }

         //建立创建策略，可以添加参数
         if (parameters != null && parameters.Length != 0)
         {
            ConstructorPolicy constuctorpolicy = new ConstructorPolicy();
            for (int i = 0; i < parameters.Length; i++)
            {
               constuctorpolicy.AddParameter(new ValueParameter(parameters[i].GetType(), parameters[i]));
            }
            policylist.Set<ICreationPolicy>(constuctorpolicy, servicedesc.ImpleType, servicedesc.Key);
         }
         return policies;
      }
   }
}
