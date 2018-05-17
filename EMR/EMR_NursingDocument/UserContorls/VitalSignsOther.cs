using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DrectSoft.Core.EMR_NursingDocument.UserControls
{

    /// <summary>
    /// 生命体征类（可能包括：血压，入量，出量，身高，体重等）
    /// </summary>
    public class VitalSignsOther
    {
        private string m_Name = string.Empty;//名称
        private int m_TimePointOfDay = 1;//一天有几个时间点
        private string m_Unit = string.Empty;//单位

        public VitalSignsOther()
        { }

        /// <summary>
        /// 名称
        /// </summary>
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

        /// <summary>
        /// 一天有几个时间点
        /// </summary>
        public int TimePointOfDay
        {
            get
            {
                return m_TimePointOfDay;
            }
            set
            {
                m_TimePointOfDay = value;
            }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get
            {
                return m_Unit;
            }
            set
            {
                m_Unit = value;
            }
        }
    }
}
