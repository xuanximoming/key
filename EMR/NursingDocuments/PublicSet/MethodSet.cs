using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Core.NursingDocuments.UserControls;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
#pragma warning disable 0618

namespace DrectSoft.Core.NursingDocuments.PublicSet
{

    /// <summary>
    /// xll 2013-1-14 修改patientInfo的全局变量的引用
    /// </summary>
    abstract class MethodSet
    {
        static IDataAccess sql_Helper;
        static IEmrHost m_app;
        static Common.Eop.Inpatient m_CurrentInPatient;


        #region 属性集
        /// <summary>
        /// 入院日期
        /// </summary>
        public static string AdmitDate { get; set; }

        /// <summary>
        /// 出院日期
        /// </summary>
        public static string OutHosDate { get; set; }

        /// <summary>
        /// 出区日期
        /// </summary>
        public static string OutWardDate { get; set; }

        /// <summary>
        /// 住院号
        /// </summary>
        public static string PatID { get; set; }


        /// <summary>
        /// 新增的床号 ywk 
        /// </summary>
        public static string BedID { get; set; }
        /// <summary>
        /// 新增的病人 名称 ywk 2012年5月29日11:08:07
        /// </summary>
        public static string PatName { get; set; }
        /// <summary>
        /// 病人的年龄 wyt 20120807
        /// </summary>
        public static string Age { get; set; }

        /// <summary>
        /// 新增的病人的首页序号 add by ywk 二〇一三年五月二十八日 15:21:07  
        /// </summary>
        public static string NoOfinPat { get; set; }
        /// <summary>
        /// 新增的病人的病案号码 add by ywk 二〇一三年五月二十八日 15:31:03 
        /// </summary>
        public static string RecordNoofinpat { get; set; }

        /// <summary>
        /// 术后日数（天）
        /// </summary>
        public static string[] DaysAfterSurgery { get; set; }

        public static IEmrHost App
        {
            get { return m_app; }
            set
            {
                m_app = value;
                sql_Helper = m_app.SqlHelper;

            }
        }

        /// <summary>
        /// 当前病人
        /// </summary>
        public static Common.Eop.Inpatient CurrentInPatient
        {
            get
            {
                return m_CurrentInPatient;
            }
            set
            {
                m_CurrentInPatient = value;
            }
        }
        #endregion

        /// <summary>
        /// 获取服务器当前时间和日期
        /// </summary>
        public static DateTime GetCurrServerDateTime
        {
            get
            {
                //获取服务器时间和日期
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@GetType", SqlDbType.Int)
                };
                param[0].Value = 1;
                DataTable dt = m_app.SqlHelper.ExecuteDataTable("usp_GetCurrSystemParam", param, CommandType.StoredProcedure);

                return Convert.ToDateTime(dt.Rows[0]["CurrServerDateTime"].ToString());

            }
        }

        #region 获取字典分类明细库
        /// <summary>
        /// 获取字典分类明细库
        /// </summary>
        /// <param name="lookUp">下拉框控件名</param>
        /// <param name="?">下拉框内容类别</param>
        public static void GetDictionarydetail(LookUpEdit lookUp, string Type, string CategoryID)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@PatNoOfHis", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = Type;
            sqlParam[1].Value = "0";
            sqlParam[2].Value = CategoryID;

