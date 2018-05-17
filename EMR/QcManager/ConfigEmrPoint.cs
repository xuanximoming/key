using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 配置对应分了详细信息实体 add by ywk 2012年3月31日16:28:46
    /// </summary>
    public class ConfigEmrPoint
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
        private string m_Ccode = String.Empty;
        /// <summary>
        /// 所属分类的编号
        /// </summary>
        public string CCODE
        {
            get { return m_Ccode; }
            set { m_Ccode = value; }
        }

        private string m_Cchildcode = String.Empty;
        /// <summary>
        /// 子分类编号
        /// </summary>
        public string CChildCode
        {
            get { return m_Cchildcode; }
            set { m_Cchildcode = value; }
        }
        private string m_Cchildname = String.Empty;
        /// <summary>
        /// 子分类名称
        /// </summary>
        public string CChildName
        {
            get { return m_Cchildname; }
            set { m_Cchildname = value; }
        }
        private string m_valid= String.Empty;
        /// <summary>
        /// 是否有效
        /// </summary>
        public string Valid
        {
            get { return m_valid; }
            set { m_valid = value; }
        }
    }
}
