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
using DrectSoft.Core;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 此页面用于配置科室质控员
    /// add by ywk 2012年7月10日 11:28:42
    /// </summary>
    public partial class ConfigQCAudit : DevBaseForm
    {
        #region 构造函数
        public ConfigQCAudit()
        {
            InitializeComponent();
        }
        public ConfigQCAudit(IEmrHost app)
        {
            m_App = app;
            m_SqlManager = new SqlManger(app);
            InitializeComponent();
            this.lookUpEditorDepartment.Focus();
        }
        #endregion

        #region 属性和字段
        private IEmrHost m_App;
        /// <summary>
        /// 存储SQL语句的类,数据操作相关
        /// </summary>
        SqlManger m_SqlManager;

        #endregion

        #region 方法
        /// <summary>
        /// 绑定科室下拉列表
        /// </summary>
        private void InitLookDept()
        {
            try
            {
                string sql = @"SELECT ID, NAME,py,wb
                          FROM department, dept2ward dw
                         WHERE department.ID = dw.deptid
                         ORDER BY ID";
                DataTable dtDept = m_App.SqlHelper.ExecuteDataTable(sql);
                //绑定科室
                lookUpEditorDepartment.Kind = WordbookKind.Sql;
                lookUpEditorDepartment.ListWindow = lookUpWDept;
                lookUpEditorDepartmentSearch.Kind = WordbookKind.Sql;
                lookUpEditorDepartmentSearch.ListWindow = lookUpWDept;
                BindDepartmentWordBook(dtDept);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 处理下拉列表的Workbook
        /// </summary>
        /// <param name="dtDept"></param>
        private void BindDepartmentWordBook(DataTable dataTableData)
        {
            try
            {
                for (int i = 0; i < dataTableData.Columns.Count; i++)
                {
                    if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "id")
                    {
                        dataTableData.Columns[i].Caption = "科室编码";
                    }
                    else if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "name")
                    {
                        dataTableData.Columns[i].Caption = "科室名称";
                    }
                }

                Dictionary<string, int> colWidths = new Dictionary<string, int>();
                colWidths.Add("ID", 70);
                colWidths.Add("NAME", 80);
                SqlWordbook wordBook = new SqlWordbook("Department", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = wordBook;
                lookUpEditorDepartmentSearch.SqlWordbook = wordBook;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 绑定科室质控员下拉框
        /// </summary>
        private void InitLookQCPerson()
        {
            try
            {
                string sqlseachdoc = @" SELECT u.ID, u.NAME, u.py, u.wb
                  FROM users u  WHERE u.valid = '1'
                  ORDER BY u.ID ";
                DataTable dtDoc = m_App.SqlHelper.ExecuteDataTable(sqlseachdoc);
                lookUpEQCPerson.Kind = WordbookKind.Sql;
                lookUpEQCPerson.ListWindow = lookUpWQCPerson;
                BindEmployeeWordBook(dtDoc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 科室质控员下拉框
        /// </summary>
        /// <param name="dtDoc"></param>
        private void BindEmployeeWordBook(DataTable dtDoc)
        {
            try
            {
                for (int i = 0; i < dtDoc.Columns.Count; i++)
                {
                    if (dtDoc.Columns[i].ColumnName.ToLower().Trim() == "id")
                    {
                        dtDoc.Columns[i].Caption = "医师代码";
                    }
                    else if (dtDoc.Columns[i].ColumnName.ToLower().Trim() == "name")
                    {
                        dtDoc.Columns[i].Caption = "医师名称";
                    }
                }
                Dictionary<string, int> colWidths = new Dictionary<string, int>();
                colWidths.Add("ID", 70);
                colWidths.Add("NAME", 80);
                SqlWordbook wordBook = new SqlWordbook("Employee", dtDoc, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
                lookUpEQCPerson.SqlWordbook = wordBook;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 绑定列表，各科室质控员配置情况
        /// </summary>
        private void BindGridControl()
        {
            try
            {
                DataTable dtData = m_SqlManager.GetDeptQCManager("");
                gridControl1.DataSource = dtData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 根据选择科室控制，质控员和科主任下拉框
        /// </summary>
        private void GetDoctorUser()
        {
            try
            {
                string deptid = lookUpEditorDepartment.CodeValue;
                if (deptid == "")
                    return;
                else
                {
                    //处理科室质控员下拉框
                    string sql = string.Format(@"select * from users a where a.valid = 1 and a.deptid = '{0}' and a.grade is not null  and a.grade <> '2004' ", deptid);
                    lookUpWQCPerson.SqlHelper = m_App.SqlHelper;
                    DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
                    dt.Columns["ID"].Caption = "医生代码";
                    dt.Columns["NAME"].Caption = "医生名称";
                    Dictionary<string, int> cols = new Dictionary<string, int>();
                    cols.Add("ID", 70);
                    cols.Add("NAME", 80);
                    SqlWordbook deptWordBook = new SqlWordbook("querybook", dt, "ID", "NAME", cols, "ID//NAME//py//wb");
                    lookUpEQCPerson.SqlWordbook = deptWordBook;
                    //处理科室主任下拉框
                    lookUpEZhuRen.Enabled = true;
                    lookUpWZhuren.SqlHelper = m_App.SqlHelper;
                    DataTable dt1 = m_SqlManager.GetDirectorDoc(deptid);
                    dt1.Columns["ID"].Caption = "医生代码";
                    dt1.Columns["NAME"].Caption = "医生名称";
                    Dictionary<string, int> cols1 = new Dictionary<string, int>();
                    cols1.Add("ID", 70);
                    cols1.Add("NAME", 80);
                    SqlWordbook deptWordBook1 = new SqlWordbook("querybook", dt1, "ID", "NAME", cols1, "ID//NAME//py//wb");
                    lookUpEZhuRen.SqlWordbook = deptWordBook1;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region 右键菜单相关操作
        /// <summary>
        /// 绑定右键菜单 
        /// </summary>
        private DevExpress.XtraBars.BarButtonItem barButtonItemDeleteInfo;
        private void BindPopupMenu()
        {
            try
            {
                barButtonItemDeleteInfo = new DevExpress.XtraBars.BarButtonItem();
                barButtonItemDeleteInfo.Caption = "删除";
                barButtonItemDeleteInfo.Id = 1;
                barButtonItemDeleteInfo.Name = "Delete";
                barButtonItemDeleteInfo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItemDeleteInfo_ItemClick);
                popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
                new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemDeleteInfo)
            });
                barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { 
                barButtonItemDeleteInfo 
            });
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void barButtonItemDeleteInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DataRow dr = grdViewConfigAudit.GetDataRow(grdViewConfigAudit.FocusedRowHandle);
                if (dr != null)
                {
                    string id = dr["ID"].ToString();
                    string deptid = string.Empty;
                    string qcmanagerid = string.Empty;
                    string chiefdoctorid = string.Empty;
                    string valid = string.Empty;

                    if (m_App.CustomMessageBox.MessageShow("您确认删除吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        m_SqlManager.DeleteQCManagerUSer(id, deptid, qcmanagerid, chiefdoctorid, valid);
                    }
                    BindGridControl();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #endregion

        #region 事件
        /// <summary>
        /// 科室下拉框改变，后面的科室质控员捞此科室的，科室主任取 此科室的主任
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lookUpEditorDepartment_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                lookUpEQCPerson.CodeValue = "";
                lookUpEZhuRen.CodeValue = "";
                GetDoctorUser();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 即为添加操作，如果原来已经配置该科室，则为更新操作 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                string deptid = lookUpEditorDepartment.CodeValue.ToString() == null ? string.Empty : lookUpEditorDepartment.CodeValue.ToString();
                string qcmanagerid = lookUpEQCPerson.CodeValue.ToString() == null ? string.Empty : lookUpEQCPerson.CodeValue.ToString();
                string chiefdoctorid = lookUpEZhuRen.CodeValue.ToString() == null ? string.Empty : lookUpEZhuRen.CodeValue.ToString();
                string valid = string.Empty;

                if (string.IsNullOrEmpty(deptid))
                {
                    m_App.CustomMessageBox.MessageShow("选项不能为空，请核对选择项！");
                    lookUpEditorDepartment.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(qcmanagerid))
                {
                    m_App.CustomMessageBox.MessageShow("选项不能为空，请核对选择项！");
                    lookUpEQCPerson.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(chiefdoctorid))
                {
                    m_App.CustomMessageBox.MessageShow("选项不能为空，请核对选择项！");
                    lookUpEZhuRen.Focus();
                    return;
                }
                DataTable dt = m_SqlManager.GetDeptQCManager(deptid);
                if (dt.Rows.Count > 0)//有就更新
                {
                    m_SqlManager.UpdateQCManagerUSer("", deptid, qcmanagerid, chiefdoctorid, "1");
                    m_App.CustomMessageBox.MessageShow("更新成功");
                }
                else//没有就插入
                {
                    m_SqlManager.InsertQCManagerUSer("", deptid, qcmanagerid, chiefdoctorid, "1");
                    m_App.CustomMessageBox.MessageShow("新增成功");
                }
                BindGridControl();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 查询操作 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtData = m_SqlManager.GetDeptQCManager(lookUpEditorDepartmentSearch.CodeValue);
                gridControl1.DataSource = dtData;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigQCAudit_Load(object sender, EventArgs e)
        {
            try
            {
                InitLookDept();//加载科室下拉框
                InitLookQCPerson();//加载科室质控员列表
                BindGridControl();//绑定科室质控人员列表
                lookUpEditorDepartment.CodeValueChanged += new EventHandler(lookUpEditorDepartment_CodeValueChanged);
                BindPopupMenu();//处理右键菜单
                grdViewConfigAudit.MouseDown += new MouseEventHandler(grdViewConfigAudit_MouseDown);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 处理列表上右键进行删除操作 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grdViewConfigAudit_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    GridHitInfo hit = grdViewConfigAudit.CalcHitInfo(e.X, e.Y);
                    if (hit.RowHandle >= 0)
                    {
                        DataRow dr = grdViewConfigAudit.GetDataRow(hit.RowHandle);
                        if (dr != null)
                        {
                            barButtonItemDeleteInfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            this.popupMenu1.ShowPopup(this.barManager1, (Cursor.Position));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void grdViewConfigAudit_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.grdViewConfigAudit.FocusedRowHandle == -1)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}