using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 指示方法是否是默认的初始化数据的方法。不可继承
   /// </summary>
   [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited=false)]
   public sealed class SpecialMethodAttribute : Attribute
   {
      /// <summary>
      /// 方法特殊属性
      /// </summary>
      public MethodSpecialKind Flag
      {
         get { return _flag; }
      }
      private MethodSpecialKind _flag;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="flag"></param>
      public SpecialMethodAttribute(MethodSpecialKind flag)
      {
         _flag = flag;
      }
   }

   /// <summary>
   /// 标记类方法和构造函数的特殊属性。在用反射方式设置、获取类属性的值时要用。
   /// </summary>
   public enum MethodSpecialKind
   { 
      /// <summary>
      /// 默认的构造函数
      /// </summary>
      DefaultCtor,
      /// <summary>
      /// 状态类的初始化方法
      /// </summary>
      StateInitValueMethod,
      /// <summary>
      /// 状态类取值的方法
      /// </summary>
      StateGetValueMethod
   }
}
