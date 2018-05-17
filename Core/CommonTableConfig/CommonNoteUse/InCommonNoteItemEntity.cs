using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public class InCommonNoteItemEntity
    {
        private string _InCommonNote_Item_Flow;
        private string _InCommonNote_Tab_Flow;
        private string _InCommonNoteFlow;
        private string _CommonNote_Item_Flow;
        private string _CommonNote_Tab_Flow;
        private string _CommonNoteFlow;
        private string _DataElementFlow;
        private string _DataElementId;
        private string _DataElementName;
        private string _OtherName;
        private string _OrderCode;
        private string _IsValidate;
        private string _CreateDoctorID;
        private string _CreateDoctorName;
        private string _CreateDateTime;
        private string _Valide;
        private string _ValueXml;
        private string _RecordDate;
        private string _RecordTime;
        private string _RecordDoctorId;
        private string _RecordDoctorName;
        private string _GroupFlow;
        private List<BaseDictory> _BaseValueList;
        private string _ValuesShow;
        private DataElementEntity _DataElement;
        private Object _DataTableValue;
        private Decimal _Xgnum;

        //修改次数
        public Decimal Xgnum
        {
            get { return _Xgnum; }
            set { _Xgnum = value; }
        }


        /// <summary>
        /// datatable 绑定的值
        /// </summary>
        public Object DataTableValue
        {
            get { return _DataTableValue; }
            set { _DataTableValue = value; }
        }

        
        /// <summary>
        /// 对应数据元
        /// </summary>
        public DataElementEntity DataElement
        {
            get
            {
                return _DataElement;
            }
            set { _DataElement = value; }
        }


        /// <summary>
        /// 用于显示的值
        /// </summary>
        public string ValuesShow
        {
            get
            {
                _ValuesShow = "";
                for (int i = 0; i < BaseValueList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(BaseValueList[i].Id))
                    {
                        _ValuesShow += BaseValueList[i].Name + ";";
                    }
                    else
                    {
                        _ValuesShow += BaseValueList[i].Name;
                    }
                }
                //if (_ValuesShow.Length > 0 && BaseValueList.Count == 1)
                //{
                //    _ValuesShow = _ValuesShow.Substring(0, _ValuesShow.Length);
                //}
                //if (_ValuesShow.Contains(";") || _ValuesShow.Contains("；"))
                //{
                //    _ValuesShow.Substring(0, _ValuesShow.Length - 1);
                //}
                return _ValuesShow;
            }
            set { _ValuesShow = value; }
        }

        /// <summary>
        /// 分组流水号
        /// </summary>
        public string GroupFlow
        {
            get { return _GroupFlow; }
            set { _GroupFlow = value; }
        }





        /// <summary>
        /// 值的对象形式
        /// </summary>
        public List<BaseDictory> BaseValueList
        {
            get
            {
                _BaseValueList = ConvertStrToBaseList(_ValueXml);
                return _BaseValueList;
            }
            set { _BaseValueList = value; }
        }


        /// <summary>
        /// 病人通用单项目流水号
        /// </summary>
        public virtual string InCommonNote_Item_Flow
        {
            get
            {
                return _InCommonNote_Item_Flow;
            }
            set
            {
                _InCommonNote_Item_Flow = value;
            }
        }
        /// <summary>
        /// 病人通用单标签流水号
        /// </summary>
        public virtual string InCommonNote_Tab_Flow
        {
            get
            {
                return _InCommonNote_Tab_Flow;
            }
            set
            {
                _InCommonNote_Tab_Flow = value;
            }
        }
        /// <summary>
        /// 病人通用单流水号
        /// </summary>
        public virtual string InCommonNoteFlow
        {
            get
            {
                return _InCommonNoteFlow;
            }
            set
            {
                _InCommonNoteFlow = value;
            }
        }
        /// <summary>
        /// 通用单据项目配置流水号
        /// </summary>
        public virtual string CommonNote_Item_Flow
        {
            get
            {
                return _CommonNote_Item_Flow;
            }
            set
            {
                _CommonNote_Item_Flow = value;
            }
        }
        /// <summary>
        /// 通用单据标签配置流水号
        /// </summary>
        public virtual string CommonNote_Tab_Flow
        {
            get
            {
                return _CommonNote_Tab_Flow;
            }
            set
            {
                _CommonNote_Tab_Flow = value;
            }
        }
        /// <summary>
        /// 通用单据配置流水号
        /// </summary>
        public virtual string CommonNoteFlow
        {
            get
            {
                return _CommonNoteFlow;
            }
            set
            {
                _CommonNoteFlow = value;
            }
        }
        /// <summary>
        /// 数据元流水号
        /// </summary>
        public virtual string DataElementFlow
        {
            get
            {
                return _DataElementFlow;
            }
            set
            {
                _DataElementFlow = value;
            }
        }
        /// <summary>
        ///数据元ID
        /// </summary>
        public virtual string DataElementId
        {
            get
            {
                return _DataElementId;
            }
            set
            {
                _DataElementId = value;
            }
        }
        /// <summary>
        /// 数据元名称
        /// </summary>
        public virtual string DataElementName
        {
            get
            {
                return _DataElementName;
            }
            set
            {
                _DataElementName = value;
            }
        }
        /// <summary>
        /// 别名 系统中显示时都用这个名称
        /// </summary>
        public virtual string OtherName
        {
            get
            {
                return _OtherName;
            }
            set
            {
                _OtherName = value;
            }
        }
        /// <summary>
        /// 排序码
        /// </summary>
        public virtual string OrderCode
        {
            get
            {
                return _OrderCode;
            }
            set
            {
                _OrderCode = value;
            }
        }
        /// <summary>
        /// 是否机型数据元验证标记 是 否
        /// </summary>
        public virtual string IsValidate
        {
            get
            {
                return _IsValidate;
            }
            set
            {
                _IsValidate = value;
            }
        }
        /// <summary>
        /// 创建者ID
        /// </summary>
        public virtual string CreateDoctorID
        {
            get
            {
                return _CreateDoctorID;
            }
            set
            {
                _CreateDoctorID = value;
            }
        }
        /// <summary>
        /// 创建者名称
        /// </summary>
        public virtual string CreateDoctorName
        {
            get
            {
                return _CreateDoctorName;
            }
            set
            {
                _CreateDoctorName = value;
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual string CreateDateTime
        {
            get
            {
                return _CreateDateTime;
            }
            set
            {
                _CreateDateTime = value;
            }
        }
        /// <summary>
        /// 删除标记
        /// </summary>
        public virtual string Valide
        {
            get
            {
                return _Valide;
            }
            set
            {
                _Valide = value;
            }
        }
        /// <summary>
        /// 值的xml
        /// <Value>
        ///<Item id=’’>aa</Item>
        ///<Item id=’’>aa</Item>
        ///</Value>
        /// </summary>
        public virtual string ValueXml
        {
            get
            {
                return _ValueXml;
            }
            set
            {
                _ValueXml = value;
            }
        }
        /// <summary>
        /// 记录日期10位  yyyy-MM-dd
        /// /// </summary>
        public virtual string RecordDate
        {
            get
            {
                return _RecordDate;
            }
            set
            {
                _RecordDate = value;
            }
        }
        /// <summary>
        /// 记录时间8位 HH:mm:ss 
        /// </summary>
        public virtual string RecordTime
        {
            get
            {
                return _RecordTime;
            }
            set
            {
                _RecordTime = value;
            }
        }
        /// <summary>
        /// 记录者ID
        /// </summary>
        public virtual string RecordDoctorId
        {
            get
            {
                return _RecordDoctorId;
            }
            set
            {
                _RecordDoctorId = value;
            }
        }
        /// <summary>
        /// 记录者名称
        /// </summary>
        public virtual string RecordDoctorName
        {
            get
            {
                return _RecordDoctorName;
            }
            set
            {
                _RecordDoctorName = value;
            }
        }


        private List<BaseDictory> ConvertStrToBaseList(string valuexml)
        {
            List<BaseDictory> baseValueList = new List<BaseDictory>();
            if (string.IsNullOrEmpty(valuexml)) return baseValueList;
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(valuexml);
                XmlNodeList xmlNodeList = xmldoc.SelectNodes("/Value/Item");
                if (xmlNodeList == null || xmlNodeList.Count == 0) return baseValueList;
                foreach (XmlNode item in xmlNodeList)
                {
                    BaseDictory baseOption = new BaseDictory();
                    baseOption.Id = item.Attributes["Id"].Value;
                    baseOption.Name = item.InnerText;
                    baseValueList.Add(baseOption);
                }
                return baseValueList;
            }
            catch (Exception ex)
            {
                return baseValueList;
            }
        }

        /// <summary>
        /// 将单字符转化成valuecml
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertStrToXml(string value)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<Value></Value>");
            XmlElement xmlElement = xmldoc.CreateElement("Item");
            XmlAttribute xmlAttribute = xmldoc.CreateAttribute("Id");
            xmlAttribute.Value = "";
            xmlElement.Attributes.Append(xmlAttribute);
            xmlElement.InnerText = value;
            xmldoc.DocumentElement.AppendChild(xmlElement);
            return xmldoc.OuterXml;
        }

        /// <summary>
        /// 将List化成valuecml
        /// </summary>
        /// <param name="basevalueList"></param>
        /// <returns></returns>
        public static string ConvertBaseToXml(List<BaseDictory> basevalueList)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<Value></Value>");
            if (basevalueList == null)
                return "";
            foreach (var item in basevalueList)
            {
                XmlElement xmlElement = xmldoc.CreateElement("Item");
                XmlAttribute xmlAttribute = xmldoc.CreateAttribute("Id");
                xmlAttribute.Value = item.Id;
                xmlElement.Attributes.Append(xmlAttribute);
                xmlElement.InnerText = item.Name;
                xmldoc.DocumentElement.AppendChild(xmlElement);
            }
            return xmldoc.OuterXml;
        }

        /// <summary>
        /// 将List化成valuecml
        /// </summary>
        /// <param name="basevalueList"></param>
        /// <returns></returns>
        public static string ConvertBaseToXml(BaseDictory basevalue)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<Value></Value>");
            XmlElement xmlElement = xmldoc.CreateElement("Item");
            XmlAttribute xmlAttribute = xmldoc.CreateAttribute("Id");
            xmlAttribute.Value = basevalue.Id;
            xmlElement.Attributes.Append(xmlAttribute);
            xmlElement.InnerText = basevalue.Name;
            xmldoc.DocumentElement.AppendChild(xmlElement as XmlNode);
            return xmldoc.OuterXml;
        }


    }
}
