using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Wordbook;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.MainEmrPad.ThreeLevelCheck
{
    public partial class UserSettingForm : DevBaseForm, IStartPlugIn
    {
        #region SQL
        const string c_GetResident = @"select distinct ID, NAME, PY, WB from users where deptid = '{0}' and valid = '1' and users.grade = '2003'";
        const string c_GetAttend = @"select distinct ID, NAME, PY, WB from users where deptid = '{0}' and valid = '1' and users.grade = '2002'";
        const string c_GetChief = @"select distinct ID, NAME, PY, WB from users where deptid = '{0}' and valid = '1' and (users.grade = '2001' or users.grade = '2000')";
        const string c_GetThreeLevelCheck = @"select THREE_LEVEL_CHECK.resident_id, THREE_LEVEL_CHECK.resident_name,
                                                     THREE_LEVEL_CHECK.attend_id, THREE_LEVEL_CHECK.attend_name,
                                                     THREE_LEVEL_CHECK.chief_id, THREE_LEVEL_CHECK.chief_name,
                                                     THREE_LEVEL_CHECK.dept_id, THREE_LEVEL_CHECK.dept_name
                                                from THREE_LEVEL_CHECK
                                               where THREE_LEVEL_CHECK.dept_id like '%{0}%' 
                                                     and resident_id like '%{1}%' 
                                                     and attend_id like '%{2}%' 
                                                     and chief_id like '%{3}%'
                                                     and valid = '1' ";
        #endregion

        private IEmrHost m_app;
        private bool m_DepartmentEnableFlag = true;

        public UserSettingForm()
        {
            InitializeComponent();
        }

        public UserSettingForm(IEmrHost host) : this()
        {
            m_app = host;
        }

        private void UserSettingForm_Load(object sender, EventArgs e)
        {
            RegisterEvent();
            InitDeptList();
            SetLookUpEditorNonEnable();
            Search();
        }

        private void RegisterEvent()
        {
            lookUpEditorDepartment.CodeValueChanged += new EventHandler(lookUpEditorDepartment_CodeValueChanged);
            lookUpEditorDepartmentSearch.CodeValueChanged += new EventHandler(lookUpEditorDepartmentSearch_CodeValueChanged);
        }

        private void InitDeptList()
        {
            lookUpWindowDepartment.SqlHelper = m_app.SqlHelper;

            string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid ");
            DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);

            Dept.Columns["ID"].Caption = "科室编码";
            Dept.Columns["NAME"].Caption = "科室名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 60);
            cols.Add("NAME", 90);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorDepartment.SqlWordbook = deptWordBook;
            lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;

            SqlWordbook deptWordBookSearch = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorDepartmentSearch.SqlWordbook = deptWordBookSearch;
            lookUpEditorDepartmentSearch.CodeValue = m_app.User.CurrentDeptId;
        }

        #region change department
        void lookUpEditorDepartment_CodeValueChanged(object sender, EventArgs e)
        {
            InitResidentList(lookUpEditorDepartment.CodeValue);
            InitAttendList(lookUpEditorDepartment.CodeValue);
            InitChiefList(lookUpEditorDepartment.CodeValue);
        }

        void lookUpEditorDepartmentSearch_CodeValueChanged(object sender, EventArgs e)
        {
            InitResidentListSearch(lookUpEditorDepartmentSearch.CodeValue);
            InitAttendListSearch(lookUpEditorDepartmentSearch.CodeValue);
            InitChiefListSearch(lookUpEditorDepartmentSearch.CodeValue);
        }
        #endregion

        #region OK -- lookupeditor
        private void InitResidentList(string deptNO)
        {
            lookUpWindowResident.SqlHelper = m_app.SqlHelper;
            DataTable Dept = m_app.SqlHelper.ExecuteDataTable(string.Format(c_GetResident, deptNO));

            Dept.Columns["ID"].Caption = "医师工号";
            Dept.Columns["NAME"].Caption = "医师名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 60);
            cols.Add("NAME", 60);

            SqlWordbook wordBookResident = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorResident.CodeValue = "";
            lookUpEditorResident.Text = "";
            lookUpEditorResident.SqlWordbook = wordBookResident;
        }

        private void InitAttendList(string deptNO)
        {
            lookUpWindowAttend.SqlHelper = m_app.SqlHelper;
            DataTable Dept = m_app.SqlHelper.ExecuteDataTable(string.Format(c_GetAttend, deptNO));

            Dept.Columns["ID"].Caption = "医师工号";
            Dept.Columns["NAME"].Caption = "医师名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 60);
            cols.Add("NAME", 60);

            SqlWordbook wordBookAttend = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorAttend.CodeValue = "";
            lookUpEditorAttend.Text = "";
            lookUpEditorAttend.SqlWordbook = wordBookAttend;
        }

        private void InitChiefList(string deptNO)
        {
            lookUpWindowChief.SqlHelper = m_app.SqlHelper;
            DataTable Dept = m_app.SqlHelper.ExecuteDataTable(string.Format(c_GetChief, deptNO));

            Dept.Columns["ID"].Caption = "医师工号";
            Dept.Columns["NAME"].Caption = "医师名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 60);
            cols.Add("NAME", 60);

            SqlWordbook wordBookChief = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorChief.CodeValue = "";
            lookUpEditorChief.Text = "";
            lookUpEditorChief.SqlWordbook = wordBookChief;
        }
        #endregion

        #region Search -- lookupeditor
        private void InitResidentListSearch(string deptNO)
        {
            lookUpWindowResident.SqlHelper = m_app.SqlHelper;
            DataTable Dept = m_app.SqlHelper.ExecuteDataTable(string.Format(c_GetResident, deptNO));

            Dept.Columns["ID"].Caption = "医师工号";
            Dept.Columns["NAME"].Caption = "医师名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 60);
            cols.Add("NAME", 60);

            SqlWordbook wordBookResidentSearch = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorResidentSearch.CodeValue = "";
            lookUpEditorResidentSearch.Text = "";
            lookUpEditorResidentSearch.SqlWordbook = wordBookResidentSearch;
        }

        private void InitAttendListSearch(string deptNO)
        {
            lookUpWindowAttend.SqlHelper = m_app.SqlHelper;
            DataTable Dept = m_app.SqlHelper.ExecuteDataTable(string.Format(c_GetAttend, deptNO));

            Dept.Columns["ID"].Caption = "医师工号";
            Dept.Columns["NAME"].Caption = "医师名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 60);
            cols.Add("NAME", 60);

            SqlWordbook wordBookAttendSearch = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorAttendSearch.CodeValue = "";
            lookUpEditorAttendSearch.Text = "";
            lookUpEditorAttendSearch.SqlWordbook = wordBookAttendSearch;
        }

        private void InitChiefListSearch(string deptNO)
        {
            lookUpWindowChief.SqlHelper = m_app.SqlHelper;
            DataTable Dept = m_app.SqlHelper.ExecuteDataTable(string.Format(c_GetChief, deptNO));

            Dept.Columns["ID"].Caption = "医师工号";
            Dept.Columns["NAME"].Caption = "医师名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 60);
            cols.Add("NAME", 60);

            SqlWordbook wordBookChiefSearch = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorChiefSearch.CodeValue = "";
            lookUpEditorChiefSearch.Text = "";
            lookUpEditorChiefSearch.SqlWordbook = wordBookChiefSearch;
        }

        #endregion

        #region IStartPlugIn 成员

        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {
            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;

            return plg;
        }

        #endregion

        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(c_GetThreeLevelCheck, 
                lookUpEditorDepartmentSearch.CodeValue,
                lookUpEditorResidentSearch.CodeValue,
                lookUpEditorAttendSearch.CodeValue,
                lookUpEditorChiefSearch.CodeValue));
            gridControlUserList.DataSource = dt;
        }

        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                Save();
                SycUpdateGridControl();
                m_app.CustomMessageBox.MessageShow("保存成功", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
            }
        }

        private bool Check()
        {
            if (lookUpEditorResident.CodeValue.Trim() == "" || lookUpEditorResident.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择住院医师", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                lookUpEditorResident.Focus();
                return false;
            }
            else if (lookUpEditorAttend.CodeValue.Trim() == "" || lookUpEditorAttend.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择主治医师", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                lookUpEditorAttend.Focus();
                return false;
            }
            else if (lookUpEditorChief.CodeValue.Trim() == "" || lookUpEditorChief.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择主任医师", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                lookUpEditorChief.Focus();
                return false;
            }
            return true;
        }

        private void Save()
        {
            SqlParameter[] para = new SqlParameter[9];
            para[0] = new SqlParameter("@residentid", SqlDbType.VarChar);
            para[0].Value = lookUpEditorResident.CodeValue;
            para[1] = new SqlParameter("@residentname", SqlDbType.VarChar);
            para[1].Value = lookUpEditorResident.Text;
            para[2] = new SqlParameter("@attendid", SqlDbType.VarChar);
            para[2].Value = lookUpEditorAttend.CodeValue;
            para[3] = new SqlParameter("@attentname", SqlDbType.VarChar);
            para[3].Value = lookUpEditorAttend.Text;
            para[4] = new SqlParameter("@chiefid", SqlDbType.VarChar);
            para[4].Value = lookUpEditorChief.CodeValue;
            para[5] = new SqlParameter("@chiefname", SqlDbType.VarChar);
            para[5].Value = lookUpEditorChief.Text;
            para[6] = new SqlParameter("@deptid", SqlDbType.VarChar);
            para[6].Value = lookUpEditorDepartment.CodeValue;
            para[7] = new SqlParameter("@deptname", SqlDbType.VarChar);
            para[7].Value = lookUpEditorDepartment.Text;
            para[8] = new SqlParameter("@createuser", SqlDbType.VarChar);
            para[8].Value = m_app.User.Id;
            m_app.SqlHelper.ExecuteNoneQuery("emr_record_input.usp_insertthreelevelcheck", para, CommandType.StoredProcedure);
        }

        private void SycUpdateGridControl()
        {
            DataTable dt = gridControlUserList.DataSource as DataTable;
            if (dt != null)
            {
                bool isInGrid = false;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["resident_id"].ToString() == lookUpEditorResident.CodeValue)
                    {
                        isInGrid = true;
                        dr["attend_id"] = lookUpEditorAttend.CodeValue;
                        dr["attend_name"] = lookUpEditorAttend.Text;
                        dr["chief_id"] = lookUpEditorChief.CodeValue;
                        dr["chief_name"] = lookUpEditorChief.Text;
                        dr["dept_id"] = lookUpEditorDepartment.CodeValue;
                        dr["dept_name"] = lookUpEditorDepartment.Text;
                        break;
                    }
                }
                if (!isInGrid)
                {
                    gridViewUserList.BeginUpdate();
                    DataRow dr = dt.NewRow();
                    dr["resident_id"] = lookUpEditorResident.CodeValue;
                    dr["resident_name"] = lookUpEditorResident.Text;
                    dr["attend_id"] = lookUpEditorAttend.CodeValue;
                    dr["attend_name"] = lookUpEditorAttend.Text;
                    dr["chief_id"] = lookUpEditorChief.CodeValue;
                    dr["chief_name"] = lookUpEditorChief.Text;
                    dr["dept_id"] = lookUpEditorDepartment.CodeValue;
                    dr["dept_name"] = lookUpEditorDepartment.Text;
                    dt.Rows.Add(dr);
                    gridViewUserList.EndUpdate();
                }
            }
        }

        /// <summary>
        /// 设置LookUpEditorDepartment不可用
        /// </summary>
        public void SetDepartmentNonEnableFlag()
        {
            m_DepartmentEnableFlag = false;
        }
        private void SetLookUpEditorNonEnable()
        {
            if (!m_DepartmentEnableFlag)
            {
                lookUpEditorDepartment.Enabled = false;
                lookUpEditorDepartmentSearch.Enabled = false;
            }
        }

        private void GridViewMouseDown()
        {
            GridHitInfo hitinfo = gridViewUserList.CalcHitInfo(gridControlUserList.PointToClient(Cursor.Position));
            if (hitinfo.RowHandle >= 0)
            {
                DataRow dr = gridViewUserList.GetDataRow(gridViewUserList.FocusedRowHandle);
                if (dr != null)
                {
                    lookUpEditorDepartment.CodeValue = dr["dept_id"].ToString();
                    lookUpEditorResident.CodeValue = dr["resident_id"].ToString();
                    lookUpEditorAttend.CodeValue = dr["attend_id"].ToString();
                    lookUpEditorChief.CodeValue = dr["chief_id"].ToString();
                }
            }
        }

        private void gridControlUserList_MouseClick(object sender, MouseEventArgs e)
        {
            GridViewMouseDown();
        }
    }
}