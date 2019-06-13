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
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.DSSqlHelper;

namespace DrectSoft.Core.Consultation
{
    public partial class UCConsultationInfoForMultiply : DevExpress.XtraEditors.XtraUserControl
    {
        private IEmrHost m_App;
        private string m_NoOfFirstPage;
        private string m_ConsultApplySN = string.Empty;
        private bool m_ReadOnly = false;

        public UCConsultationInfoForMultiply()
        {
            InitializeComponent();
            InitFocus();
        }

        public void Init(string noOfFirstPage, IEmrHost app, bool isNew/*是否是新增申请*/, bool readOnly/*是否只读*/, string consultApplySN)
        {
            m_App = app;
            DS_SqlHelper.CreateSqlHelper();
            m_NoOfFirstPage = noOfFirstPage;
            m_ReadOnly = readOnly;
            m_ConsultApplySN = consultApplySN;

            gridViewDept.OptionsSelection.EnableAppearanceFocusedRow = true;
            gridViewDept.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridViewDept.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;

            RegisterEvent();
            InitInner(isNew);
            InitFocus();
        }

        /// <summary>
        /// 设置初始化焦点
        /// </summary>
        public void InitFocus()
        {
            this.ActiveControl = memoEditAbstract;
        }

        private void InitInner(bool isNew)
        {
            BindData();
            SetDefaultValue();

            if (!isNew)
            {
                SetData();
                ControlVisible();
            }
            //会诊地点默认是本病人所在的科室 add by ywk 
            textEditLocation.Text = m_App.User.CurrentDeptName;
        }
        ///// <summary>
        ///// 根据病人的首页序号获得病人所在科室
        ///// add by ywk 
        ///// </summary>
        ///// <param name="m_NoOfFirstPage"></param>
        ///// <returns></returns>
        //private string GetInpatDept(string m_NoOfFirstPage)
        //{
        //    string sql = string.Format(@"select * fr");
        //}

        private void ControlVisible()
        {
            if (m_ReadOnly)
            {
                DeleteButton.Visible = false;
                simpleButtonNew.Enabled = false;
                btn_reset.Enabled = false;
            }
        }


        /// <summary>
        /// 读取原先填写的数据，并为控件赋值
        /// </summary>
        private void SetData()
        {
            //BindApplyEmployee();

            DataSet ds = Dal.DataAccess.GetConsultationDataSet(m_ConsultApplySN, "20");//, Convert.ToString((int)ConsultType.More));
            DataTable dtConsultApply = ds.Tables[0];
            DataTable dtConsultApplyDepartment = ds.Tables[1];

            if (dtConsultApply.Rows.Count > 0)
            {
                //紧急度
                if (dtConsultApply.Rows[0]["UrgencyTypeID"].ToString() == Convert.ToString((int)UrgencyType.Normal))
                {
                    checkEditNormal.Checked = true;
                    checkEditEmergency.Checked = false;
                }
                else
                {
                    checkEditNormal.Checked = false;
                    checkEditEmergency.Checked = true;
                }

                //摘要
                memoEditAbstract.Text = dtConsultApply.Rows[0]["Abstract"].ToString();

                //会诊目的要求
                memoEditPurpose.Text = dtConsultApply.Rows[0]["Purpose"].ToString();

                //拟会诊时间
                dateEditConsultationDate.Text = dtConsultApply.Rows[0]["ConsultTime"].ToString().Split(' ')[0];
                timeEditConsultationTime.EditValue = dtConsultApply.Rows[0]["ConsultTime"].ToString().Split(' ')[1];

                //会诊地点
                textEditLocation.Text = dtConsultApply.Rows[0]["ConsultLocation"].ToString().Split(' ')[0];

                //申请时间
                dateEditApplyDate.Text = dtConsultApply.Rows[0]["ApplyTime"].ToString().Split(' ')[0];
                //timeEditApplyTime.Text = dtConsultApply.Rows[0]["ApplyTime"].ToString().Split(' ')[1];
                timeEditApplyTime.EditValue = dtConsultApply.Rows[0]["ApplyTime"].ToString().Split(' ')[1];   //

                //申请医师
                lookUpEditorApplyEmployee.CodeValue = dtConsultApply.Rows[0]["ApplyUser"].ToString();

                //科主任
                lookUpEditorDirector.CodeValue = dtConsultApply.Rows[0]["Director"].ToString();
            }

            if (dtConsultApplyDepartment.Rows.Count > 0)
            {
                gridControlDepartment.DataSource = dtConsultApplyDepartment;

                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
            }
        }

