using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.Consultation
{
    public class ConsultationEntity
    {
        public ConsultationEntity()
        {
        }

        /// <summary>
        /// 会诊申请单流水号
        /// </summary>
        public string ConsultApplySn { get; set; }//ssyy

        public string NoOfInpat { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string SexName { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// 所在科室
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 所在科室
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 病区
        /// </summary>
        public string WardID { get; set; }

        /// <summary>
        /// 病区
        /// </summary>
        public string WardName { get; set; }

        /// <summary>
        /// 床号
        /// </summary>
        public string Bed { get; set; }

        /// <summary>
        /// 住院病历号
        /// </summary>
        public string PatNoOfHIS { get; set; }

        /// <summary>
        /// 紧急程度： 1一般会诊  2 急会诊
        /// </summary>
        public string UrgencyTypeID { get; set; }

        public string UrgencyTypeName{ get; set; }
      

        /// <summary>
        /// 会诊类别： 1一般会诊  2 多科会诊
        /// </summary>
        public string ConsultTypeID { get; set; }

        public string ConsultTypeName{ get; set; }
        

        /// <summary>
        /// 病情及治疗情况
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// 申请会诊理由和目的
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// 申请科室
        /// </summary>
        public string ApplyDeptID { get; set; }
        /// <summary>
        /// 申请科室
        /// </summary>
        public string ApplyDeptName { get; set; }

        /// <summary>
        /// 申请医生
        /// </summary>
        public string ApplyUserID { get; set; }

        /// <summary>
        /// 申请医生
        /// </summary>
        public string ApplyUserName { get; set; }


        private string _ApplyTime;

        /// <summary>
        /// 申请时间
        /// </summary>
        public string ApplyTime
        {
            get 
            {
                if (_ApplyTime == "")
                    return "";
                return Convert.ToDateTime(_ApplyTime).ToString("yyyy年MM月dd日 HH时mm分");
            }
            set { _ApplyTime = value; }
        }

        /// <summary>
        /// 申请时间 年
        /// </summary>
        public string ApplyYear
        {
            get
            {
                if (_ApplyTime == "")
                    return "";
                return Convert.ToDateTime(_ApplyTime).ToString("yyyy");
            }
        }

        /// <summary>
        /// 申请时间 月
        /// </summary>
        public string ApplyMonth
        {
            get
            {
                if (_ApplyTime == "")
                    return "";
                return Convert.ToDateTime(_ApplyTime).ToString("MM");
            }
        }

        /// <summary>
        /// 申请时间 日
        /// </summary>
        public string ApplyDay
        {
            get
            {
                if (_ApplyTime == "")
                    return "";
                return Convert.ToDateTime(_ApplyTime).ToString("dd");
            }
        }

        /// <summary>
        /// 申请时间 时
        /// </summary>
        public string ApplyHour
        {
            get
            {
                if (_ApplyTime == "")
                    return "";
                return Convert.ToDateTime(_ApplyTime).ToString("HH");
            }
        }


        /// <summary>
        /// 申请时间 分钟
        /// </summary>
        public string ApplyMinute
        {
            get
            {
                if (_ApplyTime == "")
                    return "";
                return Convert.ToDateTime(_ApplyTime).ToString("mm");
            }
        }

        /// <summary>
        /// 会诊意见及建议
        /// </summary>
        public string ConsultSuggestion { get; set; }

        /// <summary>
        /// 申请会诊科室
        /// </summary>
        public string ConsultDeptID { get; set; }

        /// <summary>
        /// 申请会诊科室
        /// </summary>
        public string ConsultDeptName { get; set; }

        /// <summary>
        /// 会诊医院
        /// </summary>
        public string ConsultHospitalID { get; set; }

        /// <summary>
        /// 会诊医院
        /// </summary>
        public string ConsultHospitalName { get; set; }


        /// <summary>
        /// 实际会诊科室
        /// </summary>
        public string ConsultDeptID2 { get; set; }
        /// <summary>
        /// 实际会诊科室
        /// </summary>
        public string ConsultDeptName2 { get; set; }

        /// <summary>
        /// 会诊医生
        /// </summary>
        public string ConsultUserID { get; set; }
        /// <summary>
        /// 会诊医生
        /// </summary>
        public string ConsultUserName { get; set; }

        private string _ConsultTime;

        /// <summary>
        /// 会诊时间
        /// </summary>
        public string ConsultTime
        {
            get
            {
                return _ConsultTime;
                //if (_ConsultTime == "")
                //    return "";
                //return Convert.ToDateTime(_ConsultTime).ToString("yyyy年MM月dd日 HH时mm分");
            }
            set { _ConsultTime = value; }
        }

        /// <summary>
        /// 会诊时间 年
        /// </summary>
        public string ConsultYear
        {
            get
            {
                if (_ConsultTime == "")
                    return "";
                return Convert.ToDateTime(_ConsultTime).ToString("yyyy");
            }
        }

        /// <summary>
        /// 会诊时间 月
        /// </summary>
        public string ConsultMonth
        {
            get
            {
                if (_ConsultTime == "")
                    return "";
                return Convert.ToDateTime(_ConsultTime).ToString("MM");
            }
        }

        /// <summary>
        /// 会诊时间 日
        /// </summary>
        public string ConsultDay
        {
            get
            {
                if (_ConsultTime == "")
                    return "";
                return Convert.ToDateTime(_ConsultTime).ToString("dd");
            }
        }

        /// <summary>
        /// 会诊时间 时
        /// </summary>
        public string ConsultHour
        {
            get
            {
                if (_ConsultTime == "")
                    return "";
                return Convert.ToDateTime(_ConsultTime).ToString("HH");
            }
        }

        /// <summary>
        /// 会诊时间 分钟
        /// </summary>
        public string ConsultMinute
        {
            get
            {
                if (_ConsultTime == "")
                    return "";
                return Convert.ToDateTime(_ConsultTime).ToString("mm");
            }
        }

        /// <summary>
        /// 会诊申请单状态
        /// </summary>
        public string StateID { get; set; }

    }
}
