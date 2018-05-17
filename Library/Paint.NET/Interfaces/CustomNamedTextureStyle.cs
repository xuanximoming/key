using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://www.DrectSoft.com.cn/DrectSoftimage")]
    public sealed class CustomNamedTextureStyle
    {

        #region Fields
        private string _name;
        private Picture _texture;
        #endregion

        #region Ctors

        public CustomNamedTextureStyle() { }

        public CustomNamedTextureStyle(Pair<Picture, string> pair)
            : this()
        {
            Pair = pair;
        }

        #endregion

        #region Members

        private Pair<Picture, string> Pair
        {
            get { return new Pair<Picture, string>(_texture, _name); }
            set
            {
                _texture = value.A;
                _name = value.B;
            }
        }

        [XmlElement(DataType = "string", ElementName = "name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlElement(ElementName = "texture")]
        public Picture Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        #endregion

        #region Explicit Operator

        public static explicit operator CustomNamedTextureStyle(Pair<Picture, string> pair)
        {
            return new CustomNamedTextureStyle(pair);
        }

        public static explicit operator Pair<Picture, string>(CustomNamedTextureStyle cnts)
        {
            return cnts.Pair;
        }

        #endregion

    }
}
