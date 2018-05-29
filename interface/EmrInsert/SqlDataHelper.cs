using SDT.ORM;
using System;
using System.Data;
using System.Data.Common;

namespace EmrInsert
{
    public class SqlDataHelper
    {


        /// <summary>
        /// 打开事物
        /// </summary>
        /// <param name="sqltra"></param>
        public static DbTransaction BeginTransaction()
        {
            try
            {
                DbTransaction newTransaction = ORMHelper.GetNewTransaction(IsolationLevel.ReadUncommitted);
                return newTransaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 事物执行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqltra"></param>
        public static int ExecuteNonQuery(string sql, DbTransaction sqltra)
        {
            try
            {
                return DBHelper.FromCustomSql(sql, sqltra);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }

        /// <summary>
        /// 关闭事物
        /// </summary>
        /// <param name="sqltra"></param>
        public static void CloseTransaction(DbTransaction sqltra)
        {
            try
            {
                sqltra.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql">查询语句</param>
        public static DataTable SelectDataTable(string sql, DbTransaction tran)
        {
            try
            {
                if (tran != null)
                {
                    return DBHelper.SelectToDataTable(sql);

                }
                else
                {
                    return DBHelper.SelectToDataTable(sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql">查询语句</param>
        public static DataTable SelectDataTable(string sql)
        {
            try
            {
                return DBHelper.SelectToDataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql">查询语句</param>
        public static DataSet SelectAdapter(string sql)
        {
            try
            {
                return DBHelper.SelectToDataSet(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int ExecuteNonQuery(string sql)
        {
            try
            {
                return DBHelper.FromCustomSql(sql); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #region 生成主键
        /// <summary>
        /// 获取客户编号+服务器编号
        /// </summary>
        /// <returns></returns>
        public static string GetUserID()
        {
            return "086028000A";
        }

        public static string MyGetNewPrimaryKey()
        {
            string maxSQL = "";
            return GetUserID() + GetTime();
        }

        /// <summary>
        /// 字符串自动增长
        /// </summary>
        /// <param name="sInput">自增字符串</param>
        /// <param name="isAdd">是否自增(true自增1;false自增-1)</param>
        /// <returns>自增后字符串</returns>
        public static string AutoAdd(string sInput, bool isAdd)
        {
            if (sInput == null)
                throw new Exception("输入的自增字符串不合法");
            if (sInput == string.Empty)
                throw new Exception("输入的自增字符串不合法");

            int nVar = 0, nInc = 0;
            string sTemp = string.Empty;
            char[] chars = new char[sInput.Length];

            if (isAdd)
                nInc = 1;
            else
                nInc = -1;

            for (int nI = sInput.Length; nI > 0; nI--)
            {
                nVar = (byte)sInput[nI - 1];
                if (nVar >= (byte)'0' && nVar <= (byte)'9')
                {
                    if (nVar + nInc > (byte)'9')
                    {
                        nVar = (byte)'0';
                        if (nI == 1)
                            sTemp = "1";
                    }
                    else if (nVar + nInc < (byte)'0')
                        nVar = (byte)'9';
                    else
                    {
                        nVar = nVar + nInc;
                        nInc = 0;
                    }
                }
                else if (nVar >= (byte)'a' && nVar <= (byte)'z')
                {
                    if (nVar + nInc > (byte)'z')
                    {
                        nVar = (byte)'a';
                        if (nI == 1)
                            sTemp = "a";
                    }
                    else if (nVar + nInc < (byte)'a')
                        nVar = (byte)'z';
                    else
                    {
                        nVar = nVar + nInc;
                        nInc = 0;
                    }
                }
                else if (nVar >= (byte)'A' && nVar <= (byte)'Z')
                {
                    if (nVar + nInc > (byte)'Z')
                    {
                        nVar = (byte)'A';
                        if (nI == 1)
                            sTemp = "A";
                    }
                    else if (nVar + nInc < (byte)'A')
                        nVar = (byte)'Z';
                    else
                    {
                        nVar = nVar + nInc;
                        nInc = 0;
                    }
                }
                else
                {
                    throw new Exception("第" + nI.ToString() + "位格式不正确！无法实现自动增长。");
                }

                chars[nI - 1] = (char)nVar;
            }

            String returnStr = new String(chars);
            if (sTemp == string.Empty)
                return returnStr;
            else
                return sTemp + returnStr;
        }

        // 获取时间字符串,年(4)月(2)日(2)小数(2)分(2)秒(2)毫秒(3)随机数(2),共19位
        private static string GetTime()
        {
            DateTime time = DateTime.Now;
            //if (time > DateTime.Parse("2011-4-20"))
            //    throw new Exception("软件已超过有效使用期,请联系软件开发商");
            //else
            return GetYear(time.Year) + GetMonth(time.Month) + GetDay(time.Day) + GetHour(time.Hour) + GetMinute(time.Minute) + GetSecond(time.Second) + GetID(5, DangqianID);
        }
        public static int DangqianID = 0;

        private static string GetID(int length, int dqdID)
        {
            DangqianID = dqdID + 1;
            if (DangqianID > 99999)
                DangqianID = 1;
            string str = "";
            for (int i = 0; i < length - DangqianID.ToString().Length; i++)
            {
                str = str + "0";
            }
            return str + DangqianID.ToString();
        }


        private static string GetYear(int year)
        {
            if (year < 1000)
            {
                year = 1000;
            }
            return year.ToString();
        }

        private static string GetMonth(int month)
        {
            if (month < 10)
            {
                return "0" + month.ToString();
            }
            return month.ToString();
        }

        private static string GetDay(int day)
        {
            if (day < 10)
            {
                return "0" + day.ToString();
            }
            return day.ToString();
        }

        private static string GetHour(int hour)
        {
            if (hour < 10)
            {
                return "0" + hour.ToString();
            }
            return hour.ToString();
        }

        private static string GetMinute(int minute)
        {
            if (minute < 10)
            {
                return "0" + minute.ToString();
            }
            return minute.ToString();
        }

        private static string GetSecond(int second)
        {
            if (second < 10)
            {
                return "0" + second.ToString();
            }
            return second.ToString();
        }

        private static string GetMillisecond(int millisecond)
        {
            if (millisecond < 10)
            {
                return "00" + millisecond.ToString();
            }
            if (millisecond < 100)
            {
                return "0" + millisecond.ToString();
            }
            return millisecond.ToString();
        }

        //随机数
        private static int random = 0;

        private static string GetRandom()
        {
            random++;
            if (random == 100)
            {
                random = 1;
            }
            if (random < 10)
            {
                return "0" + random.ToString();
            }
            return random.ToString();
        }
        #endregion
    }
}
