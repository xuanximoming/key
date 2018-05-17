using System;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.TemplatePerson
{
    internal class StartUpTemplatePerson : IStartPlugIn
    {
        private IEmrHost m_App;

        public IPlugIn Run(IEmrHost application)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }
            this.m_App = application;
            TemplatePersonForm mainForm = new TemplatePersonForm(this.m_App);
            return new PlugIn(base.GetType().ToString(), mainForm);
        }
    }
}
