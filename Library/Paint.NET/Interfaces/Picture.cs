using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://www.DrectSoft.com.cn/DrectSoftimage")]
    public  class Picture
    {

        #region Fields
        private object _item;
        private Image _picBuff;
        private static Image BadImage = new Bitmap(10, 10);
        #endregion

        #region Ctors

        public Picture() { }

        public Picture(Image image)
        {
            if (image == null)
                return;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                Item = ms.ToArray();
                _picBuff = image;
            }
        }

        public Picture(byte[] bytes)
            : this()
        {
            Item = bytes;
        }

        public Picture(string imageRef)
            : this()
        {
            Item = imageRef;
        }

        #endregion

        #region Members

        [XmlElement("picture", typeof(byte[]), DataType = "base64Binary")]
        [XmlElement("pictureRef", typeof(string))]
        public object Item
        {
            get { return _item; }
            set
            {
                _item = value;
                _picBuff = null;
            }
        }

        public Image GetImage()
        {
            if (_picBuff == null)
            {
                byte[] _byteItem = _item as byte[];
                string _stringItem = _item as string;
                if (_byteItem != null)
                {
                    MemoryStream ms = new MemoryStream(_byteItem);
                    try
                    {
                        _picBuff = Image.FromStream(ms);
                    }
                    catch
                    {
                        _picBuff = BadImage;
                    }
                }
                else if (string.IsNullOrEmpty(_stringItem))
                {
                    // load from db
                }
            }
            return _picBuff;
        }

        #endregion

    }
}
