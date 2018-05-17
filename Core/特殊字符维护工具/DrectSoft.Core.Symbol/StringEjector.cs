using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using YidanSoft.Common.Object2Editor.Encoding;

namespace YidanSoft.Core.Symbol
{
    /// <summary>
    /// StringEjector，RTF字符串核型信息抽取控制器
    /// </summary>
    public class StringEjector : ConvertController<string, string>
    {
        #region override method
        /// <summary>
        /// 实现ConvertController<S,T>中的DoSTConvert，负责将RTF字符串中重要的信息抽取出
        /// </summary>
        /// <param name="origianl"></param>
        /// <returns></returns>
        protected override string DoSTConvert(string origianl)
        {
            //还原rtf串
            m_rTBRtfBox.Rtf = origianl;
            //抽取标志
            m_strBuilder.Remove(0, m_strBuilder.Length);
            //m_strBuilder.Append(@"{\rtf1 ");
            for (int i = 0; i < m_rTBRtfBox.TextLength; i++)
            {
                m_rTBRtfBox.Select(i, 1);
                string info = string.Format(@"{0} {1}",
                        FeachTextInfo(m_rTBRtfBox.SelectedRtf), m_rTBRtfBox.SelectedText);
                m_strBuilder.Append(info);
            }
            //m_strBuilder.Append(@"}");
            //返回
            return m_strBuilder.ToString();
        }
        #endregion

        #region  private  field and method
        /// <summary>
        /// 抽取文本信息
        /// </summary>
        /// <param name="rtf"></param>
        /// <returns></returns>
        private string FeachTextInfo(string rtf)
        {
            int formatEnd = rtf.LastIndexOf(RtfEncoding.RtfFontSize);
            if (rtf.IndexOf(RtfEncoding.RtfSuper) != -1)
                if (rtf.LastIndexOf(RtfEncoding.RtfSuper) <= formatEnd)
                    return RtfEncoding.RtfSuper;
            if (rtf.IndexOf(RtfEncoding.RtfSub) != -1)
                if (rtf.LastIndexOf(RtfEncoding.RtfSub) <= formatEnd)
                    return RtfEncoding.RtfSub;
            return RtfEncoding.RtfNoSuperSub;
        }
        private string FeachTextEncoding(string txt)
        {
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < txt.Length; index++)
                builder.Append(@"\u" + char.ConvertToUtf32(txt, index) + "?");
            return builder.ToString();
        }
        private RichTextBox m_rTBRtfBox = new RichTextBox();//Rtf原生串生成池
        private StringBuilder m_strBuilder = new StringBuilder();//RtfTF字符串核型信息池
        #endregion
    }
}
