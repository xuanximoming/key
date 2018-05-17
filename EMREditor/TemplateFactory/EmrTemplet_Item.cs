using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Emr.TemplateFactory
{
    /// <summary>
    /// EmrTempletItem实体
    /// </summary>
    public class Emrtemplet_Item
    {

        #region declaration
        private string m_MrClass = String.Empty;
        private string m_MrCode = String.Empty;
        private string m_MrName = String.Empty;
        private string m_MrAttr = String.Empty;
        private string m_QcCode = String.Empty;
        private string m_DeptId = String.Empty;
        private string m_CreatorId = String.Empty;
        private string m_CreateDateTime = String.Empty;
        private string m_LastTime = String.Empty;
        private string m_ContentCode = String.Empty;
        private int m_Permission = 0;
        private int m_Visibled = 0;
        private string m_Input = String.Empty;
        private string m_HospitalCode = String.Empty;
        private string m_ITEM_DOC_NEW = String.Empty;
        #endregion declaration

        public Emrtemplet_Item()
        {
        }

        #region Properties
        public string MrClass
        {
            get
            {
                return m_MrClass;
            }
            set
            {
                m_MrClass = value;
            }
        }
        public string MrCode
        {
            get
            {
                return m_MrCode;
            }
            set
            {
                m_MrCode = value;
            }
        }
        public string MrName
        {
            get
            {
                return m_MrName;
            }
            set
            {
                m_MrName = value;
            }
        }
        public string MrAttr
        {
            get
            {
                return m_MrAttr;
            }
            set
            {
                m_MrAttr = value;
            }
        }
        public string QcCode
        {
            get
            {
                return m_QcCode;
            }
            set
            {
                m_QcCode = value;
            }
        }
        public string DeptId
        {
            get
            {
                return m_DeptId;
            }
            set
            {
                m_DeptId = value;
            }
        }
        public string CreatorId
        {
            get
            {
                return m_CreatorId;
            }
            set
            {
                m_CreatorId = value;
            }
        }
        public string CreateDateTime
        {
            get
            {
                return m_CreateDateTime;
            }
            set
            {
                m_CreateDateTime = value;
            }
        }
        public string LastTime
        {
            get
            {
                return m_LastTime;
            }
            set
            {
                m_LastTime = value;
            }
        }
        public string ContentCode
        {
            get
            {
                return m_ContentCode;
            }
            set
            {
                m_ContentCode = value;
            }
        }
        public int Permission
        {
            get
            {
                return m_Permission;
            }
            set
            {
                m_Permission = value;
            }
        }
        public int Visibled
        {
            get
            {
                return m_Visibled;
            }
            set
            {
                m_Visibled = value;
            }
        }
        public string Input
        {
            get
            {
                return m_Input;
            }
            set
            {
                m_Input = value;
            }
        }
        
        public string HospitalCode
        {
            get
            {
                return m_HospitalCode;
            }
            set
            {
                m_HospitalCode = value;
            }
        }

        /// <summary>
        /// 子模板文件
        /// </summary>
        public string ITEM_DOC_NEW
        {

            get
            {
                return RecordDal.UnzipEmrXml(m_ITEM_DOC_NEW);
            }
            set
            {
                m_ITEM_DOC_NEW = RecordDal.ZipEmrXml(value);
            }

        }

        /// <summary>
        /// 子模板压缩后文件(方便数据库操作)
        /// </summary>
        public string Zip_ITEM_DOC_NEW
        {

            get
            {
                return m_ITEM_DOC_NEW;
            }
            set
            {
                m_ITEM_DOC_NEW = value;
            }

        }
        #endregion Properties
    }

}
