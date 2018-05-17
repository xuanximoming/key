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
    public partial class UCPersonalHistory : DevExpress.XtraEditors.XtraUserControl
    {
        string m_NoOfInpat;
        
        public UCPersonalHistory(string  NoOfInpat)
        {
            InitializeComponent();
            m_NoOfInpat= NoOfInpat;

            
        }

        #region 初始化窗体
        private void InitForm()
        {
            //出生省市
            SqlUtil.GetAreas(lookUpBirthPlace, "11", "1001");
            //经历省市
            SqlUtil.GetAreas(lookUpPassPlace, "11", "1001");  

            //婚姻
            SqlUtil.GetDictionarydetail(lookUpMarital, "13", "4");

            //是否喝酒
            SqlUtil.GetDictionarydetail(lookUpDrinkWine, "21", "0");
             //是否吸烟
            SqlUtil.GetDictionarydetail(lookUpSmoke, "21", "0");

            //初始化个人史信息
            GetPersonalHistory();
        }

        #region 初始化个人史信息
        //初始化个人史信息
        private void GetPersonalHistory()
        {
                        
            DataTable table = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_RedactPatientInfoFrm",
                 new SqlParameter[] { new SqlParameter("@FrmType", "23"), new SqlParameter("@NoOfInpat", m_NoOfInpat) }, CommandType.StoredProcedure);
            if (table.Rows.Count <= 0) return;

            lookUpBirthPlace.EditValue = table.Rows[0]["BirthPlace"].ToString();
            lookUpPassPlace.EditValue = table.Rows[0]["PassPlace"].ToString();
            lookUpMarital.EditValue = table.Rows[0]["Marital"].ToString();
            lookUpDrinkWine.EditValue = table.Rows[0]["DrinkWine"].ToString();
            lookUpSmoke.EditValue = table.Rows[0]["Smoke"].ToString();
            txtNoOfChild.Text = table.Rows[0]["NoOfChild"].ToString();
            memoEditJobHistory.Text = table.Rows[0]["JobHistory"].ToString();
            memoEditSmokeHistory.Text = table.Rows[0]["SmokeHistory"].ToString();
            memoEditWineHistory.Text = table.Rows[0]["WineHistory"].ToString();

        }
        #endregion

        private void UCPersonalHistory_Load(object sender, EventArgs e)
        {
            InitForm();
        }

        #endregion

        public void SaveUCPersonalHistory()
        {
            try
            {
                if (lookUpBirthPlace.EditValue == null || lookUpPassPlace.EditValue == null ||
                   lookUpMarital.EditValue == null || lookUpDrinkWine.EditValue == null ||
                   lookUpSmoke.EditValue == null || txtNoOfChild.Text.ToString().Trim() == "")
                {
                    SqlUtil.App.CustomMessageBox.MessageShow("除饮酒史、吸烟史和职业经历情况外，其他必须填写！");
                    return;
                }


                if (SqlUtil.App.CustomMessageBox.MessageShow("确定保存当前信息吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.No) return;

                SqlParameter[] sqlParam = new SqlParameter[] 
                    { 
                        new SqlParameter("@BirthPlace", SqlDbType.VarChar),//编辑信息类型：1：添加、2：修改、3：删除
                        new SqlParameter("@PassPlace", SqlDbType.VarChar),
                        new SqlParameter("@Marital", SqlDbType.VarChar),
                        new SqlParameter("@DrinkWine", SqlDbType.VarChar),

                        new SqlParameter("@Smoke", SqlDbType.VarChar),
                        new SqlParameter("@NoOfChild", SqlDbType.VarChar),
                        new SqlParameter("@JobHistory", SqlDbType.VarChar),

                        new SqlParameter("@SmokeHistory", SqlDbType.VarChar),
                        new SqlParameter("@WineHistory", SqlDbType.VarChar),
                        new SqlParameter("@NoOfInpat", SqlDbType.VarChar)
                    };

               
                sqlParam[0].Value =  lookUpBirthPlace.EditValue.ToString().Trim();
                sqlParam[1].Value =   lookUpPassPlace.EditValue.ToString().Trim();
                sqlParam[2].Value =  lookUpMarital.EditValue.ToString().Trim();
                sqlParam[3].Value =  lookUpDrinkWine.EditValue.ToString().Trim();

                sqlParam[4].Value =  lookUpSmoke.EditValue.ToString().Trim();
                sqlParam[5].Value = txtNoOfChild.Text.ToString().Trim();
                sqlParam[6].Value = memoEditJobHistory.Text.ToString().Trim();

                sqlParam[7].Value = memoEditSmokeHistory.Text.ToString().Trim();
                sqlParam[8].Value = memoEditWineHistory.Text.ToString().Trim();
                sqlParam[9].Value = string.IsNullOrEmpty(m_NoOfInpat) ? "0" : m_NoOfInpat;

                SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_EditPersonalHistoryInfo", sqlParam, CommandType.StoredProcedure);
                SqlUtil.App.CustomMessageBox.MessageShow("保存成功!");
            }
            catch (Exception ex)
            {
                SqlUtil.App.CustomMessageBox.MessageShow("保存失败!\n详细错误：" + ex.Message);
            }

        }

          }
}
