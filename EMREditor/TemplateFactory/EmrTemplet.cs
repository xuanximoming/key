using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Emr.TemplateFactory
{
    /// <summary>
    /// EmrTemplet实体
    /// </summary>
    public class Emrtemplet
    {

        #region declaration
        private string m_TempletId = String.Empty;
        private string m_FileName = String.Empty;
        private string m_DeptId = String.Empty;
        private string m_CreatorId = String.Empty;
        private string m_CreateDatetime = String.Empty;
        private string m_LastTime = String.Empty;
        private string m_Permission = String.Empty;
        private string m_MrClass = String.Empty;
        private string m_MrCode = String.Empty;
        private string m_MrName = String.Empty;
        private string m_MrAttr = String.Empty;
        private string m_QcCode = String.Empty;
        private int m_NewPageFlag = 0;
        private int m_FileFlag = 0;
        private int m_WriteTimes = 0;
        private int valid = 0;//是否有效
        private string m_Code = String.Empty;
        private string m_HospitalCode = String.Empty;

        private string m_XML_DOC_NEW = String.Empty;

        private string m_IsFirstDailyEmr = "0"; //是否首次病程 0：否 1：是
        private string m_DailyTitle = string.Empty;
        private string m_IsShowDailyTitle = "0"; //是否显示病程标题 0：否 1：是
        private string m_IsYiHuanGouTong = "0"; //是否为医患沟通 0：否 1：是

        private string m_IsConfigPageSize = "0cc"; //是否页面配置 0：否 1：是

        private int m_NEW_PAGE_END = 0; //是否为新页结束的 0：否 1：是

        private string m_State = string.Empty; //模版状态  是否审核，保存未提交：0；待审核：1；审核通过：2，审核不通过：3；
        private string m_Auditor = string.Empty; //审核人
        private string m_AuditDate = string.Empty;  //审核时间


        private bool m_ShowStar = false;//是否显示已配对的星号


        #endregion declaration

        public Emrtemplet()
        {
        }

        #region Properties
        /// <summary>
        /// 控制是否显示星号的，封装为属性 add by ywk 2012年4月15日15:07:24
        /// </summary>
        public bool ShowStar
        {
            get
            {
                return m_ShowStar;
            }
            set {
                m_ShowStar = value;
            }
        }
        /// <summary>
        /// 模板ID
        /// </summary>
        public string TempletId
        {
            get
            {
                return m_TempletId;
            }
            set
            {
                m_TempletId = value;
            }
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get
            {
                return m_FileName;
            }
            set
            {
                m_FileName = value;
            }
        }

        /// <summary>
        /// 科室ID
        /// </summary>
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

        /// <summary>
        /// 有效标志
        /// </summary>
        public int Valid
        {
            get
            {
                return valid;
            }
            set
            {
                Valid = value;
            }
        }
        /// <summary>
        /// 创建人ID
        /// </summary>
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
        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateDatetime
        {
            get
            {
                return m_CreateDatetime;
            }
            set
            {
                m_CreateDatetime = value;
            }
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
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
        /// <summary>
        /// 访问权限
        /// </summary>
        public string Permission
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
        /// <summary>
        /// 类别
        /// </summary>
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
        /// <summary>
        /// 代码
        /// </summary>
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
        /// <summary>
        /// 名称
        /// </summary>
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
        /// <summary>
        /// 属性
        /// </summary>
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
        /// <summary>
        /// 质控代码
        /// </summary>
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
        /// <summary>
        /// 新页标记
        /// </summary>
        public int NewPageFlag
        {
            get
            {
                return m_NewPageFlag;
            }
            set
            {
                m_NewPageFlag = value;
            }
        }
        /// <summary>
        /// 文件标记:0-新建、1-科室医生审签、2-科室主任审签、3-医务科审签（模板生效）
        /// </summary>
        public int FileFlag
        {
            get
            {
                return m_FileFlag;
            }
            set
            {
                m_FileFlag = value;
            }
        }
        /// <summary>
        /// 书写次数:0 不限制次数，大于0限制书写次数
        /// </summary>
        public int WriteTimes
        {
            get
            {
                return m_WriteTimes;
            }
            set
            {
                m_WriteTimes = value;
            }
        }
        /// <summary>
        /// 代码
        /// </summary>
        public string Code
        {
            get
            {
                return m_Code;
            }
            set
            {
                m_Code = value;
            }
        }
        /// <summary>
        /// 医院代码
        /// </summary>
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
        /// 模板文件
        /// </summary>
        public string XML_DOC_NEW
        {

            get
            {
                return RecordDal.UnzipEmrXml(m_XML_DOC_NEW);
            }
            set
            {
                m_XML_DOC_NEW = RecordDal.ZipEmrXml(value);
            }
        }

        /// <summary>
        /// 模板文件压缩后(方便数据库操作)
        /// </summary>
        public string ZipXML_DOC_NEW
        {
            get
            {
                return m_XML_DOC_NEW;
            }
            set
            {
                m_XML_DOC_NEW = value;
            }
        }

        public string ZipXML_DOC_NEW_Clear
        {
            get
            {
                //根据实际需求将模板中的savelogs内容清除后保存到数据库中
                RePlaceTempletSaveLog replacesavelog = new RePlaceTempletSaveLog();
                return RecordDal.ZipEmrXml(replacesavelog.ClearTempletSaveLog(RecordDal.UnzipEmrXml(m_XML_DOC_NEW)));
            }
        }

        /// <summary>
        /// 是否是首次病程
        /// </summary>
        public string IsFirstDailyEmr
        {
            get
            {
                return m_IsFirstDailyEmr;
            }
            set
            {
                m_IsFirstDailyEmr = value;
            }
        }

        /// <summary>
        /// 医患沟通
        /// </summary>
        public string IsYiHuanGouTong
        {
            get
            {
                return m_IsYiHuanGouTong;
            }
            set
            {
                m_IsYiHuanGouTong = value;
            }
        }

        /// <summary>
        /// 页面配置 add by  ywk  2012年3月31日9:37:54
        /// </summary>
        public string IsPageConfigSize
        {
            get
            {
                return m_IsConfigPageSize;
            }
            set
            {
                m_IsConfigPageSize = value;
            }
        }

        /// <summary>
        /// 病程名称
        /// </summary>
        public string DailyTitle
        {
            get
            {
                return m_DailyTitle;
            }
            set
            {
                m_DailyTitle = value;
            }
        }

        /// <summary>
        /// 是否显示病程名称
        /// </summary>
        public string IsShowDailyTitle
        {
            get
            {
                return m_IsShowDailyTitle;
            }
            set
            {
                m_IsShowDailyTitle = value;
            }
        }

        /// <summary>
        /// 新页标记
        /// </summary>
        public int NEW_PAGE_END
        {
            get
            {
                return m_NEW_PAGE_END;
            }
            set
            {
                m_NEW_PAGE_END = value;
            }
        }

        /// <summary>
        /// 状态   是否审核，0、保存未提交   1、待审核  2、审核通过  3、审核未通过
        /// </summary>
        public string State
        {
            get { return m_State; }
            set { m_State = value; }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public string Auditor
        {
            get { return m_Auditor; }
            set { m_Auditor = value; }  
        }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string AuditDate
        {
            get { return m_AuditDate; }
            set { m_AuditDate = value; }
        }
        #endregion Properties
    }

}
