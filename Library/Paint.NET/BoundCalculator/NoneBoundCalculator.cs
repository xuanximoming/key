using System;
using System.Drawing;

namespace DrectSoft.Basic.Paint.NET
{
    public class NoneBoundCalculator
        : IBoundCalculator
    {

        #region Fields
        public static readonly NoneBoundCalculator Instance =
            new NoneBoundCalculator();
        #endregion

        #region Ctors

        private NoneBoundCalculator() { }

        #endregion

        #region IBoundCalculator Members

        public Rectangle GetBounds(Point[] points)
        {
            return Rectangle.Empty;
        }

        #endregion

    }
}
