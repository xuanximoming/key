using DrectSoft.Common.Eop;
using System;
using System.ComponentModel;
using System.Data;
using System.Xml;

namespace DrectSoft.Emr.Util
{
    [Serializable()]
    /// <summary>
    /// 病历文件
    /// </summary>
    public class EmrModel
    {
        /// <summary>
        /// 为了在文件夹中能够方便的定位文件，增加了此属性
        /// 
        /// </summary>
        [Browsable(false)]
        public string TempIdentity
        {
            get { return _tempIdentity; }
            set { _tempIdentity = value; }
        }
        private string _tempIdentity;

        /// <summary>
        /// Model的实例ID
        /// </summary>
        public int InstanceId
        {
            get { return _instanceId; }
            set { _instanceId = value; }
        }
        private int _instanceId;

        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName
        {
            get { return _modelName; }
            set { _modelName = value; }
        }
        private string _modelName;

        /// <summary>
        /// 默认的模板名称
        /// </summary>
        public string DefaultModelName
        {
            get { return _defaultModelName; }
            set { _defaultModelName = value; }
        }
        private string _defaultModelName;

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private string _description;


        /// <summary>
        /// 病历分类
        /// </summary>
        public string ModelCatalog
        {
            get { return _modelCatalog; }
            set { _modelCatalog = value; }
        }

        private string _modelCatalog;


        /// <summary>
        /// 创建医生
        /// </summary>        
        public string CreatorXH
        {
            get { return _crXh; }
            set { _crXh = value; }
        }
        private string _crXh;

        /// <summary>
        /// 适用于病程文件，标题的现实世界
        /// </summary>
        public DateTime DisplayTime
        {
            get { return _displayTime; }
            set { _displayTime = value; }
        }
        private DateTime _displayTime;

        /// <summary>
        /// 创建时间
        /// </summary>
        [Browsable(false)]
        public DateTime CreateTime
        {
            get { return _crDate; }
            set { _crDate = value; }
        }
        private DateTime _crDate;

        /// <summary>
        /// 批阅医生
        /// </summary>
        [Browsable(false)]
        public string ExaminerXH
        {
            get { return _emXh; }
            set { _emXh = value; }
        }
        private string _emXh;

        /// <summary>
        /// 批阅时间
        /// </summary>
        [Browsable(false)]
        public DateTime ExamineTime
        {
            get { return _emDate; }
            set { _emDate = value; }
        }
        private DateTime _emDate;

        /// <summary>
        /// 批阅状态
        /// </summary>
        [Browsable(false)]
        public ExamineState State
        {
            get { return _emState; }
            set { _emState = value; }
        }
        private ExamineState _emState = ExamineState.NotSubmit;


        /// <summary>
        /// 病历内容
        /// </summary>
        public XmlDocument ModelContent
        {
            get { return _modelContent; }
            set { _modelContent = value; }
        }


        private XmlDocument _modelContent;


        /// <summary>
        /// 是否已做过电子签名
        /// </summary>
        [Browsable(false)]
        public bool HadSigned
        {
            get { return _hadSigned; }
            set { _hadSigned = value; }
        }
        private bool _hadSigned = false;

        public bool Valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        private bool _valid = true;


        /// <summary>
        /// 是否已打印
        /// </summary>
        [Browsable(false)]
        public bool IsAfterPrint
        {
            get { return _isAfterPrint; }
            set
            {
                //if (value)
                IsModifiedAfterPrint = false;
                _isAfterPrint = value;
            }
        }
        private bool _isAfterPrint = false;

        /// <summary>
        /// 打印后是否做过修改
        /// </summary>
        [Browsable(false)]
        public bool IsModifiedAfterPrint
        {
            get { return _isModifiedAfterPrint; }
            set { _isModifiedAfterPrint = value; }
        }
        private bool _isModifiedAfterPrint = false;

        /// <summary>
        /// 是否做过修改
        /// </summary>
        [Browsable(false)]
        public bool IsModified
        {
            get { return _isModified; }
            set
            {
                if (value && IsAfterPrint)
                    IsModifiedAfterPrint = true;
                _isModified = value;
                if (_isModified)
                    _hadSigned = false;
            }
        }
        private bool _isModified;

