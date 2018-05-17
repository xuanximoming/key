using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DrectSoft.Basic.Paint.NET
{
    public class SerializationBasedStreamItemWriter
        : IStreamItemWriter
    {

        #region Fields
        private Stream _stream;
        private BinaryFormatter _bf;
        #endregion

        #region Ctors

        public SerializationBasedStreamItemWriter(Stream stream)
        {
            _stream = stream;
            _bf = new BinaryFormatter();
        }

        #endregion

        #region IStreamItemWriter Members

        public Stream Stream
        {
            get { return _stream; }
        }

        public void Write(params object[] objs)
        {
            foreach (object obj in objs)
                using (MemoryStream ms = new MemoryStream())
                {
                    _bf.Serialize(ms, obj);
                    int length = (int)ms.Length;
                    byte[] bytes = BitConverter.GetBytes(length);
                    Stream.Write(bytes, 0, bytes.Length);
                    ms.WriteTo(Stream);
                }
        }

        #endregion

    }
}
