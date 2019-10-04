using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DrectSoft.Core.DoctorTasks
{
    public partial class InpatientPathForm : Form, IStartPlugIn
    {

        IEmrHost m_host;

        public InpatientPathForm()
        {
            InitializeComponent();

        }

        private void InpatientPathForm_Load(object sender, EventArgs e)
        {

            if (m_host.CurrentPatientInfo == null)
            {
                webBrowser1.Navigate("http://" + ConstStr.CP_ServerURL + "/EHRDefault.aspx?SkipLogin=true&username=" + m_host.User.Id + "&viewname=/Views/UserCenterManager.xaml");
            }
            else
            {
                webBrowser1.Navigate("http://" + ConstStr.CP_ServerURL + "/EHRDefault.aspx?SkipLogin=true&username=" + m_host.User.Id + "&patid=" + m_host.CurrentPatientInfo.NoOfHisFirstPage + "&StartPage=/Views/InpatientList.xaml&StartPagePathExecute=/Views/PathEnForce.xaml");
                address.Text = "http://" + ConstStr.CP_ServerURL + "/EHRDefault.aspx";
            }
        }

        #region IStartPlugIn 成员

        public IPlugIn Run(IEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);

            plg.PatientChanging += new PatientChangingHandler(plg_PatientChanging);
            plg.PatientChanged += new PatientChangedHandler(plg_PatientChanged);
            m_host = host;
            return plg;

        }


        void plg_PatientChanged(object Sender, PatientArgs arg)
        {
            if (webBrowser1 == null)
                webBrowser1 = new WebBrowser();
            webBrowser1.Navigate("http://localhost:3531/EHRDefault.aspx?SkipLogin=true&username=" + m_host.User.Id + "&patid=" + m_host.CurrentPatientInfo.NoOfHisFirstPage + "&StartPage=/Views/InpatientList.xaml&StartPagePathExecute=/Views/PathEnForce.xaml");
        }


        void plg_PatientChanging(object sender, CancelEventArgs arg)
        {
            if (m_host.CustomMessageBox.MessageShow("是否允许系统切换当前病人?!", CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
            {
                arg.Cancel = false;
            }
        }
        #endregion

        #region 事件
        private void refresh_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void address_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                webBrowser1.Navigate(address.Text);
            }
        }
        #endregion
    }
}
