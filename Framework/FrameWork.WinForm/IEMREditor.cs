using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.FrameWork.WinForm
{
    /// <summary>
    /// 内嵌编辑器必须实现的接口
    /// </summary>
    public interface IEMREditor
    {
        /// <summary>
        /// 设置界面控件
        /// </summary>
        Control DesignUI { get; }

        /// <summary>
        /// 加载设置集合
        /// </summary>
        /// <param name="app"></param>

        void Load(IEmrHost app);

        /// <summary>
        /// 接口内保存更改的设置到ChangedConfigs
        /// 如果接口内即时更新ChangedConfigs,此方法无需实现(不要抛出未实现异常)
        /// </summary>
        void Save();
        /// <summary>
        /// 显示标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 接口打印方法
        /// </summary>
        void Print();

        /// <summary>
        /// 当前病人首页序号，不从框架接口IEmrHost获得
        /// </summary>
        string CurrentNoofinpat { get; set; }

        /// <summary>
        /// 是否只读
        /// </summary>
        bool ReadOnlyControl { get; set; }
    }
}
