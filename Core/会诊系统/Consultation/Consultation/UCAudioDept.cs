using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Wordbook;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;

namespace DrectSoft.Core.Consultation
{
    /// <summary>
    /// 会诊审核人设置，放在用户控件实现
    /// ywk 2012年6月14日 13:24:19 
    /// </summary>
    public partial class UCAudioDept : DevExpress.XtraEditors.XtraUserControl
    {
        private IEmrHost m_App;
        public UCAudioDept()
        {
            InitializeComponent();
        }
        private string CreateUserID { get; set; }
        private string CreateDateTime { get; set; }
        public UCAudioDept(IEmrHost app)
            : this()
        {
            m_App = app;
            CreateUserID = app.User.Id;
            CreateDateTime = DateTime.Now.ToString();
        }
        #region 事件
        /// <summary>
        /// 设置上级医生下拉列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lookUpEditorDepartment_CodeValueChanged(object sender, EventArgs e)
        {
            ////医师受科室的影响
            lookUpEditorEmployee.CodeValue = "";
            lookUpEditorEmployee2.CodeValue = "";
            GetDoctor();
        }
        /// <summary>
        /// 设置科室负责人下拉列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lookUpDeptFuze_CodeValueChanged(object sender, EventArgs e)
        {
            ////医师受科室的影响
            lookUpEmpFuze.CodeValue = "";
            GetFuzeDoctor();
        }
        /// <summary>
        /// 设置科室负责人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            CreateDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (lookUpDeptFuze.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择科室", CustomMessageBoxKind.WarningOk);
                lookUpDeptFuze.Focus();
                return;
            }
            else if (lookUpEmpFuze.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择科室负责人", CustomMessageBoxKind.WarningOk);
                lookUpEmpFuze.Focus();
                return;
            }
            string selectsql = string.Format(" select * from  Consult_DeptAutio where  deptid='{0}' and valid='1'", lookUpDeptFuze.CodeValue);

