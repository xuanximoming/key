using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.Permission
{
    public class PermissionStart : IStartPlugIn
    {
        #region IStartup 成员

        /// <summary>
        /// 启动类
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public IPlugIn Run(IEmrHost app)
        {
            try
            {
                if (app == null)
                {
                    throw new ArgumentNullException("application");
                }
                PermissionMainForm permission = new PermissionMainForm(app);
                PlugIn plg = new PlugIn(this.GetType().ToString(), permission);
                return plg;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
