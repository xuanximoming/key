using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public sealed class DefaultNamedHatchStyles
        : NamedHatchStyles
    {

        #region Fields
        public static readonly DefaultNamedHatchStyles Instance =
            new DefaultNamedHatchStyles();
        #endregion

        #region Ctors

        private DefaultNamedHatchStyles()
            : base() { }

        #endregion

        #region NamedHatchStyle Members

        public override bool IsEmpty
        {
            get { return true; }
        }

        public override string[] GetNames()
        {
            return Enum.GetNames(typeof(HatchStyle));
        }

        public override HatchStyle? GetHatchStyle(string name)
        {
            return (HatchStyle)Enum.Parse(typeof(HatchStyle), name);
        }

        public override string GetName(HatchStyle style)
        {
            return style.ToString();
        }

        internal override CustomNamedHatchStyle[] Styles
        {
            get { return null; }
            set { }
        }

        #endregion

    }
}
