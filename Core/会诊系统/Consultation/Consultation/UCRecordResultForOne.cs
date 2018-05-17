using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.Wordbook;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.Consultation
{
    public partial class UCRecordResultForOne : DevExpress.XtraEditors.XtraUserControl
    {
        private IYidanEmrHost m_App;
        private string m_NoOfFirstPage = string.Empty;
        private string m_ConsultApplySN = string.Empty;
        private bool m_ReadOnly = false;

        public UCRecordResultForOne()
        {
            InitializeComponent();
        }


        private void RegisterEvent()
        {
            lookUpEditorHospital.CodeValueChanged += new EventHandler(lookUpEditorHospital_CodeValueChanged);
            lookUpEditorDepartment.CodeValueChanged += new EventHandler(lookUpEditorDepartment_CodeValueChanged);
        }

        void lookUpEditorDepartment_CodeValueChanged(object sender, EventArgs e)
        {
            //医师受科室的影响
            BindEmployee();
            lookUpEditorEmployee.CodeValue = "";
            lookUpEditorEmployee.Text = "";
        }

        void lookUpEditorHospital_CodeValueChanged(object sender, EventArgs e)
        {
            //科室受医院的影响
            BindDepartment();
            lookUpEditorDepartment.CodeValue = "";
            lookUpEditorDepartment.Text = "";
        }

        /// <summary>
        /// 由外部调用的方法，对此控件进行初始化
        /// </summary>
        /// <param name="noOfFirstPage">首页序号</param>
        /// <param name="app"></param>
        /// <param name="isNew">是否新开立会诊单</param>
        public void Init(string noOfFirstPage, IYidanEmrHost app, bool isNew, bool readOnly, string consultApplySn)
        {
            m_App = app;
            m_NoOfFirstPage = noOfFirstPage;
            Dal.DataAccess.App = app;
            m_ReadOnly = readOnly;
            m_ConsultApplySN = consultApplySn;

            RegisterEvent();
            InitInner(isNew);
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
        }

        private void BindData()
        {
            BindHospitalData();
            BindDepartment();
            BindEmployee();
        }

        private void ControlVisible()
        {
            if (m_ReadOnly)
            {
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

        #region 绑定会诊医院
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

        #region 绑定会诊科室
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

            return dataTableConsultationData;
        }

        /// <summary>
        /// 控件设置默认值
        /// </summary>
        private void SetData()
        {
            DataSet ds = Dal.DataAccess.GetConsultationDataSet(m_ConsultApplySN, "20", Convert.ToString((int)ConsultType.One));
            DataTable dtConsultApply = ds.Tables[0];
            DataTable dtConsultApplyDepartment = ds.Tables[1];
            DataTable dtConsultRecordDepartment = ds.Tables[2];

            string consultTime = dtConsultApply.Rows[0]["ConsultTime"].ToString();
            string finishTime = dtConsultApply.Rows[0]["finishtime"].ToString();

            string timeTemp = consultTime;
            DataTable dtTemp = dtConsultApplyDepartment;
            if (dtConsultRecordDepartment.Rows.Count > 0)
            {
                dtTemp = dtConsultRecordDepartment;
                timeTemp = finishTime;
            }
            string hospitalCode = dtTemp.Rows[0]["HospitalCode"].ToString();
            string departmentCode = dtTemp.Rows[0]["DepartmentCode"].ToString();
            string employeeCode = dtTemp.Rows[0]["EmployeeCode"].ToString();
            lookUpEditorHospital.CodeValue = hospitalCode;
            lookUpEditorDepartment.CodeValue = departmentCode;
            lookUpEditorEmployee.CodeValue = employeeCode;

            if (timeTemp.Split(' ').Length >= 2)
            {
                dateEditConsultationDate.EditValue = timeTemp.Split(' ')[0];
                timeEditConsultationTime.EditValue = timeTemp.Split(' ')[1];
            }

            memoEditSuggestion.Text = dtConsultApply.Rows[0]["ConsultSuggestion"].ToString();
        }

        public void Save(ConsultStatus status)
        {
            if (CheckData())
            {
                try
                {
                    m_App.SqlHelper.BeginTransaction();
                    SaveConsultationApply(SaveType.RecordModify, m_ConsultApplySN, status);
                    SaveConsultationRecordDept(m_ConsultApplySN);
                    m_App.SqlHelper.CommitTransaction();

                    m_App.CustomMessageBox.MessageShow("保存成功!", CustomMessageBoxKind.InformationOk);
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
            string orderValue = "1";
            string hospitalCode = lookUpEditorHospital.CodeValue;
            string departmentCode = lookUpEditorDepartment.CodeValue;
            string departmentName = string.Empty;
            string employeeCode = lookUpEditorEmployee.CodeValue;
            string employeeName = string.Empty;
            string employeeLevelID = string.Empty;
            string createUser = m_App.User.Id;
            string createTime = System.DateTime.Now.ToString();

            Dal.DataAccess.InsertConsultationRecordDept(consultApplySn, orderValue, hospitalCode, departmentCode, departmentName,
               employeeCode, employeeName, employeeLevelID, createUser, createTime);
        }

        /// <summary>
        /// 检查控件的输入情况
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            if (memoEditSuggestion.Text.Trim() == "")
            {
                m_App.CustomMessageBox.MessageShow("请输入会诊医师意见!", CustomMessageBoxKind.WarningOk);
                memoEditSuggestion.Text = "";
                memoEditSuggestion.Focus();
                return false;
            }

            if (lookUpEditorHospital.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择会诊医院!", CustomMessageBoxKind.WarningOk);
                lookUpEditorHospital.Text = "";
                lookUpEditorHospital.Focus();
                return false;
            }

            if (lookUpEditorDepartment.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择会诊科室!", CustomMessageBoxKind.WarningOk);
                lookUpEditorDepartment.Text = "";
                lookUpEditorDepartment.Focus();
                return false;
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

            if (lookUpEditorEmployee.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择会诊医师!", CustomMessageBoxKind.WarningOk);
                lookUpEditorEmployee.Text = "";
                lookUpEditorEmployee.Focus();
                return false;
            }

            return true;
        }

        public void ReadOnlyControl()
        {
 
        }
    }
}
