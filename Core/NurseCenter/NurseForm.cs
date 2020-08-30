using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraTab;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core;
using DrectSoft.Core.OwnBedInfo;
using DrectSoft.Core.QCDeptReport;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using DrectSoft.Wordbook;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
namespace DrectSoft.Emr.NurseCenter
{
    /// <summary>
    /// edit by ywk  2012年3月13日9:51:47
    /// </summary>
    public partial class NurseForm : DevBaseForm, IStartPlugIn
    {
        IEmrHost m_App;

        private WaitDialogForm m_WaitDialog;
        //全部在院病人 数据集
        private DataTable allInpDataSourse;
        //cyq 2012-08-21
        private int? m_pageIndex1 = 1;
        private int? m_pageIndex2 = 1;
        private int m_totalCount1 = 0;
        private int m_totalCount2 = 0;
        private int? m_pageSize;
        private int pageSizeLocationX;
        private UCTran uctran = null;
        ReplenishPatRec repl = null; //病历补写界面

        StringFormat s = new StringFormat();
        /// <summary>
        /// 护士工作站
        /// </summary>
        public NurseForm()
        {
            InitializeComponent();
            m_WaitDialog = new WaitDialogForm("正在创建用户界面...", "请稍候");
            s.Alignment = StringAlignment.Near;
            s.LineAlignment = StringAlignment.Center;
            gridView1.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gridView1_CustomDrawCell);
            //针对有婴儿的母亲姓名列的显示 add by ykw  2012年11月27日9:17:59
            gridView2.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gridView2_CustomDrawCell);
        }

        public void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }

        }

        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }
        /// <summary>
        /// 初始化复选框 - 在院全部病人
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、初始化方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitCheckBox()
        {
            try
            {
                Reset1();
                check_0.CheckedChanged += new EventHandler(checkHl_CheckedChanged);
                check_I.CheckedChanged += new EventHandler(checkHl_CheckedChanged);
                check_II.CheckedChanged += new EventHandler(checkHl_CheckedChanged);
                check_III.CheckedChanged += new EventHandler(checkHl_CheckedChanged);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }


        private decimal FindFocusedPat()
        {
            decimal syxh = -1;
            int focusedRowHandle = gridView1.FocusedRowHandle;
            if (focusedRowHandle >= 0)
            {
                DataRow focusedRow = gridView1.GetDataRow(focusedRowHandle);
                if (!String.IsNullOrEmpty(focusedRow["NoOfInpat"].ToString()))
                    syxh = Convert.ToDecimal(focusedRow["NoOfInpat"]);
            }
            return syxh;

        }

        private void ViewPatBasicInfo()
        {
            decimal syxh = FindFocusedPat();
            if (syxh < 0)
            {
                return;
            }
            Assembly RedactPatientInfo = Assembly.Load("DrectSoft.Core.RedactPatientInfo");
            Type TyRedactPatientInfo = RedactPatientInfo.GetType("DrectSoft.Core.RedactPatientInfo.XtraFormPatientInfo");
            //实例化一个类
            DevExpress.XtraEditors.XtraForm patientInfo = (DevExpress.XtraEditors.XtraForm)Activator.CreateInstance(TyRedactPatientInfo, new object[] { m_App, syxh.ToString() });
            patientInfo.ShowDialog();

        }

        /// <summary>
        /// 加载病历编辑器
        /// </summary>
        private void LoadPatRecordEditor()
        {
            Decimal syxh = FindFocusedPat();
            if (syxh < 0) return;
            //处理有婴儿的情况
            DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string noofinpat = dataRow["NoOfInpat"].ToString();
            if (DataManager.HasBaby(noofinpat))
            {
                DrectSoft.Core.OwnBedInfo.ChoosePatOrBaby choosepat = new DrectSoft.Core.OwnBedInfo.ChoosePatOrBaby(m_App, noofinpat);
                choosepat.StartPosition = FormStartPosition.CenterParent;
                if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_App.ChoosePatient(decimal.Parse(choosepat.NOOfINPAT));
                    m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                }
            }
            else
            {
                m_App.ChoosePatient(syxh);
                m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());//add by ywk  2012年8月2日 16:43:27 
            }

        }


        #region IStartPlugIn 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public IPlugIn Run(IEmrHost host)
        {
            m_App = host;
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            return plg;

        }

        #endregion

        /// <summary>
        /// 护士工作站窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NurseForm_Load(object sender, EventArgs e)
        {
            SetWaitDialogCaption("正在加载病人数据...");
            gridView1.ExpandAllGroups();
            pageSizeLocationX = this.lbl_pageNote1.Location.X;
            InitCheckBox();

            //cyq 2012-08-22
            InitPageSize();
            m_pageSize = int.Parse(lue_pageSize.CodeValue);

            //BindWardPats();
            try
            {
                RefreshDataOfTab1();
            }
            catch (Exception ex)
            {

                m_App.CustomMessageBox.MessageShow(ex.Message);
            }

            HideWaitDialog();

            //add by ywk 2012年5月10日9:08:29 （泗县修改）
            //科室历史病人查询中，入院时间默认一个月
            Reset2();
            //ConsultationDefaultFocusRow();
            if (DS_SqlService.GetConfigValueByKey("ManualMaintainBasicInfo") == "1")
            {
                btnZhuanke.Visibility = btnChuYuan.Visibility = BarItemVisibility.Always;
            }
            else
            {
                btnZhuanke.Visibility = btnChuYuan.Visibility = BarItemVisibility.Never;
            }

            bool HasHZ = DrectSoft.Service.DS_BaseService.FlieHasKey("HZXT");
            if (!HasHZ)
            {
                dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            }


            //在会诊查询版面中初始化数据 默认加载 “一周会诊数据”
            //gridControlConsultationSearch.DataSource = GetConsultionData(7);

            //在会诊查询页面初始化本科室的会诊信息  add by ywk 2012年11月23日12:14:13
            //edit by cyq 2012-11-26 注释
            //gridControlOwnDept.DataSource = GetConsultionDataByDept(this.gridConsulation.DataSource as DataTable,"own");

            //在会诊查询页面初始化其他科室的会诊信息  add by ywk 2012年11月23日12:14:58
            //edit by cyq 2012-11-26 注释
            //gridControlOtherDept.DataSource = GetConsultionDataByDept(this.gridConsulation.DataSource as DataTable,"other");

            //add by cyq 2013-01-21
            BindEvent();

            //Add by wwj 2013-02-18
            InitConsultSetting();
            //控制DockPanel 是否显示add  by ywk 2013年6月13日 10:16:50
            if (DS_BaseService.IsShowThisMD("IsShowDockPanel", "Nurse"))//上方是隐藏。这个配置是为自动隐藏add by ywk 
            {
                dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            }
            else
            {
                dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;//自动隐藏
            }
            //dockPanel1.Visibility
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-21</date>
        private void BindEvent()
        {
            try
            {
                //床位号
                textEditBedNo.EditValueChanged += new EventHandler(textEdit_EditValueChanged);
                //病人姓名
                textEditPatientName.EditValueChanged += new EventHandler(textEdit_EditValueChanged);
                //病历号
                textEditPatientSN.EditValueChanged += new EventHandler(textEdit_EditValueChanged);
                //tab页切换事件
                xtraTabControl1.SelectedPageChanged += new TabPageChangedEventHandler(xtraTabControl1_SelectedPageChanged);
                //每页记录数 事件
                lue_pageSize.CodeValueChanged += new EventHandler(lue_pageSize_CodeValueChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 筛选会诊信息列表，区分开本科室和其他科室的信息
        /// add by  ywk 2012年11月23日12:18:33
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private DataTable GetConsultionDataByDept(DataTable dataTable, string type)
        {
            DataTable dtResult = dataTable.Clone();//克隆表结构
            DataRow[] splitRows = null;//用于存放筛选出的符合条件的会诊信息行
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (type == "own")//本科室
                {
                    splitRows = dataTable.Select
                        (string.Format(" outhosdept='{0}' and outhosward='{1}'", m_App.User.CurrentDeptId, m_App.User.CurrentWardId));
                    if (splitRows.Length > 0)
                    {
                        for (int i = 0; i < splitRows.Length; i++)
                        {
                            dtResult.ImportRow(splitRows[i]);
                        }
                    }
                }
                if (type == "other")//其他科室 
                {
                    splitRows = dataTable.Select
                       (string.Format(" outhosdept<>'{0}' and outhosward<>'{1}'", m_App.User.CurrentDeptId, m_App.User.CurrentWardId));
                    if (splitRows.Length > 0)
                    {
                        for (int i = 0; i < splitRows.Length; i++)
                        {
                            dtResult.ImportRow(splitRows[i]);
                        }
                    }
                }
            }
            return dtResult;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            LoadPatRecordEditor();
        }

        private void barButton_View_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ViewPatBasicInfo();

        }
        /// <summary>
        /// 护理记录书写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Bl_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPatRecordEditor();
        }

        private void check_Empty_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDataOfTab1();
        }
        int oldFocusRowHandle;
        /// <summary>
        /// 设定婴儿
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetBaby_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
                return;
            DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string syxh = dataRow["NoOfInpat"].ToString();
            string patname = dataRow["PatName"].ToString();
            if ((!string.IsNullOrEmpty(syxh)) && (syxh != "-1"))
            {
                oldFocusRowHandle = gridView1.FocusedRowHandle;
                SetPatientsBaby setBaby = new SetPatientsBaby(syxh, m_App, patname, this);
                setBaby.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
                setBaby.MinimizeBox = false;
                setBaby.MaximizeBox = false;
                setBaby.ShowDialog();
            }
        }

        /// <summary>
        /// 根据checkbox勾选状态筛选数据
        /// edit by cyq 2012-08-22
        /// </summary>
        /// <param name="queryType">查询类型</param>
        /// <param name="strNurs">是否包含空床 'Y'、'N'</param>
        /// <param name="strBed"></param>
        /// <returns></returns>
        private DataTable GetPatientDataInner(int queryType, string strNurs, string strBed)
        {
            try
            {
                DataTable dataTablePatientData = new DataTable();
                string wardAndDeptRelationship = BasicSettings.GetStringConfig("WardAndDeptRelationship");
                if (wardAndDeptRelationship == "1")//1：一个病区包含多个科室  2：一个科室包含多个病区
                {
                    dataTablePatientData = GetPatientInfoData(m_App.User.CurrentDeptId, m_App.User.CurrentWardId);
                    col_ksmc.Visible = true;
                }
                else
                {
                    //读取主框架的患者信息
                    dataTablePatientData = m_App.PatientInfos.Tables[1]; //GetPatientData(queryType, strNurs, strBed);
                    col_ksmc.Visible = false;
                }

                List<string> listAttendLevel = GetAttendLevel();

                for (int i = dataTablePatientData.Rows.Count - 1; i >= 0; i--)
                {
                    if (!listAttendLevel.Contains(dataTablePatientData.Rows[i]["AttendLevel"].ToString())
                        && dataTablePatientData.Rows[i]["PatID"].ToString().Trim() != "")
                    {
                        dataTablePatientData.Rows.RemoveAt(i);
                    }
                }

                //lbl_Total.Text = dataTablePatientData.Rows.Count + " 张床位";

                return dataTablePatientData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private SqlParameter[] SqlParams
        {
            get
            {
                if (_sqlParams == null)
                {
                    _sqlParams = new SqlParameter[] { 
                  new SqlParameter("Deptids", SqlDbType.VarChar, 8),
                  new SqlParameter("Wardid", SqlDbType.VarChar, 8)
               };
                }

                return _sqlParams;
            }
        }
        private SqlParameter[] _sqlParams;

        private DataTable GetPatientInfoData(string dept, string ward)
        {
            SqlParams[0].Value = dept; // 科室代码
            SqlParams[1].Value = ward; // 病区代码
            DataTable table1 = m_App.SqlHelper.ExecuteDataTable("usp_QueryInwardPatients2", SqlParams, CommandType.StoredProcedure);
            table1.TableName = "床位信息";
            table1.Columns["BedID"].Caption = "床位";
            table1.Columns["PatName"].Caption = "患者姓名";
            table1.Columns["Sex"].Caption = "性别";
            table1.Columns["AgeStr"].Caption = "年龄";
            table1.Columns["PatID"].Caption = "住院号";
            table1.Columns["AdmitDate"].Caption = "入院日期";
            return table1;
        }

        private List<string> GetAttendLevel()
        {
            List<string> listAttendLevel = new List<string>();
            if (check_0.Checked == true)
            {
                listAttendLevel.Add(Convert.ToString((int)NurseAttendLevel.LevelSpecial));
            }
            if (check_I.Checked == true)
            {
                listAttendLevel.Add(Convert.ToString((int)NurseAttendLevel.LevelOne));
            }
            if (check_II.Checked == true)
            {
                listAttendLevel.Add(Convert.ToString((int)NurseAttendLevel.LevelTwo));
            }
            if (check_III.Checked == true)
            {
                listAttendLevel.Add(Convert.ToString((int)NurseAttendLevel.LevelThree));
            }
            return listAttendLevel;
        }

        private DataTable GetPatientData(int queryType, string strNurs, string strBed)
        {
            DataTable dataTable = new DataTable();

            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("Deptids", SqlDbType.VarChar, 8),
                  new SqlParameter("Wardid", SqlDbType.VarChar, 8),
                  new SqlParameter("QueryType", SqlDbType.Int),
                  new SqlParameter("QueryNur", SqlDbType.VarChar,50),
                  new SqlParameter("QueryBed", SqlDbType.VarChar,1)};

            sqlParams[0].Value = m_App.User.CurrentDeptId; // 科室代码
            sqlParams[1].Value = m_App.User.CurrentWardId; // 病区代码
            sqlParams[2].Value = queryType; // 
            sqlParams[3].Value = strNurs; // 
            sqlParams[4].Value = strBed; // 
            dataTable = m_App.SqlHelper.ExecuteDataTable("usp_QueryInwardPatients", sqlParams, CommandType.StoredProcedure);

            return dataTable;
        }

        private void checkHl_CheckedChanged(object sender, EventArgs e)
        {
            //cyq 2012-08-22
            //GetPatient();
            RefreshDataOfTab1();
        }

        /// <summary>
        /// 右键事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、右键小标题应无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barManager1_QueryShowPopupMenu(object sender, DevExpress.XtraBars.QueryShowPopupMenuEventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridView1.CalcHitInfo(gridControl1.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    e.Cancel = true;
                    return;
                }
                if (e.Control == gridControl1)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void textEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //edit by cyq 2013-01-21 筛选功能为所有页面一起筛选
                RefreshGridViewData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        #region 针对科室历史病人的查询处理
        /// <summary>
        /// 用于查询带入院时间段查询的信息
        /// add by ywk 2012年5月10日9:33:14
        /// 泗县修改
        /// </summary>
        /// <param name="queryType"></param>
        /// <param name="strNurs"></param>
        /// <param name="strBed"></param>
        /// <returns></returns>
        private DataTable GetPatientDataByAdmiitDate(int queryType, string strNurs, string strBed)
        {
            //读取主框架的患者信息
            DataTable dataTablePatientData = m_App.PatientInfos.Tables[1]; //GetPatientData(queryType, strNurs, strBed);

            string startdate = dateEditFrom.DateTime.ToString("yyyy-MM-dd");//入院开始时间
            string enddate = dateEditTo.DateTime.ToString("yyyy-MM-dd");//入院结束时间
            DataTable newdt = dataTablePatientData.Clone();
            DataRow[] pliterrows = dataTablePatientData.Select(string.Format(@" admitdate> 
            '{0}' and admitdate<'{1}'", startdate, enddate));
            for (int i = 0; i < pliterrows.Length; i++)
            {
                newdt.Rows.Add(pliterrows[i]);
            }
            List<string> listAttendLevel = GetAttendLevel();

            for (int i = newdt.Rows.Count - 1; i >= 0; i--)
            {
                if (!listAttendLevel.Contains(newdt.Rows[i]["AttendLevel"].ToString())
                    && newdt.Rows[i]["PatID"].ToString().Trim() != "")
                {
                    newdt.Rows.RemoveAt(i);
                }
            }

            //lbl_Total.Text = newdt.Rows.Count + " 张床位";

            return newdt;
        }
        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dateEditFrom.DateTime > this.dateEditTo.DateTime)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("开始日期不能大于结束日期");
                    this.dateEditFrom.Focus();
                    return;
                }
                m_pageIndex2 = 1;
                m_totalCount2 = 0;
                RefreshDataOfTab2();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        #region "数据刷新 by cyq 2012-08-22"
        /// <summary>
        /// 在院全部患者 数据集刷新
        /// </summary>
        public void RefreshDataOfTab1()
        {
            try
            {
                //to do 根据CHECKBOX情况传参数
                string strEmpty = check_Empty.Checked ? "Y" : "N";

                DataTable dtSource = GetPatientDataInner(3, string.Empty, strEmpty);
                allInpDataSourse = (null == dtSource || dtSource.Rows.Count == 0) ? new DataTable() : dtSource.Copy();
                string filter = string.Format(@"  yebz='0' ");
                //页面筛选
                if (!string.IsNullOrEmpty(this.textEditBedNo.Text.Trim()))
                {
                    string bedNo = this.textEditBedNo.Text.Trim().Replace("'", "''");
                    filter += string.Format(" and bedid like '%{0}%' ", bedNo);
                }
                if (!string.IsNullOrEmpty(this.textEditPatientName.Text.Trim()))
                {
                    string patiName = this.textEditPatientName.Text.Trim().Replace("'", "''");
                    filter += string.Format(" and patname like '%{0}%' ", patiName);
                }
                if (!string.IsNullOrEmpty(this.textEditPatientSN.Text.Trim()))
                {
                    string patiSN = this.textEditPatientSN.Text.Trim().Replace("'", "''");
                    filter += string.Format(" and patid like '%{0}%' ", patiSN);
                }

                if (dtSource != null)
                {
                    dtSource.DefaultView.RowFilter = filter;
                }
                DataTable newDt = dtSource.DefaultView.ToTable();
                DataTable pgTable = newDt.Clone();
                if (newDt.Rows.Count > 0)
                {
                    pgTable = OrderBy(newDt, " BEDID asc,ADMITDATE desc,PATNAME asc ").ToPagedList(m_pageIndex1, m_pageSize);
                }

                //包含婴儿
                string ResultName = string.Empty;
                for (int i = 0; i < pgTable.Rows.Count; i++)
                {
                    ResultName = DataManager.GetPatsBabyContent(m_App, pgTable.Rows[i]["noofinpat"].ToString());
                    pgTable.Rows[i]["PatName"] = ResultName;
                }

                SetPagingTip(newDt.Rows.Count);

                //加载性别图片 add by Yanqiao.Cai 2012-11-15
                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListBrxb);

                gridControl1.DataSource = pgTable;
                //if (null == pgTable || pgTable.Rows.Count == 0)
                //{
                //Common.Ctrs.DLG.MessageBox.Show("没有符合条件的记录");
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 科室病人历史 数据集刷新
        /// </summary>
        private void RefreshDataOfTab2()
        {
            try
            {
                string deptID = string.Empty;
                string wardID = string.Empty;
                deptID = m_App.User.CurrentDeptId;
                wardID = m_App.User.CurrentWardId;
                DataTable dtSource = this.GetHistoryPat(1, deptID, wardID);

                //add by ywk  2012年11月27日9:04:32  针对母亲出院婴儿未显示出来的修改
                string ResultName = string.Empty;//声明最终要在列表显示的姓名的内容
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    ResultName = DataManager.GetPatsBabyContent(m_App, dtSource.Rows[i]["noofinpat"].ToString());
                    dtSource.Rows[i]["PatName"] = ResultName;
                }

                string filter = string.Format(@"  yebz='0' ");
                if (dtSource != null && dtSource.Rows.Count > 0)//判断条件要加全  add by ywk  2012年11月21日13:41:01
                {
                    dtSource.DefaultView.RowFilter = filter;
                }
                DataTable newDt = dtSource.DefaultView.ToTable();


                SetPagingTip(newDt.Rows.Count);
                DataRow[] drs = OrderBy(newDt, "BEDID asc,ADMITDATE desc,PATNAME asc");

                //加载性别图片 add by Yanqiao.Cai 2012-11-15
                DS_Common.InitializeImage_XB(repositoryItemImageHistoryXB, imageListBrxb);

                this.gridControl2.DataSource = null == drs ? new DataTable() : drs.ToPagedList(m_pageIndex2, m_pageSize);

                if (drs == null || drs.Count() == 0)//判断要加全  add by ywk  2012年11月21日13:41:11 
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有符合条件的记录");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 查询在院全部患者
        /// </summary>
        /// <param name="queryType"></param>
        /// <param name="deptID"></param>
        /// <param name="wardID"></param>
        /// <returns></returns>
        private DataTable GetHistoryPat(int queryType, string deptID, string wardID)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@Wardid", SqlDbType.VarChar, 8),
                  new SqlParameter("@Deptids",SqlDbType.VarChar, 8),
                  new SqlParameter("@TimeFrom",SqlDbType.VarChar,10),
                  new SqlParameter("@TimeTo",SqlDbType.VarChar,10),
                  new SqlParameter("@PatientSN",SqlDbType.VarChar,32),
                  new SqlParameter("@Name",SqlDbType.VarChar,32),
                  new SqlParameter("@QueryType", SqlDbType.Int)
                  };
            sqlParams[0].Value = wardID;
            sqlParams[1].Value = deptID;
            sqlParams[2].Value = this.dateEditFrom.DateTime.ToString("yyyy-MM-dd");
            sqlParams[3].Value = this.dateEditTo.DateTime.ToString("yyyy-MM-dd");
            sqlParams[4].Value = this.textEdit2.Text.Trim();
            sqlParams[5].Value = this.textEdit1.Text.Trim();
            sqlParams[6].Value = queryType;
            DataTable dataTable = m_App.SqlHelper.ExecuteDataTable("usp_QueryQuitPatientNoDoctor", sqlParams, CommandType.StoredProcedure);
            return dataTable;
        }
        /// <summary>
        /// 双击事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、双击小标题应无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridView2.CalcHitInfo(gridControl2.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                if (gridView2.FocusedRowHandle < 0) { return; }
                LoadPatRecordEditor1("grd2");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 针对两个列表双击都进文书录入
        /// </summary>
        /// <param name="gridviewid"> 标识列表</param>
        /// <returns></returns>
        private decimal FindFocusedPat1(string gridviewid)
        {
            decimal syxh = -1;
            if (gridviewid == "grd2")
            {
                int focusedRowHandle = gridView2.FocusedRowHandle;
                if (focusedRowHandle >= 0)
                {
                    DataRow focusedRow = gridView2.GetDataRow(focusedRowHandle);
                    if (!String.IsNullOrEmpty(focusedRow["NoOfInpat"].ToString()))
                        syxh = Convert.ToDecimal(focusedRow["NoOfInpat"]);
                }
            }
            if (gridviewid == "grd1")
            {
                int focusedRowHandle = gridView1.FocusedRowHandle;
                if (focusedRowHandle >= 0)
                {
                    DataRow focusedRow = gridView1.GetDataRow(focusedRowHandle);
                    if (!String.IsNullOrEmpty(focusedRow["NoOfInpat"].ToString()))
                        syxh = Convert.ToDecimal(focusedRow["NoOfInpat"]);
                }
            }

            return syxh;

        }


        /// <summary>
        /// 加载病历编辑器
        /// </summary>
        private void LoadPatRecordEditor1(string gridviewid)
        {
            Decimal syxh = FindFocusedPat1(gridviewid);
            if (syxh < 0) return;


            //add by ywk 针对有婴儿的双机处理 2012年11月27日9:15:12
            if (DataManager.HasBaby(syxh.ToString()))
            {
                DrectSoft.Core.OwnBedInfo.ChoosePatOrBaby choosepat = new DrectSoft.Core.OwnBedInfo.ChoosePatOrBaby(m_App, syxh.ToString());
                choosepat.StartPosition = FormStartPosition.CenterParent;
                if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_App.ChoosePatient(decimal.Parse(choosepat.NOOfINPAT));
                    m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                }
            }
            else
            {
                m_App.ChoosePatient(syxh);
                m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
            }
        }

        #endregion

        private void RefreshGridViewData()
        {
            try
            {
                if (null == allInpDataSourse || allInpDataSourse.Rows.Count == 0)
                {
                    RefreshDataOfTab1();
                }
                else
                {
                    DataTable dt = allInpDataSourse.Copy();
                    string filter = string.Format(@"  yebz='0' ");
                    //页面筛选
                    if (!string.IsNullOrEmpty(this.textEditBedNo.Text.Trim()))
                    {
                        string bedNo = this.textEditBedNo.Text.Trim().Replace("'", "''");
                        filter += string.Format(" and bedid like '%{0}%' ", bedNo);
                    }
                    if (!string.IsNullOrEmpty(this.textEditPatientName.Text.Trim()))
                    {
                        string patiName = this.textEditPatientName.Text.Trim().Replace("'", "''");
                        filter += string.Format(" and patname like '%{0}%' ", patiName);
                    }
                    if (!string.IsNullOrEmpty(this.textEditPatientSN.Text.Trim()))
                    {
                        string patiSN = this.textEditPatientSN.Text.Trim().Replace("'", "''");
                        filter += string.Format(" and patid like '%{0}%' ", patiSN);
                    }

                    DataTable filterDt = dt.Clone();
                    if (allInpDataSourse != null && allInpDataSourse.Rows.Count > 0)
                    {
                        DataRow[] rows = dt.Select(filter);
                        filterDt = (null == rows || rows.Length == 0) ? dt.Clone() : rows.CopyToDataTable();
                        SetPagingTip(null == filterDt ? 0 : filterDt.Rows.Count);
                        if (null != filterDt && filterDt.Rows.Count > 0)
                        {
                            m_pageIndex1 = 1;
                            filterDt = OrderBy(filterDt, " BEDID asc,ADMITDATE desc,PATNAME asc ").ToPagedList(m_pageIndex1, m_pageSize);
                        }
                    }

                    gridControl1.DataSource = filterDt;
                }
                //string filter = " patid like '%{0}%' and patname like '%{1}%' and bedid like '%{2}%' ";
                //DataTable dt = gridControl1.DataSource as DataTable;
                //if (dt != null)
                //{
                //    dt.DefaultView.RowFilter = string.Format(filter, textEditPatientSN.Text.Trim(), textEditPatientName.Text.Trim(), textEditBedNo.Text.Trim());
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 护理级别
        /// </summary>
        enum NurseAttendLevel : int
        {
            LevelOne = 6101, //一级护理
            LevelTwo = 6102, //二级护理
            LevelThree = 6103,  //三级护理
            LevelSpecial = 6104 //特级护理
        }

        /// <summary>
        /// 设置单元格颜色（后期微调）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //this.gridView1.RefreshData();
            if (e.RowHandle >= 0)
            {
                /*
                //e.Appearance.a
                DataRow foucesRow = gridConsulationView.GetDataRow(gridConsulationView.FocusedRowHandle);
                if (foucesRow != null)
                {
                    string noofinpat = foucesRow["NOOFINPAT"].ToString();//取得病人编号
                    DataTable dt = this.gridControl1.DataSource as DataTable;//取得左侧大列表的数据源
                    DataRowView drv = gridView1.GetRow(e.RowHandle) as DataRowView;
                    string value = drv["NOOFINPAT"].ToString().Trim();
                    if (value == noofinpat)
                    {
                        //e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);
                        //取得病人名字
                        string patname = drv["patname"].ToString().Trim();

                        if (e.Column == col_Hzxm)
                        {
                            if (patname.Contains("婴儿"))//既在右侧会诊列表选中了，又有婴儿的名字会被覆盖住 edit by ywk 
                            {
                                e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                                e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red, e.Bounds, s);
                                e.Handled = true;
                            }
                        }
                        else
                        {
                            e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds);//LightCyan
                        }
                    }
                }
                */
                s.Alignment = StringAlignment.Near;
                s.LineAlignment = StringAlignment.Center;
                if (e.RowHandle == gridView1.FocusedRowHandle)
                {
                    DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    string patname = foucesRow["patname"].ToString().Trim();

                    if (e.Column == col_Hzxm)
                    {
                        if (patname.Contains("婴儿"))//既在右侧会诊列表选中了，又有婴儿的名字会被覆盖住 edit by ywk 
                        {
                            Region oldRegion = e.Graphics.Clip;
                            e.Graphics.Clip = new Region(e.Bounds);

                            e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                            e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red,
                                new RectangleF(e.Bounds.Location, new SizeF(300, e.Bounds.Height)), s);

                            e.Graphics.Clip = oldRegion;
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds);//LightCyan
                    }
                }
            }
        }
        /// <summary>
        /// 设置单元格颜色（后期微调）
        /// 针对历史病人查询出有婴儿的情况处理
        /// add by ywk  2012年11月27日9:18:54
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                s.Alignment = StringAlignment.Near;
                s.LineAlignment = StringAlignment.Center;
                if (e.CellValue == null)
                {
                    return;
                }
                DataRowView drv = gridView2.GetRow(e.RowHandle) as DataRowView;
                //取得病人名字
                string patname = drv["patname"].ToString().Trim();
                if (e.Column.FieldName == gridColumn14.FieldName)
                {
                    if (patname.Contains("婴儿"))
                    {
                        Region oldRegion = e.Graphics.Clip;
                        e.Graphics.Clip = new Region(e.Bounds);

                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                        e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red,
                            new RectangleF(e.Bounds.Location, new SizeF(300, e.Bounds.Height)), s);

                        e.Graphics.Clip = oldRegion;
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        StringFormat sf = new StringFormat();
        private void gridView1_CustomDrawCell_1(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            if (e.CellValue == null) return;
            DataRowView drv = gridView1.GetRow(e.RowHandle) as DataRowView;
            //取得病人名字
            string patname = drv["patname"].ToString().Trim();

            if (e.Column == col_Hzxm)
            {
                if (patname.Contains("婴儿"))
                {
                    Region oldRegion = e.Graphics.Clip;
                    e.Graphics.Clip = new Region(e.Bounds);

                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                    e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red,
                        new RectangleF(e.Bounds.Location, new SizeF(300, e.Bounds.Height)), s);

                    e.Graphics.Clip = oldRegion;
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// 双击事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、双击小标题应无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridView1.CalcHitInfo(gridControl1.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                if (gridView1.FocusedRowHandle < 0) { return; }
                //LoadPatRecordEditor1("grd1");
                LoadPatRecordEditor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 从设定婴儿回来，要刷新相应的数据
        /// add by ywk  2012年7月26日11:55:52
        /// </summary>
        /// <param name="EditedPats"></param>
        internal void RefreshPat(ArrayList EditedPats)
        {
            DataTable dataTableOper = gridControl1.DataSource as DataTable;
            dataTableOper = dataTableOper.Copy();
            string ResultName = string.Empty;
            string m_noofinpat = string.Empty;
            for (int i = 0; i < dataTableOper.Rows.Count; i++)
            {
                m_noofinpat = dataTableOper.Rows[i]["Noofinpat"].ToString();

                for (int j = 0; j < EditedPats.Count; j++)
                {
                    if (EditedPats[j].ToString() == m_noofinpat)
                    {
                        ResultName = DataManager.GetPatsBabyContent(m_App, m_noofinpat);
                        dataTableOper.Rows[i]["PatName"] = ResultName;
                    }
                }
            }
            this.gridControl1.DataSource = dataTableOper;
            this.gridView1.FocusedRowHandle = oldFocusRowHandle;
        }

        #region "分页 by cyq 2012-08-21"
        //首页
        private void lkl_pageFirst_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
                {
                    m_pageIndex1 = 1;
                    RefreshDataOfTab1();
                }
                else
                {
                    m_pageIndex2 = 1;
                    RefreshDataOfTab2();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        //上一页
        private void lkl_pagePre_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
                {
                    if (m_pageIndex1 > 1)
                    {
                        m_pageIndex1 -= 1;
                        RefreshDataOfTab1();
                    }
                }
                else
                {
                    if (m_pageIndex2 > 1)
                    {
                        m_pageIndex2 -= 1;
                        RefreshDataOfTab2();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        //下一页
        private void lkl_pageNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                int pageCount;
                if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
                {
                    pageCount = (int)Math.Ceiling(m_totalCount1 / Double.Parse(m_pageSize.ToString() + ".00"));
                    if (m_pageIndex1 < pageCount)
                    {
                        m_pageIndex1 += 1;
                        RefreshDataOfTab1();
                    }
                }
                else
                {
                    pageCount = (int)Math.Ceiling(m_totalCount2 / Double.Parse(m_pageSize.ToString() + ".00"));
                    if (m_pageIndex2 < pageCount)
                    {
                        m_pageIndex2 += 1;
                        RefreshDataOfTab2();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        //末页
        private void lkl_pageLast_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
                {
                    m_pageIndex1 = (int)Math.Ceiling(m_totalCount1 / Double.Parse(m_pageSize.ToString() + ".00"));
                    RefreshDataOfTab1();
                }
                else
                {
                    m_pageIndex2 = (int)Math.Ceiling(m_totalCount2 / Double.Parse(m_pageSize.ToString() + ".00"));
                    RefreshDataOfTab2();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        //页号回车事件
        private void txt_pageNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string pageNoStr = this.txt_pageNo.Text;
                    //判断输入值是否为数字
                    if (!Tools.IsNumeric(pageNoStr) || pageNoStr.Contains("."))
                    {
                        MessageBox.Show("您输入的不是整数，请重新输入");
                        this.txt_pageNo.Text = xtraTabControl1.SelectedTabPage == xtraTabPage1 ? m_pageIndex1.ToString() : m_pageIndex2.ToString();
                        this.txt_pageNo.Focus();
                        return;
                    }

                    int pageNo = int.Parse(this.txt_pageNo.Text);
                    int pageCount;
                    if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
                    {
                        pageCount = (int)Math.Ceiling(m_totalCount1 / Double.Parse(m_pageSize.ToString() + ".00"));
                        if (pageNo > 0 && pageNo <= pageCount)
                        {
                            m_pageIndex1 = pageNo;
                            RefreshDataOfTab1();
                        }
                        else
                        {
                            MessageBox.Show("您输入数字不在页码范围内，请重新输入");
                            this.txt_pageNo.Text = m_pageIndex1.ToString();
                            this.txt_pageNo.Focus();
                            return;
                        }
                    }
                    else
                    {
                        pageCount = (int)Math.Ceiling(m_totalCount2 / Double.Parse(m_pageSize.ToString() + ".00"));
                        if (pageNo > 0 && pageNo <= pageCount)
                        {
                            m_pageIndex2 = pageNo;
                            RefreshDataOfTab2();
                        }
                        else
                        {
                            MessageBox.Show("您输入数字不在页码范围内，请重新输入");
                            this.txt_pageNo.Text = m_pageIndex2.ToString();
                            this.txt_pageNo.Focus();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        //PageSize变化事件
        private void lue_pageSize_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.lue_pageSize.CodeValue))
                {
                    this.lue_pageSize.CodeValue = null == m_pageSize ? DefaultPageSize(GetDataOfPageSize()) : m_pageSize.ToString();
                    return;
                }
                m_pageSize = int.Parse(this.lue_pageSize.CodeValue);
                if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
                {
                    if (m_pageIndex1 > (int)Math.Ceiling(m_totalCount1 / Double.Parse(m_pageSize.ToString() + ".00")))
                    {
                        m_pageIndex1 = 1;
                    }
                    RefreshDataOfTab1();
                }
                else
                {
                    if (m_pageIndex2 > (int)Math.Ceiling(m_totalCount2 / Double.Parse(m_pageSize.ToString() + ".00")))
                    {
                        m_pageIndex2 = 1;
                    }
                    RefreshDataOfTab2();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        //初始化PageSize
        private void InitPageSize()
        {
            try
            {
                this.lookUpWindowPageSize.SqlHelper = m_App.SqlHelper;
                DataTable dataTable = GetDataOfPageSize();
                dataTable.Columns["ID"].Caption = "条数";
                Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
                columnwidth.Add("ID", 45);
                SqlWordbook sqlWordBook = new SqlWordbook("querybook", dataTable, "ID", "ID", columnwidth, "ID");
                lue_pageSize.SqlWordbook = sqlWordBook;
                //默认选中离页码30最近的数字
                lue_pageSize.CodeValue = DefaultPageSize(dataTable);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //PageSize数据集
        private DataTable GetDataOfPageSize()
        {
            try
            {
                string config = DS_SqlService.GetConfigValueByKey("PageSizeConfig");
                DataTable dtable = new DataTable();
                dtable.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
                if (!string.IsNullOrEmpty(config))
                {
                    string[] str = config.Split(',');
                    foreach (string s in str)
                    {
                        dtable.Rows.Add(s);
                    }
                }
                return dtable.Select(" 1=1 ", " ID asc ").CopyToDataTable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取离该数字最近的页码数
        /// </summary>
        /// <param name="dt">页码数据集</param>
        /// <param name="aroundSize">数字</param>
        /// <returns></returns>
        public string DefaultPageSize(DataTable dataTable)
        {
            try
            {
                string aroundSizeStr = DS_SqlService.GetConfigValueByKey("PageSizeValueConfig");
                int aroundSize = string.IsNullOrEmpty(aroundSizeStr) ? 30 : int.Parse(aroundSizeStr);
                string selectValue = string.Empty;
                var listEnu = dataTable.Select(" 1=1 ");
                if (listEnu.Any(p => p["ID"].ToString() == aroundSize.ToString()))
                {
                    selectValue = aroundSize.ToString();
                }
                else
                {
                    var bigList = listEnu.Where(p => int.Parse(p["ID"].ToString()) > aroundSize);
                    if (bigList.Count() > 0)
                    {
                        var smallList = listEnu.Where(p => int.Parse(p["ID"].ToString()) < aroundSize);
                        int bigValue = int.Parse(bigList.FirstOrDefault()["ID"].ToString());
                        int smallValue = int.Parse(smallList.LastOrDefault()["ID"].ToString());
                        if (bigValue - aroundSize < aroundSize - smallValue)
                        {
                            selectValue = bigValue.ToString();
                        }
                        else
                        {
                            selectValue = smallValue.ToString();
                        }

                    }
                    else
                    {
                        selectValue = dataTable.Rows[dataTable.Rows.Count - 1]["ID"].ToString();
                    }
                }

                return selectValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //分页后显示设置
        private void SetPagingTip(int totalCount)
        {
            try
            {
                int? m_pageIndex;
                //总页数
                int pageCount = (int)Math.Ceiling(totalCount / Double.Parse(m_pageSize.ToString() + ".00"));
                if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
                {
                    m_pageIndex1 = totalCount == 0 ? null : (m_pageIndex1 > pageCount ? 1 : m_pageIndex1);
                    m_pageIndex = m_pageIndex1;
                    m_totalCount1 = totalCount;
                }
                else
                {
                    m_pageIndex2 = totalCount == 0 ? null : (m_pageIndex2 > pageCount ? 1 : m_pageIndex2);
                    m_pageIndex = m_pageIndex2;
                    m_totalCount2 = totalCount;
                }
                string pageNote1 = "共 ";
                string pageNote2 = " 条，共 ";
                pageNote1 += totalCount.ToString() + " 条记录，每页 ";
                pageNote2 += pageCount.ToString() + " 页";
                this.lbl_pageNote1.Text = pageNote1;
                this.lbl_pageNote2.Text = pageNote2;
                this.txt_pageNo.Text = totalCount == 0 ? "" : m_pageIndex.ToString();
                //设置分页label位置
                lbl_pageNote1.Location = new Point(lue_pageSize.Location.X - 110 - (totalCount.ToString().Length - 1) * 7, lbl_pageNote1.Location.Y);

                //设置分页按钮(链接)是否可用
                if (totalCount == 0 || pageCount <= 1)
                {//总记录数为0或者总页数为1
                    this.lkl_pageFirst.Enabled = false;
                    this.lkl_pagePre.Enabled = false;
                    this.lkl_pageNext.Enabled = false;
                    this.lkl_pageLast.Enabled = false;
                    this.txt_pageNo.Enabled = false;
                    this.lue_pageSize.Enabled = totalCount != 0;
                    this.lkl_pageFirst.LinkColor = Color.DarkGray;
                    this.lkl_pagePre.LinkColor = Color.DarkGray;
                    this.lkl_pageNext.LinkColor = Color.DarkGray;
                    this.lkl_pageLast.LinkColor = Color.DarkGray;
                }
                else if (m_pageIndex == 1)
                {//第一页时
                    this.lkl_pageFirst.LinkColor = Color.DarkGray;
                    this.lkl_pageFirst.Enabled = false;
                    this.lkl_pagePre.LinkColor = Color.DarkGray;
                    this.lkl_pagePre.Enabled = false;

                    this.txt_pageNo.Enabled = true;
                    this.lue_pageSize.Enabled = true;
                    this.lkl_pageNext.LinkColor = Color.Blue;
                    this.lkl_pageNext.Enabled = true;
                    this.lkl_pageLast.LinkColor = Color.Blue;
                    this.lkl_pageLast.Enabled = true;
                }
                else if (m_pageIndex == pageCount)
                {//最后一页时
                    this.lkl_pageNext.LinkColor = Color.DarkGray;
                    this.lkl_pageNext.Enabled = false;
                    this.lkl_pageLast.LinkColor = Color.DarkGray;
                    this.lkl_pageLast.Enabled = false;

                    this.txt_pageNo.Enabled = true;
                    this.lue_pageSize.Enabled = true;
                    this.lkl_pageFirst.LinkColor = Color.Blue;
                    this.lkl_pageFirst.Enabled = true;
                    this.lkl_pagePre.LinkColor = Color.Blue;
                    this.lkl_pagePre.Enabled = true;
                }
                else
                {
                    this.lkl_pageFirst.Enabled = true;
                    this.lkl_pagePre.Enabled = true;
                    this.lkl_pageNext.Enabled = true;
                    this.lkl_pageLast.Enabled = true;
                    this.txt_pageNo.Enabled = true;
                    this.lue_pageSize.Enabled = true;
                    this.lkl_pageFirst.LinkColor = Color.Blue;
                    this.lkl_pagePre.LinkColor = Color.Blue;
                    this.lkl_pageNext.LinkColor = Color.Blue;
                    this.lkl_pageLast.LinkColor = Color.Blue;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        private DataRow[] OrderBy(DataTable dt, string orderBy)
        {
            try
            {
                if (dt.Rows.Count > 0)//如果此DataTable中有数据
                {
                    return dt.Select(" 1=1 ", orderBy);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //tab页切换事件
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
                {
                    panelControl8.Visible = true;
                    SetPagingTip(m_totalCount1);
                    DataTable dtable = gridControl1.DataSource as DataTable;
                    if (null != dtable && dtable.Rows.Count != m_pageSize)
                    {
                        RefreshDataOfTab1();
                    }
                    textEditBedNo.Focus();
                }
                //xll 添加病历补写功能
                else if (xtraTabControl1.SelectedTabPage == xtraTabPage2)
                {
                    panelControl8.Visible = true;
                    SetPagingTip(m_totalCount2);
                    DataTable dtable = gridControl2.DataSource as DataTable;
                    if (null != dtable && dtable.Rows.Count != m_pageSize)
                    {
                        RefreshDataOfTab2();
                    }
                    textEdit2.Focus();
                }
                else if (xtraTabControl1.SelectedTabPage == tabBuxie)
                {
                    panelControl8.Visible = false;
                    dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                    //Replenish.Controls.Clear();
                    if (tabBuxie.Controls.Count == 0)
                    {
                        repl = new ReplenishPatRec();
                        repl.Dock = DockStyle.Fill;
                        repl.AutoSize = false;

                        tabBuxie.Controls.Add(repl);
                    }
                    repl.LoadData(m_App);
                }
                else if (xtraTabControl1.SelectedTabPage == paintTran)
                {
                    panelControl8.Visible = false;
                    dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                    //Replenish.Controls.Clear();
                    if (paintTran.Controls.Count == 0)
                    {
                        uctran = new UCTran(m_App);
                        uctran.Dock = DockStyle.Fill;
                        uctran.AutoSize = false;

                        paintTran.Controls.Add(uctran);
                    }

                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 病人出院
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChuYuan_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                    return;
                DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                string syxh = dataRow["NoOfInpat"].ToString();
                string patname = dataRow["PatName"].ToString();
                if ((!string.IsNullOrEmpty(syxh)))
                {
                    DialogResult dResult = m_App.CustomMessageBox.MessageShow("确定让病人出院吗？", CustomMessageBoxKind.QuestionYesNo);
                    if (dResult == DialogResult.Yes)
                    {
                        string sql = "update inpatient i set i.status=1503,i.outhosdept=@outhostdept, i.outhosward=@outhostward,i.outwarddate=@outwarddate,i.outhosdate=@outhostdate,i.emrouthos='1' where i.noofinpat=@noofinpat";
                        SqlParameter[] sps ={
                                               new SqlParameter("@outhostdept",m_App.User.CurrentDeptId),
                                               new SqlParameter("@outhostward",m_App.User.CurrentWardId),
                                               new SqlParameter("@outwarddate",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                               new SqlParameter("@outhostdate",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                               new SqlParameter("@noofinpat",Convert.ToInt32(syxh))
                                            };
                        m_App.SqlHelper.ExecuteNoneQuery(sql, sps, CommandType.Text);
                        RefreshDataOfTab1();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnZhuanke_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {

                if (gridView1.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                string syxh = dataRow["NoOfInpat"].ToString();
                ZhuangKeForm ZhuangKeForm = new ZhuangKeForm(m_App, syxh);
                DialogResult diaresult = ZhuangKeForm.ShowDialog();
                if (diaresult == DialogResult.Yes)
                {
                    RefreshDataOfTab1();
                    ZhuangKeForm.Close();
                }
                else if (diaresult == DialogResult.No)
                {
                    ZhuangKeForm.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-22</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    DS_Common.cbx_KeyPress(sender);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 序号 - 在院全部患者
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-25</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 序号 - 科室历史病人查询
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-25</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 重置事件 - 在院全部病人
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset1_Click(object sender, EventArgs e)
        {
            try
            {
                Reset1();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 重置事件 - 科室历史病人查询
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset2_Click(object sender, EventArgs e)
        {
            try
            {
                Reset2();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 重置方法 - 在院全部病人
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        private void Reset1()
        {
            try
            {
                this.textEditPatientSN.Text = string.Empty;
                this.textEditPatientName.Text = string.Empty;
                this.textEditBedNo.Text = string.Empty;

                check_0.Checked = true;
                check_I.Checked = true;
                check_II.Checked = true;
                check_III.Checked = true;
                check_Default.Checked = true;
                check_Default.Visible = false;
                check_Empty.Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重置方法 - 科室历史病人查询
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        private void Reset2()
        {
            try
            {
                this.textEdit2.Text = string.Empty;
                this.textEdit1.Text = string.Empty;

                this.dateEditFrom.DateTime = DateTime.Now.AddMonths(-1);
                this.dateEditTo.DateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Add by wwj 2013-02-18 初始化会诊信息
        /// </summary>
        private void InitConsultSetting()
        {
            try
            {
                UserControlConsultation ucConsultaion = new UserControlConsultation(m_App, navBarGroupConsultInfo, this, this.gridView1);
                ucConsultaion.Dock = DockStyle.Fill;
                navBarGroupControlContainer5.Controls.Add(ucConsultaion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

