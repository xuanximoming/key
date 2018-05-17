using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Core.Consultation.Dal;
using DrectSoft.Core.Consultation.NEW;
using DrectSoft.Core.Consultation.NEW.Enum;
using DrectSoft.Core.RedactPatientInfo;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
namespace Consultation.NEW
{
    /// <summary>
    /// 会诊申请界面用户控件
    /// </summary>
    public partial class UCConsultationApplyForMultiplyNew : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_app;
        string noofinpat;//病人首页序号
        string consultApplySN = string.Empty;//申请单据号
        DataRow row = null;
        string DefaultLevel = string.Empty;//默认审核医师级别
        //是否必须选择审核医师 申请医师 
        //有上级医师或者该科室有负责人则必须选择 否则根据配置项决定
        bool isNeedVerity;

        /// <summary>
        /// 当前审核人数据表
        /// </summary>
        private DataTable _currentDataDirector = null;

        #region    Enum   xlb  2013-02-18

        public enum SaveType
        {
            /// <summary>
            /// 数据库插入操作
            /// </summary>
            Insert = 1,

            /// <summary>
            /// （会诊申请界面）修改数据
            /// </summary>
            Modify = 2,

            /// <summary>
            /// (会诊记录填写界面) 修改数据
            /// </summary>
            RecordModify = 3
        }

        #endregion

        //申明委托(异步加载数据)
        //delegate void InitApplyDirecotr(string userId);

        #region  方法 by xlb 2013-02-17

