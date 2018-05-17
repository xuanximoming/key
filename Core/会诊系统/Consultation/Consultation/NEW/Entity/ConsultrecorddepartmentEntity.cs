using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.Consultation.NEW.Entity
{
    ///<summary>
    ///会诊参与科室(多科会诊) 
    ///</summary>
    public class ConsultrecorddepartmentEntity
    {
        #region declaration
        private int m_Id = 0;
        private int m_Consultapplysn = 0;
        private int m_Ordervalue = 0;
        private string m_Hospitalcode = String.Empty;
        private string m_Departmentcode = String.Empty;
        private string m_Departmentname = String.Empty;
        private string m_Employeecode = String.Empty;
        private string m_Employeename = String.Empty;
        private string m_Employeelevelid = String.Empty;
        private string m_Createuser = String.Empty;
        private int m_Valid = 0;
        private string m_Canceluser = String.Empty;
        private string m_Issignin = String.Empty;
        #endregion declaration

        public ConsultrecorddepartmentEntity()
        {
        }

        #region Properties
        ///<summary>
        ///流水ID,自增长
        ///</summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                m_Id = value;
            }
        }
        ///<summary>
        ///对应的会诊申请单号
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
        ///-顺序ID，一般会诊存1
        ///</summary>
        public int Ordervalue
        {
            get
            {
                return m_Ordervalue;
            }
            set
            {
                m_Ordervalue = value;
            }
        }
        ///<summary>
        ///医院码
        ///</summary>
        public string Hospitalcode
        {
            get
            {
                return m_Hospitalcode;
            }
            set
            {
                m_Hospitalcode = value;
            }
        }
        ///<summary>
        ///科室码
        ///</summary>
        public string Departmentcode
        {
            get
            {
                return m_Departmentcode;
            }
            set
            {
                m_Departmentcode = value;
            }
        }
        ///<summary>
        ///科室名称，用于外院
        ///</summary>
        public string Departmentname
        {
            get
            {
                return m_Departmentname;
            }
            set
            {
                m_Departmentname = value;
            }
        }
        ///<summary>
        ///被邀请会诊医师工号
        ///</summary>
        public string Employeecode
        {
            get
            {
                return m_Employeecode;
            }
            set
            {
                m_Employeecode = value;
            }
        }
        ///<summary>
        ///被邀请会诊医师姓名，用于外院
        ///</summary>
        public string Employeename
        {
            get
            {
                return m_Employeename;
            }
            set
            {
                m_Employeename = value;
            }
        }
        ///<summary>
        ///医师级别 对应Inpatient.Grade
        ///</summary>
        public string Employeelevelid
        {
            get
            {
                return m_Employeelevelid;
            }
            set
            {
                m_Employeelevelid = value;
            }
        }
        ///<summary>
        ///开立人
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
        ///作废否
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
        ///作废人
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
        ///是否签到
        ///</summary>
        public string Issignin
        {
            get
            {
                return m_Issignin;
            }
            set
            {
                m_Issignin = value;
            }
        }
        #endregion Properties
    }
}

