using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DrectSoft.Core.Consultation;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DrectSoft.Wordbook;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Emr.NurseCenter
{
    /// <summary>
    /// 会诊系统中用于受邀医师的签到
    /// </summary>
    public partial class DocSignInForm : DevBaseForm
    {
        #region   属性及字段
        private IEmrHost m_App;
        private string m_NoOfFirstPage;
        private string m_ConsultApplySN = string.Empty;

        /// <summary>
        /// 构造
        /// </summary>
        public DocSignInForm()
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
        /// 重载构造
        /// </summary>
        /// <param name="app"></param>
        /// <param name="noOfFirstPage"></param>
        /// <param name="consultApplySN"></param>
        public DocSignInForm(IEmrHost app, string noOfFirstPage, string consultApplySN)
        {
            try
            {
                InitializeComponent();
                m_App = app;
                m_NoOfFirstPage = noOfFirstPage;
                m_ConsultApplySN = consultApplySN;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 自定义事件 医师签到完成后立即执行会诊缴费 edit bt tj 2012-11-1 需求更改

        public delegate void delegatePay(bool isNeedTip/*是否需要提示*/, bool isNeedQuestion/*当满足条件时询问是否需要会诊缴费*/);
        public event delegatePay PayHandle;

        public void OnCompleteSignIn(object sender, EventArgs e)
        {
            try
            {
                if (PayHandle != null)
                {
                    //PayHandle(sender, null);
                    PayHandle(false, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得此病人会诊相关信息
        /// </summary>
        /// <param name="m_ConsultApplySN"></param>
        private void SetConsultInfo(string m_ConsultApplySN)
        {
            //DataTable MYdt = GetConsultInfoJoinTable(m_ConsultApplySN);

            DataSet ds = GetConsultationDataSet(m_ConsultApplySN, "20");
            DataTable dtConsultApply = ds.Tables[0];
            DataTable dtConsultApplyDepartment = ds.Tables[1];
            DataTable dtConsultApplyDepRecord = ds.Tables[2];
            if (dtConsultApply.Rows.Count > 0)
            {
                memoEditPurpose.Text = dtConsultApply.Rows[0]["Purpose"].ToString();
            }

            //if (MYdt.Rows.Count > 0)
            //{
            //    gridControlDepartment.DataSource = MYdt;
            //    gridColumn4.Visible = true;
            //    gridColumn5.Visible = true;
            //    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
            //}
            SetDoctorLook("");
            #region OLD
            DataColumn dtcol = new DataColumn("check", typeof(System.Boolean));

            dtConsultApplyDepartment.Columns.Add(dtcol);

            //if (dtConsultApplyDepartment.Rows.Count > 0)
            //{
            //    gridControlDepartment.DataSource = dtConsultApplyDepartment;
            //    gridColumn4.Visible = false;
            //    gridColumn5.Visible = false;
            //    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
            //}
            if (dtConsultApplyDepRecord.Rows.Count > 0)//签到完成，是往consultrecorddepartment表插数据
            {
                gridControlDepartment.DataSource = dtConsultApplyDepRecord;
                gridColumn4.Visible = true;
                gridColumn5.Visible = true;
                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);

            }
            //记录表和列表都有数据
            //if (dtConsultApplyDepRecord.Rows.Count > 0 && dtConsultApplyDepRecord.Rows.Count > 0)
            //{
            //    for (int d = 0; d < dtConsultApplyDepRecord.Rows.Count; d++)
            //    {
            //        for (int i = dtConsultApplyDepartment.Rows.Count - 1; i >= 0; i--)
            //        {
            //            if (dtConsultApplyDepartment.Rows[i]["ordervalue"].ToString() == dtConsultApplyDepRecord.Rows[d]["ordervalue"].ToString() && dtConsultApplyDepartment.Rows[i]["DepartmentCode"].ToString() == dtConsultApplyDepRecord.Rows[d]["DepartmentCode"].ToString() && dtConsultApplyDepartment.Rows[i]["employeelevelid"].ToString() == dtConsultApplyDepRecord.Rows[d]["employeelevelid"].ToString())
            //            {

            //                dtConsultApplyDepartment.Rows.Remove(dtConsultApplyDepartment.Rows[i]);
            //            }
            //        }
            //    }
            //    dtConsultApplyDepartment.Merge(dtConsultApplyDepRecord);
            //    gridControlDepartment.DataSource = dtConsultApplyDepartment;
            //    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
            //}
            #endregion
        }

        /// <summary>
        /// 原来的受邀医师为下拉框列的修改
        /// </summary>
        /// <param name="deptid"></param>
        private void SetDoctorLook(string deptid)
        {
            string sql = string.Empty;
            if (deptid != "")
            {
                sql = string.Format(@"select ID as EMPLOYEEID,id || '_' || name as EMPLOYEENAME ,deptid as DEPTID,grade as GRADE from users a where  a.grade is not null  and a.grade <> '2004' and (a.deptid='{0}') ", deptid);
            }
            else
            {
                sql = string.Format(@"select ID as EMPLOYEEID,id || '_' || name as EMPLOYEENAME,deptid as DEPTID,grade as GRADE  from users a where  a.grade is not null  and a.grade <> '2004' ");
            }
            DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);
            LookDoctor.ValueMember = "EMPLOYEEID";
            LookDoctor.DisplayMember = "EMPLOYEENAME";
            LookDoctor.DataSource = Dept;
        }

        /// <summary>
        /// 此方法暂不用，处理清单表和记录表，现在是在后台处理DataTable
        /// </summary>
        /// <param name="m_ConsultApplySN"></param>
        /// <returns></returns>
        private DataTable GetConsultInfoJoinTable(string m_ConsultApplySN)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ConsultApplySn", SqlDbType.Int)
             
            };
            sqlParam[0].Value = m_ConsultApplySN;
            return m_App.SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetConsultUseSign", sqlParam, CommandType.StoredProcedure);
        }


        /// <summary>
        /// 得到会诊信息
        /// </summary>
        /// <param name="noOfInpat"></param>
        /// <param name="typeID"></param>
        /// <param name="param1"></param>
        /// <returns></returns>
        public DataSet GetConsultationDataSet(string consultApplySn, string typeID)//, string param1)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ConsultApplySn", SqlDbType.Int),
                new SqlParameter("@TypeID", SqlDbType.Decimal)
            };

            if (consultApplySn.Trim() == "")
            {
                sqlParam[0].Value = 0f;
            }
            else
            {
                sqlParam[0].Value = Convert.ToDecimal(consultApplySn);
            }
            if (typeID.Trim() == "")
            {
                sqlParam[1].Value = 0f;
            }
            else
            {
                sqlParam[1].Value = Convert.ToDecimal(typeID);
            }

            return m_App.SqlHelper.ExecuteDataSet("EMR_CONSULTATION.usp_GetConsultationData", sqlParam, CommandType.StoredProcedure);
        }

        /// <summary>
        /// ###########签到操作 操作consultrecorddepartment表
        /// </summary>
        /// <param name="consultApplySn"></param>
        public void ConsultInfoSignIn(string consultapplysn, string ordervalue, string hospitalcode, string departmentcode, string departmentname, string employeecode, string employeename, string employeelevelid, string createuser, string createtime, string valid, string issignin, string reachtime, string id/*记录ID*/)
        {

            //            string serachsql = string.Format(@"select * from  consultrecorddepartment
            //             where consultapplysn='{0}'  and valid='1' and employeelevelid='{1}'  and departmentcode='{2}' and ordervalue='{3}' and id='{4}' ", consultapplysn, employeelevelid, departmentcode, ordervalue, id);//edit by wangj 2013 2 22 添加查询条件ordervalue
            //这里不能加上了资质的查询条件，因为签到时候的资质限制可以随便选择，中心医院需求更改add by ywk 2013年8月8日 13:33:18 
            string serachsql = string.Format(@"select * from  consultrecorddepartment
             where consultapplysn='{0}'  and valid='1'   and departmentcode='{1}' and ordervalue='{2}' and id='{3}' ", consultapplysn, departmentcode, ordervalue, id);//edit by wangj 2013 2 22 添加查询条件ordervalue
            string dosql = string.Empty;
            if (m_App.SqlHelper.ExecuteDataTable(serachsql, CommandType.Text).Rows.Count > 0)//存在就更新
            {
                #region  注销 by xlb  若邀请了同一科室多个同级别医师此Update会更新多个医师为同一人
                //                dosql = string.Format(@" UPDATE consultrecorddepartment
                //             SET issignin='1' ,reachtime='{0}',employeecode='{1}',employeename='{2}'  WHERE consultapplysn = '{3}'     AND valid = '1' and employeelevelid='{4}' and departmentcode='{5}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), employeecode, employeename, consultapplysn, employeelevelid, departmentcode);
                //上面的逻辑是错误的！！ edit by ywk 2012年11月10日9:42:03
                //                dosql = string.Format(@" UPDATE consultrecorddepartment
                //             SET issignin='1' ,reachtime='{0}'  WHERE consultapplysn = '{1}'   and 
                //              employeecode='{2}' and  employeename='{3}'   AND valid = '1' and employeelevelid='{4}' and departmentcode='{5}'", 
                //                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), consultapplysn, employeecode, employeename,  employeelevelid, departmentcode);

                // dosql = string.Format(@" UPDATE consultrecorddepartment
                //SET issignin='1' ,reachtime='{0}', employeecode='{1}', employeename='{2}' WHERE consultapplysn = '{3}'   
                //AND valid = '1' and employeelevelid='{4}' and departmentcode='{5}' and ordervalue='{6}'",
                //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), employeecode, employeename, consultapplysn, employeelevelid, departmentcode, ordervalue);//edit by wangj 2013 2 22 添加查询条件ordervalue
                //若邀请同一科室同一级别医师此update会更新多个医师为同一个人  
                #endregion
                dosql = string.Format(@" UPDATE consultrecorddepartment
            SET issignin='1' ,reachtime='{0}', employeecode='{1}', employeename='{2}',EMPLOYEELEVELID='{3}' WHERE consultapplysn = '{4}'   
                 AND valid = '1' and id={5} ",
              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), employeecode, employeename, employeelevelid, consultapplysn, id);//edit by wangj 2013 2 22 添加查询条件ordervalue
            }
            else//不存在就插入 
            {

                dosql = string.Format(@"INSERT INTO consultrecorddepartment
                  (ID,
                   consultapplysn,
                   ordervalue,
                   hospitalcode,
                   departmentcode,
                   departmentname,
                   employeecode,
                   employeename,
                   employeelevelid,
                   createuser,
                   createtime,
                   valid,
                   canceluser,
                   canceltime,
                    issignin,
                    reachtime
                    )
                VALUES
                  (seq_consultrecorddepartment_id.NEXTVAL,
                   '{0}',
                   '{1}',
                   '{2}',
                   '{3}',
                   '{4}',
                   '{5}',
                   '{6}',
                   '{7}',
                   '{8}',
                   '{9}',
                   '1',
                   NULL,
                   NULL,
                   '{10}',
                   '{11}'
                  )", consultapplysn, ordervalue, hospitalcode, departmentcode, departmentname, employeecode, employeename, employeelevelid, createuser, createtime, issignin, reachtime);
            }

            m_App.SqlHelper.ExecuteNoneQuery(dosql, CommandType.Text);

        }

        /// <summary>
        /// ####取消签到  操作consultrecorddepartment表
        /// </summary>
        /// <param name="consultapplysn"></param>
        /// <param name="doctorid"></param>
        private void CancelConsultInfoSignIn(string consultapplysn, string doctorid)
        {
            string updatesql = string.Format(@" UPDATE consultrecorddepartment
             SET issignin='0' ,reachtime=''  WHERE consultapplysn = '{0}'  and (employeecode='{1}' or  employeecode
            is null)     AND valid = '1' ", consultapplysn, doctorid);
            m_App.SqlHelper.ExecuteNoneQuery(updatesql, CommandType.Text);
        }

        #endregion

        #region 事件

        /// <summary>
        /// 窗体加载事件
        /// Edit by xlb 2013-03-13
        /// Add try catch by xlb 2013-03-13
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocSignInForm_Load(object sender, EventArgs e)
        {
            try
            {
                UCPatientInfoForMultiple.Init(m_NoOfFirstPage, m_App);
                SetConsultInfo(m_ConsultApplySN);

                Initlookdept();
                InitLookLevel();
                InitLookDoctor("");

                if (gridViewDept.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow foucesRow = gridViewDept.GetDataRow(gridViewDept.FocusedRowHandle);
                if (foucesRow == null)
                {
                    return;
                }
                ShowConsultInfo(foucesRow);
                if (foucesRow["issignin"].Equals("1"))
                {
                    setControlEdit(false);
                }
                else if (foucesRow["issignin"].Equals("0") || string.IsNullOrEmpty(foucesRow["issignin"].ToString()))
                {
                    setControlEdit(true);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 展示选中行信息方法
        /// </summary>
        /// <param name="dataRow"></param>
        private void ShowConsultInfo(DataRow dataRow)
        {
            try
            {
                if (dataRow == null)
                {
                    return;
                }
                lookUpEditorDepartment.CodeValue = dataRow["departmentcode"].ToString();
                lookUpEditorLevel.CodeValue = dataRow["employeelevelid"].ToString();
                lookUpEditorDoctor.CodeValue = dataRow["employeecode"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置界面控件是否可编辑
        /// </summary>
        /// <param name="isCanEdit"></param>
        private void setControlEdit(bool isCanEdit)
        {
            try
            {
                lookUpEditorDepartment.Enabled = false;
                lookUpEditorDoctor.Enabled = false;
                lookUpEditorLevel.Enabled = false;
                memoEditPurpose.Enabled = false;
                btnSignIn.Enabled = isCanEdit;
                btnCancelSign.Enabled = !isCanEdit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绑定受邀医师
        /// </summary>
        private void InitLookDoctor(string deptid)
        {
            try
            {
                string sql = string.Empty;
                if (deptid == "")
                {
                    sql = string.Format(@"select * from users where 1=1");

                }
                else
                {
                    sql = string.Format(@"select * from users where deptid='{0}'", deptid);
                }
                lookUpWindowDoctor.SqlHelper = m_App.SqlHelper;

                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "医生代码";
                Dept.Columns["NAME"].Caption = "医生名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 65);
                cols.Add("NAME", 160);


                //add by ywk  2012年12月3日10:21:34
                //如果有医生对应了两个科室或者多个科室这边应该再加上 (根据所选的科室)
                string sqlsearch = string.Format(@"select  USERID,DEPTID,WARDID  from USER2DEPT where deptid='{0}' ", deptid);
                DataTable dtuser2Dept = m_App.SqlHelper.ExecuteDataTable(sqlsearch, CommandType.Text);
                DataTable dtResultTemp = Dept.Clone();
                string splitersql = string.Empty;
                string sql1 = string.Empty;
                if (dtuser2Dept.Rows.Count > 0 && dtuser2Dept != null)
                {
                    //Modify by xlb 2013-04-02 拼接用户编号时用户编号存在字符引起异常
                    for (int i = 0; i < dtuser2Dept.Rows.Count; i++)
                    {
                        splitersql += "'" + dtuser2Dept.Rows[i]["USERID"].ToString() + "'" + ",";
                    }
                    splitersql = splitersql.Remove(splitersql.Length - 1);
                    sql1 = string.Format("select ID,NAME,PY,WB from users where id in ({0}) ", splitersql);
                    dtResultTemp = m_App.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
                }

                if (dtResultTemp != null && dtResultTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtResultTemp.Rows.Count; i++)
                    {
                        DataRow dr = Dept.NewRow();
                        dr["ID"] = dtResultTemp.Rows[i]["ID"].ToString();
                        dr["NAME"] = dtResultTemp.Rows[i]["NAME"].ToString();
                        dr["PY"] = dtResultTemp.Rows[i]["PY"].ToString();
                        dr["WB"] = dtResultTemp.Rows[i]["WB"].ToString();
                        Dept.Rows.Add(dr);
                    }
                }

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//py//wb");
                lookUpEditorDoctor.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                m_App.CustomMessageBox.MessageShow(ex.Message);
                return;
            }

        }

        /// <summary>
        /// 绑定医师资质
        /// </summary>
        private void InitLookLevel()
        {
            lookUpEditorLevel.Kind = WordbookKind.Sql;
            lookUpEditorLevel.ListWindow = lookUpWindowLevel;
            //DataTable dt =
            BindDoctorLevelWordBook(GetConsultationData("", "9", "20"));
        }

        /// <summary>
        /// 医生级别
        /// </summary>
        /// <param name="dataTableData"></param>
        private void BindDoctorLevelWordBook(DataTable dataTableData)
        {
            for (int i = 0; i < dataTableData.Columns.Count; i++)
            {
                if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "name")
                {
                    dataTableData.Columns[i].Caption = "医师级别";
                }
            }

            Dictionary<string, int> colWidths = new Dictionary<string, int>();
            //colWidths.Add("ID", 10);
            colWidths.Add("NAME", 120);
            SqlWordbook wordBook = new SqlWordbook("Director", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorLevel.SqlWordbook = wordBook;
        }

        /// <summary>
        /// 取得会诊信息
        /// </summary>
        /// <param name="noOfInpat"></param>
        /// <param name="typeID"></param>
        /// <param name="param1"></param>
        /// <returns></returns>
        public DataTable GetConsultationData(string noOfInpat, string typeID, string param1)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@NoOfInpat", SqlDbType.Decimal),
                new SqlParameter("@TypeID", SqlDbType.Decimal),
                new SqlParameter("@Param1", SqlDbType.VarChar)
            };

            if (noOfInpat.Trim() == "")
            {
                sqlParam[0].Value = 0f;
            }
            else
            {
                sqlParam[0].Value = Convert.ToDecimal(noOfInpat);
            }
            if (typeID.Trim() == "")
            {
                sqlParam[1].Value = 0f;
            }
            else
            {
                sqlParam[1].Value = Convert.ToDecimal(typeID);
            }
            sqlParam[2].Value = param1;

            return m_App.SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetConsultationData", sqlParam, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 绑定科室下拉列表
        /// </summary>
        private void Initlookdept()
        {
            //            string sql = @"SELECT ID, NAME,py,wb
            //                          FROM department, dept2ward dw
            //                         WHERE department.ID = dw.deptid
            //                         ORDER BY ID";
            ////中心医院要求捞取医技科室，此处没必要过多筛选 add by ywk 2013年4月2日9:21:16 
            //string sql = @"select * from department  where sort in('101','102','104','105') and valid='1'";
            //中心医院信息科周主任需求，此处科室只捞取包含主治医生以上级别的科室 add by ywk 2013年8月14日 09:30:27
            string sql = @"select distinct de.id,de.name,de.py,de.wb 
                from department de inner join users us on de.id=us.deptid and us.grade in ('2001','2002','2000')
                where de.sort in ('101', '102', '104', '105')
                and de.valid = '1' ";
            DataTable dtDept = m_App.SqlHelper.ExecuteDataTable(sql);
            //绑定科室

            //lookUpEditorDepartment.Kind = WordbookKind.Sql;
            //lookUpEditorDepartment.ListWindow = lookUpWindowDepartment;

            lookUpEditorDepartment.Kind = WordbookKind.Sql;
            lookUpEditorDepartment.ListWindow = lookUpWindowDepartment;

            BindDepartmentWordBook(dtDept);
        }

        /// <summary>
        /// 绑定科室下拉列表
        /// Add try catch by xlb 2013-03-13
        /// </summary>
        /// <param name="dataTableData"></param>
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
                colWidths.Add("ID", 60);
                colWidths.Add("NAME", 120);
                SqlWordbook wordBook = new SqlWordbook("Department", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = wordBook;
                //lookUpDeptFuze.SqlWordbook = wordBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 选择受邀医师事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LookDoctor_EditValueChanged(object sender, EventArgs e)
        {
            DataRow row = gridViewDept.GetDataRow(gridViewDept.FocusedRowHandle);//取得选择的行
            if (row != null)
            {
                string deptid = row["departmentcode"].ToString();
                SetDoctorLook(deptid);
            }
        }
        /// <summary>
        /// /签到
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            try
            {
                //if (lookUpEditorDoctor.EditValue == null && lookUpEditorDepartment.EditValue == null && lookUpEditorLevel.EditValue == null)
                //{
                //    m_App.CustomMessageBox.MessageShow("请在列表中选择要签到的记录！");
                //    return;
                //}
                if (lookUpEditorDoctor.EditValue.ToString() == "")
                {
                    m_App.CustomMessageBox.MessageShow("请在选择签到医生！");
                    return;
                }

                DataRow row = gridViewDept.GetDataRow(gridViewDept.FocusedRowHandle);//取得选择的行
                if (row == null)
                {
                    m_App.CustomMessageBox.MessageShow("请选择要签到的记录");
                    return;
                }
                if (row["issignin"].Equals("1"))
                {
                    m_App.CustomMessageBox.MessageShow("已签到");
                    btnSignIn.Enabled = false;
                    return;
                }
                //防止科室为空 add by ywk 2013年8月23日 14:05:38
                if (string.IsNullOrEmpty(lookUpEditorDepartment.CodeValue))
                {
                    m_App.CustomMessageBox.MessageShow("该受邀科室下无主治医生以上级别医生");
                    return;
                }


                //add by ywk 2013年7月29日 13:39:56验证资质
                if (string.IsNullOrEmpty(lookUpEditorLevel.CodeValue))
                {
                    m_App.CustomMessageBox.MessageShow("请选择受邀医生资质");
                    return;
                }
                string value = string.Empty;
                string consultapplysn = string.Empty;//单据号
                string doctorid = string.Empty;
                string issign = string.Empty;
                consultapplysn = row["consultapplysn"].ToString();
                string ordervalue = row["ordervalue"].ToString();
                string hospitalcode = row["hospitalcode"].ToString();
                string departmentcode = row["departmentcode"].ToString();
                string departmentname = row["departmentname"].ToString();
                string id = row["ID"].ToString();
                string ss = gridViewDept.GetRowCellDisplayText(gridViewDept.FocusedRowHandle, gridViewDept.Columns[4]);
                MYEMPCODE = ss.Split('_')[0].ToString();
                MYEMPNAME = ss.Split('_')[1].ToString();


                string employeecode = lookUpEditorDoctor.CodeValue;
                string employeename = lookUpEditorDoctor.EditValue.ToString();
                //签到的信息，以实际签到的那个人为准add by ywk 2013年8月8日 13:23:33
                string employeelevelid = lookUpEditorLevel.CodeValue.ToString().Trim();

                //string employeelevelid = row["employeelevelid"].ToString();
                string createuser = row["createuser"].ToString();
                string createtime = row["createtime"].ToString();
                string valid = "1";
                string issignin = "1";
                string reachtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                ConsultInfoSignIn(consultapplysn, ordervalue, hospitalcode, departmentcode, departmentname, employeecode, employeename, employeelevelid, createuser, createtime, valid, issignin, reachtime, id.Trim());
                m_App.CustomMessageBox.MessageShow("签到成功");
                setControlEdit(false);//签到成功签到按钮置灰取消签到可编辑
                gridColumn5.Visible = true;
                gridColumn4.Visible = true;
                SetConsultInfo(consultapplysn);

                //签到完成后立即打开缴费窗体
                OnCompleteSignIn(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取消签到
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelSign_Click(object sender, EventArgs e)
        {
            DataRow row = gridViewDept.GetDataRow(gridViewDept.FocusedRowHandle);//取得选择的行
            if (row == null)
            {
                m_App.CustomMessageBox.MessageShow("请选择要取消签到的记录！");
                return;
            }
            string value = string.Empty;
            string consultapplysn = string.Empty;//单据号
            string doctorid = string.Empty;
            string issign = string.Empty;

            issign = row["issignin"].ToString();

            if (issign == "0")
            {
                m_App.CustomMessageBox.MessageShow("还未签到");
                return;
            }
            consultapplysn = row["consultapplysn"].ToString();
            doctorid = row["EmployeeID"].ToString();

            CancelConsultInfoSignIn(consultapplysn, doctorid);
            m_App.CustomMessageBox.MessageShow("已经取消签到");
            setControlEdit(true);
            SetConsultInfo(consultapplysn);
        }


        #endregion

        #region OLD不用
        private DataView clone = null;
        //private void gridViewDept_ShownEditor(object sender, EventArgs e)
        //{
        //    GridView view = sender as GridView;
        //    if (view.FocusedColumn.FieldName == "EMPLOYEEID" && view.ActiveEditor is LookUpEdit)
        //    {
        //        Text = view.ActiveEditor.Parent.Name;
        //        DevExpress.XtraEditors.LookUpEdit edit;
        //        edit = (LookUpEdit)view.ActiveEditor;
        //        MYEMPCODE = edit.EditValue.ToString();
        //        DataTable table = edit.Properties.LookUpData.DataSource as DataTable;
        //        clone = new DataView(table);
        //        DataRow row = view.GetDataRow(view.FocusedRowHandle);
        //        string deptid = row["departmentcode"].ToString();
        //        if (!string.IsNullOrEmpty(row["EMPLOYEEID"].ToString()))
        //        {
        //            clone.RowFilter = "[EMPLOYEEID] = " + row["EMPLOYEEID"].ToString();
        //        }
        //        else
        //        {
        //            SetDoctorLook(deptid);
        //            clone.RowFilter = "[EMPLOYEELEVELID] = " + row["EMPLOYEELEVELID"].ToString();
        //        }
        //        edit.Properties.LookUpData.DataSource = clone;
        //        //edit.Properties  clone.ToTable().Rows[0]["EMPLOYEENAME"].ToString();
        //        edit.Properties.LookUpData.DisplayMember = "EMPLOYEENAME";
        //        edit.Properties.LookUpData.ValueMember = "EMPLOYEENAME";

        //        LookDoctor.ValueMember = "EMPLOYEENAME";
        //        //edit.SelectedText = clone.ToTable().Rows[0]["EMPLOYEENAME"].ToString();
        //        //edit.Focus();
        //        //        edit.EditValue = clone.ToTable().Rows[0]["EMPLOYEEID"].ToString();
        //    }

        //}

        private void gridViewDept_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (view.FocusedColumn.FieldName == "EMPLOYEEID" && view.ActiveEditor is LookUpEdit)
            //{
            //    Text = view.ActiveEditor.Parent.Name;
            //    DevExpress.XtraEditors.LookUpEdit edit;
            //    edit = (LookUpEdit)view.ActiveEditor;
            //    MYEMPCODE = edit.EditValue.ToString();
            //    DataTable table = edit.Properties.LookUpData.DataSource as DataTable;
            //    clone = new DataView(table);
            //    DataRow row = view.GetDataRow(view.FocusedRowHandle);
            //    string deptid = row["departmentcode"].ToString();
            //    if (!string.IsNullOrEmpty(row["EMPLOYEEID"].ToString()))
            //    {
            //        //clone.RowFilter = "[DEPTID] = " + '"+row["DEPARTMENTCODE"].ToString()+"';
            //        clone.RowFilter = string.Format(@"[DEPTID] = '{0}'", row["DEPARTMENTCODE"].ToString());
            //    }
            //    else
            //    {
            //        SetDoctorLook(deptid);
            //        //clone.RowFilter = "[EMPLOYEELEVELID] = " + row["EMPLOYEELEVELID"].ToString();
            //        clone.RowFilter = string.Format(@"[GRADE] = '{0}'", row["EMPLOYEELEVELID"].ToString());
            //    }
            //    LookDoctor.ValueMember = "EMPLOYEEID";
            //    LookDoctor.DisplayMember = "EMPLOYEENAME";
            //    LookDoctor.DataSource = clone;
            //}

        }
        string MYEMPCODE = string.Empty;
        string MYEMPNAME = string.Empty;
        private void gridControlDepartment_MouseDown(object sender, MouseEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (view.FocusedColumn.FieldName == "EMPLOYEEID" && view.ActiveEditor is LookUpEdit)
            //{
            //    Text = view.ActiveEditor.Parent.Name;
            //    DevExpress.XtraEditors.LookUpEdit edit;
            //    edit = (LookUpEdit)view.ActiveEditor;
            //    MYEMPCODE = edit.EditValue.ToString();
            //    DataTable table = edit.Properties.LookUpData.DataSource as DataTable;
            //    clone = new DataView(table);
            //    DataRow row = view.GetDataRow(view.FocusedRowHandle);
            //    string deptid = row["departmentcode"].ToString();
            //    if (!string.IsNullOrEmpty(row["EMPLOYEEID"].ToString()))
            //    {
            //        //clone.RowFilter = "[DEPTID] = " + '"+row["DEPARTMENTCODE"].ToString()+"';
            //        clone.RowFilter = string.Format(@"[DEPTID] = '{0}'", row["DEPARTMENTCODE"].ToString());
            //    }
            //    else
            //    {
            //        SetDoctorLook(deptid);
            //        //clone.RowFilter = "[EMPLOYEELEVELID] = " + row["EMPLOYEELEVELID"].ToString();
            //        clone.RowFilter = string.Format(@"[GRADE] = '{0}'", row["EMPLOYEELEVELID"].ToString());
            //    }
            //    LookDoctor.ValueMember = "EMPLOYEEID";
            //    LookDoctor.DisplayMember = "EMPLOYEENAME";
            //    LookDoctor.DataSource = clone;
            //}


            GridHitInfo hit = gridViewDept.CalcHitInfo(e.X, e.Y);

            if (hit.RowHandle >= 0)
            {
                DataRow dataRow = gridViewDept.GetDataRow(hit.RowHandle);
                string deptid = dataRow["departmentcode"].ToString();//
                string level = dataRow["employeelevelid"].ToString();
                SetDoctorLook(deptid);
            }
        }

        private void gridViewDept_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //DataRow row = gridViewDept.GetDataRow(gridViewDept.FocusedRowHandle);//取得选择的行
            //MYEMPCODE = row["employeecode"].ToString();
        }

        private void LookDoctor_Click(object sender, EventArgs e)
        {

        }

        #endregion

        /// <summary>
        /// 点击列表赋值
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewDept.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow foucesRow = gridViewDept.GetDataRow(gridViewDept.FocusedRowHandle);

                if (foucesRow == null)
                {
                    return;
                }
                string deptid = foucesRow["departmentcode"].ToString();
                string doclevel = foucesRow["employeelevelid"].ToString();
                string doctorid = foucesRow["employeecode"].ToString();
                //lookUpEditorDepartment.EditValue = deptid;
                lookUpEditorDepartment.CodeValue = deptid;

                lookUpEditorLevel.CodeValue = doclevel;

                lookUpEditorDepartment.Enabled = false;
                lookUpEditorDoctor.Enabled = true;
                //中心醫院要求，都可以去簽到 add by 楊偉康 2013年8月7日 17:07:29
                lookUpEditorLevel.Enabled = true;
                //lookUpEditorLevel.Enabled = false;
                //此处应该再将资质条件带进去查询add by ywk 2013年7月26日 09:56:12
                InitLookDoctor(deptid, doclevel);
                lookUpEditorDoctor.CodeValue = doctorid;
                if (foucesRow["issignin"].Equals("1"))
                {
                    btnSignIn.Enabled = false;
                    btnCancelSign.Enabled = true;
                }
                else if (foucesRow["issignin"].Equals("0") || string.IsNullOrEmpty(foucesRow["issignin"].ToString()))
                {
                    btnCancelSign.Enabled = false;
                    btnSignIn.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 点击列表应该将资质带出来
        /// add by ywk 
        /// 2013年7月26日 09:57:00
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="doclevel"></param>
        private void InitLookDoctor(string deptid, string doclevel)
        {
            try
            {
                string sql = string.Empty;
                if (deptid == "")
                {
                    sql = string.Format(@"select ID,NAME,PY,WB from users where 1=1");

                }
                else
                {
                    sql = string.Format(@"select ID,NAME,PY,WB from users where deptid='{0}' and  grade='{1}'", deptid, doclevel);
                }
                lookUpWindowDoctor.SqlHelper = m_App.SqlHelper;

                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "医生代码";
                Dept.Columns["NAME"].Caption = "医生名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 65);
                cols.Add("NAME", 160);


                //add by ywk  2012年12月3日10:21:34
                //如果有医生对应了两个科室或者多个科室这边应该再加上 (根据所选的科室)
                string sqlsearch = string.Format(@"select  USERID,DEPTID,WARDID  from USER2DEPT where deptid='{0}' ", deptid);
                DataTable dtuser2Dept = m_App.SqlHelper.ExecuteDataTable(sqlsearch, CommandType.Text);
                DataTable dtResultTemp = Dept.Clone();
                string splitersql = string.Empty;
                string sql1 = string.Empty;
                if (dtuser2Dept.Rows.Count > 0 && dtuser2Dept != null)
                {
                    //Modify by xlb 2013-04-02 拼接用户编号时用户编号存在字符引起异常
                    for (int i = 0; i < dtuser2Dept.Rows.Count; i++)
                    {
                        splitersql += "'" + dtuser2Dept.Rows[i]["USERID"].ToString() + "'" + ",";
                    }
                    splitersql = splitersql.Remove(splitersql.Length - 1);
                    sql1 = string.Format("select ID,NAME,PY,WB from users where id in ({0}) and  grade='{1}'", splitersql, doclevel);
                    dtResultTemp = m_App.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
                }

                if (dtResultTemp != null && dtResultTemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtResultTemp.Rows.Count; i++)
                    {
                        DataRow dr = Dept.NewRow();
                        dr["ID"] = dtResultTemp.Rows[i]["ID"].ToString();
                        dr["NAME"] = dtResultTemp.Rows[i]["NAME"].ToString();
                        dr["PY"] = dtResultTemp.Rows[i]["PY"].ToString();
                        dr["WB"] = dtResultTemp.Rows[i]["WB"].ToString();
                        Dept.Rows.Add(dr);
                    }
                }
                DataView dv = new DataView(Dept);                           //虚拟视图吧，我这么认为
                DataTable dtresu = dv.ToTable(true, "ID", "NAME", "PY", "WB");

                SqlWordbook deptWordBook = new SqlWordbook("querybook", dtresu, "ID", "NAME", cols, "ID//NAME//py//wb");

                lookUpEditorDoctor.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                m_App.CustomMessageBox.MessageShow(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 列表选中行改变触发事件
        /// Edit by xlb 2013-03-13
        /// Add try catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDept_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                Initlookdept();
                InitLookLevel();
                InitLookDoctor("");

                DataRow foucesRow = gridViewDept.GetDataRow(gridViewDept.FocusedRowHandle);
                if (foucesRow == null)
                    return;
                string deptid = foucesRow["departmentcode"].ToString();
                string doclevel = foucesRow["employeelevelid"].ToString();
                string doctorid = foucesRow["employeecode"].ToString();
                lookUpEditorDepartment.EditValue = deptid;
                lookUpEditorDepartment.CodeValue = deptid;

                lookUpEditorLevel.CodeValue = doclevel;

                InitLookDoctor(deptid);
                lookUpEditorDoctor.CodeValue = doctorid;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 選擇受邀醫生資質，根據資質和科室撈取醫生列表
        /// add by ywk  2013年8月7日 17:19:15
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorLevel_CodeValueChanged(object sender, EventArgs e)
        {
            this.lookUpEditorDoctor.CodeValue = "";
            InitLookDoctor(this.lookUpEditorDepartment.CodeValue, lookUpEditorLevel.CodeValue);


        }
    }
}
