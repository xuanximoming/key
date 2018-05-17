using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Windows.Forms;

namespace DrectSoft.Core.InPatientReport
{
    public partial class FormInPatientPacsReport : DevExpress.XtraEditors.XtraForm, DrectSoft.FrameWork.IStartPlugIn
    {
        public FormInPatientPacsReport()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        private IEmrHost _app;
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        public IEmrHost App
        {
            get { return _app; }
            set { _app = value; }
        }


        internal static string ServerURL
        {
            get
            {
                if (string.IsNullOrEmpty(_serverURl))
                    _serverURl = BasicSettings.GetStringConfig("YindanWebServerUrL");
                return _serverURl;
            }
        }
        private static string _serverURl;


        private IDataAccess m_DataAccessEmrly;

        private void InPatientReport_Load(object sender, EventArgs e)
        {
            UCInpatientPacsReport ucReport = new UCInpatientPacsReport(_app);
            ucReport.Dock = DockStyle.Fill;
            ucReport.BringToFront();
            this.panelControlReport.Controls.Add(ucReport);
            ucReport.Visible = true;
        }

        #region IStartPlugIn 成员

        public DrectSoft.FrameWork.IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            _app = host;
            m_DataAccessEmrly = host.SqlHelper;
            return plg;
        }

        #endregion
    }
}