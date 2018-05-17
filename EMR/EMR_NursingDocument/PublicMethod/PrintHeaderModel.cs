using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.EMR_NursingDocument.PublicMethod
{
    public class PrintHeaderModel
    {
        string personName;
        string departName;
        string inBedNo;
        string inNo;

        public string InNo
        {
            get { return inNo; }
            set { inNo = value; }
        }

        public string InBedNo
        {
            get { return inBedNo; }
            set { inBedNo = value; }
        }

        public string DepartName
        {
            get { return departName; }
            set { departName = value; }
        }

        public string PersonName
        {
            get { return personName; }
            set { personName = value; }
        }
    }
}
