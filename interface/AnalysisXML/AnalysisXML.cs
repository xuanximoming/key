using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using System.Xml;
namespace DrectSoft.AnalysisXML
{
    public class AnalysisXML
    {
        private XmlDocument xmlDoc = new XmlDocument();
        public string GetMedicalInsurance(string sortids, string patNum, string NodeNames)
        {
            try
            {
                string _NodeName = NodeNames;
                if (!PingServer())
                {
                    MessageBox.Show("连接配置错误！");
                    return "";
                }
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();

                string[] _sortids = sortids.Split(',');

                string sortid = "";

                for (int i = 0; i < _sortids.Length; i++)
                {
                    if (_sortids[i] != "")
                        sortid += "'" + _sortids[i] + "',";
                }
                sortid = sortid.Substring(0, sortid.Length - 1);
                string sql = "select t.content from recorddetail t,inpatient a where a.patid = '{0}' and t.noofinpat = a.noofinpat and t.valid = 1 and t.sortid in ({1})";
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(string.Format(sql, patNum, sortid), CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    //  创建XML文档，存在就删除再生成
                    XmlDocument RexmlDoc = new XmlDocument();
                    XmlDeclaration dec = RexmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    RexmlDoc.AppendChild(dec);

                    //  创建根结点
                    XmlElement XMLroot = RexmlDoc.CreateElement("root");
                    RexmlDoc.AppendChild(XMLroot);
                    foreach (DataRow row in dt.Rows)
                    {
                        string content = row["content"].ToString();
                        xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(content);
                        XmlNodeList xmlNodes = xmlDoc.GetElementsByTagName("roelement");
                        foreach (XmlNode xmlNode in xmlNodes)
                        {
                            if (_NodeName.Contains(xmlNode.InnerText.Trim()) && xmlNode.InnerText.Trim() != "")
                            {
                                _NodeName = _NodeName.Replace(xmlNode.InnerText.Trim(), "");
                                string name = PublicClass.GetSpellCode(xmlNode.InnerText.Trim());
                                XmlNode newxmlNodex = xmlNode.NextSibling;
                                bool stopnext = true;
                                bool stopflag = true;
                                string nodetext = "";
                                while (stopnext)
                                {
                                    if ((newxmlNodex.Name == "flag" && newxmlNodex.Attributes["direction"].Value == "Left") || newxmlNodex.Name == "roelement" || newxmlNodex.Name == "eof")
                                    {
                                        stopnext = false;
                                        stopflag = true;
                                    }
                                    if (!stopflag)
                                    {
                                        nodetext += newxmlNodex.InnerText;
                                    }
                                    if (newxmlNodex.Name == "flag" && newxmlNodex.Attributes["direction"].Value == "Right")
                                        stopflag = false;
                                    newxmlNodex = newxmlNodex.NextSibling;
                                }
                                //创建数据节点
                                XmlElement NodeName = RexmlDoc.CreateElement(name);
                                NodeName.InnerText = nodetext;
                                XMLroot.AppendChild(NodeName);
                            }
                        }
                    }
                    return RexmlDoc.OuterXml;
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Ping 数据库地址
        /// </summary>
        /// <returns></returns>
        private static bool PingServer()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration("./adcemr.exe");
            ConfigurationSection obj = config.GetSection("dataConfiguration") as ConfigurationSection;
            string connectionStringsSection = obj.ElementInformation.Properties["defaultDatabase"].Value.ToString();
            string ConnectionString = config.ConnectionStrings.ConnectionStrings[connectionStringsSection].ConnectionString;
            string IP = ConnectionString.Substring(ConnectionString.IndexOf("HOST=") + 5, ConnectionString.Substring(ConnectionString.IndexOf("HOST=") + 5).IndexOf(")"));
            return PublicClass.Ping(IP);
        }
    }
}
