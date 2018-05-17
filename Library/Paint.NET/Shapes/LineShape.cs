using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public class LineShape
        : ShapeBase
    {

        #region Ctors

        public LineShape()
            : base(ShapeType.Line) { }

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
            using (Pen pen = new Pen(Data.DrawColor, Data.LineWidth))
                g.DrawLine(pen, Data.Points[0], Data.Points[1]);
        }

        public override IGhost GetGhost()
        {
            IMouseDrawMode mode = new MouseDragDrawMode(this);
            return new Ghost(this, mode);
        }

        public override string ToString()
        {
            return "Ö±Ïß";
        }

        #endregion

    }
}
