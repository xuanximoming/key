using System;

namespace DrectSoft.DrawDriver
{
	public class DrawAutoCalculationLocation
	{
		public DrawAutoCalculationLocation clChild;

		public string tag_path = "";

		private int intNumber = 0;

		public int left;

		public int top;

		public int body_width = 0;

		public int body_height = 0;

		public string str_body_height = "";

		public int TopAddDefault = 32;

		public DrawAutoCalculationLocation(int _left, int _top)
		{
			this.left = _left;
			this.top = _top;
		}

		public int GetNumber()
		{
			return this.intNumber;
		}

		public void Next()
		{
			this.intNumber++;
		}

		public int GetLeft()
		{
			return this.left;
		}

		public int GetTop()
		{
			return this.top;
		}

		public void Br()
		{
			this.Br(this.TopAddDefault);
		}

		public void Br(int TopAdd)
		{
			this.top += TopAdd;
		}
	}
}
