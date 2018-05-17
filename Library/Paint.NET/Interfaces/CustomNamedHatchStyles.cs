using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public sealed class CustomNamedHatchStyles
        : NamedHatchStyles
    {

        #region Subclasses

        [Serializable]
        private sealed class CustomNamedHatchStyleMapping
            : OneOneMapping<HatchStyle, string>
        {

            public CustomNamedHatchStyleMapping()
                : base() { }

            public CustomNamedHatchStyle[] NamedHatchStyles
            {
                get
                {
                    Pair<HatchStyle, string>[] pairs = base.ToPairs();
                    CustomNamedHatchStyle[] result = new CustomNamedHatchStyle[pairs.Length];
                    for (int i = 0; i < result.Length; i++)
                        result[i] = (CustomNamedHatchStyle)pairs[i];
                    return result;
                }
                set
                {
                    Pair<HatchStyle, string>[] pairs = new Pair<HatchStyle, string>[value.Length];
                    for (int i = 0; i < pairs.Length; i++)
                        pairs[i] = (Pair<HatchStyle, string>)value[i];
                    base.FromPairs(pairs);
                }
            }

        }

        #endregion

        #region Fields
        CustomNamedHatchStyleMapping _mapping;
        #endregion

        #region Ctors

        internal CustomNamedHatchStyles()
            : base()
        {
            _mapping = new CustomNamedHatchStyleMapping();
        }

        #endregion

        #region NamedHatchStyle Members

        public override bool IsEmpty
        {
            get { return false; }
        }

        public override string[] GetNames()
        {
            return _mapping.GetBs();
        }

        public override HatchStyle? GetHatchStyle(string name)
        {
            HatchStyle result;
            if (_mapping.TryGetA(name, out result))
                return result;
            else
                return null;
        }

        public override string GetName(HatchStyle style)
        {
            return _mapping[style];
        }

        internal override CustomNamedHatchStyle[] Styles
        {
            get { return _mapping.NamedHatchStyles; }
            set { _mapping.NamedHatchStyles = value; }
        }

        #endregion

        #region Members

        public bool Add(HatchStyle style, string name)
        {
            return _mapping.Add(style, name);
        }

        public bool Remove(HatchStyle style)
        {
            return _mapping.Remove(style);
        }

        public bool Remove(string name)
        {
            return _mapping.Remove(name);
        }

        public void Clear()
        {
            _mapping.Clear();
        }

        #endregion

    }
}
