using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YindanSoft.Emr.QcManagerNew
{
    public class QCScoreItem_DataEntity
    {
        #region declaration
        private string m_Itemcode = String.Empty;
        private string m_Itemname = String.Empty;
        private string m_Iteminstruction = String.Empty;
        private int m_Itemdefaultscore = 0;
        private int m_Itemstandardscore = 0;
        private int m_Itemcategory = 0;
        private int m_Itemdefaulttarget = 0;
        private int m_Itemtargetstandard = 0;
        private int m_Itemscorestandard = 0;
        private int m_Itemorder = 0;
        private string m_Typecode = String.Empty;
        private string m_Itemmemo = String.Empty;
        private string m_Valide = String.Empty;
        #endregion declaration

        public QCScoreItem_DataEntity()
        {
        }

        #region Properties
        public string Itemcode
        {
            get
            {
                return m_Itemcode;
            }
            set
            {
                m_Itemcode = value;
            }
        }
        public string Itemname
        {
            get
            {
                return m_Itemname;
            }
            set
            {
                m_Itemname = value;
            }
        }
        public string Iteminstruction
        {
            get
            {
                return m_Iteminstruction;
            }
            set
            {
                m_Iteminstruction = value;
            }
        }
        public int Itemdefaultscore
        {
            get
            {
                return m_Itemdefaultscore;
            }
            set
            {
                m_Itemdefaultscore = value;
            }
        }
        public int Itemstandardscore
        {
            get
            {
                return m_Itemstandardscore;
            }
            set
            {
                m_Itemstandardscore = value;
            }
        }
        public int Itemcategory
        {
            get
            {
                return m_Itemcategory;
            }
            set
            {
                m_Itemcategory = value;
            }
        }
        public int Itemdefaulttarget
        {
            get
            {
                return m_Itemdefaulttarget;
            }
            set
            {
                m_Itemdefaulttarget = value;
            }
        }
        public int Itemtargetstandard
        {
            get
            {
                return m_Itemtargetstandard;
            }
            set
            {
                m_Itemtargetstandard = value;
            }
        }
        public int Itemscorestandard
        {
            get
            {
                return m_Itemscorestandard;
            }
            set
            {
                m_Itemscorestandard = value;
            }
        }
        public int Itemorder
        {
            get
            {
                return m_Itemorder;
            }
            set
            {
                m_Itemorder = value;
            }
        }
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
        public string Itemmemo
        {
            get
            {
                return m_Itemmemo;
            }
            set
            {
                m_Itemmemo = value;
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
