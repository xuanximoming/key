using System;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IActionBuilder
    {
        IAction Build(ActionBuildType type, IShape shape);
        IShapeSource Source { get; }
    }
}
