using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Emr.QcManager
{
    public enum OperType
    {
        QUERY = 0,
        ADD = 1,
        EDIT = 2,
        DEL = 3
    }
    /// <summary>
    ///类名:IemMainpageQC
    ///功能说明:病案首页自动评分标准配置类
    ///创建人:wyt
    ///创建时间:2012-11-28
    ///表类型0病案基本信息1诊断2手术2婴儿3病人
    /// </summary>
    class IemMainpageQC
    {
        IEmrHost m_app;
        string m_id = "";
        string m_tabletype = "0";
        string m_fields = "";
        string m_fieldsvalue = "";
        string m_conditiontabletype = "";
        string m_conditionfields = "";
        string m_conditionvalues = "";
        double m_reductScore = 0;
        string m_reductReason = "";
        int m_valide = 0;
        public IemMainpageQC(IEmrHost app)
        {
            m_app = app;
            DataAccess.App = app;
        }
        public IemMainpageQC()
        {
        }
        /// <summary>
        /// 标准ID
        /// </summary>
        public string ID
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }
        /// <summary>
        /// 校验表类型
        /// </summary>
        public string TableType
        {
            get
            {
                return m_tabletype;
            }
            set
            {
                m_tabletype = value;
            }
        }
        /// <summary>
        /// 校验字段
        /// </summary>
        public string Fields
        {
            get
            {
                return m_fields;
            }
            set
            {
                m_fields = value;
            }
        }
        /// <summary>
        /// 校验值
        /// </summary>
        public string FieldsValue
        {
            get
            {
                return m_fieldsvalue;
            }
            set
            {
                m_fieldsvalue = value;
            }
        }
        /// <summary>
        /// 条件表类型
        /// </summary>
        public string ConditionTableType
        {
            get
            {
                return m_conditiontabletype;
            }
            set
            {
                m_conditiontabletype = value;
            }
        }
        /// <summary>
        /// 条件字段
        /// </summary>
        public string ConditionFields
        {
            get
            {
                return m_conditionfields;
            }
            set
            {
                m_conditionfields = value;
            }
        }
        /// <summary>
        /// 条件字段值
        /// </summary>
        public string ConditionValues
        {
            get
            {
                return m_conditionvalues;
            }
            set
            {
                m_conditionvalues = value;
            }
        }

        /// <summary>
        /// 扣分值
        /// </summary>
        public double ReductScore
        {
            get
            {
                return m_reductScore;
            }
            set
            {
                m_reductScore = value;
            }
        }

        /// <summary>
        /// 扣分原因说明
        /// </summary>
        public string ReductReason
        {
            get
            {
                return m_reductReason;
            }
            set
            {
                m_reductReason = value;
            }
        }

        /// <summary>
        /// 条件字段值
        /// </summary>
        public int Valide
        {
            get
            {
                return m_valide;
            }
            set
            {
                m_valide = value;
            }
        }
    }
}
