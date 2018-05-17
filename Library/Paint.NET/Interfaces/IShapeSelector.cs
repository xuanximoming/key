using System;
using System.Drawing;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IShapeSelector
    {
        IShapeSource Source { get; }
        ActionBuildType Select(Point pt, out int index);
        void AdvanceSelect(Point pt, ShapeSelectorCallback callback);
    }

    public delegate void ShapeSelectorCallback(ActionBuildType type, int index);
}
