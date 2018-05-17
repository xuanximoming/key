using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.SysTableEdit.DataEntity
{
    /// <summary>
    /// 诊断库
    /// </summary>
    public class Diagnosis
    {
        #region declaration
        private string m_Markid = String.Empty;
        private string m_Icd = String.Empty;
        private string m_Mapid = String.Empty;
        private string m_Standardcode = String.Empty;
        private string m_Name = String.Empty;
        private string m_Py = String.Empty;
        private string m_Wb = String.Empty;
        private string m_Tumorid = String.Empty;
        private string m_Statist = String.Empty;
        private string m_Innercategory = String.Empty;
        private string m_Category = String.Empty;
        private string m_Othercategroy = String.Empty;
        private int m_Valid = 0;
        private string m_Memo = String.Empty;
        #endregion declaration

        public Diagnosis()
        {
        }

        #region Properties
        public string Markid
        {
            get
            {
                return m_Markid;
            }
            set
            {
                m_Markid = value;
            }
        }
        public string Icd
        {
            get
            {
                return m_Icd;
            }
            set
            {
                m_Icd = value;
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
        public string Tumorid
        {
            get
            {
                return m_Tumorid;
            }
            set
            {
                m_Tumorid = value;
            }
        }
        public string Statist
        {
            get
            {
                return m_Statist;
            }
            set
            {
                m_Statist = value;
            }
        }
        public string Innercategory
        {
            get
            {
                return m_Innercategory;
            }
            set
            {
                m_Innercategory = value;
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
        public string Othercategroy
        {
            get
            {
                return m_Othercategroy;
            }
            set
            {
                m_Othercategroy = value;
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
