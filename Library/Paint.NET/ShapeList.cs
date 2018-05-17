using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    internal class ShapeList
        : List<IShape>
    {

        public ShapeList()
            : base() { }

        public ShapeList(IEnumerable<IShape> shapes)
            : base(shapes) { }

        public ShapeList(int cap)
            : base(cap) { }

        public void AddShape(IShape shape)
        {
            int index = IndexOf(shape);
            if (index == -1)
                this.Add(shape);
            else
                this[index].Data.IsBackground = true;
        }

        public IShape Select(int index)
        {
            IShape shape = this[index];
            shape.Data.IsBackground = false;
            return shape;
        }

        public void Deselect(int index)
        {
            IShape shape = this[index];
            shape.Data.IsBackground = true;
        }

        public bool Deselect(IShape shape)
        {
            int index = IndexOf(shape);
            if (index == -1)
                return false;
            Deselect(index);
            return true;
        }

    }
}
