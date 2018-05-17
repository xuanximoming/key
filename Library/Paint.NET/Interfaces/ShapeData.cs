using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://www.DrectSoft.com.cn/DrectSoftimage")]
    public sealed class ShapeData
        : ICloneable
    {

        #region Subclass WPoint

        [Serializable]
        [DebuggerStepThrough]
        [XmlType(Namespace = "http://www.DrectSoft.com.cn/DrectSoftimage")]
        public sealed class WPoint
        {

            #region Fields
            private int _x;
            private int _y;
            #endregion

            #region Ctors

            public WPoint()
                : this(0, 0) { }

            public WPoint(Point pt)
                : this(pt.X, pt.Y) { }

            public WPoint(int x, int y)
            {
                _x = x;
                _y = y;
            }

            #endregion

            #region Properties

            [XmlElement("x", DataType="int")]
            public int X
            {
                get { return _x; }
                set { _x = value; }
            }

            [XmlElement("y", DataType="int")]
            public int Y
            {
                get { return _y; }
                set { _y = value; }
            }

            #endregion

            #region Implicit Operator

            public static implicit operator WPoint(Point pt)
            {
                return new WPoint(pt);
            }

            public static implicit operator Point(WPoint wpt)
            {
                return new Point(wpt.X, wpt.Y);
            }

            #endregion

        }

        #endregion

        #region Fields
        private Point[] _points;
        [NonSerialized]
        private Rectangle _bounds;
        private ShapeType _type;
        private Color _drawColor;
        private bool _isFillColor;
        private Color _fillColor;
        private bool _isHatch;
        private HatchStyle _hatch;
        private bool _isTexture;
        private string _textureName;
        private float _lineWidth;
        private string _text;
        private bool _enable;
        private bool _isBackground;
        #endregion

        #region Ctors

        private ShapeData() { }

        public ShapeData(ShapeType type)
        {
            _type = type;
            _enable = true;
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public Point[] Points
        {
            get { return _points; }
            set { _points = value; }
        }

        [Browsable(false)]
        [CLSCompliant(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlArrayItem("point", IsNullable = true)]
        public WPoint[] points
        {
            get
            {
                if (_points == null)
                    return null;
                WPoint[] result = new WPoint[_points.Length];
                for (int i = 0; i < _points.Length; i++)
                    result[i] = _points[i];
                return result;
            }
            set
            {
                if (value == null)
                {
                    _points = null;
                }
                else
                {
                    _points = new Point[value.Length];
                    for (int i = 0; i < value.Length; i++)
                        _points[i] = value[i];
                }
            }
        }

        [XmlIgnore]
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement(DataType = "int", ElementName = "type")]
        public int IntType
        {
            get { return (int)Type; }
            set { Type = (ShapeType)value; }
        }

        [XmlIgnore]
        public ShapeType Type
        {
            get { return _type; }
            private set { _type = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement(DataType = "int", ElementName = "drawColor")]
        public int IntDrawColor
        {
            get { return DrawColor.ToArgb(); }
            set { DrawColor = Color.FromArgb(value); }
        }

        [XmlIgnore]
        public Color DrawColor
        {
            get { return _drawColor; }
            set { _drawColor = value; }
        }

        [XmlElement(DataType = "boolean", ElementName = "isFillColor")]
        public bool IsFillColor
        {
            get { return _isFillColor; }
            set { _isFillColor = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement(DataType = "int", ElementName = "fillColor")]
        public int IntFillColor
        {
            get { return FillColor.ToArgb(); }
            set { FillColor = Color.FromArgb(value); }
        }

        [XmlIgnore]
        public Color FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        [XmlElement(DataType = "boolean", ElementName = "isHatch")]
        public bool IsHatch
        {
            get { return _isHatch; }
            set { _isHatch = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement(DataType = "int", ElementName = "hatch")]
        public int IntHatch
        {
            get { return (int)Hatch; }
            set { Hatch = (HatchStyle)value; }
        }

        [XmlIgnore]
        public HatchStyle Hatch
        {
            get { return _hatch; }
            set { _hatch = value; }
        }

        [XmlElement(DataType = "boolean", ElementName = "isTexture")]
        public bool IsTexture
        {
            get { return _isTexture; }
            set { _isTexture = value; }
        }

        [XmlElement(DataType = "string", ElementName = "textureName")]
        public string TextureName
        {
            get { return _textureName; }
            set { _textureName = value; }
        }

        [XmlElement(DataType = "float", ElementName = "lineWidth")]
        public float LineWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }

        [XmlElement(DataType = "string", ElementName = "text")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        [XmlElement(DataType = "boolean", ElementName = "enable")]
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        [XmlIgnore]
        public bool IsBackground
        {
            get { return _isBackground; }
            set { _isBackground = value; }
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public ShapeData Clone()
        {
            ShapeData result = (ShapeData)base.MemberwiseClone();
            if (result._points != null)
                result._points = (Point[])result._points.Clone();
            return result;
        }

        public ShapeData CloneWithoutPoints()
        {
            ShapeData result = (ShapeData)base.MemberwiseClone();
            result._points = null;
            result._bounds = Rectangle.Empty;
            return result;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            ShapeData data = obj as ShapeData;
            if (data == null)
                return false;
            return Equals(data);
        }

        private bool Equals(ShapeData data)
        {
            if (this._points == null || data._points == null)
            {
                if (this._points != null || data._points != null)
                    return false;
            }
            else
            {
                for (int i = 0; i < this._points.Length; i++)
                    if (this._points[i] != data._points[i])
                        return false;
            }       
            if (this._bounds != data._bounds)
                return false;
            if (this._type != data._type)
                return false;
            if (this._drawColor != data._drawColor)
                return false;
            if (this._isFillColor != data._isFillColor)
                return false;
            if (this._fillColor != data._fillColor)
                return false;
            if (this._isHatch != data._isHatch)
                return false;
            if (this._hatch != data._hatch)
                return false;
            if (this._isTexture != data._isTexture)
                return false;
            if (this._textureName != data._textureName)
                return false;
            if (this._lineWidth != data._lineWidth)
                return false;
            if (this._text != data._text)
                return false;
            if (this._enable != data._enable)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            return (int)_type ^ _bounds.GetHashCode();
        }

        #endregion

    }
}
