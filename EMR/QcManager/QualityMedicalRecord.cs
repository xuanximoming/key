using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Service;
using DrectSoft.Emr.Util;
namespace DrectSoft.Emr.QcManager
{
    // add by wyt 2012-12-06
    public enum Authority
    {
        //科室质控员
        DEPTMANAGER, //科主任
        QC,
        DEPTQC//质控科质控员
    }
    public enum CheckState
    {

        NEW,            //新建
        SUBMIT,         //提交未审核
        CHECKIN,        //审核通过
        CHECKOUT,       //审核未通过
        QC,//质控员质控
        DeptQc//科室质控
    }
    public enum QCType
    {

        PART,           //环节质控
        FINAL,          //终末质控
        Dept
    }
    public partial class QualityMedicalRecord : DevExpress.XtraEditors.XtraUserControl
    {
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
        /// 病历内容窗体 --- 逐步弃用 edit by cyq 2013-04-27
        /// </summary>
        UCEmrInput m_UCEmrInput;

        /// <summary>
        /// 病历内容窗体 --- 新版文书录入 add by cyq 2013-04-27
        /// </summary>
        DrectSoft.Core.MainEmrPad.New.UCEmrInput m_UCEmrInputNew;

        /// <summary>
        /// 界面中调用的等待窗体
        /// </summary>
        private WaitDialogForm m_WaitDialog;

        SqlManger m_SqlManger;

        private bool m_IsReLoadEmrContent = true;

        private string m_deptNo = string.Empty;
        private string m_patID = string.Empty;
        private string m_name = string.Empty;
        private string m_beginInTime = string.Empty;
        private string m_endInTime = string.Empty;
        private string m_status = string.Empty;
        private string m_sortid = string.Empty;
        //add by wyt 2012-12-06
        /// <summary>
        /// 质控权限
        /// </summary>
        private Authority m_auth = Authority.DEPTQC;
        /// <summary>
        /// 记录状态
        /// </summary>
        private CheckState m_check = CheckState.NEW;
        private bool isDeptQc;

        public bool IsDeptQc
        {
            get { return isDeptQc; }
            set { isDeptQc = value; }
        }
        /// <summary>
        /// 质控类型
        /// </summary>
        private QCType m_qctype = QCType.FINAL;
        /// <summary>
        /// 选中的病人首页序号
        /// </summary>
        string m_noofinpat = "";
        /// <summary>
        /// 选中的病人姓名
        /// </summary>
        string m_patientname = "";
        /// <summary>
        /// 选中的病人状态
        /// </summary>
        string m_patientstatus = "";
        /// <summary>
        /// 自动评分主表记录ID
        /// </summary>
        string m_autoID = string.Empty;
        /// <summary>
        /// 自动评分主表记录ID
        /// </summary>
        string m_chiefID = string.Empty;

