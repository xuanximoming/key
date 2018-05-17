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

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.HelpForm
{
    public partial class DiagForm : DevExpress.XtraEditors.XtraForm
    {
        #region SQL
        private const string SqlPatDiag = "select e.id, e.diag_type_name /*诊断类型*/," +
            " e.diag_no/*序号*/,e.DIAG_SUB_NO/*子序号*/,e.ID/*节点ID*/, e.PARENT_ID /*父节点ID*/," +
            " e.DIAG_CONTENT/*诊断名称*/, TO_CHAR(e.CONFIRMED_FLAG) CONFIRMED_FLAG /*确诊*/," +
            " to_char(e.DIAG_DATE, 'yyyy-MM-dd') DIAG_DATE/*诊断日期*/, e.DIAG_CODE/*诊断编码*/, " +
            " e.diag_doctor_id/*经治医师CODE*/, u1.name diag_doctor/*经治医师*/, " +
            " e.houseman houseman_id/*实习医师CODE*/, u2.name hoseman/*实习医师*/, " +
            " e.super_id/*主任医师CODE*/, u4.name super/*主任医师*/ , to_char(e.super_sign_date, 'yyyy-MM-dd') super_sign_date /*主任审核时间*/," +
            " e.remark/*备注*/ ,TO_CHAR(e.back_diag) BACK_DIAG /*置后*/" +
            " from PATDIAG e" +
            " left outer join users u1 on u1.id = e.diag_doctor_id and u1.valid = '1' " +
            " left outer join users u2 on u2.id = e.houseman       and u2.valid = '1' " +
            " left outer join users u4 on u4.id = e.super_id       and u4.valid = '1' " +
            " where e.patient_id ={0} and e.diag_type_name = '{1}' order by e.diag_no, e.DIAG_SUB_NO; ";

        private const string SqlPatDiagType = " SELECT code, diagname, typeid FROM patdiagtype ";

        private const string SqlCancelDiag = "delete from PATDIAG where patient_id = {0} and nad = {1} and diag_type = {2} ";

        private const string SqlInsertDiag = " INSERT INTO PATDIAG " +
            " (patient_id, nad, diag_type, diag_type_name, diag_no, " +
            " diag_sub_no, diag_class, diag_code, diag_content, diag_date, " +
            " diag_doctor_id, modify_doctor_id, last_time, parent_id, " +
            " super_id, super_sign_date, flag, houseman, confirmed_flag, ID, " +
            " uncertain_diag, back_diag, remark" +
            " ) " +
            " VALUES ('{0}', {1}, '{2}', '{3}', {4}, " +
            " {5}, '{6}', '{7}', '{8}', {9}, " +
            " '{10}', '{11}', {12}, {13}, " +
            " '{14}', {15}, '{16}', '{17}', {18}, {19}, " +
            " {20}, {21}, '{22}' " +
            " );";
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
        public DiagForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="app"></param>
        /// <param name="diagName">诊断名称</param>
        public DiagForm(IEmrHost app, string diagName)
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
                m_DiagContentRemark = "";
                GetDiagContentRemark(null);
                return m_DiagContentRemark;
            }
            return "";
        }

        /// <summary>
        /// 通过递归按照节点的顺序得到所有节点的诊断 初步诊断，如果有子诊断的话，子诊断以几点几开头，如：诊断为“1 诊断名”，子诊断为“1.1 诊断名 1.2诊断名”等
        /// </summary>
        /// <param name="node"></param>
        private void GetDiagContentRemark(TreeListNode node)
        {
            TreeListNodes nodes = null;
            if (node != null)
            {
                nodes = node.Nodes;
            }
            else
            {
                nodes = treeListDiag.Nodes;
            }

            foreach (TreeListNode subNode in nodes)
            {
                m_DiagContentRemark += GetDiagContentRemartInner(subNode);
                GetDiagContentRemark(subNode);
            }
        }

        private string GetDiagContentRemartInner(TreeListNode subNode)
        {
            string diagName = subNode.GetValue("DIAG_CONTENT").ToString();
            string remark = subNode.GetValue("REMARK").ToString();

            string contentIndex = string.Empty;

            if (subNode.ParentNode == null)
            {
                contentIndex = Convert.ToString(++m_DiagContentRemarkIndex);
            }
            else
            {
                if (subNode.PrevNode == null)
                {
                    contentIndex = subNode.ParentNode.Tag.ToString() + ".1";
                }
                else
                {
                    contentIndex = subNode.PrevNode.Tag.ToString();
                    string[] indexs = contentIndex.Split('.');
                    string index = indexs[indexs.Length - 1];
                    indexs[indexs.Length - 1] = Convert.ToString(int.Parse(index) + 1);
                    contentIndex = string.Join(".", indexs);
                }
            }

            subNode.Tag = contentIndex;
            if (subNode.GetValue("BACK_DIAG").ToString() == "0")
            {
                contentIndex = contentIndex + " " + remark + " " + diagName + " ";
            }
            else
            {
                contentIndex = contentIndex + " " + diagName + " " + remark + " ";
            }
            return contentIndex;
        }

        #endregion

        #region 初始化并绑定控件的数据源
        private void DiagForm_Load(object sender, EventArgs e)
        {
            Init();
            BindData();
            //if (m_DataSearch == null) 由于诊断列表要根据诊断类型判断是西医诊断还是中医诊断，所以每次打开m_DataSearch界面需要重新加载数据
            {
                m_DataSearch = new DictionaryDataSerach(m_App, m_DiagName);
            }
        }

        private void Init()
        {
            m_DiagContentRemark = "";
            m_IsInsert = false;
        }

        /// <summary>
        /// 绑定界面中控件的数据源
        /// </summary>
        private void BindData()
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

            //初始化诊断列表
            string sqlPatDiag = string.Format(SqlPatDiag, m_App.CurrentPatientInfo.NoOfFirstPage, m_DiagName);
            DataTable dtDiagList = m_App.SqlHelper.ExecuteDataTable(sqlPatDiag, CommandType.Text);
            treeListDiag.ClearNodes();
            treeListDiag.ParentFieldName = "PARENT_ID";
            treeListDiag.KeyFieldName = "ID";
            treeListDiag.BeginUnboundLoad();
            treeListDiag.DataSource = dtDiagList;
            treeListDiag.EndUnboundLoad();

            if (dtDiagList.Rows.Count > 0)
            {
                treeListDiag.ExpandAll();
            }
        }

        /// <summary>
        /// 得到诊断名称对应的CODE
        /// </summary>
        /// <returns></returns>
        private string GetLookUpEditValue()
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
        #endregion

        #region 点击“诊断”和“子诊断”按钮
        /// <summary>
        /// 点击诊断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDiag_Click(object sender, EventArgs e)
        {
            m_DataSearch.ShowDialog();
            AddDiag(1);
        }

        /// <summary>
        /// 点击子诊断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonSubDiag_Click(object sender, EventArgs e)
        {
            m_DataSearch.ShowDialog();
            AddDiag(2);
        }

        /// <summary>
        /// 增加诊断
        /// </summary>
        /// <param name="typeID"></param>
        private void AddDiag(int typeID)
        {
            string diagICD = m_DataSearch.DiagICD;
            if (diagICD == "")
            {
                return;
            }
            string diagName = m_DataSearch.DiagName;
            TreeListNode node = treeListDiag.FocusedNode;

            if (typeID == 1)
            {
                //增加同级的诊断
                DataTable dt = treeListDiag.DataSource as DataTable;
                DataRow dr = dt.NewRow();
                dr["DIAG_CONTENT"] = diagName;
                dr["confirmed_flag"] = "1";
                dr["DIAG_DATE"] = DateTime.Now.ToString("yyyy-MM-dd");
                dr["DIAG_CODE"] = diagICD;
                dr["diag_doctor_id"] = m_App.User.Id;
                dr["diag_doctor"] = m_App.User.Name;
                dr["back_diag"] = "1";
                dr["ID"] = dt.Rows.Count + 1;
                dr["PARENT_ID"] = (node == null ? 0 :
                    (node.GetValue("PARENT_ID") == null ? 0 : Convert.ToInt32(node.GetValue("PARENT_ID"))));
                dt.Rows.Add(dr);
                dt.AcceptChanges();

                treeListDiag.BeginUpdate();
                treeListDiag.ExpandAll();
                treeListDiag.EndUpdate();
            }
            else if (typeID == 2)
            {
                //增加子诊断
                DataTable dt = treeListDiag.DataSource as DataTable;
                DataRow dr = dt.NewRow();
                dr["DIAG_CONTENT"] = diagName;
                dr["confirmed_flag"] = "1";
                dr["DIAG_DATE"] = DateTime.Now.ToString("yyyy-MM-dd");
                dr["DIAG_CODE"] = diagICD;
                dr["diag_doctor_id"] = m_App.User.Id;
                dr["diag_doctor"] = m_App.User.Name;
                dr["back_diag"] = "1";
                dr["ID"] = dt.Rows.Count + 1;
                dr["PARENT_ID"] = (node == null ? 0 : Convert.ToInt32(node.GetValue("ID")));
                dt.Rows.Add(dr);
                dt.AcceptChanges();

                treeListDiag.BeginUpdate();
                treeListDiag.ExpandAll();
                treeListDiag.EndUpdate();
            }
        }
        #endregion

        #region 点击保存按钮
        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            Save(true);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="showMessageBox">显示提示框否</param>
        private void Save(bool showMessageBox)
        {
            DataTable dt = (DataTable)treeListDiag.DataSource;
            try
            {
                m_App.SqlHelper.BeginTransaction();

                string patientID = m_App.CurrentPatientInfo.NoOfFirstPage.ToString();
                string nad = m_App.CurrentPatientInfo.TimesOfAdmission.ToString();

                //先清空【表：PATDIAG】中针对该病人的所有诊断
                m_App.SqlHelper.ExecuteNoneQuery(
                    string.Format(SqlCancelDiag, patientID, nad, m_DiagTypeID), CommandType.Text);

                SaveLogic();

                m_App.SqlHelper.CommitTransaction();

                if (showMessageBox)
                {
                    m_App.CustomMessageBox.MessageShow("保存成功!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                }

                treeListDiag.BeginUnboundLoad();
                treeListDiag.DataSource = dt;
                treeListDiag.EndUnboundLoad();

                if (dt.Rows.Count > 0)
                {
                    treeListDiag.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                m_App.SqlHelper.RollbackTransaction();
            }
            finally
            {
                diagNO = 0;//诊断编号
                diagSubNO = 0;//子诊断编号
            }
        }

        int diagNO = 0;//诊断编号
        int diagSubNO = 0;//子诊断编号
        private void SaveLogic()
        {
            SaveInner(null);
        }

        private void SaveInner(TreeListNode node)
        {
            TreeListNodes nodes = null;
            if (node != null)
            {
                nodes = node.Nodes;
            }
            else
            {
                nodes = treeListDiag.Nodes;
            }

            foreach (TreeListNode subNode in nodes)
            {

                SaveData(subNode);

                SaveInner(subNode);
            }
        }

        private void SaveData(TreeListNode subNode)
        {
            //将界面中的诊断保存到【表：PATDIAG】
            string diagType = m_DiagTypeID;
            string diagTypeName = m_DiagName;
            string diagCode = string.Empty;//诊断ICD编码
            string diagName = string.Empty;//诊断名称
            string diagDate = string.Empty;//诊断日期
            string digDoctorID = string.Empty;//经治医师
            string hoseman = string.Empty;//实习医师
            string super = string.Empty;//主治医师
            string superSignDate = string.Empty;//主治日期审核日期
            string remark = string.Empty;//备注
            string backDiag = string.Empty;//置后否
            string confirmedFlag = string.Empty;//确诊否
            //int diagNO = 0;//诊断编号
            //int diagSubNO = 0;//子诊断编号
            string id = string.Empty;//节点ID
            string parentID = string.Empty;//父节点ID
            string patientID = m_App.CurrentPatientInfo.NoOfFirstPage.ToString();
            string nad = m_App.CurrentPatientInfo.TimesOfAdmission.ToString();

            diagCode = subNode.GetValue("DIAG_CODE").ToString();
            diagName = subNode.GetValue("DIAG_CONTENT").ToString();
            diagDate = subNode.GetValue("DIAG_DATE").ToString();
            diagDate = diagDate == "" ? "null" : "to_date('" + diagDate.Split(' ')[0] + "', 'yyyy-mm-dd')";
            digDoctorID = subNode.GetValue("DIAG_DOCTOR_ID").ToString();
            hoseman = subNode.GetValue("HOUSEMAN_ID").ToString();
            super = subNode.GetValue("SUPER_ID").ToString();
            superSignDate = subNode.GetValue("SUPER_SIGN_DATE").ToString();
            superSignDate = superSignDate == "" ? "null" : "to_date('" + superSignDate.Split(' ')[0] + "', 'yyyy-mm-dd')";
            confirmedFlag = subNode.GetValue("CONFIRMED_FLAG").ToString();
            backDiag = subNode.GetValue("BACK_DIAG").ToString();
            remark = subNode.GetValue("REMARK").ToString();
            id = subNode.GetValue("ID").ToString();
            parentID = subNode.GetValue("PARENT_ID").ToString();

            if (parentID != "0")//有父节点
            {
                diagSubNO++;
            }
            else//没有父节点
            {
                diagNO++;
                diagSubNO = 0;
            }

            string sqlInsert = string.Format(SqlInsertDiag,
                    m_App.CurrentPatientInfo.NoOfFirstPage, nad, diagType, diagTypeName, diagNO,
                    diagSubNO, "", diagCode, diagName, diagDate,
                    digDoctorID, "", "NULL", parentID,
                    super, superSignDate, "0", hoseman, confirmedFlag, id,
                    "0", backDiag, remark);
            m_App.SqlHelper.ExecuteNoneQuery(sqlInsert, CommandType.Text);


            #region 在此处理，加入到PATDIAG表后，要相应的加到病案首页中去（iem_mainpage_diagnosis_sx）--ywk
            string tablename = string.Empty;//用于处理两个病案首页诊断信息的处理，区分表名 首页的表
            string containbase = string.Empty;
            string diagtable = string.Empty;//诊断的表
            string searchdic = string.Format(@"select mname from dict_catalog where ccode='AA'");
            DataTable dtse = m_App.SqlHelper.ExecuteDataTable(searchdic, CommandType.Text);//查询病案首页的值
            if (dtse.Rows.Count > 0)
            {
                containbase = dtse.Rows[0]["mname"].ToString();
            }
            if (containbase.Contains("SX"))//是泗县的
            {
                tablename = "IEM_MAINPAGE_BASICINFO_SX";
                ////diagtable = "iem_mainpage_diagnosis_sx";
            }
            if (containbase.Contains("2012"))
            {
                tablename = "IEM_MAINPAGE_BASICINFO_2012";
                //diagtable = "iem_mainpage_diagnosis_2012";
            }

            //病人的首页序号
            string iem_mainpage_no = string.Empty;
            string type = string.Empty;//代表是中医诊断还是西医诊断
            string typename = string.Empty;//类型名称
            string order_value = string.Empty;

            string mainpagetip = string.Empty;//用于区分是泗县首页还是2012版本的
            DataTable dt = new DataTable();
            if (containbase.Contains("SX"))//是泗县的
            {
                mainpagetip = "sx";
                dt = m_App.SqlHelper.ExecuteDataTable(string.Format(@"select  iem_mainpage_no from  IEM_MAINPAGE_BASICINFO_SX 
                    where noofinpat='{0}'", m_App.CurrentPatientInfo.NoOfFirstPage), CommandType.Text);
            }
            if (containbase.Contains("2012"))//仁和
            {
                mainpagetip = "2012";
                dt = m_App.SqlHelper.ExecuteDataTable(string.Format(@"select  iem_mainpage_no from  IEM_MAINPAGE_BASICINFO_2012 
                    where noofinpat='{0}'", m_App.CurrentPatientInfo.NoOfFirstPage), CommandType.Text);
            }
            if (dt.Rows.Count == 0)
            {
                //MessageBox.Show("请先填写此病人病案首页信息！");
                return;
            }
            if (dt.Rows.Count > 0)
            {
                iem_mainpage_no = dt.Rows[0]["iem_mainpage_no"].ToString();
            }

            if (lookUpEditDiagType.Text.Contains("西医"))//这里没取到值 
            {
                type = "1";
                typename = "西医诊断";
            }
            if (lookUpEditDiagType.Text.Contains("中医"))
            {
                type = "2";
                typename = "中医诊断";
            }

            string searchdiag = string.Empty;//查找诊断SQL语句
            if (containbase.Contains("SX"))//是泗县的
            {
                searchdiag = string.Format(@"select * from iem_mainpage_diagnosis_sx  where 
               iem_mainpage_no='{0}' and valide='1' ", iem_mainpage_no);
            }
            if (containbase.Contains("2012"))//仁和
            {
                searchdiag = string.Format(@"select * from iem_mainpage_diagnosis_2012 where 
               iem_mainpage_no='{0}' and valide='1' ", iem_mainpage_no);
            }
            DataTable dtdiag = m_App.SqlHelper.ExecuteDataTable(searchdiag, CommandType.Text);
            if (dtdiag.Rows.Count > 0)
            {
                order_value = (dtdiag.Rows.Count + 1).ToString();
            }
            else
            {
                order_value = "0";
            }

            //不允许重复插入处理
            string sql1 = string.Empty;
            if (containbase.Contains("SX"))//是泗县的
            {
                sql1 = string.Format(@"select * from iem_mainpage_diagnosis_sx  where 
               iem_mainpage_no='{0}' and valide='1' and diagnosis_code='{1}'", iem_mainpage_no, diagCode);
            }
            if (containbase.Contains("2012"))//仁和
            {
                sql1 = string.Format(@"select * from iem_mainpage_diagnosis_2012 where 
               iem_mainpage_no='{0}' and valide='1' and diagnosis_code='{1}'", iem_mainpage_no, diagCode);
            }
            DataTable m_dt = m_App.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
            string diagtypeid = string.Empty;//标识是否是主诊断
            if (parentID == "0")//是父节点表示主诊断
            {
                diagtypeid = "7";
            }
            else
            {
                diagtypeid = "8";
            }
            if (m_dt.Rows.Count > 0)//病案首页的诊断有数据，进行判断是否已经插入当前诊断
            {
                foreach (DataRow dr in dtdiag.Rows)
                {
                    if (dr["iem_mainpage_no"].ToString() == iem_mainpage_no && dr["diagnosis_code"].ToString() == diagCode)
                    { return; }
                }
            }
            else
            {
                //diagnosis_type_id=7是表示主诊断，8是标识其他诊断 
                string sql = string.Empty;//要执行的插入SQL语句
                //两个病案首页的诊断表不一样
                //此处为真正将诊断信息同步插入到首页的操作，在此处读取配置表中的配置信息
                //判断点击的按钮是不是配置中的按钮名称
                string emrSetting = BasicSettings.GetStringConfig("BtnOutDiagEvent");//取得配置表中的配置信息 
                //由于处理插入数据的方法都放在了下面，传的参数用实体封装起来
                DiagEntity diagEnt = new DiagEntity();
                //iem_mainpage_no, diagtypeid, diagCode, diagName, "0", order_value, m_App.User.Id, type, typename);
                diagEnt.IEMMainPageNO = iem_mainpage_no;
                diagEnt.DiagTypeID = diagtypeid;
                diagEnt.DiagCode = diagCode;
                diagEnt.DiagName = diagName;
                diagEnt.StatusID = "0";
                diagEnt.Order_Value = order_value;
                diagEnt.Type = type;
                diagEnt.TypeName = typename;

                SetOutDiagBtnEvent(emrSetting, mainpagetip, diagEnt);
                #region 注释掉的

                //                if (tablename == "IEM_MAINPAGE_BASICINFO_SX")
                //                {
                //                    //其中status_id字段为病案首页插入诊断信息有无选择入院病请（此处为0，为没选择 ）
                //                    sql = string.Format(@"INSERT INTO iem_mainpage_diagnosis_sx
                //                          (iem_mainpage_diagnosis_no,
                //                           iem_mainpage_no,
                //                           diagnosis_type_id,
                //                           diagnosis_code,
                //                           diagnosis_name,
                //                           status_id,
                //                           order_value,
                //                           valide,
                //                           create_user,
                //                           create_time,
                //                           type,
                //                          typeName)
                //                           VALUES
                //                          (seq_iem_mainpage_diagnosis_id.NEXTVAL,
                //                           '{0}', 
                //                           '{1}',
                //                           '{2}',
                //                           '{3}',
                //                           '{4}', 
                //                           '{5}',
                //                             1,
                //                           '{6}', 
                //                           TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'),
                //                           '{7}',
                //                           '{8}') ", iem_mainpage_no, diagtypeid, diagCode, diagName, "0", order_value, m_App.User.Id, type, typename);
                //                }
                //                if (tablename == "IEM_MAINPAGE_BASICINFO_2012")
                //                {
                //                    sql = string.Format(@"INSERT INTO iem_mainpage_diagnosis_2012
                //                          (iem_mainpage_diagnosis_no,
                //                           iem_mainpage_no,
                //                           diagnosis_type_id,
                //                           diagnosis_code,
                //                           diagnosis_name,
                //                           status_id,
                //                           order_value,
                //                           valide,
                //                           create_user,
                //                           create_time
                //                         )
                //                           VALUES
                //                          (seq_iem_mainpage_diagnosis_id.NEXTVAL,
                //                           '{0}', 
                //                           '{1}',
                //                           '{2}',
                //                           '{3}',
                //                           '{4}', 
                //                           '{5}',
                //                             1,
                //                           '{6}', 
                //                           TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss')
                //                          ) ", iem_mainpage_no, diagtypeid, diagCode, diagName, "0", order_value, m_App.User.Id);
                //                }

                //                try
                //                {
                //                    m_App.SqlHelper.ExecuteNoneQuery(sql, CommandType.Text);
                //                }
                //                catch (Exception ex)
                //                {
                //                    MessageBox.Show("出现错误 :" + ex.Message);
                //                }
                #endregion
            }

            #endregion
        }
        /// <summary>
        /// 根据配置控制哪些诊断按钮要同步诊断信息
        /// add by ywk 
        /// </summary>
        /// <param name="emrSetting">配置中的诊断按钮</param>
        /// <param name="mainpagetip">用于区分是哪个版本的首页</param>
        private void SetOutDiagBtnEvent(string emrSetting, string mainpagetip, DiagEntity m_DiagEntity)
        {
            string type = string.Empty;
            string typename = string.Empty;
            string sql = string.Empty;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(emrSetting);
            XmlNodeList nodeList = doc.GetElementsByTagName("OutDiag");
            if (mainpagetip == "2012")//2012版本的
            {
                if (nodeList.Count > 0)
                {
                    XmlElement ele = nodeList[0] as XmlElement;
                    nodeList = ele.GetElementsByTagName("EmrPad2012");
                    if (nodeList.Count > 0)
                    {
                        ele = nodeList[0] as XmlElement;
                        if (ele.InnerText.Contains(","))
                        {
                            string[] BtnArray = ele.InnerText.ToString().Split(',');
                            if (BtnArray.Length > 0)
                            {
                                for (int k = 0; k < BtnArray.Length; k++)
                                {
                                    if (BtnArray[k].ToString() == lookUpEditDiagType.Text)//判断当前节点的值是不是等于弹出的诊断按钮的名称 
                                    {
                                        sql = string.Format(@"INSERT INTO iem_mainpage_diagnosis_2012
                                      (iem_mainpage_diagnosis_no,
                                       iem_mainpage_no,
                                       diagnosis_type_id,
                                       diagnosis_code,
                                       diagnosis_name,
                                       status_id,
                                       order_value,
                                       valide,
                                       create_user,
                                       create_time
                                     )
                                       VALUES
                                      (seq_iem_mainpage_diagnosis_id.NEXTVAL,
                                       '{0}', 
                                       '{1}',
                                       '{2}',
                                       '{3}',
                                       '{4}', 
                                       '{5}',
                                         1,
                                       '{6}', 
                                       TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss')
                                      ) ", m_DiagEntity.IEMMainPageNO, m_DiagEntity.DiagTypeID, m_DiagEntity.DiagCode,
                                             m_DiagEntity.DiagName, m_DiagEntity.StatusID, m_DiagEntity.Order_Value, m_App.User.Id);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (ele.InnerText.Trim() == lookUpEditDiagType.Text)//判断当前节点的值是不是等于弹出的诊断按钮的名称 
                            {
                                sql = string.Format(@"INSERT INTO iem_mainpage_diagnosis_2012
                          (iem_mainpage_diagnosis_no,
                           iem_mainpage_no,
                           diagnosis_type_id,
                           diagnosis_code,
                           diagnosis_name,
                           status_id,
                           order_value,
                           valide,
                           create_user,
                           create_time
                         )
                           VALUES
                          (seq_iem_mainpage_diagnosis_id.NEXTVAL,
                           '{0}', 
                           '{1}',
                           '{2}',
                           '{3}',
                           '{4}', 
                           '{5}',
                             1,
                           '{6}', 
                           TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss')
                          ) ", m_DiagEntity.IEMMainPageNO, m_DiagEntity.DiagTypeID, m_DiagEntity.DiagCode,
                                m_DiagEntity.DiagName, m_DiagEntity.StatusID, m_DiagEntity.Order_Value, m_App.User.Id);
                            }
                        }
                    }
                }
            }
            if (mainpagetip == "sx")//泗县版本的
            {
                if (nodeList.Count > 0)
                {
                    XmlElement ele = nodeList[0] as XmlElement;
                    nodeList = ele.GetElementsByTagName("EmrPadSX");
                    if (nodeList.Count > 0)
                    {
                        //*******************中西医的分开处理************
                        for (int j = 0; j < nodeList.Count; j++)
                        {
                            ele = nodeList[j] as XmlElement;//第一个节点是代表SXChinese中医(第二个是西医节点)
                            nodeList = ele.GetElementsByTagName("SXChinese");
                        }
                        if (nodeList.Count > 0)
                        {
                            type = "2";
                            typename = "中医诊断";
                        }
                        //处理中医
                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            ele = nodeList[i] as XmlElement;
                            //每个节点可以包含各个要起作用的按钮名称，并且用,分隔开
                            if (ele.InnerText.Contains(","))
                            {
                                string[] BtnArray = ele.InnerText.ToString().Split(',');
                                if (BtnArray.Length > 0)
                                {
                                    for (int k = 0; k < BtnArray.Length; k++)
                                    {
                                        if (BtnArray[k].ToString() == lookUpEditDiagType.Text)//判断当前节点的值是不是等于弹出的诊断按钮的名称 
                                        {
                                            sql = string.Format(@"INSERT INTO iem_mainpage_diagnosis_sx
                                            (iem_mainpage_diagnosis_no,
                                           iem_mainpage_no,
                                           diagnosis_type_id,
                                           diagnosis_code,
                                           diagnosis_name,
                                           status_id,
                                           order_value,
                                           valide,
                                           create_user,
                                           create_time,
                                           type,
                                          typeName)
                                           VALUES
                                          (seq_iem_mainpage_diagnosis_id.NEXTVAL,
                                           '{0}', 
                                           '{1}',
                                           '{2}',
                                           '{3}',
                                           '{4}', 
                                           '{5}',
                                             1,
                                           '{6}', 
                                           TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'),
                                           '{7}',
                                           '{8}') ", m_DiagEntity.IEMMainPageNO, m_DiagEntity.DiagTypeID, m_DiagEntity.DiagCode,
                                            m_DiagEntity.DiagName, m_DiagEntity.StatusID, m_DiagEntity.Order_Value, m_App.User.Id, type, typename);
                                        }
                                    }
                                }
                            }
                            else
                            {

                                if (ele.InnerText.Trim() == lookUpEditDiagType.Text)//判断当前节点的值是不是等于弹出的诊断按钮的名称 
                                {
                                    sql = string.Format(@"INSERT INTO iem_mainpage_diagnosis_sx
                                    (iem_mainpage_diagnosis_no,
                                   iem_mainpage_no,
                                   diagnosis_type_id,
                                   diagnosis_code,
                                   diagnosis_name,
                                   status_id,
                                   order_value,
                                   valide,
                                   create_user,
                                   create_time,
                                   type,
                                  typeName)
                                   VALUES
                                  (seq_iem_mainpage_diagnosis_id.NEXTVAL,
                                   '{0}', 
                                   '{1}',
                                   '{2}',
                                   '{3}',
                                   '{4}', 
                                   '{5}',
                                     1,
                                   '{6}', 
                                   TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'),
                                   '{7}',
                                   '{8}') ", m_DiagEntity.IEMMainPageNO, m_DiagEntity.DiagTypeID, m_DiagEntity.DiagCode,
                                    m_DiagEntity.DiagName, m_DiagEntity.StatusID, m_DiagEntity.Order_Value, m_App.User.Id, type, typename);
                                }
                            }
                        }

                        //处理西医
                        XmlNodeList nodeList1 = doc.GetElementsByTagName("OutDiag");
                        if (nodeList1.Count > 0)
                        {
                            XmlElement ele1 = nodeList1[0] as XmlElement;
                            nodeList1 = ele1.GetElementsByTagName("EmrPadSX");
                            nodeList1 = ele1.GetElementsByTagName("SXEn");
                            if (nodeList1.Count > 0)
                            {
                                type = "1";
                                typename = "西医诊断";
                            }
                            for (int i = 0; i < nodeList1.Count; i++)
                            {
                                ele = nodeList1[i] as XmlElement;

                                if (ele.InnerText.Contains(","))
                                {
                                    string[] BtnArray = ele.InnerText.ToString().Split(',');
                                    if (BtnArray.Length > 0)
                                    {
                                        for (int k = 0; k < BtnArray.Length; k++)
                                        {
                                            if (BtnArray[k].ToString() == lookUpEditDiagType.Text)//判断当前节点的值是不是等于弹出的诊断按钮的名称 
                                            {
                                                sql = string.Format(@"INSERT INTO iem_mainpage_diagnosis_sx
                                                  (iem_mainpage_diagnosis_no,
                                                   iem_mainpage_no,
                                                   diagnosis_type_id,
                                                   diagnosis_code,
                                                   diagnosis_name,
                                                   status_id,
                                                   order_value,
                                                   valide,
                                                   create_user,
                                                   create_time,
                                                   type,
                                                  typeName)
                                                   VALUES
                                                  (seq_iem_mainpage_diagnosis_id.NEXTVAL,
                                                   '{0}', 
                                                   '{1}',
                                                   '{2}',
                                                   '{3}',
                                                   '{4}', 
                                                   '{5}',
                                                     1,
                                                   '{6}', 
                                                   TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'),
                                                   '{7}',
                                                   '{8}') ", m_DiagEntity.IEMMainPageNO, m_DiagEntity.DiagTypeID, m_DiagEntity.DiagCode,
                                                             m_DiagEntity.DiagName, m_DiagEntity.StatusID, m_DiagEntity.Order_Value, m_App.User.Id, type, typename);
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    if (ele.InnerText.Trim() == lookUpEditDiagType.Text)//判断当前节点的值是不是等于弹出的诊断按钮的名称 
                                    {
                                        sql = string.Format(@"INSERT INTO iem_mainpage_diagnosis_sx
                                  (iem_mainpage_diagnosis_no,
                                   iem_mainpage_no,
                                   diagnosis_type_id,
                                   diagnosis_code,
                                   diagnosis_name,
                                   status_id,
                                   order_value,
                                   valide,
                                   create_user,
                                   create_time,
                                   type,
                                  typeName)
                                   VALUES
                                  (seq_iem_mainpage_diagnosis_id.NEXTVAL,
                                   '{0}', 
                                   '{1}',
                                   '{2}',
                                   '{3}',
                                   '{4}', 
                                   '{5}',
                                     1,
                                   '{6}', 
                                   TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'),
                                   '{7}',
                                   '{8}') ", m_DiagEntity.IEMMainPageNO, m_DiagEntity.DiagTypeID, m_DiagEntity.DiagCode,
                                        m_DiagEntity.DiagName, m_DiagEntity.StatusID, m_DiagEntity.Order_Value, m_App.User.Id, type, typename);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    m_App.SqlHelper.ExecuteNoneQuery(sql, CommandType.Text);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("出现错误 :" + ex.Message);
            }
        }

        #endregion

        #region 点击“删除”按钮
        private void simpleButtonsimpleButtonDelete_Click(object sender, EventArgs e)
        {
            if (m_App.CustomMessageBox.MessageShow("确定要删除吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo)
                == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            DeleteTreeListNode();

            m_App.CustomMessageBox.MessageShow("删除成功！", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
        }

        private void DeleteTreeListNode()
        {
            TreeListNode node = treeListDiag.FocusedNode;

            if (node != null)
            {
                treeListDiag.Nodes.Remove(node);
            }

            DataTable dt = treeListDiag.DataSource as DataTable;
            dt.AcceptChanges();

            Save(false);
        }
        #endregion

        #region 点击“插入诊断”按钮
        private void simpleButtonInsert_Click(object sender, EventArgs e)
        {
            this.FormClosing -= new FormClosingEventHandler(DiagForm_FormClosing);
            m_IsInsert = true;
            Save(false);
            this.Close();
            this.FormClosing += new FormClosingEventHandler(DiagForm_FormClosing);
        }
        #endregion

        #region 点击“关闭”按钮
        private void simpleButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DiagForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_IsInsert = false;
            //if (m_App.CustomMessageBox.MessageShow("确定要保存吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo)
            //    == System.Windows.Forms.DialogResult.Yes)
            //{
            //    Save(false);
            //}
        }
        #endregion

        #region 常用诊断
        private void simpleButtonDoctorCustom_Click(object sender, EventArgs e)
        {
            if (m_DoctorCustomForm == null)
            {
                m_DoctorCustomForm = new DoctorCustomForm(m_App, "诊断");
            }
            m_DoctorCustomForm.ShowDialog();


            //增加同级的诊断
            string diagICD = m_DoctorCustomForm.DiagICD;
            if (diagICD == "")
            {
                return;
            }
            string diagName = m_DoctorCustomForm.DiagName;
            TreeListNode node = treeListDiag.FocusedNode;

            DataTable dt = treeListDiag.DataSource as DataTable;
            DataRow dr = dt.NewRow();
            dr["DIAG_CONTENT"] = diagName;
            dr["confirmed_flag"] = "1";
            dr["DIAG_DATE"] = DateTime.Now.ToString("yyyy-MM-dd");
            dr["DIAG_CODE"] = diagICD;
            dr["diag_doctor_id"] = m_App.User.Id;
            dr["diag_doctor"] = m_App.User.Name;
            dr["back_diag"] = "1";
            dr["ID"] = dt.Rows.Count + 1;
            dr["PARENT_ID"] = (node == null ? 0 :
                (node.GetValue("PARENT_ID") == null ? 0 : Convert.ToInt32(node.GetValue("PARENT_ID"))));
            dt.Rows.Add(dr);
            dt.AcceptChanges();

            treeListDiag.ExpandAll();
        }
        #endregion

        #endregion
    }

    public class DiagEntity
    {
        //iem_mainpage_no, diagtypeid, diagCode, diagName, "0", order_value, m_App.User.Id, type, typename);
        public string IEMMainPageNO { get; set; }
        public string DiagTypeID { get; set; }
        public string DiagCode { get; set; }
        public string DiagName { get; set; }
        public string StatusID { get; set; }
        public string Order_Value { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
    }
}