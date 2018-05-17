using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DrectSoft.Core.RedactPatientInfo.PublicSet;

namespace DrectSoft.Core.RedactPatientInfo.UserControls
{
    public partial class UCLinkman : DevExpress.XtraEditors.XtraUserControl
    {
        string m_NoOfInpat;
        DataTable m_Table;


        public UCLinkman(string NoOfInpat)
        {
            InitializeComponent();

            m_NoOfInpat = NoOfInpat;


        }

        #region 初始化窗体控件
        private void InitForm()
        {
            //关系
            SqlUtil.GetDictionarydetail(lookUpRelation, "13", "44");
            //性别
            SqlUtil.GetDictionarydetail(lookUpSex, "13", "3");

            //读取第一联系人信息
            SetPatientContacts();

            EditControlEnabled(false);

            btnCancel.Enabled = false;

        }
        #endregion

        #region 读取第一联系人信息
        /// <summary>
        /// 读取第一联系人信息
        /// </summary>
        private void SetPatientContacts()
        {
            m_Table = SqlUtil.GetRedactPatientInfoFrm("15", "", m_NoOfInpat);
            gridControlLinkman.DataSource = m_Table;

            SqlUtil.App.PublicMethod.ConvertGridDataSourceUpper(gridviewLinkman);

        }
        #endregion

        #region 编辑控件启用/禁止
        /// <summary>
        /// 编辑控件启用/禁止
        /// </summary>
        /// <param name="bolEnabled"></param>
        private void EditControlEnabled(bool bolEnabled)
        {
            txtName.Enabled = bolEnabled;
            txtHomeAddress.Enabled = bolEnabled;
            txtHomeTel.Enabled = bolEnabled;
            txtOfficeTel.Enabled = bolEnabled;
            txtPostalCode.Enabled = bolEnabled;
            txtWorkUnit.Enabled = bolEnabled;
            lookUpRelation.Enabled = bolEnabled;
            lookUpSex.Enabled = bolEnabled;
            chkFirstLinkman.Enabled = bolEnabled;

            gridControlLinkman.Enabled = !bolEnabled;

        }
        #endregion

