using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
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

namespace DrectSoft.Core.MedicalRecordQuery
{
    /// <summary>
    /// 设置主诊医师界面
    /// 用于病历浏览中的查询控制
    /// add by ywk 2012年8月9日 16:54:16
    /// </summary>
    public partial class SetAttendDoctor : DevBaseForm
    {
        #region 属性及字段

        private IEmrHost m_app;
        IDataAccess m_SqlHelper;
        private string m_id = "";
        private string m_name = "";
        private string m_deptid = "";
        private string m_py = "";
        private string m_wb = "";
        private string m_relatedisease = "";
        private string m_formname = "";//窗体名 xlb 2012-12-19
        private List<string> m_DeleteList;//保存删除ID列表 xlb 2012-12-19
        //private string m_DiseId = "";//病种ID用来判断保存事件是修改或者新增

        /// <summary>
        /// 构造函数
        /// </summary>
        public SetAttendDoctor(IEmrHost app)
        {
            try
            {
                m_app = app;
                m_SqlHelper = app.SqlHelper;
                DS_SqlHelper.CreateSqlHelper();
                m_DeleteList = new List<string>();
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 构造函数
        /// by xlb 2012-12-19
        /// </summary>
        /// <param name="app"></param>
        /// <param name="formname">窗体名</param>
        public SetAttendDoctor(IEmrHost app, string formname)
        {
            try
            {
                m_app = app;
                m_SqlHelper = app.SqlHelper;
                m_formname = formname;
                DS_SqlHelper.CreateSqlHelper();
                m_DeleteList = new List<string>();
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 方法
        //初始化科室
        private void InitDepartment()
        {
            lookUpWindowDept.SqlHelper = m_SqlHelper;

            DataTable Dept = m_SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                 new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);

            Dept.Columns["ID"].Caption = "代码";
            Dept.Columns["NAME"].Caption = "科室名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 65);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querydept", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
            lookUpEditorDept.SqlWordbook = deptWordBook;
        }

        /// <summary>
        /// 重置方法 
        /// 用于清空文本框
        /// xlb 2012-12-19
        /// </summary>
        private void Reset()
        {
            try
            {
                textEditGroupName.Text = string.Empty;
                lookUpEditorDisease.CodeValue = string.Empty;
                richTextBoxDisease.Text = string.Empty;
                textEditGroupName.Enabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查找所有员工信息
        /// by xlb 2012-12-19
        /// </summary>
        private void InitAlluser()
        {
            try
            {
                string sql = @"   SELECT users.name as username, department.name as deptname, users.deptid as deptid, users.id as userid, users.py,users.wb FROM users
           join department on users.deptid = department.id  and users.valid = 1";
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dt == null && dt.Rows.Count == 0)
                {
                    return;
                }
                gridControlUsers.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void InitUserName()
        {
            this.lookUpWindowUserName.SqlHelper = m_SqlHelper;
            string sql = "select ID,NAME,PY,WB from users where valid = 1";
            if (this.lookUpEditorDept.CodeValue.Trim() != "")
            {
                string deptid = this.lookUpEditorDept.CodeValue == "0000" ? "" : this.lookUpEditorDept.CodeValue;

                if (deptid != "")
                {
                    sql = "select ID,NAME,PY,WB from users where Deptid='" + deptid + "' and valid = 1";
                }

            }
            DataTable Name = m_SqlHelper.ExecuteDataTable(sql);
            Name.Columns["ID"].Caption = "代码";
            Name.Columns["NAME"].Caption = "员工名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 50);
            cols.Add("NAME", 100);

            SqlWordbook nameWordBook = new SqlWordbook("queryname", Name, "ID", "NAME", cols, "ID//NAME//PY//WB");
            this.lookUpEditorUserName.SqlWordbook = nameWordBook;
        }


        private void InitShowDisease()
        {
            DataTable disease = new DataTable();
            disease.Columns.Add("ICD");
            disease.Columns.Add("NAME");
            disease.Columns.Add("PY");
            disease.Columns.Add("WB");
            DataTable diagnosis = m_SqlHelper.ExecuteDataTable("select py, wb, name, icd from diagnosis  where valid='1' union select py, wb, name, icdid from diagnosisothername where valid='1'");
            foreach (DataRow row in diagnosis.Rows)
            {
                DataRow displayRow = disease.NewRow();
                displayRow["ICD"] = row["ICD"];
                displayRow["NAME"] = row["NAME"];
                displayRow["PY"] = row["PY"];
                displayRow["WB"] = row["WB"];
                disease.Rows.Add(displayRow);
            }
            //checkedListBoxControlDisease.DisplayMember = "ICD";
            //checkedListBoxControlDisease.DisplayMember = "NAME";
            //checkedListBoxControlDisease.ValueMember = "ICD";
            //checkedListBoxControlDisease.DataSource = disease;


            this.lookUpWindowDisease.SqlHelper = m_SqlHelper;
            disease.Columns["ICD"].Caption = "代码";
            disease.Columns["NAME"].Caption = "病种";
            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ICD", 80);
            cols.Add("NAME", 160);

            SqlWordbook diagWordBook = new SqlWordbook("queryDiag", disease, "ICD", "NAME", cols, "ICD//NAME//PY//WB");
            this.lookUpEditorDisease.SqlWordbook = diagWordBook;
        }
        #endregion

        ///// <summary>
        ///// 判定某个员工对应得关联病种信息，若有某个关联病种就打钩显示出来
        ///// </summary>
        ///// <param name="strDiseaseID"></param>
        //private void FillDisease(string strDiseaseID)
        //{
        //    int k = 0;
        //    if (strDiseaseID == "")
        //    {
        //        while (this.checkedListBoxControlDisease.GetItem(k) != null)
        //        {
        //            checkedListBoxControlDisease.SetItemChecked(k, false);
        //            k++;
        //        }
        //    }
        //    else
        //    {
        //        while (this.checkedListBoxControlDisease.GetItem(k) != null)
        //        {
        //            DataRowView ch = checkedListBoxControlDisease.GetItem(k) as DataRowView;
        //            checkedListBoxControlDisease.SetItemChecked(k, false);
        //            if (ch["ICD"].ToString() == strDiseaseID)
        //            {
        //                checkedListBoxControlDisease.SetItemChecked(k, true);
        //                break;
        //            }
        //            k++;
        //        }
        //    }
        //}

        private void SetAttendDoctor_Load(object sender, EventArgs e)
        {
            try
            {
                this.InitDepartment();
                this.InitUserName();
                this.InitShowDisease();
                this.InitDiseaseGroup();
                this.BindPopupMenu();
                this.InitAlluser();
                this.popupMenu.Manager = barManagerPop;
                this.barManagerPop.Form = this;
                this.Text = m_formname;//当前窗体名 xlb 2012-12-19
                this.textEditGroupName.Focus();//焦点定位到组合名称文本框 xlb 2012-12-19
                this.richTextBoxDisease.ReadOnly = true;//xlb 2012-12-19
                //this.checkedListBoxControlDisease.Hide();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 改用参数查询，避免特殊字符带来的问题
        /// by xlb 2012-12-20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonQueryDG_Click(object sender, EventArgs e)
        {
            try
            {//add try catch 替换单引号，屏蔽基本引起问题的字符  XLB 2012-12-10
                //string sql = "select * from DiseaseGroup";
                //string name = this.textEditGroupNameQuery.Text.Trim().Replace("'","''");
                //if (name != "")
                //{
                //    if (name.Contains("["))
                //    {
                //        m_app.CustomMessageBox.MessageShow("请不要输入特殊字符");
                //        return;
                //    }
                string name = this.textEditGroupNameQuery.Text.Trim();
                string sql = "select * from DiseaseGroup where name like '%'||@name||'%'";
                SqlParameter[] sps = { new SqlParameter("@name", name) };
                // }
                //DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.gridControlDiseaseGroup.DataSource = dt;
                }
                else
                {
                    this.gridControlDiseaseGroup.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                //m_app.CustomMessageBox.MessageShow(ex.Message);
                MyMessageBox.Show(1, ex);
            }
        }
        private void InitDiseaseGroup()
        {
            try
            {
                string sql = "select * from DiseaseGroup";
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
                this.gridControlDiseaseGroup.DataSource = dt;
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }
        private DevExpress.XtraBars.BarButtonItem barButtonItemDiseaseInfo;
        private void BindPopupMenu()
        {
            barButtonItemDiseaseInfo = new DevExpress.XtraBars.BarButtonItem();
            barButtonItemDiseaseInfo.Caption = "删除";
            barButtonItemDiseaseInfo.Id = 1;
            barButtonItemDiseaseInfo.Name = "Delete";
            barButtonItemDiseaseInfo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItemClick);
            popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
                new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemDiseaseInfo)
            });
            barManagerPop.Items.AddRange(new DevExpress.XtraBars.BarItem[] { 
                barButtonItemDiseaseInfo 
            });
        }
        /// <summary>
        /// 删除操作
        /// add 当前用户再删除病种组合时，只有用户具有了改病种组合才可以删除
        /// add by 项令波 2012-12-12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void barButtonItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (MyMessageBox.Show("您确认删除吗？", "提示", MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    DataRow dr = gridViewDiseaseGroup.GetDataRow(gridViewDiseaseGroup.FocusedRowHandle);
                    if (dr != null)
                    {
                        string name = dr["NAME"].ToString();
                        string groupname = dr["DISEASEGROUP"].ToString().Trim();
                        string disaseid = dr["ID"].ToString().Trim();
                        string sql = string.Format(@"delete from DiseaseGroup where name = '" + name + "' or name is null");
                        DataTable dt = m_SqlHelper.ExecuteDataTable("select relatedisease from attendingphysician where id=@ID", new SqlParameter[] { new SqlParameter("ID", m_app.User.Id) }, CommandType.Text);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string diseasename = dt.Rows[0]["RELATEDISEASE"].ToString().Trim();
                            if (diseasename == groupname || m_DeleteList.Contains(disaseid))
                            {
                                DS_SqlHelper.ExecuteNonQuery(sql);
                            }
                            else
                            {
                                MessageBox.Show("您没有权限删除");
                            }
                        }
                        else
                        {
                            if (m_DeleteList.Contains(disaseid))
                            {
                                DS_SqlHelper.ExecuteNonQuery(sql);
                            }
                            else
                            {
                                MessageBox.Show("您没有权限删除");

                            }
                        }
                    }
                }
                InitDiseaseGroup();
            }
            catch (Exception ex)
            {
                //m_app.CustomMessageBox.MessageShow(ex.Message);
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 操作病种组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDiseaseGroup_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                GridHitInfo hit = gridViewDiseaseGroup.CalcHitInfo(e.X, e.Y);
                if (hit.RowHandle >= 0)
                {
                    DataRow dr = gridViewDiseaseGroup.GetDataRow(hit.RowHandle);
                    if (dr != null)
                    {
                        barButtonItemDiseaseInfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                    }
                    this.popupMenu.ShowPopup(this.barManagerPop, (Cursor.Position));
                }
            }
        }
        //private void checkedListBoxControlDisease_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        //{
        //    this.popupContainerEditDisease.EditValue = string.Empty;
        //    foreach (DataRowView ch in checkedListBoxControlDisease.CheckedItems)
        //    {
        //        popupContainerEditDisease.EditValue += ch["ICD"].ToString() + ",";
        //    }
        //}

        private void simpleButtonQuery_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(lookUpEditorDept.CodeValue))
            //{
            //    m_app.CustomMessageBox.MessageShow("请选择出院科室!");
            //    return;
            //}


