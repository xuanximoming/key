using Consultation.NEW;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DrectSoft.Core.Consultation.NEW
{
    /// <summary>
    /// 选择受邀医师界面
    /// Add by xlb 2013-03-11
    /// </summary>
    public partial class ChooseConsultEmployee : DevBaseForm
    {
        public DataRow dataRow = null;
        DataTable dtEmployee;//受邀医师列表
        IEmrHost m_app;
        bool isNeedCheck;//是否需要校验记录已重复

        #region 方法 Add by xlb 2013-03-11

        public ChooseConsultEmployee()
        {
            try
            {
                InitializeComponent();
                if (!DesignMode)
                {
                    DS_SqlHelper.CreateSqlHelper();
                    RegisterEvent();
                    InitDepartMent();
                    InitDoctorLevel();
                    InitDoctor(lookUpEditorDepartment.CodeValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 构造函数重载
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="row"></param>
        public ChooseConsultEmployee(IEmrHost host/*接口*/, DataTable dt/*受邀列表*/, DataRow row/*单条记录*/)
            : this()
        {
            try
            {
                dataRow = row;
                dtEmployee = dt;
                m_app = host;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化受邀科室列表
        /// Add by xlb 2013-03-11
        /// </summary>
        private void InitDepartMent()
        {
            try
            {
                lookUpEditorDepartment.Kind = WordbookKind.Sql;
                lookUpEditorDepartment.ListWindow = lookUpWindowDepartment;
                //                string sqlInitdepart = @"SELECT distinct ID, NAME, PY, WB
                //                                       FROM department, dept2ward
                //                                      WHERE department.ID = dept2ward.deptid and
                //                                      department.valid = '1' AND valid = '1'";
                //中心医院要求捞取医技科室，此处没必要过多筛选 add by ywk 2013年4月2日9:21:16 
                //string sqlInitdepart = @"select * from department  where sort in('101','102','104','105') and valid='1'";
                //中心医院信息科周主任需求，此处科室只捞取包含主治医生以上级别的科室 add by ywk 2013年8月14日 09:30:27
                string sqlInitdepart = @"select distinct de.id,de.name,de.py,de.wb 
                from department de inner join users us on de.id=us.deptid and us.grade in ('2001','2002','2000')
                where de.sort in ('101', '102', '104', '105')
                and de.valid = '1' ";
                DataTable dtDepartment = DS_SqlHelper.ExecuteDataTable(sqlInitdepart, CommandType.Text);
                for (int i = 0; i < dtDepartment.Columns.Count; i++)
                {
                    if (dtDepartment.Columns[i].ColumnName.ToUpper().Trim() == "ID")
                    {
                        dtDepartment.Columns[i].Caption = "科室代码";
                    }
                    else if (dtDepartment.Columns[i].ColumnName.ToUpper().Trim() == "NAME")
                    {
                        dtDepartment.Columns[i].Caption = "科室名称";
                    }
                }
                Dictionary<string, int> colWidths = new Dictionary<string, int>();
                colWidths.Add("ID", 60);
                colWidths.Add("NAME", 70);
                SqlWordbook wordBook = new SqlWordbook("Department", dtDepartment, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = wordBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化指定医师级别
        /// Add by xlb 2013-03-11
        /// </summary>
        private void InitDoctorLevel()
        {
            try
            {
                lookUpEditorDocLevel.Kind = WordbookKind.Sql;
                lookUpEditorDocLevel.ListWindow = lookUpWindowLevel;
                string value = ConsultCommon.GetConfigKey("ConsultDocLevel");//根据配置抓取指定级别
                string[] levelList = value.Split(',');
                if (levelList == null || levelList.Length <= 1)
                {
                    throw new Exception("会诊受邀级别配置项出错");
                }
                string str = "(";
                for (int i = 0; i < levelList.Length; i++)
                {
                    str += levelList[i] + ",";
                }
                str += "'')";
                string sqlForDocLevel = @"SELECT ID, NAME, PY, WB FROM categorydetail cd
                                        WHERE cd.ID IN " + str + "";

                DataTable dtDocLevel = DS_SqlHelper.ExecuteDataTable(sqlForDocLevel, CommandType.Text);
                if (dtDocLevel == null || dtDocLevel.Rows.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i < dtDocLevel.Columns.Count; i++)
                {
                    if (dtDocLevel.Columns[i].ColumnName.ToUpper().Trim() == "NAME")
                    {
                        dtDocLevel.Columns[i].Caption = "医生级别";
                    }
                }
                Dictionary<string, int> colWidths = new Dictionary<string, int>();
                colWidths.Add("NAME", 130);
                SqlWordbook wordBook = new SqlWordbook("Director", dtDocLevel, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
                lookUpEditorDocLevel.SqlWordbook = wordBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                lookUpEditorDepartment.CodeValueChanged += new EventHandler(lookUpEditorDepartment_CodeValueChanged);
                lookUpEditorDocLevel.CodeValueChanged += new EventHandler(lookUpEditorDocLevel_CodeValueChanged);
                lookUpEditorEmployee.CodeValueChanged += new EventHandler(lookUpEditorEmployee_CodeValueChanged);
                btnSave.Click += new EventHandler(btnSave_Click);
                btnClose.Click += new EventHandler(btnClose_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取受邀医师
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="deptId"></param>
        private void InitDoctor(string deptId)
        {
            try
            {

                lookUpEditorEmployee.Kind = WordbookKind.Sql;
                lookUpEditorEmployee.ListWindow = lookUpWindowEmployee;
                //抓取当前科室配置项里指定级别的医师
                string value = ConsultCommon.GetConfigKey("ConsultDocLevel");
                string[] levelList = value.Split(',');
                if (levelList == null || levelList.Length <= 1)
                {
                    throw new Exception("会诊受邀级别配置项出错");
                }
                string str = "(";
                for (int i = 0; i < levelList.Length; i++)
                {
                    str += levelList[i] + ",";
                }
                str += "'')";
                //Modify by xlb 2013-06-06 窜科室查询
                string sql = @"SELECT ID, NAME, PY, WB,grade FROM users u WHERE deptid =@deptid AND valid = '1' 
                             and grade in " + str +
                "union SELECT ID, NAME, PY, WB,grade FROM users u WHERE  valid = '1'" +
                            " and grade in " + str + " and exists (SELECT 1 FROM user2dept WHERE user2dept.userid = u.id and user2dept.deptid =@deptid ) ";
                SqlParameter[] sps = { 
                                         new SqlParameter("@deptid",deptId)
                                     };
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToUpper().Trim() == "ID")
                    {
                        dt.Columns[i].Caption = "代码";
                    }
                    else if (dt.Columns[i].ColumnName.ToUpper().Trim() == "NAME")
                    {
                        dt.Columns[i].Caption = "名称";
                    }
                }
                Dictionary<string, int> colWidths = new Dictionary<string, int>();
                colWidths.Add("ID", 60);
                colWidths.Add("NAME", 70);
                SqlWordbook wordBook = new SqlWordbook("ApplyDoctor", dt, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
                lookUpEditorEmployee.SqlWordbook = wordBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 校验数据方法
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool ValidateData(bool isNeedVerity, ref string message)
        {
            try
            {
                if (string.IsNullOrEmpty(lookUpEditorDepartment.CodeValue))
                {
                    message = "请选择受邀科室";
                    lookUpEditorDepartment.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(lookUpEditorDocLevel.CodeValue))
                {
                    message = "请选择受邀级别";
                    lookUpEditorDocLevel.Focus();
                    return false;
                }
                else if (!string.IsNullOrEmpty(lookUpEditorEmployee.Text.Trim())
                    && string.IsNullOrEmpty(lookUpEditorEmployee.CodeValue))//控制受邀医师框输入符合
                {
                    message = "列表不存在该医师";
                    lookUpEditorEmployee.Focus();
                    return false;
                }
                else if (lookUpEditorEmployee.CodeValue.Equals(m_app.User.Id))
                {
                    message = "当前登录人不能邀请自己";
                    lookUpEditorEmployee.Focus();
                    return false;
                }
                else if (dtEmployee == null || dtEmployee.Rows.Count <= 0)
                {
                    return true;//列表没数据时不需校验
                }
                for (int i = 0; i < dtEmployee.Rows.Count; i++)
                {
                    string employeeId = string.Empty;
                    //删除状态行则取原始受邀人ID
                    if (dtEmployee.Rows[i].RowState == DataRowState.Deleted)
                    {
                        employeeId = dtEmployee.Rows[i]["EMPLOYEECODE", DataRowVersion.Original].ToString().Trim();
                    }
                    else
                    {
                        employeeId = dtEmployee.Rows[i]["EMPLOYEECODE"].ToString().Trim();
                    }
                    //同医生不能反复邀请 受邀人可不具体到人
                    if (employeeId.Equals(lookUpEditorEmployee.CodeValue) && !string.IsNullOrEmpty(lookUpEditorEmployee.CodeValue) && isNeedVerity)
                    {
                        message = "已邀请该医生";
                        //焦点定在重复的记录上
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 事件 Add by xlb 2013-03-11

        /// <summary>
        /// 科室联动事件
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorDepartment_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                InitDoctor(lookUpEditorDepartment.CodeValue);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                if (!ValidateData(isNeedCheck/*新增需要校验记录是否重复*/, ref message))
                {
                    MessageBox.Show(message);
                    return;
                }
                //如果只选了科室和资质，就应该判断这个科室下是不是有这个资质的医生！！add by ywk 2013年7月26日 10:06:11
                ///二次修改，一个医生对应多个科室，确实不属于当前科室的情况
                ///这个判断就不要咯 add by ywk 2013年8月15日 09:25:01
                //string sql = string.Format("select  *from users where deptid='{0}' and grade='{1}'", lookUpEditorDepartment.CodeValue,
                //    lookUpEditorDocLevel.CodeValue);
                //DataTable dt = new DataTable();
                //dt = YD_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                //if (dt.Rows.Count == 0)
                //{

                //    MessageBox.Show("此科室下并无此资质医生，请重新选择!");
                //    return;
                //}

                dataRow["DepartmentName"] = lookUpEditorDepartment.Text;
                dataRow["DepartmentCode"] = lookUpEditorDepartment.CodeValue;
                dataRow["EmployeeLevelName"] = lookUpEditorDocLevel.Text;
                dataRow["EmployeeLevelID"] = lookUpEditorDocLevel.CodeValue;
                dataRow["DeleteButton"] = "删除";
                dataRow["EMPLOYEECODE"] = lookUpEditorEmployee.CodeValue;
                dataRow["EmployeeName"] = lookUpEditorEmployee.Text;
                dataRow["EmployeeNameStr"] = lookUpEditorEmployee.CodeValue == "" ? "" : lookUpEditorEmployee.CodeValue + "_" + lookUpEditorEmployee.Text;
                MessageBox.Show("保存成功");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭窗体事件
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MyMessageBox.Show("确定关闭窗体吗？", "提示", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 医师级别切换触发的事件
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorDocLevel_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                lookUpEditorEmployee.CodeValueChanged -= new EventHandler(lookUpEditorEmployee_CodeValueChanged);
                lookUpEditorEmployee.CodeValue = "";
                lookUpEditorEmployee.Kind = WordbookKind.Sql;
                lookUpEditorEmployee.ListWindow = lookUpWindowEmployee;
                if (string.IsNullOrEmpty(lookUpEditorDocLevel.CodeValue))
                {
                    InitDoctor(lookUpEditorDepartment.CodeValue);
                    return;
                }
                string sql = @"SELECT ID, NAME, PY, WB,grade FROM users u WHERE deptid =@deptid AND valid = '1' 
                                             and grade=@doctorLevel  " +
                                            "union SELECT ID, NAME, PY, WB,grade FROM users u WHERE  valid = '1'" +
                                            " and grade=@doctorLevel " +
                "and exists (SELECT 1 FROM user2dept WHERE user2dept.userid = u.id and user2dept.deptid =@deptid ) ";
                SqlParameter[] sps = { new SqlParameter("@doctorLevel", lookUpEditorDocLevel.CodeValue),
                                                      new SqlParameter("@deptid",lookUpEditorDepartment.CodeValue)
                                                     };
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                if (dt == null || dt.Columns.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToUpper().Trim() == "ID")
                    {
                        dt.Columns[i].Caption = "代码";
                    }
                    else if (dt.Columns[i].ColumnName.ToUpper().Trim() == "NAME")
                    {
                        dt.Columns[i].Caption = "名称";
                    }
                }
                Dictionary<string, int> colWidths = new Dictionary<string, int>();
                colWidths.Add("ID", 44);
                colWidths.Add("NAME", 60);
                SqlWordbook wordBook = new SqlWordbook("applydoctor", dt, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
                lookUpEditorEmployee.SqlWordbook = wordBook;
                lookUpEditorEmployee.CodeValueChanged += new EventHandler(lookUpEditorEmployee_CodeValueChanged);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 受邀医师切换联动事件
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorEmployee_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                string sql = "select grade from users where id=@userId";
                SqlParameter[] sps = { new SqlParameter("@userId", lookUpEditorEmployee.CodeValue) };
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return;
                }
                string valueLevel = dt.Rows[0][0].ToString();
                if (string.IsNullOrEmpty(valueLevel))
                {
                    return;
                }
                lookUpEditorDocLevel.CodeValueChanged -= new EventHandler(lookUpEditorDocLevel_CodeValueChanged);
                lookUpEditorDocLevel.CodeValue = valueLevel;//赋编码前取消联动事件赋值后再注册
                lookUpEditorDocLevel.CodeValueChanged += new EventHandler(lookUpEditorDocLevel_CodeValueChanged);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseConsultEmployee_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(dataRow["DepartmentCode"].ToString()))
                {
                    this.Text = "新增";
                    isNeedCheck = true;//需要校验记录重复
                }
                else
                {
                    this.Text = "编辑";//无需校验
                    isNeedCheck = false;
                }
                lookUpEditorDepartment.CodeValue = dataRow["DepartmentName"] == null ? "" : dataRow["DepartmentCode"].ToString();
                lookUpEditorDocLevel.CodeValue = dataRow["EmployeeLevelID"] == null ? "" : dataRow["EmployeeLevelID"].ToString();
                lookUpEditorEmployee.CodeValue = dataRow["EMPLOYEECODE"] == null ? "" : dataRow["EMPLOYEECODE"].ToString();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}