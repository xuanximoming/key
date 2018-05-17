using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DrectSoft.Wordbook
{
    /// <summary>
    /// 提供一些公共的处理方法
    /// </summary>
    public static class CommonOperation
    {
        /// <summary>
        /// 得到传入的字符串中字符的类型            edit by wangj 2013 1 17
        /// </summary>
        /// <param name="input">要判断类型的字符串</param>
        /// <returns></returns>
        public static StringType GetStringType(string input)
        {
            if (String.IsNullOrEmpty(input))
                return StringType.Empty;

            int index;
            bool hasMinus = (input[0] == '-'); // 标记第一个字符是否是负号
            if (hasMinus) // 如果还有负号，则从第二个字符开始处理
                index = 1;
            else
                index = 0;

            StringType type = StringType.Char;
            for (; index < input.Length; index++)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(input[index]))
                {

                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.ModifierLetter:
                    case UnicodeCategory.TitlecaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                        // 遇到全角字母则直接设为其它类型
                        if ((input[index] >= 65281) && (input[index] < 65373))
                        {
                            type = StringType.Other;
                        }
                        else
                        {
                            if (index == 0)
                                type = StringType.EnglishChar;
                            else
                            {
                                // 只有前面已判断出来的类型是全数字，才需要改变类型
                                if (type == StringType.Numeric)
                                    type = StringType.Char;
                            }
                        }
                        break;
                    case UnicodeCategory.OtherLetter:
                        type = StringType.Other;
                        break;
                    case UnicodeCategory.DecimalDigitNumber:
                        if (index == 0)
                            type = StringType.Numeric;
                        else
                        {
                            // 只有前面已判断出来的类型是全英文字母，才需要改变类型
                            if (type == StringType.EnglishChar)
                                type = StringType.Char;
                        }
                        //break; 
                        continue;                                                            //edit by wangj 2013 1 17
                    default: type = StringType.Other;                                        //add by wangj 2013 1 17
                        break;
                }
                // 如果已经判断出来的类型是其它，则可以直接退出循环
                if (type == StringType.Other)
                    break;
            }
            if (hasMinus && (type != StringType.Numeric))
                return StringType.Other;

            return type;
        }

        /// <summary>
        /// 对传入的条件表达式字符串中的特殊字符进行转义。
        /// 在生成DataView或DataColumn的Expression时需要进行此处理
        /// </summary>
        /// <param name="condition">Expression 中的条件表达式</param>
        /// <param name="operaName">操作符</param>
        /// <returns></returns>
        public static string TransferCondition(string condition, CompareOperator operaName)
        {
            if (condition == null)
                throw new ArgumentNullException(MessageStringManager.GetString("NullParameter"));
            // 现在只处理Like的表达式
            switch (operaName)
            {
                case CompareOperator.Like:
                    if (condition.Contains("["))
                        condition = condition.Replace("[", "[[]");
                    if (condition.Contains("*"))
                        condition = condition.Replace("*", "[*]");
                    if (condition.Contains("%"))
                        condition = condition.Replace("%", "[%]");
                    if (condition.Contains("_"))
                        condition = condition.Replace("_", "[_]");
                    break;
                default:
                    break;
            }

            return condition;
        }

        /// <summary>
        /// 对传入的条件表达式字符串中的特殊字符进行转义。
        /// 在生成DataView或DataColumn的Expression时需要进行此处理
        /// </summary>
        /// <param name="condition">Expression 中的条件表达式</param>
        /// <param name="operaName">操作符</param>
        /// <param name="handleQuote">是否处理单引号</param>
        /// <returns></returns>
        public static string TransferCondition(string condition, CompareOperator operaName, bool handleQuote)
        {
            if (condition == null)
                throw new ArgumentNullException(MessageStringManager.GetString("NullParameter"));
            // 单引号替换成两个单引号
            if (handleQuote)
                condition = condition.Replace("'", "''");

            return TransferCondition(condition, operaName);
        }

        /// <summary>
        /// 根据操作符枚举变量得到实际的操作符
        /// </summary>
        /// <param name="filterOperator">操作符枚举变量</param>
        /// <returns>实际的操作符</returns>
        public static string GetOperatorSign(CompareOperator filterOperator)
        {
            switch (filterOperator)
            {
                case CompareOperator.Equal:
                    return "=";
                case CompareOperator.In:
                    return "IN";
                case CompareOperator.Less:
                    return "<";
                case CompareOperator.Like:
                    return "LIKE";
                case CompareOperator.More:
                    return ">";
                case CompareOperator.NotEqual:
                    return "<>";
                case CompareOperator.NotLess:
                    return ">=";
                case CompareOperator.NotMore:
                    return "<=";
                default:
                    throw new ArgumentOutOfRangeException(MessageStringManager.GetString("OperatorSignNotDefined"));
            }
        }

        /// <summary>
        /// 根据传入的Wordbook关键信息字符串生成字典实例
        /// </summary>
        /// <param name="keyInfo">字典的关键信息组成的字符串</param>
        /// <returns>字典类实例</returns>
        public static BaseWordbook GetWordbookByString(string keyInfo)
        {
            if (String.IsNullOrEmpty(keyInfo))
                return null;
            //throw new ArgumentNullException("字典类名为空");

            // 解析出字典类名字
            int p = keyInfo.IndexOf(SeparatorSign.OtherSeparator);
            if (p < 0)
                return null;
            //throw new ArgumentException("未定义字典类名");
            keyInfo = keyInfo.Substring(p + 3, keyInfo.Length - p - 3);
            p = keyInfo.IndexOf(SeparatorSign.OtherSeparator);
            if (p < 0)
                return null;

            BaseWordbook wordbook = WordbookStaticHandle.GetWordbook(keyInfo.Substring(0, p));
            if (wordbook != null)
                wordbook.ParameterValueComb = keyInfo.Substring(p + 3, keyInfo.Length - p - 3);
            return wordbook;
        }
    }
}
