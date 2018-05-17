using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using DrectSoft.FrameWork.Plugin.Manager;

namespace DrectSoft.FrameWork.WinForm
{
   /// <summary>
   /// 插件内自定义菜单接口
   /// </summary>
   public interface IPluginOwnerMenu
   {
      /// <summary>
      /// 显示文本
      /// </summary>
      string Text { get;}

      /// <summary>
      /// 子菜单
      /// </summary>
      Collection<IPluginOwnerMenu> subMenus { get;}

      /// <summary>
      /// 单击事件
      /// </summary>
      void Click();
   }

   /// <summary>
   /// 自定义工具条接口
   /// </summary>
   public interface IPluginOwnerToolBar
   {
      /// <summary>
      /// 工具条项目
      /// </summary>
      Collection<IPluginOwnerMenu> Items { get;}

      /// <summary>
      /// 名称(同一plugin中保持唯一)
      /// </summary>
      string Name { get;}
   }

   /// <summary>
   /// 自定义弹出菜单接口
   /// </summary>
   public interface IPluginContextMenu
   {
      /// <summary>
      /// 菜单形式
      /// </summary>
      PluginContextMenuType MenuType { get;}

      /// <summary>
      /// 子菜单
      /// </summary>
      ReadOnlyCollection<IPlugInMenuInfo> Items { get;}

      /// <summary>
      /// 增加一项
      /// </summary>
      /// <param name="menu"></param>
      void AddItem(IPlugInMenuInfo menu);

      /// <summary>
      /// 指定位置增加一项
      /// </summary>
      /// <param name="index"></param>
      /// <param name="menu"></param>
      void InsertAt(int index, IPlugInMenuInfo menu);

      /// <summary>
      /// 移除一项
      /// </summary>
      /// <param name="menu"></param>
      void RemoveItem(IPlugInMenuInfo menu);

      /// <summary>
      /// 移除一项指定位置
      /// </summary>
      /// <param name="index"></param>
      void RemoveAt(int index);
   }

   /// <summary>
   /// 弹出菜单类型
   /// </summary>
   public enum PluginContextMenuType
   {
      /// <summary>
      /// .Net = ContextMenu
      /// </summary>
      DotNet = 1,

      /// <summary>
      /// Devexpress = PopupMenu
      /// </summary>
      DevExpress = 2,
   }

   /// <summary>
   /// 插件菜单接口
   /// </summary>
   public interface IPlugInMenuInfo
   {
      /// <summary>
      /// 插件信息
      /// </summary>
      PlugInConfiguration MenuInfo { get;}

      /// <summary>
      /// 子菜单
      /// </summary>
      Collection<IPlugInMenuInfo> SubItems { get;}

      /// <summary>
      /// 显示
      /// </summary>
      string Text { get;set;}

      /// <summary>
      /// 名称(定位)
      /// </summary>
      string Name { get;}
   }
}
