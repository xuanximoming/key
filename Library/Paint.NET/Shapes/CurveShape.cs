using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public class CurveShape
        : ShapeBase
    {

        #region Ctors

        public CurveShape()
            : base(ShapeType.Curve) { }

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
            using (Pen pen = new Pen(Data.DrawColor, Data.LineWidth))
                g.DrawCurve(pen, Data.Points);
        }

        public override IGhost GetGhost()
        {
            IMouseDrawMode mode = new UnlimitedMouseDownDrawMode(this);
            return new Ghost(this, mode);
        }

        public override string ToString()
        {
            return "ÇúÏß";
        }

        #endregion

    }
}
