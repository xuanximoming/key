using System;


namespace HuangF.Sys.Date
{
    /// <summary>
    /// 日期类，有农历数据
    /// </summary>
    public class HFDate
    {
        /// <summary>
        /// 农历月份数据，每年4字节，从1901年开始，共150年
        ///数据解析：
        ///如果第一字节的bit7为1，则该年1月1日位于农历12月，否则位于11月 
        ///第一字节去除bit7为该年1月1日的农历日期 
        ///        第二字节                第三字节 
        ///bit:    7  6  5  4  3  2  1  0  7  6  5  4  3  2  1  0 
        ///农历月份:16 15 14 13 12 11 10 9  8  7  6  5  4  3  2  1 
        ///农历月份指的是从该年1月1日的农历月份算起的顺序号 
        ///农历月份对应的bit为1则该月为30日，否则为29日 
        ///第四字节为闰月月份
        /// </summary>
        private static Byte[] CnData ={
            0x0b,0x52,0xba,0x00,0x16,0xa9,0x5d,0x00,0x83,0xa9,0x37,0x05,0x0e,0x74,0x9b,0x00, 
            0x1a,0xb6,0x55,0x00,0x87,0xb5,0x55,0x04,0x11,0x55,0xaa,0x00,0x1c,0xa6,0xb5,0x00, 
            0x8a,0xa5,0x75,0x02,0x14,0x52,0xba,0x00,0x81,0x52,0x6e,0x06,0x0d,0xe9,0x37,0x00, 
            0x18,0x74,0x97,0x00,0x86,0xea,0x96,0x05,0x10,0x6d,0x55,0x00,0x1a,0x35,0xaa,0x00, 
            0x88,0x4b,0x6a,0x02,0x13,0xa5,0x6d,0x00,0x1e,0xd2,0x6e,0x07,0x0b,0xd2,0x5e,0x00, 
            0x17,0xe9,0x2e,0x00,0x84,0xd9,0x2d,0x05,0x0f,0xda,0x95,0x00,0x19,0x5b,0x52,0x00, 
            0x87,0x56,0xd4,0x04,0x11,0x4a,0xda,0x00,0x1c,0xa5,0x5d,0x00,0x89,0xa4,0xbd,0x02, 
            0x15,0xd2,0x5d,0x00,0x82,0xb2,0x5b,0x06,0x0d,0xb5,0x2b,0x00,0x18,0xba,0x95,0x00, 
            0x86,0xb6,0xa5,0x05,0x10,0x56,0xb4,0x00,0x1a,0x4a,0xda,0x00,0x87,0x49,0xba,0x03, 
            0x13,0xa4,0xbb,0x00,0x1e,0xb2,0x5b,0x07,0x0b,0x72,0x57,0x00,0x16,0x75,0x2b,0x00, 
            0x84,0x6d,0x2a,0x06,0x0f,0xad,0x55,0x00,0x19,0x55,0xaa,0x00,0x86,0x55,0x6c,0x04, 
            0x12,0xc9,0x76,0x00,0x1c,0x64,0xb7,0x00,0x8a,0xe4,0xae,0x02,0x15,0xea,0x56,0x00, 
            0x83,0xda,0x55,0x07,0x0d,0x5b,0x2a,0x00,0x18,0xad,0x55,0x00,0x85,0xaa,0xd5,0x05, 
            0x10,0x53,0x6a,0x00,0x1b,0xa9,0x6d,0x00,0x88,0xa9,0x5d,0x03,0x13,0xd4,0xae,0x00, 
            0x81,0xd4,0xab,0x08,0x0c,0xba,0x55,0x00,0x16,0x5a,0xaa,0x00,0x83,0x56,0xaa,0x06, 
            0x0f,0xaa,0xd5,0x00,0x19,0x52,0xda,0x00,0x86,0x52,0xba,0x04,0x11,0xa9,0x5d,0x00, 
            0x1d,0xd4,0x9b,0x00,0x8a,0x74,0x9b,0x03,0x15,0xb6,0x55,0x00,0x82,0xad,0x55,0x07, 
            0x0d,0x55,0xaa,0x00,0x18,0xa5,0xb5,0x00,0x85,0xa5,0x75,0x05,0x0f,0x52,0xb6,0x00, 
            0x1b,0x69,0x37,0x00,0x89,0xe9,0x37,0x04,0x13,0x74,0x97,0x00,0x81,0xea,0x96,0x08, 
            0x0c,0x6d,0x52,0x00,0x16,0x2d,0xaa,0x00,0x83,0x4b,0x6a,0x06,0x0e,0xa5,0x6d,0x00, 
            0x1a,0xd2,0x6e,0x00,0x87,0xd2,0x5e,0x04,0x12,0xe9,0x2e,0x00,0x1d,0xec,0x96,0x0a, 
            0x0b,0xda,0x95,0x00,0x15,0x5b,0x52,0x00,0x82,0x56,0xd2,0x06,0x0c,0x2a,0xda,0x00, 
            0x18,0xa4,0xdd,0x00,0x85,0xa4,0xbd,0x05,0x10,0xd2,0x5d,0x00,0x1b,0xd9,0x2d,0x00, 
            0x89,0xb5,0x2b,0x03,0x14,0xba,0x95,0x00,0x81,0xb5,0x95,0x08,0x0b,0x56,0xb2,0x00, 
            0x16,0x2a,0xda,0x00,0x83,0x49,0xb6,0x05,0x0e,0x64,0xbb,0x00,0x19,0xb2,0x5b,0x00, 
            0x87,0x6a,0x57,0x04,0x12,0x75,0x2b,0x00,0x1d,0xb6,0x95,0x00,0x8a,0xad,0x55,0x02, 
            0x15,0x55,0xaa,0x00,0x82,0x55,0x6c,0x07,0x0d,0xc9,0x76,0x00,0x17,0x64,0xb7,0x00, 
            0x86,0xe4,0xae,0x05,0x11,0xea,0x56,0x00,0x1b,0x6d,0x2a,0x00,0x88,0x5a,0xaa,0x04, 
            0x14,0xad,0x55,0x00,0x81,0xaa,0xd5,0x09,0x0b,0x52,0xea,0x00,0x16,0xa9,0x6d,0x00, 
            0x84,0xa9,0x5d,0x06,0x0f,0xd4,0xae,0x00,0x1a,0xea,0x4d,0x00,0x87,0xba,0x55,0x04, 
            0x12,0x5a,0xaa,0x00,0x1d,0xab,0x55,0x00,0x8a,0xa6,0xd5,0x02,0x14,0x52,0xda,0x00, 
            0x82,0x52,0xba,0x06,0x0d,0xa9,0x3b,0x00,0x18,0xb4,0x9b,0x00,0x85,0x74,0x9b,0x05, 
            0x11,0xb5,0x4d,0x00,0x1c,0xd6,0xa9,0x00,0x88,0x35,0xaa,0x03,0x13,0xa5,0xb5,0x00, 
            0x81,0xa5,0x75,0x0b,0x0b,0x52,0xb6,0x00,0x16,0x69,0x37,0x00,0x84,0xe9,0x2f,0x06, 
            0x10,0xf4,0x97,0x00,0x1a,0x75,0x4b,0x00,0x87,0x6d,0x52,0x05,0x11,0x2d,0x69,0x00, 
            0x1d,0x95,0xb5,0x00,0x8a,0xa5,0x6d,0x02,0x15,0xd2,0x6e,0x00,0x82,0xd2,0x5e,0x07, 
            0x0e,0xe9,0x2e,0x00,0x19,0xea,0x96,0x00,0x86,0xda,0x95,0x05,0x10,0x5b,0x4a,0x00, 
	        0x1c,0xab,0x69,0x00,0x88,0x2a,0xd8,0x03};

