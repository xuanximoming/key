using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace YidanSoft.Core.Symbol
{
	/// <summary>
	/// string转图像的参数包裹
	/// </summary>
	public struct StrImgConvertWrap
	{
		public string str;
		public Size size;

		/// <summary>
		/// 结构构造器
		/// </summary>
		/// <param name="str"></param>
		/// <param name="size"></param>
		public StrImgConvertWrap(string str,Size size)
		{
			this.str = str;
			this.size = size;
		}
	}
}
