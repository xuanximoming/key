using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public class LabelShape : ShapeBase
    {
        #region fields
        private Font _font = new Font("ËÎÌå", 12f);

        #endregion

        #region properties
        public Font Font
        {
            get { return _font; }
            set { this._font = value; }
        }
        #endregion

        #region ctor
        public LabelShape()
            : base(ShapeType.Label) { }
        #endregion

        #region overrid method
        public new ShapeData Data
        {
            get { return base.Data; }
            set
            {
                base.Data = value;
                if (Data.Points == null ||
                    Data.Points.Length < MinPoints)
                    return;
                int x1, x2, y1, y2;
                x1 = Data.Points[0].X;
                y1 = Data.Points[0].Y;
                x2 = Data.Points[1].X;
                y2 = Data.Points[1].Y;
                RectangleF rect = RectangleF.FromLTRB(Math.Min(x1, x2), Math.Min(y1, y2),
                     Math.Max(x1, x2), Math.Max(y1, y2));
                //Font = new Font(Font.FontFamily, rect.Height, GraphicsUnit.Pixel);
            }
        }
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
            RectangleF rect = RectangleF.FromLTRB(Math.Min(x1, x2), Math.Min(y1, y2),
                 Math.Max(x1, x2), Math.Max(y1, y2));

            if (Data.IsFillColor)
                using (Brush br = base.GetBrush(nts))
                    g.FillRectangle(br, rect);
            try
            {
                //Font = new Font(Font.FontFamily, rect.Height, GraphicsUnit.Pixel);
                using (Brush brush = new SolidBrush(Data.DrawColor))
                {
                    g.DrawString(Data.Text, Font, brush,
                        rect, new StringFormat());
                }
            }
            catch { return; }
        }

        public override IGhost GetGhost()
        {
            IMouseDrawMode mode = new MouseDragDrawMode(this);
            return new Ghost(this, mode);
        }

        public override string ToString()
        {
            return "ÎÄ±¾";
        }
        #endregion
    }
}
