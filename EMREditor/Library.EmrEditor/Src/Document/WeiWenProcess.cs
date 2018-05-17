
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace DrectSoft.Library.EmrEditor.Src.Document
{
    /// <summary>
    /// 左 <- 光标 -> 右
    /// 中间和起始为充要条件
    /// 单独和接尾为充要条件
    /// 单独（接尾）和 中间（起始）为 左 <- 右
    /// </summary>
    public class WeiWenProcess
    {
        /// <summary>
        /// 接收文档内容
        /// </summary>
        internal static ZYTextDocument document;

        internal static ZYTextDocument myDocument
        {
            get { return document; }
            set
            {
                document = null;
                document = value;
            }
        }

        private static ArrayList myChars = new ArrayList();
        internal static bool boolEnter = false;
        /// <summary>
        /// 临时屏蔽调整维文模式
        /// </summary>
        public static bool weiwen = false;
        /// <summary>
        /// 维文字典结构
        /// </summary>
        private static int[,] Harip;

        #region 初始化维文书写样式
        /// <summary>
        /// 初始化维文书写样式
        /// </summary>
        static WeiWenProcess()
        {
            document = null;
            Harip = new int[5, 34];
            //单独形式
            WeiWenProcess.Harip[0, 0] = int.Parse("FE89", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 1] = int.Parse("FE8D", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 2] = int.Parse("FEE9", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 3] = int.Parse("FE8F", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 4] = int.Parse("FB56", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 5] = int.Parse("FE95", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 6] = int.Parse("FE9D", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 7] = int.Parse("FB7A", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 8] = int.Parse("FEA5", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 9] = int.Parse("FEA9", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 10] = int.Parse("FEAD", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 11] = int.Parse("FEAF", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 12] = int.Parse("FB8A", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 13] = int.Parse("FEB1", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 14] = int.Parse("FEB5", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 15] = int.Parse("FECD", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 16] = int.Parse("FED1", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 17] = int.Parse("FED5", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 18] = int.Parse("FED9", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 19] = int.Parse("FB92", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 20] = int.Parse("FBD3", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 21] = int.Parse("FEDD", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 22] = int.Parse("FEE1", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 23] = int.Parse("FEE5", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 24] = int.Parse("FBAA", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 25] = int.Parse("FEED", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 26] = int.Parse("FBD7", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 27] = int.Parse("FBD9", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 28] = int.Parse("FBDB", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 29] = int.Parse("FBDE", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 30] = int.Parse("FBE4", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 31] = int.Parse("FEEF", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 32] = int.Parse("FEF1", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[0, 33] = int.Parse("FEFB", System.Globalization.NumberStyles.HexNumber);
            //接尾形式
            WeiWenProcess.Harip[1, 0] = int.Parse("FE8A", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 1] = int.Parse("FE8E", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 2] = int.Parse("FEEA", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 3] = int.Parse("FE90", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 4] = int.Parse("FB57", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 5] = int.Parse("FE96", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 6] = int.Parse("FE9E", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 7] = int.Parse("FB7B", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 8] = int.Parse("FEA6", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 9] = int.Parse("FEAA", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 10] = int.Parse("FEAE", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 11] = int.Parse("FEB0", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 12] = int.Parse("FB8B", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 13] = int.Parse("FEB2", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 14] = int.Parse("FEB6", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 15] = int.Parse("FECE", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 16] = int.Parse("FED2", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 17] = int.Parse("FED6", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 18] = int.Parse("FEDA", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 19] = int.Parse("FB93", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 20] = int.Parse("FBD4", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 21] = int.Parse("FEDE", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 22] = int.Parse("FEE2", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 23] = int.Parse("FEE6", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 24] = int.Parse("FBAB", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 25] = int.Parse("FEEE", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 26] = int.Parse("FBD8", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 27] = int.Parse("FBDA", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 28] = int.Parse("FBDC", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 29] = int.Parse("FBDF", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 30] = int.Parse("FBE5", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 31] = int.Parse("FEF0", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 32] = int.Parse("FEF2", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[1, 33] = int.Parse("FEFC", System.Globalization.NumberStyles.HexNumber);
            //中间形式
            WeiWenProcess.Harip[2, 0] = int.Parse("FE8C", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 3] = int.Parse("FE92", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 4] = int.Parse("FB59", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 5] = int.Parse("FE98", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 6] = int.Parse("FEA0", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 7] = int.Parse("FB7D", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 8] = int.Parse("FEA8", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 13] = int.Parse("FEB4", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 14] = int.Parse("FEB8", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 15] = int.Parse("FED0", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 16] = int.Parse("FED4", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 17] = int.Parse("FED8", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 18] = int.Parse("FEDC", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 19] = int.Parse("FB95", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 20] = int.Parse("FBD6", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 21] = int.Parse("FEE0", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 22] = int.Parse("FEE4", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 23] = int.Parse("FEE8", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 24] = int.Parse("FBAD", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 30] = int.Parse("FBE7", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 31] = int.Parse("FBE9", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[2, 32] = int.Parse("FEF4", System.Globalization.NumberStyles.HexNumber);
            //起始形式
            WeiWenProcess.Harip[3, 0] = int.Parse("FE8B", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 3] = int.Parse("FE91", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 4] = int.Parse("FB58", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 5] = int.Parse("FE97", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 6] = int.Parse("FE9F", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 7] = int.Parse("FB7C", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 8] = int.Parse("FEA7", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 13] = int.Parse("FEB3", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 14] = int.Parse("FEB7", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 15] = int.Parse("FECF", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 16] = int.Parse("FED3", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 17] = int.Parse("FED7", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 18] = int.Parse("FEDB", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 19] = int.Parse("FB94", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 20] = int.Parse("FBD5", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 21] = int.Parse("FEDF", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 22] = int.Parse("FEE3", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 23] = int.Parse("FEE7", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 24] = int.Parse("FBAC", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 30] = int.Parse("FBE6", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 31] = int.Parse("FBE8", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[3, 32] = int.Parse("FEF3", System.Globalization.NumberStyles.HexNumber);
            //原型形式
            WeiWenProcess.Harip[4, 0] = int.Parse("0626", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 1] = int.Parse("0627", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 2] = int.Parse("06D5", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 3] = int.Parse("0628", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 4] = int.Parse("067E", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 5] = int.Parse("062A", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 6] = int.Parse("062C", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 7] = int.Parse("0686", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 8] = int.Parse("062E", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 9] = int.Parse("062F", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 10] = int.Parse("0631", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 11] = int.Parse("0632", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 12] = int.Parse("0698", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 13] = int.Parse("0633", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 14] = int.Parse("0634", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 15] = int.Parse("063A", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 16] = int.Parse("0641", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 17] = int.Parse("0642", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 18] = int.Parse("0643", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 19] = int.Parse("06AF", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 20] = int.Parse("06AD", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 21] = int.Parse("0644", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 22] = int.Parse("0645", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 23] = int.Parse("0646", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 24] = int.Parse("06BE", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 25] = int.Parse("0648", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 26] = int.Parse("06C7", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 27] = int.Parse("06C6", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 28] = int.Parse("06C8", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 29] = int.Parse("06CB", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 30] = int.Parse("06D0", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 31] = int.Parse("0649", System.Globalization.NumberStyles.HexNumber);
            WeiWenProcess.Harip[4, 32] = int.Parse("064A", System.Globalization.NumberStyles.HexNumber);
            //WeiWenProcess.Harip[4, 33] = int.Parse("FEFB", System.Globalization.NumberStyles.HexNumber);
        }
        #endregion


        #region 文档判断函数群
        /// <summary>
        /// 当前光标处左右两边是维文
        /// </summary>
        /// <returns></returns>
        internal static bool LeftAndRightIsWeiWen()
        {
            if (isWeiwen(WeiWenProcess.document.Content.GetFontChar(0).Char) && isWeiwen(WeiWenProcess.document.Content.GetPreChar(1).Char) && WeiWenProcess.document.Content.GetFontChar(0).Left != 0)
                return true;
            return false;
        }

        /// <summary>
        /// 字符是否为中文
        /// </summary>
        /// <param name="myChar"></param>
        /// <returns></returns>
        internal static bool isChenise(char myChar)
        {
            if (Regex.IsMatch(myChar.ToString(), @"[\u4e00-\u9fbb]"))
                return true;
            return false;
        }
        /// <summary>
        /// 字符元素是否为维文
        /// </summary>
        /// <returns></returns>
        public static bool isWeiwen(Char myChar)
        {
            bool result = false;
            int S = (int)myChar;
            if (S == 32) //32  ' '代表一个空格
            {
                result = false;
            }
            else
            {

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 34; j++)
                    {
                        if (Harip[i, j] == S)
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 判断维文字符类型
        /// </summary>
        /// <param name="myChar">字符</param>
        /// <returns>类型 0-单独（原型） 1-接尾 2-中间 3-起始</returns>
        internal static int weiwenType(char myChar)
        {
            int S = (int)myChar;
            for (int i = 0; i < 34; i++)
            {
                //单独形式 
                if (Harip[0, i] == S || Harip[4, i] == S)
                    return 0;
                //接尾形式
                if (Harip[1, i] == S)
                    return 1;
                //中间形式
                if (Harip[2, i] == S)
                    return 2;
                //起始形式
                if (Harip[3, i] == S)
                    return 3;
            }
            return 0;
        }
        #endregion

        #region 维文字符转换函数群
        /// <summary>
        /// 维文转换成正确格式,单独 || 接尾 -> 起始 || 中间
        /// </summary>
        /// <returns></returns>
        private static string ChangeWeiwenFrom(Char myChar)
        {
            int S = (int)myChar;
            for (int j = 0; j < 34; j++)
            {
                //单独形式（原型形式）  转换成  起始形式
                if (Harip[0, j] == S | Harip[4, j] == S)
                {
                    int i = Harip[3, j];
                    if (i == 0)
                    {
                        i = Harip[0, j];
                    }
                    return i.ToString("X4");
                }
                //接尾形式  转换成  中间形式
                if (Harip[1, j] == S)
                {
                    int i = Harip[2, j];
                    if (i == 0)
                    {
                        i = Harip[1, j];
                    }
                    return i.ToString("X4");
                }
            }
            return S.ToString("X4");
        }


        /// <summary>
        /// 维文模式下，更改维文形式
        /// </summary>
        /// <param name="myChar">字符</param>
        /// <param name="right"> 是否为右边字符</param>
        /// <returns>更改后的UNICODE编码</returns>
        private static string ChangeFromweiwen(char myChar, bool right)
        {
            int S = (int)myChar;
            for (int j = 0; j < 34; j++)
            {
                if (right)
                {
                    //中间  转换成  接尾形式
                    if (Harip[2, j] == S)
                    {
                        int i = Harip[1, j];
                        return i.ToString("X4");
                    }
                    //起始形式  转换成  单独形式
                    if (Harip[3, j] == S)
                    {
                        int i = Harip[0, j];
                        return i.ToString("X4");
                    }
                }
                else
                {
                    //中间  转换成  起始形式
                    if (Harip[2, j] == S)
                    {
                        int i = Harip[3, j];
                        return i.ToString("X4");
                    }
                    //接尾形式  转换成  单独形式
                    if (Harip[1, j] == S)
                    {
                        int i = Harip[0, j];
                        return i.ToString("X4");
                    }
                }

            }
            return S.ToString("X4");
        }

        #endregion


        #region string <----> Unicode
        /// <summary>
        /// String 型数据转换成Unicode
        /// </summary>
        /// <param name="keyChar"></param>
        /// <returns></returns>
        private static string StringToUnicode(String keyChar)
        {
            if (keyChar.Trim().Equals("")) return "";
            byte[] buffer;
            buffer = System.Text.Encoding.Unicode.GetBytes(keyChar);
            return String.Format("{0:X2}{1:X2}", buffer[1], buffer[0]);
        }


        /// <summary>
        /// Unicode 型数据转换成 String 
        /// </summary>
        /// <param name="keyChar"></param>
        /// <returns></returns>
        private static string UnicodeToString(string unicode)
        {
            if (unicode.Trim().Equals("")) return "";
            byte[] bytes = new byte[2];
            bytes[1] = byte.Parse(int.Parse(unicode.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString());
            bytes[0] = byte.Parse(int.Parse(unicode.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString());
            return System.Text.Encoding.Unicode.GetString(bytes);
        }

        #endregion

        #region 处理输入方式函数群

        #region 处理按键的单个字符
        /// <summary>
        /// 处理按键的单个字符
        /// </summary>
        /// <param name="e">键盘事件</param>
        public static void KeyPress(KeyPressEventArgs e)
        {
            try
            {
                int S = (int)e.KeyChar;
                for (int i = 0; i < 34; i++)
                {
                    //判断输入的字符编码
                    if (Harip[4, i] == S)
                    {
                        ZYTextChar myLeftChar;
                        ZYTextChar myRightChar;
                        //维文模式  得到 光标 -> 右    ,后 <- 光标 -> 前
                        myRightChar = WeiWenProcess.document.Content.GetPreChar(1);
                        //得到 左 <- 光标
                        myLeftChar = WeiWenProcess.document.Content.GetFontChar(0);
                        if (WeiWenProcess.isWeiwen(myRightChar.Char))
                        {
                            //转换右侧一个字符 
                            myRightChar.Char = UnicodeToString(ChangeWeiwenFrom(myRightChar.Char)).ToCharArray()[0];
                            myRightChar.RefreshSize();
                            switch (weiwenType(myRightChar.Char))
                            {
                                case 0:
                                    //光标在维文左侧
                                    e.KeyChar = UnicodeToString(Harip[0, i].ToString("X4")).ToCharArray()[0];
                                    break;
                                case 1:
                                    //光标在维文左侧
                                    e.KeyChar = UnicodeToString(Harip[0, i].ToString("X4")).ToCharArray()[0];
                                    break;
                                default:
                                    //光标在维文左侧
                                    e.KeyChar = UnicodeToString(Harip[1, i].ToString("X4")).ToCharArray()[0];
                                    break;
                            }
                            return;
                        }
                        else if (WeiWenProcess.isWeiwen(myLeftChar.Char))
                        {
                            //右侧不是维文，左侧是维文
                            //移动到维文字左侧
                            int j = 0;
                            do
                            {
                                j++;
                                //维文模式左右相反
                                WeiWenProcess.document._MoveRight();

                            } while (WeiWenProcess.isWeiwen(WeiWenProcess.document.Content.GetFontChar(0).Char));

                            //转换右侧一个字符，移动后重新获取前一个字符
                            myRightChar = WeiWenProcess.document.Content.GetPreChar(1);
                            myRightChar.Char = UnicodeToString(ChangeWeiwenFrom(myRightChar.Char)).ToCharArray()[0];
                            myRightChar.RefreshSize();
                            //移动到左边后按照右边是维文处理，返回接尾形式
                            switch (weiwenType(myRightChar.Char))
                            {
                                case 0:
                                    //光标在维文左侧
                                    e.KeyChar = UnicodeToString(Harip[0, i].ToString("X4")).ToCharArray()[0];
                                    break;
                                case 1:
                                    //光标在维文左侧
                                    e.KeyChar = UnicodeToString(Harip[0, i].ToString("X4")).ToCharArray()[0];
                                    break;
                                default:
                                    //光标在维文左侧
                                    e.KeyChar = UnicodeToString(Harip[1, i].ToString("X4")).ToCharArray()[0];
                                    break;
                            }
                            return;
                        }
                        //前一个不是维文，当前字符为 单独形式
                        e.KeyChar = UnicodeToString(Harip[0, i].ToString("X4")).ToCharArray()[0];
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 处理粘贴字符



        private static bool WeiwenYS(char myChar)
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 34; j++)
                {
                    if (Harip[i, j] == (int)myChar && Harip[2, j] == 0)
                        return true;
                }
            return false;
        }
        /// <summary>
        /// 处理粘贴方式传入的字符
        /// </summary>
        /// <param name="myChar">当前字符</param>
        /// <param name="myPreChar">前一个字符</param>
        /// <param name="myFontChar">后一个字符</param>
        /// <returns></returns>
        public static char strPase(char myChar, char myPreChar, char myFontChar)
        {
            int S = (int)myChar;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 34; j++)
                {
                    if (Harip[i, j] == S)
                    {
                        if (isWeiwen(myFontChar) && isWeiwen(myPreChar))
                        {
                            int intMyChar;
                            if (WeiwenYS(myPreChar))
                                intMyChar = Harip[3, j];
                            else
                                intMyChar = Harip[2, j];
                            if (intMyChar == 0)
                                intMyChar = Harip[1, j];
                            return UnicodeToString(intMyChar.ToString("X4")).ToCharArray()[0];
                        }
                        else if (!isWeiwen(myFontChar) && !isWeiwen(myPreChar))
                        {
                            return UnicodeToString(Harip[0, j].ToString("X4")).ToCharArray()[0];
                        }
                        else if (isWeiwen(myFontChar) && !isWeiwen(myPreChar))
                        {
                            int intMyChar = Harip[3, j];
                            if (intMyChar == 0)
                                intMyChar = Harip[1, j];
                            return UnicodeToString(intMyChar.ToString("X4")).ToCharArray()[0];
                        }
                        else if (!isWeiwen(myFontChar) && isWeiwen(myPreChar))
                        {
                            if (WeiwenYS(myPreChar))
                                return UnicodeToString(Harip[0, j].ToString("X4")).ToCharArray()[0];
                            return UnicodeToString(Harip[1, j].ToString("X4")).ToCharArray()[0];
                        }
                    }
                }
            return myChar;
        }

        #endregion

        #region 退格键删除后，更改最后一个字符模式


        public static void KeyBackspace()
        {
            //如果上一次是在维文中间回车换行
            if (WeiWenProcess.boolEnter)
            {
                ZYTextElement CurrentElement = WeiWenProcess.document.Content.CurrentElement;
                if (WeiWenProcess.document.Content.GetPreElement(CurrentElement) is ZYTextEOF &&
                    isWeiwen(WeiWenProcess.document.Content.GetPreChar(2).Char))
                {
                    ZYTextChar PreChar = WeiWenProcess.document.Content.GetPreChar(2);
                    (CurrentElement as ZYTextChar).Char = (char)myChars[0];
                    (CurrentElement as ZYTextChar).RefreshSize();

                    PreChar.Char = (char)myChars[1];
                    PreChar.RefreshSize();
                    boolEnter = false;
                    return;
                }

            }
            ZYTextChar myChar = WeiWenProcess.document.Content.GetPreChar(1);
            if (isWeiwen(myChar.Char))//当前要删除的是维文
            {
                switch (weiwenType(myChar.Char))
                {
                    //0-单独 1-接尾 2-中间 3-起始
                    //单独（原型） 左 false 右 false  左右不用改变任何
                    //左右都不是维文，那么左右两个相接不是维文相接，跟当前字符删不删除是一样
                    // 中间形式 左 true  右 true  左右不用改变
                    //左右都是维文，那么左右两个相接依然是维文相接，跟当前字符删不删除是一样
                    case 1:
                        //接尾形式 左 false  右 true 左不用改变  右要改变(中间 -> 接尾 或 起始 -> 单独)
                        ZYTextChar RightChar = WeiWenProcess.document.Content.GetPreChar(2);
                        RightChar.Char = UnicodeToString(ChangeFromweiwen(RightChar.Char, true)).ToCharArray()[0];
                        RightChar.RefreshSize();
                        break;
                    case 3:
                        //起始形式 左 true 右 false 左用改变 右不用改变
                        ZYTextChar LeftChar = WeiWenProcess.document.Content.GetFontChar(0);
                        LeftChar.Char = UnicodeToString(ChangeFromweiwen(LeftChar.Char, false)).ToCharArray()[0];
                        LeftChar.RefreshSize();
                        break;
                    default:
                        break;
                }
            }
        }


        #endregion

        #region  回车键处理

        internal static void KeyEnterDown()
        {
            //两个维文中间回车换行
            WeiWenProcess.boolEnter = true;
            ZYTextChar myCurrentChar = WeiWenProcess.document.Content.GetPreChar(0);
            myChars.Add(myCurrentChar.Char);
            ZYTextChar myPreChar = WeiWenProcess.document.Content.GetPreChar(1);
            myChars.Add(myPreChar.Char);
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 34; j++)
                {
                    if (Harip[i, j] == (int)myCurrentChar.Char)
                    {
                        switch (weiwenType(myCurrentChar.Char))
                        {
                            case 1:
                                myCurrentChar.Char = UnicodeToString(Harip[0, j].ToString("X4")).ToCharArray()[0];
                                myCurrentChar.RefreshSize();
                                break;
                            case 2:
                                myCurrentChar.Char = UnicodeToString(Harip[3, j].ToString("X4")).ToCharArray()[0];
                                myCurrentChar.RefreshSize();
                                break;
                            default:

                                break;
                        }
                    }

                    if (Harip[i, j] == (int)myPreChar.Char)
                    {
                        switch (weiwenType(myPreChar.Char))
                        {
                            case 3:
                                myPreChar.Char = UnicodeToString(Harip[0, j].ToString("X4")).ToCharArray()[0];
                                myPreChar.RefreshSize();
                                break;
                            case 2:
                                myPreChar.Char = UnicodeToString(Harip[1, j].ToString("X4")).ToCharArray()[0];
                                myPreChar.RefreshSize();
                                break;
                            default:

                                break;
                        }
                    }

                }

        }

        #endregion

        #endregion
    }
}
