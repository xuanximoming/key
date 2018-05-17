using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using System;
using System.Windows.Forms;

namespace DrectSoft.Core.DoctorTasks
{
    public partial class DrugInfoForm : Form, IStartPlugIn
    {
        public DrugInfoForm()
        {
            InitializeComponent();
        }

        private void DrugInfoForm_Load(object sender, EventArgs e)
        {
            string ServerURL = BasicSettings.GetStringConfig("DrectSoftWebServerUrL");

            webBrowser1.BringToFront();
            //webBroswer.Navigate("http://localhost:8010/Applications/Manage/NewsList.aspx?userid=" + m_app.User.Id + "&username=" + m_app.User.Name + "&userdept=" + m_app.User.DeptId + "");
            webBrowser1.Navigate("http://" + ServerURL + "/Search2/Default.aspx");
        }

        public IPlugIn Run(FrameWork.WinForm.Plugin.IEmrHost host)
        {
            IPlugIn plg = new PlugIn(GetType().ToString(), this);
            return plg;
        }
    }
}
