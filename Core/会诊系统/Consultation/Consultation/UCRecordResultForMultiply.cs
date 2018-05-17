using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Wordbook;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;

namespace DrectSoft.Core.Consultation
{
    public partial class UCRecordResultForMultiply : DevExpress.XtraEditors.XtraUserControl
    {
        private IEmrHost m_App;
        private string m_NoOfFirstPage;
        private string m_ConsultApplySN = string.Empty;
        private bool m_ReadOnly = false;
        private int m_RowIndexLookUpEditor = -1;

        /// <summary>
        /// 会诊信息选中行
        /// </summary>
        private DataRow m_SelectRow;
        /// <summary>
        /// 保存User表中所有的数据
        /// </summary>
        private DataTable m_DataTableAllEmployee;

        public UCRecordResultForMultiply()
        {
            InitializeComponent();
        }

        #region LookUpEditor数据绑定

        private void BindData()
        {
            BindHospitalData();
            BindDepartment();
            BindDoctorLevel();
            BindEmployee();
            BindEmployeeInGrid();
            InitGridControlDataSource();
        }

        #region 绑定医院
        /// <summary>
        /// 绑定医院
        /// </summary>
        private void BindHospitalData()
        {
            lookUpEditorHospital.Kind = WordbookKind.Sql;
            lookUpEditorHospital.ListWindow = lookUpWindowHospital;
            BindHospitalWordBook(GetConsultationData("1"));
        }

        /// <summary>
        /// 绑定医院的数据源
        /// </summary>
        /// <param name="dataTableData"></param>
        private void BindHospitalWordBook(DataTable dataTableData)
        {
            Dictionary<string, int> colWidths = new Dictionary<string, int>();
            colWidths.Add("ID", 60);
            colWidths.Add("NAME", 120);

            for (int i = 0; i < dataTableData.Columns.Count; i++)
            {
                if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "id")
                {
                    dataTableData.Columns[i].Caption = "医院编码";
                }
                else if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "name")
                {
                    dataTableData.Columns[i].Caption = "医院名称";
                }
            }

            //得到数据字典，为LookUpWindow中的数据源
            SqlWordbook wordBook = new SqlWordbook("Hospital"/*字典的唯一标示*/, dataTableData/*数据集*/,
                "ID"/*每行记录的唯一标示*/, "NAME"/*显示在LookUpEditer中的值*/
               , colWidths/*设置每列的宽度*/, "ID//Name"/*根据医院编码，医院名称进行查询*/);

