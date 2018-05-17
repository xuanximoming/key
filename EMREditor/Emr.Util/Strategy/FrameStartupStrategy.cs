using System;
using System.Collections.Generic;
using System.Text;

namespace YidanSoft.FrameWork.WinForm
{
    /// <summary>
    /// ¿ò¼ÜÆô¶¯²ßÂÔ
    /// </summary>
    public abstract class FrameStartupStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool BuildUp()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract BuilderStage GetStage();
    }
}
