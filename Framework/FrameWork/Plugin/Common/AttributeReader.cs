using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace DrectSoft.FrameWork.Plugin
{
   /// <summary>
   /// 读取特性
   /// </summary>
   public class AttributesReader
   {
      private Assembly _assembly;

      /// <summary>
      /// 构造函数
      /// </summary>
      /// <param name="assemblyName">程序集名称</param>
      public AttributesReader(string assemblyName)
      {
         try
         {
            _assembly = Assembly.LoadFrom(assemblyName);
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// 在指定应用域加载的构造函数
      /// </summary>
      /// <param name="domain">应用程序域</param>
      /// <param name="assemblyName">程序集名称</param>
      public AttributesReader(AppDomain domain, string assemblyName)
      {
         try
         {
            string assemblyNameWithExt = System.IO.Path.GetFileNameWithoutExtension(assemblyName);
            if (domain != null)
               _assembly = domain.Load(assemblyNameWithExt);
            else
               _assembly = AppDomain.CurrentDomain.Load(assemblyNameWithExt);
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// 构造函数
      /// </summary>
      /// <param name="assembly">程序集</param>
      public AttributesReader(Assembly assembly)
      {
         _assembly = assembly;
      }

      /// <summary>
      /// 取得插件的特性信息
      /// </summary>
      /// <returns>插件的属性数组</returns>
      public PluginAttribute[] GetPlugInMenuInfoAttribute()
      {
         if (_assembly != null)
         {
            try
            {
               return (PluginAttribute[])_assembly.GetCustomAttributes(typeof(PluginAttribute), true);
            }
            catch (Exception e)
            {
               throw e;
            }
         }
         else
         {
            return null;
         }
      }
   }
}
