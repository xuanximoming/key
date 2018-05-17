using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IMouseDrawMode
    {
        IShape Shape { get; }
        ActionResult MouseDraw(int pointIndex, Point newPoint,
            MouseButtons button, MouseStates state);
    }
}
