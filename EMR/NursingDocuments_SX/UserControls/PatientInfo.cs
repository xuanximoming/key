using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.Core.NursingDocuments.PublicSet;

namespace DrectSoft.Core.NursingDocuments.UserControls
{
    class PatientInfo
    {
        public static string InpatientName = string.Empty;   //姓名
        public static string Age = string.Empty;             //年龄
        public static string Gender = string.Empty;          //性别
        public static string Section = string.Empty;         //科别
        public static string BedCode = string.Empty;         //病床号
        public static string InTime = DateTime.Now.ToString("yyyy-MM-dd");  //入院日期
        public static string OutTime = string.Empty;         //出院日期
        public static string InpatientNo = string.Empty;     //住院号
        public static string NoOfInpat = string.Empty;       //首页序号
        public static string IsBaby = string.Empty;//是否是婴儿 add by ywk 2012年7月30日 13:39:31
        public static string Mother = string.Empty;//母亲的Noofinpat号

        public static DataTable DataTableTemperature = new DataTable();     //体温数据
        public static DataTable DataTableMaiBo = new DataTable();           //脉搏数据
        public static DataTable DataTableHuXi = new DataTable();            //呼吸数据
        public static DataTable DataTableWuLiJiangWen = new DataTable();    //物理降温数据
        public static DataTable DataTableXinLv = new DataTable();           //心率数据
        public static DataTable DateTableXueYa = new DataTable();           //血压数据
        public static DataTable DataTableZongRuLiang = new DataTable();     //总入量
        public static DataTable DataTableZongChuLiang = new DataTable();    //总出量
        public static DataTable DataTableYinLiuLiang = new DataTable();     //引流量
        public static DataTable DataTableDaBianCiShu = new DataTable();     //大便次数
        public static DataTable DataTableShenGao = new DataTable();         //身高
        public static DataTable DataTableTiZhong = new DataTable();         //体重
        public static DataTable DataTableGuoMinYaoWu = new DataTable();     //过敏药物
        public static DataTable DataTableTeShuZhiLiao = new DataTable();    //特殊治疗
        public static DataTable DataTablePainInfo = new DataTable();          //其他1 现在改为疼痛栏位 ywk
        public static DataTable DataTableOther2 = new DataTable();          //其他2
        //public static DataTable DataTablePainInfo = new DataTable();          //其他2 

        public static DataTable DataTableDayOfHospital = new DataTable(); //住院天数
        public static DataTable DataTableOperationTime = new DataTable(); //手术信息

        public static DataTable DateTableEventInformation = new DataTable(); //患者入院、转入、手术、分娩、出院、死亡等

        //新增物理升温-----------add by  ywk
        public static DataTable DataTableWuLiShengWen = new DataTable();    //物理升温数据

        /// <summary>
        /// 清空病人的基本信息
        /// </summary>
        public void ClearPatientInof()
        {
            InpatientName = string.Empty;
            Age = string.Empty;
            Gender = string.Empty;
            Section = string.Empty;
            BedCode = string.Empty;
            InTime = string.Empty;
            OutTime = string.Empty;
            InpatientNo = string.Empty;
            NoOfInpat = string.Empty;
        }

        /// <summary>
        /// 得到病人的基本信息
        /// </summary>
        /// <param name="dataTablePatientInfo"></param>
        public void InitPatientInfo(DataTable dataTablePatientInfo)
        {
            if (dataTablePatientInfo.Rows.Count > 0)
            {
                InpatientName = dataTablePatientInfo.Rows[0]["patient_name"].ToString();   //姓名
                Age = dataTablePatientInfo.Rows[0]["AgeStr"].ToString();                   //年龄
                Gender = dataTablePatientInfo.Rows[0]["gender"].ToString();                //性别

                //Modified By wwj 2011-10-19 科室和床位可能与病人当前所在科室和床位不一致的情况，需要通过后面对BedInfo的判断
                //Section = dataTablePatientInfo.Rows[0]["dept_name"].ToString();//string.Empty;/科别
                //BedCode = dataTablePatientInfo.Rows[0]["admitbed"].ToString();//string.Empty;床位号
                //edit by ywk 2012年4月17日16:34:31 
                Section = dataTablePatientInfo.Rows[0]["dept_name"].ToString();//string.Empty;/科别
                BedCode = dataTablePatientInfo.Rows[0]["outbed"].ToString();//string.Empty;床位号

                InTime = dataTablePatientInfo.Rows[0]["AdmitDate"].ToString();             //入院日期
                OutTime = dataTablePatientInfo.Rows[0]["OutHosDate"].ToString();           //出院日期
                InpatientNo = dataTablePatientInfo.Rows[0]["PatID"].ToString();            //住院号
                IsBaby = dataTablePatientInfo.Rows[0]["IsBaby"].ToString();//新增的是否婴儿标志 add by ywk 
                Mother = dataTablePatientInfo.Rows[0]["Mother"].ToString();//新增的母亲的Noofinpat add by ywk 
                NoOfInpat = dataTablePatientInfo.Rows[0]["NoOfInpat"].ToString();          //首页号
            }


        }

