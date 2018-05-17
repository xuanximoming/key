using System;
using System.Collections.Generic;

using System.Text;
using System.Globalization;


namespace DrectSoft.Common
{
    /// <summary>
    /// 时间处理工具
    /// </summary>
    public class DateUtil
    {
        /// <summary>
        /// yyyyMMddHHmmss
        /// </summary>
        public static string STD_FOURTEEN = "yyyyMMddHHmmss";
        /// <summary>
        /// yyyyMMddHHmm
        /// </summary>
        public static string STD_TWELVE = "yyyyMMddHHmm";
        /// <summary>
        /// yyyyMMddHH
        /// </summary>
        public static string STD_TEN = "yyyyMMddHH";
        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public static string STD_EIGHT = "yyyyMMdd";
        /// <summary>
        /// HHmmss
        /// </summary>
        public static string STD_SIX = "HHmmss";
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string NORMAL_LONG = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        public static string NORMAL_MINUTE = "yyyy-MM-dd HH:mm";

        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        public static string NORMAL_TIME = "HH:mm";

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public static string NORMAL_SHORT = "yyyy-MM-dd";
        /// <summary>
        /// yyyy年MM月dd日
        /// </summary>
        public static string NORMAL_CHINESE = "yyyy年MM月dd日";

        /// <summary>
        /// 根据提供的格式化字符串(format)格式化当前时间
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string getCurrentDateTiem(string format)
        {
            try
            {


                return DateTime.Now.ToString(format, DateTimeFormatInfo.InvariantInfo);
            }
            catch (Exception e)
            {
                dealWithException(e);
                return null;
            }

        }

        /// <summary>
        /// 根据提供的格式化字符串(format)格式化时间字符串(datestr)
        /// </summary>
        /// <param name="datestr"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string getDateTime(string datestr, string format)
        {
            try
            {
                if (datestr == null)
                {
                    return datestr;
                }
                string result = parseExact(datestr, format);
                if (result != null)
                {
                    return result;
                }
                DateTime datetime = DateTime.Parse(datestr);
                return datetime.ToString(format, DateTimeFormatInfo.InvariantInfo);
            }
            catch (Exception e)
            {
                dealWithException(e);
                return datestr;
            }
        }

        public static DateTime TryParseExact(string datestr, string format)
        {
            try
            {
                DateTime datetime = DateTime.ParseExact(getDateTime(datestr, format), format, null);
                return datetime;
            }
            catch (Exception e)
            {
                throw new Exception("时间格式转换错误！");
            }
        }

        private static string parseExact(string datestr, string format)
        {
            try
            {
                string foreFormat = STD_FOURTEEN;
                datestr = datestr.Trim();
                if (datestr.Length == 14)
                {
                    foreFormat = STD_FOURTEEN;
                }
                else if (datestr.Length == 12)
                {
                    foreFormat = STD_TWELVE;
                }
                else if (datestr.Length == 10)
                {
                    foreFormat = STD_TEN;
                }
                else if (datestr.Length == 8)
                {
                    foreFormat = STD_EIGHT;
                }
                else if (datestr.Length == 6)
                {
                    foreFormat = STD_SIX;
                }
                DateTime datetime = DateTime.ParseExact(datestr, foreFormat, null);
                return datetime.ToString(format, DateTimeFormatInfo.InvariantInfo);
            }
            catch (Exception e)
            {
                dealWithException(e);
                return null;
            }
        }

        public static string addTimeDefaultMax(string datestr)
        {
            if (datestr == null) return datestr;
            if (datestr.Length == 8) { return datestr + "235959"; }  //只有日期
            if (datestr.Length == 12) { return datestr + "59"; }   //时间只有小时和分
            if (datestr.Length == 14 && datestr.EndsWith("000000"))
            {
                return datestr.Substring(0, 8) + "235959";
            }
            return datestr;
        }

