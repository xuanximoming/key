using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Plugin
{
    /// <summary>
    /// UI框架插件特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class FramePluginAttribute:PluginAttribute
    {
       /// <summary>
       /// UI插件
       /// </summary>
       public override PluginType PluginType
        {
            get { return PluginType.Frame; }
        }
    }
}
