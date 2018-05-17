using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DrectSoft.Core.NurseDocument
{
    public abstract class Dataprocessing
    {
        /// <summary>
        /// 判断是否为指定带几位小数的数字
        /// </summary>
        /// <param name="strNumber">判断的数据</param>
        /// <param name="intdecimals">小数位数，9999为任意数</param>
        /// <returns> true是数字，false非数字</returns>
        public static bool IsNumber(String strNumber, int intdecimals)
        {
            string[] pattern = new string[]
            {
                @"^[0-9]*$", //intdecimals=0
                @"^[0-9]*[.]?[0-9]?$", // @"(^[0-9]*$)|(^[0-9]*[.]?[0-9]$)", //intdecimals=0
                @"^[0-9]*[.]?[0-9]*$"  //default @"^[0-9]*.[0-9]*$" 
            };

            Match match;

            switch (intdecimals)
            {
                case 0: //整数
                    {
                        match = Regex.Match(strNumber, pattern[0]);   // 匹配正则表达式    
                        break;
                    }
                case 1: //一位小数
                    {
                        match = Regex.Match(strNumber, pattern[1]);   // 匹配正则表达式    
                        break;
                    }
                default: //任意数字（21 、23.33）
                    {
                        match = Regex.Match(strNumber, pattern[pattern.Length - 1]);   // 匹配正则表达式    
                        break;
                    }
            }

            return match.Success;//false不是数字,true是数字
        }

    }
}
