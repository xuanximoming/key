using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.QCReport
{
   public class TempEntity
    {
        private string _AllCount;  //单据中所有的记录条数

       /// <summary>
        /// 单据中所有的记录条数
       /// </summary>
        public string AllCount
        {
            get { return _AllCount; }
            set { _AllCount = value; }
        }


        string _PageIndex;  //页码

       /// <summary>
        /// 页码
       /// </summary>
        public string PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }

        private string _HosInfo;

       /// <summary>
       /// 医院名称
       /// </summary>
        public string HosInfo
        {
            get { return _HosInfo; }
            set { _HosInfo = value; }
        }

        private string _ReportName;

       /// <summary>
       /// 报表名称
       /// </summary>
        public string ReportName
        {
            get { return _ReportName; }
            set { _ReportName = value; }
        }
    }
}
