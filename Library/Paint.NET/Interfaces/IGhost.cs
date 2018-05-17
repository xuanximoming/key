using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IGhost
    {
        bool Dirty { get; }
        GhostState State { get; set; }
        IShape Shape { get; }
        IMouseDrawMode Mode { get; }
        ShapeData OldData { get; }
        void Draw(Graphics g, NamedTextureStyles nts);
        /// <summary>
        /// GetPointIndex
        /// </summary>
        /// <param name="point"></param>
        /// <param name="state"></param>
        /// <returns>if (state == SurfaceState.New) then return int.MinValue else if (not found) then return -1</returns>
        int GetPointIndex(Point point, GhostState state);
        /// <summary>
        /// SetMousePoint
        /// </summary>
        /// <param name="pointIndex">index of the old Point, int.MinValue means append, -1 means offset all</param>
        /// <param name="newPoint"></param>
        /// <param name="button"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        ActionResult SetMousePoint(int pointIndex, Point newPoint,
            MouseButtons button, MouseStates state);
        void NotifyDirty();
        event EventHandler RequestRedraw;
    }
}
