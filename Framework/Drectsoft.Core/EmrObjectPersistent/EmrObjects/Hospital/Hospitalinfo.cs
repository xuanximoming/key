using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core
{
    public class HospitalInfo
    {
        #region declaration
        private string m_Id = String.Empty;
        private string m_Name = String.Empty;
        private string m_Subname = String.Empty;
        private string m_Sn = String.Empty;
        private string m_Medicalid = String.Empty;
        private string m_Address = String.Empty;
        private string m_Yzbm = String.Empty;
        private string m_Tel = String.Empty;
        private int m_Bzcws = 0;
        private string m_Memo = String.Empty;
        #endregion declaration

        public HospitalInfo()
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
        public string Subname
        {
            get
            {
                return m_Subname;
            }
            set
            {
                m_Subname = value;
            }
        }
        public string Sn
        {
            get
            {
                return m_Sn;
            }
            set
            {
                m_Sn = value;
            }
        }
        public string Medicalid
        {
            get
            {
                return m_Medicalid;
            }
            set
            {
                m_Medicalid = value;
            }
        }
        public string Address
        {
            get
            {
                return m_Address;
            }
            set
            {
                m_Address = value;
            }
        }
        public string Yzbm
        {
            get
            {
                return m_Yzbm;
            }
            set
            {
                m_Yzbm = value;
            }
        }
        public string Tel
        {
            get
            {
                return m_Tel;
            }
            set
            {
                m_Tel = value;
            }
        }
        public int Bzcws
        {
            get
            {
                return m_Bzcws;
            }
            set
            {
                m_Bzcws = value;
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

        #endregion
    }
}
