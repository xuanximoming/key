using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Plugin
{
    /// <summary>
    /// 外部插件特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ClientPluginAttribute:PluginAttribute
    {
        private string _menuNameSubsystem;
        private string _menuNameParent;
        private string _menuNameAssembly;
        private Type _startupClassType;
        private bool _visible = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="menuNameSubsystem">所属的子系统名称</param>
        /// <param name="menuNameParent">上级菜单名称</param>
        /// <param name="menuNameAssembly">模块菜单名称</param>
        /// <param name="startupClassType">启动类</param>
       public ClientPluginAttribute(string menuNameSubsystem, string menuNameParent, string menuNameAssembly, Type startupClassType)
        {
            _menuNameSubsystem = menuNameSubsystem;
            _menuNameParent = menuNameParent;
            _menuNameAssembly = menuNameAssembly;
            _startupClassType = startupClassType;
        }

        /// <summary>
        /// 所属的子系统名称
        /// </summary>
        /// <value></value>
        public string MenuNameSubsystem
        {
            get { return _menuNameSubsystem; }
        }

        /// <summary>
        /// 上级菜单的名称
        /// </summary>
        /// <value></value>
        public string MenuNameParent
        {
            get { return _menuNameParent; }
        }

        /// <summary>
        /// 代表Assembly的菜单名称,模块菜单名称
        /// </summary>
        /// <value></value>
        public string MenuNameAssembly
        {
            get { return _menuNameAssembly; }
        }

        /// <summary>
        /// 启动的类
        /// </summary>
        /// <value></value>
        public Type StartupClassType
        {
            get { return _startupClassType; }
        }

        /// <summary>
        /// 获得插件类型
        /// </summary>
       public override PluginType PluginType
        {
            get { return PluginType.External; }
        }

       /// <summary>
       /// 获得图标名称
       /// </summary>
       public string IconName
       {
           get { return _iconName; }
       }
       private string _iconName;


       /// <summary>
       /// 定义菜单是否可见
       /// </summary>
       public bool Visible
       {
           get { return _visible; }
           set { _visible = value; }
       }
    }
}
