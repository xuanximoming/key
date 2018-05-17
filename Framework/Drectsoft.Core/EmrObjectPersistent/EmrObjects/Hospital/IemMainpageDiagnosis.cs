using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core
{
    public class Iem_Mainpage_Diagnosis
    {
        #region declaration
        private int m_IemMainpageDiagnosisNo = 0;
        private int m_IemMainpageNo = 0;
        private int m_DiagnosisTypeId = 0;
        private string m_DiagnosisCode = String.Empty;
        private string m_DiagnosisName = String.Empty;
        private int m_StatusId = 0;
        private int m_OrderValue = 0;
        private int m_Valide = 0;
        private string m_CreateUser = String.Empty;
        private string m_CreateTime = String.Empty;
        private string m_CancelUser = String.Empty;
        private string m_CancelTime = String.Empty;
        #endregion declaration

        public Iem_Mainpage_Diagnosis()
        {
        }

        #region Properties
        public int IemMainpageDiagnosisNo
        {
            get
            {
                return m_IemMainpageDiagnosisNo;
            }
            set
            {
                m_IemMainpageDiagnosisNo = value;
            }
        }
        public int IemMainpageNo
        {
            get
            {
                return m_IemMainpageNo;
            }
            set
            {
                m_IemMainpageNo = value;
            }
        }
        public int DiagnosisTypeId
        {
            get
            {
                return m_DiagnosisTypeId;
            }
            set
            {
                m_DiagnosisTypeId = value;
            }
        }
        public string DiagnosisCode
        {
            get
            {
                return m_DiagnosisCode;
            }
            set
            {
                m_DiagnosisCode = value;
            }
        }
        public string DiagnosisName
        {
            get
            {
                return m_DiagnosisName;
            }
            set
            {
                m_DiagnosisName = value;
            }
        }
        public int StatusId
        {
            get
            {
                return m_StatusId;
            }
            set
            {
                m_StatusId = value;
            }
        }
        public int OrderValue
        {
            get
            {
                return m_OrderValue;
            }
            set
            {
                m_OrderValue = value;
            }
        }
        public int Valide
        {
            get
            {
                return m_Valide;
            }
            set
            {
                m_Valide = value;
            }
        }
        public string CreateUser
        {
            get
            {
                return m_CreateUser;
            }
            set
            {
                m_CreateUser = value;
            }
        }
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
        public string CancelUser
        {
            get
            {
                return m_CancelUser;
            }
            set
            {
                m_CancelUser = value;
            }
        }
        public string CancelTime
        {
            get
            {
                return m_CancelTime;
            }
            set
            {
                m_CancelTime = value;
            }
        }
        #endregion Properties
    }
}