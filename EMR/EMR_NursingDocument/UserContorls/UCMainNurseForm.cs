using DevExpress.Utils;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Common.Eop;
using DrectSoft.Core.EMR_NursingDocument.PublicSet;
using DrectSoft.Core.OwnBedInfo;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.EMR_NursingDocument.UserContorls
{
    public partial class UCMainNurseForm : UserControl, IEMREditor
    {

        #region 属性和字段
        private UCNusreRecordMain m_UCNusreRecordMain;

        /// <summary>
        /// 参数相关
        /// </summary>
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
        /// <summary>
        /// 提示框窗体
        /// </summary>
        private WaitDialogForm m_WaitDialog;
        private IEmrHost m_app;
        internal IEmrHost App
        {
            get { return m_app; }
        }
        public Inpatient CurrentPat
        {
            get
            {
                if (_currentPat == null)
                {
                    _currentPat = m_app.CurrentPatientInfo;
                }
                return _currentPat;
            }
            set
            {
                //if (!object.ReferenceEquals(_currentPat, value))
                //{
                _currentPat = value;
                InitMeasureTableData();
                RefreshSelectedTabPages();
                //}
            }
        }
        /// <summary>
        ///由护士工作站跳到此窗体带来的病人的首页序号
        ///add by ywk 2012年8月7日 13:58:35
        /// </summary>
        public string NoOfinpat { get; set; }
        private Inpatient _currentPat = null;
        #endregion

        #region 方法
        /// <summary>
        /// 初始窗体
        /// </summary>
        private void InitForm()
        {
            InitNursDocuTree();
            AddEvents();
            this.Text = "三测表";
            SetNewButtonEnable();
            InitTreeExpand(treeList1.Nodes);
        }
        /// <summary>
        /// 控制按钮可编辑性
        /// </summary>
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
        /// <summary>
        /// 对应的事件
        /// </summary>
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

        /// <summary>
        /// 绑定病人列表
        /// add by ywk 2012年8月7日 14:58:24 
        /// 此处的病人要选中上个窗体选择的病人
        /// </summary>
        private void BindPatInfos()
        {
            DataTable dtSource = GetPatientDataInner(3, string.Empty, "N");
            string filter = string.Format(@"  yebz='0' ");
            //gridMain.DataSource = m_DataManager.TableWard;
            if (dtSource != null)
            {
                dtSource.DefaultView.RowFilter = filter;
            }
            //处理含有婴儿的情况  ywk
            DataTable newDt = dtSource.DefaultView.ToTable();
            string ResultName = string.Empty;//声明最终要在列表显示的姓名的内容
            for (int i = 0; i < newDt.Rows.Count; i++)
            {
                ResultName = DataManager.GetPatsBabyContent(m_app, newDt.Rows[i]["noofinpat"].ToString());
                newDt.Rows[i]["PatName"] = ResultName;
            }
            this.gridControl1.DataSource = newDt;
        }

        private DataTable GetPatientDataInner(int queryType, string strNurs, string strBed)
        {
            DataTable dataTablePatientData = new DataTable();
            string wardAndDeptRelationship = BasicSettings.GetStringConfig("WardAndDeptRelationship");
            if (wardAndDeptRelationship == "1")//1：一个病区包含多个科室  2：一个科室包含多个病区
            {
                dataTablePatientData = GetPatientInfoData(m_app.User.CurrentDeptId, m_app.User.CurrentWardId);
                //col_ksmc.Visible = true;
            }
            else
            {
                //读取主框架的患者信息
                dataTablePatientData = m_app.PatientInfos.Tables[1]; //GetPatientData(queryType, strNurs, strBed);
                //col_ksmc.Visible = false;
            }

            //List<string> listAttendLevel = GetAttendLevel();

            //for (int i = dataTablePatientData.Rows.Count - 1; i >= 0; i--)
            //{
            //    if (!listAttendLevel.Contains(dataTablePatientData.Rows[i]["AttendLevel"].ToString())
            //        && dataTablePatientData.Rows[i]["PatID"].ToString().Trim() != "")
            //    {
            //        dataTablePatientData.Rows.RemoveAt(i);
            //    }
            //}

            //lbl_Total.Text = dataTablePatientData.Rows.Count + " 张床位";

            return dataTablePatientData;
        }
        /// <summary>
        /// 控制按钮
        /// </summary>
        private void SetButtonEnable()
        {
            this.simpleButtonLastWeek.Enabled = this.ucThreeMeasureTable1.DateTimeLogicForLastWeek();
        }
        private string m_DefaultRowFilter = string.Empty;
        /// <summary>
        /// 刷新病人列表控件
        /// </summary>
        private void refreshGridView()
        {
            string filter = string.Format(" PATID like '%{0}%' or PatName like '%{0}%' or BedID like '%{0}%' ", txtSearch.Text.Trim());
            //DataView dv = gridMain.DataSource as DataView;
            DataTable dt = gridControl1.DataSource as DataTable;
            if (dt != null)
            {
                dt.DefaultView.RowFilter = filter;
            }
        }

        private DataTable GetPatientInfoData(string dept, string ward)
        {
            SqlParams[0].Value = dept; // 科室代码
            SqlParams[1].Value = ward; // 病区代码
            DataTable table1 = m_app.SqlHelper.ExecuteDataTable("usp_QueryInwardPatients2", SqlParams, CommandType.StoredProcedure);
            table1.TableName = "床位信息";
            table1.Columns["BedID"].Caption = "床位";
            table1.Columns["PatName"].Caption = "患者姓名";
            table1.Columns["Sex"].Caption = "性别";
            table1.Columns["AgeStr"].Caption = "年龄";
            table1.Columns["PatID"].Caption = "住院号";
            table1.Columns["AdmitDate"].Caption = "入院日期";
            return table1;
        }

        /// <summary>
        /// 切换病人
        /// </summary>
        internal void PatientChanging()
        {
            CloseAllTabPages();
        }

        internal void PatientChanged(Inpatient inpatient)
        {
            CurrentPat = inpatient;
            CurrentPat.ReInitializeAllProperties();
        }
        /// <summary>
        /// 初始化三测表中的数据
        /// </summary>
        private void InitMeasureTableData()
        {
            //if (CurrentPat != null)
            //{
            SetWaitDialogCaption("正在读取患者信息");

            MethodSet.CurrentInPatient = CurrentPat;
            PublicSet.MethodSet.App = m_app;

            DataTable patientInfo = PublicSet.MethodSet.GetPatientInfoForThreeMeasureTable(MethodSet.CurrentInPatient.NoOfFirstPage);
            SetWaitDialogCaption("正在绘制三测单");
            this.ucThreeMeasureTable1.SetPatientInfo(patientInfo, m_app.SqlHelper);
            this.ucThreeMeasureTable1.LoadData();
            HideWaitDialog();
            this.ucThreeMeasureTable1.Load += new EventHandler(UCMainNurseForm_Load_1);
            //}
        }

        /// <summary>
        /// 双击左侧病人列表。右侧的三测单切换到相应的人
        /// addby ywk 2012年8月8日 14:22:20 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                //m_app.CustomMessageBox.MessageShow("未选择会诊单据！");
                return;
            }
            DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (foucesRow == null)
            {
                return;
            }
            if (foucesRow.IsNull("NOOFINPAT"))
            {
                return;
            }
            string noofinpat = foucesRow["NOOFINPAT"].ToString();
            decimal syxh = Convert.ToDecimal(noofinpat);

            if (DataManager.HasBaby(noofinpat))
            {
                ChoosePatOrBaby choosepat = new ChoosePatOrBaby(m_app, noofinpat);
                choosepat.StartPosition = FormStartPosition.CenterParent;
                if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    syxh = Convert.ToDecimal(choosepat.NOOfINPAT);
                }
            }

            DataTable patientInfo = PublicSet.MethodSet.GetPatientInfoForThreeMeasureTable(syxh);
            //SetWaitDialogCaption("正在绘制三测单");
            this.ucThreeMeasureTable1.SetPatientInfo(patientInfo, m_app.SqlHelper);
            this.ucThreeMeasureTable1.LoadData();
            this.ucThreeMeasureTable1.Refresh();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCMainNurseForm()
        {
            InitializeComponent();

        }
        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="app"></param>
        public UCMainNurseForm(IEmrHost app)
        {
            m_app = app;
            PublicSet.MethodSet.App = app;
            //InitForm();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="noofinpat"></param>
        public UCMainNurseForm(string noofinpat)
            : this()
        {
            CurrentNoofinpat = NoOfinpat;

        }
        /// <summary>
        /// 关闭所有Tab窗体
        /// </summary>
        public void CloseAllTabPages()
        {
            //for (int i = xtraTabControl1.TabPages.Count - 1; i >= 0; i--)
            //{
            //    XtraTabPage page = xtraTabControl1.TabPages[i];
            //    xtraTabControl1.SelectedTabPage = page;

            //    if (CurrentForm != null)
            //    {
            //        XtraTabControlCloseButton();
            //    }
            //    else if (page.Controls.Count > 0 && page.Controls[0] is IEMREditor)
            //    {
            //        //IEMREditor emrEditor = page.Controls[0] as IEMREditor;
            //        //emrEditor.Save();
            //    }
            //}
        }
        public void SetInnerVar(IEmrHost app, DrectSoft.Emr.Util.RecordDal m_RecordDal)
        {
            m_app = app;
            //m_RecordDal = m_RecordDal;
            m_app.CurrentPatientInfo.NoOfFirstPage = app.CurrentPatientInfo.NoOfFirstPage;
            InitForm();
        }
        #endregion

        #region 事件
        StringFormat sf = new StringFormat();
        StringFormat s = new StringFormat();

        /// <summary>
        /// 控制有婴儿和无婴儿的显示情况
        /// add by ywk 2012年8月8日 14:44:35 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            if (e.CellValue == null) return;
            DataRowView drv = gridView1.GetRow(e.RowHandle) as DataRowView;
            //取得病人名字
            string patname = drv["patname"].ToString().Trim();

            if (e.Column == gridColumn1)
            {
                if (patname.Contains("婴儿"))
                {
                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                    e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red, e.Bounds, s);
                    e.Handled = true;
                }
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonNew_Click(object sender, EventArgs e)
        {
            NursingRecord nursingRecord = new NursingRecord();
            nursingRecord.ShowDialog();

            this.ucThreeMeasureTable1.LoadData();
            this.ucThreeMeasureTable1.Refresh();
        }
        /// <summary>
        /// Treelist焦点改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            TreeListNode node = e.Node;
            DataRow dr = (DataRow)node.Tag;
            if (null != dr && null != dr["PARENTID"] && dr["PARENTID"].ToString() != "")
            {
                InitAllListView(node);
            }

        }

        /// <summary>
        /// 初始化 护理文档
        /// </summary>
        private void InitAllListView(TreeListNode node)
        {
            DataRow dr = (DataRow)node.Tag;
            string modelID = null == dr ? "0" : (null == dr["ID"] ? "0" : dr["ID"].ToString());
            m_UCNusreRecordMain = new UCNusreRecordMain(modelID);
            ShutOtherTabPages(modelID);
            m_UCNusreRecordMain.Dock = DockStyle.Fill;
            OpenCurrentTabPages(modelID);

            //m_UCChildNurseRecord.RefreshData();
            if (modelID == "2")
            {//出入液量记录单
                xtraTabPageOutAndInRecPage.Controls.Add(m_UCNusreRecordMain);
                xtraTabControl1.SelectedTabPage = xtraTabPageOutAndInRecPage;
            }
            else if (modelID == "3")
            {//儿科护理记录单
                xtraTabPageChildNurseRecPage.Controls.Add(m_UCNusreRecordMain);
                xtraTabControl1.SelectedTabPage = xtraTabPageChildNurseRecPage;
            }
            else if (modelID == "4")
            {//非手术护理记录单
                xtraTabPageNotNusreOperRecPage.Controls.Add(m_UCNusreRecordMain);
                xtraTabControl1.SelectedTabPage = xtraTabPageNotNusreOperRecPage;
            }
            else if (modelID == "5")
            {//手术护理记录单
                xtraTabPageNusreOperRecPage.Controls.Add(m_UCNusreRecordMain);
                xtraTabControl1.SelectedTabPage = xtraTabPageNusreOperRecPage;
            }
            m_UCNusreRecordMain.SetOperNurseRecord();
        }

        public void RefreshSelectedTabPages()
        {
            if (null != m_UCNusreRecordMain)
            {
                m_UCNusreRecordMain.SetOperNurseRecord();
            }
        }

        /// <summary>
        /// 关闭当前tab页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            XtraTabPage selectedPage = xtraTabControl1.SelectedTabPage;
            selectedPage.PageEnabled = false;
            selectedPage.PageVisible = false;
            xtraTabControl1.Refresh();
        }

        /// <summary>
        /// 关闭其它tab页
        /// </summary>
        /// <param name="recordID"></param>
        private void ShutOtherTabPages(string recordID)
        {
            if (recordID == "2")
            {//出入液量记录单
                xtraTabPageChildNurseRecPage.PageEnabled = false;
                xtraTabPageChildNurseRecPage.PageVisible = false;
                xtraTabPageNusreOperRecPage.PageEnabled = false;
                xtraTabPageNusreOperRecPage.PageVisible = false;
                xtraTabPageNotNusreOperRecPage.PageEnabled = false;
                xtraTabPageNotNusreOperRecPage.PageVisible = false;
            }
            else if (recordID == "3")
            {//儿科护理记录单
                xtraTabPageOutAndInRecPage.PageEnabled = false;
                xtraTabPageOutAndInRecPage.PageVisible = false;
                xtraTabPageNusreOperRecPage.PageEnabled = false;
                xtraTabPageNusreOperRecPage.PageVisible = false;
                xtraTabPageNotNusreOperRecPage.PageEnabled = false;
                xtraTabPageNotNusreOperRecPage.PageVisible = false;
            }
            else if (recordID == "4")
            {//非手术护理记录单
                xtraTabPageOutAndInRecPage.PageEnabled = false;
                xtraTabPageOutAndInRecPage.PageVisible = false;
                xtraTabPageChildNurseRecPage.PageEnabled = false;
                xtraTabPageChildNurseRecPage.PageVisible = false;
                xtraTabPageNusreOperRecPage.PageEnabled = false;
                xtraTabPageNusreOperRecPage.PageVisible = false;
            }
            else if (recordID == "5")
            {//手术护理记录单
                xtraTabPageOutAndInRecPage.PageEnabled = false;
                xtraTabPageOutAndInRecPage.PageVisible = false;
                xtraTabPageChildNurseRecPage.PageEnabled = false;
                xtraTabPageChildNurseRecPage.PageVisible = false;
                xtraTabPageNotNusreOperRecPage.PageEnabled = false;
                xtraTabPageNotNusreOperRecPage.PageVisible = false;
            }
        }

        /// <summary>
        /// 打开当前tab页
        /// </summary>
        /// <param name="recordID"></param>
        private void OpenCurrentTabPages(string recordID)
        {
            if (recordID == "2")
            {//出入液量记录单
                xtraTabPageOutAndInRecPage.PageEnabled = true;
                xtraTabPageOutAndInRecPage.PageVisible = true;
            }
            else if (recordID == "3")
            {//儿科护理记录单
                xtraTabPageChildNurseRecPage.PageEnabled = true;
                xtraTabPageChildNurseRecPage.PageVisible = true;
            }
            else if (recordID == "4")
            {//非手术护理记录单
                xtraTabPageNotNusreOperRecPage.PageEnabled = true;
                xtraTabPageNotNusreOperRecPage.PageVisible = true;
            }
            else if (recordID == "5")
            {//手术护理记录单
                xtraTabPageNusreOperRecPage.PageEnabled = true;
                xtraTabPageNusreOperRecPage.PageVisible = true;
            }
        }

        private void treeList1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 批量打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonPrintBat_Click(object sender, EventArgs e)
        {
            ucThreeMeasureTable1.PrintAllDocument();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonRefresh_Click(object sender, EventArgs e)
        {
            this.ucThreeMeasureTable1.LoadData();
            ucThreeMeasureTable1.Refresh();
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonPrint_Click(object sender, EventArgs e)
        {
            ucThreeMeasureTable1.PrintDocument();
        }

        /// <summary>
        /// 第一周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonFirstWeek_Click(object sender, EventArgs e)
        {
            this.ucThreeMeasureTable1.SetAllocateDateTime(m_app.SqlHelper);//入院的那一周，即第一周
            this.simpleButtonLastWeek.Enabled = false;//选择第一周的时候“上一周”的按钮不可用

            this.ucThreeMeasureTable1.LoadData();
            ucThreeMeasureTable1.Refresh();
        }
        /// <summary>
        /// 下周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonNextWeek_Click(object sender, EventArgs e)
        {
            this.ucThreeMeasureTable1.SetAllocateDateTime(7, m_app.SqlHelper);
            this.simpleButtonLastWeek.Enabled = true;

            this.ucThreeMeasureTable1.LoadData();
            ucThreeMeasureTable1.Refresh();
        }
        /// <summary>
        /// 本周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonThisWeek_Click(object sender, EventArgs e)
        {
            this.ucThreeMeasureTable1.SetAllocateDateTime(DateTime.Now, m_app.SqlHelper);
            SetButtonEnable();

            ucThreeMeasureTable1.LoadData();
            ucThreeMeasureTable1.Refresh();
        }
        /// <summary>
        /// 上周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonLastWeek_Click(object sender, EventArgs e)
        {
            this.ucThreeMeasureTable1.SetAllocateDateTime(-7, m_app.SqlHelper);
            SetButtonEnable();

            this.ucThreeMeasureTable1.LoadData();
            ucThreeMeasureTable1.Refresh();
        }

        /// <summary>
        /// 隐藏提示框 
        /// add by ywk  2012年8月7日 14:02:17
        /// </summary>
        private void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }

        /// <summary>
        /// 提示框
        /// add by ywk 2012年8月7日 14:02:23 
        /// </summary>
        /// <param name="caption"></param>
        private void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog == null)
                m_WaitDialog = new WaitDialogForm("正在加载数据......", "请您稍后！");
            if (!m_WaitDialog.Visible)
                m_WaitDialog.Visible = true;
            m_WaitDialog.Caption = caption;
        }

        /// <summary>
        /// 窗体加载事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCMainNurseForm_Load_1(object sender, EventArgs e)
        { //进来时加载上级页面带来的病人信息，此页面中，根据列表中选择的病人，上方工具栏显示相应的病人信息 
            //InitMeasureTableData();
            BindPatInfos();
        }
        /// <summary>
        /// 搜索框，输入内容改变下方列表控制
        /// add by ywk 2012年8月8日 14:57:35 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_EditValueChanged(object sender, EventArgs e)
        {
            refreshGridView();
        }

        #endregion

        #region IEMREditor 成员

        public Control DesignUI
        {
            get { return this; }
        }

        public new void Load(IEmrHost app)
        {
            m_app = app;
            PublicSet.MethodSet.App = app;
            CurrentPat = m_app.CurrentPatientInfo;
            InitForm();
        }
        public void Save()
        {

        }

        public string Title
        {
            get { return "三测单曲线"; }
        }

        public void Print()
        {
            //ucThreeMeasureTable.PrintDocument();
        }

        public string CurrentNoofinpat
        {
            get;
            set;
        }
        private bool m_ReadOnlyControl = false;
        public bool ReadOnlyControl
        {
            get { return m_ReadOnlyControl; }
            set { m_ReadOnlyControl = value; }
        }
        #endregion

        #region 左侧护理文档树
        ///修改时间：2012-08-08
        ///修改人：cyq

        /// <summary>
        /// 初始化左侧的护理文档树
        /// </summary>
        private void InitNursDocuTree()
        {
            treeList1.ClearNodes();

            DataTable dts = GetNursDocMenusByParentID(null);
            foreach (DataRow row in dts.Rows)
            {
                InitTree(row, null);
            }
        }

        private void InitTree(DataRow row, TreeListNode node)
        {
            TreeListNode nursNode = treeList1.AppendNode(new object[] { row["CNAME"] }, node);
            nursNode.Tag = row;

            //寻找子节点
            DataTable datatable = GetNursDocMenusByParentID(row["ID"].ToString());
            if (datatable.Rows.Count > 0)
            {
                foreach (DataRow nextRow in datatable.Rows)
                {
                    InitTree(nextRow, nursNode);
                }
            }
        }

        /// <summary>
        /// 默认展开
        /// </summary>
        /// <param name="nodes"></param>
        private void InitTreeExpand(TreeListNodes nodes)
        {
            foreach (TreeListNode node in nodes)
            {
                if (node.Nodes.Count > 0)
                {
                    node.Expanded = true;
                }
            }
        }

        /// <summary>
        /// 获取护理文档菜单数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetNursDocMenusByParentID(string parentID)
        {
            string sqlStr = " select ID,CNAME,MNAME,PARENTID,SORTINDEX from dict_nursingdocuments_catalog where 1=1 ";
            if (!string.IsNullOrEmpty(parentID))
            {
                sqlStr += String.Format(" and PARENTID = {0} ", parentID);
            }
            else
            {
                sqlStr += " and PARENTID is null ";
            }
            sqlStr += " order by SORTINDEX ";
            return m_app.SqlHelper.ExecuteDataTable(sqlStr);
        }
        /// <summary>
        /// 设置Treelist的图片部分
        /// edit by ywk  2012年8月8日 14:15:12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList1_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            TreeListNode node = e.Node;
            DataRow dr = (DataRow)node.Tag;

            if (string.IsNullOrEmpty(dr["PARENTID"].ToString()))
            {
                if (e.Node.Expanded)//文件夹打开
                {
                    //e.NodeImageIndex = 1; 
                    e.NodeImageIndex = 3;
                }
                else//文件夹未打开
                {
                    //e.NodeImageIndex = 0; 
                    e.NodeImageIndex = 2;
                }
            }
            else
            {
                e.NodeImageIndex = 0;
            }
        }


        #endregion

        private void simpleButtonNew_Click_1(object sender, EventArgs e)
        {

        }

        #region TreeList 提示
        //private void InitToolTip(string displayName)
        //{
        //    if (displayName.Trim() != "")
        //    {
        //        toolTipControllerTreeList.SetToolTip(treeList1, displayName);
        //    }
        //}

        //private void treeList1_MouseMove(object sender, MouseEventArgs e)
        //{
        //    TreeListHitInfo hitInfo = treeList1.CalcHitInfo(treeList1.PointToScreen(e.Location));
        //    if (hitInfo.Node == null) return;

        //    InitToolTip(hitInfo.Node.GetValue("colName").ToString());
        //}
        #endregion

    }
}
