using System;
using System.Collections.Generic;
using System.Xml;

namespace DrectSoft.Core.CommonTableConfig
{
    public class DataElementEntity : ICloneable
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
        private List<BaseDictory> _BaseOptionList;  //选项的对象

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

        public List<BaseDictory> BaseOptionList
        {
            get
            {
                _BaseOptionList = StrConvertOption(_ElementRange);
                return _BaseOptionList;
            }
            set
            {
                _BaseOptionList = value;
            }
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }


        /// <summary>
        /// 将字符串转换成数据元选项的对象
        /// </summary>
        /// <param name="dataRangeStr"></param>
        /// <returns></returns>
        public static List<BaseDictory> StrConvertOption(string dataRangeStr)
        {
            List<BaseDictory> baseOptionList = new List<BaseDictory>();
            if (string.IsNullOrEmpty(dataRangeStr)) return baseOptionList;
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(dataRangeStr);
                XmlNodeList xmlNodeList = xmldoc.SelectNodes("/ValueRange/Option");
                if (xmlNodeList == null || xmlNodeList.Count == 0) return baseOptionList;
                foreach (XmlNode item in xmlNodeList)
                {
                    BaseDictory baseOption = new BaseDictory();
                    baseOption.Id = item.Attributes["Id"].Value;
                    baseOption.Name = item.InnerText;
                    baseOptionList.Add(baseOption);
                }
                return baseOptionList;
            }
            catch (Exception ex)
            {
                //return baseOptionList;
                throw ex;
            }

        }

        /// <summary>
        /// 获取选项的默认选项
        /// </summary>
        /// <param name="dataRangeStr"></param>
        /// <returns></returns>
        public static List<BaseDictory> GetDefaultOption(string dataRangeStr)
        {
            List<BaseDictory> baseOptionList = new List<BaseDictory>();
            if (string.IsNullOrEmpty(dataRangeStr)) return baseOptionList;
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(dataRangeStr);
                XmlNodeList xmlNodeList = xmldoc.SelectNodes("/ValueRange/Option");
                if (xmlNodeList == null || xmlNodeList.Count == 0) return baseOptionList;
                foreach (XmlNode item in xmlNodeList)
                {
                    if (item.Attributes["IsDefault"] != null
                        && item.Attributes["IsDefault"].Value.ToUpper() == "TRUE")
                    {
                        BaseDictory baseOption = new BaseDictory();
                        baseOption.Id = item.Attributes["Id"].Value;
                        baseOption.Name = item.InnerText;
                        baseOptionList.Add(baseOption);
                    }
                }
                return baseOptionList;
            }
            catch (Exception ex)
            {
                return baseOptionList;
            }
        }

        /// <summary>
        /// 获取最大值 最小值和默认值 数值型和字符串型
        /// </summary>
        /// <param name="dataRangeStr"></param>
        /// <returns></returns>
        public static Dictionary<String, String> GetMaxMinDefStr(string dataRangeStr)
        {
            try
            {
                if (string.IsNullOrEmpty(dataRangeStr)) return null;
                Dictionary<String, String> dicstr = new Dictionary<string, string>();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(dataRangeStr);
                XmlNode xmlNodeMax = xmldoc.SelectSingleNode("/ValueRange/MaxValue");
                XmlNode xmlNodeMin = xmldoc.SelectSingleNode("/ValueRange/MinValue");
                XmlNode xmlNodeDef = xmldoc.SelectSingleNode("/ValueRange/DefaultValue");
                XmlNode xmlNodeStep = xmldoc.SelectSingleNode("/ValueRange/StepValue");
                if (xmlNodeMax != null)
                {
                    dicstr.Add("MaxValue", xmlNodeMax.InnerText);
                }
                if (xmlNodeMin != null)
                {
                    dicstr.Add("MinValue", xmlNodeMin.InnerText);
                }
                if (xmlNodeDef != null)
                {
                    dicstr.Add("DefaultValue", xmlNodeDef.InnerText);
                }
                if (xmlNodeStep != null)
                {
                    dicstr.Add("StepValue", xmlNodeStep.InnerText);
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
