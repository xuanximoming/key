using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ReportManager
{
    public class MethodHelper
    {
        public static int ComputeAge(DateTime dt)
        {
            int age = DateTime.Now.Year - dt.Year;
            if (DateTime.Now.Month - dt.Month < 0)
            {
                age -= 1;
            }
            else if (DateTime.Now.Month - dt.Month == 0)
            {
                if (DateTime.Now.Day - dt.Day < 0)
                {
                    age -= 1;
                }
            }
            if (age < 0)
            {
                age = 0;
            }
            return age;
        }
    }
}
