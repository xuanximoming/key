
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DrectSoft.Basic.Paint.NET
{
    public class MouseDragDrawMode
        : IMouseDrawMode
    {

        #region Fields
        private IShape _shape;
        #endregion

        #region Ctors

        public MouseDragDrawMode(IShape shape)
        {
            _shape = shape;
        }

        #endregion

        #region IMouseDrawMode Members

        public IShape Shape
        {
            get { return _shape; }
        }

        public ActionResult MouseDraw(int pointIndex,
            Point point, MouseButtons button, MouseStates state)
        {
            if (pointIndex == int.MinValue && _shape.Completed)
                return ActionResult.Failed;
            if ((button & MouseButtons.Left) != MouseButtons.Left)
                return ActionResult.Failed;
            if (pointIndex == int.MinValue &&
                (button & MouseButtons.Left) == MouseButtons.Left &&
                state == MouseStates.MouseUp)
            {
                _shape.Complete();
                if (pointIndex == int.MinValue)
                {
                    AppendPoint(point);
                    return ActionResult.Completed;
                }
            }
            if (pointIndex == int.MinValue)
                return AppendPoint(point);
            else
                return UpdatePoint(pointIndex, point);
        }

        #endregion

        #region Private Members

        private bool IsMaxPoints
        {
            get
            {
                return _shape.Data.Points != null &&
                    _shape.Data.Points.Length == 2;
            }
        }

        private ActionResult AppendPoint(Point point)
        {
            Point[] oldPts;
            Point[] newPts;
            if (_shape == null || _shape.Data == null)
                return ActionResult.Failed;
            if (_shape.Data.Points == null)
            {
                newPts = new Point[1];
            }
            else
            {
                oldPts = _shape.Data.Points;
                if (oldPts.Length == 2)
                {
                    newPts = oldPts;
                }
                else
                {
                    newPts = new Point[2];
                    newPts[0] = oldPts[0];
                }
            }
            newPts[newPts.Length - 1] = point;
            _shape.Data.Points = newPts;
            _shape.Data.Bounds = _shape.BoundCalculator.GetBounds(newPts);
            return ActionResult.Successful;
        }

        private ActionResult UpdatePoint(int pointIndex, Point point)
        {
            if (pointIndex < 0 || pointIndex > Shape.Data.Points.Length)
                return ActionResult.Failed;
            Shape.Data.Points[pointIndex] = point;
            return ActionResult.Successful;
        }

        #endregion

    }
}
