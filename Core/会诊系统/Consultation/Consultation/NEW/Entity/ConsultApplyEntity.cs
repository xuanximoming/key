using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.Consultation.NEW.Entity
{
    ///<summary>
    ///会诊申请表
    ///</summary>
    public class Consultapply
    {
        #region declaration
        private int m_Consultapplysn = 0;
        private int m_Noofinpat = 0;
        private int m_Urgencytypeid = 0;
        private int m_Consulttypeid = 0;
        private string m_Abstract = String.Empty;
        private string m_Purpose = String.Empty;
        private string m_Applyuser = String.Empty;
        private string m_Applytime = String.Empty;
        private string m_Director = String.Empty;
        private string m_Consultlocation = String.Empty;
        private int m_Stateid = 0;
        private string m_Consultsuggestion = String.Empty;
        private string m_Rejectreason = String.Empty;
        private string m_Createuser = String.Empty;
        private string m_Modifieduser = String.Empty;
        private int m_Valid = 0;
        private string m_Canceluser = String.Empty;
        private string m_Applydept = String.Empty;
        private string m_Ward = String.Empty;
        private string m_Bed = String.Empty;
        private int m_Ispay = 0;
        private string m_Audituserid = String.Empty;
        #endregion declaration

        public Consultapply()
        {
        }

        #region Properties
        ///<summary>
        ///申请单据号
        ///</summary>
        public int Consultapplysn
        {
            get
            {
                return m_Consultapplysn;
            }
            set
            {
                m_Consultapplysn = value;
            }
        }
        ///<summary>
        ///首页序号
        ///</summary>
        public int Noofinpat
        {
            get
            {
                return m_Noofinpat;
            }
            set
            {
                m_Noofinpat = value;
            }
        }
        ///<summary>
        ///会诊紧急度
        ///</summary>
        public int Urgencytypeid
        {
            get
            {
                return m_Urgencytypeid;
            }
            set
            {
                m_Urgencytypeid = value;
            }
        }
        ///<summary>
        ///会诊类型
        ///</summary>
        public int Consulttypeid
        {
            get
            {
                return m_Consulttypeid;
            }
            set
            {
                m_Consulttypeid = value;
            }
        }
        ///<summary>
        ///病历摘要
        ///</summary>
        public string Abstract
        {
            get
            {
                return m_Abstract;
            }
            set
            {
                m_Abstract = value;
            }
        }
        ///<summary>
        ///会诊目的
        ///</summary>
        public string Purpose
        {
            get
            {
                return m_Purpose;
            }
            set
            {
                m_Purpose = value;
            }
        }
        ///<summary>
        ///申请人
        ///</summary>
        public string Applyuser
        {
            get
            {
                return m_Applyuser;
            }
            set
            {
                m_Applyuser = value;
            }
        }
        ///<summary>
        ///申请时间
        ///</summary>
        public string Applytime
        {
            get
            {
                return m_Applytime;
            }
            set
            {
                m_Applytime = value;
            }
        }
        ///<summary>
        ///科室主任（待确认）
        ///</summary>
        public string Director
        {
            get
            {
                return m_Director;
            }
            set
            {
                m_Director = value;
            }
        }
        ///<summary>
        ///会诊地点
        ///</summary>
        public string Consultlocation
        {
            get
            {
                return m_Consultlocation;
            }
            set
            {
                m_Consultlocation = value;
            }
        }
        ///<summary>
        ///会诊单状态 6710:会诊申请保存 6720:待审核 6730:待会诊 6740:会诊记录保存 6741:会诊记录完成 6750:否决 6770:已取消
        ///</summary>
        public int Stateid
        {
            get
            {
                return m_Stateid;
            }
            set
            {
                m_Stateid = value;
            }
        }
        ///<summary>
        ///专家会诊意见
        ///</summary>
        public string Consultsuggestion
        {
            get
            {
                return m_Consultsuggestion;
            }
            set
            {
                m_Consultsuggestion = value;
            }
        }
        ///<summary>
        ///审核意见
        ///</summary>
        public string Rejectreason
        {
            get
            {
                return m_Rejectreason;
            }
            set
            {
                m_Rejectreason = value;
            }
        }
        ///<summary>
        ///创建人
        ///</summary>
        public string Createuser
        {
            get
            {
                return m_Createuser;
            }
            set
            {
                m_Createuser = value;
            }
        }
        ///<summary>
        ///修改人
        ///</summary>
        public string Modifieduser
        {
            get
            {
                return m_Modifieduser;
            }
            set
            {
                m_Modifieduser = value;
            }
        }
        ///<summary>
        ///是否有效
        ///</summary>
        public int Valid
        {
            get
            {
                return m_Valid;
            }
            set
            {
                m_Valid = value;
            }
        }
        ///<summary>
        ///取消人
        ///</summary>
        public string Canceluser
        {
            get
            {
                return m_Canceluser;
            }
            set
            {
                m_Canceluser = value;
            }
        }
        ///<summary>
        ///申请科室
        ///</summary>
        public string Applydept
        {
            get
            {
                return m_Applydept;
            }
            set
            {
                m_Applydept = value;
            }
        }
        ///<summary>
        ///患者病区
        ///</summary>
        public string Ward
        {
            get
            {
                return m_Ward;
            }
            set
            {
                m_Ward = value;
            }
        }
        ///<summary>
        ///患者床位
        ///</summary>
        public string Bed
        {
            get
            {
                return m_Bed;
            }
            set
            {
                m_Bed = value;
            }
        }
        ///<summary>
        ///是否缴费
        ///</summary>
        public int Ispay
        {
            get
            {
                return m_Ispay;
            }
            set
            {
                m_Ispay = value;
            }
        }
        ///<summary>
        ///审核人
        ///</summary>
        public string Audituserid
        {
            get
            {
                return m_Audituserid;
            }
            set
            {
                m_Audituserid = value;
            }
        }
        #endregion Properties
    }
}
