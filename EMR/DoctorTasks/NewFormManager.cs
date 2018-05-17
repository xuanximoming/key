using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.DoctorTasks
{
    public partial class NewFormManager : DevExpress.XtraEditors.XtraForm,DrectSoft.FrameWork.IStartPlugIn
    {

        IEmrHost m_host;
        public NewFormManager()
        {
            InitializeComponent();
            this.Load += new EventHandler(NewFormManager_Load);
        }



        void NewFormManager_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://" + ConstStr.ServerURL + "/Applications/Manage/NewsList.aspx?userid="+m_host.User.Id+"&username="+m_host.User.Name+"&userdept="+m_host.User.CurrentDeptId+"");
        }



        #region IStartPlugIn ≥…‘±

        public DrectSoft.FrameWork.IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_host=host;
            return plg;
        }

        #endregion
    }
}