using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;

namespace IemMainPageExtension
{
    /// <summary>
    /// 数据元实体类
    /// Add by xlb 2013-04-15
    /// </summary>
    public class DateElementEntity
    {
        private string _ElementFlow;  //数据元流水号
        private string _ElementId;    //数据元ID
        private string _ElementName;   //数据元名称
        private string _ElementType;   //数据元数据类型
        private string _ElementForm;   //数据元格式
        private string _ElementRange;   //数据元的取值范围 xml
        private string _ElementDescribe;   //数据元描述
        private string _ElementClass;   //数据元所属大类
        private string _IsDataElemet;   //是否是卫生部数据元
        private string _ElementPYM;   //数据元拼音码
        private string _Valid;   //是否启用

        /// <summary>
        /// 数据元流水号
        /// </summary>
        public virtual string ElementFlow
        {
            get
            {
                return _ElementFlow;
            }
            set
            {
                _ElementFlow = value;
            }
        }
        /// <summary>
        /// 数据元ID
        /// </summary>
        public virtual string ElementId
        {
            get
            {
                return _ElementId;
            }
            set
            {
                _ElementId = value;
            }
        }
        /// <summary>
        /// 数据元名称
        /// </summary>
        public virtual string ElementName
        {
            get
            {
                return _ElementName;
            }
            set
            {
                _ElementName = value;
            }
        }
        /// <summary>
        /// 数据元数据类型
        /// </summary>
        public virtual string ElementType
        {
            get
            {
                return _ElementType;
            }
            set
            {
                _ElementType = value;
            }
        }
        /// <summary>
        /// 数据元格式
        /// </summary>
        public virtual string ElementForm
        {
            get
            {
                return _ElementForm;
            }
            set
            {
                _ElementForm = value;
            }
        }
        /// <summary>
        ///数据元的取值范围
        /// </summary>
        public virtual string ElementRange
        {
            get
            {
                return _ElementRange;
            }
            set
            {
                _ElementRange = value;
            }
        }
        /// <summary>
        /// 数据元描述
        /// </summary>
        public virtual string ElementDescribe
        {
            get
            {
                return _ElementDescribe;
            }
            set
            {
                _ElementDescribe = value;
            }
        }
        /// <summary>
        /// 数据元所属大类
        /// </summary>
        public virtual string ElementClass
        {
            get
            {
                return _ElementClass;
            }
            set
            {
                _ElementClass = value;
            }
        }
        /// <summary>
        /// 是否是卫生部数据元
        /// </summary>
        public virtual string IsDataElemet
        {
            get
            {
                return _IsDataElemet;
            }
            set
            {
                _IsDataElemet = value;
            }
        }
        /// <summary>
        ///数据元拼音码
        /// </summary>
        public virtual string ElementPYM
        {
            get
            {
                return _ElementPYM;
            }
            set
            {
                _ElementPYM = value;
            }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual string Valid
        {
            get
            {
                return _Valid;
            }
            set
            {
                _Valid = value;
            }
        }

        /// <summary>
        /// 根据数据元取值范围返回数据集
        /// 最大最小值默认值等
        /// Add by xlb 2013-04-15
        /// </summary>
        /// <param name="dataElement">数据元对象</param>
        /// <returns>数据字典集合</returns>
        public static Dictionary<string, string> GetDataSource(DateElementEntity dataElement)
        {
            try
            {
                if (dataElement == null || string.IsNullOrEmpty(dataElement.ElementRange))
                {
                    return null;
                }
                Dictionary<string, string> dictData = new Dictionary<string, string>();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(dataElement.ElementRange);

                XmlNode xmlNodeMax = xmlDoc.SelectSingleNode("/ValueRange/MaxValue");
                XmlNode xmlNodeMin = xmlDoc.SelectSingleNode("/ValueRange/MinValue");
                XmlNode xmlNodeDef = xmlDoc.SelectSingleNode("/ValueRange/DefaultValue");
                XmlNode xmlNodeStep = xmlDoc.SelectSingleNode("/ValueRange/StepValue");
                if (xmlNodeMax != null)
                {
                    dictData.Add("MaxValue", xmlNodeMax.InnerText);
                }
                if (xmlNodeMin != null)
                {
                    dictData.Add("MinValue", xmlNodeMin.InnerText);
                }
                if (xmlNodeDef != null)
                {
                    dictData.Add("DefaultValue", xmlNodeDef.InnerText);
                }
                if (xmlNodeStep != null)
                {
                    dictData.Add("StepValue", xmlNodeStep.InnerText);
                }
                return dictData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取数据元数据集合
        /// </summary>
        /// <param name="dateEntiy"></param>
        public static DataTable GetDataSorce(DateElementEntity dateEntiy)
        {
            try
            {
                DataColumn dataColoumnId = new DataColumn("ID", typeof(string));
                DataColumn dataColumnName = new DataColumn("NAME", typeof(string));
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add(dataColoumnId);
                dataTable.Columns.Add(dataColumnName);

                if (string.IsNullOrEmpty(dateEntiy.ElementRange))
                {
                    return dataTable;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(dateEntiy.ElementRange);
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("/ValueRange/Option");
                if (xmlNodeList == null || xmlNodeList.Count <= 0)
                {
                    return dataTable;
                }

                foreach (XmlNode item in xmlNodeList)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["ID"] = item.Attributes["Id"].Value;
                    dataRow["NAME"] = item.InnerText;
                    dataTable.Rows.Add(dataRow);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取单选多选默认选项
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetDefaultValue(DateElementEntity dateEntiy)
        {
            try
            {
                if (dateEntiy == null || string.IsNullOrEmpty(dateEntiy._ElementRange))
                {
                    return null;
                }
                /*字典集合记录默认选项的ID和内容*/
                Dictionary<string, string> dicstr = new Dictionary<string, string>();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(dateEntiy._ElementRange);
                XmlNodeList nodeList = xmlDoc.SelectNodes("/ValueRange/Option");
                if (nodeList == null || nodeList.Count <= 0)
                {
                    return dicstr;
                }
                foreach (XmlNode xmlNode in nodeList)
                {
                    if (xmlNode.Attributes["IsDefault"] != null 
                        && xmlNode.Attributes["IsDefault"].Value.ToUpper().Trim() == "TRUE")
                    {
                        /*避免维护单选数据源时添加了多项默认选项的情况*/
                        if (!dicstr.ContainsKey("IsDefault"))
                        {
                            dicstr.Add("IsDefault", xmlNode.Attributes["Id"].Value);
                        }
                    }
                }
                return dicstr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
