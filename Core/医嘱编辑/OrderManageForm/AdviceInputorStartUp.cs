using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;

namespace DrectSoft.Core.OrderManage
{
    class AdviceInputorStartUp : IStartPlugIn
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

            AdviceInputor frmDocCenter = new AdviceInputor(application);
            PlugIn plg = new PlugIn(this.GetType().ToString(), frmDocCenter);
            plg.PatientChanging += new PatientChangingHandler(frmDocCenter.DoPatientChanging);
            plg.PatientChanged += new PatientChangedHandler(frmDocCenter.DoPatientChanged);
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
