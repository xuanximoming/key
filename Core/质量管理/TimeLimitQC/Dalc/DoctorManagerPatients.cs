using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 医生分管病人类
    /// </summary>
    public class DoctorManagerPatient
    {
        int _noOfInpat;
        string _inpatientName;
        string _patID;
        string _attend;
        string _resident;
        string _chief;

        /// <summary>
        /// 病人首页序号
        /// </summary>
        public int NoOfInpat
        {
            get { return _noOfInpat; }
        }

        /// <summary>
        /// 病人姓名
        /// </summary>
        public string InpatientName
        {
            get { return _inpatientName; }
        }

        /// <summary>
        /// 住院号码
        /// </summary>
        public string PatID
        {
            get { return _patID; }
        }

        /// <summary>
        /// 住院医生代码
        /// </summary>
        public string Attend
        {
            get { return _attend; }
        }

        /// <summary>
        /// 主治医生代码
        /// </summary>
        public string Resident
        {
            get { return _resident; }
        }

        /// <summary>
        /// 主任医生代码
        /// </summary>
        public string Chief
        {
            get { return _chief; }
        }

        /// <summary>
        /// 构造一个医生分管病人
        /// </summary>
        /// <param name="NoOfInpat">病人首页序号</param>
        /// <param name="InpatientName">病人姓名</param>
        /// <param name="PatID">住院号码</param>
        /// <param name="Attend ">住院医生代码</param>
        /// <param name="Resident">主治医生代码</param>
        /// <param name="Chief ">主任医生代码</param>
        public DoctorManagerPatient(int noOfInpat, string inpatientName, string patID,
            string attend, string resident, string chief)
        {
            _noOfInpat = noOfInpat;
            _inpatientName = inpatientName;
            _patID = patID;
            _attend = attend;
            _resident = resident;
            _chief = chief;
        }
    }
}
