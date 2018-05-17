using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Core.RedactPatientInfo.PublicSet;


namespace DrectSoft.Core.RedactPatientInfo.UserControls
{
    public partial class UCBaseInfo : DevExpress.XtraEditors.XtraUserControl
    {        
         DataTable m_Table;
         string m_NoOfInpat;


         public UCBaseInfo(DataTable Table, string NoOfInpat)  
        {
            InitializeComponent();
            m_Table=Table;
            m_NoOfInpat = NoOfInpat;

            //初始化窗体控件
            //InitForm();
        }


        #region 初始化窗体控件
        private void InitForm()
        {
            //婚姻
            SqlUtil.GetDictionarydetail(lookUpMarriage,"13", "4");
            //民族
            SqlUtil.GetDictionarydetail(lookUpNation, "13", "42");
            //国籍
            SqlUtil.GetDictionarydetail(lookUpNationality, "13", "43");
            //性别
            SqlUtil.GetDictionarydetail(lookUpSex, "13", "3");
            //文化程度
            SqlUtil.GetDictionarydetail(lookUpDiploma, "13", "25");
            //职业
            SqlUtil.GetDictionarydetail(lookUpProfession, "13", "41");
             //病人性质
            SqlUtil.GetDictionarydetail(lookUpPatientProperty, "13", "1");


            //出身省市
            SqlUtil.GetAreas(lookUpProvince, "11", "1000");            
            //籍贯省市
            SqlUtil.GetAreas(lookUpFamilyProvince, "11", "1000");

            //读取病人信息
           if (m_Table!=null) SetPatientInfo(m_Table);

        }
        #endregion

        #region 显示病人信息
        /// <summary>
        /// 显示病人信息
        /// </summary>
        /// <param name="dt">病人信息表</param>
        private void SetPatientInfo(DataTable dt)
        {
            if (dt.Rows.Count <= 0) return;
            dateEditBirthday.Text = dt.Rows[0]["Birth"].ToString().Trim();
            lookUpMarriage.EditValue = dt.Rows[0]["Marital"].ToString();
            lookUpNation.EditValue = dt.Rows[0]["NationID"].ToString().Trim();

            lookUpNationality.EditValue = dt.Rows[0]["NationalityID"].ToString().Trim();
            lookUpSex.EditValue = dt.Rows[0]["SexID"].ToString().Trim();
            lookUpDiploma.EditValue = dt.Rows[0]["EDU"].ToString().Trim();
            lookUpProvince.EditValue = dt.Rows[0]["ProvinceID"].ToString().Trim();
            lookUpCounty.EditValue = dt.Rows[0]["CountyID"].ToString().Trim();

            lookUpFamilyProvince.EditValue = dt.Rows[0]["Nativeplace_P"].ToString().Trim();
            lookUpFamilyCounty.EditValue = dt.Rows[0]["Nativeplace_C"].ToString().Trim();
            lookUpPatientProperty.EditValue = dt.Rows[0]["PayID"].ToString().Trim();
            txtName.Text = dt.Rows[0]["Name"].ToString();

            txtAge.Text = dt.Rows[0]["Age"].ToString();
            txtReligion.Text = dt.Rows[0]["Religion"].ToString();
            txtEducationTime.Text = dt.Rows[0]["EDUC"].ToString();
            txtIDCard.Text = dt.Rows[0]["IDNO"].ToString();

            lookUpProfession.EditValue = dt.Rows[0]["JobID"].ToString().Trim();
            txtWorkUnit.Text = dt.Rows[0]["Organization"].ToString();
            txtOfficeAddress.Text = dt.Rows[0]["OfficePlace"].ToString();
            txtOfficePostalCode.Text = dt.Rows[0]["OfficePost"].ToString();
            txtOfficeTel.Text = dt.Rows[0]["OfficeTEL"].ToString();

            txtHousehold.Text = dt.Rows[0]["NativeAddress"].ToString();
            txtPostalCode.Text = dt.Rows[0]["NativePost"].ToString();
            txtTel.Text = dt.Rows[0]["NativeTEL"].ToString();
            txtAddress.Text = dt.Rows[0]["Address"].ToString();

        }
        #endregion

