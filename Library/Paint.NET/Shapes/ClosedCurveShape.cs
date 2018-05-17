using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public class ClosedCurveShape
        : ShapeBase
    {

        #region Ctors

        public ClosedCurveShape()
            : base(ShapeType.ClosedCurve) { }

        #endregion

        #region Override Members

        public override int MaxPoints
        {
            get { return int.MaxValue; }
        }

        public override int MinPoints
        {
            get { return 3; }
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

            if (Data.IsFillColor)
                using (Brush br = base.GetBrush(nts))
                    g.FillClosedCurve(br, Data.Points);

            using (Pen pen = new Pen(Data.DrawColor, Data.LineWidth))
                g.DrawClosedCurve(pen, Data.Points);
        }

        public override IGhost GetGhost()
        {
            IMouseDrawMode mode = new UnlimitedMouseDownDrawMode(this);
            return new Ghost(this, mode);
        }

        public override string ToString()
        {
            return "·â±ÕÇúÏß";
        }

        #endregion

    }
}
