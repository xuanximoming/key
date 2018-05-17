using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Collections;
using System.Drawing;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 处理String和TypeXmlFont的转换
   /// </summary>
   public class TypeXmlFontConvert : ExpandableObjectConverter
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
         // 允许从String
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
         // 从String转换
         string fontValue = (string)value;
         if (fontValue != null)
         {
            try
            {
               TypeXmlFont newFont = new TypeXmlFont();
               string[] fontParts = fontValue.Split(new char[] { ',' });
               newFont.FontFamily = fontParts[0].Trim();
               newFont.Size = (float)Convert.ToDouble(fontParts[1].Trim());
               newFont.Style = (FontStyle)Enum.Parse(typeof(FontStyle), fontParts[2].Trim());
               return newFont;
            }
            catch
            {
               throw; 
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
         TypeXmlFont font = value as TypeXmlFont;
         if (font != null)
         {
            // 转成String
            if ((destinationType == typeof(string)))
            {
               return font.ToString();
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
         if (propertyValues == null)
            throw new ArgumentNullException("NullParameter");

         TypeXmlFont font = new TypeXmlFont();
         font.FontFamily = propertyValues["FontFamily"].ToString();
         font.Size = (float)propertyValues["Size"];
         font.Style = (FontStyle)Enum.Parse(typeof(FontStyle), propertyValues["Style"].ToString());
         return font;
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

   /// <summary>
   /// 处理String和TypeColorPair的转换
   /// </summary>
   public class TypeColorPairConvert : ExpandableObjectConverter
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
         // 允许从String
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
         // 从String转换
         string colorValue = (string)value;
         if (colorValue != null)
         {
            try
            {
               TypeColorPair colorPair = new TypeColorPair();
               string[] colorParts = colorValue.Split(new char[] { '|' });
               colorPair.XmlForeColor = colorParts[0].Trim();
               colorPair.XmlBackColor = colorParts[1].Trim();
               return colorPair;
            }
            catch
            {
               throw;
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
         TypeColorPair color = value as TypeColorPair;
         if (color != null)
         {
            // 转成String
            if ((destinationType == typeof(string)))
            {
               return color.ToString();
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
         if (propertyValues == null)
            throw new ArgumentNullException("NullParameter");

         TypeColorPair colorPair = new TypeColorPair();
         colorPair.ForeColor = (Color)propertyValues["ForeColor"];
         colorPair.BackColor = (Color)propertyValues["BackColor"];
         return colorPair;
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
