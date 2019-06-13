using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YidanSoft.FrameWork;
using YidanSoft.FrameWork.WinForm;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.IEMMainPage
{
    public class IEMMainPageStartUp : IStartPlugIn
    {
        #region IStartup 成员

        /// <summary>
        /// 启动类
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public IPlugIn Run(IYidanEmrHost application)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            //InpatientMainPage frmStar = new InpatientMainPage(application);
            FormMainPage frmStar = new FormMainPage(application);
            PlugIn plg = new PlugIn(this.GetType().ToString(), frmStar);
            return plg;
        }
        #endregion
    }
}