        /// <summary>
        /// 得到病人所有生命体征的数据
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public void GetNursingRecordByDate(string beginDate, string endDate)
        {
            ClearDataTable();

            DataTable dataTableAll = PublicSet.MethodSet.GetNursingRecordByDate(NoOfInpat, beginDate, endDate);


            #region 处理DataTable
            //只筛出跟当前系统里要测量的时间点的数据 
            //add by ywk 2012年5月21日 10:59:12 
            string m_times = PublicSet.MethodSet.GetConfigValueByKey("VITALSIGNSRECORDTIME");
            string[] timeArray = m_times.Split(',');
            DataRow[] SpliterRows = new DataRow[] { };
            SpliterRows = dataTableAll.DefaultView.Table.Select("timeslot in ('" + timeArray[0] + "','" + timeArray[1] + "','" + timeArray[2] + "','" + timeArray[3] + "','" + timeArray[4] + "','" + timeArray[5] + "')", "id asc");
            DataTable dataNewTable = dataTableAll.Clone();
            dataNewTable.Columns.Add("Sort", typeof(Int32));
            DataColumn[] newCol = new DataColumn[] { dataNewTable.Columns["Sort"], dataNewTable.Columns["ID"] };


            if (SpliterRows.Length > 0)
            {
                for (int k = 0; k < SpliterRows.Length; k++)
                {
                    dataNewTable.ImportRow(SpliterRows[k]);
                    dataNewTable.Rows[k]["Sort"] = SpliterRows[k]["timeslot"];
                }
            }
            dataNewTable.PrimaryKey = newCol;
            //dataNewTable.DefaultView.Sort = "Sort ASC";

            DataTable resultData = dataNewTable.Clone();//最终要的DataTable
            DataRow[] m_Rows = dataNewTable.Select("1=1", "dateofsurvey ASC");
            for (int i = 0; i < m_Rows.Length; i++)
            {
                resultData.ImportRow(m_Rows[i]);
            }
            #endregion

            #region 得到病人的所有数据

            #region 所有生命体征信息
            foreach (DataRow dr in resultData.Rows)
            {
                string date = dr["DateOfSurvey"].ToString();
                string timeSlot = dr["TimeSlot"].ToString();

                if (dr["Temperature"].ToString() != "")//体温
                {
                    DataRow dataRow = DataTableTemperature.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["TypeID"] = dr["TestType"].ToString();
                    dataRow["Value"] = dr["Temperature"].ToString();
                    dataRow["LinkNext"] = "Y";
                    DataTableTemperature.Rows.Add(dataRow);
                }
                if (dr["Pulse"].ToString() != "")//脉搏
                {
                    DataRow dataRow = DataTableMaiBo.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["Pulse"].ToString();
                    dataRow["LinkNext"] = "Y";
                    DataTableMaiBo.Rows.Add(dataRow);
                }
                if (dr["Breathe"].ToString() != "")//呼吸
                {
                    DataRow dataRow = DataTableHuXi.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["Breathe"].ToString();
                    dataRow["LinkNext"] = "Y";
                    dataRow["IsSpecial"] = "N";
                    DataTableHuXi.Rows.Add(dataRow);
                }
                if (dr["PhysicalCooling"].ToString() != "")//物理降温
                {
                    DataRow dataRow = DataTableWuLiJiangWen.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["PhysicalCooling"].ToString();
                    dataRow["LinkNext"] = "Y";
                    DataTableWuLiJiangWen.Rows.Add(dataRow);
                }

                //新增物理升温 add by ywk 
                if (dr["PhysicalHotting"].ToString() != "")//物理升温数据
                {
                    DataRow dataRow = DataTableWuLiShengWen.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["PhysicalHotting"].ToString();
                    dataRow["LinkNext"] = "Y";
                    DataTableWuLiShengWen.Rows.Add(dataRow);
                }

                if (dr["HeartRate"].ToString() != "")//心率
                {
                    DataRow dataRow = DataTableXinLv.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["HeartRate"].ToString();
                    dataRow["LinkNext"] = "Y";
                    dataRow["IsSpecial"] = "N";
                    DataTableXinLv.Rows.Add(dataRow);
                }
                if (dr["BloodPressure"].ToString() != "")//血压
                {
                    DataRow dataRow = DateTableXueYa.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["BloodPressure"].ToString();
                    DateTableXueYa.Rows.Add(dataRow);
                }
                if (dr["CountIn"].ToString() != "")//总入量
                {
                    DataRow dataRow = DataTableZongRuLiang.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["CountIn"].ToString();
                    DataTableZongRuLiang.Rows.Add(dataRow);
                }
                if (dr["CountOut"].ToString() != "")//总出量
                {
                    DataRow dataRow = DataTableZongChuLiang.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["CountOut"].ToString();
                    DataTableZongChuLiang.Rows.Add(dataRow);
                }
                if (dr["Drainage"].ToString() != "")//引流量
                {
                    DataRow dataRow = DataTableYinLiuLiang.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["Drainage"].ToString();
                    DataTableYinLiuLiang.Rows.Add(dataRow);
                }
                if (dr["TimeOfShit"].ToString() != "")//大便次数
                {
                    DataRow dataRow = DataTableDaBianCiShu.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["TimeOfShit"].ToString();
                    DataTableDaBianCiShu.Rows.Add(dataRow);
                }
                if (dr["Height"].ToString() != "")//身高
                {
                    DataRow dataRow = DataTableShenGao.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["Height"].ToString();
                    DataTableShenGao.Rows.Add(dataRow);
                }
                if (dr["Weight"].ToString() != "")//体重
                {
                    DataRow dataRow = DataTableTiZhong.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["Weight"].ToString();
                    DataTableTiZhong.Rows.Add(dataRow);
                }
                if (dr["PARAM2"].ToString() != "")//体重
                {
                    DataRow dataRow = DataTableOther2.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["PARAM2"].ToString();
                     DataTableOther2.Rows.Add(dataRow);
                }
                if (dr["Allergy"].ToString() != "")//过敏药物
                {
                    DataRow dataRow = DataTableGuoMinYaoWu.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["Allergy"].ToString();
                    DataTableGuoMinYaoWu.Rows.Add(dataRow);
                }
                if (dr["DayOfHospital"].ToString() != "")//住院天数
                {
                    DataRow dataRow = DataTableDayOfHospital.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["DayOfHospital"].ToString();
                    DataTableDayOfHospital.Rows.Add(dataRow);
                }
                if (dr["DaysAfterSurgery"].ToString() != "")//手术后天数
                {
                    DataRow dataRow = DataTableOperationTime.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["DaysAfterSurgery"].ToString();
                    DataTableOperationTime.Rows.Add(dataRow);
                }
                //新增的疼痛栏位
                if (dr["PainInfo"].ToString() != "")//疼痛
                {
                    DataRow dataRow = DataTablePainInfo.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    dataRow["Value"] = dr["PainInfo"].ToString();
                    //dataRow["LinkNext"] = "Y";
                    //dataRow["IsSpecial"] = "N";
                    DataTablePainInfo.Rows.Add(dataRow);
                }

            }
            #endregion

            InitEventInformation();
            #endregion
        }

