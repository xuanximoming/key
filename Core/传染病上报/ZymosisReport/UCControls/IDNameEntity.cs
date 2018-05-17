using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YidanSoft.Core.ZymosisReport.UCControls
{
  public  class IDNameEntity
    {
        string _ID;
        string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
    }
}
