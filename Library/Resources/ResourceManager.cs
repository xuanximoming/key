using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.IO;
using System.Globalization;

namespace DrectSoft.Resources
{
   /// <summary>
   /// 图标类型枚举
   /// </summary>
   public enum IconType
   {
      /// <summary>
      /// 正常状态
      /// </summary>
      Normal,
      /// <summary>
      /// 禁用状态
      /// </summary>
      Disable,
      /// <summary>
      /// 加亮
      /// </summary>
      Highlight
   }

   /// <summary>
   /// 提供访问资源的静态方法
   /// </summary>
   public static class ResourceManager
   {
      private static Assembly Resources
      {
         get
         {
            if (_resources == null)
               _resources = Assembly.GetExecutingAssembly();//.GetAssembly(typeof(DrectSoftResourceManager));

            return _resources;
         }
      }
      private static Assembly _resources;

      private static string Namespace
      {
         get
         {
            if (String.IsNullOrEmpty(_namespace))
               _namespace = typeof(ResourceManager).Namespace;
            return _namespace;
         }
      }
      private static string _namespace;

      #region const
      private const string ButtonIconFileType = "png";

      private const string SmallSize = "16";
      private const string MiddleSize = "24";

      private const string FlagDisable = "d";
      private const string FlagHighlight = "h";

      private const string FormatSmallNormal = "{0}_" + SmallSize;
      private const string FormatSmallDisable = "{0}_" + SmallSize + "_" + FlagDisable;
      private const string FormatSmallHigh = "{0}_" + SmallSize + "_" + FlagHighlight;

      private const string FormatMiddleNormal = "{0}_" + MiddleSize;
      private const string FormatMiddleDisable = "{0}_" + MiddleSize + "_" + FlagDisable;
      private const string FormatMiddleHigh = "{0}_" + MiddleSize + "_" + FlagHighlight;

      #endregion

      #region properties
      /// <summary>
      /// 默认的公司Logo
      /// </summary>
      public static Icon DrectSoftLogo
      {
         get
         {
            Stream mapStream = GetStreamFromResources("Icon", ResourceNames.IconDrectSoftLogoLarge, "ico");

            return new Icon(mapStream);
         }
      }

      /// <summary>
      ///获取指定系统logo
      /// </summary>
      /// <param name="shortKey"></param>
      /// <returns></returns>
      public static Icon GetDrectSoftLogo(string shortKey)
      {
         Stream mapStream = GetStreamFromResources("Icon", ResourceNames.IconDrectSoftLogoLarge+shortKey, "ico");
         return new Icon(mapStream);
      }





      #endregion

      #region private methods
      private static string FormatResourceName(string catalog, string resourceName, string fileType)
      {
         // 格式化资源名称：Namespace + 分类 + 资源名称 + 文件类型 
         if (String.IsNullOrEmpty(fileType))
            return String.Format("{0}.{1}.{2}", Namespace, catalog, resourceName);
         else
            return String.Format("{0}.{1}.{2}.{3}", Namespace, catalog, resourceName, fileType);
      }

      private static Image GetNormalIconFromResources(string iconName)
      {
         Stream mapStream = GetStreamFromResources("Icon", iconName, "ico");

         return Image.FromStream(mapStream);
      }

      private static Image GetButtonIconFromResources(string iconName)
      {
         Stream mapStream = GetStreamFromResources("Button", iconName, ButtonIconFileType);

         return Image.FromStream(mapStream);
      }

      private static Image GetImageFromResources(string imageName)
      {
         Stream mapStream = GetStreamFromResources("Images", imageName, String.Empty);

         return Image.FromStream(mapStream);
      }

      private static Stream GetStreamFromResources(string catalog, string resourceName, string fileType)
      {
         try
         {
            return Resources.GetManifestResourceStream(FormatResourceName(catalog, resourceName, fileType));
         }
         catch (FileNotFoundException)
         {
            throw new ArgumentException("指定的资源不存在", resourceName);
         }
         catch
         {
            throw;
         }
      }

      private static void CheckName(string imageName)
      {
         if (String.IsNullOrEmpty(imageName))
            throw new ArgumentNullException("资源名称为空");
      }
      #endregion

      #region public methods
      /// <summary>
      /// 获取一般性的图片，bmp、gif等
      /// </summary>
      /// <param name="imageName">图片名称(包含后缀)</param>
      /// <returns></returns>
      public static Image GetImage(string imageName)
      {
         CheckName(imageName);
         return GetImageFromResources(imageName);
      }

      /// <summary>
      /// 获取普通的图标（以ico结尾的）
      /// </summary>
      /// <param name="iconName">图标名称（不包含后缀）</param>
      /// <returns></returns>
      public static Image GetNormalIcon(string iconName)
      {
         CheckName(iconName);
         return GetNormalIconFromResources(iconName);
      }

      /// <summary>
      /// 获取按钮小尺寸图标（16×16）
      /// </summary>
      /// <param name="iconName">图标名称，不需要指明尺寸、类型和后缀</param>
      /// <param name="iconType">图标类型</param>
      /// <returns></returns>
      public static Image GetSmallIcon(string iconName, IconType iconType)
      {
         CheckName(iconName);
         switch (iconType)
         {
            case IconType.Normal:
               return GetButtonIconFromResources(String.Format(CultureInfo.CurrentCulture
                  , FormatSmallNormal, iconName));
            case IconType.Disable:
               return GetButtonIconFromResources(String.Format(CultureInfo.CurrentCulture
                  , FormatSmallDisable, iconName));
            default:
               return GetButtonIconFromResources(String.Format(CultureInfo.CurrentCulture
                  , FormatSmallHigh, iconName));
         }
      }

      /// <summary>
      /// 获取按钮中等尺寸图标（24×24）
      /// </summary>
      /// <param name="iconName">图标名称，不需要指明尺寸、类型和后缀</param>
      /// <param name="iconType">图标类型</param>
      /// <returns></returns>
      public static Image GetMiddleIcon(string iconName, IconType iconType)
      {
         CheckName(iconName);
         switch (iconType)
         {
            case IconType.Normal:
               return GetButtonIconFromResources(String.Format(CultureInfo.CurrentCulture
                  , FormatMiddleNormal, iconName));
            case IconType.Disable:
               return GetButtonIconFromResources(String.Format(CultureInfo.CurrentCulture
                  , FormatMiddleDisable, iconName));
            default:
               return GetButtonIconFromResources(String.Format(CultureInfo.CurrentCulture
                  , FormatMiddleHigh, iconName));
         }
      }

      /// <summary>
      /// 获得属性对应的资源名称
      /// </summary>
      /// <param name="fieldName"></param>
      /// <returns></returns>
      public static String GetSourceName(string fieldName)
      {
         FieldInfo field = typeof(ResourceNames).GetField(fieldName, BindingFlags.Static | BindingFlags.Public);
         if (field != null)
            return field.GetValue(null).ToString();
         else
            return String.Empty;
      }
      #endregion
   }
}
