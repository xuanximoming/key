using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DrectSoft.Core.CommonTableConfig
{
    /// <summary>
    /// 通用配置单实体
    /// </summary>
    public class CommonNoteEntity
    {
        private string _CommonNoteFlow;
        private string _CommonNoteName;
        private string _PrinteModelName;
        private string _ShowType;
        private string _CreateDoctorID;
        private string _CreateDoctorName;
        private string _CreateDateTime;
        private string _UsingFlag;
        private string _Valide;
        private string _PYM;
        private string _WBM;
        private string _UsingPicSign;
        private string _UsingCheckDoc;

        /// <summary>
        /// 是否启用审核功能  1   0
        /// </summary>
        public string UsingCheckDoc
        {
            get { return _UsingCheckDoc; }
            set { _UsingCheckDoc = value; }
        }

        /// <summary>
        /// 是否启用图片签名 1 0  如果启用会根据该字段进行删除修改权限控制
        /// </summary>
        public string UsingPicSign
        {
            get { return _UsingPicSign; }
            set { _UsingPicSign = value; }
        }

        /// <summary>
        /// 五笔码
        /// </summary>
        public string WBM
        {
            get { return _WBM; }
            set { _WBM = value; }
        }

        /// <summary>
        /// 拼音码
        /// </summary>
        public string PYM
        {
            get { return _PYM; }
            set { _PYM = value; }
        }

        private List<CommonNote_TabEntity> _CommonNote_TabList;
        private List<BaseDictory> _BaseDepartments;
        private List<BaseDictory> _BaseAreas;
        private string _DepartForShow;
        private string _AreasForShow;

        /// <summary>
        /// 用于展示的病区
        /// </summary>
        public string AreasForShow
        {
            get {
                if (BaseAreas != null)
                    _AreasForShow = "";
                   foreach (var item in BaseAreas)
                   {
                       _AreasForShow += item.Name + "；";
                   }
                return _AreasForShow; 
            }
            //set { _AreasForShow = value; }
        }

        /// <summary>
        /// 用于展示的科室
        /// </summary>
        public string DepartForShow
        {
            get {
                if (BaseDepartments != null)
                {
                    _DepartForShow = "";
                    foreach (var item in BaseDepartments)
                    {
                        _DepartForShow += item.Name + "；";
                    }
                }
                return _DepartForShow; 
            }
            //set { _DepartForShow = value; }
        }

        /// <summary>
        /// 对象化的病区
        /// </summary>
        public List<BaseDictory> BaseAreas
        {
            get { return _BaseAreas; }

            set {
                _BaseAreas = value; 
            }
        }

        /// <summary>
        /// 对象化的科室
        /// </summary>
        public List<BaseDictory> BaseDepartments
        {
            get { return _BaseDepartments; }
            set { _BaseDepartments = value; }
        }

        /// <summary>
        /// 该通用单下的标签（表格）
        /// </summary>
        public List<CommonNote_TabEntity> CommonNote_TabList
        {
            get { return _CommonNote_TabList; }
            set { _CommonNote_TabList = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示 通用单流水号
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
        /// 获取或设置一个值，该值指示 通用单名称
        /// </summary>
        public virtual string CommonNoteName
        {
            get
            {
                return _CommonNoteName;
            }
            set
            {
                _CommonNoteName = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示 打印模板名称
        /// </summary>
        public virtual string PrinteModelName
        {
            get
            {
                return _PrinteModelName;
            }
            set
            {
                _PrinteModelName = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示 显示形式
        /// </summary>
        public virtual string ShowType
        {
            get
            {
                return _ShowType;
            }
            set
            {
                _ShowType = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示 创建医生Id
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
        /// 获取或设置一个值，该值指示 启用标记
        /// </summary>
        public virtual string UsingFlag
        {
            get
            {
                return _UsingFlag;
            }
            set
            {
                _UsingFlag = value;
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
    }
}
