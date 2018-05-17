using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://www.DrectSoft.com.cn/DrectSoftimage")]
    public sealed class NamedTextureStylesForXmlSerialize
    {
        
        #region Fields
        private CustomNamedTextureStyle[] _cnts;
        #endregion

        #region Ctors

        public NamedTextureStylesForXmlSerialize() { }

        #endregion

        #region Members

        [XmlArrayItem("namedTextureStyle", IsNullable = true)]
        public CustomNamedTextureStyle[] NamedTextureStyles
        {
            get { return _cnts; }
            set { _cnts = value; }
        }

        #endregion

        #region Explicit Operator

        public static explicit operator NamedTextureStyles(
            NamedTextureStylesForXmlSerialize ntsfxs)
        {
            if (ntsfxs == null ||
                ntsfxs._cnts == null ||
                ntsfxs._cnts.Length == 0)
                return DefaultNamedTextureStyles.Instance;
            else
            {
                CustomNamedTextureStyles cnts = new CustomNamedTextureStyles();
                cnts.Styles = ntsfxs._cnts;
                return cnts;
            }
        }

        public static explicit operator NamedTextureStylesForXmlSerialize(
            NamedTextureStyles nts)
        {
            if (nts == null || nts.IsEmpty)
                return null;
            NamedTextureStylesForXmlSerialize result =
                new NamedTextureStylesForXmlSerialize();
            result.NamedTextureStyles = nts.Styles;
            return result;
        }

        #endregion

    }
}