        #region 保存添加、修改、删除操作
        public void SaveUCLinkmanInfo(bool isSave)
        {
            string strTemp = "操作";
            try
            {

                //添加
                if (btnAdd.Enabled && btnCancel.Enabled && isSave)
                {
                    if (SqlUtil.App.CustomMessageBox.MessageShow("确定添加当前记录吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        strTemp = "添加";
                        SaveLinkmanInfo();
                        InitForm();
                    }

                }
                //修改
                else if (btnModify.Enabled && btnCancel.Enabled && isSave)
                {
                    if (SqlUtil.App.CustomMessageBox.MessageShow("确定修改当前记录吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        strTemp = "修改";
                        ModifyLinkmanInfo();
                        InitForm();
                    }
                }
                //删除
                else if (btnDel.Enabled && !btnCancel.Enabled && !isSave)
                {
                    int fouceRowIndex = gridviewLinkman.FocusedRowHandle;
                    if (fouceRowIndex < 0)
                    {
                        SqlUtil.App.CustomMessageBox.MessageShow("请选择需要删除的记录! ");
                        return;
                    }
                    DataRow foucesRow = gridviewLinkman.GetDataRow(fouceRowIndex);

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

                        SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditPatientContacts", sqlParam, CommandType.StoredProcedure);
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

        #region 保存第一联系人信息
        /// <summary>
        /// 保存第一联系人信息
        /// </summary>
        private void SaveLinkmanInfo()
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                    { 
                        new SqlParameter("@EditType", SqlDbType.VarChar),//编辑信息类型：1：添加、2：修改、3：删除
                        new SqlParameter("@ID", SqlDbType.VarChar),
                        new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                        new SqlParameter("@Name", SqlDbType.VarChar),
                        new SqlParameter("@Sex", SqlDbType.VarChar),
                        new SqlParameter("@Relation", SqlDbType.VarChar),
                        new SqlParameter("@Address", SqlDbType.VarChar),
                        new SqlParameter("@WorkUnit", SqlDbType.VarChar),
                        new SqlParameter("@HomeTel", SqlDbType.VarChar),
                        new SqlParameter("@WorkTel", SqlDbType.VarChar),
                        new SqlParameter("@PostalCode", SqlDbType.VarChar),
                        new SqlParameter("@Tag", SqlDbType.VarChar)

                    };

            sqlParam[0].Value = "1";
            sqlParam[1].Value = "0";
            sqlParam[2].Value = string.IsNullOrEmpty(m_NoOfInpat) ? "0" : m_NoOfInpat;
            sqlParam[3].Value = txtName.Text.ToString().Trim();
            sqlParam[4].Value = lookUpSex.EditValue.ToString();
            sqlParam[5].Value = lookUpRelation.EditValue.ToString();
            sqlParam[6].Value = txtHomeAddress.Text.ToString().Trim();
            sqlParam[7].Value = txtWorkUnit.Text.ToString().Trim();
            sqlParam[8].Value = txtHomeTel.Text.ToString().Trim();
            sqlParam[9].Value = txtOfficeTel.Text.ToString().Trim();
            sqlParam[10].Value = txtPostalCode.Text.ToString().Trim();
            sqlParam[11].Value = chkFirstLinkman.Checked ? "1" : "0";

            SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditPatientContacts", sqlParam, CommandType.StoredProcedure);
            SqlUtil.App.CustomMessageBox.MessageShow("添加成功!");

        }
        #endregion

        #region 修改第一联系人信息
        /// <summary>
        /// 修改第一联系人信息
        /// </summary>
        /// <param name="foucesRow">一条记录</param>
        private void ModifyLinkmanInfo()
        {

            int fouceRowIndex = gridviewLinkman.FocusedRowHandle;
            if (fouceRowIndex < 0) return;
            DataRow foucesRow = gridviewLinkman.GetDataRow(fouceRowIndex);

            SqlParameter[] sqlParam = new SqlParameter[] 
                    { 
                        new SqlParameter("@EditType", SqlDbType.VarChar),//编辑信息类型：1：添加、2：修改、3：删除
                        new SqlParameter("@ID", SqlDbType.VarChar),
                        new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                        new SqlParameter("@Name", SqlDbType.VarChar),
                        new SqlParameter("@Sex", SqlDbType.VarChar),
                        new SqlParameter("@Relation", SqlDbType.VarChar),
                        new SqlParameter("@Address", SqlDbType.VarChar),
                        new SqlParameter("@WorkUnit", SqlDbType.VarChar),
                        new SqlParameter("@HomeTel", SqlDbType.VarChar),
                        new SqlParameter("@WorkTel", SqlDbType.VarChar),
                        new SqlParameter("@PostalCode", SqlDbType.VarChar),
                        new SqlParameter("@Tag", SqlDbType.VarChar)

                    };

            sqlParam[0].Value = "2";
            sqlParam[1].Value = foucesRow["ID"].ToString();
            sqlParam[2].Value = string.IsNullOrEmpty(m_NoOfInpat) ? "0" : m_NoOfInpat;
            sqlParam[3].Value = txtName.Text.ToString().Trim();
            sqlParam[4].Value = lookUpSex.EditValue.ToString();
            sqlParam[5].Value = lookUpRelation.EditValue.ToString();
            sqlParam[6].Value = txtHomeAddress.Text.ToString().Trim();
            sqlParam[7].Value = txtWorkUnit.Text.ToString().Trim();
            sqlParam[8].Value = txtHomeTel.Text.ToString().Trim();
            sqlParam[9].Value = txtOfficeTel.Text.ToString().Trim();
            sqlParam[10].Value = txtPostalCode.Text.ToString().Trim();
            sqlParam[11].Value = chkFirstLinkman.Checked ? "1" : "0";

            SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditPatientContacts", sqlParam, CommandType.StoredProcedure);
            SqlUtil.App.CustomMessageBox.MessageShow("修改成功!");

        }

        #endregion
        #endregion

        private void gridviewLinkman_Click(object sender, EventArgs e)
        {
            int fouceRowIndex = gridviewLinkman.FocusedRowHandle;
            if (fouceRowIndex < 0) return;
            DataRow foucesRow = gridviewLinkman.GetDataRow(fouceRowIndex);

            string ID = foucesRow["ID"].ToString().Trim();

            foreach (DataRow dr in m_Table.Rows)
            {
                if (dr["ID"].ToString().Equals(ID))
                {
                    txtName.Text = dr["Name"].ToString();
                    txtHomeAddress.Text = dr["Address"].ToString();
                    txtHomeTel.Text = dr["HomeTel"].ToString();
                    txtOfficeTel.Text = dr["WorkTel"].ToString();
                    txtPostalCode.Text = dr["PostalCode"].ToString();
                    txtWorkUnit.Text = dr["WorkUnit"].ToString();
                    lookUpRelation.EditValue = dr["Relation"].ToString().Trim();
                    lookUpSex.EditValue = dr["Sex"].ToString().Trim();
                    chkFirstLinkman.Checked = dr["Tag"].ToString().Equals("1") ? true : false;

                    break;
                }
            }
        }

        private void UCLinkman_Load(object sender, EventArgs e)
        {
            InitForm();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnModify.Enabled = false;
            btnDel.Enabled = false;
            btnCancel.Enabled = true;
            EditControlEnabled(true);
            return;
            //txtName.Text = "";
            //txtHomeAddress.Text = "";
            //txtHomeTel.Text = "";
            //txtOfficeTel.Text = "";
            //txtPostalCode.Text = "";
            //txtWorkUnit.Text = "";
            //lookUpRelation.EditValue = "";
            //lookUpSex.EditValue = "";
            //chkFirstLinkman.Checked = false;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            int fouceRowIndex = gridviewLinkman.FocusedRowHandle;
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
            SaveUCLinkmanInfo(false);

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
