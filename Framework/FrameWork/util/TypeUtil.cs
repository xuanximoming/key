using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork
{
    /// <summary>
    /// 类型辅助类
    /// </summary>
   public class TypeUtil
   {
      /// <summary>
      /// 验证类型继承关系
      /// </summary>
      /// <param name="type">子类型</param>
      /// <param name="baseType">父类型</param>
      /// <returns></returns>
      public static bool ConfirmInherit(Type type, Type baseType)
      {         
         if (type == null)
            throw new ArgumentNullException("输入子类型为空");
         if (baseType == null)
            throw new ArgumentNullException("输入父类型为空");

         //父类为接口，只需要获取所有验证
         if(baseType.IsInterface)
         {
            Type[] types = type.GetInterfaces();
            foreach (Type t in types)
            {
               if (t.FullName == baseType.FullName)
                  return true;
            }
            return false;
         }

         //不是接口则一层层的搜索
         Type btype = type;
         while (btype!=null)
         {
            btype = btype.BaseType;
            Console.Write(btype.ToString());
            if (baseType==btype)
               return true;
         }
         
         return false;
      }         
   }
}
