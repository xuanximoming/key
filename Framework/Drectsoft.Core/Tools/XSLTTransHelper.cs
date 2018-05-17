using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace DrectSoft.Core
{
    /// <summary>
    /// XSLT×ª»»¸¨ÖúÀà
    /// </summary>
    public class XsltTransHelper
    {
        private XPathDocument _xpDoc;
        private MemoryStream _msOut;
        private string _xsltUrl;
        private string _outFileName = string.Empty;

        private enum OutFormat
        { 
            Null = 0,
            File = 1,
            Stream = 2
        }

        private XsltTransHelper(string fileName, string xsltUrl)
        {
            _xpDoc = new XPathDocument(fileName);
            _msOut = new MemoryStream();
            _xsltUrl = xsltUrl;
        }

        private XsltTransHelper(Stream stream, string xsltUrl)
        {
            _xpDoc = new XPathDocument(stream);
            _msOut = new MemoryStream();
            _xsltUrl = xsltUrl;
        }

        /// <summary>
        /// xslt xml file
        /// </summary>
        /// <param name="xsltUraddress"></param>
        /// <param name="inStream"></param>
        /// <param name="outFileName"></param>
        public static void TransXml(string xsltUraddress, Stream inStream, out string outFileName)
        {
            XsltTransHelper xh = new XsltTransHelper(inStream, xsltUraddress);
            xh.InnerTrans(OutFormat.File);
            outFileName = xh._outFileName;
        }

        /// <summary>
        /// xslt xml file 2
        /// </summary>
        /// <param name="xsltUraddress"></param>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        public static void TransXml(string xsltUraddress, Stream inStream, out Stream outStream)
        {
            XsltTransHelper xh = new XsltTransHelper(inStream, xsltUraddress);
            xh.InnerTrans(OutFormat.Stream);
            outStream = xh._msOut;
        }

        /// <summary>
        /// xslt xml file 3
        /// </summary>
        /// <param name="xsltUraddress"></param>
        /// <param name="inFileName"></param>
        /// <param name="outFileName"></param>
        public static void TransXml(string xsltUraddress, string inFileName, out string outFileName)
        {
            XsltTransHelper xh = new XsltTransHelper(inFileName, xsltUraddress);
            xh.InnerTrans(OutFormat.File);
            outFileName = xh._outFileName;
        }

        /// <summary>
        /// xslt xml file 4
        /// </summary>
        /// <param name="xsltUraddress"></param>
        /// <param name="inFileName"></param>
        /// <param name="outStream"></param>
        public static void TransXml(string xsltUraddress, string inFileName, out Stream outStream)
        {
            XsltTransHelper xh = new XsltTransHelper(inFileName, xsltUraddress);
            xh.InnerTrans(OutFormat.Stream);
            outStream = xh._msOut;
        }

        private void InnerTrans(OutFormat formatter)
        {
            XslCompiledTransform xsl = new XslCompiledTransform();
            xsl.Load(_xsltUrl);

            xsl.Transform(_xpDoc, null, _msOut);

            if (formatter == OutFormat.File)
            {
                _outFileName = this.MemoryStreamSave2File();
            }

            _msOut.Close();
        }

        private string MemoryStreamSave2File()
        {
            string filename = Path.GetTempFileName();
            FileStream myFileStream = new FileStream(filename, FileMode.Create);

            _msOut.WriteTo(myFileStream);
            myFileStream.Flush();
            myFileStream.Close();

            return filename;
        }
    }
}
