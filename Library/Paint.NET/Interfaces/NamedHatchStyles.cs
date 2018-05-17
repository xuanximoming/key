using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public abstract class NamedHatchStyles
    {
        internal NamedHatchStyles() { }

        public abstract bool IsEmpty { get; }
        public abstract string[] GetNames();
        public abstract HatchStyle? GetHatchStyle(string name);
        public abstract string GetName(HatchStyle style);
        internal abstract CustomNamedHatchStyle[] Styles { get; set; }
    }
}
