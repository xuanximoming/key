using System;
using System.Drawing;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IBoundCalculator
    {
        Rectangle GetBounds(Point[] points);
    }
}
