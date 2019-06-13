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
    public partial class DepartmentListOut : DevExpress.XtraEditors.XtraUserControl
    {
        public DepartmentListOut()
        {
            InitializeComponent();
        }
        private DataTable m_pageSouce;

        private DepartmentDetail m_Departmentdetail;
        public void GetDepartmentListOut()
        {
            try
            {
                //OracleParameter par1 = new OracleParameter("@result", OracleType.Cursor);
                //par1.Direction = ParameterDirection.Output;
                //DataSet ds = DS_SqlHelper.ExecuteDataSet("EMRQCMANAGER.usp_QcmanagerDepartmentlistout", new OracleParameter[] { par1 }, CommandType.StoredProcedure);
                //m_pageSouce = ds.Tables[0];

                //gridControldepartment.DataSource = m_pageSouce;
                //if (m_pageSouce.Rows.Count == 0)
                //{
                //    MessageBox.Show("无数据");
                //}
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
                GetDepartmentListOut();
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
                if (gridView2.FocusedRowHandle == 0)
                {
                    return;
                }
                DataRow dataRow = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                string deptcode = dataRow["deptcode"].ToString();
                GetDepartmentDetailList(deptcode);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void GetDepartmentDetailList(string deptcode)
        {
            try
            {
                //if (m_Departmentdetail == null)
                //    m_Departmentdetail = new DepartmentDetail(m,deptcode);

                //m_Departmentdetail.Dock = DockStyle.Fill;
                //xTabPageEmrScore.Controls.Add(m_Departmentdetail);
                //xTabPageEmrScore.PageVisible = true;
                ////m_OutMedicalScore.RefreshData();
                //xtraTabQcManager.SelectedTabPage = xTabPageEmrScore;
                //xtraTabQcManager.TabPages.Add(xTabPageEmrScore);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }
    }
}
