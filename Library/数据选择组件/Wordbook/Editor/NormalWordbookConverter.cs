using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// 处理String与字典类之间的互换。在PropertyGrid中设置BaseWordbook类型的属性时需要
   /// </summary>
   public class NormalWordbookConverter : ExpandableObjectConverter
   {
      /// <summary>
      /// 返回此转换器是否可将该对象转换为指定的类型
      /// </summary>
      /// <param name="context"></param>
      /// <param name="destinationType"></param>
      /// <returns></returns>
      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
      {
         if (destinationType == typeof(InstanceDescriptor))
            return true;
         return base.CanConvertTo(context, destinationType);
      }

      /// <summary>
      /// 返回该转换器是否可以将一种类型的对象转换为此转换器的类型
      /// </summary>
      /// <param name="context"></param>
      /// <param name="sourceType"></param>
      /// <returns></returns>
      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
      {
         // 允许从String转成BaseWordbook类型
         if (sourceType == typeof(string))
         {
            return true;
         }
         return base.CanConvertFrom(context, sourceType);
      }

      /// <summary>
      /// 将给定值转换为此转换器的类型
      /// </summary>
      /// <param name="context"></param>
      /// <param name="culture"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
      {
         // 从String转成BaseWordbook类
         string bookName = (string)value;
         if (bookName != null)
         {
            // 创建BaseWordbook实例
            try
            {
               // 利用静态方法得到正确的字典类。无对应名称将返回null
               return WordbookStaticHandle.GetWordbook(bookName);
            }
            catch
            {
               throw;// new ArgumentException("参数不正确");
            }

         }
         return base.ConvertFrom(context, culture, value);
      }

      /// <summary>
      /// 将给定值对象转换为指定的类型
      /// </summary>
      /// <param name="context"></param>
      /// <param name="culture"></param>
      /// <param name="value"></param>
      /// <param name="destinationType"></param>
      /// <returns></returns>
      public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
      {
         // 将BaseWordbook类转成String或InstanceDescriptor
         BaseWordbook wordbook = value as BaseWordbook;
         if (wordbook != null)
         {
            // 转成String
            if ((destinationType == typeof(string)))
            {
               return (wordbook).ToString();
            }
            // 转成InstanceDescriptor（从设计界面保存到Designer文件时需要）
            if ((destinationType == typeof(InstanceDescriptor)))
            {
               object[] properties = new object[5];
               Type[] types = new Type[5];
               // FilterFields
               types[0] = typeof(string);
               properties[0] = wordbook.MatchFieldComb;
               // SelectedStyleIndex
               types[1] = typeof(int);
               properties[1] = wordbook.SelectedStyleIndex;
               // ParameterValues
               types[2] = typeof(string);
               properties[2] = wordbook.ParameterValueComb;
               // ExtraCondition
               types[3] = typeof(string);
               properties[3] = wordbook.ExtraCondition;
               // CacheTime
               types[4] = typeof(int);
               properties[4] = wordbook.CacheTime;

               //object[] properties = new object[1];
               //Type[] types = new Type[1];
               ////// FilterFields
               //types[0] = typeof(Collection<string>);
               //Collection<string> filters = new Collection<string>();
               //filters.Add("aa");
               //properties[0] = new Collection<string>(filters);

               //object[] properties = new object[3];
               //Type[] types = new Type[3];
               ////// FilterFields
               //types[0] = typeof(Collection<string>);
               ////Collection<string> filters = new Collection<string>(wordbook.FilterFields);
               //properties[0] = new Collection<string>();
               //((Collection<string>)properties[0]).Add("czydm");
               //((Collection<string>)properties[0]).Add("wb");
               //((Collection<string>)properties[0]).Add("py");

               //// SelectedStyleIndex
               //types[1] = typeof(int);
               //properties[1] = wordbook.SelectedStyleIndex;
               //// ExtraCondition
               //types[2] = typeof(string);
               //properties[2] = wordbook.ExtraCondition;
               // 得到构造函数信息
               ConstructorInfo ci = wordbook.GetType().GetConstructor(types);
               return new InstanceDescriptor(ci, properties);
            }
         }
         // 基类的ConvertTo()必须调用
         return base.ConvertTo(context, culture, value, destinationType);
      }

      /// <summary>
      /// 在给定 Object 的一组属性值的情况下重新创建该对象
      /// </summary>
      /// <param name="context"></param>
      /// <param name="propertyValues"></param>
      /// <returns></returns>
      public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
      {
         if ( propertyValues == null )
            throw new ArgumentNullException(MessageStringManager.GetString("NullParameter"));
         BaseWordbook wordbook = WordbookStaticHandle.GetWordbook((string)propertyValues["WordbookName"]);
         wordbook.ExtraCondition = propertyValues["ExtraCondition"].ToString();
         wordbook.SelectedStyleIndex = (int)propertyValues["SelectedStyleIndex"];
         wordbook.MatchFieldComb = propertyValues["MatchFieldComb"].ToString();
         wordbook.ParameterValueComb = propertyValues["ParameterValue"].ToString();
         wordbook.CacheTime = (int)propertyValues["SelectedStyleIndex"];
         return wordbook;
      }

      /// <summary>
      /// 返回更改此对象的值是否需要调用 CreateInstance 来创建新值。
      /// </summary>
      /// <param name="context"></param>
      /// <returns></returns>
      public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
      {
         return true;
      }
   }
}
