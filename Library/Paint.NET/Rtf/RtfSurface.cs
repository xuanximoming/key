using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;



namespace DrectSoft.Basic.Paint.NET
{
    public sealed class RtfSurface
        : IShapeReadonlySurface
    {

        #region Fields
        private IShapeSource _source;
        #endregion

        #region Ctors

        public RtfSurface()
        {
            Source = new ShapeSource();
        }

        #endregion

        #region IShapeSurface Members

        public IShapeSource Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public void Clear()
        {
            Source.Clear();
        }

        #endregion

        #region Members



        #endregion

        #region ICustomRtfObject Members

        public void AppendCustomObject(TextWriter tw,
            Control sender, IDictionary dict)
        {
            float zoom;
            if (!dict.Contains("zoom"))
                zoom = 1f;
            else
            {
                object o = dict["zoom"];
                if (!float.TryParse(o.ToString(), out zoom))
                    zoom = 1f;
                else if (zoom <= 0)
                    zoom = 1f;
            }
        }

        public string BuildText()
        {
            return "\n \n";
        }

        #endregion

    }
}
