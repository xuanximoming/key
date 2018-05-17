using System;

namespace DrectSoft.DrawDriver
{
	public class DrawInfoString
	{
		private string _Left;

		private string _Top;

		private string _Width;

		private string _Height;

		private string _Align;

		private string _Valign;

		private string _FontColor;

		private string _BgColor;

		private string _Border;

		public string Left
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

		public string Top
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

		public string Width
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

		public string Height
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

		public string Align
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

		public string Valign
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

		public string FontColor
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

		public string BgColor
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

		public string Border
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