        /// <summary>
        /// 是否是日常病程
        /// todo
        /// </summary>
        public bool DailyEmrModel
        {
            get
            {
                if ((ModelCatalog != null) && (ModelCatalog.Equals("AC")))
                    _dailyEmrModel = true;
                return _dailyEmrModel;
            }
        }
        private bool _dailyEmrModel;

        /// <summary>
        /// 是否是科室第一份病程 xll改 20120829
        /// </summary>
        public bool FirstDailyEmrModel
        {
            get
            {
                if (DailyEmrModel)//判断是病程
                {
                    return _FirstDailyEmrModel;
                }
                return false;
            }
            set
            {
                if (DailyEmrModel)//判断是病程
                {
                    _FirstDailyEmrModel = value;
                }
            }
        }
        private bool _FirstDailyEmrModel = false;//默认不是首次病程 是否是科室第一份病程 

        private bool _RealFirstDaily = false;

        /// <summary>
        /// 首次病程
        /// </summary>
        public bool RealFirstDaily
        {
            get { return _RealFirstDaily; }
            set { _RealFirstDaily = value; }
        }

        /// <summary>
        /// 是否为新页
        /// </summary>
        public bool IsNewPage
        {
            get
            {
                return _IsNewPage;
            }
            set
            {
                _IsNewPage = value;
            }
        }
        private bool _IsNewPage = false;

        /// <summary>
        /// 病程名称
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
        private string m_FileName = string.Empty;

        /// <summary>
        /// 是否显示病程名称
        /// </summary>
        public string IsShowFileName
        {
            get
            {
                return m_IsShowFileName;
            }
            set
            {
                m_IsShowFileName = value;
            }
        }
        private string m_IsShowFileName = string.Empty;

        /// <summary>
        /// 是否医患沟通
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
        private string m_IsYiHuanGouTong = string.Empty;

        /// <summary>
        /// 以新页结束 Add By wwj 2011-10-13
        /// </summary>
        public bool NewPageEnd
        {
            get
            {
                return m_NewPageEnd;
            }
            set
            {
                m_NewPageEnd = value;
            }
        }
        private bool m_NewPageEnd = false;

        /// <summary>
        /// 是否读页面大小的通用配置 Add By wwj 2012-03-31
        /// </summary>
        public bool IsReadConfigPageSize { get; set; }

        /// <summary>
        /// 历史病历
        /// </summary>
        public XmlDocument ModelContentHistory { get; set; }

        /// <summary>
        /// 是否是个人模板
        /// </summary>
        public bool PersonalTemplate
        {
            get { return _personalTemplate; }
            set { _personalTemplate = value; }
        }
        private bool _personalTemplate = false;

        /// <summary>
        /// 是否手工设置了以新页开始
        /// </summary>
        public bool IsSetNewPageStart
        {
            get { return m_IsSetNewPageStart; }
            set { m_IsSetNewPageStart = value; }
        }
        private bool m_IsSetNewPageStart = false;

        private string _DepartCode;

        /// <summary>
        /// 科室
        /// </summary>
        public string DepartCode
        {
            get { return _DepartCode; }
            set { _DepartCode = value; }
        }

        private string _WardCode;

        /// <summary>
        /// 病区
        /// </summary>
        public string WardCode
        {
            get { return _WardCode; }
            set { _WardCode = value; }
        }

        /// <summary>
        /// 病历所属科室，对应病人转科表inpatientchangeinfo.ID
        /// Add by wwj 2013-04-03
        /// </summary>
        public string DeptChangeID { get; set; }

        /// <summary>
        /// 是否显示病程预览区，在新增病程的时候使用，只能读取一次，读取后清除
        /// </summary>
        public bool IsShowDailyEmrPreView
        {
            get
            {
                if (m_IsShowDailyEmrPreView)
                {
                    m_IsShowDailyEmrPreView = false;//保证只能读取一次
                    return true;
                }
                return m_IsShowDailyEmrPreView;
            }
            set
            {
                m_IsShowDailyEmrPreView = value;
            }
        }
        private bool m_IsShowDailyEmrPreView = false;

