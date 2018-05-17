using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.DSSqlHelper;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Core;
using System.Data.SqlClient;
using System.Data.OracleClient;
using DrectSoft.Common;

namespace DrectSoft.Emr.QcManagerNew
{
    public partial class DepartmentDetail : DevExpress.XtraEditors.XtraUserControl//DevBaseForm
    {
        string m_deptcode;
        public DepartmentDetail(IEmrHost app, string deptcode)
        {
            InitializeComponent();
            m_deptcode = deptcode;
            m_App = app;
        }
        IEmrHost m_App;
        private IDataAccess sql_helper = DataAccessFactory.DefaultDataAccess;
        private DataTable m_pageSouce;
        private frmContainer m_frmContainer;
        OutPatsSubmitList m_OutPatsSubmitList;
        public void GetDepartmentDetail(string deptcode)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@deptid", SqlDbType.VarChar),
                  };
                sqlParams[0].Value = deptcode;
                DataSet ds = sql_helper.ExecuteDataSet("EMRQCMANAGER.usp_QcmanagerDepartmentDetail", sqlParams, CommandType.StoredProcedure);
                m_pageSouce = ds.Tables[0];

                gridControldepartment.DataSource = m_pageSouce;

                if (m_pageSouce.Rows.Count == 0)
                {
                    MessageBox.Show("无数据");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetDeptName(string deptcode)
        {
            try
            {
                if (deptcode == "")
                {
                    return "";
                }
                string sql = string.Format(@"select d.name from department d where d.id='{0}' ", deptcode);
                return DS_SqlHelper.ExecuteScalar(sql).ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void DepartmentList_Load(object sender, EventArgs e)
        {
            try
            {
                GetDepartmentDetail(m_deptcode);
                label2.Text = GetDeptName(m_deptcode);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void gridControldepartment_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridView2.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选择病人");
                    return;
                }
                DataRow dataRow = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                string noofinpat = dataRow["noofinpat"].ToString();
                string patstatus = dataRow["status"].ToString();
                //this.WindowState = FormWindowState.Minimized;
                GetPatientDetailList(noofinpat, patstatus);
                //this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void GetPatientDetailList(string noofinpat, string patstatus)
        {
            try
            {

                m_frmContainer = new frmContainer(m_App, noofinpat, patstatus);
                m_frmContainer.StartPosition = FormStartPosition.CenterScreen;
                //m_frmContainer.Dock = DockStyle.Fill;
                m_frmContainer.ShowDialog();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void buttonmark_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView2.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选择病人");
                    return;
                }
                DataRow dataRow = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                string noofinpat = dataRow["noofinpat"].ToString();
                string patstatus = dataRow["status"].ToString();
                GetPatientDetailList(noofinpat, patstatus);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButtonBackList_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}
