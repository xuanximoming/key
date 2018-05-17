using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://www.DrectSoft.com.cn/DrectSoftimage")]
    public sealed class NamedHatchStylesForXmlSerialize
    {

        #region Fields
        private CustomNamedHatchStyle[] _cnhs;
        #endregion

        #region Ctors

        public NamedHatchStylesForXmlSerialize() { }

        #endregion

        #region Members

        [XmlArrayItem("namedHatchStyle", IsNullable = true)]
        public CustomNamedHatchStyle[] NamedHatchStyles
        {
            get { return _cnhs; }
            set { _cnhs = value; }
        }

        #endregion

        #region Explicit Operator

        public static explicit operator NamedHatchStyles(
            NamedHatchStylesForXmlSerialize nhsfxs)
        {
            if (nhsfxs == null ||
                nhsfxs._cnhs == null ||
                nhsfxs._cnhs.Length == 0)
                return DefaultNamedHatchStyles.Instance;
            else
            {
                CustomNamedHatchStyles cnhs = new CustomNamedHatchStyles();
                cnhs.Styles = nhsfxs._cnhs;
                return cnhs;
            }
        }

        public static explicit operator NamedHatchStylesForXmlSerialize(
            NamedHatchStyles nhs)
        {
            if (nhs == null || nhs.IsEmpty)
                return null;
            NamedHatchStylesForXmlSerialize result =
                new NamedHatchStylesForXmlSerialize();
            result.NamedHatchStyles = nhs.Styles;
            return result;
        }

        #endregion

    }
}