        public UCConsultationApplyForMultiplyNew()
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
        /// 初始化基本信息供外部调用
        /// Add xlb 2013-02-17
        /// </summary>
        public void Init(string nOofInpat, IEmrHost app, string consultApplySn, bool readOnly)
        {
            try
            {
                DS_SqlHelper.CreateSqlHelper();
                noofinpat = nOofInpat;
                m_app = app;
                DataAccess.App = m_app;
                InitInpatinfo();
                consultApplySN = consultApplySn;
                Inner(consultApplySn, readOnly);
                this.ActiveControl = textEditLocation;
                #region 屏蔽第三方控件右键菜单 xlb 2013-03-07
                DS_Common.CancelMenu(groupControlApply, contextMenuStrip1);
                DS_Common.CancelMenu(groupControlDepartMent, contextMenuStrip1);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化会诊申请信息方法
        /// Add xlb 2013-02-18
        /// </summary>
        /// <param name="isNew"></param>
        private void Inner(string consultApplySN, bool readOnly)
        {
            try
            {
                InitConsultInfo();
                InitConsultApplyAbstract();
                InitGridControlDataSource();
                //异步加载数据
                //(new InitApplyDirecotr(GetApplyDirector)).BeginInvoke(m_app.User.Id, null, null);
                GetApplyDirector(m_app.User.Id);
                if (!string.IsNullOrEmpty(consultApplySN))
                {
                    SetData();
                }
                //注册事件方法
                RegisterEvent();
                SetControlEdit(readOnly);
                //解决第三方控件异步报错的问题
                //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置窗体是否只读的方法
        /// Add xlb 2013-02-21
        /// </summary>
        private void SetControlEdit(bool readOnly)
        {
            try
            {
                checkEditNormal.Properties.ReadOnly = readOnly;
                checkEditEmergency.Properties.ReadOnly = readOnly;
                memoEditAbstract.Properties.ReadOnly = readOnly;
                memoEditPurpose.Properties.ReadOnly = readOnly;
                dateEditConsultationDate.Properties.ReadOnly = readOnly;
                timeEditConsultationTime.Properties.ReadOnly = readOnly;
                textEditLocation.Properties.ReadOnly = readOnly;
                dateEditApplyDate.Properties.ReadOnly = readOnly;
                timeEditApplyTime.Properties.ReadOnly = readOnly;
                lookUpEditorApplyEmployee.ReadOnly = readOnly;
                lookUpEditorDirector.ReadOnly = readOnly;
                btnSaveConsultapply.Enabled = !readOnly;
                btnTiJiao.Enabled = !readOnly;
                btnAdd.Enabled = !readOnly;
                btnEdit.Enabled = !readOnly;
                btnDelete.Enabled = !readOnly;
                if (readOnly)//只读窗体则隐藏操作列
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
        /// 初始化默认病历摘要 根据配置在住院志中抓取
        /// Add xlb 2013-03-07
        /// </summary>
        private void InitConsultApplyAbstract()
        {
            try
            {
                ConsultCommon consultCommon = new ConsultCommon();
                string consultAbstract = consultCommon.GetConsultApplyAbstract(noofinpat);
                memoEditAbstract.Text = consultAbstract;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 存在记录则抓取并填充
        /// Add xlb 2013-02-21
        /// Modify by xlb 2013-07-19
        /// </summary>
        private void SetData()
        {
            DataSet ds = DrectSoft.Core.Consultation.Dal.DataAccess.GetConsultationDataSet(consultApplySN, "20");//, Convert.ToString((int)ConsultType.More));
            DataTable dtConsultApply = ds.Tables[0];
            DataTable dtConsultApplyDepartment = ds.Tables[1];

            if (dtConsultApply.Rows.Count > 0)
            {
                //紧急度
                if (dtConsultApply.Rows[0]["UrgencyTypeID"].ToString() == Convert.ToString((int)UrgencyType.Normal))
                {
                    checkEditNormal.Checked = true;
                    checkEditEmergency.Checked = false;
                }
                else
                {
                    checkEditNormal.Checked = false;
                    checkEditEmergency.Checked = true;
                }

                //摘要
                memoEditAbstract.Text = dtConsultApply.Rows[0]["Abstract"].ToString();

                //会诊目的要求
                memoEditPurpose.Text = dtConsultApply.Rows[0]["Purpose"].ToString();

                //拟会诊时间
                dateEditConsultationDate.Text = dtConsultApply.Rows[0]["ConsultTime"].ToString().Split(' ')[0];
                timeEditConsultationTime.EditValue = dtConsultApply.Rows[0]["ConsultTime"].ToString().Split(' ')[1];

                //会诊地点
                textEditLocation.Text = dtConsultApply.Rows[0]["ConsultLocation"].ToString().Split(' ')[0];

                //申请时间
                dateEditApplyDate.Text = dtConsultApply.Rows[0]["ApplyTime"].ToString().Split(' ')[0];
                timeEditApplyTime.EditValue = dtConsultApply.Rows[0]["ApplyTime"].ToString().Split(' ')[1];

                //申请医师
                lookUpEditorApplyEmployee.Text = dtConsultApply.Rows[0]["APPLYUSERNAME"].ToString();

                string value = ConsultCommon.GetConfigKey("ConsultAuditConfig").Split('|')[0] == "1" ? "1" : "0";
                if (value == "1")
                {
                    //在当前审核医师列表遍历查询是否有旧数据中指定的审核人没有则把旧数据指定医师名传过来
                    var director = (from dataRow in _currentDataDirector.AsEnumerable()
                                    where dataRow.Field<string>("ID").Equals(dtConsultApply.Rows[0]["AUDITUSERID"].ToString())
                                    select dataRow).FirstOrDefault();
                    //若当前审核人中不包含原指定审核人则附文本
                    if (director == null)
                    {
                        //审核医师
                        lookUpEditorDirector.Text = dtConsultApply.Rows[0]["AUDIONAME"].ToString();
                    }
                    else
                    {
                        //若当前审核列表存在指定审核医师则替换代码
                        lookUpEditorDirector.CodeValue = dtConsultApply.Rows[0]["AUDITUSERID"].ToString();
                    }
                }
            }

            if (dtConsultApplyDepartment.Rows.Count > 0)
            {
                gridControlDepartment.DataSource = dtConsultApplyDepartment;

                m_app.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
            }
        }

        /// <summary>
        /// 保存申请记录
        /// Add xlb 2013-02-21
        /// </summary>
        private void SaveConsultion(string stateId)
        {
            try
            {
                //是否需要审核
                string value = ConsultCommon.GetConfigKey("ConsultAuditConfig").Split('|')[0] == "1" ? "1" : "0";
                DS_SqlHelper.BeginTransaction();
                string consultApplySn = string.Empty;
                if (consultApplySN == string.Empty)//新增或修改时数据只插入到申请部门表
                {
                    consultApplySn = SaveConsultationApply(SaveType.Insert, "", stateId, value);
                    SaveConsultationApplyDept(consultApplySn, 1);

                    consultApplySN = consultApplySn;

                }
                else//修改
                {
                    SaveConsultationApply(SaveType.Modify, consultApplySN, stateId, value);
                    SaveConsultationApplyDept(consultApplySN, 1);

                }
                //当提交状态为待会诊或待审核则插入到实际会诊部门表
                if (stateId == Convert.ToString((int)ConsultStatus.WaitConsultation) || stateId == Convert.ToString((int)ConsultStatus.WaitApprove))
                {
                    SaveConsultationApplyDept(consultApplySN, 2);
                }
                DS_SqlHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                DS_SqlHelper.RollbackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 保存申请部门信息
        /// Add xlb 2013-02-21
        /// typeid：1：表示插入申请部门表 2：实际会诊部门表
        /// </summary>
        /// <param name="consultApplySn"></param>
        private void SaveConsultationApplyDept(string consultApplySn, int typeID/*操作类型1表示申请部门表2表示实际会诊部门*/)
        {
            try
            {
                string sql = string.Format("");
                string sql2 = @"select id from hospitalinfo";
                string hospitalCode = DS_SqlHelper.ExecuteDataTableInTran(sql2, CommandType.Text).Rows[0][0].ToString();
                DataTable dt = gridControlDepartment.DataSource as DataTable;
                string sql3 = string.Format("update consultrecorddepartment d set d.valid='0' where d.consultapplysn={0}", consultApplySn);
                DS_SqlHelper.ExecuteNonQueryInTran(sql3);//避免否决等原因出现的提交导致重复添加同医师
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string orderValue = Convert.ToString(i + 1);

                    string departmentCode = dt.Rows[i]["DepartmentCode"].ToString();
                    string departmentName = dt.Rows[i]["DepartmentName"].ToString();
                    string employeeCode = dt.Rows[i]["EMPLOYEECODE"].ToString();
                    string employeeName = dt.Rows[i]["EmployeeName"].ToString();

                    string employeeLevelID = dt.Rows[i]["employeeLevelID"].ToString();
                    string createUser = m_app.User.Id;
                    string createTime = System.DateTime.Now.ToString();

                    DrectSoft.Core.Consultation.Dal.DataAccess.SaveApplyDepartmentOrRecordDepartment(typeID, consultApplySn, orderValue, hospitalCode, departmentCode, departmentName,
                        employeeCode, employeeName, employeeLevelID, createUser, createTime);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存会诊申请记录信息对应consultapply表
        /// Add xlb 2013-02-21
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="consultApplySN"></param>
        /// <returns></returns>
        private string SaveConsultationApply(SaveType saveType, string consultApplySN, string state, string isNeedVerity/*是否需要审核*/)
        {
            try
            {
                string typeID = Convert.ToString((int)saveType);
                string urgencyTypeID = string.Empty;
                if (checkEditNormal.Checked == true)
                {
                    urgencyTypeID = Convert.ToString((int)UrgencyType.Normal);
                }
                else if (checkEditEmergency.Checked == true)
                {
                    urgencyTypeID = Convert.ToString((int)UrgencyType.Urgency);
                }

                string consultTypeID = "";
                string abstractContent = memoEditAbstract.Text.Trim();
                string purpose = memoEditPurpose.Text;
                string applyUser = lookUpEditorApplyEmployee.CodeValue;
                string applyTime = dateEditApplyDate.Text + " " + timeEditApplyTime.Text;
                string director = lookUpEditorDirector.CodeValue;
                string consultTime = dateEditConsultationDate.Text + " " + timeEditConsultationTime.Text;
                string consultLocation = textEditLocation.Text;

                string stateID = state;
                string createUser = m_app.User.Id;
                string createTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string audituserId = string.Empty;//审核人编号
                string auditlevel = string.Empty;//审核人级别
                string value = isNeedVerity.Trim();
                if (value == "0")//无需审核
                {
                    audituserId = "";
                    auditlevel = "";
                }
                else
                {//需要审核则必定存在审核医师或级别
                    audituserId = lookUpEditorDirector.CodeValue;//审核医生代码
                    string sql = "select grade from users  where id='" + audituserId + "'";
                    DataTable dt = DS_SqlHelper.ExecuteDataTableInTran(sql);
                    if (dt == null || dt.Rows.Count <= 0)
                    {
                        auditlevel = DefaultLevel;
                    }
                    else
                    {
                        auditlevel = dt.Rows[0][0].ToString();
                    }
                    if (string.IsNullOrEmpty(lookUpEditorDirector.CodeValue))//开启审核且没选择审核医生则默认级别
                    {
                        auditlevel = DefaultLevel;//如果审核医生为空
                    }
                }

                //return YD_SqlHelper.e
                return DrectSoft.Core.Consultation.Dal.DataAccess.InsertConsultationApply(typeID, consultApplySN, noofinpat, urgencyTypeID, consultTypeID, abstractContent, purpose,
                    applyUser, applyTime, director, consultTime, consultLocation, stateID, createUser, createTime, m_app.User.CurrentDeptId, audituserId, auditlevel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取系统配置内容
        /// Add xlb 2013-02-19
        /// </summary>
        /// <returns></returns>
        private string GetConfigkeyByKey(string configName)
        {
            try
            {
                //抓取指定配置的值sql
                string sql = @"select value from appcfg where configkey=@configName";
                SqlParameter[] sps = { new SqlParameter("@configName", configName) };
                DataTable dtConfig = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                if (dtConfig == null || dtConfig.Rows.Count <= 0)
                {
                    throw new Exception("" + configName + "系统配置出错");
                }
                string value = dtConfig.Rows[0][0].ToString().Trim();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 1: 需要审核 按钮（保存、提交） 
        /// 2: 不需要审核 按钮（完成）
        /// 3: 不需要审核 按钮（保存、完成）
        /// </summary>
        private string m_AuditState = "1";

        /// <summary>
        /// 针对审核设置按钮样式 Add by wwj 2013-03-04
        /// </summary>
        private void SetBtnStyleForAudit()
        {
            string auditSetting = GetConfigkeyByKey("ConsultAuditConfig");
            string number = auditSetting.Split('|')[0];
            switch (number)
            {
                //需要审核 按钮（保存、提交） 例如：1|保存:6710|提交:6720
                //  点击“保存”状态转为 会诊申请记录保存
                //  点击“提交”状态转为 待审核 
                case "1":
                    btnSaveConsultapply.Visible = true;
                    btnTiJiao.Visible = true;
                    foreach (string setting in auditSetting.Split('|'))
                    {
                        if (setting.Contains(":"))
                        {
                            ConsultStatus status = (ConsultStatus)Enum.Parse(typeof(ConsultStatus), setting.Split(':')[1]);
                            if (status == ConsultStatus.ApplySave)//点击后状态置为“会诊申请记录保存”
                            {
                                btnSaveConsultapply.Text = setting.Split(':')[0];
                            }
                            else if (status == ConsultStatus.WaitApprove)//点击后状态置为“待审核”
                            {
                                btnTiJiao.Text = setting.Split(':')[0];
                            }
                        }
                    }
                    m_AuditState = "1";
                    break;
                //不需要审核	按钮（完成） 例如：2|完成:6730
                //  点击“完成”状态直接转为 待会诊
                case "2":
                    btnSaveConsultapply.Visible = true;
                    btnTiJiao.Visible = false;
                    btnSaveConsultapply.Location = btnTiJiao.Location;
                    btnSaveConsultapply.Text = auditSetting.Split('|')[1].Split(':')[0];
                    m_AuditState = "2";
                    break;
                //不需要审核	按钮（保存、完成） 例如：3|保存:6710|完成:6730
                //  点击“保存”状态转为 会诊申请记录保存 点击“完成”状态转为 待会诊
                case "3":
                    btnSaveConsultapply.Visible = true;
                    btnTiJiao.Visible = true;
                    foreach (string setting in auditSetting.Split('|'))
                    {
                        if (setting.Contains(":"))
                        {
                            ConsultStatus status = (ConsultStatus)Enum.Parse(typeof(ConsultStatus), setting.Split(':')[1]);
                            if (status == ConsultStatus.ApplySave)//点击后状态置为“会诊申请记录保存”
                            {
                                btnSaveConsultapply.Text = setting.Split(':')[0];
                            }
                            else if (status == ConsultStatus.WaitConsultation)//点击后状态置为“待会诊”
                            {
                                btnTiJiao.Text = setting.Split(':')[0];
                            }
                        }
                    }
                    m_AuditState = "3";
                    break;
            }
        }

        /// <summary>
        /// 初始化申请医师下拉列表
        /// Add xlb 2013-02-18
        /// </summary>
        private void InitApplyDoctor()
        {
            try
            {
                lookUpEditorApplyEmployee.Kind = WordbookKind.Sql;
                lookUpEditorApplyEmployee.ListWindow = lookUpWindowApplyEmployee;
                string deptId = m_app.User.CurrentDeptId.ToString();
                //                string sql = @" SELECT u.ID, u.NAME, u.py, u.wb FROM users u where u.deptid = @deptId 
                //                           and u.valid = '1' ORDER BY u.ID";//当前科室的医师
                string sql = @"SELECT ID, NAME, PY, WB,grade FROM users u WHERE deptid =@deptid AND valid = '1' 
                union SELECT ID, NAME, PY, WB,grade FROM users u WHERE  valid = '1'  and exists (SELECT 1 FROM                                  user2dept WHERE user2dept.userid = u.id and user2dept.deptid =@deptid ) ";
                SqlParameter[] sps = { new SqlParameter("@deptId", deptId) };
                DataTable dtApplyDoc = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);

                for (int i = 0; i < dtApplyDoc.Columns.Count; i++)
                {
                    if (dtApplyDoc.Columns[i].ColumnName.ToUpper().Trim() == "ID")
                    {
                        dtApplyDoc.Columns[i].Caption = "代码";
                    }
                    else if (dtApplyDoc.Columns[i].ColumnName.ToUpper().Trim() == "NAME")
                    {
                        dtApplyDoc.Columns[i].Caption = "名称";
                    }
                }
                Dictionary<string, int> colWidths = new Dictionary<string, int>();
                colWidths.Add("ID", 50);
                colWidths.Add("NAME", 74);
                SqlWordbook wordBook = new SqlWordbook("ApplyEmployee", dtApplyDoc, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
                lookUpEditorApplyEmployee.SqlWordbook = wordBook;
                //默认当前登录人为申请人
                lookUpEditorApplyEmployee.CodeValue = m_app.User.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化审核医师列表
        /// 根据配置来获取
        /// Add xlb 2013-02-20
        /// Modified by wwj 2013-03-01
        /// </summary>
        private void GetApplyDirector(string applyCode)
        {
            try
            {
                string value = ConsultCommon.GetConfigKey("ConsultAuditConfig").Split('|')[0] == "1" ? "1" : "0";
                if (value == "0")
                {
                    return;
                }
                lookUpEditorDirector.Kind = WordbookKind.Sql;
                lookUpEditorDirector.ListWindow = lookUpWindowDirector;
                //审核人列表中默认选中的医师
                DataTable dataTable1 = new DataTable();
                //审核人列表中数据源
                DataTable dataTable2 = new DataTable();

                string valueXml = GetConfigkeyByKey("ConsultApplyDirector");
                if (string.IsNullOrEmpty(valueXml))
                {
                    throw new Exception("配置出错");
                }
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(valueXml);
                XmlNodeList xmlList = xdoc.SelectNodes("/Director/value");
                if (xmlList == null || xmlList.Count <= 0)
                {
                    throw new Exception("配置出错");
                }

                foreach (XmlNode item in xmlList)
                {
                    if (item.Attributes["name"].Value == "上级医师审核")
                    {
                        string sql = @"select d.parentuserid as id,users.name,users.py,users.wb from CONSULT_DOCTORPARENT d join users on d.parentuserid=users.id where d.userid=@userID and d.valid='1'";
                        SqlParameter[] sps = { new SqlParameter("@userID", applyCode) };
                        dataTable1 = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                        if (dataTable1 == null || dataTable1.Rows.Count <= 0 || string.IsNullOrEmpty(dataTable1.Rows[0][0].ToString()))
                        {
                            continue;
                        }
                        string isOpenChooseDirector = GetConfigkeyByKey("IsOpenSetDirector");
                        //配置项不开放修改则下拉框只有上级医师或者科室负责人
                        if (isOpenChooseDirector.Trim() != "0")
                        {
                            dataTable2 = dataTable1;
                            //存在上级医师则必须审核
                            isNeedVerity = true;
                            break;
                        }
                        string sql2 = item.InnerText;
                        SqlParameter[] sp = { new SqlParameter("@deptid", m_app.User.CurrentDeptId) };
                        dataTable2 = DS_SqlHelper.ExecuteDataTable(sql2, sp, CommandType.Text);
                        //存在上级医师则必须审核
                        isNeedVerity = true;
                        break;
                    }
                    else if (item.Attributes["name"].Value == "科室负责人审核")
                    {
                        string sql3 = @"select a.userid as id,users.name,users.py,users.wb  from consult_deptautio  a join users on users.id=a.userid  where a.deptid=@deptId and a.valid='1'";
                        SqlParameter[] sp3 = { new SqlParameter("@deptId", m_app.User.CurrentDeptId) };
                        dataTable1 = DS_SqlHelper.ExecuteDataTable(sql3, sp3, CommandType.Text);
                        if (dataTable1 == null || dataTable1.Rows.Count <= 0 || string.IsNullOrEmpty(dataTable1.Rows[0][0].ToString()))
                        {
                            continue;
                        }

                        #region Add by wwj 2013-03-01
                        string isOpenChooseDirector = GetConfigkeyByKey("IsOpenSetDirector");
                        //配置项不开放修改则下拉框只有上级医师或者科室负责人
                        if (isOpenChooseDirector.Trim() != "0")
                        {
                            dataTable2 = dataTable1;
                            //存在上级医师则必须审核
                            isNeedVerity = true;
                            break;
                        }
                        #endregion

                        string sql2 = item.InnerText;
                        SqlParameter[] sp = { new SqlParameter("@deptid", m_app.User.CurrentDeptId) };
                        dataTable2 = DS_SqlHelper.ExecuteDataTable(sql2, sp, CommandType.Text);
                        //存在上级医师则必须审核
                        isNeedVerity = true;
                        break;
                    }
                    else if (item.Attributes["name"].Value == "指定级别医师审核")
                    {
                        string sql = item.InnerText.Trim();
                        SqlParameter[] sps = { new SqlParameter("@deptid", m_app.User.CurrentDeptId) };
                        dataTable2 = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                        if (dataTable2 == null || dataTable2.Rows.Count <= 0)
                        {
                            continue;
                        }

                        //指定级别为审核人时结合配置项是否必须审核人
                        //string value=GetConfigkeyByKey("IsNeedDirector").Trim();
                        string IsNeedDirector = ConsultCommon.GetConfigKey("IsNeedDirector").Trim();
                        //0表示无需选择审核人
                        if (IsNeedDirector == "1")
                        {
                            isNeedVerity = true;//需审核
                        }
                        else
                        {
                            isNeedVerity = false;
                            DefaultLevel = item.Attributes["default"].Value;//配置项默认级别
                        }
                        break;
                    }
                }
                dataTable2.Columns["ID"].Caption = "代码";
                dataTable2.Columns["NAME"].Caption = "名称";
                Dictionary<string, int> colWidths = new Dictionary<string, int>();
                colWidths.Add("ID", 62);
                colWidths.Add("NAME", 90);
                SqlWordbook wordBook = new SqlWordbook("ApplyEmployee", dataTable2, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
                lookUpEditorDirector.SqlWordbook = wordBook;
                if (dataTable1 != null && dataTable1.Rows.Count > 0)
                {
                    lookUpEditorDirector.CodeValue = dataTable1.Rows[0][0].ToString();
                }
                _currentDataDirector = dataTable2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化病人基本信息
        /// Add xlb 2013-02-17
        /// </summary>
        private void InitInpatinfo()
        {
            try
            {
                DataTable dt = DataAccess.GetRedactPatientInfoFrm("14", noofinpat);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return;
                }

                textEditName.Text = dt.Rows[0]["NAME"].ToString().Trim();
                textEditPatientSN.Text = dt.Rows[0]["PatID"].ToString().Trim();
                textEditGender.Text = dt.Rows[0]["Gender"].ToString().Trim();
                textEditAge.Text = dt.Rows[0]["AgeStr"].ToString().Trim();
                textEditBedCode.Text = dt.Rows[0]["OutBed"].ToString().Trim();
                textEditDepartment.Text = dt.Rows[0]["OutHosDeptName"].ToString().Trim();
                textEditMarriage.Text = dt.Rows[0]["Marriage"].ToString().Trim();
                textEditJob.Text = dt.Rows[0]["JobName"].ToString().Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据配置 不需要审核则隐藏审核医师下拉框
        /// </summary>
        private void SetControlStyleForAudit()
        {
            try
            {
                SetBtnStyleForAudit();
                if (m_AuditState == "1")
                {
                    labelControl12.Visible = true;
                    lookUpEditorDirector.Visible = true;
                    labelControl7.Visible = true;
                }
                else
                {
                    labelControl12.Visible = false;
                    lookUpEditorDirector.Visible = false;
                    labelControl7.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化会诊申请相关信息
        /// Add xlb 2013-02-18
        /// </summary>
        private void InitConsultInfo()
        {
            try
            {
                dateEditApplyDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //采用24小时制
                timeEditApplyTime.EditValue = DateTime.Now.ToString("HH:mm");

                dateEditConsultationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                timeEditConsultationTime.EditValue = DateTime.Now.ToString("HH:mm");
                //默认地点为当前登录人科室
                textEditLocation.Text = m_app.User.CurrentDeptName;
                InitApplyDoctor();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 注册窗体事件
        /// Add xlb 2013-02-17
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                btnSearchForPatInfo.Click += new EventHandler(btnSearchForPatInfo_Click);
                btnAdd.Click += new EventHandler(btnAdd_Click);
                btnEdit.Click += new EventHandler(btnEdit_Click);
                btnDelete.Click += new EventHandler(btnDelete_Click);
                btnSaveConsultapply.Click += new EventHandler(btnSaveConsultapply_Click);
                btnClose.Click += new EventHandler(btnClose_Click);
                gridViewDept.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.
                RowIndicatorCustomDrawEventHandler(gridViewDept_CustomDrawRowIndicator);
                checkEditNormal.CheckedChanged += new EventHandler(checkEditNormal_CheckedChanged);
                checkEditEmergency.CheckedChanged += new EventHandler(checkEditEmergency_CheckedChanged);
                checkEditNormal.KeyPress += new KeyPressEventHandler(cbk_KeyPress);
                checkEditEmergency.KeyPress += new KeyPressEventHandler(cbk_KeyPress);
                gridViewDept.MouseDown += new MouseEventHandler(gridViewDept_MouseDown);
                lookUpEditorApplyEmployee.CodeValueChanged += new EventHandler(lookUpEditorApplyEmployee_CodeValueChanged);
                gridControlDepartment.Click += new EventHandler(gridControlDepartment_Click);
                btnTiJiao.Click += new EventHandler(btnTijiao_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 初始化gridControlDepartment数据源
        /// Add xlb 2013-02-18
        /// </summary>
        private void InitGridControlDataSource()
        {
            try
            {
                DataTable dt = new DataTable("Department");
                dt.Columns.Add("DepartmentName");
                dt.Columns.Add("DepartmentCode");
                dt.Columns.Add("EmployeeLevelName");
                dt.Columns.Add("EmployeeLevelID");
                dt.Columns.Add("DeleteButton");
                dt.Columns.Add("EmployeeName");
                dt.Columns.Add("EmployeeNameStr");
                dt.Columns.Add("EMPLOYEECODE");
                gridControlDepartment.DataSource = dt;

                m_app.PublicMethod.ConvertGridDataSourceUpper(gridViewDept);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用于设置按钮可编辑性的方法
        /// Add xlb 2013-02-19
        /// </summary>
        /// <param name="?"></param>
        private void SetEdit(bool isEdit)
        {
            try
            {
                btnAdd.Enabled = isEdit;
                btnEdit.Enabled = isEdit;
                btnDelete.Enabled = isEdit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证界面所有必填选项是否完成输入
        /// Add xlb 2013-02-19
        /// </summary>
        /// <param name="message"></param>
        private bool CheckControl(ref string message)
        {
            try
            {
                DataTable dt = gridControlDepartment.DataSource as DataTable;
                DateTime applyTime = DateTime.Parse(dateEditApplyDate.Text + " " + timeEditApplyTime.Text);
                DateTime consultTime = DateTime.Parse(dateEditConsultationDate.Text + " " + timeEditConsultationTime.Text);
                if (string.IsNullOrEmpty(textEditLocation.Text))
                {
                    message = "会诊地点不能为空";
                    textEditLocation.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(memoEditAbstract.Text))
                {
                    message = "请填写病历摘要";
                    memoEditAbstract.Focus();
                    return false;
                }
                else if (memoEditAbstract.Text.Length > 400)
                {
                    message = "病历摘要不能超过400字";
                    memoEditAbstract.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(memoEditPurpose.Text))
                {
                    message = "会诊目的不能为空";
                    memoEditPurpose.Focus();
                    return false;
                }
                else if (memoEditPurpose.Text.Length > 200)
                {
                    message = "会诊目的不能超过200字";
                    memoEditPurpose.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(dateEditApplyDate.Text))
                {
                    message = "申请时间不能为空";
                    dateEditApplyDate.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(dateEditConsultationDate.Text))
                {
                    message = "拟会诊时间不能为空";
                    dateEditConsultationDate.Focus();
                    return false;
                }
                else if (applyTime > consultTime)
                {
                    message = "申请时间不能大于会诊时间";
                    return false;
                }
                else if (string.IsNullOrEmpty(lookUpEditorApplyEmployee.CodeValue))
                {
                    message = "请选择申请医师";
                    lookUpEditorApplyEmployee.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(lookUpEditorDirector.CodeValue) && isNeedVerity)
                {
                    message = "请选择审核医师";
                    lookUpEditorDirector.Focus();
                    return false;
                }
                else if (dt == null || dt.Rows.Count <= 0)
                {
                    message = "请填写受邀医师记录";
                    btnAdd.Focus();
                    return false;
                }
                else if (dt != null && dt.Rows.Count == 0)
                {
                    message = "请填写受邀医师记录";
                    btnAdd.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 校验保存事件时需录入数据
        /// Add by xlb 2013-03-15
        /// </summary>
        /// <param name="message"></param>
        private bool Validate(ref string message)
        {
            try
            {
                if (string.IsNullOrEmpty(textEditLocation.Text))
                {
                    message = "会诊地点不能为空";
                    textEditLocation.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(dateEditApplyDate.Text))
                {
                    message = "申请时间不能为空";
                    dateEditApplyDate.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(dateEditConsultationDate.Text))
                {
                    message = "拟会诊时间不能为空";
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

        #endregion

        #region 事件 by xlb 2013-02-17

        /// <summary>
        /// 新增受邀医师事件
        /// Add by xlb 2013-02-17
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
                row["EmployeeLevelName"] = "";
                row["EmployeeLevelID"] = "";
                row["DeleteButton"] = "删除";
                row["EmployeeName"] = "";
                row["EmployeeNameStr"] = "";
                row["EMPLOYEECODE"] = "";
                ChooseConsultEmployee chooseEmployee = new ChooseConsultEmployee(m_app, dt, row);
                if (chooseEmployee == null)
                {
                    return;
                }
                chooseEmployee.StartPosition = FormStartPosition.CenterScreen;
                if (chooseEmployee.ShowDialog() == DialogResult.OK)
                {
                    dt.Rows.Add(chooseEmployee.dataRow);
                    gridControlDepartment.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 列表单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                row = gridViewDept.GetFocusedDataRow();
                if (row == null)
                {
                    return;
                }
                //lookUpEditorDepartment.CodeValue = row["DepartmentCode"].ToString();
                //lookUpEditorLevel.CodeValue = row["EmployeeLevelID"].ToString();
                //lookUpEditorDoctor.CodeValue = row["EmployeeID"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 申请医师切换触发的事件
        /// Add xlb 2013-02-19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorApplyEmployee_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                lookUpEditorDirector.Text = string.Empty;
                GetApplyDirector(lookUpWindowApplyEmployee.CodeValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 编辑受邀医师信息事件
        /// Add xlb 2013-02-17
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
                row = gridViewDept.GetDataRow(rowHandel);
                if (row == null)
                {
                    MessageBox.Show("请选择需要编辑的行");
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
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 提交事件
        /// 提交后申请记录状态为待会诊不可
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTijiao_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_AuditState == "1")
                {
                    string message = "";
                    if (!CheckControl(ref message))
                    {
                        MessageBox.Show(message);
                        return;
                    }

                    if (MyMessageBox.Show("是否提交本次会诊申请?", "提示", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon)
                        == DialogResult.Yes)
                    {
                        //执行提交按钮则该会诊申请记录状态为待审核
                        string stateId = Convert.ToString((int)ConsultStatus.WaitApprove);
                        SaveConsultion(stateId);
                        MessageBox.Show("提交成功");
                        this.FindForm().Close();//提交成功后关闭窗体
                    }
                }
                else if (m_AuditState == "3")
                {
                    string message = "";
                    if (!CheckControl(ref message))
                    {
                        //throw new Exception(message);
                        MessageBox.Show(message);
                        return;
                    }

                    if (MyMessageBox.Show("是否提交本次会诊申请?", "提示", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon)
                        == DialogResult.Yes)
                    {
                        //执行提交按钮则该会诊申请记录状态为待会诊
                        string stateId = Convert.ToString((int)ConsultStatus.WaitConsultation);
                        SaveConsultion(stateId);
                        MessageBox.Show("会诊申请成功");//申请成功后关闭窗体
                        this.FindForm().Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 复选框勾选状态相关事件 xlb 2013-02-18

        /// <summary>
        /// 普通会诊勾选状态改变触发的事件
        /// Add xlb 2013-02-18
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEditNormal_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEditNormal.Checked)
                {
                    checkEditEmergency.Checked = false;
                }
                else
                {
                    //避免普通会诊没勾选时两者都未勾选
                    checkEditEmergency.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 复选框回车事件
        /// Add xlb 2013-02-18
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbk_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    DS_Common.cbx_KeyPress(sender);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 紧急会诊勾选状态切换触发事件
        /// Add xlb 2013-02-18
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEditEmergency_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEditEmergency.Checked)
                {
                    checkEditNormal.Checked = false;
                }
                else
                {
                    //保证两者其中一个必须勾选
                    checkEditNormal.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        /// <summary>
        /// 删除受邀医生事件
        /// Add xlb 2013-02-17
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
                DialogResult dialogResult = MessageBox.Show
                ("确定删除该条受邀记录吗？", "提示", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                DataTable dt = gridControlDepartment.DataSource as DataTable;
                //删除指定行再绑定
                dt.Rows.RemoveAt(rowHandel);
                gridControlDepartment.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 删除事件
        /// Add xlb 2013-02-19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDept_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                GridHitInfo hit = gridViewDept.CalcHitInfo(e.X, e.Y);
                if (hit.RowHandle < 0)
                {
                    return;
                }
                DataTable dt = gridControlDepartment.DataSource as DataTable;
                if (null == dt || dt.Rows.Count == 0)
                {
                    return;
                }
                else if (gridViewDept.FocusedRowHandle < 0)
                {
                    throw new Exception("请选择一条记录");
                }
                if (hit.Column != null)
                {
                    if (hit.Column.Name == "DeleteButton")
                    {
                        if (MessageBox.Show("确定删除该条邀请记录？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                        int rowIndex = hit.RowHandle;
                        DataTable dataTableSource = gridControlDepartment.DataSource as DataTable;

                        if (rowIndex >= 0 && rowIndex < dataTableSource.Rows.Count)
                        {
                            dataTableSource.Rows.RemoveAt(rowIndex);
                            dataTableSource.AcceptChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 保存会诊申请记录事件
        /// 开启审核则保存记录为会诊记录保存否则为待会诊
        /// Add xlb 2013-02-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveConsultapply_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_AuditState == "1" || m_AuditState == "3")//需要审核时，点击按钮状态改为“会诊申请记录保存”
                {
                    string message = "";
                    if (!Validate(ref message))
                    {
                        MessageBox.Show(message);
                        return;
                    }
                    string stateId = Convert.ToString((int)ConsultStatus.ApplySave);
                    SaveConsultion(stateId);
                    MessageBox.Show("保存成功");
                }
                else if (m_AuditState == "2")//不需要审核时，点击按钮状态改为“待会诊”
                {
                    string message = "";
                    if (!CheckControl(ref message))
                    {
                        MessageBox.Show(message);
                        return;
                    }
                    //当前不需审核则保存状态为待会诊
                    string stateId = Convert.ToString((int)ConsultStatus.WaitConsultation);
                    SaveConsultion(stateId);
                    if (MessageBox.Show("保存成功，是否关闭？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.FindForm().Close();
                    }
                }
                #region 注销 by xlb 2013-03-15 此时情况同
                //else if (m_AuditState == "3")//不需要审核时，点击按钮状态改为“会诊申请记录保存”
                //{
                //    string fmessage = "";
                //    if (!Validate(ref fmessage))
                //    {
                //        MessageBox.Show(fmessage);
                //        return;
                //    }
                //    string stateId = Convert.ToString((int)ConsultStatus.ApplySave);
                //    SaveConsultion(stateId);
                //    MessageBox.Show("保存成功");
                //}
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 关闭会诊申请窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("是否确定退出此界面？", "提示", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                //检索用户控件所在的窗体存在在关闭
                Form form = this.FindForm();
                if (form == null)
                {
                    return;
                }
                form.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 病人基本信息相关操作

        /// <summary>
        /// 查询病人详细信息事件
        /// Add xlb 2013-02-18
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchForPatInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(noofinpat))
                {
                    MessageBox.Show("病人基本信息丢失");
                    return;
                }
                XtraFormPatientInfo patientInfo = new XtraFormPatientInfo(m_app, noofinpat);
                if (patientInfo == null)
                {
                    return;
                }
                //避免弹出窗体在别处打开
                patientInfo.TopMost = true;
                patientInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        /// <summary>
        /// 加序号列
        /// Add xlb 2013-02-18
        /// </summary>
        private void gridViewDept_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.
        Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 窗体加载事件
        /// 是否需要审核0为无需审核其余默认需要审核
        /// Add xlb 2013-02-19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCConsultationApplyForMultiply_Load(object sender, EventArgs e)
        {
            try
            {
                DS_SqlHelper.CreateSqlHelper();
                SetControlStyleForAudit();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 绘制单元格样式事件
        /// 针对医师级别区分前景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDept_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                //if (e.RowHandle < 0)
                //{
                //    return;
                //}
                //DataRow row = gridViewDept.GetDataRow(e.RowHandle) as DataRow;
                //if (row == null || row.ItemArray.Length < 0)
                //{
                //    return;
                //}
                //string levelId = row["EmployeeLevelID"] == null ? "" : row["EmployeeLevelID"].ToString();
                //DoctorGrade doctorGrade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), levelId);
                //switch (doctorGrade)
                //{
                //    //主任级别
                //    case DoctorGrade.Director:
                //        e.Appearance.ForeColor = Color.Red;
                //        break;
                //    //副主任级别
                //    case DoctorGrade.ViceDirector:
                //        e.Appearance.ForeColor = Color.Green;
                //        break;
                //    //主治医师级别
                //    case DoctorGrade.StaffPhysician:
                //        e.Appearance.ForeColor = Color.Blue;
                //        break;
                //    default://住院医师或其他级别
                //        e.Appearance.ForeColor = Color.Orange;
                //        break;
                //}
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}
