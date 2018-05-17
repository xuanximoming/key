using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.DSReportManager;
using DrectSoft.Core.ReportManager.UCControl;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Core.ReportManager
{
    /// <summary>
    /// 心脑血管病发病报告卡的填写界面
    /// add by ywk 2013年8月15日 23:21:20
    /// </summary>
    public partial class BirthDefectsDialog : DevBaseForm
    {
        #region 属性及字段
        IEmrHost m_Host;
        BirthDefectsControl m_cau;
        private SqlHelper m_SqlHelper;
        public string icd10;
        private string m_noofinpat;
        #endregion

        #region 构造函数

        public BirthDefectsDialog()
        {
            InitializeComponent();
        }
        public BirthDefectsDialog(IEmrHost app, DataTable icdtable, string noofinpat)
        {
            InitializeComponent();
            m_Host = app;
            //m_Noofinpat = noofinpat;
            if (icdtable != null && icdtable.Rows.Count > 0)
            {
                icd10 = icdtable.Rows[0]["icd"].ToString();
            }
            m_noofinpat = noofinpat;
        }

        SqlHelper SqlHelper
        {
            get
            {
                if (m_SqlHelper == null)
                    m_SqlHelper = new SqlHelper(m_Host.SqlHelper);
                return m_SqlHelper;
            }
            set { m_SqlHelper = value; }
        }
        #endregion

        #region 方法
        public void LoadBirthPage(string id, string type, string userRole)
        {
            if (m_cau == null)
            {
                m_cau = new BirthDefectsControl(m_Host);
                if (icd10 != "")
                {
                    m_cau.DiagICD10 = icd10;
                }
            }
            m_cau.LoadPage(id, type, userRole);
        }
        #endregion

        #region 事件
        /// <summary>
        /// 提交事件
        /// add by ywk2013年8月15日 23:52:23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubmit_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 保存事件
        /// add by ywk 2013年8月15日 23:52:39
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (SqlHelper == null)
                {
                    SqlHelper = new SqlHelper(m_Host.SqlHelper);
                }
                ReportCardEntity1 m_cardEntity = new ReportCardEntity1();
                m_cau.m_Noofinpat = m_noofinpat;
                ReportCardEntity1 _cardEntity = m_cau.GetEntityUI(m_cardEntity);
                _cardEntity.DiagCode = icd10;
                if (m_cau.checkdata())
                {
                    m_SqlHelper.AddBirthDefectsReportCard(_cardEntity);
                    MessageBox.Show("新增成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("新增失败，错误信息:" + ex.Message);
                return;
            }

        }
        /// <summary>
        /// 取消事件
        /// add by ywk 2013年8月15日 23:53:04
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            //Cardiovascular m_cau = new Cardiovascular(m_Host);
            //m_cau.EnableState(false);
            m_cau.ClearPage();
            //this.Close();
        }
        /// <summary>
        /// 界面加载事件
        /// add by ywk 2013年8月20日 16:12:36
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BirthDefectsDialog_Load(object sender, EventArgs e)
        {
            try
            {
                //m_cau = new BirthDefectsControl(m_Host);
                this.xtraScrollableControl1.Controls.Add(m_cau);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息:" + ex.Message);
                return;
            }

        }
        #endregion



    }
}