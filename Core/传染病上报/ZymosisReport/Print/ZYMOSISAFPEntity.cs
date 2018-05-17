using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ZymosisReport.Print
{
    /// <summary>
    /// xll 20130820
    /// </summary>
    public class ZYMOSISAFPEntity
    {
        string _afpid;

        public string Afpid
        {
            get { return _afpid; }
            set { _afpid = value; }
        }
        decimal _reportid;

        public decimal Reportid
        {
            get { return _reportid; }
            set { _reportid = value; }
        }
        string _householdscope;

        public string Householdscope
        {
            get { return _householdscope; }
            set { _householdscope = value; }
        }
        string _householdaddress;

        public string Householdaddress
        {
            get { return _householdaddress; }
            set { _householdaddress = value; }
        }
        string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        string _palsydate;

        public string Palsydate
        {
            get { return _palsydate; }
            set { _palsydate = value; }
        }
        string _palsysymptom;

        public string Palsysymptom
        {
            get { return _palsysymptom; }
            set { _palsysymptom = value; }
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
        string _diagnosisdate;

        public string Diagnosisdate
        {
            get { return _diagnosisdate; }
            set { _diagnosisdate = value; }
        }
    }
}
