using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.IEMMainPageZY
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

            //InpatientMainPage frmStar = new InpatientMainPage(application);
            FormMainPage frmStar = new FormMainPage(application);
            PlugIn plg = new PlugIn(this.GetType().ToString(), frmStar);
            return plg;
        }
        #endregion
    }
}
