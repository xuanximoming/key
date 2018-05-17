using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    public class Ghost
        : IGhost
    {

        #region Fields
        private static readonly Point[] NoPoints = new Point[0];
        private bool _dirty;
        private bool _isMotify;
        private IMouseDrawMode _mode;
        private IShape _shape;
        private ShapeData _oldData;
        #endregion

        #region Ctors

        public Ghost(IShape shape, IMouseDrawMode mode)
        {
            _dirty = true;
            _shape = shape;
            _mode = mode;
            _oldData = shape.Data;
            _shape.Data = shape.Data.Clone();
        }

        #endregion

        #region IGhost Members

        public bool Dirty
        {
            get { return _dirty; }
        }

        public GhostState State
        {
            get
            {
                if (_isMotify)
                    return GhostState.Modity;
                if (Shape == null || Shape.Data.Type == ShapeType.None)
                    return GhostState.NotAvailable;
                return GhostState.New;
            }
            set
            {
                switch (value)
                {
                    case GhostState.NotAvailable:
                    case GhostState.New:
                        _isMotify = false;
                        break;
                    case GhostState.Modity:
                        _isMotify = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public IShape Shape
        {
            get { return _shape; }
        }

        public IMouseDrawMode Mode
        {
            get { return _mode; }
        }

        public ShapeData OldData
        {
            get { return _oldData; }
        }

        public void Draw(Graphics g, NamedTextureStyles nts)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (Shape.Data.Enable)
                Shape.Draw(g, nts);
            Point[] pts = Shape.Data.Points;
            if (pts != null && pts.Length > 0)
            {
                Rectangle[] rects = new Rectangle[pts.Length];
                Rectangle rect;
                Point pt;
                Size s = new Size(5, 5);
                for (int i = 0; i < pts.Length; i++)
                {
                    pt = pts[i];
                    rect = new Rectangle(pt, s);
                    rect.Offset(-2, -2);
                    rects[i] = rect;
                }
                g.FillRectangles(Brushes.White, rects);
                g.DrawRectangles(Pens.Black, rects);
            }
            if (!Shape.Data.Bounds.IsEmpty)
                g.DrawRectangle(Pens.Blue, Shape.Data.Bounds);
            _dirty = false;
        }

        public int GetPointIndex(Point point, GhostState state)
        {
            if (state == GhostState.New)
                return int.MinValue;
            Point[] pts = Shape.Data.Points;
            Point p;
            for (int i = 0; i < pts.Length; i++)
            {
                p = point - (Size)pts[i];
                if (p.X < 3 && p.X > -3 &&
                    p.Y < 3 && p.Y > -3)
                    return i;
            }
            return -1;
        }

        public ActionResult SetMousePoint(int pointIndex, Point newPoint,
            MouseButtons button, MouseStates state)
        {
            if (pointIndex == int.MinValue && _shape.Completed)
                return ActionResult.Failed;
            ActionResult ar;
            if (pointIndex == -1)
                ar = MoveShape(newPoint, button, state);
            else
                ar = Mode.MouseDraw(pointIndex, newPoint, button, state);
            SetDirty();
            return ar;
        }

        public void NotifyDirty()
        {
            SetDirty();
        }

        public event EventHandler RequestRedraw;

        #endregion

        #region Private Members

        private ActionResult MoveShape(Point pt,
            MouseButtons button, MouseStates state)
        {
            if ((button | MouseButtons.Left) == MouseButtons.Left)
            {
                switch (state)
                {
                    case MouseStates.MouseDown:
                        return ActionResult.Successful;
                    case MouseStates.MouseMove:
                        MoveShapeCore(pt);
                        return ActionResult.Successful;
                    case MouseStates.MouseUp:
                        MoveShapeCore(pt);
                        return ActionResult.Completed;
                    default:
                        return ActionResult.Failed;
                }
            }
            return ActionResult.Failed;
        }

        private void MoveShapeCore(Point pt)
        {
            Point[] pts = Shape.Data.Points;
            Point p;
            pts = Shape.Data.Points;
            for (int i = 0; i < pts.Length; i++)
            {
                p = pts[i];
                p.Offset(pt);
                pts[i] = p;
            }
        }

        protected virtual void OnRequestRedraw(EventArgs args)
        {
            if (RequestRedraw != null)
                RequestRedraw(this, args);
        }

        private void SetDirty()
        {
            _dirty = true;
            OnRequestRedraw(EventArgs.Empty);
        }

        #endregion

    }

    public class NotAvailableGhost
        : IGhost
    {

        #region Fields
        public static readonly NotAvailableGhost Instance = new NotAvailableGhost();
        private IShape _shape;
        #endregion

        #region Ctors

        private NotAvailableGhost()
        {
            _shape = new NoneShape();
        }

        #endregion

        #region IGhost Members

        public bool Dirty
        {
            get { return false; }
        }

        public GhostState State
        {
            get { return GhostState.NotAvailable; }
            set { throw new NotImplementedException(); }
        }

        public IShape Shape
        {
            get { return _shape; }
        }

        public IMouseDrawMode Mode
        {
            get { return null; }
        }

        public ShapeData OldData
        {
            get { return _shape.Data; }
        }

        public void Draw(Graphics g, NamedTextureStyles nts) { }

        public int GetPointIndex(Point point, GhostState state)
        {
            return -1;
        }

        public ActionResult SetMousePoint(int pointIndex, Point newPoint, MouseButtons button, MouseStates state)
        {
            return ActionResult.Failed;
        }

        public void NotifyDirty() { }

        event EventHandler IGhost.RequestRedraw
        {
            add { }
            remove { }
        }

        #endregion

    }

}
