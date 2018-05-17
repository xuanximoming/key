using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DrectSoft.Core.EMR_NursingDocument.UserControls
{
    /// <summary>
    /// 线段类
    /// </summary>
    public class VitalSignsLine
    {
        private PointF m_StartPointF = new PointF();//线段的起始点
        private int m_StartPointID;//起始点的ID

        private PointF m_EndPointF = new PointF();//线段的终止点
        private int m_EndPointID;//终止点的ID

        private string m_Type = string.Empty;//线段类型 1：直线 2：虚线
        private string m_ColorName = string.Empty; //线段颜色

        private string m_MainType = string.Empty;//用于区分是体温线，呼吸线和脉搏线

        /// <summary>
        /// 线段的起始点
        /// </summary>
        public PointF StartPointF
        {
            get
            {
                return m_StartPointF;
            }
            set
            {
                m_StartPointF = value;
            }
        }

        /// <summary>
        /// 线段的终止点
        /// </summary>
        public PointF EndPointF
        {
            get
            {
                return m_EndPointF;
            }
            set
            {
                m_EndPointF = value;
            }
        }

        /// <summary>
        /// 线段类型 1：直线 2：虚线
        /// </summary>
        public string Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        /// <summary>
        /// 线段颜色
        /// </summary>
        public string ColorName
        {
            get
            {
                return m_ColorName;
            }
            set
            {
                m_ColorName = value;
            }
        }

        /// <summary>
        /// 起始点的ID
        /// </summary>
        public int StartPointID
        {
            get
            {
                return m_StartPointID;
            }
            set
            {
                m_StartPointID = value;
            }
        }

        /// <summary>
        /// 终止点的ID
        /// </summary>
        public int EndPointID
        {
            get
            {
                return m_EndPointID;
            }
            set
            {
                m_EndPointID = value;
            }
        }

        public string MainType
        {
            get
            {
                return m_MainType;
            }
            set
            {
                m_MainType = value;
            }
        }
    }
}
