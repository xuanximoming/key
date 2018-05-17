using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.DSSqlHelper;
using DrectSoft.Wordbook;
using System.Data.SqlClient;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Core.Consultation.Dal;
using DrectSoft.Core.Consultation.NEW.Enum;
using DrectSoft.Core.Consultation.NEW;

namespace Consultation.NEW
{
    /// <summary>
    /// 新的会诊记录单
    /// Add xlb 2013-02-25
    /// </summary>
    public partial class UCRecordSuggestion : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_app;
        string nOofinpat = string.Empty;
        string mconsultApplySn = string.Empty;
        ConsultRecordForWrite consultZdr;

        #region 方法 Add xlb 2013-02-26

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCRecordSuggestion()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化会诊记录单窗体
        /// Add xlb 2013-02-27
        /// </summary>
        /// <param name="noofinpat">病案号</param>
        /// <param name="host">接口</param>
        /// <param name="consultApplySn">会诊申请单号</param>
        /// <param name="readOnly">是否只读</param>
        public void Init(string noofinpat, IEmrHost host, string consultApplySn, bool readOnly, ConsultRecordForWrite consultZDR/*会诊类型*/)
        {
            try
            {
                DS_SqlHelper.CreateSqlHelper();
                RegisterEvent();
                nOofinpat = noofinpat;
                m_app = host;
                consultZdr = consultZDR;
                mconsultApplySn = consultApplySn;
                InitData();
                //窗体打开可新增记录
                SetReadonly(readOnly);
                this.ActiveControl = memoEditSuggestion;
                #region 屏蔽右键菜单 by xlb 2013-03-21
                ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
                DS_Common.CancelMenu(groupControl2, contextMenuStrip1);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Init(string noofinpat, IEmrHost host, string consultApplySn, bool readOnly)
        {
            try
            {
                DS_SqlHelper.CreateSqlHelper();
                RegisterEvent();
                nOofinpat = noofinpat;
                m_app = host;
                mconsultApplySn = consultApplySn;
                InitData();
                //窗体打开可新增记录
                SetReadonly(readOnly);
                this.ActiveControl = memoEditSuggestion;
                #region 屏蔽右键菜单 by xlb 2013-03-21
                ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
                DS_Common.CancelMenu(groupControl2, contextMenuStrip1);
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化相关数据
        /// Add xlb 2013-02-27
        /// </summary>
        private void InitData()
        {
            try
            {
                InitDataElement();
                SetData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读取原先数据
        /// Add xlb 2013-02-27
        /// </summary>
        private void SetData()
        {
            try
            {
                DataSet ds = DrectSoft.Core.Consultation.Dal.DataAccess.GetConsultationDataSet(mconsultApplySn, "20");//, Convert.ToString((int)ConsultType.More));
                DataTable dtConsultApply = ds.Tables[0];
                DataTable dtConsultApplyDepartment = ds.Tables[1];
                DataTable dtConsultRecordDepartment = ds.Tables[2];

                if (dtConsultApply == null || dtConsultApply.Rows.Count <= 0)
                {
                    return;
                }
                memoEditSuggestion.Text = dtConsultApply.Rows[0]["ConsultSuggestion"].ToString() == "" ?
                SetComposeConsultSuggestion(consultZdr) : dtConsultApply.Rows[0]["ConsultSuggestion"].ToString();
                if (dtConsultApply.Rows[0]["FinishTime"].ToString().Trim().Split(' ').Length == 2)
                {
                    dateEditConsultationDate.EditValue = dtConsultApply.Rows[0]["FinishTime"].ToString().Split(' ')[0];
                    timeEditConsultationTime.EditValue = dtConsultApply.Rows[0]["FinishTime"].ToString().Split(' ')[1];
                }
                else
                {
                    dateEditConsultationDate.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
                    timeEditConsultationTime.EditValue = DateTime.Now.ToString("HH:mm");
                }
                if (dtConsultRecordDepartment.Rows.Count > 0)
                {
                    gridControlDepartment.DataSource = dtConsultRecordDepartment;
                    m_app.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置界面只读方法
        /// Add xlb 2013=-02-27
        /// </summary>
        /// <param name="readOnly"></param>
        private void SetReadonly(bool readOnly)
        {
            try
            {
                memoEditSuggestion.Enabled = !readOnly;
                dateEditConsultationDate.Enabled = !readOnly;
                timeEditConsultationTime.Enabled = !readOnly;
                btnSaveConsultApply.Enabled = !readOnly;
                btnCompleteConsult.Enabled = !readOnly;
                btnAdd.Enabled = !readOnly;
                btnEdit.Enabled = !readOnly;
                btnDelete.Enabled = !readOnly;
                if (readOnly)
                {
                    gridViewDept.Columns["DELETEBUTTON"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 多科会诊且会诊意见由受邀医师填写申请人初次打开默认值是否组合其他医师意见
        /// Add xlb 2013-03-07
        /// </summary>
        private string SetComposeConsultSuggestion(ConsultRecordForWrite consultZDR)
        {
            try
            {
                //多科会诊且配置由受邀医师填写会诊意见 申请医师总结时根据配置是否组合其他医师意见
                if (consultZDR == ConsultRecordForWrite.MultiEmployee)
                {
                    string value = ConsultCommon.GetConfigKey("IsComposeSuggestion").Trim();
                    string ComposeInfo = string.Empty;
                    if (value != "0")
                    {
                        string sql = @"select s.consultsuggestion,u.name from consultsuggestion s join 
                     users u on s.createuser=u.id where s.valid=1 and u.valid=1 and s.consultapplysn=@consultApplySn  and s.state='20'";
                        SqlParameter[] sps = { new SqlParameter("@consultApplySn", mconsultApplySn) };
                        DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                        if (dt == null || dt.Rows.Count <= 0)
                        {
                            return "";
                        }
                        foreach (DataRow row in dt.Rows)
                        {
                            ComposeInfo += "[" + row["NAME"] + "]" + ":" + row["CONSULTSUGGESTION"];
                            ComposeInfo += "\r\n";
                        }
                        return ComposeInfo;
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add  by xlb 2013-03-02
        /// 校验方法
        /// </summary>
        /// <param name="NeedVertity">会诊记录保存时只需校验邀请列表是否为空</param>
        /// <param name="message">提示信息</param>
        /// <returns>是否通过校验 true:通过false:没通过</returns>
        private bool Validate(bool needVerity, ref string message)
        {
            try
            {
                DataTable dt = gridControlDepartment.DataSource as DataTable;
                string sql = string.Format("select consulttime from consultapply where consultapplysn={0}", mconsultApplySn);
                DataTable dtconsulttime = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                DateTime consulttime = DateTime.Now;
                if (dtconsulttime != null && dtconsulttime.Rows.Count > 0)
                {
                    consulttime = DateTime.Parse(dtconsulttime.Rows[0][0].ToString());
                }
                DateTime finishTime = DateTime.Parse(dateEditConsultationDate.Text + " " + timeEditConsultationTime.Text);
                if (dt == null || dt.DefaultView.Count <= 0)
                {
                    message = "请填写受邀医师记录";
                    btnAdd.Focus();//焦点直接定新增按钮
                    return false;
                }
                else if (string.IsNullOrEmpty(memoEditSuggestion.Text.Trim()) && needVerity)
                {
                    message = "请填写会诊意见";
                    memoEditSuggestion.Focus();
                    return false;
                }
                else if (memoEditSuggestion.Text.Trim().Length > 1500)
                {
                    message = "会诊意见不能超过1500字";
                    memoEditSuggestion.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(dateEditConsultationDate.Text))
                {
                    message = "会诊完成时间不能为空";
                    dateEditConsultationDate.Focus();
                    return false;
                }
                else if (finishTime < consulttime)
                {
                    message = "会诊完成时间不能小于拟会诊时间";
                    dateEditConsultationDate.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 注册事件方法 Add by xlb 2013-02-26

        /// <summary>
        /// 注册事件
        /// Add xlb 2013-02-26
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                gridViewDept.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.
                RowIndicatorCustomDrawEventHandler(gridViewDept_CustomDrawRowIndicator);
                btnAdd.Click += new EventHandler(btnAdd_Click);
                btnEdit.Click += new EventHandler(btnEdit_Click);
                btnDelete.Click += new EventHandler(btnDelete_Click);
                btnSaveConsultApply.Click += new EventHandler(btnSaveConsultApply_Click);
                btnCompleteConsult.Click += new EventHandler(btnCompleteConsult_Click);
                gridViewDept.MouseDown += new MouseEventHandler(gridViewDept_MouseDown);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 初始化列表数据源
        /// </summary>
        private void InitDataElement()
        {
            try
            {
                DataTable dt = new DataTable("Department");
                dt.Columns.Add("DepartmentName");
                dt.Columns.Add("DepartmentCode");
                dt.Columns.Add("EmployeeLevelName");
                dt.Columns.Add("EmployeeLevelID");
                dt.Columns.Add("EmployeeCode");
                dt.Columns.Add("EMPLOYEENAMESTR");
                dt.Columns.Add("DeleteButton");
                dt.Columns.Add("SignIn");
                dt.Columns.Add("EmployeeName");
                gridControlDepartment.DataSource = dt;

                m_app.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 更新会诊申请相关信息的方法
        /// Add xlb 2013-03-01
        /// </summary>
        /// <param name="consultStatus"></param>
        private void UpdateConsultApply(ConsultStatus consultStatus)
        {
            try
            {
                string sql = @"UPDATE consultapply set consultsuggestion = @consultsuggestion,
                finishtime=@finishtime,stateid =@stateid,modifieduser=@modifiedUser,modifiedtime=@modifiedtime WHERE consultapplysn = @consultapplysn";
                string consultSuggestion = memoEditSuggestion.Text.Trim();
                string finishTime = dateEditConsultationDate.Text + " " + timeEditConsultationTime.Text;
                string stateId = Convert.ToString((int)consultStatus);
                SqlParameter[] sps ={
                                   new SqlParameter("@consultsuggestion",consultSuggestion),
                                   new SqlParameter("@finishtime",finishTime),
                                   new SqlParameter("@stateid",stateId),
                                   new SqlParameter("@modifiedUser",m_app.User.Id),
                                   new SqlParameter("@modifiedtime",DateTime.Now.ToString("yyyy-MM-dd HH;mm:ss")),
                                   new SqlParameter("@consultapplysn",mconsultApplySn)
                                   };
                DS_SqlHelper.ExecuteNonQuery(sql, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存方法 用于删除受邀医生或修改新增记录
        /// Add xlb 2013-03-08
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="row"></param>
        private void SaveConsultRecordDepartment(int typeId/*操作类型*/, DataRow row)
        {
            try
            {
                string departmentCode = string.Empty;
                string departmentName = string.Empty;
                string employeeCode = string.Empty;
                string employeeName = string.Empty;
                string employeeLevelID = string.Empty;
                string createUser = m_app.User.Id;//创建人
                string createTime = System.DateTime.Now.ToString();
                string modifyuser = m_app.User.Id;//修改人
                string canceluser = m_app.User.Id;//作废人
                int ConsultID = 0;
                int ordervalue = 1;
                string sql = @"select id from hospitalinfo";
                //医院代码
                string hospitalCode = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text).Rows[0][0].ToString();
                //行状态为删除则记录删除行ID
                if (row.RowState == DataRowState.Deleted)
                {
                    //获取删除前的ID值
                    ConsultID = int.Parse(row["ID", DataRowVersion.Original].ToString());
                }
                else
                {//行状态为新增或修改状态则记录相应的值
                    departmentCode = row["DepartmentCode"].ToString();
                    departmentName = row["DepartmentName"].ToString();
                    employeeCode = row["EmployeeCode"].ToString();
                    employeeName = row["EmployeeName"].ToString();
                    employeeLevelID = row["EmployeeLevelID"].ToString();
                    if (!string.IsNullOrEmpty(row["ID"].ToString()))
                    {
                        ConsultID = int.Parse(row["ID"].ToString());//ID
                    }
                }
                SqlParameter[] sps ={
                                         new SqlParameter("@TypeID",typeId),
                                         new SqlParameter("@ConsultId",ConsultID),
                                         new SqlParameter("@consultapplysn",mconsultApplySn),
                                         new SqlParameter("@ordervalue",ordervalue),
                                         new SqlParameter("@hospitalcode",hospitalCode),
                                         new SqlParameter("@departmentcode",departmentCode),
                                         new SqlParameter("@departmentname",departmentName),
                                         new SqlParameter("@employeecode",employeeCode),
                                         new SqlParameter("@employeename",employeeName),
                                         new SqlParameter("@employeelevelid",employeeLevelID),
                                         new SqlParameter("@createuser",createUser),
                                         new SqlParameter("@createtime",createTime),
                                         new SqlParameter("@canceluser",canceluser),
                                         new SqlParameter("@modifyuser",modifyuser)
                                     };
                DS_SqlHelper.ExecuteNonQuery("EMR_CONSULTATION.usp_SaveConsultRecord", sps, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存会诊相关信息的方法
        /// Add xlb 2013-02-28
        /// </summary>
        private void SaveConsultRecord(ConsultStatus consultStatus)
        {
            try
            {
                DataTable dtSource = (DataTable)gridControlDepartment.DataSource;
                foreach (DataRow row in dtSource.Rows)
                {
                    //行当前状态为修改状态则修改该行数据
                    if (row.RowState == DataRowState.Modified)
                    {
                        SaveConsultRecordDepartment(2, row);
                    }
                    else if (row.RowState == DataRowState.Added)//行状态为新增则插入
                    {
                        SaveConsultRecordDepartment(3, row);
                    }
                    else if (row.RowState == DataRowState.Deleted)//视图中状态为删除的行
                    {
                        SaveConsultRecordDepartment(1, row);
                    }
                }
                UpdateConsultApply(consultStatus);
                //更新行状态
                dtSource.AcceptChanges();
                MessageBox.Show("保存成功");
                if (consultStatus == ConsultStatus.RecordeComplete)
                {
                    Form form = this.FindForm() as Form;
                    form.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 事件 Add xlb 2013-02-26

        /// <summary>
        /// 加序号列事件
        /// Add 项令波2013-02-26
        /// </summary>
        private void gridViewDept_CustomDrawRowIndicator(object sender,
        DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 新增事件
        /// Add xlb 2013-02-26
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = gridControlDepartment.DataSource as DataTable;
                DataRow row = dt.NewRow();
                row["DepartmentName"] = "";
                row["DepartmentCode"] = "";
                row["EMPLOYEENAMESTR"] = "";
                row["EMPLOYEECODE"] = "";
                row["EmployeeID"] = "";
                row["DeleteButton"] = "删除";
                row["EmployeeLevelName"] = "";
                row["EmployeeLevelID"] = "";
                row["EmployeeName"] = "";
                ChooseConsultEmployee chooseEmployee = new ChooseConsultEmployee(m_app, dt, row);
                if (chooseEmployee == null)
                {
                    return;
                }
                chooseEmployee.StartPosition = FormStartPosition.CenterScreen;
                if (chooseEmployee.ShowDialog() == DialogResult.OK)//保存成功刷新数据元
                {
                    dt.Rows.Add(chooseEmployee.dataRow);
                    gridControlDepartment.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 鼠标点击事件
        /// Add xlb 2013-02-27
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDept_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                GridHitInfo hit = gridViewDept.CalcHitInfo(e.X, e.Y);
                if (hit.RowHandle < 0 || hit.Column == null)
                {
                    return;
                }
                else if (hit.Column.Name == "DeleteButton")
                {
                    DataRow dataRow = gridViewDept.GetDataRow(hit.RowHandle);
                    string signin = dataRow["ISSIGNIN"].ToString();
                    if (signin == "1")
                    {
                        MessageBox.Show("已签到医师无法删除");
                        return;
                    }
                    string employeeCode = dataRow["EMPLOYEECODE"].ToString().Trim();
                    if (GetEmployeeStatus(employeeCode))
                    {
                        MessageBox.Show("该医师已完成会诊意见无法删除");
                        return;
                    }
                    else if (dataRow["EmployeeCode"].ToString().Trim().Equals(m_app))
                    {
                        MessageBox.Show("当前登录人无权限删除自己");
                        return;
                    }
                    else if (MessageBox.Show("您确定删除此条会诊信息吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    int rowIndex = hit.RowHandle;
                    DataTable dataTableSource = gridControlDepartment.DataSource as DataTable;
                    if (rowIndex >= 0 && rowIndex < dataTableSource.Rows.Count)
                    {
                        dataTableSource.DefaultView.Delete(rowIndex);
                        gridControlDepartment.DataSource = dataTableSource;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewDept.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选择需要编辑的行");
                    return;
                }
                int rowHandel = gridViewDept.FocusedRowHandle;
                DataRow row = gridViewDept.GetDataRow(rowHandel);
                if (row == null)
                {
                    MessageBox.Show("请选择需要编辑的行");
                    return;
                }
                if (row["ISSIGNIN"] != null && row["ISSIGNIN"].Equals("1"))
                {
                    MessageBox.Show("该医生已签到，无法替换");
                    return;
                }
                DataTable dt = gridControlDepartment.DataSource as DataTable;
                ChooseConsultEmployee chooseEmployee = new ChooseConsultEmployee(m_app, dt, row);
                if (chooseEmployee == null)
                {
                    return;
                }
                chooseEmployee.StartPosition = FormStartPosition.CenterParent;
                chooseEmployee.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// Add xlb 2013-02-26
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int rowHandel = gridViewDept.FocusedRowHandle;
                if (rowHandel < 0)
                {
                    MessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dataRow = gridViewDept.GetDataRow(rowHandel);
                string signin = dataRow["ISSIGNIN"].ToString();
                if (signin == "1")//表示已签到 
                {
                    MessageBox.Show("已签到医师无法删除");
                    return;
                }
                string employeeCode = dataRow["EMPLOYEECODE"].ToString().Trim();
                //受邀医师完成会诊意见则无法删除
                if (GetEmployeeStatus(employeeCode))
                {
                    MessageBox.Show("该医师已完成会诊意见无法删除");
                    return;
                }
                DialogResult dialogResult = MessageBox.Show
                ("确定删除该条会诊记录吗？", "提示", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                DataTable dt = gridControlDepartment.DataSource as DataTable;
                //删除指定行再绑定
                dt.DefaultView.Delete(rowHandel);
                gridControlDepartment.DataSource = dt;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// <Auth>Add BY XLB</Auth>
        /// <Date>2013-06-05</Date>
        /// <Purpose>校验受邀医师是否已完成会诊意见</Purpose>
        /// </summary>
        /// <param name="employeeCode">受邀医师代码</param>
        /// <return>是否不可删除false表示可删除true表示不可删除</return>
        private bool GetEmployeeStatus(string employeeCode)
        {
            try
            {
                DataTable dtCount = DS_SqlHelper.ExecuteDataTable(@"select count(*) from consultsuggestion where 
                consultapplysn=@consultApplySn and valid='1'and createuser=@createUser and state='20'",
                new SqlParameter[] { new SqlParameter("@consultApplySn", mconsultApplySn == null ? "" : mconsultApplySn), 
                new SqlParameter("@createUser", employeeCode == null ? "" : employeeCode)}, CommandType.Text);
                if (dtCount == null || dtCount.Rows.Count <= 0)
                {
                    return false;
                }
                int result = int.Parse(dtCount.Rows[0][0].ToString());
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存会诊记录信息事件
        /// Add xlb 2013-02-26
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveConsultApply_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                if (!Validate(false, ref message))
                {
                    MessageBox.Show(message);
                    return;
                }
                SaveConsultRecord(ConsultStatus.RecordeSave);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 会诊完成事件
        /// Add xlb 2013-02-26
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCompleteConsult_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                if (!Validate(true, ref message))
                {
                    MessageBox.Show(message);
                    return;
                }
                SaveConsultRecord(ConsultStatus.RecordeComplete);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}
