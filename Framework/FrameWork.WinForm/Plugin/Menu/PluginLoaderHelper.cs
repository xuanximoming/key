using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Configuration;
using DrectSoft.FrameWork;

namespace DrectSoft.FrameWork.Plugin.Manager
{
    #region Class PlugInLoadHelper

    /// <summary>
    /// 加载插件Assembly
    /// </summary>
    public class PlugInLoadHelper
    {
        internal const string PlugInLoadConfigSectionName = "plugInLoadSettings";
        private static PlugInLoader _plugInLoader;
        private string _appStartPath;

        /// <summary>
        /// Ctor
        /// </summary>
        public PlugInLoadHelper(string appPath)
        {
            _appStartPath = appPath;
        }

        /// <summary>
        /// 实现读取自定义工具条配置信息的委托实现
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public PlugInLoadConfiguration ReadPlugInLoadConfiguration(XmlNode section)
        {
            if (section == null) return new PlugInLoadConfiguration();

            PlugInLoadConfiguration plugInConfig = new PlugInLoadConfiguration();
            foreach (XmlNode xmlnode in section.ChildNodes)
            {
                if (xmlnode.Name == "UseShadowCopy")
                {
                    if (xmlnode.Attributes["value"] != null)
                    {
                        plugInConfig.UseShadowCopy = (bool.TrueString == xmlnode.Attributes["value"].Value);
                    }
                }
                if (xmlnode.Name == "PlugInsPath")
                {
                    if (xmlnode.Attributes["value"] != null)
                    {
                        string[] paths = xmlnode.Attributes["value"].Value.Split(';');
                        string plgpaths = string.Empty;
                        foreach (string path in paths)
                        {
                            plgpaths += System.IO.Path.Combine(_appStartPath, path) + ";";
                        }
                        plugInConfig.PlugInsPath = plgpaths;
                    }
                }
                if (xmlnode.Name == "BizPlugInsPath")
                {
                    if (xmlnode.Attributes["value"] != null)
                    {
                        string[] paths = xmlnode.Attributes["value"].Value.Split(';');
                        string plgpaths = string.Empty;
                        foreach (string path in paths)
                        {
                            plgpaths += System.IO.Path.Combine(_appStartPath, path) + ";";
                        }
                        plugInConfig.BizPlugInsPath = plgpaths;
                    }
                }
                if (xmlnode.Name == "CachePath")
                {
                    if (xmlnode.Attributes["value"] != null)
                    {
                        plugInConfig.CachePath = System.IO.Path.Combine(_appStartPath, xmlnode.Attributes["value"].Value);
                    }
                }
            }
            return plugInConfig;
        }

        static void EnsurePlugInLoader(string appPath)
        {
            if (_plugInLoader == null)
            {
                PlugInLoadHelper plugInLoadHelper = new PlugInLoadHelper(appPath);
                DrectSoftConfigurationSectionHandler.SetConfigurationDelegate(plugInLoadHelper.ReadPlugInLoadConfiguration);
                PlugInLoadConfiguration plugInLoadConfig = (PlugInLoadConfiguration)ConfigurationManager.GetSection(PlugInLoadConfigSectionName);
                _plugInLoader = new PlugInLoader(appPath, plugInLoadConfig);
            }
        }

        public static Collection<PlugInConfiguration> LoadAllPlugIns(string appPath, string interfaceType, params string[] menufile)
        {
            EnsurePlugInLoader(appPath);

            if (menufile == null || menufile.Length == 0)
                return PlugInConfigurationReader.CreateAndReadPlugInConfiguration(
                    appPath + @"\file.menu", _plugInLoader, interfaceType, true);
            else
                return PlugInConfigurationReader.CreateAndReadPlugInConfiguration(
                    appPath + @"\" + menufile[0], _plugInLoader, interfaceType, false);
        }

        internal static Assembly LoadPlugIn(string appPath, string assemblyName)
        {
            EnsurePlugInLoader(appPath);
            return _plugInLoader.RemoteLoadAssembly(assemblyName);
        }

        internal static Assembly LoadPlugIn(string assemblyName)
        {
            return Assembly.Load(assemblyName);
        }

        public static void UnLoadPlugIn(string appPath)
        {
            EnsurePlugInLoader(appPath);
            _plugInLoader.UnloadAllAssembly();
        }
    }

    #endregion

}
