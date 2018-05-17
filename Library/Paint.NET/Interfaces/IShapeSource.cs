using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IShapeSource
    {
        IList<IShape> Shapes { get; }
        IShapeFactory Factory { get; }
        IGhost Ghost { get; }
         ///
        /// 图元层操作台（编辑工作台）
        /// </summary>
        IShapeSurface Surface { get;set;}

        IShapeSelector Selector { get; }
        NamedHatchStyles NamedHatchStyle { get; }
        NamedTextureStyles NamedTextureStyle { get; }
        object Tag { get; set; }
        Picture Background { get; }
        Image Current { get; }
        Size Size { get; }
        bool IsLoaded { get; }
        bool Dirty { get; set; }
        void Add(ShapeData data);
        void Modify(int index, ShapeData data);
        void Remove();
        void Enable(int index);
        void Disable(int index);
        void Select(int index);
        void Deselect();
        void Load(Picture background, Size size,
            NamedHatchStyles namedHatchStyle,
            NamedTextureStyles namedTextureStyle,
            IEnumerable<IShape> shapes);
        void Clear();
        void Redraw();
        void SetNewShape(ShapeData data);
        void ResetGhost();
        event EventHandler CurrentChanged;
        event EventHandler Loading;
        event EventHandler Loaded;
        event EventHandler GhostChanged;
    }
}
