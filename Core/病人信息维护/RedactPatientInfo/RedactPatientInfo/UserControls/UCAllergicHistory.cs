using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Core.RedactPatientInfo.PublicSet;
using System.Data.SqlClient;

namespace DrectSoft.Core.RedactPatientInfo.UserControls
{
    public partial class UCAllergicHistory : DevExpress.XtraEditors.XtraUserControl
    {
        string m_NoOfInpat;
        DataTable m_Table;


        public UCAllergicHistory(string NoOfInpat)
        {
            InitializeComponent();
            m_NoOfInpat = NoOfInpat;

        }

        #region 初始化窗体
        private void InitForm()
        {
            //过敏类型
            SqlUtil.GetDictionarydetail(lookUpAllergyID, "21", "60");
            //过敏等级
            SqlUtil.GetDictionarydetail(lookUpAllergyLevel, "21", "61");

            //获取过敏史记录
            GetAllergicHistory();

            //禁/启用部分控件
            EditControlEnabled(false);
            btnCancel.Enabled = false;

        }

        #region 获取过敏史记录
        //获取过敏史记录
        private void GetAllergicHistory()
        {

            m_Table = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_RedactPatientInfoFrm",
                 new SqlParameter[] { new SqlParameter("@FrmType", "24"), new SqlParameter("@NoOfInpat", m_NoOfInpat) }, CommandType.StoredProcedure);
            gridViewAllergicHistory.SelectAll();
            gridViewAllergicHistory.DeleteSelectedRows();
            gridControlAllergicHistory.DataSource = m_Table;

            SqlUtil.App.PublicMethod.ConvertGridDataSourceUpper(gridViewAllergicHistory);
           
        }
        #endregion

        private void UCAllergicHistory_Load(object sender, EventArgs e)
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
            lookUpAllergyLevel.Enabled = bolEnabled;
            lookUpAllergyID.Enabled = bolEnabled;
            txtAllergyPart.Enabled = bolEnabled;
            txtDoctor.Enabled = bolEnabled;
            txtReactionType.Enabled = bolEnabled;

