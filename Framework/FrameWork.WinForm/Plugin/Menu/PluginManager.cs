using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.FrameWork.Plugin.Manager;
using System.Collections.ObjectModel;
using System.Diagnostics;
using DrectSoft.FrameWork.WinForm;
using System.Windows.Forms;
using DrectSoft.Core;

namespace DrectSoft.FrameWork.Plugin
{
    /// <summary>
    /// 插件管理器
    /// </summary>
    public class PluginManager
    {
        private Collection<PlugInConfiguration> pluginMenu;
        IPluginMenuRegister menuregister = null;
        const string _startupInterface = "DrectSoft.FrameWork.IStartup";
        DrectSoftLog _log ;
        PluginRunner _runner;
        Account _currentAccount;

        /// <summary>
        /// 有权限的MENU集合
        /// </summary>
        private Collection<PlugInConfiguration> m_PrivilegeMenu;
        /// <summary>
        /// 有权限的MENU信息集合
        /// </summary>
        public Collection<PlugInConfiguration> PrivilegeMenu
        {
            get { return m_PrivilegeMenu; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainApp"></param>
        /// <param name="log"></param>
        public PluginManager(Form mainApp, DrectSoftLog log)
        {

            _runner = new PluginRunner(mainApp);
            _log = log;
        }

        #region 注册插件

        /// <summary>
        /// 通过给定的文件加载控件菜单
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="menufile"></param>
        public void RegisterPlugins(string appPath, params string[] menufile)
        {
            //装载插件文件夹下所有插件，注册成插件菜单
            this.pluginMenu = PlugInLoadHelper.LoadAllPlugIns(appPath, _startupInterface, menufile);
            //this.RegisterMenuPlugins();

            _log.Info("Menu Reader Over");
        }

        /// <summary>
        /// 注册菜单插件
        /// </summary>
        public void RegisterMenuPlugins(Account currentAccount)
        {
            m_PrivilegeMenu = new Collection<PlugInConfiguration>();
            for (int i = 0; i < this.pluginMenu.Count; i++)
            {
                PlugInConfiguration pluginconfig = this.pluginMenu[i];
                if (!pluginconfig.Visible) continue;
                if (currentAccount.IsAdministrator() || (currentAccount.HasPermission(pluginconfig.AssemblyName, pluginconfig.AssemblyStartupClass)))
                    if (menuregister != null)
                    {
                        //menuregister.Register(pluginconfig);
                        m_PrivilegeMenu.Add(pluginconfig);
                    }
            }
        }


        #endregion

        #region 注销插件

        /// <summary>
        /// 卸载所有插件菜单
        /// </summary>
        public void UnRegisterAllPlugins()
        {
            if (this.pluginMenu == null) return;
            for (int i = 0; i < this.pluginMenu.Count; i++)
            {
                PlugInConfiguration pluginconfig = this.pluginMenu[i];
                if (menuregister != null)
                {
                    m_PrivilegeMenu.Remove(pluginconfig);
                    //menuregister.UnRegister(pluginconfig);
                }
            }
        }

        #endregion

        /// <summary>
        /// 菜单注册器
        /// </summary>
        public IPluginMenuRegister Menuregister
        {
            get { return menuregister; }
        }

        /// <summary>
        /// 设置注册器
        /// </summary>
        /// <param name="register"></param>
        public void SetMenuRegister(IPluginMenuRegister register)
        {
            menuregister = register;
            menuregister.Manager = this;
        }

        /// <summary>
        /// 插件启动器
        /// </summary>
        public PluginRunner Runner
        {
            get { return _runner; }
        }
    }

    /// <summary>
    /// 菜单注册接口
    /// </summary>
    public interface IPluginMenuRegister
    {
        /// <summary>
        /// 
        /// </summary>
        PluginManager Manager { get; set; }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="pluginconfig"></param>
        void Register(PlugInConfiguration pluginconfig);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluginconfig"></param>
        void UnRegister(PlugInConfiguration pluginconfig);
    }
}
