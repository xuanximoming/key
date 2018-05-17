using System;
using System.Collections.Generic;
using System.Text;

namespace YidanSoft.FrameWork.WinForm {
    /// <summary>
    /// 枚举类，用来表示框架构造的几个阶段
    /// </summary>
    [Flags]
    public enum BuilderStage {
        /// <summary>
        /// 登录
        /// </summary>
        Login = 0,
        /// <summary>
        /// 初始化
        /// </summary>
        Initialization = 1,
        /// <summary>
        /// 完成初始化
        /// </summary>
        PostInitialization = 2
    }
}