            gridControlAllergicHistory.Enabled = !bolEnabled;

        }
        #endregion

        #region 保存添加、修改、删除操作
        public void SaveUCAllergicHistoryInfo(bool isSave)
        {
            if (lookUpAllergyID.EditValue == null || lookUpAllergyLevel.EditValue == null)
            {
                SqlUtil.App.CustomMessageBox.MessageShow("过敏类型和过敏程度必须填写!");
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
                        SaveAllergicHistory();
                        InitForm();
                    }

                }
                //修改
                else if (btnModify.Enabled && btnCancel.Enabled && isSave)
                {
                    if (SqlUtil.App.CustomMessageBox.MessageShow("确定修改当前记录吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        strTemp = "修改";
                        ModifyAllergicHistory();
                        InitForm();
                    }
                }
                //删除
                else if (btnDel.Enabled && !btnCancel.Enabled && !isSave)
                {
                    int fouceRowIndex = gridViewAllergicHistory.FocusedRowHandle;
                    if (fouceRowIndex < 0)
                    {
                        SqlUtil.App.CustomMessageBox.MessageShow("请选择需要删除的记录! ");
                        return;
                    }
                    DataRow foucesRow = gridViewAllergicHistory.GetDataRow(fouceRowIndex);

                    if (SqlUtil.App.CustomMessageBox.MessageShow("确定删除当前记录吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        strTemp = "删除";
                        SqlParameter[] sqlParam = new SqlParameter[] 
                        { 
                            new SqlParameter("@EditType", SqlDbType.VarChar),//编辑信息类型：1：添加、2：修改、3：删除
                            new SqlParameter("@ID", SqlDbType.Int)
                        };

                        sqlParam[0].Value = "3";
                        sqlParam[1].Value = foucesRow["ID"].ToString();

                        SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditAllergyHistoryInfo", sqlParam, CommandType.StoredProcedure);
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

        #region 保存过敏史信息
        /// <summary>
        /// 保存过敏史信息
        /// </summary>
        private void SaveAllergicHistory()
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                    { 
                        new SqlParameter("@EditType", SqlDbType.VarChar),//编辑信息类型：1：添加、2：修改、3：删除
                        new SqlParameter("@ID", SqlDbType.Int),
                        new SqlParameter("@NoOfInpat", SqlDbType.Int),
                        new SqlParameter("@AllergyPart", SqlDbType.VarChar),
                        new SqlParameter("@Doctor", SqlDbType.VarChar),
                        new SqlParameter("@AllergyLevel", SqlDbType.Int),
                        new SqlParameter("@AllergyID", SqlDbType.Int),
                        new SqlParameter("@ReactionType", SqlDbType.VarChar)
                    };

            sqlParam[0].Value = "1";
            sqlParam[1].Value = "-1";
            sqlParam[2].Value = string.IsNullOrEmpty(m_NoOfInpat) ? "0" : m_NoOfInpat;
            sqlParam[3].Value = txtAllergyPart.Text.ToString().Trim();
            sqlParam[4].Value = txtDoctor.Text.ToString().Trim();
            sqlParam[5].Value = lookUpAllergyLevel.EditValue.ToString().Trim();
            sqlParam[6].Value = lookUpAllergyID.EditValue.ToString().Trim();
            sqlParam[7].Value = txtReactionType.Text.ToString().Trim();

            SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditAllergyHistoryInfo", sqlParam, CommandType.StoredProcedure);
            SqlUtil.App.CustomMessageBox.MessageShow("添加成功!");

        }
        #endregion

        #region 修改过敏史信息
        /// <summary>
        /// 修改过敏史信息
        /// </summary>
        /// <param name="foucesRow">一条记录</param>
        private void ModifyAllergicHistory()
        {

            int fouceRowIndex = gridViewAllergicHistory.FocusedRowHandle;
            if (fouceRowIndex < 0) return;
            DataRow foucesRow = gridViewAllergicHistory.GetDataRow(fouceRowIndex);

            SqlParameter[] sqlParam = new SqlParameter[] 
                    { 
                        new SqlParameter("@EditType", SqlDbType.VarChar),//编辑信息类型：1：添加、2：修改、3：删除
                        new SqlParameter("@ID", SqlDbType.Int),
                        new SqlParameter("@NoOfInpat", SqlDbType.Int),
                        new SqlParameter("@AllergyPart", SqlDbType.VarChar),
                        new SqlParameter("@Doctor", SqlDbType.VarChar),
                        new SqlParameter("@AllergyLevel", SqlDbType.Int),
                        new SqlParameter("@AllergyID", SqlDbType.Int),
                        new SqlParameter("@ReactionType", SqlDbType.VarChar)
                    };

            sqlParam[0].Value = "2";
            sqlParam[1].Value = foucesRow["ID"].ToString();
            sqlParam[2].Value = string.IsNullOrEmpty(m_NoOfInpat) ? "0" : m_NoOfInpat;
            sqlParam[3].Value = txtAllergyPart.Text.ToString().Trim();
            sqlParam[4].Value = txtDoctor.Text.ToString().Trim();
            sqlParam[5].Value = lookUpAllergyLevel.EditValue.ToString().Trim();
            sqlParam[6].Value =lookUpAllergyID.EditValue.ToString().Trim();
            sqlParam[7].Value = txtReactionType.Text.ToString().Trim();

            SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditAllergyHistoryInfo", sqlParam, CommandType.StoredProcedure);
            SqlUtil.App.CustomMessageBox.MessageShow("修改成功!");

        }

        #endregion
        #endregion

        private void gridViewAllergicHistory_Click(object sender, EventArgs e)
        {
            int fouceRowIndex = gridViewAllergicHistory.FocusedRowHandle;
            if (fouceRowIndex < 0) return;
            DataRow foucesRow = gridViewAllergicHistory.GetDataRow(fouceRowIndex);

            string ID = foucesRow["ID"].ToString().Trim();

            foreach (DataRow dr in m_Table.Rows)
            {
                if (dr["ID"].ToString().Equals(ID))
                {
                    txtAllergyPart.Text = dr["AllergyPart"].ToString();
                    txtDoctor.Text = dr["Doctor"].ToString();
                    lookUpAllergyLevel.EditValue = dr["AllergyLevel"].ToString().Trim();
                    lookUpAllergyID.EditValue = dr["AllergyID"].ToString().Trim();
                    txtReactionType.Text = dr["ReactionType"].ToString();

                    break;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnModify.Enabled = false;
            btnDel.Enabled = false;
            btnCancel.Enabled = true;
            EditControlEnabled(true);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            int fouceRowIndex = gridViewAllergicHistory.FocusedRowHandle;
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
            SaveUCAllergicHistoryInfo(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnModify.Enabled = true;
            btnDel.Enabled = true;
            btnAdd.Enabled = true;
            btnCancel.Enabled = false;
            EditControlEnabled(false);
        }

        

    
    }
}
