using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.SysTableEdit.DataEntity
{
    /// <summary>
    /// 病种设置库 
    /// </summary>
    public class DiseaseCFG
    {
        #region declaration
        private string m_Id = String.Empty;
        private string m_Mapid = String.Empty;
        private string m_Name = String.Empty;
        private string m_Py = String.Empty;
        private string m_Wb = String.Empty;
        private string m_Diseaseid = String.Empty;
        private string m_Surgeryid = String.Empty;
        private string m_Category = String.Empty;
        private string m_Mark = String.Empty;
        private string m_Parentid = String.Empty;
        private int m_Valid = 0;
        private string m_Memo = String.Empty;
        #endregion declaration

        public DiseaseCFG()
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
        public string Diseaseid
        {
            get
            {
                return m_Diseaseid;
            }
            set
            {
                m_Diseaseid = value;
            }
        }
        public string Surgeryid
        {
            get
            {
                return m_Surgeryid;
            }
            set
            {
                m_Surgeryid = value;
            }
        }
        public string Category
        {
            get
            {
                return m_Category;
            }
            set
            {
                m_Category = value;
            }
        }
        public string Mark
        {
            get
            {
                return m_Mark;
            }
            set
            {
                m_Mark = value;
            }
        }
        public string Parentid
        {
            get
            {
                return m_Parentid;
            }
            set
            {
                m_Parentid = value;
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
