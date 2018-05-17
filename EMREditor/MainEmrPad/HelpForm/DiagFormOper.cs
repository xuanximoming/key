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
    public partial class DiagFormOper : DevBaseForm
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

        private const string SqlPatDiagType = " SELECT code, diagname, typeid FROM patdiagtype ";

        private const string SqlCancelDiag = "delete from PATDIAG where patient_id = {0} and nad = {1} and diag_type = {2} ";

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
        public DiagFormOper()
        {
            InitializeComponent();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="app"></param>
        /// <param name="diagName">诊断名称</param>
        public DiagFormOper(IEmrHost app, string diagName)
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
        /// 二次修改 ywk 2013年2月19日15:57:07  
        /// <param name="node"></param>
        private void GetDiagContentRemark(TreeListNode node)
        {
            try
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

                //此处根据配置，判断返回的诊断内容是结构化元素还是自由文本
                //add by ywk 2013年2月19日15:46:13
                string diagContentType = BasicSettings.GetStringConfig("IsSetDiagContentStr") == "" ? "1" : BasicSettings.GetStringConfig("IsSetDiagContentStr");

                //string enterstring =@"<p align='0' width='2067'><eof fontsize='10.5' align='0' /></p>";
                foreach (TreeListNode subNode in nodes)
                {
                    if (diagContentType == "1")//结构化元素
                    {
                        m_DiagContentRemark += GetDiagContentRemartInner(subNode);

                    }
                    else//自由文本
                    {
                        m_DiagContentRemark += GetDiagContentRemartInner(subNode) + "\r\n";
                    }

                    GetDiagContentRemark(subNode);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetDiagContentRemartInner(TreeListNode subNode)
        {
            try
            {
                string diagcode = subNode.GetValue("DIAG_CODE").ToString();
                DataTable dtSource = subNode.TreeList.DataSource as DataTable;
                string diagOtherName = string.Empty;
                //DataRow dr = new DataRow();
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    DataRow dr = dtSource.Select(string.Format("DIAG_CODE='{0}'", diagcode))[0];

                    //这个名称是可以编辑的最终返回的add byy ywk 2013年1月6日9:48:28
                    //string diagOtherName = subNode.GetValue("DiagOtherName").ToString() == "" ? subNode.GetValue("DIAG_CONTENT").ToString() :           subNode.GetValue("DiagOtherName").ToString();
                    diagOtherName = dr["DIAGOTHERNAME"].ToString();
                }
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
                    contentIndex = contentIndex + " " + remark + " " + diagOtherName + " ";//diagName
                }
                else
                {
                    contentIndex = contentIndex + " " + diagOtherName + " " + remark + " ";
                }
                return contentIndex;
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
        /// <summary>
        /// 点击手术
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDiag_Click(object sender, EventArgs e)
        {
            try
            {
                m_DataSearch.ShowDialog();
                AddDiag(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 增加手术
        /// </summary>
        /// <param name="typeID"></param>
        private void AddDiag(int typeID)
        {
            try
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
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)//插入的ICD不能一样 add by ywk 2013年1月6日11:29:40 
                        {
                            if (diagICD == dt.Rows[i]["DIAG_CODE"].ToString())
                            {
                                m_App.CustomMessageBox.MessageShow("您已经添加了相同的诊断!", DrectSoft.Core.CustomMessageBoxKind.WarningOk);
                                return;
                            }
                        }
                    }
                    DataRow dr = dt.NewRow();
                    dr["DIAG_CONTENT"] = diagName;
                    dr["confirmed_flag"] = "1";
                    dr["DIAG_DATE"] = DateTime.Now.ToString("yyyy-MM-dd");
                    dr["DIAG_CODE"] = diagICD;
                    dr["diag_doctor_id"] = m_App.User.Id;
                    dr["diag_doctor"] = m_App.User.Name;
                    dr["back_diag"] = "1";
                    dr["ID"] = dt.Rows.Count + 1;
                    dr["DiagOtherName"] = diagName;//诊断别名 add by ywk 2013年1月6日10:22:22 
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
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)//插入的ICD不能一样 add by ywk 2013年1月6日11:29:40 
                        {
                            if (diagICD == dt.Rows[i]["DIAG_CODE"].ToString())
                            {
                                m_App.CustomMessageBox.MessageShow("您已经添加了相同的诊断!", DrectSoft.Core.CustomMessageBoxKind.WarningOk);
                                return;
                            }
                        }
                    }

                    DataRow dr = dt.NewRow();
                    dr["DIAG_CONTENT"] = diagName;
                    dr["confirmed_flag"] = "1";
                    dr["DIAG_DATE"] = DateTime.Now.ToString("yyyy-MM-dd");
                    dr["DIAG_CODE"] = diagICD;
                    dr["diag_doctor_id"] = m_App.User.Id;
                    dr["diag_doctor"] = m_App.User.Name;
                    dr["back_diag"] = "1";
                    dr["ID"] = dt.Rows.Count + 1;
                    dr["DiagOtherName"] = diagName;//诊断别名 add by ywk 2013年1月6日10:22:22 
                    dr["PARENT_ID"] = (node == null ? 0 : Convert.ToInt32(node.GetValue("ID")));
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();

                    treeListDiag.BeginUpdate();
                    treeListDiag.ExpandAll();
                    treeListDiag.EndUpdate();
                }
            }
            catch (Exception)
            {

                throw;
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
                MessageBox.Show(ex.Message);
            }
            finally
            {
                diagNO = 0;//诊断编号
                diagSubNO = 0;//子诊断编号
            }
        }

        private void SaveCurrentDiag(string patient_id, string patient_name, string diag_id, string diag_content)
        {
            try
            {
                m_App.SqlHelper.ExecuteNoneQuery("usp_updatecurrentdiaginfo"
                       , new SqlParameter[] 
                { 
                    new SqlParameter("@Patient_id", patient_id), 
                    new SqlParameter("@Patient_name", patient_name),
                    new SqlParameter("@Diag_code", diag_id), 
                    new SqlParameter("@Diag_content", diag_content)
                }
                       , CommandType.StoredProcedure);

            }
            catch (Exception)
            {
                throw;
            }
        }

        int diagNO = 0;//诊断编号
        int diagSubNO = 0;//子诊断编号
        private void SaveLogic()
        {
            try
            {
                SaveInner(null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SaveInner(TreeListNode node)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        private void SaveData(TreeListNode subNode)
        {
            try
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
                string DiagOtherName = string.Empty;//诊断别名 add  by ywk 2013年1月6日9:33:55
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
                DiagOtherName = subNode.GetValue("DIAGOTHERNAME").ToString();//诊断别名
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
                        "0", backDiag, remark, DiagOtherName);
                m_App.SqlHelper.ExecuteNoneQuery(sqlInsert, CommandType.Text);


                m_App.SqlHelper.ExecuteNoneQuery("usp_updatecurrentdiaginfo"
                       , new SqlParameter[] 
                { 
                    new SqlParameter("@patient_id", m_App.CurrentPatientInfo.NoOfFirstPage),
                    new SqlParameter("@patient_name", m_App.CurrentPatientInfo.Name),
                    new SqlParameter("@diag_code", diagCode),
                    new SqlParameter("@diag_content", diagName)
                }, CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 点击“删除”按钮
        private void simpleButtonsimpleButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = treeListDiag.DataSource as DataTable;
                if (dt != null && dt.Rows.Count == 0)
                {
                    m_App.CustomMessageBox.MessageShow("列表中无数据！", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    return;
                }
                if (m_App.CustomMessageBox.MessageShow("确定要删除吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo)
                    == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                DeleteTreeListNode();

                m_App.CustomMessageBox.MessageShow("删除成功！", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteTreeListNode()
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 点击“插入手术”按钮
        private void simpleButtonInsert_Click(object sender, EventArgs e)
        {
            try
            {
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

        #endregion
    }
}