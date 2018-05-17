using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core
{
    public class Iem_MainPage_Operation
    {
        #region declaration
        private int m_IemMainpageOperationNo = 0;
        private int m_IemMainpageNo = 0;
        private string m_OperationCode = String.Empty;
        private string m_OperationDate = String.Empty;
        private string m_OperationName = String.Empty;
        private string m_ExecuteUser1 = String.Empty;
        private string m_ExecuteUser2 = String.Empty;
        private string m_ExecuteUser3 = String.Empty;
        private int m_AnaesthesiaTypeId = 0;
        private int m_CloseLevel = 0;
        private string m_AnaesthesiaUser = String.Empty;
        private int m_Valide = 0;
        private string m_CreateUser = String.Empty;
        private string m_CreateTime = String.Empty;
        private string m_CancelUser = String.Empty;
        private string m_CancelTime = String.Empty;
        #endregion declaration

        public Iem_MainPage_Operation()
        {
        }

        #region Properties
        public int IemMainpageOperationNo
        {
            get
            {
                return m_IemMainpageOperationNo;
            }
            set
            {
                m_IemMainpageOperationNo = value;
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
        public string OperationCode
        {
            get
            {
                return m_OperationCode;
            }
            set
            {
                m_OperationCode = value;
            }
        }
        public string OperationDate
        {
            get
            {
                return m_OperationDate;
            }
            set
            {
                m_OperationDate = value;
            }
        }
        public string OperationName
        {
            get
            {
                return m_OperationName;
            }
            set
            {
                m_OperationName = value;
            }
        }
        public string ExecuteUser1
        {
            get
            {
                return m_ExecuteUser1;
            }
            set
            {
                m_ExecuteUser1 = value;
            }
        }
        public string ExecuteUser2
        {
            get
            {
                return m_ExecuteUser2;
            }
            set
            {
                m_ExecuteUser2 = value;
            }
        }
        public string ExecuteUser3
        {
            get
            {
                return m_ExecuteUser3;
            }
            set
            {
                m_ExecuteUser3 = value;
            }
        }
        public int AnaesthesiaTypeId
        {
            get
            {
                return m_AnaesthesiaTypeId;
            }
            set
            {
                m_AnaesthesiaTypeId = value;
            }
        }
        public int CloseLevel
        {
            get
            {
                return m_CloseLevel;
            }
            set
            {
                m_CloseLevel = value;
            }
        }
        public string AnaesthesiaUser
        {
            get
            {
                return m_AnaesthesiaUser;
            }
            set
            {
                m_AnaesthesiaUser = value;
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