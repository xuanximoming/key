using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.NurseDocument
{
    class PatStateEntity
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
        /// 所属状态编号
        /// </summary>
        public string CCODE
        {
            get { return m_Ccode; }
            set { m_Ccode = value; }
        }

        private string m_PatID = String.Empty;
        /// <summary>
        /// 病案首页编号
        /// </summary>
        public string PATID
        {
            get { return m_PatID; }
            set { m_PatID = value; }
        }

        private string m_DoTime = String.Empty;
        /// <summary>
        /// 时间
        /// </summary>
        public string DOTIME
        {
            get { return m_DoTime; }
            set { m_DoTime = value; }
        }

        private string m_Noofpat = String.Empty;

        public string Noofpat
        {
            get { return m_Noofpat; }
            set { m_Noofpat = value; }
        }
    }
}
