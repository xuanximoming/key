using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using DrectSoft.Emr.Util;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 仁和版本修改
    /// </summary>
    public partial class QualityMedicalRecord : DevExpress.XtraEditors.XtraUserControl
    {
        #region 属性、字段
        private const string m_AllPatientList = @"  SELECT inpatient.outhosdept deptno, department.name deptname, inpatient.patid, inpatient.name, inpatient.noofinpat
                                                    FROM inpatient 
                                                    LEFT OUTER JOIN department ON department.id = inpatient.outhosdept
                                                    WHERE inpatient.outhosdept like '%{0}%' 
                                                        AND inpatient.patid like '%{1}%'
                                                        AND inpatient.name like '%{2}%'
                                                        AND inpatient.status like '%{3}%' ";

        IEmrHost m_App;
        /// <summary>
        /// 病历时限窗体
        /// </summary>
        //UserCtrlTimeQcInfo m_UserTimeQcInfo;

        /// <summary>
        /// 病历内容窗体
        /// </summary>
        UCEmrInput m_UCEmrInput;

        /// <summary>
        /// 界面中调用的等待窗体
        /// </summary>
        private WaitDialogForm m_WaitDialog;

        /// <summary>
        /// 标识当前登录人的身份 QCDepart质控科人员  QCMANAGER 为科室指控员 CHIEF为科主任
        /// </summary>
        public string USERINDENTY;
        SqlManger m_SqlManger;

        private bool m_IsReLoadEmrContent = true;

        private string m_deptNo = string.Empty;
        private string m_patID = string.Empty;
        private string m_name = string.Empty;
        private string m_beginInTime = string.Empty;
        private string m_endInTime = string.Empty;
        private string m_status = string.Empty;


        private string noofInpat = "";
        #endregion

        #region 方法
        public QualityMedicalRecord(IEmrHost app)
        {
            InitializeComponent();
            m_App = app;
            m_SqlManger = new SqlManger(app);
            ucPoint1.EventHandlerTongGuo += EventHandlerTongGuo_UcPoint;
        }

        private void EventHandlerTongGuo_UcPoint(object sender, EventArgs e)
        {
            SetTiXiugai();
        }

        //private void AddQCForm()
        //{
        //    m_UserTimeQcInfo = new UserCtrlTimeQcInfo();
        //    xtraTabPageTimeQC.Controls.Add(m_UserTimeQcInfo);
        //    m_UserTimeQcInfo.Dock = DockStyle.Fill;
        //    m_UserTimeQcInfo.HideGridHeader();
        //}

        private void AddEmrInput()
        {
            m_UCEmrInput = new UCEmrInput();
            m_UCEmrInput.CurrentInpatient = null;
            m_UCEmrInput.HideBar();
            RecordDal m_RecordDal = new RecordDal(m_App.SqlHelper);
            m_UCEmrInput.SetInnerVar(m_App, m_RecordDal);
            xtraTabPageEmrContent.Controls.Add(m_UCEmrInput);
            m_UCEmrInput.Dock = DockStyle.Fill;
        }

        private void Search()
        {
            SetVariable();
            //加载病历列表
            //SetPatientList();
            //加载科室统计信息
            SetAllDepartmentStatInfo();
            xtraTabControlEmrInfo.SelectedTabPage = xtraTabPageAllDepartment;
        }

        private void SetVariable()
        {
            m_deptNo = lookUpEditorDepartment.CodeValue.Trim();
            m_patID = textEditPatID.Text.Trim();
            m_name = textEditName.Text.Trim();
            m_beginInTime = Convert.ToDateTime(dateEditBeginInTime.EditValue).ToString("yyyy-MM-dd HH:mm:ss").Trim();
            m_endInTime = Convert.ToDateTime(dateEditEndInTime.EditValue).ToString("yyyy-MM-dd HH:mm:ss").Trim();
            m_status = lookUpEditorStatus.CodeValue.Trim();

            textEditPatientBedNO.Text = "";
            textEditPatientID.Text = "";
            textEditPatientName.Text = "";
        }
        #region 病历内容
        private void LoadEmrContent(string noofInpat)
        {
            //DataRowView drv = gridViewSelect.GetRow(gridViewSelect.FocusedRowHandle) as DataRowView;
            //if (drv == null) return;

            SetWaitDialogCaption("正在加载病历信息！");
            //m_App.ChoosePatient(Convert.ToDecimal(drv["noofinpat"]));
            //m_UCEmrInput.PatientChanged();
            //m_UCEmrInput.PatientChangedByIEmrHost(Convert.ToDecimal(drv["noofinpat"]));
            m_UCEmrInput.PatientChangedByIEmrHost(Convert.ToDecimal(noofInpat));
            m_UCEmrInput.HideBar();
            m_IsReLoadEmrContent = false;
        }
        #endregion
        private void SetPatientList(string departNO)
        {
            m_WaitDialog = new WaitDialogForm("正在加载病人列表！", "请您稍后！");
            SetWaitDialogCaption();
            DataTable dt = m_SqlManger.GetDepartmentPatStatInfo(m_deptNo, m_patID, m_name, m_status, m_beginInTime, m_endInTime, USERINDENTY);
            gridControlPatientList.BeginUpdate();
            gridControlPatientList.DataSource = dt;
            gridControlPatientList.EndUpdate();
            HideWaitDialog();
        }
        /// <summary>
        /// 根据系统配置控制科室下拉框的可编辑性
        /// add by ywk 
        /// 当前登录人存在与配置中的角色，就可选择科室，不存在只让科室选中当前科室且不可编辑
        /// </summary>
        private void SetLookDeptEditor()
        {
            //string configvalue = m_SqlManger.GetConfigValueByKey("ShowAllDeptQuality");
            //查找仁和医院质控科人员ID

            if (USERINDENTY == "QCDepart")
            {
                lookUpEditorDepartment.Enabled = true;//科室可选择
            }
            else
            {
                lookUpEditorDepartment.Enabled = false;//科室不可选择 为确定的
            }
        }

        /// <summary>
        /// 初始化病历评分界面
        /// </summary>
        private void InitUCPoint()
        {
            ucPoint1.InitData(m_App, m_SqlManger);
        }
        #region 初始化界面

        private void RegisterEvent()
        {
            gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridView1_FocusedRowChanged);
            xtraTabControlEmrInfo.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(xtraTabControlEmrInfo_SelectedPageChanged);
        }

        private void InitInTime()
        {
            //默认为显示一月内的数据
            dateEditBeginInTime.EditValue = System.DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            dateEditEndInTime.EditValue = System.DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 初始化科室下拉框
        /// </summary>
        private void InitDepartment()
        {

            lookUpWindowDepartment.SqlHelper = m_App.SqlHelper;

            string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid ");
            DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

            Dept.Columns["ID"].Caption = "科室代码";
            Dept.Columns["NAME"].Caption = "科室名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 65);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorDepartment.SqlWordbook = deptWordBook;

            lookUpEditorDepartment.CodeValue = m_App.User.CurrentDeptId;
        }

        /// <summary>
        /// 初始化病人状态下拉框
        /// 【仁和版本需求修改】
        /// 科室质控员身份登录是看到**病区出院和病人出院
        /// 质控科人员是可看到**病区出院、病人出院、病区分床状态
        /// </summary>
        private void InitStatus()
        {
            lookUpWindowStatus.SqlHelper = m_App.SqlHelper;
            string sql = string.Empty;
            string Uidentity = m_SqlManger.JudgeIdentity(m_App.User.Id, m_SqlManger);//判断当前登录的人是科室质控员还是质控科的
            USERINDENTY = Uidentity;//赋给公公共变量 

            if (Uidentity == "QCDepart")//为空说明是质控科的
            {
                sql = string.Format(@"select c.id, c.name from categorydetail c 
                                     where c.categoryid = '15' and c.id in 
                                    (select distinct status from inpatient) and c.id in ('1501','1502','1503')");
            }
            else//不为空表明科室质控员或者科室主任
            {
                sql = string.Format(@"select c.id, c.name from categorydetail c 
                                     where c.categoryid = '15' and c.id in 
                                    (select distinct status from inpatient) and c.id in ('1502','1503')");
            }
            DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);
            Dept.Columns["ID"].Caption = "状态代码";
            Dept.Columns["NAME"].Caption = "状态名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("ID", 65);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name");
            lookUpEditorStatus.SqlWordbook = deptWordBook;
        }
        #endregion

        #region 等待页面相关
        public void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }

        }

        public void SetWaitDialogCaption()
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
            }
        }

        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }
        #endregion

        #region 病历评分
        /// <summary>
        /// 双击选择的病人跳转到病历评分窗体（用户控件）
        /// </summary>
        private void LoadEmrDocPoint(string noOfInpat)
        {
            // DataRowView drv = gridViewSelect.GetRow(gridViewSelect.FocusedRowHandle) as DataRowView;
            // if (drv == null) return;

            // string noofinpat = drv["noofinpat"].ToString();

            if (m_UCEmrInput != null)
            {
                EmrModel emrModel = m_UCEmrInput.GetCurrentModel();
                EmrModelContainer emrModelContainer = m_UCEmrInput.GetCurrentModelContainer();
                ucPoint1.RefreshLookUpEditorEmrDoc(noOfInpat, emrModel, emrModelContainer);
            }
            else
            {
                ucPoint1.RefreshLookUpEditorEmrDoc(noOfInpat, null, null);
            }
        }
        #endregion

        #region 科室统计信息

        /// <summary>
        /// 得到科室统计信息
        /// </summary>
        /// <returns></returns>
        private void SetAllDepartmentStatInfo()
        {
            //解决用线程时出现红叉的问题
            AsyncDelegate asydele = new AsyncDelegate(BindAllDepartmentStatInfo);
            gridControlAllDepartmentStatInfo.BeginInvoke(asydele);
        }
        /// <summary>
        /// 进入质控系统的主窗体的所得病人信息 
        /// </summary>
        private void BindAllDepartmentStatInfo()
        {
            m_WaitDialog = new WaitDialogForm("正在加载科室统计信息！", "请您稍后！");
            SetWaitDialogCaption();
            DataTable dt = m_SqlManger.GetAllDepartmentStatInfo(m_deptNo, m_patID, m_name, m_status, m_beginInTime, m_endInTime, USERINDENTY);
            gridControlAllDepartmentStatInfo.BeginUpdate();
            gridControlAllDepartmentStatInfo.DataSource = dt;
            gridControlAllDepartmentStatInfo.EndUpdate();
            HideWaitDialog();
        }

        delegate void AsyncDelegate();

        #endregion

        #region 科室病人信息
        /// <summary>
        /// 双击科室统计信息列表，进入病人列表
        /// </summary>
        private void LoadDepartmentPatStatInfo()
        {
            int rowIndex = gridViewMain.FocusedRowHandle;
            DataTable dt = gridControlAllDepartmentStatInfo.DataSource as DataTable;
            if (dt != null)
            {
                DataRowView drv = gridViewMain.GetRow(rowIndex) as DataRowView;
                string deptNO = drv["DEPTNO"].ToString();
                SetDepartmentPatStatInfo(deptNO);
                SetPatientList(deptNO);
            }

        }
        /// <summary>
        /// 双击科室统计信息列表，根据科室编号捞取病人列表
        /// </summary>
        /// <param name="deptNO"></param>
        private void SetDepartmentPatStatInfo(string deptNO)
        {
            if (deptNO != "")
            {
                m_deptSelect = deptNO;
                //解决用线程时出现红叉的问题
                AsyncDelegate asydele = new AsyncDelegate(BindDepartmentPatStatInfo);
                gridControlDepartmentPatStatInfo.BeginInvoke(asydele);
            }
        }
        private int SumPoint { get; set; }//满分值。通过配置中取得 ywk 2012年6月12日 14:43:29 
        private string m_deptSelect = string.Empty;
        /// <summary>
        /// 双击的科室统计信息，捞取的病人统计信息 
        /// </summary>
        private void BindDepartmentPatStatInfo()
        {
            m_WaitDialog = new WaitDialogForm("正在加载病人统计信息！", "请您稍后！");
            SetWaitDialogCaption();
            //新加个参数，计算总分
            //SumPoint = Int32.Parse(m_SqlManger.GetConfigValueByKey("EmrPointConfig"));
            DataTable dt = m_SqlManger.GetDepartmentPatStatInfo(m_deptSelect, m_patID, m_name, m_status, m_beginInTime, m_endInTime, USERINDENTY);
            gridControlDepartmentPatStatInfo.BeginUpdate();
            gridControlDepartmentPatStatInfo.DataSource = dt;
            gridControlDepartmentPatStatInfo.EndUpdate();
            m_deptSelect = "";
            HideWaitDialog();
        }

        #endregion

        private void SetTiXingTimer()
        {
            System.Windows.Forms.Timer timerTiXing = new System.Windows.Forms.Timer();
            timerTiXing.Interval = 100000; //100秒
            timerTiXing.Tick += new EventHandler(timerTiXing_Tick);
            timerTiXing.Start();
        }

        void timerTiXing_Tick(object sender, EventArgs e)
        {
            SetTiXiugai();
        }

        public void SetTiXiugai()
        {
            DataTable dtPer = m_SqlManger.SetTiXiugai(USERINDENTY);
            gridControlTiXing.DataSource = dtPer;
        }


        #endregion

        #region 事件
        #region 科室病人信息筛选

        private void textEditPatientBedNO_EditValueChanged(object sender, EventArgs e)
        {
            PatientFilterInGridControl();
        }

        private void textEditPatientID_EditValueChanged(object sender, EventArgs e)
        {
            PatientFilterInGridControl();
        }

        private void textEditPatientName_EditValueChanged(object sender, EventArgs e)
        {
            PatientFilterInGridControl();
        }

        /// <summary>
        /// 病人数据筛选
        /// </summary>
        private void PatientFilterInGridControl()
        {
            string bedNo = textEditPatientBedNO.Text.Trim();
            string patientID = textEditPatientID.Text.Trim();
            string patientName = textEditPatientName.Text.Trim();

            DataTable dt = gridControlDepartmentPatStatInfo.DataSource as DataTable;
            if (dt != null)
            {
                string rowFilter = " BEDID like '%{0}%' and PATID like '%{1}%' and PATNAME like '%{2}%' ";
                dt.DefaultView.RowFilter = string.Format(rowFilter, bedNo, patientID, patientName);
            }
        }

        #endregion
        /// <summary>
        /// 双击科室统计信息进入科室病人信息
        /// 原来版本的是捞取已经对病历有评分的病人信息
        /// 此处捞取按照要求查出的病人信息，即统计信息的病人数量会和科室病人信息的一致
        /// edit by ywk 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMain_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo hitInfo = gridViewMain.CalcHitInfo(gridControlAllDepartmentStatInfo.PointToClient(Cursor.Position));
            if (hitInfo.RowHandle >= 0)
            {
                LoadDepartmentPatStatInfo();
            }
            xtraTabControlEmrInfo.SelectedTabPage = xtraTabPageDepartment;
        }

        private void gridViewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
        }
        /// <summary>
        /// 双击科室病人信息列表操作 跳转到病历评分界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDepartment_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gridViewDepartment.GetFocusedDataRow();
            if (dr != null)
            {
                string noOfInpat = dr["NOOFINPAT"].ToString();
                DataTable dt = gridControlPatientList.DataSource as DataTable;
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRowView drv = gridView1.GetRow(i) as DataRowView;
                        if (noOfInpat == drv["NOOFINPAT"].ToString())
                        {
                            gridView1.FocusedRowHandle = i;
                            //双击病人信息跳转到病历评分页面 edit by ywk 2012年3月31日11:48:21 
                            xtraTabControlEmrInfo.SelectedTabPage = xtraTabPagePoint;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QualityMedicalRecord_Load(object sender, EventArgs e)
        {
            InitUCPoint();
            RegisterEvent();
            InitDepartment();
            InitStatus();
            //AddQCForm();
            AddEmrInput();
            InitInTime();
            SetLookDeptEditor();
            Search();

            SetTiXingTimer();
            SetTiXiugai();
        }
        /// <summary>
        ///  查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
        #region 切换病人
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GridView gridViewSelect = sender as GridView;
            LoadPatientEmrInfo(gridViewSelect);
        }
        private void LoadPatientEmrInfo(GridView gridViewSelected)
        {
            try
            {
                //GridHitInfo hitInfo = gridView1.CalcHitInfo(gridControlPatientList.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)));
                //if (hitInfo.RowHandle < 0) return;

                if (gridViewSelected.FocusedRowHandle < 0) return;

                SetWaitDialogCaption("正在切换病人！");
                //m_UserTimeQcInfo.App = m_App;
                DataRowView drv = gridViewSelected.GetRow(gridViewSelected.FocusedRowHandle) as DataRowView;
                noofInpat = drv["noofinpat"].ToString();
                //加载时限信息
                SetWaitDialogCaption("正在加载时限信息！");
                //m_UserTimeQcInfo.CheckPatientTime(Convert.ToInt32(drv["noofinpat"]));
                //m_UserTimeQcInfo.HideGridViewGroup();

                //加载病历内容
                if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPageEmrContent)
                {
                    LoadEmrContent(noofInpat);
                }
                else if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPagePoint)
                {
                    LoadEmrDocPoint(noofInpat);
                }
                else
                {
                    xtraTabControlEmrInfo.SelectedTabPage = xtraTabPagePoint;
                    //LoadEmrDocPoint(drv["noofinpat"].ToString());
                    //m_IsReLoadEmrContent = true;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                HideWaitDialog();
            }
        }
        #endregion

        #region 切换页面
        /// <summary>
        /// TAB页面的切换操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControlEmrInfo_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;
            if (string.IsNullOrEmpty(noofInpat)) return;
            if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPageEmrContent && m_IsReLoadEmrContent == true)
            {
                LoadEmrContent(noofInpat);
                HideWaitDialog();
            }
            //else if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPageDepartment)
            //{
            //    LoadDepartmentPatStatInfo();
            //}
            else if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPagePoint)
            {
                LoadEmrDocPoint(noofInpat);
            }
        }
        #endregion

        #region 收缩 展开 所有节点
        private void simpleButtonShouSuo_Click(object sender, EventArgs e)
        {
            //收缩所有节点
            gridView1.CollapseAllGroups();
        }

        private void simpleButtonZhanKai_Click(object sender, EventArgs e)
        {
            //展开所有节点
            gridView1.ExpandAllGroups();
        }
        #endregion

        private void gridControlPatientList_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void gridViewChangeInpat_DoubleClick(object sender, EventArgs e)
        {

        }

        private void gridControlTiXing_Click(object sender, EventArgs e)
        {

        }

        private void navBarGroupTiXin_ItemChanged(object sender, EventArgs e)
        {
        }
    }
}
