using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections;
using YidanSoft.Common.Object2Editor.Encoding;

namespace YidanSoft.Core.Symbol
{
	/// <summary>
	/// Rtf2Image转换核心-手动计算
	/// </summary>
	public class Rtf2ImgStrategyM :StrImgCStrategy
	{
		#region const and fields
		private const int DrawPlus = 2;
		private ArrayList m_rtfFormatList=new ArrayList();
		private Font m_font;
		private Font m_superSubFont;
		#endregion

		#region override
		protected override Bitmap DoStrImgConvert(StrImgConvertWrap wrap)
		{
			//新建绘画图像
			Bitmap map = new Bitmap(wrap.size.Width, wrap.size.Height);
			//分析Rtf串获得
			ParseRtfString(wrap.str);
			//计算绘画信息
			CalcRtfDrawingInfo(m_rtfFormatList, wrap.size);
			//执行绘画操作
			using (Graphics grap = Graphics.FromImage(map)) 
			{
				//清屏，以白色为底
				grap.Clear(Color.White);
				//绘制字符
				foreach (RtfFormat format in m_rtfFormatList) 
				{
					grap.DrawString(format.content, format.font, Brushes.Black, format.px, format.py);
				}
			}
			//释放字体信息
			m_superSubFont.Dispose();
			m_font.Dispose();
			//返回图像
			return map;
		}
		#endregion

		#region private method

		private void ParseRtfString(string rtf)
		{
			Regex regex;
			if (rtf.StartsWith(@"{\rtf1"))
				regex = new Regex(@"\\[a-z]+\s.");
			else
				regex = new Regex(@".");
			MatchCollection matchs = regex.Matches(rtf);
			m_rtfFormatList.Clear();
			RtfFormat.count = 0;
			RtfFormat.regularcount = 0;
			foreach (Match match in matchs)
			{
				RtfFormat format = new RtfFormat();
				string str = match.ToString();
				int split = str.IndexOf(" ");
				if (split == -1)
					format.prefix = str.Substring(0, split + 1);
				else
					format.prefix = str.Substring(0, split);
				format.content = str.Substring(split + 1, str.Length - split - 1);
				if (format.prefix == RtfEncoding.RtfNoSuperSub || format.prefix==string.Empty)
					RtfFormat.regularcount++;
				RtfFormat.count++;
				m_rtfFormatList.Add(format);
			}
		}

		private void CalcRtfDrawingInfo(ArrayList rtfformats,Size size)
		{
			//适应性调整绘画字体,以使图像缩放至预订大小。
			AdjustDrawEnv(size);
			//计算首字符绘画位置
			float startx = (size.Width - RtfFormat.CalcRtfDrawingLen(m_font,m_superSubFont)) / 2;
			float starty = (size.Height - m_font.Size) / 2;
			//计算各字符的绘画信息 
			for (int index = 0; index < m_rtfFormatList.Count; index++)
			{
				RtfFormat format = (RtfFormat)m_rtfFormatList[index];
				switch (format.prefix)
				{
					case RtfEncoding.RtfSuper:
						format.font = m_superSubFont;
						format.px = startx;
						format.py = starty;
						break;
					case RtfEncoding.RtfSub:
						format.font = m_superSubFont;
						format.px = startx;
						format.py = starty + (m_font.Size - m_superSubFont.Size)+DrawPlus;
						break;
					default:
						format.font = m_font;
						format.px = startx;
						format.py = starty;
						break;
				}
				m_rtfFormatList[index] = format;
				//计算下一符号位置
				startx += format.font.Size-DrawPlus;
			}
		}

		private void AdjustDrawEnv(Size size) 
		{
			float fontsize;
			if (m_rtfFormatList.Count <= 3)
				fontsize = size.Width / 2;
			else
				fontsize = size.Width / (
					(RtfFormat.count - RtfFormat.regularcount) / 2 + RtfFormat.regularcount);
			do
			{
				m_font = new Font("宋体",fontsize,GraphicsUnit.Pixel);
				if (m_font.Height <= size.Height)
					break;
				if (fontsize - 0.5F < 12F)
					break;
				fontsize -= 0.5F;
				m_font.Dispose();
			} while (true);

			m_superSubFont = new Font(m_font.FontFamily, m_font.Size / 2);
		}

		#endregion
	}

	#region assist struct
	public struct RtfFormat 
	{
		public static int regularcount;
		public static int count;

		public static float CalcRtfDrawingLen(Font regular,Font supersub) 
		{
			return regular.Size * regularcount + supersub.Size * (count-regularcount);
		}

		public string prefix;
		public string content;
		public float px, py;
		public Font font;
	}
	#endregion
}
