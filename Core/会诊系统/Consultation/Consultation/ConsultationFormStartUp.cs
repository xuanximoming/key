using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.Consultation
{
    class ConsultationFormStartUp : IStartPlugIn
    {
        private IEmrHost m_App;


        #region IStartPlugIn 成员
        public IPlugIn Run(IEmrHost host)
        {
            FormMainConsultation form = new FormMainConsultation(host);
            m_App = host;
            PlugIn plg = new PlugIn(this.GetType().ToString(), form);
            return plg;
        }
        #endregion
    }
}
