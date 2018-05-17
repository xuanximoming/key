using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DrectSoft.Core.RedactPatientInfo.PublicSet;
using DrectSoft.Wordbook;

namespace DrectSoft.Core.RedactPatientInfo.UserControls
{
    public partial class UCDiseaseHistory : DevExpress.XtraEditors.XtraUserControl
    {
        string m_NoOfInpat;
        DataTable m_Table;

        public UCDiseaseHistory(string NoOfInpat)
        {
            InitializeComponent();
            m_NoOfInpat = NoOfInpat;

            
        }

        #region 初始化窗体
        private void InitForm()
        {
           //初始化疾病名称
            GetDiacrisis();

            //获取手术史记录
            GetOperationHistory();

            //禁/启用部分控件
            EditControlEnabled(false);
            btnCancel.Enabled = false;

        }
               

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
        //获取疾病记录
        private void GetOperationHistory()
        {

            m_Table = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_RedactPatientInfoFrm",
                 new SqlParameter[] { new SqlParameter("@FrmType", "27"), new SqlParameter("@NoOfInpat", m_NoOfInpat) }, CommandType.StoredProcedure);
            gridViewDiseaseHistory.SelectAll();
            gridViewDiseaseHistory.DeleteSelectedRows();
            gridControlDiseaseHistory.DataSource = m_Table;

            SqlUtil.App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiseaseHistory);
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
            lookUpEditorDiagnosisID.Enabled = bolEnabled;
            memoEditDiscuss.Enabled = bolEnabled;
            dateEditDate.Enabled = bolEnabled;
            radioGroupCure.Enabled = bolEnabled;

            gridControlDiseaseHistory.Enabled = !bolEnabled;

        }
        #endregion

        #region 保存添加、修改、删除操作
        public void SaveUCDiseaseHistoryInfo(bool isSave)
        {
            if (lookUpEditorDiagnosisID.CodeValue == null ||
                dateEditDate.Text.ToString().Trim()=="" ||
                radioGroupCure.SelectedIndex ==-1 )
            {
                SqlUtil.App.CustomMessageBox.MessageShow("除疾病评论外，其他必须填写!");
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
                        SaveDiseaseHistory();
                        InitForm();
                    }

                }
                //修改
                else if (btnModify.Enabled && btnCancel.Enabled && isSave)
                {
                    if (SqlUtil.App.CustomMessageBox.MessageShow("确定修改当前记录吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        strTemp = "修改";
                        ModifyDiseaseHistory();
                        InitForm();
                    }
                }
                //删除
                else if (btnDel.Enabled && !btnCancel.Enabled && !isSave)
                {
                    int fouceRowIndex = gridViewDiseaseHistory.FocusedRowHandle;
                    if (fouceRowIndex < 0)
                    {
                        SqlUtil.App.CustomMessageBox.MessageShow("请选择需要删除的记录! ");
                        return;
                    }
                    DataRow foucesRow = gridViewDiseaseHistory.GetDataRow(fouceRowIndex);

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

                        SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditIllnessHistoryInfo", sqlParam, CommandType.StoredProcedure);
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

        #region 保存疾病史信息
        /// <summary>
        /// 保存疾病史信息
        /// </summary>
        private void SaveDiseaseHistory()
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                    { 
                        new SqlParameter("@EditType", SqlDbType.VarChar),//编辑信息类型：1：添加、2：修改、3：删除
                        new SqlParameter("@ID", SqlDbType.VarChar),
                        new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                        new SqlParameter("@DiagnosisICD", SqlDbType.VarChar),
                        new SqlParameter("@Discuss", SqlDbType.VarChar),
                        new SqlParameter("@DiseaseTime", SqlDbType.VarChar),
                        new SqlParameter("@Cure", SqlDbType.VarChar)
                    };

            sqlParam[0].Value = "1";
            sqlParam[1].Value = "-1";
            sqlParam[2].Value = string.IsNullOrEmpty(m_NoOfInpat) ? "0" : m_NoOfInpat;
            sqlParam[3].Value =lookUpEditorDiagnosisID.CodeValue.ToString().Trim();
            sqlParam[4].Value = memoEditDiscuss.Text.ToString().Trim();
            sqlParam[5].Value = dateEditDate.DateTime.Date.ToString("yyyy-MM-dd");
            sqlParam[6].Value = radioGroupCure.SelectedIndex.ToString();

            SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditIllnessHistoryInfo", sqlParam, CommandType.StoredProcedure);
            SqlUtil.App.CustomMessageBox.MessageShow("添加成功!");

        }
        #endregion

        #region 修改疾病史信息
        /// <summary>
        /// 修改疾病史信息
        /// </summary>
        /// <param name="foucesRow">一条记录</param>
        private void ModifyDiseaseHistory()
        {

            int fouceRowIndex = gridViewDiseaseHistory.FocusedRowHandle;
            if (fouceRowIndex < 0) return;
            DataRow foucesRow = gridViewDiseaseHistory.GetDataRow(fouceRowIndex);

            SqlParameter[] sqlParam = new SqlParameter[] 
                    { 
                         new SqlParameter("@EditType", SqlDbType.VarChar),//编辑信息类型：1：添加、2：修改、3：删除
                        new SqlParameter("@ID", SqlDbType.VarChar),
                        new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                        new SqlParameter("@DiagnosisICD", SqlDbType.VarChar),
                        new SqlParameter("@Discuss", SqlDbType.VarChar),
                        new SqlParameter("@DiseaseTime", SqlDbType.VarChar),
                        new SqlParameter("@Cure", SqlDbType.VarChar)
                    };

            sqlParam[0].Value = "2";
            sqlParam[1].Value = foucesRow["ID"].ToString();
            sqlParam[2].Value = string.IsNullOrEmpty(m_NoOfInpat) ? "0" : m_NoOfInpat;
            sqlParam[3].Value = lookUpEditorDiagnosisID.CodeValue.ToString().Trim();
            sqlParam[4].Value = memoEditDiscuss.Text.ToString().Trim();
            sqlParam[5].Value = dateEditDate.DateTime.Date.ToString("yyyy-MM-dd");
            sqlParam[6].Value = radioGroupCure.SelectedIndex.ToString();

            SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditIllnessHistoryInfo", sqlParam, CommandType.StoredProcedure);
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
            int fouceRowIndex = gridViewDiseaseHistory.FocusedRowHandle;
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
            SaveUCDiseaseHistoryInfo(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnModify.Enabled = true;
            btnDel.Enabled = true;
            btnAdd.Enabled = true;
            btnCancel.Enabled = false;
            EditControlEnabled(false);
        }

        private void gridViewDiseaseHistory_Click(object sender, EventArgs e)
        {
            int fouceRowIndex = gridViewDiseaseHistory.FocusedRowHandle;
            if (fouceRowIndex < 0) return;
            DataRow foucesRow = gridViewDiseaseHistory.GetDataRow(fouceRowIndex);

            string ID = foucesRow["ID"].ToString().Trim();

            foreach (DataRow dr in m_Table.Rows)
            {
                if (dr["ID"].ToString().Equals(ID))
                {
                    memoEditDiscuss.Text = dr["Discuss"].ToString();
                    lookUpEditorDiagnosisID.CodeValue = dr["DiagnosisICD"].ToString();
                    dateEditDate.Text = dr["DiseaseTime"].ToString().Trim();
                    radioGroupCure.SelectedIndex = int.Parse(dr["Cure"].ToString().Trim() == "" ? "-1" : dr["Cure"].ToString());

                    break;
                }
            }
        }


    }
}