        /// <summary>
        /// 获取某一天是当前显示7天中的第几天
        /// </summary>
        /// <param name="m_DataTimeAllocate"></param>
        /// <param name="theTime"></param>
        /// <returns></returns>
        public int GetDayOfCurrentDays(DateTime m_DataTimeAllocate, DateTime theTime)
        {
            int num = 0;
            DateTime firstDay = GetCurrentFirstDay(m_DataTimeAllocate);
            for (int i = 0; i < 7; i++)
            {
                if (firstDay.AddDays(i).ToString("yyyy-MM-dd") == theTime.ToString("yyyy-MM-dd"))
                {
                    num += i + 1;
                    break;
                }
            }
            return num;
        }


        /// <summary>
        /// 获取当前显示的第一天
        /// </summary>
        /// <param name="m_DataTimeAllocate">当前日期</param>
        /// <returns></returns>
        public static DateTime GetCurrentFirstDay(DateTime m_DataTimeAllocate)
        {
            DateTime starttime = DateTime.Parse(PatientInfo.InTime);
            int num = (int)Math.Floor((DateTime.Parse(m_DataTimeAllocate.ToString("yyyy-MM-dd 00:00:00")) - DateTime.Parse(starttime.ToString("yyyy-MM-dd 00:00:00"))).TotalDays / 7.00);
            if (num >= 1)
            {
                starttime = starttime.AddDays(num * 7);
            }
            return starttime;
        }

