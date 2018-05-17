using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://www.DrectSoft.com.cn/DrectSoftimage")]
    public sealed class CustomNamedHatchStyle
    {

        #region Fields
        private HatchStyle _style;
        private string _name;
        #endregion

        #region Ctors

        public CustomNamedHatchStyle() { }

        public CustomNamedHatchStyle(Pair<HatchStyle, string> pair)
        {
            Pair = pair;
        }

        #endregion

        #region Properties

        private Pair<HatchStyle, string> Pair
        {
            get { return new Pair<HatchStyle, string>(_style, _name); }
            set
            {
                _style = value.A;
                _name = value.B;
            }
        }

        [XmlElement(DataType = "int", ElementName = "style")]
        public int Style
        {
            get { return (int)_style; }
            set { _style = (HatchStyle)value; }
        }

        [XmlElement(DataType = "string", ElementName = "name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Explicit Operator

        public static explicit operator CustomNamedHatchStyle(Pair<HatchStyle, string> pair)
        {
            return new CustomNamedHatchStyle(pair);
        }

        public static explicit operator Pair<HatchStyle, string>(CustomNamedHatchStyle cnhs)
        {
            return cnhs.Pair;
        }

        #endregion

    }
}
