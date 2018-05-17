using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core
{
   /// <summary>
   /// 定义一个本地化当前工程内枚举类型的转换器
   /// </summary>
   class LocalizedEnumConverter : ResourceEnumConverter
   {
      /// <summary>
      /// Create a new instance of the converter using translations from the given resource manager
      /// </summary>
      /// <param name="type"></param>
      public LocalizedEnumConverter(Type type)
           : base(type, DrectSoft.Core.Properties.Resources.ResourceManager)
      {
      }
   }
}