        private void InitEventInformation()
        {
            DataRow drEventInformation1 = DateTableEventInformation.NewRow();
            //drEventInformation1["DateTime"] = Convert.ToDateTime(PatientInfo.InTime).ToString("yyyy-MM-dd H:m:s");
            //drEventInformation1["TimePoint"] = "";
            //drEventInformation1["EventInformation"] = "入院-";
            //drEventInformation1["StatusID"] = "";//病人状态标识 
            //DateTableEventInformation.Rows.Add(drEventInformation1);

            //其他患者状态信息的测试数据
            //DataRow drEventInformation2 = DateTableEventInformation.NewRow();
            //drEventInformation2[0] = Convert.ToDateTime(PatientInfo.InTime).AddDays(2).ToString("yyyy-MM-dd H:m:s");
            //drEventInformation2[1] = "";
            //drEventInformation2[2] = "转入-";
            //DateTableEventInformation.Rows.Add(drEventInformation2);

            //DataRow drEventInformation3 = DateTableEventInformation.NewRow();
            //drEventInformation3[0] = Convert.ToDateTime(PatientInfo.InTime).AddDays(6).ToString("yyyy-MM-dd H:m:s");
            //drEventInformation3[1] = "";
            //drEventInformation3[2] = "手术-";
            //DateTableEventInformation.Rows.Add(drEventInformation3);
            //加上体征录入中填写的患者状态信息

            string patid = PatientInfo.InpatientNo;
            //新增取出状态的字段ID
            string sql = string.Format(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on  a.ccode=b.id and a.noofinpat='{0}' order by dotime ", patid);
            DataTable dt = new DataTable();
            dt = MethodSet.App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);

            //xll 2012-09-25 暂不修改
            //DataRow drEventAdmitWard = dt.NewRow();
            //drEventAdmitWard["dotime"] = Convert.ToDateTime(PatientInfo.InTime).ToString("yyyy-MM-dd H:m:s");
            //drEventAdmitWard["name"] = "入院";//入科改为入院
            //dt.Rows.InsertAt(drEventAdmitWard, 0);

            #region  对于日期一样且同一个小时的数据，取较早的数据状态显示【暂时作废】 Modified by wwj 2012-07-20
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow m_row = DateTableEventInformation.NewRow();
            //    //对于日期一样且同一个小时的数据，取较早的数据状态显示
            //    string comparedate = Convert.ToDateTime(dt.Rows[i]["dotime"]).ToString("yyyy-MM-dd H");
            //    int compareday = 0;//相差小时数
            //    for (int j = 0; j < DateTableEventInformation.Rows.Count; j++)
            //    {
            //       compareday = (Convert.ToDateTime(dt.Rows[i]["dotime"]) - Convert.ToDateTime(DateTableEventInformation.Rows[j]["datetime"])).Hours;//对于个别相差时间需要在一个单元竖格显示的也取时间靠前的数据
            //    }
            //    //int compareday = (Convert.ToDateTime(dt.Rows[i]["dotime"]) - Convert.ToDateTime(DateTableEventInformation.Rows[i]["datetime"])).Hours;//对于个别相差时间需要在一个单元竖格显示的也取时间靠前的数据
            //    DataRow[] selectro = DateTableEventInformation.Select("datetime like '%" + comparedate + "%'");
            //    if (selectro.Length <= 0 || compareday > 1)
            //    {
            //        m_row["DateTime"] = Convert.ToDateTime(dt.Rows[i]["dotime"]).ToString("yyyy-MM-dd H:m:s");
            //        m_row["TimePoint"] = "";
            //        m_row["EventInformation"] = dt.Rows[i]["name"].ToString() + "-";
            //        m_row["StatusID"] = dt.Rows[i]["id"].ToString();
            //        DateTableEventInformation.Rows.Add(m_row);
            //    }
            //}
            #endregion

            #region 相同时间（精确到分钟）的事件进行合并 Modified by wwj 2012-07-13

            List<int> skipIndexList = new List<int>();//跳过索引的列表

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!skipIndexList.Contains(i))
                {
                    DataRow m_row = DateTableEventInformation.NewRow();
                    m_row["DateTime"] = Convert.ToDateTime(dt.Rows[i]["dotime"]).ToString("yyyy-MM-dd H:m:s");
                    m_row["EventInformation"] = dt.Rows[i]["name"].ToString().Trim();

                    for (int j = i + 1; j < dt.Rows.Count; j++)
                    {
                        if (!skipIndexList.Contains(j))
                        {
                            DateTime dateTime = Convert.ToDateTime(dt.Rows[i]["dotime"]);
                            DateTime dateTimeTemp = Convert.ToDateTime(dt.Rows[j]["dotime"]);

                            if (dateTime.ToString("yyyy-MM-dd H:m") == dateTimeTemp.ToString("yyyy-MM-dd H:m")) //【精确到分钟】
                            {
                                m_row["EventInformation"] += dt.Rows[j]["name"].ToString().Trim();
                                skipIndexList.Add(j);
                            }
                        }
                    }

                    m_row["EventInformation"] += "-";
                    DateTableEventInformation.Rows.Add(m_row);
                }
            }

