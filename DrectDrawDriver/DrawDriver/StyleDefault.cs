using System;
using System.Drawing;

namespace DrectSoft.DrawDriver
{
	public class StyleDefault
	{
		private const string _Left = "0";

		private const string _Top = "0";

		private const string _Width = "600";

		private const string _Height = "32";

		private const string _Align = "center";

		private const string _Valign = "middle";

		private const string _FontColor = "宋体 11px #000000";

		private const string _BgColor = "#FFFFFF";

		private const string _Border = "Solid 1px #000000;Solid 1px #000000;Solid 1px #000000;Solid 1px #000000";

		public static string StringLeft
		{
			get
			{
				return "0";
			}
		}

		public static string StringTop
		{
			get
			{
				return "0";
			}
		}

		public static string StringWidth
		{
			get
			{
				return "600";
			}
		}

		public static string StringHeight
		{
			get
			{
				return "32";
			}
		}

		public static string StringAlign
		{
			get
			{
				return "center";
			}
		}

		public static string StringValign
		{
			get
			{
				return "middle";
			}
		}

		public static string StringFontColor
		{
			get
			{
				return "宋体 11px #000000";
			}
		}

		public static string StringBgColor
		{
			get
			{
				return "#FFFFFF";
			}
		}

		public static string StringBorder
		{
			get
			{
				return "Solid 1px #000000;Solid 1px #000000;Solid 1px #000000;Solid 1px #000000";
			}
		}

		public static int Left
		{
			get
			{
				return CommonMethods.StringToInt32("0");
			}
		}

		public static int Top
		{
			get
			{
				return CommonMethods.StringToInt32("0");
			}
		}

		public static int Width
		{
			get
			{
				return CommonMethods.StringToInt32("600");
			}
		}

		public static int Height
		{
			get
			{
				return CommonMethods.StringToInt32("32");
			}
		}

		public static AlignType Align
		{
			get
			{
				return CommonMethods.StringToAlignType("center", AlignType.Center);
			}
		}

		public static ValignType Valign
		{
			get
			{
				return CommonMethods.StringToValignType("middle", ValignType.Middle);
			}
		}

		public static FontColor FontColor
		{
			get
			{
				return CommonMethods.StringToFontColor("宋体 11px #000000", null);
			}
		}

		public static Color BgColor
		{
			get
			{
				return CommonMethods.StringToColor("#FFFFFF");
			}
		}

		public static Border Border
		{
			get
			{
				return CommonMethods.StringToBorder("Solid 1px #000000;Solid 1px #000000;Solid 1px #000000;Solid 1px #000000", null);
			}
		}
	}
}
