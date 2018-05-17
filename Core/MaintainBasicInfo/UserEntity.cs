using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.MaintainBasicInfo
{
    /// <summary>
    ///类名:UserEntity
    ///功能说明:用户实体
    ///创建人:wyt
    ///创建时间:2012-11-12
    /// </summary>
    class UserEntity
    {
        private string m_id = "";
        private string m_name = "";
        private string m_sex = "";
        private DateTime m_birthday = DateTime.Now;
        private string m_cardid = "";
        private string m_marital = "";
        private string m_deptid = "";
        private string m_wardid = "";

        /// <summary>
        /// 用户编号
        /// </summary>
        public string ID
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }
        /// <summary>
        /// 用户性别
        /// </summary>
        public string Sex
        {
            get
            {
                return m_sex;
            }
            set
            {
                m_sex = value;
            }
        }
        /// <summary>
        /// 用户生日
        /// </summary>
        public DateTime Birthday
        {
            get
            {
                return m_birthday;
            }
            set
            {
                m_birthday = value;
            }
        }
        /// <summary>
        /// 用户身份证号
        /// </summary>
        public string CardID
        {
            get
            {
                return m_cardid;
            }
            set
            {
                m_cardid = value;
            }
        }
        /// <summary>
        /// 用户婚姻状态
        /// </summary>
        public string Marital
        {
            get
            {
                return m_marital;
            }
            set
            {
                m_marital = value;
            }
        }
        /// <summary>
        /// 用户所在科室编号
        /// </summary>
        public string DeptID
        {
            get
            {
                return m_deptid;
            }
            set
            {
                m_deptid = value;
            }
        }
        /// <summary>
        /// 用户所在病区编号
        /// </summary>
        public string WardID
        {
            get
            {
                return m_wardid;
            }
            set
            {
                m_wardid = value;
            }
        }
    }
}
