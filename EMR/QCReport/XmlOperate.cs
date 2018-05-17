using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.QCReport
{
    /// <summary>
    /// 操作报表XML文件的工具类
    /// </summary>
    public class XmlOperate
    {
        XmlDocument doc =null;
        private string path = AppDomain.CurrentDomain.BaseDirectory + @"Sheet\Sheet.xml";
        //private static XmlOperate instance = null;
        //public static XmlOperate GetInstance()
        //{
        //    if (instance == null)
        //    {
        //        instance = new XmlOperate();
        //    }
        //    return instance;
        //}

        public XmlOperate()
        {
            try
            {
                doc = new XmlDocument();
                doc.Load(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
                
        }

        /// <summary>
        /// 全路径
        /// </summary>
        /// <param name="mypath"></param>
        public XmlOperate(string mypath)
        {
            try
            {
                path = mypath;
                doc = new XmlDocument();
                doc.Load(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public XmlNode GetReportByName(string reportNodeName)
        {
            XmlNode node = null;
            try
            {
                XmlNodeList reportsList=doc.GetElementsByTagName("reports")[0].ChildNodes;
                foreach (XmlNode _node in reportsList)
                {
                    if (_node.Attributes["name"] != null && _node.Attributes["name"].Value == reportNodeName)
                    {
                        node = _node;
                        break;
                    }
                }
                return node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlQuery GetReportParam(string reportNodeName)
        {
            List<ParamInfo> list = null;
            try
            {
                SqlQuery sqlQuery = new SqlQuery(); 
                string procedureName = string.Empty;
                list = new List<ParamInfo>();
                XmlNode node = GetReportByName(reportNodeName);
                if (node != null && node.HasChildNodes)
                {
                    XmlNode sqlNode = node.SelectSingleNode("sql");
                    procedureName = sqlNode.Attributes["procedure"] == null || string.IsNullOrEmpty(sqlNode.Attributes["procedure"].Value) ? "" : sqlNode.Attributes["procedure"].Value;
                    if (sqlNode != null && sqlNode.HasChildNodes)
                    {
                        foreach (XmlNode _n in sqlNode.ChildNodes)
                        {
                            ParamInfo param = new ParamInfo(_n.Attributes["name"].Value, _n.Attributes["controltype"].Value,_n.Attributes["controlcaption"].Value, _n.Attributes["break"].Value);
                            list.Add(param);
                        }
                    }
                }
                sqlQuery.sqlStr = procedureName;
                sqlQuery.paramList = list;
                return sqlQuery;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public List<DataColumn> GetColumns(string reportNodeName)
        {
            List<DataColumn> list = null;
            try
            {
                list = new List<DataColumn>();
                XmlNode node = GetReportByName(reportNodeName);
                if (node != null && node.HasChildNodes)
                {
                    XmlNode columnsNode = node.SelectSingleNode("datacolomns");
                    if (columnsNode != null && columnsNode.HasChildNodes)
                    {
                        foreach (XmlNode _n in columnsNode.ChildNodes)
                        {
                            DataColumn dc = new DataColumn(_n.Attributes["datafield"].Value, _n.Attributes["width"].Value, _n.Attributes["caption"].Value);
                            list.Add(dc);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public XmlNode GetPrintSetting(string reportNodeName)
        {
            XmlNode returnNode = null;
            try
            {
                XmlNode temp=null;
                XmlNodeList reportsList = doc.SelectNodes("/reports/report");
                foreach (XmlNode _node in reportsList)
                {
                    if (_node.Attributes["name"] != null && _node.Attributes["name"].Value == reportNodeName)
                    {
                        temp = _node;
                        break;
                    }
                }
                if (temp == null)
                {
                    return null;
                }
                foreach (XmlNode _node in temp.ChildNodes)
                {
                    if (_node.LocalName.Trim().ToLower().Equals("printsetting"))
                    {
                        returnNode = _node;
                        break;
                    }
                }
                return returnNode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<string,List<string>> GetReportsCaption()
        {
            try 
	        {	        
                Dictionary<string,List<string>> dic=new Dictionary<string,List<string>>();
		        XmlNodeList reportNodes=doc.GetElementsByTagName("report");
                foreach(XmlNode node in reportNodes)
                {
                    if (dic.Keys.Contains(node.Attributes["groupname"].Value))//已含有此组名
                    {
                         dic[node.Attributes["groupname"].Value].Add(node.Attributes["name"].Value);      
                    }
                    else
                    {
                        dic.Add(node.Attributes["groupname"].Value, new List<string>() { node.Attributes["name"].Value });
                    }
                }
                return dic;
	        }
	        catch (Exception ex)
	        {
		
		        throw ex;
	        }
        }

    }
}
