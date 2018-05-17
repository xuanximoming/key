using System.Xml;

namespace AutoUpdate
{
    public class XmlFiles : XmlDocument
    {
        private string _xmlFileName;

        public string XmlFileName
        {
            get
            {
                return this._xmlFileName;
            }
            set
            {
                this._xmlFileName = value;
            }
        }

        public XmlFiles(string xmlFile)
        {
            this.XmlFileName = xmlFile;
            this.Load(xmlFile);
        }

        public XmlNode FindNode(string xPath)
        {
            return base.SelectSingleNode(xPath);
        }

        public string GetNodeValue(string xPath)
        {
            XmlNode xmlNode = base.SelectSingleNode(xPath);
            return xmlNode.InnerText;
        }

        public XmlNodeList GetNodeList(string xPath)
        {
            return base.SelectSingleNode(xPath).ChildNodes;
        }
    }
}
