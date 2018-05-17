using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YindanSoft.Emr.QcManagerNew
{
    public class QCScoreType_DataEntity
    {

        #region declaration
        private string m_Typecode = String.Empty;
        private string m_Typename = String.Empty;
        private string m_Typeinstruction = String.Empty;
        private int m_Typecategory = 0;
        private int m_Typeorder = 0;
        private string m_Typememo = String.Empty;
        private string m_Valide = String.Empty;
        #endregion declaration

        public QCScoreType_DataEntity()
        {
        }

        #region Properties
        public string Typecode
        {
            get
            {
                return m_Typecode;
            }
            set
            {
                m_Typecode = value;
            }
        }
        public string Typename
        {
            get
            {
                return m_Typename;
            }
            set
            {
                m_Typename = value;
            }
        }
        public string Typeinstruction
        {
            get
            {
                return m_Typeinstruction;
            }
            set
            {
                m_Typeinstruction = value;
            }
        }
        public int Typecategory
        {
            get
            {
                return m_Typecategory;
            }
            set
            {
                m_Typecategory = value;
            }
        }
        public int Typeorder
        {
            get
            {
                return m_Typeorder;
            }
            set
            {
                m_Typeorder = value;
            }
        }
        public string Typememo
        {
            get
            {
                return m_Typememo;
            }
            set
            {
                m_Typememo = value;
            }
        }
        public string Valide
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
        #endregion Properties


    }
}
