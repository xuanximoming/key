using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Library.EmrEditor.Src.Gui;
using System.Collections.Generic;
using System.Drawing;

namespace DrectSoft.Library.EmrEditor.Src.Common
{
    public class FontSizeItem
    {
        string fontSizeName = "宋体";
        float fontSize = 12.0f;

        public string FontSizeName
        {
            get { return fontSizeName; }
            set { fontSizeName = value; }
        }

        public float FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        public FontSizeItem(string fontsizename, float fontsize)
        {
            this.FontSizeName = fontsizename;
            this.FontSize = fontsize;
        }
    }
    public class FontCommon
    {
        //字号的名字
        static public string[] allFontSizeName = { "初号", "小初", "一号", "小一", "二号", "小二", "三号", "小三", "四号", "小四", "五号", "小五", "六号", "小六", "七号", "八号", "8", "9", "10", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72", };

        //字号的磅值
        static public float[] allFontSize = { 42, 36, 26, 24, 22, 18, 16, 15, 14, 12, 10.5F, 9, 7.5F, 6.5F, 5.5F, 5, 8, 9, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

        static List<FontSizeItem> fontSizeList = new List<FontSizeItem>();
        static public List<FontSizeItem> FontSizeList
        {
            get
            {
                if (fontSizeList.Count == 0)
                {
                    for (int i = 0; i < allFontSizeName.Length; i++)
                    {
                        FontSizeItem item = new FontSizeItem(allFontSizeName[i], allFontSize[i]);
                        fontSizeList.Add(item);
                    }
                }
                return fontSizeList;
            }
        }


        static List<string> fontList = new List<string>();
        static public List<string> FontList
        {
            get
            {
                if (fontList.Count == 0)
                {
                    for (int i = FontFamily.Families.Length - 1; i > 0; i--)
                    {
                        FontFamily ff = FontFamily.Families[i];
                        if (ff.IsStyleAvailable(FontStyle.Bold) && ff.IsStyleAvailable(FontStyle.Italic) && ff.IsStyleAvailable(FontStyle.Regular) && ff.IsStyleAvailable(FontStyle.Strikeout) && ff.IsStyleAvailable(FontStyle.Underline))
                        {
                            fontList.Add(FontFamily.Families[i].Name);
                        }
                    }

                }
                return fontList;
            }
        }


        public static float GetFontSizeByName(string fontSizeName)
        {
            for (int i = 0; i < allFontSizeName.Length; i++)
            {
                if (allFontSizeName[i] == fontSizeName)
                {
                    return allFontSize[i];
                }
            }
            return GetFontSizeByName(ZYEditorControl.GetDefaultSettings(ZYTextConst.c_FontSize));
        }

        public static string GetFontSizeNameBySize(float fontSize)
        {
            for (int i = 0; i < allFontSize.Length; i++)
            {
                if (allFontSize[i] == fontSize)
                {
                    return allFontSizeName[i];
                }
            }
            return ZYEditorControl.GetDefaultSettings(ZYTextConst.c_FontSize);
        }
    }
}
