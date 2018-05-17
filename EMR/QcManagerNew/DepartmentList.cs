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
    public partial class DepartmentList : DevExpress.XtraEditors.XtraUserControl
    {
        public DepartmentList(IEmrHost app)
        {
            InitializeComponent();
            m_App = app;
        }
        private DataTable m_pageSouce;
        IEmrHost m_App;
        private DepartmentDetail m_Departmentdetail;
        //private frmDetail m_frmDetail;
        public void GetDepartmentList()
        {
            try
            {
                OracleParameter par1 = new OracleParameter("@result", OracleType.Cursor);
                par1.Direction = ParameterDirection.Output;
                DataSet ds = DS_SqlHelper.ExecuteDataSet("EMRQCMANAGER.usp_QcmanagerDepartmentlist", new OracleParameter[] { par1 }, CommandType.StoredProcedure);
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
                GetDepartmentList();
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
                    MessageBox.Show("请选择科室");
                    return;
                }
                DataRow dataRow = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                string deptcode = dataRow["deptcode"].ToString();
                QcManagerForm.deptcode = deptcode;
                GetDepartmentDetailList();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void GetDepartmentDetailList()
        {
            try
            {
                m_Departmentdetail = null;
                m_Departmentdetail = new DepartmentDetail(m_App, QcManagerForm.deptcode);

                m_Departmentdetail.Dock = DockStyle.Fill;

                this.Parent.Controls.Add(m_Departmentdetail);
                m_Departmentdetail.BringToFront();
                //m_Departmentdetail.StartPosition = FormStartPosition.CenterScreen;
                //m_Departmentdetail.ShowDialog();
                //m_frmDetail = null;
                //m_frmDetail = new frmDetail(m_App, deptcode);
                //m_frmDetail.StartPosition = FormStartPosition.CenterScreen;
                //m_frmDetail.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }

        private void buttondetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView2.FocusedRowHandle <= 0)
                {
                    MessageBox.Show("请选择科室");
                    return;
                }
                DataRow dataRow = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                string deptcode = dataRow["deptcode"].ToString();
                QcManagerForm.deptcode = deptcode;
                GetDepartmentDetailList();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

    }
}