        #region 保存基本信息
        public void SaveBaseInfo()
        {
            try
             {
                if (SqlUtil.App.CustomMessageBox.MessageShow("确定修改当前记录吗？", CustomMessageBoxKind.QuestionYesNo)==DialogResult.Yes)
                {
                    SqlParameter[] sqlParam = new SqlParameter[] 
                        { 
                            new SqlParameter("@Birth", SqlDbType.VarChar), 
                            new SqlParameter("@Marital", SqlDbType.VarChar),
                            new SqlParameter("@NationID", SqlDbType.VarChar),
                            new SqlParameter("@NationalityID", SqlDbType.VarChar),

                            new SqlParameter("@SexID", SqlDbType.VarChar),
                            new SqlParameter("@EDU", SqlDbType.VarChar),
                            new SqlParameter("@ProvinceID", SqlDbType.VarChar),
                            new SqlParameter("@CountyID", SqlDbType.VarChar),

                            new SqlParameter("@Nativeplace_P", SqlDbType.VarChar),
                            new SqlParameter("@Nativeplace_C", SqlDbType.VarChar),
                            new SqlParameter("@PayID", SqlDbType.VarChar),

                            new SqlParameter("@Age", SqlDbType.VarChar),
                            new SqlParameter("@Religion", SqlDbType.VarChar),
                            new SqlParameter("@EDUC", SqlDbType.VarChar),
                            new SqlParameter("@IDNO", SqlDbType.VarChar),

                            new SqlParameter("@JobID", SqlDbType.VarChar),
                            new SqlParameter("@Organization", SqlDbType.VarChar),
                            new SqlParameter("@OfficePlace", SqlDbType.VarChar),
                            new SqlParameter("@OfficePost", SqlDbType.VarChar),

                            new SqlParameter("@OfficeTEL", SqlDbType.VarChar),
                            new SqlParameter("@NativeAddress", SqlDbType.VarChar),
                            new SqlParameter("@NativePost", SqlDbType.VarChar),
                            new SqlParameter("@NativeTEL", SqlDbType.VarChar),
                            new SqlParameter("@Address", SqlDbType.VarChar),

                            new SqlParameter("@NoOfInpat", SqlDbType.VarChar)

                        };

                    sqlParam[0].Value = dateEditBirthday.DateTime.Date.ToString("yyyy-MM-dd").Trim();
                    sqlParam[1].Value = lookUpMarriage.EditValue.ToString().Trim();
                    sqlParam[2].Value = lookUpNation.EditValue.ToString().Trim();

                    sqlParam[3].Value = lookUpNationality.EditValue.ToString().Trim();
                    sqlParam[4].Value = lookUpSex.EditValue.ToString().Trim();
                    sqlParam[5].Value = lookUpDiploma.EditValue.ToString().Trim();
                    sqlParam[6].Value = lookUpProvince.EditValue.ToString().Trim();
                    sqlParam[7].Value = lookUpCounty.EditValue.ToString().Trim();

                    sqlParam[8].Value = lookUpFamilyProvince.EditValue.ToString().Trim();
                    sqlParam[9].Value = lookUpFamilyCounty.EditValue.ToString().Trim();
                    sqlParam[10].Value = lookUpPatientProperty.EditValue.ToString().Trim();
                    sqlParam[11].Value = txtAge.Text.ToString().Trim();

                    sqlParam[12].Value = txtReligion.Text.ToString().Trim();
                    //sqlParam[13].Value = txtEducationTime.Text.ToString().Trim() == "" ? null : txtEducationTime.Text.ToString().Trim();
                    if (SqlUtil.IsNumber(txtEducationTime.Text.ToString().Trim()))
                    {
                        sqlParam[13].Value = txtEducationTime.Text.ToString().Trim();
                    }
                    else
                    {
                        txtEducationTime.Text = "";
                        sqlParam[13].Value = null;
                    }
                    sqlParam[14].Value = txtIDCard.Text.ToString().Trim();

                    sqlParam[15].Value = lookUpProfession.EditValue.ToString().Trim();
                    sqlParam[16].Value = txtWorkUnit.Text.ToString().Trim();
                    sqlParam[17].Value = txtOfficeAddress.Text.ToString().Trim();
                    sqlParam[18].Value = txtOfficePostalCode.Text.ToString().Trim();
                    sqlParam[19].Value = txtOfficeTel.Text.ToString().Trim();

                    sqlParam[20].Value =txtHousehold.Text.ToString().Trim();
                    sqlParam[21].Value = txtPostalCode.Text.ToString().Trim();
                    sqlParam[22].Value = txtTel.Text.ToString().Trim();
                    sqlParam[23].Value = txtAddress.Text.ToString().Trim();

                    sqlParam[24].Value = m_NoOfInpat;

                    SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_UpDataBasePatientInfo", sqlParam, CommandType.StoredProcedure);
                    SqlUtil.App.CustomMessageBox.MessageShow("保存成功.");
                }
             }
            catch (Exception ex)
            {
                SqlUtil.App.CustomMessageBox.MessageShow("保存失败!\n详细错误："+ex.Message);
            }
        }
        #endregion

        private void UCBaseInfo_Load(object sender, EventArgs e)
        {

            ////初始化窗体控件
            InitForm();
        }
              
        private void lookUpProvince_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit obj = sender as LookUpEdit;
            if (obj == null) return;
            if (!(obj.EditValue == null || obj.EditValue.ToString() == "nulltext"))
            {
                if (obj == lookUpProvince)
                {
                    //出身区县
                    SqlUtil.GetAreas(lookUpCounty, "12", obj.EditValue.ToString());
                }

                else
                {
                    //籍贯区县
                    SqlUtil.GetAreas(lookUpFamilyCounty, "12", obj.EditValue.ToString());
                }
            }
        }

        

    }
}
