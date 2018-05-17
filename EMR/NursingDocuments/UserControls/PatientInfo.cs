using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.Core.NursingDocuments.PublicSet;
using DrectSoft.Service;

namespace DrectSoft.Core.NursingDocuments.UserControls
{
    class PatientInfo
    {
        #region "全局变量"
        public string InpatientName = string.Empty;   //姓名
        public string Age = string.Empty;             //年龄
        public string Gender = string.Empty;          //性别
        public string Section = string.Empty;        //科别
        public string DeptID = string.Empty;         //科室编号 add by wyt 2012年8月15日
        public string BedCode = string.Empty;         //病床号
        public string InTime = DateTime.Now.ToString("yyyy-MM-dd");  //原为“入院日期”，现改为“入科日期”
        public string OutTime = string.Empty;         //出院日期
        public string InpatientNo = string.Empty;     //住院号
        public string NoOfInpat = string.Empty;       //首页序号
        public string IsBaby = string.Empty;//是否是婴儿 add by ywk 2012年7月30日 13:39:31
        public string BirthDay = string.Empty;//婴儿出生时间 add by wyt 2012年8月15日
        public string Mother = string.Empty;//母亲的Noofinpat号

        public DataTable DataTableTemperature = new DataTable();     //体温数据
        public DataTable DataTableMaiBo = new DataTable();           //脉搏数据
        public DataTable DataTableHuXi = new DataTable();            //呼吸数据
        public DataTable DataTableWuLiJiangWen = new DataTable();    //物理降温数据
        public DataTable DataTableXinLv = new DataTable();           //心率数据
        public DataTable DateTableXueYa = new DataTable();           //血压数据
        public DataTable DataTableZongRuLiang = new DataTable();     //总入量
        public DataTable DataTableZongChuLiang = new DataTable();    //总出量
        public DataTable DataTableYinLiuLiang = new DataTable();     //引流量
        public DataTable DataTableDaBianCiShu = new DataTable();     //大便次数
        public DataTable DataTableShenGao = new DataTable();         //身高
        public DataTable DataTableTiZhong = new DataTable();         //体重
        public DataTable DataTableGuoMinYaoWu = new DataTable();     //过敏药物
        public DataTable DataTableTeShuZhiLiao = new DataTable();    //特殊治疗
        public DataTable DataTablePainInfo = new DataTable();          //其他1 现在改为疼痛栏位 ywk
        public DataTable DataTableOther2 = new DataTable();          //其他2
        //public static DataTable DataTablePainInfo = new DataTable();          //其他2 
        //add by wyt
        public DataTable DataTableParam1 = new DataTable();          //自定义参数1
        public DataTable DataTableParam2 = new DataTable();          //自定义参数2

        public DataTable DataTableDayOfHospital = new DataTable(); //住院天数
        public DataTable DataTableOperationTime = new DataTable(); //手术信息

        public DataTable DateTableEventInformation = new DataTable(); //患者入院、转入、手术、分娩、出院、死亡等

        //新增物理升温-----------add by  ywk
        public DataTable DataTableWuLiShengWen = new DataTable();    //物理升温数据

        public DataTable DataTableDayTimePoint = new DataTable();    //每天的时间点
        //public static DataTable DataTableDayIndx = new DataTable();    //每天的时间点
        #endregion

        /// <summary>
        /// 清空病人的基本信息
        /// </summary>
        public void ClearPatientInof()
        {
            InpatientName = string.Empty;
            Age = string.Empty;
            Gender = string.Empty;
            Section = string.Empty;
            DeptID = string.Empty;
            BirthDay = string.Empty;
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
                DeptID = dataTablePatientInfo.Rows[0]["dept_id"].ToString();//string.Empty;/科别代码
                BedCode = dataTablePatientInfo.Rows[0]["outbed"].ToString();//string.Empty;床位号

                //InTime = dataTablePatientInfo.Rows[0]["AdmitDate"].ToString();             //入院日期
                InTime = dataTablePatientInfo.Rows[0]["inwarddate"].ToString();             //入科日期
                OutTime = dataTablePatientInfo.Rows[0]["OutHosDate"].ToString();           //出院日期
                InpatientNo = dataTablePatientInfo.Rows[0]["PatID"].ToString();            //住院号
                NoOfInpat = dataTablePatientInfo.Rows[0]["NoOfInpat"].ToString();          //首页号
                IsBaby = dataTablePatientInfo.Rows[0]["IsBaby"].ToString();//新增的是否婴儿标志 add by ywk 
                Mother = dataTablePatientInfo.Rows[0]["Mother"].ToString();//新增的母亲的Noofinpat add by ywk
                BirthDay = dataTablePatientInfo.Rows[0]["BirthDay"].ToString();//婴儿出生日期
            }
        }

        /// <summary>
        /// 得到病人所有生命体征的数据
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public void GetNursingRecordByDate(string beginDate, string endDate, DateTime m_DataTimeAllocate, PatientInfo patientInfo)
        {
            ClearDataTable();

            //是否使用呼吸机的配置,原来的直接写死为N了add by ywk 2012年11月21日11:32:01
            string Special = PublicSet.MethodSet.GetConfigValueByKey("IsUseHuxiMachine");

            DataTable dataTableAll = PublicSet.MethodSet.GetNursingRecordByDate(NoOfInpat, beginDate, endDate);

            #region 处理DataTable
            //只筛出跟当前系统里要测量的时间点的数据 
            //add by ywk 2012年5月21日 10:59:12 
            string m_times = PublicSet.MethodSet.GetConfigValueByKey("VITALSIGNSRECORDTIME");
            string[] timeArray = m_times.Split(',');
            DataRow[] SpliterRows = new DataRow[] { };
            SpliterRows = dataTableAll.DefaultView.Table.Select("timeslot in ('" + timeArray[0] + "','" + timeArray[1] + "','" + timeArray[2] + "','" + timeArray[3] + "','" + timeArray[4] + "','" + timeArray[5] + "')", "id asc");
            DataTable dataNewTable = dataTableAll.Clone();//先按ID进行升序排序
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
            DataRow[] m_Rows = dataNewTable.Select("1=1", "dateofsurvey ASC");//在按日期进行升序排序
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
                    //dataRow["Indx"] = dr["Indx"].ToString();
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
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["Pulse"].ToString();
                    dataRow["LinkNext"] = "Y";
                    DataTableMaiBo.Rows.Add(dataRow);
                }
                if (dr["Breathe"].ToString() != "")//呼吸
                {
                    DataRow dataRow = DataTableHuXi.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["Breathe"].ToString().ToUpper();
                    dataRow["LinkNext"] = "Y";
                    //dataRow["IsSpecial"] = "N";
                    //dataRow["IsSpecial"] = Special;
                    dataRow["IsSpecial"] = dr["Breathe"].ToString().Trim().ToUpper() == "R" ? "Y" : "N";
                    DataTableHuXi.Rows.Add(dataRow);
                }
                if (dr["PhysicalCooling"].ToString() != "")//物理降温
                {
                    DataRow dataRow = DataTableWuLiJiangWen.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
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
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["PhysicalHotting"].ToString();
                    dataRow["LinkNext"] = "Y";
                    DataTableWuLiShengWen.Rows.Add(dataRow);
                }

