using DrectSoft.DSSqlHelper;
using System;
using System.Data;
using System.Data.SqlClient;
namespace DrectSoft.Core.NurseDocument
{
    /// <summary>
    /// 数据加载处理类
    /// </summary>
    public class DataLoader
    {
        private DataTable allSurgeryData = null;
        //private DataTable patientSateData = null;
        public decimal CurrentPat = 0; //当前病人首页号
        public static string m_InTime = "";//存放 入病区时间
        public static int WeekIndex = 0; //当前病人的入院周数
        public static DateTime adminDate;//保存病人的入院日期
        public static DateTime m_firstDateOfCunrrentPage = DateTime.Now;//当前页表示的周的首日日期
        public string patid = string.Empty;
        public DateTime m_currentDate;//当前查询日期
        public static string outdate;
        #region 常量
        public static string DATEOFSURVEY = "DATEOFSURVEY";//日期 与表字段名一致

        public static string TEMPERATURE = "TEMPERATURE";//温度 与表字段名一致
        public static string PULSE = "PULSE";//脉搏 与表字段名一致
        public static string HEARTRATE = "HEARTRATE";//心率 与表字段名一致
        public static string BREATHE = "BREATHE";//呼吸 与表字段名一致
        public static string _BREATHE = "_BREATHE";//呼吸（机） 与表字段名一致
        public static string PHYSICALCOOLING = "PHYSICALCOOLING";//物理降温 与表字段名一致
        public static string PHYSICALHOTTING = "PHYSICALHOTTING";//物理升温 与表字段名一致
        public static string BLOODPRESSURE = "BLOODPRESSURE";
        public static string NursMaiBoTiWenGang = "NursMaiBoTiWenGang";
        public static string NursMaiBoTiWenKou = "NursMaiBoTiWenKou";
        public static string NursMaiBoTiWenYe = "NursMaiBoTiWenYe";
        public static string NursTiWenHuXiMaiBo = "NursTiWenHuXiMaiBo";

        public static string KOUWEN = "TEMPERATURE001";//口温
        public static string YEWEN = "TEMPERATURE002";//腋温
        public static string GANGWEN = "TEMPERATURE003";//肛温
        #endregion

        public DataLoader(decimal currInpatient, DateTime dateTime)
        {
            CurrentPat = currInpatient;
            //m_currentDate =DateTime.Parse(dateTime.ToString("yyyy-MM-DD HH:mm:ss")) ;
            m_currentDate = dateTime;
            GetPatData(CurrentPat.ToString());
        }

