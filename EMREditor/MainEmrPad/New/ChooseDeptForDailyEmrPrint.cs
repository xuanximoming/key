using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.DSSqlHelper;
using DrectSoft.Emr.Util;
using DrectSoft.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DrectSoft.Core.MainEmrPad.New
{
    public partial class ChooseDeptForDailyEmrPrint : DevBaseForm
    {
        #region Field && Property
        string m_Noofinpat;
        public string DeptChangeID { get; set; }//转科记录ID
        public string DeptID { get; set; }//科室ID
        public bool IsNeedShow = true;//是否需要调用ShowDialog方法
        private List<EmrModelDeptContainer> m_DeptModelList;
        private bool m_IsNeedFilter = false;
        private FloderState floaderState = FloderState.Default;
        #endregion

        #region .ctor
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="noofinpat">病人首页序号</param>
        public ChooseDeptForDailyEmrPrint(string noofinpat, bool isNeedFilter/*是否需要根据权限进行筛选*/)
        {
            InitializeComponent();
            m_Noofinpat = noofinpat;
            m_IsNeedFilter = isNeedFilter;
            InitForm();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="title">窗体标题</param>
        /// <param name="noofinpat">病人首页序号</param>
        public ChooseDeptForDailyEmrPrint(string title, string noofinpat, bool isNeedFilter/*是否需要根据权限进行筛选*/)
            : this(noofinpat, isNeedFilter)
        {
            this.Text = title;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="noofinpat">病人首页序号</param>
        public ChooseDeptForDailyEmrPrint(string noofinpat, bool isNeedFilter/*是否需要根据权限进行筛选*/, List<EmrModelDeptContainer> list)
        {
            InitializeComponent();
            m_Noofinpat = noofinpat;
            m_IsNeedFilter = isNeedFilter;
            m_DeptModelList = list;
            InitForm();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="noofinpat">病人首页序号</param>
        public ChooseDeptForDailyEmrPrint(string noofinpat, bool isNeedFilter, List<EmrModelDeptContainer> list, FloderState floaState)
        {
            InitializeComponent();
            m_Noofinpat = noofinpat;
            m_IsNeedFilter = isNeedFilter;
            floaderState = floaState;
            m_DeptModelList = list;
            InitForm();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="noofinpat">病人首页序号</param>
        public ChooseDeptForDailyEmrPrint(string title, string noofinpat, bool isNeedFilter, List<EmrModelDeptContainer> list)
        {
            InitializeComponent();
            this.Text = title;
            m_Noofinpat = noofinpat;
            m_IsNeedFilter = isNeedFilter;
            m_DeptModelList = list;
            InitForm();
        }
        #endregion

        private void InitForm()
        {
            try
            {
                ///是否为补写病历入口且为补写管理员
                bool IsWriteAdmin = false;
                if (floaderState == FloderState.NoneAudit)
                {
                    List<string> jobList = DS_BaseService.GetRolesByUserID(DS_Common.currentUser.Id);
                    IsWriteAdmin = jobList.Contains("00") || jobList.Contains("111");
                }

                //通过首页序号获得病人所待过的科室
                DataTable dt = GetDeptByNoofinpat(m_Noofinpat);
                //获取当前登录人有权限的科室
                DataTable dtUser2Dept = GetUser2Dept();

                if (m_IsNeedFilter && !IsWriteAdmin)
                {
                    //保留登录人当前科室和登录人有权限的科室
                    var dtRows = dt.AsEnumerable().Where(dr =>
                        {
                            string deptid = dr["deptid"].ToString();
                            string wardid = dr["wardid"].ToString();
                            string currentDeptID = DS_Common.currentUser.CurrentDeptId;
                            string currentWardID = DS_Common.currentUser.CurrentWardId;
                            if (currentDeptID == deptid && currentWardID == wardid)
                            {
                                return true;
                            }
                            if (dtUser2Dept.Rows.Cast<DataRow>().Where(drUser2Dept =>
                                {
                                    string drDeptid = drUser2Dept["deptid"].ToString();
                                    string drWardid = drUser2Dept["wardid"].ToString();
                                    if (deptid == drDeptid && wardid == drWardid)
                                    {
                                        return true;
                                    }
                                    return false;
                                }).Count() > 0)
                            {
                                return true;
                            }
                            return false;
                        });
                    if (null != dtRows && dtRows.Count() > 0)
                    {
                        dt = dtRows.CopyToDataTable();
                    }
                    else
                    {
                        dt = dt.Clone();
                    }
                }

                ///如果左侧菜单科室没显示，则选择框也不显示
                if (null != m_DeptModelList && m_DeptModelList.Count > 0)
                {
                    var changIDs = m_DeptModelList.Select(p => p.ChangeID);
                    var newRows = dt.AsEnumerable().Where(p => changIDs.Contains(p["id"].ToString()));
                    dt = (null != newRows && newRows.Count() > 0) ? newRows.CopyToDataTable() : dt.Clone();
                }
                gridControlDept.DataSource = dt;

                gridViewDept.FocusedRowHandle = dt.Rows.Count - 1;
                if (dt.Rows.Count == 1)
                {
                    Commit();
                    IsNeedShow = false;
                    this.DialogResult = DialogResult.Yes;
                }
                else if (dt.Rows.Count == 0)
                {
                    this.DialogResult = DialogResult.No;
                    return;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region Event
        private void ChooseDeptForDailyEmrPrint_Load(object sender, EventArgs e)
        {
        }

        private void gridControlDept_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Commit();
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Commit();
                this.DialogResult = DialogResult.Yes;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Commit();
                this.DialogResult = System.Windows.Forms.DialogResult.No;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridControlDept_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewDept.CalcHitInfo(gridControlDept.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle >= 0)
                {
                    Commit();
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region Method
        private void Commit()
        {
            try
            {
                if (gridViewDept.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow row = gridViewDept.GetDataRow(gridViewDept.FocusedRowHandle);
                if (null == row)
                {
                    return;
                }
                DeptChangeID = row["ID"].ToString();
                DeptID = row["deptid"].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 通过首页序号获得病人所待过的科室
        /// 0：入科 1：转科
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        private DataTable GetDeptByNoofinpat(string noofinpat)
        {
            try
            {
                string sqlGetDept = string.Format(@"select i.id, i.newdeptid deptid, d.name deptname, w.id wardid, w.name wardname, i.createtime indatetime
                                                      from inpatientchangeinfo i, department d, ward w
                                                    where i.newdeptid = d.id and i.newwardid = w.id and d.valid = 1 and i.noofinpat = '{0}' 
                                                     and i.valid = 1 and w.valid = 1
                                                     and i.changestyle in ('0', '1', '2') ", noofinpat);

                //if (!string.IsNullOrEmpty(m_FilterDept))
                //{
                //    sqlGetDept += string.Format(" and i.newdeptid = '{0}' ", m_FilterDept);
                //}

                sqlGetDept += " order by i.createtime";

                return DS_SqlHelper.ExecuteDataTable(sqlGetDept, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetUser2Dept()
        {
            try
            {
                return DS_SqlHelper.ExecuteDataTable(string.Format(
                    "select userid, deptid, wardid from user2dept where userid = '{0}'", DS_Common.currentUser.Id)
                    , CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}