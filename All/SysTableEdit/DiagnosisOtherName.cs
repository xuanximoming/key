using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
using System.Data.SqlClient;
using DrectSoft.Core;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.SysTableEdit
{
    /// <summary>
    /// xll 2013-08-05
    /// 用于通用的别名维护
    /// </summary>
    public partial class DiagnosisOtherNaem : DevBaseForm
    {

        private DataTable dtAll;
        private string m_ICDID;
        private string m_type; //1西医诊断 2 中医诊断 3 手术
        public DiagnosisOtherNaem(string icdID, string type)
        {
            try
            {
                InitializeComponent();
                InitDate(icdID, type);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        public void InitDate(string icdID, string type)
        {
            try
            {
                m_type = type;
                m_ICDID = icdID;
                GetOtherNameList();
                gridControl1.DataSource = dtAll;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GetOtherNameList()
        {
            try
            {
                string tableName = "";
                switch (m_type)
                {
                    case "1":
                        tableName = "diagnosisothername";
                        break;
                    case "2":
                        tableName = "diagnosischiothername";
                        break;
                    case "3":
                        tableName = "operothername";
                        break;
                    default:
                        break;

                }

                string sqlStr = string.Format(@"select * from {0} where valid='1' and icdid='{1}'", tableName,m_ICDID);
                 dtAll = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
                 dtAll.Columns.Add("XG");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
          
            DataRow dr = dtAll.NewRow();
            dr["ID"]="";
            dr["ICDID"] = m_ICDID;
            dr["NAME"] = "";
            dr["PY"] = "";
            dr["WB"] = "";
            dr["VALID"] = "";
            dr["XG"] = "1";
            dtAll.Rows.Add(dr);


            gridControl1.DataSource = dtAll;
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("未选中记录");
                    return;
                }
             DataRow dr=   gridView1.GetDataRow(e.RowHandle);
             dr["XG"] = "1";
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void bbtnDel_Click(object sender, EventArgs e)
        {
            try
            {

                DataRow dr = gridView1.GetFocusedDataRow();
                if (dr == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("未选中记录");
                    return;
                }
            DialogResult dresult=    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("确定要删除吗？", "提示", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo);
            if (dresult == DialogResult.No)
            {
                return;
            }
                if (!string.IsNullOrEmpty(dr["ID"].ToString()))
                {
                    DelOtherEntity(dr);
                    
                }
                dtAll.Rows.Remove(dr);
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void DelOtherEntity(DataRow dr)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
           {
                 new SqlParameter("@ID",SqlDbType.VarChar),
               new SqlParameter("@ICDID",SqlDbType.VarChar),
               new SqlParameter("@NAME",SqlDbType.VarChar),
               new SqlParameter("@PY",SqlDbType.VarChar),
               new SqlParameter("@WB",SqlDbType.VarChar),
               new SqlParameter("@VALID",SqlDbType.VarChar)
              
           };

                sqlParams[0].Value = dr["ID"].ToString();
                sqlParams[1].Value = dr["ICDID"].ToString() ;
                sqlParams[2].Value = dr["NAME"].ToString() ;
                sqlParams[3].Value =  dr["PY"].ToString();
                sqlParams[4].Value = dr["WB"].ToString() ;
                sqlParams[5].Value =  "0";
                string proName = "";
                switch (m_type)
                {
                    case "1":
                        proName = "EMRBaseInfo.usp_AddOrModDiaOther";
                        break;
                    case "2":
                        proName = "EMRBaseInfo.usp_AddOrModDiaChiOther";
                        break;
                    case "3":
                        proName = "EMRBaseInfo.usp_AddOrModOperOther";
                        break;
                    default:
                        break;

                }
                if (string.IsNullOrEmpty(proName))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("参数传递错误，无法找到存储过程");
                    return;
                }
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(proName, sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool Valide(DataRow dr)
        {
            if (dr["Name"].ToString() == null || dr["NAME"].ToString().Trim() == "")
            {
                MessageBox.Show("别名不能为空");
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                foreach (DataRow item in dtAll.Rows)
                {
                    if (item["XG"].ToString() != "1") continue;
                   bool IsValide= Valide(item);
                   if (!IsValide)
                   {
                       return;
                   }
                }

                foreach (DataRow item in dtAll.Rows)
                {
                    if (item["XG"].ToString() != "1") continue;
                    if (string.IsNullOrEmpty(item["ID"].ToString()))
                    {
                        item["ID"]= Guid.NewGuid().ToString();
                    }

                    //wb py valid在后台数据库赋值
                    SqlParameter[] sqlParams = new SqlParameter[]
           {
                 new SqlParameter("@ID",SqlDbType.VarChar),
               new SqlParameter("@ICDID",SqlDbType.VarChar),
               new SqlParameter("@NAME",SqlDbType.VarChar),
               new SqlParameter("@PY",SqlDbType.VarChar),
               new SqlParameter("@WB",SqlDbType.VarChar),
               new SqlParameter("@VALID",SqlDbType.VarChar)
              
           };

                    sqlParams[0].Value = item["ID"].ToString();
                    sqlParams[1].Value = item["ICDID"].ToString();
                    sqlParams[2].Value = item["NAME"].ToString();
                    sqlParams[3].Value = item["PY"].ToString();
                    sqlParams[4].Value = item["WB"].ToString();
                    sqlParams[5].Value = item["VALID"].ToString();
                    string proName = "";
                    switch (m_type)
                    {
                        case "1":
                            proName = "EMRBaseInfo.usp_AddOrModDiaOther";
                            break;
                        case "2":
                            proName = "EMRBaseInfo.usp_AddOrModDiaChiOther";
                            break;
                        case "3":
                            proName = "EMRBaseInfo.usp_AddOrModOperOther";
                            break;
                        default:
                            break;

                    }
                    if (string.IsNullOrEmpty(proName))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("参数传递错误，无法找到存储过程");
                        return;
                    }
                    DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(proName, sqlParams, CommandType.StoredProcedure);
                    item["XG"] = "1";
                }
                MessageBox.Show("保存成功！");
       
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }



    }
}