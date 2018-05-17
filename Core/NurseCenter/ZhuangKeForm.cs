using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System.Data.SqlClient;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.NurseCenter
{
    /// <summary>
    /// xll 2012-10-09 转科功能
    /// </summary>
    public partial class ZhuangKeForm :DevBaseForm
    {
        IEmrHost m_App;
        string m_NoofInpat;
        public ZhuangKeForm(IEmrHost app, string noofInpat)
        {
            try
            {
                InitializeComponent();
                InitDate(app, noofInpat);
            }
            catch (Exception ex)
            {
                this.m_App.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        /// <summary>
        /// 绑定科室病区信息
        /// </summary>
        /// <param name="app"></param>
        /// <param name="noofInpat"></param>
        public void InitDate(IEmrHost app, string noofInpat)
        {
            m_App = app;
            m_NoofInpat = noofInpat;
            BindDept();
            BindPatent();
        }



        /// <summary>
        /// 绑定所有科室
        /// </summary>
        private void BindDept()
        {
            lookUpWindowDept.SqlHelper = m_App.SqlHelper;
            string sql = string.Format(@"select distinct ID, NAME,py,wb from department where valid='1'");
            DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);
            Dept.Columns["ID"].Caption = "科室代码";
            Dept.Columns["NAME"].Caption = "科室名称";
            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("ID", 60);
            cols.Add("NAME", 90);
            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorDept.SqlWordbook = deptWordBook;
        }

        //绑定科室
        private void BindWard()
        {
            try
            {
                lookUpEditorWard.CodeValue = "";
                lookUpWindowWard.SqlHelper = m_App.SqlHelper;
                string deptcode = lookUpEditorDept.CodeValue;
                if (deptcode == null)
                    deptcode = "";

                string sqlward = string.Format(@"select distinct ID, NAME,py,wb from ward where valid='1'and id in (select wardid from dept2ward where deptid='{0}') or '{0}' is null or '{0}' = ''''", deptcode);
                DataTable Ward = m_App.SqlHelper.ExecuteDataTable(sqlward);
                Ward.Columns["ID"].Caption = "病区代码";
                Ward.Columns["NAME"].Caption = "病区名称";

                Dictionary<string, int> colsWard = new Dictionary<string, int>();
                colsWard.Add("ID", 60);
                colsWard.Add("NAME", 90);
                SqlWordbook wardWordBook = new SqlWordbook("querybook1", Ward, "ID", "NAME", colsWard, "ID//Name//PY//WB");
                lookUpEditorWard.SqlWordbook = wardWordBook;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void BindPatent()
        {
            string sqlsel = "select * from inpatient where noofinpat=@noofinpat";
            SqlParameter[] sqlParams = new SqlParameter[] { 
                   new SqlParameter("@noofinpat", SqlDbType.VarChar, 50)
            };
            sqlParams[0].Value = m_NoofInpat;
            DataTable dtInpatient = m_App.SqlHelper.ExecuteDataTable(sqlsel, sqlParams, CommandType.Text);
            if (dtInpatient != null && dtInpatient.Rows != null)
            {
                string outHosDept = dtInpatient.Rows[0]["outhosdept"].ToString();
                string outHosWard = dtInpatient.Rows[0]["outhosward"].ToString();
                lookUpEditorDept.CodeValue = outHosDept;
                lookUpEditorWard.CodeValue = outHosWard;
            }
        }


        private void lookUpEditorDept_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                BindWard();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool ValidateDate(ref string message)
        {
            if (string.IsNullOrEmpty(lookUpEditorDept.CodeValue))
            {
                message = "科室代码不能为空";
                return false;
            }
            else if (string.IsNullOrEmpty(lookUpEditorWard.CodeValue))
            {
                message = "病区代码不能为空";
                return false;
            }
            return true;
        }

        private bool Save()
        {
            string message = "";
            bool validateResult = ValidateDate(ref message);
            if (!validateResult)
            {
                m_App.CustomMessageBox.MessageShow(message);
                return false;
            }
            else
            {
                string departCode = lookUpEditorDept.CodeValue;
                string wardCode = lookUpEditorWard.CodeValue;
                string sqlInsert = "update inpatient set outhosdept=@departCode,outhosward=@wardCode where noofinpat=@noofinpat";
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@departCode", SqlDbType.VarChar, 50),
                  new SqlParameter("@wardCode", SqlDbType.VarChar, 50),
                   new SqlParameter("@noofinpat", SqlDbType.VarChar, 50)
            };
                sqlParams[0].Value = departCode;
                sqlParams[1].Value = wardCode;
                sqlParams[2].Value = m_NoofInpat;
                m_App.SqlHelper.ExecuteNoneQuery(sqlInsert, sqlParams, CommandType.Text);
                return true;
            }
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                bool result = Save();
                if (result)
                {
                    m_App.CustomMessageBox.MessageShow("转科成功");
                    this.DialogResult = DialogResult.Yes;
                }
            }
            catch (Exception ex)
            {
                m_App.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}