            DataTable table = m_SqlHelper.ExecuteDataTable("usp_selectusers"
               , new SqlParameter[] 
                { 
                    new SqlParameter("@DeptId", this.lookUpEditorDept.CodeValue.Trim()),
                    new SqlParameter("@UserName", this.lookUpEditorUserName.Text.Trim())
                }
               , CommandType.StoredProcedure);

            gridViewUsers.SelectAll();
            gridViewUsers.DeleteSelectedRows();
            gridControlUsers.DataSource = table;

            //lblTip.Text = "共" + table.Rows.Count.ToString() + "条记录";

            if (table.Rows.Count <= 0)
                m_app.CustomMessageBox.MessageShow("没有满足条件的记录！");
        }

        //private void popupContainerEditDisease_Click(object sender, EventArgs e)
        //{
        //    this.checkedListBoxControlDisease.Show();
        //}

        //private void popupContainerEditDisease_EditValueChanged(object sender, EventArgs e)
        //{

        //}

        private void gridControlUsers_Click(object sender, EventArgs e)
        {
            if (gridViewUsers.FocusedRowHandle < 0)
                return;
            else
            {
                DataRow dataRow = gridViewUsers.GetDataRow(gridViewUsers.FocusedRowHandle);
                if (dataRow == null)
                    return;
                else
                {
                    string name = dataRow["USERNAME"].ToString();
                    this.lookUpEditorUserName.Text = name;

                    this.m_id = dataRow["USERID"].ToString();
                    this.m_deptid = dataRow["DEPTID"].ToString();
                    this.m_name = dataRow["USERNAME"].ToString();
                    this.m_py = dataRow["PY"].ToString();
                    this.m_wb = dataRow["WB"].ToString();
                    DataTable diag = m_SqlHelper.ExecuteDataTable("usp_getdiagbyattendphysician",
                new SqlParameter[] { new SqlParameter("@USERID", m_id) }, CommandType.StoredProcedure);

                    if (diag.Rows.Count != 0)
                    {
                        string diagicds = diag.Rows[0][0].ToString();
                        this.richTextBoxCurrentGroup.Text = diagicds;
                        //this.popupContainerEditDisease.Text = diagicds;
                        //string[] icd = diagicds.Split(',');
                        //for (int i = 0; i < icd.Length; i++)
                        //{
                        //    if (icd[i].Trim() != "")
                        //    {
                        //        FillDisease(icd[i]);
                        //    }
                        //}
                    }
                    else
                    {
                        this.richTextBoxCurrentGroup.Text = "";
                        //this.popupContainerEditDisease.Text = "";
                        //FillDisease("");
                    }
                }
            }
        }

