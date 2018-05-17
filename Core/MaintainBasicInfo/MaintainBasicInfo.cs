using DevExpress.XtraEditors;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Library;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.MaintainBasicInfo
{
    /// <summary>
    /// 基本信息维护主窗体
    /// add by wyt 2012-08-27
    /// </summary>
    public partial class MaintainBasicInfo : XtraForm, IStartPlugIn
    {
        IEmrHost m_app;
        DataProcess m_dataprocess;
        //private string m_currentUserID = "";
        //private string m_currentDeptID = "";
        //private string m_currentWardID = "";
        public enum OperState
        {
            /// <summary>
            /// 新增
            /// </summary>
            ADD,
            /// <summary>
            /// 编辑
            /// </summary>
            EDIT,
            /// <summary>
            /// 删除
            /// </summary>
            DEL,
            VIEW
        }
        public enum OperType
        {
            USER,   //操作用户
            DEPT,   //操作科室
            WARD,   //操作病区
            OTHER
        }
        OperState m_state = new OperState();
        OperType m_type = new OperType();

        public MaintainBasicInfo()
        {
            InitializeComponent();
            InitSkinGallery();
        }

        void InitSkinGallery()
        {

        }

        #region IStartPlugIn 成员
        public IPlugIn Run(FrameWork.WinForm.Plugin.IEmrHost host)
        {

            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;

            return plg;
        }
        #endregion

        #region UI上lue的数据源赋值



        private void MaintainBasicInfo_Load(object sender, EventArgs e)
        {
            InitLueDept();
            InitLueSex();
            InitMarital();
            InitUserData();
            InitDeptWardData();
            InitDeptSort();
            InitDept2Ward();
            this.dateBirthDay.DateTime = DateTime.Now;
            m_dataprocess = new DataProcess(m_app);
        }
        /// <summary>
        /// 病人性别
        /// </summary>
        private void InitLueSex()
        {
            if (lueSex.SqlWordbook == null)
                BindLueData(lueSex, 2);
        }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        private void InitMarital()
        {
            if (lueMarital.SqlWordbook == null)
                BindLueData(lueMarital, 3);
        }
        /// <summary>
        /// 初始化科室分类
        /// </summary>
        private void InitDeptSort()
        {
            comboBoxEditDeptSort.SelectedIndex = 4;
        }
        /// <summary>
        /// 初始化用户信息
        /// </summary>
        private void InitUserData()
        {
            DataTable dataTable = m_app.SqlHelper.ExecuteDataTable("usp_selectuserinfo", CommandType.StoredProcedure);
            this.gridControlUser.DataSource = dataTable;
            this.labelControlTip.Text = "共-" + dataTable.Rows.Count.ToString() + "条记录";
        }


        /// <summary>
        /// 初始科室下拉框
        /// </summary>
        private void InitLueDept()
        {
            this.lookUpWindowDept.SqlHelper = m_app.SqlHelper;
            this.lueWard.ListWindow = this.lookUpWindowDept;

            DataTable dataTable = GetEditroData(32);
            //dataTable.Columns.Add("ID");
            //dataTable.Columns.Add("NAME");
            //dataTable.Columns.Add("PY");
            //dataTable.Columns.Add("WB");
            dataTable.Columns["ID"].Caption = "编号";
            dataTable.Columns["NAME"].Caption = "名称";
            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("ID", 40);
            columnwidth.Add("NAME", 80);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "NAME", columnwidth, "ID//NAME//PY//WB");

            lueDept.SqlWordbook = sqlWordBook;
            lueDept.CodeValue = "2401";
            //BindWardData();
        }
        /// <summary>
        /// 根据选中科室加载病区下拉框
        /// </summary>
        /// <param></param>
        private void BindWardData()
        {
            lookUpWindowWard.SqlHelper = m_app.SqlHelper;
            this.lueWard.ListWindow = this.lookUpWindowWard;

            DataTable dtward = new DataTable();
            dtward.Columns.Add("ID");
            dtward.Columns.Add("NAME");
            dtward.Columns.Add("PY");
            dtward.Columns.Add("WB");
            dtward.Columns["ID"].Caption = "编号";
            dtward.Columns["NAME"].Caption = "名称";
            string id = this.lueDept.CodeValue;
            string sql = "select dept2ward.wardid id from dept2ward where dept2ward.deptid = '" + id + "'";
            DataTable dtwardid = m_app.SqlHelper.ExecuteDataTable(sql);
            if (dtwardid.Rows.Count != 0)
            {
                for (int i = 0; i < dtwardid.Rows.Count; i++)
                {
                    string sql1 = "select id,name,py,wb from ward where ward.id = '" + dtwardid.Rows[i][0].ToString() + "'";
                    DataTable wardinfo = m_app.SqlHelper.ExecuteDataTable(sql1);
                    if (wardinfo.Rows.Count != 0)
                    {
                        foreach (DataRow dr in wardinfo.Rows)
                        {
                            dtward.ImportRow(dr);
                        }
                    }
                }
            }
            Dictionary<string, int> col = new Dictionary<String, Int32>();
            col.Add("ID", 40);
            col.Add("NAME", 80);
            SqlWordbook sqlward = new SqlWordbook("ID", dtward, "ID", "NAME", col, "ID//NAME//PY//WB");
            lueWard.SqlWordbook = sqlward;
        }

        private void lueDept_CodeValueChanged(object sender, EventArgs e)
        {
            BindWardData();
        }
        #endregion

        #region 绑定LUE
        private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_app.SqlHelper;
            DataTable dataTable = GetEditroData(queryType);
            dataTable.Columns["NAME"].Caption = "名称";
            //dataTable.Columns.Add("名称");
            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("NAME", lueInfo.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "NAME", columnwidth, true);

            lueInfo.SqlWordbook = sqlWordBook;
            lueInfo.ListWindow = lupInfo;
        }

        /// <summary>
        /// 获取lue的数据源
        /// </summary>
        /// <param name="queryType"></param>
        /// <returns></returns>
        private DataTable GetEditroData(Decimal queryType)
        {
            SqlParameter paraType = new SqlParameter("@QueryType", SqlDbType.Decimal);
            paraType.Value = queryType;
            SqlParameter[] paramCollection = new SqlParameter[] { paraType };
            DataTable dataTable = m_app.SqlHelper.ExecuteDataTable("usp_GetLookUpEditorData", paramCollection, CommandType.StoredProcedure);
            return dataTable;
        }

        #endregion

        /// <summary>
        /// 科室和病区
        /// </summary>
        private void InitDeptWardData()
        {
            #region 注释 by wyt 2012-11-12 从存储过程修改
            //int n = dept.Rows.Count;
            //if (n != 0)
            //{
            //    deptnew.Columns["SORT"].DataType = typeof(string);
            //    for (int i = 0; i < n; i++)
            //    {
            //        DataRow dr = deptnew.NewRow();
            //        dr["ID"] = dept.Rows[i]["ID"];
            //        dr["NAME"] = dept.Rows[i]["NAME"];
            //        dr["PY"] = dept.Rows[i]["PY"];
            //        dr["WB"] = dept.Rows[i]["WB"];

            //if (dept.Rows[i]["SORT"].ToString().Equals("101"))
            //{
            //    dr["SORT"] = "临床";
            //}
            //else if (dept.Rows[i]["SORT"].ToString().Equals("102"))
            //{
            //    dr["SORT"] = "医技";
            //}
            //else if (dept.Rows[i]["SORT"].ToString().Equals("103"))
            //{
            //    dr["SORT"] = "药剂";
            //}
            //else if (dept.Rows[i]["SORT"].ToString().Equals("104"))
            //{
            //    dr["SORT"] = "机关";
            //}
            //else if (dept.Rows[i]["SORT"].ToString().Equals("105"))
            //{
            //    dr["SORT"] = "其他";
            //}

            //        deptnew.Rows.Add(dr);
            //    }
            //}
            #endregion
            DataTable dept = GetEditroData(32);
            this.gridControlDept.DataSource = dept;
            this.labelControl1TipDept.Text = "共" + dept.Rows.Count.ToString() + "条记录";

            DataTable ward = GetEditroData(33);
            this.gridControlWard.DataSource = ward;
            this.labelControl1Ward.Text = "共" + ward.Rows.Count.ToString() + "条记录";
        }

        /// <summary>
        /// 科室和病区对应表
        /// </summary>
        private void InitDept2Ward()
        {
            DataTable dataTable = GetEditroData(31);
            this.gridControlDept2Ward.DataSource = dataTable;
            this.labelControl1DeptWard.Text = "共" + dataTable.Rows.Count.ToString() + "条记录";
        }

        private void InitUserView()
        {
            this.textEditUserID.Text = "";
            this.txtName.Text = "";
            this.lueSex.Text = "";
            this.textEditID.Text = "";
            this.dateBirthDay.DateTime = DateTime.Now;
            this.lueMarital.Text = "";
            this.lueDept.CodeValue = DS_Common.currentUser.CurrentDeptId;
            this.lueWard.CodeValue = DS_Common.currentUser.CurrentWardId;
            //DataTable user = this.gridControlUser.DataSource as DataTable;
            //user.DefaultView.RowFilter = "";
        }

        private void InitDeptView()
        {
            this.textEditDeptID.Text = "";
            this.textEditDeptName.Text = "";
            this.comboBoxEditDeptSort.SelectedIndex = 4;
            //DataTable dept = this.gridControlDept.DataSource as DataTable;
            //dept.DefaultView.RowFilter = "";
        }

        private void InitWardView()
        {
            this.textEditWardID.Text = "";
            this.textEditWardName.Text = "";
            //DataTable ward1 = this.gridControlWard.DataSource as DataTable;
            //ward1.DefaultView.RowFilter = "";
        }

        /// <summary>
        /// 根据操作状态更新控件状态 add by wyt 2012-11-12
        /// </summary>
        #region
        private void FreshOperView()
        {
            switch (m_type)
            {
                case OperType.USER:
                    switch (m_state)
                    {
                        case OperState.ADD:
                            this.textEditUserID.Enabled = true;
                            this.txtName.Enabled = true;
                            this.lueSex.Enabled = true;
                            this.dateBirthDay.Enabled = true;
                            this.textEditID.Enabled = true;
                            this.lueMarital.Enabled = true;
                            this.lueDept.Enabled = true;
                            this.lueWard.Enabled = true;
                            this.simpleButtonAdd.Enabled = false;
                            this.simpleButtonEdit.Enabled = false;
                            this.simpleButtonDelete.Enabled = false;
                            this.simpleButtonSave.Enabled = true;
                            this.simpleButtonCancel.Enabled = true;
                            InitUserView();
                            this.textEditUserID.Focus();
                            break;
                        case OperState.EDIT:
                            this.textEditUserID.Enabled = false;
                            this.txtName.Enabled = true;
                            this.lueSex.Enabled = true;
                            this.dateBirthDay.Enabled = true;
                            this.textEditID.Enabled = true;
                            this.lueMarital.Enabled = true;
                            this.lueDept.Enabled = true;
                            this.lueWard.Enabled = true;
                            this.simpleButtonAdd.Enabled = false;
                            this.simpleButtonEdit.Enabled = false;
                            this.simpleButtonDelete.Enabled = false;
                            this.simpleButtonSave.Enabled = true;
                            this.simpleButtonCancel.Enabled = true;
                            this.txtName.Focus();
                            break;
                        case OperState.DEL:
                            this.textEditUserID.Enabled = false;
                            this.txtName.Enabled = true;
                            this.lueSex.Enabled = true;
                            this.dateBirthDay.Enabled = true;
                            this.textEditID.Enabled = true;
                            this.lueMarital.Enabled = true;
                            this.lueDept.Enabled = true;
                            this.lueWard.Enabled = true;
                            this.simpleButtonAdd.Enabled = true;
                            this.simpleButtonEdit.Enabled = true;
                            this.simpleButtonDelete.Enabled = true;
                            this.simpleButtonSave.Enabled = false;
                            this.simpleButtonCancel.Enabled = false;
                            InitUserView();
                            break;
                        case OperState.VIEW:
                            this.textEditUserID.Enabled = false;
                            this.txtName.Enabled = false;
                            this.lueSex.Enabled = false;
                            this.dateBirthDay.Enabled = false;
                            this.textEditID.Enabled = false;
                            this.lueMarital.Enabled = false;
                            this.lueDept.Enabled = false;
                            this.lueWard.Enabled = false;
                            this.simpleButtonAdd.Enabled = true;
                            this.simpleButtonEdit.Enabled = true;
                            this.simpleButtonDelete.Enabled = true;
                            this.simpleButtonSave.Enabled = false;
                            this.simpleButtonCancel.Enabled = false;
                            InitUserView();
                            break;
                    }
                    break;
                case OperType.DEPT:
                    switch (m_state)
                    {
                        case OperState.ADD:
                            this.textEditDeptID.Enabled = true;
                            this.textEditDeptName.Enabled = true;
                            this.comboBoxEditDeptSort.Enabled = true;
                            this.simpleButtonAddDept.Enabled = false;
                            this.simpleButtonEditDept.Enabled = false;
                            this.simpleButtonDeleteDept.Enabled = false;
                            this.simpleButtonSaveDept.Enabled = true;
                            this.simpleButtonDeptCancel.Enabled = true;
                            InitDeptView();
                            break;
                        case OperState.EDIT:
                            this.textEditDeptID.Enabled = false;
                            this.textEditDeptName.Enabled = true;
                            this.comboBoxEditDeptSort.Enabled = true;
                            this.simpleButtonAddDept.Enabled = false;
                            this.simpleButtonEditDept.Enabled = false;
                            this.simpleButtonDeleteDept.Enabled = false;
                            this.simpleButtonSaveDept.Enabled = true;
                            this.simpleButtonDeptCancel.Enabled = true;
                            break;
                        case OperState.DEL:
                            this.textEditDeptID.Enabled = false;
                            this.textEditDeptName.Enabled = true;
                            this.comboBoxEditDeptSort.Enabled = true;
                            this.simpleButtonAddDept.Enabled = false;
                            this.simpleButtonEditDept.Enabled = false;
                            this.simpleButtonDeleteDept.Enabled = false;
                            this.simpleButtonSaveDept.Enabled = true;
                            this.simpleButtonDeptCancel.Enabled = true;
                            InitDeptView();
                            break;
                        case OperState.VIEW:
                            this.textEditDeptID.Enabled = false;
                            this.textEditDeptName.Enabled = false;
                            this.comboBoxEditDeptSort.Enabled = false;
                            this.simpleButtonAddDept.Enabled = true;
                            this.simpleButtonEditDept.Enabled = true;
                            this.simpleButtonDeleteDept.Enabled = true;
                            this.simpleButtonSaveDept.Enabled = false;
                            this.simpleButtonDeptCancel.Enabled = false;
                            InitDeptView();
                            break;
                    }
                    break;
                case OperType.WARD:
                    switch (m_state)
                    {
                        case OperState.ADD:
                            this.textEditWardID.Enabled = true;
                            this.textEditWardName.Enabled = true;
                            this.simpleButtonAddWard.Enabled = false;
                            this.simpleButtonEditWard.Enabled = false;
                            this.simpleButtonDeleteWard.Enabled = false;
                            this.simpleButtonSaveWard.Enabled = true;
                            this.simpleButtonWardCancel.Enabled = true;
                            InitWardView();
                            break;
                        case OperState.EDIT:
                            this.textEditWardID.Enabled = false;
                            this.textEditWardName.Enabled = true;
                            this.simpleButtonAddWard.Enabled = false;
                            this.simpleButtonEditWard.Enabled = false;
                            this.simpleButtonDeleteWard.Enabled = false;
                            this.simpleButtonSaveWard.Enabled = true;
                            this.simpleButtonWardCancel.Enabled = true;
                            break;
                        case OperState.DEL:
                            this.textEditWardID.Enabled = false;
                            this.textEditWardName.Enabled = true;
                            this.simpleButtonAddWard.Enabled = false;
                            this.simpleButtonEditWard.Enabled = false;
                            this.simpleButtonDeleteWard.Enabled = false;
                            this.simpleButtonSaveWard.Enabled = true;
                            this.simpleButtonWardCancel.Enabled = true;
                            InitWardView();
                            break;
                        case OperState.VIEW:
                            this.textEditWardID.Enabled = false;
                            this.textEditWardName.Enabled = false;
                            this.simpleButtonAddWard.Enabled = true;
                            this.simpleButtonEditWard.Enabled = true;
                            this.simpleButtonDeleteWard.Enabled = true;
                            this.simpleButtonSaveWard.Enabled = false;
                            this.simpleButtonWardCancel.Enabled = false;
                            InitWardView();
                            break;
                    }
                    break;
            }
        }
        #endregion

        private UserEntity GetUserEntityByView()
        {
            UserEntity user = new UserEntity();
            user.ID = this.textEditUserID.Text.Trim();
            user.Name = this.txtName.Text.Trim();
            user.Sex = this.lueSex.CodeValue;
            user.Birthday = dateBirthDay.DateTime;
            user.CardID = this.textEditID.Text.Trim();
            user.Marital = this.lueMarital.CodeValue;
            user.DeptID = this.lueDept.CodeValue;
            user.WardID = this.lueWard.CodeValue;
            return user;
        }

        private DeptEntity GetDeptEntityByView()
        {
            DeptEntity dept = new DeptEntity();
            dept.ID = this.textEditDeptID.Text.Trim();
            dept.Name = this.textEditDeptName.Text.Trim();
            int index = this.comboBoxEditDeptSort.SelectedIndex;
            switch (index)
            {
                case 0:
                    dept.SortID = DrectSoft.Common.CommonDict.DEPTSORT_LINCHUANG;
                    break;
                case 1:
                    dept.SortID = DrectSoft.Common.CommonDict.DEPTSORT_YIJI;
                    break;
                case 2:
                    dept.SortID = DrectSoft.Common.CommonDict.DEPTSORT_YAOJI;
                    break;
                case 3:
                    dept.SortID = DrectSoft.Common.CommonDict.DEPTSORT_JIGUAN;
                    break;
                case 4:
                    dept.SortID = DrectSoft.Common.CommonDict.DEPTSORT_QITA;
                    break;
                default:
                    dept.SortID = -1;
                    break;
            }
            return dept;
        }

        private WardEntity GetWardEntityByView()
        {
            WardEntity ward = new WardEntity();
            ward.ID = this.textEditWardID.Text.Trim();
            ward.Name = this.textEditWardName.Text.Trim();
            return ward;
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            m_type = OperType.USER;
            m_state = OperState.VIEW;
            this.FreshOperView();
        }

        /// <summary>
        /// 保存用户事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            UserEntity user = this.GetUserEntityByView();
            if (CheckItem(user))
            {
                SaveUser(user);
            }
        }

        private bool SaveUser(UserEntity user)
        {
            try
            {

                m_dataprocess.SaveUserEntity(user, olduserid, m_state);
                switch (m_state)
                {
                    case OperState.ADD:
                        MyMessageBox.Show("新增成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                        break;
                    case OperState.EDIT:
                        MyMessageBox.Show("编辑成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                        break;
                }
                m_state = OperState.VIEW;
                this.FreshOperView();
                this.InitUserData();
                this.FocusUser(user);
                return true;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
                return false;
            }
        }

        private bool CheckItem(UserEntity user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.ID.Trim()))
                {
                    MyMessageBox.Show("编号不能为空", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    this.textEditUserID.Focus();
                    return false;
                }
                else if (user.Name == "")
                {
                    MyMessageBox.Show("姓名不能为空", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    this.txtName.Focus();
                    return false;
                }
                else if (user.ID.Trim() != "00" && user.ID.Trim().Length < 6)
                {
                    string userName = user.ID.Trim().Insert(0, "000000");
                    userName = userName.Substring(userName.Length - 6, 6);
                    if (MyMessageBox.Show("编号长度应为6位，点击确定编号将以 " + userName + " 保存，点击取消返回编辑。", "", MyMessageBoxButtons.OkCancel, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Cancel)
                    {
                        return false;
                    }
                    this.textEditUserID.Text = userName;
                    user.ID = userName;
                }
                uint i_id = 0;
                try
                {
                    i_id = uint.Parse(user.ID);
                    if (i_id > 999999 || i_id < 0)
                    {
                        MyMessageBox.Show("输入的字符串格式有误，应在0-999999之间", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                        this.textEditUserID.Focus();
                        return false;
                    }
                }
                catch
                {
                    MyMessageBox.Show("输入的字符串格式有误，应在0-999999之间", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    this.textEditUserID.Focus();
                    return false;
                }
                if (m_state == OperState.ADD)
                {
                    string id = i_id.ToString("000000");
                    if (m_dataprocess.IsContainID(m_type, id) == true)
                    {
                        MyMessageBox.Show("编号已存在", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                        this.textEditUserID.Focus();
                        return false;
                    }
                }
                if (user.Name.Length > 8)
                {
                    MyMessageBox.Show("名称长度不能大于8", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    this.txtName.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FocusUser(UserEntity user)
        {
            DataTable dtsource = this.gridControlUser.DataSource as DataTable;
            int n = dtsource.Rows.Count;
            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    DataRow dr = dtsource.Rows[i];
                    if (dr["ID"].ToString().Equals(user.ID))
                    {
                        this.gridviewUsers.FocusedRowHandle = i;
                        this.gridviewUsers.Focus();
                        return;
                    }
                }
            }
        }

        private void FocusDept(DeptEntity dept)
        {
            DataTable dtsource = this.gridControlDept.DataSource as DataTable;
            int n = dtsource.Rows.Count;
            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    DataRow dr = dtsource.Rows[i];
                    if (dr["ID"].ToString().Equals(dept.ID))
                    {
                        this.gridViewDept.FocusedRowHandle = i;
                        this.gridViewDept.Focus();
                        return;
                    }
                }
            }
        }

        private void FocusDept(WardEntity ward)
        {
            DataTable dtsource = this.gridControlWard.DataSource as DataTable;
            int n = dtsource.Rows.Count;
            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    DataRow dr = dtsource.Rows[i];
                    if (dr["ID"].ToString().Equals(ward.ID))
                    {
                        this.gridViewWard.FocusedRowHandle = i;
                        this.gridViewWard.Focus();
                        return;
                    }
                }
            }
        }

        //添加用户
        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {
            m_type = OperType.USER;
            m_state = OperState.ADD;
            this.FreshOperView();
        }
        string olduserid = "";
        //编辑用户
        private void simpleButtonEdit_Click(object sender, EventArgs e)
        {
            UserEntity user = this.GetUserEntityByView();
            if (user.ID == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择一条记录");
                return;
            }
            m_type = OperType.USER;
            m_state = OperState.EDIT;
            olduserid = user.ID;
            this.FreshOperView();
        }

        //删除用户
        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                UserEntity user = this.GetUserEntityByView();
                if (user.ID == "")
                {
                    m_app.CustomMessageBox.MessageShow("请选择一条记录");
                    return;
                }
                if (DialogResult.OK == m_app.CustomMessageBox.MessageShow("确认删除？", CustomMessageBoxKind.QuestionOkCancel))
                {
                    m_state = OperState.DEL;
                    m_type = OperType.USER;
                    this.FreshOperView();
                    if (this.SaveUser(user) == true)
                    {
                        m_app.CustomMessageBox.MessageShow("删除成功");
                        this.InitUserData();
                        m_state = OperState.VIEW;
                        this.FreshOperView();
                    }
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void simpleButtonSaveDept_Click(object sender, EventArgs e)
        {
            DeptEntity dept = this.GetDeptEntityByView();
            SaveDept(dept);
        }

        private bool SaveDept(DeptEntity dept)
        {
            #region 验证
            if (dept.ID == "")
            {
                m_app.CustomMessageBox.MessageShow("编号不能为空");
                textEditDeptID.Focus();
                return false;
            }
            else if (dept.Name == "")
            {
                m_app.CustomMessageBox.MessageShow("名称不能为空");
                textEditDeptName.Focus();
                return false;
            }
            else if (dept.SortID == -1)
            {
                m_app.CustomMessageBox.MessageShow("请选择科室类别");
                comboBoxEditDeptSort.Focus();
                return false;
            }
            //uint i_id = 0;
            //try
            //{
            //    i_id = uint.Parse(dept.ID);
            //    if (i_id > 9999 || i_id < 0)
            //    {
            //        m_app.CustomMessageBox.MessageShow("输入的字符串格式有误，应在0-9999之间");
            //        textEditDeptID.Focus();
            //        return false;
            //    }
            //}
            //catch
            //{
            //    m_app.CustomMessageBox.MessageShow("输入的字符串格式有误，应在0-9999之间");
            //    textEditDeptID.Focus();
            //    return false;
            //}
            // string id = i_id.ToString("0000");
            if (m_state == OperState.ADD)
            {
                if (m_dataprocess.IsContainID(m_type, dept.ID) == true)
                {
                    m_app.CustomMessageBox.MessageShow("编号已存在");
                    textEditDeptID.Focus();
                    return false;
                }
            }
            if (dept.Name.Length > 16)
            {
                m_app.CustomMessageBox.MessageShow("名称长度不能大于16");
                textEditDeptName.Focus();
                return false;
            }
            #endregion
            m_dataprocess.SaveDeptEntity(dept, m_state);
            switch (m_state)
            {
                case OperState.ADD:
                    m_app.CustomMessageBox.MessageShow("新增成功");
                    break;
                case OperState.EDIT:
                    m_app.CustomMessageBox.MessageShow("编辑成功");
                    break;
            }
            this.InitDeptWardData();
            m_state = OperState.VIEW;
            this.FreshOperView();
            return true;
        }

        private void simpleButtonSaveWard_Click(object sender, EventArgs e)
        {
            WardEntity ward = this.GetWardEntityByView();
            SaveWard(ward);
        }

        private bool SaveWard(WardEntity ward)
        {
            #region 验证
            if (ward.ID == "")
            {
                m_app.CustomMessageBox.MessageShow("编号不能为空");
                textEditWardID.Focus();
                return false;
            }
            else if (ward.Name == "")
            {
                m_app.CustomMessageBox.MessageShow("名称不能为空");
                textEditWardName.Focus();
                return false;
            }
            if (ward.Name.Length > 16)
            {
                m_app.CustomMessageBox.MessageShow("名称长度不能大于16");
                textEditWardName.Focus();
                return false;
            }
            uint i_id = 0;
            //try
            //{
            //    i_id = uint.Parse(ward.ID);
            //    if (i_id > 9999 || i_id < 0)
            //    {
            //        m_app.CustomMessageBox.MessageShow("输入的字符串格式有误，应在0-9999之间");
            //        textEditWardID.Focus();
            //        return false;
            //    }
            //}
            //catch
            //{
            //    m_app.CustomMessageBox.MessageShow("输入的字符串格式有误，应在0-9999之间");
            //    textEditWardID.Focus();
            //    return false;
            //}
            if (m_state == OperState.ADD)
            {
                string id = i_id.ToString("0000");
                if (m_dataprocess.IsContainID(m_type, ward.ID) == true)
                {
                    m_app.CustomMessageBox.MessageShow("编号已存在");
                    textEditWardID.Focus();
                    return false;
                }
            }
            #endregion
            m_dataprocess.SaveWardEntity(ward, m_state);
            switch (m_state)
            {
                case OperState.ADD:
                    m_app.CustomMessageBox.MessageShow("新增成功");
                    break;
                case OperState.EDIT:
                    m_app.CustomMessageBox.MessageShow("编辑成功");
                    break;
            }
            this.InitDeptWardData();
            m_state = OperState.VIEW;
            this.FreshOperView();
            return true;
        }

        //添加科室病区匹配
        private void simpleButtonAddMarch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridViewDept.FocusedRowHandle < 0)
                {
                    return;
                }
                if (this.gridViewWard.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow rowDept = gridViewDept.GetDataRow(gridViewDept.FocusedRowHandle);
                if (rowDept == null)
                {
                    return;
                }
                DataRow rowWard = gridViewWard.GetDataRow(gridViewWard.FocusedRowHandle);
                if (rowWard == null)
                {
                    return;
                }
                string deptid = rowDept["ID"].ToString();
                string wardid = rowWard["ID"].ToString();
                if (gridControlDept2Ward.DataSource != null && ((DataTable)gridControlDept2Ward.DataSource).Rows.Count != 0)
                {
                    foreach (DataRow dr in ((DataTable)gridControlDept2Ward.DataSource).Rows)
                    {
                        if (dr["DEPTID"].ToString().Equals(deptid) && dr["WARDID"].ToString().Equals(wardid))
                        {
                            m_app.CustomMessageBox.MessageShow("已经存在");
                            return;
                        }
                    }
                }
                string sql = "Insert into dept2ward (deptid, wardid, totalbed) values ('" + deptid + "','" + wardid + "','0')";
                m_app.SqlHelper.ExecuteNoneQuery(sql);
                m_app.CustomMessageBox.MessageShow("添加匹配成功");
                this.InitDept2Ward();
                DataTable dtsource = ((System.Data.DataView)this.gridViewDept2Ward.DataSource).Table;
                int n = dtsource.Rows.Count;
                if (n > 0)
                {
                    for (int i = 0; i < dtsource.Rows.Count; i++)
                    {
                        DataRow dr = ((DataTable)this.gridControlDept2Ward.DataSource).Rows[i];
                        if (dr["DEPTID"].ToString().Equals(deptid) && dr["WARDID"].ToString().Equals(wardid))
                        {
                            this.gridViewDept2Ward.FocusedRowHandle = i;
                            this.gridViewDept2Ward.Focus();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        //删除科室病区匹配
        private void simpleButtonDeleteMarch_Click(object sender, EventArgs e)
        {
            if (this.gridViewDept2Ward.FocusedRowHandle < 0)
                return;
            else
            {
                DataRow row = gridViewDept2Ward.GetDataRow(gridViewDept2Ward.FocusedRowHandle);
                string deptid = row["DEPTID"].ToString();
                string wardid = row["WARDID"].ToString();
                try
                {
                    if (DialogResult.OK == m_app.CustomMessageBox.MessageShow("确认删除？", CustomMessageBoxKind.QuestionOkCancel))
                    {
                        string sql = "delete from dept2ward where deptid = '" + deptid + "' and wardid = '" + wardid + "'";
                        m_app.SqlHelper.ExecuteNoneQuery(sql);
                        m_app.CustomMessageBox.MessageShow("删除匹配成功");
                        this.InitDept2Ward();
                    }
                }
                catch (Exception ex)
                {
                    m_app.CustomMessageBox.MessageShow(ex.Message);
                }
            }
        }

        /// <summary>
        /// 设置按钮是否置灰
        /// 解决科室和病区同时点新增保存存在的状态标示符错乱问题
        /// <auth>XLB</auth>
        /// <date>2013-05-29</date>
        /// </summary>
        /// <param name="isEdit"></param>
        private void SetButtonEdit(int style/*类型1表示点击新增或编辑科室列表2表示操作病区列表*/, bool isEdit)
        {
            try
            {
                switch (style.ToString())
                {
                    case "1":
                        simpleButtonAddWard.Enabled = isEdit;
                        simpleButtonEditWard.Enabled = isEdit;
                        simpleButtonDeleteWard.Enabled = isEdit;
                        simpleButtonSaveWard.Enabled = !isEdit;
                        simpleButtonWardCancel.Enabled = !isEdit;
                        textEditWardID.Enabled = false;
                        textEditWardName.Enabled = false;
                        InitWardView();
                        break;
                    case "2":
                        simpleButtonAddDept.Enabled = isEdit;
                        simpleButtonEditDept.Enabled = isEdit;
                        simpleButtonDeleteDept.Enabled = isEdit;
                        simpleButtonSaveDept.Enabled = !isEdit;
                        simpleButtonDeptCancel.Enabled = !isEdit;
                        textEditDeptID.Enabled = false;
                        textEditDeptName.Enabled = false;
                        comboBoxEditDeptSort.Enabled = false;
                        InitDeptView();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //添加科室
        private void simpleButtonAddDept_Click(object sender, EventArgs e)
        {
            try
            {
                m_type = OperType.DEPT;
                m_state = OperState.ADD;
                SetButtonEdit(1, true);
                this.FreshOperView();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        //编辑科室
        private void simpleButtonEditDept_Click(object sender, EventArgs e)
        {
            try
            {
                DeptEntity dept = this.GetDeptEntityByView();
                if (dept.ID == "")
                {
                    m_app.CustomMessageBox.MessageShow("请选择一条记录");
                    return;
                }
                m_type = OperType.DEPT;
                m_state = OperState.EDIT;
                SetButtonEdit(1, true);
                this.FreshOperView();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        //删除科室
        private void simpleButtonDeleteDept_Click(object sender, EventArgs e)
        {
            try
            {
                DeptEntity dept = this.GetDeptEntityByView();
                if (dept.ID == "")
                {
                    m_app.CustomMessageBox.MessageShow("请选择一条记录");
                    return;
                }
                if (DialogResult.OK == m_app.CustomMessageBox.MessageShow("确认删除？", CustomMessageBoxKind.QuestionOkCancel))
                {
                    m_state = OperState.DEL;
                    m_type = OperType.DEPT;
                    //this.FreshOperView();
                    if (this.SaveDept(dept) == true)
                    {
                        m_app.CustomMessageBox.MessageShow("删除成功");
                    }
                    this.InitDeptWardData();
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void simpleButtonDeptCancel_Click(object sender, EventArgs e)
        {
            m_type = OperType.DEPT;
            m_state = OperState.VIEW;
            SetButtonEdit(1, true);
            this.FreshOperView();
        }

        //添加病区
        private void simpleButtonAddWard_Click(object sender, EventArgs e)
        {
            try
            {
                m_type = OperType.WARD;
                m_state = OperState.ADD;
                SetButtonEdit(2, true);
                this.FreshOperView();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        //编辑病区
        private void simpleButtonEditWard_Click(object sender, EventArgs e)
        {
            try
            {
                WardEntity ward = this.GetWardEntityByView();
                if (ward.ID == "")
                {
                    m_app.CustomMessageBox.MessageShow("请选择一条记录");
                    return;
                }
                m_type = OperType.WARD;
                m_state = OperState.EDIT;
                SetButtonEdit(2, true);
                this.FreshOperView();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        //删除病区
        private void simpleButtonDeleteWard_Click(object sender, EventArgs e)
        {
            try
            {
                WardEntity ward = this.GetWardEntityByView();
                if (ward.ID == "")
                {
                    m_app.CustomMessageBox.MessageShow("请选择一条记录");
                    return;
                }
                if (DialogResult.OK == m_app.CustomMessageBox.MessageShow("确认删除？", CustomMessageBoxKind.QuestionOkCancel))
                {
                    m_state = OperState.DEL;
                    m_type = OperType.WARD;
                    //this.FreshOperView();
                    if (this.SaveWard(ward) == true)
                    {
                        m_app.CustomMessageBox.MessageShow("删除成功");
                    }
                    this.InitDeptWardData();
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void simpleButtonWardCancel_Click(object sender, EventArgs e)
        {
            m_type = OperType.WARD;
            m_state = OperState.VIEW;
            this.FreshOperView();
        }

        //选择用户
        private void gridviewUsers_Click(object sender, EventArgs e)
        {

        }
        //选中科室
        private void gridViewDept_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridViewDept.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow row = gridViewDept.GetDataRow(gridViewDept.FocusedRowHandle);
                if (row == null)
                {
                    return;
                }

                this.textEditDeptID.Text = row["ID"].ToString();
                string name = row["NAME"].ToString();
                this.textEditDeptName.Text = name;
                string sort = row["SORT"].ToString();
                switch (sort)
                {
                    case "临床":
                        this.comboBoxEditDeptSort.SelectedIndex = 0;
                        break;
                    case "医技":
                        this.comboBoxEditDeptSort.SelectedIndex = 1;
                        break;
                    case "药剂":
                        this.comboBoxEditDeptSort.SelectedIndex = 2;
                        break;
                    case "机关":
                        this.comboBoxEditDeptSort.SelectedIndex = 3;
                        break;
                    case "其他":
                        this.comboBoxEditDeptSort.SelectedIndex = 4;
                        break;
                    default:
                        this.comboBoxEditDeptSort.SelectedIndex = 4;
                        break;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 单击病区列表事件填充值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewWard_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridViewWard.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow row = gridViewWard.GetDataRow(gridViewWard.FocusedRowHandle);
                if (row == null)
                {
                    return;
                }
                //m_currentWardID = row["ID"].ToString();
                this.textEditWardID.Text = row["ID"].ToString();
                string name = row["NAME"].ToString();
                this.textEditWardName.Text = name;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        //获取当前用户最多编号
        private int CalMaxUserID()
        {
            int maxid = 0;
            if (this.gridControlUser.DataSource == null)
            {
                return maxid;
            }
            foreach (DataRow dr in ((DataTable)gridControlUser.DataSource).Rows)
            {
                if (int.Parse(dr["ID"].ToString()) > maxid && int.Parse(dr["ID"].ToString()) < 900000)
                {
                    maxid = int.Parse(dr["ID"].ToString());
                }
            }
            return maxid;
        }

        //获取当前科室最多编号
        private int CalMaxDeptID()
        {
            int maxid = 0;
            if (gridControlDept.DataSource == null)
            {
                return maxid;
            }
            foreach (DataRow dr in ((DataTable)gridControlDept.DataSource).Rows)
            {
                if (int.Parse(dr["ID"].ToString()) > maxid)
                {
                    maxid = int.Parse(dr["ID"].ToString());
                }
            }
            return maxid;
        }

        //获取当前病区最多编号
        private int CalMaxWardID()
        {
            int maxid = 0;
            if (gridControlWard.DataSource == null)
            {
                return maxid;
            }
            foreach (DataRow dr in ((DataTable)gridControlWard.DataSource).Rows)
            {
                if (int.Parse(dr["ID"].ToString()) > maxid)
                {
                    maxid = int.Parse(dr["ID"].ToString());
                }
            }
            return maxid;
        }

        private void MaintainBasicInfo_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                //this.simpleButtonClear.Location = new Point(697, 5);
                //this.simpleButtonAdd.Location = new Point(760,27);
                //this.simpleButtonEdit.Location = new Point(820, 27);
                //this.simpleButtonDelete.Location = new Point(880, 27);
                this.gridControlUser.Location = new Point(0, 77);
                this.gridControlUser.Height = this.Height - 120;
                this.gridControlUser.Width = this.Width - 12;
                this.panelControlDept.Width = Convert.ToInt32(this.Width * 0.32);
                this.panelControlDept.Height = this.Height - 50;

                this.panelControlWard.Width = Convert.ToInt32(this.Width * 0.25);
                this.panelControlWard.Height = this.Height - 50;
                this.panelControlWard.Location = new Point(this.panelControlDept.Width + 9, 3);
                this.gridControlWard.Width = this.panelControlWard.Width - 16;
                this.gridControlWard.Height = this.panelControlWard.Height - 108;

                this.simpleButtonAddMarch.Location = new Point(this.panelControlDept.Width + panelControlWard.Width + 16, 180);
                this.simpleButtonDeleteMarch.Location = new Point(this.panelControlDept.Width + panelControlWard.Width + 16, 218);

                this.panelControlDept2Ward.Width = Convert.ToInt32(this.Width * 0.37);
                this.panelControlDept2Ward.Height = this.Height - 50;
                this.panelControlDept2Ward.Location = new Point(this.panelControlDept.Width + panelControlWard.Width + 60, 3);
                this.gridControlDept2Ward.Width = this.panelControlDept2Ward.Width - 16;
                this.gridControlDept2Ward.Height = this.panelControlDept2Ward.Height - 63;

                this.labelControlTip.Location = new Point(24, this.Height - 142);
                this.labelControl1TipDept.Location = new Point(14, this.Height - 200);
                this.labelControl1Ward.Location = new Point(14, this.Height - 200);
                this.labelControl1DeptWard.Location = new Point(14, this.Height - 78);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 加序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridviewUsers_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体激活时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaintainBasicInfo_Activated(object sender, EventArgs e)
        {
            try
            {
                if (this.xtraTabControl.SelectedTabPage == xtraTabPage1)
                {
                    this.textEditUserID.Focus();
                }
                else
                {
                    this.textEditDeptID.Focus();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// Tab切换事件
        /// 定位焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                string pagename = this.xtraTabControl.SelectedTabPage.Name;
                switch (pagename)
                {
                    case "xtraTabPage1":
                        this.textEditUserID.Focus();
                        break;
                    case "xtraTabPage2":
                        this.textEditDeptID.Focus();
                        break;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 为科室列表加序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDept_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DrectSoft.Common.DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 自动筛选 add by wyt 2012-11-12
        string rowfilter = "id like '%{0}%' and name like '%{1}%'";
        string m_userid = "";
        string m_username = "";
        string m_deptid = "";
        string m_deptname = "";
        string m_wardid = "";
        string m_wardname = "";
        private void textEditFilterUserID_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_userid = this.textEditFilterUserID.Text;
                RowFilter(0);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditFilterUserName_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_username = this.textEditFilterUserName.Text;
                RowFilter(0);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditFilterDeptID_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_deptid = this.textEditFilterDeptID.Text;
                RowFilter(1);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditFilterDeptName_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_deptname = this.textEditFilterDeptName.Text;
                RowFilter(1);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditFilterWardID_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_wardid = this.textEditFilterWardID.Text;
                RowFilter(2);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditFilterWardName_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_wardname = this.textEditFilterWardName.Text;
                RowFilter(2);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditFilterDept_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_deptid = this.textEditFilterDept.Text;
                RowFilter(3);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditFilterWard_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_wardid = this.textEditFilterWard.Text;
                RowFilter(3);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// <auth>WYT</auth>
        /// <date>2012-11-12</date>
        /// 显示筛选 
        /// </summary>
        /// <param name="type">类型号：1用户表2科室表3病区表</param>
        private void RowFilter(int type)
        {
            try
            {
                switch (type)
                {
                    case 0:
                        DataTable user = this.gridControlUser.DataSource as DataTable;
                        user.DefaultView.RowFilter = string.Format(rowfilter, m_userid, m_username);
                        break;
                    case 1:
                        DataTable dept = this.gridControlDept.DataSource as DataTable;
                        dept.DefaultView.RowFilter = string.Format(rowfilter, m_deptid, m_deptname);
                        break;
                    case 2:
                        DataTable ward = this.gridControlWard.DataSource as DataTable;
                        ward.DefaultView.RowFilter = string.Format(rowfilter, m_wardid, m_wardname);
                        break;
                    case 3:
                        DataTable dept2ward = this.gridControlDept2Ward.DataSource as DataTable;
                        dept2ward.DefaultView.RowFilter = string.Format("deptid like '%{0}%' and wardid like '%{1}%'", m_deptid, m_wardid);
                        break;
                }
                gridviewUsers_FocusedRowChanged(null, null);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        /// <summary>
        /// 选中行改变触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridviewUsers_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.gridviewUsers.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow row = gridviewUsers.GetDataRow(gridviewUsers.FocusedRowHandle);
                SetPageData(row);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void textEditUserID_Leave(object sender, EventArgs e)
        {
            try
            {
                string userName = this.textEditUserID.Text;
                if (userName.Trim() != "00" && !string.IsNullOrEmpty(userName.Trim()) && userName.Trim().Length < 6)
                {
                    userName = userName.Trim().Insert(0, "000000");
                    this.textEditUserID.Text = userName.Substring(userName.Length - 6, 6);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void SetPageData(DataRow row)
        {
            try
            {
                if (null == row)
                {
                    return;
                }
                this.textEditUserID.Text = row["ID"].ToString();
                this.txtName.Text = row["NAME"].ToString();
                this.lueMarital.CodeValue = row["Marital"].ToString();
                this.dateBirthDay.Text = row["BIRTH"].ToString();
                this.lueSex.CodeValue = row["Sexy"].ToString();
                this.lueDept.CodeValue = row["DEPTID"].ToString();
                this.lueWard.CodeValue = row["WARDID"].ToString();
                this.textEditID.Text = row["IDNO"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}