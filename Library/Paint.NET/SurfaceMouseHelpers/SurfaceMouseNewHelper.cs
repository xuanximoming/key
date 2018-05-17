using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    public class SurfaceMouseNewHelper
    {

        #region Fields
        private IShapeSurface _surface;
        #endregion

        #region Ctors

        public SurfaceMouseNewHelper(IShapeSurface surface)
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
            ActionResult ar = Source.Ghost.SetMousePoint(
                int.MinValue, pt, button, MouseStates.MouseDown);
            if (ar == ActionResult.Completed)
                return ActionBuildType.AddNew;
            return ActionBuildType.None;
        }

        public ActionBuildType ProcessMouseMove(
            Point pt, MouseButtons button)
        {
            ActionResult ar = Source.Ghost.SetMousePoint(
                int.MinValue, pt, button, MouseStates.MouseMove);
            if (ar == ActionResult.Completed)
                return ActionBuildType.AddNew;
            return ActionBuildType.None;
        }

        public ActionBuildType ProcessMouseUp(
            Point pt, MouseButtons button)
        {
            ActionResult ar = Source.Ghost.SetMousePoint(
                int.MinValue, pt, button, MouseStates.MouseUp);
            if (ar == ActionResult.Completed)
                return ActionBuildType.AddNew;
            return ActionBuildType.None;
        }

        #endregion

    }
}