        #region 注册、注销事件
        private void RegisterEvent()
        {
            checkEditNormal.CheckedChanged += new EventHandler(checkEditNormal_CheckedChanged);
            checkEditEmergency.CheckedChanged += new EventHandler(checkEditEmergency_CheckedChanged);
            lookUpEditorHospital.CodeValueChanged += new EventHandler(lookUpEditorHospital_CodeValueChanged);
            simpleButtonNew.Click += new EventHandler(simpleButtonNew_Click);
            gridViewDept.MouseDown += new MouseEventHandler(gridViewDept_MouseDown);
        }

        private void UnRegisterEvent()
        {
            checkEditNormal.CheckedChanged -= new EventHandler(checkEditNormal_CheckedChanged);
            checkEditEmergency.CheckedChanged -= new EventHandler(checkEditEmergency_CheckedChanged);
            lookUpEditorHospital.CodeValueChanged -= new EventHandler(lookUpEditorHospital_CodeValueChanged);
            simpleButtonNew.Click -= new EventHandler(simpleButtonNew_Click);
            gridViewDept.MouseDown -= new MouseEventHandler(gridViewDept_MouseDown);
        }

        /// <summary>
        /// 删除事件
        /// edit by Yanqiao.Cai 2012-11-05
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridViewDept_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                // edit by Yanqiao.Cai 2012-11-05
                // 小标题点击不触发事件
                GridHitInfo hit = gridViewDept.CalcHitInfo(e.X, e.Y);
                if (hit.RowHandle < 0)
                {
                    return;
                }

                DataTable dt = gridControlDepartment.DataSource as DataTable;
                if (null == dt || dt.Rows.Count == 0)
                {
                    return;
                }
                else if (gridViewDept.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }



                if (hit.Column != null)
                {
                    if (hit.Column.Name == "DeleteButton")
                    {
                        if (m_App.CustomMessageBox.MessageShow(string.Format("您确定要删除该会诊申请记录信息？"), CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
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
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        void lookUpEditorHospital_CodeValueChanged(object sender, EventArgs e)
        {
            //科室受医院的影响
            BindDepartment();
        }

        void checkEditEmergency_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditEmergency.Checked == true)
            {
                checkEditNormal.Checked = false;
            }
        }

        void checkEditNormal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditNormal.Checked == true)
            {
                checkEditEmergency.Checked = false;
            }
        }

        #endregion

        #region 绑定LookUpEditor数据源

        private void BindData()
        {
            BindHospitalData();
            BindDepartment();
            BindApplyEmployee();
            BindApplyDirector();
            BindDoctorLevel();
            InitGridControlDataSource();
            BindDoctor();
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
            colWidths.Add("NAME", 90);

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
            colWidths.Add("NAME", 90);
            SqlWordbook wordBook = new SqlWordbook("Department", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorDepartment.SqlWordbook = wordBook;
        }

        #endregion

        #region 绑定申请医生
        /// <summary>
        /// 绑定受邀科室
        /// </summary>
        private void BindApplyEmployee()
        {
            lookUpEditorApplyEmployee.Kind = WordbookKind.Sql;
            lookUpEditorApplyEmployee.ListWindow = lookUpWindowApplyEmployee;
            BindApplyEmployeeWordBook(GetConsultationData("4"));
        }

        private void BindApplyEmployeeWordBook(DataTable dataTableData)
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
            colWidths.Add("NAME", 90);
            SqlWordbook wordBook = new SqlWordbook("ApplyEmployee", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorApplyEmployee.SqlWordbook = wordBook;
        }

        #endregion

        #region 绑定申请医生科主任
        /// <summary>
        /// 绑定受邀科室
        /// </summary>
        private void BindApplyDirector()
        {
            lookUpEditorDirector.Kind = WordbookKind.Sql;
            lookUpEditorDirector.ListWindow = lookUpWindowDirector;
            BindApplyDirectorWordBook(GetConsultationData("5"));
        }

        private void BindApplyDirectorWordBook(DataTable dataTableData)
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
            colWidths.Add("NAME", 90);
            SqlWordbook wordBook = new SqlWordbook("Director", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorDirector.SqlWordbook = wordBook;
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
            colWidths.Add("NAME", 150);
            SqlWordbook wordBook = new SqlWordbook("Director", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorLevel.SqlWordbook = wordBook;
        }

        #endregion

        private void BindDoctor()
        {

            string sql = string.Format(@"select * from users where 1=2");

            lookUpWindowDoctor.SqlHelper = m_App.SqlHelper;

            DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

            Dept.Columns["ID"].Caption = "医生代码";
            Dept.Columns["NAME"].Caption = "医生名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 60);
            cols.Add("NAME", 90);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//py//wb");
            lookUpEditorDoctor.SqlWordbook = deptWordBook;
        }

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
            else if (typeID == "9")//医师级别
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, "20");
            }

