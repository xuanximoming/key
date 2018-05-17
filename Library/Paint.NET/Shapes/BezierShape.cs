using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public class BezierShape
        : ShapeBase
    {

        #region Ctors

        public BezierShape()
            : base(ShapeType.Bezier) { }

        #endregion

        #region Override Members

        public override int MaxPoints
        {
            get { return 4; }
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
            Point pt0, pt1, pt2, pt3;
            switch (Data.Points.Length)
            {
                case 2:
                    pt0 = Data.Points[0];
                    pt1 = Data.Points[1];
                    pt2 = Data.Points[0];
                    pt3 = Data.Points[1];
                    break;
                case 3:
                    pt0 = Data.Points[0];
                    pt1 = Data.Points[1];
                    pt2 = Data.Points[2];
                    pt3 = Data.Points[2];
                    break;
                case 4:
                default:
                    pt0 = Data.Points[0];
                    pt1 = Data.Points[1];
                    pt2 = Data.Points[2];
                    pt3 = Data.Points[3];
                    break;
            }
            using (Pen pen = new Pen(Data.DrawColor, Data.LineWidth))
                g.DrawBezier(pen, pt0, pt2, pt3, pt1);
        }

        public override IGhost GetGhost()
        {
            IMouseDrawMode mode = new LimitedMouseDownDrawMode(this);
            return new Ghost(this, mode);
        }

        public override string ToString()
        {
            return "±´Èû¶ûÇúÏß";
        }

        #endregion

    }
}
