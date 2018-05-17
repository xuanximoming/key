using System;
using System.IO;

namespace DrectSoft.Basic.Paint.NET
{
    public interface ISurfaceStorage
    {
        void Load(IShapeReadonlySurface surface, Stream stream);
        void Save(IShapeSurface surface, Stream stream);
    }
}
