using DrectSoft.FrameWork.Plugin.Manager;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DrectSoft.FrameWork.WinForm
{
    /// <summary>
    /// 
    /// </summary>
    public class PluginRunner
    {
        const string _startupInterface = "DrectSoft.FrameWork.IStartPlugIn";
        const string _businessInterface = "DrectSoft.FrameWork.IBusiness";
        const string PlugInLoadConfigSectionName = "plugInLoadSettings";
        private List<IPlugIn> m_PluginsLoaded = new List<IPlugIn>();

        //执行过病人修改操作的插件
        private List<IPlugIn> _patientChangeExecuted = new List<IPlugIn>();
        private List<IPlugIn> _userChangeExecuted = new List<IPlugIn>();
        //启动插件
        private List<IPlugIn> _startPlugin = new List<IPlugIn>();
        private IPlugIn _activePlugIn;
        private Form m_owner;

        public PluginRunner(Form owner)
        {
            m_owner = owner;

            if (_patientChangeExecuted == null)
                _patientChangeExecuted = new List<IPlugIn>();

            _startPlugin = new List<IPlugIn>();
        }



        /// <summary>
        /// 定位已经调用的索引
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="assemblyClassName"></param>
        /// <returns></returns>
        public int PluginIndexInLoaded(string assemblyName, string assemblyClassName)
        {
            return PlugInIndexInList(assemblyName, assemblyClassName, PluginsLoaded);
        }

        public int PlugInIndexInList(string assemblyName, string assemblyClassName, List<IPlugIn> plugins)
        {
            int foundIndex = -1;
            if (plugins == null) return foundIndex;

            for (int i = 0; i < plugins.Count; i++)
            {
                if (string.Compare(assemblyName, (plugins[i]).AssemblyFileName, true) == 0)
                {
                    if (assemblyClassName == (plugins[i]).StartClassType)
                    {
                        foundIndex = i;
                        break;
                    }
                    else
                    {
                        string[] temp = assemblyClassName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                        if (temp != null && temp.Length > 0)
                        {
                            if (plugins[i].StartClassType.Contains(temp[temp.Length - 1]))
                            {
                                foundIndex = i;
                                break;
                            }
                        }
                    }
                }
            }

            return foundIndex;
        }

        /// <summary>
        /// 运行插件(菜单信息)
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <returns></returns>
        public IPlugIn LoadPlugIn(PlugInConfiguration menuInfo)
        {
            return LoadPlugIn(menuInfo.AssemblyName, menuInfo.AssemblyStartupClass, false);
        }

        /// <summary>
        /// 运行插件(菜单)
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        public IPlugIn LoadPlugIn(IPlugInMenuInfo menuItem)
        {
            return LoadPlugIn(menuItem.MenuInfo.AssemblyName, menuItem.MenuInfo.AssemblyStartupClass, false);
        }

        /// <summary>
        /// 从给定的程序集和启动类启动插件
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="startupClassName">启动类名</param>
        /// <returns> 返回加载成功的插件</returns>
        public IPlugIn LoadPlugIn(string assemblyName, string startupClassName, bool notShowMessage)
        {
            //校验
            Type startupType;
            if (!this.ValidatePlugin(assemblyName, startupClassName, notShowMessage, out startupType))
                return null;

            IPlugIn plugin;
            int foundindex = this.PluginIndexInLoaded(assemblyName, startupClassName);
            if (foundindex < 0)
            {
                plugin = BuildPlugin(assemblyName, startupClassName);
                RunPlugin(plugin);
            }
            else
            {
                plugin = (IPlugIn)this.PluginsLoaded[foundindex];
                FocusLoadedPlugIn(plugin);
            }
            return plugin;
        }

        /// <summary>
        /// 创建插件
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="startupClassName">启动类</param>
        /// <returns></returns>
        public IPlugIn BuildPlugin(string assemblyName, string startupClassName)
        {
            Type startupType;
            if (!this.ValidatePlugin(assemblyName, startupClassName, true, out startupType))
                return null;
            IPlugIn plugin;

            //IStartPlugIn startup = (IStartPlugIn)(PlugInDynamicProxy.CreateDynamicProxyClass(startupType));
            IStartPlugIn startup = (IStartPlugIn)Activator.CreateInstance(startupType);
            plugin = startup.Run(this.m_owner as IEmrHost);
            return plugin;
        }

        private bool ValidatePlugin(string assemblyName, string startupClassName, bool notShowMessage, out Type startupType)
        {
            startupType = null;
            Assembly assembly;
            try
            {
                assembly = PlugInLoadHelper.LoadPlugIn(Application.StartupPath, assemblyName);
            }
            catch (FileNotFoundException)
            {
                if (!notShowMessage)
                {
                    MessageBox.Show("所给定的程序集不存在");
                }
                return false;
            }

            try
            {
                startupType = assembly.GetType(startupClassName, true, true);
            }
            catch (TypeLoadException)
            {
                if (!notShowMessage)
                {
                    MessageBox.Show("所给定的启动类[" + startupClassName + "]不存在！");
                }
                return false;
            }

            //检查所给定的启动类是否实现了DrectSoft.Framework.IStartup接口
            Type startupInterfaceType = startupType.GetInterface(_startupInterface);
            if (startupInterfaceType == null)
            {
                if (!notShowMessage)
                {
                    MessageBox.Show("所给定的启动类没有实现[" + _startupInterface + "]接口！");
                }
                return false;
            }
            return true;
        }
        public void RunPlugin(IPlugIn plugin)
        {
            this.m_PluginsLoaded.Add(plugin);
            this._activePlugIn = plugin;
            //加载启动窗体
            this.LoadPluginMainForm(plugin);
            //内存回收
            MemoryUtil.FlushMemory();
        }


        /// <summary>
        /// 聚焦装载的PlugIn
        /// </summary>
        /// <param name="plugin"></param>
        public void FocusLoadedPlugIn(IPlugIn plugin)
        {
            if (plugin.MainForm != null)
            {
                plugin.MainForm.BringToFront();
            }
        }


        /// <summary>
        /// 启动插件的主窗口
        /// </summary>
        /// <param name="plugin"></param>
        private void LoadPluginMainForm(IPlugIn plugin)
        {
            try
            {
                if (plugin.MainForm != null)
                {
                    Form mainForm = plugin.MainForm;
                    mainForm.ShowInTaskbar = false;
                    mainForm.Owner = this.m_owner;
                    mainForm.FormClosed += new FormClosedEventHandler(this.OnPluginFormClosed);
                    mainForm.Activated += new EventHandler(this.OnPlugInStartFormActived);
                    if (plugin.IsShowModel)
                    {
                        // CaptionBarAnother.SetFormCaption(mainForm);
                        mainForm.ShowDialog();
                    }
                    else
                    {
                        if (plugin.IsMdiChild)
                        {
                            mainForm.MdiParent = this.m_owner;
                        }
                        //mainForm.WindowState = FormWindowState.Normal;
                        mainForm.Show();
                    }
                }
                else
                {
                    //_dockingHelper.ShowDockingContent(plugin.AddInDockingForms[0].DockingWindows[0].DockingContents[0]);
                    // ToDo , 不存在主窗口,则需要对于DockWindow关闭处理,暂时不处理
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnPluginFormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender == null) throw new ArgumentNullException("sender", "未传入插件窗口");

            Form f = sender as Form;
            if (f == null) return;
            //在关闭窗口时候释放一下系统内存
            MemoryUtil.FlushMemory();
            //if (_parentMdiChildrenHandles.IndexOf(f.Handle) >= 0)
            //{
            //   _parentMdiChildrenHandles.Remove(f.Handle);
            //}
            for (int i = 0; i < this.PluginsLoaded.Count; i++)
            {
                IPlugIn plugin = (IPlugIn)((IPlugIn)this.PluginsLoaded[i] as PlugIn).Clone();
                if (plugin.MainForm == sender)
                {
                    this.ClosePlugin(plugin);
                    this.PluginsLoaded.Remove(plugin);
                    return;
                }
            }
        }

        private void OnPlugInStartFormActived(object sender, EventArgs e)
        {

            if (sender == null) throw new ArgumentNullException("sender", "未传入插件窗口");

            Form f = sender as Form;
            if (f != null)
            {
                //if (_parentMdiChildrenHandles.IndexOf(f.Handle) <= 0)
                //{
                //   _parentMdiChildrenHandles.Add(f.Handle);
                //}
                for (int i = 0; i < this.PluginsLoaded.Count; i++)
                {
                    IPlugIn plugin = (IPlugIn)((IPlugIn)this.PluginsLoaded[i] as PlugIn).Clone();
                    if (plugin.MainForm == sender)
                    {
                        if (this.ActivePlugIn == plugin) return;
                        // ActivePluginDockingForm(plugin);
                    }
                }
            }
        }

        /// <summary>
        /// 关闭插件
        /// </summary>
        /// <param name="plugin">插件对象</param>
        public void ClosePlugin(IPlugIn plugin)
        {
            if (plugin == null) return;

            this.m_PluginsLoaded.Remove(plugin);
            //modified by zhouhui 此处代码待确认
            this.ActivePlugIn = null;
        }

        /// <summary>
        /// 关闭插件,和窗口
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="closeform"></param>
        public void ClosePlugin(IPlugIn plugin, bool closeform)
        {

            if (closeform && plugin.MainForm != null)
            {
                plugin.MainForm.Close();
            }

            ClosePlugin(plugin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void CloseAllPlugins()
        {
            for (int i = PluginsLoaded.Count - 1; i >= 0; i--)
            //for (int i = 0; i < PluginsLoaded.Count; i++)
            {
                IPlugIn plugin = (IPlugIn)PluginsLoaded[i];
                if (plugin.MainForm != null)
                {
                    plugin.MainForm.Close();
                }
                else
                {
                    ClosePlugin(plugin);
                    PluginsLoaded.RemoveAt(i);
                }
                if (PluginsLoaded.Contains(plugin))
                {
                    //todo
                    //关闭插件不成功，可能是插件在关闭时做了一些工作，阻止关闭
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public List<IPlugIn> PluginsLoaded
        {
            get { return m_PluginsLoaded; }
            set { m_PluginsLoaded = value; }
        }

        public IPlugIn ActivePlugIn
        {
            get { return _activePlugIn; }
            set { _activePlugIn = value; }
        }


        public List<IPlugIn> PluginsWithMainFormLoaded
        {
            get
            {
                List<IPlugIn> plugins = new List<IPlugIn>();
                foreach (IPlugIn plg in PluginsLoaded)
                {
                    if (plg.MainForm != null)
                        plugins.Add(plg);
                }
                return plugins;
            }
        }

        /// <summary>
        /// 记载已经执行过床位号改变的事件
        /// </summary>
        public List<IPlugIn> PatientChangeExecuted
        {
            get { return _patientChangeExecuted; }
        }

        /// <summary>
        /// 记载已经执行过用户改变的事件
        /// </summary>
        public List<IPlugIn> UserChangeExecuted
        {
            get { return _userChangeExecuted; }
        }


        public Form Owner
        {
            get { return m_owner; }
            set { m_owner = value; }
        }
        /// <summary>
        /// 启动Plugin
        /// </summary>
        public List<IPlugIn> StartPlugins
        {
            get { return _startPlugin; }
            set { _startPlugin = value; }
        }
    }
}