        public static DateTime getDateTime(string datestr)
        {
            try
            {
                return Convert.ToDateTime(getDateTime(datestr, NORMAL_LONG));
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
           
        }

        /// <summary>
        /// 如果长度为6位返回yyyy:MM,8位返回yyyy-MM-dd，12位返回yyyy-MM-dd HH:mm，14位返回yyyy年MM月dd日，
        /// </summary>
        /// <param name="datestr"></param>
        /// <returns></returns>
        public static string ConvertToDateTime(string datestr)
        {
            if (datestr == null) return "";
            if (datestr.Length == 6)
            {
                return getDateTime(datestr, "yyyy:MM");
            }
            if (datestr.Length == 8)
            {
                return getDateTime(datestr, "yyyy-MM-dd");
            }
            if (datestr.Length == 12)
            {
                return getDateTime(datestr, "yyyy-MM-dd HH:mm");
            }
            if (datestr.Length == 14)
            {
                return getDateTime(datestr, "yyyy年MM月dd日");
            }
            return "";

        }

        public static string ConvertToDateTime2(string datestr)
        {
            if (datestr == null) return "";
            if (datestr.Length == 6)
            {
                return getDateTime(datestr, "HH:mm");
            }
            if (datestr.Length == 8)
            {
                return getDateTime(datestr, "yyyy-MM-dd");
            }
            if (datestr.Length == 12)
            {
                return getDateTime(datestr, "yyyy-MM-dd HH:mm");
            }
            if (datestr.Length == 14)
            {
                return getDateTime(datestr, "yyyy-MM-dd");
            }
            return "";

        }

        private static void dealWithException(Exception e)
        {
            //异常处理
        }

        /// <summary>
        /// 获取两个时间的天数
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int DateDiffDay(DateTime startDate, DateTime endDate)
        {
            DateTime startDatedate = startDate.Date;
            DateTime endDatedate = endDate.Date;
            TimeSpan startSpan = new TimeSpan(startDatedate.Ticks);
            TimeSpan endSpan = new TimeSpan(endDatedate.Ticks);
            TimeSpan span = startSpan.Subtract(endSpan).Duration();
            return span.Days;
        }

        /// <summary>
        /// 获取两个时间的分钟
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int DateDiffMinutes(DateTime startDate, DateTime endDate)
        {
            TimeSpan startSpan = new TimeSpan(startDate.Ticks);
            TimeSpan endSpan = new TimeSpan(endDate.Ticks);
            TimeSpan span = startSpan.Subtract(endSpan).Duration();
            return (span.Hours * 60) + span.Minutes;
        }

       

        /// <summary>
        /// 当前日期是当前月份的第几周
        /// </summary>
        /// <param name="dateTime">当前日期</param>
        /// <param name="currDayOfWeek">当前日期星期几</param>
        /// <returns></returns>
        public static int GetWeekOfMonth(DateTime dateTime, ref DayOfWeek currDayOfWeek)
        {
            int dayofWeek = 0;
            string month = dateTime.Month.ToString();
            if (month.Length < 2) month = "0" + month;
            string year = dateTime.Year.ToString();
            if (year.Length < 2) year = "0" + year;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            DayOfWeek dayWeek = calendar.GetDayOfWeek(TryParseExact(year + "-" + month + "-01", NORMAL_SHORT));//每个月的第一天是星期几
            int week = GetWeek(dayWeek); //转换成数字星期 
            int currDay = calendar.GetDayOfMonth(dateTime); //当前是这个月的多少号
            currDayOfWeek = calendar.GetDayOfWeek(dateTime); //当前星期几
            int spaceWeekSaturday = 6 - week;     //距离星期六还有几天
            int allspace = currDay - 1 - spaceWeekSaturday;
            if (allspace % 7 != 0)
            {
                dayofWeek = allspace / 7;
                dayofWeek++;
            }
            else
            {
                dayofWeek = allspace / 7;
            }
            return ++dayofWeek;
        }

        public static int GetWeek(DayOfWeek dayWeek)
        {
            switch (dayWeek)
            {
                case DayOfWeek.Sunday:
                    return 0;
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;

            }
            return 0;
        }

        /// <summary>
        ///  返回2个时间的差值（小时）
        /// </summary>
        /// <returns></returns>
        public static string GetHouseAndMin(string startH, string startM, string endH, string endMin)
        {
            if (startH == "" || startM == "" || endH == "" || endMin == "") return "";
            int intstartHBymin = Convert.ToInt32(startH) * 60;
            int intstartM = Convert.ToInt32(startM);
            int startAllMin = intstartHBymin + intstartM;
            int intEndHbyMin = Convert.ToInt32(endH) * 60;
            int intendM = Convert.ToInt32(endMin);
            int endAllMin = intEndHbyMin + intendM;
            int X = endAllMin - startAllMin;
            if (X < 0)
            {
                X = (endAllMin + 24 * 60) - startAllMin;   //如果结束时间小于开始时间 说明跨天了
            }
            string strX = (X / 60).ToString() + "小时" + (X % 60).ToString() + "分";
            return strX;

        }

    }
}
