using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DrectSoft.DSSqlHelper;
using System.Xml;

namespace Consultation.NEW
{
    /// <summary>
    /// 会诊常用方法类
    /// Add xlb 2013-02-22
    /// </summary>
    public class ConsultCommon
    {
        /// <summary>
        /// 抓取配置项的方法
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConfigKey(string configName)
        {
            try
            {
                //抓取指定配置的值sql
                string sql = @"select value from appcfg where configkey=@configName";
                SqlParameter[] sps = { new SqlParameter("@configName", configName) };
                DataTable dtConfig = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                if (dtConfig == null || dtConfig.Rows.Count <= 0)
                {
                    throw new Exception("" + configName + "系统配置出错");
                }
                string value = dtConfig.Rows[0][0].ToString().Trim();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过病人序号获取配置的病历摘要 Add by wwj 2013-03-07
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public string GetConsultApplyAbstract(string noofinpat)
        {
            try
            {
                DS_SqlHelper.CreateSqlHelper();
                string consultApplyAbstract = string.Empty;
                string configAbstract = GetConfigKey("ConsultationAbstract").Trim();
                if (configAbstract.Length > 0)
                {
                    string emrContent = GetEmrContent("AB", noofinpat);//获取入院志中的病历内容
                    XmlDocument doc = new XmlDocument();
                    doc.PreserveWhitespace = true;
                    doc.LoadXml(emrContent);
                    foreach (string str in configAbstract.Split(','))
                    {
                        var node = (from xmlNode in doc.GetElementsByTagName("roelement").Cast<XmlNode>()
                                    where xmlNode.Attributes["type"].Value.Equals("fixedtext") && xmlNode.Attributes["name"].Value.Equals(str)
                                    select xmlNode).FirstOrDefault();
                        if (node != null && node.ParentNode != null)
                        {
                            //string content = node.ParentNode.InnerText;
                            string content = string.Empty;
                            bool isfind = false;// 是否找到选中的选项
                            while (node.NextSibling != null)
                            {
                                node = node.NextSibling;
                                //add by ywk 2013年7月11日 09:47:59
                                if (node.Name.ToLower() == "selement")//如果标签内容中有单选项
                                {
                                    //content = string.Empty;
                                    var selectedNodes = node.ChildNodes.Cast<XmlNode>().Where(fnode =>
                                    {
                                        if (((XmlElement)fnode).HasAttribute("selected") && fnode.Attributes["selected"].Value.ToLower() == "true")
                                        {
                                            isfind = true;
                                            return isfind;
                                            //break;
                                            //return true;
                                        }
                                        //return false;
                                        isfind = false;
                                        return isfind;
                                        //continue;
                                    });
                                    if (isfind)
                                    {
                                        foreach (var selectedNode in selectedNodes)
                                        {
                                            content += selectedNode.InnerText;
                                        }
                                    }
                                    else
                                    {
                                        content += node.Attributes["text"].Value;
                                    }
                                }
                                else
                                {
                                    content += node.InnerText;
                                }

                            }

                            consultApplyAbstract += str + content + "\r\n";

                        }
                    }
                }
                return consultApplyAbstract;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取指定类型的病历内容  Add by wwj 2013-03-07
        /// </summary>
        /// <param name="emrType"></param>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        private string GetEmrContent(string emrType, string noofinpat)
        {
            try
            {
                DS_SqlHelper.CreateSqlHelper();
                string sqlGetEmrContent = string.Format("select content from recorddetail where noofinpat = '{0}' and sortid = '{1}' and valid = '1'", noofinpat, emrType);
                return DS_SqlHelper.ExecuteScalar(sqlGetEmrContent).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
