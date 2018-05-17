using DevExpress.Utils;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
namespace DrectSoft.Emr.QcManagerNew
{
    // add by wyt 2012-12-06
    public enum Authority
    {
        DEPTQC,      //科室质控员
        DEPTMANAGER, //科主任
        QC           //质控科质控员
    }
    public enum CheckState
    {
        NEW,            //新建
        SUBMIT,         //提交未审核
        CHECKIN,        //审核通过
        CHECKOUT,       //审核未通过
        QC              //质控员质控
    }
    public enum QCType
    {
        PART,           //环节质控
        FINAL           //终末质控
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

        public QualityMedicalRecord(IEmrHost app)
        {
            InitializeComponent();
            m_App = app;
            m_SqlManger = new SqlManger(app);
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
                m_status = lookUpEditorStatus.CodeValue.Trim();
                if (lookUpEditorStatus.Enabled == false)
                {
                    m_status = "1502,1503";
                }
            }
            catch (Exception)
            {
                throw;
            }
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
                switch (m_auth)
                {
                    case Authority.DEPTQC:
                        this.simpleButtonAddQC.Visible = true;
                        this.simpleButtonSubmit.Visible = true;
                        this.simpleButtonCheckIn.Visible = false;
                        this.simpleButtonCheckOut.Visible = false;
                        this.simpleButtonDel.Visible = true;
                        break;
                    case Authority.DEPTMANAGER:
                        this.simpleButtonAddQC.Visible = false;
                        this.simpleButtonSubmit.Visible = false;
                        this.simpleButtonCheckIn.Visible = true;
                        this.simpleButtonCheckOut.Visible = true;
                        this.simpleButtonDel.Visible = false;
                        break;
                    case Authority.QC:
                        this.simpleButtonAddQC.Visible = true;
                        this.simpleButtonSubmit.Visible = false;
                        this.simpleButtonCheckIn.Visible = false;
                        this.simpleButtonCheckOut.Visible = false;
                        this.simpleButtonDel.Visible = true;
                        break;
                }
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
                                    lookUpEditorStatus.Enabled = true;
                                    return;
                                }
                                else
                                {
                                    lookUpEditorDepartment.Enabled = false;
                                    //
                                    lookUpEditorStatus.Enabled = false;
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
                                lookUpEditorStatus.Enabled = true;
                                return;
                            }
                            else
                            {
                                lookUpEditorDepartment.Enabled = false;
                                lookUpEditorStatus.Enabled = false;
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
                ucPoint1.InitData(m_App, m_SqlManger, m_noofinpat, m_chiefID);
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
                lookUpEditorStatus.SqlWordbook = deptWordBook;

                lookUpEditorStatus.CodeValue = "1503";
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
                LoadEmrDocPoint();                  //加载当前评分详情
                //加载病历内容
                if (xtraTabControlEmrInfo.SelectedTabPage == xtraTabPageEmrContent)
                {
                    LoadEmrContent();
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
                if (lookUpEditorStatus.Enabled == false)
                {
                    dt = m_SqlManger.GetPatientOutList(m_deptNo, m_patID, m_name, m_beginInTime, m_endInTime);
                }
                else
                {
                    dt = m_SqlManger.GetPatientList(m_deptNo, m_patID, m_name, m_status, m_beginInTime, m_endInTime);
                }
                string patStyle = cmbEditPat.SelectedText.Trim();
                string filter = string.Empty;
                switch (patStyle)
                {
                    case "全部病人":
                        break;
                    case "死亡病人":
                        filter = "ZG_FLAG=4 OR OUTHOSTYPE='5'";
                        dt.DefaultView.RowFilter = filter;
                        break;
                    case "手术病人":
                        //filter = "CLOSE_LEVEL is not null";                         edit by wangj 2013 3 6  有重复数据 已修改
                        filter = "iem_mainpage_no is not null";
                        dt.DefaultView.RowFilter = filter;
                        break;
                    case "输血病人":
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





        private void gridViewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
        }





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
                lookUpEditorStatus.CodeValue = "";
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
                    LoadEmrDocPoint();  //刷新评分控件信息
                }
                //如果是质控员，只能编辑本人的质控记录
                if (m_auth == Authority.QC)
                {
                    string createUser = drv["UNAME"].ToString();
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
                }
                switch (drv["QCTYPE"].ToString())
                {
                    case "环节质控":
                        m_qctype = QCType.PART;
                        break;
                    case "终末质控":
                        m_qctype = QCType.FINAL;
                        break;
                }

                this.xtraTabControlEmrInfo.SelectedTabPage = xtraTabPagePoint;
                LoadEmrDocPoint();  //刷新评分控件信息
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
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
                if (lookUpEditorStatus.CodeValue == "1502" || lookUpEditorStatus.CodeValue == "1503")
                {
                    cmbEditPat.Enabled = true;
                }
                else
                {
                    cmbEditPat.Enabled = false;
                }
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
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病人信息...");
                m_App.ChoosePatient(Convert.ToDecimal(m_noofinpat));
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

    }
}
