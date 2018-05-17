using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using DrectSoft.FrameWork.Plugin.Manager;
using System.Windows.Forms;
using DrectSoft.Core;

namespace DrectSoft.FrameWork.Plugin {
    /// <summary>
    /// 插件Plugin的设置内容读取操作
    /// </summary>
    public class PlugInConfigurationReader {
        private string _configFileName;
        private string _assemblySetPath;
        private string _assemblyInterfaceType;
        private Collection<PlugInConfiguration> _plugInConfiguration;
        private PlugInLoader _plugInLoader;
        private static DrectSoftLog _log = new DrectSoftLog();

        /// <summary>
        /// Ctor2 ,指定Loader
        /// </summary>
        /// <param name="configFileName"></param>
        /// <param name="plugInLoader"></param>
        /// <param name="assemblyInterfaceType"></param>
        public PlugInConfigurationReader(string configFileName, PlugInLoader plugInLoader, string assemblyInterfaceType) {
            if (plugInLoader == null) throw new ArgumentNullException("plugInLoader", "指定的加载域为空");

            _plugInLoader = plugInLoader;
            _configFileName = configFileName;
            _assemblyInterfaceType = assemblyInterfaceType;
            _assemblySetPath = plugInLoader.PlugInsPath;
            _plugInConfiguration = new Collection<PlugInConfiguration>();
        }

        private static XmlNode FindAndCreateXmlNode(XmlDocument plugInDoc, XmlNode parentNode, string xpath
            , string nodeName, Hashtable nodeProperty) {
            XmlNode node = parentNode.SelectSingleNode(xpath);
            if (node == null) {
                XmlElement newnode = plugInDoc.CreateElement(nodeName);
                foreach (string propertyName in nodeProperty.Keys) {
                    newnode.SetAttribute(propertyName, nodeProperty[propertyName].ToString());
                }
                parentNode.AppendChild(newnode);
                return newnode;
            }
            else {
                return node;
            }
        }

        private bool CreatePlugInConfiguration() {
            if (!File.Exists(_configFileName)) {
                _log.Warn("文件[" + _configFileName + "]不存在！");

                //不存在,需要自动创建XML文档,所有信息由自定制的特性得到,
                XmlTextWriter emptyXmlDoc = new XmlTextWriter(_configFileName, Encoding.Default);
                emptyXmlDoc.WriteStartDocument();
                emptyXmlDoc.WriteStartElement("config");
                emptyXmlDoc.WriteEndElement();
                emptyXmlDoc.Flush();
                emptyXmlDoc.Close();
            }

            //创建菜单的XML文档
            XmlDocument plugInDoc = new XmlDocument();
            try {
                plugInDoc.Load(_configFileName);
            }
            catch {
                _log.Error("载入菜单文档时出错！\n");
                throw;
            }

            if (_plugInLoader == null) throw new ArgumentNullException("plugInLoader", "指定的加载域为空");

            //遍历指定路径的满足条件的插件
            string[] paths = _assemblySetPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> plugInFiles = new List<string>();
            foreach (string path in paths) {
                string[] plgs = Directory.GetFiles(path, "*.dll");
                plugInFiles.AddRange(plgs);
            }
            foreach (string pluginfile in plugInFiles) {
                //创建读取插件模块的自制特性
                try {
                    //Assembly assembly = Assembly.LoadFrom(plugInFiles[i]);
                    Assembly assembly = _plugInLoader.RemoteLoadAssembly(pluginfile);

                    if (assembly == null) continue;
                    AttributesReader attrReader = new AttributesReader(assembly);

                    PluginAttribute[] attrPlugInInfo = attrReader.GetPlugInMenuInfoAttribute();
                    for (int menuIndex = 0; menuIndex < attrPlugInInfo.Length; menuIndex++) {
                        if (!(attrPlugInInfo[menuIndex] is ClientPluginAttribute))
                            continue;
                        ClientPluginAttribute attrMenuPluginInfo = attrPlugInInfo[menuIndex] as ClientPluginAttribute;

                        //检查所给定的启动类是否实现了_assemblyIntfType接口
                        Type startupInterfaceType = attrMenuPluginInfo.StartupClassType.GetInterface(_assemblyInterfaceType);
                        if (startupInterfaceType == null) {
                            _log.Warn("所给定的启动类没有实现[" + _assemblyInterfaceType + "]接口！");
                            break;
                        }

                        XmlNode plugInNode = plugInDoc.DocumentElement.SelectSingleNode(
                                    "//plugin[@library='" + Path.GetFileName(pluginfile) + "'"
                                    + " and @class='" + attrMenuPluginInfo.StartupClassType.FullName + "']");
                        if (plugInNode != null) {
                            break;
                        }

                        Hashtable ht = new Hashtable();
                        ht.Add("caption", attrMenuPluginInfo.MenuNameSubsystem);
                        XmlNode subSystemRootNode = FindAndCreateXmlNode(plugInDoc, plugInDoc.DocumentElement
                            , "system[@caption='" + attrMenuPluginInfo.MenuNameSubsystem + "']"
                            , "system", ht);

                        ht.Clear();
                        ht.Add("caption", attrMenuPluginInfo.MenuNameParent);
                        XmlNode subSystemPlugInsNode = FindAndCreateXmlNode(plugInDoc, subSystemRootNode
                            , "plugins[@caption='" + attrMenuPluginInfo.MenuNameParent + "']"
                            , "plugins", ht);

                        ht.Clear();
                        ht.Add("caption", attrMenuPluginInfo.MenuNameAssembly);
                        ht.Add("library", Path.GetFileName(pluginfile));
                        ht.Add("class", attrMenuPluginInfo.StartupClassType.FullName);
                        if (!String.IsNullOrEmpty(attrMenuPluginInfo.IconName))
                            ht.Add("iconname", attrMenuPluginInfo.IconName);

                        FindAndCreateXmlNode(plugInDoc, subSystemPlugInsNode
                            , "plugin[@library='" + Path.GetFileName(pluginfile) + "'"
                            + " and @class='" + attrMenuPluginInfo.StartupClassType.FullName + "']"
                            , "plugin", ht);
                    }
                }
                catch (Exception e) {
                    _log.Error(e.ToString());
                    continue;
                }
            }

            plugInDoc.Save(_configFileName);

            _log.Info("Config文件更新完成");

            return true;
        }