            lookUpEditorHospital.SqlWordbook = wordBook;
        }

        #endregion

        #region 绑定受邀科室
        /// <summary>
        /// 绑定受邀科室
        /// </summary>
        private void BindDepartment()
        {
            lookUpEditorDepartment.Kind = WordbookKind.Sql;
            lookUpEditorDepartment.ListWindow = lookUpWindowDepartment;
            BindDepartmentWordBook(GetConsultationData("2"));
        }

        private void BindDepartmentWordBook(DataTable dataTableData)
        {
            for (int i = 0; i < dataTableData.Columns.Count; i++)
            {
                if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "id")
                {
                    dataTableData.Columns[i].Caption = "科室编码";
                }
                else if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "name")
                {
                    dataTableData.Columns[i].Caption = "科室名称";
                }
            }

            Dictionary<string, int> colWidths = new Dictionary<string, int>();
            colWidths.Add("ID", 60);
            colWidths.Add("NAME", 120);
            SqlWordbook wordBook = new SqlWordbook("Department", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorDepartment.SqlWordbook = wordBook;
        }

        #endregion

        #region 绑定医师资质
        /// <summary>
        /// 绑定受邀科室
        /// </summary>
        private void BindDoctorLevel()
        {
            lookUpEditorLevel.Kind = WordbookKind.Sql;
            lookUpEditorLevel.ListWindow = lookUpWindowLevel;
            BindDoctorLevelWordBook(GetConsultationData("9"));
        }

        private void BindDoctorLevelWordBook(DataTable dataTableData)
        {
            for (int i = 0; i < dataTableData.Columns.Count; i++)
            {
                if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "name")
                {
                    dataTableData.Columns[i].Caption = "医师级别";
                }
            }

            Dictionary<string, int> colWidths = new Dictionary<string, int>();
            //colWidths.Add("ID", 10);
            colWidths.Add("NAME", 120);
            SqlWordbook wordBook = new SqlWordbook("Director", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorLevel.SqlWordbook = wordBook;
        }

        #endregion

        #region 绑定会诊医生
        /// <summary>
        /// 绑定受邀科室
        /// </summary>
        private void BindEmployee()
        {
            lookUpEditorEmployee.Kind = WordbookKind.Sql;
            lookUpEditorEmployee.ListWindow = lookUpWindowEmployee;
            BindEmployeeWordBook(GetConsultationData("3"));
        }

        private void BindEmployeeWordBook(DataTable dataTableData)
        {
            for (int i = 0; i < dataTableData.Columns.Count; i++)
            {
                if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "id")
                {
                    dataTableData.Columns[i].Caption = "医师代码";
                }
                else if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "name")
                {
                    dataTableData.Columns[i].Caption = "医师名称";
                }
            }

            Dictionary<string, int> colWidths = new Dictionary<string, int>();
            colWidths.Add("ID", 60);
            colWidths.Add("NAME", 120);
            SqlWordbook wordBook = new SqlWordbook("Employee", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorEmployee.SqlWordbook = wordBook;
        }

        #endregion

        #region 绑定会诊医生
        /// <summary>
        /// 绑定受邀科室
        /// </summary>
        private void BindEmployeeInGrid()
        {
            lookUpEditorEmployeeInGrid.Kind = WordbookKind.Sql;
            lookUpEditorEmployeeInGrid.ListWindow = lookUpWindowEmployeeInGrid;
            BindEmployeeInGridWordBook(GetConsultationData("8"));
        }

        private void BindEmployeeInGridWordBook(DataTable dataTableData)
        {
            for (int i = 0; i < dataTableData.Columns.Count; i++)
            {
                if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "employeecode")
                {
                    dataTableData.Columns[i].Caption = "医师代码";
                }
                else if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "employeename")
                {
                    dataTableData.Columns[i].Caption = "医师名称";
                }
            }

            Dictionary<string, int> colWidths = new Dictionary<string, int>();
            colWidths.Add("EMPLOYEECODE", 60);
            colWidths.Add("EMPLOYEENAME", 120);
            SqlWordbook wordBook = new SqlWordbook("Employee", dataTableData, "EMPLOYEECODE", "EMPLOYEENAME", colWidths, "EMPLOYEECODE//EMPLOYEENAME//PY//WB");
            lookUpEditorEmployeeInGrid.SqlWordbook = wordBook;
        }

        #endregion

        /// <summary>
        /// 得到绑定需要的数据
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        private DataTable GetConsultationData(string typeID)
        {
            if (Dal.DataAccess.App == null)
            {
                Dal.DataAccess.App = m_App;
            }
            DataTable dataTableConsultationData = new DataTable();

            if (typeID == "1")//医院
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, "");
            }
            else if (typeID == "2")//受邀科室
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, lookUpEditorHospital.CodeValue.Trim());
            }
            else if (typeID == "3")//受邀医生
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, lookUpEditorDepartment.CodeValue.Trim());
            }
            else if (typeID == "4")//申请医师
            {
                string wardID = m_App.User.CurrentWardId.ToString();
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, wardID);
            }
            else if (typeID == "5")//申请医师科主任
            {
                string wardID = m_App.User.CurrentWardId.ToString();
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, wardID);
            }
            else if (typeID == "6")//CategoryDetail
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, "20");
            }
            else if (typeID == "8")//得到所有临床医师
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, "");
            }
            else if (typeID == "9")//医师级别
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, "20");
            }

            return dataTableConsultationData;
        }

        /// <summary>
        /// 初始化GridControl数据源
        /// </summary>
        private void InitGridControlDataSource()
        {
            DataTable dt = new DataTable("Department");
            dt.Columns.Add("HospitalName");
            dt.Columns.Add("HospitalCode");
            dt.Columns.Add("DepartmentName");
            dt.Columns.Add("DepartmentCode");
            dt.Columns.Add("EmployeeLevelName");
            dt.Columns.Add("EmployeeLevelID");
            dt.Columns.Add("EmployeeCode");
            dt.Columns.Add("EmployeeName");
            dt.Columns.Add("DeleteButton");
            dt.Columns.Add("SignIn");
            gridControlDepartment.DataSource = dt;

            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
        }

        #endregion

        private void RegisterEvent()
        {
            lookUpEditorHospital.CodeValueChanged += new EventHandler(lookUpEditorHospital_CodeValueChanged);
            lookUpEditorDepartment.CodeValueChanged += new EventHandler(lookUpEditorDepartment_CodeValueChanged);
            gridViewDept.MouseDown += new MouseEventHandler(gridViewDept_MouseDown);
            simpleButtonNew.Click += new EventHandler(simpleButtonNew_Click);
            repositoryItemLookUpEditEmployee.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(repositoryItemLookUpEditEmployee_EditValueChanging);
        }

        /// <summary>
        /// 根据gridview中选择人员的情况动态更改科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void repositoryItemLookUpEditEmployee_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            DataRow dr = gridViewDept.GetDataRow(m_RowIndexLookUpEditor);

            DataTable dt = repositoryItemLookUpEditEmployee.DataSource as DataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string empCode = dt.Rows[i]["EmployeeCode"].ToString();

                if (empCode == e.NewValue.ToString())
                {
                    string deptCode = dt.Rows[i]["DeptCode"].ToString();
                    string deptName = dt.Rows[i]["DeptName"].ToString();
                    dr["DepartmentCode"] = deptCode;
                    dr["DepartmentName"] = deptName;
                }
            }
        }

        #region Init

        /// <summary>
        /// 初始化界面，供外部调用
        /// </summary>
        /// <param name="noOfFirstPage"></param>
        /// <param name="app"></param>
        /// <param name="isNew"></param>
        /// <param name="readOnly"></param>
        /// <param name="consultApplySN"></param>
        public void Init(string noOfFirstPage, IEmrHost app, bool isNew/*是否是新增申请*/, bool readOnly/*是否只读*/, string consultApplySN)
        {
            m_App = app;
            m_NoOfFirstPage = noOfFirstPage;
            m_ReadOnly = readOnly;
            m_ConsultApplySN = consultApplySN;
            m_DataTableAllEmployee = GetConsultationData("8");
            gridControlDepartment.ShowOnlyPredefinedDetails = true;

            gridViewDept.OptionsSelection.EnableAppearanceFocusedRow = true;
            gridViewDept.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridViewDept.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;

            RegisterEvent();
            InitInner(isNew);
            SetRepositoryItemLookUpEditDataSource();
        }

        private void InitInner(bool isNew)
        {
            BindGridColumnLookUpEditorData();
            BindData();
            SetDefaultValue();

            if (!isNew)
            {
                SetData();
                ControlVisible();
            }
        }

        /// <summary>
        /// 读取原先填写的数据，并为控件赋值
        /// </summary>
        private void SetData()
        {
            DataSet ds = Dal.DataAccess.GetConsultationDataSet(m_ConsultApplySN, "20");//, Convert.ToString((int)ConsultType.More));
            DataTable dtConsultApply = ds.Tables[0];
            DataTable dtConsultApplyDepartment = ds.Tables[1];
            DataTable dtConsultRecordDepartment = ds.Tables[2];

            if (dtConsultApply.Rows.Count > 0)
            {
                memoEditSuggestion.Text = dtConsultApply.Rows[0]["ConsultSuggestion"].ToString();
                if (dtConsultApply.Rows[0]["FinishTime"].ToString().Trim().Split(' ').Length == 2)
                {
                    dateEditConsultationDate.EditValue = dtConsultApply.Rows[0]["FinishTime"].ToString().Split(' ')[0];
                    timeEditConsultationTime.EditValue = dtConsultApply.Rows[0]["FinishTime"].ToString().Split(' ')[1];
                }
                else
                {
                    dateEditConsultationDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                    timeEditConsultationTime.EditValue = System.DateTime.Now.ToString().Split(' ')[1];
                }
            }

            //if (dtConsultApplyDepartment.Rows.Count > 0)
            //{
            //    gridControlDepartment.DataSource = dtConsultApplyDepartment;
            //    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
            //}
            if (dtConsultRecordDepartment.Rows.Count > 0)
            {
                gridControlDepartment.DataSource = dtConsultRecordDepartment;
                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
            }



            //if (dtConsultApplyDepartment.Rows.Count > 0 && dtConsultRecordDepartment.Rows.Count > 0)
            //{
            //    //dtConsultApplyDepartment.Rows.Clear();
            //    for (int d = 0; d < dtConsultRecordDepartment.Rows.Count; d++)
            //    {
            //        for (int i = dtConsultApplyDepartment.Rows.Count - 1; i >= 0; i--)
            //        {
            //            if (dtConsultApplyDepartment.Rows[i]["ordervalue"].ToString() == dtConsultRecordDepartment.Rows[d]["ordervalue"].ToString() && dtConsultApplyDepartment.Rows[i]["DepartmentCode"].ToString() == dtConsultRecordDepartment.Rows[d]["DepartmentCode"].ToString() && dtConsultApplyDepartment.Rows[i]["employeelevelid"].ToString() == dtConsultRecordDepartment.Rows[d]["employeelevelid"].ToString())
            //            {
            //                dtConsultApplyDepartment.Rows.Remove(dtConsultApplyDepartment.Rows[i]);
            //            }
            //            else
            //            {
            //                //dtConsultApplyDepartment.ImportRow(dtConsultRecordDepartment.Rows[d]);
            //                //continue;
            //            }
            //        }
            //    }


            //    dtConsultApplyDepartment.Merge(dtConsultRecordDepartment);
            //    gridControlDepartment.DataSource = dtConsultApplyDepartment;
            //    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
            //}




            //m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
        }

        private void ControlVisible()
        {
            //DeleteButton.Visible = false;
            if (m_ReadOnly)
            {
                DeleteButton.Visible = false;
                simpleButtonNew.Visible = false;
            }
            //DeleteButton.Visible = false;
        }

        private void BindGridColumnLookUpEditorData()
        {
            repositoryItemLookUpEditEmployee.DataSource = m_DataTableAllEmployee;


        }

        #endregion

        #region 新增
        /// <summary>
        /// 新增事件
        /// edit by Yanqiao.Cai 2012-11-05
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonNew_Click(object sender, EventArgs e)
        {
            try
            {
                Insert();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增事件方法
        /// edit by Yanqiao.Cai 2012-11-05
        /// add try ... catch
        /// </summary>
        private void Insert()
        {
            try
            {
                if (CheckDataDept())
                {
                    InsertData();
                    m_SelectRow = null;
                    SetRepositoryItemLookUpEditDataSource();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool CheckDataDept()
        {
            if (lookUpEditorHospital.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择会诊医院!", CustomMessageBoxKind.WarningOk);
                lookUpEditorHospital.Focus();
                return false;
            }
            else if (lookUpEditorDepartment.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择会诊科室!", CustomMessageBoxKind.WarningOk);
                lookUpEditorDepartment.Focus();
                return false;
            }
            else if (lookUpEditorLevel.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择会诊医师资质!", CustomMessageBoxKind.WarningOk);
                lookUpEditorLevel.Focus();
                return false;
            }
            else if (lookUpEditorEmployee.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择会诊医师!", CustomMessageBoxKind.WarningOk);
                lookUpEditorEmployee.Focus();
                return false;
            }

            DataTable dt = gridControlDepartment.DataSource as DataTable;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //暂不开放多科会诊  edit b yywk  2012年11月10日10:19:32
                if (!dt.Rows[i]["DepartmentCode"].Equals(lookUpEditorDepartment.CodeValue))
                {
                    m_App.CustomMessageBox.MessageShow("多科会诊功能暂未上线", CustomMessageBoxKind.WarningOk);
                    gridViewDept.FocusedRowHandle = i;
                    return false;
                }
                if (dt.Rows[i]["EmployeeLevelID"].Equals(lookUpEditorLevel.CodeValue) && lookUpEditorLevel.CodeValue == "")
                {
                    m_App.CustomMessageBox.MessageShow("已经申请过该级别医生", CustomMessageBoxKind.WarningOk);
                    gridViewDept.FocusedRowHandle = i;
                    return false;
                }
                if (lookUpEditorLevel.CodeValue == "")
                    continue;
                if (dt.Rows[i]["EmployeeCode"].Equals(lookUpEditorEmployee.CodeValue))
                {
                    m_App.CustomMessageBox.MessageShow("该记录已经存在!", CustomMessageBoxKind.WarningOk);
                    gridViewDept.FocusedRowHandle = i;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 部门列表中增加一行
        /// edit by Yanqiao.Cai 2012-11-05
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertData()
        {
            try
            {
                DataTable dt = gridControlDepartment.DataSource as DataTable;
                DataRow dr = dt.NewRow();
                dr["HospitalName"] = lookUpEditorHospital.Text;
                dr["HospitalCode"] = lookUpEditorHospital.CodeValue;
                dr["DepartmentName"] = lookUpEditorDepartment.Text;
                dr["DepartmentCode"] = lookUpEditorDepartment.CodeValue;
                dr["EmployeeLevelName"] = lookUpEditorLevel.Text;
                dr["EmployeeLevelID"] = lookUpEditorLevel.CodeValue;
                dr["EmployeeCode"] = lookUpEditorEmployee.CodeValue;
                dr["EmployeeName"] = lookUpEditorEmployee.Text;
                //edit by Yanqiao.Cai 2012-11-05 修复新增记录列表中会诊医师没有值
                dr["EMPLOYEENAMESTR"] = dr["EmployeeCode"] + "_" + dr["EmployeeName"];
                dr["DeleteButton"] = "删除";
                dt.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 部门列表中增加一行
        /// edit by Yanqiao.Cai 2012-11-05
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditData()
        {
            try
            {
                m_SelectRow["HospitalName"] = lookUpEditorHospital.Text;
                m_SelectRow["HospitalCode"] = lookUpEditorHospital.CodeValue;
                m_SelectRow["DepartmentName"] = lookUpEditorDepartment.Text;
                m_SelectRow["DepartmentCode"] = lookUpEditorDepartment.CodeValue;
                m_SelectRow["EmployeeLevelName"] = lookUpEditorLevel.Text;
                m_SelectRow["EmployeeLevelID"] = lookUpEditorLevel.CodeValue;
                m_SelectRow["EmployeeCode"] = lookUpEditorEmployee.CodeValue;
                m_SelectRow["EmployeeName"] = lookUpEditorEmployee.Text;
                //edit by Yanqiao.Cai 2012-11-05 修复新增记录列表中会诊医师没有值
                m_SelectRow["EMPLOYEENAMESTR"] = m_SelectRow["EmployeeCode"] + "_" + m_SelectRow["EmployeeName"];
                m_SelectRow["DeleteButton"] = "删除";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetRepositoryItemLookUpEditDataSource()
        {
            //DataTable dt = gridControlDepartment.DataSource as DataTable;
            //if (dt != null)
            //{
            //    DataTable dtEmployee = m_DataTableAllEmployee.Clone();
            //    dtEmployee.Clear();
            //    List<string> list = new List<string>();

            //    for (int i = 0; i < m_DataTableAllEmployee.Rows.Count; i++)
            //    {
            //        string code = m_DataTableAllEmployee.Rows[i]["DeptId"].ToString();
            //        string employeeCode = m_DataTableAllEmployee.Rows[i]["EmployeeCode"].ToString();
            //        for (int j = 0; j < dt.Rows.Count; j++)
            //        {
            //            string deptCode = dt.Rows[j]["DepartmentCode"].ToString();
            //            if (code == deptCode)
            //            {
            //                if (!list.Contains(employeeCode))
            //                {
            //                    DataRow dataRow = dtEmployee.NewRow();
            //                    dataRow.ItemArray = m_DataTableAllEmployee.Rows[i].ItemArray;
            //                    dtEmployee.Rows.Add(dataRow);
            //                    list.Add(dataRow["EmployeeCode"].ToString());
            //                }
            //            }
            //        }
            //    }
            //    dtEmployee.AcceptChanges();
            //    repositoryItemLookUpEditEmployee.DataSource = dtEmployee;
            //}
        }
        #endregion

        #region 保存
        public void Save(ConsultStatus status)
        {
            if (CheckData())
            {
                try
                {
                    m_App.SqlHelper.BeginTransaction();
                    SaveConsultationApply(SaveType.RecordModify, m_ConsultApplySN, status);
                    //SaveConsultationRecordDept(m_ConsultApplySN);
                    m_App.SqlHelper.CommitTransaction();

                    m_App.CustomMessageBox.MessageShow("保存成功!", CustomMessageBoxKind.InformationOk);

                    //点击“会诊完成”后关闭界面
                    if (status == ConsultStatus.RecordeComplete)
                    {
                        XtraForm form = this.FindForm() as XtraForm;
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    m_App.SqlHelper.RollbackTransaction();
                    m_App.CustomMessageBox.MessageShow("保存失败!" + ex.Message, CustomMessageBoxKind.InformationOk);
                }
            }
        }

        private string SaveConsultationApply(SaveType saveType, string consultApplySN, ConsultStatus status)
        {

            string typeID = Convert.ToString((int)saveType);
            string consultSuggestion = memoEditSuggestion.Text.Trim();
            string finishTime = dateEditConsultationDate.Text + " " + timeEditConsultationTime.Text;

            return Dal.DataAccess.InsertConsultationApply(typeID, consultApplySN, m_NoOfFirstPage, consultSuggestion, finishTime, Convert.ToString((int)status));
        }

        private void SaveConsultationRecordDept(string consultApplySn)
        {
            DataTable dt = gridControlDepartment.DataSource as DataTable;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string orderValue = Convert.ToString(i + 1);
                string hospitalCode = dt.Rows[i]["HospitalCode"].ToString();
                string departmentCode = dt.Rows[i]["DepartmentCode"].ToString();
                string departmentName = dt.Rows[i]["DepartmentName"].ToString();
                string employeeCode = dt.Rows[i]["EmployeeCode"].ToString();
                string employeeName = dt.Rows[i]["EmployeeName"].ToString();
                string employeeLevelID = dt.Rows[i]["EmployeeLevelID"].ToString();
                string createUser = m_App.User.Id;
                string createTime = System.DateTime.Now.ToString();

                Dal.DataAccess.InsertConsultationRecordDept(consultApplySn, orderValue, hospitalCode, departmentCode, departmentName,
                    employeeCode, employeeName, employeeLevelID, createUser, createTime);
            }
        }

        /// <summary>
        /// 检查控件的输入情况
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            if (memoEditSuggestion.Text.Trim() == "")
            {
                m_App.CustomMessageBox.MessageShow("请填写会诊医师意见!", CustomMessageBoxKind.WarningOk);
                memoEditSuggestion.Text = "";
                memoEditSuggestion.Focus();
                return false;
            }

            DataTable dt = gridControlDepartment.DataSource as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    m_App.CustomMessageBox.MessageShow("请新增受邀医师科室级别!", CustomMessageBoxKind.WarningOk);
                    simpleButtonNew.Focus();
                    return false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string employeeCode = dt.Rows[i]["EmployeeCode"].ToString();
                    if (employeeCode == "")
                    {
                        m_App.CustomMessageBox.MessageShow("请选择会诊医师!", CustomMessageBoxKind.WarningOk);
                        gridViewDept.Focus();
                        return false;
                    }
                }
            }

            if (dateEditConsultationDate.Text.Trim() == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择会诊日期!", CustomMessageBoxKind.WarningOk);
                dateEditConsultationDate.Focus();
                return false;
            }

            if (timeEditConsultationTime.Text.Trim() == "0:00:00")
            {
                m_App.CustomMessageBox.MessageShow("请选择会诊时间!", CustomMessageBoxKind.WarningOk);
                timeEditConsultationTime.Focus();
                return false;
            }

            return true;
        }
        #endregion

        #region LookUpEditor CodeValueChanged事件
        void lookUpEditorDepartment_CodeValueChanged(object sender, EventArgs e)
        {
            ////医师受科室的影响
            //BindEmployee();
            //lookUpEditorEmployee.CodeValue = "";
            lookUpEditorEmployee.CodeValue = "";
            lookUpEditorLevel.CodeValue = "";
            GetDoctor();
        }

        void lookUpEditorHospital_CodeValueChanged(object sender, EventArgs e)
        {
            //科室受医院的影响
            BindDepartment();
            lookUpEditorDepartment.CodeValue = "";
            lookUpEditorEmployee.CodeValue = "";
        }
        #endregion

        #region 设置默认值
        /// <summary>
        /// 设置默认值
        /// </summary>
        private void SetDefaultValue()
        {
            SetDefaultHospital();
        }

        /// <summary>
        /// 选中默认医院
        /// </summary>
        private void SetDefaultHospital()
        {
            if (lookUpEditorHospital.SqlWordbook.BookData.Rows.Count > 0)
            {
                lookUpEditorHospital.CodeValue = lookUpEditorHospital.SqlWordbook.BookData.Rows[0]["ID"].ToString();
            }
        }
        #endregion

        #region GridControl鼠标点击事件
        void gridViewDept_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo hit = gridViewDept.CalcHitInfo(e.X, e.Y);
            if (hit.RowHandle < 0)
            {
                return;
            }

            if (hit.Column != null)
            {
                if (hit.Column.Name == "DeleteButton")
                {
                    DataRow dataRow = gridViewDept.GetDataRow(hit.RowHandle);
                    string signin = dataRow["ISSIGNIN"].ToString();
                    if (signin == "1")
                    {
                        m_App.CustomMessageBox.MessageShow("已经签到不许删除");
                        return;

                    }
                    if (m_App.CustomMessageBox.MessageShow(string.Format("是否删除会诊医生信息？"), CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        int rowIndex = hit.RowHandle;
                        DataTable dataTableSource = gridControlDepartment.DataSource as DataTable;

                        if (rowIndex >= 0 && rowIndex < dataTableSource.Rows.Count)
                        {
                            dataTableSource.Rows.RemoveAt(rowIndex);
                            dataTableSource.AcceptChanges();
                        }
                    }
                }
                else if (hit.Column.Name == "gridColumnEmployee")
                {
                    int rowIndex = hit.RowHandle;
                    m_RowIndexLookUpEditor = rowIndex;
                    DataRow dr = gridViewDept.GetDataRow(rowIndex);
                    string deptCode = dr["DepartmentCode"].ToString();

                    #region
                    DataTable dtEmployee = repositoryItemLookUpEditEmployee.DataSource as DataTable;
                    DataTable dtTempEmployee = dtEmployee.Clone();
                    dtTempEmployee.Clear();
                    if (dtEmployee != null)
                    {
                        for (int i = 0; i < dtEmployee.Rows.Count; i++)
                        {
                            string code = dtEmployee.Rows[i]["DeptId"].ToString();
                            if (code == deptCode)
                            {
                                DataRow dataRow = dtTempEmployee.NewRow();
                                dataRow.ItemArray = dtEmployee.Rows[i].ItemArray;
                                dtTempEmployee.Rows.Add(dataRow);
                            }
                        }

                        for (int i = 0; i < dtEmployee.Rows.Count; i++)
                        {
                            string code = dtEmployee.Rows[i]["DeptId"].ToString();
                            if (code != deptCode)
                            {
                                DataRow dataRow = dtTempEmployee.NewRow();
                                dataRow.ItemArray = dtEmployee.Rows[i].ItemArray;
                                dtTempEmployee.Rows.Add(dataRow);
                            }
                        }

                        dtTempEmployee.AcceptChanges();
                        //repositoryItemLookUpEditEmployee.DataSource = dtTempEmployee;

                        dtEmployee.Clear();
                        for (int i = 0; i < dtTempEmployee.Rows.Count; i++)
                        {
                            DataRow dataRow = dtEmployee.NewRow();
                            dataRow.ItemArray = dtTempEmployee.Rows[i].ItemArray;
                            dtEmployee.Rows.Add(dataRow);
                        }
                    }
                    #endregion
                }
            }
        }
        #endregion

        public void ReadOnlyControl()
        {
            DeleteButton.Visible = false;
            repositoryItemLookUpEditEmployee.ReadOnly = true;
            simpleButtonNew.Enabled = false;
            simpleButtonEdit.Enabled = false;

            memoEditSuggestion.Properties.ReadOnly = true;
            lookUpEditorHospital.ReadOnly = true;
            lookUpEditorLevel.ReadOnly = true;
            lookUpEditorDepartment.ReadOnly = true;
            lookUpEditorEmployee.ReadOnly = true;
            dateEditConsultationDate.Properties.ReadOnly = true;
            timeEditConsultationTime.Properties.ReadOnly = true;
        }

        /// <summary>
        /// 选择会诊医生记录后将对应信息绑定到上面选择框中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDept_Click(object sender, EventArgs e)
        {
            m_SelectRow = null;

            if (gridViewDept.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridViewDept.GetDataRow(gridViewDept.FocusedRowHandle);
            if (foucesRow == null)
                return;

            m_SelectRow = foucesRow;

            if (!foucesRow.IsNull("DepartmentCode"))
                lookUpEditorDepartment.CodeValue = foucesRow["DepartmentCode"].ToString();

            if (!foucesRow.IsNull("EmployeeLevelID"))
                lookUpEditorLevel.CodeValue = foucesRow["EmployeeLevelID"].ToString();

            if (!foucesRow.IsNull("EMPLOYEECODE"))
                lookUpEditorEmployee.CodeValue = foucesRow["EMPLOYEECODE"].ToString();

        }

        private void GetDoctor()
        {

            string deptid = lookUpEditorDepartment.CodeValue;
            string Level = lookUpEditorLevel.CodeValue;
            string usersid = lookUpEditorEmployee.CodeValue;
            if (deptid == "" || usersid != "")
                return;
            else
            {
                string sql = "";
                if (Level == "")
                    sql = string.Format(@"select * from users a where a.deptid = '{0}' and a.grade is not null and a.grade <> '2004'", deptid);
                else
                    sql = string.Format(@"select * from users a where a.grade = '{0}' and a.deptid = '{1}' and a.grade is not null and a.grade <> '2004'", Level, deptid);

                lookUpWindowEmployee.SqlHelper = m_App.SqlHelper;

                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "医生代码";
                Dept.Columns["NAME"].Caption = "医生名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 65);
                cols.Add("NAME", 160);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//py//wb");
                lookUpEditorEmployee.SqlWordbook = deptWordBook;

            }
        }

        /// <summary>
        /// 医生资质修改后医生跟着变换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorLevel_CodeValueChanged(object sender, EventArgs e)
        {
            lookUpEditorEmployee.CodeValue = "";
            GetDoctor();
        }

        /// <summary>
        /// 医生更换 医生资质跟着变换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorEmployee_CodeValueChanged(object sender, EventArgs e)
        {
            string userid = lookUpEditorEmployee.CodeValue;

            string sql = string.Format(@"select a.grade from users a where a.id = '{0}'", userid);

            DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
            if (dt == null || dt.Rows.Count == 0)
                return;
            if (lookUpEditorLevel.CodeValue == dt.Rows[0][0].ToString())
                return;
            lookUpEditorLevel.CodeValue = dt.Rows[0][0].ToString();
            lookUpEditorEmployee.CodeValue = userid;
        }

        private void simpleButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtSource = gridControlDepartment.DataSource as DataTable;
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    if (CheckDataDept())
                    {
                        if (m_SelectRow != null && !m_SelectRow.IsNull("HospitalCode"))
                            EditData();
                        else
                            m_App.CustomMessageBox.MessageShow("请选中一条记录！");
                    }
                }
                else
                {
                    m_App.CustomMessageBox.MessageShow("列表中无数据！");
                }


            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-25</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDept_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }




    }
}
