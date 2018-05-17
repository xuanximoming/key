using MedicalRecordManage.Object;
using MedicalRecordManage.UCControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

/// <summary>
/// <title>病例浏览</title>
/// <auth>jy.zhu</auth>
/// <date>2013-06-07<</date>
/// </summary>
namespace MedicalRecordManage.UI
{
    /// <summary>
    /// 病历借阅主界面
    /// </summary>
    public partial class MedicalRecordBrowseForm : DevBaseForm, IStartPlugIn
    {

        public string m_sUser;
        public List<object> m_listObject = null;
        /// <summary>
        /// 保存，当前自定义窗体的实体集合
        /// </summary>
        public List<RecordModel> listAll = new List<RecordModel>();
        DevExpress.Utils.WaitDialogForm m_WaitDialog = null;

        /// <summary>
        /// 
        /// </summary>
        public MedicalRecordBrowseForm()
        {
            try
            {
                InitializeComponent();
                InitializeParameter();
                m_sUser = ComponentCommand.GetCurrentDoctor();//获取登录用户信息
                m_listObject = new List<object>();
                m_listObject.Clear();
                InitUserControl();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region InitUserControl 初始化，自定义窗体，和TabControl项数据
        /// <summary>
        /// 初始化，自定义窗体，和TabControl项数据
        /// </summary>
        private void InitUserControl()
        {

            try
            {
                m_WaitDialog = new DevExpress.Utils.WaitDialogForm("正在查询数据...", "请稍等");
                int index = this.tabControl.SelectedTabPageIndex;
                if (listAll.Count == 0)
                {
                    int st = 0;
                    for (int i = 0; i < this.tabControl.TabPages.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                st = 0;
                                break;
                            case 1:
                                st = 2;
                                break;
                            case 2:
                                st = 1;
                                break;
                            case 3:
                                st = 3;
                                break;
                            case 4:
                                st = 5;
                                break;
                            case 5:
                                st = 4;
                                break;
                            default:
                                st = 0;
                                break;
                        }
                        DataTable table = GetInitDB(i, st);
                        int rowscount = table.Rows.Count;
                        RefreshInformation(i, rowscount);
                        if (i == index)
                        {
                            MedicalRecordBrowse medicalrecordbrowse = new MedicalRecordBrowse(i, st, table);
                            medicalrecordbrowse.Dock = DockStyle.Fill;
                            listAll.Add(new RecordModel(i, st, rowscount, medicalrecordbrowse, table));
                            tabControl.TabPages[i].Controls.Add(medicalrecordbrowse);
                            medicalrecordbrowse.OnButtonClick += new MedicalRecordBrowse.MyControlEventHandler(OnButtonClick);
                        }
                        else
                        {
                            listAll.Add(new RecordModel(i, st, rowscount, null, table));
                        }
                    }
                }
                else
                {
                    UserControlDB(index);
                }
                m_WaitDialog.Close();
            }
            catch (Exception ex)
            {
                m_WaitDialog.Close();
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region UserControlDB 通过实体获取已存在的RecordModel实体，设置当前显示的窗体

        /// <summary>
        /// 通过实体获取已存在的RecordModel实体，设置当前显示的窗体
        /// </summary>
        public void UserControlDB(int index)
        {

            try
            {
                foreach (RecordModel rc in listAll)
                {
                    if (rc.Index == index)
                    {
                        if (rc.MyControl == null)
                        {
                            MedicalRecordBrowse medicalrecordbrowse = new MedicalRecordBrowse(rc.Index, rc.Type, rc.MyDataTable);
                            rc.MyControl = medicalrecordbrowse;
                            rc.MyControl.Dock = DockStyle.Fill;
                            tabControl.TabPages[rc.Index].Controls.Add(medicalrecordbrowse);
                            rc.MyControl.OnButtonClick += new MedicalRecordBrowse.MyControlEventHandler(OnButtonClick);
                            RefreshInformation(rc.Index, rc.Count);
                        }
                        else
                        {
                            rc.MyControl.Dock = DockStyle.Fill;
                            tabControl.TabPages[index].Controls.Add(rc.MyControl);
                            rc.MyControl.OnButtonClick += new MedicalRecordBrowse.MyControlEventHandler(OnButtonClick);
                            RefreshInformation(rc.Index, rc.Count);
                        }
                        break;
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        #endregion

        #region GetInitDB 返回，不同类型的数据源

        /// <summary>
        /// 返回，不同类型的数据源
        /// </summary>
        /// <param name="TypeIndex">当前索引</param>
        /// <param name="TypeNumber">当前状态值（status）</param>
        /// <returns></returns>
        private DataTable GetInitDB(int TypeIndex, int TypeNumber)
        {
            DateTime dateStart = System.DateTime.Today.AddMonths(-6);
            DateTime dateEnd = System.DateTime.Today;


            //add by zjy 2013-6-14
            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            string sql = MedicalRecordManage.Object.DaoCommand._SELECT_MEDICAL_RECORD_BORROW_SQL;
            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sql = MedicalRecordManage.Object.DaoCommand._SELECT_MEDICAL_RECORD_BORROW_SQL_ZY;
            }

            List<DbParameter> sqlParams = new List<DbParameter>();

            sql = sql + " and i.applydocid = @doc";
            SqlParameter param1 = new SqlParameter("@doc", SqlDbType.VarChar);
            param1.Value = m_sUser;
            sqlParams.Add(param1);
            if (dateStart != null && dateStart.ToString() != "")
            {
                string ds = dateStart.ToString("yyyy-MM-dd");
                sql = sql + " and  to_char(i.applydate,'yyyy-mm-dd') >= @ds";
                SqlParameter param2 = new SqlParameter("@ds", SqlDbType.VarChar);
                param2.Value = ds;
                sqlParams.Add(param2);
            }
            if (dateEnd != null && dateEnd.ToString() != "")
            {
                string de = dateEnd.ToString("yyyy-MM-dd");
                sql = sql + " and  to_char(i.applydate,'yyyy-mm-dd') <= @de";
                SqlParameter param3 = new SqlParameter("@de", SqlDbType.VarChar);
                param3.Value = de;
                sqlParams.Add(param3);
            }

            if (TypeNumber >= 0)
            {
                sql = sql + " and i.status = @st";
                SqlParameter param4 = new SqlParameter("@st", SqlDbType.Int);
                param4.Value = TypeNumber;
                sqlParams.Add(param4);
            }

            DataTable dataTable = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, sqlParams, CommandType.Text);
            //压缩DateSet中的记录，将住院诊断信息合并
            ComponentCommand.ImpressDataSet(ref dataTable, "id", "cyzd");
            //填入状态值
            ComponentCommand.InitializeStatusInfo(ref dataTable);
            //控制状态显示和预警信息显示颜色显示           

            return dataTable;
        }
        #endregion

        #region 初始化头部栏目状态
        /// <summary>
        /// 初始化头部栏目状态
        /// </summary>
        private void InitializeParameter()
        {
            try
            {

                this.baBtnSubmit.Enabled = false;
                this.baBtnEdit.Enabled = false;
                this.baBtnDel.Enabled = false;
                this.baBtnRead.Enabled = true;
                this.baBtnApplyDe.Enabled = true;
                this.baBtnFeed.Enabled = true;
                this.baBtnBack.Enabled = false;
                this.baBtnReason.Enabled = false;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        /// <summary>
        /// tabControl 索引变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {

            try
            {
                int i = tabControl.SelectedTabPageIndex;
                switch (i)
                {
                    case 0:
                        this.baBtnSubmit.Enabled = true;
                        this.baBtnEdit.Enabled = true;
                        this.baBtnDel.Enabled = true;
                        //
                        this.baBtnRead.Enabled = false;
                        this.baBtnApplyDe.Enabled = false;
                        this.baBtnFeed.Enabled = false;
                        this.baBtnBack.Enabled = false;
                        this.baBtnReason.Enabled = false;
                        break;
                    case 1:
                        this.baBtnSubmit.Enabled = false;
                        this.baBtnEdit.Enabled = false;
                        this.baBtnDel.Enabled = false;
                        //
                        this.baBtnRead.Enabled = true;
                        this.baBtnApplyDe.Enabled = true;
                        this.baBtnFeed.Enabled = true;
                        this.baBtnBack.Enabled = false;
                        this.baBtnReason.Enabled = false;
                        break;
                    case 2:
                        this.baBtnSubmit.Enabled = false;
                        this.baBtnEdit.Enabled = false;
                        this.baBtnDel.Enabled = false;
                        //
                        this.baBtnRead.Enabled = false;
                        this.baBtnApplyDe.Enabled = false;
                        this.baBtnFeed.Enabled = false;
                        this.baBtnBack.Enabled = true;
                        this.baBtnReason.Enabled = false;
                        break;
                    case 3:
                        this.baBtnSubmit.Enabled = false;
                        this.baBtnEdit.Enabled = false;
                        this.baBtnDel.Enabled = false;
                        //
                        this.baBtnRead.Enabled = false;
                        this.baBtnApplyDe.Enabled = false;
                        this.baBtnFeed.Enabled = false;
                        this.baBtnBack.Enabled = false;
                        this.baBtnReason.Enabled = true;
                        break;
                    case 4:
                        this.baBtnSubmit.Enabled = false;
                        this.baBtnEdit.Enabled = false;
                        this.baBtnDel.Enabled = false;
                        //
                        this.baBtnRead.Enabled = false;
                        this.baBtnApplyDe.Enabled = false;
                        this.baBtnFeed.Enabled = false;
                        this.baBtnBack.Enabled = false;
                        this.baBtnReason.Enabled = false;
                        break;
                    case 5:
                        this.baBtnSubmit.Enabled = false;
                        this.baBtnEdit.Enabled = false;
                        this.baBtnDel.Enabled = false;
                        //
                        this.baBtnRead.Enabled = false;
                        this.baBtnApplyDe.Enabled = false;
                        this.baBtnFeed.Enabled = false;
                        this.baBtnBack.Enabled = false;
                        this.baBtnReason.Enabled = false;
                        break;
                    default:
                        break;
                }
                UserControlDB(i);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 提交事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void baBtnSubmit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ExecuteCommand(1, "提交申请");
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void baBtnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //编辑
                OpenCommandDialog(0);

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void baBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //删除
                ExecuteCommand(-1, "删除申请");
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 阅读病历事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void baBtnRead_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //查看病历,调用接口  
            try
            {
                ShowEmrMessage();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 显示病历信息方法
        /// </summary>
        private void ShowEmrMessage()
        {
            try
            {
                foreach (RecordModel re in listAll)
                {
                    if (re.Index == this.tabControl.SelectedTabPageIndex)
                    {
                        MedicalRecordBrowse mf = re.MyControl;
                        DevExpress.XtraGrid.Views.Grid.GridView dbGridView = mf.dbGridView;
                        if (dbGridView.RowCount > 0)
                        {
                            int[] list = dbGridView.GetSelectedRows();
                            if (list.Length > 0)
                            {
                                string status = dbGridView.GetRowCellValue(list[0], "STATUS").ToString();
                                //调用病历查看窗口,进行病历的查询
                                if (status.Equals("2"))
                                {
                                    string noofinpat = dbGridView.GetRowCellValue(list[0], "NOOFINPAT").ToString();
                                    LoadEmrContent(noofinpat);
                                }
                            }
                        }
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载病历
        /// </summary>
        /// <param name="noofinpat"></param>
        private void LoadEmrContent(string noofinpat)
        {
            try
            {
                EmrBrowerDlg frm = new EmrBrowerDlg(noofinpat, SqlUtil.App, FloderState.None);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Show();//edit by ywk 2013年3月16日10:31:38，应该使用show 
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 申请延期事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void baBtnApplyDe_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                OpenCommandDialog(1);
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }

        }

        //验证是否还允许延期申请
        private bool CanDoDelay()
        {
            try
            {
                //InitializeApplyList();
                if (this.m_listObject.Count > 0)
                {
                    CApplyObject obj = (CApplyObject)m_listObject[0];
                    int times = ComponentCommand.GetDealyTimes();
                    return (CApplyObject.IsCanDelay(obj, times) > 0);
                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void baBtnFeed_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //归还

            try
            {
                ExecuteCommand(5, "归还病历");
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void baBtnBack_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //撤销


            try
            {
                ExecuteCommand(4, "撤销申请");
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void baBtnReason_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //查看原因
            //编辑

            try
            {
                OpenCommandDialog(2);
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void brBtnApply_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //申请

            try
            {
                OpenCommandDialog(-1);
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        private void OpenCommandDialog(int flag)
        {
            try
            {
                foreach (RecordModel re in listAll)
                {
                    if (re.Index == this.tabControl.SelectedTabPageIndex)
                    {
                        MedicalRecordBrowse mf = re.MyControl;
                        DevExpress.XtraGrid.Views.Grid.GridView dbGridView = mf.dbGridView;
                        if (dbGridView.RowCount > 0)
                        {
                            int refresh = 0;
                            int tabIndex = 0;
                            int StatusCount = 0;
                            if (flag < 0)
                            {
                                MedicalRecordApply medicalRecordApply = new MedicalRecordApply();
                                medicalRecordApply.ShowDialog();
                                refresh = medicalRecordApply.m_iCommandFlag;
                                tabIndex = medicalRecordApply.m_iTabIndex;

                                //2是提交，0是草稿
                            }
                            else
                            {
                                InitializeApplyList(dbGridView);
                                if (this.m_listObject.Count > 0)
                                {
                                    CApplyObject obj = (CApplyObject)m_listObject[0];
                                    MedicalRecordEditAndDelay medicalRecordApply = new MedicalRecordEditAndDelay(obj, flag);
                                    medicalRecordApply.ShowDialog();
                                    refresh = medicalRecordApply.m_iCommandFlag;
                                    tabIndex = medicalRecordApply.m_iTabIndex;

                                }
                            }
                            //刷新数据
                            if (refresh > 0)
                            {
                                if (tabIndex == 2)
                                {
                                    int[] list = dbGridView.GetSelectedRows();
                                    dbGridView.DeleteRow(list[0]);
                                    StatusCount++;
                                }

                                switch (flag)
                                {
                                    case -1://申请
                                        foreach (RecordModel rd in listAll)
                                        {
                                            if (rd.Index == 2)
                                            {
                                                if (rd.MyControl == null)
                                                {
                                                    UserControlDB(rd.Index);
                                                }
                                                rd.MyControl.GetInitDB(true);
                                                rd.MyDataTable = rd.MyControl.User_Table;
                                                rd.Count = rd.MyControl.User_Table.Rows.Count;
                                                RefreshInformation(rd.Index, rd.Count);
                                            }
                                        }
                                        break;
                                    case 0://编辑
                                        foreach (RecordModel rd in listAll)
                                        {
                                            if (rd.Index == 2)
                                            {
                                                if (rd.MyControl == null)
                                                {
                                                    UserControlDB(rd.Index);
                                                }
                                                rd.MyControl.GetInitDB(true);
                                                rd.MyDataTable = rd.MyControl.User_Table;
                                                rd.Count = rd.MyControl.User_Table.Rows.Count;
                                                RefreshInformation(rd.Index, rd.Count);
                                            }
                                        }
                                        break;
                                    case 1:
                                        foreach (RecordModel rd in listAll)
                                        {
                                            if (rd.Index == 0)
                                            {
                                                if (rd.MyControl == null)
                                                {
                                                    UserControlDB(rd.Index);
                                                }
                                                rd.MyControl.GetInitDB(true);
                                                rd.MyDataTable = rd.MyControl.User_Table;
                                                rd.Count = rd.MyControl.User_Table.Rows.Count;
                                                RefreshInformation(rd.Index, rd.Count);
                                            }
                                            else if (rd.Index == 3)
                                            {
                                                if (rd.MyControl == null)
                                                {
                                                    UserControlDB(rd.Index);
                                                }
                                                rd.MyControl.GetInitDB(true);
                                                rd.MyDataTable = rd.MyControl.User_Table;
                                                rd.Count = rd.MyControl.User_Table.Rows.Count;
                                                RefreshInformation(rd.Index, rd.Count);
                                            }
                                        }
                                        break;//申请延期事件                                   
                                }
                                re.MyControl.GetInitDB(true);
                                re.Count = re.Count - StatusCount;
                                re.MyControl.dbGridView = dbGridView;
                                RefreshInformation(re.Index, re.Count);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void InitializeApplyList(DevExpress.XtraGrid.Views.Grid.GridView dbGridView)
        {
            try
            {
                m_listObject.Clear();
                int[] rows = dbGridView.GetSelectedRows();
                for (int i = 0; i < rows.Length; i++)
                {
                    CApplyObject m_applyObject = new CApplyObject();
                    m_applyObject.Clear();

                    m_applyObject.m_sNoOfInpat = dbGridView.GetRowCellValue(rows[i], "NOOFINPAT").ToString();
                    m_applyObject.m_sName = dbGridView.GetRowCellValue(rows[i], "NAME").ToString();
                    m_applyObject.m_sDepartmentName = dbGridView.GetRowCellValue(rows[i], "CYKS").ToString();

                    m_applyObject.m_iApplyTimes = int.Parse(dbGridView.GetRowCellValue(rows[i], "APPLYTIMES").ToString());
                    m_applyObject.m_sApplyContent = dbGridView.GetRowCellValue(rows[i], "APPLYCONTENT").ToString();
                    m_applyObject.m_sApply = dbGridView.GetRowCellValue(rows[i], "ID").ToString();
                    m_applyObject.m_sApplyDocId = this.m_sUser;

                    m_applyObject.m_sApproveContent = dbGridView.GetRowCellValue(rows[i], "APPROVECONTENT").ToString();
                    m_applyObject.m_sApproveDate = dbGridView.GetRowCellValue(rows[i], "APPROVEDATE").ToString();
                    m_applyObject.m_sApproveDocId = dbGridView.GetRowCellValue(rows[i], "APPROVEDOCID").ToString();
                    m_applyObject.m_sPatid = dbGridView.GetRowCellValue(rows[i], "PATID").ToString();
                    m_listObject.Add(m_applyObject);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public IPlugIn Run(IEmrHost host)
        {
            try
            {
                PlugIn plg = new PlugIn(this.GetType().ToString(), this);

                SqlUtil.App = host;

                return plg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 刷新标签信息方法
        /// Modify by xlb 2013-03-28
        /// </summary>
        /// add zjy 2013-6-5
        /// <param name="dataTable"></param>
        public void RefreshInformation(int Type, int Count)
        {
            try
            {
                switch (Type)
                {
                    case 0:
                        this.xtraTabPage1.Text = "草稿" +
                       "(" + Count.ToString() + ")";
                        break;
                    case 1:
                        this.xtraTabPage2.Text = "审核通过" +
                       "(" + Count.ToString() + ")";
                        break;
                    case 2:
                        this.xtraTabPage3.Text = "待审核" +
                        "(" + Count.ToString() + ")";
                        break;
                    case 3:
                        this.xtraTabPage4.Text = "审核不通过" +
                        "(" + Count.ToString() + ")";
                        break;
                    case 4:
                        this.xtraTabPage5.Text = "借阅历史" +
                        "(" + Count.ToString() + ")";
                        break;
                    case 5:
                        this.xtraTabPage6.Text = "撤销" +
                       "(" + Count.ToString() + ")";
                        break;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MedicalRecordBrowseForm_Load(object sender, EventArgs e)
        {

            try
            {
                AutoDealFeedMedicalRecord();
                InitUserControl();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 委托事件，获取自定义窗体参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnButtonClick(object sender, MyControlEventArgs e)
        {
            foreach (RecordModel r in listAll)
            {
                if (r.Index == this.tabControl.SelectedTabPageIndex)
                {
                    r.MyControl = e.MyMedicalRecordBrowse;
                    r.Count = int.Parse(e.Result);
                    tabControl.TabPages[r.Index].Controls.Add(r.MyControl);
                    RefreshInformation(r.Index, r.Count);
                    break;
                }
            }

        }

        /// <summary>
        /// 自动归还处理到期申请
        /// </summary>
        private void AutoDealFeedMedicalRecord()
        {
            try
            {
                CApplyObject.AutoFeed(m_sUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                //申请
                OpenCommandDialog(-1);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MedicalRecordBrowseForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, false);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="info"></param>
        private void ExecuteCommand(int status, string info)
        {
            try
            {
                foreach (RecordModel re in listAll)
                {
                    if (re.Index == this.tabControl.SelectedTabPageIndex)
                    {
                        DevExpress.XtraGrid.Views.Grid.GridView dbGridView = re.MyControl.dbGridView;
                        if (dbGridView.RowCount > 0)
                        {
                            if (dbGridView.SelectedRowsCount > 0)
                            {
                                /*
                                string name = dbGridView.GetRowCellValue(dbGridView.GetSelectedRows()[0], "NAME").ToString();
                                string number = dbGridView.GetRowCellValue(dbGridView.GetSelectedRows()[0], "NOOFINPAT").ToString();
                                info = info + " 病历：" + name + "(" + number + ")";
                                */
                                DialogResult dR = DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确认要" + info + "吗？", "信息提示", MyMessageBoxButtons.OkCancel);
                                if (dR == System.Windows.Forms.DialogResult.OK)
                                {
                                    int[] list = dbGridView.GetSelectedRows();
                                    int StatusCount = 0;
                                    for (int i = 0; i < list.Length; i++)
                                    {
                                        string id = dbGridView.GetRowCellValue(list[i], "ID").ToString();
                                        if (status > 0)
                                        {
                                            dbGridView.SetRowCellValue(list[i], "STATUSDES", status);
                                            CApplyObject.UpdateStatus(id, status);
                                            dbGridView.DeleteRow(list[i]);
                                            StatusCount++;
                                        }
                                        else
                                        {
                                            dbGridView.DeleteRow(list[i]);
                                            CApplyObject.Delete(id);
                                            StatusCount++;
                                        }
                                    }

                                    switch (status)
                                    {
                                        case 1:
                                            foreach (RecordModel rd in listAll)
                                            {
                                                if (rd.Index == 2)
                                                {
                                                    if (rd.MyControl == null)
                                                    {
                                                        UserControlDB(rd.Index);
                                                    }
                                                    rd.MyControl.GetInitDB(true);
                                                    rd.MyDataTable = rd.MyControl.User_Table;
                                                    rd.Count = rd.MyControl.User_Table.Rows.Count;
                                                    RefreshInformation(rd.Index, rd.Count);
                                                }
                                            }
                                            break;//提交申请
                                        case -1: break;//删除申请
                                        case 5:
                                            foreach (RecordModel rd in listAll)
                                            {
                                                if (rd.Index == 4)
                                                {
                                                    if (rd.MyControl == null)
                                                    {
                                                        UserControlDB(rd.Index);
                                                    }
                                                    rd.MyControl.GetInitDB(true);
                                                    rd.MyDataTable = rd.MyControl.User_Table;
                                                    rd.Count = rd.MyControl.User_Table.Rows.Count;
                                                    RefreshInformation(rd.Index, rd.Count);
                                                }
                                            }
                                            break;//归还病历
                                        case 4:
                                            foreach (RecordModel rd in listAll)
                                            {
                                                if (rd.Index == 5)
                                                {
                                                    if (rd.MyControl == null)
                                                    {
                                                        UserControlDB(rd.Index);
                                                    }
                                                    rd.MyControl.GetInitDB(true);
                                                    rd.MyDataTable = rd.MyControl.User_Table;
                                                    rd.Count = rd.MyControl.User_Table.Rows.Count;
                                                    RefreshInformation(rd.Index, rd.Count);
                                                }
                                            }
                                            break;//撤销申请
                                    }

                                    re.Count = re.Count - StatusCount;
                                    re.MyControl.dbGridView = dbGridView;
                                    RefreshInformation(re.Index, re.Count);
                                    break;
                                }
                            }
                            else
                            {
                                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(info + "请选择需要操作的记录", "信息提示");
                            }
                        }

                    }
                }
                //提交m_Apply

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }

}