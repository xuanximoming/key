using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Security.Permissions;

namespace DrectSoft.FrameWork
{
   /// <summary>
   /// 插件装载器
   /// </summary>
   public class PlugInLoader
   {
      private Hashtable domains = new Hashtable();
      private Hashtable loaders = new Hashtable();
       private string _applicationPath;
       private PlugInLoadConfiguration _adConfig;

      /// <summary>
      /// Ctor
      /// </summary>
      /// <param name="applicationPath"></param>
       /// <param name="config"></param>
      public PlugInLoader(string applicationPath, PlugInLoadConfiguration config)
      {
         _applicationPath = applicationPath;
         _adConfig = config;
      }

      /// <summary>
      /// 启动插件
      /// </summary>
      /// <param name="assemblyName"></param>
      public Assembly RemoteLoadAssembly(string assemblyName)
      {
         string assemblyNameWithNoPath = System.IO.Path.GetFileNameWithoutExtension(assemblyName);
         RemoteLoader al = null;
         try
         {
            al = (RemoteLoader)loaders[assemblyNameWithNoPath];
         }
         catch (Exception e)
         {
            System.Diagnostics.Trace.WriteLine(e.ToString());
         }
         if (al == null)
         {
            AppDomainSetup setup = new AppDomainSetup();
            if (_adConfig.UseShadowCopy)
            {
               setup.ShadowCopyFiles = "true";
               setup.ShadowCopyDirectories = _adConfig.PlugInsPath;
               setup.CachePath = _adConfig.CachePath;
            }
            setup.ApplicationBase = _applicationPath;
            setup.PrivateBinPath = _adConfig.AllPath;
            AppDomain domain = AppDomain.CreateDomain(assemblyNameWithNoPath, null, setup);
            domains.Add(assemblyNameWithNoPath, domain);
            al = (RemoteLoader)domain.CreateInstanceAndUnwrap("DrectSoft.FrameWork", "DrectSoft.FrameWork.RemoteLoader");
         }
         if (al != null)
         {
            if (!loaders.ContainsKey(assemblyNameWithNoPath))
            {
               loaders.Add(assemblyNameWithNoPath, al);
            }
            return al.LoadAssembly(assemblyNameWithNoPath);
         }
         else
         {
            return null;
         }
      }

      /// <summary>
      /// 卸载assembly,通过卸载所在的AppDomain
      /// </summary>
      /// <param name="assemblyName"></param>
      public void UnloadAssembly(string assemblyName)
      {
         string assemblyNameWithNoPath = System.IO.Path.GetFileNameWithoutExtension(assemblyName);
         if (domains.ContainsKey(assemblyNameWithNoPath))
         {
            AppDomain.Unload(domains[assemblyNameWithNoPath] as AppDomain);
            domains.Remove(assemblyNameWithNoPath);
            loaders.Remove(assemblyNameWithNoPath);
         }
      }

      /// <summary>
      /// 卸载所有的Assembly
      /// </summary>
      public void UnloadAllAssembly()
      {
         foreach (string key in domains.Keys)
         {
            AppDomain.Unload(domains[key] as AppDomain);
            //domains.Remove(key);
            //loaders.Remove(key);
         }
         domains.Clear();
         loaders.Clear();
      }

      /// <summary>
      /// 插件路径,可以多个,用；分隔
      /// </summary>
      public string PlugInsPath
      {
         get { return _adConfig.PlugInsPath; }
         set { _adConfig.PlugInsPath = value; }
      }
   }

   #region Class RemoteLoader
   /// <summary>
   /// 启动插件
   /// </summary>
   public class RemoteLoader : MarshalByRefObject
   {
      /// <summary>
      /// 启动插件
      /// </summary>
      /// <param name="assemblyName"></param>
      public Assembly LoadAssembly(string assemblyName)
      {
         Assembly asmb = AppDomain.CurrentDomain.Load(assemblyName);

         return asmb;
      }

      /// <summary>
      /// override InitializeLifetimeService
      /// </summary>
      /// <returns></returns>
      [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
      public override object InitializeLifetimeService()
      {
         return null;
      }
   }
   #endregion

    #region PlugInLoadConfiguration

    /// <summary>
   /// 加载插件Assembly的设置参数类
    /// </summary>
    public class PlugInLoadConfiguration
    {
        private bool _useShadowCopy;
        private string _plugInsPath;
        private string _cachePath;
        private string _bizPlugInsPath;

        /// <summary>
        /// 使用ShadowCopy技术
        /// </summary>
        public bool UseShadowCopy
        {
            get { return _useShadowCopy; }
            set { _useShadowCopy = value; }
        }

        /// <summary>
        /// 插件所在的路径
        /// </summary>
        public string PlugInsPath
        {
            get { return _plugInsPath; }
            set { _plugInsPath = value; }
        }

        /// <summary>
        /// ShadowCopy时的缓存目录
        /// </summary>
        public string CachePath
        {
            get { return _cachePath; }
            set { _cachePath = value; }
        }

        /// <summary>
        /// 服务插件所在的路径(单独定义,需要将这个路径加入PrivateBinPath)
        /// </summary>
        public string BizPlugInsPath
        {
            get { return _bizPlugInsPath; }
            set { _bizPlugInsPath = value; }
        }

        /// <summary>
        /// 所有插件的路径，";"分隔
        /// </summary>
        public string AllPath
        {
            get
            {
                string[] plgs = PlugInsPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string[] bizs = BizPlugInsPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string allplgpaths = string.Empty;
                foreach (string plg in plgs) allplgpaths += plg + ";";
                foreach (string biz in bizs) allplgpaths += biz + ";";
                return allplgpaths;
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public PlugInLoadConfiguration()
        {
        }

        /// <summary>
        /// Ctor2
        /// </summary>
        /// <param name="plugInsPath"></param>
        /// <param name="bizPlugInsPath"></param>
        /// <param name="useShadowCopy"></param>
        /// <param name="cachePath"></param>
        public PlugInLoadConfiguration(string plugInsPath, string bizPlugInsPath, bool useShadowCopy, string cachePath)
        {
            _plugInsPath = plugInsPath;
            _bizPlugInsPath = bizPlugInsPath;
            _useShadowCopy = useShadowCopy;
            _cachePath = cachePath;
        }
    }

    #endregion
}
