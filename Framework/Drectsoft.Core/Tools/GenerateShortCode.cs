using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data;
using System.Collections;

namespace DrectSoft.Core
{
    /// <summary>
    /// Îª×Ö·û´®Éú³ÉÆ´Òô¡¢Îå±Ê´úÂëËõÐ´
    /// </summary>
    public class GenerateShortCode
    {
        #region const
        /// <summary>
        /// Æ´Òô¡¢Îå±Ê×Ö¶ÎµÄ×î´ó³¤¶È
        /// </summary>
        public const int ShortCodeLength = 8;
        /// <summary>
        /// ¿Õ×Ö·û´®£¬³¤¶ÈÓëCODE_LENÒ»Ñù
        /// </summary>
        public const string EmptyShortCode = "\0\0\0\0\0\0\0\0";
        /// <summary>
        /// Ä¬ÈÏµÄÆ´ÒôÁÐÃû
        /// </summary>
        public const string FieldPy = "py";
        /// <summary>
        /// Ä¬ÈÏµÄÎå±ÊÁÐÃû
        /// </summary>
        public const string FieldWb = "wb";

        #endregion

        #region private variable
        /// <summary>
        /// ºº×Ö¿â
        /// </summary>
        private Hashtable m_ChineseTable;

        #endregion

        #region private methods
        /// <summary>
        /// ³õÊ¼»¯ºº×Ö¿âµÄHashtable
        /// </summary>
        private void InitChineseTable(IDataAccess sqlHelper)
        {
            m_ChineseTable = new Hashtable();

            char[] code;
            string command = "select Chinese,  py,  wb from CCLIB";
            DataTable chineseTable = sqlHelper.ExecuteDataTable
                (command, true, CommandType.Text);
            foreach (DataRow row in chineseTable.Rows)
            {
                code = new char[2];
                row["py"].ToString().CopyTo(0, code, 0, 1);
                row["wb"].ToString().CopyTo(0, code, 1, 1);
                if (!m_ChineseTable.ContainsKey(row["Chinese"].ToString().ToCharArray()[0]))
                    m_ChineseTable.Add(row["Chinese"].ToString().ToCharArray()[0], code);
            }
        }

