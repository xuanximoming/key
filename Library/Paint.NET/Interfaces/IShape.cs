using System;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IShape
        : ICloneable
    {
        bool Completed { get; }
        int MaxPoints { get; }
        int MinPoints { get; }
        ShapeData Data { get; set; }
        IBoundCalculator BoundCalculator { get; }
        void Complete();
        void Draw(Graphics g, NamedTextureStyles nts);
        void Reset();
        IGhost GetGhost();
        IShape GetNewEmptyShape();
    }
}