        private void simpleButtonApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewUsers.FocusedRowHandle < 0 || gridViewDiseaseGroup.FocusedRowHandle < 0)
                    return;
                int index = this.gridViewDiseaseGroup.FocusedRowHandle;
                DataTable dt = (DataTable)this.gridControlDiseaseGroup.DataSource;
                string diseasegroup = dt.Rows[index]["DISEASEGROUP"].ToString();
                m_SqlHelper.ExecuteNoneQuery("usp_updateattendphysicianinfo"
                   , new SqlParameter[] 
                { 
                    new SqlParameter("@ID", m_id),
                    new SqlParameter("@NAME", m_name),
                    new SqlParameter("@PY", m_py),
                    new SqlParameter("@WB", m_wb),
                    new SqlParameter("@DEPTID", m_deptid),
                    new SqlParameter("@RELATEDISEASE", diseasegroup)
                }
                   , CommandType.StoredProcedure);
                this.richTextBoxCurrentGroup.Text = diseasegroup;
                m_app.CustomMessageBox.MessageShow("设置成功!");
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void SetAttendDoctor_SizeChanged(object sender, EventArgs e)
        {
            this.gridControlUsers.Width = this.Width - 3;
            this.richTextBoxDisease.Width = this.Width - 3;
            this.gridControlDiseaseGroup.Width = this.Width - 3;
            this.gridControlDiseaseGroup.Height = this.Height - 397;
        }

        private void lookUpEditorUserName_Click(object sender, EventArgs e)
        {
            InitUserName();
        }



        //病种组合相关 add by wyt 2012-08-30
        //private void simpleButtonAddToGroup_Click(object sender, EventArgs e)
        //{
        //    if (this.richTextBoxDisease.Text.Trim() == "")
        //    {
        //        m_app.CustomMessageBox.MessageShow("病种不能为空!");
        //        richTextBoxDisease.Focus();
        //        return;
        //    }
        //    else if (this.textEditGroupName.Text.Trim() == "")
        //    {
        //        m_app.CustomMessageBox.MessageShow("病种名称不能为空!");
        //        this.textEditGroupName.Focus();
        //        return;
        //    }
        //    string id = (CalMaxDiseaseGroupID() + 1).ToString("0000");
        //    string diseasegroup = this.richTextBoxDisease.Text;
        //    string name = this.textEditGroupName.Text;
        //    string sql = "Insert Into DiseaseGroup (id,name,diseasegroup) values ('" + id + "','" + name + "','" + diseasegroup + "')";
        //    if (m_app.SqlHelper.ExecuteDataTable("select * from DiseaseGroup where name = '" + name + "'").Rows.Count > 0)
        //    {
        //        if (DialogResult.OK == m_app.CustomMessageBox.MessageShow("组合名已经存在，是否覆盖原组合？", CustomMessageBoxKind.QuestionOkCancel))
        //        {
        //            sql = "Update DiseaseGroup set id = '" + id + "', name = '" + name + "', diseasegroup = '" + diseasegroup + "' where name = '" + name + "'";
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //    m_app.SqlHelper.ExecuteNoneQuery(sql);
        //    this.InitDiseaseGroup();
        //    m_app.CustomMessageBox.MessageShow("添加成功!");
        //}
        /// <summary>
        /// edit by xlb 2012-12-19 
        /// try...catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void BtnAddDiseaseGroup_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (this.richTextBoxDisease.Text.Trim() == "")
        //        {
        //            m_app.CustomMessageBox.MessageShow("病种不能为空!");
        //            richTextBoxDisease.Focus();
        //            return;
        //        }
        //        else if (this.textEditGroupName.Text.Trim() == "")
        //        {
        //            m_app.CustomMessageBox.MessageShow("病种名称不能为空!");
        //            this.textEditGroupName.Focus();
        //            return;
        //        }
        //        string id = (CalMaxDiseaseGroupID() + 1).ToString("0000");
        //        string diseasegroup = this.richTextBoxDisease.Text;
        //        string name = this.textEditGroupName.Text;
        //        string sql = "Insert Into DiseaseGroup (id,name,diseasegroup) values ('" + id + "','" + name + "','" + diseasegroup + "')";
        //        if (m_app.SqlHelper.ExecuteDataTable("select * from DiseaseGroup where name = '" + name + "'").Rows.Count > 0)
        //        {
        //            if (DialogResult.OK == m_app.CustomMessageBox.MessageShow("组合名已经存在，是否覆盖原组合？", CustomMessageBoxKind.QuestionOkCancel))
        //            {
        //                sql = "Update DiseaseGroup set id = '" + id + "', name = '" + name + "', diseasegroup = '" + diseasegroup + "' where name = '" + name + "'";
        //            }
        //            else
        //            {
        //                return;
        //            }
        //        }
        //        m_app.SqlHelper.ExecuteNoneQuery(sql);
        //        this.InitDiseaseGroup();
        //        Reset();
        //        m_DeleteList.Add(id);
        //        m_app.CustomMessageBox.MessageShow("添加成功!");
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
        //    }
        //}

        private void simpleButtonEditDiseaseGroup_Click(object sender, EventArgs e)
        {
            if (this.gridViewDiseaseGroup.FocusedRowHandle < 0)
            {
                return;
            }
            else if (this.richTextBoxDisease.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("病种不能为空!");
                richTextBoxDisease.Focus();
                return;
            }
            else if (this.textEditGroupName.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("病种名称不能为空!");
                this.textEditGroupName.Focus();
                return;
            }
            string diseasegroup = this.richTextBoxDisease.Text;
            string name = this.textEditGroupName.Text;
            int index = this.gridViewDiseaseGroup.FocusedRowHandle;
            DataTable dt = (DataTable)this.gridControlDiseaseGroup.DataSource;
            string id = dt.Rows[index]["ID"].ToString();
            string sql = "UPDATE DiseaseGroup SET diseasegroup = '" + diseasegroup + "', name = '" + name + "' where id = '" + id + "'";
            m_app.SqlHelper.ExecuteNoneQuery(sql);
            this.InitDiseaseGroup();
            m_app.CustomMessageBox.MessageShow("修改成功!");
        }


        private void simpleButtonDelDiseaseGroup_Click(object sender, EventArgs e)
        {
            if (this.gridViewDiseaseGroup.FocusedRowHandle < 0)
            {
                return;
            }
            int index = this.gridViewDiseaseGroup.FocusedRowHandle;
            DataTable dt = (DataTable)this.gridControlDiseaseGroup.DataSource;
            string id = dt.Rows[index]["ID"].ToString();
            string sql = "DELETE from DiseaseGroup where id = '" + id + "'";
            if (DialogResult.OK == m_app.CustomMessageBox.MessageShow("确认删除？", CustomMessageBoxKind.QuestionOkCancel))
            {
                m_app.SqlHelper.ExecuteNoneQuery(sql);
                this.InitDiseaseGroup();
                m_app.CustomMessageBox.MessageShow("删除成功!");
            }
        }

        private void gridControlDiseaseGroup_Click(object sender, EventArgs e)
        {
            //if (this.gridViewDiseaseGroup.FocusedRowHandle < 0)
            //{
            //    return;
            //}
            //int index = this.gridViewDiseaseGroup.FocusedRowHandle;
            //DataTable dt = (DataTable)this.gridControlDiseaseGroup.DataSource;
            //this.richTextBoxCurrentGroup.Text = dt.Rows[index]["DISEASEGROUP"].ToString();
        }

        private void gridControlDiseaseGroup_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridViewDiseaseGroup.FocusedRowHandle < 0)
            {
                return;
            }
            int index = this.gridViewDiseaseGroup.FocusedRowHandle;
            DataTable dt = (DataTable)this.gridControlDiseaseGroup.DataSource;
            this.textEditGroupName.Text = dt.Rows[index]["NAME"].ToString();
            this.richTextBoxDisease.Text = dt.Rows[index]["DISEASEGROUP"].ToString();
            string m_DiseId = dt.Rows[index]["ID"].ToString();//add by xlb 2012-12-21
            textEditGroupName.Enabled = false;
        }

        #region 事件
        //获取当前病区最多编号
        private int CalMaxDiseaseGroupID()
        {
            int maxid = 0;
            if (gridControlDiseaseGroup.DataSource == null)
            {
                return maxid;
            }
            foreach (DataRow dr in ((DataTable)gridControlDiseaseGroup.DataSource).Rows)
            {
                if (int.Parse(dr["ID"].ToString()) > maxid)
                {
                    maxid = int.Parse(dr["ID"].ToString());
                }
            }
            return maxid;
        }
        #endregion

        //private void gridControlDiseaseGroup_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        this.popupMenu.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
        //    }
        //}

        #region 绑定右键菜单
        //private DevExpress.XtraBars.BarButtonItem barButtonItemDiseaseGroup;
        //private void BindPopupMenu()
        //{
        //    barButtonItemDiseaseGroup = new DevExpress.XtraBars.BarButtonItem();
        //    barButtonItemDiseaseGroup.Caption = "删除";
        //    barButtonItemDiseaseGroup.Id = 1;
        //    barButtonItemDiseaseGroup.Name = "Delete";
        //    barButtonItemDiseaseGroup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItemDiseaseGroup_ItemClick);
        //    popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
        //        new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemDiseaseGroup)
        //    });
        //    barManagerPop.Items.AddRange(new DevExpress.XtraBars.BarItem[] { 
        //        barButtonItemDiseaseGroup 
        //    });
        //}

        #endregion

        private void gridViewDiseaseGroup_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            //if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            //{
            //    e.Info.DisplayText = (e.RowHandle + 1).ToString();
            //}
            try
            {
                DS_Common.AutoIndex(e);  //edit by xlb 2012-12-21
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void gridControlDiseaseGroup_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    this.popupMenu.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));

            //    ListViewHitTestInfo hitInfo = listView1.HitTest(Control.MousePosition.X, Control.MousePosition.Y);
            //    if (hitInfo.Item != null && hitInfo.SubItem != null)
            //    {
            //        listView1.Focus();
            //        listView1.FocusedItem = hitInfo.Item;

            //        this.barButtonItem2.Visibility = BarItemVisibility.Always;

            //    }
            //}
        }



        /// <summary>
        /// 重置事件
        /// by xlb 2012-12-21
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReSet_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件 新增或修改
        /// by xlb 2012-12-21
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.richTextBoxDisease.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("病种不能为空!");
                    richTextBoxDisease.Focus();
                    return;
                }
                else if (this.textEditGroupName.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("病种名称不能为空!");
                    this.textEditGroupName.Focus();
                    return;
                }
                string id = (CalMaxDiseaseGroupID() + 1).ToString("0000");
                string diseasegroup = this.richTextBoxDisease.Text;
                string name = this.textEditGroupName.Text;
                string sql = "Insert Into DiseaseGroup (id,name,diseasegroup) values (@diseid,@name,@diseasegroup)";
                SqlParameter[] spr = {
                                         new SqlParameter("@diseid", id),
                                         new SqlParameter("@diseasegroup", diseasegroup),
                                         new SqlParameter("@name", name)
                                     };
                string QueryStr = @"select * from DiseaseGroup where name =@name";
                SqlParameter p_DiseName = new SqlParameter("@name", SqlDbType.NVarChar);
                p_DiseName.Value = name;
                if (DS_SqlHelper.ExecuteDataTable(QueryStr, new SqlParameter[] { p_DiseName }, CommandType.Text).Rows.Count > 0)
                {
                    sql = "Update DiseaseGroup set id =@diseid, name =@name, diseasegroup =@diseasegroup where name =@name";

                    if (textEditGroupName.Enabled)
                    {
                        if (MyMessageBox.Show("组合已存在，您确定覆盖原组合吗？", "提示", MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                        {
                            DS_SqlHelper.ExecuteNonQuery(sql, spr, CommandType.Text);
                            MessageBox.Show("覆盖成功");
                            textEditGroupName.Enabled = true;
                            this.InitDiseaseGroup();
                            Reset();
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        DS_SqlHelper.ExecuteNonQuery(sql, spr, CommandType.Text);
                        MessageBox.Show("修改成功");
                        textEditGroupName.Enabled = true;
                        this.InitDiseaseGroup();
                        Reset();
                        return;
                    }
                }
                DS_SqlHelper.ExecuteNonQuery(sql, spr, CommandType.Text);
                this.InitDiseaseGroup();
                Reset();
                m_DeleteList.Add(id);
                MessageBox.Show("添加成功");
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增事件
        /// by xlb 2012-12-21
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lookUpEditorDisease.CodeValue.Trim() == "")
                {
                    return;
                }
                string disease = this.richTextBoxDisease.Text;
                //string disease = this.richTextBoxDisease.Text;
                if (disease.Trim() != "")
                {
                    string[] diseases = disease.Split('，');

                    foreach (string str in diseases)
                    {
                        if (str.Contains("$"))
                        {
                            string[] diseasedetail = str.Split('$');
                            if (diseasedetail.Length > 0)
                            {
                                if (diseasedetail[0] == this.lookUpEditorDisease.CodeValue)
                                {
                                    //m_app.CustomMessageBox.MessageShow("已存在!");
                                    MessageBox.Show("已存在");
                                    return;
                                }
                            }
                        }
                        else if (str.Contains("-"))
                        {
                            string[] diseasedetail = str.Split('-');
                            if (diseasedetail.Length > 0)
                            {
                                if (diseasedetail[0] == this.lookUpEditorDisease.CodeValue)
                                {
                                    //m_app.CustomMessageBox.MessageShow("已存在!");
                                    MessageBox.Show("已存在");
                                    return;
                                }
                            }
                        }
                    }
                    disease = disease + "，" + this.lookUpEditorDisease.CodeValue + "$" + this.lookUpEditorDisease.Text;
                }
                else
                {
                    disease = this.lookUpEditorDisease.CodeValue + "$" + this.lookUpEditorDisease.Text;
                }
                this.richTextBoxDisease.Text = disease;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}