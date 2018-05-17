using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DrectSoft.FrameWork.Plugin.Manager
{
   /// <summary>
   /// 插件Plugin的设置内容
   /// </summary>
   public class PlugInConfiguration
   {
      //private PlugInConfiguration _instance;
      private string _assemblyName;
      private string _assemblyStartupClass;
      private string _menuCaption;
      private string _menuParentCaption;
      private string _subSystem;
      private bool _visible = true;
      private Image _icon;
      private string _iconname = string.Empty;

      /// <summary>
      /// Ctor
      /// </summary>
      public PlugInConfiguration()
      {
         _menuCaption = "Default Menu Name";
      }

      /// <summary>
      /// 插件的模块文件名称
      /// </summary>
      /// <value></value>
      [ReadOnly(true)]
      [DisplayName("模块文件名")]
      public string AssemblyName
      {
         get { return _assemblyName; }
         set { _assemblyName = value; }
      }

      /// <summary>
      /// 插件启动的类名
      /// </summary>
      /// <value></value>
      [Browsable(false)]
      public string AssemblyStartupClass
      {
         get { return _assemblyStartupClass; }
         set { _assemblyStartupClass = value; }
      }

      /// <summary>
      /// 插件的菜单名称
      /// </summary>
      /// <value></value>
      [DisplayName("菜单名称")]
      public string MenuCaption
      {
         get { return _menuCaption; }
         set { _menuCaption = value; }
      }

      /// <summary>
      /// 插件菜单加载到所属的菜单名称
      /// </summary>
      /// <value></value>
      [DisplayName("上级菜单名称")]
      public string MenuParentCaption
      {
         get { return _menuParentCaption; }
         set { _menuParentCaption = value; }
      }

      /// <summary>
      /// 插件所属的子系统
      /// </summary>
      [DisplayName("所属子系统")]
      public string SubSystem
      {
         get { return _subSystem; }
         set { _subSystem = value; }
      }

      //[Browsable(false)]
      /// <summary>
      /// 菜单是否可见
      /// </summary>
      public bool Visible
      {
         get { return _visible; }
         set { _visible = value; }
      }

      /// <summary>
      /// 得到或设置菜单图标
      /// </summary>
      public Image Icon
      {
         get { return _icon; }
         set { _icon = value; }
      }

      /// <summary>
      /// 得到或设置菜单图标名称
      /// </summary>
      public string IconName
      {
         get { return _iconname; }
         set { _iconname = value; }
      }

      /// <summary>
      /// 复制1个
      /// </summary>
      /// <returns></returns>
      public PlugInConfiguration Clone()
      {
         PlugInConfiguration cfg = new PlugInConfiguration();
         cfg.AssemblyName = this.AssemblyName;
         cfg.AssemblyStartupClass = this.AssemblyStartupClass;
         cfg.MenuCaption = this.MenuCaption;
         cfg.MenuParentCaption = this.MenuParentCaption;
         cfg.SubSystem = this.SubSystem;
         cfg.Icon = this.Icon;
         cfg.IconName = this.IconName;
         cfg.Visible = this.Visible;
         return cfg;
      }
   }
}
