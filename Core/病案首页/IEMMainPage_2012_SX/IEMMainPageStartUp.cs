using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;

namespace DrectSoft.Core.IEMMainPage
{
    public class IEMMainPageStartUp : IStartPlugIn
    {
        #region IStartup 成员

        /// <summary>
        /// 启动类
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public IPlugIn Run(IEmrHost application)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            FormMainPage frmStar = new FormMainPage(application);
            PlugIn plg = new PlugIn(this.GetType().ToString(), frmStar);
            return plg;
        }
        #endregion
    }
}
