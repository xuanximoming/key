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

        IDataAccess m_EhrSqlhelper;

        public InpatientPathForm()
        {
            InitializeComponent();

        }


        private decimal GetPatEhrPatid(decimal patid)
        {
            if (m_EhrSqlhelper == null)
            {
                m_EhrSqlhelper = DataAccessFactory.GetSqlDataAccess("EHRDB");
            }

            try
            {
                object obj = m_EhrSqlhelper.ExecuteScalar("select syxh from CP_Inpatient where hissyxh=" + patid + "");

                return Convert.ToDecimal(obj);

            }
            catch
            {
                m_host.CustomMessageBox.MessageShow("与临床路径系统数据不一致");
            }

            return -1;

        }

        private void InpatientPathForm_Load(object sender, EventArgs e)
        {

            if (m_host.CurrentPatientInfo == null)
            {

                webBrowser1.Navigate("http://" + ConstStr.CP_ServerURL + "/EHRDefault.aspx?SkipLogin=true&username=" + m_host.User.Id + "&viewname=/Views/UserCenterManager.xaml");
            }
            else
                //webBrowser1.Navigate("http://" + ConstStr.CP_ServerURL + "/EHRDefault.aspx?username=" + m_host.User.Id + "&patid=" + m_host.CurrentPatientInfo.NoOfHisFirstPage + "&viewname=/Views/PathEnForce.xaml");
                webBrowser1.Navigate("http://" + ConstStr.CP_ServerURL + "/EHRDefault.aspx?SkipLogin=true&username=" + m_host.User.Id + "&patid=" + m_host.CurrentPatientInfo.NoOfHisFirstPage + "&StartPage=/Views/InpatientList.xaml&StartPagePathExecute=/Views/PathEnForce.xaml");

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
            //
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
    }
}
