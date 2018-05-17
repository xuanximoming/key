
namespace DrectSoft.Core.CommonTableConfig
{
    /// <summary>
    /// 通用单据列对象
    /// </summary>
    public class CommonNote_ItemEntity
    {
        private string _CommonNote_Item_Flow;
        private string _CommonNote_Tab_Flow;
        private string _CommonNoteFlow;
        private string _DataElementFlow;
        private string _DataElementId;
        private string _DataElementName;
        private string _OrderCode;
        private string _IsValidate;
        private string _CreateDoctorID;
        private string _CreateDoctorName;
        private string _CreateDateTime;
        private string _Valide;
        private string _OtherName;

        private DataElementEntity _DataElement;

        /// <summary>
        /// 该项目所对应的数据元
        /// </summary>
        public DataElementEntity DataElement
        {
            get { return _DataElement; }
            set { _DataElement = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示 配置单项目流水号
        /// </summary>
        public virtual string CommonNote_Item_Flow
        {
            get
            {
                return _CommonNote_Item_Flow;
            }
            set
            {
                _CommonNote_Item_Flow = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 配置单标签流水号
        /// </summary>
        public virtual string CommonNote_Tab_Flow
        {
            get
            {
                return _CommonNote_Tab_Flow;
            }
            set
            {
                _CommonNote_Tab_Flow = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 配置单流水号
        /// </summary>
        public virtual string CommonNoteFlow
        {
            get
            {
                return _CommonNoteFlow;
            }
            set
            {
                _CommonNoteFlow = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 数据元流水号
        /// </summary>
        public virtual string DataElementFlow
        {
            get
            {
                return _DataElementFlow;
            }
            set
            {
                _DataElementFlow = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 数据元ID
        /// </summary>
        public virtual string DataElementId
        {
            get
            {
                return _DataElementId;
            }
            set
            {
                _DataElementId = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 数据元名称
        /// </summary>
        public virtual string DataElementName
        {
            get
            {
                return _DataElementName;
            }
            set
            {
                _DataElementName = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 排序码
        /// </summary>
        public virtual string OrderCode
        {
            get
            {
                return _OrderCode;
            }
            set
            {
                _OrderCode = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 是否验证标记
        /// </summary>
        public virtual string IsValidate
        {
            get
            {
                return _IsValidate;
            }
            set
            {
                _IsValidate = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 创建医生ID
        /// </summary>
        public virtual string CreateDoctorID
        {
            get
            {
                return _CreateDoctorID;
            }
            set
            {
                _CreateDoctorID = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 创建医生名称
        /// </summary>
        public virtual string CreateDoctorName
        {
            get
            {
                return _CreateDoctorName;
            }
            set
            {
                _CreateDoctorName = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 创建时间 数据库赋值
        /// </summary>
        public virtual string CreateDateTime
        {
            get
            {
                return _CreateDateTime;
            }
            set
            {
                _CreateDateTime = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 删除标记
        /// </summary>
        public virtual string Valide
        {
            get
            {
                return _Valide;
            }
            set
            {
                _Valide = value;
            }
        }

        /// <summary>
        /// 别名
        /// </summary>
        public string OtherName
        {
            get { return _OtherName; }
            set { _OtherName = value; }
        }

    }
}
