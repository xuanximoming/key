using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    /// <summary>
    /// 使用单据实体类
    /// </summary>
    public class InCommonNoteEnmtity
    {
        private string _InCommonNoteFlow;
        private string _CommonNoteFlow;
        private string _InCommonNoteName;
        private string _PrinteModelName;
        private string _NoofInpatient;
        private string _InPatientName;
        private string _CurrDepartID;
        private string _CurrDepartName;
        private string _CurrWardID;
        private string _CurrWardName;
        private string _CreateDoctorID;
        private string _CreateDoctorName;
        private string _CreateDateTime;
        private string _Valide;
        private string _CheckDocId;
        private string _CheckDocName;

        public string CheckDocName
        {
            get { return _CheckDocName; }
            set { _CheckDocName = value; }
        }

        public string CheckDocId
        {
            get { return _CheckDocId; }
            set { _CheckDocId = value; }
        }
       

        private List<InCommonNoteTabEntity> inCommonNoteTabList;

        /// <summary>
        /// 该通用单下面的标签页
        /// </summary>
        public List<InCommonNoteTabEntity> InCommonNoteTabList
        {
            get { return inCommonNoteTabList; }
            set { inCommonNoteTabList = value; }
        }

        /// <summary>
        /// 病人通用单据流水号
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
        /// 通用单据配置流水号
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
        /// 病人通用单据名称
        /// </summary>
        public virtual string InCommonNoteName
        {
            get
            {
                return _InCommonNoteName;
            }
            set
            {
                _InCommonNoteName = value;
            }
        }
        /// <summary>
        ///打印模板名称
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
        /// 病人唯一流水号
        /// </summary>
        public virtual string NoofInpatient
        {
            get
            {
                return _NoofInpatient;
            }
            set
            {
                _NoofInpatient = value;
            }
        }
        /// <summary>
        /// 病人名称
        /// </summary>
        public virtual string InPatientName
        {
            get
            {
                return _InPatientName;
            }
            set
            {
                _InPatientName = value;
            }
        }
        /// <summary>
        /// 当前科室Id
        /// </summary>
        public virtual string CurrDepartID
        {
            get
            {
                return _CurrDepartID;
            }
            set
            {
                _CurrDepartID = value;
            }
        }
        /// <summary>
        /// 当前科室名称
        /// </summary>
        public virtual string CurrDepartName
        {
            get
            {
                return _CurrDepartName;
            }
            set
            {
                _CurrDepartName = value;
            }
        }
        /// <summary>
        /// 当前病区ID
        /// </summary>
        public virtual string CurrWardID
        {
            get
            {
                return _CurrWardID;
            }
            set
            {
                _CurrWardID = value;
            }
        }
        /// <summary>
        /// 当前病区名称
        /// </summary>
        public virtual string CurrWardName
        {
            get
            {
                return _CurrWardName;
            }
            set
            {
                _CurrWardName = value;
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
        ///  删除标记
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