            return dataTableConsultationData;
        }

        #endregion

        #region 设置默认值
        /// <summary>
        /// 设置默认值
        /// </summary>
        private void SetDefaultValue()
        {
            SetDefaultHospital();
            SetDefaultDate();
            SetDefaultApplyEmployee();
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

        /// <summary>
        /// 设置默认时间
        /// </summary>
        private void SetDefaultDate()
        {
            //if (dateEditApplyDate.Text.Trim() == "" || timeEditApplyTime.Text.Trim() == "")
            //{
            dateEditApplyDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            timeEditApplyTime.EditValue = System.DateTime.Now.ToString().Split(' ')[1];
            //}

            //if (dateEditApplyDate.Text.Trim() == "" || timeEditApplyTime.Text.Trim() == "")
            //{
            dateEditConsultationDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            timeEditConsultationTime.EditValue = System.DateTime.Now.ToString().Split(' ')[1];
            //}

        }

        /// <summary>
        /// 设置默认申请医师
        /// </summary>
        private void SetDefaultApplyEmployee()
        {
            lookUpEditorApplyEmployee.CodeValue = m_App.User.Id;
        }
        #endregion

        public void Clear()
        {
            SetDefaultValue();
            checkEditNormal.Checked = false;
            checkEditEmergency.Checked = false;
            memoEditAbstract.Text = "";
            memoEditPurpose.Text = "";
            textEditLocation.Text = "";

            lookUpEditorDepartment.CodeValue = "";
            lookUpEditorLevel.CodeValue = "";
            lookUpEditorDirector.CodeValue = "";

            //清空部门列表
            DataTable dataTableDepartment = gridControlDepartment.DataSource as DataTable;
            if (dataTableDepartment != null)
            {
                dataTableDepartment.Rows.Clear();
            }
        }

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
        /// 新增事件
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
                    lookUpEditorHospital.Focus();
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
                m_App.CustomMessageBox.MessageShow("请选择受邀医院", CustomMessageBoxKind.WarningOk);
                lookUpEditorHospital.Focus();
                return false;
            }
            else if (lookUpEditorDepartment.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择受邀科室", CustomMessageBoxKind.WarningOk);
                lookUpEditorDepartment.Focus();
                return false;
            }
            else if (lookUpEditorLevel.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择受邀医师资质", CustomMessageBoxKind.WarningOk);
                lookUpEditorLevel.Focus();
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
                if (dt.Rows[i]["EmployeeLevelID"].Equals(lookUpEditorLevel.CodeValue) && lookUpEditorDoctor.CodeValue == "")
                {
                    m_App.CustomMessageBox.MessageShow("已经申请过该级别医生", CustomMessageBoxKind.WarningOk);
                    gridViewDept.FocusedRowHandle = i;
                    return false;
                }
                if (lookUpEditorDoctor.CodeValue == "")
                    continue;
                if (dt.Rows[i]["EmployeeID"].Equals(lookUpEditorDoctor.CodeValue))
                {
                    m_App.CustomMessageBox.MessageShow("该记录已经存在", CustomMessageBoxKind.WarningOk);
                    gridViewDept.FocusedRowHandle = i;
                    return false;
                }


            }
            //            string sql = string.Format(@"select id from consultapplydepartment a ,consultapply b  where a.DEPARTMENTCODE='{0}'
            //             and a.EMPLOYEECODE='{1}' and a.EMPLOYEELEVELID='{2}' and a.valid='1' and b.stateid='6730' and b.noofinpat='{3}'", lookUpEditorDepartment.CodeValue
            //                                                                           , lookUpEditorDoctor.CodeValue,
            //                                                                           lookUpEditorLevel.CodeValue,m_App.CurrentPatientInfo.NoOfFirstPage);

