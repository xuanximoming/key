using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DrectSoft.FrameWork.util
{
    /// <summary>
    /// xml 工具类
    /// </summary>
    public class XmlUtil
    {
        /// <summary>
        /// 读取指定节点属性值
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <param name="attributename">属性名称</param>
        /// <returns></returns>
        public static string GetAttributeValue(XmlNode node, string attributename)
        {
            if (node.Attributes[attributename] != null)
                return node.Attributes[attributename].Value;
            return null;
        }

        /// <summary>
        /// 读取指定属性值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributename"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string GetAttributeValue(XmlNode node, string attributename,bool ignoreCase)
        {
            string attrName=string.Empty;
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.Name.Equals(attributename, StringComparison.CurrentCultureIgnoreCase))
                    attrName = attr.Name;
            }            
            if (!string.IsNullOrEmpty(attrName))
                return node.Attributes[attrName].Value;
            return null;
        }
    }
}
