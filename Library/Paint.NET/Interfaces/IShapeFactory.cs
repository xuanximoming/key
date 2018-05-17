using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IShapeFactory
    {
        IShape Create(ShapeData data);
    }
}
