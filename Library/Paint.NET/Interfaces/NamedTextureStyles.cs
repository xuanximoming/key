using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public abstract class NamedTextureStyles
    {
        internal NamedTextureStyles() { }

        public abstract bool IsEmpty { get; }
        public abstract string[] GetNames();
        public abstract Picture GetTexture(string name);
        public abstract string GetName(Picture picture);
        internal abstract CustomNamedTextureStyle[] Styles { get; set; }
    }
}