            string operatesql = "";
            if (m_App.SqlHelper.ExecuteDataTable(selectsql).Rows.Count > 0)//已经存在（更新）
            {
                //m_App.CustomMessageBox.MessageShow("该医师已经设置");
                //return;
                //新增创建人(不进行更新此字段)和创建时间
                operatesql = string.Format(@"update Consult_DeptAutio set userid='{0}',createtime='{1}' where deptid='{2}' and valid='1'",
                    lookUpEmpFuze.CodeValue, CreateDateTime, lookUpDeptFuze.CodeValue);
            }
            else//插入
            {
                operatesql = string.Format(@"insert into Consult_DeptAutio values(seq_Consult_DeptAutio_ID.NEXTVAL,'{0}','{1}',1,'{2}','{3}')",
                    lookUpDeptFuze.CodeValue, lookUpEmpFuze.CodeValue, CreateUserID, CreateDateTime);
            }
            m_App.SqlHelper.ExecuteNoneQuery(operatesql);
            m_App.CustomMessageBox.MessageShow("设置科室负责人成功");
            InitDeptFuze();
            ResetDeptInChargePerson();
            this.lookUpDeptFuze.Focus();
        }
        /// <summary>
        /// 设定上级医师
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (lookUpEditorDepartment.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择科室", CustomMessageBoxKind.WarningOk);
                lookUpEditorDepartment.Focus();
                return;
            }
            else if (lookUpEditorEmployee.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择下级医师", CustomMessageBoxKind.WarningOk);
                lookUpEditorEmployee.Focus();
                return;
            }
            else if (lookUpEditorEmployee2.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择上级医师", CustomMessageBoxKind.WarningOk);
                lookUpEditorEmployee2.Focus();
                return;
            }
            if (lookUpEditorEmployee.CodeValue == lookUpEditorEmployee2.CodeValue)
            {
                m_App.CustomMessageBox.MessageShow("上级医师不能为本人", CustomMessageBoxKind.WarningOk);
                lookUpEditorEmployee2.Focus();
                return;
            }
            string distintsql = string.Format(@" select * from  Consult_DoctorParent where userid='{0}' and parentuserid='{1}' 
            or( userid='{1}' and parentuserid='{0}' )
                and deptid='{2}' and valid='1'", lookUpEditorEmployee.CodeValue, lookUpEditorEmployee2.CodeValue, lookUpEditorDepartment.CodeValue);
            if (m_App.SqlHelper.ExecuteDataTable(distintsql).Rows.Count > 0)//已经存在上下级对应（提示错误）
            {
                m_App.CustomMessageBox.MessageShow("上下级关系不正确");
                return;
            }
            string selectsql = string.Format(" select * from  Consult_DoctorParent where userid='{0}' and deptid='{1}' and valid='1' ", lookUpEditorEmployee.CodeValue, lookUpEditorDepartment.CodeValue);
            string operatesql = "";
            if (m_App.SqlHelper.ExecuteDataTable(selectsql).Rows.Count > 0)//已经存在（更新）
            {
                //m_App.CustomMessageBox.MessageShow("该医师已经设置");
                //return;更新时不把创建人更新掉
                CreateDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                operatesql = string.Format(@"update Consult_DoctorParent set  parentuserid='{0}',createtime='{1}' 
                where userid='{2}' and deptid='{3}' and valid='1'",
                    lookUpEditorEmployee2.CodeValue, CreateDateTime, lookUpEditorEmployee.CodeValue, lookUpEditorDepartment.CodeValue);
            }
            else//插入(加创建人和创建时间)
            {
                CreateDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                operatesql = string.Format(@"insert into Consult_DoctorParent values(seq_Consult_DoctorParent_ID.NEXTVAL,
            '{0}','{1}',1,'{2}','{3}','{4}')", lookUpEditorEmployee.CodeValue, lookUpEditorEmployee2.CodeValue, lookUpEditorDepartment.CodeValue
                                             , CreateUserID, CreateDateTime);
            }
            m_App.SqlHelper.ExecuteNoneQuery(operatesql);
            m_App.CustomMessageBox.MessageShow("设置上级医师成功");
            InitParentGird();
            ResetPreDoc();
            this.lookUpEditorDepartment.Focus();
        }
        private string DelTip = string.Empty;//用于区分要操作的是上级医生列表还是科室审核人列表

        /// <summary>
        /// 操作上级医师列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewParent_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                GridHitInfo hit = gridViewParent.CalcHitInfo(e.X, e.Y);
                if (hit.RowHandle >= 0)
                {
                    DataRow dr = gridViewParent.GetDataRow(hit.RowHandle);
                    if (dr != null)
                    {
                        string createuserID = dr["CreateUser"].ToString();
                        if (createuserID == CreateUserID)//当前操作人为此条记录的创建者则显示删除菜单 
                        {
                            barButtonItemConsultationInfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            DelTip = "Par";
                        }
                        else
                        {
                            barButtonItemConsultationInfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        }
                        this.popupMenu1.ShowPopup(this.barManager1, (Cursor.Position));
                    }
                }
            }
        }
        /// <summary>
        /// 操作负责人列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewFuZe_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                GridHitInfo hit = gridViewFuZe.CalcHitInfo(e.X, e.Y);
                if (hit.RowHandle >= 0)
                {
                    DataRow dr = gridViewFuZe.GetDataRow(hit.RowHandle);
                    if (dr != null)
                    {
                        string createuserID = dr["CreateUser"].ToString();
                        if (createuserID == CreateUserID)//当前操作人为此条记录的创建者则显示删除菜单 
                        {
                            DelTip = "Fuz";
                            barButtonItemConsultationInfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        }
                        else
                        {
                            barButtonItemConsultationInfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        }
                        this.popupMenu1.ShowPopup(this.barManager1, (Cursor.Position));
                    }
                }
            }
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCAudioDept_Load(object sender, EventArgs e)
        {
            BindPopupMenu();
            InitlookAlldept();//设置科室下拉列表数据源
            InitlookEmployee();//设置医生的下拉列表数据源
            lookUpEditorDepartment.CodeValueChanged += new EventHandler(lookUpEditorDepartment_CodeValueChanged);
            lookUpDeptFuze.CodeValueChanged += new EventHandler(lookUpDeptFuze_CodeValueChanged);

            InitParentGird();//绑定会诊审核上级医师表
            InitDeptFuze();//绑定会诊单审核负责人表
            lookUpEditorDepartment.Focus();
        }

        /// <summary>
        /// 重置 - 设置上级医师
        /// </summary>
        private void ResetPreDoc()
        {
            this.lookUpEditorDepartment.CodeValue = "";
            this.lookUpEditorEmployee.CodeValue = "";
            this.lookUpEditorEmployee2.CodeValue = "";
        }
        /// <summary>
        /// 重置 - 设置上级医师
        /// </summary>
        private void ResetDeptInChargePerson()
        {
            this.lookUpDeptFuze.CodeValue = "";
            this.lookUpEmpFuze.CodeValue = "";
        }
        #endregion

        #region 方法
        #region 绑定右键菜单
        private DevExpress.XtraBars.BarButtonItem barButtonItemConsultationInfo;
        private void BindPopupMenu()
        {
            barButtonItemConsultationInfo = new DevExpress.XtraBars.BarButtonItem();
            barButtonItemConsultationInfo.Caption = "删除";
            barButtonItemConsultationInfo.Id = 1;
            barButtonItemConsultationInfo.Name = "Delete";
            barButtonItemConsultationInfo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItemConsultationInfo_ItemClick);
            popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
                new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemConsultationInfo)
            });
            barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { 
                barButtonItemConsultationInfo 
            });
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void barButtonItemConsultationInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DelTip == "Par")//上级医生
            {
                DataRow dr = gridViewParent.GetDataRow(gridViewParent.FocusedRowHandle);
                if (dr != null)
                {
                    string id = dr["ID"].ToString();
                    string updatesql = string.Format(@"update Consult_DoctorParent set valid='0' where 
                        id='{0}'", id);
                    if (m_App.CustomMessageBox.MessageShow("确认删除上级医师？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        m_App.SqlHelper.ExecuteNoneQuery(updatesql);
                    }
                    InitParentGird();
                }
            }
            if (DelTip == "Fuz")//负责人
            {
                DataRow dr = gridViewFuZe.GetDataRow(gridViewFuZe.FocusedRowHandle);
                if (dr != null)
                {
                    string id = dr["ID"].ToString();
                    string updatesql = string.Format(@"update Consult_DeptAutio set valid='0' where 
                        id='{0}'", id);
                    if (m_App.CustomMessageBox.MessageShow("确认删除此科室负责人？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        m_App.SqlHelper.ExecuteNoneQuery(updatesql);
                    }
                    InitDeptFuze();
                }
            }
        }
        #endregion
        /// <summary>
        /// 绑定选择科室下的医生列表（上级医生）
        /// </summary>
        private void GetDoctor()
        {
            string deptid = lookUpEditorDepartment.CodeValue;
            //string Level = lookUpEditorLevel.CodeValue;
            string usersid = lookUpEditorEmployee.CodeValue;
            if (deptid == "" || usersid != "")
                return;
            else
            {
                #region 设置下级医师
                string sql = string.Format(@"select * FROM users a where a.deptid = '{0}' and a.grade is not null and a.grade <> '2004' and a.valid = '1' ", deptid);
                lookUpWindowEmployee.SqlHelper = m_App.SqlHelper;
                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);
                Dept.Columns["ID"].Caption = "医生代码";
                Dept.Columns["NAME"].Caption = "医生名称";
                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("ID", 60);
                cols.Add("NAME", 60);
                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//py//wb");
                lookUpEditorEmployee.SqlWordbook = deptWordBook;
                #endregion

                #region 设置上级医师 Modify by wwj 2013-03-05
                string sql2 = string.Format(@"SELECT * FROM users a WHERE a.deptid = '{0}' and a.grade is not null and a.grade <> '2004' and a.valid = '1'
                                             UNION
                                             SELECT * FROM users b
                                             WHERE b.grade is not null and b.grade <> '2004' and b.valid = '1'
                                             AND EXISTS (SELECT 1 FROM user2dept WHERE user2dept.userid = b.id AND user2dept.deptid = '{0}')", deptid);
                DataTable dept2 = m_App.SqlHelper.ExecuteDataTable(sql2);
                dept2.Columns["ID"].Caption = "医生代码";
                dept2.Columns["NAME"].Caption = "医生名称";
                Dictionary<string, int> cols2 = new Dictionary<string, int>();
                cols2.Add("ID", 60);
                cols2.Add("NAME", 60);
                SqlWordbook deptWordBook2 = new SqlWordbook("querybook", dept2, "ID", "NAME", cols2, "ID//NAME//py//wb");
                lookUpEditorEmployee2.SqlWordbook = deptWordBook2;
                #endregion
            }
        }

        /// <summary>
        /// 绑定科室负责人，相应科室下的负责人医生
        /// </summary>
        private void GetFuzeDoctor()
        {
            string deptid = lookUpDeptFuze.CodeValue;
            string usersid = lookUpEmpFuze.CodeValue;
            if (deptid == "" || usersid != "")
                return;
            else
            {
                #region 设置科室负责人 Modify by wwj 2013-03-05
                //筛掉护士
                string sql = string.Format(@"SELECT * FROM users a WHERE a.deptid = '{0}' and a.grade is not null and a.grade <> '2004' and a.valid = '1'
                                             UNION
                                             SELECT * FROM users b
                                             WHERE b.grade is not null and b.grade <> '2004' and b.valid = '1'
                                             AND EXISTS (SELECT 1 FROM user2dept WHERE user2dept.userid = b.id AND user2dept.deptid = '{0}')", deptid);

                lookUpWindow2.SqlHelper = m_App.SqlHelper;

                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "医生代码";
                Dept.Columns["NAME"].Caption = "医生名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 60);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//py//wb");
                lookUpEmpFuze.SqlWordbook = deptWordBook;
                #endregion
            }
        }

        /// <summary>
        /// 绑定会诊审核上级医师表
        /// </summary>
        private void InitParentGird()
        {
            string sql = string.Format(@"select A.ID,B.ID as downid,B.NAME as downuser,
            B1.id as parentuserid,
                B1.NAME as parentuser,
                C.NAME as deptname,
                 (case
                    when A.VALID = '1' then
                    '有效'
                    when A.VALID = '0' then
                    '无效'
                   end) MyValid,
                 A.CreateUser from Consult_DoctorParent A left join users B on 
             A.USERID=B.ID left join users B1 on A.PARENTUSERID=B1.ID join department C on A.DEPTID =C.ID  and A.valid='1'");
            DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
            gridViewParent.MouseDown += new MouseEventHandler(gridViewParent_MouseDown);
            gridControl1.DataSource = dt;
        }
        /// <summary>
        /// 绑定会诊单审核负责人表
        /// </summary>
        private void InitDeptFuze()
        {
            string sql = string.Format(@" 
                select A.Id,A.USERID,B.NAME as deptname,C.NAME as username,(case
               when A.VALID = '1' then
                '有效'
               when A.VALID = '0' then
                '无效'
             end) MyValid,A.CreateUser  from Consult_DeptAutio A join department  B on A.DEPTID=B.ID join users C on A.USERID=C.ID and A.valid='1'  ");//绑定审核负责人表
            DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
            gridViewFuZe.MouseDown += new MouseEventHandler(gridViewFuZe_MouseDown);
            gridControl2.DataSource = dt;
        }
        /// <summary>
        /// 绑定科室下拉列表
        /// </summary>
        private void InitlookAlldept()
        {
            string sql = @"SELECT ID, NAME,py,wb
                          FROM department /*, dept2ward dw  WHERE department.ID = dw.deptid*/
                         ORDER BY ID";
            DataTable dtDept = m_App.SqlHelper.ExecuteDataTable(sql);
            //绑定科室

            lookUpEditorDepartment.Kind = WordbookKind.Sql;
            lookUpEditorDepartment.ListWindow = lookUpWindowDepartment;

            lookUpDeptFuze.Kind = WordbookKind.Sql;
            lookUpDeptFuze.ListWindow = lookUpWindowDepartment;

            BindDepartmentWordBook(dtDept);

        }
        /// <summary>
        /// 绑定科室下拉列表
        /// </summary>
        /// <param name="dataTableData"></param>
        private void BindDepartmentWordBook(DataTable dataTableData)
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
            colWidths.Add("NAME", 90);
            SqlWordbook wordBook = new SqlWordbook("Department", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorDepartment.SqlWordbook = wordBook;
            lookUpDeptFuze.SqlWordbook = wordBook;
        }
        /// <summary>
        /// 绑定医生列表
        /// </summary>
        /// <param name="dtDoc"></param>
        private void BindEmployeeWordBook(DataTable dtDoc)
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
            colWidths.Add("ID", 60);
            colWidths.Add("NAME", 60);
            SqlWordbook wordBook = new SqlWordbook("Employee", dtDoc, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorEmployee.SqlWordbook = wordBook;
            lookUpEditorEmployee2.SqlWordbook = wordBook;
            lookUpEmpFuze.SqlWordbook = wordBook;
        }
        /// <summary>
        /// 设置医生的下拉列表数据源
        /// </summary>
        private void InitlookEmployee()
        {
            string sqlseachdoc = @" SELECT u.ID, u.NAME, u.py, u.wb
                  FROM users u  WHERE u.valid = '1'
                  ORDER BY u.ID;";
            DataTable dtDoc = m_App.SqlHelper.ExecuteDataTable(sqlseachdoc);
            lookUpEditorEmployee.Kind = WordbookKind.Sql;
            lookUpEditorEmployee.ListWindow = lookUpWindowEmployee;
            BindEmployeeWordBook(dtDoc);
        }
        #endregion

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-25</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewParent_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-25</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewFuZe_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件 - 设置上级医师
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-13</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset1_Click(object sender, EventArgs e)
        {
            try
            {
                Reset1();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件 - 设置科室负责人
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-13</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset2_Click(object sender, EventArgs e)
        {
            try
            {
                Reset2();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置方法 - 设置上级医师
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-14</date>
        /// </summary>
        private void Reset1()
        {
            try
            {
                lookUpEditorDepartment.CodeValue = string.Empty;
                lookUpEditorEmployee.CodeValue = string.Empty;
                lookUpEditorEmployee2.CodeValue = string.Empty;
                lookUpEditorDepartment.Focus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重置方法 - 设置科室负责人
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-14</date>
        /// </summary>
        private void Reset2()
        {
            try
            {
                lookUpDeptFuze.CodeValue = string.Empty;
                lookUpEmpFuze.CodeValue = string.Empty;
                lookUpDeptFuze.Focus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