        /// <summary>
        /// .ctor
        /// </summary>
        public EmrModel()
        {
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="dataRow"></param>
        public EmrModel(DataRow dataRow)
        {
            try
            {
                _instanceId = dataRow.IsNull("ID") ? 0 : Convert.ToInt32(dataRow["ID"]);
                _modelCatalog = dataRow.IsNull("SortID") ? string.Empty : dataRow["SortID"].ToString();
                _tempIdentity = dataRow.IsNull("TemplateID") ? string.Empty : dataRow["TemplateID"].ToString();
                _modelName = dataRow.IsNull("Name") ? string.Empty : dataRow["Name"].ToString();
                _hadSigned = dataRow.IsNull("HasSign") ? false : (dataRow["HasSign"].ToString()).Equals("1");
                _emState = dataRow.IsNull("HASSUBMIT") ? ExamineState.NotSubmit : ((ExamineState)Enum.Parse(typeof(ExamineState), dataRow["HASSUBMIT"].ToString()));
                _crXh = dataRow.IsNull("Owner") ? string.Empty : dataRow["Owner"].ToString();
                _crDate = dataRow.IsNull("CreateTime") ? DateTime.Now : DateTime.Parse(dataRow["CreateTime"].ToString());
                _emXh = dataRow.IsNull("Auditor") ? string.Empty : dataRow["Auditor"].ToString();
                _emDate = dataRow.IsNull("AuditTime") ? DateTime.Now : DateTime.Parse(dataRow["AuditTime"].ToString());
                _displayTime = dataRow.IsNull("CaptionDateTime") ? DateTime.Now : DateTime.Parse(dataRow["CaptionDateTime"].ToString());
                _FirstDailyEmrModel = dataRow.IsNull("FIRSTDAILYFLAG") ? false : (dataRow["FIRSTDAILYFLAG"].ToString() == "1" ? true : false);
                m_IsYiHuanGouTong = dataRow.IsNull("IsYiHuanGouTong") ? string.Empty : dataRow["IsYiHuanGouTong"].ToString();
                //是否读页面大小的通用配置 Add By wwj 2012-03-31
                IsReadConfigPageSize = dataRow.IsNull("ISCONFIGPAGESIZE") ? false : (dataRow["FIRSTDAILYFLAG"].ToString() == "1" ? true : false);
                _DepartCode = dataRow.IsNull("DEPARTCODE") ? string.Empty : dataRow["DEPARTCODE"].ToString();
                _WardCode = dataRow.IsNull("WARDCODE") ? string.Empty : dataRow["WARDCODE"].ToString();

                //病人转科ID Add by wwj 2013-04-03
                DeptChangeID = dataRow.IsNull("CHANGEID") ? string.Empty : dataRow["CHANGEID"].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EmrModel Clone()
        {
            EmrModel model = new EmrModel();
            model.InstanceId = this._instanceId;
            model.ModelCatalog = this._modelCatalog;
            model.TempIdentity = this._tempIdentity;
            model.HadSigned = this._hadSigned;
            model.State = this._emState;
            model.CreatorXH = this._crXh;
            model.CreateTime = this._crDate;
            model.ExaminerXH = this._emXh;
            model.ExamineTime = this._emDate;
            model.DisplayTime = this._displayTime;
            model.FirstDailyEmrModel = this._FirstDailyEmrModel;
            model.IsYiHuanGouTong = this.m_IsYiHuanGouTong;
            model.ModelContent = this._modelContent;
            //是否读页面大小的通用配置 Add By wwj 2012-03-31
            model.IsReadConfigPageSize = this.IsReadConfigPageSize;
            model.IsSetNewPageStart = this.IsSetNewPageStart;

            //病人转科ID Add by wwj 2013-04-03
            model.DeptChangeID = this.DeptChangeID;
            model.ModelName = this._modelName;

            //Add by wwj 2013-04-26
            model.IsShowDailyEmrPreView = this.m_IsShowDailyEmrPreView;

            return model;
        }
    }
}
