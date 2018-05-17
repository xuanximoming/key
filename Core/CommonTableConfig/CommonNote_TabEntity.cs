using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.CommonTableConfig
{

    /// <summary>
    /// 通用单据表格对象
    /// </summary>
    public class CommonNote_TabEntity
    {
        private string _CommonNote_Tab_Flow;
        private string _CommonNoteFlow;
        private string _CommonNoteTabName;
        private string _UsingRole;
        private string _ShowType;
        private string _OrderCode;
        private string _CreateDoctorID;
        private string _CreateDoctorName;
        private string _CreateDateTime;
        private string _Valide;
        private string _MaxRows;

        private List<CommonNote_ItemEntity> _CommonNote_ItemList;

        /// <summary>
        /// 该标签下的数据项目
        /// </summary>
        public List<CommonNote_ItemEntity> CommonNote_ItemList
        {
            get { return _CommonNote_ItemList; }
            set { _CommonNote_ItemList = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示 通用单标签流水号
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
        /// 标签名称
        /// </summary>
        public string CommonNoteTabName
        {
            get { return _CommonNoteTabName; }
            set { _CommonNoteTabName = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示 使用角色
        /// </summary>
        public virtual string UsingRole
        {
            get
            {
                return _UsingRole;
            }
            set
            {
                _UsingRole = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示 使用类型
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
        /// 获取或设置一个值，该值指示 创建时间数据库赋值
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
        /// 获取一个值表示能添加的最大行数
        /// xlb 2013-01-12
        /// </summary>
        public virtual string MaxRows
        {
            get
            {
                return _MaxRows;
            }
            set
            {
                _MaxRows = value;
            }
        }
    }
}
