using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DrectSoft.Core.Permission
{
    /// <summary>
    /// 图片签名窗体
    /// <summary>
    /// <auth>zyx</auth>
    /// <date>2012-12-12</date>
    /// </summary>
    /// </summary>
    public partial class UCPicSign : DevExpress.XtraEditors.XtraUserControl
    {
        private IEmrHost m_App;

        #region 方法
        /// <summary>
        ///  初始化科室信息
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        private void InitDepartment()
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] 
                {
                    new SqlParameter("@GetType",SqlDbType.VarChar),
                    new SqlParameter("@result",SqlDbType.Structured)
                };
                parms[0].Value = "1";
                parms[1].Direction = ParameterDirection.Output;
                DataTable Dept = DS_SqlHelper.ExecuteDataTable("emrproc.usp_GetMedicalRrecordViewFrm", parms, CommandType.StoredProcedure);
                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";
                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("ID", 60);
                cols.Add("NAME", 90);
                SqlWordbook deptWordBook = new SqlWordbook("querydept", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
                lookUpEditorDepartment.CodeValue = "0000";
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 有参构造
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="app"></param>
        public UCPicSign(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                DS_SqlHelper.CreateSqlHelper();
                m_App = app;
                InitDepartment();
                #region 屏蔽右键菜单
                ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
                DS_Common.CancelMenu(panelControl1, contextMenuStrip1);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///  绑定gcPicSignList
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        private void InitgcPicSignList()
        {
            try
            {
                txtUserId.Focus();
                string sqlSearch = @"select a.name UserName,a.id  UserId ,b.name DepartmentName,c.userpicflow, case when c.Valide=1 then '是' else '否' end as  Valide from users a left join department b on a.deptid=b.id left join userpicsign  c on a.id=c.userid where a.name like  '%' ||@UserName|| '%' and a.id like  '%' ||@UserId|| '%' and a.valid=1 and b.valid=1";
                SqlParameter[] parms;

                string departmentname = lookUpEditorDepartment.CodeValue;
                if (string.IsNullOrEmpty(departmentname) || departmentname.Equals("0000"))
                {
                    parms = new SqlParameter[] 
                    {
                        new SqlParameter("@UserName",SqlDbType.NVarChar),
                        new SqlParameter("@UserId",SqlDbType.NVarChar)
                    };
                    parms[0].Value = txtUserName.Text.Trim().ToString();
                    parms[1].Value = txtUserId.Text.Trim().ToString();
                }
                else
                {
                    sqlSearch += "and b.id=@departmentid";
                    parms = new SqlParameter[] 
                    {
                        new SqlParameter("@UserName",SqlDbType.NVarChar),
                        new SqlParameter("@UserId",SqlDbType.NVarChar),
                        new SqlParameter("@departmentid",SqlDbType.NChar)
                    };
                    parms[0].Value = txtUserName.Text.Trim().ToString();
                    parms[1].Value = txtUserId.Text.Trim().ToString();
                    parms[2].Value = departmentname;
                }

                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlSearch, parms, CommandType.Text);
                gcPicSignList.DataSource = dt;
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 事件
        /// <summary>
        /// 查询事件
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                InitgcPicSignList();
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPicSign_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        /// <summary>
        /// 双击事件
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcPicSignList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                DataRow foucesRow = gvPicSign.GetFocusedDataRow();
                if (foucesRow == null)
                {
                    return;
                }
                PicSignForm picSignForm = new PicSignForm(m_App, foucesRow);
                picSignForm.StartPosition = FormStartPosition.CenterParent;
                if (picSignForm.ShowDialog() == DialogResult.OK)
                {
                    InitgcPicSignList();
                }
                InitgcPicSignList();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑按钮
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvPicSign.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow foucesRow = gvPicSign.GetDataRow(gvPicSign.FocusedRowHandle);
                if (foucesRow == null)
                {
                    MessageBox.Show("请选择一条记录");
                    return;
                }
                PicSignForm picSignForm = new PicSignForm(m_App, foucesRow);

                picSignForm.ShowDialog();

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

    }
}
