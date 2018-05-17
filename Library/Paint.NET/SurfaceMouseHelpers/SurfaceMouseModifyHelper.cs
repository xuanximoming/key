using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    public class SurfaceMouseModifyHelper
    {

        #region Fields
        private IShapeSurface _surface;
        private int _mouseDownIndex = int.MinValue;
        private Point? _lastMousePosition;
        #endregion

        #region Ctors

        public SurfaceMouseModifyHelper(IShapeSurface surface)
        {
            _surface = surface;
        }

        #endregion

        #region Members

        public IShapeSource Source
        {
            get { return _surface.Source; }
        }

        public ActionBuildType ProcessMouseDown(
            Point pt, MouseButtons button)
        {
            ActionResult ar;
            if (!Source.Ghost.Shape.Data.Bounds.Contains(pt))
            {
                _lastMousePosition = null;
                _mouseDownIndex = int.MinValue;
                return ActionBuildType.Motify;
            }
            int index = Source.Ghost.GetPointIndex(pt, Source.Ghost.State);
            if (index == -1)
            {
                ar = Source.Ghost.SetMousePoint(
                    index, Point.Empty, button, MouseStates.MouseDown);
                if (ar != ActionResult.Failed)
                {
                    _mouseDownIndex = index;
                    _lastMousePosition = pt;
                }
            }
            else
            {
                ar = Source.Ghost.SetMousePoint(
                    index, pt, button, MouseStates.MouseDown);
                if (ar != ActionResult.Failed)
                    _mouseDownIndex = index;
            }
            AfterMotify(ar);
            return ActionBuildType.None;
        }

        public ActionBuildType ProcessMouseMove(
            Point pt, MouseButtons button)
        {
            if (_mouseDownIndex == int.MinValue)
                return ActionBuildType.None;
            ActionResult ar;
            int index = _mouseDownIndex;
            if (index == -1)
            {
                Point p = pt - (Size)_lastMousePosition.Value;
                ar = Source.Ghost.SetMousePoint(
                    index, p, button, MouseStates.MouseMove);
                if (ar != ActionResult.Failed)
                {
                    _mouseDownIndex = index;
                    _lastMousePosition = pt;
                }
            }
            else
            {
                ar = Source.Ghost.SetMousePoint(
                    index, pt, button, MouseStates.MouseMove);
            }
            AfterMotify(ar);
            return ActionBuildType.None;
        }

        public ActionBuildType ProcessMouseUp(
            Point pt, MouseButtons button)
        {
            if (_mouseDownIndex == int.MinValue)
                return ActionBuildType.None;
            ActionResult ar;
            int index = _mouseDownIndex;
            if (index == -1)
            {
                Point p = pt - (Size)_lastMousePosition.Value;
                ar = Source.Ghost.SetMousePoint(
                    index, p, button, MouseStates.MouseUp);
                switch (ar)
                {
                    case ActionResult.Successful:
                        _mouseDownIndex = index;
                        _lastMousePosition = pt;
                        break;
                    case ActionResult.Completed:
                        _mouseDownIndex = int.MinValue;
                        _lastMousePosition = null;
                        break;
                    case ActionResult.Failed:
                    default:
                        break;
                }
            }
            else
            {
                ar = Source.Ghost.SetMousePoint(
                    index, pt, button, MouseStates.MouseUp);
            }
            AfterMotify(ar);
            return ActionBuildType.None;
        }

        private void AfterMotify(ActionResult ar)
        {
            if (ar != ActionResult.Failed)
            {
                IShape shape = Source.Ghost.Shape;
                shape.Data.Bounds = shape.BoundCalculator.GetBounds(shape.Data.Points);
            }
        }

        #endregion

    }
}
