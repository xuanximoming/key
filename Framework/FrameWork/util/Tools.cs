using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace DrectSoft.FrameWork
{
    public partial class Tools
    {
        #region "判断一个string是否为数字"
        public bool IsNumber(String strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
                   !objTwoDotPattern.IsMatch(strNumber) &&
                   !objTwoMinusPattern.IsMatch(strNumber) &&
                   objNumberPattern.IsMatch(strNumber);
        }

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
        public static bool IsUnsign(string value)
        {
            return Regex.IsMatch(value, @"^\d*[.]?\d*$");
        }


        #endregion

        /// <summary>
        /// 判断a是否b的整数倍
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static int ModNumeric(decimal a, decimal b)
        {
            int result = 1;
            decimal tmpA = a;

            if (Decimal.ToDouble(Math.Abs(a - b)) < 0.00001) return 1;
            if (a < b) return -1;
            do
            {
                result++;
                tmpA = tmpA - b;
                if (Decimal.ToDouble(Math.Abs(tmpA - b)) < 0.00001) return result;
                if (tmpA < 0) return -1;
            }
            while (true);
        }

        /// <summary>
        /// 获取当前Process
        /// </summary>
        /// <returns></returns>
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //Loop through the running processes in with the same name 
            foreach (Process process in processes)
            {
                //Ignore the current process 
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file. 
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //Return the other process instance. 
                        return process;
                    }
                }
            }
            //No other instance was found, return null. 
            return null;
        }

        /**/
        /// <summary> 
        /// 金额小写变大写 
        /// </summary> 
        /// <param name="smallnum"></param> 
        /// <returns></returns> 
        public static string upperMoney(decimal smallnum)
        {
            string cmoney, cnumber, cnum, cnum_end, cmon, cno, snum, sno;
            int snum_len, sint_len, cbegin, zflag, i;
            if (smallnum > 1000000000000 || smallnum < -99999999999 || smallnum == 0)
                return "";
            cmoney = "仟佰拾亿仟佰拾万仟佰拾元角分";// 大写人民币单位字符串 
            cnumber = "壹贰叁肆伍陆柒捌玖";// 大写数字字符串 
            cnum = "";// 转换后的大写数字字符串 
            cnum_end = "";// 转换后的大写数字字符串的最后一位 
            cmon = "";// 取大写人民币单位字符串中的某一位 
            cno = "";// 取大写数字字符串中的某一位 

            snum = Math.Round(smallnum, 2).ToString("############.00"); ;// 小写数字字符串 
            snum_len = snum.Length;// 小写数字字符串的长度 
            sint_len = snum_len - 2;// 小写数字整数部份字符串的长度 
            sno = "";// 小写数字字符串中的某个数字字符 
            cbegin = 15 - snum_len;// 大写人民币单位中的汉字位置 
            zflag = 1;// 小写数字字符是否为0(0=0)的判断标志 
            i = 0;// 小写数字字符串中数字字符的位置 

            if (snum_len > 15)
                return "";
            for (i = 0; i < snum_len; i++)
            {
                if (i == sint_len - 1)
                    continue;


                cmon = cmoney.Substring(cbegin, 1);
                cbegin = cbegin + 1;
                sno = snum.Substring(i, 1);
                if (sno == "-")
                {
                    cnum = cnum + "负";
                    continue;
                }
                else if (sno == "0")
                {
                    cnum_end = cnum.Substring(cnum.Length - 2, 1);
                    if (cbegin == 4 || (cbegin == 8 || cnum_end.IndexOf("亿") >= 0 || cbegin == 12))
                    {
                        cnum = cnum + cmon;
                        if (cnumber.IndexOf(cnum_end) >= 0)
                            zflag = 1;
                        else
                            zflag = 0;
                    }
                    else
                    {
                        zflag = 0;
                    }
                    continue;
                }
                else if (sno != "0" && zflag == 0)
                {
                    cnum = cnum + "零";
                    zflag = 1;
                }
                cno = cnumber.Substring(System.Convert.ToInt32(sno) - 1, 1);
                cnum = cnum + cno + cmon;
            }
            if (snum.Substring(snum.Length - 2, 1) == "0")
            {
                return cnum + "整";
            }
            else
                return cnum;
        }

        public static double ChinaRound(double value, int decimals)
        {
            if (value < 0)
            {
                return Math.Round(value + 5 / Math.Pow(10, decimals + 1), decimals, MidpointRounding.AwayFromZero);
            }
            else
            {
                return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
            }
        }

        public static double Round(double value, int digit)
        {
            double vt = Math.Pow(10, digit);
            double vx = value * vt;

            vx += 0.5;
            return (Math.Floor(vx) / vt);
        }

        /// <summary>
        /// 判断是否包含中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IfContainsChinese(string str)
        {
            if(Regex.IsMatch(@str,@"[\u4e00-\u9fa5]+$")) 
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 实现数据的四舍五入法
        /// </summary>
        /// <param name="v">要进行处理的数据</param>
        /// <param name="x">保留的小数位数</param>
        /// <returns>四舍五入后的结果</returns>
        public static double Round2(double v, int x)
        {
            bool isNegative = false;
            //如果是负数
            if (v < 0)
            {
                isNegative = true;
                v = -v;
            }

            int IValue = 1;
            for (int i = 1; i <= x; i++)
            {
                IValue = IValue * 10;
            }
            double Int = Math.Round(v * IValue + 0.5, 0);
            v = Int / IValue;

            if (isNegative)
            {
                v = -v;
            }
            return v;
        }

        public static int GetByteLength(string text)
        {
            return Encoding.Default.GetBytes(text).Length;
        }

        public static int GetLength(string Str)
        {
            return Regex.Replace(Str.Trim(), "[^\0-\x00ff]", "**").Length;
        }

        /// <summary>
        /// 按指定长度(单字节)截取字符串
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="len">截取字节数</param>
        /// <returns>string</returns>
        public static string CutStringByte(string str, int startIndex, int len)
        {
            if (str == null || str.Trim() == "")
            {
                return "";
            }
            if (Encoding.Default.GetByteCount(str) < startIndex + 1 + len)
            {
                return str;
            }
            int i = 0;//字节数
            int j = 0;//实际截取长度
            foreach (char newChar in str)
            {
                if ((int)newChar > 127)
                {
                    //汉字
                    i += 2;
                }
                else
                {
                    i++;
                }
                if (i > startIndex + len)
                {
                    str = str.Substring(startIndex, j);
                    break;
                }
                if (i > startIndex)
                {
                    j++;
                }
            }
            return str;
        }

        /// <summary>
        /// 是否包含中文
        /// </summary>
        /// <param name="textboxTextStr"></param>
        /// <returns></returns>
        public static bool IsContainChinese(string textboxTextStr)
        {
            foreach (char ch in textboxTextStr)
            {
                if (GetByteLength(ch.ToString()) > 1)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsDigital(string sentence)
        {
            GetLength("");
            string str = "0123456789";
            foreach (char ch in sentence)
            {
                if (str.IndexOf(ch) == -1)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 是否中文
        /// </summary>
        /// <param name="C"></param>
        /// <returns></returns>
        public static bool IsChinese(string C)
        {
            if (Convert.ToInt32(Convert.ToChar(C)) >= Convert.ToInt32(Convert.ToChar(128)))
            {
                return true;// 是
            }
            else
            {
                return false;// 否
            }
        }

        /// <summary>
        /// 获取字符拼音简码
        /// </summary>
        /// <param name="cnChar"></param>
        /// <returns></returns>
        private static string getSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        byte[] newbyte = new byte[] { (byte)(65 + i) };
                        return Encoding.Default.GetString(newbyte, 0, newbyte.Length);

                    }
                }
                return "*";
            }
            else return cnChar;
        }

        /// <summary>
        /// 获取字符串拼音码
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                if (IsChinese(strText.Substring(i, 1)))
                {
                    myStr += getSpell(strText.Substring(i, 1));
                }
                else
                {
                    myStr += strText.Substring(i, 1);
                }
            }
            return myStr;
        }

        /// <summary>
        /// 是否为数字或英文字母
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public static bool IsDigitalOrLetter(string sentence)
        {
            bool flag = true;
            foreach (char ch in sentence)
            {
                if ((((ch >= '0') && (ch <= '9')) || ((ch >= 'A') && (ch <= 'Z'))) || ((ch >= 'a') && (ch <= 'z')))
                {
                    flag = true;
                }
                else
                {
                    return false;
                }
            }
            return flag;
        }

        public static bool IsFloat(string strValue)
        {
            return Regex.IsMatch(strValue.Trim(), @"^(-?\d+)(\.\d+)?$");
        }

        public static bool IsValidEnName(string EnName)
        {
            return Regex.IsMatch(EnName.Trim(), "^[a-zA-Z]+$");
        }

        public static bool IsZFloat(string strValue)
        {
            return Regex.IsMatch(strValue.Trim(), @"^(\d+)(\.\d+)?$");
        }

        /// <summary>
        /// 将List<string>列表转换为以split分隔的字符串
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string List2String(List<string> SQLStringList, string split)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < SQLStringList.Count; i++)
            {
                if (builder.Length < 1)
                {
                    builder.Append(SQLStringList[i].ToString());
                }
                else
                {
                    builder.Append(split + SQLStringList[i].ToString());
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 例：partof("A:B", ':', 0)=="A", partof("A:B", ':', 1)=="B"
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ch"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string partof(string s, char ch, int index)
        {
            if (s != null)
            {
                return s.Split(new char[] { ch })[index];
            }
            return null;
        }

        /// <summary>
        /// SizeToString(new Size(3, 4)) == "3:4"
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string SizeToString(Size size)
        {
            return (size.Width.ToString() + ":" + size.Height.ToString());
        }
        /// <summary>
        /// StringToSize("3:4") == Size(3, 4)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Size StringToSize(string s)
        {
            string str = partof(s, ':', 0);
            string str2 = partof(s, ':', 1);
            return new Size(Convert.ToInt32(str), Convert.ToInt32(str2));
        }

        public static DataTable StringsToDataTable(string[] strings, Type type)
        {
            if ((strings == null) || (strings.Length < 1))
            {
                return null;
            }
            DataTable table = new DataTable();
            table.Columns.Add("key", type);
            table.Columns.Add("value", typeof(string));
            int num = 0;
            foreach (string str in strings)
            {
                DataRow row = table.NewRow();
                row[0] = num++;
                row[1] = str;
                table.Rows.Add(row);
            }
            return table;
        }

        public static DataTable StringsToDataTable(string[] texts, string[] values, Type type)
        {
            if ((texts == null) || (texts.Length < 1))
            {
                return null;
            }
            if (values.Length != texts.Length)
            {
                return null;
            }
            DataTable table = new DataTable();
            table.Columns.Add("key", type);
            table.Columns.Add("value", typeof(string));
            for (int i = 0; i < texts.Length; i++)
            {
                DataRow row = table.NewRow();
                row[0] = values[i];
                row[1] = texts[i];
                table.Rows.Add(row);
            }
            return table;
        }

        public static string ToDBC(string input)
        {
            char[] chArray = input.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (chArray[i] == '　')
                {
                    chArray[i] = ' ';
                }
                else if ((chArray[i] > 0xff00) && (chArray[i] < 0xff5f))
                {
                    chArray[i] = (char)(chArray[i] - 0xfee0);
                }
            }
            return new string(chArray);
        }
        public static string ToSBC(string input)
        {
            char[] chArray = input.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (chArray[i] == ' ')
                {
                    chArray[i] = '　';
                }
                else if (chArray[i] < '\x007f')
                {
                    chArray[i] = (char)(chArray[i] + 0xfee0);
                }
            }
            return new string(chArray);
        }

        [DllImport("kernel32.dll", EntryPoint = "GetSystemDefaultLCID")]
        public static extern int GetSystemDefaultLCID();
        [DllImport("kernel32.dll", EntryPoint = "SetLocaleInfoA")]
        public static extern int SetLocaleInfo(int Locale, int LCType, string lpLCData);

        public const int LOCALE_SLONGDATE = 0x20;
        public const int LOCALE_SSHORTDATE = 0x1F;
        public const int LOCALE_STIME = 0x1E;

        public static void setLocalDateFormat(string strDateFormat, string strTimeFormat)
        {
            int x = GetSystemDefaultLCID();
            SetLocaleInfo(x, LOCALE_SSHORTDATE, strDateFormat);   //短日期格式   
            SetLocaleInfo(x, LOCALE_SLONGDATE, strDateFormat);   //长日期格式   
            SetLocaleInfo(x, LOCALE_STIME, strTimeFormat);   //时间格式
        }
        public static void setLocalDateFormat()
        {
            string strDateFormat = "yyyy-MM-dd";
            string strTimeFormat = "H:mm:ss";
            setLocalDateFormat(strDateFormat, strTimeFormat);
        }

        public static string getLocalLongDateFormat()
        {
            System.Globalization.CultureInfo current = System.Globalization.CultureInfo.CurrentCulture;
            return current.DateTimeFormat.LongDatePattern.ToString();
        }
        public static string getLocalShortDateFormat()
        {
            System.Globalization.CultureInfo current = System.Globalization.CultureInfo.CurrentCulture;
            return current.DateTimeFormat.ShortDatePattern.ToString();
        }
        public static string getLocalLongTimeFormat()
        {
            System.Globalization.CultureInfo current = System.Globalization.CultureInfo.CurrentCulture;
            return current.DateTimeFormat.LongTimePattern.ToString();
        }
    }
}
