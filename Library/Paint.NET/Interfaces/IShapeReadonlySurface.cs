using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IShapeReadonlySurface
    {
        IShapeSource Source { get; set; }
        void Clear();
    }
}
