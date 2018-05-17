using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common;
using DrectSoft.Service;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Wordbook;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using System.Data.Common;
using DrectSoft.DSSqlHelper;
using System.Xml;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;

namespace DrectSoft.Core.MajorDiagnoseDoctor
{
    /// <summary>
    /// 功能描述：主诊医师设置
    /// 创 建 者：Yanqiao.Cai
    /// 创建日期：2012-12-28
    /// </summary>
    public partial class SetAttendDoctorForm : DevBaseForm, IStartPlugIn
    {
        IEmrHost m_app;

        //全部组合数据集(未勾选)
        private DataTable allSource = new DataTable();
        //初始化组合数据集
        private DataTable defaultSource = new DataTable();
        //已勾选和未勾选的数据集
        private List<DataRow> checkedList = new List<DataRow>();
        private List<DataRow> notCheckedList = new List<DataRow>();

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="App"></param>
        public SetAttendDoctorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetAttendDoctorForm_Load(object sender, EventArgs e)
        {
            try
            {
                //初始化科室
                InitDepartment();
                //设置默认科室
                this.lookUpEditorDepartment.CodeValue = DS_Common.currentUser.CurrentDeptId;
                //导入历史数据按钮是否显示
                string config = DS_SqlService.GetConfigValueByKey("ImportDiseasesGroupHistoryData");
                XmlDocument doc = new XmlDocument();
                if (!string.IsNullOrEmpty(config))
                {
                    doc.LoadXml(config);
                }
                XmlNodeList nodeList = doc.GetElementsByTagName("btnflag");
                string btnflag = string.Empty;
                if (null != nodeList && nodeList.Count > 0)
                {
                    btnflag = nodeList[0].InnerText;
                }
                if (!string.IsNullOrEmpty(btnflag) && btnflag.Trim() == "1")
                {
                    this.btn_importData.Visible = true;
                }
                else
                {
                    this.btn_importData.Visible = false;
                }

                this.ActiveControl = this.txt_userName;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #region 事件
        /// <summary>
        /// 查询事件 --- 员工列表
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_queryUsers_Click(object sender, EventArgs e)
        {
            try
            {
                //刷新用户列表
                RefreashUsersData();
                //默认点击第一条
                SearchRightData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 用户列表单击事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl_user_Click(object sender, EventArgs e)
        {
            try
            {
                SearchRightData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 刷新组合权限事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-07</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            try
            {
                SearchRightData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存权限事件(授权事件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_saveRights_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow userRow = gridView_user.GetDataRow(gridView_user.FocusedRowHandle);
                if (null == userRow)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条员工记录");
                    return;
                }
                if (checkedList.Count == 0)
                {
                    //if (Common.Ctrs.DLG.MessageBox.Show("您没有勾选组合权限，确定要保存吗？", "提示", Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                    //{
                    //    return;
                    //}

                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择要对应的病种组合");
                    return;
                }
                else
                {
                    string focusedUserID = userRow["USERID"].ToString().Trim();
                    DataTable rightDt = DS_SqlService.GetUserMatchDiseaseGroup(focusedUserID);
                    //设置参数
                    List<DbParameter> parameters = new List<DbParameter>();
                    SqlParameter param1 = new SqlParameter("@groupids", SqlDbType.Char);
                    param1.Value = string.Join("$", checkedList.Select(p => p["ID"].ToString()).ToArray());
                    parameters.Add(param1);

                    int result = 0;
                    if (null == rightDt || rightDt.Rows.Count == 0)
                    {//新增权限
                        SqlParameter param2 = new SqlParameter("@userid", SqlDbType.Char);
                        param2.Value = focusedUserID;
                        parameters.Add(param2);
                        SqlParameter param3 = new SqlParameter("@username", SqlDbType.Char);
                        param3.Value = userRow["USERNAME"].ToString().Trim();
                        parameters.Add(param3);
                        SqlParameter param4 = new SqlParameter("@valid", SqlDbType.Int);
                        param4.Value = 1;
                        parameters.Add(param4);
                        SqlParameter param5 = new SqlParameter("@create_user", SqlDbType.Char);
                        param5.Value = DS_Common.currentUser.Id;
                        parameters.Add(param5);
                        SqlParameter param6 = new SqlParameter("@create_time", SqlDbType.Char);
                        param6.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        parameters.Add(param6);

                        result = DS_SqlService.InsertUserMatchDiseaseGroup(parameters);
                    }
                    else
                    {//编辑权限
                        SqlParameter param7 = new SqlParameter("@updateuser", SqlDbType.Char);
                        param7.Value = DS_Common.currentUser.Id;
                        parameters.Add(param7);
                        SqlParameter param8 = new SqlParameter("@updatetime", SqlDbType.Char);
                        param8.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        parameters.Add(param8);
                        SqlParameter param9 = new SqlParameter("@id", SqlDbType.Int);
                        param9.Value = int.Parse(rightDt.Rows[0]["ID"].ToString());
                        parameters.Add(param9);

                        result = DS_SqlService.UpdateUserMatchDiseaseGroup(parameters);
                    }
                    if (result == 1)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("授权成功");
                    }
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 勾选事件 --- 组合权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_groupRight_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.FieldName != "FLAG")
                {
                    return;
                }
                DataRow row = gridView_groupRight.GetDataRow(e.RowHandle);
                if (null == row || null == row["FLAG"])
                {
                    return;
                }

                if (Boolean.Parse(row["FLAG"].ToString()) == false)//勾选操作
                {
                    if (notCheckedList.Any(p => p["ID"].ToString() == row["ID"].ToString()))
                    {
                        DataRow theRow = notCheckedList.FirstOrDefault(p => p["ID"].ToString() == row["ID"].ToString());
                        if (null != theRow)
                        {
                            notCheckedList.Remove(theRow);
                        }
                    }
                    if (!checkedList.Any(p => p["ID"].ToString() == row["ID"].ToString()))
                    {
                        row["FLAG"] = true;
                        checkedList.Add(row);
                    }
                }
                else//反勾选操作
                {
                    if (checkedList.Any(p => p["ID"].ToString() == row["ID"].ToString()))
                    {
                        DataRow theRow = checkedList.FirstOrDefault(p => p["ID"].ToString() == row["ID"].ToString());
                        if (null != theRow)
                        {
                            checkedList.Remove(theRow);
                        }
                    }
                    if (!notCheckedList.Any(p => p["ID"].ToString() == row["ID"].ToString()))
                    {
                        row["FLAG"] = false;
                        notCheckedList.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增事件 --- 新增组合
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_addGroup_Click(object sender, EventArgs e)
        {
            try
            {
                DiseasesForm frm = new DiseasesForm(m_app, -1);
                frm.StartPosition = FormStartPosition.CenterParent;
                //添加窗体关闭事件 add by cyq 2012-12-06
                frm.FormClosed += new FormClosedEventHandler(DiseasesForm_FormClosed);
                frm.ShowDialog();
                //移除窗体关闭事件 add by cyq 2012-12-06
                frm.FormClosed -= new FormClosedEventHandler(DiseasesForm_FormClosed);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑事件 --- 编辑组合
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_editGroup_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow drow = gridView_groups.GetDataRow(gridView_groups.FocusedRowHandle);
                if (null == drow)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                //验证权限
                string createUser = null == drow["CREATE_USER"] ? "" : drow["CREATE_USER"].ToString();
                string errorStr = CheckDiseaseGroupRight(createUser);
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }
                //弹出窗体
                DiseasesForm frm = new DiseasesForm(m_app, int.Parse(drow["ID"].ToString()));
                frm.StartPosition = FormStartPosition.CenterParent;
                //添加窗体关闭事件 add by cyq 2012-12-06
                frm.FormClosed += new FormClosedEventHandler(DiseasesForm_FormClosed);
                frm.ShowDialog();
                //移除窗体关闭事件 add by cyq 2012-12-06
                frm.FormClosed -= new FormClosedEventHandler(DiseasesForm_FormClosed);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件 --- 删除组合
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_deleteGroup_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow drow = gridView_groups.GetDataRow(gridView_groups.FocusedRowHandle);
                if (null == drow)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                //验证权限
                string createUser = null == drow["CREATE_USER"] ? "" : drow["CREATE_USER"].ToString();
                string errorStr = CheckDiseaseGroupRight(createUser);
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除病种组合 " + drow["NAME"].ToString() + " 吗？", "删除病种组合", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }

                int result = DS_SqlService.DeleteDiseaseGroup(int.Parse(drow["ID"].ToString()));
                if (result == 1)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");
                    gridView_groups.DeleteRow(gridView_groups.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// tab页切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (this.xtraTabControl1.SelectedTabPage == xtraTabPage1)
                {
                    SearchRightData();
                    this.ActiveControl = this.txt_userName;
                }
                else if (this.xtraTabControl1.SelectedTabPage == xtraTabPage2)
                {
                    RefreashGroupsData();
                    this.ActiveControl = this.txt_search;
                }
                //this.ActiveControl = this.xtraTabControl1.SelectedTabPage.Controls[0];
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 弹出窗体关闭事件
        /// 1、若当前所编辑病历记录已归档，则移除该记录
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiseasesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                RefreashGroupsData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 筛选事件 --- 病种组合授权
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-07</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_searchRight_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchRightData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 筛选事件 --- 病种组合维护
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_search_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = this.gridControl_groups.DataSource as DataTable;
                string searchStr = this.txt_search.Text.Trim().Replace("'", "''");
                string filter = string.Format(" NAME like '%{0}%' or PY like '%{0}%' or WB like '%{0}%' or DiseasesGroup like '%{0}%' or MEMO like '%{0}%' ", searchStr);
                if (null != dt && dt.Rows.Count > 0)
                {
                    dt.DefaultView.RowFilter = filter;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 导入历史数据 --- 病种组合维护
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-07</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_importData_Click(object sender, EventArgs e)
        {
            try
            {
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要导入历史数据吗？", "导入历史数据", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }

                #region 导入原病种组合
                string sqlStr1 = " select id,diseasegroup,name from DiseaseGroup ";
                DS_SqlHelper.CreateSqlHelper();
                DataTable dt1 = DS_SqlHelper.ExecuteDataTable(sqlStr1, CommandType.Text);
                if (null != dt1 && dt1.Rows.Count > 0)
                {
                    foreach (DataRow drow in dt1.Rows)
                    {
                        List<DbParameter> parameters = new List<DbParameter>();
                        SqlParameter param1 = new SqlParameter("@name", SqlDbType.Char);
                        SqlParameter param2 = new SqlParameter("@py", SqlDbType.Char);
                        SqlParameter param3 = new SqlParameter("@wb", SqlDbType.Char);
                        //名称、拼音、五笔
                        string groupName = null == drow["NAME"] ? "" : drow["NAME"].ToString();
                        if (string.IsNullOrEmpty(groupName))
                        {
                            continue;
                        }
                        GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);
                        string[] code = shortCode.GenerateStringShortCode(groupName);
                        param1.Value = groupName;
                        parameters.Add(param1);
                        if (null != code && code.Length >= 2)
                        {
                            param2.Value = null == code[0] ? string.Empty : code[0].ToString();
                            param3.Value = null == code[1] ? string.Empty : code[1].ToString();
                            parameters.Add(param2);
                            parameters.Add(param3);
                        }
                        //病种IDs (Y58.501$白喉疫苗接种反应，E70.301$白化病，E25.001$11-羟化酶缺陷，Y58.601$百日咳疫苗反应)
                        string IDs = string.Empty;
                        string IDAndNames = null == drow["diseasegroup"] ? "" : drow["diseasegroup"].ToString();
                        if (string.IsNullOrEmpty(IDAndNames))
                        {
                            continue;
                        }
                        string[] IDAndNameArray = IDAndNames.Split('，');
                        if (null != IDAndNameArray && IDAndNameArray.Length > 0)
                        {
                            foreach (string IDAndName in IDAndNameArray)
                            {
                                string[] str = IDAndName.Split('$');
                                if (null != str && str.Length > 0)
                                {
                                    IDs += str[0] + "$";
                                }
                            }
                            if (!string.IsNullOrEmpty(IDs))
                            {
                                IDs = IDs.Substring(0, IDs.Length - 1);
                            }
                        }
                        SqlParameter param4 = new SqlParameter("@diseaseids", SqlDbType.Char);
                        param4.Value = IDs;
                        parameters.Add(param4);
                        //是否有效
                        SqlParameter param5 = new SqlParameter("@valid", SqlDbType.Int);
                        param5.Value = 1;
                        parameters.Add(param5);
                        //创建人
                        SqlParameter param6 = new SqlParameter("@create_user", SqlDbType.Char);
                        param6.Value = DS_Common.currentUser.Id;
                        parameters.Add(param6);
                        //创建时间
                        SqlParameter param7 = new SqlParameter("@create_time", SqlDbType.Char);
                        param7.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        parameters.Add(param7);
                        DS_SqlService.InsertDiseaseGroup(parameters);
                    }
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("病种组合历史数据导入成功");
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("病种组合不存在历史数据");
                }
                #endregion

                #region 导入原用户&病种组合关系
                int newNum = 1;
                string sqlStr2 = " select id,name,PY,WB,DEPTID,RELATEDISEASE from attendingphysician ";
                DS_SqlHelper.CreateSqlHelper();
                DataTable dt2 = DS_SqlHelper.ExecuteDataTable(sqlStr2, CommandType.Text);
                if (null != dt2 && dt2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt2.Rows)
                    {
                        List<DbParameter> pars = new List<DbParameter>();
                        //匹配组合ID (Y58.601$百日咳疫苗反应，A37.905$百日咳脑病，E25.002$17-羟化酶缺陷)
                        string IDs = string.Empty;
                        string IDAndNames = null == dr["RELATEDISEASE"] ? "" : dr["RELATEDISEASE"].ToString();
                        if (string.IsNullOrEmpty(IDAndNames))
                        {
                            continue;
                        }
                        string[] IDAndNameArray = IDAndNames.Split('，');
                        if (null != IDAndNameArray && IDAndNameArray.Length > 0)
                        {
                            foreach (string IDAndName in IDAndNameArray)
                            {
                                string[] str = IDAndName.Split('$');
                                if (null != str && str.Length > 0)
                                {
                                    IDs += str[0] + "$";
                                }
                            }
                            if (!string.IsNullOrEmpty(IDs))
                            {
                                IDs = IDs.Substring(0, IDs.Length - 1);
                            }
                        }
                        string sqlStr3 = " select * from DiseasesGroup where diseaseids = @diseaseids ";
                        DbParameter[] sqlParams = new DbParameter[]
                        {
                            new SqlParameter("@diseaseids",SqlDbType.Char)
                        };
                        sqlParams[0].Value = IDs;
                        DS_SqlHelper.CreateSqlHelper();
                        DataTable dt3 = DS_SqlHelper.ExecuteDataTable(sqlStr3, sqlParams, CommandType.Text);

                        SqlParameter par1 = new SqlParameter("@groupids", SqlDbType.Char);
                        if (null != dt3 && dt3.Rows.Count > 0)
                        {//该用户对应组合存在
                            par1.Value = null == dt3.Rows[0]["ID"] ? "" : dt3.Rows[0]["ID"].ToString();
                        }
                        else
                        {//该用户对应组合不存在,此时需新增组合
                            #region 新增组合
                            string newGroupID = AddDiseaseGroupUnknow(newNum, IDs);
                            newNum++;
                            if (!string.IsNullOrEmpty(newGroupID))
                            {
                                par1.Value = newGroupID;
                            }
                            #endregion
                        }
                        pars.Add(par1);

                        SqlParameter par2 = new SqlParameter("@userid", SqlDbType.Char);
                        par2.Value = null == dr["ID"] ? "" : dr["ID"].ToString();
                        if (string.IsNullOrEmpty(par2.Value.ToString()))
                        {
                            continue;
                        }
                        pars.Add(par2);
                        SqlParameter par3 = new SqlParameter("@username", SqlDbType.Char);
                        par3.Value = null == dr["NAME"] ? "" : dr["NAME"].ToString();
                        pars.Add(par3);
                        SqlParameter par4 = new SqlParameter("@valid", SqlDbType.Int);
                        par4.Value = 1;
                        pars.Add(par4);
                        SqlParameter par5 = new SqlParameter("@create_user", SqlDbType.Char);
                        par5.Value = DS_Common.currentUser.Id;
                        pars.Add(par5);
                        SqlParameter par6 = new SqlParameter("@create_time", SqlDbType.Char);
                        par6.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pars.Add(par6);

                        DS_SqlService.InsertUserMatchDiseaseGroup(pars);
                    }
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("用户权限历史数据导入成功");
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("用户权限不存在历史数据");
                }
                #endregion

                //隐藏导入历史数据按钮
                this.btn_importData.Visible = false;
                string hideSql = " update appcfg set value = '<mainDoc><isnew>1</isnew><btnflag>0</btnflag></mainDoc>' where configkey = 'ImportDiseasesGroupHistoryData' ";
                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.ExecuteNonQuery(hideSql, CommandType.Text);

                RefreashGroupsData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号 --- 用户列表
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_user_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 序号 --- 组合权限
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_groupRight_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 序号 --- 组合列表
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_groups_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        #endregion

        #region 方法
        /// <summary>
        /// 初始化科室
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        private void InitDepartment()
        {
            try
            {
                lookUpWindowDepartment.SqlHelper = m_app.SqlHelper;

                DataTable Dept = m_app.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);

                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 刷新数据 --- 员工列表
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        private void RefreashUsersData()
        {
            try
            {
                string sqlStr = " select u.id as userid,u.name as username,u.py,u.wb,u.sexy,u.deptID,d1.name as deptName from users u join department d1 on u.deptid=d1.id ";
                List<DbParameter> listPars = new List<DbParameter>();
                if (!string.IsNullOrEmpty(this.lookUpEditorDepartment.CodeValue.Trim()) && this.lookUpEditorDepartment.CodeValue.Trim() != "0000")
                {
                    sqlStr += " and u.deptid = @deptid ";
                    SqlParameter param1 = new SqlParameter("@deptid", SqlDbType.Char, 12);
                    param1.Value = this.lookUpEditorDepartment.CodeValue.Trim();
                    listPars.Add(param1);
                }
                if (!string.IsNullOrEmpty(this.txt_userName.Text.Trim()))
                {
                    sqlStr += " and (u.id like @userName or u.name like @userName or u.py like @userName or u.wb like @userName) ";
                    SqlParameter param2 = new SqlParameter("@userName", SqlDbType.Char, 32);
                    param2.Value = "%" + this.txt_userName.Text.Trim() + "%";
                    listPars.Add(param2);
                }
                sqlStr += " order by deptName,username ";
                DS_SqlHelper.CreateSqlHelper();
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, listPars, CommandType.Text);
                this.gridControl_user.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 刷新数据 --- 组合权限列表
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        private void SearchRightData()
        {
            try
            {
                DataRow drow = gridView_user.GetDataRow(gridView_user.FocusedRowHandle);
                if (null == drow)
                {
                    this.gridControl_groupRight.DataSource = new DataTable();
                    return;
                }
                //查询所有组合
                DataTable dt = DS_SqlService.GetDiseaseGroups();
                //添加组合病种显示列
                dt.Columns.Add("DiseasesGroup");
                if (null != dt && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["DiseasesGroup"] = DS_BaseService.GetDiseaseStringByGroupID(int.Parse(dr["ID"].ToString()));
                    }
                }
                //添加组合权限列
                dt.Columns.Add(new DataColumn("FLAG", typeof(bool)));
                DataRow[] listArry = dt.Select(" 1=1 ");
                if (allSource.Rows.Count == 0)
                {
                    allSource = listArry.Length == 0 ? new DataTable() : listArry.CopyToDataTable();
                }
                //筛选条件
                string searchStr = this.txt_searchRight.Text.ToUpper();//.Replace("'","''");

                checkedList.Clear();
                notCheckedList.Clear();
                //此用户权限内的组合ID集合
                List<string> idList = DS_BaseService.GetDiseaseGroupIDsByUserID(drow["USERID"].ToString().Trim());
                if (null == idList || idList.Count == 0)
                {//此用户没有组合权限
                    foreach (DataRow dr in listArry)
                    {
                        dr["FLAG"] = false;
                    }
                    notCheckedList = listArry.Where(p => p["NAME"].ToString().ToUpper().Contains(searchStr) || p["PY"].ToString().ToUpper().Contains(searchStr) || p["WB"].ToString().ToUpper().Contains(searchStr) || p["DiseasesGroup"].ToString().ToUpper().Contains(searchStr)).OrderBy(q => q["NAME"]).ToList();
                }
                else
                {
                    //已勾选项 不过滤
                    var checkedEnu = listArry.Where(p => null != p["ID"] && idList.Contains(p["ID"].ToString().Trim())).OrderBy(q => q["NAME"]);
                    foreach (DataRow dr in checkedEnu)
                    {
                        dr["FLAG"] = true;
                        checkedList.Add(dr);
                    }
                    //对未勾选项进行过滤
                    var notCheckedEnu = listArry.Where(p => null != p["ID"] && !idList.Contains(p["ID"].ToString().Trim()) && (p["NAME"].ToString().ToUpper().Contains(searchStr) || p["PY"].ToString().ToUpper().Contains(searchStr) || p["WB"].ToString().ToUpper().Contains(searchStr) || p["DiseasesGroup"].ToString().ToUpper().Contains(searchStr))).OrderBy(q => q["NAME"]);
                    foreach (DataRow dr in notCheckedEnu)
                    {
                        dr["FLAG"] = false;
                        notCheckedList.Add(dr);
                    }
                }
                List<DataRow> unionList = checkedList.Union(notCheckedList).ToList();
                DataTable endDt = (null == unionList || unionList.Count() == 0) ? new DataTable() : unionList.CopyToDataTable();
                defaultSource = endDt.Copy();
                this.gridControl_groupRight.DataSource = endDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 刷新数据 --- 病种组合列表
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        private void RefreashGroupsData()
        {
            try
            {
                DataTable dt = DS_SqlService.GetDiseaseGroups();
                dt.Columns.Add("DiseasesGroup");
                if (null != dt && dt.Rows.Count > 0)
                {
                    foreach (DataRow drow in dt.Rows)
                    {
                        drow["DiseasesGroup"] = DS_BaseService.GetDiseaseStringByGroupID(int.Parse(drow["ID"].ToString()));
                    }
                }
                string searchStr = this.txt_search.Text.Trim().Replace("'", "''");
                string filter = string.Format(" NAME like '%{0}%' or PY like '%{0}%' or WB like '%{0}%' or DiseasesGroup like '%{0}%' or MEMO like '%{0}%' ", searchStr);
                dt.DefaultView.RowFilter = filter;
                this.gridControl_groups.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 验证病种组合操作权限
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="createUser">创建人</param>
        /// <returns></returns>
        private string CheckDiseaseGroupRight(string createUser)
        {
            try
            {
                if (string.IsNullOrEmpty(createUser))
                {
                    return "该组合为系统默认组合，您没有操作权限。";
                }
                else if (createUser != DS_Common.currentUser.Id)
                {
                    string nameStr = string.Empty;
                    DataTable userDt = DS_SqlService.GetUserByID(createUser);
                    if (null != userDt && userDt.Rows.Count > 0)
                    {
                        nameStr = userDt.Rows[0]["NAME"].ToString() + "(" + createUser + ")";
                    }
                    else
                    {
                        nameStr = createUser;
                    }
                    return "该组合为 " + nameStr + " 创建，您没有操作权限。";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 新增病种组合 --- 导入历史数据专用
        /// </summary>
        /// <param name="newNum">未知组合序号</param>
        /// <param name="IDs">病种ID集合</param>
        /// <returns></returns>
        private string AddDiseaseGroupUnknow(int newNum, string IDs)
        {
            try
            {
                if (string.IsNullOrEmpty(IDs))
                {
                    return string.Empty;
                }
                List<DbParameter> paramets = new List<DbParameter>();
                SqlParameter pa1 = new SqlParameter("@name", SqlDbType.Char);
                SqlParameter pa2 = new SqlParameter("@py", SqlDbType.Char);
                SqlParameter pa3 = new SqlParameter("@wb", SqlDbType.Char);
                //名称、拼音、五笔
                string groupNameNew = "未知组合" + newNum.ToString();
                GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);
                string[] code = shortCode.GenerateStringShortCode(groupNameNew);
                pa1.Value = groupNameNew;
                paramets.Add(pa1);
                if (null != code && code.Length >= 2)
                {
                    pa2.Value = null == code[0] ? string.Empty : code[0].ToString();
                    pa3.Value = null == code[1] ? string.Empty : code[1].ToString();
                    paramets.Add(pa2);
                    paramets.Add(pa3);
                }
                //病种IDs
                SqlParameter param4 = new SqlParameter("@diseaseids", SqlDbType.Char);
                param4.Value = IDs;
                paramets.Add(param4);
                //是否有效
                SqlParameter param5 = new SqlParameter("@valid", SqlDbType.Int);
                param5.Value = 1;
                paramets.Add(param5);
                //创建人
                SqlParameter param6 = new SqlParameter("@create_user", SqlDbType.Char);
                param6.Value = DS_Common.currentUser.Id;
                paramets.Add(param6);
                //创建时间
                SqlParameter param7 = new SqlParameter("@create_time", SqlDbType.Char);
                param7.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                paramets.Add(param7);
                DS_SqlService.InsertDiseaseGroup(paramets);

                string newGroupID = string.Empty;
                string sqlStr = " select * from DiseasesGroup where diseaseids = @diseaseids ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                     new SqlParameter("@diseaseids",SqlDbType.Char)
                };
                sqlParams[0].Value = IDs;
                DS_SqlHelper.CreateSqlHelper();
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
                if (null != dt && dt.Rows.Count > 0)
                {
                    newGroupID = dt.Rows[0]["ID"].ToString();
                }

                return newGroupID;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        public IPlugIn Run(IEmrHost host)
        {
            PlugIn plg = null;
            try
            {
                plg = new PlugIn(this.GetType().ToString(), this);
                m_app = host;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return plg;
        }
    }
}
