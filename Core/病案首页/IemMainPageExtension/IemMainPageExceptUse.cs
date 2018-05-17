using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IemMainPageExtension
{
    /// <summary>
    /// 维护对象使用对象
    /// Add by xlb 
    /// </summary>
    public class IemMainPageExceptUse
    {
        /*使用病案扩展元项流水号*/
        private string iemexuseId;
                                                                                  
        /*病案对象元流水号*/
        private string iemexId;

        /*病人病案号*/
        private string noofinpat;

        /*元素值*/                         
        private string _value;

        /*创建医师编号*/
        private string createDocId;

        /*创建时间*/
        private string createDatetime;

        /*修改人编号*/
        private string modifyDocId;

        /*修改时间*/
        private string modifyDatetime;

        /// <summary>
        /// 使用项流水号
        /// </summary>
        public string IemexUseId
        {
            get { return iemexuseId; }
            set { iemexuseId = value;}
        }

        /// <summary>
        /// 对应iemmainpageexcept项流水号
        /// </summary>
        public string IemexId
        {
            get { return iemexId; }
            set { iemexId = value;}
        }

        /// <summary>
        /// 病人病案号
        /// </summary>
        public string Noofinpat
        {
            get { return noofinpat; }
            set { noofinpat = value;}
        }

        /// <summary>
        /// 维护对象属性值
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// 创建人编号
        /// </summary>
        public string CreateDocId
        {
            get { return createDocId; }
            set { createDocId = value;}
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDatetime
        {
            get { return createDatetime; }
            set { createDatetime = value;}
        }

        /// <summary>
        /// 修改人编号
        /// </summary>
        public string ModifyDocId 
        {
            get { return modifyDocId; }
            set { modifyDocId = value;}
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifyDatetime 
        {
            get { return modifyDatetime;}
            set { modifyDatetime = value;}
        }
    }
}
