using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace DrectSoft.Common.Eop
{
    public class EmrDefaultSetting
    {
        const string c_FontName = "fontname";
        const string c_FontSize = "fontsize";
        const string c_LineSpace = "linespace";
        const string c_EleStyle = "elestyle";
        const string c_EleColor = "elecolor";
        const string c_LineHeight = "lineheight";

        /// <summary>
        /// 字体 如：宋体等
        /// </summary>
        public string FontName
        {
            get
            {
                return m_FontName;
            }
            set
            {
                m_FontName = value;
            }
        }
        string m_FontName;

        /// <summary>
        /// 字号 如：9，小四，小五，一号等
        /// </summary>
        public string FontSize
        {
            get
            {
                return m_FontSize;
            }
            set
            {
                m_FontSize = value;
            }
        }
        string m_FontSize;

        /// <summary>
        /// 行距 如：最小，0.5倍，2倍等
        /// </summary>
        public string LineSpace
        {
            get
            {
                return m_LineSpace;
            }
            set
            {
                m_LineSpace = value;
            }
        }
        string m_LineSpace;

        /// <summary>
        /// 表现形式 如：背景色，下划线
        /// </summary>
        string m_EleStyle;
        public string EleStyle
        {
            get
            {
                return m_EleStyle;
            }
            set
            {
                m_EleStyle = value;
            }
        }

        /// <summary>
        /// 元素背景色
        /// </summary>
        Color m_EleColor;
        public Color EleColor
        {
            get
            {
                return m_EleColor;
            }
            set
            {
                m_EleColor = value;
            }
        }

        /// <summary>
        /// 行间距
        /// </summary>
        string m_LineHeight;
        public string LineHeight
        {
            get
            {
                return m_LineHeight;
            }
            set
            {
                m_LineHeight = value;
            }
        }

        public EmrDefaultSetting(string xml)
        {
            if (xml.Trim() != "")
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                m_FontName =  GetAttributeValue(doc, c_FontName);
                m_FontSize =  GetAttributeValue(doc, c_FontSize);
                m_LineSpace = GetAttributeValue(doc, c_LineSpace);
                m_EleStyle =  GetAttributeValue(doc, c_EleStyle);
                m_EleColor =  ColorTranslator.FromHtml(GetAttributeValue(doc, c_EleColor));
                //m_LineHeight = GetAttributeValue(doc, c_LineHeight);
            }
        }

        private string GetAttributeValue(XmlDocument doc, string nodeName)
        {
            return doc.GetElementsByTagName(nodeName).Item(0).InnerText;
        }
    }
}
