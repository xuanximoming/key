using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IemMainPageExtension
{
    /// <summary>
    /// 病案首页扩展信息维护实体
    /// 对应Iem_mainPage_Except表
    /// Add by xlb 2013-04-09
    /// </summary>
    public class IemMainPageExcept
    {
        private string iemExId;

        private string iemExName;

        private string dateElementFlow;

        private string elementName;

        private string elementType;

        private string iemControl;

        private string orderCode;

        private string isOtherLine;

        private string valide;

        private string iemOtherName;

        private string createDocId;

        private string createDateTime;

        private string modifyDocId;

        private string modifyDateTime;

        private string elementTypeDescribe;



        private DateElementEntity dateElement;

        # region Property

        /// <summary>
        /// 流水号
        /// </summary>
        public string IemExId
        {
            get
            {
                if (string.IsNullOrEmpty(iemExId))
                {
                    return "";
                }
                else
                {
                    return iemExId;
                };
            }
            set
            {
                iemExId = value;
            }
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string IemExName
        {
            get
            {
                if (string.IsNullOrEmpty(iemExName))
                {
                    return "";
                }
                else
                {
                    return iemExName;
                };
            }
            set
            {
                iemExName = value;
            }
        }

        /// <summary>
        /// 数据元流水号
        /// </summary>
        public string DateElementFlow
        {
            get
            {

                if (string.IsNullOrEmpty(dateElementFlow))
                {
                    return "";
                }
                else
                {
                    return dateElementFlow;
                };
            }
            set
            {
                dateElementFlow = value;
            }
        }

        /// <summary>
        /// 控件名
        /// </summary>
        public string IemControl
        {
            get
            {

                if (string.IsNullOrEmpty(iemControl))
                {
                    return "";
                }
                else
                {
                    return iemControl;
                };
            }
            set
            {
                iemControl = value;
            }
        }

        public string IemOtherName
        {
            get
            {

                if (string.IsNullOrEmpty(iemOtherName))
                {
                    return "";
                }
                else
                {
                    return iemOtherName;
                };
            }
            set
            {
                iemOtherName = value;
            }
        }

        /// <summary>
        /// 排序码
        /// </summary>
        public string OrderCode
        {
            get
            {

                if (string.IsNullOrEmpty(orderCode))
                {
                    return "";
                }
                else
                {
                    return orderCode;
                };
            }
            set
            {
                orderCode = value;
            }
        }

        /// <summary>
        /// 是否换行
        /// </summary>
        public string IsOtherLine
        {
            get
            {

                if (string.IsNullOrEmpty(isOtherLine))
                {
                    return "否";
                }
                else
                {
                    return isOtherLine;
                };
            }
            set
            {
                if (value.ToString().Trim().Equals("1"))
                {
                    isOtherLine = "是";
                }
                else if (value.ToString().Trim().Equals("0"))
                {
                    isOtherLine = "否";
                }
                else
                {
                    isOtherLine = value;
                }
            }
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public string Vailde
        {

            get
            {

                if (string.IsNullOrEmpty(valide))
                {
                    return "1";
                }
                else
                {
                    return valide;
                };
            }
            set
            {
                valide = value;
            }
        }

        /// <summary>
        /// 创建人编号
        /// </summary>
        public string CreateDocId
        {
            get
            {

                if (string.IsNullOrEmpty(createDocId))
                {
                    return "";
                }
                else
                {
                    return createDocId;
                };
            }
            set
            {
                createDocId = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDateTime
        {
            get
            {

                if (string.IsNullOrEmpty(createDateTime))
                {
                    return DateTime.Now.ToString();
                }
                else
                {
                    return createDateTime;
                };
            }
            set
            {
                createDateTime = value;
            }
        }

        /// <summary>
        /// 修改人编号
        /// 记录相关信息方便追溯
        /// </summary>
        public string ModifyDocId
        {
            get
            {

                if (string.IsNullOrEmpty(modifyDocId))
                {
                    return "";
                }
                else
                {
                    return modifyDocId;
                };
            }
            set
            {
                modifyDocId = value;
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifyDateTime
        {
            get
            {

                if (string.IsNullOrEmpty(modifyDateTime))
                {
                    return DateTime.Now.ToString();
                }
                else
                {
                    return modifyDateTime;
                };
            }
            set
            {
                modifyDateTime = value;
            }
        }

        /// <summary>
        /// 数据元类型
        /// </summary>
        public string ElementType
        {
            get
            {

                if (string.IsNullOrEmpty(elementType))
                {
                    return "";
                }
                else
                {
                    return elementType;
                };
            }
            set
            {
                elementType = value;
            }
        }

        /// <summary>
        /// 数据元名称
        /// </summary>
        public string ElementName
        {
            get
            {

                if (string.IsNullOrEmpty(elementName))
                {
                    return "";
                }
                else
                {
                    return elementName;
                };
            }
            set
            {
                elementName = value;
            }
        }

        /// <summary>
        /// 数据元实体
        /// </summary>
        public DateElementEntity DateElement
        {
            get
            {
                return dateElement;
            }
            set
            {
                dateElement = value;
            }
        }

        /// <summary>
        /// 数据元类型描述
        /// </summary>
        public string ElementTypeDescribe
        {
            get
            {
                switch (ElementType.ToString().Trim().ToLower())
                {
                    case "s1":
                        elementTypeDescribe = "字符串";
                        break;
                    case "s2":
                        elementTypeDescribe = "单选项目";
                        break;
                    case "s3":
                        elementTypeDescribe = "单选项目";
                        break;
                    case "s4":
                        elementTypeDescribe = "大文本格式";
                        break;
                    case "s9":
                        elementTypeDescribe = "多选项目";
                        break;
                    case "l":
                        elementTypeDescribe = "布尔逻辑";
                        break;
                    case "n":
                        elementTypeDescribe = "数值型";
                        break;
                    case "d":
                        elementTypeDescribe = "日期型";
                        break;
                    case "dt":
                        elementTypeDescribe = "日期时间型";
                        break;
                    case "t":
                        elementTypeDescribe = "时间型";
                        break;
                    case "by":
                        elementTypeDescribe = "二进制";
                        break;
                    default:
                        elementTypeDescribe = "";
                        break;
                }
                return elementTypeDescribe;
                ;
            }
        }

        #endregion
    }
}
