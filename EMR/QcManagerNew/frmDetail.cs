using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YidanSoft.FrameWork.WinForm.Plugin;
using YiDanCommon.Ctrs.DLG;
using YiDanSqlHelper;
using System.Data.SqlClient;
using YidanSoft.Core;
using YiDanCommon;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class frmDetail : Form
    {

        public frmDetail(IYidanEmrHost app, string deptcode)
        {
            InitializeComponent();
            m_app = app;
            m_deptcode = deptcode;
        }
        IYidanEmrHost m_app;
        string m_deptcode;
        DepartmentDetail m_departmentdetail = null;
        private IDataAccess sql_helper = DataAccessFactory.DefaultDataAccess;
        private DataTable m_pageSouce;
        private frmContainer m_frmContainer;
        private void frmDetail_Load(object sender, EventArgs e)
        {
            try
            {
                GetDepartmentDetail(m_deptcode);
                label2.Text = GetDeptName(m_deptcode);
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(1, ex);
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage1;
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(1, ex);
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
                return YD_SqlHelper.ExecuteScalar(sql).ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

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
                    YiDanMessageBox.Show("无数据");
                }
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
                YD_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(1, ex);
            }
        }

        private void gridControldepartment_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DataRow dataRow = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                string noofinpat = dataRow["noofinpat"].ToString();
                string patstatus = dataRow["status"].ToString();
                GetPatientDetailList(noofinpat, patstatus);
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(1, ex);
            }
        }


        private void GetPatientDetailList(string noofinpat, string patstatus)
        {
            try
            {
                m_frmContainer = new frmContainer(m_app, noofinpat, patstatus);
                //m_frmContainer.StartPosition = FormStartPosition.CenterScreen;
                m_frmContainer.Dock = DockStyle.Fill;
                //m_frmContainer.ShowDialog();
                xtraTabPage2.Controls.Add(m_frmContainer);
                xtraTabControl1.SelectedTabPage = xtraTabPage2;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