        public DataLoader()
        {
            try
            {
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// 查询出病人的护理记录数据
        /// </summary>
        /// <param name="noOfInpat"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public void GetNursingRecordByDate(string noOfInpat, string _beginDate, string _endDate)
        {
            try
            {
                if (noOfInpat == "")
                {
                    return;
                }
                string sqlStr = string.Format(@" select * from notesonnursing where noofinpat='{0}' and to_date(dateofsurvey,'YYYY-MM-DD')>= to_date('{1}','YYYY-MM-DD') and to_date(dateofsurvey,'YYYY-MM-DD')< =to_date('{2}','YYYY-MM-DD')
                                          order by dateofsurvey asc,to_number(timeslot) asc ", noOfInpat, _beginDate, _endDate);
                allSurgeryData = DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetNursingRecordByDate(string beginDate, string endDate)
        {
            GetNursingRecordByDate(CurrentPat.ToString(), beginDate, endDate);
        }

        public void GetNursingRecordByDate(string endDate)
        {
            GetNursingRecordByDate(CurrentPat.ToString(), m_InTime, endDate);
        }

        //过滤集合，获得指定列的体征数据点集合 并包装成 DataPoint集合类型
        public DataCollection GetVitalSignsDataPoints(string name)
        {
            DataCollection list = new DataCollection();
            list.FieldName = name;
            DataTable dt = GetDateTimeForColumns(m_currentDate);
            //for (int i = 0; i < dt.Rows.co; i++)
            //{
            //    foreach (string item in ConfigInfo.dic_Timelot.Values)
            //    {

            //    }
            //}
            foreach (DataRow itemRow in dt.Rows)
            {
                foreach (string item in ConfigInfo.dic_Timelot.Values)
                {
                    DataRow row = GetContainVitalSignsDate(DateTime.Parse(itemRow["BeforeConvertDate"].ToString()).ToString("yyyy-MM-dd"), item);
                    DataPoint dp = new DataPoint();
                    if (row != null)
                    {
                        dp.value = row[name].ToString(); //值     
                        dp.date = row["DATEOFSURVEY"].ToString();
                        dp.timeslot = row["TIMESLOT"].ToString();
                        if (name.Equals(TEMPERATURE))//若为体温值
                            dp.temperatureType = Int32.Parse(row["WAYOFSURVEY"].ToString());
                        else
                            dp.temperatureType = 0;

                        list.Add(dp);
                    }
                    else
                    {
                        dp.value = "";
                        dp.date = DateTime.Parse(itemRow["BeforeConvertDate"].ToString()).ToString("yyyy-MM-dd");
                        dp.timeslot = item;
                        list.Add(dp);
                    }
                }
            }
            return list;
        }

        public DataRow GetContainVitalSignsDate(string dateTime, string datelot)
        {
            try
            {
                DataRow dataRow = null;
                foreach (DataRow row in allSurgeryData.Rows)
                {
                    if (row["DATEOFSURVEY"].ToString().Trim().Equals(dateTime) && row["TIMESLOT"].ToString().Trim().Equals(datelot))
                    {
                        return dataRow = row;
                    }
                }
                return dataRow;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //获得非体征数据
        public DataCollection GetDailyDataPoints(string name)
        {
            if (name.Equals("DATEOFSURVEY")) return null;//日期数据单独处理 

            //根据首页序号获得这个人的信息add by ywk 2013年4月26日10:24:43  
            DataTable dtPatInfo = GetPatData(CurrentPat.ToString());
            string status = string.Empty;//存储这个人的真正的状态 
            if (dtPatInfo != null && dtPatInfo.Rows.Count > 0)
            {
                status = dtPatInfo.Rows[0]["STATUS"].ToString();
            }


            DataCollection list = new DataCollection();
            list.FieldName = name;

            if (ConfigInfo.editable == 1 && name.Equals("DAYSAFTERSURGERY"))
            {
                DateTime CunrrentTime = m_firstDateOfCunrrentPage;
                for (int i = 0; i < 7; i++)
                {
                    if (CunrrentTime.AddDays(i) < DateTime.Now)
                    {
                        if (!string.IsNullOrEmpty(outdate) && DateTime.Parse(DateTime.Parse(outdate).ToString("yyyy-MM-dd")) < CunrrentTime.AddDays(i) && (status == "1503" || status == "1502"))
                        {
                            break;
                        }
                        DataPoint dp = new DataPoint();
                        dp.value = SetDaysAfterSurgery((CunrrentTime.AddDays(i)).ToString("yyyy-MM-dd"), CurrentPat.ToString()); //值     
                        dp.date = CunrrentTime.AddDays(i).ToString("yyyy-MM-dd");
                        dp.temperatureType = 0;
                        list.Add(dp);
                    }
                }
            }
            else if (ConfigInfo.editDays == 1 && name.Equals("DAYOFHOSPITAL"))
            {
                int inhospital = int.Parse(SetDayInHospital(m_firstDateOfCunrrentPage.ToString("yyyy-MM-dd")));
                DateTime CunrrentTime = m_firstDateOfCunrrentPage;
                for (int i = 0; i < 7; i++)
                {
                    if (CunrrentTime.AddDays(i) < DateTime.Now)
                    {
                        //如果这个病人没有出院，出院是根据状态进行判断的，这样画会出问题的
                        //edit by ywk 2013年4月26日10:19:22 
                        if (!string.IsNullOrEmpty(outdate) && DateTime.Parse(DateTime.Parse(outdate).ToString("yyyy-MM-dd")) < CunrrentTime.AddDays(i)
                            && (status == "1503" || status == "1502"))
                        {
                            break;
                        }
                        DataPoint dp = new DataPoint();
                        dp.value = (inhospital + i).ToString(); //值     
                        dp.date = CunrrentTime.AddDays(i).ToString("yyyy-MM-dd");
                        dp.temperatureType = 0;
                        list.Add(dp);
                    }
                }
            }
            else
            {
                foreach (DataRow row in allSurgeryData.Rows)
                {
                    DataPoint dp = new DataPoint();
                    if (row[name].ToString() == "") continue;
                    dp.value = row[name].ToString(); //值     
                    dp.date = row["DATEOFSURVEY"].ToString();
                    dp.timeslot = row["TIMESLOT"].ToString(); ;
                    dp.temperatureType = 0;

                    list.Add(dp);
                }
            }


            return list;
        }

        /// <summary>
        /// 得到三测单表格第一行的时间文本数据
        /// </summary>
        /// <param name="allocateDate">分配日期</param>
        /// <returns></returns>
        public DataTable GetDateTimeForColumns(DateTime allocateDate)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BeforeConvertDate"); //转换前的值
            dt.Columns.Add("ConvertedDate"); //转换后的值

            string str1 = m_InTime.Split(' ')[0];//入院时间
            DateTime inDate = Convert.ToDateTime(str1);

            allocateDate = Convert.ToDateTime(allocateDate.ToString("yyyy-MM-dd"));

            DateTime dateTimeBeginTime = new DateTime(inDate.Year, inDate.Month, inDate.Day);
            int weeks = (allocateDate - inDate).Days / ConfigInfo.m_Days;
            WeekIndex = weeks;//保存周数
            dateTimeBeginTime = dateTimeBeginTime.AddDays(ConfigInfo.m_Days * weeks);
            m_firstDateOfCunrrentPage = dateTimeBeginTime;//获取周首日时间，保存为全局变量
            for (int i = 0; i < ConfigInfo.m_Days; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = dateTimeBeginTime;
                dr[1] = "";
                dateTimeBeginTime = dateTimeBeginTime.AddDays(1);
                dt.Rows.Add(dr);
            }
            if (Convert.ToDateTime(dt.Rows[0][0]).ToString("yyyy-MM-dd") == inDate.ToString("yyyy-MM-dd") //住院日期首页第1日 需填写年-月-日（如：2010－03－26）
                || Convert.ToDateTime(dt.Rows[0][0]).ToString("MM-dd") == "01-01") //跨年度第1日需填写年-月-日（如：2010－03－26）
            {
                dt.Rows[0][1] = Convert.ToDateTime(dt.Rows[0][0]).ToString("yyyy-MM-dd");
            }
            else //其余页的体温单的第1日 填写月-日（如03-26）
            {
                dt.Rows[0][1] = Convert.ToDateTime(dt.Rows[0][0]).ToString("MM-dd");
            }
            for (int i = dt.Rows.Count - 1; i > 0; i--)
            {
                DateTime dateTime1 = Convert.ToDateTime(dt.Rows[i][0]);
                DateTime dateTime2 = Convert.ToDateTime(dt.Rows[i - 1][0]);

                if (dateTime1.Year > dateTime2.Year)
                {
                    dt.Rows[i][1] = dateTime1.ToString("yyyy-MM-dd");
                }
                else if (dateTime1.Month > dateTime2.Month)
                {
                    dt.Rows[i][1] = dateTime1.ToString("MM-dd");
                }
                else if (dateTime1.Day > dateTime2.Day)
                {
                    dt.Rows[i][1] = dateTime1.ToString("dd");
                }
            }
            return dt;
        }


        /// <summary>
        /// 取得病人基本数据
        /// edit by zyx 添加入科时间验证，如果如何时间为空则取入院时间
        /// </summary>
        /// <param name="m_noofinpat"></param>
        /// <returns></returns>
        public DataTable GetPatData(string m_noofinpat)
        {
            try
            {
                string sql = string.Format(@"select * from inpatient where noofinpat='{0}' ", m_noofinpat);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dt.Rows[0]["inwarddate"].ToString() != "" && dt.Rows[0]["inwarddate"].ToString() != null)
                {
                    m_InTime = dt.Rows[0]["inwarddate"].ToString();
                }
                else if (dt.Rows[0]["ADMITDATE"].ToString() != null && dt.Rows[0]["ADMITDATE"].ToString() != "")
                {
                    m_InTime = dt.Rows[0]["ADMITDATE"].ToString();
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("入院和入科时间不能同时为空");
                }
                adminDate = DateTime.Parse((DateTime.Parse(dt.Rows[0]["ADMITDATE"].ToString())).ToString("yyyy-MM-dd"));
                if (dt.Rows[0]["OUTWARDDATE"].ToString().Trim() != null && dt.Rows[0]["OUTWARDDATE"].ToString().Trim() != "")
                {
                    outdate = dt.Rows[0]["OUTWARDDATE"].ToString().Trim();
                }
                else
                {
                    outdate = dt.Rows[0]["OUTHOSDATE"].ToString().Trim();
                }

                patid = dt.Rows[0]["patid"].ToString();
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable GetPatData()
        {
            return GetPatData(CurrentPat.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noOfInpat"></param>
        /// <returns></returns>
        public DataTable GetPatientInfoForThreeMeasureTable(decimal noOfInpat)
        {

            SqlParameter[] sqlParam = new SqlParameter[] 
            {

                new SqlParameter("@NoOfInpat", SqlDbType.Decimal),
                new SqlParameter("@result", SqlDbType.Structured)
            };
            sqlParam[0].Value = noOfInpat;
            sqlParam[1].Direction = ParameterDirection.Output;
            return DS_SqlHelper.ExecuteDataTable("EMRPROC.usp_GetPatientInfoForThreeMeas", sqlParam, CommandType.StoredProcedure);
        }

        public DataTable GetPatientInfoForThreeMeasureTable()
        {
            return GetPatientInfoForThreeMeasureTable(CurrentPat);
        }

        /// <summary>
        /// 获取某一天的患者状态
        /// edit by zyx 2013-01-23 1、添加try catch ；
        /// </summary>
        /// <param name="theTime"></param>
        /// <param name="patid"></param>
        /// <returns></returns>
        public DataTable GetPatStatesInDatetime(DateTime theTime, string patid)
        {
            try
            {
                if (null == theTime)
                {
                    return new DataTable();
                }
                string sql = string.Format(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on  a.ccode=b.id and a.noofinpat='{0}' and dotime like '{1}%' order by dotime ", patid, ((DateTime)theTime).ToString("yyyy-MM-dd"));
                return DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 获取某段时间内的患者状态
        /// edit by zyx 1、添加try catch ；2、解决病人状态时间为00：00时不显示问题
        /// </summary>
        /// <param name="theTime"></param>
        /// <param name="patid"></param>
        /// <returns></returns>
        public DataTable GetPatStatesByDateDiv(DateTime startdate, DateTime enddate, string NOOFINPAT)
        {
            try
            {
                string sql = string.Format(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on  a.ccode=b.id and a.noofinpat='{0}' and a.dotime >='{1}' and a.dotime <'{2}' order by dotime ", NOOFINPAT, startdate.ToString("yyyy-MM-dd 00:00"), enddate.ToString("yyyy-MM-dd 00:00"));
                //string sql = string.Format(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on  a.ccode=b.id and a.noofinpat='{0}'  order by dotime ", NOOFINPAT);
                return DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 插入入院记录
        /// 中心需求
        /// Ad By XLB 2013-06-09
        /// </summary>
        /// <param name="noofinpat">病案号</param>
        /// <param name="inHosDate">入院时间</param>
        /// <param name="inCode">入院状态代码</param>
        private void InsertInHosStatus(string noofinpat, string inHosDate, string inCode)
        {
            try
            {
                DS_SqlHelper.ExecuteNonQuery(@"insert into PatientStatus(ID,CCODE,NOOFINPAT,DOTIME,PATID) 
                                           values(seq_Emr_ConfigPoint_ID.Nextval,@cCode,@noofinpat,@doTime,@patId)",
                                            new SqlParameter[]{new SqlParameter("@cCode",inCode),
                                                new SqlParameter("@noofinpat",noofinpat),new SqlParameter("@doTime",inHosDate),
                                            new SqlParameter("@patId",patid) }, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <auth>XLB</auth>
        /// <date>2013-06-09</date>
        /// <Purpose>抓取当前病人所有的状态</Purpose>
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        private DataTable GetAllPatStatusByNoofinpat(string noofinpat)
        {
            try
            {
                return DS_SqlHelper.ExecuteDataTable(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on  a.ccode=b.id and a.noofinpat=@nOofinpat", new SqlParameter[] { new SqlParameter("@nOofinpat", noofinpat) }, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取状态集合
        /// Modify by xlb 2013-06-09
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="NOOFINPAT"></param>
        /// <returns></returns>
        public DataCollection GetPatStates(DateTime startdate, DateTime enddate, string NOOFINPAT)
        {
            DataTable dt = GetPatStatesByDateDiv(startdate, enddate, NOOFINPAT);
            #region Add by xlb 2013-06-09 如果当前病人不存在入院状态则自动插入
            DataTable dataTable = GetAllPatStatusByNoofinpat(NOOFINPAT);//病人所有的状态
            bool IsNeedInsertInHos = false;//用于判断是否需要插入入院状态的开关false表示不需要插入true表示需要插入
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow item in dataTable.Rows)
                {
                    if (item["ID"].ToString().Trim().Equals(ConfigInfo.InHospitalCode))
                    {//存在入院记录则终止遍历
                        IsNeedInsertInHos = false;
                        break;
                    }
                    else
                    {
                        IsNeedInsertInHos = true;//遍历不到则打开开关
                        continue;
                    }
                }
            }
            else//集合为空且显示入院状态则插入入院记录
            {
                if (ConfigInfo.isInHospital)
                {
                    InsertInHosStatus(NOOFINPAT, m_InTime.Substring(0, 16), ConfigInfo.InHospitalCode);
                }
            }
            if (IsNeedInsertInHos)//true且显示入院状态则插入入院状态
            {
                if (ConfigInfo.isInHospital)
                {
                    //有入科时间，而且病人状态表中维护了值就进行更新
                    if (CheckIsExsitInwardDate(NOOFINPAT) && CheckISExistStatus(NOOFINPAT, ConfigInfo.InHospitalCode))//存在入科时间而且状态表里也有
                    {
                        UpdateInHosStatus(NOOFINPAT, m_InTime.Substring(0, 16), ConfigInfo.InHospitalCode);

                    }
                    else
                    {
                        InsertInHosStatus(NOOFINPAT, m_InTime.Substring(0, 16), ConfigInfo.InHospitalCode);
                    }
                }
                //else//不需要插入就更新这个状态，改为入病区时间 add by ywk 2013年8月12日 18:07:36 (状态表里有了)
                //{
                //    UpdateInHosStatus(NOOFINPAT, m_InTime.Substring(0, 16), ConfigInfo.InHospitalCode);
                //}
            }

            dt = GetPatStatesByDateDiv(startdate, enddate, NOOFINPAT);
            #endregion
            DataCollection list = new DataCollection();
            list.FieldName = "";
            foreach (DataRow row in dt.Rows)
            {
                DataPoint dp = new DataPoint();
                dp.value = row["name"].ToString(); //值     
                dp.date = row["DOTIME"].ToString();
                dp.timeslot = "";
                list.Add(dp);
            }
            if (ConfigInfo.isInHospital)
            {
                if (DateTime.Parse(startdate.ToString("yyyy-MM-dd")) == DateTime.Parse(DateTime.Parse(m_InTime).ToString("yyyy-MM-dd")))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        bool isContainsHospital = false;
                        foreach (DataRow item in dt.Rows)
                        {
                            if (item["ID"].ToString().Trim().Equals(ConfigInfo.InHospitalCode))
                            {
                                isContainsHospital = true;
                                break;
                            }
                        }
                        if (!isContainsHospital)
                        {
                            DataPoint dp = new DataPoint();
                            dp.value = ConfigInfo.ShowStatus;//Add by xlb 2013-06-09 
                            dp.date = m_InTime;
                            dp.timeslot = "";
                            list.Add(dp);
                        }
                    }
                    else
                    {
                        DataPoint dp = new DataPoint();
                        dp.value = ConfigInfo.ShowStatus;
                        dp.date = m_InTime;
                        dp.timeslot = "";
                        list.Add(dp);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 查看状态表里是否有此人入院状态 
        /// </summary>
        /// <param name="NOOFINPAT"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CheckISExistStatus(string NOOFINPAT, string ccode)
        {
            string sql = string.Format("select * from PatientStatus a join THERE_CHECK_EVENT b on  a.ccode=b.id and a.noofinpat='{0}' and  a.ccode='{1}'  ", NOOFINPAT, ccode);
            DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            bool isExist = false;
            if (dt != null && dt.Rows.Count > 0)
            {
                isExist = true;//存在此状态 
            }
            else
            {
                isExist = false;
            }
            return isExist;
        }

        /// <summary>
        /// 判断当前这个病人是不是存在入科时间
        /// </summary>
        /// <param name="NOOFINPAT"></param>
        /// <returns></returns>
        private bool CheckIsExsitInwardDate(string NOOFINPAT)
        {
            string sql = string.Format("select INWARDDATE from inpatient where noofinpat='{0}' ", NOOFINPAT);
            DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            bool isExist = false;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["INWARDDATE"].ToString()))
                {
                    isExist = true;//存在入科时间
                }
                else
                {
                    isExist = false;
                }
            }
            return isExist;
        }
        /// <summary>
        /// add by ywk 2013年8月12日 18:02:41
        /// </summary>
        /// <param name="NOOFINPAT"></param>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        private void UpdateInHosStatus(string NOOFINPAT, string intime, string hoscode)
        {
            string sql = string.Format("update PatientStatus set DOTIME='{0}' where NOOFINPAT='{1}' and CCODE='{2}' ", intime, NOOFINPAT, hoscode);
            DS_SqlHelper.ExecuteNonQuery(sql, CommandType.Text);

        }

        /// <summary>
        /// 获得母亲住院号
        /// <auth>zyx</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public string GetMotherPatid(string noofinpat)
        {
            try
            {
                string sql = "select patid from inpatient where noofinpat=@noofinpat";
                SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@noofinpat", noofinpat) };
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, parms, CommandType.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// 计算手术后天数
        /// </summary>
        public string SetDaysAfterSurgery(string inputdate, string patid)
        {
            //DateOfSurvey 当前录入的日期时间 
            //string serachsql = string.Format(@"select * from patientstatus where noofinpat='{0}' and trunc(to_date(patientstatus.dotime,'yyyy-mm-dd hh24:mi:ss'))>=trunc(to_date('{1}','yyyy-mm-dd hh24:mi:ss'))-14 and dotime<'{1}'  and ccode='7000' order by dotime asc", patid, inputdate + " 23:59:59");
            string serachsql = string.Format
                (@"    select * from patientstatus  p left join THERE_CHECK_EVENT t on p.ccode=t.id  where noofinpat='{0}' 
and trunc(to_date(p.dotime,'yyyy-mm-dd hh24:mi:ss'))>=trunc(to_date('{1}','yyyy-mm-dd hh24:mi:ss'))-14 
and dotime<'{1}'  and (p.ccode='7000' or t.name ='分娩·手术') order by dotime asc", patid, inputdate + " 23:59:59");
            DataTable AllSurgeryData = MethodSet.App.SqlHelper.ExecuteDataTable(serachsql, CommandType.Text);

            if (AllSurgeryData.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(inputdate))
                {
                    DateTime nowInputDate = DateTime.Parse(inputdate + " 23:59:59");

                    if (AllSurgeryData.Rows.Count > 0)//有一次以上的手术 
                    {
                        string content = ReturnCotent(nowInputDate, AllSurgeryData);
                        return content;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 对于一次以上的手术返回要显示的内容
        /// ywk
        /// </summary>
        /// <param name="count"></param>
        /// <param name="nowinputtime"></param>
        /// <returns></returns>
        private string ReturnCotent(DateTime nowinputtime, DataTable dtdata)
        {
            string mycontent = string.Empty;
            string outcontent = string.Empty;
            for (int i = 0; i < dtdata.Rows.Count; i++)
            {
                mycontent = (nowinputtime - DateTime.Parse(dtdata.Rows[i]["dotime"].ToString())).Days.ToString();
                if (mycontent == "0")
                {
                    outcontent = ReturnCount((i + 1).ToString());
                }
                else if (i == (dtdata.Rows.Count - 1))
                {
                    outcontent = (nowinputtime - DateTime.Parse(dtdata.Rows[i]["dotime"].ToString())).Days.ToString();
                }

            }
            return outcontent;
        }

        /// <summary>
        /// 处理手术后第几次的显示内容
        /// </summary>
        /// <param name="showcount"></param>
        /// <returns></returns>
        private string ReturnCount(string showcount)
        {
            string show = string.Empty;
            switch (showcount)
            {
                case "1":
                    show = "";
                    break;
                case "2":
                    show = "II-0";
                    break;
                case "3":
                    show = "III-0";
                    break;
                case "4":
                    show = "IV-0";
                    break;
                case "5":
                    show = "V-0";
                    break;
                case "6":
                    show = "VI-0";
                    break;
                case "7":
                    show = "VII-0";
                    break;
                case "8":
                    show = "VIII-0";
                    break;
                case "9":
                    show = "IX-0";
                    break;
                case "10":
                    show = "X-0";
                    break;


                default:
                    break;
            }
            return show;
        }

        /// <summary>
        /// 住院天数
        /// </summary>
        /// <param name="inputdate"></param>
        /// <returns></returns>
        public string SetDayInHospital(string inputdate)
        {
            DateTime AdmitDate;
            if (ConfigInfo.isAdminDate)
            {
                AdmitDate = Convert.ToDateTime(Convert.ToDateTime(adminDate).Date.ToString("yyyy-MM-dd 00:00:01"));
            }
            else
            {
                AdmitDate = Convert.ToDateTime(Convert.ToDateTime(m_InTime).Date.ToString("yyyy-MM-dd 00:00:01"));
            }
            DateTime CurrDate = Convert.ToDateTime(inputdate + "  00:00:01");

            return (CurrDate.Subtract(AdmitDate).Days + 1).ToString();
        }
    }
}
