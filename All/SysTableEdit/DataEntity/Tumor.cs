using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.SysTableEdit.DataEntity
{
    /// <summary>
    /// 肿瘤库
    /// </summary>
    public class Tumor
    {
        #region declaration
        private string m_Id = String.Empty;
        private string m_Mapid = String.Empty;
        private string m_Standardcode = String.Empty;
        private string m_Name = String.Empty;
        private string m_Py = String.Empty;
        private string m_Wb = String.Empty;
        private int m_Valid = 0;
        private string m_Memo = String.Empty;
        #endregion declaration

        public Tumor()
        {
        }

        #region Properties
        public string Id
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
        public string Mapid
        {
            get
            {
                return m_Mapid;
            }
            set
            {
                m_Mapid = value;
            }
        }
        public string Standardcode
        {
            get
            {
                return m_Standardcode;
            }
            set
            {
                m_Standardcode = value;
            }
        }
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
        public string Py
        {
            get
            {
                return m_Py;
            }
            set
            {
                m_Py = value;
            }
        }
        public string Wb
        {
            get
            {
                return m_Wb;
            }
            set
            {
                m_Wb = value;
            }
        }
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
        public string Memo
        {
            get
            {
                return m_Memo;
            }
            set
            {
                m_Memo = value;
            }
        }
        #endregion Properties
    }
}
