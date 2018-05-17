using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 【评分配置的实体
    /// add by  ywk 
    /// </summary>
    public class ConfigReduction
    {
        private string m_id = String.Empty;
        /// <summary>
        /// 表主键，序列号
        /// </summary>
        public string ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private string m_REDUCEPOINT = String.Empty;
        /// <summary>
        /// 扣分标准（分数）
        /// </summary>
        public string REDUCEPOINT
        {
            get { return m_REDUCEPOINT; }
            set { m_REDUCEPOINT = value; }
        }

        private string m_PROBLEM_DESC = String.Empty;
        /// <summary>
        /// 扣分理由
        /// </summary>
        public string PROBLEMDESC
        {
            get { return m_PROBLEM_DESC; }
            set { m_PROBLEM_DESC = value; }
        }

        private string m_CreateUserID = string.Empty;
        /// <summary>
        /// 创建人工号
        /// </summary>
        public string CreateUserID
        {
            get
            {
                return m_CreateUserID;
            }
            set { m_CreateUserID = value; }
        }
        private string m_CreateUserName = string.Empty;
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateUserName
        {
            get { return m_CreateUserName; }
            set { m_CreateUserName = value; }
        }

        private string m_CreateTime = string.Empty;
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime
        {
            get
            {
                return m_CreateTime;
            }
            set
            {
                m_CreateTime = value;
            }
        }

        private string m_valid = String.Empty;
        /// <summary>
        /// 是否有效
        /// </summary>
        public string Valid
        {
            get { return m_valid; }
            set { m_valid = value; }
        }

        private string usertype = String.Empty;

        /// <summary>
        /// 用户类别
        /// </summary>
        public string Usertype
        {
            get { return usertype; }
            set { usertype = value; }
        }
    }
}