        /// <summary>
        /// Îª´«ÈëµÄ×Ö·û´®Éú³ÉÆ´Òô¡¢Îå±Ê´úÂë¡£´úÂë±£´æÔÚ´«ÈëµÄÊý×éÖÐ
        /// </summary>
        /// <param name="chinese">ÒªÉú³É´úÂëµÄ×Ö·û´®</param>
        /// <param name="py">±£´æÆ´Òô´úÂëµÄÊý×é</param>
        /// <param name="wb">±£´æÎå±Ê´úÂëµÄÊý×é</param>
        private void GenerateStringShortCode(string chinese, char[] py, char[] wb)
        {
            int cutLength = ShortCodeLength + 4;
            if (chinese.Length < cutLength)
                cutLength = chinese.Length;

            // Öð×ÖÉú³É´úÂë£¬Ö»Éú³ÉÖÐÎÄ×ÖµÄ´úÂë£¬Ó¢ÎÄ×ÖÄ¸×ª³ÉÐ¡Ð´£¬Êý×Ö¡¢±êµã·ûºÅµÈ¶¼Ìø¹ý
            char[] name = new char[cutLength];

            // ¶àÈ¡¼¸¸ö×Ö£¬ÕâÑùÔÚÌø¹ý·ûºÅ¡¢Êý×ÖµÈÄÚÈÝºó£¬Ò»°ãÈÔÄÜ±£Ö¤ÓÐ8¸öÓÐÐ§ºº×Ö¿ÉÒÔÓÃÀ´Éú³É´úÂë
            chinese.CopyTo(0, name, 0, cutLength);

            char[] code;
            int codeIndex = 0;
            EmptyShortCode.CopyTo(0, py, 0, ShortCodeLength);
            EmptyShortCode.CopyTo(0, wb, 0, ShortCodeLength);

            for (int index = 0; (index < cutLength) && (codeIndex < ShortCodeLength); index++)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(name[index]))
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.ModifierLetter:
                    case UnicodeCategory.TitlecaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                        // È«½Ç×ÖÄ¸µÄÒª×ª³É°ë½Ç
                        if ((name[index] >= 65281) && (name[index] < 65373))
                            name[index] = (char)(name[index] - 65248);
                        if (name[index] < 127)
                        {
                            py[codeIndex] = Char.ToLower(name[index], CultureInfo.CurrentCulture);
                            wb[codeIndex] = py[codeIndex];
                            codeIndex++;
                        }
                        break;
                    case UnicodeCategory.OtherLetter:
                        code = m_ChineseTable[name[index]] as char[];
                        if (code != null)
                        {
                            py[codeIndex] = code[0];
                            wb[codeIndex] = code[1];
                            codeIndex++;
                        }
                        break;
                }
            }
        }
        #endregion

        /// <summary>
        /// Îª±íÊÖ¹¤Ìí¼ÓÆ´Òô¡¢Îå±Ê×Ö¶Î£¨Èç¹û²»´æÔÚµÄ»°£©£¬²¢Ìî³äÆ´Òô»òÎå±Ê´úÂëÎª¿ÕµÄÐÐ
        /// </summary>
        /// <param name="table">Òª´¦ÀíµÄ±í</param>
        /// <param name="nameField">Ãû³Æ×Ö¶Î</param>
        /// <returns></returns>
        public void AutoAddShortCode(DataTable table, string nameField)
        {
            if (table == null)
                throw new ArgumentNullException("±íÎª¿Õ");
            if (String.IsNullOrEmpty(nameField))
                throw new ArgumentNullException("Î´´«ÈëÃû³Æ×Ö¶Î");

            DataColumn column;
            if (!table.Columns.Contains(FieldPy))
            {
                column = new DataColumn(FieldPy, typeof(string));
                table.Columns.Add(column);
            }
            if (!table.Columns.Contains(FieldWb))
            {
                column = new DataColumn(FieldWb, typeof(string));
                table.Columns.Add(column);
            }

            GenerateTableShortCode(table, nameField, true, true, true);
        }

        /// <summary>
        /// ´´½¨Éú³ÉÆ´Òô¡¢Îå±Ê´úÂëËõÐ´µÄÀàÊµÀý¡£ÐèÌá¹©sqlHelperÊµÀý£¬ÒÔ±ãÈ¡³ö»ù´¡Êý¾Ý
        /// </summary>
        /// <param name="sqlHelper"></param>
        public GenerateShortCode(IDataAccess sqlHelper)
        {
            if (sqlHelper == null)
                throw new ArgumentNullException("sqlHelper", "Ìá¹©µÄÊý¾Ý·ÃÎÊ²ã¶ÔÏóÎª¿Õ");
            InitChineseTable(sqlHelper);
        }

        /// <summary>
        /// Îª´«ÈëµÄ×Ö·û´®Éú³ÉÆ´ÒôºÍÎå±Ê´úÂë
        /// </summary>
        /// <param name="source">ÒªÉú³É´úÂëµÄ×Ö·û´®</param>
        /// <returns></returns>
        public string[] GenerateStringShortCode(string source)
        {
            string[] code;
            if (string.IsNullOrEmpty(source))
            {
                code = new string[2] { "", "" };
            }
            else
            {
                //char[] py = new char[ShortCodeLength];
                // char[] wb = new char[ShortCodeLength];

                // GenerateStringShortCode(source, py, wb);
                //  code = new string[2] { new string(py), new string(wb) };
                string py = ChineseConverter.GetChineseSpell(source);
                string wb = ChineseConverter.GetChineseWB(source);
                code = new string[2] { py, wb };

            }
            return code;
        }

        /// <summary>
        /// ÎªDataTableÖÐµÄÃû³Æ×Ö¶ÎÉú³É×ÖÍ·ËõÐ´¡£ÐèÒªÖ¸¶¨Ãû³Æ×Ö¶Î¡£Æ´Òô¡¢Îå±Ê±ØÐë´æÔÚ£¬ÇÒÊ¹ÓÃÄ¬ÈÏÁÐÃû£¨py/wb£©
        /// </summary>
        /// <param name="sourceTable">ÐèÒªÉú³É´úÂëµÄDataTable</param>
        /// <param name="nameField">Ö¸¶¨µÄÃû³Æ×Ö¶Î</param>
        /// <param name="onlyEmpty">ÊÇ·ñÖ»ÎªÃ»ÓÐÉú³É¹ýÆ´Òô»òÎå±Ê´úÂëµÄÉú³ÉËõÐ´</param>
        /// <param name="needPY">ÊÇ·ñÉú³ÉÆ´ÒôËõÐ´</param>
        /// <param name="needWB">ÊÇ·ñÉú³ÉÎå±ÊËõÐ´</param>
        /// <returns>Éú³É½á¹ûÐÅÏ¢£¬·Ç¿Õ±íÊ¾ÓÐ´íÎóÐÅÏ¢</returns>
        public string GenerateTableShortCode(DataTable sourceTable, string nameField, bool onlyEmpty, bool needPY, bool needWB)
        {
            if (sourceTable == null) return "DataTable Î´¸³Öµ";
            if (string.IsNullOrEmpty(nameField)) return "Î´Ö¸¶¨Ãû³Æ×Ö¶Î";

            if ((!needPY) && (!needWB))
                return "";

            char[] py = new char[ShortCodeLength];
            char[] wb = new char[ShortCodeLength];

            foreach (DataRow row in sourceTable.Rows)
            {
                if ((onlyEmpty) && ((!needPY) || (row["py"].ToString().Trim().Length > 0))
                      && ((!needWB) || (row["wb"].ToString().Trim().Length > 0)))
                    continue;

                GenerateStringShortCode(row[nameField].ToString(), py, wb);

                if (needPY)
                    row["py"] = new string(py);
                if (needWB)
                    row["wb"] = new string(wb);
            }
            return "";
        }
    }

    public class ChineseConverter
    {
        #region ÖÐÎÄ×ªÆ´ÒôÊ××ÖÄ¸
        public static string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += GetSpell(strText.Substring(i, 1));
            }
            return myStr.ToLower();
        }
        //ÓÃÀ´»ñµÃÒ»¸ö×ÖµÄÆ´ÒôÊ××ÖÄ¸
        private static string GetSpell(string cnChar)
        {
            //½«ºº×Ö×ª»¯ÎªASNIÂë,¶þ½øÖÆÐòÁÐ
            byte[] arrCN = System.Text.Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217,45253,45761,46318,46826,47010,
                                    47297,47614,48119,48119,49062,49324,
                                    49896,50371,50614,50622,50906,51387,
                                    51446,52218,52698,52698,52698,52980,
                                    53689,54481
                                    };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return System.Text.Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "Z"; // return "*";
            }
            else
                return cnChar;
        }
        #endregion

        #region ÖÐÎÄ×ªÎå±ÊÊ××ÖÄ¸


        /// <summary>
        /// Îå±ÊÊ××ÖÄ¸×Ö¿â
        /// </summary>
        private static String[] wbLib = new String[] {
            "A÷¹÷¸÷·÷¶÷µ÷´÷³÷²÷±÷°öÆõ¼ôëôèòËò©ðÙðÅð´ð°í«êîêêêÛê±ê¬éÑåÂß°ß¯ß®ÞþÞôÞÃÞÂÞÁÞÀÞ¿Þ¾Þ½Þ¼Þ»ÞºÞ¹Þ¸Þ·Þ¶ÞµÞ´Þ³Þ²Þ±Þ°Þ¯Þ®Þ­Þ¬Þ«ÞªÞ©Þ¨Þ§Þ¦Þ¥Þ¤Þ£Þ¢Þ¡ÝþÝýÝüÝûÝúÝùÝøÝ÷ÝöÝõÝôÝóÝòÝñÝðÝïÝîÝíÝìÝëÝêÝéÝèÝçÝæÝåÝäÝãÝâÝáÝàÝßÝÞÝÝÝÜÝÛÝÚÝÙÝØÝ×ÝÖÝÕÝÔÝÓÝÒÝÑÝÐÝÏÝÎÝÍÝÌÝËÝÊÝÉÝÈÝÇÝÆÝÅÝÄÝÃÝÂÝÁÝÀÝ¿Ý¾Ý½Ý¼Ý»ÝºÝ¹Ý¸Ý·Ý¶ÝµÝ´Ý³Ý²Ý±Ý°Ý¯Ý®Ý­Ý¬Ý«ÝªÝ©Ý¨Ý§Ý¦Ý¥Ý¤Ý£Ý¢Ý¡ÜþÜýÜüÜûÜúÜùÜøÜ÷ÜöÜõÜôÜóÜòÜñÜðÜïÜîÜíÜìÜëÜêÜéÜèÜçÜæÜåÜäÜãÜâÜáÜàÜßÜÞÜÝÜÜÜÛÜÚÜÙÜØÜ×ÜÖÜÕÜÔÜÓÜÒÜÑÜÐÜÏÜÎÜÍÜÌÜËÜÊÜÉÜÈÜÇÜÆÜÅÜÄÜÃÜÂÜÁÜÀÜ¿Ü¾Ü½Ü¼Ü»ÜºÜ¹Ü¸Ü·Ü¶ÜµÜ´Ü³Û´Û±ÚöØåØáØÓØÒØÑØÐØÏØÎØ¥×ÂÖøÖ¥ÕôÕåÕáÕºÔåÔáÔÑÔÌÔ·ÓóÓ«ÓªÓ©Ó¨Ó¢ÒñÒðÒÕÒÃÒ½Ò©Ò¢ÑàÑÅÑÀÑ¿Ñ»Ñ¦Ñ¥ÐîÐ¾Ð½Ð°Ð¬ÏôÏïÏîÏ»ÎõÎôÎßÎ×ÎÔÎµÎ®Î­ÌÙÌÑÌ¦ËòËâËÕË¹ÊíÊßÊÀÊ½ÉõÉÖÉ»É¯É¢ÈøÈôÈïÈãÈÙÈØÈ×ÈÖÈÇÈµÈ§ÇøÇæÇÛÇÚÁ«ÀóÀòÀÙÀÕÀÍÀ¶À³¿û¿ï¿à¿Ö¿Á¿±¾ú¾Þ¾Õ¾Ï¾´¾¯¾¥¾£½ù½æ½å½ä½Ú½¶½³½¯¼ö¼ë¼Ô¼»»ù»ó»ò»ñ»çÇÑÇÐÇÌÇÊÇÉÇ¾ÆäÆßÆÛÆÚÆÑÆÐÆÏÆÎÆ¼Æ»Æ¥ÅîÅºÅ¹Å¸Å·ÄõÄèÄäÄ½Ä¼Ä»ÄºÄ¹Ä³ÄªÄ¢Ä¡ÃïÃêÃçÃÉÃÈÃ¯Ã©Ã§Ã£Ã¢ÂûÂäÂÜÂ«Áâ»Æ»Ä»®»¨ºùºÊºÉºª¹½¹¶¹²¹±¹¯¹®¹§¹¦¹¥¹¤¸ð¸ï¸ê¸Ê·Ò·Ë·Æ·¼·¶·ª·¡¶­¶«µÙµ´´Ð´Ä³¼²è²ç²Ý²Ø²Ô²Ì²Ë²¤±Þ±Î±Í±½±¡°ú°Ð°Å°°°¬°ª",
            "BôÐò¿ò¨ñùñøñ÷ñöñõñôæïæßæÞæÝåøãÄÛÉÛÂÛ¸ÚôÚóÚòÚñÚðÚïÚîÚíÚìÚëÚêÚéÚèÚçÚæÚåÚäÚãÚâÚáØ½Ø©×è×Ó×Î×¹Ö°ÕóÕÏÔÉÔºÓçÒþÒõÒ²Ò®ÑôÑ·ÏÞÏÝÏÕÏ¶ÎÀÍÓÌÕËïËîËíËæËåÉÂÈîÈæÈ¢È¡ÁËÁÉÁÄÁª¿×¾Û½×½µ¼ÊÆ¸ÅãÄôÄ°ÃÏÂ½ÂªÂ¤Â¡Áêº¯º¢¹Â¹¢¸ô¸½·À¶ú¶é¶Ó¶¸µ¢´Ï³ý³ö³Ü³Ð³Â±Ý°¯°¢",
            "CöÊó±òúòªñæñåðÖðÍð®î¦í¡ìÆë§æøæ÷æöæõæôæóæòæðæîæíæìæëæêæéæèæçæææäæãæâæáæàæÄåÒåÊåÉÛÏÛÎÛÍÛÌÛ¢ØÙ×¤ÖèÔéÔÊÔ¦Ô¥Ô¤ÓèÓÖÓÂÓÁÒÔÒÓÑéÑ±ÐÜÏ·ÍÕÍÔÍ¨Ì¨Ë«Ê»Ê¥É§É£ÈþÈáÈ°ÇýÀÝ¿¥¾Ô¾±¾¢½¾¼è¼¦ÆïÆ­ÄÜÄÑÄ²Ã¬ÂíÂæÂâÂ¿»¶º§¹Û¶ÔµþµËµ¡³Û³Ò²æ²Î²µ°Í",
            "D÷à÷ß÷Þ÷Ý÷Ü÷Û÷Ú÷Ù÷Ø÷×÷Ö÷Õ÷Ô÷Êõ¾õ»ôäô©òãò×ò²ñòñññðñïñîñíñìñëñêñéñèñçðÓðÉðÆð¹ð³ð­íçíæíåíäíãíâíáíßíÞíÝíÜíÛíÚíÙíØí×íÖíÕíÔíÓíÒíÑíÐíÏíÍíÌíËíÊíÉíÈíÇíÆíÅíÄíÃíÂíÁíÀí¿í¾í½í¼í»íºí¹í¸í·í¶í¤í¢ìâì³ì­ì¥ëéêüê°ê©åçÞÏÞÎÞÍÞÌÞËÞÇÞÆÞÅÛ½ÛºØãØÞØÚØÍØÌØËØÊØÉØÈØÇØÆØÅ×ó×à×××©ÕèÕÉÔÚÔÒÔÅÔ¸Ô­ÓôÓÒÓÑÓÐÓÈÓ²Ò³ÑãÑâÑáÑÞÑÙÑÐÑ¹ÐçÐÛÏõÏáÏÌÏÄÏÃÏ®ÎùÎøÎìÍþÍòÍëÌüÌ×Ì¼Ì¬Ì«Ì©ËéËÁË¶Ë¬Ë£ÊùÊÙÊ¯Ê¢ÉéÉÝÉ°ÈýÈèÈ·È®ÇØÀúÀùÀøÀ÷ÀåÀÚ¿ü¿ø¿ó¿ä¿Ë¿Ä¿³¾Ç¾Â¾¤½¸¼ï¼î»Û»Ò»ÇÇ£ÆöÆõÆæÆÝÆÆÅøÅöÅðÅéÅáÅÕÄëÄÎÄÍÃæÂõÂëÂµÂ¢ÁûÁúÁòÁ×ºúºñºÄº»¹è¹Ë¹Ê¹Å¹¼¹¨¸û¸Ð·î·á·Ü·Ç·¯¶ø¶ò¶áµúµïµâ´ó´ï´è´æ´Å´À´½´º´¡³ø³É³½³§²ê²Þ²¼±Ã±¼±²±¯±®°õ°ï°î°Ù°Ò°­",
            "EöÂõùõøõ÷õöõõõôõ¹ôíðÎìÞì¢ì¡ëþëýëüëûëúëùëøë÷ëõëôëóëòëñëðëïëîëíëìëëëêëèëçëæëåëäëãëâëáëàëßëÞëÝëÜëÛëÚëÙëØë×ëÖëÕëÔëÓëÒëÑëÏëÎëÍëÌëËëÊëÉëÇëÆëÅëÄëÃëÂëÀë¿ë¾ë½ë¼ê®æÚæØåãáêáÙÞÉÛÒÛ®Ø¾ÖúÖðÖâÖ×Ö¬Ö«ÕÍÔàÔÐÔÂÓÃÓ·Ó¯ÒÜÒÈÒ¸Ò¨Ò£ÑüÐüÐëÐØÐÈÐ²ÏÙÏØÏ¥ÍóÍ×ÍÑÍÈÌóÌÚÌÅÌ¥Ë´Ë¦ÊÜÊ¤ÉÅÈùÈéÁ³ÀßÀ°¿è¾ô½Å½º¼°¼¡ÇÒÇ»ÆêÆ¢ÅôÅóÅòÅßÅÖÅ§ÄåÄÔÄËÄ¤ÃÙÃ²ÂöºÑ¹É¸ì¸Ø¸Î¸¹¸­¸¬·þ·ô·Î·Ê·¾¶Çµ¨´à³¦²Ê²É²²²±±ª°û°ò°¹°·°®",
            "Fö½ö²ö±ö°ö¯ö®ö­ö¬ö«öªö©ö¨ö§ö¥ô÷ôöôõôôôóôòôñôðôãôÃóäóÀó§ò«ò¡ñüñóð¾îÁî­íàí¨í£ìäì²ì±ì°ë£êíêëêåêÚêÈê´êªéýèºè¹æËåÜåÓåÏÜ²Ü±Ü°Ü¯Ü®Ü­Ü¬Ü«ÜªÜ©Ü¨Ü§Ü¦Ü¥Ü¤Ü£Ü¢Ü¡ÛþÛýÛüÛûÛúÛùÛøÛ÷ÛöÛõÛôÛóÛñÛðÛïÛîÛíÛìÛëÛêÛéÛèÛçÛæÛåÛäÛãÛâÛáÛàÛßÛÞÛÝÛÜÛÛÛÚÛÙÛØÛ×Û¹ÚõØÔØÄØÃØÁØ£Ø¡×ß×¨ÖóÖ¾Ö·Ö±Ö§ÕðÕæÕßÕÔÔöÔØÔÕÔÔÔËÔÆÔ½Ô¶Ô¬Ô«ÔªÓòÓêÒ¼ÑßÑÏÑÎÑ©ÐèÐæÐÒÐ­Ð¢ÏöÏ×ÏÖÏ¼Ï²ÎíÎëÎÞÎÓÎ´Î¥Î¤ÍçÍÁÌîÌæÌËÌÁÌ¹Ì³Ì®ËþËúË÷ËÂËªÊ¿Ê¾Ê®ÉùÉâÉÊÉ¥ÈÍÈÀÈ´È¥È¤Ç÷ÇóÀ×ÀÏÀ¬À¤¿÷¿î¿é¿å¿Ó¿Ç¿À¿¼¿²¿°¿¯¾ù¾È¾³¾®½ø½Ù½Ø½Ì¼Î¼ª»ø»÷»ê»ÜÇ½Ç¬ÆðÆÒÆÂÆºÅùÅ÷ÅíÅàÄÞÄÏÃ¹ÂôÂñÂ¶ÁãÁØ»µºøºÕºÂº¾º²º«¹ý¹ç¹æ¹Ä¹¸¹¡¸Ï¸É¸°·ò·â·Ø·»¶þ¶â¶Õ¶Ñ¶Â¶¼¶¯µßµØµÌ´÷´ç´£³á³à³Ç³Ã³¯³¬³¡²Å²Ã²º²ª²©±¢°Ô°Ó°£",
            "G÷Ñ÷Ð÷¡öËö¦ôùôøôïôîôçó¼òüò³ñúðÄð¿ìýì£ë·êãê¯ê§ê¦éìéëéêéééèéçéæéåéäéãéâéáéÒè¶è³è²è±è°è¯è®è­è¬è«èªè©è¨è§è¦è¥è¤è£è¢è¡çþçýçüçûçúçùçøç÷çöçõçóçòçñçðçïçîçíçìçëçêçéçèçççæçåçäçãçâçáçàæñåÛåÎåÍãÃÛÔÛÑÛ³ÛªÛ¤Û£ÚüÚûØÝØÂØ¬Ø«ØªØ¨Ø§Ø¦Ø¤×Á×¸ÖéÖÂÖÁÖ³ÕþÕýÕûÕéÕäÕµÔðÔæÔâÔÙÓñÓëÓÛÓÚÓØÒÄÒ»ÑþÑêÑÉÑÇÑ³ÐÏÐÎÐÍÐÌÏÂÎåÎäÎáÍõÍæÍãÍáÍßÍÍÍÌÌìËöËÙËØËÀÊøÊâÊÂÊ´ÉºÉªÈðÈÚÇòÇíÇàÇÙÁÑÁÐÁ½Á§ÀöÀíÀÅÀµÀ´¿ª¾Á¾²½ú¼í¼ß¼Õ¼Ð¼¬»ô»ÝÆÞÆ½ÅýÅÃÅªÄÒÄ©ÃðÃµÂóÂêÁðÁáÁÕÁÒ»¹»·»­»¥º÷¹å¸ü¸±¸¦·ó·ñ·©¶ñ¶Ù¶¾¶º¶¹µåµ½´ù´ø´õ´Ì²Ü²Ð²Ï²»²£±û±í±Ì±Æ±Â°à°ß°¾°½",
            "Hò®öÄö»öºö¹ö¸ö·ö¶öµö´ö³ö¤õþõºôÓò¯ò­ò¬ðµî¬î«îªî©î¨î§î¥î¤î£î¢î¡íþíýíüíûíúíùíøí÷íöíõíôíñíðíïíîíííìíÎìþë¬êïêèêßê·åáÛÖÛÇØÕØÀØ­×Ï×À×¿ÖõÖ¹ÕöÕêÕ½Õ¼Õ°Õ£ÓÝÑÛÑ£ÐéÐ©Ï¹Í¹Í«Ì÷Ë²Ë¯ÊåÉÏÈ£¿ô¿Ï¿¨¾ß¾É¾¦½ÞÇÆÆçÆÄÆµÆ¤ÅÎÅ°ÄÀÄ¿ÃéÃßÃÐÂ÷ÂÇÂ²Â±Â­Â¬Áä»¢¶Ã¶½¶¦¶¢µãµÉ´Ë´Æ³ò³Ý²ñ²Í²Ç²½²·±ë",
            "Iöèö×öÌöÈôÄíµí´í³í¯ë©êýæÙæ¶åÐå±å°å¯å®å­å¬å«åªå©å¨å§å¦å¥å¤å£å¢å¡äþäýäüäûäúäùäøä÷äöäõäôäóäòäñäðäïäîäíäìäëäêäéäèäçäæäåäääãäâäáäàäßäÞäÝäÜäÛäÚäÙäØä×äÖäÕäÔäÓäÒäÑäÐäÏäÎäÍäÌäËäÊäÉäÈäÇäÆäÅäÄäÃäÂäÁäÀä¿ä¾ä½ä¼ä»äºä¹ä¸ä·ä¶äµä´ä³ä²ä±ä°ä¯ä®ä­ä¬ä«äªä©ä¨ä§ä¦ä¥ä¤ä£ä¢ä¡ãþãýãüãûãúãùãøã÷ãöãõãôãóãòãñãðãïãîãíãìãëãêãéãèãçãæãåãäãããâãáãàãßà·ÛÊÙäØ»×Õ×Ò×Í×Ì×¢ÖÞÖÎÖÍÖ­ÕãÕ×ÕÓÕÇÕÆÕÄÕ¿Õ´ÔüÔóÔèÔ´Ô¨Ô¡ÓþÓæÓåÓÙÓÎÓÍÓ¿Ó¾ÒùÒçÒÊÒºÒ«ÑúÑóÑÝÑÍÑÄÑ´Ñ§ÐÚÐËÐºÐ¹Ð¤Ð¡ÏýÏûÏ÷ÏæÏÑÏ´Ï«ÏªÎÛÎÖÎÐÎÂÎ¼Î«ÍôÍåÍÝÍÄÍ¿Í¡ÌíÌéÌÔÌÓÌÏÌÎÌÌÌÊÌÄÌÃÌÀÌ¶Ì²Ì­ËÝË®ÊþÊçÊªÊ¡ÉøÉòÉîÉæÉÙÉÑÉÐÉÍÉÇÉ³É¬È÷ÈóÈêÈÜÈ¾È¸ÇþÇöÇåÇßÁÊÁ»ÁºÁ°Á¤ÀìÀáÀÔÀËÀÄÀ½À£¿Ê¿£¾õ¾Ú¾Ù¾Æ½þ½ò½à½½½­½§½¦½¥¼â¼Ã¼¹¼³¼¤»î»ì»ë»ã»ÔÇ³Ç±Ç¢ÆüÆûÆãÆâÆáÆÙÆÖÆÅÆÃÆ¯ÅìÅæÅÝÅËÅÉÅÈÅ½Å¨Å¢ÄùÄçÄàÄ×Ä®Ä­ÃìÃÚÃ»ÂþÂúÂåÂÙÂÐÂËÂºÂ©Á÷ÁïÁÜÁÓ»Á»´»¬»¦ºþºéºèºÔºÓºÆººº¹º­º£¹â¹à¹Á¹µ¸Û¸È¸¢¸¡·Ú·Ð·º·¨¶ý¶É¶´µíµáµÓµÎµ³µ±µ­´ã´¾³ü³Ø³Î³Á³¾³º³±³¨³£³¢²â²×²´²³²¨±õ±ô°Ä",
            "Jó½óºó¹ó¸ó·ó¶óµó´ó³ó²ó°ó¯ó­ó¬ó«óªó©ó¨ó¦ó¥ó¤ó£ó¢ó¡òþòýòûòùòøò÷òöòõòôòóòòòñòðòïòîòíòìòëòêòéòèòçòæòåòäòâòáòàòßòÞòÝòÜòÛòÚòÙòÖòÕòÔòÓòÒòÑòÐòÏòÎòÍòÌòÊòÉòÈòÇòÆòÅòÄòÃòÂòÁòÀò¾ò½ò¼ò»òºò¹ò¸ò·ò¶òµò´ò±ò°ìãêÙêØê×êÖêÕêÔêÓêÒêÑêÐêÏêÎêÍêÌêËêÊêÉêÇêÅêÄêÂêÁêÀê¿ê¾ê½ê¼ê»êºê¹ê­è¸åßåÝâ·ÛÃØÖØ®×ò×îÖûÖëÖ©ÕÕÕÑÔçÔÎÔ»ÓöÓÞÓ¼Ó³Ó°Ó¬Ò×ÒÏÒ·Ò°ÑÑÑÁÐÇÐ«ÐªÏþÏÔÏÍÏ¾ÏºÎúÎîÎÏÎÃÍúÍíÍÜÍÉÌâÌÞË§ÊûÊúÊïÊîÊÇÊ±Ê¦ÉöÉêÉßÉÎÉ¹ÈäÈÕÇùÇçÁÀÁ¿ÀïÀÀÀ¯À¥¿Å¾°¾§½ô¼ø¼á¼à»Þ»×»Î»ÈÆØÅ¯ÃøÃ÷ÃáÃËÃÁÃ°ÂüÂìÂÝÁÙºûºçºµ¹û¹ö¹é¹Æ¸ò·ä¶ô¶êµûµçµ©³æ³×³¿³©²ý²õ±©°ö°º°µ",
            "K÷Òö¾ö¼õóõòõñõðõïõîõíõìõëõêõéõèõçõæõåõäõãõâõáõàõßõÞõÝõÜõÛõÚõÙõØõ×õÖõÕõÔõÓõÒõÑõÐõÏõÎõÍõÌõËõÊõÉõÈõÇõÆõÅõÄõÃõÂõÁõÀò¦ðØðÊê«è´àìàëàêàéàèàçàæàåàäàãàâàáàààßàÞàÝàÜàÛàÚàÙàØà×àÖàÕàÔàÓàÒàÑàÐàÏàÍàÌàËàÊàÉàÈàÇàÆàÅàÄàÃàÂàÁàÀà¿à¾à½à¼à»àºà¹à¸à¶àµà³à²à±à°à¯à®à­à¬à«àªà©à¨à§à¦à¥à¤à£à¢à¡ßþßýßüßûßúßùßøß÷ßößõßôßóßòßñßðßïßîßíßìßëßêßéßèßçßæßåßäßãßâßáßàßßßÞßÝßÜßÛßÚßÙßØß×ßÖßÕßÔßÓßÒßÑßÐßÏßÎßÍßÌßËßÊßÉßÈßÇßÆßÅßÄßÃßÂßÁßÀß¿ß¾ß½ß¼ß»ßºß¹ß¸ß·ß¶ßµß´ß³ß²Û«×ì×ã×Ù×ÄÖöÖäÖÒÖÑÖÐÖ»ÖºÖ¨Õ¦ÔûÔëÔêÔÛÔÇÔ¾Ô±Ó÷ÓõÓ½Ó»Ó´Ò÷ÒØÒÅÒ¶Ò­Ò§ÑäÑÊÑÆÑ½Ñ«ÐúÐêÐáÐÖÐ¥ÏùÏøÏìÏÅÎüÎûÎâÎØÎËÎÇÎ¹Î¶Î¨ÍÛÍÙÍÂÌýÌøÌçÌäÌãÌßÌ¾Ì¤Ì£ËôËäËÔËÃË»Ë³Ë±ÊÉÊÈÊ·ÉëÉÚÉ¶É¤ÈÂÁ¨À²À®¿õ¿ç¿Þ¿Ú¿Ô¿Ð¿È¿©¿§¿¦¾é¾á¾à¾×½Ð½À¼ùÇºÇ²Æ÷Æ·Æ¡ÅçÅÞÅÜÅØÅ¿Å¾Å»Å¶ÄöÄØÄÅÄÄÃùÂðÂïÂîÂÀÂ·ÁüÁí»½»¼»©»£ºôºðºíºåºßºÙºÈºÇºÅº¿º°¹þ¹ó¹ò¹Ð¹¾¸ú¸Â¸Á¸À·Ô·Í·È¶õ¶å¶ß¶×¶Ö¶£µøµõµðµÅµ¸´ô´Ú´µ´®´­´¨³ù³ì³Ô³Ñ³Ê³³³°³ª²ä²È²¸±ð±É±Ä°Ï°É°È°¦°¥°¡",
            "L÷ö÷õ÷ô÷ò÷ñ÷ð÷ï÷î÷íöÉôÂîÀî¿î¾î½î¼î»îºî¹î¸î·î¶îµî³î²î±î°î¯î®ê¥ê¤ê£ê¢ê¡éþéüéûéúéùéøé÷éöéõéôéóéòéñéðéïéîéíèýæÕåÈà÷àöàõàôàóàòàñàðàïàîàíÛÄÛÁ×ï×Ç×ªÖáÖÃÕÞÕÖÕ·Õ¶ÔþÔÝÔ²Ô°Ô¯ÒòÑ¼ÐùÏ½Î¸Î·Î§ÍÅÍ¼ÌïËÄË¼ÊñÊðÊäÈíÈ¦ÇôÇáÁ¾Á¬Á¦ÀÛÀ§½ç½Ï½Î¼Ý¼Ü¼×¼Ó¼­»û»ØÇµÇ­ÆèÆÔÅþÅÏÄÐÄ¬Ä«ÃóÂßÂÞÂÖÂÔºäºÚºØ¹ú¹õ¹ì¹Ì¸¨·ø·£¶÷¶Ú³ë³µ±ß°ì°Õ",
            "M÷Ç÷Æ÷Å÷Ä÷Ã÷Â÷Á÷À÷¿÷¾÷½÷¼÷»÷ºó¿ò§ðÐì¯ì®ì¬ì«ì©ëÐêéêçêæêäêâêáêàêÝêÜå×åÄáÛáÚáØá×áÖáÕáÔáÓáÑáÐáÏáÎáÍáÌáËáÊáÉáÇáÆáÅáÄáÃáÂáÁáÀá¿á¾á½á¼á»áºá¹á¸á¶áµá´á³á²á±á°á¯á­á¬á«áªá©á¨á§á¦á¥á¤á£á¢á¡àþàýàüàúàùàøÙîÙíØèØçØÜØÛ×¬ÖüÖÜÖÅÖÄÖ¡ÕËÕÊÕ¸ÔùÔôÔòÔßÓøÓìÓÊÓÉÓ¤ÒÙÑìÑëÑÒÑÂÏ¿Î¡ÍøÍ®Í¬ÌûÌùÌ¿ËêËèÊêÉÞÉÄÉ¾É½ÈâÈ½Çú¿ù¿­¾þ½í¼û¼ú¼¸»ß»Ï»ËÇÍÇ¶ÆñÆéÅâÅÁÄÚÃ±Â¸Áëº¡¹Ç¹º¸Ú¸Õ¸Ô¸³·ù·ï·ç·å···²·«¶ë¶ç¶ä¶ÄµñµäµÏµ¤´Þ´Í´±³ç²á²Æ±á±À±´°Ü°¼°»°¶",
            "NöÍôàôÅñãñâðÒðÌíªìÙë¢ê¶éÞèµæÔåñåðåïåîåíåìåëåêåÚãÞãÝãÂãÁãÀã¿ã¾ã½ã¼ã»ãºã¹ã¸ã·ã¶ãµã´ã³ã²ã±ã°ã¯ã®ã­ã¬ã«ãªã©ã¨ã§ã¦ã¥ã¤ã£ã¢ã¡âþâýâüâûâúâùâøâ÷âöâõâôâóâòâñâðâïâîâíâìâëâêâéâèâçâæâåâäâãâââáâàáÒáÈÞÊÙãØ¿ÖçÕúÕ¹Ô÷ÔÃÓðÓäÓÇÒîÒíÒìÒäÒÒÒÑÑ¸ÐôÐÔÐÊÐÄÐÃÐ¼Ð¸Ï°Ï¬Ï§ÎòÎÝÎ¿Î¾Î²Î©ÍïÍÎÍÀÌñÌëÌèËÈË¾Ë¢ÊôÊéÊèÊÕÊÑÊºÊ­Ê¬É÷ÉåÇüÇéÁ¯ÀÁÀ¢¿ì¿¶¿®¾ç¾å¾Ö¾Ó¾ª¾¡½ì¼É¼Â¼º»Ú»Ö»Ð»ÌÇÓÇÄÇ¡ÆÁÆ©Æ¨ÅüÅÂÅ³ÄòÄáÄÕÃõÃñÃ¼Ã¦ÂýÂòÂÅÂÄÂ¾»Å»³ºãºÞº·º¶º©¹ß¹Ö¸Ò¸Ä·ß·É·¢¶è¶²¶®µóµîµëµÔµ¿µ¼µ°µ¬´ä´Á³ó³ß³Ù³À²ã²Ò²Ñ²À±Ü±Û±Ú±Ù±Ø°Ã",
            "OôÝôÜôÛôÚôÙôØô×ôÖôÕôÔôÑôÏôÎôÍôÌíëíêíéìáìßìÝìÜìÛìÚìØì×ìÖìÕìÔìÓìÑìÐìÏìÎìÍìÌìËìÊìÉìÈìÇìÅìÄìÃìÂìÁìÀì¿ì¾åàÛÆÛ°ÚþØß×Ñ×ÆÖòÕ³Õ¨ÔïÔîÔäÔãÒµÑæÑ×ÑÌÏ©Ï¨ÍéÌþÌÇË¸ÊýÉÕÉ¿ÈÛÈ¼È²ÁÏÁÇÁ¸Á¶Á£ÀàÀÓÀÃ¿¾¿»¿·¾æ¾¼¾¬¾«½ý»ð»â»ÍÆÉÅÚÅ´Ã×ÃÔÃºÂ¯Â¦»Àºýºæº¸¸â·é·à·Û·³¶ÏµÆ´â´Ö´¸´¶³ã³´²Ú²Ó±þ±º±¬",
            "PñÂñÁñÀñäñáñàñßñÞñÝñÜñÛñÚñÙñØñ×ñÖñÕñÔñÓñÒñÑñÐñÏñÎñÍñÌñËñÊñÉñÈñÇñÆñÅñÄñÃñ¿ñ¾ñ½ñ¼ñ»ñºñ¹ñ¸ñ·ñ¶ð²ìüìûìúìùìøì÷ìöìõìôìóìòìñìðìïìîìíìììëìêåäåÕåÁåÀå¿å¾å½å¼å»åºå¹å¸å·å¶åµå´å³å²ÛÈÛ©Ú¤Ú£Ú¢Øà×æ×Ú×Ö×£ÖæÖÏÖ®Õ¯Õ­Õ¬Ô×ÔÖÔ©Ô£Ô¢ÓîÒúÒËÒ¾Ò¤ÑçÑ¨ÐûÐäÐ´ÏüÏéÏÜÎÑÍðÍêÍàÍÊÍ»Ì»ËüËÞËÎÊØÊÓÊÒÊµÉóÉñÉçÉÀÈüÈûÈìÈßÈÝÈ¹ÇîÇÞÁÈÁ±ÀñÀÎ¿ú¿í¿ã¿ß¿Ü¿Õ¿Í¾ü¾¿¾½½ó½Ñ¼Ò¼Å¼Ä»ö»íÇÔÇÏÆîÆíÅÛÅ©ÄþÄ¯ÃÝÃÜÃÛÃÂÂãÂ»Áþ»Âºêº×ºÖº±º®º¦¹Ú¹Ù¹Ó¹Ñ¹¬¸î¸»¸¤¸£¶î¶¨µ»´Ü´°´©³õ³è³Ä²ì²¹±ö±»±¦°À°¸°²",
            "Q÷¯÷®÷­÷¬÷«÷©÷¨÷§÷¦÷¥÷¤÷£÷¢öþöýöüöûöúöùöøö÷öööõöôöóöòöñöðöïöîöíöìöëöêöéöçöæöåöäöãöâöáöàöÞöÝöÜöÛöÚöÙöØöÖöÕöÔöÓöÒöÑöÐöÏöÎö£ö¢ö¡õýõüõûõúôÁð×ð·ð¶ïñïðïïïîïíïìïëïêïéïèïçïæïåïäïãïâïáïàïßïÞïÝïÜïÛïÚïÙïØï×ïÖïÕïÔïÓïÒïÑïÐïÏïÎïÍïÌïËïÊïÉïÈïÇïÆïÅïÄïÃïÂïÁïÀï¿ï¾ï½ï¼ï»ïºï¹ï¸ï·ï¶ïµï´ï³ï²ï±ï°ï¯ï®ï­ï¬ï«ïªï©ï¨ï§ï¦ï¥ï¤ï£ï¢ï¡îþîýîüîûîúîùîøî÷îöîõîôîóîòîñîðîïîîîíîìîëîêîéîèîçîæîåîäîãîâîáîàîßîÞîÝîÜîÛîÚîÙîØî×îÖîÕîÔîÓîÒîÑîÐîÏîÎîÍîÌîËîÊîÉîÈîÇîÆîÅîÄîÂíóì¤ëÈéÍèîèÉçôåâåÞåÑåÇâÎâÍâÌâËâÊâÉâÈâÇâÆâÅâÄâÃâÂâÁâÀâ¿â¾â½â¼â»â¹â¸â¶âµâ´â³â²â±â°â¯â®â­â¬â«âªâ©â¨â§â¦â¥â¤â£â¢â¡áþáýáüáûáúáùáøá÷áöáõáôáóáòáñáðáïáîáíáìáëß±ÛËÛ¾Û¼Û»Û­Û¨Û§Û¡ÚùÙìÙëÙêÙéÙèØØØ×ØµØ³Ø¢×ê×Þ×¶ÖýÖíÖåÖÓÖËÕùÕøÕòÕëÕàÕ²Õ¡ÔÈÔ¿Ô¹Ô³Ô§ÓüÓãÓÌÓËÓ­Ó¡ÒûÒøÒÝÒ¿Ñ®ÐâÐÙÐ×ÐÉÐ¿Ð·ÏúÏóÏâÏÚÏÊÏÇÏÁÏ³Ï¦Ï£ÎýÎðÎÚÎÙÎ£ÍâÍÒÍÃÍ­ÌúÌàÌ¡ËøËÇÊÏÊÎÊ¨É×É·É²É±É«ÈúÈñÈÄÈ»ÇäÁÍÁ´Á­ÀôÀðÀêÀØÀÖÀÇÀ¡¿ñ¾û¾ä¾â¾Ñ¾Ä¾Ã¾µ¾¨½õ½ð½â½È½Ç½Æ½Â½¤¼ü¼Ø¼±¼¢»èÇÕÇÂÇ·Ç¯Ç®Ç¦Ç¥ÆÌÅÙÅ¥ÄüÄøÄ÷ÄñÄÙÄÆÃûÃúÃãÃâÃÍÃÌÃ¾Ã³Ã®Ã­ÃªÃ¨ÂøÂàÂÑÂÁÂ³ÁôÁóÁåÁÔ»«ºüºöºïºÝ¹ø¹ê¹ä¹Ý¹»¹·¹´¹³¸õ¸ä¸Ö¸Æ¸º·õ·æ·¹·¸·°¶ü¶û¶ù¶ö¶à¶Û¶Í¶Æ¶À¶µ¶§¶¤µöµéµÒµº´í´Ò´¥³û³ú³®²þ²ù²ö²Â²¬²§±·±µ±«±¥°ü°÷",
            "R÷Î÷Í÷Ì÷Ë÷É÷Èõ½ôêóÁó¾òØñýðÇðºð¬ð«ð©ð¨ð§ë¸ëµë´ë³ë²ë±ë°ë¯ë®ë­ë¡êþêÞåØåËß­ß¬ß«ßªß©ß¨ß§ß¦ß¥ß¤ß£ß¢ß¡ÞýÞüÞûÞúÞùÞøÞ÷ÞöÞõÞóÞòÞñÞðÞïÞîÞíÞìÞëÞêÞéÞèÞçÞæÞåÞäÞãÞâÞáÞàÞßÞÞÞÝÞÜÞÛÞÚÞÙÞØÞ×ÞÖÞÕÞÔÞÓÞÒÞÑÞÐÛ¯Û¥Ø´×á×¾×½×²×«×§×¦×¥ÖôÖìÖÊÖÆÖÀÖ¿Ö¸Ö´ÕüÕõÕñÕÝÕÜÕÛÕÒÕÐÕªÔúÔñÔíÔÜÔÀÔ®ÓµÒóÒÖÒ´Ò¡ÑûÑõÑïÑÚÑºÐÀÐ¶Ð¯Ð®ÏÆÎèÎæÎÕÎÎÍîÍìÍÚÍØÍÐÍÏÍÆÍ¶Í±Í¦ÌôÌáÌÍÌÂÌ½Ì¯Ì§Ì¢ËùËðËÓËÑËºË©Ë¤ÊãÊÚÊÖÊÆÊÅÊÄÊÃÊ°Ê§ÉãÉÓÉÃÉ¨É¦ÈöÈàÈÓÈÈÈÅÈÁÈ±ÈªÇñÇðÇèÇâÇÜÁÌÁÃÀÞÀÌÀ¿À¹À­À©À¨À¦¿ý¿æ¿Û¿Ù¿Ø¿½¿¹¿¸¿´¿«¾ò¾ñ¾ð¾ï¾è¾Ý¾Ü¾Ð¾¾½ü½ï½Ý½Ó½Ò½Á¼ñ¼ð¼¼¼·»Ó»ÊÇËÇÀÇ¤ÆþÆøÆËÆÈÆÇÆ¹Æ´Æ²ÅûÅúÅõÅêÅ×ÅÒÅÅÅÄÅÀÅ²Å¤Å£Å¡ÄóÄíÄìÄêÄéÄâÄÓÄÊÄ´Ä¨ÃþÃòÃèÂÕÂÓÂÈÂ°Â§Â£Áà»»»¤ºóº´º³º¤¹í¹Þ¹Õ¹Ò¹Ï¹°¸é¸ã¸Þ¸×¸§·ú·÷·ö·Õ·µ·´¶ó¶Þ¶Ý¶Ü¶¶µüµôµæµàµÖµÄµ·µ²µªµ§µ£´ò´î´ì´ë´ê´é´Ý´·´§´¤³é³â³Ö³Å³¸³·³¶³­²ô²ó²ð²ë²å²Ù²Á²¶²¯²«²¦²¥±ø±÷±ì±°±¨±§°è°ç°â°á°Ý°Ú°×°Ñ°Î°Ç°Æ°¿°´°±°¨°¤",
            "Sõ¸õ·õ¶õµõ´õ³õ²õ±õ°õ¯õ®õ­õ¬õ«õªõ©õ¨õ§õ¦õ¥õ¤õ£õ¢õ¡ôþôýôüôûôúñûðªí®éßéÝéÜéÛéÚéÙéØé×éÖéÕéÔéÓéÐéÏéÎéÌéËéÊéÉéÈéÇéÆéÅéÄéÂéÁéÀé¿é¾é½é¼é»éºé¹é¸é·é¶éµé´é³é²é±é¯é®é­é¬é«éªé©é¨é§é¦é¥é¤é£é¢é¡èþèüèûèúèùèøè÷èöèõèôèóèòèñèíèìèëèêèéèèèçèæèåèäèãèâèáèàèßèÞèÝèÜèÛèÚèÙèØè×èÖèÕèÔèÓèÒèÑèÐèÏèÎèÍèÌèËèÊèÈèÇèÆèÅèÄèÃèÂèÁèÀè¿è¾è½è¼è»Û²Øâ×õ×í×Ø×Ã×µ×®ÖùÖêÖ²Ö¦ÕíÕçÕÈÕÁÕ»Õ¥Õ¤ÔýÔÍÓÜÓÏÓ£ÒÎÒ¬ÒªÑùÑîÐïÐàÐÓÐÑÐµÐ¨Ð£ÏðÏëÏàÏ­Î÷ÎöÎàÎ¦Í÷ÍÖÍ°ÍªÍ©ÌÝÌÒÌ´ÌªËóËáËÚËÖËÉË¨Ê÷ÊöÊõÊáÊàÊÁÉÒÉ¼É­È¶È©È¨ÀõÀîÀãÀâÀÒÀÆÀ¸À·¿ò¿á¿Ý¿É¿Ã¿Â¿¬½û½Ü½Û½Í½·¼÷¼ì¼Ö¼Ï¼«»úÇÅÇÁÇ¹ÆåÆÜÆÓÆ±Æ°Æ®ÅïÅäÅÊÄûÄðÄ¾Ä£ÃÞÃÑÃÎÃÊÃ¸Ã·Ã¶Â´Â¥ÁøÁñÁÖ»¸»±ºáºËº¼º¨¹÷¹ñ¹ð¹×¹¹¹£¸ù¸ñ¸è¸ç¸Ü¸Ì¸Ë¸Å¸²·ã·Ù·Ó·®¶Å¶°¶¥¶¡µµ´å´×´¼´»´ª³þ³÷³ê³È³»²é²Û²Ä±ú±ò±ê±¾±­°ô°ñ°ð°å°Ø",
            "T÷þ÷ý÷ü÷ó÷ªöÃô¿ô¾ô½ô¼ô»ôºô¹ô¸ô·ô¶ôµô´ô³ô²ô±ô°ô¯ô®ô­ô¬ô«ô¦ô¥ô¤ô£ô¢ô¡óþóýóüóûóúóùóøó÷óöóõóôóóóòóñóðóïóîóíóìóëóêóéóèóçóæóåóãóâóáóàóßóÞóÝóÜóÛóÚóÙóØó×óÖóÕóÔóÓóÒóÑóÐóÏóÎóÍóÌóËóÊóÉóÈóÇóÆóÅóÄóÃóÂó®ðÀð»ð¦ð¥ð¤ð£ð¢ð¡ïþïýïüïûïúïùïøï÷ïöïõïôïóïòíòí¬í©ì¦ë»ëºë¹ë¶ë«ëªë¦ë¥ë¤êûêúêùêøê÷êöêõêôêóêòêÃé°åÔåÌåÆåÅåÃâºáéáèáçáæáåáäáãáâáááàáßáÞáÝáÜá®Û¶Û¬ÙáÙàØæØºØ¹Ø·Ø¶Ø²Ø±Ø¯×ë×â×Ô×­ÖþÖñÖÛÖØÖÖÖÉÖÈÖÇÖªÕ÷Õ±Õ§ÔõÔìÔÞÔÁÓùÓíÓÔÒÛÒÆÑíÑÜÑÓÑÃÑ­Ñ¬ÑªÑ¡ÐìÐãÐÐÐÆÐ¦ÏòÏäÏãÏÏÏÎÏÈÏµÏ¤Ï¢Ï¡ÎþÎñÎïÎçÎÒÎÈÎºÎ¯Î¢ÍùÍÇÍ½ÍºÍ¸Í²Í§Í¢ÌõÌòÌðÌØÌÉÌºËñËëËãËÒË½Ë°ÊòÊÍÊÊÊ¸Ê£ÉýÉüÉûÉúÉíÉäÉàÉÔÉÈÉ¸ÈëÈÉÇûÇïÀûÀéÀèÀçÀæÀº¿ð¿ê¿Æ¿¿¾Ø¾Ì¾¶½î½Ö½Õ½Ã½¢¼ý¼ò¼ã¼Ú¼¾¼®»þ»ý»ü»à»Õ»ÉÇÇÇ©Ç¨Ç§ÆùÆòÆ¬ÆªÅñÅÍÅÌÅÇÅÆÄÂÄÁÄµÃôÃëÃØÃ¿Ã´Ã«ÂáÂÒÂÉÂ¨Áý»²ºõºâºÜºÍºÌº½¹Ü¹Ô¹Î¹¿¹ª¸÷¸æ¸å¸Ý¸Ñ¸Í¸´·û·ê·±·­·¬·¦·¤¶ì¶æ¶ã¶Ì¶¿¶¬¶ªµÚµÑµÐµÈµÃµÂµ¾´ý´ð´Û´Ø´Ñ´Ç´¹´¬´¦³ô³ï³î³í³Ó³Í³Ì³Ë³Æ³¹³¤²ß²Õ²¾²°²­±ü±Ò±Ë±Ê±Ç±¿±¹±¸°æ°ã°Þ°Ê°Â°«",
            "Uößõ¿ôåôÒôËôÊôÉôÈôÇôÆñµñ´ñ³ñ²ñ±ñ°ñ¯ñ®ñ­ñ¬ñ«ñªñ©ñ¨ñ§ñ¦ñ¥ñ¤ñ£ñ¢ñ¡ðþðýðüðûðúðùðøð÷ðöðõðôðóðòðñðððïðîðíðìðëðêðéðèðçðæðåðäðãðâðáðàðßðÞðÝðÜðÛðÚðÏðËðÃðÂîÃí°í§í¦ìªì§ê¸êµê³éàéÃèðæÜæªåÙãÜãÛãÚãÙãØã×ãÖãÕãÔãÓãÒãÑãÐãÏãÎãÍãÌãËãÊãÉãÈãÇãÆãÅà´Û·ÛµÚýÚ¡ÙþÙýÙüÙûÙòÙðÙçÙæÙå×ñ×ð×Ü×Ë×Ê×É×È×Å×¼×´×³×±×°ÖÌÖ£Ö¢ÕîÕÎÕÃÕÂÕ¾Õ¢ÔøÔÏÔÄÓ¸ÒôÒæÒãÒâÒßÒ±ÑøÑ÷ÑòÑñÑåÑØÑÖÑÕÑËÑ¾Ñ¢ÐßÐÂÐÁÐ§ÏèÏÛÏÐÎÊÎÅÎÁÍ·Í´Í¯ÌêÌÜÌÛÌµÌ±ËìËÜËÍË·ÊÞÊÝÊ×ÉØÉÌÉÆÉÁÈòÈ³È¯È­È¬ÇõÇ×ÁÆÁ¹Á¢Á¡ÀäÀ¼À»À±À«¿ö¿¢¾ö¾í¾ì¾Ò¾Î¾»¾º¾¹¾¸¾·½ê½ß½¼½»½´½±½°½¬½«½«½ª¼õ¼ô¼æ¼å¼ä¼½¼²Ç¼Ç¸Ç°ÆàÆÕÆÊÆ¿Æ³Æ¦Æ£ÅÔÅÑÅÐÅ±ÄýÄæÄÖÃöÃÆÃÅÃÀÁùÁöÁèÁçÁÝ»¿»¾ºÛºÒ¹ë¹Ø¸þ¸ó¸í¸á¸Ó¸Ç·ë·è·§¶Ò¶Ë¶»¶·¶³µòµìµÝµÜµÛµÁµÀµ¦µ¥´ñ´á´Õ´Î´É´È´Ã´³´¯³å³Õ²û²ú²î²¿²¢²¡±ù±ñ±ï±î±è±ç±æ±×±Ö±Õ±Ô±Å±³±±°ë°ê°Ì°©",
            "V÷û÷ú÷ù÷ø÷÷ôßôÞôªô¨ô§ð¯í²í±çßçÞçÝæåæÛæ×æÖæÓæÒæÑæÐæÏæÎæÍæÌæÊæÉæÈæÇæÆæÅæÃæÂæÁæÀæ¿æ¾æ½æ¼æ»æºæ¹æ¸æ·æµæ´æ³æ²æ±æ°æ¯æ­æ¬æ«æ©æ¨æ§æ¦æ¥æ¤æ£æ¢æ¡åþåýåüåûåúåùåóåæåååÖàûÛÅÛ¿Ø¸ÖãÕÙÔÓÓéÒüÒöÒÌÒ¦ÑýÑ²Ñ°ÐöÐõÐñÐÕÏÓÏ±ÍñÍèÍÞÍËÌöËýËàË¡ÊóÊ¼ÉôÉïÉÛÉ©ÈçÈÑÈÐÈÌÈºÁ¥ÀÑ¿Ò¿Ñ¿¤¾ý¾ê¾Ë¾Ê¾Å½ã½Ë½¿½¨¼é¼Þ¼Ë¼È¼µ¼´¼§»é»ÙÅ®Å­Å¬Å«ÄïÄÝÄÛÄÌÄÈÄÇÄ·ÃîÃäÃÄÃÃÃ½ÂèÂ¼ÁéºÃ¹Ã¸¾·Á¶ð¶ÊµÕµ¶´þ³²°þ",
            "W÷ìöÅöÁöÀö¿ôâôáôÀò¥ò¢ðÔðÁð¼î´íèí¥ìàìÒì¨ë¨êðêìê²ê¨è·á·ÛÐÛ¦ÙâÙßÙÞÙÝÙÜÙÛÙÚÙÙÙØÙ×ÙÖÙÕÙÔÙÓÙÒÙÑÙÐÙÏÙÎÙÍÙÌÙËÙÊÙÉÙÈÙÇÙÆÙÅÙÄÙÃÙÂÙÁÙÀÙ¿Ù¾Ù½Ù¼Ù»ÙºÙ¹Ù¸Ù·Ù¶ÙµÙ´Ù³Ù²Ù±Ù°Ù¯Ù®Ù­Ù¬Ù«ÙªÙ©Ù¨Ù§Ù¦Ù¥Ù¤Ù£Ù¢Ù¡ØþØýØüØûØúØùØøØ÷ØöØõØôØóØòØñØðØïØîØíØìØëØêØéØä×ø×÷×ö×ô×Ð×·×¡ÖÚÖÙÖ¶ÖµÕìÕÌÕ®ÓûÓúÓâÓáÓàÓßÓÓÓÆÓÅÓ¶ÒÚÒÐÒÇÒÁÒÀÒ¯ÑöÑðÐðÐÞÐÝÐÅÐ±ÏñÏÉÏÀÎêÎéÎÍÎÌÎ»Î±Î°Í¾ÍµÍ£ÌåÌÈÌ°ËûËÛË×ËÌËËËÊËÆËÅÊæÊÛÊÌÊËÊ¹Ê³Ê²ÉìÉáÉËÉµÉ®É¡ÈåÈÔÈÎÈËÈÊÈ«ÇãÇÝÇÖÁÅÁ²Á©ÀþÀýÀüÀÜÀÐ¿þ¿ë¿¡¾ó¾ë¾ã½ö½ñ½é½è½Ä½¹½©½£½¡¼þ¼ó¼Û¼Ù¼Ñ¼À¼¿¼¯»õ»ï»áÇÎÇÈÇªÆóÆÍÆ¾Æ¶Æ«Æ§ÅèÅåÅ¼ÄúÄîÄãÄßÄÃÃüÃÇÂØÂ×ÂÂÁîÁìÁæÁÞÁÛÁÚ»¯»ªºòºîºÐºÏºÎº¬¹ô¹ï¹È¹À¹«¹©¸ö¸ë¸¸¸·¸¶¸µ¸«¸ª¸©·ý·ü·ð·Þ·Ý·Ö·Â·¥¶í¶Î¶±µùµèµÍµÊµÇµ¹µ«´ü´û´ú´ö´ß´Ù´Ô´Ó´´´«´¢³ð³Þ³«³¥²ò²í²à²Ö²®±ý±ã±¶±¤±£°ø°é°ä°Û°Ö°Ë°Á°³",
            "X÷Ïôéó»ò£ð¸ð±êñçÜçÛçÚçÙçØç×çÖçÕçÔçÓçÒçÑçÐçÏçÎçÍçÌçËçÊçÉçÈçÇçÆçÅçÄçÃçÂçÁçÀç¿ç¾ç½ç¼ç»çºç¹ç¸ç·ç¶çµç´ç³ç²ç±ç°ç¯ç®ç­ç¬ç«çªç©ç¨ç§ç¦ç¥ç¤ç£ç¢ç¡æþæýæüæûæúæùå÷åöåõåôåòåéåèàÎØ°×é×Ý×Û×ºÖàÖÕÖ½Ö¼Ö¯ÕÅÕÀÔ¼ÔµÓ×ÓÄÓ±Ó§ÒýÒïÒÞÒÍÒÉÑ¤ÐøÐ÷ÐåÏçÏßÏÒÏËÏ¸ÎãÎÆÎ³Î¬Í³ÌÐËõËçË¿ÉþÉðÉÜÉÉÉ´ÈõÈÞÈÒÈÆÇêÁ·ÀÂ¾ø¾î¾À¾­½á½Ô½Ê½É½®¼ê¼Í¼Ì¼¶¼©¼¨»æÇ¿Å¦ÄÉÄ¸ÃåÃàÃÖÂçÂÚÂÌÂÆ»Ã»º»¡ºìºë¹á¹­¸ø¸Ù¸¿¸¥·ì·×·Ñ·Ä¶ÐµÞµ¯´Â´¿³ñ³Ú²ø±à±Ñ±Ð±Ï±È±Á°ó°í",
            "Y÷ë÷ê÷é÷è÷ç÷æ÷å÷ä÷ã÷â÷á÷ÓöÇôìôæò¤ñþðÕðÑðÈð½í­ìéìèìçìæìåì½ì¼ì»ìºì¹ì¸ì·ì¶ìµì´ëöëÁêÆèïæ®âßâÞâÝâÜâÛâÚâÙâØâ×âÖâÕâÔâÓâÒâÑâÐâÏÞÈÞÄÛÕÛÓÛÀÚúÚøÚ÷ÚßÚÞÚÝÚÜÚÛÚÚÚÙÚØÚ×ÚÖÚÕÚÔÚÓÚÒÚÑÚÐÚÏÚÎÚÍÚÌÚËÚÊÚÉÚÈÚÇÚÆÚÅÚÄÚÃÚÂÚÁÚÀÚ¿Ú¾Ú½Ú¼Ú»ÚºÚ¹Ú¸Ú·Ú¶ÚµÚ´Ú³Ú²Ú±Ú°Ú¯Ú®Ú­Ú¬Ú«ÚªÚ©Ú¨Ú§Ú¦Ú¥ÙúÙùÙøÙ÷ÙöÙõÙôÙóÙñÙïØ¼×ù×ç×å×ä×»×¯Ö÷ÖïÖîÖßÖÝÖÔÖ¤ÕïÕâÕÚÕØÕ«Õ©ÓýÓïÓÕÓÀÓºÓ¹Ó®Ó¦Ó¥ÒëÒêÒéÒèÒåÒáÒàÒÂÒ¹Ò¥ÑèÑÔÑÈÑ¶ÑµÑ¯ÐþÐýÐóÐòÐíÐ»Ð³ÏíÏêÏåÏ¯ÎóÎÜÎÉÎÄÎ½ÎªÍýÍüÍûÍöÍäÍ¥Í¤ÌÖÌÆÌ¸Ì·ËßËÐËÏËµË­Ë¥ÊüÊìÊëÊÔÊÐÊ¶Ê«Ê©ÉèÈÏÈÃÈ¿ÇìÇëÁÎÁÂÁÁÁ¼ÁµÁ®ÀëÀÊÀÉÀÈÀ¾Àª¿â¿Î¿Ì¿º¿µ¾÷¾Í¾©½÷½ë½²¼ç¼Ç¼Æ¼Á¼¥¼£»å»ä»ÑÇÃÇ´Ç«ÆýÆúÆôÆìÆëÆ×ÆÀÅëÅÓÅµÄ¶Ä±Ä§Ä¦Ä¥ÃýÃíÃÕÃÓÃÒÃ¥Ã¤Ã¡ÂùÂéÂÛÂÏÂÎÂÍÂÊÂÃÂ¹Â®ÁõÁß»°»§ºàºÁºÀº¥¹ü¹ù¹î¹ã¹Í¸ý¸à¸ß¸Ã¸¼¸¯¸®·í·Ï·Ì·Å·Ã·¿·½¶ï¶Ø¶È¶Á¶©µýµ÷µêµ×µ®´Ê´²³ä³Ï²ü²÷²ï±ó±é±å±ä±â±Ó°ý°ù°§",
            "£¡@#%¡­&*£¨£©¡ª£»£º¡±¡¯£¿¡¶¡·£¬¡£¡¢~={}','|"};

        /// <summary>
        /// ÖÐÎÄ×ªÎå±ÊÊ××ÖÄ¸
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetChineseWB(string strText)
        {
            StringBuilder result = new StringBuilder();
            int iAscii = 0;
            for (int i = 0; i < strText.Length; i++)
            {
                char charTemp = strText[i];
                iAscii = Convert.ToInt32(charTemp);
                if (iAscii > 1 && iAscii < 254)
                {
                    //ÊÇÓ¢ÎÄ
                    result.Append(charTemp);
                }
                else
                {
                    for (int j = 0; j < wbLib.Length; j++)
                    {
                        if (wbLib[j].Contains(charTemp.ToString()))
                        {
                            result.Append(j == wbLib.Length - 1 ? charTemp : wbLib[j][0]);
                            break;
                        }
                    }
                }
            }
            return result.ToString().ToLower();
        }


        #endregion
    }
}