            DataTable frmDateTable = sql_Helper.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);

            lookUp.Properties.Columns.Clear();
            lookUp.Properties.DataSource = frmDateTable;
            lookUp.Properties.DisplayMember = "Name";
            lookUp.Properties.ValueMember = "DetailID";
            lookUp.Properties.ShowHeader = false;
            lookUp.Properties.ShowFooter = false;

            LookUpColumnInfoCollection coll = lookUp.Properties.Columns;
            //coll.Add(new LookUpColumnInfo("CategoryID"));
            //coll.Add(new LookUpColumnInfo("DetailID"));
            //coll.Add(new LookUpColumnInfo("Name"));
            //coll.Add(new LookUpColumnInfo("Py"));
            //coll.Add(new LookUpColumnInfo("Wb"));
            coll.Add(new LookUpColumnInfo("CATEGORYID"));
            coll.Add(new LookUpColumnInfo("DETAILID"));
            coll.Add(new LookUpColumnInfo("NAME"));
            coll.Add(new LookUpColumnInfo("PY"));
            coll.Add(new LookUpColumnInfo("WB"));

            //lookUp.Properties.Columns["Name"].Visible = true;
            //lookUp.Properties.Columns["CategoryID"].Visible = false;
            //lookUp.Properties.Columns["DetailID"].Visible = false;
            //lookUp.Properties.Columns["Py"].Visible = false;
            //lookUp.Properties.Columns["Wb"].Visible = false;
            lookUp.Properties.Columns["NAME"].Visible = true;
            lookUp.Properties.Columns["CATEGORYID"].Visible = false;
            lookUp.Properties.Columns["DETAILID"].Visible = false;
            lookUp.Properties.Columns["PY"].Visible = false;
            lookUp.Properties.Columns["WB"].Visible = false;

            lookUp.Properties.BestFit();

            lookUp.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            lookUp.Properties.AutoSearchColumnIndex = 2 | 3;
        }

        /// <summary>
        /// 绑定获取字典分类明细库
        /// </summary>
        /// <param name="lookUp">下拉框控件名</param>
        /// <param name="frmDateTable">数据源</param>
        /// <param name="intWidth">下拉框矩形宽度</param>
        public static void GetDictionarydetail(LookUpEdit lookUp, DataTable frmDateTable)
        {
            lookUp.Properties.Columns.Clear();
            lookUp.Properties.DataSource = frmDateTable;
            lookUp.Properties.DisplayMember = "NAME";   //"Name";
            lookUp.Properties.ValueMember = "DETAILID"; //"DetailID";
            lookUp.Properties.ShowHeader = false;
            lookUp.Properties.ShowFooter = false;

            LookUpColumnInfoCollection coll = lookUp.Properties.Columns;
            //coll.Add(new LookUpColumnInfo("CategoryID"));
            //coll.Add(new LookUpColumnInfo("DetailID"));
            //coll.Add(new LookUpColumnInfo("Name"));
            //coll.Add(new LookUpColumnInfo("Py"));
            //coll.Add(new LookUpColumnInfo("Wb"));
            coll.Add(new LookUpColumnInfo("CATEGORYID"));
            coll.Add(new LookUpColumnInfo("DETAILID"));
            coll.Add(new LookUpColumnInfo("NAME"));
            coll.Add(new LookUpColumnInfo("PY"));
            coll.Add(new LookUpColumnInfo("WB"));

            //lookUp.Properties.Columns["Name"].Visible = true;
            //lookUp.Properties.Columns["CategoryID"].Visible = false;
            //lookUp.Properties.Columns["DetailID"].Visible = false;
            //lookUp.Properties.Columns["Py"].Visible = false;
            //lookUp.Properties.Columns["Wb"].Visible = false;
            lookUp.Properties.Columns["NAME"].Visible = true;
            lookUp.Properties.Columns["CATEGORYID"].Visible = false;
            lookUp.Properties.Columns["DETAILID"].Visible = false;
            lookUp.Properties.Columns["PY"].Visible = false;
            lookUp.Properties.Columns["WB"].Visible = false;

            lookUp.Properties.BestFit();

            lookUp.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            lookUp.Properties.AutoSearchColumnIndex = 2 | 3;
        }
        #endregion

        #region 读取病人信息
        /// <summary>
        /// 读取病人信息
        /// </summary>
        /// <param name="PatNoOfHis">首页序号</param>
        public static DataTable GetRedactPatientInfoFrm(string Type, string CategoryID, string NoOfInpat)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = Type;
            sqlParam[1].Value = NoOfInpat;
            sqlParam[2].Value = CategoryID;

            return sql_Helper.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);


        }

        /// <summary>
        /// Add By wwj 2011-03-24 得到三测表中病人的基本信息
        /// </summary>
        /// <param name="noOfInpat">首页号</param>
        /// <returns></returns>
        public static DataTable GetPatientInfoForThreeMeasureTable(decimal noOfInpat)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@NoOfInpat", SqlDbType.Decimal),
            };
            sqlParam[0].Value = noOfInpat;
            return sql_Helper.ExecuteDataTable("usp_GetPatientInfoForThreeMeas", sqlParam, CommandType.StoredProcedure);
        }

        /// <summary>
        ///  Add By wwj 2011-03-24 由于病人可能出现转床转科等情况，所以要通过制定的时间找到病人的病床信息
        /// </summary>
        /// <param name="noOfInpat">首页号</param>
        /// <param name="allocateDate">制定的时间</param>
        /// <returns></returns>
        public static DataTable GetPatientBedInfoByDate(string noOfInpat, DateTime allocateDate)
        {
            if (noOfInpat == "")
            {
                return new DataTable();
            }

            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@NoOfInpat", SqlDbType.Decimal),
                new SqlParameter("@AllocateDate", SqlDbType.NVarChar),
            };
            sqlParam[0].Value = noOfInpat;
            sqlParam[1].Value = allocateDate.ToString("yyyy-MM-dd");
            return sql_Helper.ExecuteDataTable("usp_GetPatientBedInfoByDate", sqlParam, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Add By wwj 2011-03-24 获取病人在指定时间段中的生命体征，用于三测表
        /// </summary>
        /// <param name="noOfInpat"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static DataTable GetNursingRecordByDate(string noOfInpat, string beginDate, string endDate)
        {
            if (noOfInpat == "")
            {
                return new DataTable();
            }

            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@NoOfInpat", SqlDbType.Decimal),
                new SqlParameter("@BeginDate", SqlDbType.NVarChar),
                new SqlParameter("@EndDate", SqlDbType.NVarChar),
            };

            sqlParam[0].Value = noOfInpat;
            sqlParam[1].Value = (Convert.ToDateTime(beginDate)).ToString("yyyy-MM-dd");
            sqlParam[2].Value = (Convert.ToDateTime(endDate)).ToString("yyyy-MM-dd");
            return sql_Helper.ExecuteDataTable("usp_GetNursingRecordByDate", sqlParam, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Add By wwj 2011-03-25 得到配置的时间点的信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTimePoint()
        {
            string[] timePoints;
            DataTable dataTableTimePoint = new DataTable();
            dataTableTimePoint.Columns.Add("TimePoint");
            dataTableTimePoint.Columns.Add("Color");

            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar)
            };
            sqlParam[0].Value = "1";

            DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable("usp_GetNursingRecordParam", sqlParam, CommandType.StoredProcedure);

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Configkey"].ToString().Equals("VITALSIGNSRECORDTIME"))
                {
                    timePoints = dr["Value"].ToString().Split(new char[] { ',', '，' });

                    foreach (string timePoint in timePoints)
                    {
                        DataRow dataRowTimePoint = dataTableTimePoint.NewRow();
                        dataRowTimePoint[0] = timePoint;
                        dataRowTimePoint[1] = "Black";
                        dataTableTimePoint.Rows.Add(dataRowTimePoint);
                    }
                }
            }

            return dataTableTimePoint;
        }

        /// <summary>
        /// 得到医院信息
        /// </summary>
        /// <returns></returns>
        private static DataTable GetHospitalInfo()
        {
            return sql_Helper.ExecuteDataTable("usp_GetHospitalInfo", new SqlParameter[] { }, CommandType.StoredProcedure);
        }

        public static string GetHospitalName()
        {
            DataTable dt = GetHospitalInfo();
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["name"].ToString();
                //return DrectSoft.MainFrame.PublicClass.getHosName();
            }
            return "";
        }
        #endregion

        /// <summary>
        /// 从字典表中获得各个状态的信息 
        /// add by ywk  2012年4月23日10:05:30
        /// </summary>
        /// <returns></returns>
        public static DataTable GetStates()
        {
            string serchsql = string.Format(@" select * from THERE_CHECK_EVENT where valid = '1' order by id ");
            return sql_Helper.ExecuteDataTable(serchsql, CommandType.Text);
        }

        /// <summary>
        /// 根据病案首页序号获得患者的状态数据
        /// id,noofinpat,ccode,dotime
        /// </summary>
        /// author:cyq
        /// date:2012-08-16
        /// <param name="noPatID">病案首页序号</param>
        /// <returns></returns>
        public static DataTable GetStatesByNoPatID(string noPatID)
        {
            string sqlStr = string.Format(@" select * from PatientStatus where noofinpat='{0}' order by id ", noPatID);
            return sql_Helper.ExecuteDataTable(sqlStr, CommandType.Text);
        }

        /// <summary>
        /// 根据病案首页序号获得，此患者的状态数据
        /// add by ywk 
        /// </summary>
        /// <param name="NoPatID">住院号</param>
        /// <param name="noofinpat">真正的首页序号</param>
        /// <returns></returns>
        public static DataTable GetStateData(string NoPatID, string noofinpat, string outwarddate, string admitdate)
        {
            ////add by cyq 2013-03-05
            if (string.IsNullOrEmpty(noofinpat) || string.IsNullOrEmpty(admitdate))
            {
                return null;
            }
            ////            string serchsql = string.Format(@" select distinct  a.noofinpat,a.id,a.dotime,a.ccode,c.name,b.name as patname 
            ////                                                from PatientStatus a join inpatient b 
            ////                                                on a.noofinpat=b.patid join THERE_CHECK_EVENT c
            ////                                                on a.ccode=c.id
            ////                                                where a.noofinpat='{0}' ", NoPatID);//categoryid要是id的前两位
            ////add by cyq 2013-03-05 获取本次入院时间范围(多次入院记录的情况--->上次入院时间至下次入院时间)
            //DataTable inHosDetails = DS_SqlService.GetInHosDetails(int.Parse(noofinpat));
            //if (null == inHosDetails || inHosDetails.Rows.Count == 0)
            //{
            //    return null;
            //}
            //DataRow thisInpatient = inHosDetails.Select(" NOOFINPAT = " + noofinpat).FirstOrDefault();
            //string startTime = string.IsNullOrEmpty(thisInpatient["inwarddate"].ToString()) ? thisInpatient["admitdate"].ToString() : DateTime.Parse(thisInpatient["inwarddate"].ToString()).ToString("yyyy-MM-dd 00:00");
            //string endTime = outwarddate;
            //if (null != thisInpatient && (thisInpatient["STATUS"].ToString() == "1502" || thisInpatient["STATUS"].ToString() == "1503"))
            //{//病人已出院
            //    if (!string.IsNullOrEmpty(thisInpatient["outhosdate"].ToString().Trim()))
            //    {
            //        endTime = DateTime.Parse(thisInpatient["outhosdate"].ToString()).ToString("yyyy-MM-dd 00:00");
            //    }
            //    else if (!string.IsNullOrEmpty(thisInpatient["outwarddate"].ToString().Trim()))
            //    {
            //        endTime = DateTime.Parse(thisInpatient["outwarddate"].ToString()).ToString("yyyy-MM-dd 00:00");
            //    }
            //}
            //else
            //{
            //    endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            //}

            //edit by ywk 此处的病案号对一个病人多次入院的时候就会导致取值错误
            //增加出院时间，及连接Inpatient中的首页序号 时间大于在院小于出院
            //            string serchsql = string.Format(@"select distinct  a.noofinpat,a.id,a.dotime,a.ccode,c.name,b.name as patname ,b.noofinpat 
            //                                                from PatientStatus a  join inpatient b 
            //                                                on a.noofinpat=b.patid  join THERE_CHECK_EVENT c
            //                                                on a.ccode=c.id
            //                                                where a.noofinpat='{0}'and a.dotime<='{1}' and a.dotime>='{2}' and b.noofinpat='{3}' order by a.dotime",
            //                                              NoPatID, endTime, startTime, noofinpat);
            string serchsql = string.Format(@"select distinct  a.noofinpat,a.id,a.dotime,a.ccode,c.name,b.name as patname ,a.patid 
                                                from PatientStatus a  join inpatient b 
                                                on a.noofinpat=to_char(b.noofinpat)  join THERE_CHECK_EVENT c
                                                on a.ccode=c.id
                                                where a.noofinpat='{0}' order by a.dotime", noofinpat);
            return sql_Helper.ExecuteDataTable(serchsql, CommandType.Text);
        }
        /// <summary>
        /// 保存操作，（更新删除添加）
        /// add by ywk 
        /// </summary>
        /// <param name="patStateEnt"></param>
        /// <param name="edittype"></param>
        public static string SaveStateData(PatStateEntity patStateEnt, string edittype)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@CCode",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar),
                    new SqlParameter("@NoofInPat",SqlDbType.VarChar),
                    new SqlParameter("@DoTime",SqlDbType.VarChar),
                    new SqlParameter("@Patid",SqlDbType.VarChar)
                };
                sqlParam[0].Value = edittype;
                sqlParam[1].Value = patStateEnt.CCODE;
                sqlParam[2].Value = patStateEnt.ID;
                sqlParam[3].Value = patStateEnt.NOOFINPAT;
                sqlParam[4].Value = patStateEnt.DOTIME;
                sqlParam[5].Value = patStateEnt.PATID;

                return sql_Helper.ExecuteDataSet("EMRPROC.usp_Edit_PatStates", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 得到配置信息  ywk 2012年5月21日 11:29:14
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigValueByKey(string key)
        {
            if (MethodSet.App == null) return "";
            string sql1 = " select * from appcfg where configkey = '" + key + "'  ";
            DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
            string config = string.Empty;
            if (dt.Rows.Count > 0)
            {
                config = dt.Rows[0]["value"].ToString();
            }
            return config;
        }

        /// <summary>
        /// 得到不显示时间的事件 wwj 2012-05-28
        /// </summary>
        /// <returns></returns>
        public static List<DataRow> GetNotShowTimeEvent()
        {
            DataTable DTState = MethodSet.GetStates();
            List<DataRow> notShowTimeEvent = new List<DataRow>();
            notShowTimeEvent = DTState.AsEnumerable().Select(r => r).ToList();
            return notShowTimeEvent;
        }
        /// <summary>
        /// 获得当前科室在院患者信息
        /// </summary>
        /// <param name="dept"></param>
        /// <param name="ward"></param>
        /// <returns></returns>
        internal static DataTable GetCurrentPatient(string dept, string ward)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                      new SqlParameter("Deptids", SqlDbType.VarChar, 8),
                  new SqlParameter("Wardid", SqlDbType.VarChar, 8)
                };
            sqlParam[0].Value = dept;
            sqlParam[1].Value = ward;
            DataTable table1 = App.SqlHelper.ExecuteDataTable("usp_QueryInwardPatients", sqlParam, CommandType.StoredProcedure);
            return table1;
        }
        /// <summary>
        /// 根据首页序号获得病人的信息 以判断是不是婴儿
        /// add by ywk 
        /// </summary>
        /// <param name="m_noofinpat"></param>
        /// <returns></returns>
        internal static DataTable GetPatData(string m_noofinpat)
        {
            string sql = string.Format(@"select * from inpatient where noofinpat='{0}' ", m_noofinpat);
            return App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
        }

        /// <summary>
        /// 获取某一天的患者状态
        /// edit by Yanqiao.Cai 2012-11-23
        /// 1、add try ... catch
        /// 2、添加婴儿状态
        /// </summary>
        /// <param name="theTime"></param>
        /// <param name="patid"></param>
        /// <returns></returns>
        public static DataTable GetPatStatesInDatetime(DateTime? theTime, string patid, PatientInfo patientInfo)
        {
            try
            {
                if (null == theTime)
                {
                    return new DataTable();
                }
                //string sql = string.Format(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on  a.ccode=b.id and a.noofinpat='{0}' and dotime like '{1}%' order by dotime ", patid, ((DateTime)theTime).ToString("yyyy-MM-dd"));
                string sql = string.Format(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on  a.ccode=b.id and a.noofinpat='{0}' and dotime like '{1}%' order by dotime ", patientInfo.NoOfInpat, ((DateTime)theTime).ToString("yyyy-MM-dd"));
                DataTable dt = sql_Helper.ExecuteDataTable(sql, CommandType.Text);
                if (patientInfo.IsBaby == "1" && ((DateTime)theTime).ToString("yyyy-MM-dd") == DateTime.Parse(patientInfo.BirthDay).ToString("yyyy-MM-dd"))
                {
                    //添加婴儿状态
                    DataRow dr = dt.NewRow();
                    dr["dotime"] = patientInfo.BirthDay;
                    dr["name"] = "出生";
                    dt.Rows.InsertAt(dr, 0);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取某段时间内的患者状态
        /// edit by Yanqiao.Cai 2012-11-23
        /// 1、add try ... catch
        /// 2、添加婴儿状态(时间范围内)
        /// </summary>
        /// <param name="theTime"></param>
        /// <param name="patid"></param>
        /// <returns></returns>
        public static DataTable GetPatStatesByDateDiv(DateTime startdate, DateTime enddate, string patid, PatientInfo patientInfo)
        {
            try
            {
                //string sql = string.Format(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on  a.ccode=b.id and a.noofinpat='{0}' and a.dotime >='{1}' and a.dotime <='{2}' order by dotime ", patid, startdate.ToString("yyyy-MM-dd 00:00"), enddate.ToString("yyyy-MM-dd 23:59"));
                string sql = string.Format(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on  a.ccode=b.id and a.noofinpat='{0}' and a.dotime >='{1}' and a.dotime <='{2}' order by dotime ", patientInfo.NoOfInpat, startdate.ToString("yyyy-MM-dd 00:00"), enddate.ToString("yyyy-MM-dd 23:59"));
                DataTable dt = sql_Helper.ExecuteDataTable(sql, CommandType.Text);
                if (patientInfo.IsBaby == "1" && DateTime.Parse(patientInfo.BirthDay) >= DateTime.Parse(startdate.ToString("yyyy-MM-dd 00:00")) && DateTime.Parse(patientInfo.BirthDay) <= DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59")))
                {
                    //添加婴儿状态
                    DataRow dr = dt.NewRow();
                    dr["dotime"] = patientInfo.BirthDay;
                    dr["name"] = "出生";
                    dt.Rows.InsertAt(dr, 0);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取某段时间内所在的科室
        /// </summary>
        /// <param name="patNoofHis"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public static string GetChangedDeptName(string patNoofHis, DateTime starttime, DateTime endtime, PatientInfo patientInfo)
        {
            string returnValue = string.Empty;
            using (OracleConnection conn = new OracleConnection(DataAccessFactory.GetSqlDataAccess("HISDB").GetDbConnection().ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(@"select OLD_DATA_NAME from changeddeptrecord where INPATIENT_NO='{0}' and OPER_DTIME >= '{1}' and OPER_DTIME <= '{2}' order by OPER_DTIME", patNoofHis, starttime.ToString("yyyy-MM-dd 00:00:00"), endtime.ToString("yyyy-MM-dd 23:59:59"));

                try
                {
                    Object obj = cmd.ExecuteOracleScalar();
                    if (null != obj)
                    {
                        returnValue = obj.ToString();
                    }
                    else
                    {
                        cmd.CommandText = string.Format(@"select new_data_name from changeddeptrecord where INPATIENT_NO='{0}' and OPER_DTIME < '{1}' order by OPER_DTIME desc", patNoofHis, starttime.ToString("yyyy-MM-dd 00:00:00"));
                        Object ob = cmd.ExecuteOracleScalar();
                        if (null != ob)
                        {
                            returnValue = ob.ToString();
                        }
                        else
                        {
                            cmd.CommandText = string.Format(@"select OLD_DATA_NAME from changeddeptrecord where INPATIENT_NO='{0}' and OPER_DTIME > '{1}' order by OPER_DTIME asc", patNoofHis, endtime.ToString("yyyy-MM-dd 23:59:59"));
                            Object oj = cmd.ExecuteOracleScalar();
                            if (null != oj)
                            {
                                returnValue = oj.ToString();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(returnValue))
                    {
                        returnValue = patientInfo.Section;
                    }
                }
                catch (Exception ex)
                {
                    m_app.CustomMessageBox.MessageShow("调用HIS接口报错！" + ex.Message, CustomMessageBoxKind.ErrorOk);
                }
            }
            return returnValue;
        }
    }
}