            //这个逻辑判断不应该加  杨伟康注释掉 2013年1月14日12:10:04
            //            string sql = string.Format(@"select A.Consultapplysn,B.Consultapplysn from (
            //        (select Consultapplysn from consultapplydepartment where DEPARTMENTCODE='{0}'
            //             and EMPLOYEECODE='{1}' and EMPLOYEELEVELID='{2}' and valid='1' )A) 
            //inner join consultapply B on A.Consultapplysn=B.Consultapplysn and B.Stateid='6730' and B.Noofinpat='{3}'", lookUpEditorDepartment.CodeValue
            //                                                                           , lookUpEditorDoctor.CodeValue,
            //                                                                           lookUpEditorLevel.CodeValue, m_App.CurrentPatientInfo.NoOfFirstPage);

            //            if (m_App.SqlHelper.ExecuteDataTable(sql, CommandType.Text).Rows.Count > 0)
            //            {
            //                m_App.CustomMessageBox.MessageShow("此病人已申请会诊，请勿重复提交！", CustomMessageBoxKind.WarningOk);
            //                //lookUpEditorHospital.Focus();
            //                return false;
            //            }

            return true;
        }

        /// <summary>
        /// 部门列表中增加一行
        /// </summary>
        private void InsertData()
        {
            DataTable dt = gridControlDepartment.DataSource as DataTable;
            DataRow dr = dt.NewRow();
            dr["HospitalName"] = lookUpEditorHospital.Text;
            dr["HospitalCode"] = lookUpEditorHospital.CodeValue;
            dr["DepartmentName"] = lookUpEditorDepartment.Text;
            dr["DepartmentCode"] = lookUpEditorDepartment.CodeValue;
            dr["EmployeeLevelName"] = lookUpEditorLevel.Text;
            dr["EmployeeLevelID"] = lookUpEditorLevel.CodeValue;
            dr["DeleteButton"] = "删除";
            dr["EmployeeName"] = lookUpEditorDoctor.Text;
            dr["EmployeeNameStr"] = lookUpEditorDoctor.CodeValue == "" ? "" : lookUpEditorDoctor.CodeValue + "_" + lookUpEditorDoctor.Text;
            dr["EmployeeID"] = lookUpEditorDoctor.CodeValue;
            dt.Rows.Add(dr);
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
            dt.Columns.Add("DeleteButton");
            dt.Columns.Add("EmployeeName");
            dt.Columns.Add("EmployeeNameStr");
            dt.Columns.Add("EmployeeID");
            gridControlDepartment.DataSource = dt;

            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
        }

        public void ReadOnlyControl()
        {
            try
            {
                //edit by cyq 2012-10-19
                //checkEditNormal.Properties.ReadOnly = true;
                //checkEditEmergency.Properties.ReadOnly = true;
                //memoEditAbstract.Properties.ReadOnly = true;
                //memoEditPurpose.Properties.ReadOnly = true;
                //lookUpEditorHospital.ReadOnly = true;
                //lookUpEditorDepartment.ReadOnly = true;
                //lookUpEditorLevel.ReadOnly = true;
                //lookUpEditorDoctor.ReadOnly = true;
                //dateEditConsultationDate.Properties.ReadOnly = true;
                //timeEditConsultationTime.Properties.ReadOnly = true;
                //textEditLocation.Properties.ReadOnly = true;
                //dateEditApplyDate.Properties.ReadOnly = true;
                //timeEditApplyTime.Properties.ReadOnly = true;
                //lookUpEditorApplyEmployee.ReadOnly = true;
                //lookUpEditorDirector.ReadOnly = true;

                checkEditNormal.Enabled = false;
                checkEditEmergency.Enabled = false;
                memoEditAbstract.Enabled = false;
                memoEditPurpose.Enabled = false;
                lookUpEditorHospital.Enabled = false;
                lookUpEditorDepartment.Enabled = false;
                lookUpEditorLevel.Enabled = false;
                lookUpEditorDoctor.Enabled = false;
                dateEditConsultationDate.Enabled = false;
                timeEditConsultationTime.Enabled = false;
                textEditLocation.Enabled = false;
                dateEditApplyDate.Enabled = false;
                timeEditApplyTime.Enabled = false;
                //gridControlDepartment.Enabled = false;
                lookUpEditorApplyEmployee.Enabled = false;
                lookUpEditorDirector.Enabled = false;

                simpleButtonNew.Enabled = false;
                btn_reset.Enabled = false;
                DeleteButton.Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存
        /// edit by wangji 2013 1.6
        /// </summary>
        public void Save()
        {
            if (CheckData())
            {
                try
                {
                    //m_App.SqlHelper.BeginTransaction();
                    DS_SqlHelper.BeginTransaction();
                    string consultApplySn = string.Empty;

                    if (m_ConsultApplySN == string.Empty)//新增
                    {
                        consultApplySn = SaveConsultationApply(SaveType.Insert, "");
                        SaveConsultationApplyDept(consultApplySn);
                        #region 注释
                        //                        DataTable dt = gridControlDepartment.DataSource as DataTable;

                        //                        for (int i = 0; i < dt.Rows.Count; i++)
                        //                        {
                        //                            //string orderValue = Convert.ToString(i + 1);
                        //                            //string hospitalCode = lookUpEditorHospital.CodeValue;
                        //                            //string departmentCode = lookUpEditorDepartment.CodeValue;
                        //                            string hospitalCode = dt.Rows[i]["HospitalCode"].ToString();
                        //                            string departmentCode = dt.Rows[i]["DepartmentCode"].ToString();
                        //                            string departmentName = dt.Rows[i]["DepartmentName"].ToString();
                        //                            string employeeCode = dt.Rows[i]["EmployeeID"].ToString();
                        //                            string employeeName = dt.Rows[i]["EmployeeName"].ToString();
                        //                            //string employeeLevelID = lookUpEditorLevel.CodeValue;
                        //                            string employeeLevelID = dt.Rows[i]["employeeLevelID"].ToString();

                        //                            string sql = string.Format(@"select  ID from 
                        //consultapplydepartment where CONSULTAPPLYSN='{0}' and DEPARTMENTCODE='{1}' 
                        //and EMPLOYEECODE='{2}' and EMPLOYEELEVELID='{3}'", consultApplySn, departmentCode, employeeCode, employeeLevelID);
                        //                            if (m_App.SqlHelper.ExecuteDataTable(sql, CommandType.Text).Rows.Count > 0)
                        //                            {
                        //                                m_App.CustomMessageBox.MessageShow("此病人已申请会诊，请勿重复提交！");

                        //                                string updatesql = string.Format("update consultapplydepartment set valid='0' where CONSULTAPPLYSN='{0}'",consultApplySn);
                        //                                m_App.SqlHelper.ExecuteDataTable(updatesql,CommandType.Text);
                        //                                string updatesql1 = string.Format("update consultapply set valid='0' where CONSULTAPPLYSN='{0}'", consultApplySn);
                        //                                m_App.SqlHelper.ExecuteDataTable(updatesql1, CommandType.Text);
                        //                                string updatesql2 = string.Format("update consultrecorddepartment set valid='0' where CONSULTAPPLYSN='{0}'", consultApplySn);
                        //                                m_App.SqlHelper.ExecuteDataTable(updatesql2, CommandType.Text);
                        //                                gridControlDepartment.DataSource = null;
                        //                            }

                        //}
                        #endregion
                        m_ConsultApplySN = consultApplySn;

                        //m_App.SqlHelper.CommitTransaction();

                    }
                    else//修改
                    {
                        SaveConsultationApply(SaveType.Modify, m_ConsultApplySN);
                        SaveConsultationApplyDept(m_ConsultApplySN);
                        //m_App.SqlHelper.CommitTransaction();
                        //DS_SqlHelper.CommitTransaction();
                    }
                    DS_SqlHelper.CommitTransaction();
                    m_App.CustomMessageBox.MessageShow("保存成功", CustomMessageBoxKind.InformationOk);
                }
                catch (Exception ex)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                    //m_App.SqlHelper.RollbackTransaction();
                }
            }
        }


        /// <summary>
        /// 检查控件的输入情况
        /// edit by Yanqiao.Cai 2012-11-05
        /// add try ... catch
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            try
            {
                DataTable dt = gridControlDepartment.DataSource as DataTable;
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        m_App.CustomMessageBox.MessageShow("请先新增会诊申请记录", CustomMessageBoxKind.WarningOk);
                        simpleButtonNew.Focus();
                        return false;
                    }
                }

                if (checkEditEmergency.Checked == false && checkEditNormal.Checked == false)
                {
                    m_App.CustomMessageBox.MessageShow("请选择紧急度", CustomMessageBoxKind.WarningOk);
                    checkEditNormal.Focus();
                    return false;
                }

                if (memoEditAbstract.Text.Trim() == "")
                {
                    m_App.CustomMessageBox.MessageShow("病历摘要不能为空", CustomMessageBoxKind.WarningOk);
                    memoEditAbstract.Focus();
                    return false;
                }

                if (memoEditAbstract.Text.Trim().Length > 3000)
                {
                    m_App.CustomMessageBox.MessageShow("病历摘要长度不能大于3000", CustomMessageBoxKind.WarningOk);
                    memoEditAbstract.Focus();
                    return false;
                }

                if (memoEditPurpose.Text.Trim() == "")
                {
                    m_App.CustomMessageBox.MessageShow("目的要求不能为空", CustomMessageBoxKind.WarningOk);
                    memoEditPurpose.Focus();
                    return false;
                }

                if (memoEditPurpose.Text.Trim().Length > 3000)
                {
                    m_App.CustomMessageBox.MessageShow("目的要求长度不能大于3000", CustomMessageBoxKind.WarningOk);
                    memoEditPurpose.Focus();
                    return false;
                }

                //if (lookUpEditorHospital.CodeValue == "")
                //{
                //    m_App.CustomMessageBox.MessageShow("请选择受邀医院", CustomMessageBoxKind.WarningOk);
                //    lookUpEditorHospital.Focus();
                //    return false;
                //}

                //if (lookUpEditorDepartment.CodeValue == "")
                //{
                //    m_App.CustomMessageBox.MessageShow("请选择受邀科室", CustomMessageBoxKind.WarningOk);
                //    lookUpEditorDepartment.Focus();
                //    return false;
                //}

                //if (lookUpEditorLevel.CodeValue == "")
                //{
                //    m_App.CustomMessageBox.MessageShow("请选择受邀医师级别", CustomMessageBoxKind.WarningOk);
                //    lookUpEditorLevel.Focus();
                //    return false;
                //}

                if (dateEditConsultationDate.Text.Trim() == "")
                {
                    m_App.CustomMessageBox.MessageShow("请选择拟会诊日期", CustomMessageBoxKind.WarningOk);
                    dateEditConsultationDate.Focus();
                    return false;
                }

                if (timeEditConsultationTime.Text.Trim() == "0:00:00")
                {
                    m_App.CustomMessageBox.MessageShow("请选择拟会诊时间", CustomMessageBoxKind.WarningOk);
                    timeEditConsultationTime.Focus();
                    return false;
                }

                if (DateTime.Parse(dateEditApplyDate.Text + " " + timeEditApplyTime.Text) >= DateTime.Parse(dateEditConsultationDate.Text + " " + timeEditConsultationTime.Text))
                {
                    m_App.CustomMessageBox.MessageShow("拟会诊时间不能小于或等于申请时间", CustomMessageBoxKind.WarningOk);
                    timeEditConsultationTime.Focus();
                    return false;
                }

                if (textEditLocation.Text.Trim() == "")
                {
                    m_App.CustomMessageBox.MessageShow("会诊地点不能为空", CustomMessageBoxKind.WarningOk);
                    textEditLocation.Focus();
                    return false;
                }

                if (dateEditApplyDate.Text.Trim() == "")
                {
                    m_App.CustomMessageBox.MessageShow("请选择申请日期", CustomMessageBoxKind.WarningOk);
                    dateEditApplyDate.Focus();
                    return false;
                }

                if (timeEditApplyTime.Text.Trim() == "0:00:00")
                {
                    m_App.CustomMessageBox.MessageShow("请选择申请时间", CustomMessageBoxKind.WarningOk);
                    timeEditApplyTime.Focus();
                    return false;
                }

                if (lookUpEditorApplyEmployee.CodeValue == "")
                {
                    m_App.CustomMessageBox.MessageShow("请选择申请医师", CustomMessageBoxKind.WarningOk);
                    lookUpEditorApplyEmployee.Text = "";
                    lookUpEditorApplyEmployee.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string SaveConsultationApply(SaveType saveType, string consultApplySN)
        {
            try
            {
                string typeID = Convert.ToString((int)saveType);
                string urgencyTypeID = string.Empty;
                if (checkEditNormal.Checked == true)
                {
                    urgencyTypeID = Convert.ToString((int)UrgencyType.Normal);
                }
                else if (checkEditEmergency.Checked == true)
                {
                    urgencyTypeID = Convert.ToString((int)UrgencyType.Urgency);
                }

                //string consultTypeID = Convert.ToString((int)ConsultType.More);
                string consultTypeID = "";
                string abstractContent = memoEditAbstract.Text;
                string purpose = memoEditPurpose.Text;
                string applyUser = lookUpEditorApplyEmployee.CodeValue;
                string applyTime = dateEditApplyDate.Text + " " + timeEditApplyTime.Text;
                string director = lookUpEditorDirector.CodeValue;
                string consultTime = dateEditConsultationDate.Text + " " + timeEditConsultationTime.Text;
                string consultLocation = textEditLocation.Text;
                //edit by tj 2012-10-26  如果是不需要会诊设流程 则在新增会诊申请时直接将状态设为待【待会诊】，否则设为【待审核】
                string stateID = string.Empty;
                if (CommonObjects.IsNeedVerifyInConsultation)
                {
                    stateID = Convert.ToString((int)ConsultStatus.WaitApprove);
                }
                else
                {
                    stateID = Convert.ToString((int)ConsultStatus.WaitConsultation);
                }
                string createUser = m_App.User.Id;
                string createTime = System.DateTime.Now.ToString();

                return Dal.DataAccess.InsertConsultationApply(typeID, consultApplySN, m_NoOfFirstPage, urgencyTypeID, consultTypeID, abstractContent, purpose,
                    applyUser, applyTime, director, consultTime, consultLocation, stateID, createUser, createTime, m_App.User.CurrentDeptId,lookUpEditorApplyEmployee.CodeValue,"");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SaveConsultationApplyDept(string consultApplySn)
        {
            try
            {
                string sql = string.Format("");
                DataTable dt = gridControlDepartment.DataSource as DataTable;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string orderValue = Convert.ToString(i + 1);
                    //string hospitalCode = lookUpEditorHospital.CodeValue;
                    //string departmentCode = lookUpEditorDepartment.CodeValue;
                    string hospitalCode = dt.Rows[i]["HospitalCode"].ToString();
                    string departmentCode = dt.Rows[i]["DepartmentCode"].ToString();
                    string departmentName = dt.Rows[i]["DepartmentName"].ToString();
                    string employeeCode = dt.Rows[i]["EmployeeID"].ToString();
                    string employeeName = dt.Rows[i]["EmployeeName"].ToString();
                    //string employeeLevelID = lookUpEditorLevel.CodeValue;
                    string employeeLevelID = dt.Rows[i]["employeeLevelID"].ToString();
                    string createUser = m_App.User.Id;
                    string createTime = System.DateTime.Now.ToString();

                    Dal.DataAccess.InsertConsultationApplyDept(consultApplySn, orderValue, hospitalCode, departmentCode, departmentName,
                        employeeCode, employeeName, employeeLevelID, createUser, createTime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetDoctor()
        {
            string deptid = lookUpEditorDepartment.CodeValue;
            string Level = lookUpEditorLevel.CodeValue;
            string usersid = lookUpEditorDoctor.CodeValue;
            if (deptid == "" || usersid != "")
                return;
            else
            {
                try
                {
                    string sql = "";
                    if (Level == "")
                        sql = string.Format(@"select ID,NAME,PY,WB from users a where a.deptid = '{0}' and a.grade is not null and a.grade <> '2004'", deptid);
                    else
                        sql = string.Format(@"select ID,NAME,PY,WB from users a where a.grade = '{0}' and a.deptid = '{1}' and a.grade is not null and a.grade <> '2004'", Level, deptid);

                    lookUpWindowDoctor.SqlHelper = m_App.SqlHelper;

                    DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);
                    //add by ywk  2012年12月3日10:21:34
                    //如果有医生对应了两个科室或者多个科室这边应该再加上 (根据所选的科室)
                    string sqlsearch = string.Format(@"select  USERID,DEPTID,WARDID  from USER2DEPT where deptid='{0}' ", deptid);
                    DataTable dtuser2Dept = m_App.SqlHelper.ExecuteDataTable(sqlsearch, CommandType.Text);
                    DataTable dtResultTemp = Dept.Clone();
                    string splitersql = string.Empty;
                    string sql1 = string.Empty;
                    if (dtuser2Dept.Rows.Count > 0 && dtuser2Dept != null)
                    {

                        for (int i = 0; i < dtuser2Dept.Rows.Count; i++)
                        {
                            splitersql += dtuser2Dept.Rows[i]["USERID"].ToString() + ",";
                        }
                        splitersql = splitersql.Remove(splitersql.Length - 1);
                        sql1 = string.Format("select ID,NAME,PY,WB from users where id in ({0}) and grade='{1}'", splitersql, Level);
                        dtResultTemp = m_App.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
                    }

                    if (dtResultTemp != null && dtResultTemp.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtResultTemp.Rows.Count; i++)
                        {
                            DataRow dr = Dept.NewRow();
                            dr["ID"] = dtResultTemp.Rows[i]["ID"].ToString();
                            dr["NAME"] = dtResultTemp.Rows[i]["NAME"].ToString();
                            dr["PY"] = dtResultTemp.Rows[i]["PY"].ToString();
                            dr["WB"] = dtResultTemp.Rows[i]["WB"].ToString();
                            Dept.Rows.Add(dr);
                        }
                    }


                    Dept.Columns["ID"].Caption = "医生代码";
                    Dept.Columns["NAME"].Caption = "医生名称";

                    Dictionary<string, int> cols = new Dictionary<string, int>();

                    cols.Add("ID", 65);
                    cols.Add("NAME", 160);

                    DataView dv = new DataView(Dept);//虚拟视图 edit by ywk 
                    DataTable dtresu = dv.ToTable(true, "ID", "NAME", "PY", "WB");

                    SqlWordbook deptWordBook = new SqlWordbook("querybook", dtresu, "ID", "NAME", cols, "ID//NAME//py//wb");
                    lookUpEditorDoctor.SqlWordbook = deptWordBook;

                }
                catch (Exception ex)
                {
                    m_App.CustomMessageBox.MessageShow(ex.Message);
                    return;
                }


            }
        }

        /// <summary>
        /// 选择科室事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorDepartment_CodeValueChanged(object sender, EventArgs e)
        {
            lookUpEditorDoctor.CodeValue = "";
            lookUpEditorLevel.CodeValue = "";
            GetDoctor();
        }

        /// <summary>
        /// 选中医生级别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorLevel_CodeValueChanged(object sender, EventArgs e)
        {
            lookUpEditorDoctor.CodeValue = "";
            GetDoctor();
        }

        private void lookUpEditorDoctor_CodeValueChanged(object sender, EventArgs e)
        {
            string userid = lookUpEditorDoctor.CodeValue;

            string sql = string.Format(@"select a.grade from users a where a.id = '{0}'", userid);

            DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
            if (dt == null || dt.Rows.Count == 0)
                return;
            if (lookUpEditorLevel.CodeValue == dt.Rows[0][0].ToString())
                return;
            lookUpEditorDoctor.CodeValue = "";
            lookUpEditorLevel.CodeValue = dt.Rows[0][0].ToString();
            lookUpEditorDoctor.CodeValue = userid;
        }

        /// <summary>
        /// 复选框回车事件
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
                m_App.CustomMessageBox.MessageShow(ex.Message);
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

        /// <summary>
        /// 重置事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-14</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 重置方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-14</date>
        /// </summary>
        private void Reset()
        {
            try
            {
                lookUpEditorHospital.CodeValue = string.Empty;
                lookUpEditorDepartment.CodeValue = string.Empty;
                lookUpEditorLevel.CodeValue = string.Empty;
                lookUpEditorDoctor.CodeValue = string.Empty;
                lookUpEditorHospital.Focus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
