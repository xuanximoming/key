using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Eop;
using DrectSoft.Common.Library;
using DrectSoft.Core.Consultation.Dal;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.Consultation
{
    public partial class UCConsultationMultiply : DevExpress.XtraEditors.XtraUserControl, IEMREditor
    {
        ConsultationEntity m_ConsultationEntity;
        Inpatient CurrentInpatient;
        private string m_ConsultationID = "";

        IEmrHost m_app;
        private int m_RowIndexLookUpEditor = -1;
        /// <summary>
        /// 保存User表中所有的数据
        /// </summary>
        private DataTable m_DataTableAllEmployee;
        private string m_ConsultTypeID = "";
        private StringFormat m_SFCenter;

        /// <summary>
        /// 会诊信息选中行
        /// </summary>
        private DataRow m_SelectRow;

        public UCConsultationMultiply(string noofinpat, string consultationID)
            : this(consultationID)
        {
            CurrentNoofinpat = noofinpat;
        }

        public UCConsultationMultiply(string ConsultationID)
        {
            InitializeComponent();
            m_ConsultationID = ConsultationID;

        }

        public UCConsultationMultiply(ConsultationEntity consultationEntity, Inpatient _CurrentInpatient)
        {
            InitializeComponent();
            m_ConsultationID = consultationEntity.ConsultApplySn;
            CurrentInpatient = _CurrentInpatient;
            m_ConsultationEntity = consultationEntity;
        }

        public UCConsultationMultiply()
        {
            InitializeComponent();
            m_ConsultationID = "";
        }

        private void GetConsultationEntity()
        {
            if (m_ConsultationID == "")
                return;
            else
            {
                DataTable dt = DataAccess.GetConsultationTable(m_ConsultationID);
                DataTable dtinp = Dal.DataAccess.GetRedactPatientInfoFrm("14", CurrentInpatient.NoOfFirstPage.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    m_ConsultationEntity = new ConsultationEntity();

                    m_ConsultationEntity.ConsultApplySn = m_ConsultationID;
                    m_ConsultationEntity.NoOfInpat = CurrentInpatient.NoOfFirstPage.ToString();
                    m_ConsultationEntity.HospitalName = m_app.CurrentHospitalInfo.Name;

                    if (dtinp.Rows.Count > 0)
                    {
                        m_ConsultationEntity.Name = dtinp.Rows[0]["NAME"].ToString().Trim();
                        m_ConsultationEntity.PatNoOfHIS = dtinp.Rows[0]["PatID"].ToString().Trim();
                        m_ConsultationEntity.SexName = dtinp.Rows[0]["Gender"].ToString().Trim();
                        m_ConsultationEntity.Age = dtinp.Rows[0]["AgeStr"].ToString().Trim();
                        m_ConsultationEntity.Bed = dtinp.Rows[0]["OutBed"].ToString().Trim();
                        m_ConsultationEntity.DeptName = dtinp.Rows[0]["OutHosDeptName"].ToString().Trim();
                        m_ConsultationEntity.WardName = dtinp.Rows[0]["outhoswardname"].ToString().Trim();
                        m_ConsultationEntity.DeptID = dtinp.Rows[0]["OutHosDept"].ToString().Trim();
                        m_ConsultationEntity.WardID = dtinp.Rows[0]["outhosward"].ToString().Trim();
                        //textEditMarriage.Text = dtinp.Rows[0]["Marriage"].ToString().Trim();
                        //textEditJob.Text = dtinp.Rows[0]["JobName"].ToString().Trim();
                    }
                    m_ConsultTypeID = dr["consulttypeid"].ToString();

                    m_ConsultationEntity.UrgencyTypeID = dr["urgencytypeid"].ToString();
                    m_ConsultationEntity.UrgencyTypeName = dr["urgencytypeName"].ToString();
                    m_ConsultationEntity.ConsultTypeID = dr["consulttypeid"].ToString();
                    m_ConsultationEntity.ConsultTypeName = dr["consulttypeName"].ToString();
                    m_ConsultationEntity.Abstract = dr["abstract"].ToString();

                    m_ConsultationEntity.Purpose = dr["purpose"].ToString();
                    m_ConsultationEntity.ApplyDeptID = dr["ApplyDeptID"].ToString();
                    m_ConsultationEntity.ApplyDeptName = dr["ApplyDeptName"].ToString();
                    m_ConsultationEntity.ApplyUserID = dr["applyuserID"].ToString();
                    m_ConsultationEntity.ApplyUserName = dr["applyuserName"].ToString();

                    m_ConsultationEntity.ApplyTime = dr["applytime"].ToString();
                    m_ConsultationEntity.ConsultSuggestion = dr["consultsuggestion"].ToString();
                    m_ConsultationEntity.ConsultDeptID = dr["ConsultDeptID"].ToString();
                    m_ConsultationEntity.ConsultDeptName = dr["ConsultDeptName"].ToString();
                    m_ConsultationEntity.ConsultHospitalID = dr["hospitalcode"].ToString();

                    m_ConsultationEntity.ConsultHospitalName = dr["ConsultHospitalName"].ToString();
                    m_ConsultationEntity.ConsultDeptID2 = dr["ConsultDeptID2"].ToString();
                    m_ConsultationEntity.ConsultDeptName2 = dr["ConsultDeptName2"].ToString();
                    m_ConsultationEntity.ConsultUserID = dr["ConsultUserID"].ToString();
                    m_ConsultationEntity.ConsultUserName = dr["ConsultUserName"].ToString();

                    m_ConsultationEntity.ConsultTime = dr["ConsultTime"].ToString();
                    m_ConsultationEntity.StateID = dr["StateID"].ToString();
                    return;
                }
            }
        }

        private void FillUI()
        {
            if (m_ConsultationEntity == null || m_ConsultationEntity.ConsultApplySn == "")
            {
                memoConsultSuggestion.Properties.ReadOnly = true;
                lookUpEditorHospital.Enabled = false;
                lookUpEditorDepartment.Enabled = false;
                lookUpEditorEmployee.Enabled = false;

                dateEditConsultationDate.Enabled = false;
                timeEditConsultationTime.Enabled = false;
                return;
            }

            else
            {
                txtName.Text = m_ConsultationEntity.Name;
                txtSex.Text = m_ConsultationEntity.SexName;
                txtAge.Text = m_ConsultationEntity.Age;
                txtDept.Text = m_ConsultationEntity.DeptName;
                txtWard.Text = m_ConsultationEntity.WardName;

                txtBed.Text = m_ConsultationEntity.Bed;
                txtPatNoOfHis.Text = m_ConsultationEntity.PatNoOfHIS;

                memoAbstract.Text = m_ConsultationEntity.Abstract;
                memoPurpose.Text = m_ConsultationEntity.Purpose;


                //txtConsultDept.Text = m_ConsultationEntity.ConsultDeptName;
                txtApplyDept.Text = m_ConsultationEntity.ApplyDeptName;
                txtApplyUser.Text = m_ConsultationEntity.ApplyUserName;
                txtApplyTime.Text = m_ConsultationEntity.ApplyTime;

                if (m_ConsultationEntity.StateID == "6741" || m_ConsultationEntity.StateID == "6740")
                {
                    memoConsultSuggestion.Text = m_ConsultationEntity.ConsultSuggestion;
                    //lookUpEditorHospital.Text = m_ConsultationEntity.ConsultHospitalName;
                    //lookUpEditorDepartment.Text = m_ConsultationEntity.ConsultDeptName2;
                    //lookUpEditorEmployee.Text = m_ConsultationEntity.ConsultUserName;
                    //txtConsultTime.Text = m_ConsultationEntity.ConsultTime;

                    if (m_ConsultationEntity.ConsultTime.Trim().Split(' ').Length == 2)
                    {
                        dateEditConsultationDate.EditValue = m_ConsultationEntity.ConsultTime.Split(' ')[0];
                        timeEditConsultationTime.EditValue = m_ConsultationEntity.ConsultTime.Split(' ')[1];
                    }
                    //会诊记录完成(中心人民医院需求，完成后会诊单仍然需要开放修改功能，防止需要将手填会诊意见填入系统的需求出现】)
                    if (m_ConsultationEntity.StateID == "6741")
                    {
                        memoConsultSuggestion.Properties.ReadOnly = false;//true
                        lookUpEditorHospital.Enabled = false;
                        lookUpEditorDepartment.Enabled = false;
                        lookUpEditorEmployee.Enabled = false;
                        lookUpEditorLevel.Enabled = false;

                        DeleteButton.Visible = false;
                        simpleButtonNew.Visible = false;
                        simpleButtonEdit.Visible = false;

                        dateEditConsultationDate.Enabled = false;
                        timeEditConsultationTime.Enabled = false;

                        this.btnSave.Enabled = false;
                        this.btnOver.Enabled = true;//false

                        //SetControlReadOnlyColor();

                        return;
                    }
                }
                else
                {
                    m_ConsultationEntity.ConsultTime = "";
                    m_ConsultationEntity.ConsultSuggestion = "";
                    m_ConsultationEntity.ConsultHospitalName = "";
                    m_ConsultationEntity.ConsultDeptName2 = "";
                    m_ConsultationEntity.ConsultUserName = "";
                }


                //else
                //{
                memoConsultSuggestion.Properties.ReadOnly = false;
                lookUpEditorHospital.Enabled = true;
                lookUpEditorDepartment.Enabled = true;
                lookUpEditorEmployee.Enabled = true;
                lookUpEditorLevel.Enabled = true;

                //dateEditConsultationDate.Enabled = true;
                //timeEditConsultationTime.Enabled = true;

                timeEditConsultationTime.Properties.ReadOnly = true;
                dateEditConsultationDate.Properties.ReadOnly = true;

                //设置页面按钮状态
                InitBtnState(m_ConsultationEntity.ApplyDeptID);


                //}

            }
        }

        private void SetControlReadOnlyColor()
        {
            lookUpEditorHospital.BackColor = Color.White;
            lookUpEditorHospital.ForeColor = Color.Black;
            lookUpEditorDepartment.BackColor = Color.White;
            lookUpEditorDepartment.ForeColor = Color.Black;
            lookUpEditorEmployee.BackColor = Color.White;
            lookUpEditorEmployee.ForeColor = Color.Black;
            lookUpEditorLevel.BackColor = Color.White;
            lookUpEditorLevel.ForeColor = Color.Black;
            timeEditConsultationTime.BackColor = Color.White;
            timeEditConsultationTime.ForeColor = Color.Black;
            dateEditConsultationDate.BackColor = Color.White;
            dateEditConsultationDate.ForeColor = Color.Black;
        }

        private void PanelControlInit()
        {
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(141)))), ((int)(((byte)(189)))));
        }

        private void InitInner(bool isNew)
        {
            BindGridColumnLookUpEditorData();
            BindData();
            SetDefaultValue();

            if (!isNew)
            {
                SetData();
            }
        }

        /// <summary>
        /// 读取原先填写的数据，并为控件赋值
        /// </summary>
        private void SetData()
        {
            DataSet ds = Dal.DataAccess.GetConsultationDataSet(m_ConsultationID, "20");//, Convert.ToString((int)ConsultType.More));
            DataTable dtConsultApply = ds.Tables[0];
            DataTable dtConsultApplyDepartment = ds.Tables[1];
            DataTable dtConsultRecordDepartment = ds.Tables[2];

            if (dtConsultApply.Rows.Count > 0)
            {
                this.memoConsultSuggestion.Text = dtConsultApply.Rows[0]["ConsultSuggestion"].ToString();
                if (dtConsultApply.Rows[0]["FinishTime"].ToString().Trim().Split(' ').Length == 2)
                {
                    dateEditConsultationDate.EditValue = dtConsultApply.Rows[0]["FinishTime"].ToString().Split(' ')[0];
                    timeEditConsultationTime.EditValue = dtConsultApply.Rows[0]["FinishTime"].ToString().Split(' ')[1];
                }
            }

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

            if (dtConsultApplyDepartment.Rows.Count > 0)
            {
                gridControlDepartment.DataSource = dtConsultApplyDepartment;
            }
            if (dtConsultRecordDepartment.Rows.Count > 0)
            {
                gridControlDepartment.DataSource = dtConsultRecordDepartment;
            }
            gridControlApplyDepartment.DataSource = dtConsultApplyDepartment.Copy();
            m_app.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);


        }

        private void BindGridColumnLookUpEditorData()
        {
            repositoryItemLookUpEditEmployee.DataSource = m_DataTableAllEmployee;


        }

        #region LookUpEditor数据绑定

        private void BindData()
        {
            BindHospitalData();
            BindDepartment();
            BindDoctorLevel();
            BindEmployee();
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
        //private void BindEmployeeInGrid()
        //{
        //    lookUpEditorEmployeeInGrid.Kind = WordbookKind.Sql;
        //    lookUpEditorEmployeeInGrid.ListWindow = lookUpWindowEmployeeInGrid;
        //    BindEmployeeInGridWordBook(m_DataTableAllEmployee);
        //}

        //private void BindEmployeeInGridWordBook(DataTable dataTableData)
        //{
        //    for (int i = 0; i < dataTableData.Columns.Count; i++)
        //    {
        //        if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "id")
        //        {
        //            dataTableData.Columns[i].Caption = "医师代码";
        //        }
        //        else if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "name")
        //        {
        //            dataTableData.Columns[i].Caption = "医师名称";
        //        }
        //    }

        //    Dictionary<string, int> colWidths = new Dictionary<string, int>();
        //    colWidths.Add("ID", 60);
        //    colWidths.Add("NAME", 120);
        //    SqlWordbook wordBook = new SqlWordbook("Employee", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
        //    lookUpEditorEmployeeInGrid.SqlWordbook = wordBook;
        //}

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
                Dal.DataAccess.App = m_app;
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
                string wardID = m_app.User.CurrentWardId.ToString();
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, wardID);
            }
            else if (typeID == "5")//申请医师科主任
            {
                string wardID = m_app.User.CurrentWardId.ToString();
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
            gridControlDepartment.DataSource = dt;

            m_app.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
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
                    if (m_app.CustomMessageBox.MessageShow(string.Format("是否删除会诊医生信息？"), CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
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

        #region 新增
        void simpleButtonNew_Click(object sender, EventArgs e)
        {
            Insert();
        }

        private void Insert()
        {
            if (CheckDataDept())
            {
                InsertData();
                m_SelectRow = null;
                SetRepositoryItemLookUpEditDataSource();
            }
        }

        private bool CheckDataDept()
        {
            if (lookUpEditorHospital.CodeValue == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择会诊医院!", CustomMessageBoxKind.WarningOk);
                lookUpEditorHospital.Focus();
                return false;
            }
            else if (lookUpEditorDepartment.CodeValue == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择会诊科室!", CustomMessageBoxKind.WarningOk);
                lookUpEditorDepartment.Focus();
                return false;
            }
            else if (lookUpEditorLevel.CodeValue == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择会诊医师资质!", CustomMessageBoxKind.WarningOk);
                lookUpEditorLevel.Focus();
                return false;
            }
            else if (lookUpEditorEmployee.CodeValue == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择会诊医师!", CustomMessageBoxKind.WarningOk);
                lookUpEditorEmployee.Focus();
                return false;
            }

            DataTable dt = gridControlDepartment.DataSource as DataTable;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["EmployeeCode"].Equals(lookUpEditorEmployee.CodeValue))
                {
                    m_app.CustomMessageBox.MessageShow("该记录已经存在!", CustomMessageBoxKind.WarningOk);
                    gridViewDept.FocusedRowHandle = i;
                    return false;
                }
            }

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
            dr["EmployeeCode"] = lookUpEditorEmployee.CodeValue;
            dr["EmployeeName"] = lookUpEditorEmployee.Text;
            dr["DeleteButton"] = "删除";
            dt.Rows.Add(dr);
        }

        /// <summary>
        /// 部门列表中修改一行
        /// </summary>
        private void EditData()
        {

            m_SelectRow["HospitalName"] = lookUpEditorHospital.Text;
            m_SelectRow["HospitalCode"] = lookUpEditorHospital.CodeValue;
            m_SelectRow["DepartmentName"] = lookUpEditorDepartment.Text;
            m_SelectRow["DepartmentCode"] = lookUpEditorDepartment.CodeValue;
            m_SelectRow["EmployeeLevelName"] = lookUpEditorLevel.Text;
            m_SelectRow["EmployeeLevelID"] = lookUpEditorLevel.CodeValue;
            m_SelectRow["EmployeeCode"] = lookUpEditorEmployee.CodeValue;
            m_SelectRow["EmployeeName"] = lookUpEditorEmployee.Text;
            m_SelectRow["DeleteButton"] = "删除";
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


        private void UCConsultation_Resize(object sender, EventArgs e)
        {
            this.panelControl1.Location = new Point((this.Width - panelControl1.Width) / 2, this.AutoScrollPosition.Y);
        }

        #region LookUpEditor CodeValueChanged事件
        private void lookUpEditorHospital_CodeValueChanged(object sender, EventArgs e)
        {
            BindDepartment();
        }


        private void lookUpEditorDepartment_CodeValueChanged(object sender, EventArgs e)
        {
            lookUpEditorEmployee.CodeValue = "";
            lookUpEditorLevel.CodeValue = "";
            GetDoctor();
        }

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

            DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
            if (dt == null || dt.Rows.Count == 0)
                return;
            if (lookUpEditorLevel.CodeValue == dt.Rows[0][0].ToString())
                return;
            lookUpEditorLevel.CodeValue = dt.Rows[0][0].ToString();
            lookUpEditorEmployee.CodeValue = userid;
        }

        #endregion

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            DrawUnderLine(e.Graphics);
            e.Graphics.DrawString(labHospital.Text, labHospital.Font, Brushes.Black,
                new RectangleF(0, 0, panelControl1.Width - 1, 60), m_SFCenter);
            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, panelControl1.Width - 1, panelControl1.Height - 1));
        }

        private void DrawUnderLine(Graphics g)
        {
            foreach (Control control in this.panelControl1.Controls)
            {
                if (control is TextEdit && !(control is MemoEdit) || control is DateEdit || control is LookUpEditor)
                {
                    g.DrawLine(Pens.Black,
                        new Point(control.Location.X, control.Location.Y + control.Height),
                        new Point(control.Location.X + control.Width, control.Location.Y + control.Height));
                }
            }
        }

        public new void Load(IEmrHost app)
        {
            m_SFCenter = new StringFormat();
            m_SFCenter.Alignment = StringAlignment.Center;
            m_SFCenter.LineAlignment = StringAlignment.Center;
            labHospital.Text = app.CurrentHospitalInfo.Name;
            labHospital.Visible = false;
            DataAccess.App = app;
            m_app = app;

            if (!string.IsNullOrEmpty(CurrentNoofinpat))
            {
                CurrentInpatient = new Common.Eop.Inpatient(Convert.ToDecimal(CurrentNoofinpat));
            }
            else if (m_app.CurrentPatientInfo != null)
            {
                CurrentInpatient = m_app.CurrentPatientInfo;
            }
            else
            {
                return;
            }
            CurrentInpatient.ReInitializeAllProperties();


            GetConsultationEntity();
            if (m_ConsultationID == "")
                return;

            RegisterEvent();
            m_DataTableAllEmployee = GetConsultationData("8");
            PanelControlInit();
            InitInner(false);
            FillUI();
        }

        #region 打印

        public void Print()
        {
            try
            {
                GetPrintEntity();
                GetConsultationEntity();
                PrintForm form = new PrintForm(m_ConsultationEntity);
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //XmlCommomOp.Doc = null;
            //XmlCommomOp.xmlPath = AppDomain.CurrentDomain.BaseDirectory + "consultPrint.xml";
            //XmlCommomOp.CreaetDocument();
            //XmlCommomOp.BindingDate(null, CreateData());
            //List<Metafile> listMetafile = DrawOp.MakeImagesByXmlDocument(XmlCommomOp.Doc);
            //DrawOp.PrintView(listMetafile);
        }

        public void GetPrintEntity()
        {
            DataTable dt = gridControlDepartment.DataSource as DataTable;
            string ConsultDept = "";
            string ConsultUserName = "";
            string HospitalName = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (!ConsultDept.StartsWith(dt.Rows[i]["DepartmentName"].ToString()))
                    ConsultDept += dt.Rows[i]["DepartmentName"].ToString() + ",";
                if (!HospitalName.StartsWith(dt.Rows[i]["HospitalName"].ToString()))
                    HospitalName += dt.Rows[i]["HospitalName"].ToString() + ",";
                ConsultUserName += dt.Rows[i]["EmployeeName"].ToString() + ",";
            }

            ConsultDept = ConsultDept.TrimEnd(',');
            ConsultUserName = ConsultUserName.TrimEnd(',');
            HospitalName = HospitalName.TrimEnd(',');

            m_ConsultationEntity.ConsultDeptName2 = ConsultDept;
            m_ConsultationEntity.ConsultUserName = ConsultUserName;
            m_ConsultationEntity.ConsultHospitalName = HospitalName;

            DataTable dt_apply = gridControlApplyDepartment.DataSource as DataTable;
            string ConsultDept_apply = "";
            for (int i = 0; i < dt_apply.Rows.Count; i++)
            {
                if (!ConsultDept_apply.StartsWith(dt_apply.Rows[i]["DepartmentName"].ToString()))
                    ConsultDept_apply += dt_apply.Rows[i]["DepartmentName"].ToString() + ",";
            }
            ConsultDept_apply = ConsultDept_apply.TrimEnd(',');

            m_ConsultationEntity.ConsultDeptName = ConsultDept_apply;
            //m_ConsultationEntity.ConsultTime=
        }

        ///// <summary>
        ///// 创建参数集合
        ///// Add by xlb 2013-05-23
        ///// </summary>
        ///// <returns></returns>
        //private Dictionary<string, ParamObject> CreateData()
        //{
        //    try
        //    {
        //        Dictionary<string, ParamObject> paramList = new Dictionary<string, ParamObject>();
        //        PropertyInfo[] propertyes = m_ConsultationEntity.GetType().GetProperties();//会诊实体属性集合
        //        //遍历各个属性依次加入参数集合
        //        foreach (PropertyInfo item in propertyes)
        //        {
        //            ParamObject paramObject = new ParamObject(item.Name, "", item.GetValue(m_ConsultationEntity, null) == null ? "" : item.GetValue(m_ConsultationEntity, null).ToString());
        //            if (!paramList.ContainsKey(item.Name))
        //            {
        //                paramList.Add(item.Name, paramObject);
        //            }
        //        }
        //        return paramList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

        public void Save()
        {
            if (m_ConsultationID == "")
                return;

            if (btnOver.Enabled != true)
            {
                m_app.CustomMessageBox.MessageShow("不能修改，会诊已完成！");
                return;
            }

            if (m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.WaitConsultation) || m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.RecordeSave))
                Save(ConsultStatus.RecordeSave);
        }

        #region 保存
        public void Save(ConsultStatus status)
        {
            if (CheckData())
            {
                try
                {
                    m_app.SqlHelper.BeginTransaction();
                    SaveConsultationApply(SaveType.RecordModify, m_ConsultationID, status);
                    SaveConsultationRecordDept(m_ConsultationID);
                    m_app.SqlHelper.CommitTransaction();

                    m_app.CustomMessageBox.MessageShow("保存成功!", CustomMessageBoxKind.InformationOk);

                    GetConsultationEntity();
                    InitInner(false);
                    FillUI();

                }
                catch (Exception ex)
                {
                    m_app.SqlHelper.RollbackTransaction();
                    m_app.CustomMessageBox.MessageShow("保存失败!" + ex.Message, CustomMessageBoxKind.InformationOk);
                }
            }
        }

        private string SaveConsultationApply(SaveType saveType, string consultApplySN, ConsultStatus status)
        {

            string typeID = Convert.ToString((int)saveType);
            string consultSuggestion = this.memoConsultSuggestion.Text.Trim();
            string finishTime = dateEditConsultationDate.Text + " " + timeEditConsultationTime.Text;

            return Dal.DataAccess.InsertConsultationApply(typeID, consultApplySN, m_ConsultationEntity.NoOfInpat, consultSuggestion, finishTime, Convert.ToString((int)status));
        }

        private void SaveConsultationRecordDept(string consultApplySn)
        {
            DataTable dt = gridControlDepartment.DataSource as DataTable;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string orderValue = Convert.ToString(i + 1);
                string hospitalCode = dt.Rows[i]["HospitalCode"].ToString();
                string departmentCode = dt.Rows[i]["DepartmentCode"].ToString();
                string departmentName = "";
                string employeeCode = dt.Rows[i]["EmployeeCode"].ToString();
                string employeeName = "";
                string employeeLevelID = dt.Rows[i]["EmployeeLevelID"].ToString();
                string createUser = m_app.User.Id;
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
            if (memoConsultSuggestion.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请填写会诊医师意见!", CustomMessageBoxKind.WarningOk);
                memoConsultSuggestion.Text = "";
                memoConsultSuggestion.Focus();
                return false;
            }

            DataTable dt = gridControlDepartment.DataSource as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    m_app.CustomMessageBox.MessageShow("请新增受邀医师科室级别!", CustomMessageBoxKind.WarningOk);
                    simpleButtonNew.Focus();
                    return false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string employeeCode = dt.Rows[i]["EmployeeCode"].ToString();
                    if (employeeCode == "")
                    {
                        m_app.CustomMessageBox.MessageShow("请选择会诊医师!", CustomMessageBoxKind.WarningOk);
                        gridViewDept.Focus();
                        return false;
                    }
                }
            }

            if (dateEditConsultationDate.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择会诊日期!", CustomMessageBoxKind.WarningOk);
                dateEditConsultationDate.Focus();
                return false;
            }

            if (timeEditConsultationTime.Text.Trim() == "0:00:00")
            {
                m_app.CustomMessageBox.MessageShow("请选择会诊时间!", CustomMessageBoxKind.WarningOk);
                timeEditConsultationTime.Focus();
                return false;
            }



            return true;
        }
        #endregion

        private void UCConsultationMultiply_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (m_ConsultationID == "")
                return;
            if (m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.WaitConsultation) || m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.RecordeSave))
                Save(ConsultStatus.RecordeSave);
        }
        /// <summary>
        /// 会诊结束操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOver_Click(object sender, EventArgs e)
        {
            if (m_ConsultationID == "")
                return;
            //会诊完成后，可以再次修改
            if (m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.WaitConsultation) || m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.RecordeSave) || m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.RecordeComplete))
                Save(ConsultStatus.RecordeComplete);
        }

        /// <summary>
        /// 判断会诊保存与会诊结束按钮状态(多科会诊整个流程由申请科室完成)
        /// </summary>
        /// <param name="applyDeptID">申请科室</param>
        private void InitBtnState(string applyDeptID)
        {
            if (ReadOnlyControl)
            {
                InitReadOnlyControl();
                return;
            }
            if (m_app.User.CurrentDeptId == applyDeptID)
            {

                this.btnSave.Enabled = true;
                this.btnOver.Enabled = true;

            }

        }

        private void panelControl1_Click(object sender, EventArgs e)
        {
            int verticalScrollValue = this.VerticalScroll.Value;
            //Point pt = this.AutoScrollPosition;
            this.btn1.Focus();
            this.VerticalScroll.Value = verticalScrollValue;

        }

        private void UCConsultationMultiply_Click(object sender, EventArgs e)
        {
            int verticalScrollValue = this.VerticalScroll.Value;
            //Point pt = this.AutoScrollPosition;
            this.btn1.Focus();
            this.VerticalScroll.Value = verticalScrollValue;
        }

        public string CurrentNoofinpat
        {
            get;
            set;
        }

        public Control DesignUI
        {
            get { return this; }
        }

        public string Title
        {
            get { return "会诊记录单"; }
        }

        public bool ReadOnlyControl
        {
            get { return m_ReadOnlyControl; }
            set { m_ReadOnlyControl = value; }
        }
        private bool m_ReadOnlyControl = false;

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

                lookUpWindowEmployee.SqlHelper = m_app.SqlHelper;

                DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "医生代码";
                Dept.Columns["NAME"].Caption = "医生名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 65);
                cols.Add("NAME", 160);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//py//wb");
                lookUpEditorEmployee.SqlWordbook = deptWordBook;

            }
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            e.Appearance.BackColor = Color.White;
        }

        private void simpleButtonEdit_Click(object sender, EventArgs e)
        {
            if (CheckDataDept())
            {
                if (m_SelectRow != null && !m_SelectRow.IsNull("HospitalCode"))
                    EditData();
                else
                    m_app.CustomMessageBox.MessageShow("请选中一条记录！");
            }
        }

        /// <summary>
        /// 外部调用  将页面中空间置为不可读写 
        /// </summary>
        public void InitReadOnlyControl()
        {

            memoConsultSuggestion.Properties.ReadOnly = m_ReadOnlyControl;
            lookUpEditorHospital.ReadOnly = m_ReadOnlyControl;
            lookUpEditorDepartment.ReadOnly = m_ReadOnlyControl;
            lookUpEditorEmployee.ReadOnly = m_ReadOnlyControl;
            lookUpEditorLevel.ReadOnly = m_ReadOnlyControl;

            DeleteButton.Visible = !m_ReadOnlyControl;
            simpleButtonNew.Visible = !m_ReadOnlyControl;
            simpleButtonEdit.Visible = !m_ReadOnlyControl;

            this.btnSave.Enabled = !m_ReadOnlyControl;
            this.btnOver.Enabled = !m_ReadOnlyControl;

            dateEditConsultationDate.Enabled = !m_ReadOnlyControl;
            timeEditConsultationTime.Enabled = !m_ReadOnlyControl;


        }

        #region 文本框输入颜色变化控制 by cyq 2012-10-22
        private void Dev_Enter(object sender, EventArgs e)
        {
            DS_Common.setBackColor(sender, true);
        }
        private void Dev_Leave(object sender, EventArgs e)
        {
            DS_Common.setBackColor(sender, false);
        }
        #endregion

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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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
