using MedicalRecordManage.Object;
using MedicalRecordManage.UCControl;
using System.Windows.Forms;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace MedicalRecordManage
{
    public partial class FormMain : DevExpress.XtraEditors.XtraForm, IStartPlugIn
    {
        IEmrHost m_app;

        public FormMain(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }
        public FormMain()
        {
            InitializeComponent();
        }
        #region IStartPlugIn 成员
        /// <summary>
        /// 以控件模式运行模块
        /// </summary>
        /// <param name="host">数据成员</param>
        /// <returns></returns>
        public IPlugIn Run(IEmrHost host)
        {
            SqlUtil.App = host;
            m_app = host;
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            return plg;
        }
        #endregion

        private void FormMain_Load(object sender, System.EventArgs e)
        {
            MedicalRecordReadOnly medicalRecordReadOnly = new MedicalRecordReadOnly();
            medicalRecordReadOnly.Dock = DockStyle.Fill;
            this.Controls.Add(medicalRecordReadOnly);
        }
    }
}
