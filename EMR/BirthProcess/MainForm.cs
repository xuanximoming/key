using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Windows.Forms;

namespace DrectSoft.Core.BirthProcess
{
    public partial class MainForm : DevExpress.XtraEditors.XtraUserControl, IEMREditor
    {
        private IEmrHost m_app;
        internal IEmrHost App
        {
            get { return m_app; }
        }

        private Inpatient _currentPat = null;
        public Inpatient CurrentNoofinpat
        {
            get
            {
                if (_currentPat == null)
                {
                    _currentPat = m_app.CurrentPatientInfo;
                }
                return _currentPat;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(IEmrHost app)
        {
            m_app = app;
        }

        public MainForm(string noofinpat)
            : this()
        {
            Init();
        }

        private void Init()
        {
            timeEditCheckTime.Time = DateTime.Now;
        }

        private void simpleButtonShowImage_Click(object sender, EventArgs e)
        {
            BirthProcessImage birthProcessImage = new BirthProcessImage();
            birthProcessImage.ShowDialog();
        }

        #region IEMREditor 接口实现

        public Control DesignUI
        {
            get { return this; }
        }

        public new void Load(IEmrHost app)
        {
            try
            {
                if (app == null) return;
                m_app = app;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        public void Print()
        {
        }

        public void Save()
        {
        }

        string IEMREditor.CurrentNoofinpat
        {
            get
            {
                return "";
            }
            set
            { }
        }

        public bool ReadOnlyControl
        {
            get
            {
                return false;
            }
            set
            { }
        }

        public string Title
        {
            get { return ""; }
        }
        #endregion

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            FrmEdit frm = new FrmEdit();
            frm.ShowDialog();
            frm.Dispose();
        }
        private void DevButtonAdd_Click(object sender, EventArgs e)
        {
            gridView1.AddNewRow();
        }
    }
    class recoder1
    {
        string datetimenow { get; set; }
        string gongkai { get; set; }
        string taitou { get; set; }
        string fangshi { get; set; }
        string fengmian { get; set; }
        string qianzi { get; set; }
        string luru { get; set; }
        string shanchu { get; set; }
    }
}