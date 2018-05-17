using System;
using System.IO;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IStreamItemReader
    {
        Stream Stream { get; }
        object[] Read();
    }
}
