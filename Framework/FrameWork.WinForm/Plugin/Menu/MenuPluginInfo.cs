using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.FrameWork.Plugin.Manager;
using System.Collections.ObjectModel;

namespace DrectSoft.FrameWork.Plugin
{
    //public class MenuPluginInfo:PluginInfo
    //{

    //}

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
