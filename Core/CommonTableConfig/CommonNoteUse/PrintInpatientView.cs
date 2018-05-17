
namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public class PrintInpatientView
    {
        string _HospitalName;
        string _SubHospitalName;
        string _InpatientName;
        string _InpatientAge;
        string _PatId;
        string _Depart;
        string _Ward;
        string _InNo;
        string _InDateTime;
        string _Sex;
        string _InBedNo;
        string _AdmitDiagnosis;
        string _RecordName;
        string _ListCount;
        string _CurrPage;


        /// <summary>
        /// 当前页
        /// </summary>
        public string CurrPage
        {
            get { return _CurrPage; }
            set { _CurrPage = value; }
        }

        /// <summary>
        /// 一张纸中集合的条数
        /// </summary>
        public string ListCount
        {
            get { return _ListCount; }
            set { _ListCount = value; }
        }

        /// <summary>
        /// 记录单据名
        /// </summary>
        public string RecordName
        {
            get { return _RecordName; }
            set { _RecordName = value; }
        }

        /// <summary>
        /// 医院名称
        /// </summary>
        public virtual string HospitalName
        {
            get
            {
                return _HospitalName;
            }
            set
            {
                _HospitalName = value;
            }
        }
        /// <summary>
        /// 医院子名称
        /// </summary>
        public virtual string SubHospitalName
        {
            get
            {
                return _SubHospitalName;
            }
            set
            {
                _SubHospitalName = value;
            }
        }
        /// <summary>
        /// 病人名称
        /// </summary>
        public virtual string InpatientName
        {
            get
            {
                return _InpatientName;
            }
            set
            {
                _InpatientName = value;
            }
        }
        /// <summary>
        /// 年龄
        /// </summary>
        public virtual string InpatientAge
        {
            get
            {
                return _InpatientAge;
            }
            set
            {
                _InpatientAge = value;
            }
        }
        /// <summary>
        ///科室
        /// </summary>
        public virtual string Depart
        {
            get
            {
                return _Depart;
            }
            set
            {
                _Depart = value;
            }
        }
        /// <summary>
        /// 病区
        /// </summary>
        public virtual string Ward
        {
            get
            {
                return _Ward;
            }
            set
            {
                _Ward = value;
            }
        }
        /// <summary>
        /// 住院号
        /// </summary>
        public virtual string InNo
        {
            get
            {
                return _InNo;
            }
            set
            {
                _InNo = value;
            }
        }

        /// <summary>
        /// 病人号
        /// </summary>
        public virtual string PatId
        {
            get
            {
                return _PatId;
            }
            set
            {
                _PatId = value;
            }
        }
        /// <summary>
        ///入院日期
        /// </summary>
        public virtual string InDateTime
        {
            get
            {
                return _InDateTime;
            }
            set
            {
                _InDateTime = value;
            }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public virtual string Sex
        {
            get
            {
                return _Sex;
            }
            set
            {
                _Sex = value;
            }
        }
        /// <summary>
        /// 床号
        /// </summary>
        public virtual string InBedNo
        {
            get
            {
                return _InBedNo;
            }
            set
            {
                _InBedNo = value;
            }
        }
        /// <summary>
        /// 入院诊断
        /// </summary>
        public virtual string AdmitDiagnosis
        {
            get
            {
                return _AdmitDiagnosis;
            }
            set
            {
                _AdmitDiagnosis = value;
            }
        }

    }
}
