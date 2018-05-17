using System;
using System.IO;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IStreamItemWriter
    {
        Stream Stream { get; }
        void Write(params object[] objs);
    }
}
