using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    [DebuggerStepThrough]
    public sealed class CustomNamedTextureStyles
        : NamedTextureStyles
    {

        #region Subclasses

        [Serializable]
        private sealed class CustomNamedTextureStyleMapping
            : OneOneMapping<Picture, string>
        {

            public CustomNamedTextureStyleMapping()
                : base() { }

            public CustomNamedTextureStyle[] NamedTextureStyles
            {
                get
                {
                    Pair<Picture, string>[] pairs = base.ToPairs();
                    CustomNamedTextureStyle[] result = new CustomNamedTextureStyle[pairs.Length];
                    for (int i = 0; i < result.Length; i++)
                        result[i] = (CustomNamedTextureStyle)pairs[i];
                    return result;
                }
                set
                {
                    Pair<Picture, string>[] pairs = new Pair<Picture, string>[value.Length];
                    for (int i = 0; i < pairs.Length; i++)
                        pairs[i] = (Pair<Picture, string>)value[i];
                    base.FromPairs(pairs);
                }
            }

        }

        #endregion

        #region Fields
        CustomNamedTextureStyleMapping _mapping;
        #endregion

        #region Ctors

        internal CustomNamedTextureStyles()
            : base()
        {
            _mapping = new CustomNamedTextureStyleMapping();
        }

        #endregion

        #region NamedTextureStyle Members

        public override bool IsEmpty
        {
            get { return false; }
        }

        public override string[] GetNames()
        {
            return _mapping.GetBs();
        }

        public override Picture GetTexture(string name)
        {
            return _mapping[name];
        }

        public override string GetName(Picture picture)
        {
            return _mapping[picture];
        }

        internal override CustomNamedTextureStyle[] Styles
        {
            get { return _mapping.NamedTextureStyles; }
            set { _mapping.NamedTextureStyles = value; }
        }

        #endregion

        #region Members

        public bool Add(Picture picture, string name)
        {
            return _mapping.Add(picture, name);
        }

        public bool Remove(Picture picture)
        {
            return _mapping.Remove(picture);
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