            #endregion
        }

        /// <summary>
        /// 根据入院时间得到病人入院的时间点  Modified by wwj 2012-06-12 修改事件对应时间点
        /// </summary>
        /// <param name="dataTableDayTimePoint"></param>
        /// <returns></returns>
        public string GetTimePointInTime(DataTable dataTableDayTimePoint, string hour)
        {
            //string timePoint = string.Empty;

            //foreach (DataRow dr in dataTableDayTimePoint.Rows)
            //{
            //    if (Convert.ToInt32(dr[0]) >= Convert.ToInt32(hour))
            //    {
            //        timePoint = dr[0].ToString();
            //        return timePoint;
            //    }
            //}

            string timePoint = string.Empty;
            int i = 0;
            foreach (DataRow dr in dataTableDayTimePoint.Rows)
            {
                i++;
                if (i == 1 && Convert.ToInt32(hour) <= Convert.ToInt32(dr[0]))
                {
                    //2  ----> 0 1 2
                    timePoint = dr[0].ToString();
                    return timePoint;
                }
                else if (i == dataTableDayTimePoint.Rows.Count && Convert.ToInt32(hour) >= Convert.ToInt32(dr[0]))
                {
                    //22 ----> 22 23 24
                    timePoint = dr[0].ToString();
                    return timePoint;
                }
                    //xll 2012-10-30 6点管控 5 6 7时间点
                else if ((hour == "5" || hour == "6" || hour == "7"))
                {
                    //6  ----> 5 6 7
                    timePoint ="6";
                    return timePoint;
                }
                //xll 2012-10-30 10点管控 8 9 10 11 12 时间点
                else if ((hour == "8" || hour == "9" || hour == "10" || hour == "11" || hour == "12"))
                {
                    //10 ----> 8 9 10 11 12
                    timePoint ="10";
                    return timePoint;
                }
                else if (Convert.ToInt32(hour) >= (Convert.ToInt32(dr[0]) - 1) && Convert.ToInt32(hour) <= (Convert.ToInt32(dr[0]) + 2))
                {
                    //2  ----> 3 4
                    //6  ----> 5 6 7 8
                    //10 ----> 9 10 11 12
                    //14 ----> 13 14 15 16
                    //18 ----> 17 18 19 20
                    //22 ----> 21
                    timePoint = dr[0].ToString();
                    return timePoint;
                }
            }

            timePoint = dataTableDayTimePoint.Rows[dataTableDayTimePoint.Rows.Count - 1][0].ToString();

            return timePoint;
        }

        /// <summary>
        /// 得到几时几分【24小时制】
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetHourAndMinute(DateTime time)
        {
            string hour = time.ToString("yyyy-H").Split('-')[1];
            string minute = time.ToString("yyyy-MM-dd-m").Split('-')[3];
            return CombineTime(hour, minute);
        }

        /// <summary>
        /// 得到几时几分【12小时制】Add by wwj 2012-06-12
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetHourAndMinute2(DateTime time)
        {
            string hour = time.ToString("yyyy-H").Split('-')[1];

            if (Convert.ToInt32(hour) > 12)
            {
                hour = (Convert.ToInt32(hour) - 12).ToString();
            }

            string minute = time.ToString("yyyy-MM-dd-m").Split('-')[3];
            return CombineTime(hour, minute);
        }

        private string CombineTime(string hour, string minute)
        {
            StringBuilder sb = new StringBuilder();

            if (hour.Length == 2)
            {
                sb.Append(GetChiness2(hour[0].ToString())).Append(GetChiness(hour[1].ToString())).Append("时");
            }
            else
            {
                if (hour == "0")//对整时的控制 add by ywk
                {
                    sb.Append("零时");
                }
                else
                {
                    sb.Append(GetChiness(hour[0].ToString())).Append("时");
                }
            }

            if (minute.Length == 2)
            {
                sb.Append(GetChiness2(minute[0].ToString())).Append(GetChiness(minute[1].ToString())).Append("分");
            }
            else
            {
                if (minute == "0")//对整分钟的控制add by ywk 
                {
                    sb.Append("整");
                }
                else
                {
                    sb.Append(GetChiness(minute[0].ToString())).Append("分");
                }
            }

            return sb.ToString();
        }

        private string GetChiness2(string value)
        {
            string returnValue = string.Empty;
            switch (value)
            {
                case "1": returnValue += "十"; break;
                case "2": returnValue += "二十"; break;
                case "3": returnValue += "三十"; break;
                case "4": returnValue += "四十"; break;
                case "5": returnValue += "五十"; break;
            }
            return returnValue;
        }

        private string GetChiness(string value)
        {
            string returnValue = string.Empty;
            switch (value)
            {
                //case "0": returnValue += "十"; break;
                case "1": returnValue += "一"; break;
                case "2": returnValue += "二"; break;
                case "3": returnValue += "三"; break;
                case "4": returnValue += "四"; break;
                case "5": returnValue += "五"; break;
                case "6": returnValue += "六"; break;
                case "7": returnValue += "七"; break;
                case "8": returnValue += "八"; break;
                case "9": returnValue += "九"; break;
            }
            return returnValue;
        }

        /// <summary>
        /// 初始化体温，脉搏，呼吸，物理降温 和 心率数据
        /// </summary>
        public void InitData()
        {
            DataTableTemperature = new DataTable();     //体温数据
            DataTableMaiBo = new DataTable();           //脉搏数据
            DataTableHuXi = new DataTable();            //呼吸数据
            DataTableWuLiJiangWen = new DataTable();    //物理降温数据

            //物理升温数据 add by ywk
            DataTableWuLiShengWen = new DataTable();    //物理升温数据

            DataTableXinLv = new DataTable();           //心率数据
            DateTableXueYa = new DataTable();           //血压数据
            DataTableZongRuLiang = new DataTable();     //总入量
            DataTableZongChuLiang = new DataTable();    //总出量
            DataTableYinLiuLiang = new DataTable();     //引流量
            DataTableDaBianCiShu = new DataTable();     //大便次数
            DataTableShenGao = new DataTable();         //身高
            DataTableTiZhong = new DataTable();         //体重
            DataTableGuoMinYaoWu = new DataTable();     //过敏药物
            DataTableTeShuZhiLiao = new DataTable();    //特殊治疗
            DataTablePainInfo = new DataTable();          //其他1 现在改为疼痛栏位 
            DataTableOther2 = new DataTable();          //其他2
            //DataTablePainInfo = new DataTable();          //其他2  
            DateTableEventInformation = new DataTable();
            DataTableDayOfHospital = new DataTable();
            DataTableOperationTime = new DataTable();

            #region 体温数据
            DataTableTemperature.Columns.Add("DateTime");
            DataTableTemperature.Columns.Add("TimePoint");
            DataTableTemperature.Columns.Add("TypeID"); //1: 口温  2: 腋温 3: 肛温
            DataTableTemperature.Columns.Add("Value");
            DataTableTemperature.Columns.Add("Memo");
            DataTableTemperature.Columns.Add("LinkNext");//是否与下一个点连接
            #endregion

            #region 脉搏数据
            DataTableMaiBo.Columns.Add("DateTime");
            DataTableMaiBo.Columns.Add("TimePoint");
            DataTableMaiBo.Columns.Add("Value");
            DataTableMaiBo.Columns.Add("Memo");
            DataTableMaiBo.Columns.Add("LinkNext");//是否与下一个点连接
            #endregion

            #region 呼吸数据
            DataTableHuXi.Columns.Add("DateTime");
            DataTableHuXi.Columns.Add("TimePoint");
            DataTableHuXi.Columns.Add("Value");
            DataTableHuXi.Columns.Add("Memo");
            DataTableHuXi.Columns.Add("LinkNext");//是否与下一个点连接
            DataTableHuXi.Columns.Add("IsSpecial");//Y:表示有用呼吸机
            #endregion

            #region 物理降温
            DataTableWuLiJiangWen.Columns.Add("DateTime");
            DataTableWuLiJiangWen.Columns.Add("TimePoint");
            DataTableWuLiJiangWen.Columns.Add("Value");
            DataTableWuLiJiangWen.Columns.Add("LinkNext");//是否与下一个点连接
            #endregion

            #region 新增的物理升温
            DataTableWuLiShengWen.Columns.Add("DateTime");
            DataTableWuLiShengWen.Columns.Add("TimePoint");
            DataTableWuLiShengWen.Columns.Add("Value");
            DataTableWuLiShengWen.Columns.Add("LinkNext");//是否与下一个点连接
            #endregion

            #region 心率数据
            DataTableXinLv.Columns.Add("DateTime");
            DataTableXinLv.Columns.Add("TimePoint");
            DataTableXinLv.Columns.Add("Value");
            DataTableXinLv.Columns.Add("Memo");
            DataTableXinLv.Columns.Add("LinkNext");//是否与下一个点连接
            DataTableXinLv.Columns.Add("IsSpecial");//Y :表示有用“起搏器”
            #endregion

            #region 血压数据
            DateTableXueYa.Columns.Add("DateTime");
            DateTableXueYa.Columns.Add("TimePoint");
            DateTableXueYa.Columns.Add("Value");
            #endregion

            #region 总入量数据
            DataTableZongRuLiang.Columns.Add("DateTime");
            DataTableZongRuLiang.Columns.Add("TimePoint");
            DataTableZongRuLiang.Columns.Add("Value");
            #endregion

            #region 总出量数据
            DataTableZongChuLiang.Columns.Add("DateTime");
            DataTableZongChuLiang.Columns.Add("TimePoint");
            DataTableZongChuLiang.Columns.Add("Value");
            #endregion

            #region 疼痛数据  （新增的  ywk ）
            DataTablePainInfo.Columns.Add("DateTime");
            DataTablePainInfo.Columns.Add("TimePoint");
            DataTablePainInfo.Columns.Add("Value");
            //DataTableHuXi.Columns.Add("LinkNext");//是否与下一个点连接
            //DataTableHuXi.Columns.Add("IsSpecial");//Y:表示有用
            #endregion

            #region 其他2徐亮亮
            DataTableOther2.Columns.Add("DateTime");
            DataTableOther2.Columns.Add("TimePoint");
            DataTableOther2.Columns.Add("Value");
           
            #endregion


            #region 引流量数据
            DataTableYinLiuLiang.Columns.Add("DateTime");
            DataTableYinLiuLiang.Columns.Add("TimePoint");
            DataTableYinLiuLiang.Columns.Add("Value");
            #endregion

            #region 大便次数数据
            DataTableDaBianCiShu.Columns.Add("DateTime");
            DataTableDaBianCiShu.Columns.Add("TimePoint");
            DataTableDaBianCiShu.Columns.Add("Value");
            #endregion

            #region 身高数据
            DataTableShenGao.Columns.Add("DateTime");
            DataTableShenGao.Columns.Add("TimePoint");
            DataTableShenGao.Columns.Add("Value");
            #endregion

            #region 体重数据
            DataTableTiZhong.Columns.Add("DateTime");
            DataTableTiZhong.Columns.Add("TimePoint");
            DataTableTiZhong.Columns.Add("Value");
            #endregion

            #region 过敏药物数据
            DataTableGuoMinYaoWu.Columns.Add("DateTime");
            DataTableGuoMinYaoWu.Columns.Add("TimePoint");
            DataTableGuoMinYaoWu.Columns.Add("Value");
            #endregion

            #region 特殊治疗数据
            DataTableTeShuZhiLiao.Columns.Add("DateTime");
            DataTableTeShuZhiLiao.Columns.Add("TimePoint");
            DataTableTeShuZhiLiao.Columns.Add("Value");
            #endregion

            #region 住院天数
            DataTableDayOfHospital.Columns.Add("DateTime");
            DataTableDayOfHospital.Columns.Add("TimePoint");
            DataTableDayOfHospital.Columns.Add("Value");
            #endregion

            #region 术后天数
            DataTableOperationTime.Columns.Add("DateTime");
            DataTableOperationTime.Columns.Add("TimePoint");
            DataTableOperationTime.Columns.Add("Value");
            #endregion

            #region 初始化 患者入院、转入、手术、分娩、出院、死亡等,除手术不写具体时间外，其余均按24小时制，精确到分钟
            DateTableEventInformation.Columns.Add("DateTime");
            DateTableEventInformation.Columns.Add("TimePoint");
            DateTableEventInformation.Columns.Add("EventInformation");
            DateTableEventInformation.Columns.Add("StatusID");

            //DataRow drEventInformation1 = DateTableEventInformation.NewRow();
            //drEventInformation1[0] = Convert.ToDateTime(PatientInfo.InTime).ToString("yyyy-MM-dd");
            //drEventInformation1[1] = "10";
            //drEventInformation1[2] = "入院-九时四十分";
            //DateTableEventInformation.Rows.Add(drEventInformation1);

            //drEventInformation1 = DateTableEventInformation.NewRow();
            //drEventInformation1[0] = Convert.ToDateTime(PatientInfo.InTime).AddDays(2).ToString("yyyy-MM-dd");
            //drEventInformation1[1] = "6";
            //drEventInformation1[2] = "转入-五时十分";
            //DateTableEventInformation.Rows.Add(drEventInformation1);

            //drEventInformation1 = DateTableEventInformation.NewRow();
            //drEventInformation1[0] = Convert.ToDateTime(PatientInfo.InTime).AddDays(6).ToString("yyyy-MM-dd");
            //drEventInformation1[1] = "22";
            //drEventInformation1[2] = "手术";
            //DateTableEventInformation.Rows.Add(drEventInformation1);
            #endregion

        }

        /// <summary>
        /// 清空所有数据
        /// </summary>
        public void ClearDataTable()
        {
            DataTableTemperature.Rows.Clear();     //体温数据
            DataTableMaiBo.Rows.Clear();           //脉搏数据
            DataTableHuXi.Rows.Clear();            //呼吸数据
            DataTableWuLiJiangWen.Rows.Clear();    //物理降温数据

            DataTableWuLiShengWen.Rows.Clear();//新增的物理升温数据

            DataTableXinLv.Rows.Clear();           //心率数据
            DateTableXueYa.Rows.Clear();           //血压数据
            DataTableZongRuLiang.Rows.Clear();     //总入量
            DataTableZongChuLiang.Rows.Clear();    //总出量
            DataTableYinLiuLiang.Rows.Clear();     //引流量
            DataTableDaBianCiShu.Rows.Clear();     //大便次数
            DataTableShenGao.Rows.Clear();         //身高
            DataTableTiZhong.Rows.Clear();         //体重
            DataTableGuoMinYaoWu.Rows.Clear();     //过敏药物
            DataTableTeShuZhiLiao.Rows.Clear();    //特殊治疗
            DataTablePainInfo.Rows.Clear();          //其他1
            DataTableOther2.Rows.Clear();          //其他2

            //DataTablePainInfo.Rows.Clear();          //其他2 现在改为疼痛  ywk 

            DateTableEventInformation.Rows.Clear();
            DataTableDayOfHospital.Rows.Clear();
            DataTableOperationTime.Rows.Clear();
        }
    }

    /// <summary>
    /// 与病人有关的信息
    /// </summary>
    class PatientInfoLocation
    {
        //病人信息的纵坐标
        public int InpatientInformationY = 100;//100

        //病人信息的横坐标
        public int InpatientNameX = 60 - 40;

        public int AgeX = 150 - 40;
        public int GenderX = 230 - 40;
        public int SectionX = 300 - 40;
        public int BedCodeX = 410 - 40;
        public int InTimeX = 490 - 40;
        public int InpatientNoX = 630 - 40;
    }
}