        private bool ReadPlugInConfiguration() {
            //创建菜单的XML文档
            XmlDocument plugInDoc = new XmlDocument();
            try {
                plugInDoc.Load(_configFileName);
            }
            catch {
                _log.Error("载入菜单文档时出错！\n");
                throw;
            }

            Collection<PlugInConfiguration> _genericPlugInConfig = new Collection<PlugInConfiguration>();
            foreach (XmlNode subSystemNode in plugInDoc.SelectNodes("config"))
            {
                foreach (XmlNode plugInNodes in subSystemNode.SelectNodes("plugins")) {
                    foreach (XmlNode plugInNode in plugInNodes.SelectNodes("plugin")) {
                        PlugInConfiguration _singlePlugInConfig = new PlugInConfiguration();
                        //_singlePlugInConfig.SubSystem = subSystemNode.Attributes["caption"].Value;
                        _singlePlugInConfig.MenuParentCaption = plugInNodes.Attributes["caption"].Value;
                        _singlePlugInConfig.MenuCaption = plugInNode.Attributes["caption"].Value;
                        _singlePlugInConfig.AssemblyName = plugInNode.Attributes["library"].Value;
                        _singlePlugInConfig.AssemblyStartupClass = plugInNode.Attributes["class"].Value;
                        try {
                            if (plugInNode.Attributes["iconname"] != null) {
                                _singlePlugInConfig.IconName = plugInNode.Attributes["iconname"].Value;
                                //_singlePlugInConfig.Icon = DrectSoft.Resource.DrectSoftResourceManager.GetMiddleIcon(plugInNode.Attributes["iconname"].Value, DrectSoft.Resource.IconType.Normal);
                            }
                        }
                        catch (Exception e) {
                            _log.Error(e.Message + "\r\nIconname:" + plugInNode.Attributes["iconname"].Value);
                        }
                        if (plugInNode.Attributes["visible"] != null)
                            _singlePlugInConfig.Visible = bool.Parse(plugInNode.Attributes["visible"].Value);
                        _genericPlugInConfig.Add(_singlePlugInConfig);
                    }
                }
            }
            _plugInConfiguration = _genericPlugInConfig;

            return true;
        }

