using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Common.Library;
using DrectSoft.DrawDriver;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DrectSoft.Core.DoctorTasks
{
    /// <summary>
    /// 医嘱浏览界面
    /// Add by xlb 2013-04-08
    /// </summary>
    public partial class FormOrderForInpat : UserControl, IEMREditor
    {
        private DataTable dtOrder = null;
        private InpatientSim inpatient;//病人对象
        private int rowsIndex = 0;//当前行数
        private int pageCount = 20;//每页显示行数
        private int circulateCount = 1;//总页数
        private string dept = null;//当前部门
        private int pageIndex = 0;//当前页数
        private string IsSortByDepartment = "0";//是否按科室分类打印,0为不安科室分类，1按科室分类
        private Inpatient CurrentInpatient;
        private bool IsLongOrder = true;  //当前查询医嘱是否是长期医嘱
        #region IEMREditor Members

        private bool m_ReadOnlyControl = false;

        public bool ReadOnlyControl
        {
            get { return m_ReadOnlyControl; }
            set { m_ReadOnlyControl = value; }
        }

        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        private IEmrHost _app;
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        public IEmrHost App
        {
            get { return _app; }
            set { _app = value; }
        }

        public string CurrentNoofinpat
        {
            get;
            set;
        }

        public void Load(IEmrHost m_app)
        {
            try
            {
                _app = m_app;
                if (!string.IsNullOrEmpty(CurrentNoofinpat))
                {
                    CurrentInpatient = new Inpatient(Convert.ToDecimal(CurrentNoofinpat));
                }
                else if (_app.CurrentPatientInfo != null)
                {
                    CurrentInpatient = _app.CurrentPatientInfo;
                }
                else
                {
                    return;
                }
                CurrentInpatient.ReInitializeAllProperties();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Title
        {
            get { return "病人医嘱浏览"; }
        }

        string[] config;

        public FormOrderForInpat()
        {
            try
            {
                InitializeComponent();
                IsSortByDepartment = DS_SqlService.GetConfigValueByKey("IsSortByDepartment");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Control DesignUI
        {
            get { return this; }
        }

        public void Print()
        {

        }

        public void Save()
        {

        }

        #endregion

        #region Methods

        public FormOrderForInpat(string nOofinpat)
            : this()
        {
            try
            {
                CurrentNoofinpat = nOofinpat;
                DS_SqlHelper.CreateSqlHelper();
                GetConfig();
                InitDataDept(lookUpEditorDept);
                InitDefault();
                RegisterEvent();
                GeInpat();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化
        /// Add by xlb 2013-04-08
        /// </summary>
        private void InitDefault()
        {
            try
            {
                rdgOrderStyle.SelectedIndex = Int32.Parse(config[0]);
                rdgDate.SelectedIndex = Int32.Parse(config[1]);
                InitDate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化时间
        /// 通过配置项来控制时间差
        /// Add by xlb 2013-04-08
        /// </summary>
        private void InitDate()
        {
            try
            {
                if (config[1].Trim() == "0")
                {
                    dateEditBegin.Properties.ReadOnly = true;
                    dateEditEnd.Properties.ReadOnly = true;
                }
                else
                {
                    dateEditEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    dateEditBegin.Text = DateTime.Now.AddDays(double.Parse(config[2])).ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过配置项来设置默认值
        /// Add by xlb 2013-04-08
        /// </summary>
        private void GetConfig()
        {
            try
            {
                string defaultValue = DS_SqlService.GetConfigValueByKey("IemrOrderDefault");
                if (string.IsNullOrEmpty(defaultValue))
                {
                    defaultValue = "0,1,-10";
                }
                config = defaultValue.Split(',');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 注册事件方法
        /// Add by xlb 2013-04-08
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                rdgDate.SelectedIndexChanged += new EventHandler(rdgDate_SelectedIndexChanged);
                btnQuery.Click += new EventHandler(btnQuery_Click);
                this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(gridView1_CustomDrawRowIndicator);
                btnReset.Click += new EventHandler(btnReset_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化科室下拉框
        /// Add by xlb 2013-04-08
        /// </summary>
        /// <param name="lookUpEditor"></param>
        private void InitDataDept(LookUpEditor lookUpEditor)
        {
            try
            {
                LookUpWindow lookUpWindow = new LookUpWindow();
                lookUpEditor.Kind = WordbookKind.Sql;
                lookUpEditor.ListWindow = lookUpWindow;
                DataTable dtDept = DS_SqlHelper.ExecuteDataTable(@"select ID,NAME,py,wb from department  
                where sort in('101','102') and valid='1' ", CommandType.Text);
                if (dtDept == null || dtDept.Columns.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i > dtDept.Columns.Count; i++)
                {
                    if (dtDept.Columns[i].ColumnName.ToUpper().Trim() == "ID")
                    {
                        dtDept.Columns[i].Caption = "编号";
                    }
                    else if (dtDept.Columns[i].ColumnName.ToUpper().Trim() == "NAME")
                    {
                        dtDept.Columns[i].Caption = "科室";
                    }
                }
                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                dictionary.Add("ID", 30);
                dictionary.Add("NAME", 76);
                SqlWordbook sqlWordBook = new SqlWordbook("deptMent", dtDept, "ID", "NAME", dictionary, "ID//NAME//PY//WB");
                lookUpEditor.SqlWordbook = sqlWordBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询方法
        /// Add by xlb 2013-04-08
        /// </summary>
        private void Search()
        {
            try
            {
                //存储过程名称
                string procedureName = DS_SqlService.GetConfigValueByKey("MadeOrderProcedure");
                string IsView = DS_SqlService.GetConfigValueByKey("MadeOrderIsView"); ;
                if (string.IsNullOrEmpty(procedureName))
                {
                    procedureName = "usp_GetDoctorAdvice";
                }

                DS_SqlHelper.CreateSqlHelperByDBName("HISDB");
                CheckConnectHIS();
                if (IsView == "1")
                {
                    GetDateTable();
                }
                else
                {
                    GetDateTableByPrc(procedureName);
                }

                gridView1.Columns.Clear();
                gridControlOrder.DataSource = dtOrder;

                for (int i = 0; i < gridView1.Columns.Count; i++)
                {
                    //列头居中
                    gridView1.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    if (gridView1.Columns[i].FieldName.Trim() == "医嘱内容")
                    {
                        gridView1.Columns[i].Width = 200;
                    }
                    else
                    {
                        gridView1.Columns[i].Width = (gridControlOrder.Width - 200) / gridView1.Columns.Count - 1;
                    }
                    //RepositoryItem repository = new RepositoryItemMemoEdit();
                    //gridView1.Columns[i].ColumnEdit = repository;
                }

                gridView1.OptionsCustomization.AllowColumnMoving = false;
                gridView1.OptionsCustomization.AllowColumnResizing = true;
                gridView1.OptionsCustomization.AllowRowSizing = false;
                gridView1.OptionsCustomization.AllowQuickHideColumns = false;
                gridView1.OptionsCustomization.AllowFilter = false;
                gridView1.OptionsCustomization.AllowSort = false;//禁掉自带排序
                gridView1.OptionsBehavior.Editable = false;//不可编辑
                DS_SqlHelper.CreateSqlHelper();
            }
            catch (Exception ex)
            {
                DS_SqlHelper.CreateSqlHelper();
                throw ex;
            }
        }

        /// <summary>
        /// 通过存储过程来查找医嘱信息
        /// </summary>
        /// <param name="procedureName"></param>
        private void GetDateTableByPrc(string procedureName)
        {
            try
            {
                SqlParameter p_result = new SqlParameter("@result", SqlDbType.Structured);
                p_result.Direction = ParameterDirection.Output;
                SqlParameter[] sps =
             {
              new SqlParameter("@InpatId",SqlDbType.VarChar),
              new SqlParameter("@DeptId",SqlDbType.VarChar),
              new SqlParameter("@Type",rdgOrderStyle.SelectedIndex),
              new SqlParameter("@BeginDate",SqlDbType.VarChar),
              new SqlParameter("@EndDate",SqlDbType.VarChar),
             p_result
             };
                if (CurrentInpatient != null)
                {
                    sps[0].Value = CurrentInpatient.NoOfHisFirstPage;
                }
                else
                {
                    sps[0].Value = CurrentNoofinpat;
                }
                sps[1].Value = lookUpEditorDept.CodeValue;
                sps[3].Value = dateEditBegin.Text;
                sps[4].Value = dateEditEnd.Text;
                dtOrder = DS_SqlHelper.ExecuteDataTable(procedureName, sps, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过视图来查找数据
        /// xll 添加视图查询方法
        /// </summary>
        /// <returns></returns>
        private void GetDateTable()
        {
            try
            {
                string sqlCheck = "";
                string startTime = "1000-01-01 00:00:00";
                string endTime = "9000-01-01 23:59:59";
                string deptName = "";
                string noofHis = "";
                if (CurrentInpatient != null)
                {
                    noofHis = CurrentInpatient.NoOfHisFirstPage;
                }
                else
                {
                    noofHis = CurrentNoofinpat;
                }

                if (!string.IsNullOrEmpty(dateEditBegin.Text))
                {
                    startTime = dateEditBegin.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                }
                if (!string.IsNullOrEmpty(dateEditEnd.Text))
                {
                    endTime = dateEditEnd.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                }
                deptName = lookUpEditorDept.Text;

                if (rdgOrderStyle.SelectedIndex == 0)
                {

                    sqlCheck = string.Format(@"select * from yd_shortdocadvice y where y.医嘱时间>='{0}' and y.医嘱时间<='{1}' and y.开立科室 like '%{2}%' and 病人编号='{3}'", startTime, endTime, deptName, noofHis);
                }
                else
                {
                    sqlCheck = string.Format(@"select * from yd_longdocadvice y where y.开始时间>='{0}' and y.开始时间<='{1}' and y.开立科室 like '%{2}%' and 病人编号='{3}'", startTime, endTime, deptName, noofHis);
                }
                dtOrder = DS_SqlHelper.ExecuteDataTable(sqlCheck, CommandType.Text);
                dtOrder.Columns.Remove("病人编号");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取病人对象
        /// </summary>
        private void GeInpat()
        {
            try
            {
                DataTable dtInpat = DS_SqlHelper.ExecuteDataTable(@"select noofinpat,patid,outhosdept,
                outhosward,outbed,name from inpatient where noofinpat=@noofinpat",
                new SqlParameter[] { new SqlParameter("@noofinpat", CurrentNoofinpat) },
                CommandType.Text);
                //datatable->inpatientsim
                inpatient = DataTableToList<InpatientSim>.ConvertToModelOne(dtInpat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证是否可以连接到HIS
        /// Add by xlb 2013-04-08
        /// </summary>
        private void CheckConnectHIS()
        {
            try
            {
                //sql无法使用一下语句 xll 2013-06-17
                //DS_SqlHelper.ExecuteDataTable("select 1 from dual", CommandType.Text);
            }
            catch (Exception e)
            {
                throw new Exception("无法连接到HIS数据库");
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// 重置事件
        /// Add by xlb 2013-04-10
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                InitDefault();
                lookUpEditorDept.CodeValue = string.Empty;
                dtOrder = null;
                gridControlOrder.DataSource = null;
                gridView1.Columns.Clear();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 时间切换事件
        /// Add by xlb 2013-04-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdgDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdgDate.SelectedIndex == 0)
                {
                    dateEditBegin.Properties.ReadOnly = true;
                    dateEditEnd.Properties.ReadOnly = true;
                    dateEditBegin.Text = string.Empty;
                    dateEditEnd.Text = string.Empty;
                }
                else
                {
                    dateEditBegin.Properties.ReadOnly = false;
                    dateEditEnd.Properties.ReadOnly = false;
                    dateEditBegin.Text = DateTime.Now.AddDays(double.Parse(config[2])).ToString("yyyy-MM-dd");
                    dateEditEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 查询事件
        /// Add by xlb 2013-04-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdgOrderStyle.SelectedIndex == 1)
                {
                    IsLongOrder = true;
                }
                else
                {
                    IsLongOrder = false;
                }
                Search();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(2, ex);
            }
        }

        /// <summary>
        /// 加序号列
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
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 打印指定页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtOrder == null || dtOrder.Rows.Count <= 0)
                {
                    MessageBox.Show("医嘱数据为空");
                    return;
                }
                if (IsLongOrder)
                {
                    List<Metafile> metaList = new List<Metafile>();
                    dept = dtOrder.Rows[0]["开立科室"].ToString();
                    for (int i = 0; i < circulateCount; i++)
                    {
                        pageIndex = i + 1;
                        XmlCommomOp.Doc = null;
                        XmlCommomOp.CopyTempalteXmlFile(AppDomain.CurrentDomain.BaseDirectory + "LongtermTaskTemp.xml", AppDomain.CurrentDomain.BaseDirectory + "LongtermTask.xml");
                        XmlCommomOp.xmlPath = AppDomain.CurrentDomain.BaseDirectory + "LongtermTask.xml";
                        XmlCommomOp.CreaetDocument();
                        XmlCommomOp.BindingDate(CreateDataSet(), CreateDate());
                        metaList.Add(DrawOp.MakeImagesByXmlDocument(XmlCommomOp.Doc)[0]);
                    }
                    rowsIndex = 0;
                    circulateCount = 1;
                    DrawOp.PrintView(metaList);
                    if (File.Exists(XmlCommomOp.xmlPath))
                    {
                        File.Delete(XmlCommomOp.xmlPath);
                    }
                }
                else
                {
                    List<Metafile> metaList = new List<Metafile>();
                    dept = dtOrder.Rows[0]["开立科室"].ToString();
                    for (int i = 0; i < circulateCount; i++)
                    {
                        XmlCommomOp.Doc = null;
                        pageIndex = i + 1;
                        XmlCommomOp.CopyTempalteXmlFile(AppDomain.CurrentDomain.BaseDirectory + "ShorttermTaskTemp.xml", AppDomain.CurrentDomain.BaseDirectory + "ShorttermTask.xml");
                        XmlCommomOp.xmlPath = AppDomain.CurrentDomain.BaseDirectory + "ShorttermTask.xml";
                        XmlCommomOp.CreaetDocument();
                        XmlCommomOp.BindingDate(CreateDataSet(), CreateDate());
                        metaList.Add(DrawOp.MakeImagesByXmlDocument(XmlCommomOp.Doc)[0]);
                    }
                    rowsIndex = 0;
                    circulateCount = 1;
                    DrawOp.PrintView(metaList);
                    if (File.Exists(XmlCommomOp.xmlPath))
                    {
                        File.Delete(XmlCommomOp.xmlPath);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 构造参数集合
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, ParamObject> CreateDate()
        {
            try
            {
                Dictionary<string, ParamObject> dicParamsList = new Dictionary<string, ParamObject>();
                PropertyInfo[] propertys = inpatient.GetType().GetProperties();
                foreach (PropertyInfo item in propertys)
                {
                    ParamObject param = new ParamObject(item.Name, "", item.GetValue(inpatient, null) == null ? "" : item.GetValue(inpatient, null).ToString());
                    if (!dicParamsList.ContainsKey(item.Name))
                    {

                        dicParamsList.Add(item.Name, param);
                    }

                }
                dicParamsList.Add("dept", new ParamObject("dept", "", dept));
                dicParamsList.Add("pageIndex", new ParamObject("pageIndex", "", pageIndex.ToString()));
                return dicParamsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 构造DataSet
        /// </summary>
        /// <returns></returns>
        private DataSet CreateDataSet()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = dtOrder.Clone();
                if (!IsLongOrder)
                {
                    dtOrder.DefaultView.Sort = "医嘱时间 DESC";
                }
                else
                {
                    dtOrder.DefaultView.Sort = "开始时间 DESC";
                }
                int index = 1;
                for (int i = rowsIndex; i < dtOrder.Rows.Count; i++)
                {
                    if (!dept.Equals(dtOrder.Rows[i]["开立科室"].ToString().Trim()) && IsSortByDepartment.Trim().ToString().Equals("1"))
                    {
                        circulateCount++;
                        rowsIndex = i;
                        for (int beginIndex = index; beginIndex <= pageCount; beginIndex++)
                        {
                            DataRow row = dt.NewRow();
                            dt.Rows.Add(row);
                        }
                        dept = dtOrder.Rows[i]["开立科室"].ToString().Trim();
                        break;
                    }
                    dt.Rows.Add(dtOrder.Rows[i].ItemArray);

                    if (index == int.Parse(pageCount.ToString()))
                    {
                        rowsIndex = i + 1;
                        circulateCount++;
                        break;
                    }
                    if (i + 1 == dtOrder.Rows.Count && index < pageCount)
                    {
                        for (int beginIndex = index; beginIndex < pageCount; beginIndex++)
                        {
                            DataRow row = dt.NewRow();
                            dt.Rows.Add(row);
                        }
                    }
                    index++;
                }
                dt.TableName = "DoctorPrint";
                ds.Tables.Add(dt);
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

    }
}
