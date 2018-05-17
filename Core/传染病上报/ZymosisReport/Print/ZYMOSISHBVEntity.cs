using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ZymosisReport.Print
{
    /// <summary>
    /// 乙肝报告卡对象
    /// /// <summary>
    /// xll 20130820
    /// </summary>
    /// </summary>
    public class ZYMOSISHBVEntity
    {
        string _HBVID;

        public string HBVID
        {
            get { return _HBVID; }
            set { _HBVID = value; }
        }
        decimal _REPORTID;

        public decimal REPORTID
        {
            get { return _REPORTID; }
            set { _REPORTID = value; }
        }
        string _HBSAGDATE;

        public string HBSAGDATE
        {
            get { return _HBSAGDATE; }
            set { _HBSAGDATE = value; }
        }
        string _FRISTDATE;

        public string FRISTDATE
        {
            get { return _FRISTDATE; }
            set { _FRISTDATE = value; }
        }
        string _ALT;

        public string ALT
        {
            get { return _ALT; }
            set { _ALT = value; }
        }
        string _ANTIHBC;

        public string ANTIHBC
        {
            get { return _ANTIHBC; }
            set { _ANTIHBC = value; }
        }
        string _LIVERBIOPSY;

        public string LIVERBIOPSY
        {
            get { return _LIVERBIOPSY; }
            set { _LIVERBIOPSY = value; }
        }
        string _RECOVERYHBSAG;

        public string RECOVERYHBSAG
        {
            get { return _RECOVERYHBSAG; }
            set { _RECOVERYHBSAG = value; }
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
    }
}
