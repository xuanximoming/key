using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;

namespace DrectSoft.Core.OwnBedInfo
{
    /// <summary>
    /// 病区一览启动类
    /// </summary>
    public class BedMappingStartUp : IStartPlugIn
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

            DocCenter frmDocCenter = new DocCenter(application);
            PlugIn plg = new PlugIn(this.GetType().ToString(), frmDocCenter);
            return plg;
        }

        void plg_PatientChanging(object sender, System.ComponentModel.CancelEventArgs arg)
        {
            //TODO
            //检查是否需要保存
        }
        #endregion
    }
}