        /// <summary>
        /// 农历日期中文表示
        /// </summary>
        private static string[] CNDays ={
                              "初一","初二","初三","初四","初五",
							  "初六","初七","初八","初九","初十",
							  "十一","十二","十三","十四","十五",
							  "十六","十七","十八","十九","二十",
							  "廿一","廿二","廿三","廿四","廿五",
	                          "廿六","廿七","廿八","廿九","三十"
                                          };

        /// <summary>
        /// 农历月份中文表示
        /// </summary>
        private static string[] CNMonths = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "冬", "腊" };


        /// <summary>
        /// 阿拉伯数字的汉字表示
        /// </summary>
        private static string[] CNNumber = { "○", "一", "二", "三", "四", "五", "六", "七", "八", "九" };


        /// <summary>
        /// 公历时间
        /// </summary>
        private DateTime date;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dt"></param>
        public HFDate(DateTime dt)
        {
            date = dt;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        public HFDate()
        {
            date = DateTime.Now;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="mon">月</param>
        /// <param name="d">日</param>
        /// <param name="h">时</param>
        /// <param name="m">分</param>
        /// <param name="s">秒</param>
        /// <param name="ms">微秒</param>
        public HFDate(int y, int mon, int d, int h, int m, int s, int ms)
        {
            date = new DateTime(y, m, d, h, m, s, ms);
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="d">日</param>
        public HFDate(int y, int m, int d)
        {
            date = new DateTime(y, m, d);
        }

        /// <summary>
        /// 返回当前日期加上指定天数后的日期
        /// </summary>
        /// <param name="d">天数</param>
        /// <returns></returns>
        public HFDate AddDays(int d)
        {
            DateTime dt = Value.AddDays(d);
            return new HFDate(dt);
        }

        /// <summary>
        ///日期的农历日期，返回农历格式：月份*100 + 日，负数为闰月，超出范围则返回0 
        /// </summary>
        /// <param name="dt1">指定的公历日期</param>
        /// <returns>农历的日期</returns>
        private static int LunarCalendarDate(DateTime dt1)
        {
            DateTime dt;
            if (dt1.Hour == 23)        //如果是23点后，应为第二日
                dt = dt1.AddDays(1);
            else
                dt = dt1;
            int[] CnMonth = new int[16];
            int[] CnMonthDays = new int[16];
            int CnBeginDay, LeapMonth, CnMonthData, DaysCount, CnDaysCount, ResultMonth, ResultDay;
            byte[] Bytes = new byte[4];
            if ((dt.Year < 1901) || (dt.Year > 2050))
                return 0;
            Bytes[0] = CnData[(dt.Year - 1901) * 4];
            Bytes[1] = CnData[(dt.Year - 1901) * 4 + 1];
            Bytes[2] = CnData[(dt.Year - 1901) * 4 + 2];
            Bytes[3] = CnData[(dt.Year - 1901) * 4 + 3];
            if ((Bytes[0] & 0x80) != 0)
                CnMonth[0] = 12;
            else
                CnMonth[0] = 11;
            CnBeginDay = (Bytes[0] & 0x7f);
            CnMonthData = Bytes[1];
            CnMonthData = (CnMonthData << 8);
            CnMonthData = (CnMonthData | Bytes[2]);
            LeapMonth = Bytes[3];
            for (int I = 15; I > 0; I--)
            {
                CnMonthDays[15 - I] = 29;
                if (((1 << I) & CnMonthData) != 0)
                    CnMonthDays[15 - I]++;
                if (CnMonth[15 - I] == LeapMonth)
                    CnMonth[15 - I + 1] = -1 * LeapMonth;
                else
                {
                    if (CnMonth[15 - I] < 0)
                        CnMonth[15 - I + 1] = -1 * CnMonth[15 - I] + 1;
                    else CnMonth[15 - I + 1] = CnMonth[15 - I] + 1;
                    if (CnMonth[15 - I + 1] > 12)
                        CnMonth[15 - I + 1] = 1;
                }
            }
            DaysCount = dt.DayOfYear - 1;

            if (DaysCount <= (CnMonthDays[0] - CnBeginDay))
            {
                if ((dt.Year > 1901) && (LunarCalendarDate(new DateTime(dt.Year - 1, 12, 31)) < 0))
                    ResultMonth = -1 * CnMonth[0];
                else ResultMonth = CnMonth[0];
                ResultDay = CnBeginDay + DaysCount;
            }
            else
            {
                CnDaysCount = CnMonthDays[0] - CnBeginDay;
                int i1 = 1;
                while ((CnDaysCount < DaysCount) && ((CnDaysCount + CnMonthDays[i1]) < DaysCount))
                {
                    CnDaysCount += CnMonthDays[i1];
                    i1++;
                }
                ResultMonth = CnMonth[i1];
                ResultDay = DaysCount - CnDaysCount;
            }
            if (ResultMonth > 0)
                return (ResultMonth * 100 + ResultDay);
            else
                return (ResultMonth * 100 - ResultDay);
        }


        /// <summary>
        /// 取农历日的数字表示
        /// </summary>
        /// <returns>农历日</returns>
        private int GetLunarCalendarDayInt()
        {
            return Math.Abs(LunarCalendarDate(Value) % 100);
        }


        /// <summary>
        /// 取农历月的数字表示
        /// </summary>
        /// <returns>农历月，负数表示闰月</returns>
        private int GetLunarCalendarMonthInt()
        {
            return LunarCalendarDate(Value) / 100;
        }

        /// <summary>
        /// 日期/时间
        /// </summary>
        public DateTime Value
        {
            get { return date; }
        }


        /// <summary>
        /// 农历月的数字表示,小于0表示闰月
        /// </summary>
        public int LunarCalendarMonth
        {
            get { return GetLunarCalendarMonthInt(); }
        }

        /// <summary>
        /// 农历月的中文表示
        /// </summary>
        public string LunarCalendarMonthString
        {
            get
            {
                int i = LunarCalendarMonth;
                if (i < 0)
                    return "闰" + CNMonths[(-1 * i) - 1] + "月";
                else
                    return CNMonths[i - 1] + "月";
            }
        }


        /// <summary>
        /// 农历日的数字表示
        /// </summary>
        public int LunarCalendarDay
        {
            get { return GetLunarCalendarDayInt(); }
        }


        /// <summary>
        /// 农历日的中文表示
        /// </summary>
        public string LunarCalendarDayString
        {
            get
            {
                int i = GetLunarCalendarDayInt();
                return CNDays[i - 1];
            }
        }

        /// <summary>
        /// 农历日期表示，如：三月初三
        /// </summary>
        public string ShortLunarCalendarDate
        {
            get
            {
                return LunarCalendarMonthString + LunarCalendarDayString;
            }
        }


        /// <summary>
        /// 农历日期表示，如：甲子年三月初三
        /// </summary>
        public string LongLunarCalendarDate
        {
            get
            {
                GanZhi nz = LunarYear;
                return nz.Name + '年' + ShortLunarCalendarDate;
            }
        }

        /// <summary>
        /// 取指定年的正月初一的日期，也即春节日期
        /// </summary>
        /// <param name="y">年份</param>
        /// <returns>正月初一(春节)的日期</returns>
        public static DateTime GetSpringFestival(int y)
        {
            System.DateTime dt = new System.DateTime(y, 1, 1, 0, 0, 0, 0);
            int m, d;
            while (true)
            {
                m = Math.Abs(LunarCalendarDate(dt) / 100);
                d = (Math.Abs(LunarCalendarDate(dt)) % 100);
                if ((m == 1) && (d == 1))
                    return dt;
                else
                    dt = dt.AddDays(1);
            }
        }


        /// <summary>
        /// 取指定公历年的天干
        /// </summary>
        /// <param name="y">年份</param>
        /// <returns>天干</returns>
        private TianGan GetYearTianGan(int y)
        {
            int i = y - 2000;//以2000年为基准
            TianGan tg = new TianGan(TianGanTypes.Geng);
            if (i == 0)
                return tg;
            else
                return tg.Inc(i);
        }


        /// <summary>
        /// 取指定公历年的地支
        /// </summary>
        /// <param name="y">年份</param>
        /// <returns>地支</returns>
        private DiZhi GetYearDiZhi(int y)
        {
            int i = y - 2000;//以2000年为基准
            DiZhi dz = new DiZhi(DiZhiTypes.Chen);
            if (i == 0)
                return dz;
            else
                return dz.Inc(i);
        }


        /// <summary>
        /// 获取本日期的年柱
        /// </summary>
        /// <returns></returns>
        private GanZhi GetLunarYear()
        {
            int y = Value.Year;
            DateTime d1 = GetSpringFestival(Value.Year);//获取该年的正月初一的公历日期
            DateTime d2 = new DateTime(Value.Year, Value.Month, Value.Day, 0, 0, 0, 0);
            if (d2 < d1)
                y--;
            TianGan tg = GetYearTianGan(y);
            DiZhi dz = GetYearDiZhi(y);
            return new GanZhi(tg, dz);
        }

        /// <summary>
        /// 年柱
        /// </summary>
        public GanZhi LunarYear
        {
            get { return GetLunarYear(); }
        }


        /// <summary>
        /// 年上取月干
        /// </summary>
        /// <param name="ng">年干</param>
        /// <param name="m1">农历月份</param>
        /// <returns>月干</returns>
        private TianGan GetMonthGanByYear(TianGanTypes ng, int m1) //年上取月干
        {
            int m = Math.Abs(m1);
            TianGanTypes tg;
            if ((ng == TianGanTypes.Jia) || (ng == TianGanTypes.Ji))
                tg = TianGanTypes.Bing;
            else if ((ng == TianGanTypes.Yi) || (ng == TianGanTypes.Geng))
                tg = TianGanTypes.Wu;
            else if ((ng == TianGanTypes.Bing) || (ng == TianGanTypes.Xin))
                tg = TianGanTypes.Geng;
            else if ((ng == TianGanTypes.Ding) || (ng == TianGanTypes.Ren))
                tg = TianGanTypes.Ren;
            else
                tg = TianGanTypes.Jia;
            TianGan t = new TianGan(tg);
            return t.Inc(m - 1);
        }


        /// <summary>
        /// 农历整数月转换为地支代表
        /// </summary>
        /// <param name="m">农历月份</param>
        /// <returns></returns>
        private DiZhi MonthToDiZhi(int m)
        {
            int m1 = Math.Abs(m) + 2;//正月建寅
            if (m1 > 12)
                m1 -= 12;
            DiZhiTypes dz = (DiZhiTypes)m1;
            return new DiZhi(dz);
        }

        /// <summary>
        /// 取本日期的月柱
        /// </summary>
        /// <returns></returns>
        private GanZhi GetLunarMonth()
        {
            int m = LunarCalendarMonth;
            GanZhi nz = LunarYear;
            TianGan tg = GetMonthGanByYear(nz.Gan.ID, m);
            DiZhi dz = MonthToDiZhi(m);
            return new GanZhi(tg, dz);
        }

        /// <summary>
        /// 月柱
        /// </summary>
        public GanZhi LunarMonth
        {
            get { return GetLunarMonth(); }
        }

        /// <summary>
        /// 取某天的日干
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>日干</returns>
        private TianGan GetDayGan(DateTime dt)
        {
            System.DateTime dt1 = new System.DateTime(2000, 2, 5, 12, 0, 0, 0);//以2000.2.5为基准
            System.DateTime dt2 = new System.DateTime(dt.Year, dt.Month, dt.Day, 12, 0, 0, 0);
            int n = (dt2 - dt1).Days;
            TianGan tg = new TianGan(TianGanTypes.Gui);
            return tg.Inc(n);
        }

        /// <summary>
        /// 取某天的日支
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        private DiZhi GetDayZhi(System.DateTime dt)
        {
            System.DateTime dt1 = new System.DateTime(2000, 2, 5, 12, 0, 0, 0);////以2000.2.5为基准
            System.DateTime dt2 = new System.DateTime(dt.Year, dt.Month, dt.Day, 12, 0, 0, 0);
            int n = (dt2 - dt1).Days;
            DiZhi dz = new DiZhi(DiZhiTypes.Si);
            return dz.Inc(n);
        }

        /// <summary>
        /// 日柱
        /// </summary>
        /// <returns></returns>
        private GanZhi GetLunarDay()
        {
            TianGan rg = GetDayGan(Value);
            DiZhi rz = GetDayZhi(Value);
            return new GanZhi(rg, rz);
        }

        /// <summary>
        /// 日柱
        /// </summary>
        public GanZhi LunarDay
        {
            get { return GetLunarDay(); }
        }


        /// <summary>
        /// 小时数转换为地支(时辰)表示
        /// </summary>
        /// <param name="h">小时</param>
        /// <returns>地支</returns>
        private DiZhi HourToDiZhi(int h)
        {
            DiZhiTypes dz;
            if ((h >= 1) && (h < 3))
                dz = DiZhiTypes.Chou;
            else if ((h >= 3) && (h < 5))
                dz = DiZhiTypes.Yin;
            else if ((h >= 5) && (h < 7))
                dz = DiZhiTypes.Mao;
            else if ((h >= 7) && (h < 9))
                dz = DiZhiTypes.Chen;
            else if ((h >= 9) && (h < 11))
                dz = DiZhiTypes.Si;
            else if ((h >= 11) && (h < 13))
                dz = DiZhiTypes.Wu;
            else if ((h >= 13) && (h < 15))
                dz = DiZhiTypes.Wei;
            else if ((h >= 15) && (h < 17))
                dz = DiZhiTypes.Shen;
            else if ((h >= 17) && (h < 19))
                dz = DiZhiTypes.You;
            else if ((h >= 19) && (h < 21))
                dz = DiZhiTypes.Xu;
            else if ((h >= 21) && (h < 23))
                dz = DiZhiTypes.Hai;
            else
                dz = DiZhiTypes.Zi;
            return new DiZhi(dz);
        }


        /// <summary>
        /// 日上起时干
        /// </summary>
        /// <param name="rg">日干</param>
        /// <param name="sc">时辰</param>
        /// <returns>时干</returns>
        private TianGan GetTimeGan(TianGanTypes rg, DiZhiTypes sc)
        {
            int sc1 = ((int)sc) - 1;
            TianGanTypes tg;
            if ((rg == TianGanTypes.Jia) || (rg == TianGanTypes.Ji))
                tg = TianGanTypes.Jia;
            else if ((rg == TianGanTypes.Yi) || (rg == TianGanTypes.Geng))
                tg = TianGanTypes.Bing;
            else if ((rg == TianGanTypes.Bing) || (rg == TianGanTypes.Xin))
                tg = TianGanTypes.Wu;
            else if ((rg == TianGanTypes.Ding) || (rg == TianGanTypes.Ren))
                tg = TianGanTypes.Geng;
            else
                tg = TianGanTypes.Ren;
            TianGan t = new TianGan(tg);
            return t.Inc(sc1);
        }


        /// <summary>
        /// 取时柱
        /// </summary>
        /// <returns></returns>
        private GanZhi GetLunarTime()
        {
            GanZhi rz = LunarDay;
            DiZhi sz = HourToDiZhi(Value.Hour);
            TianGan sg = GetTimeGan(rz.Gan.ID, sz.ID);
            return new GanZhi(sg, sz);
        }

        /// <summary>
        /// 时柱
        /// </summary>
        public GanZhi LunarTime
        {
            get { return GetLunarTime(); }
        }


        /// <summary>
        /// 取中文星期短形式表示
        /// </summary>
        /// <param name="i">星期几</param>
        /// <returns></returns>
        private static string GetCNWeekStr(int i)
        {
            string[] s = { "日", "一", "二", "三", "四", "五", "六" };
            return s[i];
        }

        /// <summary>
        /// 中文星期的短格式，如：日，六，三
        /// </summary>
        public string ShortCNWeek
        {
            get
            {
                int w = (int)Value.DayOfWeek;
                return GetCNWeekStr(w);
            }
        }


        /// <summary>
        /// 中文星期的完整格式,如：星期三
        /// </summary>
        public string LongCNWeek
        {
            get { return "星期" + ShortCNWeek; }
        }


        /// <summary>
        /// 表示当日的24节气信息。如果当日没有任何节气，则此值为空。不应直接使用该值，
        /// 而应使用属性SolarTermInfo。
        /// </summary>
        private HFDateSolarTermInfo solarTermInfo = null;


        /// <summary>
        /// 获取本实例日期值是否正值某一节气日期。
        /// </summary>
        /// <returns>如果正好在某一节气上，返回节气信息，否则返回null</returns>
        public string GetSolarTermInfo(DateTime Value)
        {
            try
            {
                string returnValue = "";
                int y = Value.Year;
                string ds = Value.ToString("yyyy-MM-dd");
                SolarTerm24 t = new SolarTerm24(SolarTerm24Types.XiaoHan);//公历1月份第一个节气为小寒
                int n = Value.Month - 1;
                SolarTerm24 t1 = t.Inc(2 * n);//每个公历月上有2个节气 ，该月第一个节气
                DateTime d1 = CalendarUtil.GetSolarTermDateTime(y, t1.ID);
                if (ds == d1.ToString("yyyy-MM-dd"))
                    solarTermInfo = new HFDateSolarTermInfo(t1.ID, d1);
                SolarTerm24 t2 = new SolarTerm24();

                SolarTerm24 t3 = new SolarTerm24();//上个月的第二个节气
                DateTime d3;//上个月第二个节气的时间
                t3 = t1.Prior();
                if (Value.Month == 1) // edit by Ukey 2017-01-03 每年一月份计算上一个月第二个节气出错
                {
                    d3 = CalendarUtil.GetSolarTermDateTime(y - 1, t3.ID);
                }
                else
                {
                    d3 = CalendarUtil.GetSolarTermDateTime(y, t3.ID);
                }

                DateTime d2;//本月第二个节气的日期
                t2 = t1.Next(); //本月第二个节气
                d2 = CalendarUtil.GetSolarTermDateTime(y, t2.ID);
                if (ds == d2.ToString("yyyy-MM-dd"))
                    solarTermInfo = new HFDateSolarTermInfo(t2.ID, d2);
                SolarTerm24 t4 = new SolarTerm24();//下个月的第一个节气
                DateTime d4;//下个月第一个节气的日期
                t4 = t2.Next(); //下个月的第一个节气
                d4 = CalendarUtil.GetSolarTermDateTime(y, t4.ID);
                if (ds == d2.ToString("yyyy-MM-dd"))
                    solarTermInfo = new HFDateSolarTermInfo(t2.ID, d2);
                if (solarTermInfo != null)
                {
                    returnValue = solarTermInfo.Name;
                }
                else
                {
                    int daydiff = 0;
                    if (Value < d1)  //d1 该月的第一个节气时间 d2 该月的第二个节气时间 d3 上个月的第二个节气的时间  前七后八的显示方式
                    {
                        daydiff = (Value - d3).Days + 1;
                        if (daydiff <= 7)
                        {
                            returnValue = t3.Name + "后" + daydiff.ToString() + "天";
                        }
                        else
                        {
                            daydiff = (d1 - Value).Days;
                            returnValue = t1.Name + "前" + daydiff.ToString() + "天";
                        }
                    }
                    else if (Value > d1 && Value < d2)
                    {
                        // returnValue = t1.Name + "第" + (Value.Date - d1.Date).Days.ToString() + "天";
                        daydiff = (Value - d1).Days + 1;
                        if (daydiff <= 7)
                        {
                            returnValue = t1.Name + "后" + daydiff.ToString() + "天";
                        }
                        else
                        {
                            daydiff = (d2 - Value).Days;
                            returnValue = t2.Name + "前" + daydiff.ToString() + "天";
                        }
                    }
                    else if (Value > d2)
                    {
                        //  returnValue = t2.Name + "第" + (Value.Date - d2.Date).Days.ToString() + "天";

                        daydiff = (Value - d2).Days + 1;
                        if (daydiff <= 7)
                        {
                            returnValue = t2.Name + "后" + daydiff.ToString() + "天";
                        }
                        else
                        {
                            daydiff = (d4 - Value).Days;
                            returnValue = t4.Name + "前" + daydiff.ToString() + "天";
                        }
                    }

                }
                solarTermInfo = null;
                return returnValue;
            }
            catch (Exception ex)
            {
                return "小寒";
            }


        }


        /// <summary>
        /// 表示当日的24节气信息。如果当日不在任何节气上，则此值为空。
        /// </summary>
        //public HFDateSolarTermInfo SolarTermInfo
        //{
        //    get
        //    {
        //        if (solarTermInfo == null)
        //            solarTermInfo = GetSolarTermInfo(DateTime.Now);
        //        return solarTermInfo;
        //    }
        //}


    }
}
