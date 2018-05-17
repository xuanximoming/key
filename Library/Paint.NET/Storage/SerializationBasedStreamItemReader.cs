using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace DrectSoft.Basic.Paint.NET
{
    public class SerializationBasedStreamItemReader
        : IStreamItemReader
    {

        #region Fields
        private Stream _stream;
        private BinaryFormatter _bf;
        #endregion

        #region Ctors

        public SerializationBasedStreamItemReader(Stream stream)
        {
            _stream = stream;
            _bf = new BinaryFormatter();
        }

        #endregion

        #region IStreamItemReader Members

        public Stream Stream
        {
            get { return _stream; }
        }

        public object[] Read()
        {
            ArrayList al = new ArrayList();
            long streamLength = Stream.Length;
            byte[] bytes, data;
            int length;
            while (Stream.Position < streamLength)
            {
                bytes = new byte[4];
                Stream.Read(bytes, 0, 4);
                length = BitConverter.ToInt32(bytes, 0);
                data = new byte[length];
                Stream.Read(data, 0, length);
                using (MemoryStream ms = new MemoryStream(data))
                    al.Add(_bf.Deserialize(ms));
            }
            return al.ToArray();
        }

        #endregion

    }
}
