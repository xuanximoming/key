using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core
{
   /// <summary>
   /// 系统设置接口
   /// </summary>
   public interface IAppConfigDesign
   {
      /// <summary>
      /// 设置界面控件
      /// </summary>
      Control DesignUI { get;}

      /// <summary>
      /// 加载设置集合
      /// </summary>
      /// <param name="app"></param>
      /// <param name="configs"></param>
      void Load(IEmrHost app, Dictionary<string, EmrAppConfig> configs);

      /// <summary>
      /// 接口内保存更改的设置到ChangedConfigs
      /// 如果接口内即时更新ChangedConfigs,此方法无需实现(不要抛出未实现异常)
      /// </summary>
      void Save();

      /// <summary>
      /// 更新设置集合
      /// </summary>
      Dictionary<string, EmrAppConfig> ChangedConfigs { get;}

      /// <summary>
      /// 设置对象(如果有返回,没有则null)
      /// </summary>
      object ConfigObj { get;}
   }

}

