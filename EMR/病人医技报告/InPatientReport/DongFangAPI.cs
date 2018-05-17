using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DrectSoft.Core.InPatientReport
{
    public class DongFangAPI
    {
        [DllImport("EncodeURL.dll")]
        private static extern int EncodeURLParamStr(string pParamStr, StringBuilder pResultStr);


        /// <summary>
        /// 获取加密字符
        /// </summary>
        /// <param name="_strvalue"></param>
        /// <returns></returns>
        public static string GetEncodeURLParamStr(string _strvalue)
        {
            try
            {
                string result = "";
                if (_strvalue == "")
                {
                    return "";
                }
                StringBuilder sb = new StringBuilder();
                sb.Capacity = 1000;
                if (EncodeURLParamStr(_strvalue, sb) <= 0)
                {
                    return "";
                }
                result = sb.ToString();
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
    }
}
