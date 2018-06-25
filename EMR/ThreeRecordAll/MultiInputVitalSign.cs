using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.BandedGrid;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Eop;
using DrectSoft.DSSqlHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.EMR.ThreeRecordAll
{
    public partial class MultiInputVitalSign : DevBaseForm
    {
        private readonly string m_xmlFilePath = "";
        private XmlDocument xmlDoc = new XmlDocument();
        List<CheckEdit> radios = new List<CheckEdit>();
        List<int> m_ChangedRowIndex = new List<int>();//数据修改的行的索引
        private bool m_dataChanged = false;//当前页面数据有无修改
        private string m_insertSql = "", m_updateSql = ""; //插入/更新数据的SQL语句头部分
        private StringBuilder m_sbSql = new StringBuilder("");//拼接Sql
        private string m_CurTimeslot = "";//当前时段
        private string m_CurrDatetime = "";  //当前日期
        private bool m_IsSaveOk = false;
        private List<string> m_ListStaticDataFields = new List<string>();//需配置的表字段(固定列)
        private int m_colIndexTimesOfShit = -1;//大便次数所在列的列Index
        private string timelotSave = "2";

        private Dictionary<string, VitalSignInfoEntity> m_ListDanymicDataFields = new Dictionary<string, VitalSignInfoEntity>();//需配置的表<字段,关联实体对象>(动态所绑定的)
        private Dictionary<string, CheckItem> m_dicCheckItems = new Dictionary<string, CheckItem>();//单元格数据检查项
        private Dictionary<string, string> m_FieldDefaultValue = new Dictionary<string, string>();//字段默认值

        #region 表字段常量
        private const string WAYOFSURVEY = "WAYOFSURVEY";
        private const string NOOFINPAT = "NOOFINPAT";
        private const string DAYSAFTERSURGERY = "DAYSAFTERSURGERY";
        private const string DAYOFHOSPITAL = "DAYOFHOSPITAL";
        private const string TIMESOFSHIT = "TIMEOFSHIT";
        private const string TEMPERATURE = "TEMPERATURE";//体温
        private const string PHYSICALCOOLING = "PHYSICALCOOLING"; //物理降温
        private const string PHYSICALHOTTING = "PHYSICALHOTTING";//物理升温
        private const string PULSE = "PULSE";//脉搏
        private const string HEARTRATE = "HEARTRATE";//心率
        private const string BREATHE = "BREATHE";//呼吸
        #endregion

        bool IsLoad = false;  //判断窗体是否加载
        public MultiInputVitalSign()
        {
            try
            {
                IsLoad = false;
                InitializeComponent();
                this.TopLevel = false;
                m_xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "Sheet\\Config.xml";
                radios.Clear();
                radios.AddRange(new CheckEdit[] { radio1, radio2, radio3, radio4, radio5, radio6 });
                dateEdit.Text = DateTime.Now.ToShortDateString();
                m_CurrDatetime = dateEdit.DateTime.ToString("yyyy-MM-dd");
                DS_SqlHelper.CreateSqlHelper();//只需在一处运行一次即可 创建连接对象
                m_insertSql = "insert into notesonnursing(ID,DATEOFSURVEY,TIMESLOT,DATEOFRECORD,DOCTOROFRECORD,";
                m_updateSql = "update notesonnursing set ";
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void MultiInputVitalSign_Load(object sender, EventArgs e)
        {
            try
            {
                WaitDialogForm m_WaitDialog = new WaitDialogForm("正在初始化数据...", "请稍后");
                //如果登陆人不是护士的话，应该不能启用保存按钮 
                //add by ywk  2013年4月23日15:37:03 
                Employee emp = new Employee(CommonObjects.CurrentUser.Id);
                emp.ReInitializeProperties();
                if (emp.Kind == EmployeeKind.Nurse)//当前登录人是护士
                {
                    DevButtonSave1.Enabled = true;
                }
                else
                {
                    DevButtonSave1.Enabled = false;
                }
                xmlDoc.Load(m_xmlFilePath);
                XmlNode nodeElement = null;
                if (xmlDoc != null)
                {
                    if (xmlDoc.GetElementsByTagName("TimelotSave") != null && (xmlDoc.GetElementsByTagName("TimelotSave").Count > 0))
                    {
                        nodeElement = xmlDoc.GetElementsByTagName("TimelotSave")[0];
                    }
                }

                if (nodeElement != null)
                {
                    timelotSave = nodeElement.Attributes["Timelot"] == null || nodeElement.Attributes["Timelot"].Value == "" ? "2" : nodeElement.Attributes["Timelot"].Value.Trim();
                }
                else
                {
                    timelotSave = "2";
                }

                CreateColumns();
                ReadCheckItems();
                ReadFieldDefaultValue();
                InitHourDuraing();
                //InitTodayRecords();
                this.bandedGridView1.BestFitColumns();
                BindComboBoxDataSource(repositoryItemGridLookUpEdit1, "88");
                BindGridRecordData(GetCurrentSelectTimeslot(), m_CurrDatetime);
                labelTimelot.Text = "【" + radio1.Tag.ToString() + " 时段】";
                m_CurTimeslot = radio1.Tag.ToString();
                m_WaitDialog.Close();
                m_WaitDialog.Dispose();
                IsLoad = true;
            }
            catch (Exception ex)
            {
                IsLoad = true;
                MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 读取检查项信息
        /// </summary>
        private void ReadCheckItems()
        {
            try
            {
                m_dicCheckItems.Clear();
                XmlNode nodeElement = xmlDoc.GetElementsByTagName("dataCheck")[0];
                XmlNodeList nodeList = nodeElement.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    CheckItem item = new CheckItem(node.InnerText, decimal.Parse(node.Attributes["maxValue"].Value), decimal.Parse(node.Attributes["minValue"].Value), node.Attributes["datafield"].Value);
                    switch (item.fieldName)
                    {
                        case TEMPERATURE:
                        case PHYSICALCOOLING:
                        case PHYSICALHOTTING:
                            SpinEditTemperareture.MaxValue = item.maxValue;
                            SpinEditTemperareture.MinValue = item.minValue;
                            SpinEditTemperareture.Increment = 0.1M;
                            break;
                        case PULSE:
                        case HEARTRATE:
                            SpinEditPulse.MaxValue = item.maxValue;
                            SpinEditPulse.MinValue = item.minValue;
                            SpinEditPulse.Increment = 1M;
                            break;
                        case BREATHE:
                            SpinEditBreathe.MaxValue = item.maxValue;
                            SpinEditBreathe.MinValue = item.minValue;
                            SpinEditBreathe.Increment = 1M;
                            break;
                        default:
                            break;
                    }
                    if (!m_dicCheckItems.ContainsKey(item.fieldName))
                    {
                        m_dicCheckItems.Add(item.fieldName, item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读取字段默认值
        /// </summary>
        private void ReadFieldDefaultValue()
        {
            try
            {
                m_dicCheckItems.Clear();
                if (xmlDoc.GetElementsByTagName("FieldDeFaultValue").Count <= 0)
                {
                    MessageBox.Show("节点 FieldDeFaultValue 不存在");
                    return;
                }
                XmlNode nodeElement = xmlDoc.GetElementsByTagName("FieldDeFaultValue")[0];
                XmlNodeList nodeList = nodeElement.ChildNodes;
                m_FieldDefaultValue.Clear();
                foreach (XmlNode node in nodeList)
                {
                    string fieldName = node.Attributes["datafield"] == null ? "" : node.Attributes["datafield"].Value;
                    string defaultValue = node.Attributes["nullText"] == null ? "" : node.Attributes["nullText"].Value;
                    m_FieldDefaultValue.Add(fieldName, defaultValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 动态创建DataGridViewColumn
        /// </summary>
        private void CreateColumns()
        {
            m_ListStaticDataFields.Clear();
            m_ListDanymicDataFields.Clear();
            //添加固定的列
            m_ListStaticDataFields.Add("NOOFINPAT");//病案首页号
            m_ListStaticDataFields.Add("TEMPERATURE");//体温
            m_ListStaticDataFields.Add("WAYOFSURVEY");//测量方式
            m_ListStaticDataFields.Add("PULSE");//
            m_ListStaticDataFields.Add("HEARTRATE");//
            m_ListStaticDataFields.Add("BREATHE");//
            m_ListStaticDataFields.Add("PHYSICALCOOLING");//
            m_ListStaticDataFields.Add("PHYSICALHOTTING");//
            m_ListStaticDataFields.Add("DAYSAFTERSURGERY");//术后天数
            m_ListStaticDataFields.Add("DAYOFHOSPITAL");//入院天数
            //添加动态的列
            try
            {
                XmlNode nodeElement = xmlDoc.GetElementsByTagName("dataColumns")[0];
                XmlNodeList nodeList = nodeElement.ChildNodes;
                BandedGridColumn col = null;
                foreach (XmlNode node in nodeList)
                {
                    col = new BandedGridColumn();
                    col.Caption = node.InnerText;
                    col.Name = node.Attributes["name"] == null ? "" : node.Attributes["name"].Value;
                    col.Visible = node.Attributes["isShow"] == null || node.Attributes["isShow"].Value == "1" ? true : false;
                    col.FieldName = node.Attributes["datafield"] == null ? "" : node.Attributes["datafield"].Value; //字段绑定
                    VitalSignInfoEntity obj = new VitalSignInfoEntity();
                    obj.caption = col.Caption;
                    obj.cellCtlType = (CellControlType)Enum.Parse(typeof(CellControlType), node.Attributes["cellControl"] == null ? "1" : node.Attributes["cellControl"].Value);
                    obj.showForm = node.Attributes["showDlg"] == null ? "" : node.Attributes["showDlg"].Value;
                    obj.datasource = node.Attributes["listItems"] == null ? null : node.Attributes["listItems"].Value;
                    obj.showType = node.Attributes["showtype"] == null ? null : node.Attributes["showtype"].Value;
                    obj.datafield = col.FieldName;
                    col.Tag = obj;
                    //列外观设置
                    col.OptionsFilter.AllowAutoFilter = false;
                    col.OptionsFilter.AllowFilter = false;
                    col.OptionsFilter.ImmediateUpdateAutoFilter = false;
                    col.OptionsColumn.AllowEdit = true;
                    col.OptionsFilter.AllowAutoFilter = false;
                    col.OptionsFilter.AllowFilter = false;
                    col.OptionsColumn.AllowMove = false;
                    col.OptionsColumn.AllowSort = DefaultBoolean.False;
                    col.OptionsColumn.AllowShowHide = false;


                    if ((col.Tag as VitalSignInfoEntity).cellCtlType == CellControlType.ShowDlg) //需要弹出框编辑
                    {
                        RepositoryItemButtonEdit m_ButtonEdit = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();//供【大便次数】编辑的特殊列样式
                        m_ButtonEdit.Click += new EventHandler(m_ButtonEdit_Click);
                        this.gridControlVitalSigns.RepositoryItems.Add(m_ButtonEdit);

                        col.ColumnEdit = m_ButtonEdit;
                        m_ButtonEdit.ReadOnly = true;
                        m_ButtonEdit.AutoHeight = false;
                        m_ButtonEdit.TextEditStyle = TextEditStyles.Standard;

                    }
                    else if ((col.Tag as VitalSignInfoEntity).cellCtlType == CellControlType.ComboBox) //需要下拉框编辑
                    {
                        RepositoryItemComboBox m_ComboBoxEdit = new RepositoryItemComboBox();
                        this.gridControlVitalSigns.RepositoryItems.Add(m_ComboBoxEdit);
                        col.ColumnEdit = m_ComboBoxEdit;
                        string[] items = (col.Tag as VitalSignInfoEntity).datasource == null ? null : (col.Tag as VitalSignInfoEntity).datasource.ToString().Split(';');
                        if (items != null && items.Length > 0)
                        {
                            foreach (string str in items)
                            {
                                m_ComboBoxEdit.Items.Add(str);
                            }
                        }
                    }
                    if (obj.showType.Equals("0"))//时段数据
                    {
                        this.gridBand2.Columns.Add(col);
                        col.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    }
                    else //天(日)数据
                    {
                        this.gridBand3.Columns.Add(col);
                    }
                    this.bandedGridView1.Columns.Add(col);
                    if (col.FieldName == TIMESOFSHIT)
                    {
                        m_colIndexTimesOfShit = col.AbsoluteIndex;
                    }
                    col.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    m_ListDanymicDataFields.Add(col.FieldName, obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绑定控件的数据源
        /// </summary>
        private void BindComboBoxDataSource(RepositoryItemGridLookUpEdit comboBox, string value)
        {
            try
            {
                repositoryItemGridLookUpEdit1.DataSource = GetDictionaryData(value); ;
                repositoryItemGridLookUpEdit1.DisplayMember = "NAME";
                repositoryItemGridLookUpEdit1.ValueMember = "DETAILID";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDictionaryData(string categoryValue)
        {
            try
            {
                string sqlStr = " select DETAILID,NAME from dictionary_detail where CATEGORYID = '" + categoryValue + "' ";
                return CommonObjects.SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化时间段
        /// </summary>
        private void InitHourDuraing()
        {
            try
            {
                XmlNode nodeElement = xmlDoc.GetElementsByTagName("HourOfday")[0];
                XmlNodeList nodeList = nodeElement.ChildNodes;
                int index = 1;
                foreach (XmlNode node in nodeList)
                {
                    radios[index - 1].Text = node.Attributes["timeslotvalue"].Value + " 时";
                    radios[index - 1].Tag = node.Attributes["timeslotvalue"].Value.ToString();
                    index++;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 初始化当前护理数据
        /// </summary>
        //private void InitTodayRecords()
        //{
        //    try
        //    {
        //        DataTable temp = null;
        //        foreach (CheckEdit radio in radios)
        //        {
        //            string hour = radio.Tag.ToString();
        //            m_CurTimeslot = hour;

        //            //temp = SearchInpatentDataZheHe(m_CurTimeslot, dateEdit.DateTime.ToString("yyyy-MM-dd"));
        //           // temp = SearchInpatientData(m_CurTimeslot, dateEdit.DateTime.ToString("yyyy-MM-dd"));
        //            //gridControlVitalSigns.DataSource = null;
        //            //gridControlVitalSigns.DataSource = temp;// edit zyx 2013-01-18
        //           //SaveData();
        //            //if (!m_IsSaveOk)
        //            //{
        //            //    MessageBox.Show("初始化数据失败,请稍后再试...");
        //            //    break;
        //            //}
        //        }
        //        //BindGridRecordData(radio1.Tag.ToString(), dateEdit.Text.Replace('/', '-'));
        //        BindGridRecordData(radio1.Tag.ToString(), dateEdit.DateTime.ToString("yyyy-MM-dd"));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        void radio_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckEdit ck = sender as CheckEdit;
                labelTimelot.Text = "【" + ck.Tag.ToString() + " 时段】";
                m_CurTimeslot = ck.Tag.ToString();

                if (!ck.Checked)
                {
                    if (m_dataChanged)
                    {
                        if (MessageBox.Show("数据已修改，是否保存？", "提醒", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            SaveData();
                        }
                    }
                    m_dataChanged = false;
                }
                else
                {
                    // 若当前选择的不是第一时段，将日数据列区域设为不可编辑
                    if (m_CurTimeslot == radio1.Tag.ToString()) //第一个时段
                    {
                        VitalSignInfoEntity tag = null;
                        foreach (BandedGridColumn col in bandedGridView1.Columns)
                        {
                            tag = col.Tag as VitalSignInfoEntity;
                            if (tag != null)
                            {
                                col.OptionsColumn.AllowEdit = true;
                            }
                        }
                    }
                    else
                    {
                        VitalSignInfoEntity tag = null;
                        foreach (BandedGridColumn col in bandedGridView1.Columns)
                        {
                            tag = col.Tag as VitalSignInfoEntity;
                            if (tag != null && tag.showType == "1") //天数据
                            {
                                col.OptionsColumn.AllowEdit = false;
                            }
                        }
                    }
                    // BindGridRecordData(GetCurrentSelectTimeslot(), dateEdit.Text.Replace('/', '-'));
                    BindGridRecordData(GetCurrentSelectTimeslot(), m_CurrDatetime);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 检索当前科室(用户科室)的所有病人
        /// </summary>
        public void BindGridRecordData(string hour, string date)
        {
            WaitDialogForm m_WaitDialog = new WaitDialogForm("正在初始化数据...", "请稍后");
            try
            {

                SearchInpatentDataZheHe(hour, date);

                //string departcode = CommonObjects.CurrentUser.CurrentDeptId;
                //string wardcode = CommonObjects.CurrentUser.CurrentWardId;
                //SqlParameter[] sqlParams = new SqlParameter[]
                //{
                //    new SqlParameter("@departCode",SqlDbType.VarChar,50),
                //    new SqlParameter("@wardcode",SqlDbType.VarChar,50),
                //    new SqlParameter("@Timelot",SqlDbType.VarChar,50),
                //    new SqlParameter("@TimelotSave",SqlDbType.VarChar,50),
                //    new SqlParameter("@date",SqlDbType.NVarChar,50),
                //    new SqlParameter("@result",SqlDbType.Structured,int.MaxValue)
                //};
                //sqlParams[0].Value = departcode;
                //sqlParams[1].Value = wardcode;
                //sqlParams[2].Value = hour;
                //sqlParams[3].Value = timelotSave;//天数据存为2时段
                //string surveyDate = dateEdit.DateTime.ToString("yyyy-MM-dd");//dateEdit.Text.Replace('/','-');
                //sqlParams[4].Value = surveyDate;
                //sqlParams[5].Direction = ParameterDirection.Output;
                //DataTable dtInpatient = YD_SqlHelper.ExecuteDataTable("EMR_NURSE_STATION.usp_GetPatientsOfDept", sqlParams, CommandType.StoredProcedure);
                //gridControlVitalSigns.DataSource = null;
                //dtInpatient.DefaultView.Sort = "BED asc ";
                //gridControlVitalSigns.DataSource = dtInpatient;
                DataTable dtInpatient = SearchInpatentDataZheHe(hour, date);
                dtInpatient.DefaultView.Sort = "BED asc ";
                gridControlVitalSigns.DataSource = dtInpatient;
                m_WaitDialog.Hide();
                m_WaitDialog.Close();
            }
            catch (Exception ex)
            {
                m_WaitDialog.Hide();
                m_WaitDialog.Close();
                MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// xll 2013-06-25 
        /// 查询数据
        /// 将病人信息 时段数据和日数据整合到一个DataTable中来
        /// </summary>
        /// <returns></returns>
        public DataTable SearchInpatentDataZheHe(string hour, string date)
        {
            try
            {
                string departcode = CommonObjects.CurrentUser.CurrentDeptId;
                string wardcode = CommonObjects.CurrentUser.CurrentWardId;
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@departCode",SqlDbType.VarChar,50),
                    new SqlParameter("@wardcode",SqlDbType.VarChar,50),
                    new SqlParameter("@Timelot",SqlDbType.VarChar,50),
                    new SqlParameter("@TimelotSave",SqlDbType.VarChar,50),
                    new SqlParameter("@date",SqlDbType.NVarChar,50),
                    new SqlParameter("@result",SqlDbType.Structured,int.MaxValue),
                     new SqlParameter("@result1",SqlDbType.Structured,int.MaxValue),
                      new SqlParameter("@result2",SqlDbType.Structured,int.MaxValue)
                };
                sqlParams[0].Value = departcode;
                sqlParams[1].Value = wardcode;
                sqlParams[2].Value = hour;
                sqlParams[3].Value = timelotSave;//天数据存为2时段
                string surveyDate = m_CurrDatetime;
                sqlParams[4].Value = surveyDate;
                sqlParams[5].Direction = ParameterDirection.Output;
                sqlParams[6].Direction = ParameterDirection.Output;
                sqlParams[7].Direction = ParameterDirection.Output;
                DataSet dataSet = DS_SqlHelper.ExecuteDataSet("EMR_NURSE_STATION.usp_GetPatientsOfDept2", sqlParams, CommandType.StoredProcedure);
                DataTable dtInpatient = new DataTable();
                dtInpatient = ConvertToDataTable(dataSet);
                dtInpatient.DefaultView.Sort = "BED asc ";
                return dtInpatient;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private DataTable ConvertToDataTable(DataSet dataSet)
        {
            try
            {
                DataTable dtAll = new DataTable();
                DataTable dtInpat = dataSet.Tables[0];
                DataTable dtNoteShiDuan = dataSet.Tables[1];
                DataTable dtNoteTian = dataSet.Tables[2];

                //var linqbitem = from wip in dtInpat.AsEnumerable()
                //                join bitem in dtNoteShiDuan.AsEnumerable() on wip.Field<string>("noofinpat").Trim() equals bitem.Field<string>("noofinpat").Trim()
                //                join bitemt in dtNoteTian.AsEnumerable() on wip.Field<string>("noofinpat").Trim() equals bitemt.Field<string>("noofinpat").Trim()
                //                into s
                //                from t in s.DefaultIfEmpty()
                //                select s;


                foreach (DataColumn item in dtInpat.Columns)
                {
                    dtAll.Columns.Add(item.ColumnName, item.DataType);
                }

                foreach (DataColumn item in dtNoteShiDuan.Columns)
                {
                    if (item.ColumnName.ToLower() != "noofinpat")
                    {
                        dtAll.Columns.Add(item.ColumnName, item.DataType);
                    }
                }
                foreach (DataColumn item in dtNoteTian.Columns)
                {
                    if (item.ColumnName.ToLower() != "noofinpat")
                    {
                        dtAll.Columns.Add(item.ColumnName, item.DataType);
                    }
                }

                foreach (DataRow itemrow in dtInpat.Rows)
                {
                    DataRow dr = dtAll.NewRow();
                    //循环病人table 将病人table中的值赋给新table
                    foreach (DataColumn itemcol in dtInpat.Columns)
                    {
                        dr[itemcol.ColumnName] = itemrow[itemcol.ColumnName];
                    }


                    string fliter = string.Format(@"noofinpat='{0}'", dr["noofinpat"].ToString());
                    DataRow[] DRshiduans = dtNoteShiDuan.Select(fliter);
                    if (DRshiduans != null && DRshiduans.Length > 0)  //时段数据存在值时
                    {
                        //循环病人时段数据table 将病人时段数据table中的值赋给新table
                        foreach (DataColumn itemcol in dtNoteShiDuan.Columns)
                        {
                            if (itemcol.ColumnName != "noofinpat")
                            {
                                dr[itemcol.ColumnName] = DRshiduans[0][itemcol.ColumnName];
                            }
                        }
                    }
                    else
                    {
                        //如果该病人没有值时
                        dr["WAYOFSURVEY"] = "8801";
                        dr["NAME"] = "腋温";
                    }


                    string fliterTian = string.Format(@"noofinpat='{0}'", dr["noofinpat"].ToString());
                    DataRow[] DRTian = dtNoteTian.Select(fliter);
                    if (DRTian != null && DRTian.Length > 0)  //天数据存在值时
                    {
                        //循环病人时段数据table 将病人时段数据table中的值赋给新table
                        foreach (DataColumn itemcol in dtNoteTian.Columns)
                        {
                            if (itemcol.ColumnName != "noofinpat")
                            {
                                dr[itemcol.ColumnName] = DRTian[0][itemcol.ColumnName];
                            }
                        }
                    }
                    else
                    {
                        //如果该病人没有值时

                    }



                    dtAll.Rows.Add(dr);
                }

                return dtAll;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable SearchInpatientData(string hour, string date)
        {
            try
            {
                string departcode = CommonObjects.CurrentUser.CurrentDeptId;
                string wardcode = CommonObjects.CurrentUser.CurrentWardId;
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@departCode",SqlDbType.VarChar,50),
                    new SqlParameter("@wardcode",SqlDbType.VarChar,50),
                    new SqlParameter("@Timelot",SqlDbType.VarChar,50),
                    new SqlParameter("@TimelotSave",SqlDbType.VarChar,50),
                    new SqlParameter("@date",SqlDbType.NVarChar,50),
                    new SqlParameter("@result",SqlDbType.Structured,int.MaxValue)
                };
                sqlParams[0].Value = departcode;
                sqlParams[1].Value = wardcode;
                sqlParams[2].Value = hour;
                sqlParams[3].Value = timelotSave;//天数据存为2时段
                string surveyDate = m_CurrDatetime;//dateEdit.Text.Replace('/', '-');
                sqlParams[4].Value = surveyDate;
                sqlParams[5].Direction = ParameterDirection.Output;
                DataTable dtInpatient = DS_SqlHelper.ExecuteDataTable("EMR_NURSE_STATION.usp_GetPatientsOfDept", sqlParams, CommandType.StoredProcedure);
                dtInpatient.DefaultView.Sort = "BED asc ";
                return dtInpatient;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
            return null;
        }

        private void DevButtonQurey1_Click(object sender, EventArgs e)
        {
            try
            {

                WaitDialogForm m_WaitDialog = new WaitDialogForm("正在初始化数据...", "请稍后");
                InitHourDuraing();
                //InitTodayRecords();
                this.bandedGridView1.BestFitColumns();
                BindComboBoxDataSource(repositoryItemGridLookUpEdit1, "88");
                //BindGridRecordData(radio1.Tag.ToString(), dateEdit.DateTime.ToString("yyyy-MM-dd"));
                m_CurTimeslot = GetCurrentSelectTimeslot();
                labelTimelot.Text = "【" + m_CurTimeslot.ToString() + " 时段】";

                if (m_dataChanged)
                {
                    if (MessageBox.Show("数据已修改，是否保存？", "提醒", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SaveData();
                    }
                }
                m_dataChanged = false;
                //BindGridRecordData(GetCurrentSelectTimeslot(), dateEdit.Text.Replace('/', '-'));dateEdit.DateTime.ToString("yyyy-MM-dd")

                BindGridRecordData(GetCurrentSelectTimeslot(), m_CurrDatetime);
                m_WaitDialog.Close();
                m_WaitDialog.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private string GetCurrentSelectTimeslot()
        {
            foreach (CheckEdit radio in radios)
            {
                if (radio.Checked)
                    return radio.Tag.ToString();
            }
            return null;
        }

        private void bandedGridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            m_dataChanged = true;
            DataRow dr = bandedGridView1.GetDataRow(e.RowHandle);
            //if (!string.IsNullOrEmpty(dr["ID"].ToString())) //该行不是新增行
            //{
            if (!m_ChangedRowIndex.Contains(e.RowHandle))
            {
                m_ChangedRowIndex.Add(e.RowHandle);
            }
            // }
        }

        private void DevButtonSave1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveData();
                m_dataChanged = false;
                m_ChangedRowIndex.Clear();
                bandedGridView1.RefreshData();
                //DevButtonQurey1_Click(null,null);//重新查询，代替界面数据刷新功能
                BindGridRecordData(GetCurrentSelectTimeslot(), m_CurrDatetime);
                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 创建插入SQl语句
        /// </summary>
        private void CreateInsertSql(DataRow dr)
        {
            StringBuilder fieldsValues = new StringBuilder();
            string FieldsName = "";
            int temp = 0;
            //获得字段名称
            foreach (string val in m_ListStaticDataFields)
            {
                FieldsName += val + ",";
            }
            foreach (KeyValuePair<string, VitalSignInfoEntity> val in m_ListDanymicDataFields)
            {
                temp++;
                FieldsName += val.Key + ",";
                if (temp == m_ListDanymicDataFields.Count)
                {
                    FieldsName = FieldsName.Substring(0, FieldsName.Length - 1);
                }
            }
            temp = 0;
            //获得表字段值
            foreach (string val in m_ListStaticDataFields)
            {
                if (val == WAYOFSURVEY && dr[val].ToString() == "")
                {
                    fieldsValues.Append("'" + "8801" + "',");
                }
                else
                {
                    fieldsValues.Append("'" + dr[val].ToString() + "',");
                }
            }
            if (m_CurTimeslot == radio1.Tag.ToString()) //第一个时段
            {
                foreach (KeyValuePair<string, VitalSignInfoEntity> val1 in m_ListDanymicDataFields)
                {
                    temp++;
                    //if (dr[val1.Key].ToString() == "")
                    //{
                    if (m_FieldDefaultValue.ContainsKey(val1.Key)) //此列需要有默认值
                    {
                        fieldsValues.Append("'" + m_FieldDefaultValue[val1.Key] + "',");
                    }
                    else
                    {
                        fieldsValues.Append("'" + dr[val1.Key].ToString() + "',");
                    }
                    //}

                    if (temp == m_ListDanymicDataFields.Count)
                    {
                        fieldsValues = fieldsValues.Remove(fieldsValues.Length - 1, 1);
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, VitalSignInfoEntity> val in m_ListDanymicDataFields)
                {
                    temp++;
                    fieldsValues.Append("'',");
                    if (temp == m_ListDanymicDataFields.Count)
                    {
                        fieldsValues = fieldsValues.Remove(fieldsValues.Length - 1, 1);
                    }
                }
            }
            fieldsValues.Append(")");
            string dateOfSurvey = m_CurrDatetime; //dateEdit.Text.Replace('/', '-');
            //string dateOfSurvey = dateEdit.DateTime.ToString("yyyy-MM-dd");
            m_sbSql.Append(m_insertSql);
            string dateTime = string.Format(DateTime.Now.ToString(), "yyyy-mm-dd");
            m_sbSql.Append(FieldsName + ")values(SEQ_NOTESONNURSING_ID.NEXTVAL,'" + dateOfSurvey + "','" + m_CurTimeslot + "'," + "to_char(sysdate,'yyyy-mm-dd HH24:MI:SS')" + ",'" + CommonObjects.CurrentUser.Id + "',");
            m_sbSql.Append(fieldsValues);
        }

        /// <summary>
        /// 创建更新SQl语句
        /// </summary>
        private void CreateUpdateSql(DataRow dr, bool isFirstTimeslot)
        {
            string FieldsName = "";
            int temp = 0;
            foreach (string val in m_ListStaticDataFields)
            {
                if (val != NOOFINPAT && val != DAYSAFTERSURGERY && val != DAYOFHOSPITAL) //这些列动态算，无需编辑
                {
                    if (val == WAYOFSURVEY && dr[val].ToString() == "")
                    {
                        FieldsName += val + "='" + "8801" + "',";
                    }
                    else
                    {
                        FieldsName += val + "='" + dr[val].ToString().Replace("[", "[[ ").Replace("]", " ]]").Replace("*", "[*]").Replace("%", "[%]").Replace("[[ ", "[[]").Replace(" ]]", "[]]").Replace("\'", "''") + "',";
                    }
                }

            }
            foreach (KeyValuePair<string, VitalSignInfoEntity> val in m_ListDanymicDataFields)
            {
                temp++;
                if (isFirstTimeslot) //是第一时段
                {
                    FieldsName += val.Key + "='" + dr[val.Key].ToString().Replace("[", "[[ ").Replace("]", " ]]").Replace("*", "[*]").Replace("%", "[%]").Replace("[[ ", "[[]").Replace(" ]]", "[]]").Replace("\'", "''") + "',";
                }
                else
                {
                    if (val.Value.showType.Equals("0"))//是时段数据
                    {
                        FieldsName += val.Key + "='" + dr[val.Key].ToString().Replace("[", "[[ ").Replace("]", " ]]").Replace("*", "[*]").Replace("%", "[%]").Replace("[[ ", "[[]").Replace(" ]]", "[]]").Replace("\'", "''") + "',";
                    }
                    else //天数据
                    {
                        FieldsName += val.Key + "='" + "" + "',";
                    }
                }
                if (temp == m_ListDanymicDataFields.Count)
                {
                    FieldsName = FieldsName.Substring(0, FieldsName.Length - 1);
                }
            }
            m_sbSql.Append(m_updateSql);
            m_sbSql.Append(FieldsName);
        }

        /// <summary>
        /// 保存数据 之前有的数据修改更新，没有的添加新行
        /// </summary>
        private void SaveData()
        {
            try
            {
                for (int i = 0; i < bandedGridView1.RowCount; i++)
                {
                    StringBuilder fieldsValues = new StringBuilder();
                    try
                    {
                        DataRow dr = bandedGridView1.GetDataRow(i);
                        if (dr == null) continue;


                        // if (string.IsNullOrEmpty(dr["ID"].ToString())) //该行是新增行
                        bool IsInsert = IsInSertDate(dr);

                        if (IsInsert && m_ChangedRowIndex.Contains(i))
                        {
                            CreateInsertSql(dr);
                            DS_SqlHelper.ExecuteNonQuery(m_sbSql.ToString());
                        }
                        else //更新数据
                        {
                            if (m_ChangedRowIndex.Contains(i)) //该行数据修改
                            {
                                if (m_CurTimeslot == radio1.Tag.ToString()) //第一个时段
                                {
                                    CreateUpdateSql(dr, true);
                                }
                                else
                                {
                                    CreateUpdateSql(dr, false);
                                }
                                string surveyDate = m_CurrDatetime;// dateEdit.Text.Replace('/', '-');
                                m_sbSql.Append(" where " + "NOOFINPAT='" + dr[NOOFINPAT] + "' and dateofsurvey='" + surveyDate + "' and Timeslot='" + m_CurTimeslot + "'");
                                DS_SqlHelper.ExecuteNonQuery(m_sbSql.ToString());
                            }
                        }
                        m_sbSql.Remove(0, m_sbSql.Length);
                    }
                    catch (Exception ex)
                    {
                        m_IsSaveOk = false;
                        throw ex;
                    }
                }
                m_IsSaveOk = true;
            }
            catch (Exception ex)
            {
                m_IsSaveOk = false;
                throw ex;
            }
        }


        private bool IsInSertDate(DataRow dr)
        {
            try
            {
                string noofinpat = dr["noofinpat"].ToString();
                string dateofsurvey = m_CurrDatetime;
                string timeslot = m_CurTimeslot;
                string sql = string.Format(@"select count(*) from notesonnursing n where n.noofinpat='{0}' and n.dateofsurvey='{1}' and n.timeslot='{2}'", noofinpat, dateofsurvey, timeslot);
                Object obValue = DS_SqlHelper.ExecuteScalar(sql, CommandType.Text);
                int intValue = Convert.ToInt32(obValue);
                return intValue <= 0 ? true : false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void bandedGridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            DataRowView drv = bandedGridView1.GetRow(e.RowHandle) as DataRowView;
            if (e.CellValue == null || string.IsNullOrEmpty(txtPName.Text.Trim())) return;
            if (drv["PNAME"].ToString() == txtPName.Text.Trim())
            {
                e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void bandedGridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #region 单元格数据效验
        private void bandedGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            decimal parseResult = -1;
            foreach (KeyValuePair<string, CheckItem> kv in m_dicCheckItems)
            {
                if (kv.Key.Equals(this.bandedGridView1.FocusedColumn.FieldName))
                {
                    string val = e.Value.ToString().Trim();//当前单元格中的值
                    if (string.IsNullOrEmpty(val))
                    {
                        return;
                    }
                    if (decimal.TryParse(val, out parseResult)) //如果转换成功
                    {
                        if (parseResult < kv.Value.minValue || parseResult > kv.Value.maxValue)
                        {
                            e.Valid = false;
                            e.ErrorText = kv.Value.caption + "的值超过区间【" + kv.Value.minValue.ToString() + " - " + kv.Value.maxValue + "】";
                        }
                    }
                    else
                    {
                        e.Valid = false;
                        e.ErrorText = "请确认输入的值为数字类型";
                    }
                    break;
                }
            }
        }

        private void bandedGridView1_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            if (MessageBox.Show(e.ErrorText, "提醒", MessageBoxButtons.OK) == DialogResult.OK)
            {
                e.ExceptionMode = ExceptionMode.Ignore;
            }
        }
        #endregion

        private void m_ButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_CurTimeslot != radio1.Tag.ToString()) return;
                string curCellValue = this.bandedGridView1.GetFocusedDisplayText();
                string frmName = (this.bandedGridView1.Columns[this.bandedGridView1.FocusedColumn.AbsoluteIndex].Tag as VitalSignInfoEntity).showForm;
                if (!String.IsNullOrEmpty(frmName))
                {
                    Form obj = (Form)Activator.CreateInstance(Type.GetType(frmName), curCellValue);
                    obj.StartPosition = FormStartPosition.CenterParent;
                    //obj.Location = MousePosition;
                    if (obj.ShowDialog() == DialogResult.OK)
                    {
                        this.bandedGridView1.SetRowCellValue(this.bandedGridView1.FocusedRowHandle, this.bandedGridView1.FocusedColumn, ((IDlg)obj).EditValue);
                    }
                    obj.Dispose();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void dateEdit_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                if (!IsLoad)
                {
                    return;
                }
                WaitDialogForm m_WaitDialog = new WaitDialogForm("正在初始化数据...", "请稍后");
                InitHourDuraing();
                //InitTodayRecords();
                this.bandedGridView1.BestFitColumns();
                BindComboBoxDataSource(repositoryItemGridLookUpEdit1, "88");
                // BindGridRecordData(radio1.Tag.ToString(), dateEdit.DateTime.ToString("yyyy-MM-dd"));
                m_CurTimeslot = GetCurrentSelectTimeslot();
                labelTimelot.Text = "【" + m_CurTimeslot.ToString() + " 时段】";

                if (m_dataChanged)
                {
                    if (MessageBox.Show("数据已修改，是否保存？", "提醒", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SaveData();
                    }
                }
                m_dataChanged = false;
                m_CurrDatetime = dateEdit.DateTime.ToString("yyyy-MM-dd");
                //BindGridRecordData(GetCurrentSelectTimeslot(), dateEdit.Text.Replace('/', '-'));dateEdit.DateTime.ToString("yyyy-MM-dd")

                BindGridRecordData(GetCurrentSelectTimeslot(), m_CurrDatetime);
                m_WaitDialog.Close();
                m_WaitDialog.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }


    //标识体征数据信息
    public class VitalSignInfoEntity
    {
        public string caption;//名称
        public string datafield; //对应表字段类型
        public CellControlType cellCtlType;//单元格控件类型
        public string showForm; //如设置它对应的弹出窗体，在此定义
        public object datasource; //如需绑定下拉列表数据，在此定义;如设置它对应的弹出窗体，在此定义
        public string showType; //"0":时段数据;"1":天数据

        public VitalSignInfoEntity(string _caption, string _dataifield, CellControlType _cellCtlType)
        {
            caption = _caption;
            datafield = _dataifield;
            cellCtlType = _cellCtlType;
        }
        public VitalSignInfoEntity(string _caption, string _dataifield)
        {
            caption = _caption;
            datafield = _dataifield;
        }

        public VitalSignInfoEntity()
        {
        }
    }

    //DataGrid 单元格编辑类型
    public enum CellControlType
    {
        ComboBox = 0, //下拉列表
        Normal = 1, //普通文本框
        ShowDlg = 2, //弹框编辑 
        Other = 3 //其他 
    }
}