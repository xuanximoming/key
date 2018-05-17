using DevExpress.Utils;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DrectSoft.Core.NurseDocument
{
    public partial class MainNursingMeasure : DevExpress.XtraEditors.XtraUserControl, IEMREditor
    {
        public static bool m_showTempareture = true; //是否显示体温数据
        public static bool m_showPulse = true;//是否显示脉搏数据
        public static bool m_showBreath = true;//是否显示呼吸数据
        private DataLoader dataLoader = new DataLoader();//数据查询工具类对象
        private string m_InTime = string.Empty;//病人入科(院)日期

        public MainNursingMeasure()
        {
            InitializeComponent();
        }

        public MainNursingMeasure(IEmrHost app)
        {
            m_app = app;
            MethodSet.App = app;
            InitForm();
        }

        public MainNursingMeasure(string noofinpat)
            : this()
        {
            CurrentNoofinpat = noofinpat;
            dataLoader.m_currentDate = DateTime.Now;//初始化日期默认今天

        }

        private WaitDialogForm m_WaitDialog;

        public string CurrentPat
        {
            get
            {
                return _currentPat;
            }
            set
            {
                _currentPat = value;
                InitMeasureTableData();
            }
        }

        private string _currentPat = null;

        internal IEmrHost App
        {
            get { return m_app; }
        }
        private IEmrHost m_app;


        public UC_ImageBoard ImageBoard
        {
            get
            {
                return this.uC_ImageBoard1;
            }
        }

        #region 事件&&函数

        private void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog == null)
                m_WaitDialog = new WaitDialogForm("正在加载数据......", "请您稍后！");
            if (!m_WaitDialog.Visible)
                m_WaitDialog.Visible = true;
            m_WaitDialog.Caption = caption;

        }

        private void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }


        private void InitForm()
        {
            AddEvents();
            this.Text = "三测表";
            //xll 2013-05-24 注释该方法
            //SetNewButtonEnable();
            simpleButtonNew.Enabled = !ReadOnlyControl;
        }

        private void AddEvents()
        {
            this.simpleButtonNew.Click += new EventHandler(simpleButtonNew_Click);//新增
            this.simpleButtonFirstWeek.Click += new EventHandler(simpleButtonFirstWeek_Click);//第一周
            this.simpleButtonLastWeek.Click += new EventHandler(simpleButtonLastWeek_Click);//上一周
            this.simpleButtonNextWeek.Click += new EventHandler(simpleButtonNextWeek_Click);//下一周
            this.simpleButtonThisWeek.Click += new EventHandler(simpleButtonThisWeek_Click);//本周
            this.simpleButtonRefresh.Click += new EventHandler(simpleButtonRefresh_Click);//更新
            this.simpleButtonPrint.Click += new EventHandler(simpleButtonPrint_Click);//打印
            this.simpleButtonPrintBat.Click += new EventHandler(simpleButtonPrintBat_Click);//批量打印
        }

        void simpleButtonPrintBat_Click(object sender, EventArgs e)
        {
            uC_ImageBoard1.PrintAllDocument(decimal.Parse(CurrentPat), dataLoader);
        }

        /// <summary>
        /// 初始化三测表中的数据
        /// </summary>
        private void InitMeasureTableData()
        {
            if (CurrentPat != null)
            {
                SetWaitDialogCaption("正在读取患者信息");
                //DataTable patientInfo = MethodSet.GetPatientInfoForThreeMeasureTable(CurrentPat.NoOfFirstPage);
                SetWaitDialogCaption("正在绘制三测单");
                HideWaitDialog();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void simpleButtonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                // m_currentDate = DateTime.Now;//zyx 2013-01-22 刷新时刷新当前选中的周数据
                LoadDataImage(decimal.Parse(CurrentPat));
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }

        }



        /// <summary>
        /// 本周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonThisWeek_Click(object sender, EventArgs e)
        {
            //dataLoader.m_currentDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-DD HH:mm:ss"));
            dataLoader.m_currentDate = DateTime.Now;

            LoadDataImage(decimal.Parse(CurrentPat));
            this.simpleButtonNextWeek.Enabled = false;
            if (DateTime.Parse(dataLoader.m_currentDate.AddDays(-7).ToString("yyyy-MM-dd")) < DateTime.Parse(DateTime.Parse(m_InTime).ToString("yyyy-MM-dd")))
            {
                this.simpleButtonLastWeek.Enabled = false;
            }
            else
            {
                this.simpleButtonLastWeek.Enabled = true;
            }

        }

        /// <summary>
        /// 下周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonNextWeek_Click(object sender, EventArgs e)
        {
            this.simpleButtonLastWeek.Enabled = true;
            this.simpleButtonNextWeek.Enabled = true;

            dataLoader.m_currentDate = dataLoader.m_currentDate.AddDays(7);
            if (DateTime.Parse(dataLoader.m_currentDate.AddDays(7).ToString("yyyy-MM-dd")) > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                this.simpleButtonNextWeek.Enabled = false;
            LoadDataImage(decimal.Parse(CurrentPat));
        }

        /// <summary>
        /// 上周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonLastWeek_Click(object sender, EventArgs e)
        {
            this.simpleButtonLastWeek.Enabled = true;
            this.simpleButtonNextWeek.Enabled = true;

            dataLoader.m_currentDate = dataLoader.m_currentDate.AddDays(-7);
            if (DateTime.Parse(dataLoader.m_currentDate.AddDays(-7).ToString("yyyy-MM-dd")) < DateTime.Parse(DateTime.Parse(m_InTime).ToString("yyyy-MM-dd")))
                this.simpleButtonLastWeek.Enabled = false;
            LoadDataImage(decimal.Parse(CurrentPat));
        }

        /// <summary>
        /// 第一周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonFirstWeek_Click(object sender, EventArgs e)
        {
            this.simpleButtonLastWeek.Enabled = false;//选择第一周的时候“上一周”的按钮不可用
            if (DateTime.Parse((DateTime.Parse(m_InTime)).AddDays(7).ToString("yyyy-MM-dd")) > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
            {
                this.simpleButtonNextWeek.Enabled = false;
            }
            else
            {
                this.simpleButtonNextWeek.Enabled = true;
            }


            dataLoader.m_currentDate = DateTime.Parse(m_InTime);

            LoadDataImage(decimal.Parse(CurrentPat));
        }

        public EventHandler eventHandlerXieRu;

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonNew_Click(object sender, EventArgs e)
        {
            if (eventHandlerXieRu != null)
            {
                eventHandlerXieRu(sender, e);
            }
            else
            {
                string version = DrectSoft.Core.NurseDocument.ConfigInfo.GetNurseMeasureVersion(decimal.Parse(CurrentPat));
                Assembly a = Assembly.Load("DrectSoft.Core.NurseDocument");
                Type type = a.GetType(version);
                Form form = null;
                form = (Form)Activator.CreateInstance(type, new object[] { m_app, CurrentPat });
                form.Height = DrectSoft.Core.NurseDocument.ConfigInfo.GetNurseRecordSize(decimal.Parse(CurrentPat));
                form.ShowDialog();
                LoadDataImage(decimal.Parse(CurrentPat));
                form.Dispose();
            }
        }


        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonPrint_Click(object sender, EventArgs e)
        {
            uC_ImageBoard1.PrintDocument(decimal.Parse(CurrentPat), dataLoader);
        }

        #endregion

        #region IEMREditor 成员

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
                InitForm();
                MethodSet.App = m_app;
                CurrentPat = CurrentNoofinpat;
                InitBtnStatus();
                LoadDataImage(decimal.Parse(CurrentPat));

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 整体录入时调用病人
        /// </summary>
        /// <param name="app"></param>
        /// <param name="currInpat"></param>
        public new void Load(IEmrHost app, Inpatient currInpat)
        {
            try
            {
                if (app == null) return;
                m_app = app;
                InitForm();
                MethodSet.App = m_app;
                CurrentPat = currInpat.NoOfFirstPage.ToString();
                dataLoader.m_currentDate = DateTime.Now;
                InitBtnStatus();
                LoadDataImage(decimal.Parse(CurrentPat));

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void Save()
        {

        }

        public string Title
        {
            get { return "三测单曲线"; }
        }

        public bool ReadOnlyControl
        {
            get { return m_ReadOnlyControl; }
            set { m_ReadOnlyControl = value; }
        }
        private bool m_ReadOnlyControl = false;
        #endregion


        public void Print()
        {
            //throw new NotImplementedException();
        }

        public string CurrentNoofinpat
        {
            get;
            set;
        }

        private void MainNursingMeasure_Load(object sender, EventArgs e)
        {

        }

        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkShowTempareture.Checked)
                    m_showTempareture = true;
                else m_showTempareture = false;
                if (chkShowPulse.Checked)
                    m_showPulse = true;
                else m_showPulse = false;
                if (chkShowBreath.Checked)
                    m_showBreath = true;
                else m_showBreath = false;

                LoadDataImage(decimal.Parse(CurrentPat));
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void InitBtnStatus()
        {
            try
            {
                DataTable dt = dataLoader.GetPatData(CurrentPat);
                if (dt.Rows[0]["inwarddate"].ToString() != "" && dt.Rows[0]["inwarddate"].ToString() != null)
                {
                    m_InTime = dt.Rows[0]["inwarddate"].ToString();
                }
                else if (dt.Rows[0]["ADMITDATE"].ToString() != null && dt.Rows[0]["ADMITDATE"].ToString() != "")
                {
                    m_InTime = dt.Rows[0]["ADMITDATE"].ToString();
                }
                else
                {
                    throw new Exception("入院和入科时间不能同时为空");
                }

                if (dataLoader.m_currentDate.AddDays(7) > DateTime.Now)
                {
                    simpleButtonNextWeek.Enabled = false;
                }
                if (dataLoader.m_currentDate.AddDays(-7) < DateTime.Parse(m_InTime))
                {
                    this.simpleButtonLastWeek.Enabled = false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 创建数据图片
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public void LoadDataImage(decimal noOfInpat)
        {
            try
            {
                dataLoader.CurrentPat = noOfInpat;
                dataLoader.GetPatientInfoForThreeMeasureTable(noOfInpat);
                ThreeMeasureDrawHepler threeMeasureDrawHepler = new ThreeMeasureDrawHepler(noOfInpat, dataLoader);
                Size size = ConfigInfo.GetImagePageBound();
                Bitmap _dataImage = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb); //用于绘制数据表单
                Graphics g = Graphics.FromImage(_dataImage);

                threeMeasureDrawHepler.DrawDataImage(g);
                ConfigInfo.dataIamgeSize = size;
                uC_ImageBoard1.DataImage = _dataImage;
                g.Save();
                g.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Bitmap GetDataImage(decimal noOfInpat)
        {
            try
            {
                dataLoader.CurrentPat = noOfInpat;
                //dataLoader.GetPatientInfoForThreeMeasureTable(noOfInpat);
                ThreeMeasureDrawHepler threeMeasureDrawHepler = new ThreeMeasureDrawHepler(noOfInpat, dataLoader);

                Size size = ConfigInfo.GetImagePageBound();
                Bitmap _dataImage = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb); //用于绘制数据表单
                Graphics g = Graphics.FromImage(_dataImage);
                threeMeasureDrawHepler.DrawDataImage(g);
                ConfigInfo.dataIamgeSize = size;
                g.Save();
                g.Dispose();
                return _dataImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnPrintHistory_Click(object sender, EventArgs e)
        {
            try
            {
                PrintHistoryForm printHistoryForm = new PrintHistoryForm(dataLoader.CurrentPat.ToString(), "2");
                printHistoryForm.ShowDialog();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }
}