        /// <summary>
        /// 创建并读取插件的配置信息,指定加载类实例
        /// </summary>
        /// <param name="configFileName"></param>
        /// <param name="plugInLoader">加载类</param>
        /// <param name="assemblyInterfaceType"></param>
        /// <returns></returns>
        public static Collection<PlugInConfiguration> CreateAndReadPlugInConfiguration(
            string configFileName, PlugInLoader plugInLoader, string assemblyInterfaceType) {
            return CreateAndReadPlugInConfiguration(configFileName, plugInLoader, assemblyInterfaceType, true);
        }

        /// <summary>
        /// 创建并读取插件的配置信息,指定加载类实例
        /// </summary>
        /// <param name="configFileName"></param>
        /// <param name="plugInLoader">加载类</param>
        /// <param name="assemblyInterfaceType"></param>
        /// <param name="needSearchPlugIn">是否需要遍历PlugIn以生成菜单文件</param>
        /// <returns></returns>
        public static Collection<PlugInConfiguration> CreateAndReadPlugInConfiguration(
            string configFileName, PlugInLoader plugInLoader, string assemblyInterfaceType, bool needSearchPlugIn) {
            
            PlugInConfigurationReader instance = new PlugInConfigurationReader(configFileName, plugInLoader, assemblyInterfaceType);
            if (needSearchPlugIn) {
                if (!instance.CreatePlugInConfiguration()) {
                    _log.Error("生成配置文件失败,请确定有访问文件的权限");
                    return new Collection<PlugInConfiguration>();
                }
            }

            if (!instance.ReadPlugInConfiguration()) {
                _log.Error("读取配置文件失败,请确定有访问文件的权限");
                return new Collection<PlugInConfiguration>();
            }

            return instance._plugInConfiguration;
        }

        /// <summary>
        /// 保存菜单设置数组到菜单文件
        /// </summary>
        /// <param name="configFileName"></param>
        /// <param name="plugInConfigurations"></param>
        /// <returns></returns>
        public static bool SavePlugInConfiguration2Xml(string configFileName, Collection<PlugInConfiguration> plugInConfigurations) {
            if (File.Exists(configFileName)) File.Delete(configFileName);

            XmlTextWriter emptyXmlDoc = new XmlTextWriter(configFileName, Encoding.Default);
            emptyXmlDoc.WriteStartDocument();
            emptyXmlDoc.WriteStartElement("config");
            emptyXmlDoc.WriteEndElement();
            emptyXmlDoc.Flush();
            emptyXmlDoc.Close();

            XmlDocument plugInDoc = new XmlDocument();
            plugInDoc.Load(configFileName);

            Hashtable ht = new Hashtable();
            for (int i = 0; i < plugInConfigurations.Count; i++) {
                ht.Clear();
                ht.Add("caption", plugInConfigurations[i].SubSystem);
                XmlNode subSystemRootNode = FindAndCreateXmlNode(plugInDoc, plugInDoc.DocumentElement
                    , "system[@caption='" + plugInConfigurations[i].SubSystem + "']"
                    , "system", ht);

                ht.Clear();
                ht.Add("caption", plugInConfigurations[i].MenuParentCaption);
                XmlNode subSystemPlugInsNode = FindAndCreateXmlNode(plugInDoc, subSystemRootNode
                    , "plugins[@caption='" + plugInConfigurations[i].MenuParentCaption + "']"
                    , "plugins", ht);

                ht.Clear();
                ht.Add("caption", plugInConfigurations[i].MenuCaption);
                ht.Add("library", plugInConfigurations[i].AssemblyName);
                ht.Add("class", plugInConfigurations[i].AssemblyStartupClass);
                ht.Add("visible", plugInConfigurations[i].Visible);
                if (plugInConfigurations[i].Icon != null) ht.Add("iconname", plugInConfigurations[i].IconName);
                FindAndCreateXmlNode(plugInDoc, subSystemPlugInsNode
                    , "plugin[@library='" + plugInConfigurations[i].AssemblyName + "'"
                    + " and @class='" + plugInConfigurations[i].AssemblyStartupClass + "']"
                    , "plugin", ht);
            }
            plugInDoc.Save(configFileName);

            return true;
        }
    }
}
