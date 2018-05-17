using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ZymosisReport.Print
{
    /// <summary>
    /// xll 20130820
    /// </summary>
    public class ZYMOSISHFMDEntity
    {
        string _hfmdid;

        public string Hfmdid
        {
            get { return _hfmdid; }
            set { _hfmdid = value; }
        }
        Decimal _reportid;

        public Decimal Reportid
        {
            get { return _reportid; }
            set { _reportid = value; }
        }
        string _labresult;

        public string Labresult
        {
            get { return _labresult; }
            set { _labresult = value; }
        }
        string _issevere;

        public string Issevere
        {
            get { return _issevere; }
            set { _issevere = value; }
        }
        string _vaild;

        public string Vaild
        {
            get { return _vaild; }
            set { _vaild = value; }
        }
        string _creator;

        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        string _createdate;

        public string Createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        string _mender;

        public string Mender
        {
            get { return _mender; }
            set { _mender = value; }
        }
        string _alterdate;

        public string Alterdate
        {
            get { return _alterdate; }
            set { _alterdate = value; }
        }
    }
}
