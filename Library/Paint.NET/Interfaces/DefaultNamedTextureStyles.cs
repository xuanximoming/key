using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://www.DrectSoft.com.cn/DrectSoftimage")]
    public sealed class DefaultNamedTextureStyles
        : NamedTextureStyles
    {

        #region Fields
        public static readonly DefaultNamedTextureStyles Instance =
            new DefaultNamedTextureStyles();
        #endregion

        #region Ctors

        private DefaultNamedTextureStyles()
            : base() { }

        #endregion

        #region Overrides

        [XmlIgnore]
        public override bool IsEmpty
        {
            get { return true; }
        }

        public override string[] GetNames()
        {
            return new string[0];
        }

        public override Picture GetTexture(string name)
        {
            return null;
        }

        public override string GetName(Picture picture)
        {
            return string.Empty;
        }

        internal override CustomNamedTextureStyle[] Styles
        {
            get { return null; }
            set { }
        }

        #endregion

    }
}
