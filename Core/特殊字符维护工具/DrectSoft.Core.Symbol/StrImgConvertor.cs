using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace YidanSoft.Core.Symbol
{
	/// <summary>
	/// StrImgConvertor,字符串转图像控制器
	/// </summary>
	public  class StrImgConvertor :ConvertController<StrImgConvertWrap,Image>
	{
		#region fields

		private StrImgCStrategy m_convertStrategy;

		#endregion

		#region ctor
		/// <summary>
		/// ImageConvertor构造器
		/// </summary>
		/// <param name="convertCore"></param>
		public StrImgConvertor(StrImgCStrategy cStrategy) 
		{
			this.m_convertStrategy = cStrategy;
		}
		#endregion

		#region overrid method
		/// <summary>
		/// 实现ConvertController<S,T>中的DoSTConvert，负责根据预设的图像大小将字符串对象转换为图像
		/// </summary>
		/// <param name="origianl"></param>
		/// <returns></returns>
		protected override Image DoSTConvert(StrImgConvertWrap origianl)
		{
			if (m_convertStrategy != null)
				return m_convertStrategy.DoConvert(origianl);
			return null;
		}
		#endregion
	}
}