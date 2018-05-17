using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IAction
    {
        ActionType Action { get; }
    }

    public interface INewShape
        : IAction
    {
        ShapeData Data { get; }
    }

    public interface IModifyShape
        : IAction
    {
        int ShapeIndex { get; }
        ShapeData NewData { get; }
        ShapeData OldData { get; }
    }

    public interface ISelectShape
        : IAction
    {
        int ShapeIndex { get; }
        SelectType Type { get; }
    }

    public interface IDeleteShape
        : IAction
    {
        int ShapeIndex { get; }
    }

    public interface ICreateGhost
        : IAction
    {
        ShapeData Data { get; }
    }
}
