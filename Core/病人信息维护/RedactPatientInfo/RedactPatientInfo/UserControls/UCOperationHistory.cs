using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Wordbook;
using DrectSoft.Core.RedactPatientInfo.PublicSet;
using System.Data.SqlClient;

namespace DrectSoft.Core.RedactPatientInfo.UserControls
{
    public partial class UCOperationHistory : DevExpress.XtraEditors.XtraUserControl
    {
        string m_NoOfInpat;
        DataTable m_Table;

        public UCOperationHistory(string NoOfInpat)
        {
            InitializeComponent();
            m_NoOfInpat = NoOfInpat;

            
        }

        #region 初始化窗体
        private void InitForm()
        {
            //初始化手术名称
            GetSurgery();
            //初始化疾病名称
            //GetDiacrisis();

            //获取手术史记录
            GetOperationHistory();

            //禁/启用部分控件
            EditControlEnabled(false);
            btnCancel.Enabled = false;

        }

        #region 初始化手术名称
        //初始化手术名称
        private void GetSurgery()
        {
            lookUpWindowSurgeryID.SqlHelper = SqlUtil.App.SqlHelper;

            DataTable Surgery = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_RedactPatientInfoFrm",
                 new SqlParameter[] { new SqlParameter("@FrmType", "25") }, CommandType.StoredProcedure);

            Surgery.Columns["ID"].Caption = "手术代码";
            Surgery.Columns["Name"].Caption = "手术名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 95);
            cols.Add("NAME", 150);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Surgery, "ID", "NAME", cols);

            //MethodSet.App.PublicMethod.ConvertSqlWordbookDataSourceUpper(deptWordBook);

