using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.Consultation.NEW.Entity
{
    ///<summary>
    ///会诊审核科室负责人表
    ///</summary>
    public class ConsultDeptautioEntity
    {
        #region declaration
        private int m_Id = 0;
        private string m_Deptid = String.Empty;
        private string m_Userid = String.Empty;
        private int m_Valid = 0;
        private string m_Createuser = String.Empty;
        #endregion declaration

        public ConsultDeptautioEntity()
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
        ///科室编号
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
        ///负责人工号
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
