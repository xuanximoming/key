using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public class EllipseShape
        : ShapeBase
    {

        #region Ctors

        public EllipseShape()
            : base(ShapeType.Ellipse) { }

        #endregion

        #region Override Members

        public override int MaxPoints
        {
            get { return 2; }
        }

        public override int MinPoints
        {
            get { return 2; }
        }

        public override IBoundCalculator BoundCalculator
        {
            get { return NormalBoundCalculator.Instance; }
        }

        public override void Draw(Graphics g, NamedTextureStyles nts)
        {
            if (Data.Points == null ||
                Data.Points.Length < MinPoints)
                return;
            int x1, x2, y1, y2;
            x1 = Data.Points[0].X;
            y1 = Data.Points[0].Y;
            x2 = Data.Points[1].X;
            y2 = Data.Points[1].Y;
            Rectangle rect = Rectangle.FromLTRB(
                Math.Min(x1, x2), Math.Min(y1, y2),
                Math.Max(x1, x2), Math.Max(y1, y2));

            if (Data.IsFillColor)
                using (Brush br = base.GetBrush(nts))
                    g.FillEllipse(br, rect);

            using (Pen pen = new Pen(Data.DrawColor, Data.LineWidth))
                g.DrawEllipse(pen, rect);
        }

        public override IGhost GetGhost()
        {
            IMouseDrawMode mode = new MouseDragDrawMode(this);
            return new Ghost(this, mode);
        }

        public override string ToString()
        {
            return "ÍÖÔ²";
        }

        #endregion

    }
}
