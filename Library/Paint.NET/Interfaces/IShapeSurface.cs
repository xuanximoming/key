using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IShapeSurface
        : IShapeReadonlySurface
    {
        IActionBuilder Builder { get; }
        bool ReadOnly { get; set; }
        int CurrentStep { get; }
        int MaxStep { get; }
        Color SurfaceDrawColor { get; set; }
        bool SurfaceIsFillColor { get; set; }
        Color SurfaceFillColor { get; set; }
        bool SurfaceIsHatch { get; set; }
        HatchStyle SurfaceHatch { get; set; }
        bool SurfaceIsTexture { get; set; }
        string SurfaceTexture { get; set; }
        float SurfaceLineWidth { get; set; }
        void NewGhost(ShapeType type);
        void Redo();
        void Undo();
        event EventHandler CurrentStepChanged;
        event EventHandler MaxStepChanged;
    }
}
