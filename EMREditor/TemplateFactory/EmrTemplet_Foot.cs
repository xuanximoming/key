using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Emr.TemplateFactory
{
    /// <summary>
    /// 模板页眉
    /// </summary>
    public class EmrTemplet_Foot
    {

        #region declaration
        private string m_FootId = String.Empty;
        private string m_Name = String.Empty;
        private string m_CreatorId = String.Empty;
        private string m_CreateDatetime = String.Empty;
        private string m_LastTime = String.Empty;
        private string m_HospitalCode = String.Empty;
        private string m_Content = string.Empty;

        #endregion declaration

        public EmrTemplet_Foot()
        {
        }

        #region Properties
        
        /// <summary>
        /// 页眉ID
        /// </summary>
        public string FootId
        {
            get
            {
                return m_FootId;
            }
            set
            {
                m_FootId = value;
            }
        }
        /// <summary>
        /// 页面名称
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
        /// <summary>
        /// 页眉创建人
        /// </summary>
        public string CreatorId
        {
            get
            {
                return m_CreatorId;
            }
            set
            {
                m_CreatorId = value;
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDatetime
        {
            get
            {
                return m_CreateDatetime;
            }
            set
            {
                m_CreateDatetime = value;
            }
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public string LastTime
        {
            get
            {
                return m_LastTime;
            }
            set
            {
                m_LastTime = value;
            }
        }
        /// <summary>
        /// 医院编码
        /// </summary>
        public string HospitalCode
        {
            get
            {
                return m_HospitalCode;
            }
            set
            {
                m_HospitalCode = value;
            }
        }

        /// <summary>
        /// 页眉内容
        /// </summary>
        public string Content
        {
            get
            {
                return m_Content;
            }
            set
            {
                m_Content = value;
            }
        }
        #endregion Properties


    }
}
