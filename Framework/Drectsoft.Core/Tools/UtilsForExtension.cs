using System;
using System.Windows.Forms;

namespace DrectSoft.Core
{
    /// <summary>
    /// 扩展方法工具类
    /// </summary>
    public static class UtilsForExtension
    {
        static GenerateShortCode g = new GenerateShortCode(DataAccessFactory.GetSqlDataAccess());

        /// <summary>
        /// 生成py/wb
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string[] GenShortCut(this string source)
        {
            return g.GenerateStringShortCode(source);
        }

        /// <summary>
        /// 转Short?类型值 成 布尔
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool ToBoolean(this short? source)
        {
            if (source.HasValue)
            {
                return Convert.ToBoolean(source.Value);
            }
            else
                return false;
        }

        /// <summary>
        /// 转Short类型值 成 布尔
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool ToBoolean(this short source)
        {
            return Convert.ToBoolean(source);
        }

        /// <summary>
        /// 转布尔类型值 成 Short
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static short ToShort(this bool source)
        {
            return Convert.ToInt16(source);
        }

        public static String ToString(Object o)
        {
            String str = String.Empty;
            try
            {
                str = o.ToString();
                return str;
            }
            catch
            {
                return str;
            }
        }

        public static Decimal ToDecimal(Object o)
        {
            Decimal r = 0;
            try
            {
                r = Convert.ToDecimal(o.ToString());
                return r;
            }
            catch
            {
                return 0;
            }
        }

        public static Int32 ToInt32(Object o)
        {
            Int32 r = 0;
            try
            {
                r = Convert.ToInt32(o.ToString());
                return r;
            }
            catch
            {
                return 0;
            }
        }

        #region Extension for MessageBox Show String

        const string AppMessageCaption = "提示";
        /// <summary>
        /// 信息提示
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ShowInfo(this string source)
        {
            return (int)MessageBox.Show(source, AppMessageCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 信息提问(YesNoCancel)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ShowQuestion(this string source)
        {
            return (int)MessageBox.Show(source, AppMessageCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 信息确认(OkCancel)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ShowConfirm(this string source)
        {
            return (int)MessageBox.Show(source, AppMessageCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        }

        #endregion

        /// <summary>
        /// 判断整数是否在范围内: [min,max]
        /// </summary>
        /// <param name="source"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool Between(this int source, int min, int max)
        {
            return (source >= min && source <= max);
        }

        /// <summary>
        /// 判断decimal是否在范围内: [min,max]
        /// </summary>
        /// <param name="source"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool Between(this decimal source, decimal min, decimal max)
        {
            return (source >= min && source <= max);
        }


        /// <summary>
        /// 判断DateTime是否在范围内: [min,max]
        /// </summary>
        /// <param name="source"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool Between(this DateTime source, DateTime min, DateTime max)
        {
            return (source >= min && source <= max);
        }

        #region 服务器时间方法

        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
        private static extern bool GetSystemTime(out SystemTime systemTime);
        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
        private static extern bool SetSystemTime(ref SystemTime systemTime);
        internal struct SystemTime
        {
            internal ushort wYear;
            internal ushort wMonth;
            internal ushort wDayOfWeek;
            internal ushort wDay;
            internal ushort wHour;
            internal ushort wMinute;
            internal ushort wSecond;
            internal ushort wMilliseconds;
        }

        /// <summary>
        /// 更改本机时间
        /// </summary>
        /// <param name="source"></param>
        public static void Set2LocalSystemTime(this DateTime source)
        {
            DateTime utcTime = source - TimeZoneInfo.Local.BaseUtcOffset;

            SystemTime stt;
            stt.wYear = (ushort)utcTime.Year;
            stt.wMonth = (ushort)utcTime.Month;
            stt.wDayOfWeek = (ushort)utcTime.DayOfWeek;
            stt.wDay = (ushort)utcTime.Day;
            stt.wHour = (ushort)utcTime.Hour;
            stt.wMinute = (ushort)utcTime.Minute;
            stt.wSecond = (ushort)utcTime.Second;
            stt.wMilliseconds = (ushort)utcTime.Millisecond;
            SetSystemTime(ref stt);
        }
        #endregion
    }
}
