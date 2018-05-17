using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public class InCommonNoteTabEntity
    {

        private string _InCommonNote_Tab_Flow;
        private string _CommonNote_Tab_Flow;
        private string _InCommonNoteFlow;
        private string _CommonNoteTabName;
        private string _UsingRole;
        private string _ShowType;
        private string _OrderCode;
        private string _CreateDoctorID;
        private string _CreateDoctorName;
        private string _CreateDateTime;
        private string _Valide;

        private List<InCommonNoteItemEntity> _InCommonNoteItemList;

        /// <summary>
        /// 该标签下的项目
        /// </summary>
        public List<InCommonNoteItemEntity> InCommonNoteItemList
        {
            get { return _InCommonNoteItemList; }
            set { _InCommonNoteItemList = value; }
        }


        /// <summary>
        /// 病人通用单标签页流水号
        /// </summary>
        public virtual string InCommonNote_Tab_Flow
        {
            get
            {
                return _InCommonNote_Tab_Flow;
            }
            set
            {
                _InCommonNote_Tab_Flow = value;
            }
        }
        /// <summary>
        /// 通用单配置的标签页流水号
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
        /// 病人通用单流水号
        /// </summary>
        public virtual string InCommonNoteFlow
        {
            get
            {
                return _InCommonNoteFlow;
            }
            set
            {
                _InCommonNoteFlow = value;
            }
        }
        /// <summary>
        /// 通用单标签名称
        /// </summary>
        public virtual string CommonNoteTabName
        {
            get
            {
                return _CommonNoteTabName;
            }
            set
            {
                _CommonNoteTabName = value;
            }
        }
        /// <summary>
        /// 使用角色
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
        /// 显示类别
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
        /// 排序码
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
        /// 创建者ID
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
        /// 创建者名称
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
        /// 创建时间
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
        ///删除标记
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
