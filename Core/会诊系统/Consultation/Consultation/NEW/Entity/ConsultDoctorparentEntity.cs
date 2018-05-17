using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.Consultation.NEW.Entity
{
    ///<summary>
    ///会诊审核配置（上级医师）
    ///</summary>
    public class ConsultDoctorparentEntity
    {
        #region declaration
        private int m_Id = 0;
        private string m_Userid = String.Empty;
        private string m_Parentuserid = String.Empty;
        private int m_Valid = 0;
        private string m_Deptid = String.Empty;
        private string m_Createuser = String.Empty;
        #endregion declaration

        public ConsultDoctorparentEntity()
        {
        }

        #region Properties
        ///<summary>
        ///编号
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
        ///医生工号
        ///</summary>
        public string Userid
        {
            get
            {
                return m_Userid;
            }
            set
            {
                m_Userid = value;
            }
        }
        ///<summary>
        ///上级医师工号
        ///</summary>
        public string Parentuserid
        {
            get
            {
                return m_Parentuserid;
            }
            set
            {
                m_Parentuserid = value;
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
        ///部门编号
        ///</summary>
        public string Deptid
        {
            get
            {
                return m_Deptid;
            }
            set
            {
                m_Deptid = value;
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
        #endregion Properties
    }
}
