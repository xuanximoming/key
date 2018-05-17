using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Core;
using System.Xml;
using System.Data.SqlClient;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.MainEmrPad.HelpForm
{
    public partial class OperForm : DevBaseForm
    {
        #region SQL
        //增加别名字段，用于
        private const string SqlPatDiag = "select e.id, e.diag_type_name /*诊断类型*/," +
            " e.diag_no/*序号*/,e.DIAG_SUB_NO/*子序号*/,e.ID/*节点ID*/, e.PARENT_ID /*父节点ID*/," +
            " e.DIAG_CONTENT/*诊断名称*/, TO_CHAR(e.CONFIRMED_FLAG) CONFIRMED_FLAG /*确诊*/," +
            " to_char(e.DIAG_DATE, 'yyyy-MM-dd') DIAG_DATE/*诊断日期*/, e.DIAG_CODE/*诊断编码*/, " +
            " e.diag_doctor_id/*经治医师CODE*/, u1.name diag_doctor/*经治医师*/, " +
            " e.houseman houseman_id/*实习医师CODE*/, u2.name hoseman/*实习医师*/, " +
            " e.super_id/*主任医师CODE*/, u4.name super/*主任医师*/ ,nvl(e.DiagOtherName,e.DIAG_CONTENT) DiagOtherName,/*诊断别名，真正显示使用的*/  to_char(e.super_sign_date, 'yyyy-MM-dd') super_sign_date /*主任审核时间*/," +
            " e.remark/*备注*/ ,TO_CHAR(e.back_diag) BACK_DIAG /*置后*/" +
            " from PATDIAG e" +
            " left outer join users u1 on u1.id = e.diag_doctor_id and u1.valid = '1' " +
            " left outer join users u2 on u2.id = e.houseman       and u2.valid = '1' " +
            " left outer join users u4 on u4.id = e.super_id       and u4.valid = '1' " +
            " where e.patient_id ={0} and e.diag_type_name = '{1}' order by e.diag_no, e.DIAG_SUB_NO; ";

        private const string SqlPatOper = @"SELECT iem.id iem_mainpage_operation_no,iem.patient_id iem_mainpage_no,
             iem.operation_code,  iem.operation_date,  iem.operation_name, u1.name execute_user1_Name,iem.execute_user1,
             u2.name  execute_user2_Name, iem.execute_user2, u3.name execute_user3_Name, iem.execute_user3,
             ab.name anaesthesia_type_Name, iem.anaesthesia_type_id, (case when iem.close_level = '1' then
                'I/甲' when iem.close_level = '2' then 'II/甲' when iem.close_level = '3' then 'III/甲'
               when iem.close_level = '4' then 'I/乙'  when iem.close_level = '5' then   'II/乙'
               when iem.close_level = '6' then 'III/乙'  when iem.close_level = '7' then  'I/丙'
               when iem.close_level = '8' then 'II/丙'   when iem.close_level = '9' then  'III/丙'
               else  '' end) close_level_Name, iem.operation_level, (case when iem.operation_level = '1' then 
                '一级手术'  when iem.operation_level = '2' then  '二级手术'  when iem.operation_level = '3' then
                '三级手术' when iem.operation_level = '4' then  '四级手术' else ''  end) operation_level_Name,
             iem.close_level, ua.name anaesthesia_user_Name, iem.anaesthesia_user, iem.valide, iem.create_user,
             iem.create_time, iem.ischoosedate, iem.isclearope, iem.isganran, (case when iem.ischoosedate = '1' then
                '是' when iem.ischoosedate = '2' then '否' when iem.ischoosedate = '0' then '未知' else '' end) ischoosedatename,
             (case when iem.isclearope = '1' then '是' when iem.isclearope = '2' then  '否' when iem.isclearope = '0' then
                '未知' else '' end) isclearopename, (case when iem.isganran = '1' then   '是'  when iem.isganran = '2' then
                '否'  when iem.isganran = '0' then   '未知'   else  ''  end) isganranname ,iem.anesthesia_level,
             iem.opercomplication_code FROM patoperation iem left join users u1  on iem.execute_user1 = u1.id
         and u1.valid = 1  left join users u2 on iem.execute_user2 = u2.id and u2.valid = 1 left join users u3
          on iem.execute_user3 = u3.id and u3.valid = 1 left join users ua on iem.anaesthesia_user = ua.id
         and ua.valid = 1 left join anesthesia ab on iem.anaesthesia_type_id = ab.id WHERE valide = 1
         and iem.patient_id = '{0}' and iem.nad='{1}';";

        private const string SqlPatDiagType = " SELECT code, diagname, typeid FROM patdiagtype ";

        private const string SqlCancelDiag = "delete from PATDIAG where patient_id = {0} and nad = {1} and diag_type = {2} ";
        //保存前先清空手术信息
        private const string SqlCancelOper = "delete from PATOPERATION where patient_id = {0} and nad = {1}";

        private const string SqlInsertDiag = " INSERT INTO PATDIAG " +
            " (patient_id, nad, diag_type, diag_type_name, diag_no, " +
            " diag_sub_no, diag_class, diag_code, diag_content, diag_date, " +
            " diag_doctor_id, modify_doctor_id, last_time, parent_id, " +
            " super_id, super_sign_date, flag, houseman, confirmed_flag, ID, " +
            " uncertain_diag, back_diag, remark,DiagOtherName" +
            " ) " +
            " VALUES ('{0}', {1}, '{2}', '{3}', {4}, " +
            " {5}, '{6}', '{7}', '{8}', {9}, " +
            " '{10}', '{11}', {12}, {13}, " +
            " '{14}', {15}, '{16}', '{17}', {18}, {19}, " +
            " {20}, {21}, '{22}','{23}' " +
            " );";

        private const string SqlInsertOper = @"  insert into PATOPERATION(ID, PATIENT_ID, NAD, OPERATION_NO, OPERATION_CODE, 
                                                         OPERATION_NAME, OPERATION_DATE, EXECUTE_USER1, EXECUTE_USER2, EXECUTE_USER3, 
                                                         ANAESTHESIA_TYPE_ID, CLOSE_LEVEL, ANAESTHESIA_USER, VALIDE, CREATE_USER, 
                                                         CREATE_TIME, OPERATION_LEVEL, ISCHOOSEDATE, ISCLEAROPE, ISGANRAN, ANESTHESIA_LEVEL, 
                                                         OPERCOMPLICATION_CODE)values(seq_patoperation.nextval, '{0}', '{1}', '{2}', '{3}',
                                                         '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', 
                                                         TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'),'{14}', 
                                                         '{15}', '{16}', '{17}', '{18}', '{19}')";

        string SqlInsertIEMOper = @"  INSERT INTO {0}(iem_mainpage_operation_no, iem_mainpage_no, operation_code,
                                          operation_date,operation_name, execute_user1, execute_user2, execute_user3, 
                                          anaesthesia_type_id, close_level, anaesthesia_user,valide, create_user, 
                                          create_time, OPERATION_LEVEL, IsChooseDate, IsClearOpe, IsGanRan, anesthesia_level, 
                                          opercomplication_code)VALUES(seq_iem_mainpage_operation_id.NEXTVAL,'{1}','{2}','{3}',
                                          '{4}','{5}','{6}','{7}','{8}','{9}','{10}',1,'{11}',TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'),
                                          '{12}','{13}','{14}','{15}','{16}','{17}')";

        string SqlInsertIEMOperSX = @"  INSERT INTO {0}(iem_mainpage_operation_no, iem_mainpage_no, operation_code,
                                          operation_date,operation_name, execute_user1, execute_user2, execute_user3, 
                                          anaesthesia_type_id, close_level, anaesthesia_user,valide, create_user, create_time,
                                          OPERATION_LEVEL)VALUES(seq_iem_mainpage_operation_id.NEXTVAL,'{1}','{2}','{3}', '{4}','{5}',
                                          '{6}','{7}','{8}','{9}','{10}',1,'{11}',TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'),'{12}')";
        #endregion

        #region FIELD && PROPERTY
        private IEmrHost m_App;

        /// <summary>
        /// 诊断名称
        /// </summary>
        private string m_DiagName = string.Empty;

        /// <summary>
        /// 诊断名称对应的类别号
        /// </summary>
        private string m_DiagTypeID = string.Empty;

        private bool m_IsInsert = false;

        int m_DiagContentRemarkIndex = 0;
        string m_DiagContentRemark = string.Empty;

        private DictionaryDataSerach m_DataSearch;
        private DoctorCustomForm m_DoctorCustomForm;

        #endregion

        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public OperForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="app"></param>
        /// <param name="diagName">诊断名称</param>
        public OperForm(IEmrHost app, string diagName)
            : this()
        {
            m_App = app;
            m_DiagName = diagName;
        }

        #endregion

        #region METHOD && EVENT

        #region 设置诊断名称 【供外部调用】
        /// <summary>
        /// 设置诊断名称
        /// </summary>
        /// <param name="name"></param>
        public void SetDiagName(string name)
        {
            m_DiagName = name;
        }
        #endregion

        #region 按节点的顺序得到界面中的诊断 【供外部调用】
        /// <summary>
        /// 按节点的顺序得到界面中的诊断  初步诊断，如果有子诊断的话，子诊断以几点几开头，如：诊断为“1 诊断名”，子诊断为“1.1 诊断名 1.2诊断名”等
        /// </summary>
        /// <returns></returns>
        public string GetDiag()
        {
            if (m_IsInsert)
            {
                m_DiagContentRemarkIndex = 0;
                m_DiagContentRemark = GetDiagContentRemark();
                return m_DiagContentRemark;
            }
            return "";
        }

        /// <summary>
        /// 通过递归按照节点的顺序得到所有节点的诊断 初步诊断，如果有子诊断的话，子诊断以几点几开头，如：诊断为“1 诊断名”，子诊断为“1.1 诊断名 1.2诊断名”等
        /// </summary>
        /// 二次修改 ywk 2013年2月19日15:57:07  
        /// <param name="node"></param>
        private string GetDiagContentRemark()
        {
            try
            {
                //此处根据配置，判断返回的诊断内容是结构化元素还是自由文本
                string diagContentType = BasicSettings.GetStringConfig("IsSetDiagContentStr") == "" ? "1" : BasicSettings.GetStringConfig("IsSetDiagContentStr");
                foreach (DataRow dr in ((DataTable)gridControl2.DataSource).Rows)
                {
                    if (diagContentType == "1")//结构化元素
                    {
                        m_DiagContentRemark += "、" + dr["Operation_Name"].ToString();

                    }
                    else//自由文本
                    {
                        m_DiagContentRemark += dr["Operation_Name"].ToString() + "\r\n";//GetDiagContentRemartInner(dr) + "\r\n";
                    }
                }
                if (diagContentType == "1")
                {
                    return m_DiagContentRemark.Substring(1);
                }
                return m_DiagContentRemark;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetDiagContentRemartInner(DataRow dr)
        {
            try
            {
                //string diagcode = dr["Operation_Code"].ToString();//subNode.GetValue("DIAG_CODE").ToString();
                //DataTable dtSource = subNode.TreeList.DataSource as DataTable;
                //string diagOtherName = string.Empty;
                ////DataRow dr = new DataRow();
                //if (dtSource != null && dtSource.Rows.Count > 0)
                //{
                //    DataRow dr = dtSource.Select(string.Format("DIAG_CODE='{0}'", diagcode))[0];

                //    //这个名称是可以编辑的最终返回的add byy ywk 2013年1月6日9:48:28
                //    //string diagOtherName = subNode.GetValue("DiagOtherName").ToString() == "" ? subNode.GetValue("DIAG_CONTENT").ToString() :           subNode.GetValue("DiagOtherName").ToString();
                //    diagOtherName = dr["DIAGOTHERNAME"].ToString();
                //}
                //string diagName = subNode.GetValue("DIAG_CONTENT").ToString();
                //string remark = subNode.GetValue("REMARK").ToString();

                //string contentIndex = string.Empty;

                //if (subNode.ParentNode == null)
                //{
                //    contentIndex = Convert.ToString(++m_DiagContentRemarkIndex);
                //}
                //else
                //{
                //    if (subNode.PrevNode == null)
                //    {
                //        contentIndex = subNode.ParentNode.Tag.ToString() + ".1";
                //    }
                //    else
                //    {
                //        contentIndex = subNode.PrevNode.Tag.ToString();
                //        string[] indexs = contentIndex.Split('.');
                //        string index = indexs[indexs.Length - 1];
                //        indexs[indexs.Length - 1] = Convert.ToString(int.Parse(index) + 1);
                //        contentIndex = string.Join(".", indexs);
                //    }
                //}

                //subNode.Tag = contentIndex;
                //if (subNode.GetValue("BACK_DIAG").ToString() == "0")
                //{
                //    contentIndex = contentIndex + " " + remark + " " + diagOtherName + " ";//diagName
                //}
                //else
                //{
                //    contentIndex = contentIndex + " " + diagOtherName + " " + remark + " ";
                //}
                //return contentIndex;
                return "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 初始化并绑定控件的数据源
        private void DiagForm_Load(object sender, EventArgs e)
        {
            try
            {
                Init();
                BindData();
                //if (m_DataSearch == null) 由于诊断列表要根据诊断类型判断是西医诊断还是中医诊断，所以每次打开m_DataSearch界面需要重新加载数据
                {
                    m_DataSearch = new DictionaryDataSerach(m_App, m_DiagName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Init()
        {
            try
            {
                m_DiagContentRemark = "";
                m_IsInsert = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 绑定界面中控件的数据源
        /// </summary>
        private void BindData()
        {
            try
            {
                //初始化诊断类别下拉框
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(SqlPatDiagType, CommandType.Text);
                lookUpEditDiagType.Properties.ValueMember = "CODE";
                lookUpEditDiagType.Properties.DisplayMember = "DIAGNAME";
                lookUpEditDiagType.Properties.BeginUpdate();
                lookUpEditDiagType.Properties.DataSource = dt;
                lookUpEditDiagType.Properties.EndUpdate();

                if (!string.IsNullOrEmpty(m_DiagName))
                {
                    lookUpEditDiagType.EditValue = GetLookUpEditValue();
                    lookUpEditDiagType.Enabled = false;
                }

                //初始化手术列表
                string sqlPatOper = string.Format(SqlPatOper, m_App.CurrentPatientInfo.NoOfFirstPage, m_App.CurrentPatientInfo.TimesOfAdmission);

                DataTable dtOperList = m_App.SqlHelper.ExecuteDataTable(sqlPatOper, CommandType.Text);
                gridControl2.DataSource = dtOperList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 得到手术名称对应的CODE
        /// </summary>
        /// <returns></returns>
        private string GetLookUpEditValue()
        {
            try
            {
                string name = m_DiagName;
                DataTable dt = lookUpEditDiagType.Properties.DataSource as DataTable;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["diagname"].ToString().Trim() == name.Trim())
                    {
                        m_DiagTypeID = dr["code"].ToString(); //dr["typeid"].ToString();
                        return dr["code"].ToString();
                    }
                }
                return "";
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region

        #endregion

        #region 点击保存按钮
        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (GetIemMainPageNo() == "")
                {
                    return;
                }
                Save(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="showMessageBox">显示提示框否</param>
        private void Save(bool showMessageBox)
        {
            DataTable dtOper = (DataTable)gridControl2.DataSource;
            try
            {
                m_App.SqlHelper.BeginTransaction();

                string patientID = m_App.CurrentPatientInfo.NoOfFirstPage.ToString();
                string nad = m_App.CurrentPatientInfo.TimesOfAdmission.ToString();

                //先清空【表：PATOPERATION】中针对该病人的所有手术
                m_App.SqlHelper.ExecuteNoneQuery(
                    string.Format(SqlCancelOper, patientID, nad, m_DiagTypeID), CommandType.Text);

                SaveOper(dtOper);

                m_App.SqlHelper.CommitTransaction();

                if (showMessageBox)
                {
                    m_App.CustomMessageBox.MessageShow("保存成功!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                }
            }
            catch (Exception ex)
            {
                m_App.SqlHelper.RollbackTransaction();
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveOper(DataTable dtOper)
        {
            try
            {
                for (int i = 0; i < dtOper.Rows.Count; i++)
                {
                    SaveData(dtOper.Rows[i], i.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取病案首页
        /// </summary>
        /// <returns></returns>
        private string GetIemTableName()
        {
            try
            {
                string tablename = string.Empty;//用于处理两个病案首页诊断信息的处理，区分表名 首页的表
                string containbase = string.Empty;
                string opertable = string.Empty;//诊断的表
                string searchdic = string.Format(@"select mname from dict_catalog where ccode='AA'");
                DataTable dtse = m_App.SqlHelper.ExecuteDataTable(searchdic, CommandType.Text);//查询病案首页的值
                if (dtse.Rows.Count > 0)
                {
                    containbase = dtse.Rows[0]["mname"].ToString();
                }
                if (containbase.Contains("SX"))
                {
                    tablename = "IEM_MAINPAGE_BASICINFO_SX";
                }
                else
                {
                    tablename = "IEM_MAINPAGE_BASICINFO_2012";
                }
                return tablename;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取病案首页手术表
        /// </summary>
        /// <returns></returns>
        private string GetIemOperTableName()
        {
            try
            {
                if (GetIemTableName().Contains("SX"))
                {
                    return "iem_mainpage_operation_sx";
                }
                else
                {
                    return "iem_mainpage_operation_2012";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取首页序号
        /// </summary>
        /// <returns></returns>
        private string GetIemMainPageNo()
        {
            try
            {
                //病人的首页序号
                string iem_mainpage_no = string.Empty;
                string searchdic = GetIemTableName();
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(string.Format(@"select  iem_mainpage_no from  {0}
                    where noofinpat='{1}'", searchdic, m_App.CurrentPatientInfo.NoOfFirstPage), CommandType.Text);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("请先完成病案首页信息");
                    return "";
                }
                if (dt.Rows.Count > 0)
                {
                    iem_mainpage_no = dt.Rows[0]["iem_mainpage_no"].ToString();
                }
                return iem_mainpage_no;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SaveData(DataRow row, string number)
        {
            try
            {

                string patientID = m_App.CurrentPatientInfo.NoOfFirstPage.ToString();
                string nad = m_App.CurrentPatientInfo.TimesOfAdmission.ToString();
                string operNo = number;//手术序号
                string operCode = row["Operation_Code"].ToString();//手术ICD编码
                string operName = row["Operation_Name"].ToString();//手术名称
                string operDate = row["Operation_Date"].ToString();//手术日期
                string operDoctorID = row["Execute_User1"].ToString();//手术医师
                string executUser1 = row["Execute_User2"].ToString();//一助
                string executUser2 = row["Execute_User3"].ToString();//二助
                string anaesthesiaType = row["Anaesthesia_Type_Id"].ToString();//麻醉
                string closeLevel = row["Close_Level"].ToString();//愈合等级                
                string anaesthesiaUser = row["Anaesthesia_User"].ToString();//麻醉师
                string valid = "1";
                string createUser = m_App.User.Id;
                string operationLevel = row["operation_level"].ToString();//手术等级
                string isChooseDate = row["IsChooseDate"].ToString();//是否择期手术
                string isClearOpe = row["IsClearOpe"].ToString();//是否无菌手术
                string isGanRan = row["IsGanRan"].ToString();//是否感染
                string anesthesiaLevel = row["Anesthesia_Level"].ToString();//麻醉分级
                string operComplicationCode = row["OperComplication_Code"].ToString();//手术并发症


                string sqlInsert = string.Format(SqlInsertOper,
                        patientID, nad, operNo, operCode, operName, operDate,
                        operDoctorID, executUser1, executUser2, anaesthesiaType, closeLevel,
                        anaesthesiaUser, valid, createUser, operationLevel, isChooseDate,
                        isClearOpe, isGanRan, anesthesiaLevel, operComplicationCode);
                m_App.SqlHelper.ExecuteNoneQuery(sqlInsert, CommandType.Text);


                #region 加入到PATOPERATION表后，要相应的加到病案首页中去

                string tablename = GetIemOperTableName();
                string sqlInsertIem = string.Empty;
                if (tablename.Contains("sx"))
                {
                    sqlInsertIem = string.Format(SqlInsertIEMOperSX, tablename, GetIemMainPageNo(),
                    operCode, operDate, operName, operDoctorID, executUser1, executUser2, anaesthesiaType, closeLevel,
                    anaesthesiaUser, createUser, operationLevel);
                }
                else
                {
                    sqlInsertIem = string.Format(SqlInsertIEMOper, tablename, GetIemMainPageNo(),
                    operCode, operDate, operName, operDoctorID, executUser1, executUser2, anaesthesiaType, closeLevel,
                    anaesthesiaUser, createUser, operationLevel, isChooseDate, isClearOpe, isGanRan, anesthesiaLevel, operComplicationCode);
                }
                m_App.SqlHelper.ExecuteNoneQuery(sqlInsertIem, CommandType.Text);
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }



        private DataTable _DataOper = GetNewTable();
        private DataTable m_DataOper
        {
            get { return _DataOper; }
            set { _DataOper = value; }
        }
        private static DataTable GetNewTable()
        {
            try
            {
                DataTable m_DataOper = new DataTable();
                #region
                if (!m_DataOper.Columns.Contains("Operation_Code"))
                    m_DataOper.Columns.Add("Operation_Code");
                if (!m_DataOper.Columns.Contains("Operation_Date"))
                    m_DataOper.Columns.Add("Operation_Date");
                if (!m_DataOper.Columns.Contains("Operation_Name"))
                    m_DataOper.Columns.Add("Operation_Name");

                if (!m_DataOper.Columns.Contains("operation_level"))
                    m_DataOper.Columns.Add("operation_level");
                if (!m_DataOper.Columns.Contains("operation_level_Name"))
                    m_DataOper.Columns.Add("operation_level_Name");

                if (!m_DataOper.Columns.Contains("Execute_User1"))
                    m_DataOper.Columns.Add("Execute_User1");
                if (!m_DataOper.Columns.Contains("Execute_User1_Name"))
                    m_DataOper.Columns.Add("Execute_User1_Name");

                if (!m_DataOper.Columns.Contains("Execute_User2"))
                    m_DataOper.Columns.Add("Execute_User2");
                if (!m_DataOper.Columns.Contains("Execute_User2_Name"))
                    m_DataOper.Columns.Add("Execute_User2_Name");

                if (!m_DataOper.Columns.Contains("Execute_User3"))
                    m_DataOper.Columns.Add("Execute_User3");
                if (!m_DataOper.Columns.Contains("Execute_User3_Name"))
                    m_DataOper.Columns.Add("Execute_User3_Name");

                if (!m_DataOper.Columns.Contains("Anaesthesia_Type_Id"))
                    m_DataOper.Columns.Add("Anaesthesia_Type_Id");
                if (!m_DataOper.Columns.Contains("Anaesthesia_Type_Name"))
                    m_DataOper.Columns.Add("Anaesthesia_Type_Name");

                if (!m_DataOper.Columns.Contains("Close_Level"))
                    m_DataOper.Columns.Add("Close_Level");
                if (!m_DataOper.Columns.Contains("Close_Level_Name"))
                    m_DataOper.Columns.Add("Close_Level_Name");

                if (!m_DataOper.Columns.Contains("Anaesthesia_User"))
                    m_DataOper.Columns.Add("Anaesthesia_User");
                if (!m_DataOper.Columns.Contains("Anaesthesia_User_Name"))
                    m_DataOper.Columns.Add("Anaesthesia_User_Name");
                //新增是否择期手术是否无菌手术是否感染
                if (!m_DataOper.Columns.Contains("IsChooseDateName"))
                    m_DataOper.Columns.Add("IsChooseDateName");
                if (!m_DataOper.Columns.Contains("IsClearOpeName"))
                    m_DataOper.Columns.Add("IsClearOpeName");
                if (!m_DataOper.Columns.Contains("IsGanRanName"))
                    m_DataOper.Columns.Add("IsGanRanName");
                if (!m_DataOper.Columns.Contains("IsChooseDate"))
                    m_DataOper.Columns.Add("IsChooseDate");
                if (!m_DataOper.Columns.Contains("IsClearOpe"))
                    m_DataOper.Columns.Add("IsClearOpe");
                if (!m_DataOper.Columns.Contains("IsGanRan"))
                    m_DataOper.Columns.Add("IsGanRan");
                if (!m_DataOper.Columns.Contains("Anesthesia_Level"))
                    m_DataOper.Columns.Add("Anesthesia_Level");
                if (!m_DataOper.Columns.Contains("OperComplication_Code"))
                    m_DataOper.Columns.Add("OperComplication_Code");
                return m_DataOper;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void GetOperationUI()
        {
            try
            {
                if (this.gridControl2.DataSource != null)
                {
                    //手术

                    DataTable dtOperation = m_DataOper.Clone();
                    dtOperation.Rows.Clear();

                    DataTable dataTable = this.gridControl2.DataSource as DataTable;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        DataRow imOut = dtOperation.NewRow();

                        imOut["Operation_Code"] = row["Operation_Code"].ToString();
                        imOut["Operation_Name"] = row["Operation_Name"].ToString();
                        imOut["Operation_Date"] = row["Operation_Date"].ToString();

                        imOut["operation_level"] = row["operation_level"].ToString();
                        imOut["operation_level_Name"] = row["operation_level_Name"].ToString();

                        imOut["Execute_User1"] = row["Execute_User1"].ToString();
                        imOut["Execute_User1_Name"] = row["Execute_User1_Name"];
                        imOut["Execute_User2"] = row["Execute_User2"].ToString();
                        imOut["Execute_User2_Name"] = row["Execute_User2_Name"].ToString();
                        imOut["Execute_User3"] = row["Execute_User3"].ToString();
                        imOut["Execute_User3_Name"] = row["Execute_User3_Name"].ToString();
                        imOut["Anaesthesia_Type_Id"] = row["Anaesthesia_Type_Id"].ToString();
                        imOut["Anaesthesia_Type_Name"] = row["Anaesthesia_Type_Name"].ToString();
                        imOut["Close_Level"] = row["Close_Level"].ToString();
                        imOut["Close_Level_Name"] = row["Close_Level_Name"].ToString();
                        imOut["Anaesthesia_User"] = row["Anaesthesia_User"].ToString();
                        imOut["Anaesthesia_User_Name"] = row["Anaesthesia_User_Name"].ToString();

                        imOut["IsChooseDate"] = row["IsChooseDate"].ToString();
                        imOut["IsClearOpe"] = row["IsClearOpe"].ToString();
                        imOut["IsGanRan"] = row["IsGanRan"].ToString();
                        imOut["IsChooseDateName"] = row["IsChooseDateName"].ToString();
                        imOut["IsClearOpeName"] = row["IsClearOpeName"].ToString();
                        imOut["IsGanRanName"] = row["IsGanRanName"].ToString();
                        //麻醉分级和手术并发症 add by cyq 2012-10-17
                        imOut["Anesthesia_Level"] = row["Anesthesia_Level"].ToString();
                        imOut["OperComplication_Code"] = row["OperComplication_Code"].ToString();
                        dtOperation.Rows.Add(imOut);
                    }

                    m_DataOper = dtOperation;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

                #endregion


        #region 点击“插入手术”按钮
        private void simpleButtonInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (GetIemMainPageNo() == "")
                {
                    return;
                }
                this.FormClosing -= new FormClosingEventHandler(DiagForm_FormClosing);
                m_IsInsert = true;
                Save(false);
                this.Close();
                this.FormClosing += new FormClosingEventHandler(DiagForm_FormClosing);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 点击“关闭”按钮
        private void simpleButtonExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DiagForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                m_IsInsert = false;
                //if (m_App.CustomMessageBox.MessageShow("确定要保存吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo)
                //    == System.Windows.Forms.DialogResult.Yes)
                //{
                //    Save(false);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void btn_addOperation_Click(object sender, EventArgs e)
        {
            try
            {
                OperInfo m_OperInfoFrom = new OperInfo(m_App, "new", null);
                m_OperInfoFrom.ShowDialog();
                if (m_OperInfoFrom.DialogResult == DialogResult.OK)
                {
                    m_OperInfoFrom.IemOperInfo = null;
                    DataTable dataTable = m_OperInfoFrom.DataOper;

                    DataTable dataTableOper = new DataTable();
                    if (this.gridControl2.DataSource != null)
                    {
                        dataTableOper = this.gridControl2.DataSource as DataTable;
                    }
                    if (dataTableOper.Rows.Count == 0)
                    {
                        dataTableOper = dataTable.Clone();
                    }
                    foreach (DataRow row in dataTable.Rows)
                    {
                        dataTableOper.ImportRow(row);
                    }
                    gridControl2.BeginUpdate();
                    this.gridControl2.DataSource = dataTableOper;

                    gridControl2.EndUpdate();
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void btn_editOperation_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewOper.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选中一条记录");
                    return;
                }
                DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                if (dataRow == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选中一条记录");
                    return;
                }
                DataTable dataTableOper = this.gridControl2.DataSource as DataTable;
                DataTable dataTable = new DataTable();
                dataTable = dataTableOper.Clone();
                dataTable.ImportRow(dataRow);

                OperInfo m_OperInfoFrom = new OperInfo(m_App, "edit", dataTable);

                m_OperInfoFrom.ShowDialog();
                if (m_OperInfoFrom.DialogResult == DialogResult.OK)
                {
                    m_OperInfoFrom.IemOperInfo = null;

                    dataTableOper.Rows.Remove(dataRow);
                    foreach (DataRow row in m_OperInfoFrom.DataOper.Rows)
                    {
                        dataTableOper.ImportRow(row);
                    }
                    gridControl2.BeginUpdate();
                    this.gridControl2.DataSource = dataTableOper;

                    gridControl2.EndUpdate();
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btn_deleteOperation_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewOper.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                if (dataRow == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                if (MessageBox.Show("确认要删除该记录吗", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                DataTable dataTableOper = this.gridControl2.DataSource as DataTable;
                dataTableOper.Rows.Remove(dataRow);

                this.gridControl2.BeginUpdate();
                this.gridControl2.DataSource = dataTableOper;
                this.gridControl2.EndUpdate();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

    }
}