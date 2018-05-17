using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.FORM;
using DevExpress.XtraPrinting;
using DrectSoft.Common.Ctrs.DLG;
using System.Data.SqlClient;
using DevExpress.Utils;
using DrectSoft.Common;
using DrectSoft.Service;
using System.Xml;
using DrectSoft.Wordbook;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;

namespace DrectSoft.Core.MajorDiagnoseDoctor
{
    public partial class MedicalRecordView : DevBaseForm, IStartPlugIn
    {
        IEmrHost m_app;
        IDataAccess sql_Helper;

        public MedicalRecordView()
        {
            InitializeComponent();
        }

        #region Private

        /// <summary>
        /// 加载病历
        /// edit by Yanqiao.Cai 2012-11-05
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadRecord()
        {
            try
            {
                DataRow dataRow;
                //未归档病历记录
                dataRow = gridviewMedicalRecord.GetDataRow(gridviewMedicalRecord.FocusedRowHandle);
                if (dataRow==null)
                {
                    MessageBox.Show("请选择一条记录");//Modify by xlb 2013-06-19 给出弹窗提示
                    return;
                }
                string deptid = Convert.ToString(dataRow["OutHosDept"]);
                if (m_app.User.CurrentDeptId != deptid && !m_app.User.RelateDeptWards.Any(pt => pt.DeptId.Equals(deptid)))
                {
                    MessageBox.Show("该病人不在您的管理病区内，您无权查看。");
                    return;
                }
                string noofinpat = dataRow["NOOFINPAT"].ToString();

                #region 已注释 by cyq 2013-02-18
                //string msg;
                //string noofinpat = GetCurrentNOofinpat(out msg);
                //if (!string.IsNullOrEmpty(msg))
                //{
                //    Common.Ctrs.DLG.MessageBox.Show(msg);
                //    return;
                //}
                //if (string.IsNullOrEmpty(noofinpat))
                //{
                //    Common.Ctrs.DLG.MessageBox.Show("请选中一条病历记录");
                //    return;
                //}
                #endregion

                //edit by wyt 2012-11-09 新建病历显示窗口
                EmrBrowser frm = new EmrBrowser(noofinpat, m_app);
                frm.StartPosition = FormStartPosition.CenterParent;
               // frm.TopMost = true;
                frm.Show(this);

                #region 取消加载插件方式显示病历 edit by wyt 2012-11-09
                //m_app.ChoosePatient(syxh);
                //m_app.LoadPlugIn("DrectSoft.Core.MainEmrPad.MainForm,DrectSoft.Core.MainEmrPad");
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetCurrentNOofinpat(out string msg)
        {
            try
            {
                msg = string.Empty;
                GridView view = gridviewMedicalRecord;

                if (view.FocusedRowHandle < 0)
                {
                    return "";
                }
                else
                {
                    DataRow dataRow = view.GetDataRow(view.FocusedRowHandle);
                    if (dataRow == null) return "";

                    if (dataRow["ApplyStatusID"].ToString() != Convert.ToInt32(BorrowStatus.ApplyPass).ToString())
                    {
                        return "";
                    }
                    
                    string deptid = Convert.ToString(dataRow["OutHosDept"]);
                    if (m_app.User.RelateDeptWards.Where(pt => pt.DeptId.Equals(deptid)).Count() < 0)
                    {
                        msg = "该患者不在您的管理病区内";
                        return "";
                    }
                    return dataRow["NOOFINPAT"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitSqlWorkBook()
        {
            try
            {
                lookUpWindowDepartment.SqlHelper = sql_Helper;
                //科室
                DataTable Dept = sql_Helper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);
                Dept.Columns["ID"].Caption = "科室编码";
                Dept.Columns["NAME"].Caption = "科室名称";
                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("ID", 60);
                cols.Add("NAME", 120);
                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
                lookUpEditorDepartment.CodeValue = DS_Common.currentUser.CurrentDeptId;

                //诊断
                this.lookUpWindowInDiag.SqlHelper = sql_Helper;
                this.lookUpWindowOutDiag.SqlHelper = sql_Helper;
                this.lookUpWindowCurrentDiag.SqlHelper = sql_Helper;
                Dictionary<string, int> col = new Dictionary<string, int>();
                //edit by cyq 2013-01-07 新版&老板主诊医师(配置)
                DataTable diagDt = new DataTable();
                string config = DS_SqlService.GetConfigValueByKey("ImportDiseasesGroupHistoryData");
                XmlDocument doc = new XmlDocument();
                if (!string.IsNullOrEmpty(config))
                {
                    doc.LoadXml(config);
                }
                XmlNodeList nodeList = doc.GetElementsByTagName("isnew");
                string isnew = string.Empty;
                if (null != nodeList && nodeList.Count > 0)
                {
                    isnew = nodeList[0].InnerText;
                }
                if (!string.IsNullOrEmpty(isnew) && isnew.Trim() == "1")
                {//新版主诊医师
                    diagDt = DS_BaseService.GetDiagResourceByUserID(DS_Common.currentUser.Id);
                    if (null == diagDt || diagDt.Rows.Count == 0)
                    {
                        if (!diagDt.Columns.Contains("ICD"))
                        {
                            diagDt.Columns.Add("ICD");
                        }
                        if (!diagDt.Columns.Contains("NAME"))
                        {
                            diagDt.Columns.Add("NAME");
                        }
                        if (!diagDt.Columns.Contains("PY"))
                        {
                            diagDt.Columns.Add("PY");
                        }
                        if (!diagDt.Columns.Contains("WB"))
                        {
                            diagDt.Columns.Add("WB");
                        }
                    }
                    diagDt.Columns["ICD"].Caption = "诊断编码";
                    diagDt.Columns["NAME"].Caption = "诊断名称";
                    col.Add("ICD", 60);
                    col.Add("NAME", 120);
                    SqlWordbook inWordBook = new SqlWordbook("inDiag", diagDt, "ICD", "NAME", col, "ICD//NAME//PY//WB");
                    this.lookUpEditorInDiag.SqlWordbook = inWordBook;
                    SqlWordbook outWordBook = new SqlWordbook("outDiag", diagDt, "ICD", "NAME", col, "ICD//NAME//PY//WB");
                    this.lookUpEditorOutDiag.SqlWordbook = outWordBook;
                    SqlWordbook currentWordBook = new SqlWordbook("curDiag", diagDt, "ICD", "NAME", col, "ICD//NAME//PY//WB");
                    this.lookUpEditorCurrentDiag.SqlWordbook = currentWordBook;
                }
                else
                {//原版主诊医师
                    #region 原诊断数据集
                    DataTable diagofuser = new DataTable();
                    diagofuser.Columns.Add("ICD");
                    diagofuser.Columns.Add("DIAGNOSIS");
                    diagofuser.Columns.Add("PY");
                    diagofuser.Columns.Add("WB");
                    diagofuser.Columns["ICD"].Caption = "诊断编码";
                    diagofuser.Columns["DIAGNOSIS"].Caption = "诊断名称";
                    DataTable diag = sql_Helper.ExecuteDataTable("usp_getdiagbyattendphysician",
                         new SqlParameter[] { new SqlParameter("@USERID", m_app.User.Id) }, CommandType.StoredProcedure);
                    if (diag.Rows.Count != 0)
                    {
                        string diagnames = diag.Rows[0][0].ToString();
                        string[] name = diagnames.Split('，');
                        for (int i = 0; i < name.Length; i++)
                        {
                            if (name[i].Trim() != "")
                            {
                                if (name[i].Trim().Contains("$"))
                                {
                                    string[] str = name[i].Split('$');
                                    DataRow dr = diagofuser.NewRow();
                                    dr[0] = str[0];
                                    dr[1] = str[1];
                                    diagofuser.Rows.Add(dr);
                                }
                                else if (name[i].Trim().Contains("-"))
                                {
                                    string[] str = name[i].Split('-');
                                    DataRow dr = diagofuser.NewRow();
                                    dr[0] = str[0];
                                    dr[1] = str[1];
                                    diagofuser.Rows.Add(dr);
                                }
                            }
                        }
                    }
                    #endregion

                    diagDt = diagofuser;
                    new GenerateShortCode(sql_Helper).AutoAddShortCode(diagDt, "DIAGNOSIS");
                    col.Add("ICD", 60);
                    col.Add("DIAGNOSIS", 120);
                    SqlWordbook inWordBook = new SqlWordbook("inDiag", diagDt, "ICD", "DIAGNOSIS", col, "ICD//DIAGNOSIS//PY//WB");
                    this.lookUpEditorInDiag.SqlWordbook = inWordBook;
                    SqlWordbook outWordBook = new SqlWordbook("outDiag", diagDt, "ICD", "DIAGNOSIS", col, "ICD//DIAGNOSIS//PY//WB");
                    this.lookUpEditorOutDiag.SqlWordbook = outWordBook;
                    SqlWordbook currentWordBook = new SqlWordbook("curDiag", diagDt, "ICD", "DIAGNOSIS", col, "ICD//DIAGNOSIS//PY//WB");
                    this.lookUpEditorCurrentDiag.SqlWordbook = currentWordBook;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化病种组合
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-10</date>
        /// </summary>
        private void InitDiseaseGroup()
        {
            try
            {
                this.lookUpWindowInDiagGroup.SqlHelper = sql_Helper;
                this.lookUpWindowOutDiagGroup.SqlHelper = sql_Helper;
                DataTable diseaseGroupDT = DS_BaseService.GetDiseaseGroupsByUserID(DS_Common.currentUser.Id);
                if (null == diseaseGroupDT || diseaseGroupDT.Rows.Count == 0)
                {
                    if (!diseaseGroupDT.Columns.Contains("ID"))
                    {
                        diseaseGroupDT.Columns.Add("ID");
                    }
                    if (!diseaseGroupDT.Columns.Contains("NAME"))
                    {
                        diseaseGroupDT.Columns.Add("NAME");
                    }
                    if (!diseaseGroupDT.Columns.Contains("PY"))
                    {
                        diseaseGroupDT.Columns.Add("PY");
                    }
                    if (!diseaseGroupDT.Columns.Contains("WB"))
                    {
                        diseaseGroupDT.Columns.Add("WB");
                    }
                }
                diseaseGroupDT.Columns["NAME"].Caption = "诊断组合";

                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("NAME", lookUpEditorInDiagGroup.Width);

                SqlWordbook inGroupWordBook = new SqlWordbook("queryname", diseaseGroupDT, "ID", "NAME", cols, "NAME//PY//WB");
                SqlWordbook outGroupWordBook = new SqlWordbook("queryname", diseaseGroupDT, "ID", "NAME", cols, "NAME//PY//WB");
                this.lookUpEditorInDiagGroup.SqlWordbook = inGroupWordBook;
                this.lookUpEditorOutDiagGroup.SqlWordbook = outGroupWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


        //根据病种ICD返回病种名称 --add by wyt 2012-08-15
        private string GetDiagNameByICD(string icd)
        {
            string name = "";
            for (int i = 0; i < icd.Length; i++)
            {
                DataTable diagname = sql_Helper.ExecuteDataTable("usp_getdiagnamebyicd",
                     new SqlParameter[] { new SqlParameter("@ICD", icd) }, CommandType.StoredProcedure);
                if (diagname.Rows.Count != 0)
                {
                    name = diagname.Rows[0][0].ToString();
                }
            }
            return name;
        }

        //根据病种name返回病种icd --add by wyt 2012-08-15
        private string GetDiagICDByName(string name)
        {
            string icd = "";
            for (int i = 0; i < name.Length; i++)
            {
                DataTable diagicd = sql_Helper.ExecuteDataTable("usp_getdiagicdbyname",
                     new SqlParameter[] { new SqlParameter("@NAME", name) }, CommandType.StoredProcedure);
                if (diagicd.Rows.Count != 0)
                {
                    icd = diagicd.Rows[0][0].ToString();
                }
            }
            return icd;
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by ywk 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MedicalRecordView_Load(object sender, EventArgs e)
        {
            try
            {
                this.sql_Helper = m_app.SqlHelper;
                InitSqlWorkBook();
                InitDiseaseGroup();
                Reset();
                this.dateEditInBegin.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 得到配置信息  ywk 2012年5月21日 11:29:14
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] GetConfigValueByKey(string key)
        {
            try
            {
                if (m_app == null) return null;
                string sql1 = " select * from appcfg where configkey = '" + key + "'  ";
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
                string config = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    config = dt.Rows[0]["value"].ToString();
                }
                if (config != "")
                {
                    string[] configs = config.Split(',');
                    return configs;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        #region public methods of WaitDialog

        public void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }

        }

        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }
        #endregion

        /// <summary>
        /// 界面中调用的等待窗体
        /// add by ywk 2012年12月13日16:04:41
        /// </summary>
        private WaitDialogForm m_WaitDialog;

        /// <summary>
        /// 查询事件
        /// edit by Yanqiao.Cai 2012-11-15
        /// 1、add try ... catch
        /// 2、性别加载图片
        /// Modify by xlb 2013-06-19 解决检索不出当天数据的问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryMedicalRecord_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sqlParam;
                DataTable MedicalRrecordSet = new DataTable();

                //性别加载图片
                m_WaitDialog = new WaitDialogForm("正在查询数据...", "请稍等");
                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListBrxb);
                sqlParam = new SqlParameter[] 
                    { 
                        new SqlParameter("@DeptID", SqlDbType.VarChar),
                        new SqlParameter("@DateOutHosBegin", SqlDbType.VarChar),
                        new SqlParameter("@DateOutHosEnd", SqlDbType.VarChar),
                        new SqlParameter("@DateInHosBegin", SqlDbType.VarChar),
                        new SqlParameter("@DateInHosEnd", SqlDbType.VarChar),
                        new SqlParameter("@Status", SqlDbType.VarChar),
                        new SqlParameter("@PatientName", SqlDbType.VarChar),
                        new SqlParameter("@RecordID", SqlDbType.VarChar),
                        new SqlParameter("@InDiag", SqlDbType.VarChar),
                        new SqlParameter("@OutDiag", SqlDbType.VarChar),
                        new SqlParameter("@CurDiag", SqlDbType.VarChar),
                        new SqlParameter("@diaGroupStatus", SqlDbType.VarChar),
                        new SqlParameter("@PatientStatus", SqlDbType.VarChar),
                        new SqlParameter("@ingroupid", SqlDbType.VarChar),
                        new SqlParameter("@outgroupid", SqlDbType.VarChar)
                    };
                //科室编码
                sqlParam[0].Value = lookUpEditorDepartment.CodeValue == "0000" ? "" : lookUpEditorDepartment.CodeValue;
                //出院起始时间
                sqlParam[1].Value = dateEditStart.DateTime.Date.ToString("yyyy-MM-dd 00:00:00");
                //DateTime dteditEnd = DateTime.Now;
                //出院结束时间
                sqlParam[2].Value = dateEditEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                //if (dateEditEnd.DateTime.Date.ToString("yyyy-MM-dd") != "0001-01-01")
                //{
                //    dteditEnd = dateEditEnd.DateTime.Date;
                //}
                //sqlParam[2].Value = dteditEnd.ToString("yyyy-MM-dd");
                //入院开始时间
                sqlParam[3].Value = dateEditInBegin.DateTime.Date.ToString("yyyy-MM-dd 00:00:00");

                //DateTime dteditInEnd = DateTime.Now;
                //if (dateEditInEnd.DateTime.Date.ToString("yyyy-MM-dd") != "0001-01-01")
                //{
                //    dteditInEnd = dateEditInEnd.DateTime.Date;
                //}
                sqlParam[4].Value = dateEditInEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                //sqlParam[4].Value = dteditInEnd.ToString("yyyy-MM-dd");
                sqlParam[5].Value = "4700"; //"'4700','4702','4703'";

                if (cmbPatientInfo.SelectedIndex == 0)
                {//病历号
                    sqlParam[6].Value = string.Empty;
                    sqlParam[7].Value = txtcmbPatientInfo.Text.Trim();
                }
                else
                {//病人姓名
                    sqlParam[6].Value = txtcmbPatientInfo.Text.Trim();
                    sqlParam[7].Value = string.Empty;
                }
                sqlParam[8].Value = this.lookUpEditorInDiag.CodeValue.Trim();
                sqlParam[9].Value = this.lookUpEditorOutDiag.CodeValue.Trim();
                sqlParam[10].Value = "";// this.lookUpEditorCurrentDiag.CodeValue.Trim();
                sqlParam[11].Value = this.rdg_diag.SelectedIndex.ToString();
                #region
                //if (this.rdg_patient.SelectedIndex == 2)
                //{
                //    sqlParam[11].Value = "1";
                //    sqlParam[12].Value = "2";
                //}
                //else
                //{
                //    sqlParam[11].Value = "1";
                //    sqlParam[12].Value = "1";
                //}
                #endregion
                sqlParam[12].Value = this.rdg_patient.SelectedIndex.ToString();
                sqlParam[13].Value = "";
                sqlParam[14].Value = "";
                if (this.rdg_diag.SelectedIndex == 1)
                {
                    sqlParam[8].Value = "";
                    sqlParam[9].Value = "";
                    //入院组合
                    if (!string.IsNullOrEmpty(this.lookUpEditorInDiagGroup.CodeValue))
                    {
                        #region
                        //int groupID = int.Parse(this.lookUpEditorInDiagGroup.CodeValue.Trim());
                        //List<string> diseaseIDs = DS_BaseService.GetDiseaseIDsByGroupID(groupID);
                        //string filterExpression = string.Empty;
                        //for (int i = 0; i < diseaseIDs.Count; i++)
                        //{
                        //    filterExpression += "'" + diseaseIDs[i] + "',";
                        //}
                        //filterExpression = filterExpression.Substring(0, filterExpression.Length - 1);
                        sqlParam[13].Value = "";//"admitdiagnosis in (" + filterExpression + ")";
                        #endregion
                        sqlParam[13].Value = this.lookUpEditorInDiagGroup.CodeValue.Trim();
                    }
                    //出院组合
                    if (!string.IsNullOrEmpty(this.lookUpEditorOutDiagGroup.CodeValue))
                    {
                        #region
                        //int groupID = int.Parse(this.lookUpEditorOutDiagGroup.CodeValue.Trim());
                        //List<string> diseaseIDs = DS_BaseService.GetDiseaseIDsByGroupID(groupID);
                        //string filterExpression = string.Empty;
                        //for (int i = 0; i < diseaseIDs.Count; i++)
                        //{
                        //    filterExpression += "'" + diseaseIDs[i] + "',";
                        //}
                        //filterExpression = filterExpression.Substring(0, filterExpression.Length - 1);
                        sqlParam[13].Value = "";//"diagnosis_code in (" + filterExpression + ")";
                        #endregion
                        sqlParam[14].Value = this.lookUpEditorOutDiagGroup.CodeValue.Trim();
                    }
                }
                MedicalRrecordSet = sql_Helper.ExecuteDataTable("usp_getattendrecordnoonfile", sqlParam, CommandType.StoredProcedure);
                #region
                ////edit by cyq 2013-01-10
                //List<DataRow> fillterList = MedicalRrecordSet.Select(" 1=1 ").ToList();
                //if (this.rdg_patient.SelectedIndex == 1)
                //{//在院病人
                //    fillterList = fillterList.Where(p => p["STATUS"].ToString() == "1500" || p["STATUS"].ToString() == "1501").ToList();
                //}
                //else if (this.rdg_patient.SelectedIndex == 2)
                //{//出院病人
                //    fillterList = fillterList.Where(p => p["STATUS"].ToString() == "1502" || p["STATUS"].ToString() == "1503").ToList();
                //}
                //if (null != fillterList && fillterList.Count() > 0)
                //{
                //    MedicalRrecordSet = fillterList.CopyToDataTable();
                //}
                //else
                //{
                //    MedicalRrecordSet = MedicalRrecordSet.Clone();
                //}

                //List<DataRow> fillterList1 = fillterList;

                ////按组合  zyx 2013-1-11
                //if (this.rdg_diag.SelectedIndex == 1)
                //{
                //    //入院组合
                //    if (!string.IsNullOrEmpty(this.lookUpEditorInDiagGroup.CodeValue))
                //    {
                //        int groupID = int.Parse(this.lookUpEditorInDiagGroup.CodeValue.Trim());
                //        List<string> diseaseIDs = DS_BaseService.GetDiseaseIDsByGroupID(groupID);
                //        string filterExpression = string.Empty;
                //        for (int i = 0; i < diseaseIDs.Count; i++)
                //        {
                //            filterExpression += "'" + diseaseIDs[i] + "',";
                //        }
                //        filterExpression = filterExpression.Substring(0, filterExpression.Length - 1);
                //        filterExpression = "admitdiagnosis in (" + filterExpression + ")";
                //        if (MedicalRrecordSet.Select(filterExpression).Count() > 0)
                //        {
                //            MedicalRrecordSet = MedicalRrecordSet.Select(filterExpression).CopyToDataTable();
                //        }
                //        else
                //        {
                //            gridControlMedicalRecord.DataSource = null;
                //            lbl_totalCount1.Text = "共0条记录";
                //            m_app.CustomMessageBox.MessageShow("没有符合条件的记录");
                //            HideWaitDialog();
                //            return;
                //        }
                //    }
                //    //出院组合
                //    if (!string.IsNullOrEmpty(this.lookUpEditorOutDiagGroup.CodeValue))
                //    {
                //        int groupID = int.Parse(this.lookUpEditorOutDiagGroup.CodeValue.Trim());
                //        List<string> diseaseIDs = DS_BaseService.GetDiseaseIDsByGroupID(groupID);
                //        string filterExpression = string.Empty;
                //        for (int i = 0; i < diseaseIDs.Count; i++)
                //        {
                //            filterExpression += "'" + diseaseIDs[i] + "',";
                //        }
                //        filterExpression.Substring(0, filterExpression.Length - 1);
                //        filterExpression = "diagnosis_code in (" + filterExpression + ")";
                //        if (MedicalRrecordSet.Select(filterExpression).Count() > 0)
                //        {
                //            MedicalRrecordSet = MedicalRrecordSet.Select(filterExpression).CopyToDataTable();
                //        }
                //        else
                //        {
                //            gridControlMedicalRecord.DataSource = null;
                //            lbl_totalCount1.Text = "共0条记录";
                //            m_app.CustomMessageBox.MessageShow("没有符合条件的记录");
                //            HideWaitDialog();
                //            return;
                //        }
                //    }
                //}
                #endregion
                //MedicalRrecordSet = fillterList.CopyToDataTable();
                DataTable dataSum = MedicalRrecordSet.Clone();
                //-------------分组 合并 2013-3-7 add by tj-----------------
                DataRow[] drs = MedicalRrecordSet.Select("1=1");//所有结果行
                for (int i = 0; i < drs.Length; i++)
                {
                    if (dataSum.Rows.Count == 0)
                    {
                        dataSum.Rows.Add(drs[i].ItemArray);
                        continue;
                    }
                    if (drs[i]["NOOFINPAT"].ToString().Equals(dataSum.Rows[dataSum.Rows.Count - 1]["NOOFINPAT"].ToString()))//与上一条记病案首页号相等，认为是同一病人
                    {
                        dataSum.Rows[dataSum.Rows.Count - 1]["DIAGNOSISNAME"] += ";" + drs[i]["DIAGNOSISNAME"].ToString();
                    }
                    else
                    {
                        dataSum.Rows.Add(drs[i].ItemArray);
                    }
                }
                //----------------------------------------------------------
                gridControlMedicalRecord.DataSource = dataSum;
                lbl_totalCount1.Text = "共" + dataSum.Rows.Count + "条记录";
                m_app.PublicMethod.ConvertGridDataSourceUpper(gridviewMedicalRecord);
                this.HideWaitDialog();

                if (MedicalRrecordSet.Rows.Count <= 0)
                {
                    MessageBox.Show("没有符合条件的记录");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-29</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, EventArgs e)
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
        /// 重置
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-29</date>
        /// </summary>
        private void Reset()
        {
            try
            {
                dateEditStart.DateTime = DateTime.Now.AddMonths(-3);
                dateEditEnd.DateTime = DateTime.Now;
                dateEditInBegin.DateTime = DateTime.Now.AddMonths(-3);
                dateEditInEnd.DateTime = DateTime.Now;

                this.lookUpEditorInDiag.CodeValue = string.Empty;
                this.lookUpEditorOutDiag.CodeValue = string.Empty;
                this.lookUpEditorCurrentDiag.CodeValue = string.Empty;
                this.lookUpEditorDepartment.CodeValue = DS_Common.currentUser.CurrentDeptId;
                this.cmbPatientInfo.SelectedIndex = 0;
                this.txtcmbPatientInfo.Text = string.Empty;

                this.rdg_diag.SelectedIndex = 0;
                this.rdg_patient.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 阅览病历事件
        /// edit by Yanqiao.Cai 2012-11-09
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseMedicalRecord_Click(object sender, EventArgs e)
        {
            try
            {
                LoadRecord();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 列表双击事件
        /// edit by Yanqiao.Cai 2012-11-05
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridviewMedicalRecord_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridviewMedicalRecord.CalcHitInfo(gridControlMedicalRecord.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                LoadRecord();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region Enum

        /// <summary>
        /// 借阅状态
        /// </summary>
        private enum BorrowStatus : int
        {
            /// <summary>
            /// 待审批
            /// </summary>
            UnderApprove = 5201,

            /// <summary>
            /// 申请通过
            /// </summary>
            ApplyPass = 5202,

            /// <summary>
            /// 申请未通过
            /// </summary>
            ApplyUnPass = 5203,

            /// <summary>
            /// 申请作废
            /// </summary>
            ApplyInvalid = 5204,

            /// <summary>
            /// 借阅到期
            /// </summary>
            Expire = 5205,

            /// <summary>
            /// 已签收
            /// </summary>
            SignIn = 5206,
        }

        #endregion

        #region IStartPlugIn 成员

        public IPlugIn Run(IEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;

            return plg;
        }

        #endregion

        /// <summary>
        /// 导出Excel  2013-3-6 by tj
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "导出Excel";
                saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
                DialogResult dialogResult = saveFileDialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    options.SheetName = "病历信息";
                    options.ShowGridLines = true;

                    string caption = gridControlMedicalRecord.MainView.ViewCaption;
                    gridControlMedicalRecord.MainView.ViewCaption = options.SheetName;
                    gridControlMedicalRecord.ExportToXls(saveFileDialog.FileName, options);
                    MessageBox.Show("导出成功");
                    gridControlMedicalRecord.MainView.ViewCaption = caption;
                }
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
        private void gridviewMedicalRecord_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 按诊断、按组合切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdg_diag_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rdg_diag.SelectedIndex == 0)
                {//按诊断
                    this.lookUpEditorInDiag.Enabled = true;
                    this.lookUpEditorOutDiag.Enabled = true;
                    this.lookUpEditorInDiagGroup.Enabled = false;
                    this.lookUpEditorOutDiagGroup.Enabled = false;
                    this.lookUpEditorInDiagGroup.CodeValue = string.Empty;
                    this.lookUpEditorOutDiagGroup.CodeValue = string.Empty;
                    this.lookUpEditorInDiag.Focus();
                }
                else if (this.rdg_diag.SelectedIndex == 1)
                {//按组合
                    this.lookUpEditorInDiag.Enabled = false;
                    this.lookUpEditorOutDiag.Enabled = false;
                    this.lookUpEditorInDiagGroup.Enabled = true;
                    this.lookUpEditorOutDiagGroup.Enabled = true;
                    this.lookUpEditorInDiag.CodeValue = string.Empty;
                    this.lookUpEditorOutDiag.CodeValue = string.Empty;
                    this.lookUpEditorInDiagGroup.Focus();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}