        /// <summary>
        /// <auth>Modify by xlb</auth>
        /// <date>2013-06-18</date>
        /// </summary>
        /// <param name="app"></param>
        public QualityMedicalRecord(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_App = app;
                m_SqlManger = new SqlManger(app);
                //Add by xlb 2013-06-18
                DS_Common.CancelMenu(panelControl1, new ContextMenuStrip());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void AddQCForm()
        //{
        //    try
        //    {
        //        m_UserTimeQcInfo = new UserCtrlTimeQcInfo();
        //        xtraTabPageTimeQC.Controls.Add(m_UserTimeQcInfo);
        //        m_UserTimeQcInfo.Dock = DockStyle.Fill;
        //        m_UserTimeQcInfo.HideGridHeader();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private void AddEmrInput()
        {
            try
            {
                m_UCEmrInput = new UCEmrInput();
                m_UCEmrInput.CurrentInpatient = null;
                m_UCEmrInput.HideBar();
                RecordDal m_RecordDal = new RecordDal(m_App.SqlHelper);
                m_UCEmrInput.SetInnerVar(m_App, m_RecordDal);
                xtraTabPageEmrContent.Controls.Add(m_UCEmrInput);
                m_UCEmrInput.Dock = DockStyle.Fill;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 查询

        /// <summary>
        /// 查询功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void Search()
        {
            try
            {
                SetVariable();

                //加载病历列表
                SetPatientList();

                //加载科室统计信息
                SetAllDepartmentStatInfo();
                //加载自动评分记录
                SetAutoMarkInfo();

                if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPage1)
                {
                    xtraTabControlEmrInfo.SelectedTabPage = xtraTabPage1;
                }
                else
                {
                    xtraTabControlEmrInfo.SelectedTabPage = xtraTabPageAllDepartment;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SetVariable()
        {
            try
            {
                m_deptNo = lookUpEditorDepartment.CodeValue.Trim();
                if (lookUpEditorDepartment.Enabled == false)
                {
                    m_deptNo = m_App.User.CurrentDeptId;
                }
                m_patID = textEditPatID.Text.Trim();
                m_name = textEditName.Text.Trim();
                m_beginInTime = Convert.ToDateTime(dateEditBeginInTime.EditValue).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                m_endInTime = Convert.ToDateTime(dateEditEndInTime.EditValue).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                m_status = GetLockStatusValue(this.cbxLockStatus.Text).ToString();
                //if (lookUpEditorStatus.Enabled == false)
                //{
                //    m_status = "1502,1503";
                //}
                textEditPatientBedNO.Text = "";
                textEditPatientID.Text = "";
                textEditPatientName.Text = "";
            }
            catch (Exception)
            {
                throw;
            }
        }
        //归档4700和null;未归档4701;撤销归档4702  add by zjy
        public int GetLockStatusValue(string describle)
        {
            int value = 0;
            switch (describle)
            {
                case "未完成":
                    value = 4700;
                    break;
                case "已归档":
                    value = 4701;
                    break;
                case "撤销归档":
                    value = 4702;
                    break;
                case "科室质控":
                    value = 4705;
                    break;
                case "已提交":
                    value = 4706;
                    break;
                case "已完成":
                    value = 4704;
                    break;
                //         '
                //when i.islock='4701'then '已归档'
                //when i.islock='4702' then '撤销归档'
                //  when i.islock='4704'then '已完成'
                //when i.islock='4705' then '科室质控'
                //   when i.islock='4706' then '已提交'
            }
            return value;
        }
        #endregion

        #region Load

        private void QualityMedicalRecord_Load(object sender, EventArgs e)
        {
            try
            {
                InitUCPoint();
                RegisterEvent();
                InitDepartment();
                InitStatus();
                //AddQCForm();
                string config = DS_SqlService.GetConfigValueByKey("IsNewUcInput");
                if (null != config && config.Trim() == "1")
                {
                    AddEmrInputNew();
                }
                else
                {
                    AddEmrInput();
                }
                InitInTime();
                SetLookDeptEditor();
                Search();
                this.InitQCManager();
                xtraTabControlEmrInfo.SelectedTabPage = xtraTabPage4;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 根据用户质控权限初始化窗口 add by wyt 2012-12-06
        /// </summary>
        private void InitQCManager()
        {
            try
            {
                bool haveRole = false;
                m_auth = Authority.DEPTQC;  //默认科室质控员
                string deptid = m_App.User.CurrentDeptId;
                string userid = m_App.User.DoctorId;
                //质控科质控员
                string configvalue = m_SqlManger.GetConfigValueByKey("ShowAllDeptQuality");
                string c_UserJobId = m_App.User.GWCodes;                //当前登录人的jobid标识
                string[] userJobid = c_UserJobId.Split(',');
                if (!string.IsNullOrEmpty(configvalue))
                {
                    if (configvalue.Contains(","))                      //配置了多个角色可查看
                    {
                        string[] configjobid = configvalue.Split(',');  //配置里的多个角色jobid
                        for (int i = 0; i < configjobid.Length; i++)    //先循环配置里所有jobid
                        {
                            if (haveRole == true)
                            {
                                break;
                            }
                            for (int j = 0; j < userJobid.Length; j++)  //再循环登录人的多个jobid
                            {
                                if (configjobid[i] == userJobid[j])
                                {
                                    m_auth = Authority.QC;
                                    haveRole = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (string item in userJobid)//取出
                        {
                            if (item == configvalue)//当前登录人的jobid在系统配置中,可以查看全院质控
                            {
                                m_auth = Authority.QC;
                                haveRole = true;
                                break;
                            }
                        }
                    }
                }
                # region 王运涛 注释
                //string gwcodes = m_App.User.GWCodes;
                //string[] gwcode = gwcodes.Split(',');
                //foreach (string code in gwcode)
                //{
                //    if (code == "88")
                //    {
                //        m_auth = Authority.QC;
                //        break;
                //    }
                //}
                //string qcuserid = GetConfigValueByKey("RHQCMangesConfig");
                //string[] qcuserids = qcuserid.Split(',');
                //foreach (string id in qcuserids)
                //{
                //    if (m_App.User.DoctorId == id)
                //    {
                //        m_auth = Authority.QC;
                //        break;
                //    }
                //}
                //if (m_App.User.CurrentDeptId == "5117")
                //{
                //    m_auth = Authority.QC;
                //}
                #endregion
                if (haveRole == false)
                {
                    DataTable deptmanager = m_SqlManger.GetDirectorDoc(deptid);
                    foreach (DataRow dr in deptmanager.Rows)
                    {
                        if (dr["ID"].ToString() == userid)
                        {
                            m_auth = Authority.DEPTMANAGER;
                            break;
                        }
                    }
                }
                //switch (m_auth)
                // {
                //case Authority.DEPTQC:
                //    this.simpleButtonAddQC.Visible = true;
                //    this.simpleButtonSubmit.Visible = true;
                //    this.simpleButtonCheckIn.Visible = false;
                //    this.simpleButtonCheckOut.Visible = false;
                //    this.simpleButtonDel.Visible = true;
                //    break;
                //case Authority.DEPTMANAGER:
                //    this.simpleButtonAddQC.Visible = false;
                //    this.simpleButtonSubmit.Visible = false;
                //    this.simpleButtonCheckIn.Visible = true;
                //    this.simpleButtonCheckOut.Visible = true;
                //    this.simpleButtonDel.Visible = false;
                //    break;
                //case Authority.QC:
                this.simpleButtonAddQC.Visible = true;
                this.simpleButtonSubmit.Visible = false;
                this.simpleButtonCheckIn.Visible = false;
                this.simpleButtonCheckOut.Visible = false;
                this.simpleButtonDel.Visible = true;
                //        break;
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 根据系统配置控制科室下拉框的可编辑性
        /// add by ywk 
        /// 当前登录人存在与配置中的角色，就可选择科室，不存在只让科室选中当前科室且不可编辑
        /// </summary>
        private void SetLookDeptEditor()
        {
            try
            {
                string configvalue = m_SqlManger.GetConfigValueByKey("ShowAllDeptQuality");
                string c_UserJobId = m_App.User.GWCodes;//当前登录人的jobid标识
                string[] userJobid = c_UserJobId.Split(',');
                if (!string.IsNullOrEmpty(configvalue))
                {
                    if (configvalue.Contains(","))//配置了多个角色可查看
                    {
                        string[] configjobid = configvalue.Split(',');//配置里的多个角色jobid
                        for (int i = 0; i < configjobid.Length; i++)//先循环配置里所有jobid
                        {
                            for (int j = 0; j < userJobid.Length; j++)//再循环登录人的多个jobid
                            {
                                if (configjobid[i] == userJobid[j])
                                {
                                    lookUpEditorDepartment.Enabled = true;//循环到有可查看全院质控时，就没必要再循环了
                                    //   lookUpEditorStatus.Enabled = true;
                                    return;
                                }
                                else
                                {
                                    lookUpEditorDepartment.Enabled = false;
                                    //
                                    //   lookUpEditorStatus.Enabled = false;
                                }
                            }
                        }
                    }
                    else//只配置了一个角色能查看全院质控
                    {
                        foreach (string item in userJobid)//取出
                        {
                            if (item == configvalue)//当前登录人的jobid在系统配置中,可以查看全院质控
                            {
                                lookUpEditorDepartment.Enabled = true;
                                //lookUpEditorStatus.Enabled = true;
                                return;
                            }
                            else
                            {
                                lookUpEditorDepartment.Enabled = false;
                                // lookUpEditorStatus.Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void SetLookDeptEditorNew()
        {
            try
            {
                string configvalue = m_SqlManger.GetConfigValueByKey("ShowAllDeptQuality");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 初始化病历评分界面
        /// </summary>
        private void InitUCPoint()
        {
            try
            {
                ucPoint1.InitData(m_App, m_SqlManger);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 初始化界面

        private void RegisterEvent()
        {
            try
            {
                gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridView1_FocusedRowChanged);
                xtraTabControlEmrInfo.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(xtraTabControlEmrInfo_SelectedPageChanged);

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitInTime()
        {
            //默认为显示二月内的数据    王冀  默认两个月
            try
            {
                dateEditBeginInTime.EditValue = System.DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
                dateEditEndInTime.EditValue = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 初始化科室下拉框
        /// </summary>
        private void InitDepartment()
        {
            try
            {
                lookUpWindowDepartment.SqlHelper = m_App.SqlHelper;

                string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid ");
                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 70);
                cols.Add("NAME", 80);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;

                lookUpEditorDepartment.CodeValue = m_App.User.CurrentDeptId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 初始化病人状态下拉框
        /// </summary>
        private void InitStatus()
        {
            try
            {
                lookUpWindowStatus.SqlHelper = m_App.SqlHelper;

                string sql = string.Format(@"select c.id, c.name from categorydetail c 
                                         where c.categoryid = '15' and c.id in 
                                         (select distinct status from inpatient)
                                         ");
                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "状态代码";
                Dept.Columns["NAME"].Caption = "状态名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 70);
                cols.Add("NAME", 80);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name");
                //  lookUpEditorStatus.SqlWordbook = deptWordBook;

                // lookUpEditorStatus.CodeValue = "1503";
                if (isDeptQc)
                {
                    cbxLockStatus.Enabled = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #endregion

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
        }

        #region 切换病人
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                LoadPatientEmrInfo();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void LoadPatientEmrInfo()
        {
            try
            {
                //GridHitInfo hitInfo = gridView1.CalcHitInfo(gridControlPatientList.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)));
                //if (hitInfo.RowHandle < 0) return;

                if (gridView1.FocusedRowHandle < 0)
                {
                    return;
                }
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在切换病人...");
                ///新版或老版文书录入配置
                string config = DS_SqlService.GetConfigValueByKey("IsNewUcInput");
                if (null != config && config.Trim() == "1" && null == m_UCEmrInputNew)
                {
                    AddEmrInputNew();
                }

                //m_UserTimeQcInfo.App = m_App;
                DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;

                if (drv == null) return;
                m_noofinpat = drv["NOOFINPAT"].ToString();
                m_patientname = drv["NAME"].ToString();
                m_patientstatus = drv["STATUS"].ToString();
                m_chiefID = string.Empty;
                //加载时限信息
                //YD_Common.SetWaitDialogCaption(m_WaitDialog,"正在加载时限信息！");
                //m_UserTimeQcInfo.CheckPatientTime(Convert.ToInt32(m_noofinpat));
                //m_UserTimeQcInfo.HideGridViewGroup();
                LoadAutoMarkRecord(m_chiefID, "0"); //加载主评分记录
                LoadAutoMarkRecord(m_chiefID, "1"); //加载自动评分记录
                LoadEmrDocPoint();                  //加载当前评分详情
                //加载病历内容
                if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPageEmrContent)
                {
                    if (null != config && config.Trim() == "1")
                    {
                        LoadEmrContentNew();
                    }
                    else
                    {
                        LoadEmrContent();
                    }
                }
                else if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPage1)
                {
                    InitlookUpEditEMR();
                }
                else
                {
                    m_IsReLoadEmrContent = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
        }
        #endregion

        #region  已弃用 YD_Common已有方法 by xlb 2012-12-26
        //public void SetWaitDialogCaption(string caption)
        //{
        //    try
        //    {
        //        if (m_WaitDialog != null)
        //        {
        //            if (!m_WaitDialog.Visible)
        //                m_WaitDialog.Visible = true;
        //            m_WaitDialog.Caption = caption;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        //public void SetWaitDialogCaption()
        //{
        //    try
        //    {
        //        if (m_WaitDialog != null)
        //        {
        //            if (!m_WaitDialog.Visible)
        //                m_WaitDialog.Visible = true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void HideWaitDialog()
        //{
        //    try
        //    {
        //        if (m_WaitDialog != null)
        //            m_WaitDialog.Hide();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        #region 切换页面
        private void xtraTabControlEmrInfo_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (xtraTabControlEmrInfo.SelectedTabPage.Controls.Count > 0)
                {
                    xtraTabControlEmrInfo.SelectedTabPage.Controls[0].Select();
                }
                if (gridView1.FocusedRowHandle < 0)
                {
                    return;
                }
                if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPageEmrContent && m_IsReLoadEmrContent == true)
                {
                    string config = DS_SqlService.GetConfigValueByKey("IsNewUcInput");
                    if (null != config && config.Trim() == "1")
                    {
                        LoadEmrContentNew();
                    }
                    else
                    {
                        LoadEmrContent();
                    }
                    DS_Common.HideWaitDialog(m_WaitDialog);
                }
                //else if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPageDepartment)
                //{
                //    LoadDepartmentPatStatInfo();
                //}
                else if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPagePoint)
                {
                    LoadEmrDocPoint();
                }
                else if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPage1)
                {
                    //InitlookUpEditEMR();
                    SetAutoMarkInfo();

                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }
        #endregion

        #region 病历内容
        private void LoadEmrContent()
        {
            try
            {
                DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
                if (drv == null) return;

                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病历信息！");
                //m_App.ChoosePatient(Convert.ToDecimal(drv["noofinpat"]));
                //m_UCEmrInput.PatientChanged();
                m_UCEmrInput.PatientChangedByIEmrHost(Convert.ToDecimal(m_noofinpat));
                m_UCEmrInput.HideBar();
                m_IsReLoadEmrContent = false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 病历评分
        private void LoadEmrDocPointOld()
        {
            try
            {
                DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
                if (drv == null) return;

                if (m_UCEmrInput != null)
                {
                    EmrModel emrModel = m_UCEmrInput.GetCurrentModel();
                    EmrModelContainer emrModelContainer = m_UCEmrInput.GetCurrentModelContainer();
                    ucPoint1.RefreshLookUpEditorEmrDoc(m_auth, m_check, m_qctype, m_noofinpat, m_chiefID, emrModel, emrModelContainer);
                }
                else
                {
                    ucPoint1.RefreshLookUpEditorEmrDoc(m_auth, m_check, m_qctype, m_noofinpat, m_chiefID, null, null);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 评分 --- 配置(老版/新版)
        /// </summary>
        private void LoadEmrDocPoint()
        {
            try
            {
                string config = DS_SqlService.GetConfigValueByKey("IsNewUcInput");
                if (null != config && config.Trim() == "1")
                {
                    LoadEmrDocPointNew();
                }
                else
                {
                    LoadEmrDocPointOld();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 收缩 展开 所有节点
        private void simpleButtonShouSuo_Click(object sender, EventArgs e)
        {
            try
            {
                //收缩所有节点
                gridView1.CollapseAllGroups();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButtonZhanKai_Click(object sender, EventArgs e)
        {
            try
            {
                //展开所有节点
                gridView1.ExpandAllGroups();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        /// <summary>
        /// edit by xlb 2012-12-26
        /// </summary>
        private void SetPatientList()
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("正在加载病人列表...", "请稍候");
                //SetWaitDialogCaption();
                DataTable dt;
                //   if (lookUpEditorStatus.Enabled == false)
                //   {
                //  dt = m_SqlManger.GetPatientOutList(m_deptNo, m_patID, m_name, m_beginInTime, m_endInTime);
                //}
                //else
                //{
                dt = m_SqlManger.GetPatientList(m_deptNo, m_patID, m_name, m_status, m_beginInTime, m_endInTime);
                //}
                string patStyle = cmbEditPat.SelectedText.Trim();
                string filter = string.Empty;
                switch (patStyle)
                {
                    case "全部病人":
                        break;
                    case "死亡病人":
                        if (dt.Columns.Contains("ZG_FLAG"))
                        {
                            filter = "ZG_FLAG=4 OR OUTHOSTYPE='5'";
                            dt.DefaultView.RowFilter = filter;
                        }

                        break;
                    case "手术病人":
                        //filter = "CLOSE_LEVEL is not null";                         edit by wangj 2013 3 6  有重复数据 已修改
                        if (dt.Columns.Contains("iem_mainpage_no"))
                        {
                            filter = "iem_mainpage_no is not null";
                            dt.DefaultView.RowFilter = filter;
                        }
                        break;
                    case "输血病人":
                        if (dt.Columns.Contains("BLOODFEE"))
                        {
                            for (int i = dt.Rows.Count - 1; i >= 0; i--)
                            {
                                string bloodFee = dt.Rows[i]["BLOODFEE"].ToString();
                                decimal d = 0;
                                bool isBloodFee = !decimal.TryParse(bloodFee, out d);
                                if (isBloodFee || d <= 0)
                                {
                                    dt.Rows.RemoveAt(i);
                                }
                            }
                        }

                        break;
                    default:
                        break;
                }

                gridControlPatientList.BeginUpdate();
                gridControlPatientList.DataSource = dt;
                gridView1.ExpandAllGroups();//默认打开时展开组合 xlb 2013-01-06
                gridControlPatientList.EndUpdate();
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 科室统计信息

        /// <summary>
        /// 得到科室统计信息
        /// </summary>
        /// <returns></returns>
        private void SetAllDepartmentStatInfo()
        {
            try
            {
                //Thread bindAllDepartmentThread = new Thread(new ThreadStart(BindAllDepartmentStatInfo));
                //bindAllDepartmentThread.Start();

                //解决用线程时出现红叉的问题
                AsyncDelegate asydele = new AsyncDelegate(BindAllDepartmentStatInfo);
                gridControlAllDepartmentStatInfo.BeginInvoke(asydele);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void BindAllDepartmentStatInfo()
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("正在加载科室统计信息！", "请您稍后！");
                //SetWaitDialogCaption();
                DataTable dt = m_SqlManger.GetAllDepartmentStatInfo(m_deptNo, m_patID, m_name, m_status, m_beginInTime, m_endInTime);
                gridControlAllDepartmentStatInfo.BeginUpdate();
                gridControlAllDepartmentStatInfo.DataSource = dt;
                gridControlAllDepartmentStatInfo.EndUpdate();
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception)
            {
                throw;
            }
        }

        delegate void AsyncDelegate();

        #endregion

        #region 科室病人信息

        private void LoadDepartmentPatStatInfo()
        {
            try
            {
                int rowIndex = gridViewMain.FocusedRowHandle;
                DataTable dt = gridControlAllDepartmentStatInfo.DataSource as DataTable;
                if (dt != null)
                {
                    DataRowView drv = gridViewMain.GetRow(rowIndex) as DataRowView;
                    string deptNO = drv["DEPTNO"].ToString();
                    SetDepartmentPatStatInfo(deptNO);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetDepartmentPatStatInfo(string deptNO)
        {
            try
            {
                if (deptNO != "")
                {
                    m_deptSelect = deptNO;

                    //Thread bindAllDepartmentThread = new Thread(new ThreadStart(BindDepartmentPatStatInfo));
                    //bindAllDepartmentThread.Start();

                    //解决用线程时出现红叉的问题
                    AsyncDelegate asydele = new AsyncDelegate(BindDepartmentPatStatInfo);
                    gridControlDepartmentPatStatInfo.BeginInvoke(asydele);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int SumPoint { get; set; }//满分值。通过配置中取得 ywk 2012年6月12日 14:43:29 
        private string m_deptSelect = string.Empty;
        private void BindDepartmentPatStatInfo()
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("正在加载病人统计信息！", "请您稍后！");
                //SetWaitDialogCaption();
                //新加个参数，计算总分
                //SumPoint = Int32.Parse(m_SqlManger.GetConfigValueByKey("EmrPointConfig"));
                string point = m_SqlManger.GetConfigValueByKey("EmrPointConfig");
                int sumpoint1 = 85;
                int sumpoint2 = 100;
                if (point.Contains(","))
                {
                    string[] points = point.Split(',');
                    sumpoint1 = Int32.Parse(points[0]);
                    sumpoint2 = Int32.Parse(points[1]);
                }
                DataTable dt = m_SqlManger.GetDepartmentPatStatInfo(m_deptSelect, m_patID, m_name, m_status, m_beginInTime, m_endInTime, m_sortid, sumpoint1, sumpoint2, "time", m_App.User.DoctorId, m_auth);
                gridControlDepartmentPatStatInfo.BeginUpdate();
                gridControlDepartmentPatStatInfo.DataSource = dt;
                gridControlDepartmentPatStatInfo.EndUpdate();
                m_deptSelect = "";
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        private void gridViewMain_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewMain.CalcHitInfo(gridControlAllDepartmentStatInfo.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle >= 0)
                {
                    LoadDepartmentPatStatInfo();
                    xtraTabControlEmrInfo.SelectedTabPage = xtraTabPageDepartment;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void gridViewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
        }

        private void gridViewDepartment_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //GridHitInfo hitInfo = gridViewDepartment.CalcHitInfo(gridControlAllDepartmentStatInfo.PointToClient(Cursor.Position));
                //if (gridViewDepartment.FocusedRowHandle >= 0)
                //{
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
                else
                {
                    return;
                }
                //}
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 科室病人信息筛选

        private void textEditPatientBedNO_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                PatientFilterInGridControl();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditPatientID_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                PatientFilterInGridControl();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditPatientName_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                PatientFilterInGridControl();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 病人数据筛选
        /// </summary>
        private void PatientFilterInGridControl()
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        private void gridViewMain_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void gridViewDepartment_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置搜索条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                InitInTime();
                this.textEditPatID.Text = "";
                this.textEditName.Text = "";
                lookUpEditorDepartment.CodeValue = m_App.User.CurrentDeptId;
                //  lookUpEditorStatus.CodeValue = "";
                cmbEditPat.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        public void Reset()
        {
            try
            {
                InitInTime();
                this.textEditPatID.Text = "";
                this.textEditName.Text = "";
                lookUpEditorDepartment.CodeValue = m_App.User.CurrentDeptId;
                // lookUpEditorStatus.CodeValue = "";
                cmbEditPat.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void gridView1_DoubleClick_1(object sender, EventArgs e)
        {
            try
            {
                if (this.gridView1.FocusedRowHandle == -1)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        //edit by wyt 2012-12-06 没用到
        //private void gridView2_DoubleClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataRow dr = gridView2.GetFocusedDataRow();
        //        if (dr == null)
        //        {
        //            return;
        //        }
        //        DataTable dt = gridControlAutoMarkRecord.DataSource as DataTable;
        //        if (dt == null)
        //        {
        //            return;
        //        }
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            DataRowView drv = gridView2.GetRow(i) as DataRowView;
        //            if (m_noofinpat == drv["NOOFINPAT"].ToString())
        //            {
        //                gridView2.FocusedRowHandle = i;
        //                //双击病人信息跳转到病历评分页面 
        //                xtraTabControlEmrInfo.SelectedTabPage = xtraTabPagePoint;
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
        //    }
        //}
        #region 自动评分记录
        private void SetAutoMarkInfo()
        {
            try
            {
                AsyncDelegate asydele = new AsyncDelegate(BindAutoMarkInfo);
                gridControlAutoMarkRecord.BeginInvoke(asydele);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 加载自动评分记录信息
        /// 王冀 2012 11-14
        /// </summary>
        private void BindAutoMarkInfo()
        {
            try
            {
                DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
                if (drv == null)
                {
                    return;
                }
                m_WaitDialog = new WaitDialogForm("正在加载自动评分记录信息！", "请您稍后！");
                //SetWaitDialogCaption();
                //edit by wyt 2012-12-06
                //DataTable dt = m_SqlManger.GetAutoMarkInfo(m_noofinpat,"1");
                //gridControlAutoMarkRecord.BeginUpdate();
                //gridControlAutoMarkRecord.DataSource = dt;
                //gridControlAutoMarkRecord.EndUpdate();
                LoadAutoMarkRecord(m_noofinpat, "1");
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 双击选中的评分记录信息 跳转到 自动评分详情
        /// 王冀 2012 11-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gridView2.FocusedRowHandle;
                DataRowView drv = gridView2.GetRow(rowIndex) as DataRowView;
                if (drv == null)
                {
                    return;
                }
                m_autoID = drv["ID"].ToString();
                BindPatRecordDetail();
                //GridHitInfo hitInfo = gridView2.CalcHitInfo(gridControlAutoMarkRecord.PointToClient(Cursor.Position));
                //if (hitInfo.RowHandle >= 0)
                //{
                //LoadAutoMarkDetail();
                //lbCurrentRecordDetail.Text = drv["PANAME"].ToString() + drv["RECNAME"].ToString();
                lbCurrentRecordDetail.Text = drv["ID"].ToString() + "    " + drv["RECNAME"].ToString();
                xtraTabControlEmrInfo.SelectedTabPage = xtraTabPage2;
                //}
                //else
                //{
                //    return;
                //}
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
            finally
            {
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
        }

        /// <summary>
        /// 根据选中的自动评分主表ID 加载自动评分详情
        /// 王冀 2012 11-14
        /// </summary>
        //private void LoadAutoMarkDetail()
        //{
        //    //try
        //    //{

        //    //    BindPatRecordDetail();
        //    //        //edit by wyt 2012-12-06
        //    //        //SetPatRecordDetail(m_autoID);
        //    //}
        //    //catch (Exception)
        //    //{

        //    //    throw;
        //    //}

        //}

        //edit by wyt 2012-12-06
        ///// <summary>
        ///// 处理加载自动评分详情
        ///// 王冀 2012 11-14
        ///// </summary>
        ///// <param name="ID"></param>
        //private void SetPatRecordDetail(string ID)
        //{
        //    try
        //    {
        //        if (ID == "")
        //        {
        //            return;
        //        }
        //        m_IDSelect = ID;
        //        AsyncDelegate asydele = new AsyncDelegate(BindPatRecordDetail);
        //        gridControlAutoMarkRecord.BeginInvoke(asydele);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        /// <summary>
        /// 加载自动评分详情
        /// 王冀 2012 11-14
        /// </summary>
        private void BindPatRecordDetail()
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("正在加载选中评估记录自动评分内容！", "请您稍候！");
                //SetWaitDialogCaption();
                DataTable dt = m_SqlManger.GetPatRecordDetail(m_autoID);
                #region add by wyt 2012-11-19 复选框列
                //dt.Columns.Add("CHECK", typeof(bool));
                //foreach (DataRow dr in dt.Rows)
                //{
                //    dr["CHECK"] = true;
                //}
                #endregion
                gridControlAutoRecordDetail.BeginUpdate();
                gridControlAutoRecordDetail.DataSource = dt;
                gridControlAutoRecordDetail.EndUpdate();
                m_deptSelect = "";
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception)
            {
                DS_Common.HideWaitDialog(m_WaitDialog);
                throw;
            }
        }

        /// <summary>
        /// 加载自动评分记录信息
        /// 王冀 2012 11-14 edit by wyt 2012-11-21    //edit by wyt 2012-12-10
        /// </summary>
        private void LoadAutoMarkRecord(string id, string isAuto)
        {
            try
            {
                //DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
                //if (drv == null)
                //{
                //    return;
                //}
                //string noofinpat = drv["NOOFINPAT"].ToString();
                if (isAuto == "0")
                {
                    gridControlChiefMark.DataSource = null;
                    DataTable dt = m_SqlManger.GetAutoMarkInfo(m_noofinpat, isAuto, m_auth);
                    if (dt == null && dt.Rows.Count <= 0)
                    {
                        return;
                    }
                    gridControlChiefMark.BeginUpdate();
                    gridControlChiefMark.DataSource = dt;
                    gridControlChiefMark.EndUpdate();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["ID"].ToString() == id)
                        {
                            gridView4.FocusedRowHandle = i;
                            return;
                        }
                    }
                }
                else
                {
                    gridControlAutoMarkRecord.DataSource = null;
                    DataTable dt = m_SqlManger.GetAutoMarkInfo(m_noofinpat, isAuto, m_auth);
                    if (dt == null && dt.Rows.Count <= 0)
                    {
                        return;
                    }
                    gridControlAutoMarkRecord.BeginUpdate();
                    gridControlAutoMarkRecord.DataSource = dt;
                    gridControlAutoMarkRecord.EndUpdate();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["ID"].ToString() == id)
                        {
                            gridView2.FocusedRowHandle = i;
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 加载病人病历下拉框数据
        /// 王冀 2012-11-14
        /// </summary>
        private void InitlookUpEditEMR()
        {
            try
            {
                DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
                if (drv == null)
                {
                    return;
                }
                DataTable dt = m_SqlManger.GetPatientsEMRS(m_noofinpat);
                lookUpEditEMR.Properties.DataSource = dt;
                lookUpEditEMR.Properties.ValueMember = "ID";
                lookUpEditEMR.Properties.DisplayMember = "NAME";
                lookUpEditEMR.Properties.NullText = "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 屏蔽 by wyt 2012-11-20 自动新增主表记录
        ///// <summary>
        ///// 新增一条评分记录
        ///// 王冀 2012 11-14
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void DevButtonAdd1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (DialogResult.OK == m_App.CustomMessageBox.MessageShow("确定要新增一条记录？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
        //        {
        //            DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
        //            if (drv == null)
        //            {
        //                Common.Ctrs.DLG.MessageBox.Show("请先选择一个病人");
        //                return;
        //            }
        //            if (this.lookUpEditEMR.EditValue.ToString().Trim() == "")
        //            {
        //                Common.Ctrs.DLG.MessageBox.Show("请先选择一份病历");
        //                return;
        //            }
        //            bool bo = m_SqlManger.InsertAutoMarkRecord(lookUpEditEMR.Text.Trim(), lookUpEditEMR.EditValue.ToString(), drv["NOOFINPAT"].ToString());
        //            if (bo)
        //            {
        //                Common.Ctrs.DLG.MessageBox.Show("新增成功！");
        //                LoadAutoMarkRecord();
        //                lookUpEditEMR.ItemIndex = -1;
        //            }
        //            else { Common.Ctrs.DLG.MessageBox.Show("新增失败！"); }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
        //    }
        //}
        #endregion

        /// <summary>
        /// 对自动评分主记录进行假删除操作
        /// 王冀 2012 11-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                //删除前先判断哟没有记录 add by ywk 二〇一三年六月四日 08:51:25  
                if (gridView2.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选择要删除的记录！");
                    return;
                }
                if (DialogResult.OK == m_App.CustomMessageBox.MessageShow("确定要删除记录？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
                {
                    DataRowView drv = gridView2.GetRow(gridView2.FocusedRowHandle) as DataRowView;

                    m_autoID = drv["ID"].ToString();
                    if (m_autoID == "")
                    {
                        MessageBox.Show("请选择要删除的记录！");
                        return;
                    }
                    bool bo = m_SqlManger.DeleteAutoMarkRecord(m_autoID);
                    if (bo)
                    {
                        MessageBox.Show("删除成功！");
                    }
                    else
                    {
                        MessageBox.Show("删除失败！");
                    }
                    LoadAutoMarkRecord(string.Empty, "1");

                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }
        #endregion

        ///// <summary>
        ///// 对当前病历开始自动评测
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void DevButtonAdd2_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (m_SqlManger.IsHaveDone(m_IDSelect))
        //        {
        //            Common.Ctrs.DLG.MessageBox.Show("已经自动评测过，如果您需要再次评测，请新增一条空白评测记录！");
        //            return;
        //        }
        //        AutoMarkCheck();

        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
        //    }
        //}

        ///// <summary>
        ///// 自动评分检测
        ///// </summary>
        //private void AutoMarkCheck()
        //{
        //    try
        //    {
        //        if (DialogResult.Cancel == m_App.CustomMessageBox.MessageShow("确定要开始进行自动评测？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
        //        {
        //            return;
        //        }
        //        string emrclass = m_SqlManger.GetEMRClass(RecordID);
        //        DataTable resultDt = new DataTable();
        //        if (emrclass == "AB")
        //        {
        //            OperationOfZhuyuanzhi sa = new OperationOfZhuyuanzhi(m_App);
        //            sa.GetResultPoint(RecordID, emrclass, noofinpat, m_IDSelect, patientname);
        //        }
        //        if (emrclass == "AC")
        //        {
        //            OperationOfBingchengjilu sa = new OperationOfBingchengjilu(m_App);
        //            sa.GetResultPoint(RecordID, emrclass, noofinpat, m_IDSelect, patientname);
        //        }









        //        SetRecordDone(m_IDSelect);
        //        SetPatRecordDetail(m_IDSelect);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// 将该条记录设置为已测
        /// 王冀 2012 11 19
        /// </summary>
        /// <param name="id"></param>
        private void SetRecordDone()
        {
            try
            {
                m_SqlManger.SetRecordDone(m_autoID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 自动序列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView3_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 自动评分明细项导入总评分明细项 wyt 2012-11-20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonSave_Score_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Cancel == m_App.CustomMessageBox.MessageShow("确定要将自动评分记录导入总评分记录？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
                {
                    return;
                }
                DataTable autoMarkRecord = m_SqlManger.GetAutoMarkRecord(m_autoID);
                if (autoMarkRecord.Rows.Count == 0)
                {
                    return;
                }
                m_WaitDialog = new WaitDialogForm("正在导入评分记录", "请您稍候！");
                //SetWaitDialogCaption();
                DataRow row = autoMarkRecord.Rows[0];
                DataTable autoRecordDetail = this.gridControlAutoRecordDetail.DataSource as DataTable;
                foreach (DataRow dr in autoRecordDetail.Rows)
                {
                    if (bool.Parse(dr["CHECK"].ToString()) == false)
                    {
                        continue;
                    }
                    EmrPoint emrPoint = new EmrPoint();
                    emrPoint.Valid = "1";       //是否有效
                    emrPoint.DoctorID = dr["ERRORDOCTOR"].ToString();      //责任医师ID
                    emrPoint.DoctorName = dr["ERRORDOCTORNAME"].ToString();    //责任医师，
                    emrPoint.CreateUserID = m_App.User.Id;  //登记人ID，取当前用户ID
                    emrPoint.CreateUserName = m_App.User.Name;  //登记人姓名，取当前用户ID
                    emrPoint.ProblemDesc = dr["CONFIGREDUCTIONNAME"].ToString();   //扣分理由
                    emrPoint.RecordDetailID = "";   //病案ID,如果是病案首页则显示IEM_MAINPAGE_NO
                    emrPoint.ReducePoint = dr["POINT"].ToString();   //扣分
                    emrPoint.Grade = dr["POINT"].ToString() + "级";  //等级
                    emrPoint.Num = "1";         //次数，暂取1
                    emrPoint.Noofinpat = dr["NOOFINPAT"].ToString();    //病人首页序号
                    emrPoint.RecordDetailName = dr["cname"].ToString(); //病案名称，自动评分主表和病案记录表病案名称
                    emrPoint.EMR_MARK_RECORD_ID = m_chiefID;    //评分主记录ID


                    //string id = "";  //大类别编号（AC，AB），暂时取不到，需要关联查询
                    //DataTable dtRecord = new DataTable();
                    //string searchsq = string.Format(@" select sortid from  recorddetail where id ='{0}' ", id);
                    //dtRecord = m_App.SqlHelper.ExecuteDataTable(searchsq);
                    //string sortid = "";
                    //if (dtRecord.Rows.Count > 0)
                    //{
                    //    sortid = dtRecord.Rows[0]["sortid"].ToString();
                    //}
                    //else
                    //{
                    //    //大项从dict_catalog表里取数据
                    //    string slq = string.Format(@" select ccode from   dict_catalog where cname='{0}'", lookUpEditEmrDoc.Text);
                    //    if (m_App.SqlHelper.ExecuteDataTable(slq).Rows.Count > 0)
                    //    {
                    //        sortid = m_App.SqlHelper.ExecuteDataTable(slq).Rows[0]["ccode"].ToString();
                    //    }
                    //}

                    //评分配置表的主键
                    //edit by wyt 2012-12-11
                    //emrPoint.EmrPointID = Convert.ToInt32(dr["CHILDREN"]);   //评分小项编号，如主诉（15），暂时取不到
                    emrPoint.EmrPointID = dr["CHILDREN"].ToString();   //评分小项编号，如主诉（15）
                    emrPoint.SortID = dr["PARENTS"].ToString(); //评分大项ID，如住院志（AB），取病案记录表的SORTID字段
                    m_SqlManger.InsertEmrPoint(emrPoint);
                }
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        ///// <summary>
        ///// 添加病案首页自动评分记录 add by wyt 2012-11-20
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void simpleButtonMainPage_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (DialogResult.OK == m_App.CustomMessageBox.MessageShow("确定要新增一条首页质控记录？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
        //        {
        //            DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
        //            if (drv == null)
        //            {
        //                Common.Ctrs.DLG.MessageBox.Show("请先选择一个病人");
        //                return;
        //            }
        //            string iem_mainpage_no = m_SqlManger.GetIEM_MAINPAGE_NO(drv["NOOFINPAT"].ToString());
        //            if (iem_mainpage_no == "")
        //            {
        //                //为空不能插入，库里有字段非空验证
        //                Common.Ctrs.DLG.MessageBox.Show("新增失败！");
        //                return;
        //            }
        //            bool bo = m_SqlManger.InsertAutoMarkRecord("病案首页", iem_mainpage_no, drv["NOOFINPAT"].ToString());
        //            if (bo)
        //            {
        //                Common.Ctrs.DLG.MessageBox.Show("新增成功！");
        //                LoadAutoMarkRecord();
        //                lookUpEditEMR.ItemIndex = -1;
        //            }
        //            else { Common.Ctrs.DLG.MessageBox.Show("新增失败！"); }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
        //    }
        //}

        private string InsertNewAutoRecord(string isAuto)
        {
            try
            {

                //DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
                //if (drv == null)
                //{
                //    Common.Ctrs.DLG.MessageBox.Show("请先选择一个病人");
                //    return "";
                //}

                if (m_noofinpat == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请先选择一个病人");
                    return "";
                }
                //string iem_mainpage_no = m_SqlManger.GetIEM_MAINPAGE_NO(drv["NOOFINPAT"].ToString());
                if (isDeptQc)
                {
                    m_qctype = QCType.Dept;
                }
                else
                {
                    if (cbxLockStatus.Text.Trim() == "未完成")
                    {
                        m_qctype = QCType.PART;
                    }
                    else
                    {
                        m_qctype = QCType.FINAL;
                    }
                }
                string id = m_SqlManger.InsertAutoMarkRecord(m_noofinpat, isAuto, m_auth, m_qctype);//返回主表记录ID
                if (id == "")
                {
                    return id;
                }
                //bool bo = m_SqlManger.InsertAutoMarkRecord(drv["NOOFINPAT"].ToString());
                //if (bo)
                //{
                //    //Common.Ctrs.DLG.MessageBox.Show("新增成功！");
                LoadAutoMarkRecord(id, isAuto);
                lookUpEditEMR.ItemIndex = -1;
                return id;
                //}
                //else 
                //{ 
                //    //Common.Ctrs.DLG.MessageBox.Show("新增失败！");
                //}
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                return "";
            }
        }
        /// <summary>
        /// 新增自动评分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonAutoScore_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.OK == m_App.CustomMessageBox.MessageShow("确定要开始一次自动评分？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
                {
                    m_WaitDialog = new WaitDialogForm("正在进行自动评分", "请您稍后！");
                    //SetWaitDialogCaption();
                    //在主表新增一条自动评分记录
                    if (m_noofinpat == "")
                    {
                        m_App.CustomMessageBox.MessageShow("请在左侧的病人列表中选择一个病人");
                        return;
                    }

                    m_autoID = InsertNewAutoRecord("1");
                    if (m_autoID == "")
                    {
                        m_App.CustomMessageBox.MessageShow("评分出错");
                        return;
                    }

                    #region 注释 王冀 2012 11 30
                    ////CHECK病案文件是否存在
                    //OperationOfFirstCheck ishave = new OperationOfFirstCheck(m_App);
                    //ishave.GetResultPoint(noofinpat, patientname, id);
                    ////病案首页评分
                    //EmrAutoScore mainpage = new EmrAutoScore(m_App);
                    //mainpage.AutoScoreAA(noofinpat, patientname, id);
                    ////除首页外的病案文档评分
                    //DataTable dtEmrDoc = m_SqlManger.GetPatientsEMRS(noofinpat);
                    //foreach (DataRow dr in dtEmrDoc.Rows)
                    //{
                    //    string RecordID = dr["ID"].ToString();
                    //    string emrclass = m_SqlManger.GetEMRClass(RecordID);
                    //    string errordoctorID = dr["OWNER"].ToString();
                    //    if (emrclass == "AB")
                    //    {
                    //        OperationOfZhuyuanzhi sa = new OperationOfZhuyuanzhi(m_App);
                    //        sa.GetResultPoint(RecordID, emrclass, noofinpat, id, patientname, errordoctorID);
                    //    }
                    //    if (emrclass == "AC")
                    //    {
                    //        OperationOfBingchengjilu sa = new OperationOfBingchengjilu(m_App);
                    //        sa.GetResultPoint(RecordID, emrclass, noofinpat, id, patientname, errordoctorID);
                    //    }
                    //}
                    #endregion

                    AutoMarkRecoed autoMark = new AutoMarkRecoed(m_App);
                    autoMark.Automark(m_autoID, m_noofinpat, m_patientname, m_qctype);
                    SetRecordDone();
                    DS_Common.HideWaitDialog(m_WaitDialog);
                    m_App.CustomMessageBox.MessageShow("成功");
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DrectSoft.Common.DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButtonAddQC_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = gridControlChiefMark.DataSource as DataTable;
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (isDeptQc)
                    {
                        m_qctype = QCType.Dept;
                    }
                    else
                    {
                        if (cbxLockStatus.Text.Trim() == "未完成")
                        {
                            m_qctype = QCType.PART;
                        }
                        else
                        {
                            m_qctype = QCType.FINAL;
                        }
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["qctype"].ToString() == "环节质控" && m_qctype == QCType.PART || dr["qctype"].ToString() == "终末质控" && m_qctype == QCType.FINAL || dr["qctype"].ToString() == "科室质控" && m_qctype == QCType.Dept)
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("已存在该质控数据，无法添加新纪录！");
                            return;
                        }
                    }
                }
                m_WaitDialog = new WaitDialogForm("正在创建综合评分", "请您稍后！");
                //SetWaitDialogCaption();
                //在主表新增一条自动评分记录
                if (m_noofinpat == "")
                {
                    m_App.CustomMessageBox.MessageShow("请在左侧的病人列表中选择一个病人");
                    DS_Common.HideWaitDialog(m_WaitDialog);
                    return;
                }

                m_chiefID = InsertNewAutoRecord("0");
                setQcTimemesage();

                if (m_chiefID == "")
                {
                    m_App.CustomMessageBox.MessageShow("出错");
                    DS_Common.HideWaitDialog(m_WaitDialog);
                    return;
                }
                m_check = CheckState.NEW;
                //AutoMarkRecoed autoMark = new AutoMarkRecoed(m_App);
                //autoMark.Automark(id, noofinpat, patientname);
                //SetRecordDone(id);
                //SetPatRecordDetail(id);

                DS_Common.HideWaitDialog(m_WaitDialog);
                this.xtraTabControlEmrInfo.SelectedTabPage = xtraTabPage4;
                //m_App.CustomMessageBox.MessageShow("成功");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 时限质控信息
        /// </summary>
        void setQcTimemesage()
        {
            AutoMarkRecoed autoMark = new AutoMarkRecoed(m_App);

            autoMark.GetTimeLimitedQc(m_noofinpat, m_chiefID, m_patientname);//   时限质控
        }
        private void gridView4_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int index = this.gridView4.FocusedRowHandle;
                if (index < 0)
                {
                    return;
                }
                DataRowView drv = gridView4.GetRow(index) as DataRowView;
                m_chiefID = drv["ID"].ToString();
                switch (drv["CheckState"].ToString())
                {
                    case "新建":
                        m_check = CheckState.NEW;
                        break;
                    case "提交未审核":
                        m_check = CheckState.SUBMIT;
                        break;
                    case "审核通过":
                        m_check = CheckState.CHECKIN;
                        break;
                    case "审核未通过":
                        m_check = CheckState.CHECKOUT;
                        break;
                    case "质控员质控":
                        m_check = CheckState.QC;
                        break;
                }
                //如果是科室主任，只能查看
                if (m_auth == Authority.DEPTMANAGER)
                {
                    m_check = CheckState.CHECKIN;
                    // LoadEmrDocPoint();  //刷新评分控件信息
                    //   openSetPoint();
                }
                //如果是质控员，只能编辑本人的质控记录
                if (m_auth == Authority.QC)
                {
                    string createUser = drv["UNAME"].ToString();
                    if (createUser == m_App.User.DoctorName)
                    {
                        m_check = CheckState.NEW;
                        //  LoadEmrDocPoint();  //刷新评分控件信息
                        // openSetPoint();
                    }
                    else
                    {
                        m_check = CheckState.CHECKIN;
                        // LoadEmrDocPoint();  //刷新评分控件信息
                        //  openSetPoint();
                    }
                }
                switch (drv["QCTYPE"].ToString())
                {
                    case "环节质控":
                        m_qctype = QCType.PART;
                        break;
                    case "终末质控":
                        m_qctype = QCType.FINAL;
                        break;
                    case "科室质控":
                        m_qctype = QCType.Dept;
                        break;
                }

                this.xtraTabControlEmrInfo.SelectedTabPage = xtraTabPagePoint;

                m_check = CheckState.CHECKIN;
                // LoadEmrDocPoint();  //刷新评分控件信息

                openSetPoint();

                LoadEmrDocPoint();  //刷新评分控件信息
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        void openSetPoint()
        {
            SetPoint m_configPoint = new SetPoint(m_App);
            m_configPoint.InitData(m_App, m_SqlManger);
            if (m_UCEmrInputNew != null)
            {
                EmrModel emrModel = m_UCEmrInputNew.CurrentInputBody.GetCurrentModel();
                EmrModelContainer emrModelContainer = m_UCEmrInputNew.CurrentInputBody.GetCurrentModelContainer();

                m_configPoint.RefreshLookUpEditorEmrDoc(m_auth, m_check, m_qctype, m_noofinpat, m_chiefID, emrModel, emrModelContainer);
            }
            else
            {
                // m_configPoint.RefreshLookUpEditorEmrDoc(m_auth, m_check, m_qctype, m_noofinpat, m_chiefID, null, null);

                m_configPoint.RefreshLookUpEditorEmrDoc(m_auth, m_check, m_qctype, m_noofinpat, m_chiefID, null, null);
            }

            m_configPoint.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
            m_configPoint.ShowDialog();
        }
        private void simpleButtonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int index = this.gridView4.FocusedRowHandle;
                if (index < 0)
                {
                    return;
                }
                DataRowView drv = gridView4.GetRow(index) as DataRowView;
                m_chiefID = drv["ID"].ToString();
                m_SqlManger.UpdateAutoMarkRecord(m_chiefID, "1");
                m_check = CheckState.SUBMIT;
                this.simpleButtonSubmit.Enabled = false;
                this.simpleButtonDel.Enabled = false;
                LoadAutoMarkRecord(m_chiefID, "0"); //加载主评分记录
                LoadEmrDocPoint();  //刷新评分控件信息
                gridView1.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButtonCheckIn_Click(object sender, EventArgs e)
        {
            try
            {
                int index = this.gridView4.FocusedRowHandle;
                if (index < 0)
                {
                    return;
                }
                DataRowView drv = gridView4.GetRow(index) as DataRowView;
                m_chiefID = drv["ID"].ToString();
                m_SqlManger.UpdateAutoMarkRecord(m_chiefID, "2");

                m_check = CheckState.CHECKIN;
                this.simpleButtonCheckIn.Enabled = false;
                this.simpleButtonCheckOut.Enabled = false;

                LoadAutoMarkRecord(m_chiefID, "0"); //加载主评分记录
                LoadEmrDocPoint();  //刷新评分控件信息
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButtonCheckOut_Click(object sender, EventArgs e)
        {
            try
            {
                int index = this.gridView4.FocusedRowHandle;
                if (index < 0)
                {
                    return;
                }
                DataRowView drv = gridView4.GetRow(index) as DataRowView;
                m_chiefID = drv["ID"].ToString();
                m_SqlManger.UpdateAutoMarkRecord(m_chiefID, "3");
                m_check = CheckState.CHECKOUT;
                this.simpleButtonCheckIn.Enabled = false;
                this.simpleButtonCheckOut.Enabled = false;
                LoadAutoMarkRecord(m_chiefID, "0"); //加载主评分记录
                LoadEmrDocPoint();  //刷新评分控件信息
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void gridView4_Click(object sender, EventArgs e)
        {
            try
            {
                int index = this.gridView4.FocusedRowHandle;
                if (index < 0)
                {
                    return;
                }
                DataRowView drv = gridView4.GetRow(index) as DataRowView;
                if (m_auth == Authority.DEPTQC)
                {
                    string state = drv["CHECKSTATE"].ToString();
                    string createuser = drv["UNAME"].ToString();
                    if (state == "新建" || state == "审核未通过")
                    {
                        this.simpleButtonSubmit.Enabled = true;
                    }
                    else
                    {
                        this.simpleButtonSubmit.Enabled = false;
                    }
                    //可删除自己建立的记录
                    if (m_App.User.DoctorName == createuser && (state == "新建" || state == "审核未通过"))
                    {
                        this.simpleButtonDel.Enabled = true;
                    }
                    else
                    {
                        this.simpleButtonDel.Enabled = false;
                    }
                }
                if (m_auth == Authority.DEPTMANAGER)
                {
                    string state = drv["CHECKSTATE"].ToString();
                    if (state == "提交未审核")
                    {
                        this.simpleButtonCheckIn.Enabled = true;
                        this.simpleButtonCheckOut.Enabled = true;
                    }
                    else
                    {
                        this.simpleButtonCheckIn.Enabled = false;
                        this.simpleButtonCheckOut.Enabled = false;
                    }

                    //如果是科室主任，只能查看
                    m_check = CheckState.CHECKIN;
                    LoadEmrDocPoint();  //刷新评分控件信息
                }

                //如果是质控员，只能编辑本人的质控记录
                if (m_auth == Authority.QC)
                {
                    string createUser = drv["UNAME"].ToString();
                    string createuser = drv["UNAME"].ToString();
                    if (createUser == m_App.User.DoctorName)
                    {
                        m_check = CheckState.NEW;
                        LoadEmrDocPoint();  //刷新评分控件信息
                    }
                    else
                    {
                        m_check = CheckState.CHECKIN;
                        LoadEmrDocPoint();  //刷新评分控件信息
                    }
                    //可删除自己建立的记录
                    if (m_App.User.DoctorName == createuser)
                    {
                        this.simpleButtonDel.Enabled = true;
                    }
                    else
                    {
                        this.simpleButtonDel.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }

        private void gridView4_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void simpleButtonDel_Click(object sender, EventArgs e)
        {
            try
            {
                int index = this.gridView4.FocusedRowHandle;
                if (index < 0)
                {
                    m_App.CustomMessageBox.MessageShow("未选中记录");
                    return;
                }
                else
                {
                    if (m_App.CustomMessageBox.MessageShow("确定删除选中记录？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
                    {
                        DataRowView drv = gridView4.GetRow(index) as DataRowView;
                        m_chiefID = drv["ID"].ToString();
                        m_SqlManger.DelAutoMarkRecord(m_chiefID);
                        this.simpleButtonDel.Enabled = true;
                        LoadAutoMarkRecord(m_chiefID, "0"); //加载主评分记录
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 病人状态改变时触发的事件
        /// by xlb 2012-12-26
        /// 只对出院和出区病人使用重点病人类型查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorStatus_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                //if (lookUpEditorStatus.CodeValue == "1502" || lookUpEditorStatus.CodeValue == "1503")
                //{
                //    cmbEditPat.Enabled = true;
                //}
                //else
                //{
                //    cmbEditPat.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 新版文书录入
        private void AddEmrInputNew()
        {
            try
            {
                DataRow drv = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (drv == null)
                {
                    return;
                }
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病人信息...");
                m_App.ChoosePatient(Convert.ToDecimal(drv["noofinpat"]));//切换病人
                DS_Common.HideWaitDialog(m_WaitDialog);

                m_UCEmrInputNew = new DrectSoft.Core.MainEmrPad.New.UCEmrInput(m_App.CurrentPatientInfo, m_App, FloderState.None);
                m_UCEmrInputNew.SetVarData(m_App);
                xtraTabPageEmrContent.Controls.Add(m_UCEmrInputNew);
                m_UCEmrInputNew.OnLoad();
                m_UCEmrInputNew.HideBar();
                m_UCEmrInputNew.Dock = DockStyle.Fill;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadEmrContentNew()
        {
            try
            {
                DataRow drv = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (drv == null)
                {
                    return;
                }

                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病人信息...");
                m_App.ChoosePatient(Convert.ToDecimal(drv["noofinpat"]));
                DS_Common.HideWaitDialog(m_WaitDialog);

                m_UCEmrInputNew.PatientChangedByIEmrHost(Convert.ToDecimal(m_noofinpat));
                m_UCEmrInputNew.HideBar();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadEmrDocPointNew()
        {
            try
            {
                DataRow drv = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (drv == null)
                {
                    return;
                }

                if (m_UCEmrInputNew != null)
                {
                    EmrModel emrModel = m_UCEmrInputNew.CurrentInputBody.GetCurrentModel();
                    EmrModelContainer emrModelContainer = m_UCEmrInputNew.CurrentInputBody.GetCurrentModelContainer();
                    ucPoint1.RefreshLookUpEditorEmrDoc(m_auth, m_check, m_qctype, m_noofinpat, m_chiefID, emrModel, emrModelContainer);
                }
                else
                {
                    ucPoint1.RefreshLookUpEditorEmrDoc(m_auth, m_check, m_qctype, m_noofinpat, m_chiefID, null, null);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public void refreshQuery()
        {
            try
            {
                Search();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

    }
}
