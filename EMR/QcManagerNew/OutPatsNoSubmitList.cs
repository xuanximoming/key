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
using DrectSoft.Wordbook;

namespace DrectSoft.Emr.QcManagerNew
{
    public partial class OutPatsNoSubmitList : DevExpress.XtraEditors.XtraUserControl
    {
        public OutPatsNoSubmitList(IEmrHost app)
        {
            InitializeComponent();
            m_App = app;
        }
        private DataTable m_pageSouce;
        IEmrHost m_App;
        private frmContainer m_frmContainer;

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
                InitDepartment();
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
                lookUpEditorDepartment.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void InitDepartment()
        {
            try
            {

                lookUpWindowDepartment.SqlHelper = m_App.SqlHelper;

                string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid ");
                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 70);
                cols.Add("NAME", 80);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void gridControlpatlist_DoubleClick(object sender, EventArgs e)
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
                MyMessageBox.Show(1, ex);
            }
        }


        private void GetPatientDetailList(string noofinpat, string patstatus)
        {
            try
            {
                m_frmContainer = new frmContainer(m_App, noofinpat, patstatus);
                m_frmContainer.StartPosition = FormStartPosition.CenterScreen;
                m_frmContainer.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DevButtonQurey_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dateEdit_begin.DateTime > this.dateEdit_end.DateTime)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("起始时间不能大于结束时间");
                    return;
                }
                BindData();

                if ((DataView)this.gridView2.DataSource == null || ((DataView)this.gridView2.DataSource).ToTable() == null || ((DataView)this.gridView2.DataSource).ToTable().Rows.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有符合条件的记录");
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void BindData()
        {
            try
            {
                string dept_id = this.lookUpEditorDepartment.CodeValue.Trim();
                string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                string end_time = this.dateEdit_end.DateTime.AddDays(1).ToString("yyyy-MM-dd") + " 23:59:59";
                string sql = @"select distinct inp.noofinpat,
                inp.name pname,
                d.name dname,
                decode(inp.sexid, '1', '男', '2', '女', '未知') sex,
                inp.agestr,
                inp.outbed,
                u.name resident,
                dg.name dgname,
                inp.admitdate,
                inp.outhosdate,
                inp.status,
                rd.islock
  from inpatient inp
  left join recorddetail rd on inp.noofinpat = rd.noofinpat
  left join department d on d.id = inp.outhosdept
  left join diagnosis dg on dg.markid = inp.admitdiagnosis
  left join users u  on u.id=inp.resident
 where (rd.islock <> '4701' or rd.islock is null)
   and (inp.outhosdept='{0}' or '{0}'='' or '{0}' is null )
   and to_date(inp.admitdate, 'yyyy-mm-dd hh24:mi:ss') between
       to_date('{1}', 'yyyy-mm-dd hh24:mi:ss') and
       to_date('{2}', 'yyyy-mm-dd hh24:mi:ss')
   and (inp.status='1503' or inp.status='1502')
   and d.valid = '1'  and rd.valid='1'";

                sql = string.Format(sql, dept_id, begin_time, end_time);
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
                gridControlpatlist.DataSource = dt;
                //this.labPatCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
                //lookUpEditorDepartment.CodeValue = m_App.User.CurrentDeptId;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }
}
