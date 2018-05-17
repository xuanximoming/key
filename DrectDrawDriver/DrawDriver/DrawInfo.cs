using System;
using System.Drawing;

namespace DrectSoft.DrawDriver
{
	public class DrawInfo
	{
		private int _Left;

		private int _Top;

		private int _Width;

		private int _Height;

		private AlignType _Align;

		private ValignType _Valign;

		private FontColor _FontColor;

		private Color _BgColor;

		private Border _Border;

		public int Left
		{
			get
			{
				return this._Left;
			}
			set
			{
				this._Left = value;
			}
		}

		public int Top
		{
			get
			{
				return this._Top;
			}
			set
			{
				this._Top = value;
			}
		}

		public int Width
		{
			get
			{
				return this._Width;
			}
			set
			{
				this._Width = value;
			}
		}

		public int Height
		{
			get
			{
				return this._Height;
			}
			set
			{
				this._Height = value;
			}
		}

		public AlignType Align
		{
			get
			{
				return this._Align;
			}
			set
			{
				this._Align = value;
			}
		}

		public ValignType Valign
		{
			get
			{
				return this._Valign;
			}
			set
			{
				this._Valign = value;
			}
		}

		public FontColor FontColor
		{
			get
			{
				return this._FontColor;
			}
			set
			{
				this._FontColor = value;
			}
		}

		public Color BgColor
		{
			get
			{
				return this._BgColor;
			}
			set
			{
				this._BgColor = value;
			}
		}

		public Border Border
		{
			get
			{
				return this._Border;
			}
			set
			{
				this._Border = value;
			}
		}
	}
}