            lookUpEditorSurgeryID.SqlWordbook = deptWordBook;

        }
        #endregion

        #region 初始化疾病名称
        //初始化疾病名称
        private void GetDiacrisis()
        {
            lookUpWindowDiagnosisID.SqlHelper = SqlUtil.App.SqlHelper;

            DataTable Diacrisis = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_RedactPatientInfoFrm",
                 new SqlParameter[] { new SqlParameter("@FrmType", "16") }, CommandType.StoredProcedure);

            Diacrisis.Columns["ICD"].Caption = "诊断代码";
            Diacrisis.Columns["NAME"].Caption = "疾病名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ICD", 89);
            cols.Add("NAME", 120);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Diacrisis, "ICD", "NAME", cols);

            //MethodSet.App.PublicMethod.ConvertSqlWordbookDataSourceUpper(deptWordBook);

            lookUpEditorDiagnosisID.SqlWordbook = deptWordBook;

        }
        #endregion

        #region 获取手术史记录
        //获取手术史记录
        private void GetOperationHistory()
        {

            m_Table = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_RedactPatientInfoFrm",
                 new SqlParameter[] { new SqlParameter("@FrmType", "26"), new SqlParameter("@NoOfInpat", m_NoOfInpat) }, CommandType.StoredProcedure);
            gridViewSurgeryHistory.SelectAll();
            gridViewSurgeryHistory.DeleteSelectedRows();
            gridControlSurgeryHistory.DataSource = m_Table;

            SqlUtil.App.PublicMethod.ConvertGridDataSourceUpper(gridViewSurgeryHistory);

        }
        #endregion

        private void UCOperationHistory_Load(object sender, EventArgs e)
        {
            InitForm();
        }

        #endregion

        #region 编辑控件启用/禁止
        /// <summary>
        /// 编辑控件启用/禁止
        /// </summary>
        /// <param name="bolEnabled"></param>
        private void EditControlEnabled(bool bolEnabled)
        {
            txtDoctor.Enabled = bolEnabled;
            memoEditDiscuss.Enabled = bolEnabled;
            lookUpEditorDiagnosisID.Enabled = bolEnabled;
            lookUpEditorSurgeryID.Enabled = bolEnabled;

            gridControlSurgeryHistory.Enabled = !bolEnabled;

        }
        #endregion

        #region 保存添加、修改、删除操作
        public void SaveUCOperationHistoryInfo(bool isSave)
        {
            if (lookUpEditorDiagnosisID.CodeValue == null || lookUpEditorSurgeryID.CodeValue == null)
            {
                SqlUtil.App.CustomMessageBox.MessageShow("手术名称和病种名称必须填写!");
                return;
            }

            string strTemp = "操作";
            try
            {

                //添加
                if (btnAdd.Enabled && btnCancel.Enabled && isSave)
                {
                    if (SqlUtil.App.CustomMessageBox.MessageShow("确定添加当前记录吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        strTemp = "添加";
                        SaveOperationHistory();
                        InitForm();
                    }

                }
                //修改
                else if (btnModify.Enabled && btnCancel.Enabled && isSave)
                {
                    if (SqlUtil.App.CustomMessageBox.MessageShow("确定修改当前记录吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        strTemp = "修改";
                        ModifyOperationHistory();
                        InitForm();
                    }
                }
                //删除
                else if (btnDel.Enabled && !btnCancel.Enabled && !isSave)
                {
                    int fouceRowIndex = gridViewSurgeryHistory.FocusedRowHandle;
                    if (fouceRowIndex < 0)
                    {
                        SqlUtil.App.CustomMessageBox.MessageShow("请选择需要删除的记录! ");
                        return;
                    }
                    DataRow foucesRow = gridViewSurgeryHistory.GetDataRow(fouceRowIndex);

                    if (SqlUtil.App.CustomMessageBox.MessageShow("确定删除当前记录吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        strTemp = "删除";
                        SqlParameter[] sqlParam = new SqlParameter[] 
                        { 
                            new SqlParameter("@EditType", SqlDbType.VarChar),//编辑信息类型：1：添加、2：修改、3：删除
                            new SqlParameter("@ID", SqlDbType.VarChar)
                        };

                        sqlParam[0].Value = "3";
                        sqlParam[1].Value = foucesRow["ID"].ToString();

                        SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditSurgeryHistoryInfo", sqlParam, CommandType.StoredProcedure);
                        SqlUtil.App.CustomMessageBox.MessageShow("删除成功!");
                        InitForm();

                    }

                }

            }
            catch (Exception ex)
            {
                SqlUtil.App.CustomMessageBox.MessageShow(strTemp + "失败!\n详细错误：" + ex.Message);
            }
            finally
            {
                //部分控件启用/禁止
                btnCancel_Click(this, null);
            }
        }

        #region 保存手术史信息
        /// <summary>
        /// 保存手术史信息
        /// </summary>
        private void SaveOperationHistory()
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                    { 
                        new SqlParameter("@EditType", SqlDbType.VarChar),//编辑信息类型：1：添加、2：修改、3：删除
                        new SqlParameter("@ID", SqlDbType.VarChar),
                        new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                        new SqlParameter("@SurgeryID", SqlDbType.VarChar),
                        new SqlParameter("@DiagnosisID", SqlDbType.VarChar),
                        new SqlParameter("@Discuss", SqlDbType.VarChar),
                        new SqlParameter("@Doctor", SqlDbType.VarChar)
                    };

            sqlParam[0].Value = "1";
            sqlParam[1].Value = "-1";
            sqlParam[2].Value = string.IsNullOrEmpty(m_NoOfInpat) ? "0" : m_NoOfInpat;
            sqlParam[3].Value = lookUpWindowSurgeryID.CodeValue.ToString().Trim();
            sqlParam[4].Value = lookUpWindowDiagnosisID.CodeValue.ToString().Trim();
            sqlParam[5].Value = memoEditDiscuss.Text.ToString().Trim();
            sqlParam[6].Value = txtDoctor.Text.ToString().Trim();

            SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditSurgeryHistoryInfo", sqlParam, CommandType.StoredProcedure);
            SqlUtil.App.CustomMessageBox.MessageShow("添加成功!");

        }
        #endregion

        #region 修改手术史信息
        /// <summary>
        /// 修改手术史信息
        /// </summary>
        /// <param name="foucesRow">一条记录</param>
        private void ModifyOperationHistory()
        {

            int fouceRowIndex = gridViewSurgeryHistory.FocusedRowHandle;
            if (fouceRowIndex < 0) return;
            DataRow foucesRow = gridViewSurgeryHistory.GetDataRow(fouceRowIndex);

            SqlParameter[] sqlParam = new SqlParameter[] 
                    { 
                         new SqlParameter("@EditType", SqlDbType.VarChar),//编辑信息类型：1：添加、2：修改、3：删除
                        new SqlParameter("@ID", SqlDbType.VarChar),
                        new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                        new SqlParameter("@SurgeryID", SqlDbType.VarChar),
                        new SqlParameter("@DiagnosisID", SqlDbType.VarChar),
                        new SqlParameter("@Discuss", SqlDbType.VarChar),
                        new SqlParameter("@Doctor", SqlDbType.VarChar)
                    };

            sqlParam[0].Value = "2";
            sqlParam[1].Value = foucesRow["ID"].ToString();
            sqlParam[2].Value = string.IsNullOrEmpty(m_NoOfInpat) ? "0" : m_NoOfInpat;
            sqlParam[3].Value = lookUpWindowSurgeryID.CodeValue.ToString().Trim();
            sqlParam[4].Value = lookUpWindowDiagnosisID.CodeValue.ToString().Trim();
            sqlParam[5].Value = memoEditDiscuss.Text.ToString().Trim();
            sqlParam[6].Value = txtDoctor.Text.ToString().Trim();

            SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditSurgeryHistoryInfo", sqlParam, CommandType.StoredProcedure);
            SqlUtil.App.CustomMessageBox.MessageShow("修改成功!");

        }

        #endregion
        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnModify.Enabled = false;
            btnDel.Enabled = false;
            btnCancel.Enabled = true;
            EditControlEnabled(true);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            int fouceRowIndex = gridViewSurgeryHistory.FocusedRowHandle;
            if (fouceRowIndex < 0)
            {
                SqlUtil.App.CustomMessageBox.MessageShow("请选择需要修改的记录! ");
                return;
            }

            btnAdd.Enabled = false;
            btnDel.Enabled = false;
            btnCancel.Enabled = true;
            EditControlEnabled(true);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            SaveUCOperationHistoryInfo(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnModify.Enabled = true;
            btnDel.Enabled = true;
            btnAdd.Enabled = true;
            btnCancel.Enabled = false;
            EditControlEnabled(false);
        }

        private void gridViewSurgeryHistory_Click(object sender, EventArgs e)
        {
            int fouceRowIndex = gridViewSurgeryHistory.FocusedRowHandle;
            if (fouceRowIndex < 0) return;
            DataRow foucesRow = gridViewSurgeryHistory.GetDataRow(fouceRowIndex);

            string ID = foucesRow["ID"].ToString().Trim();

            foreach (DataRow dr in m_Table.Rows)
            {
                if (dr["ID"].ToString().Equals(ID))
                {
                    txtDoctor.Text = dr["Doctor"].ToString();
                    memoEditDiscuss.Text = dr["Discuss"].ToString();
                    lookUpEditorDiagnosisID.CodeValue = dr["DiagnosisID"].ToString().Trim();
                    lookUpEditorSurgeryID.CodeValue = dr["SurgeryID"].ToString().Trim();
                    
                    break;
                }
            }
        }

    }
}
