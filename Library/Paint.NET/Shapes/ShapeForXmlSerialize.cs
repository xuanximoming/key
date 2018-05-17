using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://www.DrectSoft.com.cn/DrectSoftimage")]
    public sealed class ShapeForXmlSerialize
        : IShape
    {

        #region Fields
        private ShapeData _data;
        #endregion

        #region Ctors

        public ShapeForXmlSerialize() { }

        public ShapeForXmlSerialize(IShape shape)
        {
            _data = shape.Data;
        }

        #endregion

        #region IShape Members

        bool IShape.Completed
        {
            get { return true; }
        }

        int IShape.MaxPoints
        {
            get { return 0; }
        }

        int IShape.MinPoints
        {
            get { return 0; }
        }

        [XmlElement(ElementName = "data")]
        public ShapeData Data
        {
            get { return _data; }
            set { _data = value; }
        }

        IBoundCalculator IShape.BoundCalculator
        {
            get { return null; }
        }

        void IShape.Complete() { }

        void IShape.Draw(Graphics g, NamedTextureStyles nts) { }

        void IShape.Reset() { }

        IGhost IShape.GetGhost()
        {
            return null;
        }

        IShape IShape.GetNewEmptyShape()
        {
            return null;
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this;
        }

        #endregion

    }
}
