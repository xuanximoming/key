using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    /// <summary>
    /// 一些静态方法 add bt tj 2013-1-12
    /// </summary>
    public abstract class CommonMethods
    {

        /// <summary>
        /// 读某个节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static XmlNode GetElementByTagName(string nodeName, XmlDocument Doc)
        {
            try
            {
                return Doc.GetElementsByTagName(nodeName).Count == 0 ? null : Doc.GetElementsByTagName(nodeName)[0];
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读节点集合
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static XmlNodeList GetElementsByTagName(string nodeName, XmlDocument Doc)
        {
            try
            {
                return Doc.GetElementsByTagName(nodeName);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读节点属性
        /// </summary>
        /// <param name="elementNode"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string GetElementAttribute(XmlNode elementNode, string attribute, XmlDocument Doc)
        {
            try
            {
                return elementNode.Attributes[attribute] == null ? "" : elementNode.Attributes[attribute].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetElementAttribute(string elementNodeName, string attribute, XmlDocument Doc)
        {
            try
            {
                XmlNode node= GetElementByTagName(elementNodeName,  Doc);
                if (node == null)
                {
                    return "";
                }
                else
                {
                    return node.Attributes[attribute] == null ? "" : node.Attributes[attribute].Value;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
