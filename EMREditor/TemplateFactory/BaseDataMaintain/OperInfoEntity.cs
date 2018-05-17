using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    /// <summary>
    /// 手术信息实体类
    /// add by ywk 2012年6月25日 13:10:18
    /// </summary>
    class OperInfoEntity
    {
        #region declaration
        private string m_Id = String.Empty;
        private string m_Mapid = String.Empty;
        private string m_Name = String.Empty;
        private string m_Py = String.Empty;
        private string m_Wb = String.Empty;
        private int m_Valid = 0;
        private string m_Memo = String.Empty;
        private string m_Sslb = String.Empty;
        #endregion declaration

        public OperInfoEntity()
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
       
        public string Sslb
        {
            get
            {
                return m_Sslb;
            }
            set
            {
                m_Sslb = value;
            }
        }
        #endregion Properties
    }
  
}
