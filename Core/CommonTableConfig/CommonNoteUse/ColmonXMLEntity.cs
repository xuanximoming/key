using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    /// <summary>
    /// <title>对应report.xml中colmon的属性</title>
    /// <auth>xuliangliang</auth>
    /// <date>2012-11-09</date>
    /// </summary>
   public class ColmonXMLEntity
    {
        private string name;
        private string maxpix;
        private string fontfamily;
        private string fontsize;

        public string Fontsize
        {
            get { return fontsize; }
            set { fontsize = value; }
        }

        public string Fontfamily
        {
            get { return fontfamily; }
            set { fontfamily = value; }
        }


        public string Maxpix
        {
            get { return maxpix; }
            set { maxpix = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
