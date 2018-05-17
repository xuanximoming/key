using System;
using System.Drawing;

namespace DrectSoft.DrawDriver
{
	public class Border
	{
		private bool _IsDrawTop;

		private bool _IsDrawRight;

		private bool _IsDrawBottom;

		private bool _IsDrawLeft;

		private Color _TopColor;

		private LineType _TopStyle;

		private int _TopWidth;

		private Color _RightColor;

		private LineType _RightStyle;

		private int _RightWidth;

		private Color _BottomColor;

		private LineType _BottomStyle;

		private int _BottomWidth;

		private Color _LeftColor;

		private LineType _LeftStyle;

		private int _LeftWidth;

		public bool IsDrawTop
		{
			get
			{
				return this._IsDrawTop;
			}
			set
			{
				this._IsDrawTop = value;
			}
		}

		public bool IsDrawRight
		{
			get
			{
				return this._IsDrawRight;
			}
			set
			{
				this._IsDrawRight = value;
			}
		}

		public bool IsDrawBottom
		{
			get
			{
				return this._IsDrawBottom;
			}
			set
			{
				this._IsDrawBottom = value;
			}
		}

		public bool IsDrawLeft
		{
			get
			{
				return this._IsDrawLeft;
			}
			set
			{
				this._IsDrawLeft = value;
			}
		}

		public Color TopColor
		{
			get
			{
				return this._TopColor;
			}
			set
			{
				this._TopColor = value;
			}
		}

		public LineType TopStyle
		{
			get
			{
				return this._TopStyle;
			}
			set
			{
				this._TopStyle = value;
			}
		}

		public int TopWidth
		{
			get
			{
				return this._TopWidth;
			}
			set
			{
				this._TopWidth = value;
			}
		}

		public Color RightColor
		{
			get
			{
				return this._RightColor;
			}
			set
			{
				this._RightColor = value;
			}
		}

		public LineType RightStyle
		{
			get
			{
				return this._RightStyle;
			}
			set
			{
				this._RightStyle = value;
			}
		}

		public int RightWidth
		{
			get
			{
				return this._RightWidth;
			}
			set
			{
				this._RightWidth = value;
			}
		}

		public Color BottomColor
		{
			get
			{
				return this._BottomColor;
			}
			set
			{
				this._BottomColor = value;
			}
		}

		public LineType BottomStyle
		{
			get
			{
				return this._BottomStyle;
			}
			set
			{
				this._BottomStyle = value;
			}
		}

		public int BottomWidth
		{
			get
			{
				return this._BottomWidth;
			}
			set
			{
				this._BottomWidth = value;
			}
		}

		public Color LeftColor
		{
			get
			{
				return this._LeftColor;
			}
			set
			{
				this._LeftColor = value;
			}
		}

		public LineType LeftStyle
		{
			get
			{
				return this._LeftStyle;
			}
			set
			{
				this._LeftStyle = value;
			}
		}

		public int LeftWidth
		{
			get
			{
				return this._LeftWidth;
			}
			set
			{
				this._LeftWidth = value;
			}
		}
	}
}
