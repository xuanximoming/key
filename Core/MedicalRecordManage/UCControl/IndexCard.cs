using DevExpress.Utils;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Service;
using DrectSoft.DSSqlHelper;
using MedicalRecordManage.Object;
using MedicalRecordManage.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace MedicalRecordManage.UCControl
{
    public partial class IndexCard : UserControl
    {
        CGridCheckMarksSelection selection;
        internal CGridCheckMarksSelection Selection
        {
            get
            {
                return selection;
            }
        }

        public IndexCard()
        {
            InitializeComponent();
        }

        private void IndexCard_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeGrid();
                Init();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 事件
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Query_Click(object sender, EventArgs e)
        {
            try
            {
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    MyMessageBox.Show(errorStr, "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
                WaitDialogForm m_WaitDialog = new WaitDialogForm("正在查询数据...", "请稍后");
                //InitOptions();
                if (radio_CardType.SelectedIndex == 0)//姓名索引卡
                {
                    RefreshNameCardData();
                }
                else if (radio_CardType.SelectedIndex == 1)//死亡索引卡
                {
                    RefreshDeathCardData();
                }
                else if (radio_CardType.SelectedIndex == 2)//手术索引卡
                {
                    RefreshOperationCardData();
                }
                else if (radio_CardType.SelectedIndex == 3)//疾病索引卡
                {
                    RefreshDiseaseCardData();
                }
                Selection.SelectAll();

                m_WaitDialog.Hide();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSet_Click(object sender, EventArgs e)
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
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Print_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable dt = gridControlMain.DataSource as DataTable;

                //DataTable dtTemp = dt.Clone();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (dr["SELECT"].ToString().ToUpper() == "TRUE")
                //    {
                //        DataRow drTemp = dtTemp.NewRow();
                //        drTemp.ItemArray = dr.ItemArray;
                //        dtTemp.Rows.Add(drTemp);
                //    }
                //}
                //dtTemp.AcceptChanges();

                if (Selection.selection.Count > 0)
                {
                    if (Selection.selection.Cast<DataRowView>().ToList().Count > 280)
                    {
                        if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("打印预览最多只能28页，是否继续？", "打印预览", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            PrintForm printForm = new PrintForm(Selection.selection.Cast<DataRowView>().ToList(), radio_CardType.SelectedIndex);
                            printForm.WindowState = FormWindowState.Maximized;
                            printForm.ShowDialog();
                        }
                    }
                    else
                    {
                        PrintForm printForm = new PrintForm(Selection.selection.Cast<DataRowView>().ToList(), radio_CardType.SelectedIndex);
                        printForm.WindowState = FormWindowState.Maximized;
                        printForm.ShowDialog();
                    }

                }
                else
                {
                    MessageBox.Show("请选择需要打印的记录");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 索引卡类型切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radio_CardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                InitOptions();
                if (radio_CardType.SelectedIndex == 0)
                {///姓名索引卡
                    RefreshNameCardData();
                }
                else if (radio_CardType.SelectedIndex == 1)
                {///死亡索引卡
                    RefreshDeathCardData();
                }
                else if (radio_CardType.SelectedIndex == 2)
                {///手术索引卡
                    RefreshOperationCardData();
                }
                else if (radio_CardType.SelectedIndex == 3)
                {///疾病索引卡
                    RefreshDiseaseCardData();
                }
                this.Selection.SelectAll();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 在院状态切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radio_HosState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (radio_HosState.SelectedIndex == 1)
                {
                    date_OutHosBegin.Enabled = false;
                    date_OutHosEnd.Enabled = false;
                    //date_OutHosBegin.Text = string.Empty;
                    //date_OutHosEnd.Text = string.Empty;
                }
                else
                {
                    date_OutHosBegin.Enabled = true;
                    date_OutHosEnd.Enabled = true;
                    //date_OutHosBegin.DateTime = DateTime.Now.AddMonths(-1);
                    //date_OutHosEnd.DateTime = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMain_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        #endregion

        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            try
            {
                date_InHosBegin.DateTime = DateTime.Now.AddMonths(-1);
                date_InHosEnd.DateTime = DateTime.Now;
                date_OutHosBegin.DateTime = DateTime.Now.AddMonths(-1);
                date_OutHosEnd.DateTime = DateTime.Now;
                ComponentCommand.InitializeDepartment(ref this.lookUpEditorDepartment, ref this.lookUpWindowDepartment);
                lookUpEditorDepartment.CodeValue = DS_Common.currentUser.CurrentDeptId;
                ///初始化显示项(默认为姓名索引卡)
                InitOptions();
                txt_Patid.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化索引卡显示项
        /// </summary>
        private void InitOptions()
        {
            try
            {
                string config = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                if (!string.IsNullOrEmpty(config))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(config);
                    XmlNodeList nodeList = doc.GetElementsByTagName("IsReadAddressInfo");
                    if (null != nodeList && nodeList.Count > 0)
                    {
                        string cfgValue = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                        if (cfgValue == "0")
                        {//省、市、县
                            //gridViewMain.Columns[gridViewMain.Columns.Count - 1].FieldName = "XZZADDRESSUNION";
                            if (gridViewMain.Columns["XZZADDRESS"] != null)//edit by ywk 
                            {
                                gridViewMain.Columns["XZZADDRESS"].FieldName = "XZZADDRESSUNION";
                            }

                        }
                        else
                        {//长地址
                            //gridViewMain.Columns[gridViewMain.Columns.Count - 1].FieldName = "XZZADDRESS";
                            if (gridViewMain.Columns["XZZADDRESS"] != null)//edit by ywk 
                            {
                                gridViewMain.Columns["XZZADDRESS"].FieldName = "XZZADDRESS";
                            }
                        }
                    }
                }

                if (radio_CardType.SelectedIndex == 0)
                {///姓名索引卡
                    gridViewMain.Columns["ZG_NAME"].Visible = true;
                    gridViewMain.Columns["DEATHDATE"].Visible = false;
                    gridViewMain.Columns["DEATHTIME"].Visible = false;
                    gridViewMain.Columns["EMERGENCY_TIMES"].Visible = false;
                    gridViewMain.Columns["OPERATIONNAME"].Visible = false;
                }
                else if (radio_CardType.SelectedIndex == 1)
                {///死亡索引卡
                    gridViewMain.Columns["ZG_NAME"].Visible = false;
                    gridViewMain.Columns["DEATHDATE"].Visible = true;
                    gridViewMain.Columns["DEATHTIME"].Visible = true;
                    gridViewMain.Columns["EMERGENCY_TIMES"].Visible = true;
                    gridViewMain.Columns["DEATHDATE"].VisibleIndex = 10;
                    gridViewMain.Columns["DEATHTIME"].VisibleIndex = 11;
                    gridViewMain.Columns["EMERGENCY_TIMES"].VisibleIndex = 12;
                    gridViewMain.Columns["OPERATIONNAME"].Visible = false;
                }
                else if (radio_CardType.SelectedIndex == 2)
                {///手术索引卡
                    gridViewMain.Columns["ZG_NAME"].Visible = false;
                    gridViewMain.Columns["DEATHDATE"].Visible = false;
                    gridViewMain.Columns["DEATHTIME"].Visible = false;
                    gridViewMain.Columns["EMERGENCY_TIMES"].Visible = false;
                    gridViewMain.Columns["OPERATIONNAME"].Visible = true;
                    gridViewMain.Columns["DIAGNOSIS_NAME"].Visible = false;
                    gridViewMain.Columns["DIAGNOSIS_NAME"].Visible = false;
                }
                else if (radio_CardType.SelectedIndex == 3)
                {///疾病索引卡
                    gridViewMain.Columns["ZG_NAME"].Visible = false;
                    gridViewMain.Columns["DEATHDATE"].Visible = false;
                    gridViewMain.Columns["DEATHTIME"].Visible = false;
                    gridViewMain.Columns["EMERGENCY_TIMES"].Visible = false;
                    gridViewMain.Columns["OPERATIONNAME"].Visible = false;
                    gridViewMain.Columns["DIAGNOSIS_NAME"].Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void Reset()
        {
            try
            {
                date_InHosBegin.DateTime = DateTime.Now.AddMonths(-1);
                date_InHosEnd.DateTime = DateTime.Now;
                date_OutHosBegin.Enabled = true;
                date_OutHosEnd.Enabled = true;
                date_OutHosBegin.DateTime = DateTime.Now.AddMonths(-1);
                date_OutHosEnd.DateTime = DateTime.Now;
                lookUpEditorDepartment.CodeValue = DS_Common.currentUser.CurrentDeptId;
                txt_Patid.Text = string.Empty;
                radio_CardType.SelectedIndex = 0;
                radio_HosState.SelectedIndex = 0;
                txt_Patid.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询条件检查
        /// </summary>
        /// <returns></returns>
        private string CheckItem()
        {
            try
            {
                if (date_InHosBegin.DateTime > date_InHosEnd.DateTime)
                {
                    date_InHosBegin.Focus();
                    return "入院开始日期不能大于结束日期";
                }
                else if (date_OutHosBegin.DateTime > date_OutHosEnd.DateTime)
                {
                    date_OutHosBegin.Focus();
                    return "出院开始日期不能大于结束日期";
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刷新数据 - 姓名索引卡
        /// </summary>
        private void RefreshNameCardData()
        {
            try
            {
                string sqlStr = @" select i.noofinpat,i.patid,i.name,i.py,i.wb,i.sexid,i.age,i.agestr,i.outbed,
                                          case when i.status in(1502,1503) then '出院' else '在院' end as statusname,
                                          i.admitdate,i.inwarddate,i.admitdept,d1.name as admitdeptname,i.outhosdate,
                                          i.outwarddate,i.outhosdept,
                                          (sp.provincename || sc.cityname || sd.districtname || i.xzzdetailaddress) as xzzaddressunion,
                                          i.xzzaddress,d2.name as outhosdeptname,dia.diagnosis_code,dia.diagnosis_name,bas.zg_flag,
                                          case bas.zg_flag when '1' then '治愈' 
                                                           when '2' then '好转' 
                                                           when '3' then '未愈' 
                                                           when '4' then '死亡' 
                                          end zg_name,i.status
                                     from inpatient i 
                                     left join department d1 on i.admitdept=d1.id 
                                     left join department d2 on i.outhosdept=d2.id 
                                     left join iem_mainpage_basicinfo_2012 bas on i.noofinpat=bas.noofinpat 
                                     left join iem_mainpage_diagnosis_2012 dia on bas.iem_mainpage_no=dia.iem_mainpage_no 
                                     left join s_province sp on i.xzzproviceid=sp.provinceid 
                                     left join s_city sc on i.xzzcityid=sc.cityid 
                                     left join s_district sd on i.xzzdistrictid=sd.districtid where 1=1 ";
                List<OracleParameter> paramList = new List<OracleParameter>();
                if (!string.IsNullOrEmpty(date_InHosBegin.Text))
                {///入院开始时间
                    sqlStr += " and i.inwarddate >= @inwarddatebegin ";
                    OracleParameter param1 = new OracleParameter("inwarddatebegin", OracleType.VarChar);
                    param1.Value = date_InHosBegin.DateTime.ToString("yyyy-MM-dd 00:00:00");
                    paramList.Add(param1);
                }
                if (!string.IsNullOrEmpty(date_InHosEnd.Text))
                {///入院结束时间
                    sqlStr += " and i.inwarddate <= @inwarddateend ";
                    OracleParameter param2 = new OracleParameter("inwarddateend", OracleType.VarChar);
                    param2.Value = date_InHosEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                    paramList.Add(param2);
                }
                if (radio_HosState.SelectedIndex == 1)
                {///在院状态
                    sqlStr += " and i.status not in(1502,1503) ";
                }
                else
                {
                    string outHosSql = string.Empty;
                    if (!string.IsNullOrEmpty(date_OutHosBegin.Text))
                    {///出院开始时间
                        outHosSql += " and i.outwarddate >= @outwarddatebegin ";
                        OracleParameter param3 = new OracleParameter("outwarddatebegin", OracleType.VarChar);
                        param3.Value = date_OutHosBegin.DateTime.ToString("yyyy-MM-dd 00:00:00");
                        paramList.Add(param3);
                    }
                    if (!string.IsNullOrEmpty(date_OutHosEnd.Text))
                    {///出院结束时间
                        outHosSql += " and i.outwarddate <= @outwarddateend ";
                        OracleParameter param4 = new OracleParameter("outwarddateend", OracleType.VarChar);
                        param4.Value = date_OutHosEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                        paramList.Add(param4);
                    }
                    if (!string.IsNullOrEmpty(outHosSql))
                    {
                        if (radio_HosState.SelectedIndex == 0)
                        {///全部
                            sqlStr += " and (i.status not in(1502,1503) or (i.status in(1502,1503) " + outHosSql + ") ) ";
                        }
                        else if (radio_HosState.SelectedIndex == 2)
                        {///出院状态 Modified By wwj 2013-08-07
                            sqlStr += outHosSql + " and i.status in(1502,1503) ";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(lookUpEditorDepartment.CodeValue.ToString()) && lookUpEditorDepartment.CodeValue.ToString() != "0000")
                {///科室
                    sqlStr += " and i.outhosdept = @outhosdept ";
                    OracleParameter param5 = new OracleParameter("outhosdept", OracleType.VarChar);
                    param5.Value = lookUpEditorDepartment.CodeValue.ToString();
                    paramList.Add(param5);
                }
                if (!string.IsNullOrEmpty(txt_Patid.Text.Trim()))
                {///住院号
                    string patid = DS_Common.FilterSpecialCharacter(txt_Patid.Text.Trim());
                    sqlStr += " and i.patid like '%" + patid + "%' ";
                }
                sqlStr += " and dia.diagnosis_type_id=7 and dia.valide=1 and bas.valide=1 order by i.name,i.outwarddate,dia.order_value";

                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListBrxb);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, paramList, CommandType.Text);

                //在此dt中将姓名拼音码产生更新到PY列 add by ywk 2013年8月30日 11:18:01

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["PY"] = ChinessConverter.GetPYResultStr(dt.Rows[i]["NAME"].ToString().Trim());
                }
                DataView dv = dt.DefaultView;
                dv.Sort = "py asc";
                dt = dv.ToTable();

                SetDataTableAllNotSelect(dt);
                if (null != dt && dt.Rows.Count > 0)
                {
                    var rowList = dt.AsEnumerable().GroupBy(p => p["patid"]);
                    List<DataRow> resultList = new List<DataRow>();
                    foreach (var keyAndValue in rowList)
                    {
                        resultList.Add(keyAndValue.FirstOrDefault());
                    }
                    gridControlMain.DataSource = resultList.CopyToDataTable();
                    lbl_TotalCount.Text = "共" + resultList.Count() + "条记录";
                }
                else
                {
                    gridControlMain.DataSource = dt.Clone();
                    lbl_TotalCount.Text = "共0条记录";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刷新数据 - 死亡索引卡
        /// </summary>
        private void RefreshDeathCardData()
        {
            try
            {
                string sqlStr = @" select i.noofinpat,i.patid,i.name,i.py,i.wb,i.sexid,i.age,i.agestr,i.outbed,
                                          case when i.status in(1502,1503) then '出院' else '在院' end as statusname,i.admitdate,i.inwarddate,
                                          i.admitdept,d1.name as admitdeptname,i.outhosdate,i.outwarddate,i.outhosdept,
                                          (sp.provincename || sc.cityname || sd.districtname || i.xzzdetailaddress) as xzzaddressunion,
                                          i.xzzaddress,d2.name as outhosdeptname,dia.diagnosis_code,dia.diagnosis_name,
                                          to_char(to_date(i.deathdatetime,'yyyy-MM-dd HH24:mi:ss'),'yyyy-MM-dd') as deathdate,
                                          to_char(to_date(i.deathdatetime,'yyyy-MM-dd HH24:mi:ss'),'HH24:mi:ss') as deathtime,
                                          oth.emergency_times, i.status
                                    from inpatient i 
                                    left join department d1 on i.admitdept=d1.id left join department d2 on i.outhosdept=d2.id 
                                    left join iem_mainpage_basicinfo_2012 bas on i.noofinpat=bas.noofinpat 
                                    left join iem_mainpage_diagnosis_2012 dia on bas.iem_mainpage_no=dia.iem_mainpage_no 
                                    left join iem_mainpage_other_2012 oth on bas.iem_mainpage_no=oth.iem_mainpage_no 
                                    left join s_province sp on i.xzzproviceid=sp.provinceid 
                                    left join s_city sc on i.xzzcityid=sc.cityid left join s_district sd on i.xzzdistrictid=sd.districtid 
                                    where 1=1 ";
                //string sqlStr = " select i.noofinpat,i.patid,i.name,i.py,i.wb,i.sexid,i.age,i.agestr,i.outbed,case when i.status in(1502,1503) then '出院' else '在院' end as statusname,i.admitdate,i.inwarddate,i.admitdept,d1.name as admitdeptname,i.outhosdate,i.outwarddate,i.outhosdept,(sp.provincename || sc.cityname || sd.districtname || i.xzzdetailaddress) as xzzaddressunion,i.xzzaddress,d2.name as outhosdeptname,dia.diagnosis_code,dia.diagnosis_name,i.deathdatetime deathdate,i.deathdatetime  as deathtime,oth.emergency_times from inpatient i left join department d1 on i.admitdept=d1.id left join department d2 on i.outhosdept=d2.id left join iem_mainpage_basicinfo_2012 bas on i.noofinpat=bas.noofinpat left join iem_mainpage_diagnosis_2012 dia on bas.iem_mainpage_no=dia.iem_mainpage_no  left join s_province sp on i.xzzproviceid=sp.provinceid left join s_city sc on i.xzzcityid=sc.cityid left join s_district sd on i.xzzdistrictid=sd.districtid where 1=1 ";
                List<OracleParameter> paramList = new List<OracleParameter>();
                if (!string.IsNullOrEmpty(date_InHosBegin.Text))
                {///入院开始时间
                    sqlStr += " and i.inwarddate >= @inwarddatebegin ";
                    OracleParameter param1 = new OracleParameter("inwarddatebegin", OracleType.VarChar);
                    param1.Value = date_InHosBegin.DateTime.ToString("yyyy-MM-dd 00:00:00");
                    paramList.Add(param1);
                }
                if (!string.IsNullOrEmpty(date_InHosEnd.Text))
                {///入院结束时间
                    sqlStr += " and i.inwarddate <= @inwarddateend ";
                    OracleParameter param2 = new OracleParameter("inwarddateend", OracleType.VarChar);
                    param2.Value = date_InHosEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                    paramList.Add(param2);
                }
                if (radio_HosState.SelectedIndex == 1)
                {///在院状态
                    sqlStr += " and i.status not in(1502,1503) ";
                }
                else
                {
                    string outHosSql = string.Empty;
                    if (!string.IsNullOrEmpty(date_OutHosBegin.Text))
                    {///出院开始时间
                        outHosSql += " and i.outwarddate >= @outwarddatebegin ";
                        OracleParameter param3 = new OracleParameter("outwarddatebegin", OracleType.VarChar);
                        param3.Value = date_OutHosBegin.DateTime.ToString("yyyy-MM-dd 00:00:00");
                        paramList.Add(param3);
                    }
                    if (!string.IsNullOrEmpty(date_OutHosEnd.Text))
                    {///出院结束时间
                        outHosSql += " and i.outwarddate <= @outwarddateend ";
                        OracleParameter param4 = new OracleParameter("outwarddateend", OracleType.VarChar);
                        param4.Value = date_OutHosEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                        paramList.Add(param4);
                    }
                    if (!string.IsNullOrEmpty(outHosSql))
                    {
                        if (radio_HosState.SelectedIndex == 0)
                        {///全部
                            sqlStr += " and (i.status not in(1502,1503) or (i.status in(1502,1503) " + outHosSql + ") ) ";
                        }
                        else if (radio_HosState.SelectedIndex == 2)
                        {///出院状态 Modified By wwj 2013-08-07
                            sqlStr += outHosSql + " and i.status in(1502,1503) ";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(lookUpEditorDepartment.CodeValue.ToString()) && lookUpEditorDepartment.CodeValue.ToString() != "0000")
                {///科室
                    sqlStr += " and i.outhosdept = @outhosdept ";
                    OracleParameter param5 = new OracleParameter("outhosdept", OracleType.VarChar);
                    param5.Value = lookUpEditorDepartment.CodeValue.ToString();
                    paramList.Add(param5);
                }
                if (!string.IsNullOrEmpty(txt_Patid.Text.Trim()))
                {///住院号
                    string patid = DS_Common.FilterSpecialCharacter(txt_Patid.Text.Trim());
                    sqlStr += " and i.patid like '%" + patid + "%' ";
                }
                sqlStr += " and bas.outhostype='5' and bas.valide=1 and dia.diagnosis_type_id=7 and dia.valide=1  order by i.name,i.outwarddate,dia.order_value";

                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListBrxb);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, paramList, CommandType.Text);
                SetDataTableAllNotSelect(dt);
                if (null != dt && dt.Rows.Count > 0)
                {
                    var rowList = dt.AsEnumerable().GroupBy(p => p["noofinpat"]);
                    List<DataRow> resultList = new List<DataRow>();
                    foreach (var keyAndValue in rowList)
                    {
                        resultList.Add(keyAndValue.FirstOrDefault());
                    }
                    gridControlMain.DataSource = resultList.CopyToDataTable();
                    lbl_TotalCount.Text = "共" + resultList.Count() + "条记录";
                }
                else
                {
                    gridControlMain.DataSource = dt.Clone();
                    lbl_TotalCount.Text = "共0条记录";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刷新数据 - 手术索引卡
        /// Add by wwj 2013-07-29
        /// </summary>
        private void RefreshOperationCardData()
        {
            try
            {
                string sqlStr = @"select distinct i.noofinpat,i.patid,i.name,i.py,i.wb,i.sexid,i.age,i.agestr,i.outbed,
                                case when i.status in(1502,1503) then '出院' else '在院' end as statusname,
                                i.admitdate,i.inwarddate,i.admitdept,d1.name as admitdeptname,i.outhosdate,i.outwarddate,i.outhosdept,
                                (sp.provincename || sc.cityname || sd.districtname || i.xzzdetailaddress) as xzzaddressunion,
                                i.xzzaddress,d2.name as outhosdeptname, oper.id operationcode, oper.name operationname, i.status
                                from inpatient i 
                                        left join department d1 on i.admitdept=d1.id
                                        left join department d2 on i.outhosdept=d2.id
                                        left join iem_mainpage_basicinfo_2012 bas on i.noofinpat=bas.noofinpat and bas.valide = '1'
                                        left join iem_mainpage_operation_2012 imo on bas.iem_mainpage_no = imo.iem_mainpage_no and imo.valide = '1'
                                        left join operation oper on imo.operation_code = oper.id
                                        left join s_province sp on i.xzzproviceid=sp.provinceid
                                        left join s_city sc on i.xzzcityid=sc.cityid 
                                        left join s_district sd on i.xzzdistrictid=sd.districtid 
                                where 1=1 ";
                List<OracleParameter> paramList = new List<OracleParameter>();
                if (!string.IsNullOrEmpty(date_InHosBegin.Text))
                {///入院开始时间
                    sqlStr += " and i.inwarddate >= @inwarddatebegin ";
                    OracleParameter param1 = new OracleParameter("inwarddatebegin", OracleType.VarChar);
                    param1.Value = date_InHosBegin.DateTime.ToString("yyyy-MM-dd 00:00:00");
                    paramList.Add(param1);
                }
                if (!string.IsNullOrEmpty(date_InHosEnd.Text))
                {///入院结束时间
                    sqlStr += " and i.inwarddate <= @inwarddateend ";
                    OracleParameter param2 = new OracleParameter("inwarddateend", OracleType.VarChar);
                    param2.Value = date_InHosEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                    paramList.Add(param2);
                }
                if (radio_HosState.SelectedIndex == 1)
                {///在院状态
                    sqlStr += " and i.status not in(1502,1503) ";
                }
                else
                {
                    string outHosSql = string.Empty;
                    if (!string.IsNullOrEmpty(date_OutHosBegin.Text))
                    {///出院开始时间
                        outHosSql += " and i.outwarddate >= @outwarddatebegin ";
                        OracleParameter param3 = new OracleParameter("outwarddatebegin", OracleType.VarChar);
                        param3.Value = date_OutHosBegin.DateTime.ToString("yyyy-MM-dd 00:00:00");
                        paramList.Add(param3);
                    }
                    if (!string.IsNullOrEmpty(date_OutHosEnd.Text))
                    {///出院结束时间
                        outHosSql += " and i.outwarddate <= @outwarddateend ";
                        OracleParameter param4 = new OracleParameter("outwarddateend", OracleType.VarChar);
                        param4.Value = date_OutHosEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                        paramList.Add(param4);
                    }
                    if (!string.IsNullOrEmpty(outHosSql))
                    {
                        if (radio_HosState.SelectedIndex == 0)
                        {///全部
                            sqlStr += " and (i.status not in(1502,1503) or (i.status in(1502,1503) " + outHosSql + ") ) ";
                        }
                        else if (radio_HosState.SelectedIndex == 2)
                        {///出院状态 Modified By wwj 2013-08-07
                            sqlStr += outHosSql + " and i.status in(1502,1503) ";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(lookUpEditorDepartment.CodeValue.ToString()) && lookUpEditorDepartment.CodeValue.ToString() != "0000")
                {///科室
                    sqlStr += " and i.outhosdept = @outhosdept ";
                    OracleParameter param5 = new OracleParameter("outhosdept", OracleType.VarChar);
                    param5.Value = lookUpEditorDepartment.CodeValue.ToString();
                    paramList.Add(param5);
                }
                if (!string.IsNullOrEmpty(txt_Patid.Text.Trim()))
                {///住院号
                    string patid = DS_Common.FilterSpecialCharacter(txt_Patid.Text.Trim());
                    sqlStr += " and i.patid like '%" + patid + "%' ";
                }
                sqlStr += " and oper.id is not null order by i.name,i.outwarddate";

                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListBrxb);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, paramList, CommandType.Text);
                SetDataTableAllNotSelect(dt);
                if (null != dt && dt.Rows.Count > 0)
                {
                    //var rowList = dt.AsEnumerable().GroupBy(p => p["noofinpat"]);
                    //List<DataRow> resultList = new List<DataRow>();
                    //foreach (var keyAndValue in rowList)
                    //{
                    //    resultList.Add(keyAndValue.FirstOrDefault());
                    //}
                    //gridControlMain.DataSource = resultList.CopyToDataTable();
                    //lbl_TotalCount.Text = "共" + resultList.Count() + "条记录";

                    gridControlMain.DataSource = dt;
                    lbl_TotalCount.Text = "共" + dt.Rows.Count + "条记录";
                }
                else
                {
                    gridControlMain.DataSource = dt.Clone();
                    lbl_TotalCount.Text = "共0条记录";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 刷新数据 - 疾病索引卡
        /// Add by wwj 2013-07-29
        /// </summary>
        private void RefreshDiseaseCardData()
        {
            try
            {
                string sqlStr = @"select distinct i.noofinpat,i.patid,i.name,i.py,i.wb,i.sexid,i.age,i.agestr,i.outbed,
                            case when i.status in(1502,1503) then '出院' else '在院' end as statusname,
                            i.admitdate,i.inwarddate,i.admitdept,d1.name as admitdeptname,i.outhosdate,i.outwarddate,i.outhosdept,
                            (sp.provincename || sc.cityname || sd.districtname || i.xzzdetailaddress) as xzzaddressunion,
                            i.xzzaddress,d2.name as outhosdeptname, imd.diagnosis_code, diag.name DIAGNOSIS_NAME, i.status
                            from inpatient i 
                                 left join department d1 on i.admitdept=d1.id
                                 left join department d2 on i.outhosdept=d2.id
                                 left join iem_mainpage_basicinfo_2012 bas on i.noofinpat=bas.noofinpat and bas.valide = '1'
                                 left join iem_mainpage_diagnosis_2012 imd on bas.iem_mainpage_no = imd.iem_mainpage_no and imd.valide = '1'
                                 left join diagnosis diag on imd.diagnosis_code = diag.icd
                                 left join s_province sp on i.xzzproviceid=sp.provinceid
                                 left join s_city sc on i.xzzcityid=sc.cityid 
                                 left join s_district sd on i.xzzdistrictid=sd.districtid 
                            where 1=1 ";
                List<OracleParameter> paramList = new List<OracleParameter>();
                if (!string.IsNullOrEmpty(date_InHosBegin.Text))
                {///入院开始时间
                    sqlStr += " and i.inwarddate >= @inwarddatebegin ";
                    OracleParameter param1 = new OracleParameter("inwarddatebegin", OracleType.VarChar);
                    param1.Value = date_InHosBegin.DateTime.ToString("yyyy-MM-dd 00:00:00");
                    paramList.Add(param1);
                }
                if (!string.IsNullOrEmpty(date_InHosEnd.Text))
                {///入院结束时间
                    sqlStr += " and i.inwarddate <= @inwarddateend ";
                    OracleParameter param2 = new OracleParameter("inwarddateend", OracleType.VarChar);
                    param2.Value = date_InHosEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                    paramList.Add(param2);
                }
                if (radio_HosState.SelectedIndex == 1)
                {///在院状态
                    sqlStr += " and i.status not in(1502,1503) ";
                }
                else
                {
                    string outHosSql = string.Empty;
                    if (!string.IsNullOrEmpty(date_OutHosBegin.Text))
                    {///出院开始时间
                        outHosSql += " and i.outwarddate >= @outwarddatebegin ";
                        OracleParameter param3 = new OracleParameter("outwarddatebegin", OracleType.VarChar);
                        param3.Value = date_OutHosBegin.DateTime.ToString("yyyy-MM-dd 00:00:00");
                        paramList.Add(param3);
                    }
                    if (!string.IsNullOrEmpty(date_OutHosEnd.Text))
                    {///出院结束时间
                        outHosSql += " and i.outwarddate <= @outwarddateend ";
                        OracleParameter param4 = new OracleParameter("outwarddateend", OracleType.VarChar);
                        param4.Value = date_OutHosEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                        paramList.Add(param4);
                    }
                    if (!string.IsNullOrEmpty(outHosSql))
                    {
                        if (radio_HosState.SelectedIndex == 0)
                        {///全部
                            sqlStr += " and (i.status not in(1502,1503) or (i.status in(1502,1503) " + outHosSql + ") ) ";
                        }
                        else if (radio_HosState.SelectedIndex == 2)
                        {///出院状态 Modified By wwj 2013-08-07
                            sqlStr += outHosSql + " and i.status in(1502,1503) ";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(lookUpEditorDepartment.CodeValue.ToString()) && lookUpEditorDepartment.CodeValue.ToString() != "0000")
                {///科室
                    sqlStr += " and i.outhosdept = @outhosdept ";
                    OracleParameter param5 = new OracleParameter("outhosdept", OracleType.VarChar);
                    param5.Value = lookUpEditorDepartment.CodeValue.ToString();
                    paramList.Add(param5);
                }
                if (!string.IsNullOrEmpty(txt_Patid.Text.Trim()))
                {///住院号
                    string patid = DS_Common.FilterSpecialCharacter(txt_Patid.Text.Trim());
                    sqlStr += " and i.patid like '%" + patid + "%' ";
                }
                sqlStr += " and imd.diagnosis_code is not null and imd.diagnosis_type_id = '7' order by i.name,i.outwarddate";

                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListBrxb);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, paramList, CommandType.Text);
                SetDataTableAllNotSelect(dt);
                if (null != dt && dt.Rows.Count > 0)
                {
                    //var rowList = dt.AsEnumerable().GroupBy(p => p["noofinpat"]);
                    //List<DataRow> resultList = new List<DataRow>();
                    //foreach (var keyAndValue in rowList)
                    //{
                    //    resultList.Add(keyAndValue.FirstOrDefault());
                    //}
                    //gridControlMain.DataSource = resultList.CopyToDataTable();
                    //lbl_TotalCount.Text = "共" + resultList.Count() + "条记录";

                    gridControlMain.DataSource = dt;
                    lbl_TotalCount.Text = "共" + dt.Rows.Count + "条记录";
                }
                else
                {
                    gridControlMain.DataSource = dt.Clone();
                    lbl_TotalCount.Text = "共0条记录";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        private void SetDataTableAllNotSelect(DataTable dt)
        {
            try
            {
                //dt.Columns.Add(Selection.CheckMarkColumn.FieldName, typeof(Boolean));
                //foreach (DataRow dr in dt.Rows)
                //{
                //    dr[Selection.CheckMarkColumn.FieldName] = true;
                //}
                //checkBoxALL.CheckedChanged -= new EventHandler(checkBoxALL_CheckedChanged);
                //checkBoxALL.Checked = true;
                //checkBoxALL.CheckedChanged += new EventHandler(checkBoxALL_CheckedChanged);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitializeGrid()
        {
            try
            {
                for (int i = 0; i < this.gridViewMain.Columns.Count; i++)
                {
                    gridViewMain.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }

                selection = new CGridCheckMarksSelection(gridViewMain);//把多选框绑定到你指定的grid
                selection.CheckMarkColumn.VisibleIndex = 0;//使多选框排第一列

                //YD_Common.CancelMenu(date_InHosBegin, contextMenuStrip1);
                //YD_Common.CancelMenu(date_InHosEnd, contextMenuStrip1);
                //YD_Common.CancelMenu(date_OutHosBegin, contextMenuStrip1);
                //YD_Common.CancelMenu(date_OutHosEnd, contextMenuStrip1);
                //YD_Common.CancelMenu(lookUpEditorDepartment, contextMenuStrip1);
                //YD_Common.CancelMenu(txt_Patid, contextMenuStrip1);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void date_InHosBegin_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ChinessConverter.GoConvert();
        }
    }
}
