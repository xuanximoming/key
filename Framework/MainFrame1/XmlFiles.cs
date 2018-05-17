using System.Xml;

namespace DrectSoft.MainFrame
{
    public class XmlFiles : XmlDocument
    {
        #region 自定义成员
        private string _xmlFileName;
        public string XmlFileName
        {
            set
            {
                _xmlFileName = value;
            }
            get
            {
                return _xmlFileName;
            }
        }
        #endregion

        #region 构造函数
        public XmlFiles(string xmlFile)
        {
            XmlFileName = xmlFile;

            this.Load(xmlFile);
        }
        #endregion

        #region 给定一个节点的xPath表达式，并返回一个节点
        /// <summary>
        /// 给定一个节点的xPath表达式，并返回一个节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public XmlNode FindNode(string xPath)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            return xmlNode;
        }
        #endregion

        #region 给定一个节点的xPath表达式，返回其值
        /// <summary>
        /// 给定一个节点的xPath表达式，返回其值
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public string GetNodeValue(string xPath)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            return xmlNode.InnerText;
        }
        #endregion

        #region 给定一个节点的xPath表达式，返回此节点下的子节点列表
        /// <summary>
        /// 给定一个节点的xPath表达式，返回此节点下的子节点列表
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public XmlNodeList GetNodeList(string xPath)
        {
            XmlNodeList nodeList = this.SelectSingleNode(xPath).ChildNodes;
            return nodeList;
        }
        #endregion
    }
}
