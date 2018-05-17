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
   /// 处理String与SQL字典类之间的互换。在PropertyGrid中设置SqlWordbook类型的属性时需要
   /// </summary>
   public class SqlWordbookConverter : ExpandableObjectConverter
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
         string propertyList = (string)value;

         if (propertyList != null)
         {
            if (propertyList.Length == 0)
               return null;
            // 创建BaseWordbook实例
            try
            {
               string[] properties = propertyList.Split(';');

               return new SqlWordbook(properties[0].Trim()
                  , properties[1].Trim()
                  , properties[2].Trim()
                  , properties[3].Trim()
                  , properties[5].Trim()
                  , properties[4].Trim());
            }
            catch
            {
               throw new ArgumentException();
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
         // 将SqlWordbook类转成String或InstanceDescriptor
         SqlWordbook sqlBook = value as SqlWordbook;
         if (sqlBook != null)
         {
            // 转成String
            if ((destinationType == typeof(string)))
            {
               return sqlBook.ToString();
            }
            // 转成InstanceDescriptor（从设计界面保存到Designer文件时需要）
            if ((destinationType == typeof(InstanceDescriptor)))
            {
               object[] properties = new object[6];
               Type[] types = new Type[6];

               types[0] = typeof(string);
               properties[0] = sqlBook.WordbookName;

               types[1] = typeof(string);
               properties[1] = sqlBook.QuerySentence;

               types[2] = typeof(string);
               properties[2] = sqlBook.CodeField;

               types[3] = typeof(string);
               properties[3] = sqlBook.NameField;

               types[4] = typeof(string);
               properties[4] = sqlBook.DefaultGridStyle.ToString();

               types[5] = typeof(string);
               properties[5] = sqlBook.MatchFieldComb;

               // 得到构造函数信息
               ConstructorInfo ci = sqlBook.GetType().GetConstructor(types);
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
         return new SqlWordbook(propertyValues["WordbookName"].ToString()
                                , propertyValues["QuerySentence"].ToString()
                                , propertyValues["DefaultCodeField"].ToString()
                                , propertyValues["DefaultDisplayField"].ToString()
                                , propertyValues["DefaultGridStyle"].ToString()
                                , propertyValues["MatchFieldComb"].ToString());
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
