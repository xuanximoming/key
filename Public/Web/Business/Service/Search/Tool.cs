using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrectWeb.Business.Service.Search
{
    public static class Tool
    {
        public static String ConvertNull2OString(this Object o)
        {
            if (o == null)
                return "";
            else
                return o.ToString();
        }
    }
}