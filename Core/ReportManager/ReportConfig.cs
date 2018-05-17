using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ReportManager
{
    /// <summary>
    /// <date>2013-07-09</date>
    /// 报告卡对应实体
    /// <auth>XLB</auth>
    /// </summary>
    public class ReportConfig
    {
        /// <summary>
        /// 患者名称
        /// </summary>
        public string _name;
        /// <summary>
        /// 患者名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public List<ReportCategory>_ReportCategory;
        public List<ReportCategory> ReportCategory
        {
            get
            {
                return _ReportCategory;
            }
            set
            {
                _ReportCategory = value;
            }
        }


    }


    public class ReportCategory
    {
        private string _NAME;
    
        public string NAME
        {
            get
            {
                return _NAME;
            }
            set
            {
                _NAME = value;
            }
        }

        private List<ReportCategoryInfo> _ReportCategoryInfo;
        public List<ReportCategoryInfo> ReportCategoryInfo
        {
            get
            {
                return _ReportCategoryInfo;
            }
            set
            {
                _ReportCategoryInfo = value;
            }
        }      

    }

    public class ReportCategoryInfo
    {
        private string _NAME;
        private string _VALID;
        private string _ORDERID;
        private string _TABLENAME;
        private string _PRINTMODEL;
        private string _ID;
        private string _State;
        private string _noofinpat;
        public string Noofinpat
        {
            get
            {
                return _noofinpat;
            }
            set
            {
                _noofinpat = value;
            }
        }
        public string State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
            }
        }
        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public string NAME
        {
            get
            {
                return _NAME;
            }
            set
            {
                _NAME = value;
            }
        }

        public string VALID
        {
            get
            {
                return _VALID;
            }
            set
            {
                _VALID = value;
            }
        }

        public string ORDERID
        {
            get
            {
                return _ORDERID;
            }
            set
            {
                _ORDERID = value;
            }
        }

        public string TABLENAME
        {
            get
            {
                return _TABLENAME;
            }
            set
            {
                _TABLENAME = value;
            }
        }

        public string PRINTMODEL
        {
            get
            {
                return _PRINTMODEL;
            }
            set
            {
                _PRINTMODEL = value;
            }
        }
    
    }
}
