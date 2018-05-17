using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.FrameWork.Plugin;
using DrectSoft.FrameWork.Plugin.Manager;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;


namespace DrectSoft.FrameWork.WinForm
{
    /// <summary>
    /// 插件菜单注册器
    /// </summary>
    public class PluginMenuRegister : IPluginMenuRegister
    {
        private PluginManager manager;
        private List<IPluginMenuRegister> childMenuRegister = new List<IPluginMenuRegister>();

        /// <summary>
        /// ctor
        /// </summary>
        public PluginMenuRegister()
        {

        }

        /// <summary>
        /// 子菜单注册
        /// </summary>
        public ReadOnlyCollection<IPluginMenuRegister> ChildMenuRegister
        {
            get { return new ReadOnlyCollection<IPluginMenuRegister>(childMenuRegister); }
        }

        /// <summary>
        /// 增加子菜单
        /// </summary>
        /// <param name="menuRegister"></param>
        public void AddChildRegister(IPluginMenuRegister menuRegister)
        {
            menuRegister.Manager = this.Manager;
            childMenuRegister.Add(menuRegister);
        }

        /// <summary>
        /// 清除子菜单信息
        /// </summary>
        public void ClearChildRegisters()
        {
            childMenuRegister.Clear();
        }

        #region IPluginMenuRegister Members

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="pluginconfig"></param>
        public void Register(PlugInConfiguration pluginconfig)
        {
            foreach (IPluginMenuRegister register in childMenuRegister)
            {
                register.Register(pluginconfig);
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="pluginconfig"></param>
        public void UnRegister(PlugInConfiguration pluginconfig)
        {
            foreach (IPluginMenuRegister register in childMenuRegister)
            {
                register.UnRegister(pluginconfig);
            }
        }

        /// <summary>
        /// 插件管理器
        /// </summary>
        public PluginManager Manager
        {
            get { return manager; }
            set { manager = value; }
        }
        #endregion
    }
}
