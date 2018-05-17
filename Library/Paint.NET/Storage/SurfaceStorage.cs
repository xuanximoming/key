using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    public class SurfaceStorage
        : ISurfaceStorage
    {

        public SurfaceStorage() { }

        public void Load(IShapeReadonlySurface surface, Stream stream)
        {
            if (!stream.CanRead)
                throw new ArgumentException("stream");
            SurfaceStoreData data;
            SerializationBasedStreamItemReader reader =
                new SerializationBasedStreamItemReader(stream);
            long length = stream.Length;
            if (length > 0)
            {
                object[] datas = reader.Read();
                data = datas[0] as SurfaceStoreData;
                SurfaceStoreData.LoadStoredData(surface, data);
                if (datas.Length > 1)
                    surface.Source.Tag = datas[1];
            }
            else
            {
                surface.Source.Load(null, new Size(100, 100), null, null, null);
            }
        }

        public void Save(IShapeSurface surface, Stream stream)
        {
            if (!stream.CanWrite)
                throw new ArgumentException("stream");
            SerializationBasedStreamItemWriter writer =
                new SerializationBasedStreamItemWriter(stream);
            SurfaceStoreData data = SurfaceStoreData.CreateStoreData(surface);
            if (surface.Source.Tag == null)
                writer.Write(data);
            else
                writer.Write(data, surface.Source.Tag);
        }

    }

    public class XmlSurfaceStorage
        : ISurfaceStorage
    {

        public XmlSurfaceStorage() { }

        public void Load(IShapeReadonlySurface surface, Stream stream)
        {
            if (surface == null)
                throw new ArgumentNullException("surface");
            if (!stream.CanRead)
                throw new ArgumentException("stream");
            XmlSerializer xs = new XmlSerializer(typeof(SurfaceStoreData));
            try
            {
                SurfaceStoreData data = (SurfaceStoreData)xs.Deserialize(stream);
                SurfaceStoreData.LoadStoredData(surface, data);
            }
            catch (Exception ex)
            {
#if TRACE
                System.Diagnostics.Trace.TraceWarning("An exception occur on Loading NetWorkStudio Image: " + ex.Message);
#endif
                surface.Source.Load(null, new Size(100, 100), null, null, null);
            }
        }

        public void Save(IShapeSurface surface, Stream stream)
        {
            if (surface == null)
                throw new ArgumentNullException("surface");
            if (!stream.CanWrite)
                throw new ArgumentException("stream");
            SurfaceStoreData data = SurfaceStoreData.CreateStoreData(surface);
            XmlSerializer xs = new XmlSerializer(typeof(SurfaceStoreData));
            xs.Serialize(stream, data);
        }

    }
}
