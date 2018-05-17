using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://www.DrectSoft.com.cn/DrectSoftimage")]
    public sealed class SurfaceStoreData
    {

        #region Fields
        private Picture _picture;
        private Size _size;
        private NamedHatchStylesForXmlSerialize _namedHatchStyle;
        private NamedTextureStylesForXmlSerialize _namedTextureStyle;
        private ShapeForXmlSerialize[] _shapes;
        #endregion

        #region Ctors

        private SurfaceStoreData() { }

        #endregion

        #region Static Members

        public static SurfaceStoreData CreateStoreData(
            IShapeSurface surface)
        {
            SurfaceStoreData result = new SurfaceStoreData();
            IShapeSource src = surface.Source;
            // save the last ghost
            surface.NewGhost(ShapeType.None);
            result._picture = src.Background;
            result._size = src.Size;
            if (!src.NamedHatchStyle.IsEmpty)
                result._namedHatchStyle = (NamedHatchStylesForXmlSerialize)src.NamedHatchStyle;
            if (!src.NamedTextureStyle.IsEmpty)
                result._namedTextureStyle = (NamedTextureStylesForXmlSerialize)src.NamedTextureStyle;
            IList<IShape> shapes = src.Shapes;
            if (shapes != null && shapes.Count > 0)
            {
                result._shapes = new ShapeForXmlSerialize[shapes.Count];
                for (int i = 0; i < shapes.Count; i++)
                    result._shapes[i] = new ShapeForXmlSerialize(shapes[i]);
            }
            return result;
        }

        public static void LoadStoredData(
            IShapeReadonlySurface surface, SurfaceStoreData data)
        {
            surface.Clear();
            IShape[] shapes = null;
            if (data._shapes != null)
            {
                shapes = new IShape[data._shapes.Length];
                IShape shape;
                for (int i = 0; i < data._shapes.Length; i++)
                {
                    shape = data._shapes[i];
                    shape.Data.IsBackground = true;
                    shapes[i] = surface.Source.Factory.Create(shape.Data);
                }
            }
            surface.Source.Load(data._picture, data._size,
                (NamedHatchStyles)data._namedHatchStyle,
                (NamedTextureStyles)data._namedTextureStyle, shapes);
        }

        #endregion

        #region Properties

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement(ElementName = "size")]
        public Size Size
        {
            get { return _size; }
            set { _size = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement(ElementName = "background")]
        public Picture Background
        {
            get { return _picture; }
            set { _picture = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement(ElementName = "namedHatchStyles")]
        public NamedHatchStylesForXmlSerialize NamedHatchStyles
        {
            get { return _namedHatchStyle; }
            set { _namedHatchStyle = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement(ElementName = "namedTextureStyles")]
        public NamedTextureStylesForXmlSerialize NamedTextureStyles
        {
            get { return _namedTextureStyle; }
            set { _namedTextureStyle = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlArrayItem("shape", IsNullable = true)]
        public ShapeForXmlSerialize[] shapes
        {
            get { return _shapes; }
            set { _shapes = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlArrayItem("trace", IsNullable = true)]
        public object[] traces
        {
            get { return null; }
            set { }
        }

        #endregion

    }
}
