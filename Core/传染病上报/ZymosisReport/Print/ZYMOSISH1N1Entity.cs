using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ZymosisReport.Print
{

    /// <summary>
    /// H1N1流感对象
    /// /// <summary>
    /// xll 20130820
    /// </summary>
    /// </summary>
    public class ZYMOSISH1N1Entity
    {
        string _H1N1ID;

        public string H1N1ID
        {
            get { return _H1N1ID; }
            set { _H1N1ID = value; }
        }
        Decimal _REPORTID;

        public Decimal REPORTID
        {
            get { return _REPORTID; }
            set { _REPORTID = value; }
        }
        string _CASETYPE;

        public string CASETYPE
        {
            get { return _CASETYPE; }
            set { _CASETYPE = value; }
        }
        string _HOSPITALSTATUS;

        public string HOSPITALSTATUS
        {
            get { return _HOSPITALSTATUS; }
            set { _HOSPITALSTATUS = value; }
        }
        string _ISCURE;

        public string ISCURE
        {
            get { return _ISCURE; }
            set { _ISCURE = value; }
        }
        string _ISOVERSEAS;

        public string ISOVERSEAS
        {
            get { return _ISOVERSEAS; }
            set { _ISOVERSEAS = value; }
        }
        string _VAILD;

        public string VAILD
        {
            get { return _VAILD; }
            set { _VAILD = value; }
        }
        string _CREATOR;

        public string CREATOR
        {
            get { return _CREATOR; }
            set { _CREATOR = value; }
        }
        string _CREATEDATE;

        public string CREATEDATE
        {
            get { return _CREATEDATE; }
            set { _CREATEDATE = value; }
        }
        string _MENDER;

        public string MENDER
        {
            get { return _MENDER; }
            set { _MENDER = value; }
        }
        string _ALTERDATE;

        public string ALTERDATE
        {
            get { return _ALTERDATE; }
            set { _ALTERDATE = value; }
        }

        string _InHosDate;

        /// <summary>
        /// 入院日期
        /// </summary>
        public string InHosDate
        {
            get { return _InHosDate; }
            set { _InHosDate = value; }
        }

        string _OutHosDate;

        /// <summary>
        /// 出院日期
        /// </summary>
        public string OutHosDate
        {
            get { return _OutHosDate; }
            set { _OutHosDate = value; }
        }


        string _ZhiyuDate;

        /// <summary>
        /// 治愈时间
        /// </summary>
        public string ZhiyuDate
        {
            get { return _ZhiyuDate; }
            set { _ZhiyuDate = value; }
        }
    }
}
