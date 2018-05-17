using System;
using System.Drawing;

namespace DrectSoft.Basic.Paint.NET
{
    public class NormalBoundCalculator
        : IBoundCalculator
    {

        #region Fields
        public static readonly NormalBoundCalculator Instance =
            new NormalBoundCalculator();
        #endregion

        #region Ctors

        private NormalBoundCalculator() { }

        #endregion

        #region IBoundCalculator Members

        public Rectangle GetBounds(Point[] points)
        {
            if (points == null || points.Length == 0)
                return Rectangle.Empty;
            if (points.Length == 1)
                return new Rectangle(points[0], Size.Empty);

            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            foreach (Point pt in points)
            {
                minX = Math.Min(minX, pt.X);
                maxX = Math.Max(maxX, pt.X);
                minY = Math.Min(minY, pt.Y);
                maxY = Math.Max(maxY, pt.Y);
            }
            return Rectangle.FromLTRB(
                minX - 1, minY - 1, maxX + 1, maxY + 1);
        }

        #endregion

    }
}
