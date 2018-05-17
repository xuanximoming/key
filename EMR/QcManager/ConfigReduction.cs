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
        /// <summary>
        /// add by  王冀 2012-11-8
        /// </summary>
        public string m_ParentsCode = String.Empty;
        /// <summary>
        /// 父类名称序号
        /// </summary>
        public string ParentsCode
        {
            get { return m_ParentsCode; }
            set { m_ParentsCode = value; }
        }

        public string m_Parents = String.Empty;
        /// <summary>
        /// 父类名称
        /// </summary>
        public string Parents
        {
            get { return m_Parents; }
            set { m_Parents = value; }
        }

        public string m_ChildCode = String.Empty;
        /// <summary>
        /// 子类名称序号
        /// </summary>
        public string ChildCode
        {
            get { return m_ChildCode; }
            set { m_ChildCode = value; }
        }
        public string m_Child = String.Empty;
        /// <summary>
        /// 子类名称
        /// </summary>
        public string Child
        {
            get { return m_Child; }
            set { m_Child = value; }
        }
        public string m_Isauto = String.Empty;
        /// <summary>
        /// 自动评分类型
        /// </summary>
        public string Isauto
        {
            get { return m_Isauto; }
            set { m_Isauto = value; }
        }
        public string m_Selectcondition = String.Empty;
        /// <summary>
        /// 关键字
        /// </summary>
        public string Selectcondition
        {
            get { return m_Selectcondition; }
            set { m_Selectcondition = value; }
        }
    }
}