                if (dr["HeartRate"].ToString() != "")//心率
                {
                    DataRow dataRow = DataTableXinLv.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
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
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["BloodPressure"].ToString();
                    DateTableXueYa.Rows.Add(dataRow);
                }
                if (dr["CountIn"].ToString() != "")//总入量
                {
                    DataRow dataRow = DataTableZongRuLiang.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["CountIn"].ToString();
                    DataTableZongRuLiang.Rows.Add(dataRow);
                }
                if (dr["CountOut"].ToString() != "")//总出量
                {
                    DataRow dataRow = DataTableZongChuLiang.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["CountOut"].ToString();
                    DataTableZongChuLiang.Rows.Add(dataRow);
                }
                if (dr["Drainage"].ToString() != "")//引流量
                {
                    DataRow dataRow = DataTableYinLiuLiang.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["Drainage"].ToString();
                    DataTableYinLiuLiang.Rows.Add(dataRow);
                }
                if (dr["TimeOfShit"].ToString() != "")//大便次数
                {
                    DataRow dataRow = DataTableDaBianCiShu.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["TimeOfShit"].ToString();
                    DataTableDaBianCiShu.Rows.Add(dataRow);
                }
                if (dr["Height"].ToString() != "")//身高
                {
                    DataRow dataRow = DataTableShenGao.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["Height"].ToString();
                    DataTableShenGao.Rows.Add(dataRow);
                }
                if (dr["Weight"].ToString() != "")//体重
                {
                    DataRow dataRow = DataTableTiZhong.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["Weight"].ToString();
                    DataTableTiZhong.Rows.Add(dataRow);
                }
                if (dr["Param1"].ToString() != "")//自定义参数1
                {
                    DataRow dataRow = DataTableParam1.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["Param1"].ToString();
                    DataTableParam1.Rows.Add(dataRow);
                }
                if (dr["Param2"].ToString() != "")//自定义参数2
                {
                    DataRow dataRow = DataTableParam2.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["Param2"].ToString();
                    DataTableParam2.Rows.Add(dataRow);
                }
                if (dr["Allergy"].ToString() != "")//过敏药物
                {
                    DataRow dataRow = DataTableGuoMinYaoWu.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["Allergy"].ToString();
                    DataTableGuoMinYaoWu.Rows.Add(dataRow);
                }
                if (dr["DayOfHospital"].ToString() != "")//住院天数
                {
                    DataRow dataRow = DataTableDayOfHospital.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["DayOfHospital"].ToString();
                    DataTableDayOfHospital.Rows.Add(dataRow);
                }
                if (dr["DaysAfterSurgery"].ToString() != "")//手术后天数
                {
                    DataRow dataRow = DataTableOperationTime.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["DaysAfterSurgery"].ToString();
                    DataTableOperationTime.Rows.Add(dataRow);
                }
                //新增的疼痛栏位
                if (dr["PainInfo"].ToString() != "")//疼痛
                {
                    DataRow dataRow = DataTablePainInfo.NewRow();
                    dataRow["DateTime"] = date;
                    dataRow["TimePoint"] = timeSlot;
                    //dataRow["Indx"] = dr["Indx"].ToString();
                    dataRow["Value"] = dr["PainInfo"].ToString();
                    //dataRow["LinkNext"] = "Y";
                    //dataRow["IsSpecial"] = "N";
                    DataTablePainInfo.Rows.Add(dataRow);
                }
            }
            #endregion

            //三测单患者状态显示方式(按配置文件) by cyq 2012-09-07
            string nursingMeasureStateFlag = MethodSet.GetConfigValueByKey("NursingMeasureStateFlag");
            if (nursingMeasureStateFlag == "0")
            {
                //默认
                InitEventInformation();
            }
            else if (nursingMeasureStateFlag == "1" || nursingMeasureStateFlag == "2")
            {
                //因仁和医院入院状态ID为7009，其它医院入院ID为7008，此处特殊处理。
                //仁和医院和其它与仁和医院对患者状态有相同需求的医院
                InitEventInformation2(m_DataTimeAllocate, patientInfo);
            }
            #endregion
        }

        private void InitEventInformation()
        {
            DataRow drEventInformation1 = DateTableEventInformation.NewRow();

            #region 已注释
            //drEventInformation1["DateTime"] = Convert.ToDateTime(PatientInfo.InTime).ToString("yyyy-MM-dd H:m:s");
            //drEventInformation1["TimePoint"] = "";
            //drEventInformation1["EventInformation"] = "入科-";
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
            #endregion

            ////add by cyq 2013-03-05 获取本次入院时间范围(多次入院记录的情况--->上次入院时间至下次入院时间)
            //DataTable inHosDetails = DS_SqlService.GetInHosDetails(int.Parse(NoOfInpat));
            //if (null == inHosDetails || inHosDetails.Rows.Count == 0)
            //{
            //    return;
            //}
            //DataRow thisInpatient = inHosDetails.Select(" NOOFINPAT = " + NoOfInpat).FirstOrDefault();
            //string startTime = string.IsNullOrEmpty(thisInpatient["inwarddate"].ToString()) ? thisInpatient["admitdate"].ToString() : DateTime.Parse(thisInpatient["inwarddate"].ToString()).ToString("yyyy-MM-dd 00:00");
            //string endTime = OutTime;
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


            //string patid = InpatientNo;
            ////新增取出事件的状态ID用于操作DT这个表 edit by ywk 2012年1月8日10:33:16
            //string sql = string.Format(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on a.ccode=b.id and a.noofinpat='{0}' and a.dotime >='{1}' and a.dotime <= '{2}' order by dotime ", patid, startTime, endTime);
            string sql = string.Format(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on a.ccode=b.id and a.noofinpat='{0}'   order by dotime ", NoOfInpat);
            DataTable dt = new DataTable();
            dt = MethodSet.App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            //处理此处的dt，将状态ID为7009（入科的）


            //wyt
            //DataRow drEventAdmitWard = dt.NewRow();
            //drEventAdmitWard["dotime"] = Convert.ToDateTime(PatientInfo.InTime).ToString("yyyy-MM-dd H:m:s");
            //drEventAdmitWard["name"] = "入科";
            //dt.Rows.InsertAt(drEventAdmitWard, 0);

            #region  对于日期一样且同一个小时的数据，取较早的数据状态显示【暂时作废】 Modified by wwj 2012-07-13
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow m_row = DateTableEventInformation.NewRow();
            //    //对于日期一样且同一个小时的数据，取较早的数据状态显示
            //    string comparedate = Convert.ToDateTime(dt.Rows[i]["dotime"]).ToString("yyyy-MM-dd H");
            //    int compareday = 0;//相差小时数
            //    for (int j = 0; j < DateTableEventInformation.Rows.Count; j++)
            //    {
            //        compareday = (Convert.ToDateTime(dt.Rows[i]["dotime"]) - Convert.ToDateTime(DateTableEventInformation.Rows[j]["datetime"])).Hours;//对于个别相差时间需要在一个单元竖格显示的也取时间靠前的数据
            //    }
            //    //int compareday = (Convert.ToDateTime(dt.Rows[i]["dotime"]) - Convert.ToDateTime(DateTableEventInformation.Rows[i]["datetime"])).Hours;//对于个别相差时间需要在一个单元竖格显示的也取时间靠前的数据
            //    DataRow[] selectro = DateTableEventInformation.Select("datetime like '%" + comparedate + "%'");
            //    if (selectro.Length <= 0 || compareday > 1)
            //    {
            //        m_row["DateTime"] = Convert.ToDateTime(dt.Rows[i]["dotime"]).ToString("yyyy-MM-dd H:m:s");
            //        m_row["TimePoint"] = "";
            //        m_row["EventInformation"] = dt.Rows[i]["name"].ToString() + "-";
            //        DateTableEventInformation.Rows.Add(m_row);
            //    }
            //}
            #endregion

            #region 相同时间（精确到分钟）的事件进行合并 Modified by wwj 2012-07-13

            List<int> skipIndexList = new List<int>();//跳过索引的列表
            string m_Info = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!skipIndexList.Contains(i))
                {
                    DataRow m_row = DateTableEventInformation.NewRow();
                    m_row["DateTime"] = Convert.ToDateTime(dt.Rows[i]["dotime"]).ToString("yyyy-MM-dd H:m:s");
                    m_row["EventInformation"] = dt.Rows[i]["name"].ToString().Trim();
                    m_row["DoTime"] = m_row["DateTime"];
                    for (int j = i + 1; j < dt.Rows.Count; j++)
                    {
                        if (!skipIndexList.Contains(j))
                        {
                            DateTime dateTime = Convert.ToDateTime(dt.Rows[i]["dotime"]);
                            DateTime dateTimeTemp = Convert.ToDateTime(dt.Rows[j]["dotime"]);

                            if (dateTime.ToString("yyyy-MM-dd H:m") == dateTimeTemp.ToString("yyyy-MM-dd H:m")) //【精确到分钟】
                            {
                                m_row["EventInformation"] += dt.Rows[j]["name"].ToString().Trim();
                                if (m_row["EventInformation"].ToString().Contains("入科"))//如果包含入科的记录
                                {
                                    m_Info = m_row["EventInformation"].ToString();
                                    if (m_Info.Length > 2)
                                    {
                                        m_Info = m_Info.Replace("入科", "").Insert(0, "入科");//入科放首位edit by ywk 2012年1月8日 11:17:14
                                        m_row["EventInformation"] = m_Info;
                                        if (m_Info.Length == 4)//只有两个状态中间加"."
                                        {
                                            m_Info = m_Info.Insert(2, "。");
                                            m_row["EventInformation"] = m_Info;
                                        }
                                    }
                                }
                                skipIndexList.Add(j);
                            }
                        }
                    }
                    //m_row["EventInformation"] = m_Info;
                    m_row["EventInformation"] += "--";//--
                    DateTableEventInformation.Rows.Add(m_row);
                }
            }

            #endregion
        }

        /// <summary>
        /// 优先级按顺序
        /// 1、同一段位内包含 '入院'和'出院' 将'出院'移至下一个段位
        /// 2、精确至同一分钟内的多个状态合并
        /// 3、同一段位(不属于同一分钟)内多个状态，则依次順移，每个段位一个状态
        /// 
        /// 修改时间：2012-08-15 10:50
        /// 修改人：cyq
        /// </summary>
        private void InitEventInformation2(DateTime m_DataTimeAllocate, PatientInfo patientInfo)
        {
            DataRow drEventInformation1 = DateTableEventInformation.NewRow();
            string patid = InpatientNo;
            DataTable timePoints = PublicSet.MethodSet.GetTimePoint();
            DateTime endTime = GetCurrentEndDay(m_DataTimeAllocate);

            //获取当前显示7天的前一天应该后移至当前7天的状态
            DataTable toNextDayStates = GetStatusPushedToNextDay(m_DataTimeAllocate, patientInfo);
            #region "原来取所有状态的方法 --- 已弃用"
            //新增取出事件的状态ID用于操作DT这个表 edit by ywk 2012年1月8日10:33:16
            //string sql = string.Format(@"select a.dotime,b.name,b.id from PatientStatus a join THERE_CHECK_EVENT b on  a.ccode=b.id and a.noofinpat='{0}' order by dotime ", patid);
            //DataTable dt = new DataTable();
            //dt = MethodSet.App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            #endregion
            //获取当前七天的状态(同一分钟内合并)
            DataTable dtCurrent = GetStatusCombined(GetStatusTable(timePoints, m_DataTimeAllocate, patientInfo));
            List<string> dateCurrentList = dtCurrent.Rows.Count == 0 ? new List<string>() : dtCurrent.AsEnumerable().Select(p => DateTime.Parse(p["DateTime"].ToString()).ToString("yyyy-MM-dd")).Distinct().ToList();
            //将当前七天的状态順移排好
            DataTable dtCurrentPushed = AllPushedDataTable(dtCurrent, timePoints, dateCurrentList, endTime, true);
            //将排好的两组状态合并成同一DataTable
            DataTable dt = CombineDatatable(CombineDatatable(NewTheDataTable(), toNextDayStates), dtCurrentPushed);
            List<string> dateList = dt.Rows.Count == 0 ? new List<string>() : dt.AsEnumerable().Select(p => DateTime.Parse(p["DateTime"].ToString()).ToString("yyyy-MM-dd")).Distinct().ToList();
            //将合并后的状态再順移排好
            DateTableEventInformation = AllPushedDataTable(dt, timePoints, dateList, endTime, false);
            #region "相同时间（精确到分钟）的事件进行合并 Modified by wwj 2012-07-13 --- 已弃用"
            //List<int> skipIndexList = new List<int>();//跳过索引的列表
            //string m_Info = string.Empty;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (!skipIndexList.Contains(i))
            //    {
            //        DataRow m_row = DateTableEventInformation.NewRow();
            //        m_row["DateTime"] = Convert.ToDateTime(dt.Rows[i]["dotime"]).ToString("yyyy-MM-dd H:m:s");
            //        m_row["EventInformation"] = dt.Rows[i]["name"].ToString().Trim();

            //        for (int j = i + 1; j < dt.Rows.Count; j++)
            //        {
            //            if (!skipIndexList.Contains(j))
            //            {
            //                DateTime dateTime = Convert.ToDateTime(dt.Rows[i]["dotime"]);
            //                DateTime dateTimeTemp = Convert.ToDateTime(dt.Rows[j]["dotime"]);

            //                if (dateTime.ToString("yyyy-MM-dd H:m") == dateTimeTemp.ToString("yyyy-MM-dd H:m")) //【精确到分钟】
            //                {
            //                    m_row["EventInformation"] += dt.Rows[j]["name"].ToString().Trim();
            //                    if (m_row["EventInformation"].ToString().Contains("入科"))//如果包含入科的记录
            //                    {
            //                        m_Info = m_row["EventInformation"].ToString();
            //                        if (m_Info.Length > 2)
            //                        {
            //                            m_Info = m_Info.Replace("入科", "").Insert(0, "入科");//入科放首位edit by ywk 2012年1月8日 11:17:14
            //                            m_row["EventInformation"] = m_Info;
            //                            if (m_Info.Length == 4)//只有两个状态中间加"."
            //                            {
            //                                m_Info = m_Info.Insert(2, "。");
            //                                m_row["EventInformation"] = m_Info;
            //                            }
            //                        }
            //                    }
            //                    skipIndexList.Add(j);
            //                }
            //            }
            //        }
            //        //m_row["EventInformation"] = m_Info;
            //        m_row["EventInformation"] += "--";//--
            //        DateTableEventInformation.Rows.Add(m_row);
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// 根据入院时间得到病人入院的时间点序列  Modified by wyt 2012-08-13 
        /// </summary>
        /// <param name="dataTableDayTimePoint"></param>
        /// <returns></returns>
        public int GetIndxInTime(DataTable dataTableDayTimePoint, string hour)
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
                    return i - 1;
                }
                else if (i == dataTableDayTimePoint.Rows.Count && Convert.ToInt32(hour) >= Convert.ToInt32(dr[0]))
                {
                    //22 ----> 22 23 24
                    timePoint = dr[0].ToString();
                    return i - 1;
                }

                if (Convert.ToInt32(hour) >= (Convert.ToInt32(dr[0]) - 1) && Convert.ToInt32(hour) <= (Convert.ToInt32(dr[0]) + 2))
                {
                    //2  ----> 3 4
                    //6  ----> 5 6 7 8
                    //10 ----> 9 10 11 12
                    //14 ----> 13 14 15 16
                    //18 ----> 17 18 19 20
                    //22 ----> 21
                    timePoint = dr[0].ToString();
                    return i - 1;
                }
            }

            timePoint = dataTableDayTimePoint.Rows[dataTableDayTimePoint.Rows.Count - 1][0].ToString();

            return dataTableDayTimePoint.Rows.Count - 1;
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

                if (Convert.ToInt32(hour) >= (Convert.ToInt32(dr[0]) - 1) && Convert.ToInt32(hour) <= (Convert.ToInt32(dr[0]) + 2))
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
        /// 根据入院时间得到病人入院的时间点
        /// 中心医院需求
        /// 
        /// ---上午---------------------
        /// 2包括以0、1、2、3、4开头的时间，即凌晨0:00—凌晨4:59；
        /// 6包括以5、6、7开头的时间，即凌晨5:00—凌晨7:59；
        /// 10包括以8、9、10、11、12开头的时间，即凌晨8:00—凌晨12:59；
        /// ---下午---------------------
        /// 2包括以13、14、15、16开头的时间，即凌晨13:00—凌晨16:59；
        /// 6包括以17、18、19开头的时间，即凌晨17:00—凌晨19:59；
        /// 10包括以20、21、22、23开头的时间，即凌晨20:00—凌晨23:59；
        /// 
        /// edit by cyq 2012-11-13 时间段调整
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-15</date>
        /// </summary>
        /// <param name="dataTableDayTimePoint">时间段集合</param>
        /// <param name="hour">当前时间的小时</param>
        /// <returns></returns>
        public string GetTimePointInTime_zx(DataTable dataTableDayTimePoint, string hour)
        {
            try
            {
                int i = 0;
                foreach (DataRow dr in dataTableDayTimePoint.Rows)
                {
                    i++;
                    if (i == 1 && Convert.ToInt32(hour) <= Convert.ToInt32(dr[0]) + 2)
                    {
                        //2 ---> 0 1 2 3 4
                        return dr[0].ToString();
                    }
                    else if ((i == 2 || i == 5) && Convert.ToInt32(hour) >= (Convert.ToInt32(dr[0]) - 1) && Convert.ToInt32(hour) <= (Convert.ToInt32(dr[0]) + 1))
                    {
                        //6 ---> 5、6、7
                        //18 ---> 17、18、19
                        return dr[0].ToString();
                    }
                    else if (i == 3 && Convert.ToInt32(hour) >= (Convert.ToInt32(dr[0]) - 2) && Convert.ToInt32(hour) <= (Convert.ToInt32(dr[0]) + 2))
                    {
                        //10 ---> 8、9、10、11、12
                        return dr[0].ToString();
                    }
                    else if (i == 4 && Convert.ToInt32(hour) >= (Convert.ToInt32(dr[0])) - 1 && Convert.ToInt32(hour) <= (Convert.ToInt32(dr[0]) + 2))
                    {
                        //14 ----> 13 14 15 16
                        return dr[0].ToString();
                    }
                    else if (i == 6 && Convert.ToInt32(hour) >= (Convert.ToInt32(dr[0])) - 2 && Convert.ToInt32(hour) <= (Convert.ToInt32(dr[0]) + 1))
                    {
                        //22 ----> 20 21 22 23
                        return dr[0].ToString();
                    }
                }

                return dataTableDayTimePoint.Rows[dataTableDayTimePoint.Rows.Count - 1][0].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region "关于时间及段位的方法 by cyq"
        /// <summary>
        /// 将后一个DataTable行添加至前一个DataTable
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public DataTable CombineDatatable(DataTable dt1, DataTable dt2)
        {
            foreach (DataRow dr in dt2.Rows)
            {
                DataRow dRow = dt1.NewRow();
                for (int i = 0; i < dt1.Columns.Count; i++)
                {
                    dRow[i] = dr[i];
                }
                dt1.Rows.Add(dRow);
            }
            return dt1;
        }

        /// <summary>
        /// 获取段位順移之后的DataTable
        /// </summary>
        /// <param name="dt">原DataTable</param>
        /// <param name="timePoints">每天显示的段位</param>
        /// <param name="dateList">需要循环判断的日期List(格式：yyyy-MM-dd)</param>
        /// <param name="timePoints">当前显示的最后一天</param>
        /// <param name="timePoints">是否将出院放置在入院之后</param>
        /// <returns></returns>
        public DataTable AllPushedDataTable(DataTable dt, DataTable timePoints, List<string> dateList, DateTime endday, bool flag)
        {
            //入院状态特殊处理
            string inpState = GetInpStateByConfig();
            if (dt.Rows.Count == 0 || dateList.Count() == 0)
            {
                return dt;
            }
            foreach (string day in dateList)
            {
                dateList = dateList.Where(p => p != day).ToList();
                //获取这一天的所有状态
                var thisDayStatus = dt.AsEnumerable().Where(p => p["DateTime"].ToString().Contains(day));
                if (thisDayStatus.Count() > 0)
                {
                    List<string> timePointList = new List<string>();
                    for (int i = 0; i < timePoints.Rows.Count; i++)
                    {
                        //获取同一段位的状态
                        var currentStatus = thisDayStatus.Where(p => p["TimePoint"].ToString() == timePoints.Rows[i]["TimePoint"].ToString());
                        if (currentStatus.Count() > 1)
                        {//7003-出院；7008-入院；
                            DataRow firstRow = currentStatus.FirstOrDefault();
                            string leftID = string.Empty;
                            if (flag == true && (firstRow["ID"].ToString() == "7003" || firstRow["ID"].ToString() == inpState))
                            {
                                leftID = currentStatus.Any(p => p["ID"].ToString().Contains(inpState)) ? currentStatus.FirstOrDefault(p => p["ID"].ToString().Contains(inpState))["GUID"].ToString() : currentStatus.FirstOrDefault(p => p["ID"].ToString().Contains("7003"))["GUID"].ToString();
                            }
                            else
                            {
                                leftID = firstRow["GUID"].ToString();
                            }
                            //将这一天这一段位内除固定项外的所有状态后移一个段位
                            foreach (DataRow item in currentStatus.Where(p => p["GUID"].ToString() != leftID))
                            {
                                for (int j = dt.Rows.Count - 1; j >= 0; j--)
                                {
                                    if (dt.Rows[j]["GUID"] == item["GUID"])
                                    {
                                        if (i != timePoints.Rows.Count - 1)
                                        {
                                            dt.Rows[j]["DateTime"] = GetNextPointDatetime(DateTime.Parse(dt.Rows[j]["DateTime"].ToString()), 1).ToString("yyyy-MM-dd HH:mm");
                                            dt.Rows[j]["TimePoint"] = GetNextTimePoint(dt.Rows[j]["TimePoint"].ToString(), 1);
                                        }
                                        else
                                        {//这一天最后一个段位
                                            dt.Rows[j]["DateTime"] = GetNextPointDatetime(DateTime.Parse(dt.Rows[j]["DateTime"].ToString()), 1).ToString("yyyy-MM-dd HH:mm");
                                            dt.Rows[j]["TimePoint"] = GetNextTimePoint(dt.Rows[j]["TimePoint"].ToString(), 1);
                                            dateList.Add(DateTime.Parse(day).AddDays(1).ToString("yyyy-MM-dd"));
                                            AllPushedDataTable(dt, timePoints, dateList, endday, flag);
                                        }
                                    }
                                }
                            }
                        }
                        timePointList.Add(timePoints.Rows[i][0].ToString());
                    }
                }
            }

            return dt.AsEnumerable().Where(p => DateTime.Parse(p["DateTime"].ToString()) <= DateTime.Parse(endday.ToString("yyyy-MM-dd 23:59:59"))).CopyToDataTable();
        }

        /// <summary>
        /// 获取当前显示7天的前一天应该后移至当前7天的状态
        /// </summary>
        /// <param name="m_DataTimeAllocate">当前日期(不一定是今天)</param>
        /// <returns></returns>
        public DataTable GetStatusPushedToNextDay(DateTime m_DataTimeAllocate, PatientInfo patientInfo)
        {
            DataTable newTable = NewTheDataTable();
            DataTable timePoints = MethodSet.GetTimePoint();
            //获取当前显示7天的前一天的所有状态(合并同一分钟的状态)
            DataTable preStatus = GetStatusCombined(GetStatusTablePreDay(timePoints, m_DataTimeAllocate, patientInfo));
            if (preStatus.Rows.Count == 0)
            {
                return new DataTable();
            }
            List<string> timePointList = new List<string>();
            for (int i = 0; i < timePoints.Rows.Count; i++)
            {
                //入院状态特殊处理
                string inpState = GetInpStateByConfig();

                //获取同一段位的状态
                var currentStatus = preStatus.AsEnumerable().Where(p => p["TimePoint"].ToString() == timePoints.Rows[i]["TimePoint"].ToString());
                if (i != timePoints.Rows.Count - 1)
                {
                    if (currentStatus.Count() > 1)
                    {//7003-出院；7008-入院；
                        DataRow firstRow = currentStatus.FirstOrDefault();
                        string leftID = string.Empty;
                        if (firstRow["ID"].ToString() == "7003")
                        {
                            leftID = currentStatus.Any(p => p["ID"].ToString().Contains(inpState)) ? inpState : "7003";
                        }
                        else
                        {
                            leftID = firstRow["ID"].ToString();
                        }

                        foreach (DataRow item in currentStatus.Where(p => p["ID"].ToString() != leftID))
                        {
                            for (int j = 0; j < preStatus.Rows.Count; j++)
                            {
                                if (preStatus.Rows[j]["ID"] == item["ID"] && !timePointList.Contains(item["TimePoint"].ToString()))
                                {
                                    preStatus.Rows[j]["DateTime"] = GetNextPointDatetime(DateTime.Parse(preStatus.Rows[j]["DateTime"].ToString()), 1).ToString("yyyy-MM-dd HH:mm");
                                    preStatus.Rows[j]["TimePoint"] = GetNextTimePoint(preStatus.Rows[j]["TimePoint"].ToString(), 1);
                                }
                            }
                            timePointList.Add(timePoints.Rows[i][0].ToString());
                        }
                    }
                }
                else
                {
                    if (currentStatus.Count() > 1)
                    {//7003-出院；7008-入院(仁和医院为7009)；
                        DataRow firstRow = currentStatus.FirstOrDefault();
                        string leftID = string.Empty;
                        if (firstRow["ID"].ToString() == "7003")
                        {
                            leftID = currentStatus.Any(p => p["ID"].ToString().Contains(inpState)) ? inpState : "7003";
                        }
                        else
                        {
                            leftID = firstRow["ID"].ToString();
                        }

                        foreach (DataRow item in currentStatus.Where(p => p["ID"].ToString() != leftID))
                        {
                            item["DateTime"] = GetNextPointDatetime(DateTime.Parse(item["DateTime"].ToString()), 1).ToString("yyyy-MM-dd HH:mm");
                            item["TimePoint"] = GetNextTimePoint(item["TimePoint"].ToString(), 1);
                            AddDataRowToDatatable(newTable, item);
                        }
                        timePointList.Add(timePoints.Rows[i][0].ToString());
                    }
                }
            }
            return newTable;
        }

        /// <summary>
        /// 将精确至同一分钟的状态合并
        /// 注：'入院'不能和'出院'合并
        /// 提示：1、仁和医院入院状态为7009，与其它医院不同；
        ///       2、其它医院入院状态为7008，入科状态为7009；
        ///       固此处合并顺序不冲突。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable GetStatusCombined(DataTable dt)
        {
            DataTable newTable = NewTheDataTable();
            var timesEnu = dt.AsEnumerable();
            var timeList = timesEnu.Select(p => DateTime.Parse(p["DateTime"].ToString()).ToString("yyyy-MM-dd HH:mm")).Distinct();
            //入院状态特殊处理
            string inpState = GetInpStateByConfig();

            foreach (var item in timeList)
            {
                var theTimes = timesEnu.Where(q => DateTime.Parse(q["DateTime"].ToString()).ToString("yyyy-MM-dd HH:mm") == item);
                if (theTimes.Count() > 1)
                {
                    DataRow dRow = newTable.NewRow();
                    string m_name = string.Empty;
                    string m_id = string.Empty;
                    //入科
                    DataRow row7009 = theTimes.FirstOrDefault(b => b["ID"].ToString() == "7009");
                    if (null != row7009)
                    {
                        dRow[0] = row7009[0];
                        dRow[1] = row7009[1];
                        dRow[4] = row7009[4];
                        if (null == row7009[5] || row7009[5].ToString().Trim() == "")
                        {
                            dRow[5] = Guid.NewGuid().ToString();
                        }
                        else
                        {
                            dRow[5] = row7009[5];
                        }
                        m_name += row7009["EventInformation"].ToString();
                        m_id += row7009["ID"].ToString();
                    }

                    var timesEnd = theTimes.Where(c => c["ID"].ToString() != "7009");
                    if (theTimes.Any(a => a["ID"].ToString() == "7003") && theTimes.Any(a => a["ID"].ToString() == inpState))
                    {//7003-出院；7008-入院；7009-入科
                        timesEnd = timesEnd.Where(d => d["ID"].ToString() != "7003");
                    }

                    if (timesEnd.Any(a => a["ID"].ToString() == inpState))
                    {//遵循入科'第一'，入院'第二'
                        DataRow ryDr = timesEnd.FirstOrDefault(a => a["ID"].ToString() == inpState);
                        dRow[0] = (null == dRow[0] || dRow[0].ToString() == "") ? ryDr[0] : dRow[0];
                        dRow[1] = (null == dRow[1] || dRow[1].ToString() == "") ? ryDr[1] : dRow[1];
                        dRow[4] = (null == dRow[4] || dRow[4].ToString() == "") ? ryDr[4] : dRow[4];
                        dRow[5] = (null == dRow[5] || dRow[5].ToString() == "") ? ryDr[5] : (null == ryDr[5] || ryDr[5].ToString().Trim() == "") ? Guid.NewGuid().ToString() : ryDr[5].ToString();
                        m_name += string.IsNullOrEmpty(m_name) ? ryDr["EventInformation"].ToString() : ("·" + ryDr["EventInformation"].ToString());
                        m_id += string.IsNullOrEmpty(m_id) ? ryDr["ID"].ToString() : ("·" + ryDr["ID"].ToString());
                    }
                    foreach (DataRow dr in timesEnd.Where(a => a["ID"].ToString() != inpState))
                    {
                        if (string.IsNullOrEmpty(dRow[0].ToString()))
                        {
                            dRow[0] = dr[0].ToString();
                        }
                        if (string.IsNullOrEmpty(dRow[1].ToString()))
                        {
                            dRow[1] = dr[1].ToString();
                        }
                        if (string.IsNullOrEmpty(dRow[4].ToString()))
                        {
                            dRow[4] = dr[4].ToString();
                        }
                        if (string.IsNullOrEmpty(dRow[5].ToString()))
                        {
                            dRow[5] = (null == dr[5] || dr[5].ToString().Trim() == "") ? Guid.NewGuid().ToString() : dr[5].ToString();
                        }
                        m_name += string.IsNullOrEmpty(m_name) ? dr["EventInformation"].ToString() : ("·" + dr["EventInformation"].ToString());
                        m_id += string.IsNullOrEmpty(m_id) ? dr["ID"].ToString() : ("·" + dr["ID"].ToString());
                    }
                    dRow[2] = m_id;
                    dRow[3] = m_name + "--";
                    newTable.Rows.Add(dRow);

                    if (theTimes.Any(a => a["ID"].ToString() == "7003") && theTimes.Any(a => a["ID"].ToString() == inpState))
                    {
                        //添加出院
                        DataRow row7003 = theTimes.FirstOrDefault(b => b["ID"].ToString() == "7003");
                        row7003["EventInformation"] += "--";
                        if (null != row7003)
                        {
                            newTable = AddDataRowToDatatable(newTable, row7003);
                        }
                    }
                }
                else
                {
                    DataRow oneRow = theTimes.FirstOrDefault();
                    oneRow["EventInformation"] += "--";
                    newTable = AddDataRowToDatatable(newTable, oneRow);
                }
            }
            return newTable;
        }

        /// <summary>
        /// 将行添加至datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public DataTable AddDataRowToDatatable(DataTable dt, DataRow dr)
        {
            DataRow newRow = dt.NewRow();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                newRow[i] = dr[i];
            }
            dt.Rows.Add(newRow);
            return dt;
        }

        /// <summary>
        /// 比较两个时间是否属于同一个段位
        /// </summary>
        /// author:cyq
        /// date:2012-08-16
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public bool CompareTimePoint(DataTable timePoints, DateTime time1, DateTime time2)
        {
            bool boo = false;
            if (time1.ToString("yyyy-MM-dd") == time2.ToString("yyyy-MM-dd"))
            {
                boo = GetTimePointInTime(timePoints, time1.Hour.ToString()) == GetTimePointInTime(timePoints, time2.Hour.ToString());
            }
            return boo;
        }

        /// <summary>
        /// 获取某一时间段位后N个段位的时间
        /// </summary>
        /// author:cyq
        /// date:2012-08-16
        /// <param name="dtime"></param>
        /// <param name="thePoint">后thePoint个段位</param>
        /// <returns></returns>
        public DateTime GetNextPointDatetime(DateTime dtime, int thePoint)
        {
            DataTable timePointDt = PublicSet.MethodSet.GetTimePoint();
            string hour = dtime.Hour.ToString();
            string newDateStr = dtime.ToString("yyyy-MM-dd");// 日期部分 yyyy-MM-dd
            string timePoint = string.Empty;                //小时部分
            int days = thePoint / timePointDt.Rows.Count;
            int leftHour = thePoint % timePointDt.Rows.Count;
            if (leftHour == 0)
            {
                return dtime.AddDays(days);
            }
            int i = 0;
            foreach (DataRow dr in timePointDt.Rows)
            {
                i++;
                if (i == 1 && Convert.ToInt32(hour) <= Convert.ToInt32(dr[0]))
                {
                    //2  ----> 0 1 2
                    if ((i + thePoint) <= timePointDt.Rows.Count)
                    {
                        timePoint = timePointDt.Rows[i + thePoint - 1][0].ToString();
                    }
                    else
                    {
                        timePoint = timePointDt.Rows[((i + thePoint) % timePointDt.Rows.Count) - 1][0].ToString();
                        newDateStr = dtime.AddDays(1).ToString("yyyy-MM-dd");
                    }
                    break;
                }
                else if (i == timePointDt.Rows.Count && Convert.ToInt32(hour) >= Convert.ToInt32(dr[0]))
                {
                    //22 ----> 22 23 24
                    if ((i + thePoint) <= timePointDt.Rows.Count)
                    {
                        timePoint = timePointDt.Rows[i + thePoint - 1][0].ToString();
                    }
                    else
                    {
                        timePoint = timePointDt.Rows[((i + thePoint) % timePointDt.Rows.Count) - 1][0].ToString();
                        newDateStr = dtime.AddDays(1).ToString("yyyy-MM-dd");
                    }
                    break;
                }
                if (Convert.ToInt32(hour) >= (Convert.ToInt32(dr[0]) - 1) && Convert.ToInt32(hour) <= (Convert.ToInt32(dr[0]) + 2))
                {
                    //2  ----> 3 4
                    //6  ----> 5 6 7 8
                    //10 ----> 9 10 11 12
                    //14 ----> 13 14 15 16
                    //18 ----> 17 18 19 20
                    //22 ----> 21
                    if ((i + thePoint) <= timePointDt.Rows.Count)
                    {
                        timePoint = timePointDt.Rows[i + thePoint - 1][0].ToString();
                    }
                    else
                    {
                        timePoint = timePointDt.Rows[((i + thePoint) % timePointDt.Rows.Count) - 1][0].ToString();
                        newDateStr = dtime.AddDays(1).ToString("yyyy-MM-dd");
                    }
                    break;
                }
            }
            if (string.IsNullOrEmpty(timePoint))
            {
                timePoint = timePointDt.Rows[timePointDt.Rows.Count][0].ToString();
            }

            return DateTime.Parse(newDateStr + " " + (int.Parse(timePoint) < 10 ? ("0" + timePoint) : timePoint) + ":" + (dtime.Minute < 10 ? ("0" + dtime.Minute.ToString()) : dtime.Minute.ToString())).AddDays(days);
        }

        /// <summary>
        /// 获取某一时间段位后移N个段位后所属段位
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public string GetNextTimePoint(string currentPoint, int thePoint)
        {
            DataTable timePointDt = PublicSet.MethodSet.GetTimePoint();
            string returnValue = string.Empty;
            int newPoint = thePoint;
            if (thePoint > timePointDt.Rows.Count)
            {
                newPoint = thePoint % timePointDt.Rows.Count;
            }
            for (int i = 0; i < timePointDt.Rows.Count; i++)
            {
                if (timePointDt.Rows[i][0].ToString() == currentPoint)
                {
                    if (i + 1 + newPoint > timePointDt.Rows.Count)
                    {
                        returnValue = timePointDt.Rows[((i + 1 + newPoint) % timePointDt.Rows.Count) - 1][0].ToString();
                    }
                    else
                    {
                        returnValue = timePointDt.Rows[i + newPoint][0].ToString();
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 获取入院状态ID （by cyq 2012-09-07 16:20）
        /// 特别注明：除仁和医院的入院状态为7009外，其它医院的入院状态均为7008.(此处针对仁和医院特殊处理)
        /// </summary>
        /// <returns></returns>
        public string GetInpStateByConfig()
        {
            string nursingMeasureStateFlag = MethodSet.GetConfigValueByKey("NursingMeasureStateFlag");
            return nursingMeasureStateFlag == "1" ? "7009" : "7008";
        }

        /// <summary>
        /// 获取当前显示7天的前一天的所有状态
        /// 当前日期(m_DataTimeAllocate)
        /// author:cyq
        /// date:2012-09-04
        /// </summary>
        /// <returns></returns>
        public DataTable GetStatusTablePreDay(DataTable timePoints, DateTime m_DataTimeAllocate, PatientInfo patientInfo)
        {
            DataTable dt = MethodSet.GetPatStatesInDatetime(GetPreDayByAllocateTime(m_DataTimeAllocate), InpatientNo, patientInfo);
            DataTable newTable = NewTheDataTable();
            PatientInfo pInfo = new PatientInfo();
            foreach (DataRow dr in dt.Rows)
            {
                DataRow drow = newTable.NewRow();
                drow[0] = dr["DOTIME"];
                drow[1] = pInfo.GetTimePointInTime(timePoints, DateTime.Parse(dr["dotime"].ToString()).Hour.ToString());
                drow[2] = dr["ID"];
                drow[3] = dr["NAME"];
                drow[4] = dr["DOTIME"];
                if (null == drow[5] || drow[5].ToString().Trim() == "")
                {
                    drow[5] = Guid.NewGuid().ToString();
                }
                newTable.Rows.Add(drow);
            }
            return newTable;
        }

        /// <summary>
        /// 获取当前显示7天的所有状态
        /// 当前日期(m_DataTimeAllocate)
        /// author:cyq
        /// date:2012-09-04
        /// </summary>
        /// <returns></returns>
        public DataTable GetStatusTable(DataTable timePoints, DateTime m_DataTimeAllocate, PatientInfo patientInfo)
        {
            DateTime inTime = DateTime.Parse(InTime);
            DateTime starttime = GetCurrentFirstDay(m_DataTimeAllocate);
            DateTime endtime = GetCurrentEndDay(m_DataTimeAllocate);

            DataTable dt = MethodSet.GetPatStatesByDateDiv(starttime, endtime, InpatientNo, patientInfo);
            DataTable newTable = NewTheDataTable();
            PatientInfo pInfo = new PatientInfo();
            foreach (DataRow dr in dt.Rows)
            {
                DataRow drow = newTable.NewRow();
                drow[0] = dr["DOTIME"];
                drow[1] = pInfo.GetTimePointInTime(timePoints, DateTime.Parse(dr["dotime"].ToString()).Hour.ToString());
                drow[2] = dr["ID"];
                drow[3] = dr["NAME"];
                drow[4] = dr["DOTIME"];
                drow[5] = Guid.NewGuid().ToString();
                newTable.Rows.Add(drow);
            }
            return newTable;
        }

        /// <summary>
        /// 根据当前日期获取当前显示七天的前一天
        /// </summary>
        /// <param name="m_DataTimeAllocate">当前日期</param>
        /// <returns></returns>
        public DateTime? GetPreDayByAllocateTime(DateTime m_DataTimeAllocate)
        {
            DateTime inTime = DateTime.Parse(InTime);
            int num = (int)Math.Floor((DateTime.Parse(m_DataTimeAllocate.ToString("yyyy-MM-dd 00:00:00")) - DateTime.Parse(inTime.ToString("yyyy-MM-dd 00:00:00"))).TotalDays / 7.00);
            if (num >= 1)
            {
                return inTime.AddDays(7 * num - 1);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前显示的第一天
        /// </summary>
        /// <param name="m_DataTimeAllocate">当前日期</param>
        /// <returns></returns>
        public DateTime GetCurrentFirstDay(DateTime m_DataTimeAllocate)
        {
            try
            {
                DateTime starttime = DateTime.Parse(InTime);
                int num = (int)Math.Floor((DateTime.Parse(m_DataTimeAllocate.ToString("yyyy-MM-dd 00:00:00")) - DateTime.Parse(starttime.ToString("yyyy-MM-dd 00:00:00"))).TotalDays / 7.00);
                if (num >= 1)
                {
                    // starttime = starttime.AddDays(num * 7);
                    m_DataTimeAllocate = starttime.AddDays(num * 7);
                }
                //return starttime;
                return m_DataTimeAllocate;// zyx 2013-1-15
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// 获取当前显示的最后一天
        /// </summary>
        /// <param name="m_DataTimeAllocate">当前日期</param>
        /// <returns></returns>
        public DateTime GetCurrentEndDay(DateTime m_DataTimeAllocate)
        {
            return GetCurrentFirstDay(m_DataTimeAllocate).AddDays(6);
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
        /// 创建固定类型的datatable
        /// 注：三测单状态专用
        /// </summary>
        /// <returns></returns>
        public DataTable NewTheDataTable()
        {
            DataTable newTable = new DataTable();
            newTable.Columns.Add("DoTime");
            newTable.Columns.Add("TimePoint");
            newTable.Columns.Add("ID");
            newTable.Columns.Add("EventInformation");
            newTable.Columns.Add("DateTime");
            newTable.Columns.Add("GUID");
            return newTable;
        }
        #endregion

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
            //DataTablePainInfo = new DataTable();      //其他2  
            //add by wyt
            DataTableParam1 = new DataTable();          //自定义参数1
            DataTableParam2 = new DataTable();          //自定义参数2
            DateTableEventInformation = new DataTable();
            DataTableDayOfHospital = new DataTable();
            DataTableOperationTime = new DataTable();

            #region 体温数据
            DataTableTemperature.Columns.Add("DateTime");
            DataTableTemperature.Columns.Add("TimePoint");
            //DataTableTemperature.Columns.Add("Indx");
            DataTableTemperature.Columns.Add("TypeID"); //1: 口温  2: 腋温 3: 肛温
            DataTableTemperature.Columns.Add("Value");
            DataTableTemperature.Columns.Add("Memo");
            DataTableTemperature.Columns.Add("LinkNext");//是否与下一个点连接
            #endregion

            #region 脉搏数据
            DataTableMaiBo.Columns.Add("DateTime");
            DataTableMaiBo.Columns.Add("TimePoint");
            //DataTableMaiBo.Columns.Add("Indx");
            DataTableMaiBo.Columns.Add("Value");
            DataTableMaiBo.Columns.Add("Memo");
            DataTableMaiBo.Columns.Add("LinkNext");//是否与下一个点连接
            #endregion

            #region 呼吸数据
            DataTableHuXi.Columns.Add("DateTime");
            DataTableHuXi.Columns.Add("TimePoint");
            //DataTableHuXi.Columns.Add("Indx");
            DataTableHuXi.Columns.Add("Value");
            DataTableHuXi.Columns.Add("Memo");
            DataTableHuXi.Columns.Add("LinkNext");//是否与下一个点连接
            DataTableHuXi.Columns.Add("IsSpecial");//Y:表示有用呼吸机
            #endregion

            #region 物理降温
            DataTableWuLiJiangWen.Columns.Add("DateTime");
            DataTableWuLiJiangWen.Columns.Add("TimePoint");
            //DataTableWuLiJiangWen.Columns.Add("Indx");
            DataTableWuLiJiangWen.Columns.Add("Value");
            DataTableWuLiJiangWen.Columns.Add("LinkNext");//是否与下一个点连接
            #endregion

            #region 新增的物理升温
            DataTableWuLiShengWen.Columns.Add("DateTime");
            DataTableWuLiShengWen.Columns.Add("TimePoint");
            //DataTableWuLiShengWen.Columns.Add("Indx");
            DataTableWuLiShengWen.Columns.Add("Value");
            DataTableWuLiShengWen.Columns.Add("LinkNext");//是否与下一个点连接
            #endregion

            #region 心率数据
            DataTableXinLv.Columns.Add("DateTime");
            DataTableXinLv.Columns.Add("TimePoint");
            //DataTableXinLv.Columns.Add("Indx");
            DataTableXinLv.Columns.Add("Value");
            DataTableXinLv.Columns.Add("Memo");
            DataTableXinLv.Columns.Add("LinkNext");//是否与下一个点连接
            DataTableXinLv.Columns.Add("IsSpecial");//Y :表示有用“起搏器”
            #endregion

            #region 血压数据
            DateTableXueYa.Columns.Add("DateTime");
            DateTableXueYa.Columns.Add("TimePoint");
            //DateTableXueYa.Columns.Add("Indx");
            DateTableXueYa.Columns.Add("Value");
            #endregion

            #region 总入量数据
            DataTableZongRuLiang.Columns.Add("DateTime");
            DataTableZongRuLiang.Columns.Add("TimePoint");
            //DataTableZongRuLiang.Columns.Add("Indx");
            DataTableZongRuLiang.Columns.Add("Value");
            #endregion

            #region 总出量数据
            DataTableZongChuLiang.Columns.Add("DateTime");
            DataTableZongChuLiang.Columns.Add("TimePoint");
            //DataTableZongChuLiang.Columns.Add("Indx");
            DataTableZongChuLiang.Columns.Add("Value");
            #endregion

            #region 疼痛数据  （新增的  ywk ）
            DataTablePainInfo.Columns.Add("DateTime");
            DataTablePainInfo.Columns.Add("TimePoint");
            //DataTablePainInfo.Columns.Add("Indx");
            DataTablePainInfo.Columns.Add("Value");
            //DataTableHuXi.Columns.Add("LinkNext");//是否与下一个点连接
            //DataTableHuXi.Columns.Add("IsSpecial");//Y:表示有用
            #endregion

            #region 引流量数据
            DataTableYinLiuLiang.Columns.Add("DateTime");
            DataTableYinLiuLiang.Columns.Add("TimePoint");
            //DataTableYinLiuLiang.Columns.Add("Indx");
            DataTableYinLiuLiang.Columns.Add("Value");
            #endregion

            #region 大便次数数据
            DataTableDaBianCiShu.Columns.Add("DateTime");
            DataTableDaBianCiShu.Columns.Add("TimePoint");
            //DataTableDaBianCiShu.Columns.Add("Indx");
            DataTableDaBianCiShu.Columns.Add("Value");
            #endregion

            #region 身高数据
            DataTableShenGao.Columns.Add("DateTime");
            DataTableShenGao.Columns.Add("TimePoint");
            //DataTableShenGao.Columns.Add("Indx");
            DataTableShenGao.Columns.Add("Value");
            #endregion

            #region 体重数据
            DataTableTiZhong.Columns.Add("DateTime");
            DataTableTiZhong.Columns.Add("TimePoint");
            //DataTableTiZhong.Columns.Add("Indx");
            DataTableTiZhong.Columns.Add("Value");
            #endregion

            //add by wyt
            #region 自定义参数1
            DataTableParam1.Columns.Add("DateTime");
            DataTableParam1.Columns.Add("TimePoint");
            //DataTableParam1.Columns.Add("Indx");
            DataTableParam1.Columns.Add("Value");
            #endregion

            #region 自定义参数2
            DataTableParam2.Columns.Add("DateTime");
            DataTableParam2.Columns.Add("TimePoint");
            //DataTableParam2.Columns.Add("Indx");
            DataTableParam2.Columns.Add("Value");
            #endregion

            #region 过敏药物数据
            DataTableGuoMinYaoWu.Columns.Add("DateTime");
            DataTableGuoMinYaoWu.Columns.Add("TimePoint");
            //DataTableGuoMinYaoWu.Columns.Add("Indx");
            DataTableGuoMinYaoWu.Columns.Add("Value");
            #endregion

            #region 特殊治疗数据
            DataTableTeShuZhiLiao.Columns.Add("DateTime");
            DataTableTeShuZhiLiao.Columns.Add("TimePoint");
            //DataTableTeShuZhiLiao.Columns.Add("Indx");
            DataTableTeShuZhiLiao.Columns.Add("Value");
            #endregion

            #region 住院天数
            DataTableDayOfHospital.Columns.Add("DateTime");
            DataTableDayOfHospital.Columns.Add("TimePoint");
            //DataTableDayOfHospital.Columns.Add("Indx");
            DataTableDayOfHospital.Columns.Add("Value");
            #endregion

            #region 术后天数
            DataTableOperationTime.Columns.Add("DateTime");
            DataTableOperationTime.Columns.Add("TimePoint");
            //DataTableOperationTime.Columns.Add("Indx");
            DataTableOperationTime.Columns.Add("Value");
            #endregion

            #region 初始化 患者入院、转入、手术、分娩、出院、死亡等,除手术不写具体时间外，其余均按24小时制，精确到分钟
            DateTableEventInformation.Columns.Add("DateTime");
            DateTableEventInformation.Columns.Add("TimePoint");
            DateTableEventInformation.Columns.Add("ID");
            DateTableEventInformation.Columns.Add("EventInformation");
            DateTableEventInformation.Columns.Add("DoTime");
            DateTableEventInformation.Columns.Add("GUID");

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

            //add by wyt
            DataTableParam1.Rows.Clear();           //自定义参数1
            DataTableParam2.Rows.Clear();           //自定义参数2

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
        public int InpatientInformationY = 100;

        //病人信息的横坐标
        public int InpatientNameX = 55 - 40;

        public int AgeX = 165 - 40;
        public int GenderX = 245 - 40;
        public int SectionX = 310 - 40;
        public int BedCodeX = 420 - 40;
        public int InTimeX = 500 - 40;
        public int InpatientNoX = 635 - 40;
    }
}
