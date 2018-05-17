using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Core.NursingDocuments.PublicSet;
using DrectSoft.Common.Eop;
using DevExpress.Utils;

namespace DrectSoft.Core.NursingDocuments
{
    public partial class MainNursingMeasure : DevExpress.XtraEditors.XtraUserControl, IEMREditor
    {
        //xll 2012-10-10 录入事件 整体录入时使用
        public EventHandler eventHandlerDaoRu;
        public MainNursingMeasure(IEmrHost app)
        {
            m_app = app;
            PublicSet.MethodSet.App = app;
            InitForm();
            this.ActiveControl = this.simpleButtonNew;
        }

        public MainNursingMeasure(string noofinpat)
        {
            CurrentNoofinpat = noofinpat;
            InitializeComponent();
        }

        public MainNursingMeasure()
        {
            InitializeComponent();
        }

        private WaitDialogForm m_WaitDialog;

        public Inpatient CurrentPat
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
        private Inpatient _currentPat = null;

        internal IEmrHost App
        {
            get { return m_app; }
        }
        private IEmrHost m_app;

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
            SetNewButtonEnable();
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
            ucThreeMeasureTable.PrintAllDocument();
        }

        /// <summary>
        /// 初始化三测表中的数据
        /// </summary>
        public void InitMeasureTableData()
        {
            if (CurrentPat != null)
            {
                SetWaitDialogCaption("正在读取患者信息");

                //MethodSet.CurrentInPatient = CurrentPat;
                DataTable patientInfo = PublicSet.MethodSet.GetPatientInfoForThreeMeasureTable(CurrentPat.NoOfFirstPage);
                SetWaitDialogCaption("正在绘制三测单");
                this.ucThreeMeasureTable.SetPatientInfo(patientInfo);
                this.ucThreeMeasureTable.LoadData();
                HideWaitDialog();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonRefresh_Click(object sender, EventArgs e)
        {
            this.ucThreeMeasureTable.LoadData();
            ucThreeMeasureTable.Refresh();
        }

        /// <summary>
        /// 本周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonThisWeek_Click(object sender, EventArgs e)
        {
            this.ucThreeMeasureTable.SetAllocateDateTime(DateTime.Now);
            SetButtonEnable();

            ucThreeMeasureTable.LoadData();
            ucThreeMeasureTable.Refresh();
        }

        /// <summary>
        /// 下周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonNextWeek_Click(object sender, EventArgs e)
        {
            this.ucThreeMeasureTable.SetAllocateDateTime(7);
            this.simpleButtonLastWeek.Enabled = true;

            this.ucThreeMeasureTable.LoadData();
            ucThreeMeasureTable.Refresh();
        }

        /// <summary>
        /// 上周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonLastWeek_Click(object sender, EventArgs e)
        {
            this.ucThreeMeasureTable.SetAllocateDateTime(-7);
            SetButtonEnable();

            this.ucThreeMeasureTable.LoadData();
            ucThreeMeasureTable.Refresh();
        }

        /// <summary>
        /// 第一周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonFirstWeek_Click(object sender, EventArgs e)
        {
            this.ucThreeMeasureTable.SetAllocateDateTime();//入院的那一周，即第一周
            this.simpleButtonLastWeek.Enabled = false;//选择第一周的时候“上一周”的按钮不可用

            this.ucThreeMeasureTable.LoadData();
            ucThreeMeasureTable.Refresh();
        }



        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonNew_Click(object sender, EventArgs e)
        {
            //xll 2012-10-10 三测单整体录入时处理 
            if (eventHandlerDaoRu != null)
            {
                eventHandlerDaoRu(sender, e);
            }
            else
            {
                NursingRecord nursingRecord = new NursingRecord(m_app, CurrentPat);
                nursingRecord.ShowDialog();
                this.ucThreeMeasureTable.LoadData();
                this.ucThreeMeasureTable.Refresh();
            }
        }


        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonPrint_Click(object sender, EventArgs e)
        {
            ucThreeMeasureTable.PrintDocument();
        }

        private void SetButtonEnable()
        {
            this.simpleButtonLastWeek.Enabled = this.ucThreeMeasureTable.DateTimeLogicForLastWeek();
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
                m_app = app;
                PublicSet.MethodSet.App = app;
                CurrentPat = m_app.CurrentPatientInfo;
                InitForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }


        /// <summary>
        /// xll 2013-01-10
        /// </summary>
        /// <param name="app"></param>
        /// <param name="CurrentPatientInfo"></param>
        public  void Load(IEmrHost app, Inpatient CurrentPatientInfo)
        {
            try
            {
                m_app = app;
                PublicSet.MethodSet.App = app;
                CurrentPat = CurrentPatientInfo;
                InitForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        public new void Load(IEmrHost app, bool firstPageEditFlag)
        {
            try
            {

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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

        private void MainNursingMeasure_SizeChanged(object sender, EventArgs e)
        {
            ReSetUCLocaton();
        }

        /// <summary>
        /// 改变UserControl的位置
        /// </summary>
        private void ReSetUCLocaton()
        {
            Int32 pointX = (this.Width - this.ucThreeMeasureTable.Width) / 2;
            Int32 pointY = ucThreeMeasureTable.Location.Y;
            this.ucThreeMeasureTable.Location = new Point(pointX, pointY);
        }

        private void SetNewButtonEnable()
        {
            Employee emp = new Employee(m_app.User.Id);
            emp.ReInitializeProperties();
            if (emp.Kind == EmployeeKind.Nurse)//当前登录人是护士
            {
                simpleButtonNew.Enabled = true;
            }
            else
            {
                simpleButtonNew.Enabled = false;
            }
        }


        #region IEMREditor 成员

        public void Print()
        {
            ucThreeMeasureTable.PrintDocument();
        }

        public string CurrentNoofinpat
        {
            get;
            set;
        }

        #endregion
    }
}