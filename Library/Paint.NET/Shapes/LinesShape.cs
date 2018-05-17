using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public class LinesShape
        : ShapeBase
    {

        #region Ctors

        public LinesShape()
            : base(ShapeType.Lines) { }

        #endregion

        #region Override Members

        public override int MaxPoints
        {
            get { return int.MaxValue; }
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
                g.DrawLines(pen, Data.Points);
        }

        public override IGhost GetGhost()
        {
            IMouseDrawMode mode = new MouseMoveDrawMode(this);
            return new Ghost(this, mode);
        }

        public override string ToString()
        {
            return "Ç¦±ÊÏß";
        }

        #endregion

    }
}
